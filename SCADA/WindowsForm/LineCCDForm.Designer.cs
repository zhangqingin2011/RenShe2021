﻿namespace SCADA
{
    partial class LineCCDForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageCCDPara = new System.Windows.Forms.TabPage();
            this.btnCCDConnect = new System.Windows.Forms.Button();
            this.textBoxCCDPort = new System.Windows.Forms.TextBox();
            this.textBoxCCDIP = new System.Windows.Forms.TextBox();
            this.labPORT = new System.Windows.Forms.Label();
            this.labCCDIP = new System.Windows.Forms.Label();
            this.tabPageCCDPhoto = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCCDSend = new System.Windows.Forms.Button();
            this.textBoxCCDReceive = new System.Windows.Forms.TextBox();
            this.textBoxCCDSend = new System.Windows.Forms.TextBox();
            this.dataGridViewCCD = new System.Windows.Forms.DataGridView();
            this.listViewCCDHisAlarm = new System.Windows.Forms.ListView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageCCDPara.SuspendLayout();
            this.tabPageCCDPhoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCCD)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageCCDPara);
            this.tabControl1.Controls.Add(this.tabPageCCDPhoto);
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(443, 343);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageCCDPara
            // 
            this.tabPageCCDPara.BackColor = System.Drawing.Color.White;
            this.tabPageCCDPara.Controls.Add(this.btnCCDConnect);
            this.tabPageCCDPara.Controls.Add(this.textBoxCCDPort);
            this.tabPageCCDPara.Controls.Add(this.textBoxCCDIP);
            this.tabPageCCDPara.Controls.Add(this.labPORT);
            this.tabPageCCDPara.Controls.Add(this.labCCDIP);
            this.tabPageCCDPara.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPageCCDPara.Location = new System.Drawing.Point(4, 29);
            this.tabPageCCDPara.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCCDPara.Name = "tabPageCCDPara";
            this.tabPageCCDPara.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCCDPara.Size = new System.Drawing.Size(435, 310);
            this.tabPageCCDPara.TabIndex = 0;
            this.tabPageCCDPara.Text = "CCD通讯参数";
            // 
            // btnCCDConnect
            // 
            this.btnCCDConnect.Location = new System.Drawing.Point(141, 208);
            this.btnCCDConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCCDConnect.Name = "btnCCDConnect";
            this.btnCCDConnect.Size = new System.Drawing.Size(135, 52);
            this.btnCCDConnect.TabIndex = 67;
            this.btnCCDConnect.Text = "连接";
            this.btnCCDConnect.UseVisualStyleBackColor = true;
            this.btnCCDConnect.Click += new System.EventHandler(this.btnCCDConnect_Click);
            // 
            // textBoxCCDPort
            // 
            this.textBoxCCDPort.Location = new System.Drawing.Point(167, 132);
            this.textBoxCCDPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCCDPort.Name = "textBoxCCDPort";
            this.textBoxCCDPort.Size = new System.Drawing.Size(191, 26);
            this.textBoxCCDPort.TabIndex = 66;
            this.textBoxCCDPort.Text = "1001";
            // 
            // textBoxCCDIP
            // 
            this.textBoxCCDIP.Location = new System.Drawing.Point(167, 57);
            this.textBoxCCDIP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCCDIP.Name = "textBoxCCDIP";
            this.textBoxCCDIP.Size = new System.Drawing.Size(191, 26);
            this.textBoxCCDIP.TabIndex = 65;
            this.textBoxCCDIP.Text = "161.254.1.120";
            // 
            // labPORT
            // 
            this.labPORT.AutoSize = true;
            this.labPORT.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labPORT.Location = new System.Drawing.Point(61, 132);
            this.labPORT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labPORT.Name = "labPORT";
            this.labPORT.Size = new System.Drawing.Size(51, 20);
            this.labPORT.TabIndex = 64;
            this.labPORT.Text = "端口号";
            // 
            // labCCDIP
            // 
            this.labCCDIP.AutoSize = true;
            this.labCCDIP.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labCCDIP.Location = new System.Drawing.Point(61, 57);
            this.labCCDIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labCCDIP.Name = "labCCDIP";
            this.labCCDIP.Size = new System.Drawing.Size(50, 20);
            this.labCCDIP.TabIndex = 63;
            this.labCCDIP.Text = "IP地址";
            // 
            // tabPageCCDPhoto
            // 
            this.tabPageCCDPhoto.BackColor = System.Drawing.Color.White;
            this.tabPageCCDPhoto.Controls.Add(this.label2);
            this.tabPageCCDPhoto.Controls.Add(this.label1);
            this.tabPageCCDPhoto.Controls.Add(this.btnCCDSend);
            this.tabPageCCDPhoto.Controls.Add(this.textBoxCCDReceive);
            this.tabPageCCDPhoto.Controls.Add(this.textBoxCCDSend);
            this.tabPageCCDPhoto.Location = new System.Drawing.Point(4, 29);
            this.tabPageCCDPhoto.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCCDPhoto.Name = "tabPageCCDPhoto";
            this.tabPageCCDPhoto.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCCDPhoto.Size = new System.Drawing.Size(435, 310);
            this.tabPageCCDPhoto.TabIndex = 1;
            this.tabPageCCDPhoto.Text = "拍照指令";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 183);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "接收数据";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "发送指令";
            // 
            // btnCCDSend
            // 
            this.btnCCDSend.Location = new System.Drawing.Point(127, 240);
            this.btnCCDSend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCCDSend.Name = "btnCCDSend";
            this.btnCCDSend.Size = new System.Drawing.Size(108, 53);
            this.btnCCDSend.TabIndex = 8;
            this.btnCCDSend.Text = "发送";
            this.btnCCDSend.UseVisualStyleBackColor = true;
            this.btnCCDSend.Click += new System.EventHandler(this.btnCCDSend_Click);
            // 
            // textBoxCCDReceive
            // 
            this.textBoxCCDReceive.Location = new System.Drawing.Point(159, 173);
            this.textBoxCCDReceive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCCDReceive.Name = "textBoxCCDReceive";
            this.textBoxCCDReceive.Size = new System.Drawing.Size(137, 26);
            this.textBoxCCDReceive.TabIndex = 7;
            // 
            // textBoxCCDSend
            // 
            this.textBoxCCDSend.Location = new System.Drawing.Point(159, 62);
            this.textBoxCCDSend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCCDSend.Name = "textBoxCCDSend";
            this.textBoxCCDSend.Size = new System.Drawing.Size(137, 26);
            this.textBoxCCDSend.TabIndex = 6;
            this.textBoxCCDSend.Text = "CC,0";
            // 
            // dataGridViewCCD
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewCCD.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCCD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCCD.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewCCD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCCD.Location = new System.Drawing.Point(443, 2);
            this.dataGridViewCCD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewCCD.Name = "dataGridViewCCD";
            this.dataGridViewCCD.RowHeadersWidth = 105;
            this.dataGridViewCCD.RowTemplate.Height = 30;
            this.dataGridViewCCD.Size = new System.Drawing.Size(487, 342);
            this.dataGridViewCCD.TabIndex = 1;
            // 
            // listViewCCDHisAlarm
            // 
            this.listViewCCDHisAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCCDHisAlarm.BackColor = System.Drawing.Color.White;
            this.listViewCCDHisAlarm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewCCDHisAlarm.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewCCDHisAlarm.FullRowSelect = true;
            this.listViewCCDHisAlarm.GridLines = true;
            this.listViewCCDHisAlarm.Location = new System.Drawing.Point(0, 355);
            this.listViewCCDHisAlarm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listViewCCDHisAlarm.Name = "listViewCCDHisAlarm";
            this.listViewCCDHisAlarm.Size = new System.Drawing.Size(929, 500);
            this.listViewCCDHisAlarm.TabIndex = 2;
            this.listViewCCDHisAlarm.UseCompatibleStateImageBehavior = false;
            this.listViewCCDHisAlarm.View = System.Windows.Forms.View.Details;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LineCCDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(931, 815);
            this.Controls.Add(this.listViewCCDHisAlarm);
            this.Controls.Add(this.dataGridViewCCD);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "LineCCDForm";
            this.Text = "LineCCD";
            this.Load += new System.EventHandler(this.LineCCDForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageCCDPara.ResumeLayout(false);
            this.tabPageCCDPara.PerformLayout();
            this.tabPageCCDPhoto.ResumeLayout(false);
            this.tabPageCCDPhoto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCCD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageCCDPara;
        private System.Windows.Forms.TabPage tabPageCCDPhoto;
        private System.Windows.Forms.DataGridView dataGridViewCCD;
        private System.Windows.Forms.ListView listViewCCDHisAlarm;
        private System.Windows.Forms.Button btnCCDConnect;
        private System.Windows.Forms.TextBox textBoxCCDPort;
        private System.Windows.Forms.TextBox textBoxCCDIP;
        private System.Windows.Forms.Label labPORT;
        private System.Windows.Forms.Label labCCDIP;
        private System.Windows.Forms.Button btnCCDSend;
        private System.Windows.Forms.TextBox textBoxCCDReceive;
        private System.Windows.Forms.TextBox textBoxCCDSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}