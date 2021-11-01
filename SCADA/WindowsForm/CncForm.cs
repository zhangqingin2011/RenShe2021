using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HNCAPI;
using HNC_MacDataService;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Collections;
using System.Net.NetworkInformation;
using HNC.API;
using HNCAPI_INTERFACE;
using ScadaHncData;

namespace SCADA
{
    public partial class CncForm : Form
    {
       // CNC cnc;
        CNCV2 cncv2;
        private UserControlParaman ctrlParaman = new UserControlParaman();//NC参数表对象

        //数控按键板
        private HNC848CKeyboard1 cncboard848C_1 = new HNC848CKeyboard1();
        private HNC848CKeyboard2 cncboard848C_2 = new HNC848CKeyboard2();
        private HNC818AKeyboard1 cncboard818A_1 = new HNC818AKeyboard1();
        private HNC818AKeyboard2 cncboard818A_2 = new HNC818AKeyboard2();
        private HNC818BKeyboard1 cncboard818B_1 = new HNC818BKeyboard1();
        private HNC818BKeyboard2 cncboard818B_2 = new HNC818BKeyboard2();
        private HNC848BKeyboard1 cncboard848B_1 = new HNC848BKeyboard1();
        private HNC848BKeyboard2 cncboard848B_2 = new HNC848BKeyboard2();

        bool UpDataCNCId_PROGGCUpdata = true;//cnc设备选择变化时刷新RegVave
        string OldPROGNAME = "";//记住上一次执行的G代码名称
        int RegValeShowType = 0;//寄存器值显示方式：二进制、十进制、十六进制
        int reg_num_x = 0,reg_num_y = 0,reg_num_f = 0,reg_num_g = 0,reg_num_r = 0,reg_num_b = 0;
        public static int tab_index = 0;

        Color[] Bt_bgcoler = { System.Drawing.Color.FromArgb(255, 251, 240), System.Drawing.Color.FromArgb(0, 255, 0) };//按钮颜色
         AutoSizeFormClass asc = new AutoSizeFormClass();
         AutoSizeFormClass asc1 = new AutoSizeFormClass();
         AutoSizeFormClass asc2 = new AutoSizeFormClass();
        //string[] State_str = { "运行", "空闲", "报警", "离线" };//状态字符串

        Hashtable m_Hashtable;

        public CncForm()
        {
            InitializeComponent();
            CncForm.CheckForIllegalCrossThreadCalls = false;
            comboBoxCNC.Items.Clear();
            //foreach (CNC m_cnc in MainForm.cnclist)
            //{
            //    string str = "CNC:";
            //    str += m_cnc.JiTaiHao;
            //    comboBoxCNC.Items.Add(str);
            //}
            int cncno = 0;
            foreach (var cncv2temp in MainForm.cncv2list)
            {
                string str = "CNC:";
                cncno++;
                str += cncno.ToString();
                comboBoxCNC.Items.Add(str);
            }
            dataGridView_REG.ReadOnly = true;
            radioButtonRegValBit.Checked = true;

          
            rbtnCurrentAlarmCNC.Checked = true;
            panelCNCParam.Controls.Clear();
            panelCNCParam.Controls.Add(ctrlParaman);

            Keyboard1.Controls.Add(cncboard848C_1);
            Keyboard1.Controls.Add(cncboard818A_1);
            Keyboard1.Controls.Add(cncboard818B_1);
            Keyboard1.Controls.Add(cncboard848B_1);
            cncboard818A_1.Parent = Keyboard1;
            cncboard818B_1.Parent = Keyboard1;
            cncboard848B_1.Parent = Keyboard1;
            cncboard848C_1.Parent = Keyboard1;

            //Keyboard2.Controls.Clear();
            Keyboard2.Controls.Add(cncboard848C_2);
            Keyboard2.Controls.Add(cncboard818A_2);
            Keyboard2.Controls.Add(cncboard818B_2);
            Keyboard2.Controls.Add(cncboard848B_2);
            cncboard818A_2.Parent = Keyboard2;
            cncboard818B_2.Parent = Keyboard2;
            cncboard848B_2.Parent = Keyboard2;
            cncboard848C_2.Parent = Keyboard2;

        }
        public System.Threading.AutoResetEvent Get_Reg_threaFucEvent = new System.Threading.AutoResetEvent(true);
        bool threaFucRuningF = true;
        bool threaFucRuningF_OK = false;

        public void LoadCNCLanguage()
        {
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
            dataGridView_REG.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column_Reg_index");
            dataGridView_REG.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column_RegVal_x");
            dataGridView_REG.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column_RegVal_y");
            dataGridView_REG.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column_RegVal_f");
            dataGridView_REG.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column_RegVal_g");
            dataGridView_REG.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column_RegVal_r");
            dataGridView_REG.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column_RegVal_b");
            dataGridViewalarmdata.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "AlarmColumn01");
            dataGridViewalarmdata.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "AlarmColumn02");
            dataGridViewalarmdata.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "AlarmColumn03");
            dataGridView_Comp.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "KnifeColumn01");
            dataGridView_Comp.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "KnifeColumn02");
            dataGridView_Comp.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "KnifeColumn03");
            dataGridView_Comp.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "KnifeColumn04");
            dataGridView_Comp.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "KnifeColumn05");
            ctrlParaman.LoadParamanLanguage();
            cncboard818A_1.LoadLanguage();
            cncboard818A_2.LoadLanguage();
            cncboard848C_1.LoadLanguage();
            cncboard848C_2.LoadLanguage();
            cncboard818B_1.LoadLanguage();
            cncboard818B_2.LoadLanguage();
        }

        //private void UpDataUIThreaFuc()
        //{
        //    while (threaFucRuningF)
        //    {
        //        if (threaFucRuningF_OK && MainForm.InitializeComponentFinish)
        //        {                    
        //            try
        //            {
        //                //Console.WriteLine(Convert.ToString(cnc.dbNo));
        //                bool connect = MainForm.PingTestCNC(cnc.ip, 300);
        //                if (cnc.HCNCShareData.sysData.isConnect && connect == true)
        //                {
        //                 //   Updata_Reg_Value();
        //                    Updata_Comp_Value();
        //                    Updata_NCStandardPanel_Value(cnc);
        //                    Updatacncstate_1s(cnc);
        //                    ThreaSetLaBText(labelLinckText, ChangeLanguage.GetString("LinckedText"));
        //                    ThreaSetPictureBoxBackColor(pictureBoxLinckState, SCADA.Properties.Resources.top_bar_green);
        //                }
        //                else
        //                {

        //                    ThreaSetLaBText(labelLinckText, ChangeLanguage.GetString("UnLinckedText"));
        //                    if (labelLinckText.Text != ChangeLanguage.GetString("LinckedText"))
        //                    {
        //                        ClearAllCNCstate(cnc);
        //                    }
        //                    ThreaSetPictureBoxBackColor(pictureBoxLinckState, SCADA.Properties.Resources.top_bar_black);

        //                }
        //            }
        //            catch (System.Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //            System.Threading.Thread.Sleep(100);
        //            if (!this.Visible)
        //            {
        //                threaFucRuningF_OK = false;
        //            }
        //        }
        //        else
        //        {
        //            Get_Reg_threaFucEvent.WaitOne();
        //        }
        //    }
        //    LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
        //    SendParm.Node1NameIndex = (int)LogData.Node1Name.Equipment_CNC;
        //    SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
        //    SendParm.DisplayLang = SCADA.MainForm.CurrentLanguage;
        //    SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
        //    SendParm.Keywords = SCADA.ChangeLanguage.GetString("LogContentUIRefreshThreadExit");
        //    SendParm.EventData = "";
        //    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
        //}

        /// <summary>
        /// 更新UI
        /// </summary>
        private void UpDataUIThreaFucv2()
        {
            while (threaFucRuningF)
            {
                if (threaFucRuningF_OK && MainForm.InitializeComponentFinish)
                {
                    try
                    {

                        cncv2 = MainForm.cncv2list[comboBoxCNC.SelectedIndex];
                        if (cncv2.IsConnected())
                        {
                            //   Updata_Reg_Value();
                            Updata_Comp_Value(true);
                            Updata_NCStandardPanel_Value(cncv2);
                            Updatacncstate_1s(cncv2);
                            Updata_Alarm_Value(true);
                            ThreaSetLaBText(labelLinckText, ChangeLanguage.GetString("LinckedText"));
                            ThreaSetPictureBoxBackColor(pictureBoxLinckState, SCADA.Properties.Resources.top_bar_green);
                        }
                        else
                        {

                            ThreaSetLaBText(labelLinckText, ChangeLanguage.GetString("UnLinckedText"));
                            if (labelLinckText.Text != ChangeLanguage.GetString("LinckedText"))
                            {
                                ClearAllCNCstate(cncv2);
                            }
                            ThreaSetPictureBoxBackColor(pictureBoxLinckState, SCADA.Properties.Resources.top_bar_black);

                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    System.Threading.Thread.Sleep(100);
                    if (!this.Visible)
                    {
                        threaFucRuningF_OK = false;
                    }
                }
                else
                {
                    Get_Reg_threaFucEvent.WaitOne();
                }
            }
            LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
            SendParm.Node1NameIndex = (int)LogData.Node1Name.Equipment_CNC;
            SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
            SendParm.DisplayLang = SCADA.MainForm.CurrentLanguage;
            SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
            SendParm.Keywords = SCADA.ChangeLanguage.GetString("LogContentUIRefreshThreadExit");
            SendParm.EventData = "";
            SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
        }

        /// <summary>
        /// 委托设置文本控件显示
        /// </summary>
        /// <param name="LB"></param>
        /// <param name="str"></param>
        private void ThreaSetLaBText(Label LB,String str)
        {
            if (LB.Text != str && threaFucRuningF)
            {
                if (LB.InvokeRequired && threaFucRuningF)//等待异步
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<string> actionDelegate = (x) => { LB.Text = x; };
                    LB.Invoke(actionDelegate, str);
                }
                else if(threaFucRuningF)
                {
                    LB.Text = str;
                }
            }
        }
        /// <summary>
        /// 委托设置文本控件显示
        /// </summary>
        /// <param name="LB"></param>
        /// <param name="str"></param>
        private void ThreaSetLaBText(RichTextBox RLB, String str)
        {
            if (RLB.Text != str && threaFucRuningF)
            {
                if (RLB.InvokeRequired && threaFucRuningF)//等待异步
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<string> actionDelegate = (x) => { RLB.Text = x; };
                    RLB.Invoke(actionDelegate, str);
                }
                else if(threaFucRuningF)
                {
                    RLB.Text = str;
                }
            }
        }

        private void SetAxis_AVisible(bool flag)
        {
            tableLayoutPanel15.Visible = flag;
            label_cmdpos_a.Visible = flag;
            panel3.Visible = flag;
        }

        private void SetAxis_YVisible(bool flag)
        {
            tableLayoutPanel8.Visible = flag;
            label_cmdpos_y.Visible = flag;
            panel5.Visible = flag;
        }

        CNCType type1 = CNCType.Lathe;
        ushort serial = 10000;
