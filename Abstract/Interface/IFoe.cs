namespace Abstract.Interface;

public interface IFoe
{
    string Description { get; }
    int MaxHitPoint { get; }
    int AttackPower { get; }
}

public interface IFoodThief : IFoe
{
    public int FoodSteal { get; }
}

public interface IBuildingDestroyer : IFoe
{
    public int BuildingDestroy { get; }
}

public interface IRelicBearer : IFoe
{
    public IRelic Relic { get; }
}