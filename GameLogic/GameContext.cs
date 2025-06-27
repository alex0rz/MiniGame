using System.Collections.Generic;
using System.Linq;
using Abstract.Interface;
using GameLogic.Interface;
using GameLogic.Manager;

namespace GameLogic;

public interface IGameContext
{
    decimal Food { get; }
    int Beds { get; }
    int BuildingCompletedCount { get; }
    int RolesCount { get; }
    int FarmersCount { get; }
    int WheatFarmerCount { get; }
    int RiceFarmerCount { get; }
    int SoldiersCount { get; }
    int BuildersCount { get; }
    int FoesCount { get; }
    int Turns { get; }
    bool GameFinished { get; }
    IReadOnlyCollection<string> Messages { get; }

    (bool IsEnough, decimal RecruitmentConsumption) IsFoodEnoughForRecruitment(int farmers, int wheatFarmers,
        int riceFarmers,
        int soldiers, int builders);
}

public class GameContext : IGameContext
{
    public GameContext(TurnProcessor.InitialSettingUps initial, MessageManager messageManager )
    {
        this._resourceManagerProxy = new ResourceManagerProxy(initial.Buildings, initial.Food, messageManager);
        this._unitManagerProxy = new UnitManagerProxy(
            initial.Farmers,
            initial.WheatFarmers,
            initial.RiceFarmers,
            initial.Soldiers,
            initial.Builders,
            initial.Foes,
            initial.FoodThieves,
            initial.BuildingDestroyers,
            initial.RelicBearers,
            messageManager);
        this._weatherManagerProxy = new WeatherManagerProxy(messageManager);
        this._buffManagerProxy = new BuffManagerProxy(messageManager);
        this._possessionManagerProxy = new PossessionManagerProxy(messageManager);
        this._messageManager = messageManager;
        this.TryChangeWeather(initial.WeatherType);
    }

    public GameContext(ResourceManagerProxy resourceManagerProxy, UnitManagerProxy unitManagerProxy,
        WeatherManagerProxy weatherManagerProxy, BuffManagerProxy buffManagerProxy,
        MessageManager messageManager, WeatherType weatherType)
    {
        _resourceManagerProxy = resourceManagerProxy;
        _unitManagerProxy = unitManagerProxy;
        _weatherManagerProxy = weatherManagerProxy;
        _buffManagerProxy = buffManagerProxy;
        _messageManager = messageManager;
    }

    public ITrackableWeather WeatherStatus => _weatherManagerProxy.Weather;
    public int BuildingCompletedCount => _resourceManagerProxy.Buildings.Count;
    public int RolesCount => _unitManagerProxy.Roles.Count;
    public int FarmersCount => _unitManagerProxy.Farmers.Count;
    public int WheatFarmerCount => _unitManagerProxy.WheatFarmers.Count;
    public int RiceFarmerCount => _unitManagerProxy.RiceFarmers.Count;
    public int BuildersCount => _unitManagerProxy.Builders.Count;
    public int SoldiersCount => _unitManagerProxy.Soldiers.Count;
    public int FoesCount => _unitManagerProxy.Foes.Count;
    public int Turns { get; private set; } = 1;
    public bool GameFinished { get; internal set; }
    public IReadOnlyCollection<string> Messages => _messageManager.Message;
    public decimal Food => _resourceManagerProxy.Food;
    public int Beds => _resourceManagerProxy.Beds;
    public IReadOnlyCollection<ITrackableRole> Roles => _unitManagerProxy.Roles;
    public IReadOnlyCollection<ITrackableFoe> Foes => _unitManagerProxy.Foes;
    public IReadOnlyCollection<ITrackableRole> Soldiers => _unitManagerProxy.Soldiers;
    public IReadOnlyCollection<ITrackableRole> Builders => _unitManagerProxy.Builders;
    public IReadOnlyCollection<ITrackableRole> Farmers => _unitManagerProxy.Farmers;
    public IReadOnlyCollection<ITrackableCrop> Crops => _resourceManagerProxy.Crops;
    public IReadOnlyCollection<ITrackableBuilding> Buildings => _resourceManagerProxy.Buildings;
    public IReadOnlyCollection<ITrackableBuff> Buffs => _buffManagerProxy.Buffs;

    private readonly ResourceManagerProxy _resourceManagerProxy;
    private readonly UnitManagerProxy _unitManagerProxy;
    private readonly WeatherManagerProxy _weatherManagerProxy;
    private readonly BuffManagerProxy _buffManagerProxy;
    private readonly MessageManager _messageManager;
    private readonly PossessionManagerProxy _possessionManagerProxy;