//        private void comboBoxCNC_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (serial != MainForm.cnclist[comboBoxCNC.SelectedIndex].serial)
//            {
//                cnc = MainForm.cnclist[comboBoxCNC.SelectedIndex];
//                tab_index = comboBoxCNC.SelectedIndex;
//                ClearAllCNCstate(cnc);
//                if (cnc.isConnected())
//                {
//                    InitdataGridView_REG(cnc);//初始寄存器列表           
//                    InitdataGridView_Comp();  //初始化刀补表
//                    ctrlParaman.InitParamanTreeView(cnc.HCNCShareData.sysData.dbNo);          //读取参数结构
//                }
//                UpDataCNCId_PROGGCUpdata = true;
//                serial = cnc.serial;
////                 TextJiTaiHao.Text = cnc.BujianID.Substring(cnc.BujianID.Length - 3, 3);
//                txtIP.Text = cnc.ip;//IP
//                textPoort.Text = cnc.port.ToString();
//                textGongXu.Text = cnc.OP_CODE;
//                //if (cnc.Type == "HNC_848C")
//                if (cnc.Type == "HNC_848C")
//                {
//                    cncboard818A_1.Visible = false;
//                    cncboard818A_2.Visible = false;
//                    cncboard818B_1.Visible = false;
//                    cncboard818B_2.Visible = false;
//                    cncboard848B_1.Visible = false;
//                    cncboard848B_2.Visible = false;
//                    cncboard848C_1.Visible = true;
//                    cncboard848C_2.Visible = true;
//                    SetAxis_AVisible(true);
//                    SetAxis_YVisible(true);
//                    tableLayoutPanel6.Visible = false;
//                    tableLayoutPanel3.Visible = false;
                   


//                }
//                //else if (cnc.Type == "HNC_818A")
//                else if (cnc.Type == "HNC_818A")
//                {
//                    SetAxis_AVisible(false);
//                    SetAxis_YVisible(false);
//                    cncboard848C_1.Visible = false;
//                    cncboard848C_2.Visible = false;
//                    cncboard848B_1.Visible = false;
//                    cncboard848B_2.Visible = false;
//                    cncboard818B_1.Visible = false;
//                    cncboard818B_2.Visible = false;
//                    cncboard818A_1.Visible = true;
//                    cncboard818A_2.Visible = true;
//                    tableLayoutPanel6.Visible = false;
//                    tableLayoutPanel3.Visible = false;

//                }
//                //else if (cnc.Type == "HNC_818B")
//                else if (cnc.Type == "HNC_818B")
//                {
//                    SetAxis_AVisible(false);
//                    SetAxis_YVisible(true);
//                    cncboard818B_1.Visible = true;
//                    cncboard818B_2.Visible = true;
//                    cncboard848C_1.Visible = false;
//                    cncboard848C_2.Visible = false;
//                    tableLayoutPanel6.Visible = false;
//                    tableLayoutPanel3.Visible = false;
//                    cncboard818A_1.Visible = false;
//                    cncboard818A_2.Visible = false;
//                    cncboard848B_1.Visible = false;
//                    cncboard848B_2.Visible = false;

//                }
//                else if (cnc.Type == "HNC_848B")
//                {
//                    SetAxis_AVisible(false);
//                    SetAxis_YVisible(true);
//                    tableLayoutPanel6.Visible = true;
//                    tableLayoutPanel3.Visible = true;
//                    cncboard848B_1.Visible = false;
//                    cncboard848B_2.Visible = false;
//                    cncboard848C_1.Visible = false;
//                    cncboard848C_2.Visible = false;
//                    cncboard818A_1.Visible = false;
//                    cncboard818A_2.Visible = false;
//                    cncboard818B_1.Visible = false;
//                    cncboard818B_2.Visible = false;

//                }
//                else//默认818a
//                {

//                    SetAxis_AVisible(false);
//                    SetAxis_YVisible(false);
//                    cncboard848C_1.Visible = false;
//                    cncboard848C_2.Visible = false;
//                    cncboard848B_1.Visible = false;
//                    cncboard848B_2.Visible = false;
//                    cncboard818B_1.Visible = false;
//                    cncboard818B_2.Visible = false;
//                    cncboard818A_1.Visible = true;
//                    cncboard818A_2.Visible = true;
//                    tableLayoutPanel6.Visible = false;
//                    tableLayoutPanel3.Visible = false;
//                }
//            }
//        }

        private void comboBoxCNC_SelectedIndexChangedv2(object sender, EventArgs e)
        {
            if (type1 != MainForm.cncv2list[comboBoxCNC.SelectedIndex].cnctype)
            {
                cncv2 = MainForm.cncv2list[comboBoxCNC.SelectedIndex];
                tab_index = comboBoxCNC.SelectedIndex;

                ClearAllCNCstate(cncv2);
                if (cncv2.IsConnected())
                {
                   // InitdataGridView_REG(cncv2);//初始寄存器列表           
                    InitdataGridView_Comp(true);  //初始化刀补表
                }
                UpDataCNCId_PROGGCUpdata = true;
                type1 = MainForm.cncv2list[comboBoxCNC.SelectedIndex].cnctype;
                //                 TextJiTaiHao.Text = cnc.BujianID.Substring(cnc.BujianID.Length - 3, 3);
                txtIP.Text = cncv2.IP;//IP
                textPoort.Text = cncv2.Port.ToString();
               
                 if (cncv2.cnctype == CNCType.Lathe )
                {
                    SetAxis_AVisible(false);
                    SetAxis_YVisible(false);
                    cncboard848C_1.Visible = false;
                    cncboard848C_2.Visible = false;
                    cncboard848B_1.Visible = false;
                    cncboard848B_2.Visible = false;
                    cncboard818B_1.Visible = false;
                    cncboard818B_2.Visible = false;
                    cncboard818A_1.Visible = true;
                    cncboard818A_2.Visible = true;
                    tableLayoutPanel6.Visible = false;
                    tableLayoutPanel3.Visible = false;

                }
                //else if (cnc.Type == "HNC_818B")
                else if (cncv2.cnctype == CNCType.CNC)
                {
                    
                    SetAxis_AVisible(false);
                    SetAxis_YVisible(true);
                    cncboard818B_1.Visible = true;
                    cncboard818B_2.Visible = true;
                    cncboard848C_1.Visible = false;
                    cncboard848C_2.Visible = false;
                    tableLayoutPanel6.Visible = false;
                    tableLayoutPanel3.Visible = false;
                    cncboard818A_1.Visible = false;
                    cncboard818A_2.Visible = false;
                    cncboard848B_1.Visible = false;
                    cncboard848B_2.Visible = false;

                } 
                else//默认818a
                {

                    SetAxis_AVisible(false);
                    SetAxis_YVisible(false);
                    cncboard848C_1.Visible = false;
                    cncboard848C_2.Visible = false;
                    cncboard848B_1.Visible = false;
                    cncboard848B_2.Visible = false;
                    cncboard818B_1.Visible = false;
                    cncboard818B_2.Visible = false;
                    cncboard818A_1.Visible = true;
                    cncboard818A_2.Visible = true;
                    tableLayoutPanel6.Visible = false;
                    tableLayoutPanel3.Visible = false;
                }
            }
        }

//        private void ClearAllCNCstate(CNC cnc)
//        {
//            HNC_SYS_NCK_VER.Text = "-";//版本
//            HNC_SYS_DRV_VER.Text = "-";//版本
//            HNC_SYS_PLC_VER.Text = "-";//版本
//            HNC_SYS_CNC_VER.Text = "-";//版本
//            HNC_SYS_NC_VER.Text = "-";//版本
//            txtCount.Text = "-";//加工数量
//            //labCurToolNo.Text = "000";//当前刀号

//            pictureBoxLinckState.Image = SCADA.Properties.Resources.top_bar_black;//连接状态
//            ThreaSetLaBText(labelLinckText, ChangeLanguage.GetString("UnLinckedText"));


//            labPROGNAME.Text = "";//G代码名
            
