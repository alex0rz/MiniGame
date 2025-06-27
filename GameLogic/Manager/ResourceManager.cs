using System;
using System.Collections.Generic;
using System.Linq;
using Abstract.Interface;
using GameLogic.BuffApplicator;
using GameLogic.Interface;
using GameModels.Model.Building;

namespace GameLogic.Manager;

public class ResourceManager
{
    private class TrackableCrop : ITrackableCrop, IEquatable<TrackableCrop>
    {
        public TrackableCrop(ITrackableRole farmer, ICrop crop)
        {
            this.UniqueId = new UniqueId();
            Crop = crop;
            Farmer = farmer;
        }

        public UniqueId UniqueId { get; }
        public string Description => Crop.Description;
        public int FullState => Crop.TurnsForGrowingPeriod;
        public int CurrentState => CurrentGrowthTurns;
        public bool IsAvailable => CurrentGrowthTurns >= Crop.TurnsForGrowingPeriod;
        public ICrop Crop { get; }
        public ITrackableRole Farmer { get; set; }
        public int CurrentGrowthTurns { get; set; }

        public bool Equals(TrackableCrop other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            return UniqueId == other.UniqueId;
        }

        public override bool Equals(object obj) => Equals(obj as TrackableCrop);

        public override int GetHashCode() => this?.UniqueId.GetHashCode() ?? 0;

        public static bool operator ==(TrackableCrop x, TrackableCrop y) => Equals(x, y);
        
        public static bool operator !=(TrackableCrop x, TrackableCrop y) => !(x == y);
    }

    private class TrackableBuilding : ITrackableBuilding, IEquatable<TrackableBuilding>
    {
        public TrackableBuilding(ITrackableRole builder, IBuilding building)
        {
            UniqueId = new UniqueId();
            Building = building;
            Builder = builder;
        }

        public UniqueId UniqueId { get; }
        public string Description => Building.Description;
        public int FullState => Building.TurnsForBuildingPeriod;
        public int CurrentState => CurrentBuildTurns;
        public bool IsAvailable => CurrentBuildTurns >= Building.TurnsForBuildingPeriod;
        public IBuilding Building { get; }
        public ITrackableRole Builder { get; set; }
        public int CurrentBuildTurns { get; set; }

        public bool Equals(TrackableBuilding other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return UniqueId == other.UniqueId;
        }

        public override bool Equals(object obj) => Equals(obj as TrackableBuilding);
        
        public override int GetHashCode() => UniqueId?.GetHashCode() ?? 0;

        public static bool operator ==(TrackableBuilding x, TrackableBuilding y) => Equals(x, y);

        public static bool operator !=(TrackableBuilding x, TrackableBuilding y) => !(x == y);
    }

    public ResourceManager(IEnumerable<IBuilding> buildings, IEnumerable<ICrop> crops, decimal food)
    {
        if (buildings == null)
            _trackedBuildings = [];
        else
            _trackedBuildings = [..buildings.Select(b => new TrackableBuilding(null, b))];

        if (crops == null)
            _trackedCrops = [];
        else
            _trackedCrops = [..crops.Select(c => new TrackableCrop(null, c))];

        Food = (food + Math.Abs(food)) / 2m;
    }

    public ResourceManager(int buildings, int food) : this(
        Enumerable.Repeat(1, buildings).Select(e => new Building()), null, food)
    {
    }

    public ResourceManager() : this(null, null, 0)
    {
    }

    private readonly HashSet<TrackableBuilding> _trackedBuildings;
    private readonly HashSet<TrackableCrop> _trackedCrops;
    public decimal Food { get; private set; }
    public IReadOnlyCollection<ITrackableBuilding> Buildings => _trackedBuildings.ToArray();
    public IReadOnlyCollection<ITrackableCrop> Crops => _trackedCrops.ToArray();
    public int Beds => _trackedBuildings.Where(tb => tb.IsAvailable).Sum(tb => tb.Building.BedProvided);

    internal void ConsumeFood(decimal foodAte)
    {
        Food -= foodAte;
    }

    internal void Plant(PlantResult plantResult, IEnumerable<IBuff> buffs)
    {
        foreach (var newCrop in plantResult.NewCrops)
        {
            var newTrackableCrop = new TrackableCrop(newCrop.Farmer, newCrop.Crop);
            if(newCrop.Growth != 0)
                newTrackableCrop.CurrentGrowthTurns = newCrop.Growth;
            _trackedCrops.Add(newTrackableCrop);
        }


        foreach (var oldCrop in plantResult.GrowingCrops)
        {
            if (!_trackedCrops.TryGetValue(oldCrop.Crop as TrackableCrop, out var trackedCrop))
                continue;
            trackedCrop.CurrentGrowthTurns += oldCrop.Growth;
            trackedCrop.Farmer ??= oldCrop.Farmer;
        }
        Mature(_trackedCrops, buffs);
    }

    internal void Build(BuildResult result)
    {
        foreach (var build in result.NewConstructions)
        {   
                var newTrackedBuilding = new TrackableBuilding(build.Builder, build.Building);
                if (build.Built != 0)
                    newTrackedBuilding.CurrentBuildTurns = build.Built;
                _trackedBuildings.Add(newTrackedBuilding);
        }

        foreach (var build in result.UnderConstructions)
        {
            var trackedBuilding = _trackedBuildings.FirstOrDefault(tb => tb.Building == build.Building.Building);
            if (trackedBuilding != null)
            {
                trackedBuilding.CurrentBuildTurns += build.Built;
                trackedBuilding.Builder ??= build.Builder;
            }
        }
    }


