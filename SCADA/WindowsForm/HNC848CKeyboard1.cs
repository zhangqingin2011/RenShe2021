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
    public partial class HNC848CKeyboard1 : UserControl
    {
        public HNC848CKeyboard1()
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
            btnBackColorSet(ref btnAuto, 0);
            btnBackColorSet(ref btnSingleSegmentCNC, 0);
            btnBackColorSet(ref btnReZeroCNC, 0);
            btnBackColorSet(ref btnkongrunCNC, 0);
            btnBackColorSet(ref btnManualCNC, 0);
            btnBackColorSet(ref btnGCSkipCNC, 0);
            btnBackColorSet(ref btnRunIncrementCNC, 0);
            btnBackColorSet(ref btnxuanzhetingCNC, 0);
            btnBackColorSet(ref btnchaochenjiechuCNC, 0);
            btnBackColorSet(ref btnjichuangshuoCNC, 0);
            btnBackColorSet(ref btnSpindleForwardCNC, 0);
            btnBackColorSet(ref btnSpindleStopCNC, 0);
            btnBackColorSet(ref btnzhuzhoufanzhuan, 0);
            btnBackColorSet(ref btnzhuzhoudingxiangCNC, 0);
            btnBackColorSet(ref btnzhuzhoudiandong, 0);
            btnBackColorSet(ref btnkuaijingCNC, 0);
        }
        public void LoadLanguage()
        {
            btnAuto.Text = ChangeLanguage.GetString("btnAuto");
            btnSingleSegmentCNC.Text = ChangeLanguage.GetString("btnSingle");
            btnReZeroCNC.Text = ChangeLanguage.GetString("btn⁯ReZero");
            btnkongrunCNC.Text = ChangeLanguage.GetString("btn⁯⁯⁯AirRun");
            btnManualCNC.Text = ChangeLanguage.GetString("btnManual");
            btnGCSkipCNC.Text = ChangeLanguage.GetString("btn⁯⁯ProgramSkip");
            btnRunIncrementCNC.Text = ChangeLanguage.GetString("btn⁯Increment");
            btnxuanzhetingCNC.Text = ChangeLanguage.GetString("btn⁯⁯⁯ChoiceStop");
            btnchaochenjiechuCNC.Text = ChangeLanguage.GetString("btnOverRemove");
            btnjichuangshuoCNC.Text = ChangeLanguage.GetString("btnCNCLock");
            btnSpindleForwardCNC.Text = ChangeLanguage.GetString("btnSpindleForward");
            btnSpindleStopCNC.Text = ChangeLanguage.GetString("btnSpindleStop");
            btnzhuzhoufanzhuan.Text = ChangeLanguage.GetString("btnSpindleReversal");
            btnzhuzhoudingxiangCNC.Text = ChangeLanguage.GetString("btnSpindleQrientation");
            btnzhuzhoudiandong.Text = ChangeLanguage.GetString("btnSpindleMove");
            btnkuaijingCNC.Text = ChangeLanguage.GetString("btnFast");
        }

        public void BtnY480Set(int tmp)
        {
            btnBackColorSet(ref btnAuto, tmp & 0x0001);//自动,Y480.0
            btnBackColorSet(ref btnReZeroCNC, tmp & 0x0002);//回参考点,Y480.1
            btnBackColorSet(ref btnManualCNC, tmp & 0x0004);//手动,Y480.2
            btnBackColorSet(ref btnRunIncrementCNC, tmp & 0x0008);//增量,Y480.3
            btnBackColorSet(ref btnchaochenjiechuCNC, tmp & 0x0008);//超程解除,Y480.4            
            btnBackColorSet(ref btnSingleSegmentCNC, tmp & 0x0020);//单段,Y480.5
            btnBackColorSet(ref btnkongrunCNC, tmp & 0x0040);//空运行,Y480.6
            btnBackColorSet(ref btnGCSkipCNC, tmp & 0x0080);//程序跳段,Y481.7
        }

        public void BtnY481Set(int tmp)
        {

            btnBackColorSet(ref btnxuanzhetingCNC, tmp & 0x0001);//选择停,Y481.0
            btnBackColorSet(ref btnjichuangshuoCNC, tmp & 0x0002);//机床锁住,Y481.1
            btnBackColorSet(ref btnSpindleForwardCNC, tmp & 0x0080);//主轴正转,Y481.7

        }

        public void BtnY482Set(int tmp)
        {
            btnBackColorSet(ref btnzhuzhoudingxiangCNC, tmp & 0x0001);//主轴定向,Y482.0
            btnBackColorSet(ref btnX, tmp & 0x0002);//X ,Y482.1
            btnBackColorSet(ref btnA, tmp & 0x0004);//A ,Y482.2
            btnBackColorSet(ref btnjian, tmp & 0x0010);//-  ,Y482.4            
            btnBackColorSet(ref btnSpindleStopCNC, tmp & 0x0020);//主轴停止,Y482.5
            btnBackColorSet(ref btnY, tmp & 0x0080);//Y  ,Y482.7
        }

        public void BtnY483Set(int tmp)
        {
            btnBackColorSet(ref btnB, tmp & 0x0001);//B ,Y483.0
            btnBackColorSet(ref btnkuaijingCNC, tmp & 0x0004);//快进,Y483.2
            btnBackColorSet(ref btnzhuzhoufanzhuan, tmp & 0x0008);//主轴反转,Y483.3      
            btnBackColorSet(ref btnzhuzhoudiandong, tmp & 0x0008);//主轴点动,Y483.4  
            btnBackColorSet(ref btnZ, tmp & 0x0020);//Z ,Y483.5
            btnBackColorSet(ref btnC, tmp & 0x0040);//C ,Y483.6
        }

        public void BtnY484Set(int tmp)
        {
            btnBackColorSet(ref btnjia, tmp & 0x0001);//+ ,Y484.0
        }

        public void BtnY485Set(int tmp)
        {
        }

        public void BtnY486Set(int tmp)
        {

        }

        public void BtnR29Set(int tmp)
        {

        }
    }
}
