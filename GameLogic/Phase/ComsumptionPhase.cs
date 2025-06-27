using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract.Interface;
using GameLogic.Interface;

namespace GameLogic.Phase;

internal class ConsumptionPhase : IPhase
{
    public string Description { get; } = "Consumption Phase";

    public void ExecutePhase(GameContext gameContext)
    {
        CalculateConsumption(gameContext);
    }


    private void CalculateConsumption(in GameContext gameContext)
    {
        var foodCheckResult = IsFoodSufficient(gameContext.Roles, gameContext.Food);
        if (!foodCheckResult.IsSufficient)
        {
            var casualtiesDueToStarved = CutRolesDueToFoodShortage(gameContext.Roles, foodCheckResult.Shortage);
            gameContext.TakeCasualties(casualtiesDueToStarved);
        }

        gameContext.ConsumeFood(foodCheckResult.FoodConsumption);

        var bedCheckResult = IsBedSufficient(gameContext.Roles, gameContext.Beds);
        if (!bedCheckResult.IsSufficient)
        {
            var casualtiesDueToHomeless = CutRolesDueToBedShortage(gameContext.Roles, bedCheckResult.Shortage);
            gameContext.TakeCasualties(casualtiesDueToHomeless);
        }
    }

    private (bool IsSufficient, int Shortage) IsBedSufficient(IEnumerable<ITrackableRole> trackableRoles,
        int currentBeds)
    {
        var totalBedConsumption = trackableRoles.Sum(tr => tr.Role.BedOccupied);
        return (currentBeds >= totalBedConsumption, Math.Max(totalBedConsumption - currentBeds, 0));
    }

    private IEnumerable<ITrackableRole> ReOrderCuttingSequenceDueToBedShortage(
        IEnumerable<ITrackableRole> trackableRoles)
    {
        var roles = trackableRoles?.ToArray();
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

    private IEnumerable<(ITrackableRole Role, int Damage)> CutRolesDueToBedShortage(
        IEnumerable<ITrackableRole> trackableRoles, int bedShortage)
    {
        if (bedShortage > 0)
            foreach (var trackableRole in ReOrderCuttingSequenceDueToBedShortage(trackableRoles))
            {
                if (bedShortage >= 0)
                    yield return (trackableRole, trackableRole.FullState - trackableRole.CurrentState);
                else
                    break;
                bedShortage -= trackableRole.Role.BedOccupied;
            }
    }

    private (bool IsSufficient, decimal FoodConsumption, decimal Shortage) IsFoodSufficient(
        IEnumerable<ITrackableRole> trackableRoles,
        decimal currentFood)
    {
        var totalFoodConsumption = trackableRoles.Sum(r => r.Role.FoodConsumption);
        if (totalFoodConsumption <= currentFood)
            return (true, totalFoodConsumption, 0);
        else
            return (false, currentFood, totalFoodConsumption - currentFood);
    }

    private IEnumerable<ITrackableRole> ReOrderCuttingSequenceDueToFoodShortage(IEnumerable<ITrackableRole> roles)
    {
        return roles.OrderByDescending(r => r.Role.FoodConsumption);
    }

    private IEnumerable<(ITrackableRole Role, int Damage)> CutRolesDueToFoodShortage(IEnumerable<ITrackableRole> roles,
        decimal foodShortage)
    {
        if (foodShortage > 0)
            foreach (var trackableRole in ReOrderCuttingSequenceDueToFoodShortage(roles))
            {
                if (foodShortage > 0)
                    yield return (trackableRole, trackableRole.FullState - trackableRole.CurrentState);
                else
                    break;
                foodShortage -= trackableRole.Role.FoodConsumption;
            }
    }
}