﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using LineDevice;
using HNCAPI;
using System.Runtime.InteropServices;

namespace SCADA
{
    public partial class HomeForm : Form
    {
        static string[] CNCStateColorPictrueFile = { "..\\picture\\picCNCgreed.png",
        "..\\picture\\picCNCyel.png","..\\picture\\picCNCred.png","..\\picture\\picCNCray.png" };//CNC设备状态图片路径
        static string[] ROBOTStateColorPictrueFile = { "..\\picture\\picRobotgreed.png",
        "..\\picture\\picRobotyel.png","..\\picture\\picRobotgred.png","..\\picture\\picRobotgray.png" };//robot设备状态图片路径

        static string[] RGVStateColorPictrueFile = { "..\\picture\\picRGVgreed.png",
        "..\\picture\\picRGVyel.png","..\\picture\\picRGVgred.png","..\\picture\\picRGVgray.png" };//RGV设备状态图片路径

        static string[] CheckStateColorPictrueFile = { "..\\picture\\picCheckgreed.png",
        "..\\picture\\picCheckgred.png","..\\picture\\picCheckgray.png" };//Chexk设备状态图片路径

        static string AGVStateColorPictrueFile = "..\\picture\\picAGV.jpg";//AGV设备状态图片路径

        static string ConveyorStateColorPictrueFile = "..\\picture\\picConveyor.jpg";//传送带状态图片路径

        static string[] zidongjuaFile = { "..\\picture\\ru1.jpg", "..\\picture\\ru2.jpg",
                                            "..\\picture\\ru3.jpg", "..\\picture\\ru4.jpg", "..\\picture\\ru5.jpg",
                                        "..\\picture\\dabiao.jpg"};


        static string[] ToolTip_str = { "名称：", "IP：", "运行程序：" };//ToolTip显示信息
        static string[] State_str = { "运行", "空闲", "报警", "离线" };//状态字符串
        static int lineType =1; //产线类型  1：RGV方式    0：ROBROT 一拖二    2：智能产线
        static int hOffset = 0; //水平间隔

        public enum ShowItemType
        {
            CNC = 0,
            ROBOT,
            RFID,  //CheckEq
           // Storehouse,
           Measure, //AGV
           Storage //Conveyor
        }


        class ShowItem//显示对象数据结构
        {
            Button m_bt;
            Panel m_panel;
            Label m_label;
            public List<PictureBox> m_pictrureboxlist;
            public List<PictureBox> m_pictrurebox2list;
            int m_pictrureboxlistcount;
            public int m_showtype;
            public int m_showitemserial;
            string m_ColorPictrue_str;
            int m_label_Height;
            Color[] Bt_bgcoler = { System.Drawing.Color.FromArgb(255, 251, 240), System.Drawing.Color.FromArgb(0, 255, 0), System.Drawing.Color.FromArgb(0, 0, 255), System.Drawing.Color.FromArgb(255, 0, 0) };//按钮颜色

