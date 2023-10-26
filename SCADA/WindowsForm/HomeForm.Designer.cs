﻿namespace SCADA
{
    partial class HomeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeForm));
            this.HomeTimer = new System.Windows.Forms.Timer(this.components);
            this.labelUnconnettext = new System.Windows.Forms.Label();
            this.labelKongXianText = new System.Windows.Forms.Label();
            this.labelRuningText = new System.Windows.Forms.Label();
            this.labelAlarText = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxRuningColor = new System.Windows.Forms.PictureBox();
            this.pictureBoxArrColor = new System.Windows.Forms.PictureBox();
            this.pictureBoxKongXianColor = new System.Windows.Forms.PictureBox();
            this.pictureBoxUnConnetColor = new System.Windows.Forms.PictureBox();
            this.btnGC = new System.Windows.Forms.Button();
            this.btnCNC = new System.Windows.Forms.Button();
            this.btnRobot = new System.Windows.Forms.Button();
            this.btnRFID = new System.Windows.Forms.Button();
            this.btnRack = new System.Windows.Forms.Button();
            this.btnPC = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRuningColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxKongXianColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUnConnetColor)).BeginInit();
            this.SuspendLayout();
            // 
            // HomeTimer
            // 
            this.HomeTimer.Tick += new System.EventHandler(this.HomeTimer_Tick);
            // 
            // labelUnconnettext
            // 
            this.labelUnconnettext.AutoSize = true;
            this.labelUnconnettext.Location = new System.Drawing.Point(17, 22);
            this.labelUnconnettext.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUnconnettext.Name = "labelUnconnettext";
            this.labelUnconnettext.Size = new System.Drawing.Size(58, 21);
            this.labelUnconnettext.TabIndex = 155;
            this.labelUnconnettext.Text = "离线：";
            this.labelUnconnettext.Visible = false;
            // 
            // labelKongXianText
            // 
            this.labelKongXianText.AutoSize = true;
            this.labelKongXianText.Location = new System.Drawing.Point(148, 22);
            this.labelKongXianText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKongXianText.Name = "labelKongXianText";
            this.labelKongXianText.Size = new System.Drawing.Size(58, 21);
            this.labelKongXianText.TabIndex = 155;
            this.labelKongXianText.Text = "空闲：";
            this.labelKongXianText.Visible = false;
            // 
            // labelRuningText
            // 
            this.labelRuningText.AutoSize = true;
            this.labelRuningText.Location = new System.Drawing.Point(275, 22);
            this.labelRuningText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRuningText.Name = "labelRuningText";
            this.labelRuningText.Size = new System.Drawing.Size(58, 21);
            this.labelRuningText.TabIndex = 155;
            this.labelRuningText.Text = "运行：";
            this.labelRuningText.Visible = false;
            // 
            // labelAlarText
            // 
            this.labelAlarText.AutoSize = true;
            this.labelAlarText.Location = new System.Drawing.Point(409, 22);
            this.labelAlarText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAlarText.Name = "labelAlarText";
            this.labelAlarText.Size = new System.Drawing.Size(58, 21);
            this.labelAlarText.TabIndex = 155;
            this.labelAlarText.Text = "报警：";
            this.labelAlarText.Visible = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.BackgroundImage = global::SCADA.Properties.Resources.top_hcnc;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(7, 0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(1398, 836);
            this.pictureBox4.TabIndex = 166;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Location = new System.Drawing.Point(405, 380);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(113, 92);
            this.pictureBox8.TabIndex = 164;
            this.pictureBox8.TabStop = false;
            this.pictureBox8.Visible = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Location = new System.Drawing.Point(597, 560);
            this.pictureBox7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(71, 62);
            this.pictureBox7.TabIndex = 163;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.Visible = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Location = new System.Drawing.Point(597, 248);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(71, 72);
            this.pictureBox6.TabIndex = 162;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Visible = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Location = new System.Drawing.Point(735, 410);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(111, 62);
            this.pictureBox5.TabIndex = 161;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Visible = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(151, 102);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(13, 17);
            this.pictureBox3.TabIndex = 159;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(107, 102);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(13, 17);
            this.pictureBox2.TabIndex = 158;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(59, 102);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(13, 17);
            this.pictureBox1.TabIndex = 157;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // pictureBoxRuningColor
            // 
            this.pictureBoxRuningColor.Location = new System.Drawing.Point(339, 15);
            this.pictureBoxRuningColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxRuningColor.Name = "pictureBoxRuningColor";
            this.pictureBoxRuningColor.Size = new System.Drawing.Size(39, 35);
            this.pictureBoxRuningColor.TabIndex = 156;
            this.pictureBoxRuningColor.TabStop = false;
            this.pictureBoxRuningColor.Visible = false;
            // 
            // pictureBoxArrColor
            // 
            this.pictureBoxArrColor.Location = new System.Drawing.Point(480, 15);
            this.pictureBoxArrColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxArrColor.Name = "pictureBoxArrColor";
            this.pictureBoxArrColor.Size = new System.Drawing.Size(39, 35);
            this.pictureBoxArrColor.TabIndex = 156;
            this.pictureBoxArrColor.TabStop = false;
            this.pictureBoxArrColor.Visible = false;
            // 
            // pictureBoxKongXianColor
            // 
            this.pictureBoxKongXianColor.Location = new System.Drawing.Point(215, 15);
            this.pictureBoxKongXianColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxKongXianColor.Name = "pictureBoxKongXianColor";
            this.pictureBoxKongXianColor.Size = new System.Drawing.Size(39, 35);
            this.pictureBoxKongXianColor.TabIndex = 156;
            this.pictureBoxKongXianColor.TabStop = false;
            this.pictureBoxKongXianColor.Visible = false;
            // 
            // pictureBoxUnConnetColor
            // 
            this.pictureBoxUnConnetColor.Location = new System.Drawing.Point(81, 15);
            this.pictureBoxUnConnetColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxUnConnetColor.Name = "pictureBoxUnConnetColor";
            this.pictureBoxUnConnetColor.Size = new System.Drawing.Size(39, 35);
            this.pictureBoxUnConnetColor.TabIndex = 156;
            this.pictureBoxUnConnetColor.TabStop = false;
            this.pictureBoxUnConnetColor.Visible = false;
            // 
            // btnGC
            // 
            this.btnGC.BackColor = System.Drawing.Color.Transparent;
            this.btnGC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGC.FlatAppearance.BorderSize = 0;
            this.btnGC.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGC.ForeColor = System.Drawing.Color.Transparent;
            this.btnGC.Location = new System.Drawing.Point(509, 208);
            this.btnGC.Name = "btnGC";
            this.btnGC.Size = new System.Drawing.Size(66, 62);
            this.btnGC.TabIndex = 172;
            this.btnGC.UseVisualStyleBackColor = false;
            this.btnGC.Click += new System.EventHandler(this.btnGC_Click);
            this.btnGC.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnGC.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_MouseMove);
            // 
            // btnCNC
            // 
            this.btnCNC.BackColor = System.Drawing.Color.Transparent;
            this.btnCNC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCNC.FlatAppearance.BorderSize = 0;
            this.btnCNC.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCNC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCNC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCNC.ForeColor = System.Drawing.Color.Transparent;
            this.btnCNC.Location = new System.Drawing.Point(438, 39);
            this.btnCNC.Name = "btnCNC";
            this.btnCNC.Size = new System.Drawing.Size(394, 378);
            this.btnCNC.TabIndex = 170;
            this.btnCNC.UseVisualStyleBackColor = false;
            this.btnCNC.Visible = false;
            this.btnCNC.Click += new System.EventHandler(this.btnCNC_Click);
            this.btnCNC.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnCNC.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_MouseMove);
            // 
            // btnRobot
            // 
            this.btnRobot.BackColor = System.Drawing.Color.Transparent;
            this.btnRobot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRobot.FlatAppearance.BorderSize = 0;
            this.btnRobot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRobot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRobot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRobot.ForeColor = System.Drawing.Color.Transparent;
            this.btnRobot.Location = new System.Drawing.Point(707, 240);
            this.btnRobot.Name = "btnRobot";
            this.btnRobot.Size = new System.Drawing.Size(157, 231);
            this.btnRobot.TabIndex = 173;
            this.btnRobot.UseVisualStyleBackColor = false;
            this.btnRobot.Visible = false;
            this.btnRobot.Click += new System.EventHandler(this.btnRob_Click);
            this.btnRobot.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnRobot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_MouseMove);
            // 
            // btnRFID
            // 
            this.btnRFID.BackColor = System.Drawing.Color.Transparent;
            this.btnRFID.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRFID.FlatAppearance.BorderSize = 0;
            this.btnRFID.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRFID.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRFID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRFID.ForeColor = System.Drawing.Color.Transparent;
            this.btnRFID.Location = new System.Drawing.Point(944, 287);
            this.btnRFID.Name = "btnRFID";
            this.btnRFID.Size = new System.Drawing.Size(47, 33);
            this.btnRFID.TabIndex = 175;
            this.btnRFID.UseVisualStyleBackColor = false;
            this.btnRFID.Click += new System.EventHandler(this.btnRFID_Click);
            this.btnRFID.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnRFID.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_MouseMove);
            // 
            // btnRack
            // 
            this.btnRack.BackColor = System.Drawing.Color.Transparent;
            this.btnRack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRack.FlatAppearance.BorderSize = 0;
            this.btnRack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRack.ForeColor = System.Drawing.Color.Transparent;
            this.btnRack.Location = new System.Drawing.Point(840, 188);
            this.btnRack.Margin = new System.Windows.Forms.Padding(0);
            this.btnRack.Name = "btnRack";
            this.btnRack.Size = new System.Drawing.Size(175, 259);
            this.btnRack.TabIndex = 171;
            this.btnRack.UseVisualStyleBackColor = false;
            this.btnRack.Visible = false;
            this.btnRack.Click += new System.EventHandler(this.btnRack_Click);
            this.btnRack.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnRack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_MouseMove);
            // 
            // btnPC
            // 
            this.btnPC.BackColor = System.Drawing.Color.Transparent;
            this.btnPC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPC.FlatAppearance.BorderSize = 0;
            this.btnPC.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPC.ForeColor = System.Drawing.Color.Transparent;
            this.btnPC.Location = new System.Drawing.Point(721, 474);
            this.btnPC.Name = "btnPC";
            this.btnPC.Size = new System.Drawing.Size(173, 123);
            this.btnPC.TabIndex = 174;
            this.btnPC.UseVisualStyleBackColor = false;
            this.btnPC.Visible = false;
            this.btnPC.Click += new System.EventHandler(this.btnPC_Click);
            this.btnPC.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btnPC.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_MouseMove);
            // 
            // HomeForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1366, 746);
            this.Controls.Add(this.btnRobot);
            this.Controls.Add(this.btnPC);
            this.Controls.Add(this.btnGC);
            this.Controls.Add(this.btnRFID);
            this.Controls.Add(this.btnRack);
            this.Controls.Add(this.btnCNC);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox8);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBoxRuningColor);
            this.Controls.Add(this.pictureBoxArrColor);
            this.Controls.Add(this.pictureBoxKongXianColor);
            this.Controls.Add(this.pictureBoxUnConnetColor);
            this.Controls.Add(this.labelAlarText);
            this.Controls.Add(this.labelRuningText);
            this.Controls.Add(this.labelKongXianText);
            this.Controls.Add(this.labelUnconnettext);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "HomeForm";
            this.Text = "HomeForm";
            this.Load += new System.EventHandler(this.HomeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRuningColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxKongXianColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUnConnetColor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer HomeTimer;
        private System.Windows.Forms.Label labelUnconnettext;
        private System.Windows.Forms.Label labelKongXianText;
        private System.Windows.Forms.Label labelRuningText;
        private System.Windows.Forms.Label labelAlarText;
        private System.Windows.Forms.PictureBox pictureBoxUnConnetColor;
        private System.Windows.Forms.PictureBox pictureBoxKongXianColor;
        private System.Windows.Forms.PictureBox pictureBoxRuningColor;
        private System.Windows.Forms.PictureBox pictureBoxArrColor;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button btnGC;
        private System.Windows.Forms.Button btnCNC;
        private System.Windows.Forms.Button btnRobot;
        private System.Windows.Forms.Button btnRFID;
        private System.Windows.Forms.Button btnRack;
        private System.Windows.Forms.Button btnPC;
    }
}