using System.Collections.Generic;
using System.Linq;
using Abstract.Interface;
using GameLogic.Interface;
using GameLogic.Manager;
using GameModels.Model.Buff;
using GameModels.Model.Crop;

namespace GameLogic.BuffApplicator;

internal struct PhaseBuffApplicator
{
    internal struct CutCropGrowth
    {
        public PlantResult Plant(PlantResult plantResult, IEnumerable<IBuff> buffs)
        {
            if (buffs?.Any(b => b is FreezeCropsGrowth) ?? false)
            {
                var newCrops = plantResult.NewCrops.Where(nc => nc.Crop is not Rice);
                var growingCrops = plantResult.GrowingCrops.Where(gc => gc.Crop is not Rice);
                return new PlantResult(newCrops, growingCrops);
            }
            return plantResult;
        }
    }
}