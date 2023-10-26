using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Collector;
using HNC_MacDataService;
using HNCAPI;
// using LineDevice;

using PLC;



namespace SCADA
{
    public partial class PlcForm : Form
    {
       PLC_MITSUBISHI_HNC8 plc = null;
       public static int plcindex = 0;
       public static bool isinalarmform = false;
       Color[] Bt_bgcoler = { System.Drawing.Color.FromArgb(255, 251, 240), System.Drawing.Color.FromArgb(0, 255, 0), System.Drawing.Color.FromArgb(0, 0, 255), System.Drawing.Color.FromArgb(255, 0, 0) };//按钮颜色
        //1.声明自适应类
//        AutoSizeFormClass asc = new AutoSizeFormClass();
       #region  HNC8 PLC按钮设置
       public String[] PLCStationNameArr = { "测量站", "加工站", "移料机构", "料仓" };
//        int PLCStationButtonNameArr1Lenth = 13;
       int PLCStationButtonNameArr2Lenth = 10;
       public static String[,] PLCStationButtonNameArr = new String[4, 10] { 
                                                                { 
                                                                    "RFID阻挡气缸上升=R500.0;RFID阻挡气缸上升=R11.0", 
                                                                    "RFID阻挡气缸下降=R500.1;RFID阻挡气缸下降=R11.1", 
                                                                    "顶升1上升=R500.2;顶升1上升=R11.2",
                                                                    "顶升1下降=R500.3;顶升1下降=R11.3",
                                                                    "顶升1阻挡上升=R500.4;顶升1阻挡上升=R11.4", 
                                                                    "顶升1阻挡下降=R500.5;顶升1阻挡下降=R11.5",
                                                                    "顶升2上升=R500.6;顶升2上升=R11.6",
                                                                    "顶升2下降=R500.7;顶升2下降=R11.7",
                                                                    "顶升2阻挡上升=R501.0;顶升2阻挡上升=R12.0",
                                                                    "顶升2阻挡下降=R501.1;顶升2阻挡下降=R12.1"
                                                                },//测量站
                                                                { 
                                                                     "RFID阻挡气缸上升=R507.0;RFID阻挡气缸上升=R71.0", 
                                                                    "RFID阻挡气缸下降=R507.1;RFID阻挡气缸下降=R71.1", 
                                                                    "顶升1上升=R507.2;顶升1上升=R71.2",
                                                                    "顶升1下降=R507.3;顶升1下降=R71.3",
                                                                    "顶升1阻挡上升=R507.4;顶升1阻挡上升=R71.4", 
                                                                    "顶升1阻挡下降=R507.5;顶升1阻挡下降=R71.5",
                                                                    "顶升2上升=R507.6;顶升2上升=R71.6",
                                                                    "顶升2下降=R507.7;顶升2下降=R71.7",
                                                                    "顶升2阻挡上升=R508.0;顶升2阻挡上升=R72.0",
                                                                    "顶升2阻挡下降=R508.1;顶升2阻挡下降=R72.1"
                                                                },//加工站
                                                                { 
                                                                    "移料机构上升=R510.0;移料机构上升=R133.0", 
                                                                    "移料机构下降=R510.1;移料机构下降=R133.1", 
                                                                    "移料机构左移=R510.2;移料机构左移=R133.2",
                                                                    "移料机构右移=R510.3;移料机构右移=R133.3",
                                                                    "移料机构手爪夹紧=R510.4;移料机构手爪夹紧=R133.4",
                                                                    "移料机构手爪松开=R510.5;移料机构手爪松开=R133.5",
                                                                    "","","",""
                                                                },//移料机构
                                                                { 
                                                                    "初始化A料盘=R513.0;初始化A料盘=R513.0",
                                                                    "初始化B料盘=R513.1;初始化B料盘=R513.1",
                                                                    "AGV车测试=R513.2;AGV车测试=R513.2",
                                                                    "","","","","","",""
                                                                },//料仓
       };
       OperationButton[] OperationButtonArr = null;
       #endregion

       public PlcForm()
       {
           InitializeComponent();
           comboBox_PLCStationSelet.Visible = false;
           for (int ii = 0; ii < PLCStationButtonNameArr2Lenth; ii++)
           {
               String buttonName = "button" + (ii + 1).ToString();
               foreach (Control item in this.tabPage_PLCOperation.Controls)//将界面上的按钮隐藏
               {
                   if (buttonName == item.Name)
                   {
                       item.Visible = false;
                   }
               }
           }

           if (PLCDataShare.m_plclist != null && PLCDataShare.m_plclist.Count > 0 && PLCDataShare.m_plclist[0].system == m_xmlDociment.PLC_System[1])//HNC8
           {
               comboBox_PLCStationSelet.Visible = true;
               OperationButtonArr = new OperationButton[PLCStationButtonNameArr2Lenth];
               for (int ii = 0; ii < PLCStationButtonNameArr2Lenth; ii++)
               {
                   String buttonName = "button" + (ii + 1).ToString();
                   foreach (Control item in this.tabPage_PLCOperation.Controls)
                   {
                       if (buttonName == item.Name)
                       {
                           OperationButton m_bt = new OperationButton((Button)item);
                           OperationButtonArr[ii] = m_bt;
                       }
                   }
               }
           }
           else
           {
               CurrentAlarm.Visible = false;
               HistoricalAlarm.Visible = false;
               dataGridViewalarmdata.Visible = false;
               button_alarmReFlesh.Visible = false;
           }

           label_PLCSysTemText.Text = "";

           plcDGV1.AllowUserToAddRows = false;
           plcDGV2.AllowUserToAddRows = false;
           dataGridViewalarmdata.AllowUserToAddRows = false;
       }


