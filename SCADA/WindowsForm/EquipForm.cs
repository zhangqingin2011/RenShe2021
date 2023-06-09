﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace SCADA
{
    public partial class EquipForm : Form
    {
        AutoSizeFormClass asc = new AutoSizeFormClass();

        private static Form[] m_Formarrequip = null;
        public EquipForm()
        {
            InitializeComponent();

        }
        //在选项卡中生成窗体
        public void GenerateForm(int form_index, object sender)
        {
            // 反射生成窗体//只生成一次
            if (m_Formarrequip[form_index] == null && form_index >= 0 && form_index < tabControlEQ1.TabCount)
            {
                string formClassSTR = ((TabControl)sender).SelectedTab.Tag.ToString();
                m_Formarrequip[form_index] = (Form)Assembly.GetExecutingAssembly().CreateInstance(formClassSTR);
                // m_Formarr[10 ] = { "SCADA.VideoForm, Text: VideoForm"};
                if (m_Formarrequip[form_index] != null)
                {
                    //设置窗体没有边框 加入到选项卡中
                    m_Formarrequip[form_index].FormBorderStyle = FormBorderStyle.None;
                    m_Formarrequip[form_index].TopLevel = false;
                    m_Formarrequip[form_index].Parent = ((TabControl)sender).SelectedTab;
                    m_Formarrequip[form_index].ControlBox = false;
                    m_Formarrequip[form_index].Dock = DockStyle.Fill;
                    if (ChangeLanguage.defaultcolor != Color.White)
                    {
                        ChangeLanguage.LoadSkin(m_Formarrequip[form_index], ChangeLanguage.defaultcolor);
                    }
                    m_Formarrequip[form_index].Show();

                }
            }

        }


       
        private void tabControlEQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateForm(tabControlEQ1.SelectedIndex, sender);
        }

        private void EquipForm_Load(object sender, EventArgs e)
        {

            m_Formarrequip = new Form[tabControlEQ1.TabCount];
            GenerateForm(0, tabControlEQ1);
        }

        private void tabPage11_SizeChanged(object sender, EventArgs e)
        {

           // asc.controlAutoSize(this);
        }

        private void EquipForm_SizeChanged(object sender, EventArgs e)
        {

            this.WindowState = (System.Windows.Forms.FormWindowState)(2);
        }

        private void tabControlEQ1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Image backImage = Properties.Resources.whiteBack;
            Rectangle rec = tabControlEQ1.ClientRectangle;

            StringFormat StrFormat = new StringFormat();

            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;

            SolidBrush tabBackBrush = new SolidBrush(Color.DarkGray);
            //文字色
            SolidBrush FrontBrush = new SolidBrush(Color.White);
            //StringFormat stringF = new StringFormat();
            Font wordfont = new Font("微软雅黑", 12F);
            e.Graphics.DrawImage(backImage, 0, 0, tabControlEQ1.Width, tabControlEQ1.Height);
            for (int i = 0; i < tabControlEQ1.TabCount; i++)
            {
                //标签工作区
                Rectangle rec1 = tabControlEQ1.GetTabRect(i);
                //e.Graphics.DrawImage(backImage, 0, 0, tabPagemain.Width, tabPagemain.Height);
                e.Graphics.FillRectangle(tabBackBrush, rec1);
                ////标签头背景色
                e.Graphics.DrawString(tabControlEQ1.TabPages[i].Text, wordfont, FrontBrush, rec1, StrFormat);
                ////标签头文字
            }
        }
    }
}
