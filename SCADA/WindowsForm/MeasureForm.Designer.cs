﻿namespace SCADA
{
    partial class MeasureForm
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxToolNo = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tBMax = new System.Windows.Forms.TextBox();
            this.tBMin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tBRefer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCompent = new System.Windows.Forms.Button();
            this.tBCompen = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tBDepth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tBMeasureState = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tBFile2 = new System.Windows.Forms.TextBox();
            this.tBFile1 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tBAgentState = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartAgent = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxToolNo);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.tBMax);
            this.groupBox2.Controls.Add(this.tBMin);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tBRefer);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnCompent);
            this.groupBox2.Controls.Add(this.tBCompen);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tBDepth);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tBMeasureState);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(13, 14);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(757, 467);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测量";
            // 
            // comboBoxToolNo
            // 
            this.comboBoxToolNo.FormattingEnabled = true;
            this.comboBoxToolNo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.comboBoxToolNo.Location = new System.Drawing.Point(130, 326);
            this.comboBoxToolNo.Name = "comboBoxToolNo";
            this.comboBoxToolNo.Size = new System.Drawing.Size(60, 29);
            this.comboBoxToolNo.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 326);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 21);
            this.label13.TabIndex = 31;
            this.label13.Text = "补偿刀号:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(530, 246);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(22, 21);
            this.label12.TabIndex = 30;
            this.label12.Text = "~";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(530, 329);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 21);
            this.label10.TabIndex = 29;
            this.label10.Text = "mm";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(621, 246);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 21);
            this.label9.TabIndex = 28;
            this.label9.Text = "mm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(284, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 21);
            this.label8.TabIndex = 27;
            this.label8.Text = "mm";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(306, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 21);
            this.label7.TabIndex = 4;
            this.label7.Text = "mm";
            // 
            // tBMax
            // 
            this.tBMax.Enabled = false;
            this.tBMax.Location = new System.Drawing.Point(559, 244);
            this.tBMax.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBMax.Name = "tBMax";
            this.tBMax.ReadOnly = true;
            this.tBMax.Size = new System.Drawing.Size(55, 29);
            this.tBMax.TabIndex = 26;
            this.tBMax.Text = "+0.0";
            // 
            // tBMin
            // 
            this.tBMin.Enabled = false;
            this.tBMin.Location = new System.Drawing.Point(477, 243);
            this.tBMin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBMin.Name = "tBMin";
            this.tBMin.ReadOnly = true;
            this.tBMin.Size = new System.Drawing.Size(46, 29);
            this.tBMin.TabIndex = 26;
            this.tBMin.Text = "-0.0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(335, 247);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 21);
            this.label6.TabIndex = 25;
            this.label6.Text = "公差范围:";
            // 
            // tBRefer
            // 
            this.tBRefer.Enabled = false;
            this.tBRefer.Location = new System.Drawing.Point(157, 239);
            this.tBRefer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBRefer.Name = "tBRefer";
            this.tBRefer.ReadOnly = true;
            this.tBRefer.Size = new System.Drawing.Size(120, 29);
            this.tBRefer.TabIndex = 24;
            this.tBRefer.Text = "1.000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 247);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 21);
            this.label4.TabIndex = 23;
            this.label4.Text = "参考值:";
            // 
            // btnCompent
            // 
            this.btnCompent.Location = new System.Drawing.Point(587, 320);
            this.btnCompent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCompent.Name = "btnCompent";
            this.btnCompent.Size = new System.Drawing.Size(88, 35);
            this.btnCompent.TabIndex = 19;
            this.btnCompent.Text = "设置补偿";
            this.btnCompent.UseVisualStyleBackColor = true;
            this.btnCompent.Click += new System.EventHandler(this.btnSetCompen_Click);
            // 
            // tBCompen
            // 
            this.tBCompen.Location = new System.Drawing.Point(403, 323);
            this.tBCompen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBCompen.Name = "tBCompen";
            this.tBCompen.Size = new System.Drawing.Size(120, 29);
            this.tBCompen.TabIndex = 18;
            this.tBCompen.Text = "0.000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(234, 327);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 21);
            this.label5.TabIndex = 17;
            this.label5.Text = "补偿值:";
            // 
            // tBDepth
            // 
            this.tBDepth.Enabled = false;
            this.tBDepth.Location = new System.Drawing.Point(176, 162);
            this.tBDepth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBDepth.Name = "tBDepth";
            this.tBDepth.ReadOnly = true;
            this.tBDepth.Size = new System.Drawing.Size(120, 29);
            this.tBDepth.TabIndex = 16;
            this.tBDepth.Text = "0.000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 170);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 21);
            this.label3.TabIndex = 15;
            this.label3.Text = "测量深度:";
            // 
            // tBMeasureState
            // 
            this.tBMeasureState.Enabled = false;
            this.tBMeasureState.Location = new System.Drawing.Point(132, 65);
            this.tBMeasureState.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBMeasureState.Name = "tBMeasureState";
            this.tBMeasureState.ReadOnly = true;
            this.tBMeasureState.Size = new System.Drawing.Size(145, 29);
            this.tBMeasureState.TabIndex = 4;
            this.tBMeasureState.Text = "连接正常";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 70);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "测量状态:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tBFile2);
            this.groupBox1.Controls.Add(this.tBFile1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tBAgentState);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnStartAgent);
            this.groupBox1.Location = new System.Drawing.Point(13, 491);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(639, 221);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工艺优化";
            this.groupBox1.Visible = false;
            // 
            // tBFile2
            // 
            this.tBFile2.Enabled = false;
            this.tBFile2.Location = new System.Drawing.Point(149, 172);
            this.tBFile2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBFile2.Name = "tBFile2";
            this.tBFile2.ReadOnly = true;
            this.tBFile2.Size = new System.Drawing.Size(278, 29);
            this.tBFile2.TabIndex = 24;
            this.tBFile2.Visible = false;
            // 
            // tBFile1
            // 
            this.tBFile1.Enabled = false;
            this.tBFile1.Location = new System.Drawing.Point(149, 122);
            this.tBFile1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBFile1.Name = "tBFile1";
            this.tBFile1.ReadOnly = true;
            this.tBFile1.Size = new System.Drawing.Size(278, 29);
            this.tBFile1.TabIndex = 23;
            this.tBFile1.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(29, 122);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 21);
            this.label11.TabIndex = 22;
            this.label11.Text = "数据文件:";
            this.label11.Visible = false;
            // 
            // tBAgentState
            // 
            this.tBAgentState.Enabled = false;
            this.tBAgentState.Location = new System.Drawing.Point(149, 63);
            this.tBAgentState.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBAgentState.Name = "tBAgentState";
            this.tBAgentState.ReadOnly = true;
            this.tBAgentState.Size = new System.Drawing.Size(167, 29);
            this.tBAgentState.TabIndex = 21;
            this.tBAgentState.Text = "未开启";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 21);
            this.label1.TabIndex = 20;
            this.label1.Text = "数据采集状态:";
            // 
            // btnStartAgent
            // 
            this.btnStartAgent.Location = new System.Drawing.Point(339, 55);
            this.btnStartAgent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStartAgent.Name = "btnStartAgent";
            this.btnStartAgent.Size = new System.Drawing.Size(88, 40);
            this.btnStartAgent.TabIndex = 0;
            this.btnStartAgent.Text = "开启";
            this.btnStartAgent.UseVisualStyleBackColor = true;
            this.btnStartAgent.Click += new System.EventHandler(this.btnStartAgent_Click);
            // 
            // MeasureForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1204, 742);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MeasureForm";
            this.Text = "MeasureForm";
            this.Load += new System.EventHandler(this.MeasureForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCompent;
        private System.Windows.Forms.TextBox tBCompen;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tBDepth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBMeasureState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStartAgent;
        private System.Windows.Forms.TextBox tBAgentState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBRefer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tBMin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tBFile2;
        private System.Windows.Forms.TextBox tBFile1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tBMax;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxToolNo;
    }
}