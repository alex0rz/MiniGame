using Abstract.Interface;
using System.Collections.Generic;
using GameLogic.Interface;
using GameLogic.Manager;
using GameModels.Model.Role;

namespace GameLogic.Phase;

public class EnvironmentPhase : IPhase
{   
    public void ExecutePhase(GameContext gameContext)
    {
        gameContext.GenerateFoes(GenerateFoes());
        if(gameContext.WeatherStatus.CurrentState >= 2)
            gameContext.TryChangeWeather(GetNextWeatherType(gameContext.WeatherStatus.Weather.WeatherType));
    }

    private WeatherType GetNextWeatherType(WeatherType currentWeatherType)
    {
        switch (currentWeatherType)
        {
            case WeatherType.Comfort:
                return WeatherType.FreezingCold;
            case WeatherType.FreezingCold:
                return WeatherType.DamnHot;
            default:
                return WeatherType.Comfort;
        }
    }

    private IEnumerable<IFoe> GenerateFoes()
    {
        return new List<IFoe>(){ new Foe(), new FoodThief(), new BuildingDestroyer(), new RelicBearer(null)};
    }

    public string Description { get; }

    private struct GetWeatherBuffs
    {
        public IEnumerable<IBuff> GetBuffs(WeatherType weatherType)
        {
            switch (weatherType)
            {
                case WeatherType.DamnHot:
                    return new IBuff[] { };
                case WeatherType.FreezingCold:
                    return null;
                default:
                    return null;
            }
        }
    }
}