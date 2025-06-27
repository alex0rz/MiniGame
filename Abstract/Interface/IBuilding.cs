using System;

namespace Abstract.Interface
{
    public interface IBuilding
    {
        string Description { get; }
        int BedProvided { get; }
        int TurnsForBuildingPeriod { get; }
    }
}