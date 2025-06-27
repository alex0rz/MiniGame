using System;

namespace Abstract.Interface
{
    public interface ICrop
    {
        string Description { get; }
        int TurnsForGrowingPeriod { get; }
        int FoodProduction { get; }
    }
}