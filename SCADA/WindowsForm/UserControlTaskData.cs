﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LineDevice;
using System.Xml;
using ScadaHncData;
using System.IO;
using System.Collections;

namespace SCADA
{

    public partial class UserControlTaskData : UserControl
    {
        public ShareData TaskDataObj; //2015.10.24
        const string taskListPathXml = ".\\TaskList"; //2015.10.25
        const string cncTaskListXml = "\\CNCTaskList.xml";
        const string oldcncTaskListXml = "\\oldCNCTaskList.xml";

        const int resendNum = 3; //文件发送失败后的重发次数
        const int maxFileSize = 2; //发送单个文件大小最大默认为2M

       string[] STR_dataGridView_CNCGCode_Columns = { "CNC机台号","下载情况", "备注" };
        string[] STR_dataGridView_GCodeSele_Columns = { "G代码路径", "备注"};
        string[] MessageboxStr_登录 = {"你的操作权限不够，请先登录","提示" };
        private Color sendGcodeBackColor = Color.Chocolate;
        private Color frezenBackColor = Color.BurlyWood;
        //System.Data.DataTable TaskDb;
        System.Data.DataTable dataGridView_CNCGCodeDb = new DataTable();
        System.Data.DataTable dataGridView_GCodeSeleDb = new DataTable();


        public UserControlTaskData()
        {
            InitializeComponent();
            comboBoxtype.Items.Add("1:车床");
            comboBoxtype.Items.Add("2:加工中心");
            comboBoxtype.Items.Add("3:车床+加工中心");
            comboBoxtype.Items.Add("4:加工中心+车床");
            dataGridView_CNCGCode.AllowUserToAddRows = false;
            dataGridView_CNCGCode.ReadOnly = false;
            dataGridView_CNCGCode.RowHeadersVisible = false;

            dataGridView_GCodeSele.AllowUserToAddRows = false;
            dataGridView_GCodeSele.ReadOnly = false;
            dataGridView_GCodeSele.RowHeadersVisible = false;

            DataGridViewCheckBoxColumn dtCheck = new DataGridViewCheckBoxColumn();
            dtCheck.HeaderText = "选择";
            dtCheck.ReadOnly = false;
            dtCheck.Selected = false;

            dtCheck = new DataGridViewCheckBoxColumn();
            dtCheck.HeaderText = "选择";
            dtCheck.ReadOnly = false;
            dtCheck.Selected = false;
            dataGridView_CNCGCode.Columns.Insert(0, dtCheck);
            dataGridView_CNCGCode.Columns[0].Frozen = true;
            this.dataGridView_CNCGCode.DefaultCellStyle.WrapMode = DataGridViewTriState.True;//设置自动换行
            for (int ii = 0; ii < STR_dataGridView_CNCGCode_Columns.Length; ii++)
            {
                dataGridView_CNCGCodeDb.Columns.Add(STR_dataGridView_CNCGCode_Columns[ii]);
            }
            dataGridView_CNCGCode.DataSource = dataGridView_CNCGCodeDb;

            dtCheck = new DataGridViewCheckBoxColumn();
            dtCheck.HeaderText = "选择";
            dtCheck.ReadOnly = false;
            dtCheck.Selected = false;
            dataGridView_GCodeSele.Columns.Insert(0, dtCheck);
            dataGridView_GCodeSele.Columns[0].Frozen = true;
            this.dataGridView_GCodeSele.DefaultCellStyle.WrapMode = DataGridViewTriState.True;//设置自动换行
            this.dataGridView_GCodeSele.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //设置自动调整高度
            for (int ii = 0; ii < STR_dataGridView_GCodeSele_Columns.Length; ii++)
            {
                dataGridView_GCodeSeleDb.Columns.Add(STR_dataGridView_GCodeSele_Columns[ii]);
            }
            dataGridView_GCodeSele.DataSource = dataGridView_GCodeSeleDb;
            ExChangeLanguage();
        }