    internal void IncreaseTurns()
    {
        this.Turns += 1;
        this._weatherManagerProxy.IncreaseTurns();
        this._buffManagerProxy.IncreaseTurns();
        this._possessionManagerProxy.IncreaseTurns();
    }

    internal bool TryChangeWeather(WeatherType weatherType)
    {
        var result = false;
        try
        {
            return result = _weatherManagerProxy.TryChangeWeather(weatherType);
        }
        finally
        {
            if (result)
            {
                var currentWeather = _weatherManagerProxy.Weather.Weather.WeatherType;
                foreach (var buff in _weatherManagerProxy.Weather.Weather.WeatherBuffs)
                {
                    _buffManagerProxy.AddBuff(buff, () => _weatherManagerProxy.Weather.Weather.WeatherType == currentWeather);
                }
            }
        }
    }

    internal void Recruit(TurnProcessor.UserInput userInput)
    {
        Recruit(userInput.RecruitedFarmers, userInput.RecruitedWheatFarmers, userInput.RecruitedRiceFarmers,
            userInput.RecruitedSoldiers, userInput.RecruitedBuilders);
    }

    internal void Recruit(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders)
    {
        var recruitCost =
            _unitManagerProxy.CalculateRecruitCost(farmers, wheatFarmers, riceFarmers, soldiers, builders, _buffManagerProxy.Buffs);
        if (recruitCost <= _resourceManagerProxy.Food)
        {
            _unitManagerProxy.Recruit(farmers, wheatFarmers, riceFarmers, soldiers, builders);
            _resourceManagerProxy.ConsumeFood(recruitCost);
        }
    }

    internal void Adjust(TurnProcessor.UserInput userInput)
    {
        Adjust(userInput.AdjustedFarmers, userInput.AdjustedWheatFarmers, userInput.AdjustedRiceFarmers,
            userInput.AdjustedSoldiers, userInput.AdjustedBuilders);
    }

    internal void Adjust(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders)
    {
        _unitManagerProxy.Adjust(farmers, wheatFarmers, riceFarmers, soldiers, builders);
    }

    internal void Plant(PlantResult plantResult, IEnumerable<IBuff> buffs)
    {
        _resourceManagerProxy.Plant(plantResult, buffs);
    }

    internal void Build(BuildResult buildResult)
    {
        _resourceManagerProxy.Build(buildResult);
    }

    internal void ConsumeFood(decimal foodAte)
    {
        _resourceManagerProxy.ConsumeFood(foodAte);
    }

    internal void DestroyBuilding(IEnumerable<ITrackableBuilding> destroyedBuildings)
    {
        _resourceManagerProxy.DestroyBuildings(destroyedBuildings);
    }

    internal void TakeCasualties(IEnumerable<(ITrackableRole TrackableRole, int Damage)> casualties)
    {
        _unitManagerProxy.TakeCasualties(casualties);
        Clean();
    }

    internal void AttackEnemies(IEnumerable<(ITrackableFoe TrackableFoe, int Damage)> combatResults)
    {
        var killed = _unitManagerProxy.AttackEnemies(combatResults);
        var possessions = killed.Where(k => k.Foe is IRelicBearer { Relic: not null })
            .Select(k => (k.Foe as IRelicBearer).Relic);
        foreach (var possession in possessions)
        {
            _possessionManagerProxy.AddRelic(possession);
            if(possession.Buff != null)
                _buffManagerProxy.AddBuff(possession.Buff, null);
        }
    }

    internal void GenerateFoes(IEnumerable<IFoe> newFoes)
    {
        _unitManagerProxy.GenerateFoes(newFoes);
    }

    public (bool IsEnough, decimal RecruitmentConsumption) IsFoodEnoughForRecruitment(int farmers, int wheatFarmers,
        int riceFarmers, int soldiers, int builders)
    {
        var recruitCost =
            _unitManagerProxy.CalculateRecruitCost(farmers, wheatFarmers, riceFarmers, soldiers, builders, this.Buffs);
        return (recruitCost <= _resourceManagerProxy.Food, recruitCost);
    }

    internal void AdjustRoles(int farmers, int wheatFarmers, int riceFarmers, int soldiers, int builders)
    {
        if (farmers + wheatFarmers + riceFarmers + soldiers + builders == _unitManagerProxy.Roles.Count())
            _unitManagerProxy.Adjust(farmers, wheatFarmers, riceFarmers, soldiers, builders);
    }

    private void Clean()
    {
        _resourceManagerProxy.CleanTrackableCorps(Roles);
        _resourceManagerProxy.CleanTrackableBuildings(Roles);
    }

    internal void RemoveRelicBearer()
    {
        _unitManagerProxy.RemoveRelicBearer();
    }
}