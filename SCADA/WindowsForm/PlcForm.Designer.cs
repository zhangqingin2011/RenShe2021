﻿namespace SCADA
{
    partial class PlcForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlcForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.spContainerPLC = new System.Windows.Forms.SplitContainer();
            this.plcDGV1 = new System.Windows.Forms.DataGridView();
            this.input_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.input_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.input_Monitor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.input_EQUIP_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.input_ACTION_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxPLCJiankong1 = new System.Windows.Forms.ComboBox();
            this.comboBoxPLCJiankong2 = new System.Windows.Forms.ComboBox();
            this.plcDGV2 = new System.Windows.Forms.DataGridView();
            this.output_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.output_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.output_Monitor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.output_EQUIP_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oput_ACTION_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelLinckText = new System.Windows.Forms.Label();
            this.pictureBox_LinkState = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.switchPlc = new SCADA.mComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_shliujinzhi = new System.Windows.Forms.RadioButton();
            this.radioButton_shijinzhi = new System.Windows.Forms.RadioButton();
            this.radioButton_serjinzhi = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_PLCRegMonitor = new System.Windows.Forms.TabPage();
            this.tabPage_PLCOperation = new System.Windows.Forms.TabPage();
            this.comboBox_PLCStationSelet = new SCADA.mComboBox();
            this.button27 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage_Alarm = new System.Windows.Forms.TabPage();
            this.button_alarmReFlesh = new System.Windows.Forms.Button();
            this.dataGridViewalarmdata = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HistoricalAlarm = new System.Windows.Forms.RadioButton();
            this.CurrentAlarm = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label_PLCSysTemText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.spContainerPLC)).BeginInit();
            this.spContainerPLC.Panel1.SuspendLayout();
            this.spContainerPLC.Panel2.SuspendLayout();
            this.spContainerPLC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plcDGV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plcDGV2)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LinkState)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_PLCRegMonitor.SuspendLayout();
            this.tabPage_PLCOperation.SuspendLayout();
            this.tabPage_Alarm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewalarmdata)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // spContainerPLC
            // 
            this.spContainerPLC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.spContainerPLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spContainerPLC.Location = new System.Drawing.Point(0, 0);
            this.spContainerPLC.Name = "spContainerPLC";
            // 
            // spContainerPLC.Panel1
            // 
            this.spContainerPLC.Panel1.AutoScrollMargin = new System.Drawing.Size(300, 0);
            this.spContainerPLC.Panel1.Controls.Add(this.plcDGV1);
            this.spContainerPLC.Panel1.Controls.Add(this.comboBoxPLCJiankong1);
            // 
            // spContainerPLC.Panel2
            // 
            this.spContainerPLC.Panel2.Controls.Add(this.comboBoxPLCJiankong2);
            this.spContainerPLC.Panel2.Controls.Add(this.plcDGV2);
            this.spContainerPLC.Size = new System.Drawing.Size(574, 532);
            this.spContainerPLC.SplitterDistance = 286;
            this.spContainerPLC.TabIndex = 7;
            // 
            // plcDGV1
            // 
            this.plcDGV1.AllowUserToAddRows = false;
            this.plcDGV1.AllowUserToDeleteRows = false;
            this.plcDGV1.BackgroundColor = System.Drawing.Color.White;
            this.plcDGV1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.plcDGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.plcDGV1.ColumnHeadersHeight = 25;
            this.plcDGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.input_id,
            this.input_name,
            this.Column1,
            this.input_Monitor,
            this.input_EQUIP_CODE,
            this.input_ACTION_ID});
            this.plcDGV1.Location = new System.Drawing.Point(1, 24);
            this.plcDGV1.Name = "plcDGV1";
            this.plcDGV1.ReadOnly = true;
            this.plcDGV1.RowHeadersVisible = false;
            this.plcDGV1.RowTemplate.Height = 23;
            this.plcDGV1.Size = new System.Drawing.Size(288, 503);
            this.plcDGV1.TabIndex = 0;
            // 
            // input_id
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.input_id.DefaultCellStyle = dataGridViewCellStyle2;
            this.input_id.FillWeight = 144.0867F;
            this.input_id.HeaderText = "地址";
            this.input_id.Name = "input_id";
            this.input_id.ReadOnly = true;
            this.input_id.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.input_id.Width = 54;
            // 
            // input_name
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.input_name.DefaultCellStyle = dataGridViewCellStyle3;
            this.input_name.FillWeight = 119.0281F;
            this.input_name.HeaderText = "定义";
            this.input_name.Name = "input_name";
            this.input_name.ReadOnly = true;
            this.input_name.Width = 54;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "值";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // input_Monitor
            // 
            this.input_Monitor.HeaderText = "动作上报";
            this.input_Monitor.Name = "input_Monitor";
            this.input_Monitor.ReadOnly = true;
            this.input_Monitor.Width = 78;
            // 
            // input_EQUIP_CODE
            // 
            this.input_EQUIP_CODE.HeaderText = "部件ID";
            this.input_EQUIP_CODE.Name = "input_EQUIP_CODE";
            this.input_EQUIP_CODE.ReadOnly = true;
            this.input_EQUIP_CODE.Width = 66;
            // 
            // input_ACTION_ID
            // 
            this.input_ACTION_ID.HeaderText = "触发事件ID";
            this.input_ACTION_ID.Name = "input_ACTION_ID";
            this.input_ACTION_ID.ReadOnly = true;
            this.input_ACTION_ID.Width = 90;
            // 
            // comboBoxPLCJiankong1
            // 
            this.comboBoxPLCJiankong1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPLCJiankong1.FormattingEnabled = true;
            this.comboBoxPLCJiankong1.Location = new System.Drawing.Point(122, 2);
            this.comboBoxPLCJiankong1.Name = "comboBoxPLCJiankong1";
            this.comboBoxPLCJiankong1.Size = new System.Drawing.Size(121, 20);
            this.comboBoxPLCJiankong1.TabIndex = 8;
            this.comboBoxPLCJiankong1.SelectedIndexChanged += new System.EventHandler(this.comboBoxPLCJiankong1_SelectedIndexChanged);
            // 
            // comboBoxPLCJiankong2
            // 
            this.comboBoxPLCJiankong2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPLCJiankong2.FormattingEnabled = true;
            this.comboBoxPLCJiankong2.Location = new System.Drawing.Point(128, 2);
            this.comboBoxPLCJiankong2.Name = "comboBoxPLCJiankong2";
            this.comboBoxPLCJiankong2.Size = new System.Drawing.Size(121, 20);
            this.comboBoxPLCJiankong2.TabIndex = 8;
            this.comboBoxPLCJiankong2.SelectedIndexChanged += new System.EventHandler(this.comboBoxPLCJiankong2_SelectedIndexChanged);
            // 
            // plcDGV2
            // 
            this.plcDGV2.AllowUserToAddRows = false;
            this.plcDGV2.AllowUserToDeleteRows = false;
            this.plcDGV2.BackgroundColor = System.Drawing.Color.White;
            this.plcDGV2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.plcDGV2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.plcDGV2.ColumnHeadersHeight = 25;
            this.plcDGV2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.output_id,
            this.output_name,
            this.dataGridViewTextBoxColumn1,
            this.output_Monitor,
            this.output_EQUIP_CODE,
            this.oput_ACTION_ID});
            this.plcDGV2.Location = new System.Drawing.Point(5, 24);
            this.plcDGV2.Name = "plcDGV2";
            this.plcDGV2.ReadOnly = true;
            this.plcDGV2.RowHeadersVisible = false;
            this.plcDGV2.RowTemplate.Height = 23;
            this.plcDGV2.Size = new System.Drawing.Size(280, 503);
            this.plcDGV2.TabIndex = 0;
            // 
            // output_id
            // 
            this.output_id.DataPropertyName = "address";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.output_id.DefaultCellStyle = dataGridViewCellStyle5;
            this.output_id.HeaderText = "地址";
            this.output_id.Name = "output_id";
            this.output_id.ReadOnly = true;
            this.output_id.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.output_id.Width = 54;
            // 
            // output_name
            // 
            this.output_name.DataPropertyName = "name";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.output_name.DefaultCellStyle = dataGridViewCellStyle6;
            this.output_name.HeaderText = "定义";
            this.output_name.Name = "output_name";
            this.output_name.ReadOnly = true;
            this.output_name.Width = 54;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "值";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // output_Monitor
            // 
            this.output_Monitor.HeaderText = "动作上报";
            this.output_Monitor.Name = "output_Monitor";
            this.output_Monitor.ReadOnly = true;
            this.output_Monitor.Width = 78;
            // 
            // output_EQUIP_CODE
            // 
            this.output_EQUIP_CODE.HeaderText = "部件ID";
            this.output_EQUIP_CODE.Name = "output_EQUIP_CODE";
            this.output_EQUIP_CODE.ReadOnly = true;
            this.output_EQUIP_CODE.Width = 66;
            // 
            // oput_ACTION_ID
            // 
            this.oput_ACTION_ID.HeaderText = "动作ID";
            this.oput_ACTION_ID.Name = "oput_ACTION_ID";
            this.oput_ACTION_ID.ReadOnly = true;
            this.oput_ACTION_ID.Width = 66;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelLinckText);
            this.groupBox3.Controls.Add(this.pictureBox_LinkState);
            this.groupBox3.Location = new System.Drawing.Point(514, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(139, 58);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PLC连接状态";
            // 
            // labelLinckText
            // 
            this.labelLinckText.AutoSize = true;
            this.labelLinckText.Location = new System.Drawing.Point(12, 29);
            this.labelLinckText.Name = "labelLinckText";
            this.labelLinckText.Size = new System.Drawing.Size(29, 12);
            this.labelLinckText.TabIndex = 8;
            this.labelLinckText.Text = "离线";
            // 
            // pictureBox_LinkState
            // 
            this.pictureBox_LinkState.Location = new System.Drawing.Point(105, 23);
            this.pictureBox_LinkState.Name = "pictureBox_LinkState";
            this.pictureBox_LinkState.Size = new System.Drawing.Size(24, 29);
            this.pictureBox_LinkState.TabIndex = 7;
            this.pictureBox_LinkState.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.switchPlc);
            this.groupBox2.Location = new System.Drawing.Point(24, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(139, 58);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PLC选择";
            // 
            // switchPlc
            // 
            this.switchPlc.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.switchPlc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.switchPlc.FormattingEnabled = true;
            this.switchPlc.Location = new System.Drawing.Point(11, 21);
            this.switchPlc.Margin = new System.Windows.Forms.Padding(4);
            this.switchPlc.Name = "switchPlc";
            this.switchPlc.Size = new System.Drawing.Size(116, 20);
            this.switchPlc.TabIndex = 0;
            this.switchPlc.SelectedIndexChanged += new System.EventHandler(this.switchPlc_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_shliujinzhi);
            this.groupBox1.Controls.Add(this.radioButton_shijinzhi);
            this.groupBox1.Controls.Add(this.radioButton_serjinzhi);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 197);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "寄存器值显示方式";
            // 
            // radioButton_shliujinzhi
            // 
            this.radioButton_shliujinzhi.AutoSize = true;
            this.radioButton_shliujinzhi.Checked = true;
            this.radioButton_shliujinzhi.Location = new System.Drawing.Point(21, 143);
            this.radioButton_shliujinzhi.Name = "radioButton_shliujinzhi";
            this.radioButton_shliujinzhi.Size = new System.Drawing.Size(71, 16);
            this.radioButton_shliujinzhi.TabIndex = 5;
            this.radioButton_shliujinzhi.TabStop = true;
            this.radioButton_shliujinzhi.Text = "十六进制";
            this.radioButton_shliujinzhi.UseVisualStyleBackColor = true;
            // 
            // radioButton_shijinzhi
            // 
            this.radioButton_shijinzhi.AutoSize = true;
            this.radioButton_shijinzhi.Location = new System.Drawing.Point(21, 96);
            this.radioButton_shijinzhi.Name = "radioButton_shijinzhi";
            this.radioButton_shijinzhi.Size = new System.Drawing.Size(59, 16);
            this.radioButton_shijinzhi.TabIndex = 5;
            this.radioButton_shijinzhi.TabStop = true;
            this.radioButton_shijinzhi.Text = "十进制";
            this.radioButton_shijinzhi.UseVisualStyleBackColor = true;
            // 
            // radioButton_serjinzhi
            // 
            this.radioButton_serjinzhi.AutoSize = true;
            this.radioButton_serjinzhi.Location = new System.Drawing.Point(21, 50);
            this.radioButton_serjinzhi.Name = "radioButton_serjinzhi";
            this.radioButton_serjinzhi.Size = new System.Drawing.Size(59, 16);
            this.radioButton_serjinzhi.TabIndex = 5;
            this.radioButton_serjinzhi.TabStop = true;
            this.radioButton_serjinzhi.Text = "二进制";
            this.radioButton_serjinzhi.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewImageColumn1.DataPropertyName = "getPicture";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle7.NullValue = null;
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewImageColumn1.HeaderText = "信号状态";
            this.dataGridViewImageColumn1.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn1.Image")));
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewImageColumn2.DataPropertyName = "getPicture";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle8.NullValue = null;
            this.dataGridViewImageColumn2.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewImageColumn2.HeaderText = "信号状态";
            this.dataGridViewImageColumn2.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn2.Image")));
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.spContainerPLC);
            this.splitContainer1.Size = new System.Drawing.Size(693, 532);
            this.splitContainer1.SplitterDistance = 115;
            this.splitContainer1.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_PLCRegMonitor);
            this.tabControl1.Controls.Add(this.tabPage_PLCOperation);
            this.tabControl1.Controls.Add(this.tabPage_Alarm);
            this.tabControl1.Location = new System.Drawing.Point(4, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(707, 564);
            this.tabControl1.TabIndex = 9;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage_PLCRegMonitor
            // 
            this.tabPage_PLCRegMonitor.Controls.Add(this.splitContainer1);
            this.tabPage_PLCRegMonitor.Location = new System.Drawing.Point(4, 22);
            this.tabPage_PLCRegMonitor.Name = "tabPage_PLCRegMonitor";
            this.tabPage_PLCRegMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_PLCRegMonitor.Size = new System.Drawing.Size(699, 538);
            this.tabPage_PLCRegMonitor.TabIndex = 0;
            this.tabPage_PLCRegMonitor.Text = "寄存器监控";
            this.tabPage_PLCRegMonitor.UseVisualStyleBackColor = true;
            // 
            // tabPage_PLCOperation
            // 
            this.tabPage_PLCOperation.Controls.Add(this.comboBox_PLCStationSelet);
            this.tabPage_PLCOperation.Controls.Add(this.button27);
            this.tabPage_PLCOperation.Controls.Add(this.button26);
            this.tabPage_PLCOperation.Controls.Add(this.button25);
            this.tabPage_PLCOperation.Controls.Add(this.button24);
            this.tabPage_PLCOperation.Controls.Add(this.button20);
            this.tabPage_PLCOperation.Controls.Add(this.button22);
            this.tabPage_PLCOperation.Controls.Add(this.button18);
            this.tabPage_PLCOperation.Controls.Add(this.button23);
            this.tabPage_PLCOperation.Controls.Add(this.button21);
            this.tabPage_PLCOperation.Controls.Add(this.button19);
            this.tabPage_PLCOperation.Controls.Add(this.button17);
            this.tabPage_PLCOperation.Controls.Add(this.button16);
            this.tabPage_PLCOperation.Controls.Add(this.button14);
            this.tabPage_PLCOperation.Controls.Add(this.button15);
            this.tabPage_PLCOperation.Controls.Add(this.button13);
            this.tabPage_PLCOperation.Controls.Add(this.button12);
            this.tabPage_PLCOperation.Controls.Add(this.button10);
            this.tabPage_PLCOperation.Controls.Add(this.button11);
            this.tabPage_PLCOperation.Controls.Add(this.button9);
            this.tabPage_PLCOperation.Controls.Add(this.button8);
            this.tabPage_PLCOperation.Controls.Add(this.button6);
            this.tabPage_PLCOperation.Controls.Add(this.button7);
            this.tabPage_PLCOperation.Controls.Add(this.button5);
            this.tabPage_PLCOperation.Controls.Add(this.button4);
            this.tabPage_PLCOperation.Controls.Add(this.button3);
            this.tabPage_PLCOperation.Controls.Add(this.button2);
            this.tabPage_PLCOperation.Controls.Add(this.button1);
            this.tabPage_PLCOperation.Location = new System.Drawing.Point(4, 22);
            this.tabPage_PLCOperation.Name = "tabPage_PLCOperation";
            this.tabPage_PLCOperation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_PLCOperation.Size = new System.Drawing.Size(699, 538);
            this.tabPage_PLCOperation.TabIndex = 1;
            this.tabPage_PLCOperation.Text = "操作";
            this.tabPage_PLCOperation.UseVisualStyleBackColor = true;
            // 
            // comboBox_PLCStationSelet
            // 
            this.comboBox_PLCStationSelet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PLCStationSelet.FormattingEnabled = true;
            this.comboBox_PLCStationSelet.Location = new System.Drawing.Point(275, 17);
            this.comboBox_PLCStationSelet.Name = "comboBox_PLCStationSelet";
            this.comboBox_PLCStationSelet.Size = new System.Drawing.Size(121, 20);
            this.comboBox_PLCStationSelet.TabIndex = 1;
            this.comboBox_PLCStationSelet.SelectedIndexChanged += new System.EventHandler(this.comboBox_PLCStationSelet_SelectedIndexChanged);
            // 
            // button27
            // 
            this.button27.Location = new System.Drawing.Point(689, 185);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(146, 35);
            this.button27.TabIndex = 0;
            this.button27.Tag = "9999";
            this.button27.Text = "button27";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Visible = false;
            // 
            // button26
            // 
            this.button26.Location = new System.Drawing.Point(689, 267);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(146, 35);
            this.button26.TabIndex = 0;
            this.button26.Tag = "9999";
            this.button26.Text = "button26";
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Visible = false;
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(689, 342);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(146, 35);
            this.button25.TabIndex = 0;
            this.button25.Tag = "9999";
            this.button25.Text = "button25";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Visible = false;
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(496, 362);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(146, 35);
            this.button24.TabIndex = 0;
            this.button24.Tag = "9999";
            this.button24.Text = "button24";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Visible = false;
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(496, 303);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(146, 35);
            this.button20.TabIndex = 0;
            this.button20.Tag = "9999";
            this.button20.Text = "button20";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Visible = false;
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(184, 362);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(146, 35);
            this.button22.TabIndex = 0;
            this.button22.Tag = "9999";
            this.button22.Text = "button22";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Visible = false;
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(184, 303);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(146, 35);
            this.button18.TabIndex = 0;
            this.button18.Tag = "9999";
            this.button18.Text = "button18";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Visible = false;
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(340, 362);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(146, 35);
            this.button23.TabIndex = 0;
            this.button23.Tag = "9999";
            this.button23.Text = "button23";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Visible = false;
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(28, 362);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(146, 35);
            this.button21.TabIndex = 0;
            this.button21.Tag = "9999";
            this.button21.Text = "button21";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Visible = false;
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(340, 303);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(146, 35);
            this.button19.TabIndex = 0;
            this.button19.Tag = "9999";
            this.button19.Text = "button19";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Visible = false;
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(28, 303);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(146, 35);
            this.button17.TabIndex = 0;
            this.button17.Tag = "9999";
            this.button17.Text = "button17";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Visible = false;
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(496, 244);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(146, 35);
            this.button16.TabIndex = 0;
            this.button16.Tag = "9999";
            this.button16.Text = "button16";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Visible = false;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(184, 244);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(146, 35);
            this.button14.TabIndex = 0;
            this.button14.Tag = "9999";
            this.button14.Text = "button14";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Visible = false;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(340, 244);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(146, 35);
            this.button15.TabIndex = 0;
            this.button15.Tag = "9999";
            this.button15.Text = "button15";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Visible = false;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(28, 244);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(146, 35);
            this.button13.TabIndex = 0;
            this.button13.Tag = "9999";
            this.button13.Text = "button13";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Visible = false;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(496, 185);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(146, 35);
            this.button12.TabIndex = 0;
            this.button12.Tag = "9999";
            this.button12.Text = "button12";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Visible = false;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(184, 185);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(146, 35);
            this.button10.TabIndex = 0;
            this.button10.Tag = "9999";
            this.button10.Text = "button10";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Visible = false;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(340, 185);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(146, 35);
            this.button11.TabIndex = 0;
            this.button11.Tag = "9999";
            this.button11.Text = "button11";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Visible = false;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(28, 185);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(146, 35);
            this.button9.TabIndex = 0;
            this.button9.Tag = "9999";
            this.button9.Text = "button9";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Visible = false;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(496, 126);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(146, 35);
            this.button8.TabIndex = 0;
            this.button8.Tag = "9999";
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(184, 126);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(146, 35);
            this.button6.TabIndex = 0;
            this.button6.Tag = "9999";
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(340, 126);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(146, 35);
            this.button7.TabIndex = 0;
            this.button7.Tag = "9999";
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(28, 126);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(146, 35);
            this.button5.TabIndex = 0;
            this.button5.Tag = "9999";
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(496, 67);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(146, 35);
            this.button4.TabIndex = 0;
            this.button4.Tag = "9999";
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(340, 67);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(146, 35);
            this.button3.TabIndex = 0;
            this.button3.Tag = "9999";
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(184, 67);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(146, 35);
            this.button2.TabIndex = 0;
            this.button2.Tag = "9999";
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 35);
            this.button1.TabIndex = 0;
            this.button1.Tag = "9999";
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // tabPage_Alarm
            // 
            this.tabPage_Alarm.Controls.Add(this.button_alarmReFlesh);
            this.tabPage_Alarm.Controls.Add(this.dataGridViewalarmdata);
            this.tabPage_Alarm.Controls.Add(this.HistoricalAlarm);
            this.tabPage_Alarm.Controls.Add(this.CurrentAlarm);
            this.tabPage_Alarm.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Alarm.Name = "tabPage_Alarm";
            this.tabPage_Alarm.Size = new System.Drawing.Size(699, 538);
            this.tabPage_Alarm.TabIndex = 2;
            this.tabPage_Alarm.Text = "报警";
            this.tabPage_Alarm.UseVisualStyleBackColor = true;
            // 
            // button_alarmReFlesh
            // 
            this.button_alarmReFlesh.Location = new System.Drawing.Point(316, 5);
            this.button_alarmReFlesh.Name = "button_alarmReFlesh";
            this.button_alarmReFlesh.Size = new System.Drawing.Size(75, 23);
            this.button_alarmReFlesh.TabIndex = 8;
            this.button_alarmReFlesh.Text = "刷新";
            this.button_alarmReFlesh.UseVisualStyleBackColor = true;
            this.button_alarmReFlesh.Click += new System.EventHandler(this.button_alarmReFlesh_Click);
            // 
            // dataGridViewalarmdata
            // 
            this.dataGridViewalarmdata.AllowUserToDeleteRows = false;
            this.dataGridViewalarmdata.AllowUserToOrderColumns = true;
            this.dataGridViewalarmdata.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewalarmdata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewalarmdata.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.Column2,
            this.Column3});
            this.dataGridViewalarmdata.Location = new System.Drawing.Point(0, 35);
            this.dataGridViewalarmdata.Name = "dataGridViewalarmdata";
            this.dataGridViewalarmdata.ReadOnly = true;
            this.dataGridViewalarmdata.RowHeadersVisible = false;
            this.dataGridViewalarmdata.RowTemplate.Height = 23;
            this.dataGridViewalarmdata.Size = new System.Drawing.Size(696, 503);
            this.dataGridViewalarmdata.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "序号";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "报警号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "报警内容";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 400;
            // 
            // HistoricalAlarm
            // 
            this.HistoricalAlarm.AutoSize = true;
            this.HistoricalAlarm.Location = new System.Drawing.Point(133, 13);
            this.HistoricalAlarm.Name = "HistoricalAlarm";
            this.HistoricalAlarm.Size = new System.Drawing.Size(71, 16);
            this.HistoricalAlarm.TabIndex = 6;
            this.HistoricalAlarm.Text = "历史报警";
            this.HistoricalAlarm.UseVisualStyleBackColor = true;
            this.HistoricalAlarm.CheckedChanged += new System.EventHandler(this.HistoricalAlarm_CheckedChanged);
            // 
            // CurrentAlarm
            // 
            this.CurrentAlarm.AutoSize = true;
            this.CurrentAlarm.Checked = true;
            this.CurrentAlarm.Location = new System.Drawing.Point(14, 13);
            this.CurrentAlarm.Name = "CurrentAlarm";
            this.CurrentAlarm.Size = new System.Drawing.Size(71, 16);
            this.CurrentAlarm.TabIndex = 5;
            this.CurrentAlarm.TabStop = true;
            this.CurrentAlarm.Text = "当前报警";
            this.CurrentAlarm.UseVisualStyleBackColor = true;
            this.CurrentAlarm.CheckedChanged += new System.EventHandler(this.CurrentAlarm_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label_PLCSysTemText);
            this.groupBox4.Location = new System.Drawing.Point(269, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(139, 58);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PLC系统";
            // 
            // label_PLCSysTemText
            // 
            this.label_PLCSysTemText.AutoSize = true;
            this.label_PLCSysTemText.Location = new System.Drawing.Point(43, 29);
            this.label_PLCSysTemText.Name = "label_PLCSysTemText";
            this.label_PLCSysTemText.Size = new System.Drawing.Size(47, 12);
            this.label_PLCSysTemText.TabIndex = 8;
            this.label_PLCSysTemText.Text = "PLC系统";
            // 
            // PlcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(712, 630);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "PlcForm";
            this.Text = "PlcForm";
            this.Load += new System.EventHandler(this.PlcForm_Load);
            this.SizeChanged += new System.EventHandler(this.PlcForm_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.PlcForm_VisibleChanged);
            this.spContainerPLC.Panel1.ResumeLayout(false);
            this.spContainerPLC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spContainerPLC)).EndInit();
            this.spContainerPLC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.plcDGV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plcDGV2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LinkState)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_PLCRegMonitor.ResumeLayout(false);
            this.tabPage_PLCOperation.ResumeLayout(false);
            this.tabPage_Alarm.ResumeLayout(false);
            this.tabPage_Alarm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewalarmdata)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spContainerPLC;
        private System.Windows.Forms.DataGridView plcDGV1;
        private System.Windows.Forms.DataGridView plcDGV2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private mComboBox switchPlc;
        private System.Windows.Forms.ComboBox comboBoxPLCJiankong1;
        private System.Windows.Forms.ComboBox comboBoxPLCJiankong2;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_Monitor;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_EQUIP_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_ACTION_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn output_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn output_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn output_Monitor;
        private System.Windows.Forms.DataGridViewTextBoxColumn output_EQUIP_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn oput_ACTION_ID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_serjinzhi;
        private System.Windows.Forms.RadioButton radioButton_shliujinzhi;
        private System.Windows.Forms.RadioButton radioButton_shijinzhi;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox_LinkState;
        private System.Windows.Forms.Label labelLinckText;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPage_PLCRegMonitor;
        private System.Windows.Forms.TabPage tabPage_PLCOperation;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private mComboBox comboBox_PLCStationSelet;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label_PLCSysTemText;
        private System.Windows.Forms.TabPage tabPage_Alarm;
        private System.Windows.Forms.DataGridView dataGridViewalarmdata;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.RadioButton HistoricalAlarm;
        private System.Windows.Forms.RadioButton CurrentAlarm;
        private System.Windows.Forms.Button button_alarmReFlesh;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl1;

    }
}