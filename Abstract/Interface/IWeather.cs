using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract.Interface
{
    public enum WeatherType
    {
        Comfort,
        FreezingCold,
        DamnHot
    }
    public interface IWeather
    {
        string Description { get; }
        IEnumerable<IBuff> WeatherBuffs { get; }
        WeatherType WeatherType { get; }
    }
}
