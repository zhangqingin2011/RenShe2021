﻿namespace SCADA
{
    partial class FormSetNumber
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
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cans = new System.Windows.Forms.Button();
            this.textBox_Number = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(28, 84);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(94, 41);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cans
            // 
            this.button_Cans.Location = new System.Drawing.Point(187, 84);
            this.button_Cans.Name = "button_Cans";
            this.button_Cans.Size = new System.Drawing.Size(94, 41);
            this.button_Cans.TabIndex = 0;
            this.button_Cans.Text = "取消";
            this.button_Cans.UseVisualStyleBackColor = true;
            this.button_Cans.Click += new System.EventHandler(this.button_Cans_Click);
            // 
            // textBox_Number
            // 
            this.textBox_Number.Location = new System.Drawing.Point(93, 30);
            this.textBox_Number.Name = "textBox_Number";
            this.textBox_Number.Size = new System.Drawing.Size(125, 21);
            this.textBox_Number.TabIndex = 1;
            // 
            // FormSetNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 138);
            this.Controls.Add(this.textBox_Number);
            this.Controls.Add(this.button_Cans);
            this.Controls.Add(this.button_OK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetNumber";
            this.Text = "FormSetNumber";
            this.Load += new System.EventHandler(this.FormSetNumber_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cans;
        private System.Windows.Forms.TextBox textBox_Number;
    }
}