        public void ExChangeLanguage()
        {
            button_AddGCode.Text = ChangeLanguage.GetString("button_AddGCode");
            button_DeletGCode.Text = ChangeLanguage.GetString("button_DeletGCode");
            button_DowLoadGCode.Text = ChangeLanguage.GetString("button_DowLoadGCode");
            label1.Text = ChangeLanguage.GetString("label1");
            label2.Text = ChangeLanguage.GetString("label2");
            label3.Text = ChangeLanguage.GetString("label3");
            labelmagno.Text = ChangeLanguage.GetString("labelmagno");
            labeltype.Text = ChangeLanguage.GetString("labeltype");
            buttondingdanxiada.Text = ChangeLanguage.GetString("buttondingdanxiada");
            buttondingdanquxiao.Text = ChangeLanguage.GetString("buttondingdanquxiao");
            dataGridView_CNCGCode.Columns[0].HeaderText = ChangeLanguage.GetString("CNCGcodeColumn01");
            dataGridView_CNCGCode.Columns[1].HeaderText = ChangeLanguage.GetString("CNCGcodeColumn02");
            dataGridView_CNCGCode.Columns[2].HeaderText = ChangeLanguage.GetString("CNCGcodeColumn03");
            dataGridView_CNCGCode.Columns[3].HeaderText = ChangeLanguage.GetString("CNCGcodeColumn04");
            dataGridView_GCodeSele.Columns[0].HeaderText = ChangeLanguage.GetString("GcodeSeleColumn01");
            dataGridView_GCodeSele.Columns[1].HeaderText = ChangeLanguage.GetString("GcodeSeleColumn02");
            dataGridView_GCodeSele.Columns[2].HeaderText = ChangeLanguage.GetString("GcodeSeleColumn03");
        }

        private void UserControlTaskData_Load(object sender, EventArgs e)
        {
            dataGridView_CNCGCode.Columns[0].Width = this.dataGridView_CNCGCode.Width / 5;
            dataGridView_CNCGCode.Columns[1].Width = this.dataGridView_CNCGCode.Width / 5;
            dataGridView_CNCGCode.Columns[2].Width = this.dataGridView_CNCGCode.Width * 2 / 5;
            dataGridView_CNCGCode.Columns[3].Width = this.dataGridView_CNCGCode.Width - this.dataGridView_CNCGCode.Width * 4 / 5; 

            dataGridView_GCodeSele.Columns[0].Width = this.dataGridView_GCodeSele.Width / 5;
            dataGridView_GCodeSele.Columns[1].Width = this.dataGridView_GCodeSele.Width * 2 / 5;
            dataGridView_GCodeSele.Columns[2].Width = this.dataGridView_GCodeSele.Width - this.dataGridView_GCodeSele.Width * 3 / 5;  

            if (MainForm.cnclist != null && MainForm.cnclist.Count > 0)
            {
                for (int ii = 0; ii < MainForm.cnclist.Count; ii++)
                {
                    string[] array = new string[STR_dataGridView_CNCGCode_Columns.Length];
                    array[0] = MainForm.cnclist[ii].BujianID;
                    array[1] = "";
                    array[2] = "";
                    dataGridView_CNCGCodeDb.Rows.Add(array);

                }
            }
        }   

         private void button_DeletGCode_Click(object sender, EventArgs e)
         {
             System.Data.DataTable Db = new DataTable();
             for (int ii = 0; ii < STR_dataGridView_GCodeSele_Columns.Length; ii++)
             {
                 Db.Columns.Add(STR_dataGridView_GCodeSele_Columns[ii]);
             }
             for (int ii = 0; ii < dataGridView_GCodeSeleDb.Rows.Count; ii++)
             {
                 if (!(bool)dataGridView_GCodeSele.Rows[ii].Cells[0].EditedFormattedValue)
                 {
                     string[] array = new string[STR_dataGridView_GCodeSele_Columns.Length];
                     array[0] = dataGridView_GCodeSeleDb.Rows[ii][0].ToString();
                     array[1] = dataGridView_GCodeSeleDb.Rows[ii][1].ToString();
                     Db.Rows.Add(array);
                 }
             }
             dataGridView_GCodeSeleDb = Db;
             dataGridView_GCodeSele.DataSource = null;
             dataGridView_GCodeSele.DataSource = dataGridView_GCodeSeleDb;
         }
         private void button_AddGCode_Click(object sender, EventArgs e)
         {
             try
             {
                 OpenFileDialog fileDialog = new OpenFileDialog();
                 fileDialog.RestoreDirectory = true; //记忆上次浏览路径
                 fileDialog.Multiselect = true;
                 fileDialog.Title = ChangeLanguage.GetString("SelectFileTitle");
                 fileDialog.Filter = ChangeLanguage.GetString("SelectFileFilter");

                 if (fileDialog.ShowDialog() == DialogResult.OK)
                 {
                     dataGridView_GCodeSele.DataSource = null;
                     foreach (string filaname in fileDialog.FileNames)
                     {
                         string[] array = new string[STR_dataGridView_GCodeSele_Columns.Length];
                         array[0] = filaname;
                         array[1] = "";
                         dataGridView_GCodeSeleDb.Rows.Add(array);
                     }
                     dataGridView_GCodeSele.DataSource = null;
                     dataGridView_GCodeSele.DataSource = dataGridView_GCodeSeleDb;
                 }
             }
             catch
             {

             }
         }

