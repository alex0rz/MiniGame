using System;
using System.Collections.Generic;
using System.Linq;
using Abstract.Interface;
using GameLogic.Interface;

namespace GameLogic.Phase;

internal class CombatPhase : IPhase
{
    public string Description { get; } = "Combat Phase";

    public void ExecutePhase(GameContext gameContext)
    {
        var attackResult = CalculateCombat(gameContext.Roles, gameContext.Foes);
        gameContext.AttackEnemies(attackResult);
        var defendResult = CalculateCasualtiesAndLoss(gameContext.Roles, gameContext.Foes, gameContext.Food,
            gameContext.BuildersCount);
        gameContext.ConsumeFood(defendResult.FoodStolen);
        gameContext.DestroyBuilding(gameContext.Buildings.Take(defendResult.BuildingDestroyed));
        gameContext.TakeCasualties(defendResult.Casuaties);
    }

    private List<(ITrackableFoe trackableFoe, int damage)> CalculateCombat(IEnumerable<ITrackableRole> trackableRoles,
        IEnumerable<ITrackableFoe> trackableFoes)
    {
        var total = trackableRoles?.Where(tr => tr.Role is ISoldier).Sum(tr => (tr.Role as ISoldier).Power) ?? 0;
        using var foeEnumerator = trackableFoes.GetEnumerator();

        var result = new List<(ITrackableFoe trackableFoe, int damage)>();
        while (total > 0 && foeEnumerator.MoveNext())
        {
            var currentFoe = foeEnumerator.Current;
            if (currentFoe == null) break;
            if (currentFoe.CurrentState <= total)
            {
                result.Add(new ValueTuple<ITrackableFoe, int>(currentFoe, currentFoe.CurrentState));
                total -= currentFoe.CurrentState;
            }
            else
            {
                result.Add(new ValueTuple<ITrackableFoe, int>(currentFoe, currentFoe.CurrentState - total));
                total = 0;
            }
        }
        return result;
    }

    private IEnumerable<ITrackableRole> ReOrderCuttingSequenceDueToInsufficientSoldiers(
        IEnumerable<ITrackableRole> trackableRoles)
    {
        var roles = trackableRoles?.ToArray();
        try
        {
            using var erSoldier = roles?.Where(tr => tr.Role is ISoldier).GetEnumerator();
            using var erBuilder = roles?.Where(tr => tr.Role is IBuilder).GetEnumerator();
            using var erFarmer = roles?.Where(tr => tr.Role is IFarmer).GetEnumerator();

            while (erSoldier?.MoveNext() ?? false)
                yield return erSoldier.Current;
            while (erBuilder?.MoveNext() ?? false)
                yield return erBuilder.Current;
            while (erFarmer?.MoveNext() ?? false)
                yield return erFarmer.Current;
        }
        finally
        {
            if(roles != null)
                Array.Clear(roles, 0, roles.Length);
        }
    }

    private (int Damage, decimal FoodStealed, int BuildingDestroied) CalculateImpact(
        IEnumerable<ITrackableRole> trackableRoles, IEnumerable<ITrackableFoe> trackableFoes, decimal foods, int buildings)
    {
        var damage = 0;
        var foodStolen = 0m;
        var buildingDestroyed = 0;
        foreach (var trackableFoe in trackableFoes)
        {
            switch (trackableFoe.Foe)
            {
                    case IFoodThief it:
                        if (foods <= 0 || foodStolen >= foods) break;

                        if (foodStolen + it.FoodSteal < foods)
                            foodStolen += it.FoodSteal;
                        else if (foodStolen < foods)
                            foodStolen = foods;
                        else
                            break;
                        continue;

                    case IBuildingDestroyer bd:
                        if (buildings <= 0 || buildingDestroyed >= buildings) break;

                        if (buildingDestroyed + bd.BuildingDestroy < buildings)
                            buildingDestroyed += bd.BuildingDestroy;
                        else if (buildingDestroyed < buildings)
                            buildingDestroyed = buildings;
                        else
                            break;
                        continue;
            }
            damage += trackableFoe.Foe.AttackPower;
        }
        return (damage, foodStolen, buildingDestroyed);
    }

    private (IEnumerable<(ITrackableRole Role, int Damage)> Casuaties, decimal FoodStolen, int BuildingDestroyed)
        CalculateCasualtiesAndLoss(IEnumerable<ITrackableRole> trackableRoles, IEnumerable<ITrackableFoe> trackableFoes,
            decimal food, int buildings)
    {
        var roles = trackableRoles?.ToArray();
        try
        {
            var impact = CalculateImpact(roles, trackableFoes, food, buildings);
            var result = new List<(ITrackableRole trackableRole, int damage)>();
            if (impact.Damage > 0)
            {
                var total = impact.Damage;
                using var roleEnumerator = ReOrderCuttingSequenceDueToInsufficientSoldiers(roles).GetEnumerator();

                while (impact.Damage > 0 && roleEnumerator.MoveNext())
                {
                    var currentRole = roleEnumerator.Current;
                    if (currentRole == null) break;
                    if (currentRole.CurrentState <= impact.Damage)
                    {
                        result.Add(new ValueTuple<ITrackableRole, int>(currentRole, currentRole.CurrentState));
                        total -= currentRole.CurrentState;
                    }
                    else
                    {
                        result.Add(new ValueTuple<ITrackableRole, int>(currentRole, currentRole.CurrentState - total));
                        total = 0;
                    }
                }
            }
            return (result, impact.FoodStealed, impact.BuildingDestroied);
        }
        finally
        {
            if(roles != null)
                Array.Clear(roles, 0, roles.Length);
        }
    }
}