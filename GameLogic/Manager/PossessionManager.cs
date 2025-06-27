using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Abstract;
using GameLogic.Interface;
[assembly:InternalsVisibleTo("GameLogic.Test")]
namespace GameLogic.Manager;

internal class PossessionManager
{
    private class TrackableRelic : ITrackableRelic, IEquatable<TrackableRelic>
    {
        public TrackableRelic(IRelic relic)
        {
            UniqueId = new UniqueId();
            Relic = relic;
        }

        public UniqueId UniqueId { get; }
        public string Description => Relic.Description;
        public int CurrentState => CurrentTurns;
        public bool IsAvailable => CurrentTurns <= FullState;
        public int FullState => Relic?.Buff?.MaxBuffTurns ?? 0;
        public IRelic Relic { get; }
        public int CurrentTurns { get; set; }

        public bool Equals(TrackableRelic other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return UniqueId == other.UniqueId;
        }

        public override bool Equals(object obj) => Equals((TrackableRelic)obj);

        public override int GetHashCode() => this?.UniqueId.GetHashCode() ?? 0;

        public static bool operator ==(TrackableRelic x, TrackableRelic y) => Equals(x, y);
    
        public static bool operator !=(TrackableRelic x, TrackableRelic y) => !(x == y);

    }

    private readonly HashSet<TrackableRelic> _trackedRelics = [];

    public IReadOnlyCollection<ITrackableRelic> TrackableRelics => _trackedRelics.ToArray();

    private void Clean()
    {
        _trackedRelics.RemoveWhere(tr => !tr.IsAvailable);
    }

    internal void AddRelic(IRelic relic)
    {
        _trackedRelics.Add(new TrackableRelic(relic));
    }

    internal void IncreaseTurns()
    {
        foreach (var relic in _trackedRelics) relic.CurrentTurns += 1;
        Clean();
    }
}

internal class PossessionManagerProxy(MessageManager messageManager)
{
    private readonly PossessionManager _possessionManager = new();
    private readonly MessageManager _messageManager = messageManager;

    internal void AddRelic(IRelic relic)
    {
        _possessionManager.AddRelic(relic);
        _messageManager.LogMessage($"Adding relic: {relic.Description}");
    }

    internal void IncreaseTurns()
    {
        _possessionManager.IncreaseTurns();
        _messageManager.LogMessage("Relics Clean");
    }
}