         private void button_jitaiquanxuan_Click(object sender, EventArgs e)
         {
             for (int ii = 0; ii < dataGridView_CNCGCode.Rows.Count; ii++)
             {
                 dataGridView_CNCGCode.Rows[ii].Cells[0].Value = true;
             }
         }
         private void button_jitaiquanbuxuan_Click(object sender, EventArgs e)
         {
             for (int ii = 0; ii < dataGridView_CNCGCode.Rows.Count; ii++)
             {
                 dataGridView_CNCGCode.Rows[ii].Cells[0].Value = false;
             }
         }
         private void button_GCodequanbuxuan_Click(object sender, EventArgs e)
         {
             for (int ii = 0; ii < dataGridView_GCodeSele.Rows.Count; ii++)
             {
                 dataGridView_GCodeSele.Rows[ii].Cells[0].Value = false;
             }
         }
         private void button_GCodequanxuan_Click(object sender, EventArgs e)
         {
             for (int ii = 0; ii < dataGridView_GCodeSele.Rows.Count; ii++)
             {
                 dataGridView_GCodeSele.Rows[ii].Cells[0].Value = true;
             }
         }

         private void button_DowLoadGCode_Click(object sender, EventArgs e)
         {
             if (SetForm.LogIn)
             {
                 string str3 = "";
                 for (int ii = 0; ii < dataGridView_CNCGCode.Rows.Count;ii++ )
                 {
                     if ((bool)dataGridView_CNCGCode.Rows[ii].Cells[0].EditedFormattedValue)
                     {
                         string str1 = "",str2 = "";
                         for (int jj = 0; jj < dataGridView_GCodeSele.Rows.Count; jj++)
                         {
                             if ((bool)dataGridView_GCodeSele.Rows[jj].Cells[0].EditedFormattedValue)
                             {
                                 string[] filename = dataGridView_GCodeSeleDb.Rows[jj][0].ToString().Split('\\');
                                 if (MainForm.cnclist[ii].sendFile(dataGridView_GCodeSeleDb.Rows[jj][0].ToString(), "h/lnc8/prog/" + filename[filename.Length - 1], 0, false) == 0)
                                 {
                                     str1 += filename[filename.Length - 1] + "；";
                                 }
                                 else
                                 {
                                     str2 += filename[filename.Length - 1] + "；";
                                 }
                             }
                         }
                         if (str1.Length > 0)
                         {
                             str1 = ChangeLanguage.GetString("LoadSuccess01") + str1;
                         }
                         if (str2.Length > 0)
                         {
                             str2 = ChangeLanguage.GetString("LoadFailed01") + str2;
                         }
                         dataGridView_CNCGCodeDb.Rows[ii][1] = str1 + "\r\n" + str2;
                         if(str2.Length > 0)
                         {
                             dataGridView_CNCGCodeDb.Rows[ii][2] = DateTime.Now.ToString() + ":" + ChangeLanguage.GetString("LoadFailed02");
                             str3 += dataGridView_CNCGCodeDb.Rows[ii][0] + ":" + str2;
                         }
                         else
                         {
                             dataGridView_CNCGCodeDb.Rows[ii][2] = DateTime.Now.ToString() + ":" + ChangeLanguage.GetString("LoadSuccess02");
                         }
                     }
                 }
                 if (str3.Length > 0)
                 {
                     MessageBox.Show(str3, MessageboxStr_登录[1], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 }
             }
             else
             {
                 MessageBox.Show(MessageboxStr_登录[0], MessageboxStr_登录[1], MessageBoxButtons.OK, MessageBoxIcon.Warning);
             }
         }
         private int getnumformstring(string str)
         {
             int num;
             int[] str1 = { 0, 0 };
             for (int i = 0; i < str.Length; i++)
             {
                 char item = str.ElementAt(i);
                 if (item <= '9' && item >= '0')
                 {
                     str1[1] = str1[0];
                     str1[0] = (int)(item) - (int)('0');
                 }
                 else
                     return -1;
             }
             return num = str1[1] * 10 + str1[0];
         }
         private void textBoxMagno_Leave(object sender, EventArgs e)
         {
              string MagNoStr = ((TextBox) sender).Text;
               int MagNo= getnumformstring(MagNoStr);
             if(MagNo>30||MagNo<0)
             {
                 MessageBox.Show("请输入1-30之间的数字");
                 textBoxMagno.Focus();
             }
             else
             {
                 int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;
                 int maglength = (int)ModbusTcp.MagLength;
                 int magstatei = magstart + maglength * MagNo -1;
                if(ModbusTcp.DataMoubus[magstatei] == (int)SCADA.ModbusTcp.Mag_state_config.Statewait)//仓位待加工
                 {
                     return;
                 } 
                  else 
                 {
                     return;
                     //MessageBox.Show("当前仓位不是待加工，请重新派单");
                    // textBoxMagno.Focus();
                 }
             }

         }

         private void textBoxMagno_KeyPress(object sender, KeyPressEventArgs e)
         {
             if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back||e.KeyChar == (char)Keys.Enter)
             {
                 e.Handled = false;
             }
             else
             {
                 MessageBox.Show("请输入数字");
                 textBoxMagno.Focus();
             }
         }
        /// <summary>
        /// 绑定料仓与工艺，对plc下达加工指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void buttondingdanxiada_Click(object sender, EventArgs e)
         {
             string MagNoStr = textBoxMagno.Text;
             int MagNo = getnumformstring(MagNoStr);
             string gongyiType = comboBoxtype.Text;
             int gongyiNo = 0;
             int MesReq_order = (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm;
             int MesReq_order_code = (int)ModbusTcp.MesCommandToPlc.ComProcessManage;
             int Mesreq_Rack_number = (int)ModbusTcp.DataConfigArr.Rack_number_comfirm;
             int Mesreq_Order_type = (int)ModbusTcp.DataConfigArr.Order_type_comfirm;
             int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
             int maglength = (int)ModbusTcp.MagLength;
             int magstatei = magstart + maglength * (MagNo-1) ;
             int robortishome = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm];
             if (MainForm.linestart==true)
             {

                 MessageBox.Show("订单下达失败，请启动产线!");
                 return; 
             }
             if (ModbusTcp.DataMoubus[magstatei] != (int)ModbusTcp.Mag_state_config.Statewait)
             {
                 MessageBox.Show("指定仓位不是待加工状态，请重新下达订单!");
                 return;
             }
             if (robortishome == 0)
             {
                 MessageBox.Show("机器人不在HOME位置无法下达订单！");
                 return;
             }
             if (gongyiType == "1:车床")//规定人社部车床为1号机床，加工中心为2号机床
             {               
                     if(MainForm.cnclist[0].isConnected() == true)//20180312
                 {
                      if (MainForm.cnclist[0].MagNo == 0)//机床状态判定，如果机床占用，不可下单 //20180312  
                     {
                         MainForm.cnclist[0].MagNo = MagNo;//机床绑定料仓号
                         gongyiNo = 1;
                         ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                         ModbusTcp.DataMoubus[Mesreq_Rack_number] = MagNo;//料仓编号
                         ModbusTcp.DataMoubus[Mesreq_Order_type] = gongyiNo;//加工类型
                         ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                       
                     }
                      else MessageBox.Show("车床有未完成订单，本次订单下达失败");    //20180312           
                 }
                 else MessageBox.Show("车床离线，本次订单下达失败");    //20180312                      
             }
             else if (gongyiType == "2:加工中心")
             {
                if (MainForm.cnclist[1].isConnected() == true)//20180312 
                 {
                     if (MainForm.cnclist[1].MagNo == 0)//机床状态判定，如果机床占用，不可下单   //20180312 
                     {
                         MainForm.cnclist[1].MagNo = MagNo;//机床绑定料仓号
                         gongyiNo = 2;
                         ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                         ModbusTcp.DataMoubus[Mesreq_Rack_number] = MagNo;//料仓编号
                         ModbusTcp.DataMoubus[Mesreq_Order_type] = gongyiNo;//加工类型
                         ModbusTcp.Rack_number_write_flage = true;//订单下发标识                    
                     }
                     else MessageBox.Show("加工中心有未完成订单，本次订单下达失败");   //20180312 
                 }
                else MessageBox.Show("加工中心离线，本次订单下达失败");      //20180312
             }
             else if (gongyiType == "3:车床+加工中心")
             {              
               if (MainForm.cnclist[0].isConnected() == true&&MainForm.cnclist[0].isConnected() )
                 {
                     if (MainForm.cnclist[0].MagNo != 0 && MainForm.cnclist[0].MagNo == 0)
                     {
                         MessageBox.Show("车床有未完成订单，本次订单下达失败");
                     }
                     else if (MainForm.cnclist[1].MagNo == 0 && MainForm.cnclist[0].MagNo != 0)
                     {
                         MessageBox.Show("加工中心有未完成订单，本次订单下达失败");
                     }
                     else if (MainForm.cnclist[1].MagNo == 0 && MainForm.cnclist[0].MagNo == 0)//机床状态判定，如果机床占用，不可下单
                     {
                         MainForm.cnclist[1].MagNo = MagNo;//机床绑定料仓号
                         MainForm.cnclist[0].MagNo = MagNo;//机床绑定料仓号
                         gongyiNo = 3;
                         ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                         ModbusTcp.DataMoubus[Mesreq_Rack_number] = MagNo;//料仓编号
                         ModbusTcp.DataMoubus[Mesreq_Order_type] = gongyiNo;//加工类型
                         ModbusTcp.Rack_number_write_flage = true;//订单下发标识           
                     }
                     else
                         MessageBox.Show("车床和加工中心有未完成订单，本次订单下达失败");
                 }
                  else MessageBox.Show("车床或者加工中心离线，本次订单下达失败");
             }
             else if (gongyiType == "4:加工中心+车床")
             {             
               if (MainForm.cnclist[0].isConnected() == true && MainForm.cnclist[0].isConnected())
                 {
                     if (MainForm.cnclist[0].MagNo != 0 && MainForm.cnclist[0].MagNo == 0)
                     {
                         MessageBox.Show("车床有未完成订单，本次订单下达失败");
                     }
                     else if (MainForm.cnclist[1].MagNo == 0 && MainForm.cnclist[0].MagNo != 0)
                     {
                         MessageBox.Show("加工中心有未完成订单，本次订单下达失败");
                     }
                     else if (MainForm.cnclist[1].MagNo == 0 && MainForm.cnclist[0].MagNo == 0)//机床状态判定，如果机床占用，不可下单
                     {
                         ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                         MainForm.cnclist[1].MagNo = MagNo;//机床绑定料仓号
                         MainForm.cnclist[0].MagNo = MagNo;//机床绑定料仓号
                         gongyiNo = 4;
                         //下发任务到plc
                         ModbusTcp.DataMoubus[Mesreq_Rack_number] = MagNo;//料仓编号
                         ModbusTcp.DataMoubus[Mesreq_Order_type] = gongyiNo;//加工类型
                         ModbusTcp.Rack_number_write_flage = true;//订单下发标识                               
                     }
                     else
                         MessageBox.Show("车床和机床有未完成订单，本次订单下达失败");
                 }
                 else MessageBox.Show("车床或者加工中心离线，本次订单下达失败");            
             }
             else
             {
                 MessageBox.Show("订单配置错误，本次订单下达失败");
             }
         }

