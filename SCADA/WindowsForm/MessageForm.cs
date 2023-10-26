﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA.WindowsForm
{
    public partial class MessageForm : Form
    {
        public MessageForm(string title, string Contenttext, string OK, string Cancel)
        {
            InitializeComponent1();
            this.Text = title;
            label1.Text = Contenttext;
            button1.Text = OK;
            button2.Text = Cancel;
        }

        public MessageForm(string title, string Contenttext, string OK)
        {
            InitializeComponent1();
            this.Text = title;
            label1.Text = Contenttext;
            button1.Visible = false;
            button2.Text = OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
