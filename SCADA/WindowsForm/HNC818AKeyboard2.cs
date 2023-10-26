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
    public partial class HNC818AKeyboard2 : UserControl
    {
        public HNC818AKeyboard2()
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
            fbtnProtectDoorCNC.Text = ChangeLanguage.GetString("btnProtectDoor");
            fbtnCoolingCNC.Text = ChangeLanguage.GetString("btnCooling");
            fbtnLubricationCNC.Text = ChangeLanguage.GetString("btnLubrication");
            fbtnchaochenjiechuCNC.Text = ChangeLanguage.GetString("btnOverRemove");
            fbtnkapansongjin.Text = ChangeLanguage.GetString("btnTightChuck");
            fbtnweitaisongjin.Text = ChangeLanguage.GetString("btnTailLoose");
            fbtnFeedHoldRunCNC.Text = ChangeLanguage.GetString("btnFeedHold");
            fToolchangemanual.Text = ChangeLanguage.GetString("btnToolChange");
            fbutton3AxsHome.Text = ChangeLanguage.GetString("btnThreeAxisHome");
            fbtnToolHome.Text = ChangeLanguage.GetString("btnToolHome");
        }

        public void ClearState()
        {
            btnBackColorSet(ref fbtnkapansongjin, 0);
            btnBackColorSet(ref fbtnweitaisongjin, 0);
            btnBackColorSet(ref fbtnProtectDoorCNC, 0);
            btnBackColorSet(ref fbtnyeyaqidong, 0);
            btnBackColorSet(ref fbtnFeedHoldRunCNC, 0);
            btnBackColorSet(ref fToolchangemanual, 0);
            btnBackColorSet(ref fjingei1, 0);
            btnBackColorSet(ref fjingei25, 0);
            btnBackColorSet(ref fjingei50, 0);
            btnBackColorSet(ref fjingei100, 0);
            btnBackColorSet(ref fbtnF1, 0);
            btnBackColorSet(ref fbtnF2, 0);
            btnBackColorSet(ref fbtnF3, 0);
            btnBackColorSet(ref fbtnF4, 0);
            btnBackColorSet(ref fbtnCoolingCNC, 0);
            btnBackColorSet(ref fbtnLubricationCNC, 0);
            btnBackColorSet(ref fbtnchaochenjiechuCNC, 0);
        }

        public void BtnY480Set(int tmp)
        {
            btnBackColorSet(ref fbtnkapansongjin, tmp & 0x0020);//卡盘松紧,Y480.5a
            btnBackColorSet(ref fbtnweitaisongjin, tmp & 0x0040);//尾台松紧,Y480.6a
        }

        public void BtnY481Set(int tmp)
        {
            btnBackColorSet(ref fbtnProtectDoorCNC, tmp & 0x0010);//防护门,Y481.4a
            btnBackColorSet(ref fbtnyeyaqidong, tmp & 0x0020);//液压启动,Y481.5a
            btnBackColorSet(ref fbtnFeedHoldRunCNC, tmp & 0x0040);//进给保持,Y481.6
            btnBackColorSet(ref fToolchangemanual, tmp & 0x0080);//手动换刀,Y481.7a
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
            //btnBackColorSet(ref fbtnToolHome, tmp & 0x0001);//刀库回零,Y484.0
            // btnBackColorSet(ref fbutton3AxsHome, tmp & 0x0002);//三轴回零,Y484.0
        }

        public void BtnY485Set(int tmp)
        {
            btnBackColorSet(ref fbtnchaochenjiechuCNC, tmp & 0x0004);//解除超程,Y485.2
            //Y486.4循环启动
        }

        public void BtnY486Set(int tmp)
        {
            
        }
    }
}
