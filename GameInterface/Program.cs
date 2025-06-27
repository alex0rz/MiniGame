using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Abstract.Interface;
using GameLogic.Manager;

namespace GameInterface
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var initialSetting = new TurnProcessor.InitialSettingUps(10, 10, 10, 0, 0, 5, 1, 0, 0, 0, 0, WeatherType.Comfort);
            var turnManager = new TurnProcessor(initialSetting);
            Application.Run(new GameForm(turnManager));
        }
    }
}
