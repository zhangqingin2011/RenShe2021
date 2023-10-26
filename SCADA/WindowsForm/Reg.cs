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
    public partial class Reg : Form
    {
        public String RegMunber;
        public String RegSet;
        public Reg()
        {
            InitializeComponent();
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            RegSet = textBox_RegSet.Text;
            this.Close();
        }

        private void Reg_Load(object sender, EventArgs e)
        {
            textBox_RegMunber.Text = RegMunber;
        }
    }
}
