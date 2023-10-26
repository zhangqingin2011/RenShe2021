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
    public partial class HNC848BKeyboard2 : UserControl
    {
        public HNC848BKeyboard2()
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
            btnBackColorSet(ref fbtnLubricationCNC, 0);
            btnBackColorSet(ref fbtnchaochenjiechuCNC, 0);
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
            fbtnLubricationCNC.Text = ChangeLanguage.GetString("btnLubrication");
            fbtnchaochenjiechuCNC.Text = ChangeLanguage.GetString("btnOverRemove");
        }

        public void BtnY480Set(int tmp)
        {

        }

        public void BtnY481Set(int tmp)
        {
            btnBackColorSet(ref fbtnProtectDoorCNC, tmp & 0x0010);//防护门,Y481.4a

        }

        public void BtnY482Set(int tmp)
        {
            btnBackColorSet(ref fjingei1, tmp & 0x0008);//进给1%,Y482.3a
            btnBackColorSet(ref fjingei25, tmp & 0x0010);//进给25%,Y482.4a
            btnBackColorSet(ref fjingei50, tmp & 0x0020);//进给50%,Y482.5a
            btnBackColorSet(ref fjingei100, tmp & 0x0040);//进给100%,Y482.6a
            btnBackColorSet(ref fbtnF1, tmp & 0x0080);//F1,Y482.7a
        }

        public void BtnY483Set(int tmp)
        {
            btnBackColorSet(ref fbtnF2, tmp & 0x0001);//F2,Y483.0

            btnBackColorSet(ref fbtnCoolingCNC, tmp & 0x0020);//冷却,Y483.5a
            btnBackColorSet(ref fbtnLubricationCNC, tmp & 0x0040);//润滑,Y483.6a
        }

        public void BtnY484Set(int tmp)
        {

        }

        public void BtnY485Set(int tmp)
        {
            btnBackColorSet(ref fbtnchaochenjiechuCNC, tmp & 0x0004);//解除超程,Y485.2a

        }

        public void BtnY486Set(int tmp)
        {

        }
    }
}
