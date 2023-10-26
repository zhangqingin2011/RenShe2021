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
    public partial class HNC818BKeyboard2 : UserControl
    {
        public HNC818BKeyboard2()
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

        public void LoadLanguage()
        {
            fjingei1.Text = ChangeLanguage.GetString("btnFeed1");
            fjingei25.Text = ChangeLanguage.GetString("btnFeed25");
            fjingei50.Text = ChangeLanguage.GetString("btnFeed50");
            fjingei100.Text = ChangeLanguage.GetString("btnFeed100");
            fbtnhuandaoyunxu.Text = ChangeLanguage.GetString("btnSwichPermit");
            fbtndaojusongjin.Text = ChangeLanguage.GetString("btnToolCutter");
            fbtnProtectDoorCNC.Text = ChangeLanguage.GetString("btnProtectDoor");
            fbtngongzuodeng.Text = ChangeLanguage.GetString("btnWorkLight");
            fbtndaokuzhengzhuan.Text = ChangeLanguage.GetString("btnToolForward");
            fbtndaokufanzhuan.Text = ChangeLanguage.GetString("btnToolReversal");
            fbuttonchuixie.Text = ChangeLanguage.GetString("btnBlowingDust");
            btnzidongduandian.Text = ChangeLanguage.GetString("btnAutoShutdown");
            fbtnCoolingCNC.Text = ChangeLanguage.GetString("btnCooling");
            fbtnLubricationCNC.Text = ChangeLanguage.GetString("btnLubrication");
            fbtnchaochenjiechuCNC.Text = ChangeLanguage.GetString("btnOverRemove");
        }

        public void ClearState()
        {
            btnBackColorSet(ref fbtnhuandaoyunxu, 0);
            btnBackColorSet(ref fbtndaojusongjin, 0);  
            btnBackColorSet(ref fbtndaokuzhengzhuan, 0);
            btnBackColorSet(ref fbtndaokufanzhuan, 0);
            btnBackColorSet(ref fjingei1, 0);
            btnBackColorSet(ref fjingei25, 0);
            btnBackColorSet(ref fbtngongzuodeng, 0);
            btnBackColorSet(ref fjingei50, 0);
            btnBackColorSet(ref fjingei100, 0);
            btnBackColorSet(ref fbtnLubricationCNC, 0);
            btnBackColorSet(ref fbuttonchuixie, 0);
            btnBackColorSet(ref btnzidongduandian, 0);
            btnBackColorSet(ref fbtnF1, 0);
            btnBackColorSet(ref fbtnF2, 0);
            btnBackColorSet(ref fbtnF3, 0);
            btnBackColorSet(ref fbtnF4, 0);
            btnBackColorSet(ref fbtnchaochenjiechuCNC, 0);
            btnBackColorSet(ref fbtnCoolingCNC, 0);
            btnBackColorSet(ref fbtnProtectDoorCNC, 0);
        }

        public void BtnY480Set(int tmp)
        {
            btnBackColorSet(ref fbtnhuandaoyunxu, tmp & 0x0020);//换刀允许, Y480.5
            btnBackColorSet(ref fbtndaojusongjin, tmp & 0x0040);//刀具松/紧, Y480.6            
        }

        public void BtnY481Set(int tmp)
        {
            btnBackColorSet(ref fbtndaokuzhengzhuan, tmp & 0x0040);//刀库正转, Y481.6
            btnBackColorSet(ref fbtndaokufanzhuan, tmp & 0x0080);//刀库反转, Y481.7
        }

        public void BtnY482Set(int tmp)
        {
            btnBackColorSet(ref fjingei1, tmp & 0x0008);//进给1, Y482.3
            btnBackColorSet(ref fjingei25, tmp & 0x0010);//进给25, Y482.4
        }

        public void BtnY483Set(int tmp)
        {
            btnBackColorSet(ref fbtngongzuodeng, tmp & 0x0001);//工作灯, Y483.0
            btnBackColorSet(ref fjingei50, tmp & 0x0010);//进给50, Y483.4
            btnBackColorSet(ref fjingei100, tmp & 0x0020);//进给100, Y483.5

        }

        public void BtnY484Set(int tmp)
        {
            btnBackColorSet(ref fbtnProtectDoorCNC, tmp & 0x0002);//防护门, Y483.1
            btnBackColorSet(ref fbtnF1, tmp & 0x0020);//F1, Y483.5
            btnBackColorSet(ref fbtnF2, tmp & 0x0040);//F2, Y483.6
            btnBackColorSet(ref fbtnCoolingCNC, tmp & 0x0080);//冷却, Y483.7

        }

        public void BtnY485Set(int tmp)
        {
            btnBackColorSet(ref fbtnLubricationCNC, tmp & 0x0001);//润滑, Y485.0
            btnBackColorSet(ref fbuttonchuixie, tmp & 0x0002);//吹屑, Y485.1
            btnBackColorSet(ref btnzidongduandian, tmp & 0x0004);//自动断电, Y485.2
            btnBackColorSet(ref fbtnF3, tmp & 0x0040);//F3, Y485.6
            btnBackColorSet(ref fbtnF4, tmp & 0x0080);//F4, Y485.7
        }

        public void BtnY486Set(int tmp)
        {
            btnBackColorSet(ref fbtnchaochenjiechuCNC, tmp & 0x0008);//超程解除, Y486.3           
        }
    }
}