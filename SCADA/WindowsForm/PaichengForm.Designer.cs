﻿namespace SCADA
{
    partial class PaichengForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageorder = new System.Windows.Forms.TabPage();
            this.tabPageaotoorder = new System.Windows.Forms.TabPage();
            this.tabPageprogram = new System.Windows.Forms.TabPage();
            this.tabPagewcs = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageorder);
            this.tabControl1.Controls.Add(this.tabPageaotoorder);
            this.tabControl1.Controls.Add(this.tabPageprogram);
            this.tabControl1.Controls.Add(this.tabPagewcs);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 40);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1184, 742);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageorder
            // 
            this.tabPageorder.Location = new System.Drawing.Point(4, 44);
            this.tabPageorder.Name = "tabPageorder";
            this.tabPageorder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageorder.Size = new System.Drawing.Size(1176, 694);
            this.tabPageorder.TabIndex = 0;
            this.tabPageorder.Tag = "SCADA.OrderForm1";
            this.tabPageorder.Text = "排程";
            this.tabPageorder.UseVisualStyleBackColor = true;
            // 
            // tabPageaotoorder
            // 
            this.tabPageaotoorder.Location = new System.Drawing.Point(4, 44);
            this.tabPageaotoorder.Name = "tabPageaotoorder";
            this.tabPageaotoorder.Size = new System.Drawing.Size(1176, 694);
            this.tabPageaotoorder.TabIndex = 2;
            this.tabPageaotoorder.Tag = "SCADA.AotoOrderForm";
            this.tabPageaotoorder.Text = "自动排程";
            this.tabPageaotoorder.UseVisualStyleBackColor = true;
            // 
            // tabPageprogram
            // 
            this.tabPageprogram.Location = new System.Drawing.Point(4, 44);
            this.tabPageprogram.Name = "tabPageprogram";
            this.tabPageprogram.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageprogram.Size = new System.Drawing.Size(1176, 694);
            this.tabPageprogram.TabIndex = 1;
            this.tabPageprogram.Tag = "SCADA.TaskDataForm";
            this.tabPageprogram.Text = "程序管理";
            this.tabPageprogram.UseVisualStyleBackColor = true;
            // 
            // tabPagewcs
            // 
            this.tabPagewcs.Location = new System.Drawing.Point(4, 44);
            this.tabPagewcs.Name = "tabPagewcs";
            this.tabPagewcs.Size = new System.Drawing.Size(1176, 694);
            this.tabPagewcs.TabIndex = 3;
            this.tabPagewcs.Tag = "SCADA.WCSForm";
            this.tabPagewcs.Text = "物料调度";
            this.tabPagewcs.UseVisualStyleBackColor = true;
            // 
            // PaichengForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 742);
            this.Controls.Add(this.tabControl1);
            this.Name = "PaichengForm";
            this.Text = "PaichengForm";
            this.SizeChanged += new System.EventHandler(this.PaichengForm_SizeChanged);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageorder;
        private System.Windows.Forms.TabPage tabPageprogram;
        private System.Windows.Forms.TabPage tabPageaotoorder;
        private System.Windows.Forms.TabPage tabPagewcs;
    }
}