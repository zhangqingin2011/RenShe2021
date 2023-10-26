﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA
{
    public partial class BoardForm : Form
    {
        public int choosedid = 0;
        public BoardForm()
        {
            InitializeComponent();
        }
       
       

        private void pictureBoxmain_Click(object sender, EventArgs e)
        {
            choosedid = 1;
            this.pictureBoxmain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表           
        }

        private void pictureBoxbin_Click(object sender, EventArgs e)
        {
            choosedid = 2;
            this.pictureBoxbin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表  

        }

        private void pictureBoxstats_Click(object sender, EventArgs e)
        {
            choosedid = 3;
            this.pictureBoxstats.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表  

        }

        private void pictureBoxlathe_Click(object sender, EventArgs e)
        {
            choosedid = 4;
            this.pictureBoxlathe.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表  
        }

        private void pictureBoxcnc_Click(object sender, EventArgs e)
        {
            choosedid = 5;
            this.pictureBoxcnc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表  
        }

        private void pictureBoxrobot_Click(object sender, EventArgs e)
        {
            choosedid = 6;
            this.pictureBoxrobot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表  

        }

        private void pictureBoxmeter_Click(object sender, EventArgs e)
        {
            choosedid = 7;
            this.pictureBoxmeter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表  
        }

        private void pictureBoxtool_Click(object sender, EventArgs e)
        {
            choosedid = 8;
            this.pictureBoxtool.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表  
        }

        private void pictureBoxuser_Click(object sender, EventArgs e)
        {
            choosedid = 9;
            this.pictureBoxuser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picturechoosedchange();
            //写入数据表  
        }
        private void picturechoosedchange()
        {
            
            if (choosedid != 1 && this.pictureBoxmain.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxmain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
            if (choosedid != 2 && this.pictureBoxbin.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxbin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
            if (choosedid != 3 && this.pictureBoxstats.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxstats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
            if (choosedid != 4 && this.pictureBoxlathe.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxlathe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
            if (choosedid != 5 && this.pictureBoxcnc.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxcnc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
            if (choosedid != 6 && this.pictureBoxrobot.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxrobot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
            if (choosedid != 7 && this.pictureBoxmeter.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxmeter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
            if (choosedid != 8 && this.pictureBoxtool.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxtool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
            if (choosedid != 9 && this.pictureBoxuser.BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                this.pictureBoxuser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; 
            }
        }
    }
}