//            richTextBoxCurrentProgramRun.Text = "";
//            label_acpos_x.Text = "0.000";
//            label_acpos_y.Text = "0.000";
//            label_acpos_z.Text = "0.000";
//            label_acpos_c.Text = "0.000";
//            label_acpos_a.Text = "0.000";
//            label_cmdpos_x.Text = "0.000";
//            label_cmdpos_y.Text = "0.000";
//            label_cmdpos_z.Text = "0.000";
//            label_cmdpos_c.Text = "0.000";
//            label_cmdpos_a.Text = "0.000";
//            label_aci_x.Text = "0%";
//            label_aci_y.Text = "0%";
//            label_aci_z.Text = "0%"; 
//            label_aci_c.Text = "0%";
//            label_aci_a.Text = "0%";
//            labFSpeed.Text = "0";
//            labCurToolNo.Text = "000";
//            labSpdlSpeed.Text = "0";
//            labCurLineNo.Text = "-1";
//            labFSpeedRate.Text = "0%";
//            labSpdlRate.Text = "0%";
//            labRapidRate.Text = "0%";
//            labPROGNAME.Text = "";
//            txtRunProc.Text = "";
//            progressBar_ruti_x.Value = 0;
//            progressBar_ruti_y.Value = 0;
//            progressBar_ruti_z.Value = 0;
//            progressBar_ruti_c.Value = 0;
//            progressBar_ruti_a.Value = 0;
//            richTextBoxCurrentProgramRun.Text = "";
//            ClearButtonShow(cnc);
////             dataGridView_REG.Rows.Clear();
//            ctrlParaman.ClearAlldata();
//            dataGridViewalarmdata.Rows.Clear();
//        }
        private void ClearAllCNCstate(CNCV2 cnc)
        {
            HNC_SYS_NCK_VER.Text = "-";//版本
            HNC_SYS_DRV_VER.Text = "-";//版本
            HNC_SYS_PLC_VER.Text = "-";//版本
                                       //  HNC_SYS_CNC_VER.Text = "-";//版本
            HNC_SYS_NC_VER.Text = "-";//版本
            txtCount.Text = "-";//加工数量
            //labCurToolNo.Text = "000";//当前刀号
            if(comboBoxCNC.SelectedIndex == 0 && MainForm.SEquipmentlist[0].IsConnect )
            {

                pictureBoxLinckState.Image = SCADA.Properties.Resources.top_bar_green;//连接状态   
                ThreaSetLaBText(labelLinckText, ChangeLanguage.GetString("LinckedText"));
            }
            else if (comboBoxCNC.SelectedIndex == 1 && MainForm.SEquipmentlist[1].IsConnect)
            {
                pictureBoxLinckState.Image = SCADA.Properties.Resources.top_bar_green;//连接状态
                ThreaSetLaBText(labelLinckText, ChangeLanguage.GetString("LinckedText"));
            }
            else
            {
                pictureBoxLinckState.Image = SCADA.Properties.Resources.top_bar_black;//连接状态
                ThreaSetLaBText(labelLinckText, ChangeLanguage.GetString("UnLinckedText"));
            }



            labPROGNAME.Text = "";//G代码名

            richTextBoxCurrentProgramRun.Text = "";
            label_acpos_x.Text = "0.000";
            label_acpos_y.Text = "0.000";
            label_acpos_z.Text = "0.000";
            label_acpos_c.Text = "0.000";
            label_acpos_a.Text = "0.000";
            label_cmdpos_x.Text = "0.000";
            label_cmdpos_y.Text = "0.000";
            label_cmdpos_z.Text = "0.000";
            label_cmdpos_c.Text = "0.000";
            label_cmdpos_a.Text = "0.000";
            label_aci_x.Text = "0%";
            label_aci_y.Text = "0%";
            label_aci_z.Text = "0%";
            label_aci_c.Text = "0%";
            label_aci_a.Text = "0%";
            labFSpeed.Text = "0";
            labCurToolNo.Text = "000";
            labSpdlSpeed.Text = "0";
            labCurLineNo.Text = "-1";
            labFSpeedRate.Text = "0%";
            labSpdlRate.Text = "0%";
            labRapidRate.Text = "0%";
            labPROGNAME.Text = "";
            txtRunProc.Text = "";
            progressBar_ruti_x.Value = 0;
            progressBar_ruti_y.Value = 0;
            progressBar_ruti_z.Value = 0;
            progressBar_ruti_c.Value = 0;
            progressBar_ruti_a.Value = 0;
            richTextBoxCurrentProgramRun.Text = "";
            ClearButtonShow(cncv2);
            //             dataGridView_REG.Rows.Clear();
            ctrlParaman.ClearAlldata();
            dataGridViewalarmdata.Rows.Clear();
        }


        //private void ClearButtonShow(CNC cnc)
        //{
        //    if (cnc.Type == "HNC_848B")
        //    {
        //        cncboard848B_1.ClearState();
        //        cncboard848B_2.ClearState();

        //    }
        //    else if (cnc.Type == "HNC_848C")
        //    {
        //        cncboard848C_1.ClearState();
        //        cncboard848C_2.ClearState();
        //    }
        //    else if (cnc.Type == "HNC_818A")
        //    {
        //        cncboard818A_1.ClearState();
        //        cncboard818A_2.ClearState();
        //    }
        //    else if (cnc.Type == "HNC_818B")
        //    {
        //        cncboard818B_1.ClearState();
        //        cncboard818B_2.ClearState();
        //    }
        //    else
        //    {
        //        btnBackColorSet(ref btnAuto, 0);
        //        btnBackColorSet(ref btnSingleSegmentCNC, 0);
        //        btnBackColorSet(ref btnManualCNC, 0);
        //        btnBackColorSet(ref btnRunIncrementCNC, 0);
        //        btnBackColorSet(ref btnReZeroCNC, 0);
        //        btnBackColorSet(ref btnzhaomingCNC, 0);
        //        btnBackColorSet(ref btndaosongjingCNC, 0);
        //        btnBackColorSet(ref btnToolfanzhuan, 0);
        //        btnBackColorSet(ref btnhuandaozhuangdaoCNC, 0);
        //        btnBackColorSet(ref btnGCSkipCNC, 0);
        //        btnBackColorSet(ref btnkongrunCNC, 0);
        //        btnBackColorSet(ref btnxuanzhetingCNC, 0);
        //        btnBackColorSet(ref btnZsuoCNC, 0);
        //        btnBackColorSet(ref btnjichuangshuoCNC, 0);
        //        btnBackColorSet(ref btnProtectDoorCNC, 0);
        //        btnBackColorSet(ref btnF4CNC, 0);
        //        btnBackColorSet(ref btnToolzhengzhuan, 0);
        //        btnBackColorSet(ref btnToolTest, 0);
        //        btnBackColorSet(ref btnaddACNC, 0);
        //        btnBackColorSet(ref btnaddzCNC, 0);
        //        btnBackColorSet(ref btnsubyCNC, 0);
        //        btnBackColorSet(ref btnF1CNC, 0);
        //        btnBackColorSet(ref btnF2CNC, 0);
        //        btnBackColorSet(ref btnF3CNC, 0);
        //        btnBackColorSet(ref btnCoolingCNC, 0);
        //        btnBackColorSet(ref button3AxsHome, 0);
        //        btnBackColorSet(ref btnToolHome, 0);
        //        btnBackColorSet(ref btnaddxCNC, 0);
        //        btnBackColorSet(ref btnkuaijingCNC, 0);
        //        btnBackColorSet(ref btnsubxCNC, 0);
        //        btnBackColorSet(ref btnzhuzhoudingxiangCNC, 0);
        //        btnBackColorSet(ref btnshouyaoshiqie, 0);
        //        btnBackColorSet(ref btnF1, 0);
        //        btnBackColorSet(ref btnLubricationCNC, 0);
        //        btnBackColorSet(ref btnchaochenjiechuCNC, 0);
        //        btnBackColorSet(ref btnChongxue, 0);
        //        btnBackColorSet(ref btnaddyCNC, 0);
        //        btnBackColorSet(ref btnsubzCNC, 0);
        //        btnBackColorSet(ref btnsubACNC, 0);
        //        btnBackColorSet(ref btnSpindleForwardCNC, 0);
        //        btnBackColorSet(ref btnSpindleStopCNC, 0);
        //        btnBackColorSet(ref btnzhuzhoufanzhuangCNC, 0);
        //        btnBackColorSet(ref btnxhqdCNC, 0);
        //        btnBackColorSet(ref btnFeedHoldRunCNC, 0);
        //        btnBackColorSet(ref btnEStopCNC, 0);
        //    }
        //}

        private void ClearButtonShow(CNCV2 cnc)
        {
            if (cncv2.cnctype ==CNCType.Lathe)
            {
                cncboard818A_1.ClearState();
                cncboard818A_2.ClearState();
            }
            else if (cncv2.cnctype == CNCType.CNC)
            {
                cncboard818B_1.ClearState();
                cncboard818B_2.ClearState();
            }
            else
            {
                btnBackColorSet(ref btnAuto, 0);
                btnBackColorSet(ref btnSingleSegmentCNC, 0);
                btnBackColorSet(ref btnManualCNC, 0);
                btnBackColorSet(ref btnRunIncrementCNC, 0);
                btnBackColorSet(ref btnReZeroCNC, 0);
                btnBackColorSet(ref btnzhaomingCNC, 0);
                btnBackColorSet(ref btndaosongjingCNC, 0);
                btnBackColorSet(ref btnToolfanzhuan, 0);
                btnBackColorSet(ref btnhuandaozhuangdaoCNC, 0);
                btnBackColorSet(ref btnGCSkipCNC, 0);
                btnBackColorSet(ref btnkongrunCNC, 0);
                btnBackColorSet(ref btnxuanzhetingCNC, 0);
                btnBackColorSet(ref btnZsuoCNC, 0);
                btnBackColorSet(ref btnjichuangshuoCNC, 0);
                btnBackColorSet(ref btnProtectDoorCNC, 0);
                btnBackColorSet(ref btnF4CNC, 0);
                btnBackColorSet(ref btnToolzhengzhuan, 0);
                btnBackColorSet(ref btnToolTest, 0);
                btnBackColorSet(ref btnaddACNC, 0);
                btnBackColorSet(ref btnaddzCNC, 0);
                btnBackColorSet(ref btnsubyCNC, 0);
                btnBackColorSet(ref btnF1CNC, 0);
                btnBackColorSet(ref btnF2CNC, 0);
                btnBackColorSet(ref btnF3CNC, 0);
                btnBackColorSet(ref btnCoolingCNC, 0);
                btnBackColorSet(ref button3AxsHome, 0);
                btnBackColorSet(ref btnToolHome, 0);
                btnBackColorSet(ref btnaddxCNC, 0);
                btnBackColorSet(ref btnkuaijingCNC, 0);
                btnBackColorSet(ref btnsubxCNC, 0);
                btnBackColorSet(ref btnzhuzhoudingxiangCNC, 0);
                btnBackColorSet(ref btnshouyaoshiqie, 0);
                btnBackColorSet(ref btnF1, 0);
                btnBackColorSet(ref btnLubricationCNC, 0);
                btnBackColorSet(ref btnchaochenjiechuCNC, 0);
                btnBackColorSet(ref btnChongxue, 0);
                btnBackColorSet(ref btnaddyCNC, 0);
                btnBackColorSet(ref btnsubzCNC, 0);
                btnBackColorSet(ref btnsubACNC, 0);
                btnBackColorSet(ref btnSpindleForwardCNC, 0);
                btnBackColorSet(ref btnSpindleStopCNC, 0);
                btnBackColorSet(ref btnzhuzhoufanzhuangCNC, 0);
                btnBackColorSet(ref btnxhqdCNC, 0);
                btnBackColorSet(ref btnFeedHoldRunCNC, 0);
                btnBackColorSet(ref btnEStopCNC, 0);
            }
        }

        /// <summary>
        /// 1s刷新一次状态
        /// </summary>
        /// <param name="cnctmp">选中的cnc设备</param>
        System.Data.DataTable dataGridViewalarmdata_DataSource;//= new System.Data.DataTable();
//        private void Updatacncstate_1s(CNC cnctmp)
//        {
//            if (cnc.isConnected())
//            {
//                ThreaSetLaBText(txtIP, cnctmp.ip);
//                m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
//                ThreaSetLaBText(HNC_SYS_NCK_VER, ChangeLanguage.GetString(m_Hashtable, "NCKVersion") + cnctmp.HCNCShareData.sysData.sysver.ncu);
//                ThreaSetLaBText(HNC_SYS_DRV_VER, ChangeLanguage.GetString(m_Hashtable, "DRVVersion") + cnctmp.HCNCShareData.sysData.sysver.drv);
//                ThreaSetLaBText(HNC_SYS_PLC_VER, ChangeLanguage.GetString(m_Hashtable, "PLCVersion") + cnctmp.HCNCShareData.sysData.sysver.plc);
//                ThreaSetLaBText(HNC_SYS_CNC_VER, ChangeLanguage.GetString(m_Hashtable, "CNCVersion") + cnctmp.HCNCShareData.sysData.sysver.cnc);
//                ThreaSetLaBText(HNC_SYS_NC_VER, ChangeLanguage.GetString(m_Hashtable, "NCVersion") + cnctmp.HCNCShareData.sysData.sysver.nc);

//                ThreaSetLaBText(txtCount, cnctmp.get_partNum().ToString());
//                ThreaSetLaBText(labCurToolNo, cnctmp.get_toolUse().ToString("D4"));
////                 string str = "";
////                 CNC.CNCState state = cnctmp.Checkcnc_state();
////                 if (state == CNC.CNCState.DISCON)
////                 {
////                     str = State_str[3];
////                 }
////                 else if (state == CNC.CNCState.IDLE)
////                 {
////                     str = State_str[1];
////                 }
////                 else if (state == CNC.CNCState.ALARM)
////                 {
////                     str = State_str[2];
////                 }
////                 else if (state == CNC.CNCState.RUNING)
////                 {
////                     str = State_str[0];
////                 }

//                OldPROGNAME = labPROGNAME.Text;
//                ThreaSetLaBText(labPROGNAME, cnctmp.get_gCodeName());

