using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic.Interface;

namespace GameLogic.Phase
{
    internal class SummaryPhase : IPhase
    {
        public string Description { get; } = "總結";
        public void ExecutePhase(GameContext gameContext)
        {
            if (gameContext.Farmers.Any() || gameContext.Soldiers.Any() || gameContext.Builders.Any())
            {
                gameContext.RemoveRelicBearer();
                gameContext.IncreaseTurns();
            }
            else
                gameContext.GameFinished = true;
        }
    }
}
