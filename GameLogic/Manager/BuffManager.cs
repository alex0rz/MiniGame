using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Abstract.Interface;
using GameLogic.Interface;
[assembly: InternalsVisibleTo("GameLogic.Test")]

namespace GameLogic.Manager;

public class BuffManager
{
    private class TrackableBuff : ITrackableBuff, IEquatable<TrackableBuff>
    {
        public TrackableBuff(IBuff buff, Func<bool> available)
        {
            UniqueId = new UniqueId();
            Buff = buff;
            _available = available ?? (() => CurrentTurns <= Buff.MaxBuffTurns);
        }

        private readonly Func<bool> _available;
        public UniqueId UniqueId { get; }
        public string Description => Buff.Description;
        public int FullState => Buff.MaxBuffTurns;
        public int CurrentState => CurrentTurns;
        public bool IsAvailable => _available();
        public IBuff Buff { get; }
        public int CurrentTurns { get; set; }

        public override bool Equals(object obj) => Equals(obj as TrackableBuff);

        public bool Equals(TrackableBuff other) => other is not null && other.UniqueId == UniqueId;

        public override int GetHashCode() => this?.UniqueId.GetHashCode() ?? 0;

        public static bool operator ==(TrackableBuff x, TrackableBuff y) => Equals(x, y);

        public static bool operator !=(TrackableBuff x, TrackableBuff y) => !(x == y);
    }

    private readonly HashSet<TrackableBuff> _trackedBuffs = new();
    public IReadOnlyCollection<ITrackableBuff> Buffs => _trackedBuffs.Where(b => b.IsAvailable).ToArray();

    internal void AddBuff(IBuff buff, Func<bool> available)
    {
        _trackedBuffs.Add(new TrackableBuff(buff, available));
    }

    private void Clean()
    {
        _trackedBuffs.RemoveWhere(tb => !tb.IsAvailable);
    }

    internal void IncreaseTurns()
    {
        foreach (var buff in _trackedBuffs)
        {
            buff.CurrentTurns++;
        }
        Clean();
    }
}

public class BuffManagerProxy(MessageManager messageManager)
{
    private readonly BuffManager _buffManager = new();
    public IReadOnlyCollection<ITrackableBuff> Buffs => _buffManager.Buffs;

    internal void AddBuff(IBuff buff, Func<bool> available)
    {
        _buffManager.AddBuff(buff, available);
        messageManager.LogMessage(@$"Add Buff {buff.Description}");
    }

    internal void IncreaseTurns()
    {
        _buffManager.IncreaseTurns();
        messageManager.LogMessage("Clean Buffs");
    }
}