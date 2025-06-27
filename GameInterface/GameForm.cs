using System;
using System.Drawing;
using System.Windows.Forms;
using GameLogic;
using GameLogic.Manager;

namespace GameInterface
{
    public partial class GameForm : Form
    {
        private readonly IGameContext _gameContext;
        private readonly TurnProcessor _turnManager;

        private GameForm()
        {
            InitializeComponent(); // 這裡會呼叫 `GameForm.Designer.cs` 中的 UI 初始化
        }

        public GameForm(TurnProcessor manager) : this()
        {
            _turnManager = manager;
            _gameContext = _turnManager.GameContext;
            UpdateUi();
            btnNextTurn.Click += BtnNextTurn_Click;
        }

        private void Recruit_ValueChanged(object sender, EventArgs e)
        {
            var result = _gameContext.IsFoodEnoughForRecruitment((int)nudRecruitFarmer.Value,
                (int)nudRecruitSoldier.Value,
                (int)nudRecruitBuilder.Value, 0, 0);
            if (result.IsEnough)
                nudFoodConsumption.ForeColor = Color.Black;
            else
                nudFoodConsumption.ForeColor = Color.Red;

            nudFoodConsumption.Value = result.RecruitmentConsumption;
        }

        private void NudRole_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender is NumericUpDown o)) return;
            var newValue = (int)o.Value;
            int.TryParse(o.Tag?.ToString() ?? null, out var oldValue);
            var diff = newValue - oldValue;
            o.Tag = newValue;
            nudRemain.Value -= diff;
        }

        private void BtnNextTurn_Click(object sender, EventArgs e)
        {
            try
            {
                nudFarmer.ReadOnly = nudWheatFarmer.ReadOnly =
                    nudRiceFarmer.ReadOnly = nudSoldier.ReadOnly = nudBuilder.ReadOnly = true;
                nudRecruitFarmer.ReadOnly = nudRecruitWheatFarmer.ReadOnly = nudRecruitRiceFarmer.ReadOnly =
                    nudRecruitSoldier.ReadOnly = nudRecruitBuilder.ReadOnly = true;

                var recruitCheck = CheckBeforeRecruit();
                if (recruitCheck.IsFoodEnough)
                {
                    nudRecruitFarmer.Value = nudRecruitSoldier.Value =
                        nudRecruitBuilder.Value = nudWheatFarmer.Value = nudRiceFarmer.Value = 0;
                }
                else
                {
                    MessageBox.Show($@"糧食不足，缺少 {recruitCheck.Shortfall} 單位");
                    return;
                }

                var isFullyDistributed = IsRolesFullyDistributed();
                if (!isFullyDistributed.IsFullyDistributed)
                {
                    MessageBox.Show(@"調整角色數量尚未完成");
                    return;
                }

                _turnManager.TurnStart(new TurnProcessor.UserInput(recruitCheck.RecruitedFarmers,
                    recruitCheck.RecruitWheatFarmers, recruitCheck.RecruitRiceFarmers,
                    recruitCheck.RecruitedSoldiers, recruitCheck.RecruitedBuilders, isFullyDistributed.Farmers,
                    isFullyDistributed.WheatFarmers, isFullyDistributed.RiceFarmers,
                    isFullyDistributed.Soldiers, isFullyDistributed.Builders));
                UpdateUi();
            }
            finally
            {
                nudFarmer.ReadOnly = nudWheatFarmer.ReadOnly =
                    nudRiceFarmer.ReadOnly = nudSoldier.ReadOnly = nudBuilder.ReadOnly = false;
                nudRecruitFarmer.ReadOnly = nudRecruitWheatFarmer.ReadOnly = nudRecruitRiceFarmer.ReadOnly =
                    nudRecruitSoldier.ReadOnly = nudRecruitBuilder.ReadOnly = false;
            }
        }

        private (bool IsFoodEnough, decimal Shortfall, int RecruitedFarmers, int RecruitWheatFarmers, int
            RecruitRiceFarmers, int RecruitedSoldiers, int RecruitedBuilders)
            CheckBeforeRecruit()
        {
            var farmers = (int)nudRecruitFarmer.Value;
            var soldiers = (int)nudRecruitSoldier.Value;
            var builders = (int)nudRecruitBuilder.Value;
            var wheatFarmers = (int)nudWheatFarmer.Value;
            var riceFarmers = (int)nudRiceFarmer.Value;
            var result =
                _gameContext.IsFoodEnoughForRecruitment(farmers, wheatFarmers, riceFarmers, soldiers, builders);
            return (result.IsEnough, _gameContext.Food - result.RecruitmentConsumption, farmers, wheatFarmers,
                riceFarmers, soldiers, builders);
        }

        private (bool IsFullyDistributed, int Farmers, int WheatFarmers, int RiceFarmers, int Soldiers, int Builders)
            IsRolesFullyDistributed()
        {
            var farmers = (int)nudFarmer.Value;
            var wheatFarmers = (int)nudWheatFarmer.Value;
            var riceFarmers = (int)nudRiceFarmer.Value;
            var soldiers = (int)nudSoldier.Value;
            var builders = (int)nudBuilder.Value;
            var remain = (int)nudRemain.Value;
            return (remain == 0, farmers, wheatFarmers, riceFarmers, soldiers, builders);
        }

        private void UpdateUi()
        {
            try
            {
                nudFarmer.ValueChanged -= NudRole_ValueChanged;
                nudWheatFarmer.ValueChanged -= NudRole_ValueChanged;
                nudRiceFarmer.ValueChanged -= NudRole_ValueChanged;
                nudSoldier.ValueChanged -= NudRole_ValueChanged;
                nudBuilder.ValueChanged -= NudRole_ValueChanged;
                nudRecruitFarmer.ValueChanged -= Recruit_ValueChanged;
                nudRecruitWheatFarmer.ValueChanged -= Recruit_ValueChanged;
                nudRecruitRiceFarmer.ValueChanged -= Recruit_ValueChanged;
                nudRecruitSoldier.ValueChanged -= Recruit_ValueChanged;
                nudRecruitBuilder.ValueChanged -= Recruit_ValueChanged;

                nudFarmer.Value = _gameContext.FarmersCount;
                nudWheatFarmer.Value = _gameContext.WheatFarmerCount;
                nudRiceFarmer.Value = _gameContext.RiceFarmerCount;
                nudSoldier.Value = _gameContext.SoldiersCount;
                nudBuilder.Value = _gameContext.BuildersCount;
                lblFoesCount.Text = _gameContext.FoesCount.ToString();
                lblFood.Text = _gameContext.Food.ToString("n2");
                lblBeds.Text = _gameContext.Beds.ToString();
                lblBuildings.Text = _gameContext.BuildingCompletedCount.ToString();
                lblTurn.Text = _gameContext.Turns.ToString();
                nudFarmer.Tag = nudFarmer.Value;
                nudWheatFarmer.Tag = nudWheatFarmer.Value;
                nudRiceFarmer.Tag = nudRiceFarmer.Value;
                nudSoldier.Tag = nudSoldier.Value;
                nudBuilder.Tag = nudBuilder.Value;
            }
            finally
            {
                nudFarmer.ValueChanged += NudRole_ValueChanged;
                nudWheatFarmer.ValueChanged += NudRole_ValueChanged;
                nudRiceFarmer.ValueChanged += NudRole_ValueChanged;
                nudSoldier.ValueChanged += NudRole_ValueChanged;
                nudBuilder.ValueChanged += NudRole_ValueChanged;
                nudRecruitFarmer.ValueChanged += Recruit_ValueChanged;
                nudRecruitWheatFarmer.ValueChanged += Recruit_ValueChanged;
                nudRecruitRiceFarmer.ValueChanged += Recruit_ValueChanged;
                nudRecruitSoldier.ValueChanged += Recruit_ValueChanged;
                nudRecruitBuilder.ValueChanged += Recruit_ValueChanged;
            }
        }
    }
}