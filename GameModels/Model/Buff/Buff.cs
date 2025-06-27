using Abstract.Interface;

namespace GameModels.Model.Buff;

public class Buff : IBuff
{
    public string Description { get; }
    public BuffType BuffType { get; }
    public int MaxBuffTurns { get; }
}

public class FreezeCropsGrowth : IBehaviorBuff
{
    public string Description { get; } = "嚴寒停止作物生長";
    public BuffType BuffType { get; } = BuffType.Food;
    public int MaxBuffTurns { get; } = 2;
}

public class CutCropFoodProduction : INumericBuff
{
    public string Description { get; } = "食物產量減半";
    public BuffType BuffType { get; } = BuffType.Food;
    public int MaxBuffTurns { get; } = 2;
    public decimal BuffValue { get; } = .5m;
}

public class DoubleCropFoodProduction : INumericBuff
{
    public string Description { get; } = "食物產量翻倍";
    public BuffType BuffType { get; } = BuffType.Food;
    public int MaxBuffTurns { get; } = 2;
    public decimal BuffValue { get; } = 2m;
}

public class RaiseRecruitCost : INumericBuff
{
    public string Description { get; } = "招募成本提高";
    public BuffType BuffType { get; } = BuffType.Recruit;
    public int MaxBuffTurns { get; } = 2;
    public decimal BuffValue { get; } = 1.5m;
}

public class ReduceRecruitCost : INumericBuff
{
    public string Description { get; } = "招募成本降低";
    public BuffType BuffType { get; } = BuffType.Recruit;
    public int MaxBuffTurns { get; } = 2;
    public decimal BuffValue { get; } = .5m;
}