        private void AutoUpDataHandler(object sender, PlcEvent Args)
        {
//             if (plc == (PLC_MITSUBISHI_HNC8)sender)
//             {
//                 if (Args.Type == "State")
//                 {
//                     if (Args.Value == 1)//连接上
//                     {
// 
//                     }
//                     else if (Args.Value == 0)//掉线
//                     {
// 
//                     }
//                 }
//             }
        }


        /// <summary>
        /// 委托设置文本控件显示
        /// </summary>
        /// <param name="LB"></param>
        /// <param name="str"></param>
        private void ThreaSetLaBText(Label LB, String str)
        {
//             if (LB.Text != str)
            {
                if (LB.InvokeRequired)//等待异步
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<string> actionDelegate = (x) => { LB.Text = x; };
                    LB.Invoke(actionDelegate, str);
                }
                else
                {
                    LB.Text = str;
                }
            }
        }


        private void PlcForm_Load(object sender, EventArgs e)
        {

            ChangeLanguage.LoadLanguage(this);//zxl 4.19

            //2.在PlcForm_Load中调用类初始化方法,记录控件和窗体的初始化位置和大小
//             asc.controllInitializeSize(this);
            MainForm.plcform_Ptr = this.Handle;
            switchPlc.Items.Clear();

            foreach (PLC_MITSUBISHI_HNC8 m_slplc in PLCDataShare.m_plclist)
            {
                string str = "PLC#";
                str += m_slplc.ID.ToString();
                switchPlc.Items.Add(str);
                m_slplc.AutoUpDataPLCDataHandler += new PLC.PLC_MITSUBISHI_HNC8.EventHandler<PLC.PlcEvent>(this.AutoUpDataHandler);
            }
            
            if (switchPlc.Items.Count != 0)
            {
                switchPlc.SelectedIndex = 0;
            }
            pictureBox_LinkState.Image = SCADA.Properties.Resources.top_bar_black;
            /*if (ChangeLanguage.GetDefaultLanguage() == "Chinese")
                labelLinckText.Text = MainForm.UnLinckedText;
            else if (ChangeLanguage.GetDefaultLanguage() == "English")
                labelLinckText.Text = MainForm.ENUnLinckedText;*/


            radioButton_serjinzhi.Checked = true;
//             if (Localization.HasLang)
//             {
//                 Localization.RefreshLanguage(this);
//                 changeDGV();
//             }
            //设置信号列表内容
            comboBox_PLCStationSelet.DataSource = PLCStationNameArr;
        }

        private void InitOperationButton(int Station)
        {
            for (int ii = 0; ii < PLCStationButtonNameArr2Lenth; ii++)
            {
                String[] str1 = PLCStationButtonNameArr[Station,ii].Split(';');
                if (str1.Length != 2)
                {
                    str1 = new String[2];
                }
                String[] str2 = new String[2]; 
                String[] str3 = new String[2];
                if (str1[0] != null && str1[0].Length > 0)
                {
                    str2 = str1[0].Split('=');
                    if (str2.Length != 2)
                    {
                        str2 = new String[2];
                    }
                }
                if (str1[1] != null && str1[1].Length > 0)
                {
                    str3 = str1[1].Split('=');
                    if (str3.Length != 2)
                    {
                        str3 = new String[2]; 
                    }
                }
                OperationButtonArr[ii].BindingHnc8Reg(str2[0], str2[1], str3[0], str3[1]);
            }
        }

