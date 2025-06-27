namespace Abstract.Interface;

public interface IBuff
{
    public string Description { get; }
    public BuffType BuffType { get; }
    public int MaxBuffTurns { get; }
}

public enum BuffType
{
    None,
    Food,
    Recruit,

}

public interface INumericBuff : IBuff
{
    public decimal BuffValue { get; }
}

public interface IBehaviorBuff : IBuff
{
}