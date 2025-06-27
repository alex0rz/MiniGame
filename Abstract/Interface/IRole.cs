using System;

namespace Abstract.Interface
{
    public interface IRole
    {   
        string Description { get; }
        int FoodConsumption { get; }
        int BedOccupied { get; }
        int MaxHitPoint { get; }
        int RecruitCost { get; }
    }
}