        public System.Drawing.Image GerImage(string path)
        {
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
            byte[] bydata = new byte[fs.Length];
            fs.Read(bydata, 0, bydata.Length);
            fs.Close();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bydata);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }

        private void switchPlc_SelectedIndexChanged(object sender, EventArgs e)
        {
            plc = PLCDataShare.m_plclist[switchPlc.SelectedIndex];
            if (plc.system == m_xmlDociment.PLC_System[0])//三菱PLC
            {
                comboBoxPLCJiankong1.DataSource = m_xmlDociment.Default_MITSUBISHI_Device1;
                comboBoxPLCJiankong2.DataSource = m_xmlDociment.Default_MITSUBISHI_Device2;
            }
            else//HNC8PLC
            {
                comboBoxPLCJiankong1.DataSource = m_xmlDociment.Default_HNC8_Device1;
                comboBoxPLCJiankong2.DataSource = m_xmlDociment.Default_HNC8_Device2;
            }
            label_PLCSysTemText.Text = plc.system;
        }

        /// <summary>
        /// 获取PLC信号并将其列表出来
        /// </summary>
        /// <param name="plc"></param>
        private void getPlcSingalList2plcDGV(String Device, System.Windows.Forms.DataGridView DGV,
            ref PLC_MITSUBISHI_HNC8.MITSUBISHISignalType[] MITSUBISHIPLC_SignalList_result, ref PLC_MITSUBISHI_HNC8.HNC8SignalType[] HNC8PLC_SignalList_result)
        {
            DGV.Rows.Clear();
            if (plc.system == m_xmlDociment.PLC_System[0])//三菱
            {
                if (MITSUBISHIPLC_SignalList_result != null)
                {
                    for (int ii = 1; ii < MITSUBISHIPLC_SignalList_result.Length;ii++ )
                    {
                        if (MITSUBISHIPLC_SignalList_result[ii].IsShow && MITSUBISHIPLC_SignalList_result[ii].ACTION_ID == "-1")
                        {
                            MITSUBISHIPLC_SignalList_result[ii].IsShow = false;
                        }
                    }
                }
                MITSUBISHIPLC_SignalList_result = plc.MITSUBISHIPLC_SignalList.Find(
                        delegate(PLC_MITSUBISHI_HNC8.MITSUBISHISignalType[] temp)
                        {
                            return String.Equals(temp[0].EQUIP_CODE, Device, StringComparison.Ordinal);
                        }
                        );

                if(MITSUBISHIPLC_SignalList_result != null)
                {
                    DGV.RowCount = MITSUBISHIPLC_SignalList_result.Length - 1;
                    for (int ii = 0; ii < DGV.RowCount; ii++)
                    {
                        if (MITSUBISHIPLC_SignalList_result[0].Address == 10)//10进制
                        {
                            DGV.Rows[ii].Cells[0].Value = Device + MITSUBISHIPLC_SignalList_result[ii + 1].Address;
                        }
                        else//十六进制
                        {
                            DGV.Rows[ii].Cells[0].Value = Device + "0x" + String.Format("{0:X}", MITSUBISHIPLC_SignalList_result[ii + 1].Address);
                        }
                        if (MITSUBISHIPLC_SignalList_result[ii + 1].SubAddress != -1)//位
                        {
                            DGV.Rows[ii].Cells[0].Value = DGV.Rows[ii].Cells[0].Value.ToString() + "." + MITSUBISHIPLC_SignalList_result[ii + 1].SubAddress.ToString();
                        }
                        DGV.Rows[ii].Cells[1].Value = MITSUBISHIPLC_SignalList_result[ii + 1].ArrLabel;
                        DGV.Rows[ii].Cells[2].Value = MITSUBISHIPLC_SignalList_result[ii + 1].Value;
                        if (MITSUBISHIPLC_SignalList_result[ii + 1].MonitoringFlg)
                        {
                            DGV.Rows[ii].Cells[3].Value = "是";
                        }
                        else
                        {
                            DGV.Rows[ii].Cells[3].Value = "否";
                        }
                        DGV.Rows[ii].Cells[4].Value = MITSUBISHIPLC_SignalList_result[ii + 1].EQUIP_CODE;
                        DGV.Rows[ii].Cells[5].Value = MITSUBISHIPLC_SignalList_result[ii + 1].ACTION_ID;

                        if (MITSUBISHIPLC_SignalList_result[ii + 1].ACTION_ID == "-1")//显示类型
                        {
                            MITSUBISHIPLC_SignalList_result[ii + 1].IsShow = false;//将所有的显示类型采集属性设置为false
                        }
                        else
                        {
                            MITSUBISHIPLC_SignalList_result[ii + 1].IsShow = true;
                        }
                        
                    }
                }
            }
            else if (plc.system == m_xmlDociment.PLC_System[1])//hnc8
            {
                if (HNC8PLC_SignalList_result != null)
                {
                    for (int ii = 1; ii < HNC8PLC_SignalList_result.Length; ii++)
                    {
                        if (HNC8PLC_SignalList_result[ii].IsShow && HNC8PLC_SignalList_result[ii].ACTION_ID == "-1")
                        {
                            HNC8PLC_SignalList_result[ii].IsShow = false;
                        }
                    }
                }

                HNC8PLC_SignalList_result = plc.HNC8PLC_SignalList.Find(
                        delegate(PLC_MITSUBISHI_HNC8.HNC8SignalType[] temp)
                        {
                            return String.Equals(temp[0].ArrLabel, Device, StringComparison.Ordinal);
                        }
                        );

                if (HNC8PLC_SignalList_result != null)//找到
                {
                    DGV.RowCount = HNC8PLC_SignalList_result.Length - 1;
                    for (int ii = 0; ii < DGV.RowCount; ii++)
                    {
                        if (HNC8PLC_SignalList_result[ii + 1].SubAddress == -1)//是值
                        {
                            DGV.Rows[ii].Cells[0].Value = HNC8PLC_SignalList_result[ii + 1].Address;
                        }
                        else
                        {
                            DGV.Rows[ii].Cells[0].Value = HNC8PLC_SignalList_result[ii + 1].Address.ToString() + "." + HNC8PLC_SignalList_result[ii + 1].SubAddress.ToString();
                        }
                        DGV.Rows[ii].Cells[1].Value = HNC8PLC_SignalList_result[ii + 1].ArrLabel;
                        DGV.Rows[ii].Cells[2].Value = HNC8PLC_SignalList_result[ii + 1].Value;
                        if (HNC8PLC_SignalList_result[ii + 1].MonitoringFlg)
                        {
                            DGV.Rows[ii].Cells[3].Value = "是";
                        }
                        else
                        {
                            DGV.Rows[ii].Cells[3].Value = "否";
                        }
                        DGV.Rows[ii].Cells[4].Value = HNC8PLC_SignalList_result[ii + 1].EQUIP_CODE;
                        DGV.Rows[ii].Cells[5].Value = HNC8PLC_SignalList_result[ii + 1].ACTION_ID;
                        if(HNC8PLC_SignalList_result[ii + 1].ACTION_ID == "-1")
                        {
                            HNC8PLC_SignalList_result[ii + 1].IsShow = false;//将所有的显示类型采集属性设置为false
                        }
                        else
                        {
                            HNC8PLC_SignalList_result[ii + 1].IsShow = true;//将所有的显示类型采集属性设置为false
                        }
                    }
                }
            }
        }

        //3.为窗体添加Sizechanged事件,调用类自适应方法,完成自适应
        private void PlcForm_SizeChanged(object sender, EventArgs e)
        {
            //记录控件的初始位置和大小后,再最大化
//             asc.controlAutoSize(this);
//             this.WindowState = (System.Windows.Forms.FormWindowState)(2);
        }

        private bool DGVUpData1 = false;
        private bool DGVUpData2 = false;
        private String comboBoxPLCJiankongText1;
        private String comboBoxPLCJiankongText2;
        PLC_MITSUBISHI_HNC8.MITSUBISHISignalType[] MITSUBISHIPLC_SignalList_result1;
        PLC_MITSUBISHI_HNC8.MITSUBISHISignalType[] MITSUBISHIPLC_SignalList_result2;
        PLC_MITSUBISHI_HNC8.HNC8SignalType[] HNC8PLC_SignalList_result1;
        PLC_MITSUBISHI_HNC8.HNC8SignalType[] HNC8PLC_SignalList_result2;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (plc != null && plc.conneted)
            {
                /*if (ChangeLanguage.GetDefaultLanguage() == "Chinese")
                {
                    if (labelLinckText.Text != MainForm.LinckedText)
                    {
                        pictureBox_LinkState.Image = SCADA.Properties.Resources.top_bar_green;
                        labelLinckText.Text = MainForm.LinckedText;
                    }
                }
                else if (ChangeLanguage.GetDefaultLanguage() == "English")
                {
                    if (labelLinckText.Text != MainForm.ENLinckedText)
                    {
                        pictureBox_LinkState.Image = SCADA.Properties.Resources.top_bar_green;
                        labelLinckText.Text = MainForm.ENLinckedText;
                    }
                }*/

                if (DGVUpData1)
                {
                    getPlcSingalList2plcDGV(comboBoxPLCJiankongText1, this.plcDGV1, ref MITSUBISHIPLC_SignalList_result1, ref HNC8PLC_SignalList_result1);
                    DGVUpData1 = false;
                }
                if (DGVUpData2)
                {
                    getPlcSingalList2plcDGV(comboBoxPLCJiankongText2, this.plcDGV2, ref MITSUBISHIPLC_SignalList_result2, ref HNC8PLC_SignalList_result2);
                    DGVUpData2 = false;
                }
                UpdatePlcSignal(comboBoxPLCJiankongText1, ref this.plcDGV1, ref MITSUBISHIPLC_SignalList_result1, ref HNC8PLC_SignalList_result1);//刷新X
                UpdatePlcSignal(comboBoxPLCJiankongText2, ref this.plcDGV2, ref MITSUBISHIPLC_SignalList_result2, ref HNC8PLC_SignalList_result2);//刷新Y            
            }

            if (tabControl1.Visible && tabControl1.TabCount > 0)
            {
                if (plcindex != tabControl1.SelectedIndex)
                {
                    tabControl1.SelectedIndex = plcindex;
                }
                else
                {
                    isinalarmform = true;
                }

                if (tabControl1.SelectedIndex != 2)
                {
                    isinalarmform = false;
                }
            }
            else
            {
                isinalarmform = false;
            }
        }

        private void UpdatePlcSignal(String Device, ref System.Windows.Forms.DataGridView DGV,
            ref PLC_MITSUBISHI_HNC8.MITSUBISHISignalType[] MITSUBISHIPLC_SignalList_result, ref PLC_MITSUBISHI_HNC8.HNC8SignalType[] HNC8PLC_SignalList_result) 
        {
//             if (DGV.Visible )
            {

                for (int ii = 0; ii < DGV.RowCount; ii++)
                {
                    if (DGV.Rows[ii].Displayed)
                    {
                        if (plc.system == m_xmlDociment.PLC_System[0] && MITSUBISHIPLC_SignalList_result != null)//三菱
                        {
                            if (!MITSUBISHIPLC_SignalList_result[ii + 1].IsShow)
                            {
                                MITSUBISHIPLC_SignalList_result[ii + 1].IsShow = true;//开启采集
                            }
                            if (Device == m_xmlDociment.Default_MITSUBISHI_Device1[0] ||
                                Device == m_xmlDociment.Default_MITSUBISHI_Device2[0] ||
                                Device == m_xmlDociment.Default_MITSUBISHI_Device1[3] ||
                                MITSUBISHIPLC_SignalList_result[ii + 1].SubAddress != -1)//X、Y、M、点位
                            {
                                DGV.Rows[ii].Cells[2].Value = MITSUBISHIPLC_SignalList_result[ii + 1].Value;
                            }
                            else
                            {
                                int jinzhi = 0;
                                if (radioButton_shijinzhi.Checked)
                                {
                                    jinzhi = 1;
                                }
                                else if (radioButton_shliujinzhi.Checked)
                                {
                                    jinzhi = 2;
                                }
                                DGV.Rows[ii].Cells[2].Value = GetRegValueChage2string(16, MITSUBISHIPLC_SignalList_result[ii + 1].Value, jinzhi);
                            }
                        }
                        else if (plc.system == m_xmlDociment.PLC_System[1] && HNC8PLC_SignalList_result != null)//hnc8
                        {
                            if (!HNC8PLC_SignalList_result[ii + 1].IsShow)
                            {
                                HNC8PLC_SignalList_result[ii + 1].IsShow = true;//开启采集
                            }

                            if (HNC8PLC_SignalList_result[ii + 1].SubAddress == -1)
                            {
                                int jinzhi = 0;
                                if (radioButton_shijinzhi.Checked)
                                {
                                    jinzhi = 1;
                                }
                                else if (radioButton_shliujinzhi.Checked)
                                {
                                    jinzhi = 2;
                                }
                                DGV.Rows[ii].Cells[2].Value = GetRegValueChage2string(HNC8PLC_SignalList_result[0].Value, HNC8PLC_SignalList_result[ii + 1].Value, jinzhi);
                            }
                            else
                            {
                                DGV.Rows[ii].Cells[2].Value = HNC8PLC_SignalList_result[ii + 1].Value;
                            }
                        }
                    }
                    else
                    {
                        if (plc.system == m_xmlDociment.PLC_System[0] && MITSUBISHIPLC_SignalList_result != null &&
                            MITSUBISHIPLC_SignalList_result[ii + 1].IsShow && MITSUBISHIPLC_SignalList_result[ii + 1].ACTION_ID == "-1"
                            && MITSUBISHIPLC_SignalList_result[ii + 1].IsShow)//三菱
                        {
                            MITSUBISHIPLC_SignalList_result[ii + 1].IsShow = false;
                        }
                        else if (plc.system == m_xmlDociment.PLC_System[1] && MITSUBISHIPLC_SignalList_result != null &&
                            HNC8PLC_SignalList_result[ii + 1].IsShow && HNC8PLC_SignalList_result[ii + 1].ACTION_ID == "-1"
                            && HNC8PLC_SignalList_result[ii + 1].IsShow)
                        {
                            HNC8PLC_SignalList_result[ii + 1].IsShow = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 从下位机中获取到对应寄存器值，将其转换为二进制或者十进制或者十六进制
        /// 返回一个字符串
        /// </summary>
        /// <param name="Reg_type">寄存器类型</param>
        /// <param name="Ren_index">寄存器index</param>
        /// <param name="flag">0：二进制，1：十进制，2：十六进制</param>
        private string GetRegValueChage2string(Int32 Reg_Wei, Int32 tmp, Int32 flag)
        {
//             int tmp = 0;
            uint uint_tmp = 0, shi = 0;
            if (Reg_Wei == 8)
            {
                tmp &= 0x00ff;                  //因为X，Y,R及R寄存器是8位的
                uint_tmp = 7;
                shi = 3;
            }
            else if (Reg_Wei == 16)
            {
                tmp &= 0xffff;                  //因为X，Y及R寄存器是16位的
                uint_tmp = 15;
                shi = 5;
            }
            else if (Reg_Wei == 32)
            {
                uint_tmp = (uint)tmp;
                uint_tmp &= 0xffffffff;                  //因为B及R寄存器是32位的
                tmp = (int)uint_tmp;
                uint_tmp = 31;
                shi = 10;
            }
            string str = "";
            if (flag == 0)//二进制
            {
                str = Convert.ToString(tmp, 2);
                str += "B";
                while (str.Length - 1 < uint_tmp + 1)
                    str = "0" + str;
            }
            else if (flag == 1)//十进制
            {
                str = tmp.ToString();
                str += "D";
                while (str.Length - 1 < shi)
                    str = "0" + str;
            }
            else if (flag == 2)//十六进制
            {
                str = Convert.ToString(tmp, 16);
                str += "H";
                while (str.Length - 1 < (uint_tmp + 1) / 4)
                    str = "0" + str;
            }
            return str;
        }


        private void changeDGV()
        {
//             plcDGV1.Columns[0].HeaderText = Localization.Forms["PlcForm"]["input_id"];
//             plcDGV1.Columns[1].HeaderText = Localization.Forms["PlcForm"]["input_name"];
//             plcDGV1.Columns[2].HeaderText = Localization.Forms["PlcForm"]["input_getPicture"];
//             plcDGV2.Columns[0].HeaderText = Localization.Forms["PlcForm"]["output_id"];
//             plcDGV2.Columns[1].HeaderText = Localization.Forms["PlcForm"]["output_name"];
//             plcDGV2.Columns[2].HeaderText = Localization.Forms["PlcForm"]["output_getPicture"];
        }


        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MainForm.LanguageChangeMsg:
                    Localization.RefreshLanguage(this);
                    changeDGV();
                    break;
                case MainForm.ClosingMsg:
                    if (OperationButtonArr != null)
                    {
                        for (int ii = 0; ii < PLCStationButtonNameArr2Lenth; ii++)
                        {
                            OperationButtonArr[ii].SetOperationRegBigHandlerFucRuning = false;
                        }
                    }
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }


        private void comboBoxPLCJiankong1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPLCJiankongText1 != comboBoxPLCJiankong1.Text)
            {
                DGVUpData1 = true;
                comboBoxPLCJiankongText1 = comboBoxPLCJiankong1.Text;
            }
        }

        private void comboBoxPLCJiankong2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPLCJiankongText2 != comboBoxPLCJiankong2.Text)
            {
                DGVUpData2 = true;
                comboBoxPLCJiankongText2 = comboBoxPLCJiankong2.Text;
            }
        }


        private void comboBox_PLCStationSelet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OperationButtonArr != null)
            {
                InitOperationButton(comboBox_PLCStationSelet.SelectedIndex);
            }
        }

        private void CurrentAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (PLCDataShare.m_plclist != null && PLCDataShare.m_plclist.Count > 0 && PLCDataShare.m_plclist[0].m_hncPLCCollector != null)
            {
                PLCDataShare.m_plclist[0].m_hncPLCCollector.get_alarm_data(ref dataGridViewalarmdata);
            }
        }

        private void HistoricalAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (PLCDataShare.m_plclist != null && PLCDataShare.m_plclist.Count > 0 && PLCDataShare.m_plclist[0].m_hncPLCCollector != null)
            {
                PLCDataShare.m_plclist[0].m_hncPLCCollector.get_alarm_histroy_data(ref dataGridViewalarmdata);
            }
        }

        private void button_alarmReFlesh_Click(object sender, EventArgs e)
        {
            if (CurrentAlarm.Checked)
            {
                CurrentAlarm_CheckedChanged(null, null);
            }
            else
            {
                HistoricalAlarm_CheckedChanged(null, null);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            plcindex = tabControl1.SelectedIndex;
            if (tabControl1.SelectedIndex == 2)
            {
                if (CurrentAlarm.Checked)
                {
                    CurrentAlarm_CheckedChanged(null, null);
                }
                else
                {
                    HistoricalAlarm_CheckedChanged(null, null);
                }
            }
        }

        private void PlcForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((PlcForm)sender).Visible && tabControl1.TabCount > 0)
            {
                tabControl1.SelectedIndex = plcindex;
            }
        }
    }


    public class HNC8Reg//8型系统寄存器数据类型
    {
        public String Name;
        public int HncRegType;
        public int index;
        public int bit;
        public int value;
        public bool status;
    }

    public class OperationButton
    {
        private HNC8Reg OperationReg;
        private HNC8Reg StateShowReg;
        private Button m_Button;

        EventHandler<int> SetOperationRegBigHandler;
        EventHandler<int> GetStateShowRegHandler;
        Color[] Bt_bgcoler = { System.Drawing.Color.FromArgb(255, 251, 240), System.Drawing.Color.FromArgb(0, 255, 0) };//按钮颜色

        public OperationButton(Button m_Button)
        {
            this.m_Button = m_Button;
            this.m_Button.Click += new EventHandler(this.m_Button_Click);
            this.m_Button.VisibleChanged += new EventHandler(this.m_Button_VisibleChanged);
            SetOperationRegBigHandler = new EventHandler<int>(this.SetOperationRegBigHandlerFuc);
            GetStateShowRegHandler += new EventHandler<int>(this.GetStateShowRegHandlerFuc);
        }


        public void BindingHnc8Reg(String OperationRegName, String OperationRegInitStr, String StateShowRegName, String StateShowRegInitStr)
        {
            GetStateShowRegHandlerFucRuning = false;
            if (OperationRegInitStr != null)
            {
                OperationReg = new HNC8Reg();
                OperationReg.Name = OperationRegName;
                String[] RegStr = new String[2];
                RegStr[0] = OperationRegInitStr.Substring(0,1);
                RegStr[1] = OperationRegInitStr.Substring(1, OperationRegInitStr.Length - 1);
                OperationReg.HncRegType = PLC_MITSUBISHI_HNC8.GetHncRegType(RegStr[0].ToUpper());
                RegStr = RegStr[1].Split('.');
                OperationReg.index = int.Parse(RegStr[0]);
                OperationReg.value = 0;
                OperationReg.status = false;
                if (RegStr.Length == 2)
                {
                    OperationReg.bit = int.Parse(RegStr[1]);
//                     if (SetOperationRegBigHandler == null)
//                     {
//                         SetOperationRegBigHandler = new EventHandler<int>(this.SetOperationRegBigHandlerFuc);
//                     }
                }
                else
                {
                    OperationReg.bit = -1;
//                     SetOperationRegBigHandler = null;
                }
            }
            else
            {
                OperationReg = null;
//                 SetOperationRegBigHandler = null;
            }
            if (StateShowRegName != null && StateShowRegInitStr != null)
            {
                StateShowReg = new HNC8Reg();
                StateShowReg.Name = StateShowRegName;
                String[] RegStr = new String[2];
                RegStr[0] = StateShowRegInitStr.Substring(0, 1);
                RegStr[1] = StateShowRegInitStr.Substring(1, StateShowRegInitStr.Length - 1);
                StateShowReg.HncRegType = PLC_MITSUBISHI_HNC8.GetHncRegType(RegStr[0].ToUpper());
                RegStr = RegStr[1].Split('.');
                StateShowReg.index = int.Parse(RegStr[0]);
                StateShowReg.value = 0;
                StateShowReg.status = false;
                if (RegStr.Length == 2)
                {
                    StateShowReg.bit = int.Parse(RegStr[1]);
                }
                else
                {
                    StateShowReg.bit = -1;
                }
            }
            else
            {
                StateShowReg = null;
            }

            if (OperationReg == null && StateShowReg == null)
            {
                m_Button.Visible = false;
//                 GetStateShowRegHandler = null;
            }
            else
            {
                m_Button.Visible = true;
                if(OperationReg != null)
                {
                    m_Button.Text = OperationReg.Name;
                }
                else
                {
                    m_Button.Text = StateShowReg.Name;
                }
//                 if (GetStateShowRegHandler == null)
//                 {
//                     GetStateShowRegHandler += new EventHandler<int>(this.GetStateShowRegHandlerFuc);
//                 }
                if (GetStateShowRegHandler != null && GetStateShowRegHandlerFucRuning == false)
                {
                    GetStateShowRegHandler.BeginInvoke(null, 200, null, null);
                }
            }
        }

        private void m_Button_Click(object sender, EventArgs e)
        {
            if (SetForm.LogIn)
            {
                if (SetOperationRegBigHandler != null && !SetOperationRegBigHandlerFucRuning)
                {
                    SetOperationRegBigHandler.BeginInvoke(null, 5, null, null);
                }
            }
            else
            {
                MessageBox.Show("你的操作权限不够！", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void m_Button_VisibleChanged(object sender, EventArgs e)
        {
            if (((Button)sender).Visible)
            {
                if (GetStateShowRegHandler != null && GetStateShowRegHandlerFucRuning == false)
                {
                    GetStateShowRegHandler.BeginInvoke(null, 200, null, null);
                }
            }
        }
        
        public bool SetOperationRegBigHandlerFucRuning = false;
        private void SetOperationRegBigHandlerFuc(object obj,int time)
        {
            SetOperationRegBigHandlerFucRuning = true;
//             String[] str3 = PlcForm.PLCStationButtonNameArr[1, 14].Split(';');
//             str3 = str3[1].Split('=');//设置码垛最高层数

            if (OperationReg != null && OperationReg.bit != -1)
            {
                for (int ii = 0; ii < 50; ii++)
                {
                    if (PLCDataShare.m_plclist == null || OperationReg == null
                        || PLCDataShare.m_plclist.Count == 0 || PLCDataShare.m_plclist[0].m_hncPLCCollector == null)
                    {
                        break;
                    }
                    if (PLCDataShare.m_plclist[0].m_hncPLCCollector.connectStat)
                    {
//                         String[] str1 = PlcForm.PLCStationButtonNameArr[3,22].Split(';');
//                         str1 = str1[0].Split('=');//是否1出2
//                         String[] str2 = PlcForm.PLCStationButtonNameArr[3,23].Split(';');
//                         str2 = str2[0].Split('=');//是否清洗
//                         String[] str10 = PlcForm.PLCStationButtonNameArr[3,0].Split(';');
//                         str10 = str10[0].Split('=');//上料带启动
//                         String[] str20 = PlcForm.PLCStationButtonNameArr[3,1].Split(';');
//                         str20 = str20[0].Split('=');//下料带启动
// 
// 
//                         if (m_Button.Text == str1[0] || m_Button.Text == str2[0] ||
//                             m_Button.Text == str10[0] || m_Button.Text == str20[0])//特殊处理，每次按按钮取反
                        if (m_Button.Text.Contains("/") || m_Button.Text.Contains("是否"))
                        {
                            int value = 0;
                            if (CollectShare.GetRegValue(OperationReg.HncRegType, OperationReg.index, out value, Collector.CollectHNCPLC.dbNo) == 0)
                            {
                                value = value & (1 << OperationReg.bit);
                                value >>= OperationReg.bit;
                                if (value == 0)
                                {
                                    if (CollectShare.Instance().HNC_RegSetBit(OperationReg.HncRegType, OperationReg.index, OperationReg.bit, Collector.CollectHNCPLC.dbNo) == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(time);
                                    }
                                }
                                else
                                {
                                    if (CollectShare.Instance().HNC_RegClrBit(OperationReg.HncRegType, OperationReg.index, OperationReg.bit, Collector.CollectHNCPLC.dbNo) == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(time);
                                    }
                                }
                            }
                        }
                        else//按按钮置一
                        {
                            if (CollectShare.Instance().HNC_RegSetBit(OperationReg.HncRegType, OperationReg.index, OperationReg.bit, Collector.CollectHNCPLC.dbNo) == 0)
                            {
                                break;
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(time);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
//             else if (m_Button.Text.Contains(str3[0]))
//             {
//                 FormSetNumber setform = new FormSetNumber(m_Button.Location);
//                 setform.Text = m_Button.Text;
//                 setform.ShowDialog();
//                 if (setform.m_Number != 0)
//                 {
//                     if (HNCAPI.HncApi.HNC_ParamanSetI32((Int32)HNCAPI.HNCDATADEF.PARAMAN_FILE_MAC, 0, OperationReg.index + 300, setform.m_Number, Collector.CollectHNCPLC.m_clientNo) == 0)
//                     {
//                         if (HNCAPI.HncApi.HNC_ParamanSave(Collector.CollectHNCPLC.m_clientNo) == 0)
//                         {
//                             ThreaSetLaBText(m_Button, str3[0] + "=" + setform.m_Number.ToString());
//                         }
//                     }
//                 }
//             }
            SetOperationRegBigHandlerFucRuning = false;
        }

        public bool GetStateShowRegHandlerFucRuning = false;
        private void GetStateShowRegHandlerFuc(object obj, int time)
        {
            GetStateShowRegHandlerFucRuning = true;
            if (StateShowReg != null)
            {
                while (true)
                {
                    if (!m_Button.Visible || !GetStateShowRegHandlerFucRuning || StateShowReg == null
                        || PLCDataShare.m_plclist == null || PLCDataShare.m_plclist.Count == 0
                        || PLCDataShare.m_plclist[0].m_hncPLCCollector == null)
                    {
                        break;
                    }
                    if (PLCDataShare.m_plclist[0].m_hncPLCCollector.connectStat)
                    {
                        int value = 0;
                        if (StateShowReg.HncRegType == 8)//P参数
                        {
                            if (MacDataService.GetInstance().HNC_ParamanGetI32((Int32)HNCAPI.HNCDATADEF.PARAMAN_FILE_MAC, 0, StateShowReg.index + 300, ref value, Collector.CollectHNCPLC.dbNo) == 0)//P参数从300开始偏移
                            {
                                String[] str = m_Button.Text.Split('=');
                                ThreaSetLaBText(m_Button, str[0] + "=" + value);
                                break;
                            }
                        }
                        else
                        {
                            if (CollectShare.GetRegValue(StateShowReg.HncRegType, StateShowReg.index, out value, Collector.CollectHNCPLC.dbNo) == 0)
                            {
                                if (StateShowReg != null && StateShowReg.bit == -1 && value > -1000 && value < 1000)//显示值
                                {
                                    String[] str = m_Button.Text.Split('=');
                                    ThreaSetLaBText(m_Button, str[0] + "=" + value);
                                }
                                else if (StateShowReg != null && StateShowReg.bit != -1)//显示IO状态
                                {
                                    value = value & (1 << StateShowReg.bit);
                                    value >>= StateShowReg.bit;
                                    if (value == 0)
                                    {
                                        if (m_Button.BackColor != this.Bt_bgcoler[0])
                                        {
                                            m_Button.BackColor = this.Bt_bgcoler[0];
                                        }
                                    }
                                    else
                                    {
                                        if (m_Button.BackColor != this.Bt_bgcoler[1])
                                        {
                                            m_Button.BackColor = this.Bt_bgcoler[1];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (m_Button.BackColor != this.Bt_bgcoler[0])
                        {
                            m_Button.BackColor = this.Bt_bgcoler[0];
                        }
                    }
                    System.Threading.Thread.Sleep(time);
                }
            }
            GetStateShowRegHandlerFucRuning = false;
        }

        /// <summary>
        /// 委托设置文本控件显示
        /// </summary>
        /// <param name="LB"></param>
        /// <param name="str"></param>
        private void ThreaSetLaBText(Button bt, String str)
        {
            if (bt.InvokeRequired)//等待异步
            {
                // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                Action<string> actionDelegate = (x) => { bt.Text = x; };
                bt.Invoke(actionDelegate, str);
            }
            else
            {
                bt.Text = str;
            }
        }

    }
}
