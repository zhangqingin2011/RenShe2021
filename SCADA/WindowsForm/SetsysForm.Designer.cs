﻿namespace SCADA
{
    partial class SetsysForm
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
            this.tabPageset = new System.Windows.Forms.TabPage();
            this.tabPagelog = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPagetest = new System.Windows.Forms.TabPage();
            this.tabControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageset
            // 
            this.tabPageset.Location = new System.Drawing.Point(4, 44);
            this.tabPageset.Name = "tabPageset";
            this.tabPageset.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageset.Size = new System.Drawing.Size(1176, 614);
            this.tabPageset.TabIndex = 0;
            this.tabPageset.Tag = "SCADA.SetForm";
            this.tabPageset.Text = "设置";
            this.tabPageset.UseVisualStyleBackColor = true;
            // 
            // tabPagelog
            // 
            this.tabPagelog.Location = new System.Drawing.Point(4, 34);
            this.tabPagelog.Name = "tabPagelog";
            this.tabPagelog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagelog.Size = new System.Drawing.Size(1176, 624);
            this.tabPagelog.TabIndex = 1;
            this.tabPagelog.Tag = "SCADA.LogForm";
            this.tabPagelog.Text = "日志";
            this.tabPagelog.UseVisualStyleBackColor = true;
            this.tabPagelog.Visible = false;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageset);
            this.tabControl2.Controls.Add(this.tabPagetest);
            this.tabControl2.Controls.Add(this.tabPagelog);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl2.ItemSize = new System.Drawing.Size(100, 40);
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1184, 662);
            this.tabControl2.TabIndex = 1;
            this.tabControl2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl2_DrawItem);
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPagetest
            // 
            this.tabPagetest.Location = new System.Drawing.Point(4, 34);
            this.tabPagetest.Name = "tabPagetest";
            this.tabPagetest.Size = new System.Drawing.Size(1176, 624);
            this.tabPagetest.TabIndex = 2;
            this.tabPagetest.Tag = "SCADA.TestForm";
            this.tabPagetest.Text = "验证";
            this.tabPagetest.UseVisualStyleBackColor = true;
            // 
            // SetsysForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 662);
            this.Controls.Add(this.tabControl2);
            this.Name = "SetsysForm";
            this.Text = "SetsysForm";
            this.Load += new System.EventHandler(this.SetsysForm_Load);
            this.SizeChanged += new System.EventHandler(this.SetsysForm_SizeChanged);
            this.tabControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPageset;
        private System.Windows.Forms.TabPage tabPagelog;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPagetest;
    }
}