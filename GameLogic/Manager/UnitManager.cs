using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Abstract.Interface;
using GameLogic.BuffApplicator;
using GameLogic.Interface;
using GameModels.Model.Role;

[assembly: InternalsVisibleTo("GameLogic.Test")]

namespace GameLogic.Manager;

public class UnitManager
{
    private class TrackableFoe(IFoe foe) : ITrackableFoe, IEquatable<TrackableFoe>
    {
        public UniqueId UniqueId { get; } = new();
        public string Description => Foe.Description;
        public int FullState => Foe.MaxHitPoint;
        public int CurrentState => DamageTaken;
        public bool IsAvailable => Foe.MaxHitPoint > DamageTaken;
        public int DamageTaken { get; set; }
        public IFoe Foe { get; } = foe;

        public bool Equals(TrackableFoe other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return UniqueId == other.UniqueId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TrackableFoe);
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        public static bool operator ==(TrackableFoe x, TrackableFoe y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(TrackableFoe x, TrackableFoe y)
        {
            return !(x == y);
        }
    }

    private class TrackableRole(IRole role) : ITrackableRole, IEquatable<TrackableRole>
    {
        public UniqueId UniqueId { get; } = new();
        public string Description => Role.Description;
        public int FullState => Role.MaxHitPoint;
        public int CurrentState => DamageTaken;
        public bool IsAvailable => Role.MaxHitPoint > DamageTaken;
        public int DamageTaken { get; set; }
        public IRole Role { get; } = role;

        public bool Equals(TrackableRole other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return UniqueId == other.UniqueId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TrackableRole);
        }

        public override int GetHashCode()
        {
            return this?.UniqueId.GetHashCode() ?? 0;
        }

        public static bool operator ==(TrackableRole x, TrackableRole y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(TrackableRole x, TrackableRole y)
        {
            return !(x == y);
        }
    }

    public UnitManager(IEnumerable<IRole> roles, IEnumerable<IFoe> foes)
    {
        if (roles != null)
            _trackedRoles = [.. roles.Select(r => new TrackableRole(r))];
        else
            _trackedRoles = [];

        if (foes != null)
            _trackedFoes = [.. foes.Select(f => new TrackableFoe(f))];
        else
            _trackedFoes = [];

        _recruitHandlers = new Dictionary<Type, Func<int, IRole[]>>
        {
            [typeof(IFarmer)] = RecruitFarmer,
            [typeof(IWheatFarmer)] = RecruitWheatFarmer,
            [typeof(IRiceFarmer)] = RecruitRiceFarmer,
            [typeof(ISoldier)] = RecruitSoldier,
            [typeof(IBuilder)] = RecruitBuilder
        };
    }

    public UnitManager(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders, int foes,
        int foodThieves, int buildingDestroyers, int relicBearers) : this(
        Enumerable.Repeat(1, farmers).Select(e => new Farmer()).Cast<IRole>()
            .Union(Enumerable.Repeat(1, wheatFarmers).Select(e => new WheatFarmer()))
            .Union(Enumerable.Repeat(1, riceFarmers).Select(e => new RiceFarmer()))
            .Union(Enumerable.Repeat(1, soldiers).Select(e => new Soldier()))
            .Union(Enumerable.Repeat(1, builders).Select(e => new Builder())),
        Enumerable.Repeat(1, foes).Select(e => new Foe()).Cast<IFoe>()
            .Union(Enumerable.Repeat(1, foodThieves).Select(e => new FoodThief()))
            .Union(Enumerable.Repeat(1, buildingDestroyers).Select(e => new BuildingDestroyer()))
            .Union(Enumerable.Repeat(1, relicBearers).Select(e => new RelicBearer(null))))
    {
    }

    public decimal CalculateRecruitCost(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders,
        IEnumerable<ITrackableBuff> buffs)
    {
        return new ManagerBuffApplicator.RecruitCost().CalculateRecruitCost(farmers, wheatFarmers, riceFarmers,
            soldiers, builders, buffs.Select(b => b.Buff).ToArray());
    }

    public UnitManager() : this(null, null)
    {
    }

    private readonly HashSet<TrackableRole> _trackedRoles;
    private readonly HashSet<TrackableFoe> _trackedFoes;

    public IReadOnlyCollection<ITrackableRole> Roles =>
        _trackedRoles.Where(tr => tr.IsAvailable).OfType<ITrackableRole>().ToArray();

    public IReadOnlyCollection<ITrackableRole> Farmers => Roles.Where(r => r.Role is IFarmer).ToArray();

    public IReadOnlyCollection<ITrackableRole> Builders => Roles.Where(r => r.Role is IBuilder).ToArray();

    public IReadOnlyCollection<ITrackableRole> Soldiers => Roles.Where(r => r.Role is ISoldier).ToArray();

    public IReadOnlyCollection<ITrackableRole> WheatFarmers => Roles.Where(r => r.Role is IWheatFarmer).ToArray();

