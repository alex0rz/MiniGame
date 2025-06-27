using System;
using System.Collections.Generic;
using System.Linq;
using Abstract.Interface;
using GameLogic.BuffApplicator;
using GameLogic.Interface;
using GameLogic.Manager;
using GameModels.Model.Buff;
using GameModels.Model.Building;
using GameModels.Model.Crop;

namespace GameLogic.Phase;

public class YieldPhase : IPhase
{
    private readonly Dictionary<Type, Func<ICrop>> _newPlantHandlers;

    public YieldPhase()
    {
        _newPlantHandlers = new Dictionary<Type, Func<ICrop>>()
        {
            [typeof(IFarmer)] = PlantCommonCrop,
            [typeof(IWheatFarmer)] = PlantWheat,
            [typeof(IRiceFarmer)] = PlantRice
        };
    }

    internal void RegisterPlantHandlers(IRole role, Func<ICrop> plantMethod)
    {
        if (!_newPlantHandlers.ContainsKey(role.GetType()))
            _newPlantHandlers.Add(role.GetType(), plantMethod);
    }

    private ICrop PlantCommonCrop() => new CommonCrop();

    private ICrop PlantWheat() => new Wheat();

    private ICrop PlantRice() => new Rice();

    public string Description { get; } = "Yield Phase";

    public void ExecutePhase(GameContext gameContext)
    {
        var buffs = gameContext.Buffs.Select(tb => tb.Buff).ToArray();
        var plantResult = new PhaseBuffApplicator.CutCropGrowth().Plant(Plant(gameContext.Roles, gameContext.Crops), buffs);
        gameContext.Plant(plantResult, buffs);
        var buildResult = Build(gameContext.Roles, gameContext.Buildings);
        gameContext.Build(buildResult);
    }

    private PlantResult Plant(IEnumerable<ITrackableRole> trackableRoles, IEnumerable<ITrackableCrop> trackableCrops)
    {
        ITrackableRole[] farmers = null;
        ITrackableCrop[] crops = trackableCrops?.ToArray();
        HashSet<(ICrop Crop, ITrackableRole Farmer, int Growth)> newPlants = new();
        HashSet<(ITrackableCrop Crop, ITrackableRole Farmer, int Growth)> growthPlants = new ();
        try
        {
            farmers = trackableRoles
                .Where(tr => tr.Role is IFarmer || tr.Role is IWheatFarmer || tr.Role is IRiceFarmer).ToArray();
            
            foreach (var farmer in farmers)
            {
                var newCrop = _newPlantHandlers[farmer.GetType()]();
                var crop = crops?.FirstOrDefault(c =>
                    c.Farmer == farmer.Role || (c.Farmer == null && c.Crop.GetType() == newCrop.GetType()));
                if (crop != null)
                    growthPlants.Add(new ValueTuple<ITrackableCrop, ITrackableRole, int>(crop, farmer, 1));
                else
                    newPlants.Add(new ValueTuple<ICrop, ITrackableRole, int>(newCrop, farmer, 1));
            }

            return new PlantResult(newPlants.ToArray(), growthPlants.ToArray());
        }
        finally
        {
            if(farmers != null)
                Array.Clear(farmers, 0, farmers.Length);
            if(crops != null)
                Array.Clear(crops, 0, crops.Length);
            newPlants.Clear();
            growthPlants.Clear();
        }
    }

    private BuildResult Build(IEnumerable<ITrackableRole> trackableRoles, IEnumerable<ITrackableBuilding> trackableBuildings)
    {
        var builders = trackableRoles?.Where(tr => tr.Role is IBuilder).ToArray();
        var buildings = trackableBuildings?.ToArray();
        HashSet<(IBuilding Building, ITrackableRole Builder, int Built)> newBuildings =new();
        HashSet<(ITrackableBuilding Building, ITrackableRole Builder, int Built)> underConstructionBuildings = new();
        try
        {   
            foreach (var builder in builders)
            {
                var building = buildings.FirstOrDefault(c => c.Builder?.Equals(builder) ?? false);
                if (building != null)
                    underConstructionBuildings.Add(new ValueTuple<ITrackableBuilding, ITrackableRole, int>(building, builder, 1));
                else
                    newBuildings.Add(new (new Building(), builder, 1));
            }

            return new BuildResult(newBuildings.ToArray(), underConstructionBuildings.ToArray());
        }
        finally
        {
            if(builders != null)
                Array.Clear(builders, 0, builders.Length);
            if(buildings != null)
                Array.Clear(buildings, 0, buildings.Length);
            newBuildings.Clear();
            underConstructionBuildings.Clear();
        }
    }
}

