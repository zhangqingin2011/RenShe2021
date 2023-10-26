﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Collector;
using HNC_MacDataService;
using HNCAPI;
using PLC;

namespace SCADA
{
    public partial class BinForm : Form
    {
        Color[] Bt_bgcoler = { System.Drawing.Color.FromArgb(255, 251, 240), System.Drawing.Color.FromArgb(0, 255, 0), System.Drawing.Color.FromArgb(0, 0, 255), System.Drawing.Color.FromArgb(255, 0, 0) };//按钮颜色
        public BinForm()
        {
            InitializeComponent();
            for (int ii = 0; ii < 40; ii++)
            {
                String buttonName = "buttonck" + (ii + 1).ToString();
                foreach (Control item in this.groupBox5.Controls)
                {
                    if (buttonName == item.Name)
                    {
                        if (((Button)item).BackColor != Bt_bgcoler[1])
                        {
                            ((Button)item).BackColor = Bt_bgcoler[1];
                        }
                    }
                }
                buttonName = "pictureBoxck" + (ii + 1).ToString();
                foreach (Control item in this.groupBox5.Controls)
                {
                    if (buttonName == item.Name)
                    {
                        if (((PictureBox)item).BackColor != Bt_bgcoler[0])
                        {
                            ((PictureBox)item).BackColor = Bt_bgcoler[0];
                        }
                    }
                }

                HNC8Reg tt1 = new HNC8Reg();
                HNC8Reg tt2 = new HNC8Reg();

                m_仓库允许位变量list.Add(tt1);
                m_仓库允许位变量list[ii].Name = "bin" + (ii + 1).ToString();
                m_仓库允许位变量list[ii].HncRegType = PLC_MITSUBISHI_HNC8.GetHncRegType("P");
                m_仓库允许位变量list[ii].index = 100 + ii + 2 * (ii / 8);
                m_仓库允许位变量list[ii].bit = -1;
                m_仓库允许位变量list[ii].value = 0;
                m_仓库允许位变量list[ii].status = false;

                m_创位是否空状态list.Add(tt2);
                m_创位是否空状态list[ii].Name = "pdt" + (ii + 1).ToString();
                m_创位是否空状态list[ii].HncRegType = PLC_MITSUBISHI_HNC8.GetHncRegType("B");
                m_创位是否空状态list[ii].index = 20 + ii + 2 * (ii / 8);
                m_创位是否空状态list[ii].bit = -1;
                m_创位是否空状态list[ii].value = 0;
                m_创位是否空状态list[ii].status = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (HncApi.HNC_NetIsConnect(Collector.CollectHNCPLC.dbNo) >= 0)
            if (MacDataService.GetInstance().IsNCConnectToDatabase(Collector.CollectHNCPLC.dbNo))
            {
                for (int ii = 0; ii < 40; ii++)
                {
                    if (PLCDataShare.m_plclist[0].m_hncPLCCollector.GetPParam(m_仓库允许位变量list[ii].index, ref m_仓库允许位变量list[ii].value))
                    {
                        String buttonName = "buttonck" + (ii + 1).ToString();
                        foreach (Control item in this.groupBox5.Controls)
                        {
                            if (buttonName == item.Name)
                            {
                                if (m_仓库允许位变量list[ii].value == 0)
                                {
//                                        Int32 asf = (~(1 << ii));
//                                        仓库允许位变量 = 仓库允许位变量 & asf;
                                    if (((Button)item).BackColor != Bt_bgcoler[0])
                                    {
                                        ((Button)item).BackColor = Bt_bgcoler[0];
                                    }
                                }
                                else if (m_仓库允许位变量list[ii].value == 1)
                                {
//                                        仓库允许位变量 = 仓库允许位变量 | (1 << ii);
                                    if (((Button)item).BackColor != Bt_bgcoler[1])
                                    {
                                        ((Button)item).BackColor = Bt_bgcoler[1];
                                    }
                                }    
                                else if (m_仓库允许位变量list[ii].value == 3)
                                {
                                    if (((Button)item).BackColor != Bt_bgcoler[2])
                                    {
                                        ((Button)item).BackColor = Bt_bgcoler[2];
                                    }
                                }
                                else
                                {
                                    ((Button)item).BackColor = Bt_bgcoler[0];
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int nowbin = 0;
                for (int ii = 0; ii < 40; ii++)
                {
                    if (CollectShare.GetRegValue((Int32)m_创位是否空状态list[ii].HncRegType, m_创位是否空状态list[ii].index, out m_创位是否空状态list[ii].value, Collector.CollectHNCPLC.dbNo) == 0)
                    {
                        String pict = "pictureBoxck" + (ii + 1).ToString();
                        nowbin = m_创位是否空状态list[ii].value;
                        foreach (Control item in groupBox5.Controls)
                        {
                            if (pict == item.Name)
                            {
                                if (nowbin == 0)
                                {
                                    if (((PictureBox)item).BackColor != Bt_bgcoler[0])
                                    {
                                        ((PictureBox)item).BackColor = Bt_bgcoler[0];
                                    }
                                }
                                else if (nowbin == 7)
                                {
                                    if (((PictureBox)item).BackColor != Bt_bgcoler[1])
                                    {
                                        ((PictureBox)item).BackColor = Bt_bgcoler[1];
                                    }
                                }
                                else if (nowbin == 11)
                                {
                                    if (((PictureBox)item).BackColor != Bt_bgcoler[2])
                                    {
                                        ((PictureBox)item).BackColor = Bt_bgcoler[2];
                                    }
                                }
                                else if (nowbin == 19)
                                {
                                    if (((PictureBox)item).BackColor != Bt_bgcoler[3])
                                    {
                                        ((PictureBox)item).BackColor = Bt_bgcoler[3];
                                    }
                                }
                                else
                                {
                                    ((PictureBox)item).BackColor = Bt_bgcoler[0];
                                }
                            }
                        }
                    }
                }
            }
        }

        Int32 仓库允许位变量 = Int32.MaxValue;//初始化允许所用仓位
        List<HNC8Reg> m_仓库允许位变量list = new List<HNC8Reg>();
        List<HNC8Reg> m_创位是否空状态list = new List<HNC8Reg>();
        private void buttonck1_Click(object sender, EventArgs e)
        {
            for (Int32 ii = 0; ii < 40; ii++)
            {
                String buttonName = "buttonck" + (ii + 1).ToString();
                if (buttonName == ((Button)sender).Name)
                {
                    int value = -1;
                    if (m_仓库允许位变量list[ii].value == 0)
                    {
                        m_仓库允许位变量list[ii].value = 1;
                    }
                    else if (m_仓库允许位变量list[ii].value == 1)
                    {
                        m_仓库允许位变量list[ii].value = 3;
                    }
                    else
                    {
                        m_仓库允许位变量list[ii].value = 0;
                    }
                    value = m_仓库允许位变量list[ii].value;
                    //                    Int32 flag = 仓库允许位变量 & (1 << ii);
                    //                    flag >>= ii;
                    if (value == 0)
                    {
                        if (PLCDataShare.m_plclist != null && PLCDataShare.m_plclist.Count > 0 && PLCDataShare.m_plclist[0].system == m_xmlDociment.PLC_System[1]
                                && PLCDataShare.m_plclist[0].m_hncPLCCollector.ChangePParam(m_仓库允许位变量list[ii].index, 0))//HNC8
                        {
                            //                            Int32 asf = (~(1 << ii));
                            //                            仓库允许位变量 = 仓库允许位变量 & asf;
                            if (((Button)sender).BackColor != Bt_bgcoler[0])
                            {
                                ((Button)sender).BackColor = Bt_bgcoler[0];
                            }
                        }
                    }
                    else if (value == 1)
                    {
                        if (PLCDataShare.m_plclist != null && PLCDataShare.m_plclist.Count > 0 && PLCDataShare.m_plclist[0].system == m_xmlDociment.PLC_System[1]
                            && PLCDataShare.m_plclist[0].m_hncPLCCollector.ChangePParam(m_仓库允许位变量list[ii].index, 1))//HNC8
                        {
                            //                            仓库允许位变量 = 仓库允许位变量 | (1 << ii);
                            if (((Button)sender).BackColor != Bt_bgcoler[1])
                            {
                                ((Button)sender).BackColor = Bt_bgcoler[1];
                            }
                        }
                    }
                    else if (value == 3)
                    {
                        if (PLCDataShare.m_plclist != null && PLCDataShare.m_plclist.Count > 0 && PLCDataShare.m_plclist[0].system == m_xmlDociment.PLC_System[1]
                           && PLCDataShare.m_plclist[0].m_hncPLCCollector.ChangePParam(m_仓库允许位变量list[ii].index, 3))//HNC8
                        {
                            if (((Button)sender).BackColor != Bt_bgcoler[2])
                            {
                                ((Button)sender).BackColor = Bt_bgcoler[2];
                            }
                        }
                    }
                    else
                    {
                        ((Button)sender).BackColor = Bt_bgcoler[0];
                    }
                }
            }
        }
    }
}