    private void Mature(HashSet<TrackableCrop> trackedCrops, IEnumerable<IBuff> buffs)
    {
        var foodProduction = new ManagerBuffApplicator.CutCropProduction().CalculateFoodProduction(trackedCrops, buffs);
        trackedCrops.RemoveWhere(tc => tc.IsAvailable);
        Food += foodProduction;
    }

    internal void CleanTrackableCrop(IEnumerable<ITrackableRole> aliveRoles)
    {
        var roles = aliveRoles.ToHashSet();
        _trackedCrops.RemoveWhere(tb => tb.Farmer != null && !roles.Contains(tb.Farmer));
    }

    internal void CleanTrackableBuildings(IEnumerable<ITrackableRole> aliveRoles)
    {
        var roles = aliveRoles.ToHashSet();
        _trackedBuildings.RemoveWhere(tb => tb.Builder != null && !roles.Contains(tb.Builder));
    }

    internal void DestroyBuilding(IEnumerable<ITrackableBuilding> destroyedBuildings)
    {
        foreach (var destroyed in destroyedBuildings)
        {
            _trackedBuildings.RemoveWhere(tb => tb.UniqueId == destroyed.UniqueId);
        }
    }
}

public readonly struct BuildResult(
    IEnumerable<(IBuilding Building, ITrackableRole Builder, int Built)> newConstructions,
    IEnumerable<(ITrackableBuilding Building, ITrackableRole Builder, int Built)> underConstructions)
{
    public readonly IEnumerable<(IBuilding Building, ITrackableRole Builder, int Built)> NewConstructions { get; } =
        newConstructions;

    public readonly IEnumerable<(ITrackableBuilding Building, ITrackableRole Builder, int Built)> UnderConstructions { get; } =
        underConstructions;
}

public readonly struct PlantResult(
    IEnumerable<(ICrop Crop, ITrackableRole Farmer, int Growth)> newCrops,
    IEnumerable<(ITrackableCrop Crop, ITrackableRole Farmer, int Growth)> growingCrops)
{
    public readonly IEnumerable<(ICrop Crop, ITrackableRole Farmer, int Growth)> NewCrops { get; } = newCrops;

    public readonly IEnumerable<(ITrackableCrop Crop, ITrackableRole Farmer, int Growth)> GrowingCrops { get; } = growingCrops;
}

public class ResourceManagerProxy
{
    public ResourceManagerProxy(ResourceManager resourceManager, MessageManager messageManager)
    {
        _resourceManager = resourceManager;
        _messageManager = messageManager;
    }

    public ResourceManagerProxy(int buildings, int food, MessageManager messageManager) : this(
        new ResourceManager(buildings, food), messageManager)
    {
    }

    private readonly MessageManager _messageManager;
    private readonly ResourceManager _resourceManager;
    public decimal Food => _resourceManager.Food;
    public int Beds => _resourceManager.Beds;
    public IReadOnlyCollection<ITrackableBuilding> Buildings => _resourceManager.Buildings;
    public IReadOnlyCollection<ITrackableCrop> Crops => _resourceManager.Crops;

    public void ConsumeFood(decimal foodAte)
    {
        _resourceManager.ConsumeFood(foodAte);
        _messageManager?.LogMessage($"Food consumed: {foodAte}");
    }

    public void Build(BuildResult buildResult)
    {
        var countBefore = _resourceManager.Buildings.Count();
        _resourceManager.Build(buildResult);
        var countAfter = _resourceManager.Buildings.Count();
        _messageManager?.LogMessage($"Buildings completed: {countAfter - countBefore}");
    }

    public void Plant(PlantResult plantResult, IEnumerable<IBuff> buffs)
    {
        var countBefore = _resourceManager.Food;
        _resourceManager.Plant(plantResult, buffs);
        var countAfter = _resourceManager.Food;
        _messageManager?.LogMessage($"Food planted: {countAfter - countBefore}");
    }

    internal void CleanTrackableCorps(IEnumerable<ITrackableRole> aliveRoles)
    {
        var countBefore = _resourceManager.Crops.Count;
        _resourceManager.CleanTrackableCrop(aliveRoles);
        var countAfter = _resourceManager.Crops.Count;
        _messageManager?.LogMessage($"Crop Destroyed: {countAfter - countBefore}");
    }

    internal void CleanTrackableBuildings(IEnumerable<ITrackableRole> aliveRoles)
    {
        var countBefore = _resourceManager.Buildings.Count;
        _resourceManager.CleanTrackableBuildings(aliveRoles);
        var countAfter = _resourceManager.Buildings.Count;
        _messageManager?.LogMessage($"Buildings Destroyed: {countAfter - countBefore}");
    }

    internal void DestroyBuildings(IEnumerable<ITrackableBuilding> destroyedBuildings)
    {
        var countBefore = _resourceManager.Buildings.Count;
        _resourceManager.DestroyBuilding(destroyedBuildings);
        var countAfter = _resourceManager.Buildings.Count;
        _messageManager?.LogMessage($"Buildings Destroyed: {countAfter - countBefore}");
    }
}