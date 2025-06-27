using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract.Interface;
using GameModels.Model.Buff;

namespace GameModels.Model.Weather;

public class ComfortWeather(IEnumerable<IBuff> buffs) : IWeather
{
    public string Description { get; } = "Comfort Weather";
    public IEnumerable<IBuff> WeatherBuffs { get; } = buffs ?? new List<IBuff>();
    public WeatherType WeatherType { get; } = WeatherType.Comfort;
}

public class HotSummer(IEnumerable<IBuff> buffs) : IWeather
{
    public string Description { get; } = "Damn Hot";
    public IEnumerable<IBuff> WeatherBuffs { get; } = buffs ?? new List<IBuff>() { new RaiseRecruitCost() };
    public WeatherType WeatherType { get; } = WeatherType.DamnHot;
}

public class Winter(IEnumerable<IBuff> buffs) : IWeather
{
    public string Description { get; } = "Freezing Cold";

    public IEnumerable<IBuff> WeatherBuffs { get; } = buffs ?? new List<IBuff>()
        { new CutCropFoodProduction(), new FreezeCropsGrowth() };

    public WeatherType WeatherType { get; } = WeatherType.FreezingCold;
}