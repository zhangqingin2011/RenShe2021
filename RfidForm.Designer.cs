﻿namespace SCADA
{ 
    partial class RfidForm
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
            this.listView2 = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSet = new System.Windows.Forms.TabPage();
            this.btnSet = new System.Windows.Forms.Button();
            this.comboBoxBlockSize = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tBStart = new System.Windows.Forms.TextBox();
            this.tBCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPageInit = new System.Windows.Forms.TabPage();
            this.tBInitState = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInitMem = new System.Windows.Forms.Button();
            this.tabPageR = new System.Windows.Forms.TabPage();
            this.btnReadUID = new System.Windows.Forms.Button();
            this.textBoxUID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnReadMem = new System.Windows.Forms.Button();
            this.tBMemData = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPageW = new System.Windows.Forms.TabPage();
            this.btnWUID = new System.Windows.Forms.Button();
            this.textBoxWUID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnWriteMem = new System.Windows.Forms.Button();
            this.mComboBoxData = new SCADA.mComboBox();
            this.dataGridViewRFID = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPageSet.SuspendLayout();
            this.tabPageInit.SuspendLayout();
            this.tabPageR.SuspendLayout();
            this.tabPageW.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRFID)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listView2
            // 
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView2.BackColor = System.Drawing.Color.White;
            this.listView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.Location = new System.Drawing.Point(1, 392);
            this.listView2.Margin = new System.Windows.Forms.Padding(5);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(1101, 264);
            this.listView2.TabIndex = 20;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSet);
            this.tabControl1.Controls.Add(this.tabPageInit);
            this.tabControl1.Controls.Add(this.tabPageR);
            this.tabControl1.Controls.Add(this.tabPageW);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(534, 384);
            this.tabControl1.TabIndex = 35;
            // 
            // tabPageSet
            // 
            this.tabPageSet.BackColor = System.Drawing.Color.White;
            this.tabPageSet.Controls.Add(this.btnSet);
            this.tabPageSet.Controls.Add(this.comboBoxBlockSize);
            this.tabPageSet.Controls.Add(this.label4);
            this.tabPageSet.Controls.Add(this.tBStart);
            this.tabPageSet.Controls.Add(this.tBCount);
            this.tabPageSet.Controls.Add(this.label2);
            this.tabPageSet.Controls.Add(this.label3);
            this.tabPageSet.Location = new System.Drawing.Point(4, 30);
            this.tabPageSet.Margin = new System.Windows.Forms.Padding(5);
            this.tabPageSet.Name = "tabPageSet";
            this.tabPageSet.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageSet.Size = new System.Drawing.Size(526, 350);
            this.tabPageSet.TabIndex = 1;
            this.tabPageSet.Text = "参数设置";
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(220, 253);
            this.btnSet.Margin = new System.Windows.Forms.Padding(5);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(106, 34);
            this.btnSet.TabIndex = 58;
            this.btnSet.Text = "设置";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // comboBoxBlockSize
            // 
            this.comboBoxBlockSize.FormattingEnabled = true;
            this.comboBoxBlockSize.Items.AddRange(new object[] {
            "4字节",
            "8字节"});
            this.comboBoxBlockSize.Location = new System.Drawing.Point(193, 164);
            this.comboBoxBlockSize.Margin = new System.Windows.Forms.Padding(5);
            this.comboBoxBlockSize.Name = "comboBoxBlockSize";
            this.comboBoxBlockSize.Size = new System.Drawing.Size(238, 29);
            this.comboBoxBlockSize.TabIndex = 56;
            this.comboBoxBlockSize.Text = "4字节";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 167);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 21);
            this.label4.TabIndex = 55;
            this.label4.Text = "读写块大小:";
            // 
            // tBStart
            // 
            this.tBStart.Location = new System.Drawing.Point(193, 52);
            this.tBStart.Margin = new System.Windows.Forms.Padding(5);
            this.tBStart.Name = "tBStart";
            this.tBStart.Size = new System.Drawing.Size(238, 29);
            this.tBStart.TabIndex = 53;
            this.tBStart.Text = "0";
            // 
            // tBCount
            // 
            this.tBCount.Location = new System.Drawing.Point(193, 108);
            this.tBCount.Margin = new System.Windows.Forms.Padding(5);
            this.tBCount.Name = "tBCount";
            this.tBCount.Size = new System.Drawing.Size(238, 29);
            this.tBCount.TabIndex = 52;
            this.tBCount.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 21);
            this.label2.TabIndex = 50;
            this.label2.Text = "读写起始块地址:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 110);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 21);
            this.label3.TabIndex = 49;
            this.label3.Text = "读写块数量:";
            // 
            // tabPageInit
            // 
            this.tabPageInit.Controls.Add(this.tBInitState);
            this.tabPageInit.Controls.Add(this.label1);
            this.tabPageInit.Controls.Add(this.btnInitMem);
            this.tabPageInit.Location = new System.Drawing.Point(4, 22);
            this.tabPageInit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageInit.Name = "tabPageInit";
            this.tabPageInit.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageInit.Size = new System.Drawing.Size(526, 358);
            this.tabPageInit.TabIndex = 4;
            this.tabPageInit.Text = "RFID初始化";
            this.tabPageInit.UseVisualStyleBackColor = true;
            // 
            // tBInitState
            // 
            this.tBInitState.Location = new System.Drawing.Point(157, 76);
            this.tBInitState.Margin = new System.Windows.Forms.Padding(5);
            this.tBInitState.Name = "tBInitState";
            this.tBInitState.ReadOnly = true;
            this.tBInitState.Size = new System.Drawing.Size(268, 29);
            this.tBInitState.TabIndex = 53;
            this.tBInitState.Text = "未初始化";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 21);
            this.label1.TabIndex = 52;
            this.label1.Text = "标签数据:";
            // 
            // btnInitMem
            // 
            this.btnInitMem.Location = new System.Drawing.Point(157, 189);
            this.btnInitMem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnInitMem.Name = "btnInitMem";
            this.btnInitMem.Size = new System.Drawing.Size(140, 41);
            this.btnInitMem.TabIndex = 0;
            this.btnInitMem.Text = "初始化";
            this.btnInitMem.UseVisualStyleBackColor = true;
            this.btnInitMem.Click += new System.EventHandler(this.btnInitMem_Click);
            // 
            // tabPageR
            // 
            this.tabPageR.BackColor = System.Drawing.Color.White;
            this.tabPageR.Controls.Add(this.btnReadUID);
            this.tabPageR.Controls.Add(this.textBoxUID);
            this.tabPageR.Controls.Add(this.label5);
            this.tabPageR.Controls.Add(this.btnReadMem);
            this.tabPageR.Controls.Add(this.tBMemData);
            this.tabPageR.Controls.Add(this.label6);
            this.tabPageR.Location = new System.Drawing.Point(4, 30);
            this.tabPageR.Margin = new System.Windows.Forms.Padding(5);
            this.tabPageR.Name = "tabPageR";
            this.tabPageR.Size = new System.Drawing.Size(526, 350);
            this.tabPageR.TabIndex = 3;
            this.tabPageR.Text = "RFID读数据";
            // 
            // btnReadUID
            // 
            this.btnReadUID.Location = new System.Drawing.Point(346, 67);
            this.btnReadUID.Margin = new System.Windows.Forms.Padding(5);
            this.btnReadUID.Name = "btnReadUID";
            this.btnReadUID.Size = new System.Drawing.Size(78, 34);
            this.btnReadUID.TabIndex = 61;
            this.btnReadUID.Text = "读取";
            this.btnReadUID.UseVisualStyleBackColor = true;
            this.btnReadUID.Visible = false;
            this.btnReadUID.Click += new System.EventHandler(this.btnReadUID_Click);
            // 
            // textBoxUID
            // 
            this.textBoxUID.Location = new System.Drawing.Point(125, 73);
            this.textBoxUID.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxUID.Name = "textBoxUID";
            this.textBoxUID.ReadOnly = true;
            this.textBoxUID.Size = new System.Drawing.Size(200, 29);
            this.textBoxUID.TabIndex = 60;
            this.textBoxUID.Text = "04328749A";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 73);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 21);
            this.label5.TabIndex = 59;
            this.label5.Text = "标签识别码:";
            // 
            // btnReadMem
            // 
            this.btnReadMem.Location = new System.Drawing.Point(346, 171);
            this.btnReadMem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnReadMem.Name = "btnReadMem";
            this.btnReadMem.Size = new System.Drawing.Size(78, 40);
            this.btnReadMem.TabIndex = 57;
            this.btnReadMem.Text = "读数据";
            this.btnReadMem.UseVisualStyleBackColor = true;
            this.btnReadMem.Click += new System.EventHandler(this.btnReadMem_Click);
            // 
            // tBMemData
            // 
            this.tBMemData.Location = new System.Drawing.Point(125, 178);
            this.tBMemData.Margin = new System.Windows.Forms.Padding(5);
            this.tBMemData.Name = "tBMemData";
            this.tBMemData.ReadOnly = true;
            this.tBMemData.Size = new System.Drawing.Size(200, 29);
            this.tBMemData.TabIndex = 56;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 181);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 21);
            this.label6.TabIndex = 55;
            this.label6.Text = "数据:";
            // 
            // tabPageW
            // 
            this.tabPageW.Controls.Add(this.btnWUID);
            this.tabPageW.Controls.Add(this.textBoxWUID);
            this.tabPageW.Controls.Add(this.label8);
            this.tabPageW.Controls.Add(this.label7);
            this.tabPageW.Controls.Add(this.btnWriteMem);
            this.tabPageW.Controls.Add(this.mComboBoxData);
            this.tabPageW.Location = new System.Drawing.Point(4, 30);
            this.tabPageW.Name = "tabPageW";
            this.tabPageW.Size = new System.Drawing.Size(526, 350);
            this.tabPageW.TabIndex = 5;
            this.tabPageW.Text = "RFID写数据";
            this.tabPageW.UseVisualStyleBackColor = true;
            // 
            // btnWUID
            // 
            this.btnWUID.Location = new System.Drawing.Point(358, 57);
            this.btnWUID.Margin = new System.Windows.Forms.Padding(5);
            this.btnWUID.Name = "btnWUID";
            this.btnWUID.Size = new System.Drawing.Size(78, 34);
            this.btnWUID.TabIndex = 64;
            this.btnWUID.Text = "读取";
            this.btnWUID.UseVisualStyleBackColor = true;
            this.btnWUID.Visible = false;
            // 
            // textBoxWUID
            // 
            this.textBoxWUID.Location = new System.Drawing.Point(122, 60);
            this.textBoxWUID.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxWUID.Name = "textBoxWUID";
            this.textBoxWUID.ReadOnly = true;
            this.textBoxWUID.Size = new System.Drawing.Size(202, 29);
            this.textBoxWUID.TabIndex = 63;
            this.textBoxWUID.Text = "04328749A";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 60);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 21);
            this.label8.TabIndex = 62;
            this.label8.Text = "标签识别码:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 142);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 21);
            this.label7.TabIndex = 60;
            this.label7.Text = "写入数据:";
            // 
            // btnWriteMem
            // 
            this.btnWriteMem.Location = new System.Drawing.Point(358, 136);
            this.btnWriteMem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnWriteMem.Name = "btnWriteMem";
            this.btnWriteMem.Size = new System.Drawing.Size(78, 34);
            this.btnWriteMem.TabIndex = 59;
            this.btnWriteMem.Text = "写数据";
            this.btnWriteMem.UseVisualStyleBackColor = true;
            this.btnWriteMem.Click += new System.EventHandler(this.btnWriteMem_Click);
            // 
            // mComboBoxData
            // 
            this.mComboBoxData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mComboBoxData.FormattingEnabled = true;
            this.mComboBoxData.Items.AddRange(new object[] {
            "生料",
            "合格",
            "不合格"});
            this.mComboBoxData.Location = new System.Drawing.Point(122, 139);
            this.mComboBoxData.Name = "mComboBoxData";
            this.mComboBoxData.Size = new System.Drawing.Size(201, 29);
            this.mComboBoxData.TabIndex = 61;
            // 
            // dataGridViewRFID
            // 
            this.dataGridViewRFID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRFID.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewRFID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewRFID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRFID.Location = new System.Drawing.Point(541, 14);
            this.dataGridViewRFID.Margin = new System.Windows.Forms.Padding(5);
            this.dataGridViewRFID.Name = "dataGridViewRFID";
            this.dataGridViewRFID.RowHeadersVisible = false;
            this.dataGridViewRFID.RowHeadersWidth = 10;
            this.dataGridViewRFID.RowTemplate.Height = 23;
            this.dataGridViewRFID.Size = new System.Drawing.Size(561, 368);
            this.dataGridViewRFID.TabIndex = 36;
            // 
            // RfidForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1116, 662);
            this.Controls.Add(this.dataGridViewRFID);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listView2);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(6, 46);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RfidForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LineDetect";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RfidForm_FormClosed);
            this.Load += new System.EventHandler(this.RfidForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSet.ResumeLayout(false);
            this.tabPageSet.PerformLayout();
            this.tabPageInit.ResumeLayout(false);
            this.tabPageInit.PerformLayout();
            this.tabPageR.ResumeLayout(false);
            this.tabPageR.PerformLayout();
            this.tabPageW.ResumeLayout(false);
            this.tabPageW.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRFID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;

        public int m_ComOpenNum;

        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSet;
        private System.Windows.Forms.ComboBox comboBoxBlockSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tBStart;
        private System.Windows.Forms.TextBox tBCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPageR;
        private System.Windows.Forms.DataGridView dataGridViewRFID;
        private System.Windows.Forms.TabPage tabPageInit;
        private System.Windows.Forms.Button btnInitMem;
        private System.Windows.Forms.TextBox tBInitState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBMemData;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnReadMem;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnReadUID;
        private System.Windows.Forms.TextBox textBoxUID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPageW;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnWriteMem;
        private mComboBox mComboBoxData;
        private System.Windows.Forms.Button btnWUID;
        private System.Windows.Forms.TextBox textBoxWUID;
        private System.Windows.Forms.Label label8;
    }
}