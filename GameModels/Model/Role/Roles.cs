using System;
using Abstract;
using Abstract.Interface;

namespace GameModels.Model.Role;

internal class Civilian : IRole
{
    public string Description { get; } = "Civilian";
    public int FoodConsumption { get; } = 1;
    public int BedOccupied { get; } = 1;
    public int MaxHitPoint { get; } = 1;
    public int RecruitCost { get; } = 1;
}

public class Soldier : IRole, ISoldier
{   
    public string Description { get; } = "Soldier";
    public int FoodConsumption { get; } = 2;
    public int BedOccupied { get; } = 1;
    public int MaxHitPoint { get; } = 1;
    public int RecruitCost => 2 * FoodConsumption;
    public int Power { get; } = 1;
}

public class Farmer : IRole, IFarmer
{
    public string Description { get; } = "Farmer";
    public int FoodConsumption { get; } = 1;
    public int BedOccupied { get; } = 1;
    public int MaxHitPoint { get; } = 1;
    public int RecruitCost => 2 * FoodConsumption;
}

public class WheatFarmer : IRole, IWheatFarmer
{   
    public string Description { get; } = "Wheat Framer";
    public int FoodConsumption { get; } = 1;
    public int BedOccupied { get; } = 1;
    public int MaxHitPoint { get; } = 1;
    public int RecruitCost => 2 * FoodConsumption;

}

public class RiceFarmer : IRole, IRiceFarmer
{
    public string Description { get; } = "Rice Farmer";
    public int FoodConsumption { get; } = 1;
    public int BedOccupied { get; } = 1;
    public int MaxHitPoint { get; } = 1;
    public int RecruitCost => 2 * FoodConsumption;

}

public class Builder : IRole, IBuilder
{
    public string Description { get; } = "Builder";
    public int FoodConsumption { get; } = 1;
    public int BedOccupied { get; } = 1;
    public int MaxHitPoint { get; } = 1;
    public int RecruitCost => 2 * FoodConsumption;

}

public class Foe : IFoe
{
    public string Description { get; } = "Common Foe";
    public int MaxHitPoint { get; } = 1;
    public int AttackPower { get; } = 1;
}

public class FoodThief : IFoodThief
{
    public string Description { get; } = "Food Thief";
    public int MaxHitPoint { get; } = 1;
    public int AttackPower { get; } = 1;
    public int FoodSteal { get; } = 3;
}

public class BuildingDestroyer : IBuildingDestroyer
{
    public string Description { get; } = "Building Destroyer";
    public int MaxHitPoint { get; } = 5;
    public int AttackPower { get; } = 5;
    public int BuildingDestroy { get; } = 1;
}

public class RelicBearer(IRelic relic) : IRelicBearer
{
    public string Description { get; } = "Relic Bearer";
    public int MaxHitPoint { get; } = 10;
    public int AttackPower { get; } = 10;
    public IRelic Relic { get; } = relic;
}