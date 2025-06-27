using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Abstract.Interface;
using GameLogic.Interface;
using GameModels.Model.Weather;
[assembly: InternalsVisibleTo("GameLogic.Test")]

namespace GameLogic.Manager;

public class WeatherManager
{
    private class TrackableWeather : ITrackableWeather, IEquatable<TrackableWeather>
    {
        public TrackableWeather(IWeather weather, int maxTurns)
        {
            this.UniqueId = new UniqueId();
            Weather = weather;
            LastTurns = 0;
            IsAvailable = true;
        }

        public UniqueId UniqueId { get; }
        public string Description => Weather.Description;
        public int CurrentState => LastTurns;
        public bool IsAvailable { get; }
        public IWeather Weather { get; }
        public int LastTurns { get; set; }
        public bool Equals(TrackableWeather other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return UniqueId.Equals(other.UniqueId);
        }

        public override bool Equals(object obj) => Equals(obj as TrackableWeather);

        public override int GetHashCode() => this?.UniqueId.GetHashCode() ?? 0;
        
        public static bool operator ==(TrackableWeather x, TrackableWeather y) => Equals(x, y);

        public static bool operator !=(TrackableWeather x, TrackableWeather y) => !(x == y);
    }

    private const int WeatherLastingTurns = 2;
    private TrackableWeather CreateComfortWeather() => new TrackableWeather(new ComfortWeather(null), WeatherLastingTurns);

    private TrackableWeather CreateHotSummer() => new TrackableWeather(new HotSummer(null), WeatherLastingTurns);

    private TrackableWeather CreateFreezingCold() => new TrackableWeather(new Winter(null), WeatherLastingTurns);

    private readonly Dictionary<WeatherType, Func<TrackableWeather>> _weatherHandlers;

    public ITrackableWeather Weather => _trackedWeather;
    private TrackableWeather _trackedWeather;

    public WeatherManager()
    {
        _weatherHandlers = new Dictionary<WeatherType, Func<TrackableWeather>>
        {
            [WeatherType.Comfort] = CreateComfortWeather,
            [WeatherType.DamnHot] = CreateHotSummer,
            [WeatherType.FreezingCold] = CreateFreezingCold
        };
    }

    internal bool TryChangeWeather(WeatherType weatherType)
    {

        if (_trackedWeather?.Weather.WeatherType == weatherType)
            return false;
        else if (_weatherHandlers.TryGetValue(weatherType, out var handler))
        {
            _trackedWeather = handler();
            return true;
        }
        else
            return false;
    }

    internal void IncreaseTurns(int turns = 1) => _trackedWeather.LastTurns += turns;
}

public class WeatherManagerProxy
{
    public WeatherManagerProxy(MessageManager messageManager)
    {
        _weatherManager = new WeatherManager();
        _messageManager = messageManager;
    }

    public ITrackableWeather Weather => _weatherManager.Weather;
    private readonly WeatherManager _weatherManager;
    private readonly MessageManager _messageManager;

    internal bool TryChangeWeather(WeatherType weatherType)
    {
        try
        {
            return _weatherManager.TryChangeWeather(weatherType);
        }
        finally
        {
            _messageManager.LogMessage(@$"Weather Changed {_weatherManager.Weather.Weather.Description}");
        }
    }

    internal void IncreaseTurns(int turns = 1)
    {
        _weatherManager.IncreaseTurns(turns);
    }
}