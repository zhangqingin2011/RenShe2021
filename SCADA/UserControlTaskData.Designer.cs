namespace SCADA
{
    partial class UserControlTaskData
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_DeletGCode = new System.Windows.Forms.Button();
            this.button_AddGCode = new System.Windows.Forms.Button();
            this.button_DowLoadGCode = new System.Windows.Forms.Button();
            this.dataGridView_CNCGCode = new System.Windows.Forms.DataGridView();
            this.dataGridView_GCodeSele = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.timercncstate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CNCGCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_GCodeSele)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_DeletGCode
            // 
            this.button_DeletGCode.Location = new System.Drawing.Point(61, 120);
            this.button_DeletGCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_DeletGCode.Name = "button_DeletGCode";
            this.button_DeletGCode.Size = new System.Drawing.Size(97, 55);
            this.button_DeletGCode.TabIndex = 1;
            this.button_DeletGCode.Text = "删除G代码";
            this.button_DeletGCode.UseVisualStyleBackColor = true;
            this.button_DeletGCode.Click += new System.EventHandler(this.button_DeletGCode_Click);
            // 
            // button_AddGCode
            // 
            this.button_AddGCode.Location = new System.Drawing.Point(61, 11);
            this.button_AddGCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_AddGCode.Name = "button_AddGCode";
            this.button_AddGCode.Size = new System.Drawing.Size(97, 55);
            this.button_AddGCode.TabIndex = 1;
            this.button_AddGCode.Text = "添加G代码";
            this.button_AddGCode.UseVisualStyleBackColor = true;
            this.button_AddGCode.Click += new System.EventHandler(this.button_AddGCode_Click);
            // 
            // button_DowLoadGCode
            // 
            this.button_DowLoadGCode.Location = new System.Drawing.Point(61, 230);
            this.button_DowLoadGCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_DowLoadGCode.Name = "button_DowLoadGCode";
            this.button_DowLoadGCode.Size = new System.Drawing.Size(97, 55);
            this.button_DowLoadGCode.TabIndex = 1;
            this.button_DowLoadGCode.Text = "程序下发";
            this.button_DowLoadGCode.UseVisualStyleBackColor = true;
            this.button_DowLoadGCode.Click += new System.EventHandler(this.button_DowLoadGCode_Clickv2);
            // 
            // dataGridView_CNCGCode
            // 
            this.dataGridView_CNCGCode.AllowUserToDeleteRows = false;
            this.dataGridView_CNCGCode.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_CNCGCode.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_CNCGCode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CNCGCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_CNCGCode.Location = new System.Drawing.Point(4, 5);
            this.dataGridView_CNCGCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView_CNCGCode.Name = "dataGridView_CNCGCode";
            this.dataGridView_CNCGCode.RowHeadersVisible = false;
            this.dataGridView_CNCGCode.RowTemplate.Height = 23;
            this.dataGridView_CNCGCode.Size = new System.Drawing.Size(971, 388);
            this.dataGridView_CNCGCode.TabIndex = 0;
            // 
            // dataGridView_GCodeSele
            // 
            this.dataGridView_GCodeSele.AllowUserToDeleteRows = false;
            this.dataGridView_GCodeSele.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_GCodeSele.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_GCodeSele.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_GCodeSele.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_GCodeSele.Location = new System.Drawing.Point(4, 403);
            this.dataGridView_GCodeSele.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView_GCodeSele.Name = "dataGridView_GCodeSele";
            this.dataGridView_GCodeSele.RowTemplate.Height = 23;
            this.dataGridView_GCodeSele.Size = new System.Drawing.Size(971, 389);
            this.dataGridView_GCodeSele.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_AddGCode);
            this.splitContainer1.Panel1.Controls.Add(this.button_DowLoadGCode);
            this.splitContainer1.Panel1.Controls.Add(this.button_DeletGCode);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(1199, 797);
            this.splitContainer1.SplitterDistance = 215;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.dataGridView_CNCGCode, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dataGridView_GCodeSele, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(979, 797);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // timercncstate
            // 
            this.timercncstate.Enabled = true;
            this.timercncstate.Interval = 2000;
            // 
            // UserControlTaskData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UserControlTaskData";
            this.Size = new System.Drawing.Size(1199, 797);
            this.Load += new System.EventHandler(this.UserControlTaskData_Loadv2);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CNCGCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_GCodeSele)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_CNCGCode;
        private System.Windows.Forms.Button button_DowLoadGCode;
        private System.Windows.Forms.DataGridView dataGridView_GCodeSele;
        private System.Windows.Forms.Button button_DeletGCode;
        private System.Windows.Forms.Button button_AddGCode;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Timer timercncstate;
    }
}
