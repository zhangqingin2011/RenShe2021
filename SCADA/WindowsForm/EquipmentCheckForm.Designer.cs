namespace SCADA
{
    partial class EquipmentCheckForm
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
            this.dataGridView_Checkcontent = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewalarmdata = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBox_YiChang = new System.Windows.Forms.CheckBox();
            this.checkBox_ZhengChang = new System.Windows.Forms.CheckBox();
            this.checkBox_DongTai = new System.Windows.Forms.CheckBox();
            this.button_UpDataTable = new System.Windows.Forms.Button();
            this.button_ClreaLine = new System.Windows.Forms.Button();
            this.button_StarLine = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Checkcontent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewalarmdata)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_Checkcontent
            // 
            this.dataGridView_Checkcontent.AllowUserToDeleteRows = false;
            this.dataGridView_Checkcontent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_Checkcontent.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_Checkcontent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Checkcontent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Checkcontent.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dataGridView_Checkcontent.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Checkcontent.Margin = new System.Windows.Forms.Padding(5);
            this.dataGridView_Checkcontent.MultiSelect = false;
            this.dataGridView_Checkcontent.Name = "dataGridView_Checkcontent";
            this.dataGridView_Checkcontent.ReadOnly = true;
            this.dataGridView_Checkcontent.RowHeadersVisible = false;
            this.dataGridView_Checkcontent.RowHeadersWidth = 51;
            this.dataGridView_Checkcontent.RowTemplate.Height = 23;
            this.dataGridView_Checkcontent.Size = new System.Drawing.Size(1362, 663);
            this.dataGridView_Checkcontent.TabIndex = 7;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewalarmdata);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView_Checkcontent);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBox_YiChang);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox_ZhengChang);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox_DongTai);
            this.splitContainer1.Panel2.Controls.Add(this.button_UpDataTable);
            this.splitContainer1.Panel2.Controls.Add(this.button_ClreaLine);
            this.splitContainer1.Panel2.Controls.Add(this.button_StarLine);
            this.splitContainer1.Size = new System.Drawing.Size(1362, 742);
            this.splitContainer1.SplitterDistance = 663;
            this.splitContainer1.SplitterWidth = 7;
            this.splitContainer1.TabIndex = 8;
            // 
            // dataGridViewalarmdata
            // 
            this.dataGridViewalarmdata.AllowUserToDeleteRows = false;
            this.dataGridViewalarmdata.AllowUserToOrderColumns = true;
            this.dataGridViewalarmdata.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewalarmdata.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewalarmdata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewalarmdata.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridViewalarmdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewalarmdata.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewalarmdata.Name = "dataGridViewalarmdata";
            this.dataGridViewalarmdata.ReadOnly = true;
            this.dataGridViewalarmdata.RowHeadersVisible = false;
            this.dataGridViewalarmdata.RowHeadersWidth = 51;
            this.dataGridViewalarmdata.RowTemplate.Height = 23;
            this.dataGridViewalarmdata.Size = new System.Drawing.Size(1362, 663);
            this.dataGridViewalarmdata.TabIndex = 8;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "设备";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "报警内容";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // checkBox_YiChang
            // 
            this.checkBox_YiChang.Location = new System.Drawing.Point(0, 0);
            this.checkBox_YiChang.Name = "checkBox_YiChang";
            this.checkBox_YiChang.Size = new System.Drawing.Size(104, 24);
            this.checkBox_YiChang.TabIndex = 0;
            // 
            // checkBox_ZhengChang
            // 
            this.checkBox_ZhengChang.Location = new System.Drawing.Point(0, 0);
            this.checkBox_ZhengChang.Name = "checkBox_ZhengChang";
            this.checkBox_ZhengChang.Size = new System.Drawing.Size(104, 24);
            this.checkBox_ZhengChang.TabIndex = 1;
            // 
            // checkBox_DongTai
            // 
            this.checkBox_DongTai.Location = new System.Drawing.Point(0, 0);
            this.checkBox_DongTai.Name = "checkBox_DongTai";
            this.checkBox_DongTai.Size = new System.Drawing.Size(104, 24);
            this.checkBox_DongTai.TabIndex = 2;
            // 
            // button_UpDataTable
            // 
            this.button_UpDataTable.Location = new System.Drawing.Point(0, 0);
            this.button_UpDataTable.Name = "button_UpDataTable";
            this.button_UpDataTable.Size = new System.Drawing.Size(75, 23);
            this.button_UpDataTable.TabIndex = 3;
            // 
            // button_ClreaLine
            // 
            this.button_ClreaLine.Location = new System.Drawing.Point(0, 0);
            this.button_ClreaLine.Name = "button_ClreaLine";
            this.button_ClreaLine.Size = new System.Drawing.Size(75, 23);
            this.button_ClreaLine.TabIndex = 4;
            // 
            // button_StarLine
            // 
            this.button_StarLine.Location = new System.Drawing.Point(0, 0);
            this.button_StarLine.Name = "button_StarLine";
            this.button_StarLine.Size = new System.Drawing.Size(75, 23);
            this.button_StarLine.TabIndex = 5;
            // 
            // EquipmentCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 742);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "EquipmentCheckForm";
            this.Text = "EquipmentCheckForm";
            this.Load += new System.EventHandler(this.EquipmentCheckForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Checkcontent)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewalarmdata)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Checkcontent;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button_StarLine;
        private System.Windows.Forms.CheckBox checkBox_DongTai;
        private System.Windows.Forms.CheckBox checkBox_YiChang;
        private System.Windows.Forms.CheckBox checkBox_ZhengChang;
        private System.Windows.Forms.Button button_UpDataTable;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_ClreaLine;
        private System.Windows.Forms.DataGridView dataGridViewalarmdata;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}