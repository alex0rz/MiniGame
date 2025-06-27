using System;
using Abstract.Interface;

namespace GameModels.Model.Building
{
    public class Building : IBuilding
    {
        public string Description { get; set; }
        public int BedProvided { get;} = 2;
        public int TurnsForBuildingPeriod { get; } = 1;
    }
}
