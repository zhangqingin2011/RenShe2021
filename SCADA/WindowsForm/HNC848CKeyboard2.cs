﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA
{
    public partial class HNC848CKeyboard2 : UserControl
    {
        public HNC848CKeyboard2()
        {
            InitializeComponent();
        }
        Color[] Bt_bgcoler = { System.Drawing.Color.FromArgb(255, 251, 240), System.Drawing.Color.FromArgb(0, 255, 0) };//按钮颜色
        private void btnBackColorSet(ref Button btn, int flag)
        {
            if (flag != 0)
            {
                flag = 1;
            }
            if (btn.BackColor != Bt_bgcoler[flag])
            {
                btn.BackColor = Bt_bgcoler[flag];
            }
        }
        public void ClearState()
        {
            btnBackColorSet(ref fjingei1, 0);
            btnBackColorSet(ref fjingei25, 0);
            btnBackColorSet(ref fjingei50, 0);
            btnBackColorSet(ref fjingei100, 0);
            btnBackColorSet(ref buttongongzuodeng, 0);
            btnBackColorSet(ref fbtnProtectDoorCNC, 0);
            btnBackColorSet(ref fbtnCoolingCNC, 0);
            btnBackColorSet(ref btnLubricationCNC, 0);
            btnBackColorSet(ref btnxhqdCNC, 0);
            btnBackColorSet(ref btnFeedHoldRunCNC, 0);
        }
        public void LoadLanguage()
        {
            fjingei1.Text = ChangeLanguage.GetString("btnFeed1");
            fjingei25.Text = ChangeLanguage.GetString("btnFeed25");
            fjingei50.Text = ChangeLanguage.GetString("btnFeed50");
            fjingei100.Text = ChangeLanguage.GetString("btnFeed100");
            buttongongzuodeng.Text = ChangeLanguage.GetString("btnWorkLight");
            fbtnProtectDoorCNC.Text = ChangeLanguage.GetString("btnProtectDoor");
            fbtnCoolingCNC.Text = ChangeLanguage.GetString("btnCooling");
            btnLubricationCNC.Text = ChangeLanguage.GetString("btnLubrication");
            btnxhqdCNC.Text = ChangeLanguage.GetString("btnCycleStart");
            btnFeedHoldRunCNC.Text = ChangeLanguage.GetString("btnFeedHold");
        }

        public void BtnY480Set(int tmp)
        {

        }

        public void BtnY481Set(int tmp)
        {
            btnBackColorSet(ref fjingei1, tmp & 0x0004);//进给1,Y481.2   
            btnBackColorSet(ref fjingei25, tmp & 0x0008);//进给25,Y481.3  
            btnBackColorSet(ref fjingei50, tmp & 0x0010);//进给50,Y481.4
            btnBackColorSet(ref fjingei100, tmp & 0x0020);//进给100,Y481.5
            btnBackColorSet(ref buttongongzuodeng, tmp & 0x0020);//工作灯,Y481.5           
        }

        public void BtnY482Set(int tmp)
        {
            btnBackColorSet(ref fbtnCoolingCNC, tmp & 0x0040);//冷却,Y482.6             
        }

        public void BtnY483Set(int tmp)
        {

        }

        public void BtnY484Set(int tmp)
        {

        }

        public void BtnY485Set(int tmp)
        {
            btnBackColorSet(ref btnLubricationCNC, tmp & 0x0001);//润滑,Y485.0

        }

        public void BtnY486Set(int tmp)
        {
            btnBackColorSet(ref btnxhqdCNC, tmp & 0x0010);//循环启动,Y486.4
            btnBackColorSet(ref btnFeedHoldRunCNC, tmp & 0x0020);//进给保持,Y486.5
        }

        public void BtnR29Set(int tmp)
        {

        }
    }
}
