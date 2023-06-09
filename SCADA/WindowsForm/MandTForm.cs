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
    public partial class MandTForm : Form
    {
        AutoSizeFormClass asc = new AutoSizeFormClass();
        private static Form[] m_Formarre1 = null;

        public MandTForm()
        {
            InitializeComponent();
            m_Formarre1 = new Form[tabControl1.TabCount];

            GenerateForm(0, tabControl1);
        }

        public void GenerateForm(int form_index, object sender)
        {
            // 反射生成窗体//只生成一次
            if (m_Formarre1[form_index] == null && form_index >= 0 && form_index < tabControl1.TabCount)
            {
                string formClassSTR = ((TabControl)sender).SelectedTab.Tag.ToString();
                m_Formarre1[form_index] = (Form)Assembly.GetExecutingAssembly().CreateInstance(formClassSTR);
                // m_Formarr[10 ] = { "SCADA.VideoForm, Text: VideoForm"};
                if (m_Formarre1[form_index] != null)
                {
                    //设置窗体没有边框 加入到选项卡中
                    m_Formarre1[form_index].FormBorderStyle = FormBorderStyle.None;
                    m_Formarre1[form_index].TopLevel = false;
                    m_Formarre1[form_index].Parent = ((TabControl)sender).SelectedTab;
                    m_Formarre1[form_index].ControlBox = false;
                    m_Formarre1[form_index].Dock = DockStyle.Fill;
                    if (ChangeLanguage.defaultcolor != Color.White)
                    {
                        ChangeLanguage.LoadSkin(m_Formarre1[form_index], ChangeLanguage.defaultcolor);
                    }
                    m_Formarre1[form_index].Show();
                }
            }

        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            GenerateForm(tabControl1.SelectedIndex, sender);
        }

        private void MandTForm_SizeChanged(object sender, EventArgs e)
        {

           // asc.controlAutoSize(this);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Image backImage = Properties.Resources.whiteBack;
            Rectangle rec = tabControl1.ClientRectangle;

            StringFormat StrFormat = new StringFormat();

            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;

            SolidBrush tabBackBrush = new SolidBrush(Color.DarkGray);
            //文字色
            SolidBrush FrontBrush = new SolidBrush(Color.White);
            //StringFormat stringF = new StringFormat();
            Font wordfont = new Font("微软雅黑", 12F);
            e.Graphics.DrawImage(backImage, 0, 0, tabControl1.Width, tabControl1.Height);
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                //标签工作区
                Rectangle rec1 = tabControl1.GetTabRect(i);
                //e.Graphics.DrawImage(backImage, 0, 0, tabPagemain.Width, tabPagemain.Height);
                e.Graphics.FillRectangle(tabBackBrush, rec1);
                ////标签头背景色
                e.Graphics.DrawString(tabControl1.TabPages[i].Text, wordfont, FrontBrush, rec1, StrFormat);
                ////标签头文字
            }
        }
    }
}
