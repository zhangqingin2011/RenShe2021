namespace SCADA
{
    partial class EquipForm
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
            this.tabPage21 = new System.Windows.Forms.TabPage();
            this.tabPage31 = new System.Windows.Forms.TabPage();
            this.tabPage41 = new System.Windows.Forms.TabPage();
            this.tabPage51 = new System.Windows.Forms.TabPage();
            this.tabControlEQ1 = new System.Windows.Forms.TabControl();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.tabPage61 = new System.Windows.Forms.TabPage();
            this.tabControlEQ1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage21
            // 
            this.tabPage21.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tabPage21.Location = new System.Drawing.Point(4, 44);
            this.tabPage21.Name = "tabPage21";
            this.tabPage21.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage21.Size = new System.Drawing.Size(1176, 584);
            this.tabPage21.TabIndex = 1;
            this.tabPage21.Tag = "SCADA.RobortForm";
            this.tabPage21.Text = "机器人";
            this.tabPage21.UseVisualStyleBackColor = true;
            this.tabPage21.Visible = false;
            // 
            // tabPage31
            // 
            this.tabPage31.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tabPage31.Location = new System.Drawing.Point(4, 44);
            this.tabPage31.Name = "tabPage31";
            this.tabPage31.Size = new System.Drawing.Size(1176, 584);
            this.tabPage31.TabIndex = 2;
            this.tabPage31.Tag = "SCADA.RackForm";
            this.tabPage31.Text = "料仓";
            this.tabPage31.UseVisualStyleBackColor = true;
            this.tabPage31.Visible = false;
            // 
            // tabPage41
            // 
            this.tabPage41.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tabPage41.Location = new System.Drawing.Point(4, 44);
            this.tabPage41.Name = "tabPage41";
            this.tabPage41.Size = new System.Drawing.Size(1176, 584);
            this.tabPage41.TabIndex = 3;
            this.tabPage41.Tag = "SCADA.VideoForm";
            this.tabPage41.Text = "监视";
            this.tabPage41.UseVisualStyleBackColor = true;
            this.tabPage41.Visible = false;
            // 
            // tabPage51
            // 
            this.tabPage51.Location = new System.Drawing.Point(4, 44);
            this.tabPage51.Name = "tabPage51";
            this.tabPage51.Size = new System.Drawing.Size(1176, 584);
            this.tabPage51.TabIndex = 4;
            this.tabPage51.Tag = "SCADA.EquipmentCheckForm";
            this.tabPage51.Text = "报警";
            this.tabPage51.UseVisualStyleBackColor = true;
            // 
            // tabControlEQ1
            // 
            this.tabControlEQ1.Controls.Add(this.tabPage31);
            this.tabControlEQ1.Controls.Add(this.tabPage11);
            this.tabControlEQ1.Controls.Add(this.tabPage21);
            this.tabControlEQ1.Controls.Add(this.tabPage41);
            this.tabControlEQ1.Controls.Add(this.tabPage61);
            this.tabControlEQ1.Controls.Add(this.tabPage51);
            this.tabControlEQ1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEQ1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlEQ1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tabControlEQ1.ItemSize = new System.Drawing.Size(100, 40);
            this.tabControlEQ1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEQ1.Multiline = true;
            this.tabControlEQ1.Name = "tabControlEQ1";
            this.tabControlEQ1.SelectedIndex = 0;
            this.tabControlEQ1.Size = new System.Drawing.Size(1184, 632);
            this.tabControlEQ1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlEQ1.TabIndex = 0;
            this.tabControlEQ1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControlEQ1_DrawItem);
            this.tabControlEQ1.SelectedIndexChanged += new System.EventHandler(this.tabControlEQ_SelectedIndexChanged);
            // 
            // tabPage11
            // 
            this.tabPage11.Location = new System.Drawing.Point(4, 44);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(1176, 584);
            this.tabPage11.TabIndex = 5;
            this.tabPage11.Tag = "SCADA.CncForm";
            this.tabPage11.Text = "机床";
            this.tabPage11.UseVisualStyleBackColor = true;
            this.tabPage11.SizeChanged += new System.EventHandler(this.tabPage11_SizeChanged);
            // 
            // tabPage61
            // 
            this.tabPage61.Location = new System.Drawing.Point(4, 44);
            this.tabPage61.Name = "tabPage61";
            this.tabPage61.Size = new System.Drawing.Size(1176, 584);
            this.tabPage61.TabIndex = 6;
            this.tabPage61.Tag = "SCADA.AGVForm";
            this.tabPage61.Text = "AGV";
            this.tabPage61.UseVisualStyleBackColor = true;
            // 
            // EquipForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 632);
            this.Controls.Add(this.tabControlEQ1);
            this.Name = "EquipForm";
            this.Text = "EquipForm";
            this.Load += new System.EventHandler(this.EquipForm_Load);
            this.SizeChanged += new System.EventHandler(this.EquipForm_SizeChanged);
            this.tabControlEQ1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage21;
        private System.Windows.Forms.TabPage tabPage31;
        private System.Windows.Forms.TabPage tabPage41;
        private System.Windows.Forms.TabPage tabPage51;
        private System.Windows.Forms.TabControl tabControlEQ1;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.TabPage tabPage61;
    }
}