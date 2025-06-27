using System.Drawing;
using System.Reflection;
using System.Windows.Forms.VisualStyles;

namespace GameInterface
{
    using System.Windows.Forms;

    partial class GameForm
    {
        private Label lblTurn;
        private Label lblFood;
        private Label lblFoesCount;
        private Label lblBeds;
        private Label lblBuildings;
        private Button btnNextTurn;
        private PictureBox pbFarmer;
        private PictureBox pbBuilder;
        private PictureBox pbSoldier;
        private PictureBox pbFoe;
        private PictureBox pbFood;
        private PictureBox pbTurns;
        private PictureBox pbBuildings;
        private PictureBox pbBeds;
        private NumericUpDown nudFarmer;
        private NumericUpDown nudSoldier;
        private NumericUpDown nudBuilder;
        private NumericUpDown nudRemain;
        private Label label1;
        private Label label2;
        private NumericUpDown nudFoodConsumption;
        private NumericUpDown nudRecruitSoldier;
        private NumericUpDown nudRecruitBuilder;
        private NumericUpDown nudRecruitFarmer;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.lblFoesCount = new System.Windows.Forms.Label();
            this.btnNextTurn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbFoe = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.nudRecruitRiceFarmer = new System.Windows.Forms.NumericUpDown();
            this.nudRecruitWheatFarmer = new System.Windows.Forms.NumericUpDown();
            this.nudRiceFarmer = new System.Windows.Forms.NumericUpDown();
            this.nudWheatFarmer = new System.Windows.Forms.NumericUpDown();
            this.pbFarmer = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudFoodConsumption = new System.Windows.Forms.NumericUpDown();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.nudFarmer = new System.Windows.Forms.NumericUpDown();
            this.nudRecruitSoldier = new System.Windows.Forms.NumericUpDown();
            this.pbSoldier = new System.Windows.Forms.PictureBox();
            this.nudBuilder = new System.Windows.Forms.NumericUpDown();
            this.nudRecruitBuilder = new System.Windows.Forms.NumericUpDown();
            this.pbBuilder = new System.Windows.Forms.PictureBox();
            this.nudSoldier = new System.Windows.Forms.NumericUpDown();
            this.nudRecruitFarmer = new System.Windows.Forms.NumericUpDown();
            this.nudRemain = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbTurns = new System.Windows.Forms.PictureBox();
            this.pbFood = new System.Windows.Forms.PictureBox();
            this.pbBuildings = new System.Windows.Forms.PictureBox();
            this.pbBeds = new System.Windows.Forms.PictureBox();
            this.lblTurn = new System.Windows.Forms.Label();
            this.lblFood = new System.Windows.Forms.Label();
            this.lblBuildings = new System.Windows.Forms.Label();
            this.lblBeds = new System.Windows.Forms.Label();
            this.pbFoodThief = new System.Windows.Forms.PictureBox();
            this.lblFoodThief = new System.Windows.Forms.Label();
            this.lblBuildingDestroyer = new System.Windows.Forms.Label();
            this.pbBuildingDestroyer = new System.Windows.Forms.PictureBox();
            this.lblRelicBearer = new System.Windows.Forms.Label();
            this.pbRelicBearer = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFoe)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitRiceFarmer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitWheatFarmer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRiceFarmer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWheatFarmer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFarmer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFoodConsumption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFarmer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitSoldier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSoldier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBuilder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitBuilder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuilder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoldier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitFarmer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRemain)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTurns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuildings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBeds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFoodThief)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuildingDestroyer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRelicBearer)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFoesCount
            // 
            this.lblFoesCount.Location = new System.Drawing.Point(132, 95);
            this.lblFoesCount.Name = "lblFoesCount";
            this.lblFoesCount.Size = new System.Drawing.Size(100, 30);
            this.lblFoesCount.TabIndex = 9;
            this.lblFoesCount.Text = "0";
            this.lblFoesCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNextTurn
            // 
            this.btnNextTurn.Location = new System.Drawing.Point(532, 672);
            this.btnNextTurn.Name = "btnNextTurn";
            this.btnNextTurn.Size = new System.Drawing.Size(150, 50);
            this.btnNextTurn.TabIndex = 10;
            this.btnNextTurn.Text = "結束本回合";
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::Properties.Resources.Right_Info_Panel;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.lblRelicBearer);
            this.panel2.Controls.Add(this.pbRelicBearer);
            this.panel2.Controls.Add(this.lblBuildingDestroyer);
            this.panel2.Controls.Add(this.pbBuildingDestroyer);
            this.panel2.Controls.Add(this.lblFoodThief);
            this.panel2.Controls.Add(this.pbFoodThief);
            this.panel2.Controls.Add(this.pbFoe);
            this.panel2.Controls.Add(this.lblFoesCount);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(863, 137);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(256, 691);
            this.panel2.TabIndex = 27;
            // 
            // pbFoe
            // 
            this.pbFoe.Image = global::Properties.Resources.Villain;
            this.pbFoe.Location = new System.Drawing.Point(19, 63);
            this.pbFoe.Name = "pbFoe";
            this.pbFoe.Size = new System.Drawing.Size(100, 100);
            this.pbFoe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFoe.TabIndex = 8;
            this.pbFoe.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::Properties.Resources.Left_Info_Panel;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.nudRecruitRiceFarmer);
            this.panel3.Controls.Add(this.nudRecruitWheatFarmer);
            this.panel3.Controls.Add(this.nudRiceFarmer);
            this.panel3.Controls.Add(this.nudWheatFarmer);
            this.panel3.Controls.Add(this.pbFarmer);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.nudFoodConsumption);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Controls.Add(this.nudFarmer);
            this.panel3.Controls.Add(this.nudRecruitSoldier);
            this.panel3.Controls.Add(this.pbSoldier);
            this.panel3.Controls.Add(this.nudBuilder);
            this.panel3.Controls.Add(this.nudRecruitBuilder);
            this.panel3.Controls.Add(this.pbBuilder);
            this.panel3.Controls.Add(this.nudSoldier);
            this.panel3.Controls.Add(this.nudRecruitFarmer);
            this.panel3.Controls.Add(this.nudRemain);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 137);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(379, 691);
            this.panel3.TabIndex = 28;
            // 
            // nudRecruitRiceFarmer
            // 
            this.nudRecruitRiceFarmer.Location = new System.Drawing.Point(255, 319);
            this.nudRecruitRiceFarmer.Name = "nudRecruitRiceFarmer";
            this.nudRecruitRiceFarmer.Size = new System.Drawing.Size(120, 29);
            this.nudRecruitRiceFarmer.TabIndex = 29;
            // 
            // nudRecruitWheatFarmer
            // 
            this.nudRecruitWheatFarmer.Location = new System.Drawing.Point(255, 209);
            this.nudRecruitWheatFarmer.Name = "nudRecruitWheatFarmer";
            this.nudRecruitWheatFarmer.Size = new System.Drawing.Size(120, 29);
            this.nudRecruitWheatFarmer.TabIndex = 28;
            // 
            // nudRiceFarmer
            // 
            this.nudRiceFarmer.Location = new System.Drawing.Point(122, 319);
            this.nudRiceFarmer.Name = "nudRiceFarmer";
            this.nudRiceFarmer.Size = new System.Drawing.Size(120, 29);
            this.nudRiceFarmer.TabIndex = 27;
            // 
            // nudWheatFarmer
            // 
            this.nudWheatFarmer.Location = new System.Drawing.Point(122, 209);
            this.nudWheatFarmer.Name = "nudWheatFarmer";
            this.nudWheatFarmer.Size = new System.Drawing.Size(120, 29);
            this.nudWheatFarmer.TabIndex = 26;
            // 
            // pbFarmer
            // 
            this.pbFarmer.Image = global::Properties.Resources.farmer;
            this.pbFarmer.Location = new System.Drawing.Point(16, 63);
            this.pbFarmer.Name = "pbFarmer";
            this.pbFarmer.Size = new System.Drawing.Size(100, 100);
            this.pbFarmer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFarmer.TabIndex = 2;
            this.pbFarmer.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(16, 173);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(275, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 30);
            this.label2.TabIndex = 23;
            this.label2.Text = "招募區";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudFoodConsumption
            // 
            this.nudFoodConsumption.Location = new System.Drawing.Point(255, 617);
            this.nudFoodConsumption.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudFoodConsumption.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.nudFoodConsumption.Name = "nudFoodConsumption";
            this.nudFoodConsumption.ReadOnly = true;
            this.nudFoodConsumption.Size = new System.Drawing.Size(120, 29);
            this.nudFoodConsumption.TabIndex = 20;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Properties.Resources.Rice_Farmer1;
            this.pictureBox2.Location = new System.Drawing.Point(16, 283);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 100);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 25;
            this.pictureBox2.TabStop = false;
            // 
            // nudFarmer
            // 
            this.nudFarmer.Location = new System.Drawing.Point(122, 99);
            this.nudFarmer.Name = "nudFarmer";
            this.nudFarmer.Size = new System.Drawing.Size(120, 29);
            this.nudFarmer.TabIndex = 12;
            // 
            // nudRecruitSoldier
            // 
            this.nudRecruitSoldier.Location = new System.Drawing.Point(255, 539);
            this.nudRecruitSoldier.Name = "nudRecruitSoldier";
            this.nudRecruitSoldier.Size = new System.Drawing.Size(120, 29);
            this.nudRecruitSoldier.TabIndex = 21;
            // 
            // pbSoldier
            // 
            this.pbSoldier.Image = global::Properties.Resources.Soldier;
            this.pbSoldier.Location = new System.Drawing.Point(16, 503);
            this.pbSoldier.Name = "pbSoldier";
            this.pbSoldier.Size = new System.Drawing.Size(100, 100);
            this.pbSoldier.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSoldier.TabIndex = 6;
            this.pbSoldier.TabStop = false;
            // 
            // nudBuilder
            // 
            this.nudBuilder.Location = new System.Drawing.Point(122, 429);
            this.nudBuilder.Name = "nudBuilder";
            this.nudBuilder.Size = new System.Drawing.Size(120, 29);
            this.nudBuilder.TabIndex = 15;
            // 
            // nudRecruitBuilder
            // 
            this.nudRecruitBuilder.Location = new System.Drawing.Point(255, 429);
            this.nudRecruitBuilder.Name = "nudRecruitBuilder";
            this.nudRecruitBuilder.Size = new System.Drawing.Size(120, 29);
            this.nudRecruitBuilder.TabIndex = 22;
            // 
            // pbBuilder
            // 
            this.pbBuilder.Image = global::Properties.Resources.Architect;
            this.pbBuilder.Location = new System.Drawing.Point(16, 393);
            this.pbBuilder.Name = "pbBuilder";
            this.pbBuilder.Size = new System.Drawing.Size(100, 100);
            this.pbBuilder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBuilder.TabIndex = 4;
            this.pbBuilder.TabStop = false;
            // 
            // nudSoldier
            // 
            this.nudSoldier.Location = new System.Drawing.Point(122, 539);
            this.nudSoldier.Name = "nudSoldier";
            this.nudSoldier.Size = new System.Drawing.Size(120, 29);
            this.nudSoldier.TabIndex = 14;
            // 
            // nudRecruitFarmer
            // 
            this.nudRecruitFarmer.Location = new System.Drawing.Point(255, 99);
            this.nudRecruitFarmer.Name = "nudRecruitFarmer";
            this.nudRecruitFarmer.Size = new System.Drawing.Size(120, 29);
            this.nudRecruitFarmer.TabIndex = 19;
            // 
            // nudRemain
            // 
            this.nudRemain.Location = new System.Drawing.Point(122, 617);
            this.nudRemain.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudRemain.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.nudRemain.Name = "nudRemain";
            this.nudRemain.ReadOnly = true;
            this.nudRemain.Size = new System.Drawing.Size(120, 29);
            this.nudRemain.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(142, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 30);
            this.label1.TabIndex = 18;
            this.label1.Text = "目前角色";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.pbTurns);
            this.panel1.Controls.Add(this.pbFood);
            this.panel1.Controls.Add(this.pbBuildings);
            this.panel1.Controls.Add(this.pbBeds);
            this.panel1.Controls.Add(this.lblTurn);
            this.panel1.Controls.Add(this.lblFood);
            this.panel1.Controls.Add(this.lblBuildings);
            this.panel1.Controls.Add(this.lblBeds);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1119, 137);
            this.panel1.TabIndex = 26;
            // 
            // pbTurns
            // 
            this.pbTurns.ErrorImage = global::Properties.Resources.Turn;
            this.pbTurns.Image = global::Properties.Resources.Turn;
            this.pbTurns.Location = new System.Drawing.Point(53, 0);
            this.pbTurns.Name = "pbTurns";
            this.pbTurns.Size = new System.Drawing.Size(100, 100);
            this.pbTurns.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTurns.TabIndex = 8;
            this.pbTurns.TabStop = false;
            // 
            // pbFood
            // 
            this.pbFood.ErrorImage = global::Properties.Resources.Food;
            this.pbFood.Image = global::Properties.Resources.Food;
            this.pbFood.Location = new System.Drawing.Point(322, 0);
            this.pbFood.Name = "pbFood";
            this.pbFood.Size = new System.Drawing.Size(100, 100);
            this.pbFood.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFood.TabIndex = 8;
            this.pbFood.TabStop = false;
            // 
            // pbBuildings
            // 
            this.pbBuildings.ErrorImage = global::Properties.Resources.House;
            this.pbBuildings.Image = global::Properties.Resources.House;
            this.pbBuildings.Location = new System.Drawing.Point(591, 0);
            this.pbBuildings.Name = "pbBuildings";
            this.pbBuildings.Size = new System.Drawing.Size(100, 100);
            this.pbBuildings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBuildings.TabIndex = 17;
            this.pbBuildings.TabStop = false;
            // 
            // pbBeds
            // 
            this.pbBeds.ErrorImage = global::Properties.Resources.Bed;
            this.pbBeds.Image = global::Properties.Resources.Bed;
            this.pbBeds.Location = new System.Drawing.Point(860, 0);
            this.pbBeds.Name = "pbBeds";
            this.pbBeds.Size = new System.Drawing.Size(100, 100);
            this.pbBeds.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBeds.TabIndex = 16;
            this.pbBeds.TabStop = false;
            // 
            // lblTurn
            // 
            this.lblTurn.Location = new System.Drawing.Point(53, 103);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(100, 23);
            this.lblTurn.TabIndex = 0;
            this.lblTurn.Text = "0";
            this.lblTurn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFood
            // 
            this.lblFood.Location = new System.Drawing.Point(322, 103);
            this.lblFood.Name = "lblFood";
            this.lblFood.Size = new System.Drawing.Size(100, 23);
            this.lblFood.TabIndex = 1;
            this.lblFood.Text = "0";
            this.lblFood.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBuildings
            // 
            this.lblBuildings.Location = new System.Drawing.Point(591, 103);
            this.lblBuildings.Name = "lblBuildings";
            this.lblBuildings.Size = new System.Drawing.Size(100, 23);
            this.lblBuildings.TabIndex = 0;
            this.lblBuildings.Text = "0";
            this.lblBuildings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBeds
            // 
            this.lblBeds.Location = new System.Drawing.Point(860, 103);
            this.lblBeds.Name = "lblBeds";
            this.lblBeds.Size = new System.Drawing.Size(100, 23);
            this.lblBeds.TabIndex = 0;
            this.lblBeds.Text = "0";
            this.lblBeds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbFoodThief
            // 
            this.pbFoodThief.Image = global::Properties.Resources.Food_Thief_Villain;
            this.pbFoodThief.Location = new System.Drawing.Point(19, 173);
            this.pbFoodThief.Name = "pbFoodThief";
            this.pbFoodThief.Size = new System.Drawing.Size(100, 100);
            this.pbFoodThief.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFoodThief.TabIndex = 10;
            this.pbFoodThief.TabStop = false;
            // 
            // lblFoodThief
            // 
            this.lblFoodThief.Location = new System.Drawing.Point(132, 205);
            this.lblFoodThief.Name = "lblFoodThief";
            this.lblFoodThief.Size = new System.Drawing.Size(100, 30);
            this.lblFoodThief.TabIndex = 11;
            this.lblFoodThief.Text = "0";
            this.lblFoodThief.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBuildingDestroyer
            // 
            this.lblBuildingDestroyer.Location = new System.Drawing.Point(132, 315);
            this.lblBuildingDestroyer.Name = "lblBuildingDestroyer";
            this.lblBuildingDestroyer.Size = new System.Drawing.Size(100, 30);
            this.lblBuildingDestroyer.TabIndex = 13;
            this.lblBuildingDestroyer.Text = "0";
            this.lblBuildingDestroyer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbBuildingDestroyer
            // 
            this.pbBuildingDestroyer.Image = global::Properties.Resources.Building_Destroyer;
            this.pbBuildingDestroyer.Location = new System.Drawing.Point(19, 283);
            this.pbBuildingDestroyer.Name = "pbBuildingDestroyer";
            this.pbBuildingDestroyer.Size = new System.Drawing.Size(100, 100);
            this.pbBuildingDestroyer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBuildingDestroyer.TabIndex = 12;
            this.pbBuildingDestroyer.TabStop = false;
            // 
            // lblRelicBearer
            // 
            this.lblRelicBearer.Location = new System.Drawing.Point(132, 425);
            this.lblRelicBearer.Name = "lblRelicBearer";
            this.lblRelicBearer.Size = new System.Drawing.Size(100, 30);
            this.lblRelicBearer.TabIndex = 15;
            this.lblRelicBearer.Text = "0";
            this.lblRelicBearer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbRelicBearer
            // 
            this.pbRelicBearer.Image = global::Properties.Resources.Relic_Bearer_Final;
            this.pbRelicBearer.Location = new System.Drawing.Point(19, 393);
            this.pbRelicBearer.Name = "pbRelicBearer";
            this.pbRelicBearer.Size = new System.Drawing.Size(100, 100);
            this.pbRelicBearer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbRelicBearer.TabIndex = 14;
            this.pbRelicBearer.TabStop = false;
            // 
            // GameForm
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1119, 828);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNextTurn);
            this.Name = "GameForm";
            this.Text = "回合制遊戲 UI";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbFoe)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitRiceFarmer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitWheatFarmer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRiceFarmer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWheatFarmer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFarmer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFoodConsumption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFarmer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitSoldier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSoldier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBuilder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitBuilder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuilder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoldier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecruitFarmer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRemain)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTurns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuildings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBeds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFoodThief)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBuildingDestroyer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRelicBearer)).EndInit();
            this.ResumeLayout(false);

        }

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private NumericUpDown nudRiceFarmer;
        private NumericUpDown nudWheatFarmer;
        private NumericUpDown nudRecruitRiceFarmer;
        private NumericUpDown nudRecruitWheatFarmer;
        private Label lblFoodThief;
        private PictureBox pbFoodThief;
        private Label lblRelicBearer;
        private PictureBox pbRelicBearer;
        private Label lblBuildingDestroyer;
        private PictureBox pbBuildingDestroyer;
    }
}