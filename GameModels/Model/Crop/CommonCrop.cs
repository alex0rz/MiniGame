using System;
using Abstract.Interface;

namespace GameModels.Model.Crop
{
    public class CommonCrop : ICrop
    {   
        public string Description { get; } = "Common Crop";
        public int TurnsForGrowingPeriod { get; } = 1;
        public int FoodProduction { get; } = 2;
    }

    public class Wheat : ICrop
    {
        public string Description { get; } = "Wheat";
        public int TurnsForGrowingPeriod { get; } = 3;
        public int FoodProduction { get; } = 6;
    }

    public class Rice : ICrop
    {
        public string Description { get; } = "Rice";
        public int TurnsForGrowingPeriod { get; } = 3;
        public int FoodProduction { get; } = 8;
    }
}