         private void buttondingdanquxiao_Click(object sender, EventArgs e)
         {
            MainForm.RebackOrder();
         }

         private void timercncstate_Tick(object sender, EventArgs e)
         {
              
                if (MainForm.cnclist.Count > 0 )
             {
                 string language = ChangeLanguage.GetDefaultLanguage();
                if (MainForm.cnclist[0].isConnected() == false)
                {
                    if (language == "English")
                    {
                        this.label4.Text = "Off-line";
                    }
                    else
                    {
                        this.label4.Text = "离线";
                    }
                }
                else
                {
                    if (MainForm.cnclist[0].MagNo != 0)
                    {
                        string temp = MainForm.cnclist[0].MagNo.ToString();
                        if (language == "English")
                        {
                            temp = "No." + temp + "is machining";
                            this.label4.Text = temp;
                        }
                        else
                        {
                            temp = "No." + temp + "加工中";
                            this.label4.Text = temp;
                        }

                    }
                    else
                    {
                        if (language == "English")
                        {
                            this.label4.Text = "idle";
                        }
                        else
                        {
                            this.label4.Text = "空闲";
                        }
                        
                    }

                }


                if (MainForm.cnclist[1].isConnected() == false)
                {
                    if (language == "English")
                    {
                        this.label5.Text = "Off-line";
                    }
                    else
                    {
                        this.label5.Text = "离线";
                    }
                }
                else
                {
                    if (MainForm.cnclist[1].MagNo != 0)
                    {
                        string temp = MainForm.cnclist[1].MagNo.ToString();
                        if (language == "English")
                        {
                            temp = "No." + temp + "is machining";
                            this.label5.Text = temp;
                        }
                        else
                        {
                            temp =  "No." + temp + "加工中";
                            this.label5.Text = temp;
                        }

                    }
                    else
                    {
                        if (language == "English")
                        {
                            this.label5.Text = "idle";
                        }
                        else
                        {
                            this.label5.Text = "空闲";
                        }
                    }
                }
             }
             else 
             return ;
         }
 
        
    } 
}


