using System.Collections.Generic;
using Abstract.Interface;
using GameLogic.Interface;
using GameLogic.Phase;

namespace GameLogic.Manager;

public class TurnProcessor
{
    private readonly List<IPhase> _phases = new();
    public readonly GameContext GameContext;
    private readonly MessageManager _messageManager;
    public int Turn { get; private set; }

    public TurnProcessor(in InitialSettingUps initial, IEnumerable<IPhase> phases)
    {
        _phases.AddRange(phases);
        _messageManager = new MessageManager();
        GameContext = new GameContext(initial, _messageManager);
    }

    public TurnProcessor(InitialSettingUps initial) : this(initial, [
        new EnvironmentPhase(), new ConsumptionPhase(), new CombatPhase(), new YieldPhase(), new SummaryPhase()
    ])
    {
    }

    public void TurnStart(UserInput userInput)
    {
        if (GameContext.GameFinished)
            return;
        GameContext.Adjust(userInput);
        GameContext.Recruit(userInput);
        Process();
    }

    private void Process()
    {
        foreach (var phase in _phases)
        {
            _messageManager.LogMessage(phase.Description);
            phase.ExecutePhase(GameContext);
        }
    }

    internal void AddPhase(IPhase phase)
    {
        _phases.Add(phase);
    }

    public readonly struct UserInput(
        int recruitedFarmers,
        int recruitedWheatFarmers,
        int recruitedRiceFarmers,
        int recruitedSoldiers,
        int recruitedBuilders,
        int adjustedFarmers,
        int adjustedWheatFarmers,
        int adjustedRiceFarmers,
        int adjustedSoldiers,
        int adjustedBuilders)
    {
        public int RecruitedFarmers { get; } = recruitedFarmers;
        public int RecruitedWheatFarmers { get; } = recruitedWheatFarmers;
        public int RecruitedRiceFarmers { get; } = recruitedRiceFarmers;
        public int RecruitedSoldiers { get; } = recruitedSoldiers;
        public int RecruitedBuilders { get; } = recruitedBuilders;
        public int AdjustedFarmers { get; } = adjustedFarmers;
        public int AdjustedWheatFarmers { get; } = adjustedWheatFarmers;
        public int AdjustedRiceFarmers { get; } = adjustedRiceFarmers;
        public int AdjustedBuilders { get; } = adjustedBuilders;
        public int AdjustedSoldiers { get; } = adjustedSoldiers;
    }

    public readonly struct InitialSettingUps(
        int food,
        int building,
        int farmers,
        int wheatFarmers,
        int riceFarmers,
        int soldiers,
        int builders,
        int foes,
        int foodThieves,
        int buildingDestroyers,
        int relicBearers,
        WeatherType weatherType)
    {
        public int Food { get; } = food;
        public int Buildings { get; } = building;
        public int Farmers { get; } = farmers;
        public int WheatFarmers { get; } = wheatFarmers;
        public int RiceFarmers { get; } = riceFarmers;
        public int Builders { get; } = builders;
        public int Soldiers { get; } = soldiers;
        public int Foes { get; } = foes;
        public int FoodThieves { get; } = foodThieves;
        public int BuildingDestroyers { get; } = buildingDestroyers;
        public int RelicBearers { get; } = relicBearers;
        public WeatherType WeatherType { get; } = weatherType;
    }
}