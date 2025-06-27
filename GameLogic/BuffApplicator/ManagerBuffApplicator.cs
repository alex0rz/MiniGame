using System.Collections.Generic;
using System.Linq;
using Abstract.Interface;
using GameLogic.Interface;
using GameModels.Model.Crop;
using GameModels.Model.Role;

namespace GameLogic.BuffApplicator;

internal struct ManagerBuffApplicator
{
    internal struct RecruitCost
    {
        public decimal CalculateRecruitCost(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders,
            IEnumerable<IBuff> buffs)
        {
            decimal result = farmers * new Farmer().RecruitCost +
                             wheatFarmers * new WheatFarmer().RecruitCost +
                             riceFarmers * new RiceFarmer().RecruitCost +
                             soldiers * new Soldier().RecruitCost +
                             builders * new Builder().RecruitCost;
            if (buffs?.Any(b => b is INumericBuff && b.BuffType == BuffType.Recruit) ?? false)
                result *= buffs.OfType<INumericBuff>().Where(b => b.BuffType == BuffType.Recruit).Sum(b => b.BuffValue);
            return result;
        }
    }

    internal struct CutCropProduction
    {
        public decimal CalculateFoodProduction(IEnumerable<ITrackableCrop> trackedCrops, IEnumerable<IBuff> buffs)
        {
            var result = 0m;
            if (buffs?.Any(b => b is INumericBuff && b.BuffType == BuffType.Food) ?? false)
            {
                var totalBuff = buffs.OfType<INumericBuff>().Where(b => b.BuffType == BuffType.Food)
                    .Sum(b => b.BuffValue);
                foreach (var maturedCrop in trackedCrops.Select(tc => tc.Crop)
                             .ToArray())
                    switch (maturedCrop)
                    {
                        case CommonCrop c:
                            result += c.FoodProduction * totalBuff;
                            break;
                        case Rice r:
                            continue;
                        default:
                            result += maturedCrop.FoodProduction;
                            break;
                    }
            }
            else
            {
                result = trackedCrops.Where(tc => tc.IsAvailable).Sum(tc => tc.Crop.FoodProduction);
            }
            return result;
        }
    }
}