//                string[] strgcodename;
//                if (cnctmp.get_gCodeName() != null && cnctmp.get_gCodeName().Length > 0)
//                {
//                    strgcodename = cnctmp.get_gCodeName().Split('/');
//                    ThreaSetLaBText(txtRunProc, strgcodename[strgcodename.Length - 1]);
//                }
//                else
//                {
//                    ThreaSetLaBText(txtRunProc, "");
//                }

//                /*
//                List<string> GcodeContent = new List<string>();
//                if (UpDataCNCId_PROGGCUpdata == true || OldPROGNAME != cnctmp.get_gCodeName())//重新上载G代码文件并显示
//                {
//                    if (labPROGNAME.Text != "N/A" && labPROGNAME.Text != "")
//                    {
//                        //if (cnctmp.netFileGet(".\\cncgc.nc", labPROGNAME.Text) == 0)
//                        GcodeContent = cnctmp.netFileGet(cnctmp.get_gCodeName(), cnctmp.dbNo);
//                        if (GcodeContent.Count != 0)
//                        {
//                            AppendMessage(GcodeContent, int.Parse(labCurLineNo.Text));
//                        }
//                        else
//                        {
//                            ThreaSetLaBText(richTextBoxCurrentProgramRun, "");
//                        }
//                    }
//                    else
//                    {
//                        ThreaSetLaBText(richTextBoxCurrentProgramRun, "");
//                    }
//                    UpDataCNCId_PROGGCUpdata = false;
//                }*/
//                UpdataButtonShow();
//                if (rbtnCurrentAlarmCNC.Checked)
//                {
//                    cnc.UpAlarmShowDataTable(ref dataGridViewalarmdata_DataSource);
//                }
//                else 
//                {
//                    cnc.UpHisAlarmShowDataTable(ref dataGridViewalarmdata_DataSource);
//                }
//                ThreadGridEdet(dataGridViewalarmdata);
//            }
//        }
        private void Updatacncstate_1s(CNCV2 cnctmp)
        {
            if (cnctmp.IsConnected())
            {
                ThreaSetLaBText(txtIP, cnctmp.IP);
                m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
                ThreaSetLaBText(HNC_SYS_NCK_VER, ChangeLanguage.GetString(m_Hashtable, "NCKVersion") + cnctmp.NCKVersion);
                ThreaSetLaBText(HNC_SYS_DRV_VER, ChangeLanguage.GetString(m_Hashtable, "DRVVersion") + cnctmp.DRVVersion);
                ThreaSetLaBText(HNC_SYS_PLC_VER, ChangeLanguage.GetString(m_Hashtable, "PLCVersion") + cnctmp.PLCVersion);
                //  ThreaSetLaBText(HNC_SYS_CNC_VER, ChangeLanguage.GetString(m_Hashtable, "CNCVersion") + cnctmp.NCKVersion);
                ThreaSetLaBText(HNC_SYS_NC_VER, ChangeLanguage.GetString(m_Hashtable, "NCVersion") + cnctmp.NCVersion);

                //  ThreaSetLaBText(txtCount, cnctmp.get_partNum().ToString());
                ThreaSetLaBText(labCurToolNo, cnctmp.InstructionsTool.ToString("D4"));
            }

            OldPROGNAME = labPROGNAME.Text;
            ThreaSetLaBText(labPROGNAME, cnctmp.GProgram);

            string[] strgcodename;
            if (cnctmp.CurProgContentLine == null)
            {
                ThreaSetLaBText(txtRunProc, "");
            }
            else if (cnctmp.GProgram != null && cnctmp.CurProgContentLine.Length > 0)
            {
                strgcodename = cnctmp.GProgram.Split('/');
                ThreaSetLaBText(txtRunProc, strgcodename[strgcodename.Length - 1]);
            }
            else
            {
                ThreaSetLaBText(txtRunProc, "");
            }


            UpdataButtonShow(cnctmp);

            //if (rbtnCurrentAlarmCNC.Checked)
            //{
            //    cnc.UpAlarmShowDataTable(ref dataGridViewalarmdata_DataSource);
            //}
            //else
            //{
            //    cnc.UpHisAlarmShowDataTable(ref dataGridViewalarmdata_DataSource);
            //}
            //ThreadGridEdet(dataGridViewalarmdata);
        }
        private delegate void DelSetDvSource(DataGridView dgvEx);
        private void ThreadGridEdet(DataGridView dgvEx)
        {
            if (dgvEx.InvokeRequired && threaFucRuningF)
            {
                DelSetDvSource d = new DelSetDvSource(ThreadGridEdet);
                this.Invoke(d, new object[] { dgvEx });
            }
            else if (threaFucRuningF)
            {
                try
                {
                    if (dgvEx.Rows.Count != dataGridViewalarmdata_DataSource.Rows.Count)
                    {
                        dgvEx.RowCount = dataGridViewalarmdata_DataSource.Rows.Count;
                    }
                    for (int ii = 0; ii < dgvEx.Rows.Count; ii++)
                    {
                        if (dgvEx.Rows[ii].Displayed)
                        {
                            for (int jj = 0; jj < dataGridViewalarmdata_DataSource.Columns.Count; jj++)
                            {
                                if (dgvEx.Rows[ii].Cells[jj].Value != dataGridViewalarmdata_DataSource.Rows[ii][jj])
                                {
                                    dgvEx.Rows[ii].Cells[jj].Value = dataGridViewalarmdata_DataSource.Rows[ii][jj];
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void ThreaSetPictureBoxBackColor(PictureBox PB, Bitmap BT)
        {
            if (PB.Image != BT && PB != null && threaFucRuningF)
            {
                if (PB.InvokeRequired && threaFucRuningF)//等待异步
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<Bitmap> actionDelegate = (x) => 
                    {
                        if (PB.Image != x && threaFucRuningF)
                        {
                            PB.Image = x;
                        }
                    };
                    PB.Invoke(actionDelegate, BT);
                }
                else if(threaFucRuningF)
                {
                    if (PB.Image != BT)
                    {
                        PB.Image = BT;
                    }
                }
            }
        }

        private void btnBackColorSet(ref Button btn,int flag)
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

        /// <summary>
        /// 更新运行界面按钮显示状态
        /// </summary>
  //      private void UpdataButtonShow()
  //      {
  //          if (cnc != null)
  //          {
               
  //              //if(cnc.Type == "HNC_848C")
  //              if (cnc.Type == "HNC_848C" || cnc.Type == "HNC_848B" || cnc.Type == "HNC_818B" || cnc.Type == "HNC_818A")
  //              {
  //                  int tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 480, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      if (tmp < 0)
  //                      {
  //                          tmp = 0;
  //                      }
  //                      cncboard818A_1.BtnY480Set(tmp);
  //                      cncboard818A_2.BtnY480Set(tmp);
  //                      cncboard818B_1.BtnY480Set(tmp);
  //                      cncboard818B_2.BtnY480Set(tmp);
  //                      cncboard848B_1.BtnY480Set(tmp);
  //                      cncboard848B_2.BtnY480Set(tmp);
  //                      cncboard848C_1.BtnY480Set(tmp);
  //                      cncboard848C_2.BtnY480Set(tmp);
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 481, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      if (tmp < 0)
  //                      {
  //                          tmp = 0;
  //                      }
  //                      cncboard818A_1.BtnY481Set(tmp);
  //                      cncboard818A_2.BtnY481Set(tmp);
  //                      cncboard818B_1.BtnY481Set(tmp);
  //                      cncboard818B_2.BtnY481Set(tmp);
  //                      cncboard848B_1.BtnY481Set(tmp);
  //                      cncboard848B_2.BtnY481Set(tmp);
  //                      cncboard848C_1.BtnY481Set(tmp);
  //                      cncboard848C_2.BtnY481Set(tmp);
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 482, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      if (tmp < 0)
  //                      {
  //                          tmp = 0;
  //                      }
  //                      cncboard818A_1.BtnY482Set(tmp);
  //                      cncboard818A_2.BtnY482Set(tmp);
  //                      cncboard818B_1.BtnY482Set(tmp);
  //                      cncboard818B_2.BtnY482Set(tmp);
  //                      cncboard848B_1.BtnY482Set(tmp);
  //                      cncboard848B_2.BtnY482Set(tmp);
  //                      cncboard848C_1.BtnY482Set(tmp);
  //                      cncboard848C_2.BtnY482Set(tmp);
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 483, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      if (tmp < 0)
  //                      {
  //                          tmp = 0;
  //                      }
  //                      cncboard818A_1.BtnY483Set(tmp);
  //                      cncboard818A_2.BtnY483Set(tmp);
  //                      cncboard818B_1.BtnY483Set(tmp);
  //                      cncboard818B_2.BtnY483Set(tmp);
  //                      cncboard848B_1.BtnY483Set(tmp);
  //                      cncboard848B_2.BtnY483Set(tmp);
  //                      cncboard848C_1.BtnY483Set(tmp);
  //                      cncboard848C_2.BtnY483Set(tmp);
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 484, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      if (tmp < 0)
  //                      {
  //                          tmp = 0;
  //                      }
  //                      cncboard818A_1.BtnY484Set(tmp);
  //                      cncboard818A_2.BtnY484Set(tmp);
  //                      cncboard818B_1.BtnY484Set(tmp);
  //                      cncboard818B_2.BtnY484Set(tmp);
  //                      cncboard848B_1.BtnY484Set(tmp);
  //                      cncboard848B_2.BtnY484Set(tmp);
  //                      cncboard848C_1.BtnY484Set(tmp);
  //                      cncboard848C_2.BtnY484Set(tmp);
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 485, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      if (tmp < 0)
  //                      {
  //                          tmp = 0;
  //                      }
  //                      cncboard818A_1.BtnY485Set(tmp);
  //                      cncboard818A_2.BtnY485Set(tmp);
  //                      cncboard818B_1.BtnY485Set(tmp);
  //                      cncboard818B_2.BtnY485Set(tmp);
  //                      cncboard848B_1.BtnY485Set(tmp);
  //                      cncboard848B_2.BtnY485Set(tmp);
  //                      cncboard848C_1.BtnY485Set(tmp);
  //                      cncboard848C_2.BtnY485Set(tmp);
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 486, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      if (tmp < 0)
  //                      {
  //                          tmp = 0;
  //                      }
  //                      cncboard818A_1.BtnY486Set(tmp);
  //                      cncboard818A_2.BtnY486Set(tmp);
  //                      cncboard818B_1.BtnY486Set(tmp);
  //                      cncboard818B_2.BtnY486Set(tmp);
  //                      cncboard848B_1.BtnY486Set(tmp);
  //                      cncboard848B_2.BtnY486Set(tmp);
  //                      cncboard848C_1.BtnY486Set(tmp);
  //                      cncboard848C_2.BtnY486Set(tmp);
  //                  }
  //              }
  //              else
  //              {
  //                  int tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 480, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      btnBackColorSet(ref btnAuto, tmp & 0x0001);//自动,Y480.0
  //                      btnBackColorSet(ref btnSingleSegmentCNC, tmp & 0x0002);//单段,Y480.1
  //                      btnBackColorSet(ref btnManualCNC, tmp & 0x0004);//手动,Y480.2
  //                      btnBackColorSet(ref btnRunIncrementCNC, tmp & 0x0008);//增量,Y480.3
  //                      btnBackColorSet(ref btnReZeroCNC, tmp & 0x0010);//回参考点,Y480.4
  //                      btnBackColorSet(ref btnhuandaozhuangdaoCNC, tmp & 0x0020);//换刀允许,Y480.5
  //                      btnBackColorSet(ref btndaosongjingCNC, tmp & 0x0040);//刀具松紧,Y480.6
  //                      btnBackColorSet(ref btnkongrunCNC, tmp & 0x0080);//空运行,Y480.7
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 481, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      btnBackColorSet(ref btnGCSkipCNC, tmp & 0x0001);//程序跳段,Y481.0
  //                      btnBackColorSet(ref btnxuanzhetingCNC, tmp & 0x0002);//选择停,Y481.1
  //                      btnBackColorSet(ref btnZsuoCNC, tmp & 0x0004);//Z轴锁住,Y481.2
  //                      btnBackColorSet(ref btnjichuangshuoCNC, tmp & 0x0008);//机床锁住,Y481.3
  //                      btnBackColorSet(ref btnProtectDoorCNC, tmp & 0x0010);//防护门,Y481.4
  //                      btnBackColorSet(ref btnzhaomingCNC, tmp & 0x0020);//机床照明,Y481.5
  //                      btnBackColorSet(ref btnToolzhengzhuan, tmp & 0x0040);//刀库正转,Y481.6
  //                      btnBackColorSet(ref btnToolfanzhuan, tmp & 0x0080);//刀库反转,Y481.7
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 482, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      btnBackColorSet(ref btnaddACNC, tmp & 0x0001);//+A,Y482.0
  //                      btnBackColorSet(ref btnaddzCNC, tmp & 0x0002);//+Z,Y482.1
  //                      btnBackColorSet(ref btnsubyCNC, tmp & 0x0004);//-Y,Y482.2
  //                      btnBackColorSet(ref btnF1CNC, tmp & 0x0008);//新F1,Y482.3
  //                      btnBackColorSet(ref btnF2CNC, tmp & 0x0010);//x10,Y482.4
  //                      btnBackColorSet(ref btnF3CNC, tmp & 0x0020);//x100,Y482.5
  //                      btnBackColorSet(ref btnF4CNC, tmp & 0x0040);//x1000,Y482.6
  //                      btnBackColorSet(ref btnF1, tmp & 0x0080);//F1,Y482.6
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 483, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      btnBackColorSet(ref btnhuandaozhuangdaoCNC, tmp & 0x0001);//换刀/装刀,Y483.0
  //                      btnBackColorSet(ref btnaddxCNC, tmp & 0x0002);//+X,Y483.1
  //                      btnBackColorSet(ref btnkuaijingCNC, tmp & 0x0004);//快进,Y483.2
  //                      btnBackColorSet(ref btnsubxCNC, tmp & 0x0008);//-X,Y483.3
  //                      btnBackColorSet(ref btnzhuzhoudingxiangCNC, tmp & 0x0010);//主轴定向,Y483.4
  //                      btnBackColorSet(ref btnshouyaoshiqie, tmp & 0x0020);//手摇试切,Y483.5
  //                      btnBackColorSet(ref btnChongxue, tmp & 0x0040);//冲屑,Y483.6
  //                      btnBackColorSet(ref btnCoolingCNC, tmp & 0x0080);//冷却,Y483.7
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 484, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      btnBackColorSet(ref btnToolHome, tmp & 0x0001);//刀库回零,Y484.0
  //                      btnBackColorSet(ref button3AxsHome, tmp & 0x0002);//三轴回零,Y484.0
  //                      btnBackColorSet(ref btnaddyCNC, tmp & 0x0004);//+Y,Y484.2
  //                      btnBackColorSet(ref btnsubzCNC, tmp & 0x0008);//-Z,Y484.3
  //                      btnBackColorSet(ref btnsubACNC, tmp & 0x0010);//-A,Y484.4
  //                      btnBackColorSet(ref btnSpindleForwardCNC, tmp & 0x0020);//主轴正转,Y484.5
  //                      btnBackColorSet(ref btnSpindleStopCNC, tmp & 0x0040);//主轴停止,Y484.6
  //                      btnBackColorSet(ref btnzhuzhoufanzhuangCNC, tmp & 0x0080);//主轴反转,Y484.7
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 485, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      btnBackColorSet(ref btnLubricationCNC, tmp & 0x0001);//润滑,Y485.0
  //                      btnBackColorSet(ref btnchaochenjiechuCNC, tmp & 0x0004);//解除超程,Y485.2
  //                      btnBackColorSet(ref btnjichuangshuoCNC, tmp & 0x0008);//机床锁住,Y485.3
  //                      //btnBackColorSet(ref btnProtectDoorCNC, tmp & 0x0010);//防护门,Y485.4
  //                      //btnBackColorSet(ref btnzhaomingCNC, tmp & 0x0020);//机床照明,Y485.5
  //                      btnBackColorSet(ref btnToolzhengzhuan, tmp & 0x0040);//进给保持2,Y485.6
  //                      btnBackColorSet(ref btnToolTest, tmp & 0x0080);//手动换刀,Y485.7
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_Y, 486, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      btnBackColorSet(ref btnxhqdCNC, tmp & 0x0010);//循环启动,Y486.4
  //                      btnBackColorSet(ref btnFeedHoldRunCNC, tmp & 0x0020);//进给保持,Y486.5
  //                  }
  //                  tmp = 0;
  //                  if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_R, 29, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
  //                  {
  //                      btnBackColorSet(ref btnEStopCNC, tmp & 0x0010);//急停,R29.4
  //                  }
  //              }
		//	}
            
		//}
        /// <summary>
        /// 更新运行界面按钮显示状态
        /// </summary>
        private void UpdataButtonShow(CNCV2 cnctemp)
        {
            if (cnctemp != null)
            {

                //if(cnc.Type == "HNC_848C")
                if (cnctemp.cnctype == CNCType.CNC || cnctemp.cnctype == CNCType.Lathe)
                {
                    int tmp = cnctemp.CNCButtonY480;

                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    cncboard818A_1.BtnY480Set(tmp);
                    cncboard818A_2.BtnY480Set(tmp);
                    cncboard818B_1.BtnY480Set(tmp);
                    cncboard818B_2.BtnY480Set(tmp);
                    cncboard848B_1.BtnY480Set(tmp);
                    cncboard848B_2.BtnY480Set(tmp);
                    cncboard848C_1.BtnY480Set(tmp);
                    cncboard848C_2.BtnY480Set(tmp);

                    tmp = cnctemp.CNCButtonY481;

                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    cncboard818A_1.BtnY481Set(tmp);
                    cncboard818A_2.BtnY481Set(tmp);
                    cncboard818B_1.BtnY481Set(tmp);
                    cncboard818B_2.BtnY481Set(tmp);
                    cncboard848B_1.BtnY481Set(tmp);
                    cncboard848B_2.BtnY481Set(tmp);
                    cncboard848C_1.BtnY481Set(tmp);
                    cncboard848C_2.BtnY481Set(tmp);

                    tmp = cnctemp.CNCButtonY482;

                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    cncboard818A_1.BtnY482Set(tmp);
                    cncboard818A_2.BtnY482Set(tmp);
                    cncboard818B_1.BtnY482Set(tmp);
                    cncboard818B_2.BtnY482Set(tmp);
                    cncboard848B_1.BtnY482Set(tmp);
                    cncboard848B_2.BtnY482Set(tmp);
                    cncboard848C_1.BtnY482Set(tmp);
                    cncboard848C_2.BtnY482Set(tmp);

                    tmp = cnctemp.CNCButtonY483;

                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    cncboard818A_1.BtnY483Set(tmp);
                    cncboard818A_2.BtnY483Set(tmp);
                    cncboard818B_1.BtnY483Set(tmp);
                    cncboard818B_2.BtnY483Set(tmp);
                    cncboard848B_1.BtnY483Set(tmp);
                    cncboard848B_2.BtnY483Set(tmp);
                    cncboard848C_1.BtnY483Set(tmp);
                    cncboard848C_2.BtnY483Set(tmp);

                    tmp = cnctemp.CNCButtonY484;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    cncboard818A_1.BtnY484Set(tmp);
                    cncboard818A_2.BtnY484Set(tmp);
                    cncboard818B_1.BtnY484Set(tmp);
                    cncboard818B_2.BtnY484Set(tmp);
                    cncboard848B_1.BtnY484Set(tmp);
                    cncboard848B_2.BtnY484Set(tmp);
                    cncboard848C_1.BtnY484Set(tmp);
                    cncboard848C_2.BtnY484Set(tmp);

                    tmp = cnctemp.CNCButtonY485;

                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    cncboard818A_1.BtnY485Set(tmp);
                    cncboard818A_2.BtnY485Set(tmp);
                    cncboard818B_1.BtnY485Set(tmp);
                    cncboard818B_2.BtnY485Set(tmp);
                    cncboard848B_1.BtnY485Set(tmp);
                    cncboard848B_2.BtnY485Set(tmp);
                    cncboard848C_1.BtnY485Set(tmp);
                    cncboard848C_2.BtnY485Set(tmp);

                    tmp = cnctemp.CNCButtonY486;

                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    cncboard818A_1.BtnY486Set(tmp);
                    cncboard818A_2.BtnY486Set(tmp);
                    cncboard818B_1.BtnY486Set(tmp);
                    cncboard818B_2.BtnY486Set(tmp);
                    cncboard848B_1.BtnY486Set(tmp);
                    cncboard848B_2.BtnY486Set(tmp);
                    cncboard848C_1.BtnY486Set(tmp);
                    cncboard848C_2.BtnY486Set(tmp);

                }
                else
                {
                    int tmp = cnctemp.CNCButtonY480;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    btnBackColorSet(ref btnAuto, tmp & 0x0001);//自动,Y480.0
                    btnBackColorSet(ref btnSingleSegmentCNC, tmp & 0x0002);//单段,Y480.1
                    btnBackColorSet(ref btnManualCNC, tmp & 0x0004);//手动,Y480.2
                    btnBackColorSet(ref btnRunIncrementCNC, tmp & 0x0008);//增量,Y480.3
                    btnBackColorSet(ref btnReZeroCNC, tmp & 0x0010);//回参考点,Y480.4
                    btnBackColorSet(ref btnhuandaozhuangdaoCNC, tmp & 0x0020);//换刀允许,Y480.5
                    btnBackColorSet(ref btndaosongjingCNC, tmp & 0x0040);//刀具松紧,Y480.6
                    btnBackColorSet(ref btnkongrunCNC, tmp & 0x0080);//空运行,Y480.7

                    tmp = cnctemp.CNCButtonY481;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    btnBackColorSet(ref btnGCSkipCNC, tmp & 0x0001);//程序跳段,Y481.0
                    btnBackColorSet(ref btnxuanzhetingCNC, tmp & 0x0002);//选择停,Y481.1
                    btnBackColorSet(ref btnZsuoCNC, tmp & 0x0004);//Z轴锁住,Y481.2
                    btnBackColorSet(ref btnjichuangshuoCNC, tmp & 0x0008);//机床锁住,Y481.3
                    btnBackColorSet(ref btnProtectDoorCNC, tmp & 0x0010);//防护门,Y481.4
                    btnBackColorSet(ref btnzhaomingCNC, tmp & 0x0020);//机床照明,Y481.5
                    btnBackColorSet(ref btnToolzhengzhuan, tmp & 0x0040);//刀库正转,Y481.6
                    btnBackColorSet(ref btnToolfanzhuan, tmp & 0x0080);//刀库反转,Y481.7

                    tmp = cnctemp.CNCButtonY482;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    btnBackColorSet(ref btnaddACNC, tmp & 0x0001);//+A,Y482.0
                    btnBackColorSet(ref btnaddzCNC, tmp & 0x0002);//+Z,Y482.1
                    btnBackColorSet(ref btnsubyCNC, tmp & 0x0004);//-Y,Y482.2
                    btnBackColorSet(ref btnF1CNC, tmp & 0x0008);//新F1,Y482.3
                    btnBackColorSet(ref btnF2CNC, tmp & 0x0010);//x10,Y482.4
                    btnBackColorSet(ref btnF3CNC, tmp & 0x0020);//x100,Y482.5
                    btnBackColorSet(ref btnF4CNC, tmp & 0x0040);//x1000,Y482.6
                    btnBackColorSet(ref btnF1, tmp & 0x0080);//F1,Y482.6
                    tmp = cnctemp.CNCButtonY483;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    btnBackColorSet(ref btnhuandaozhuangdaoCNC, tmp & 0x0001);//换刀/装刀,Y483.0
                    btnBackColorSet(ref btnaddxCNC, tmp & 0x0002);//+X,Y483.1
                    btnBackColorSet(ref btnkuaijingCNC, tmp & 0x0004);//快进,Y483.2
                    btnBackColorSet(ref btnsubxCNC, tmp & 0x0008);//-X,Y483.3
                    btnBackColorSet(ref btnzhuzhoudingxiangCNC, tmp & 0x0010);//主轴定向,Y483.4
                    btnBackColorSet(ref btnshouyaoshiqie, tmp & 0x0020);//手摇试切,Y483.5
                    btnBackColorSet(ref btnChongxue, tmp & 0x0040);//冲屑,Y483.6
                    btnBackColorSet(ref btnCoolingCNC, tmp & 0x0080);//冷却,Y483.7

                    tmp = cnctemp.CNCButtonY483;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    btnBackColorSet(ref btnToolHome, tmp & 0x0001);//刀库回零,Y484.0
                    btnBackColorSet(ref button3AxsHome, tmp & 0x0002);//三轴回零,Y484.0
                    btnBackColorSet(ref btnaddyCNC, tmp & 0x0004);//+Y,Y484.2
                    btnBackColorSet(ref btnsubzCNC, tmp & 0x0008);//-Z,Y484.3
                    btnBackColorSet(ref btnsubACNC, tmp & 0x0010);//-A,Y484.4
                    btnBackColorSet(ref btnSpindleForwardCNC, tmp & 0x0020);//主轴正转,Y484.5
                    btnBackColorSet(ref btnSpindleStopCNC, tmp & 0x0040);//主轴停止,Y484.6
                    btnBackColorSet(ref btnzhuzhoufanzhuangCNC, tmp & 0x0080);//主轴反转,Y484.7

                    tmp = cnctemp.CNCButtonY485;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    btnBackColorSet(ref btnLubricationCNC, tmp & 0x0001);//润滑,Y485.0
                    btnBackColorSet(ref btnchaochenjiechuCNC, tmp & 0x0004);//解除超程,Y485.2
                    btnBackColorSet(ref btnjichuangshuoCNC, tmp & 0x0008);//机床锁住,Y485.3
                                                                          //btnBackColorSet(ref btnProtectDoorCNC, tmp & 0x0010);//防护门,Y485.4
                                                                          //btnBackColorSet(ref btnzhaomingCNC, tmp & 0x0020);//机床照明,Y485.5
                    btnBackColorSet(ref btnToolzhengzhuan, tmp & 0x0040);//进给保持2,Y485.6
                    btnBackColorSet(ref btnToolTest, tmp & 0x0080);//手动换刀,Y485.7

                    tmp = cnctemp.CNCButtonY486;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    btnBackColorSet(ref btnxhqdCNC, tmp & 0x0010);//循环启动,Y486.4
                    btnBackColorSet(ref btnFeedHoldRunCNC, tmp & 0x0020);//进给保持,Y486.5

                    tmp = cnctemp.CNCButtonR29;
                    if (tmp < 0)
                    {
                        tmp = 0;
                    }
                    btnBackColorSet(ref btnEStopCNC, tmp & 0x0010);//急停,R29.4

                }
            }

        }

        delegate void richTextBoxCurrentProgramRunAppendCallback(List<string> Gcodetxt, int lineNum);
        public void AppendMessage(List<string> Gcodetxt, int lineNum) 
        {
            if (richTextBoxCurrentProgramRun.InvokeRequired) 
            {
                richTextBoxCurrentProgramRunAppendCallback d = new richTextBoxCurrentProgramRunAppendCallback(AppendMessage);
                richTextBoxCurrentProgramRun.Invoke(d, new object[] { Gcodetxt, lineNum });
            } 
            else 
            {
                try
                {
                    UpdataGCodeShow(Gcodetxt, lineNum);
                }
                catch (System.Exception ex)
                {
                    LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
                    SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                    SendParm.LevelIndex = (int)LogData.Node2Level.ERROR;
                    SendParm.DisplayLang = SCADA.MainForm.CurrentLanguage;
                    SendParm.EventID = ((int)LogData.Node2Level.ERROR).ToString();
                    SendParm.Keywords = SCADA.ChangeLanguage.GetString("LogContentGCodeDisplayError");
                    SendParm.EventData = ex.ToString();
                    //                     SendParm.Provider = "MainForm";
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(null, SendParm, null, null);
                }
            } 
        }


        /// <summary>
        /// 更新G代码显示
        /// </summary>
        /// <param name="name">G代码文件名</param>
        /// <param name="lineNum">执行到第几行了</param>
        ///                     static int up_stari = 0;
        int up_stari = 0;
        int up_endi = 0;
        private void UpdataGCodeShow(List<string> Gcodetxt, int lineNum)
        {
            string str = "";
            if (Gcodetxt.Count != 0)
            {
                ThreaSetLaBText(richTextBoxCurrentProgramRun, "");
               
                int lineID = 0;
                foreach (var a in Gcodetxt)
                {
                    str = "N";
                    str += lineID.ToString("D4");
                    str += "   ";
                    str += a.ToString();
                    str += Environment.NewLine;
                    lineID++;
                    richTextBoxCurrentProgramRun.AppendText(str);

                }
            }

            if (richTextBoxCurrentProgramRun.Text.Length > 0)
            {
                str = "";
                str += "N";
                str += lineNum.ToString("D4");
                str += "   ";
                int stari = richTextBoxCurrentProgramRun.Text.IndexOf(str);
                lineNum++;
                str = "";
                str += "N";
                str += lineNum.ToString("D4");
                str += "   ";
                int endi = richTextBoxCurrentProgramRun.Text.IndexOf(str);
                if (stari != -1)
                {
                    if (endi != -1)//最后一行
                    {
                        endi = endi - stari;
                    }
                    else
                    {
                        endi = richTextBoxCurrentProgramRun.Text.Length - stari;
                    }
                    richTextBoxCurrentProgramRun.Select(up_stari, up_endi);
                    richTextBoxCurrentProgramRun.SelectionBackColor = richTextBoxCurrentProgramRun.BackColor;
                    richTextBoxCurrentProgramRun.Select(stari, endi);
                    richTextBoxCurrentProgramRun.SelectionBackColor = Color.BurlyWood;
//                     richTextBoxCurrentProgramRun.ScrollToCaret();//此行导致程序崩溃
                    up_stari = stari;
                    up_endi = endi;
                }
            }
        }


        //private void Updata_NCStandardPanel_Value(CNC cnctmp)
        //{
        //    if (labFSpeed.Visible)
        //    {
        //        ThreaSetLaBText(labFSpeed, cnctmp.get_act_feedrate().ToString("F1"));
        //        ThreaSetLaBText(labSpdlSpeed, cnctmp.get_act_spdl_speed().ToString("F1"));
        //        ThreaSetLaBText(labFSpeedRate, cnctmp.get_feed_override().ToString("D3") + "%");
        //        ThreaSetLaBText(labRapidRate, cnctmp.get_rapid_override().ToString("D3") + "%");
        //        ThreaSetLaBText(labSpdlRate, cnctmp.get_spdl_override().ToString("D3") + "%");
        //        int gcrunningline = (int)cnctmp.get_run_row();
        //        int gOldline = 0;
        //        List<string> GcodeContent = new List<string>();
        //        string stringgcode = cnctmp.get_gCodeName();
        //        //Console.WriteLine("G代码名称:" + stringgcode);
        //        GcodeContent = cnctmp.netFileGet(cnctmp.get_gCodeName(), cnctmp.dbNo);
        //        if (int.TryParse(labCurLineNo.Text, out gOldline) && gcrunningline != gOldline)
        //        {
        //            ThreaSetLaBText(labCurLineNo, gcrunningline.ToString("D4"));
        //            UpdataGCodeShow(GcodeContent, gcrunningline);
        //        }
        //        int axis = 0;
        //        for (int i = 0; i < cnc.HCNCShareData.axlist.Count; i++)
        //        {
        //            axis = cnc.HCNCShareData.axlist[i].axisNo;
        //            double cmdData = 0, wcsData = 0, cur_i = 0, eding_i = 0;
        //            int moveunit = 1;// 00000;
        //            cmdData = cnctmp.get_axis_act_pos(axis);
        //            wcsData = cnctmp.get_axis_cmd_pos(axis);
        //            cur_i = cnctmp.get_axis_rated_cur(axis);
        //            eding_i = cnctmp.get_axis_load_cur(axis);
        //            int cur_i_show = 0;
        //            if (eding_i != 0)
        //            {
        //                cur_i_show = (int)((eding_i/cur_i) * 100);
        //                if (cur_i_show < 0)
        //                {
        //                    cur_i_show = 0;
        //                }
        //                else if (cur_i_show > 150)
        //                {
        //                    cur_i_show = 150;
        //                }
        //            }
        //            //moveunit = cnctmp.get_sys_move_unit();
        //            if (axis == 0)
        //            {
        //                ThreaSetLaBText(label_acpos_x, ((double)cmdData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_cmdpos_x, ((double)wcsData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_aci_x, cur_i_show.ToString() + "%");

        //                if (cur_i_show <= 50)
        //                {
        //                    //progressBar_ruti_x.bacColor = Color.Gray;
        //                }
        //                else if (cur_i_show <= 100)
        //                {
        //                    //progressBar_ruti_x.ForeColor = Color.Green;
        //                }
        //                else
        //                {
        //                    //progressBar_ruti_x.ForeColor = Color.Red;
        //                }
        //                progressBar_ruti_x.Value = cur_i_show;
                        
        //            }
        //            else if (axis == 1)
        //            {
        //                ThreaSetLaBText(label_acpos_y, ((double)cmdData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_cmdpos_y, ((double)wcsData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_aci_y, cur_i_show.ToString() + "%");

        //                progressBar_ruti_y.Value = cur_i_show;
        //            }
        //            else if (axis == 2)
        //            {
        //                ThreaSetLaBText(label_acpos_z, ((double)cmdData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_cmdpos_z, ((double)wcsData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_aci_z, cur_i_show.ToString() + "%");

        //                progressBar_ruti_z.Value = cur_i_show;
        //            }
        //            else if (axis == 3)
        //            {
        //                ThreaSetLaBText(label_acpos_a, ((double)cmdData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_cmdpos_a, ((double)wcsData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_aci_a, cur_i_show.ToString() + "%");

        //                progressBar_ruti_z.Value = cur_i_show; 
        //            }
        //            else if (axis == 5)
        //            {
        //                ThreaSetLaBText(label_acpos_c, ((double)cmdData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_cmdpos_c, ((double)wcsData / moveunit).ToString("F3"));
        //                ThreaSetLaBText(label_aci_c, cur_i_show.ToString() + "%");

        //                progressBar_ruti_c.Value = cur_i_show;
        //            }
        //        }
        //    }
        //}
        private void Updata_NCStandardPanel_Value(CNCV2 cnctmp)
        {
            if (labFSpeed.Visible)
            {
                ThreaSetLaBText(labFSpeed, cnctmp.FeedRate.ToString("F3"));
                ThreaSetLaBText(labSpdlSpeed, cnctmp.SpindleSpeed.ToString("F3"));
                ThreaSetLaBText(labFSpeedRate, cnctmp.SpeedRate.ToString("F3") + "%");
                ThreaSetLaBText(labRapidRate, cnctmp.RapidRate.ToString("F3") + "%");
                ThreaSetLaBText(labSpdlRate, cnctmp.SpdlRate.ToString("F3") + "%");
                int gcrunningline = cnctmp.InstructionsCount;
                int gOldline = 0;
                List<string> GcodeContent = new List<string>();
                string stringgcode = cnctmp.GProgram;
                //Console.WriteLine("G代码名称:" + stringgcode);
                if (cnctmp.CurProgContentLine == null)
                {
                    GcodeContent = null;

                }
                else
                {
                    for (int i = 0; i < cnctmp.CurProgContentLine.Length; i++)
                    {
                        GcodeContent.Add(cnctmp.CurProgContentLine[i]);
                    }
                    if (int.TryParse(labCurLineNo.Text, out gOldline) && gcrunningline != gOldline)
                    {

                        ThreaSetLaBText(labCurLineNo, gcrunningline.ToString("D4"));
                        UpdataGCodeShow(GcodeContent, gcrunningline);
                    }
                }

                int axis = 0;
                for (int i = 0; i < 5; i++)
                {
                    axis = i;
                    if (axis == 0)
                    {
                        ThreaSetLaBText(label_acpos_x, cnctmp.SiteX.ToString("F3"));
                        ThreaSetLaBText(label_cmdpos_x, cnctmp.SiteXF.ToString("F3"));
                        ThreaSetLaBText(label_aci_x, cnctmp.LoadX.ToString() + "%");
                        progressBar_ruti_x.Value = (int)cnctmp.LoadX;

                    }
                    else if (axis == 1)
                    {
                        ThreaSetLaBText(label_acpos_y, cnctmp.SiteY.ToString("F3"));
                        ThreaSetLaBText(label_cmdpos_y, cnctmp.SiteYF.ToString("F3"));
                        ThreaSetLaBText(label_aci_y, cnctmp.LoadY.ToString() + "%");

                        progressBar_ruti_y.Value = (int)cnctmp.LoadY;
                    }
                    else if (axis == 2)
                    {
                        ThreaSetLaBText(label_acpos_z, cnctmp.SiteZ.ToString("F3"));
                        ThreaSetLaBText(label_cmdpos_z, cnctmp.SiteZF.ToString("F3"));
                        ThreaSetLaBText(label_aci_z, cnctmp.LoadZ.ToString() + "%");

                        progressBar_ruti_z.Value = (int)cnctmp.LoadZ;
                    }

                    else if (axis == 5)
                    {
                        ThreaSetLaBText(label_acpos_c, cnctmp.SiteC.ToString("F3"));
                        ThreaSetLaBText(label_cmdpos_c, cnctmp.SiteCF.ToString("F3"));
                        ThreaSetLaBText(label_aci_c, cnctmp.LoadC.ToString() + "%");

                        progressBar_ruti_c.Value = (int)cnctmp.LoadC;
                    }
                }
            }
        }
        /// <summary>
        /// 初始化dataGridView_REG列表
        /// </summary>
        /// <param name="cnctmp"></param>
        //private void InitdataGridView_REG(CNC cnctmp)
        //{
        //    dataGridView_REG.Rows.Clear();

        //    int index,tmp = 0;
        //    reg_num_x = cnctmp.get_reg_num((int)HncRegType.REG_TYPE_X);
        //    reg_num_y = cnctmp.get_reg_num((int)HncRegType.REG_TYPE_Y);
        //    reg_num_f = cnctmp.get_reg_num((int)HncRegType.REG_TYPE_F);
        //    reg_num_g = cnctmp.get_reg_num((int)HncRegType.REG_TYPE_G);
        //    reg_num_r = cnctmp.get_reg_num((int)HncRegType.REG_TYPE_R);
        //    reg_num_b = cnctmp.get_reg_num((int)HncRegType.REG_TYPE_B);
        //    tmp = (reg_num_x > reg_num_y) ? reg_num_x : reg_num_y;
        //    tmp = (tmp > reg_num_f) ? tmp : reg_num_f;
        //    tmp = (tmp > reg_num_g) ? tmp : reg_num_g;
        //    tmp = (tmp > reg_num_r) ? tmp : reg_num_r;
        //    tmp = (tmp > reg_num_b) ? tmp : reg_num_b;

        //    for (index = 0; index < tmp; index++)
        //    {
        //        int ii = dataGridView_REG.Rows.Add();
        //        dataGridView_REG.Rows[ii].Cells[0].Value = ii;
        //    }
        //    /*if (tmp > dataGridView_REG.RowCount)
        //    {
        //        for (index = dataGridView_REG.RowCount ; index < tmp; index++)
        //        {
        //            int ii = dataGridView_REG.Rows.Add();
        //            dataGridView_REG.Rows[ii].Cells[0].Value = ii;
        //        }
        //    }
        //    else if (tmp < dataGridView_REG.RowCount )
        //    {
        //        for (index = dataGridView_REG.RowCount - 1; index >= tmp; index--)
        //        {
        //            dataGridView_REG.Rows.RemoveAt(index);
        //        }
        //    }*/
        //}


        /// <summary>
        /// 刷新寄存器
        /// </summary>
        /// <param name="cnctmp"></param> 
    //    private void Updata_Reg_Value()
    //    {
    //        if (dataGridView_REG.Visible)
    //        {
    //            int index_1,index_2;
    //            int dgvminrow = dataGridView_REG.Rows.GetFirstRow(DataGridViewElementStates.Displayed);//显示在屏幕上的第一行
    //            int dgvmaxrow = dataGridView_REG.Rows.GetLastRow(DataGridViewElementStates.Displayed);//显示在屏幕上的最后一行
    //            if (dgvminrow < 0 || dgvminrow > dgvmaxrow || dgvmaxrow >= dataGridView_REG.Rows.Count)
    //            {
    //                return;
    //            }
    //            for (index_1 = dgvminrow; index_1 < dgvmaxrow; index_1++)
    //            {
    //                for (index_2 = 1; index_2 < dataGridView_REG.ColumnCount; index_2++)
    //                {
    //                    HncRegType Reg_type = HncRegType.REG_TYPE_X;
    //                    if (index_2 == 1)
    //                    {
    //                        if (index_1 >= reg_num_x)
    //                        {
    //                            continue;
    //                        }
    //                    }
    //                    else if (index_2 == 2)
    //                    {
    //                        if (index_1 >= reg_num_y)
    //                        {
    //                            continue;
    //                        }
    //                        Reg_type = HncRegType.REG_TYPE_Y;
    //                    }
    //                    else if (index_2 == 3)
    //                    {
    //                        if (index_1 >= reg_num_f)
    //                        {
    //                            continue;
    //                        }
    //                        Reg_type = HncRegType.REG_TYPE_F;
    //                    }
    //                    else if (index_2 == 4)
    //                    {
    //                        if (index_1 >= reg_num_g)
    //                        {
    //                            continue;
    //                        }
    //                        Reg_type = HncRegType.REG_TYPE_G;
    //                    }
    //                    else if (index_2 == 5)
    //                    {
    //                        if (index_1 >= reg_num_r)
    //                        {
    //                            continue;
    //                        }
    //                        Reg_type = HncRegType.REG_TYPE_R;
    //                    }
    //                    else if (index_2 == 6)
    //                    {
    //                        if (index_1 >= reg_num_b)
    //                        {
    //                            continue;
    //                        }
    //                        Reg_type = HncRegType.REG_TYPE_B;
    //                    }
                        
    //                    //hxb  2017.4.1
    //                    dataGridView_REG.Rows[index_1].Cells[index_2].Value = GetRegValueChage2string(Reg_type, index_1, RegValeShowType);
    //                    //dataGridView_REG.Rows[index_1].Cells[index_2].Value =
    ////GetRegValueChage2string(Reg_type, int.Parse(dataGridView_REG.Rows[index_1].Cells[0].Value.ToString()), RegValeShowType);

    //                }
    //            }
    //        }
    //    }

        /// <summary>
        /// 从下位机中获取到对应寄存器值，将其转换为二进制或者十进制或者十六进制
        /// 返回一个字符串
        /// </summary>
        /// <param name="Reg_type">寄存器类型</param>
        /// <param name="Ren_index">寄存器index</param>
        /// <param name="flag">0：二进制，1：十进制，2：十六进制</param>
        //private string GetRegValueChage2string(HncRegType Reg_type, int Ren_index, int flag)
        //{
        //    string str = "";
        //    int tmp = 0;
        //    uint uint_tmp = 0,shi = 0;
        //    if (cnc.isConnected() && Collector.CollectShare.GetRegValue((int)Reg_type, Ren_index, out tmp, cnc.HCNCShareData.sysData.dbNo) == 0)
        //    {
        //        if (Reg_type == HncRegType.REG_TYPE_X || Reg_type == HncRegType.REG_TYPE_Y || Reg_type == HncRegType.REG_TYPE_R)
        //        {
        //            tmp &= 0x00ff;                  //因为X，Y,R及R寄存器是8位的
        //            uint_tmp = 7;
        //            shi = 3;
        //        }
        //        else if (Reg_type == HncRegType.REG_TYPE_F || Reg_type == HncRegType.REG_TYPE_G )
        //        {
        //            tmp &= 0xffff;                  //因为F，G寄存器是16位的
        //            uint_tmp = 15;
        //            shi = 5;
        //        }
        //        else if (Reg_type == HncRegType.REG_TYPE_B)
        //        {
        //            uint_tmp = (uint)tmp;
        //            uint_tmp &= 0xffffffff;                  //因为B及R寄存器是32位的
        //            tmp = (int)uint_tmp;
        //            uint_tmp = 31;
        //            shi = 10;
        //        }
        //        if (flag == 0)//二进制
        //        {
        //            str = Convert.ToString(tmp, 2);
        //            str += "B";
        //            while (str.Length - 1 < uint_tmp + 1)
        //                str = "0" + str;
        //        }
        //        else if (flag == 1)//十进制
        //        {
        //            str = tmp.ToString();
        //            str += "D";
        //            while (str.Length - 1 < shi)
        //                str = "0" + str;
        //        }
        //        else if (flag == 2)//十六进制
        //        {
        //            str = Convert.ToString(tmp, 16);
        //            str += "H";
        //            while (str.Length - 1 < (uint_tmp + 1) / 4)
        //                str = "0" + str;
        //        }
        //    }
        //    return str;
        //}

        /// <summary>
        /// 初始化刀补列表
        /// </summary>
        /// <param name="cnctmp"></param>
        //private void InitdataGridView_Comp()
        //{
        //    List<String> toolArray = MacDataService.GetInstance().GetHashAllString(cnc.HCNCShareData.sysData.dbNo, "Tool:List");
        //    if (toolArray != null)
        //    {
        //        dataGridView_Comp.Rows.Clear();
        //        foreach (String toolStr in toolArray)
        //        {
        //            int i = dataGridView_Comp.Rows.Add();
        //            dataGridView_Comp.Rows[i].Cells[0].Value = i+1;
        //        }
        //    }
        //}

        /// <summary>
        /// 初始化刀补列表
        /// </summary>
        /// <param name="cnctmp"></param>
        private void InitdataGridView_Comp(bool t = true)
        {

            dataGridView_Comp.Rows.Clear();
            for (int i = 0; i < cncv2.ToolNum; i++)
            {
                dataGridView_Comp.Rows.Add();
                dataGridView_Comp.Rows[i].Cells[0].Value = i + 1;
            }

        }


        /// <summary>
        /// 刷新刀补表
        /// </summary>
        /// <param name="cnctmp"></param> 
        //private void Updata_Comp_Value()
        //{
        //    if (dataGridView_Comp.Visible)
        //    {
        //        for(int i=0;i<dataGridView_Comp.Rows.Count; i++)
        //        {
        //            string toolStr = "";
        //            int ret = MacDataService.GetInstance().GetHashKeyValueString(cnc.HCNCShareData.sysData.dbNo, "Tool:List", String.Format("{0:D4}", i+1), ref toolStr);
        //            if (ret == 0)
        //            {
        //                if (comboBoxCNC.SelectedIndex == 0)//车床刀具长度乘以2
        //                {
        //                    string temps = "";
        //                    double tempi = 0;
        //                    toolComp tool = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr);
        //                    dataGridView_Comp.Rows[i].Cells[0].Value = i + 1;//刀号
        //                    tempi = tool.GTOOL_LEN1;
        //                    dataGridView_Comp.Rows[i].Cells[1].Value = tempi.ToString("F2");//长度

        //                    tempi = tool.GTOOL_RAD1;
        //                    dataGridView_Comp.Rows[i].Cells[2].Value = tempi.ToString("F2");//半径

        //                    tempi = tool.WTOOL_LEN1;
        //                    dataGridView_Comp.Rows[i].Cells[3].Value = tempi.ToString("F2"); //长度磨损

        //                    tempi = tool.WTOOL_RAD1;
        //                    dataGridView_Comp.Rows[i].Cells[4].Value = tempi.ToString("F2");//半径磨损;
        //                }
        //                else
        //                {
        //                    toolComp tool = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr);
        //                    dataGridView_Comp.Rows[i].Cells[0].Value = i + 1;//刀号
        //                    dataGridView_Comp.Rows[i].Cells[1].Value = tool.GTOOL_LEN1.ToString("F2");//长度
        //                    dataGridView_Comp.Rows[i].Cells[2].Value = tool.GTOOL_RAD1.ToString("F2");//半径
        //                    dataGridView_Comp.Rows[i].Cells[3].Value = tool.WTOOL_LEN1.ToString("F2");//长度磨损
        //                    dataGridView_Comp.Rows[i].Cells[4].Value = tool.WTOOL_RAD1.ToString("F2");//半径磨损
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 刷新刀补表
        /// </summary>
        /// <param name="cnctmp"></param> 
        private void Updata_Comp_Value(bool t = true)
        {
            if (dataGridView_Comp.Visible)
            {
                for (int i = 0; i < dataGridView_Comp.Rows.Count; i++)
                {

                    dataGridView_Comp.Rows[i].Cells[0].Value = i + 1;//刀号
                    dataGridView_Comp.Rows[i].Cells[1].Value = cncv2.TOOLData[i].Length.ToString();//长度
                    dataGridView_Comp.Rows[i].Cells[2].Value = cncv2.TOOLData[i].Radius.ToString();//半径
                    dataGridView_Comp.Rows[i].Cells[3].Value = cncv2.TOOLData[i].LengthComp.ToString();//长度磨损
                    dataGridView_Comp.Rows[i].Cells[4].Value = cncv2.TOOLData[i].RadiusComp.ToString();//长度磨损//半径磨损


                }
            }
        }

        private void Updata_Alarm_Value(bool t = true)
        {
            if (dataGridViewalarmdata.Visible)
            {
                if (cncv2.Alarms == null|| cncv2.Alarms.Count ==0)
                {
                    if(dataGridViewalarmdata.Rows.Count >0)
                    {
                        dataGridViewalarmdata.Rows.Clear();
                    }
                    return;
                }
                int ii = 0;
                foreach (KeyValuePair<int, string> temp in cncv2.Alarms)
                {
                    if (dataGridViewalarmdata.Rows.Count < ii + 1)
                    {
                        dataGridViewalarmdata.Rows.Add();
                    }
                    var n = ii + 1;
                    dataGridViewalarmdata.Rows[ii].Cells[0].Value = n.ToString();//刀号
                    dataGridViewalarmdata.Rows[ii].Cells[1].Value = temp.Key.ToString();//长度
                    var ss = temp.Value.ToString();
                    dataGridViewalarmdata.Rows[ii].Cells[2].Value = ss.ToString();//半径
                    ii++;

                }
            }
        }
        /// <summary>

        private void radioButtonRegValBit_CheckedChanged(object sender, EventArgs e)
        {
            RegValeShowType = 0;
        }

        private void radioButtonRegValDecimal_CheckedChanged(object sender, EventArgs e)
        {
            RegValeShowType = 1;
        }

        private void radioButtonRegValHex_CheckedChanged(object sender, EventArgs e)
        {
            RegValeShowType = 2;
        }


        /// <summary>
        /// 复位按钮响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetCNC_Click(object sender, EventArgs e)
        {
            //btnResetCNC.BackColor = Color.Red;
        }

//         private delegate void FlushClient();//代理
        private void CncForm_Load(object sender, EventArgs e)
        {
            MainForm.cncform_Ptr = this.Handle;

            asc.controllInitializeSize(this);
            asc1.controllInitializeSize(this.Keyboard1);
            asc2.controllInitializeSize(this.Keyboard2);
            tabControlCNC.TabPages.Remove(tabPageState);  //隐藏机器人
            tabControlCNC.TabPages.Remove(tabPageParam);  //隐藏机器人
       
            LoadCNCLanguage();
            if (tab_index < 0)
            {
                tab_index = 0;
            }

            System.Threading.Thread t = new System.Threading.Thread(this.UpDataUIThreaFucv2);
            t.Start();

            dataGridView_REG.AllowUserToAddRows = false;
            dataGridViewalarmdata.AllowUserToAddRows = false;
            dataGridView_Comp.AllowUserToAddRows = false;
            //MainForm.languagechangeEvent += LanguageChange; 
        }

        void LanguageChange(object sender, string Language)
        {
            LoadCNCLanguage();
        }

        /// <summary>
        /// CNC界面刷新时响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CncForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((CncForm)sender).Visible && comboBoxCNC.Items.Count > 0)
            {
                comboBoxCNC.SelectedIndex = tab_index;
            }
            threaFucRuningF_OK = true;
            Get_Reg_threaFucEvent.Set();

        }

        private void CncForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
            asc1.controlAutoSize(this.Keyboard1);
            asc2.controlAutoSize(this.Keyboard2);
            this.WindowState = (System.Windows.Forms.FormWindowState)(2);
        }
        private void changeDGVCnc()
        {
//             dataGridView_REG.Columns[0].HeaderText = Localization.Forms["CncForm"]["Column_Reg_index"];
//             dataGridView_REG.Columns[1].HeaderText = Localization.Forms["CncForm"]["Column_RegVal_x"];
//             dataGridView_REG.Columns[2].HeaderText = Localization.Forms["CncForm"]["Column_RegVal_y"];
//             dataGridView_REG.Columns[3].HeaderText = Localization.Forms["CncForm"]["Column_RegVal_f"];
//             dataGridView_REG.Columns[4].HeaderText = Localization.Forms["CncForm"]["Column_RegVal_g"];
//             dataGridView_REG.Columns[5].HeaderText = Localization.Forms["CncForm"]["Column_RegVal_r"];
//             dataGridView_REG.Columns[6].HeaderText = Localization.Forms["CncForm"]["Column_RegVal_b"];
//             dataGridViewalarmdata.Columns[0].HeaderText = Localization.Forms["CncForm"]["CNCAlarmNumi"];
//             dataGridViewalarmdata.Columns[1].HeaderText = Localization.Forms["CncForm"]["CNCAlarmNum"];
//             dataGridViewalarmdata.Columns[2].HeaderText = Localization.Forms["CncForm"]["CNCAlarmContent"];
//             dataGridViewalarmdata.Columns[3].HeaderText = Localization.Forms["CncForm"]["CNCAlarmReason"];
            
         //   dataGridViewParam.
      //      listView11.Columns[0].
        }
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MainForm.LanguageChangeMsg:
//                     Localization.RefreshLanguage(this);
//                     changeDGVCnc();
                    break;
                case MainForm.ClosingMsg:
                    threaFucRuningF = false;
                    threaFucRuningF_OK = true;
                    Get_Reg_threaFucEvent.Set();
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        //hxb  2017.4.1
        private void tabControlCNC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlCNC.SelectedTab.Name == "tabPageParam")
            {
                bool connect = MainForm.PingTestCNC(cncv2.IP, 300);
                if(connect)
                    ctrlParaman.ReinitParameterContent();
            }
            else if (tabControlCNC.SelectedTab.Name == "tabPageCutter")
            {
                InitdataGridView_Comp();
            }
            else if(tabControlCNC.SelectedTab.Name == "tabPageAlarm")
            {
                Updata_Alarm_Value(true);
            }
        }


      


	}
}
