﻿namespace SCADA
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labeltype = new System.Windows.Forms.Label();
            this.buttondingdanquxiao = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelmagno = new System.Windows.Forms.Label();
            this.buttondingdanxiada = new System.Windows.Forms.Button();
            this.comboBoxtype = new System.Windows.Forms.ComboBox();
            this.textBoxMagno = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.timercncstate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CNCGCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_GCodeSele)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_DeletGCode
            // 
            this.button_DeletGCode.Location = new System.Drawing.Point(61, 91);
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
            this.button_AddGCode.Location = new System.Drawing.Point(61, 5);
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
            this.button_DowLoadGCode.Location = new System.Drawing.Point(61, 156);
            this.button_DowLoadGCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_DowLoadGCode.Name = "button_DowLoadGCode";
            this.button_DowLoadGCode.Size = new System.Drawing.Size(97, 55);
            this.button_DowLoadGCode.TabIndex = 1;
            this.button_DowLoadGCode.Text = "订单派发";
            this.button_DowLoadGCode.UseVisualStyleBackColor = true;
            this.button_DowLoadGCode.Click += new System.EventHandler(this.button_DowLoadGCode_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labeltype);
            this.panel1.Controls.Add(this.buttondingdanquxiao);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelmagno);
            this.panel1.Controls.Add(this.buttondingdanxiada);
            this.panel1.Controls.Add(this.comboBoxtype);
            this.panel1.Controls.Add(this.textBoxMagno);
            this.panel1.Location = new System.Drawing.Point(0, 238);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 442);
            this.panel1.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(87, 304);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 21);
            this.label5.TabIndex = 12;
            this.label5.Text = "text2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 268);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 21);
            this.label4.TabIndex = 11;
            this.label4.Text = "text1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 304);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 21);
            this.label3.TabIndex = 10;
            this.label3.Text = "加工中心:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 268);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 21);
            this.label2.TabIndex = 9;
            this.label2.Text = "车床:";
            // 
            // labeltype
            // 
            this.labeltype.AutoSize = true;
            this.labeltype.Location = new System.Drawing.Point(70, 91);
            this.labeltype.Name = "labeltype";
            this.labeltype.Size = new System.Drawing.Size(78, 21);
            this.labeltype.TabIndex = 8;
            this.labeltype.Text = "加工类型:";
            // 
            // buttondingdanquxiao
            // 
            this.buttondingdanquxiao.Location = new System.Drawing.Point(44, 161);
            this.buttondingdanquxiao.Name = "buttondingdanquxiao";
            this.buttondingdanquxiao.Size = new System.Drawing.Size(121, 36);
            this.buttondingdanquxiao.TabIndex = 7;
            this.buttondingdanquxiao.Text = "撤销订单";
            this.buttondingdanquxiao.UseVisualStyleBackColor = true;
            this.buttondingdanquxiao.Click += new System.EventHandler(this.buttondingdanquxiao_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "加工订单:";
            // 
            // labelmagno
            // 
            this.labelmagno.AutoSize = true;
            this.labelmagno.Location = new System.Drawing.Point(70, 25);
            this.labelmagno.Name = "labelmagno";
            this.labelmagno.Size = new System.Drawing.Size(78, 21);
            this.labelmagno.TabIndex = 2;
            this.labelmagno.Text = "料仓编号:";
            // 
            // buttondingdanxiada
            // 
            this.buttondingdanxiada.Location = new System.Drawing.Point(44, 203);
            this.buttondingdanxiada.Name = "buttondingdanxiada";
            this.buttondingdanxiada.Size = new System.Drawing.Size(121, 36);
            this.buttondingdanxiada.TabIndex = 6;
            this.buttondingdanxiada.Text = "生成订单";
            this.buttondingdanxiada.UseVisualStyleBackColor = true;
            this.buttondingdanxiada.Click += new System.EventHandler(this.buttondingdanxiada_Click);
            // 
            // comboBoxtype
            // 
            this.comboBoxtype.FormattingEnabled = true;
            this.comboBoxtype.Location = new System.Drawing.Point(23, 115);
            this.comboBoxtype.Name = "comboBoxtype";
            this.comboBoxtype.Size = new System.Drawing.Size(158, 29);
            this.comboBoxtype.TabIndex = 4;
            // 
            // textBoxMagno
            // 
            this.textBoxMagno.Location = new System.Drawing.Point(23, 49);
            this.textBoxMagno.Name = "textBoxMagno";
            this.textBoxMagno.Size = new System.Drawing.Size(158, 29);
            this.textBoxMagno.TabIndex = 3;
            this.textBoxMagno.Text = "1";
            this.textBoxMagno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMagno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMagno_KeyPress);
            this.textBoxMagno.Leave += new System.EventHandler(this.textBoxMagno_Leave);
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
            this.timercncstate.Tick += new System.EventHandler(this.timercncstate_Tick);
            // 
            // UserControlTaskData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UserControlTaskData";
            this.Size = new System.Drawing.Size(1199, 797);
            this.Load += new System.EventHandler(this.UserControlTaskData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CNCGCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_GCodeSele)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.TextBox textBoxMagno;
        private System.Windows.Forms.Label labelmagno;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxtype;
        private System.Windows.Forms.Button buttondingdanxiada;
        private System.Windows.Forms.Button buttondingdanquxiao;
        private System.Windows.Forms.Label labeltype;
        private System.Windows.Forms.Label label5;
       private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timercncstate;
    }
}
