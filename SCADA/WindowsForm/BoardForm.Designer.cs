﻿namespace SCADA
{
    partial class BoardForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxstats = new System.Windows.Forms.PictureBox();
            this.pictureBoxbin = new System.Windows.Forms.PictureBox();
            this.pictureBoxmain = new System.Windows.Forms.PictureBox();
            this.pictureBoxuser = new System.Windows.Forms.PictureBox();
            this.pictureBoxtool = new System.Windows.Forms.PictureBox();
            this.pictureBoxmeter = new System.Windows.Forms.PictureBox();
            this.pictureBoxrobot = new System.Windows.Forms.PictureBox();
            this.pictureBoxcnc = new System.Windows.Forms.PictureBox();
            this.pictureBoxlathe = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxstats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxbin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxmain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxuser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxtool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxmeter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxrobot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcnc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxlathe)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxlathe, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxcnc, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxrobot, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxmeter, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxtool, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxuser, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxstats, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxbin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxmain, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1084, 662);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBoxstats
            // 
            this.pictureBoxstats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxstats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxstats.Image = global::SCADA.Properties.Resources.统计;
            this.pictureBoxstats.Location = new System.Drawing.Point(725, 3);
            this.pictureBoxstats.Name = "pictureBoxstats";
            this.pictureBoxstats.Size = new System.Drawing.Size(356, 214);
            this.pictureBoxstats.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxstats.TabIndex = 4;
            this.pictureBoxstats.TabStop = false;
            this.pictureBoxstats.Click += new System.EventHandler(this.pictureBoxstats_Click);
            // 
            // pictureBoxbin
            // 
            this.pictureBoxbin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxbin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxbin.Image = global::SCADA.Properties.Resources.料仓;
            this.pictureBoxbin.Location = new System.Drawing.Point(364, 3);
            this.pictureBoxbin.Name = "pictureBoxbin";
            this.pictureBoxbin.Size = new System.Drawing.Size(355, 214);
            this.pictureBoxbin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxbin.TabIndex = 3;
            this.pictureBoxbin.TabStop = false;
            this.pictureBoxbin.Click += new System.EventHandler(this.pictureBoxbin_Click);
            // 
            // pictureBoxmain
            // 
            this.pictureBoxmain.BackColor = System.Drawing.Color.White;
            this.pictureBoxmain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxmain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxmain.Image = global::SCADA.Properties.Resources.首页;
            this.pictureBoxmain.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxmain.Name = "pictureBoxmain";
            this.pictureBoxmain.Size = new System.Drawing.Size(355, 214);
            this.pictureBoxmain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxmain.TabIndex = 2;
            this.pictureBoxmain.TabStop = false;
            this.pictureBoxmain.Click += new System.EventHandler(this.pictureBoxmain_Click);
            // 
            // pictureBoxuser
            // 
            this.pictureBoxuser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxuser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxuser.Image = global::SCADA.Properties.Resources.用户;
            this.pictureBoxuser.Location = new System.Drawing.Point(725, 443);
            this.pictureBoxuser.Name = "pictureBoxuser";
            this.pictureBoxuser.Size = new System.Drawing.Size(356, 216);
            this.pictureBoxuser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxuser.TabIndex = 12;
            this.pictureBoxuser.TabStop = false;
            this.pictureBoxuser.Click += new System.EventHandler(this.pictureBoxuser_Click);
            // 
            // pictureBoxtool
            // 
            this.pictureBoxtool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxtool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxtool.Image = global::SCADA.Properties.Resources.刀具;
            this.pictureBoxtool.Location = new System.Drawing.Point(364, 443);
            this.pictureBoxtool.Name = "pictureBoxtool";
            this.pictureBoxtool.Size = new System.Drawing.Size(355, 216);
            this.pictureBoxtool.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxtool.TabIndex = 13;
            this.pictureBoxtool.TabStop = false;
            this.pictureBoxtool.Click += new System.EventHandler(this.pictureBoxtool_Click);
            // 
            // pictureBoxmeter
            // 
            this.pictureBoxmeter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxmeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxmeter.Image = global::SCADA.Properties.Resources.测量;
            this.pictureBoxmeter.Location = new System.Drawing.Point(3, 443);
            this.pictureBoxmeter.Name = "pictureBoxmeter";
            this.pictureBoxmeter.Size = new System.Drawing.Size(355, 216);
            this.pictureBoxmeter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxmeter.TabIndex = 14;
            this.pictureBoxmeter.TabStop = false;
            this.pictureBoxmeter.Click += new System.EventHandler(this.pictureBoxmeter_Click);
            // 
            // pictureBoxrobot
            // 
            this.pictureBoxrobot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxrobot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxrobot.Image = global::SCADA.Properties.Resources.机器人;
            this.pictureBoxrobot.Location = new System.Drawing.Point(725, 223);
            this.pictureBoxrobot.Name = "pictureBoxrobot";
            this.pictureBoxrobot.Size = new System.Drawing.Size(356, 214);
            this.pictureBoxrobot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxrobot.TabIndex = 15;
            this.pictureBoxrobot.TabStop = false;
            this.pictureBoxrobot.Click += new System.EventHandler(this.pictureBoxrobot_Click);
            // 
            // pictureBoxcnc
            // 
            this.pictureBoxcnc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxcnc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxcnc.Image = global::SCADA.Properties.Resources.加工中心;
            this.pictureBoxcnc.Location = new System.Drawing.Point(364, 223);
            this.pictureBoxcnc.Name = "pictureBoxcnc";
            this.pictureBoxcnc.Size = new System.Drawing.Size(355, 214);
            this.pictureBoxcnc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxcnc.TabIndex = 16;
            this.pictureBoxcnc.TabStop = false;
            this.pictureBoxcnc.Click += new System.EventHandler(this.pictureBoxcnc_Click);
            // 
            // pictureBoxlathe
            // 
            this.pictureBoxlathe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxlathe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxlathe.Image = global::SCADA.Properties.Resources.车床;
            this.pictureBoxlathe.Location = new System.Drawing.Point(3, 223);
            this.pictureBoxlathe.Name = "pictureBoxlathe";
            this.pictureBoxlathe.Size = new System.Drawing.Size(355, 214);
            this.pictureBoxlathe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxlathe.TabIndex = 17;
            this.pictureBoxlathe.TabStop = false;
            this.pictureBoxlathe.Click += new System.EventHandler(this.pictureBoxlathe_Click);
            // 
            // BoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 662);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BoardForm";
            this.Text = "BoardForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxstats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxbin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxmain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxuser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxtool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxmeter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxrobot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcnc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxlathe)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBoxstats;
        private System.Windows.Forms.PictureBox pictureBoxbin;
        private System.Windows.Forms.PictureBox pictureBoxmain;
        private System.Windows.Forms.PictureBox pictureBoxlathe;
        private System.Windows.Forms.PictureBox pictureBoxcnc;
        private System.Windows.Forms.PictureBox pictureBoxrobot;
        private System.Windows.Forms.PictureBox pictureBoxmeter;
        private System.Windows.Forms.PictureBox pictureBoxtool;
        private System.Windows.Forms.PictureBox pictureBoxuser;
    }
}