    public IReadOnlyCollection<ITrackableRole> RiceFarmers => Roles.Where(r => r.Role is IRiceFarmer).ToArray();

    public IReadOnlyCollection<ITrackableFoe> Foes =>
        _trackedFoes.Where(tf => tf.IsAvailable).OfType<ITrackableFoe>().ToArray();

    public IReadOnlyCollection<ITrackableFoe> FoodThieves => Foes.Where(f => f.Foe is IFoodThief).ToArray();

    public IReadOnlyCollection<ITrackableFoe> BuildingDestroyers =>
        Foes.Where(f => f.Foe is IBuildingDestroyer).ToArray();

    public IReadOnlyCollection<ITrackableFoe> RelicBearers => Foes.Where(f => f.Foe is IRelicBearer).ToArray();

    internal void Recruit(int farmers, int wheatFramers, int riceFramers, int soldiers, int builders)
    {
        var recruited =
            Enumerable.Repeat(1, farmers).Select(e => new TrackableRole(new Farmer()))
                .Union(Enumerable.Repeat(1, wheatFramers).Select(e => new TrackableRole(new WheatFarmer())))
                .Union(Enumerable.Repeat(1, riceFramers).Select(e => new TrackableRole(new RiceFarmer())))
                .Union(Enumerable.Repeat(1, soldiers).Select(e => new TrackableRole(new Soldier())))
                .Union(Enumerable.Repeat(1, builders).Select(e => new TrackableRole(new Builder())));
        foreach (var role in recruited) _trackedRoles.Add(role);
    }

    internal IEnumerable<ITrackableFoe> EliminateFoes()
    {
        var killedFoes = _trackedFoes.Where(tf => !tf.IsAvailable).ToArray();
        _trackedFoes.RemoveWhere(tf => !tf.IsAvailable);
        return killedFoes;
    }

    internal void EliminateRoles()
    {
        _trackedRoles.RemoveWhere(tr => !tr.IsAvailable);
    }

    internal void Adjust(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders)
    {
        if (Farmers.Count - farmers > 0) Remove(Farmers.Count - farmers, typeof(IFarmer));
        if (WheatFarmers.Count - wheatFarmers > 0) Remove(WheatFarmers.Count - wheatFarmers, typeof(IWheatFarmer));
        if (RiceFarmers.Count - riceFarmers > 0) Remove(RiceFarmers.Count - riceFarmers, typeof(IRiceFarmer));
        if (Soldiers.Count - soldiers > 0) Remove(Soldiers.Count - soldiers, typeof(ISoldier));
        if (Builders.Count - builders > 0) Remove(Builders.Count - builders, typeof(IBuilder));

        if (farmers - Farmers.Count > 0) Recruit(farmers - Farmers.Count, typeof(IFarmer));
        if (wheatFarmers - WheatFarmers.Count > 0) Recruit(wheatFarmers - WheatFarmers.Count, typeof(IWheatFarmer));
        if (riceFarmers - RiceFarmers.Count > 0) Recruit(riceFarmers - RiceFarmers.Count, typeof(IRiceFarmer));
        if (soldiers - Soldiers.Count > 0) Recruit(soldiers - Soldiers.Count, typeof(ISoldier));
        if (builders - Builders.Count > 0) Recruit(builders - Builders.Count, typeof(IBuilder));
    }

    private void Recruit(int numbers, Type type)
    {
        if (_recruitHandlers.TryGetValue(type, out var handlers))
            foreach (var role in handlers(numbers))
                _trackedRoles.Add(new TrackableRole(role));
    }

    private readonly Dictionary<Type, Func<int, IRole[]>> _recruitHandlers;

    private IRole[] RecruitFarmer(int numbers)
    {
        return Enumerable.Repeat(1, numbers).Select(e => new Farmer()).ToArray();
    }

    private IRole[] RecruitWheatFarmer(int numbers)
    {
        return Enumerable.Repeat(1, numbers).Select(e => new WheatFarmer()).ToArray();
    }

    private IRole[] RecruitRiceFarmer(int numbers)
    {
        return Enumerable.Repeat(1, numbers).Select(e => new RiceFarmer()).ToArray();
    }

    private IRole[] RecruitSoldier(int numbers)
    {
        return Enumerable.Repeat(1, numbers).Select(e => new Soldier()).ToArray();
    }

    private IRole[] RecruitBuilder(int numbers)
    {
        return Enumerable.Repeat(1, numbers).Select(e => new Builder()).ToArray();
    }


    private void Remove(int numbers, Type type)
    {
        var rolesToRemove = _trackedRoles.Where(r => type.IsAssignableFrom(r.Role.GetType())).Take(numbers).ToArray();
        foreach (var role in rolesToRemove) _trackedRoles.Remove(role);
    }

    internal void GenerateFoes(IEnumerable<IFoe> newFoes)
    {
        foreach (var foe in newFoes) _trackedFoes.Add(new TrackableFoe(foe));
    }

