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
    public partial class HNC848BKeyboard1 : UserControl
    {
        public HNC848BKeyboard1()
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
            btnBackColorSet(ref fbtnAuto, 0);
            btnBackColorSet(ref fbtnSingleSegmentCNC, 0);
            btnBackColorSet(ref fbtnManualCNC, 0);
            btnBackColorSet(ref fbtnRunIncrementCNC, 0);
            btnBackColorSet(ref fbtnReZeroCNC, 0);
            btnBackColorSet(ref fbtnkongrunCNC, 0);
            btnBackColorSet(ref fbtnGCSkipCNC, 0);
            btnBackColorSet(ref fbtnxuanzhetingCNC, 0);
            btnBackColorSet(ref fbtnjichuangshuoCNC, 0);
            btnBackColorSet(ref fbtnkuaijingCNC, 0);
            btnBackColorSet(ref fbtnzhuzhoudingxiang, 0);
            btnBackColorSet(ref fbtnSpindleForwardCNC, 0);
            btnBackColorSet(ref fbtnzhuzhoufanzhuangCNC, 0);
            btnBackColorSet(ref fbtnzhuzhoudiandongCNC, 0);
            btnBackColorSet(ref fbtnSpindleStopCNC, 0);
        }

        public void LoadLanguage()
        {
            fbtnAuto.Text = ChangeLanguage.GetString("btnAuto");
            fbtnSingleSegmentCNC.Text = ChangeLanguage.GetString("btnSingle");
            fbtnManualCNC.Text = ChangeLanguage.GetString("btnManual");
            fbtnRunIncrementCNC.Text = ChangeLanguage.GetString("btn⁯Increment");
            fbtnGCSkipCNC.Text = ChangeLanguage.GetString("btn⁯⁯ProgramSkip");
            fbtnkongrunCNC.Text = ChangeLanguage.GetString("btn⁯⁯⁯AirRun");
            fbtnxuanzhetingCNC.Text = ChangeLanguage.GetString("btn⁯⁯⁯ChoiceStop");
            fbtnReZeroCNC.Text = ChangeLanguage.GetString("btn⁯ReZero");
            fbtnkuaijingCNC.Text = ChangeLanguage.GetString("btnFast");
            fbtnzhuzhoudingxiang.Text = ChangeLanguage.GetString("btnSpindleQrientation");
            fbtnjichuangshuoCNC.Text = ChangeLanguage.GetString("btnCNCLock");
            fbtnSpindleForwardCNC.Text = ChangeLanguage.GetString("btnSpindleForward");
            fbtnzhuzhoufanzhuangCNC.Text = ChangeLanguage.GetString("btnSpindleReversal");
            fbtnzhuzhoudiandongCNC.Text = ChangeLanguage.GetString("btnSpindleMove");
            fbtnSpindleStopCNC.Text = ChangeLanguage.GetString("btnSpindleStop");
        }

        public void BtnY480Set(int tmp)
        {

        }

        public void BtnY481Set(int tmp)
        {

        }

        public void BtnY482Set(int tmp)
        {

        }

        public void BtnY483Set(int tmp)
        {

        }

        public void BtnY484Set(int tmp)
        {

        }

        public void BtnY485Set(int tmp)
        {

        }

        public void BtnY486Set(int tmp)
        {

        }
    }
}