            public ShowItem(int m_showtype, int m_showitemserial)//初始化
            {
                m_bt = new Button();
                m_panel = new Panel();
                m_label = new Label();
                m_label_Height = 20;
                this.m_showitemserial = m_showitemserial;
                this.m_showtype = m_showtype;

                switch (m_showtype)
                {
                    case (int)ShowItemType.CNC:
                        m_ColorPictrue_str = CNCStateColorPictrueFile[3];
                        break;
                    case (int)ShowItemType.ROBOT:
                        if (lineType == 1)  //2015.11.28 RGV
                            m_ColorPictrue_str = RGVStateColorPictrueFile[3];
                        else
                            m_ColorPictrue_str = ROBOTStateColorPictrueFile[3];
                        break;
                    case (int)ShowItemType.RFID: //CheckEq
                        m_ColorPictrue_str = CheckStateColorPictrueFile[2];
                        break;
                    //case (int)ShowItemType.Storehouse:
                    //    m_pictrureboxlistcount = m_showitemserial;
                    //    m_pictrureboxlist = new List<PictureBox>();
                    //    m_pictrurebox2list = new List<PictureBox>();
                    //    for (int ii = 0; ii < m_showitemserial; ii++)
                    //    {
                    //        m_pictrureboxlist.Add(new PictureBox());
                    //        m_pictrurebox2list.Add(new PictureBox());
                    //    }
                    //    break;
                    case (int)ShowItemType.Measure://AGV
                        m_ColorPictrue_str = AGVStateColorPictrueFile;
                        break;
                    case (int)ShowItemType.Storage://Conveyor
                        m_ColorPictrue_str = ConveyorStateColorPictrueFile;
                        break;
                    default:
                        break;
                }
                m_bt.Click += new EventHandler(SkipWindow);//单击事件
                m_bt.MouseHover += new EventHandler(MouseHoverTooltip);//鼠标停留事件
            }
            /// <summary>
            /// 鼠标点击事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void SkipWindow(object sender, System.EventArgs e)
            {
                Button m_button = (Button)sender;
                if (m_showtype == (int)ShowItemType.CNC || m_showtype == (int)ShowItemType.ROBOT)
                {
                    PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + m_showtype, m_showtype + 1, m_showitemserial);
                }
                else if (m_showtype == (int)ShowItemType.RFID)//RIFD CheckEq2017.3.20  7
                {
                    PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + 14 , m_showtype + 4, m_showitemserial);
                }
                else if (m_showtype == (int)ShowItemType.Measure)//工件测量AGV   8
                {
                    PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + 15, m_showtype + 3, m_showitemserial); 
                }
                else if (m_showtype == (int)ShowItemType.Storage)//Conveyor智能料仓  9
                {
                    PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + 13, m_showtype + 5, m_showitemserial);
                }
              
            }

            /// <summary>
            /// 鼠标停留事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void MouseHoverTooltip(object sender, System.EventArgs e)
            {
                ToolTip m_tooltip = new ToolTip();
                string str = "";
                switch (m_showtype)
                {
                    case (int)ShowItemType.CNC:
                        CNC m_cnc = MainForm.cnclist[m_showitemserial];
                        str = "";
                        str += HomeForm.ToolTip_str[0] + m_cnc.JiTaiHao + "\r\n";
                        str += ToolTip_str[1] + m_cnc.ip + "\r\n";
                        str += ToolTip_str[2] + m_cnc.get_gCodeName() + "\r\n";
                        m_tooltip.SetToolTip((Control)sender, str);
                        break;
                    case (int)ShowItemType.ROBOT:
                        ROBOT m_robot = MainForm.robotlist[m_showitemserial];
                        str = "";
                        str += HomeForm.ToolTip_str[0] + m_robot.EQUIP_CODE + "\r\n";
                        //                      str += ToolTip_str[1] + m_cnc.ip + "\r\n";
                        //                      str += ToolTip_str[2] + m_cnc.get_gCodeName() + "\r\n";
                        m_tooltip.SetToolTip((Control)sender, str);
                        break;
                    case (int)ShowItemType.RFID://CheckEq
                        //str = "";
                        //int productOK = 0;
                        //str += HomeForm.ToolTip_str[0] + "RFID" + "\r\n";
                        //if(MainForm.cmm.sendadr==""|MainForm.cmm.sendadr==null)
                        //{
                        //    str += "测量仪未连接" + "\r\n";
                        //}
                        //else
                        //{
                        //    str += "地址：" + MainForm.cmm.sendadr + "\r\n";
                        //}
                        //if (MainForm.cmm.ShowDataTable != null)
                        //{
                        //    for (int ii = 0; ii < MainForm.cmm.ShowDataTable.Rows.Count; ii++)
                        //    {
                        //        if (MainForm.cmm.ShowDataTable.Rows[ii][4].ToString() == "合格")
                        //        {
                        //            productOK++;
                        //        }
                        //    }
                        //    str += "结果：" + "合格：" + productOK.ToString() + "，不合格：" + (MainForm.cmm.ShowDataTable.Rows.Count - productOK).ToString() + "\r\n";
                        //}

                        //    //str += "结果：" + CMMForm.xData[0] + "：" + CMMForm.yData[0].ToString() + "，" + CMMForm.xData[1] + "：" + CMMForm.yData[1].ToString() + "\r\n";
                        //m_tooltip.SetToolTip((Control)sender, str);
                        break;
                    //case (int)ShowItemType.Storehouse:
                    //    str = "";
                    //    str += HomeForm.ToolTip_str[0] + "料仓" + "\r\n";
                        //if (m_pictrureboxlist != null && HncApi.HNC_NetIsConnect(Collector.CollectHNCPLC.dbNo) >= 0)
                        //{
                        //    int index = 0;
                        //    int value = 0;
                        //    int productA = 0;
                        //    int productB = 0;
                        //    int productSL = 0;
                        //    int productCL = 0;
                        //    int productFL = 0;
                        //    for (int ii = 0; ii < m_pictrureboxlist.Count; ii++)
                        //    {
                        //        index = 100 + ii + 2 * (ii / 8);
                        //        if (PLCDataShare.m_plclist[0].m_hncPLCCollector.GetPParam(index, ref value))
                        //        {
                        //            if (value == 1)
                        //            {
                        //                productA++;
                        //            }
                        //            else if (value == 3)
                        //            {
                        //                productB++;
                        //            }
                        //        }

                        //        index = 20 + ii + 2 * (ii / 8);
                        //        if (HncApi.HNC_RegGetValue(7, index, out value, Collector.CollectHNCPLC.dbNo) == 0)
                        //        {
                        //            if (value == 7)
                        //            {
                        //                productSL++;
                        //            }
                        //            else if (value == 11)
                        //            {
                        //                productCL++;
                        //            }
                        //            else if (value == 19)
                        //            {
                        //                productFL++;
                        //            }
                        //        }
                        //    }
                        //    str += "A料：" + productA.ToString() + "，B料：" + productB.ToString() + "，禁用：" + (40 - productA - productB).ToString() + "\r\n";
                        //    str += "成料：" + productCL.ToString() + "，废料：" + productFL.ToString() + "，生料：" + productSL.ToString() + "，加工中：" + (40 - productSL - productCL - productFL).ToString() + "\r\n";
                        //}
                       // m_tooltip.SetToolTip((Control)sender, str);
                       // break;
                    default:
                        break;
                }                
//                 if (m_showtype == 0)//CNC
//                 {
//                     CNC m_cnc = MainForm.cnclist[m_showitemserial];
//                     string str = "";
//                     str += HomeForm.ToolTip_str[0] + m_cnc.JiTaiHao + "\r\n";
//                     str += ToolTip_str[1] + m_cnc.ip + "\r\n";
//                     str += ToolTip_str[2] + m_cnc.get_gCodeName() + "\r\n";
//                     m_tooltip.SetToolTip((Control)sender, str);
//                 }
//                 else if (m_showtype == 10)//robot
//                 {
//                     ROBOT m_robot = MainForm.robotlist[m_showitemserial];
//                     string str = "";
//                     str += HomeForm.ToolTip_str[0] + m_robot.EQUIP_CODE + "\r\n";
// //                     str += ToolTip_str[1] + m_cnc.ip + "\r\n";
// //                     str += ToolTip_str[2] + m_cnc.get_gCodeName() + "\r\n";
//                     m_tooltip.SetToolTip((Control)sender, str);
//                 }                
            }

            /// <summary>
            /// 显示对象
            /// </summary>
            /// <param name="Parent"></param>
            /// <param name="m_panel_Width"></param>
            /// <param name="m_panel_Height"></param>
            /// <param name="m_panel_Left"></param>
            /// <param name="m_panel_Top"></param>
            public void ShowItem2window(Form Parent,int m_panel_Width, int m_panel_Height, int m_panel_Left, int m_panel_Top, int index)
            {
                return; //hxb 2017.6.6

                m_panel.Width = m_panel_Width;
                m_panel.Height = m_panel_Height;
                m_panel.Left = m_panel_Left;
                m_panel.Top = m_panel_Top;

                m_bt.Parent = m_panel;
                m_label.Parent = m_panel;

                m_label.AutoSize = false;
                m_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

//                 m_bt.Tag = 9999;
//                 m_label.Tag = 9999;
                m_bt.Width = m_panel.Width;
                m_bt.Height = m_panel_Height - m_label_Height;
                m_label.Top = m_bt.Height;
                m_label.Width = m_panel.Width;
                m_label.Height = m_label_Height;
                m_bt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                Parent.Controls.Add(m_panel);
                Size m_imagesize = new Size();
                m_imagesize.Width = m_bt.Width;
                m_imagesize.Height = m_bt.Height;

                Bitmap m_bitmap = null;
//                 m_bitmap.MakeTransparent(Color.White);//设置背景是透明的
                //double TextSize = m_panel_Width / 9;//6.8
                //m_label.Font = new System.Drawing.Font("宋体", float.Parse(TextSize.ToString()), FontStyle.Bold);

                switch (m_showtype)
                {
                    case (int)ShowItemType.CNC:
                        m_label.Text = MainForm.cnclist[m_showitemserial].JiTaiHao;
                        break;
                    case (int)ShowItemType.ROBOT:
                        m_label.Text = "ROBOT0" + MainForm.robotlist[m_showitemserial].EQUIP_CODE;
                        if (lineType != 2)
                        {
                            if (lineType == 1)
                            {
                                if (index == MainForm.cnclist.Count)
                                    m_ColorPictrue_str = ROBOTStateColorPictrueFile[3];
                                else
                                    m_ColorPictrue_str = RGVStateColorPictrueFile[3];
                            }
                            else
                            {
                                if (index == MainForm.cnclist.Count)
                                    m_panel.Top -= 70;
                                else if (index == (MainForm.cnclist.Count + 1))
                                {
                                    m_panel.Left -= hOffset;
                                    m_panel.Top += 70;
                                }
                                else
                                    m_panel.Left -= hOffset;

                                m_ColorPictrue_str = ROBOTStateColorPictrueFile[3];
                            }
                        }
                        break;
                    case (int)ShowItemType.RFID://CheckEq
                        m_label.Text = "RFID系统";
                        break;
                    //case (int)ShowItemType.Storehouse:
                    //    TextSize = m_label.Height / 1.5;
                    //    m_label.Font = new System.Drawing.Font("宋体", float.Parse(TextSize.ToString()), FontStyle.Bold);
                    //    m_label.Text = "立体仓库  A料:  B料:  禁用:  ";
                    //    StorehouseShowDisp();
                    //    break;
                    case (int)ShowItemType.Measure: //AGV
                        m_label.Text = "测量";
                        break;
                    case (int)ShowItemType.Storage://Conveyor:
                        //TextSize = m_label.Height / 1.5;
                        //m_label.Font = new System.Drawing.Font("宋体", float.Parse(TextSize.ToString()), FontStyle.Bold);

                        m_label.Text = "数字化立体料仓";
                        break;
                    default:
                        break;
                }
                //if (m_showtype != (int)ShowItemType.Storehouse)
                {
                    m_bitmap = new Bitmap(System.Drawing.Image.FromFile(m_ColorPictrue_str), m_imagesize);
                    m_bt.Image = m_bitmap;
                }
                m_bt.FlatStyle = FlatStyle.Flat;//样式
                m_bt.ForeColor = Color.Transparent;//前景
                m_bt.FlatAppearance.BorderSize = 0;

                m_panel.Show();  
            }


            /// <summary>
            /// 更新对象状态
            /// </summary>
            public void UpdataShowItem()
            {
                LineDevice.CNC.CNCState stad = 0;//状态

                switch (m_showtype)
                {
                    case (int)ShowItemType.CNC:
                        MainForm.cnclist[m_showitemserial].Checkcnc_state(ref stad);
                        String ShowStr = MainForm.cnclist[m_showitemserial].JiTaiHao;
                        switch (stad)
                        {
                            case LineDevice.CNC.CNCState.DISCON:
                                m_label.Text = ShowStr;
                                m_ColorPictrue_str = CNCStateColorPictrueFile[3];
                                break;
                            case LineDevice.CNC.CNCState.IDLE:
                                m_label.Text = ShowStr;
                                m_ColorPictrue_str = CNCStateColorPictrueFile[1];
                                break;
                            case LineDevice.CNC.CNCState.ALARM:
                                m_label.Text = ShowStr;
                                m_ColorPictrue_str = CNCStateColorPictrueFile[2];
                                break;
                            case LineDevice.CNC.CNCState.RUNING://运行
                                m_label.Text = ShowStr;
                                m_ColorPictrue_str = CNCStateColorPictrueFile[0];
                                break;
                            default:
                                break;
                        }
                        Size m_imagesize = new Size();
                        m_imagesize.Width = m_bt.Width;
                        m_imagesize.Height = m_bt.Height;
                        Bitmap m_bitmap = new Bitmap(System.Drawing.Image.FromFile(m_ColorPictrue_str), m_imagesize);
                        if (m_bitmap != m_bt.Image)
                        {
                            m_bt.Image = m_bitmap;
                        }
                        break;
                    case (int)ShowItemType.ROBOT:
                        
                        break;
                    case (int)ShowItemType.RFID://CheckEq
                        //switch (CMM.cmmstatus)
                        //{
                        //    case -1:
                        //        m_ColorPictrue_str = CheckStateColorPictrueFile[2];
                        //        break;
                        //    case 0:
                        //        m_ColorPictrue_str = CheckStateColorPictrueFile[2];
                        //        break;
                        //    case 1:
                        //        m_ColorPictrue_str = CheckStateColorPictrueFile[0];
                        //        break;
                        //    case 2:
                        //        m_ColorPictrue_str = CheckStateColorPictrueFile[0];
                        //        break;
                        //    case 3:
                        //        m_ColorPictrue_str = CheckStateColorPictrueFile[0];
                        //        break;
                        //    case 4:
                        //        m_ColorPictrue_str = CheckStateColorPictrueFile[1];
                        //        break;
                        //    default:
                        //        break;
                        //}
                        //Size m_cmagesize = new Size();
                        //m_cmagesize.Width = m_bt.Width;
                        //m_cmagesize.Height = m_bt.Height;
                        //Bitmap m_cbitmap = new Bitmap(System.Drawing.Image.FromFile(m_ColorPictrue_str), m_cmagesize);
                        //if (m_cbitmap != m_bt.Image)
                        //{
                        //    m_bt.Image = m_cbitmap;
                        //}
                        break;
                    //case (int)ShowItemType.Storehouse:
                    //    //if (m_pictrureboxlist != null && HncApi.HNC_NetIsConnect(Collector.CollectHNCPLC.dbNo) >= 0)
                    //    {
                    //        int index = 0;
                    //        int value = 0;
                    //        for (int ii = 0; ii < m_pictrureboxlist.Count; ii++)
                    //        {
                    //            index = 100 + ii + 2 * (ii / 8);
                    //            if (PLCDataShare.m_plclist[0].m_hncPLCCollector.GetPParam(index, ref value))
                    //            {
                    //                if (value == 0)
                    //                {
                    //                    if (m_pictrureboxlist[ii].BackColor != Bt_bgcoler[0])
                    //                    {
                    //                        m_pictrureboxlist[ii].BackColor = Bt_bgcoler[0];
                    //                    }
                    //                }
                    //                else if (value == 1)
                    //                {
                    //                    if (m_pictrureboxlist[ii].BackColor != Bt_bgcoler[1])
                    //                    {
                    //                        m_pictrureboxlist[ii].BackColor = Bt_bgcoler[1];
                    //                    }
                    //                }
                    //                else if (value == 3)
                    //                {
                    //                    if (m_pictrureboxlist[ii].BackColor != Bt_bgcoler[2])
                    //                    {
                    //                        m_pictrureboxlist[ii].BackColor = Bt_bgcoler[2];
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    m_pictrureboxlist[ii].BackColor = Bt_bgcoler[0];
                    //                }
                    //            }

                    //            index = 20 + ii + 2 * (ii / 8);
                                //if (HncApi.HNC_RegGetValue(7, index, out value, Collector.CollectHNCPLC.m_clientNo) == 0)
                                //{
                                //    if (value == 0)
                                //    {
                                //        if (m_pictrurebox2list[ii].BackColor != Bt_bgcoler[0])
                                //        {
                                //            m_pictrurebox2list[ii].BackColor = Bt_bgcoler[0];
                                //        }
                                //    }
                                //    else if (value == 7)
                                //    {
                                //        if (m_pictrurebox2list[ii].BackColor != Bt_bgcoler[1])
                                //        {
                                //            m_pictrurebox2list[ii].BackColor = Bt_bgcoler[1];
                                //        }
                                //    }
                                //    else if (value == 11)
                                //    {
                                //        if (m_pictrurebox2list[ii].BackColor != Bt_bgcoler[2])
                                //        {
                                //            m_pictrurebox2list[ii].BackColor = Bt_bgcoler[2];
                                //        }
                                //    }
                                //    else if (value == 19)
                                //    {
                                //        if (m_pictrurebox2list[ii].BackColor != Bt_bgcoler[3])
                                //        {
                                //            m_pictrurebox2list[ii].BackColor = Bt_bgcoler[3];
                                //        }
                                //    }
                                //    else
                                //    {
                                //        m_pictrurebox2list[ii].BackColor = Bt_bgcoler[0];
                                //    }
                                //}
                        //    }
                        //}
                        //break;
                    //case (int)ShowItemType.Measure: //AGV

                    //    break;
                    //case (int)ShowItemType.Storage: //Conveyor:

                    //    break;
                    default:
                        break;
                }



//                 if (m_showtype == 0)//CNC
//                 {
//                     MainForm.cnclist[m_showitemserial].Checkcnc_state(ref stad);
// //                     if(Oldstad != stad)
//                     {
//                         String ShowStr = MainForm.cnclist[m_showitemserial].JiTaiHao;
//                         switch (stad)
//                         {
//                             case LineDevice.CNC.CNCState.DISCON:
//                                 m_label.Text = ShowStr;
//                                 m_ColorPictrue_str = CNCStateColorPictrueFile[3];
//                                 break;
//                             case LineDevice.CNC.CNCState.IDLE:
//                                 m_label.Text = ShowStr;
//                                 m_ColorPictrue_str = CNCStateColorPictrueFile[1];
//                                 break;
//                             case LineDevice.CNC.CNCState.ALARM:
//                                 m_label.Text = ShowStr;
//                                 m_ColorPictrue_str = CNCStateColorPictrueFile[2];
//                                 break;
//                             case LineDevice.CNC.CNCState.RUNING://运行
//                                 m_label.Text = ShowStr;
//                                 m_ColorPictrue_str = CNCStateColorPictrueFile[0];
//                                 break;
//                             default:
//                                 break;
//                         }
//                         Size m_imagesize = new Size();
//                         m_imagesize.Width = m_bt.Width;
//                         m_imagesize.Height = m_bt.Height;
//                         Bitmap m_bitmap = new Bitmap(System.Drawing.Image.FromFile(m_ColorPictrue_str), m_imagesize);
//                         if (m_bitmap != m_bt.Image)
//                         {
//                             m_bt.Image = m_bitmap;
//                         }
//                     }
//                 }
//                 else if (m_showtype == 10)//ROBOT
//                 {
//                     if (PLCDataShare.m_plclist.Count > 0)
//                     {
//                         String ShowStr = MainForm.robotlist[m_showitemserial].EQUIP_CODE ;
//                         bool connet = false;
//                         if (PLCDataShare.m_plclist[0].system == m_xmlDociment.PLC_System[0])
//                         {
//                             connet = PLCDataShare.m_plclist[0].conneted;
//                         }
//                         else if (PLCDataShare.m_plclist[0].system == m_xmlDociment.PLC_System[1])
//                         {
//                             connet = PLCDataShare.m_plclist[0].m_hncPLCCollector.connectStat;
//                         }
//                         if (ROBOTstad != connet)
//                         {
//                             if (connet)
//                             {
//                                 m_label.Text = ShowStr;
//                                 if (m_showitemserial != MainForm.cnclist.Count)
//                                 {
//                                     if (lineType == 1)  //2015.11.28 RGV
//                                         m_ColorPictrue_str = RGVStateColorPictrueFile[0];
//                                     else
//                                         m_ColorPictrue_str = ROBOTStateColorPictrueFile[0];
//                                 }
// 
//                             }
//                             else
//                             {
//                                 m_label.Text = ShowStr;
//                                 if (m_showitemserial != MainForm.cnclist.Count)
//                                 {
//                                     if (lineType == 1)  //2015.11.28 RGV
//                                         m_ColorPictrue_str = RGVStateColorPictrueFile[3];
//                                     else
//                                         m_ColorPictrue_str = ROBOTStateColorPictrueFile[3];
//                                 }
//                             }
//                             Size m_imagesize = new Size();
//                             m_imagesize.Width = m_bt.Width;
//                             m_imagesize.Height = m_bt.Height;
//                             Bitmap m_bitmap = new Bitmap(System.Drawing.Image.FromFile(m_ColorPictrue_str), m_imagesize);
//                             m_bt.Image = m_bitmap;
//                             ROBOTstad = connet;
//                         }
//                     }
//                 }
//                 else if (m_showtype == 10)
//                 {
//                     if (m_pictrureboxlist != null)
//                     {
//                         for (int ii = 0; ii < m_pictrureboxlist.Count; ii++)
//                         {
//                             if (m_pictrureboxlist[ii].BackColor != MainForm.binfunc.colrec[ii])
//                             {
//                                 m_pictrureboxlist[ii].BackColor = MainForm.binfunc.colrec[ii];
//                             }
//                         }
//                     }
//                 }

            }

            private void StorehouseShowDisp()
            {
                int lineNumb = 8;//10个仓位一行
                int lineii = 5;
                int Width_Stp1 = m_bt.Width / 12;//在按钮上排布10个pictrurebox标示仓位，仓位之间间隔半个仓位宽度
                int Width_Stp2 = m_bt.Width / 16;

                int pictrurebox_Width = Width_Stp1;
                int pictrurebox_Height = Width_Stp2;
                int pictrurebox_Left = Width_Stp1 / 4;//第一个仓位的left
                int pictrurebox_Top = Width_Stp2 / 4;


                for (int ii = 0; ii < m_pictrureboxlist.Count; ii++)
                {
                    if (ii % lineNumb == 0)
                    {
                        lineii--;
                        pictrurebox_Left = Width_Stp1 / 4;
                        pictrurebox_Top = Width_Stp2 / 4 + (Width_Stp2 + Width_Stp2 / 2) * lineii;
                    }
                    else
                    {
                        pictrurebox_Left += ((Width_Stp1 * 3) / 2);
                    }
                    m_pictrureboxlist[ii].Width = pictrurebox_Width;
                    m_pictrureboxlist[ii].Height = pictrurebox_Height;
                    m_pictrureboxlist[ii].Left = pictrurebox_Left;
                    m_pictrureboxlist[ii].Top = pictrurebox_Top;
                    m_pictrureboxlist[ii].Parent = m_bt;
                    m_pictrureboxlist[ii].BackColor = Color.Green;

                    m_pictrurebox2list[ii].Width = pictrurebox_Width;
                    m_pictrurebox2list[ii].Height = pictrurebox_Height / 6;
                    m_pictrurebox2list[ii].Left = pictrurebox_Left;
                    m_pictrurebox2list[ii].Top = pictrurebox_Top + pictrurebox_Height + 2;
                    m_pictrurebox2list[ii].Parent = m_bt;
                    m_pictrurebox2list[ii].BackColor = Color.Green;
                }
            }
        }

        List<ShowItem> Showlist = new List<ShowItem>();
        int robot_num = 0;

        [DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public HomeForm()
        {
            InitializeComponent();

            this.btnPC.Parent = pictureBox4;
            this.btnGC.Parent = pictureBox4;
            this.btnRFID.Parent = pictureBox4;
            this.btnRack.Parent = pictureBox4;
            this.btnRobot.Parent = pictureBox4;
            this.btnCNC.Parent = pictureBox4;
        }


        private void HomeForm_Load(object sender, EventArgs e)
        {
         //   pictureBoxUnConnetColor.BackColor = new Bitmap(System.Drawing.Image.FromFile(CNCStateColorPictrueFile[3])).GetPixel(30, 30);
         //   pictureBoxKongXianColor.BackColor = new Bitmap(System.Drawing.Image.FromFile(CNCStateColorPictrueFile[1])).GetPixel(30, 30);
         //   pictureBoxRuningColor.BackColor = new Bitmap(System.Drawing.Image.FromFile(CNCStateColorPictrueFile[0])).GetPixel(30, 30);
         //   pictureBoxArrColor.BackColor = new Bitmap(System.Drawing.Image.FromFile(CNCStateColorPictrueFile[2])).GetPixel(30, 30);
         //   pictureBox1.BackColor = System.Drawing.Color.FromArgb(0, 255, 0);
         //   pictureBox2.BackColor = System.Drawing.Color.FromArgb(0, 0, 255);
         //   pictureBox3.BackColor = System.Drawing.Color.FromArgb(255, 251, 240);

         //   pictureBox5.BackgroundImage = new Bitmap(System.Drawing.Image.FromFile(zidongjuaFile[1]));
         //   pictureBox6.BackgroundImage = new Bitmap(System.Drawing.Image.FromFile(zidongjuaFile[4]));//zxl 2017.3.7
         //   pictureBox7.BackgroundImage = new Bitmap(System.Drawing.Image.FromFile(zidongjuaFile[2]));
         //   pictureBox8.BackgroundImage = new Bitmap(System.Drawing.Image.FromFile(zidongjuaFile[3]));//zxl 2017.3.7
            
         //   if (MainForm.m_xml.m_Read(m_xmlDociment.Path_linetype, -1, m_xmlDociment.Default_Attributes_linetype[0]) == m_xmlDociment.Default_linetype_value[0])
         //   {
         //       lineType = 0;
         //   }
         //   else if (MainForm.m_xml.m_Read(m_xmlDociment.Path_linetype, -1, m_xmlDociment.Default_Attributes_linetype[0]) == m_xmlDociment.Default_linetype_value[1])
         //   {
         //       lineType = 1;
         //   }
         //   else if (MainForm.m_xml.m_Read(m_xmlDociment.Path_linetype, -1, m_xmlDociment.Default_Attributes_linetype[0]) == m_xmlDociment.Default_linetype_value[2])
         //   {
         //       lineType = 2;
         //   }
         //   robot_num = 0;
         //   Showlist.Clear();
         //   if (MainForm.cnclist.Count > 0)
         //   {
         //       foreach (CNC m_cnc in MainForm.cnclist)
         //       {
         //           Showlist.Add(new ShowItem((int)ShowItemType.CNC, m_cnc.serial));
         //       }
         //   }
         //   if (MainForm.robotlist.Count > 0)
         //   {
         //       foreach (ROBOT m_robot in MainForm.robotlist)
         //       {
         //           Showlist.Add(new ShowItem((int)ShowItemType.ROBOT, m_robot.serial));
         //           robot_num++;
         //       }
         //   }
         ////   Showlist.Add(new ShowItem((int)ShowItemType.Storehouse, 50));
         //   Showlist.Add(new ShowItem((int)ShowItemType.RFID, 0));//CheckEq
         //   Showlist.Add(new ShowItem((int)ShowItemType.Measure, 0)); //AGV
         //   Showlist.Add(new ShowItem((int)ShowItemType.Storage, 0)); //Conveyor
         //   switch (lineType)
         //   {
         //       case 0://ROBROT 一拖二
         //       case 1://RGV方式
         //           ShowLineItem();
         //           break;
         //       case 2://智能产线
         //           Showzhinengchanxian();
         //           break;
         //       default:
         //           break;
         //   }

         //   HomeTimer.Enabled = true;
         //   HomeTimer.Interval = 500;
        }

        private void HomeTimer_Tick(object sender, EventArgs e)
        {
            //if (this.Visible && MainForm.InitializeComponentFinish)
            //{
            //    foreach (ShowItem m_Item in Showlist)
            //    {
            //        m_Item.UpdataShowItem();
            //    }
            //}
        }

        private void ShowLineItem()
        {
            int showitem_minWidth = 60;//图片宽度最小界限

            int showitem_Width = 100;//图片初始宽度
            int showitem_Height = 120;//图片初始高度
            int showitem_Left = 50;
            int showitem_Top = 80;
            int offsetLeft = 0; 

            float Width = (float)MainForm.cnclist.Count / 2;
            int int_Width = MainForm.cnclist.Count / 2;
            if (Width > int_Width)//奇数
            {
                int_Width++;
            }
            if (int_Width == 0)
            {
                int_Width = robot_num;
            }
            if (int_Width != 0)
            {
                hOffset = int_Width = (this.Width - 200) / (int_Width + 1); //2015.11.28 +1
                while (int_Width - 10 < showitem_Width && showitem_Width > showitem_minWidth)
                {
                    showitem_Width -= 5;
                    showitem_Height = (int)(showitem_Width * 1.2);
                }
                showitem_Left = 0;
                int index = 0;
                foreach (ShowItem m_Item in Showlist)
                {
                    switch (m_Item.m_showtype)
                    {
                        case 0:
                            if (index % 2 == 0)
                            {
                                showitem_Top = 80;
                                showitem_Left = int_Width * (index / 2);
                            }
                            else
                            {
                                showitem_Top = this.Height - showitem_Height - 80;
                            }
                            offsetLeft = int_Width;  //2015.11.28
                            break;
                        case 1:
                            showitem_Left = int_Width * (index - MainForm.cnclist.Count);
                            showitem_Top = this.Height / 2 - showitem_Height / 2;
                            offsetLeft = 0; //2015.11.28
                            break;
                        case 2:
                            break;
//                         case 3:
//                             showitem_Left = 10;
//                             showitem_Top = 100;
//                             offsetLeft = 0; //2015.11.28
//                             showitem_Width = 400;//图片初始宽度
//                             showitem_Height = 200;//图片初始高度
// 
//                             break;
                        default:
                            break;
                    }
//                     if (m_Item.m_showtype == 0)//cnc
//                     {
//                         if (index % 2 == 0)
//                         {
//                             showitem_Top = 80;
//                             showitem_Left = int_Width * (index / 2);
//                         }
//                         else
//                         {
//                             showitem_Top = this.Height - showitem_Height - 80;
//                         }
//                         offsetLeft = int_Width;  //2015.11.28
//                     }
//                     else if (m_Item.m_showtype == 1)//机器人
//                     {
//                         showitem_Left = int_Width * (index - MainForm.cnclist.Count);
//                         showitem_Top = this.Height / 2 - showitem_Height / 2;
//                         offsetLeft = 0; //2015.11.28
//                     }
//                     else if (m_Item.m_showtype == 2)//PLC
//                     {
//                     }

                    m_Item.ShowItem2window(this, showitem_Width, showitem_Height, showitem_Left + offsetLeft, showitem_Top, index);
                    index++;
                }
            }
        }

        private void Showzhinengchanxian()
        {
            int HorizontalSeg = 6;//水平分割6分
            int VerticalSeg = 5;//垂直分割4分
            int Horizontaljiange = 10;//水平分间隔
            int Verticaljiange = 20;//垂直间隔
            int LeftStar = 20;
            int TopStar = 50;

            int width_Step = (this.Width - 2 * LeftStar) / HorizontalSeg;
            int Height_Step = (this.Height - TopStar - LeftStar) / VerticalSeg;
            int showitem_Left = 0;
            int showitem_Top = 0;

            int showitem_Width = width_Step - Horizontaljiange/2;//图片初始宽度
            int showitem_Height = Height_Step - Verticaljiange/2;//图片初始高度
            foreach (ShowItem m_Item in Showlist)
            {
                showitem_Width = width_Step; //-Horizontaljiange / 2;//图片初始宽度
                showitem_Height = Height_Step;// -Verticaljiange / 2;//图片初始高度
                if (showitem_Width > showitem_Height)
                {
                    showitem_Width = showitem_Height;
                }

                switch (m_Item.m_showtype)
                {
                    case (int)ShowItemType.CNC:
                        switch (m_Item.m_showitemserial)
                        {
                            case 0:
                                showitem_Left = LeftStar + width_Step * 3 + Horizontaljiange + HorizontalSeg;
                                showitem_Top = TopStar + Height_Step * 0 + Verticaljiange * 6 - TopStar / 2 - VerticalSeg-100;
                                break;
                            case 1:
                                showitem_Left = LeftStar + width_Step * 1+ Horizontaljiange + HorizontalSeg;
                                showitem_Top = TopStar + Height_Step * 0 + Verticaljiange *6 - TopStar / 2 - VerticalSeg- 100;
                                break;
                            case 2:
                                showitem_Left = LeftStar + width_Step * 5 + Horizontaljiange + HorizontalSeg;
                                showitem_Top = TopStar + Height_Step * 0 + Verticaljiange * 6 - TopStar / 2 + VerticalSeg - 100;
                                break;
                            case 3:
                                showitem_Left = LeftStar + width_Step * 7 + Horizontaljiange + HorizontalSeg;
                                showitem_Top = TopStar + Height_Step * 0 + Verticaljiange * 6 - TopStar / 2 + VerticalSeg -100;
                                break;
                            default:
                                break;
                        }
                        
                        m_Item.ShowItem2window(this, showitem_Width, showitem_Height, showitem_Left, showitem_Top, 0);
                        break;
                    case (int)ShowItemType.ROBOT:
                        switch (m_Item.m_showitemserial)
                        {
                            case 0:
                                showitem_Left = LeftStar+width_Step * 3 + Horizontaljiange - HorizontalSeg;
                                showitem_Top = TopStar + Height_Step*1 + Verticaljiange * 6-TopStar/2 -Verticaljiange;
                                break;
                            case 1:
                                showitem_Left = LeftStar + width_Step * 1 + Horizontaljiange - HorizontalSeg;;
                                showitem_Top = TopStar + Height_Step * 1 + Verticaljiange * 6  - TopStar / 2  - Verticaljiange ;
                                break;
                            case 2:
                                showitem_Left = LeftStar + width_Step * 5 + Horizontaljiange - HorizontalSeg;
                                showitem_Top = TopStar + Height_Step * 1 + Verticaljiange * 6  - TopStar / 2 - Verticaljiange;
                                break;
                            case 3:
                                showitem_Left = LeftStar + width_Step * 7 + Horizontaljiange - HorizontalSeg;
                                showitem_Top = TopStar + Height_Step * 1 + Verticaljiange * 6 - TopStar / 2 -Verticaljiange;

                                break;
                            default:
                                break;
                        }
                        m_Item.ShowItem2window(this, showitem_Width, showitem_Height, showitem_Left, showitem_Top, 0);
                        break;
                    case (int)ShowItemType.RFID: //CheckEq
                        showitem_Left = LeftStar + width_Step *3 + Horizontaljiange / 2;
                        showitem_Top = TopStar + Height_Step *3 + 50 + Verticaljiange * 5 - 3 * TopStar + VerticalSeg * 3;
                        m_Item.ShowItem2window(this, showitem_Width, showitem_Height, showitem_Left, showitem_Top, 0);
                        break;
                    //case (int)ShowItemType.Storehouse:
                    //    showitem_Width = width_Step * 2 - Horizontaljiange / 2;
                    //    showitem_Height = Height_Step * 2 - 6 * Verticaljiange;
                    //    showitem_Left = LeftStar;
                    //    showitem_Top = TopStar;
                    //    m_Item.ShowItem2window(this, showitem_Width, showitem_Height, showitem_Left, showitem_Top, 0);
                    //    break;
                    case (int)ShowItemType.Measure: //AGV
                        showitem_Left = LeftStar + width_Step * 5 + Horizontaljiange /2;
                        showitem_Top = TopStar + Height_Step * 3 + 50 + Verticaljiange * 5 -3 * TopStar + HorizontalSeg * 3;
                        m_Item.ShowItem2window(this, showitem_Width, showitem_Height, showitem_Left, showitem_Top, 0);
                        break;
                    case (int)ShowItemType.Storage://Conveyor: 料仓
                        showitem_Width = width_Step * 1 ;
                        showitem_Height = Height_Step - Verticaljiange / 2;
                        showitem_Left = LeftStar + width_Step * 1 - Horizontaljiange / 2;
                        showitem_Top = TopStar + Height_Step * 3 + 50 + Verticaljiange * 5 - 3 * TopStar + HorizontalSeg * 3;
                        m_Item.ShowItem2window(this, showitem_Width, showitem_Height, showitem_Left, showitem_Top, 0);
                        break;
                    default:
                        break;
                }
                
            }
        }

        //private void btnCNC_MouseMove(object sender, MouseEventArgs e)
        //{
        //    pictureBox4.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picboxCNCSel.jpg");
        //}

        //private void picBoxCNC_MouseLeave_1(object sender, EventArgs e)
        //{
        //    pictureBox4.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\home.png");
        //}

        //private void btnGC_MouseMove(object sender, MouseEventArgs e)
        //{
        //    pictureBox4.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picGCSel.jpg");
        //}

        //private void picBoxGC_MouseLeave(object sender, EventArgs e)
        //{
        //    picBoxGC.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picGC.jpg");
        //}

        //private void btnRobot_MouseMove(object sender, MouseEventArgs e)
        //{
        //    pictureBox4.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picBoxRobotSel.jpg");
        //}

        //private void picBoxRob_MouseLeave(object sender, EventArgs e)
        //{
        //    picBoxRob.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picBoxRobot.jpg");
        //}

        //private void btnRack_MouseMove(object sender, MouseEventArgs e)
        //{
        //    pictureBox4.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picBoxReckSel.jpg");
        //}

        //private void pictureBoxRack_MouseLeave(object sender, EventArgs e)
        //{
        //    pictureBoxReck.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picBoxReck.jpg");
        //}

        //private void btnRFID_MouseMove(object sender, MouseEventArgs e)
        //{
        //    pictureBox4.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picRFIDSel.jpg");
        //}

        //private void pictureBoxRFID_MouseLeave(object sender, EventArgs e)
        //{
        //    pictureBoxRFID.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picRFID.jpg");
        //}

        //private void btnPC_MouseMove(object sender, MouseEventArgs e)
        //{
        //    pictureBox4.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picPCSel.jpg");
        //}

        //private void pictureBoxPC_MouseLeave(object sender, EventArgs e)
        //{
        //    pictureBoxPC.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\picPC.jpg");
        //}

        private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            //string image="";
            if (btnCNC.Name == btn.Name)
            {
                //image = "..\\picture\\CNCSel.jpg";
                btnCNC.FlatAppearance.BorderSize = 0;
                btnCNC.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\CNC1.png");

                btnRobot.Visible = false;
            }
            else if (btnRobot.Name == btn.Name)
            {
                //image = "..\\picture\\RobotSel.jpg";
                btnRobot.FlatAppearance.BorderSize = 0;
                btnRobot.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\Robot1.png");

                btnCNC.Visible = false;
                btnRack.Visible = false;
                btnPC.Visible = false;
            }
            else if (btnRFID.Name == btn.Name)
            {
                //image = "..\\picture\\RFIDSel.jpg";
                btnRFID.FlatAppearance.BorderSize = 0;
                btnRFID.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\RFID.png");
            }
            else if (btnRack.Name == btn.Name)
            {
                //image = "..\\picture\\RackSel.jpg";
                btnRack.FlatAppearance.BorderSize = 0;
                btnRack.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\Rack_home1.png");

                btnRobot.Visible = false;
                //btnRFID.Visible = false;
                btnPC.Visible = false;
            }
            else if (btnGC.Name == btn.Name)
            {
                //image = "..\\picture\\GCSel.jpg";
                btnGC.FlatAppearance.BorderSize = 0;
                btnGC.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\GC.png");
            }
            else if (btnPC.Name == btn.Name)
            {
                //image = "..\\picture\\PCSel.jpg";
                btnPC.FlatAppearance.BorderSize = 0;
                btnPC.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\PC.png");
            }
            //if (image.Length > 0)
            //    pictureBox4.BackgroundImage = System.Drawing.Image.FromFile(image);
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            //PictureBox pBox = (PictureBox)sender;
            //pictureBox4.BackgroundImage = System.Drawing.Image.FromFile("..\\picture\\homePage.jpg");
            Button btn = (Button)sender;
            if (btnCNC.Name == btn.Name)
            {
                btnCNC.FlatAppearance.BorderSize = 0;
                btnCNC.BackgroundImage = null;

                btnRobot.Visible = true;
            }
            else if (btnRobot.Name == btn.Name)
            {
                btnRobot.FlatAppearance.BorderSize = 0;
                btnRobot.BackgroundImage = null;

                btnCNC.Visible = true;
                btnRack.Visible = true;
                btnPC.Visible = true;
            }
            else if (btnRFID.Name == btn.Name)
            {
                btnRFID.FlatAppearance.BorderSize = 0;
                btnRFID.BackgroundImage = null;
            }
            else if (btnRack.Name == btn.Name)
            {
                btnRack.FlatAppearance.BorderSize = 0;
                btnRack.BackgroundImage = null;

                btnRobot.Visible = true;
                //btnRFID.Visible = true;
                btnPC.Visible = true;
            }
            else if (btnGC.Name == btn.Name)
            {
                btnGC.FlatAppearance.BorderSize = 0;
                btnGC.BackgroundImage = null;
            }
            else if (btnPC.Name == btn.Name)
            {
                btnPC.FlatAppearance.BorderSize = 0;
                btnPC.BackgroundImage = null;
            }
        }

        private void btnCNC_Click(object sender, EventArgs e)
        {            
            PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + (int)ShowItemType.CNC, 1, 0);
            pictureBox4.Focus();
        }

        private void btnRob_Click(object sender, EventArgs e)
        {
            PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + (int)ShowItemType.ROBOT, 2, 0);
            pictureBox4.Focus();
        }

        private void btnRack_Click(object sender, EventArgs e)
        {
            PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + 13, 5, 0);
            pictureBox4.Focus();
        }

        private void btnRFID_Click(object sender, EventArgs e)
        {
            PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + 14, 4, 0);
            pictureBox4.Focus();
        }

        private void btnGC_Click(object sender, EventArgs e)
        {
            PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + 15, 6, 0);
            pictureBox4.Focus();
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            PostMessage(MainForm.mainform_Ptr, MainForm.USERMESSAGE + 16, 3, 0);
            pictureBox4.Focus();
        }

 

    }      
}