    internal IEnumerable<ITrackableFoe> AttackEnemies(
        IEnumerable<(ITrackableFoe TrackableFoe, int Damage)> attackResults)
    {
        foreach (var attackResult in attackResults)
        {
            var foe = _trackedFoes.FirstOrDefault(tf => tf.Foe == attackResult.TrackableFoe?.Foe);
            if (foe != null)
                foe.DamageTaken += attackResult.Damage;
        }

        return EliminateFoes();
    }

    internal void TakeCasualties(IEnumerable<(ITrackableRole TrackableRole, int Damage)> defendResults)
    {
        foreach (var defendResult in defendResults)
        {
            var role = _trackedRoles.FirstOrDefault(tf => tf.Role == defendResult.TrackableRole?.Role);
            if (role != null)
                role.DamageTaken += defendResult.Damage;
        }

        EliminateRoles();
    }

    internal void RemoveUnDeadRelicBearer()
    {
        _trackedFoes.RemoveWhere(tf => tf.IsAvailable && tf.Foe is IRelicBearer);
    }
}

public class UnitManagerProxy(UnitManager unitManager, MessageManager messageManager)
{
    public UnitManagerProxy(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders, int foes,
        int foodThieves, int buildingDestroyer, int relicBearers, MessageManager messageManager) :
        this(
            new UnitManager(farmers, wheatFarmers, riceFarmers, soldiers, builders, foes, foodThieves,
                buildingDestroyer, relicBearers), messageManager)
    {
    }

    public IReadOnlyCollection<ITrackableRole> Roles => unitManager.Roles;
    public IReadOnlyCollection<ITrackableRole> Soldiers => unitManager.Soldiers;
    public IReadOnlyCollection<ITrackableRole> Builders => unitManager.Builders;
    public IReadOnlyCollection<ITrackableRole> Farmers => unitManager.Farmers;
    public IReadOnlyCollection<ITrackableRole> WheatFarmers => unitManager.WheatFarmers;
    public IReadOnlyCollection<ITrackableRole> RiceFarmers => unitManager.RiceFarmers;
    public IReadOnlyCollection<ITrackableFoe> Foes => unitManager.Foes;


    internal void Recruit(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders)
    {
        var countBefore = unitManager.Roles.Count();
        unitManager.Recruit(farmers, wheatFarmers, riceFarmers, soldiers, builders);
        var countAfter = unitManager.Roles.Count();
        messageManager?.LogMessage($"Roles recruited: {countAfter - countBefore}");
    }

    internal IEnumerable<ITrackableFoe> AttackEnemies(
        IEnumerable<(ITrackableFoe TrackableFoe, int Damage)> combatResults)
    {
        var countBefore = unitManager.Foes.Count();
        var killedFoes = unitManager.AttackEnemies(combatResults);
        var countAfter = unitManager.Foes.Count();
        messageManager?.LogMessage($"Foes eliminated: {countBefore - countAfter}");
        return killedFoes;
    }

    internal void TakeCasualties(IEnumerable<(ITrackableRole trackableRole, int Damage)> defendResults)
    {
        var countBefore = unitManager.Roles.Count();
        unitManager.TakeCasualties(defendResults);
        var countAfter = unitManager.Roles.Count();
        messageManager?.LogMessage($"Role Casualties: {countBefore - countAfter}");
    }

    internal void Adjust(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders)
    {
        var before = unitManager.Roles.GroupBy(r => r.GetType())
            .ToDictionary(ig => ig.Key, ig => ig.Count());
        unitManager.Adjust(farmers, wheatFarmers, riceFarmers, soldiers, builders);
        var after = unitManager.Roles.GroupBy(r => r.GetType())
            .ToDictionary(ig => ig.Key, ig => ig.Count());
        foreach (var t in after.Keys.Union(before.Keys))
        {
            before.TryGetValue(t, out var b);
            after.TryGetValue(t, out var a);
            var diff = a - b;
            if (diff > 0)
                messageManager?.LogMessage($"{t.Name} Add: {diff}");
            else if (diff < 0)
                messageManager?.LogMessage($"{t.Name} Remove: {Math.Abs(diff)}");
        }
    }

    internal void GenerateFoes(IEnumerable<IFoe> newFoes)
    {
        var countBefore = unitManager.Foes.Count();
        unitManager.GenerateFoes(newFoes);
        var countAfter = unitManager.Foes.Count();
        messageManager?.LogMessage($"Foes generated: {countAfter - countBefore}");
    }

    public decimal CalculateRecruitCost(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders,
        IEnumerable<ITrackableBuff> trackedBuffs)
    {
        return unitManager.CalculateRecruitCost(farmers, wheatFarmers, riceFarmers, soldiers, builders, trackedBuffs);
    }

    public void RemoveRelicBearer()
    {
        var countBefore = unitManager.Foes.Count();
        unitManager.RemoveUnDeadRelicBearer();
        var countAfter = unitManager.Foes.Count();
        messageManager?.LogMessage($"RelicBearer Removed: {countAfter - countBefore}");
    }
}