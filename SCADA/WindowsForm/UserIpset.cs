﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace SCADA.WindowsForm
{
    public partial class UserIpset : UserControl
    {
        public UserIpset()
        {
            InitializeComponent();
        }

        public UserIpset(string name)
            : this()
        {
            label1.Text = name;
        }

        public void SetIPName(string name)
        {
            label1.Text = name;
        }

        public void SetIPAdress(int[] name)
        {
            textBox1.Text = name[0].ToString();
            textBox2.Text = name[1].ToString();
            textBox3.Text = name[2].ToString();
            textBox4.Text = name[3].ToString();
        }

        public string IP
        {
            get
            {
                if (textBox1.TextLength == 0 || textBox2.TextLength == 0 || textBox3.TextLength == 0 || textBox4.TextLength == 0)
                {
                    return string.Empty;
                }
                return textBox1.Text + "." + textBox2.Text + "." + textBox3.Text + "." + textBox4.Text;
            }
        }

        private void UserIpset_Load(object sender, EventArgs e)
        {
            textBox1.KeyPress += textBox_KeyPress;
            textBox2.KeyPress += textBox_KeyPress;
            textBox3.KeyPress += textBox_KeyPress;
            textBox4.KeyPress += textBox_KeyPress;
            textBox1.Leave += textBox_Leave;
            textBox2.Leave += textBox_Leave;
            textBox3.Leave += textBox_Leave;
            textBox4.Leave += textBox_Leave;
        }

        void textBox_Leave(object sender, EventArgs e)
        {
            string str = ((TextBox)sender).Text;
            if (str == "")
            {
                return;
            }
            try
            {
                int shebeiNo = Convert.ToInt32(str);
                if (shebeiNo > 255 || shebeiNo <0)
                {
                     MessageBox.Show("请输入0-255");
                     (sender as TextBox).Focus();
                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("请输入0-255");
                (sender as TextBox).Focus();
            }
        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {

                MessageBox.Show("请输入0-9数字");
                // Message.Show("请输入数字或者'.'");
                (sender as TextBox).Focus();
            }
        }

       

    }
}
