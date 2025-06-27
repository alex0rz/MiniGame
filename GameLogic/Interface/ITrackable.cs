using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using Abstract.Interface;
using GameModels.Model.Role;

namespace GameLogic.Interface;

public sealed class UniqueId : IEquatable<UniqueId>
{
    public Guid Id { get; } = Guid.NewGuid();

    public bool Equals(UniqueId other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is UniqueId other && Equals(other);
    public override int GetHashCode() => this?.Id.GetHashCode() ?? 0;
    public static bool operator ==(UniqueId x, UniqueId y) => Equals(x, y);
    public static bool operator !=(UniqueId x, UniqueId y) => !(x == y);
}

public interface ITrackable
{
    UniqueId UniqueId { get; }
    string Description { get; }
    int CurrentState { get; }
    bool IsAvailable { get; }
}

public interface ITrackableFoe : ITrackable
{
    int FullState { get; }
    IFoe Foe { get; }
}

public interface ITrackableRole : ITrackable
{
    int FullState { get; }
    IRole Role { get; }
}

public interface ITrackableCrop : ITrackable
{
    int FullState { get; }
    ICrop Crop { get; }
    ITrackableRole Farmer { get; }
    int CurrentGrowthTurns { get; }
}

public interface ITrackableBuilding : ITrackable
{
    int FullState { get; }
    IBuilding Building { get; }
    ITrackableRole Builder { get; }
    int CurrentBuildTurns { get; }
}

public interface ITrackableBuff : ITrackable
{
    int FullState { get; }
    IBuff Buff { get; }
}

public interface ITrackableWeather : ITrackable
{
    IWeather Weather { get; }
}

public interface ITrackableRelic : ITrackable
{
    int FullState { get; }
    IRelic Relic { get; }
}