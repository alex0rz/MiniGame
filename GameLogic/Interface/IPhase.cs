namespace GameLogic.Interface
{
    public interface IPhase
    {
        string Description { get; }
        
        void ExecutePhase(GameContext gameContext);
    }
}
