﻿namespace SCADA
{
    partial class TaskDataForm
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
            this.panelTaskData = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelTaskData
            // 
            this.panelTaskData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTaskData.Location = new System.Drawing.Point(0, 0);
            this.panelTaskData.Name = "panelTaskData";
            this.panelTaskData.Size = new System.Drawing.Size(1266, 617);
            this.panelTaskData.TabIndex = 0;
            // 
            // TaskDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1496, 722);
            this.Controls.Add(this.panelTaskData);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TaskDataForm";
            this.Text = "派发任务";
            this.Load += new System.EventHandler(this.TaskDataForm_Load);
            this.SizeChanged += new System.EventHandler(this.TaskDataForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTaskData;
    }
}
