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
    public partial class HNC818AKeyboard1 : UserControl
    {
        public HNC818AKeyboard1()
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
            fbtnAuto.Text = ChangeLanguage.GetString("btnAuto");
            fbtnSingleSegmentCNC.Text = ChangeLanguage.GetString("btnSingle");
            fbtnManualCNC.Text = ChangeLanguage.GetString("btnManual");
            fbtnRunIncrementCNC.Text = ChangeLanguage.GetString("btn⁯Increment");
            fbtnReZeroCNC.Text = ChangeLanguage.GetString("btn⁯ReZero");
            fbtnkongrunCNC.Text = ChangeLanguage.GetString("btn⁯⁯⁯AirRun");
            fbtnGCSkipCNC.Text = ChangeLanguage.GetString("btn⁯⁯ProgramSkip");
            fbtnxuanzhetingCNC.Text = ChangeLanguage.GetString("btn⁯⁯⁯ChoiceStop");
            fbtnZsuoCNC.Text = ChangeLanguage.GetString("btnMSTLock");
            fbtnjichuangshuoCNC.Text = ChangeLanguage.GetString("btnCNCLock");
            fbtnkuaijingCNC.Text = ChangeLanguage.GetString("btnFast");
            fbtnSpindleForwardCNC.Text = ChangeLanguage.GetString("btnSpindleForward");
            fbtnzhuzhoufanzhuangCNC.Text = ChangeLanguage.GetString("btnSpindleReversal");
            fbtnzhuzhoudiandongCNC.Text = ChangeLanguage.GetString("btnSpindleMove");
            fbtnSpindleStopCNC.Text = ChangeLanguage.GetString("btnSpindleStop");
            fbtnzhuzhoujiangdangCNC.Text = ChangeLanguage.GetString("btnSpindleDown");
            fbtnzhuzhoushengdangCNC.Text = ChangeLanguage.GetString("btnSpindleLift");
        }

        public void ClearState()
        {
            btnBackColorSet(ref fbtnAuto, 0);
            btnBackColorSet(ref fbtnSingleSegmentCNC, 0);
            btnBackColorSet(ref fbtnManualCNC, 0);
            btnBackColorSet(ref fbtnRunIncrementCNC, 0);
            btnBackColorSet(ref fbtnReZeroCNC, 0);
            btnBackColorSet(ref fbtnkongrunCNC, 0);
            btnBackColorSet(ref fbtnGCSkipCNC, 0);
            btnBackColorSet(ref fbtnxuanzhetingCNC, 0);
            btnBackColorSet(ref fbtnZsuoCNC, 0);
            btnBackColorSet(ref fbtnjichuangshuoCNC, 0);
            btnBackColorSet(ref fbtnsubxCNC, 0);
            btnBackColorSet(ref fbtnsubzCNC, 0);
            btnBackColorSet(ref fbtnkuaijingCNC, 0);
            btnBackColorSet(ref fbtnaddzCNC, 0);
            btnBackColorSet(ref fbtnzhuzhoudiandongCNC, 0);
            btnBackColorSet(ref fbtnzhuzhoushengdangCNC, 0);
            btnBackColorSet(ref fbtnaddxCNC, 0);
            btnBackColorSet(ref fbtnzhuzhoujiangdangCNC, 0);
            btnBackColorSet(ref fbtnSpindleForwardCNC, 0);
            btnBackColorSet(ref fbtnzhuzhoufanzhuangCNC, 0);
            btnBackColorSet(ref fbtnSpindleStopCNC, 0);         
        }

        public void BtnY480Set(int tmp)
        {
            btnBackColorSet(ref fbtnAuto, tmp & 0x0001);//自动,Y480.0a
            btnBackColorSet(ref fbtnSingleSegmentCNC, tmp & 0x0002);//单段,Y480.1a
            btnBackColorSet(ref fbtnManualCNC, tmp & 0x0004);//手动,Y480.2a
            btnBackColorSet(ref fbtnRunIncrementCNC, tmp & 0x0008);//增量,Y480.3a
            btnBackColorSet(ref fbtnReZeroCNC, tmp & 0x0010);//回参考点,Y480.4a

            btnBackColorSet(ref fbtnkongrunCNC, tmp & 0x0080);//空运行,Y480.7a
        }

        public void BtnY481Set(int tmp)
        {
            btnBackColorSet(ref fbtnGCSkipCNC, tmp & 0x0001);//程序跳段,Y481.0a
            btnBackColorSet(ref fbtnxuanzhetingCNC, tmp & 0x0002);//选择停,Y481.1a
            btnBackColorSet(ref fbtnZsuoCNC, tmp & 0x0004);//Z轴锁住,Y481.2a
            btnBackColorSet(ref fbtnjichuangshuoCNC, tmp & 0x0008);//机床锁住,Y481.3a
        }

        public void BtnY482Set(int tmp)
        {
            btnBackColorSet(ref fbtnsubxCNC, tmp & 0x0002);//-X,Y482.1a
        }

        public void BtnY483Set(int tmp)
        {
            btnBackColorSet(ref fbtnsubzCNC, tmp & 0x0002);//-Z,Y483.1a
            btnBackColorSet(ref fbtnkuaijingCNC, tmp & 0x0004);//快进,Y483.2a
            btnBackColorSet(ref fbtnaddzCNC, tmp & 0x0008);//+Z,Y483.3a
            btnBackColorSet(ref fbtnzhuzhoudiandongCNC, tmp & 0x0010);//主轴点动,Y483.4a
            btnBackColorSet(ref fbtnzhuzhoushengdangCNC, tmp & 0x0080);//主轴升档,Y483.7
            
        }

        public void BtnY484Set(int tmp)
        {
            btnBackColorSet(ref fbtnaddxCNC, tmp & 0x0008);//+X,Y484.3a
            btnBackColorSet(ref fbtnSpindleForwardCNC, tmp & 0x0020);//主轴正转,Y484.5a
            btnBackColorSet(ref fbtnSpindleStopCNC, tmp & 0x0040);//主轴停止,Y484.6a
            btnBackColorSet(ref fbtnzhuzhoufanzhuangCNC, tmp & 0x0080);//主轴反转,Y484.7a
            //btnBackColorSet(ref fbtnToolHome, tmp & 0x0001);//刀库回零,Y484.0
            // btnBackColorSet(ref fbutton3AxsHome, tmp & 0x0002);//三轴回零,Y484.0
        }

        public void BtnY485Set(int tmp)
        {
            btnBackColorSet(ref fbtnzhuzhoujiangdangCNC, tmp & 0x0001);//主轴降挡,Y485.0
            
        }

        public void BtnY486Set(int tmp)
        {

        }
    }
}
