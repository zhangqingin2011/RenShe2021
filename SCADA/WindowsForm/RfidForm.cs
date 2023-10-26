﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Collector;
using HFAPItest;
using HNC_MacDataService;
using HNCAPI;
using Sygole.HFReader;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

/*
 *2017.9.9   更改说明
 *1、初始化增加毛坯A料、B料分别初始化功能；
 *
*/
namespace SCADA
{   
    public partial class RfidForm : Form
    {
        /// <summary>
        [DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public static int  m_src = 0;//工件来源

        public static bool m_IsWrong = true;//只发一次不合格
        private delegate void beginInvokeDelegate();

        private bool m_ifaddReadlabel = false; //防止列表刷屏
        private bool m_ifaddWritelabel = false;
        private bool m_ifoneInitlabel = false;
        private bool m_iftwoInitlabel = false;
        private bool m_ifthreeInitlabel = false;
        private bool m_iffourInitlabel = false;
        private bool m_iffiveInitlabel = false;
        private bool m_ifspecialInitlabel = false;
        private Thread ClearThread;

        private string prelanguage; //记录切换语言之前的语言,用于防止列表闪屏
        int maxWidth, maxHeight;
        #region 所有自定义类实便化

        byte ReaderID = 0x00;//读写器ID
        bool Rfidflag = false;//#1RFID连接标志
        Opcode_enum opcode = Opcode_enum.ADDRESS_MODE;
        //bool Rfidflag1 = false;//#2RFID连接标志
        //bool Rfidflag2 = false;//#3RFID连接标志
        string IPAdress = "";//#1RFID IP 地址
        //string IPAdress1 = "";//#2RFID IP 地址
        //string IPAdress2 = "";//#3RFID IP 地址

        byte[] UID = new byte[9];
        string curUID = null; 
        const int maxRowsToDispRFID = 80;  //显示RFID记录上限
        byte blockStart = 0, blockCnt = 2 , blockSize = 4; //多块读写  blocksize只能4和8
        string initRFID = "3362020000000000";  //初始化标签  初始化为未加工料

        string CompleteRFID = "3363020101000000";  //加工完成料

        private static string data = null, rfidUID = null;
        //string rfidDataXMLname = ".RFIDdata.XML";
        string[] STR_PARM_IN = { "序号", "RFID标签读写状态" };
        string[] columnTitle = { "索引", "时间戳", "标签识别码", "标签数据", "读/写","物料类型", "工序要求", "工序1", "工序2", "质检信息", " " };
        enum TYPEGONGJIAN
        {
            A,
            B,
            C
        };

        enum GONGJIAN
        {
            JU,
            MA,
            XIANG,
            SHI,
            PAO,
            ZU,
            JIANG
        };

        #endregion
        Hashtable m_Hashtable;

        public RfidForm()
        {
            int i = 0;
            InitializeComponent();
            dataGridViewRFID.AllowUserToAddRows = true;
            dataGridViewRFID.ColumnCount = columnTitle.Length;


            maxWidth = Screen.PrimaryScreen.WorkingArea.Width;
            maxHeight = Screen.PrimaryScreen.WorkingArea.Height;

            int RFIDiiSum = int.Parse(MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]));

            tabControl1.TabPages.Remove(tabPageSet);  //隐藏参数设置界面

        }


        private void RfidForm_Load(object sender, EventArgs e)
        {
            ChangeLanguage.LoadLanguage(this);
            int offset = 200;
            //布局设计

            this.Height = (maxHeight - 28) * 9 / 10 * 8 / 9 - 35;

            this.tabControl1.Width = this.Width / 2 - 2 - offset;
            this.dataGridViewRFID.Width = this.Width / 2 + offset;
            this.dataGridViewRFID.Left = this.tabControl1.Left + this.Width / 2 + 2 - offset;
            this.listView2.Width = this.Width;

            this.tabControl1.Height = this.Height / 2;
            this.dataGridViewRFID.Height = this.tabControl1.Height;

            this.listView2.Height = this.tabControl1.Height;
            this.listView2.Top = this.tabControl1.Top + this.tabControl1.Height + 5;

            //this.panel1.Width = this.Width;
            //this.panel1.Height = (((MainForm)this.MdiParent).rightForm.Height - 6) / 2;

            
            prelanguage = ChangeLanguage.GetDefaultLanguage();
            LoadRFIDLanguage();

            //LoadRFIDLanguage(ChangeLanguage.GetDefaultLanguage());
           // btnConnect_Click(sender, e); 

            //总控PLC只有一个  hxb  2017.4.27
            string ip = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
            MacDataService.GetInstance().GetMachineDbNo(ip, ref MainForm.plc_dbNo);

            ///添加RFID XML文件初始化 2017.6.14 wwj,再次连接
            int RFIDiiSum = int.Parse(MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]));
            string[] Attributesstr = new String[m_xmlDociment.Default_Attributes_RFID.Length];
            IPAdress = Attributesstr[13] = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[13]);//读写器IP地址
            Attributesstr[14] = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[14]);//读写器端口
            ConnectRFID(Attributesstr[13], int.Parse(Attributesstr[14]));//连接读写器1   
            //this.BeginInvoke(new beginInvokeDelegate(RefreshConnectInfo));
        }

        public void CommunityBody()
        {
            while (true)
            {
                MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 205, 100, MainForm.plc_dbNo);
                Thread.Sleep(1000);
                MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 205, 0, MainForm.plc_dbNo);
                Thread.Sleep(1000);
                //Console.WriteLine("&&&&&&&&&&");
            }
        }


        //清除发给PLC的信号的函数功能
        private void Communicationverfication() 
        {
            ClearThread = new Thread(new ThreadStart(CommunityBody));
            ClearThread.Start();
        }

        //private void RefreshConnectInfo()
        //{
        //    //总控PLC只有一个  hxb  2017.4.27
        //    string ip = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
        //    MacDataService.GetInstance().GetMachineDbNo(ip, ref MainForm.plc_dbNo);

        //    ///添加RFID XML文件初始化 2017.6.14 wwj,再次连接
        //    int RFIDiiSum = int.Parse(MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]));
        //    string[] Attributesstr = new String[m_xmlDociment.Default_Attributes_RFID.Length];
        //    IPAdress = Attributesstr[13] = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[13]);//读写器IP地址
        //    Attributesstr[14] = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[14]);//读写器端口
        //    ConnectRFID(Attributesstr[13], int.Parse(Attributesstr[14]));//连接读写器1   
        //}

        private void RfidForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainForm.SGreader.ConnectStatus == ConnectStatusEnum.CONNECTED)
            {
                try
                {
                    MainForm.SGreader.DisConnect();
                }
                catch (Exception ex)
                {
                      //连接按钮旁的状态标签
                     AddStatus(ex.Message);
                }
            }

            this.Dispose();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (prelanguage != ChangeLanguage.GetDefaultLanguage())
            {
                LoadRFIDLanguage();
                prelanguage = ChangeLanguage.GetDefaultLanguage();
            }
            monitorRFIDReadAndWrite(IPAdress);
        }

        public void LoadRFIDLanguage()
        {
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);

            mComboBoxData.Items.Clear();
            mComboBoxData.Items.AddRange(new object[] {
                ChangeLanguage.GetString(m_Hashtable, "RFIDData0005"),
                ChangeLanguage.GetString(m_Hashtable, "RFIDData0006"),
                /*ChangeLanguage.GetString(m_Hashtable, "RFIDData0003")*/});
            columnTitle[0] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0001");
            columnTitle[1] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0002");
            columnTitle[2] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0003");
            columnTitle[3] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0004");
            columnTitle[4] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0005");
            columnTitle[5] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0011");//2017.9.9
            columnTitle[6] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0006");
            columnTitle[7] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0007");
            columnTitle[8] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0008");
            columnTitle[9] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0009");
            columnTitle[10] = ChangeLanguage.GetString(m_Hashtable, "ColumnTitle0010");

            for (int i = 0; i < columnTitle.Length; i++)
            {
                dataGridViewRFID.Columns[i].HeaderText = columnTitle[i];
            }
            STR_PARM_IN[0] = ChangeLanguage.GetString(m_Hashtable, "STR_RARM_IN0");
            STR_PARM_IN[1] = ChangeLanguage.GetString(m_Hashtable, "STR_RARM_IN1");
            listView2.Columns.Clear();
            int[] columnHeadWidth1 = { this.Width / 8, this.Width * 7 / 8 };
            for (int col = 0; col < STR_PARM_IN.Length; col++)
            {
                listView2.Columns.Add(STR_PARM_IN[col], columnHeadWidth1[col]);
            }
            mComboBoxData.SelectedIndex = 0;
            Communicationverfication();
        }
/********************************************************************
	created:	2017/04/26
	created:	24:3:2017   17:20
	filename: 	D:\LatheLine\Latheline-武汉城市学院-20170323\LatheLine\LineDetect.cs
	file path:	从XML文件直接读取RFID的IP地址和端口连接
	file base:	LineDetect
	file ext:	cs
	author:		zhangxinglan
	
	purpose:	连接RFID
*********************************************************************/
        private void ConnectRFID(string IPAdress,int Port)
        {
             Rfidflag = MainForm.SGreader.Connect(IPAdress, Port);
            if (Rfidflag)
            {
                //tabControl1.Enabled = true;
                //btnConnect.Text = "断开";
                //textBoxIPRfid.Enabled = false;
                //textBoxPort.Enabled = false;
                //textBoxIPRfid2.Enabled = false;
                //textBoxPort2.Enabled = false;
                //textBoxIPRfid3.Enabled = false;
                //textBoxPort3.Enabled = false;
                       
                //IPAdress0 = GetIPAdress(0);//读写器1 IP地址
                //IPAdress1 = GetIPAdress(1);//读写器2 IP地址
                //IPAdress2 = GetIPAdress(2);//读写器3 IP地址    
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0001") + IPAdress + "   Port=" + Port);
            }
            else
            {
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0002"));
            }                 
        }

 

        /// <summary>
        /// 写多块
        /// </summary>
        /// <param name="flag">读写器系列号 0:#1RFID  1：#2RFID  2：#3RFID</param>
        private void Writeblock00(int flag, bool isOK= true)
        {
            Opcode_enum Opcode = new Opcode_enum();
            byte BlockSize = 4;//块大小  
            string str = null, str1 = null, str2 = null;

            string MenStr = ReadMem(ref str, m_ifaddWritelabel);
            //AddStatus(MenStr);
            str1 = MenStr.Remove(3, 7);  //wwj 2017.6.20
            if (isOK)
                str2 = str1.Insert(3, "2020000"); //未加工
            else
                str2 = str1.Insert(3, "3020101"); //加工完成

            byte[] BlockDatas0 = tool.HexStringToByteArray(str2);//00 替换成 01

            if (Status_enum.SUCCESS == MainForm.SGreader.WriteMBlock(ReaderID, Opcode, UID, blockStart, blockCnt, BlockSize, BlockDatas0))
            {
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0003"));
            }
            else
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0004"));

            if (!m_ifaddWritelabel)
            {
                if (isOK)
                    dispRfidDataToGrid(curUID, str2, "Init");
                else
                dispRfidDataToGrid(curUID, str2, "Write");
                m_ifaddWritelabel = true; 
            }
        }

        private void monitorRFIDReadAndWrite(string Adress)
        {
            if (MainForm.plc_dbNo >= 0 && MacDataService.GetInstance().IsNCConnectToDatabase(MainForm.plc_dbNo))  //测量默认连接为第一个机床
            {
                int value = 0;
                int value1 = 0;

                byte len = 0;

                int regIndexRead = 200;  // 
                int regIndexRead1 = 203;

                string tmpUID = null;
                Antenna_enum ant = Antenna_enum.ANT_1;
                Opcode_enum Opcode = new Opcode_enum();
                byte[] datas = new byte[9];
                int pos = 0;
                int length = datas.Length - 1;

                Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_R, regIndexRead, out value, MainForm.plc_dbNo);
                Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_R, regIndexRead1, out value1, MainForm.plc_dbNo);

                //initMblock(0);//初始化


                if (value == 100)
                {
                    Console.WriteLine("RFID请求读R200");

                    bool ret = readUID(ref tmpUID);//读取UID
                    if (ret)
                    {
                        MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 201, 100, MainForm.plc_dbNo);
                    }
                    else
                    {
                        MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 201, 80, MainForm.plc_dbNo);
                    }
                    Console.WriteLine("RFID读完成R201");
                    if (Status_enum.SUCCESS == MainForm.SGreader.ReadMBlock(0, Opcode, UID, blockStart, blockCnt, ref datas, ref len, ant))
                    {
                        //获取标签内存
                        string str = tool.ByteToHexString(datas, pos, length);
                        string MemSum = str.Substring(0, 4);//0~4内存数据 即3361
                        string gongxu1 = str.Substring(6, 2);//6~7 工序1数据  00 为生料 01 为熟料
                        string Number = str.Substring(2, 1); //物料编号

                        if (MemSum == "0000")// //若标签为全为空 进行初始化为 3361020000000000 即3361为空
                        {
                            initMblock(TYPEGONGJIAN.A);//进行初始化
                            MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 202, 100, MainForm.plc_dbNo); //反馈未加工信息
                        }
                        else if (MemSum != "0000")//已经初始化
                        {
                            if (MemSum == "3362")
                            {
                                MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 202, 100, MainForm.plc_dbNo);//反馈未加工信息
                            }
                            else if (MemSum == "3363")
                            {
                                MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 214, 50, MainForm.plc_dbNo); //反馈加工完成信息
                                //Console.WriteLine("RFID反馈读信息R214=50");
                            }

                            //if (!MainForm.bIsDebug)
                            if (!m_ifaddReadlabel)
                            {
                                dispRfidDataToGrid(curUID, str, "Read");
                                m_ifaddReadlabel = true;
                            }

                        }
                        string data = null;
                        textBoxUID.Text = tmpUID;
                        ReadMem(ref data, m_ifaddReadlabel);
                        tBMemData.Text = data;


                    }

                    /*Opcode_enum Opcode1 = new Opcode_enum();
                    byte BlockSize = 4;//块大小   
                    string tmpInit = initA;

                    byte[] BlockDatas = tool.HexStringToByteArray(tmpInit);

                    if (Status_enum.SUCCESS == MainForm.SGreader.WriteMBlock(ReaderID, Opcode1, UID, blockStart, blockCnt, BlockSize, BlockDatas))
                    {
                        AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0005"));
                    }
                    else
                    {
                        AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0006"));
                    }
                    if (!m_ifoneInitlabel)
                    {
                        dispRfidDataToGrid(curUID, tmpInit, "Init");
                        m_ifoneInitlabel = true;
                    }*/

                }
                else
                {
                    m_ifaddReadlabel = false;
                }
 
                if (value1 == 100) //写加工完成
                {
                    Console.WriteLine("RFID请求写加工完成R203");  

                    bool ret = readUID(ref tmpUID);//读取UID
                    if (ret)
                    {
                        MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 204, 100, MainForm.plc_dbNo);
                    }
                    else
                    {
                        MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 204, 80, MainForm.plc_dbNo);
                    }
                    Console.WriteLine("RFID写完成R204");
                    if (Status_enum.SUCCESS == MainForm.SGreader.ReadMBlock(0, Opcode, UID, blockStart, blockCnt, ref datas, ref len, ant))
                    {
                        //获取标签内存
                        string str = tool.ByteToHexString(datas, pos, length);

                        string MemSum = str.Substring(0, 4);//0~4内存数据 即3362

                        if (MemSum == "0000")// //若标签为全为空 进行初始化为 3362020000000000 即3362为空
                        {
                            initMblock(TYPEGONGJIAN.A);//进行初始化
                        }

                        Writeblock00(0, false);//写进去 
                    }
                }
                else if (value1 == 50) //初始化
                {
                    Console.WriteLine("RFID请求初始化R203");

                    bool ret = readUID(ref tmpUID);//读取UID
                    if (ret)
                    {
                        MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 204, 100, MainForm.plc_dbNo);
                    }
                    else
                    {
                        MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, 204, 70, MainForm.plc_dbNo);
                    }
                    Console.WriteLine("RFID写完成R204");
                    if (Status_enum.SUCCESS == MainForm.SGreader.ReadMBlock(0, Opcode, UID, blockStart, blockCnt, ref datas, ref len, ant))
                    {
                        //获取标签内存
                        string str = tool.ByteToHexString(datas, pos, length);

                        string MemSum = str.Substring(0, 4);//0~4内存数据 即3362

                        if (MemSum == "0000")// //若标签为全为空 进行初始化为 3362020000000000 即3362为空
                        {
                            initMblock(TYPEGONGJIAN.A);//进行初始化
                        }

                        Writeblock00(0, true);//写进去 

                    }
                }
                else
                {
                    m_ifaddWritelabel = false;
                }
            }

        }

        //private void UpdateMeasureRes()  //测量合格、不合格
        //{
        //    //int Rvalue = 0;
        //    //int regIndexRead = 200;
        //    if (MainForm.cnclist[0].dbNo >= 0 && MacDataService.GetInstance().IsNCConnectToDatabase(MainForm.cnclist[0].dbNo))  //测量默认连接为第一个机床
        //    {
        //        double value = 0.0;
        //        string key = "MacroVariables:USER";
        //        string macVarStr = "";
        //        int ret = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[0].dbNo, key, MeasureForm.MEASURE_VALUE, ref macVarStr);
        //        if (ret == 0 && macVarStr.Length > 0)
        //        {
        //            //SDataUnion macVar = Newtonsoft.Json.JsonConvert.DeserializeObject<SDataUnion>(macVarStr);
        //            //value = macVar.v.f;   //解析并显示测量结果
        //            int strStart = macVarStr.IndexOf("f\":");
        //            // Console.WriteLine("Start=  " + strStart);
        //            int len = macVarStr.IndexOf(",", strStart + 3) - (strStart + 3);
        //            string strTmp = macVarStr.Substring(strStart + 3, len);
        //            // Console.WriteLine("Value=  " + strTmp);
        //            value = Convert.ToDouble(strTmp);

        //            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
        //            if (OrderForm.checkedInputValue((double)value))
        //            {
        //                MainForm.bIsOK = true;
        //            }
        //            else
        //            {
        //                MainForm.bIsOK = false;
        //            }
        //        }
        //    }
        //}

        private bool readRFID()
        {
            if (readUID(ref rfidUID))
            {
                //textBoxUID.text
                if (readMblock(ref data))
                {
                    if (data.Length < 16) //初始化RFID标签
                    {
                        initMblock(TYPEGONGJIAN.A); //默认为A料
                        data = initRFID;
                    }
                    dispRfidDataToGrid(rfidUID, data, "Read" );
                }
            }

            return true;
        }

        /*private bool writeRFID(byte ReaderID)
        {
            if (readUID(ref rfidUID))
            {
                //HncApi.HNC_RegSetValue((int)HncRegType.REG_TYPE_R, regIndexRead, value, 2);

                writeMblock(true, true, GONGJIAN.MA);
                dispRfidDataToGrid(rfidUID, data, "Write");                
            }
            return true;
        }*/

        /********************************************************************
            created:	2017/05/03
            author:		zhangxinglan 	
            purpose:	初始化RFID标签
         *  格式：16位
         *  0~4：物料类型,默认 3361
         *  5~6:工序数,   默认 02，表示有2个工序
         *  7~8:工序1加工情况，默认00：表示未加工；    01：表示已加工
         *  9~10:工序2加工情况，默认00：表示未加工；   01：表示已加工
         *  11~12:工序3加工情况，默认00：表示未加工；  01：表示已加工
         *  13~14:工序4加工情况，默认00：表示未加工；  01：表示已加工
         *  15~16:工序5加工情况，默认00：表示未加工；  01：表示已加工
        *********************************************************************/
        /// <summary>
        /// 初始化块
        /// </summary>
        /// <param name="flag"></param>
        private bool initMblock( TYPEGONGJIAN type)
        {
            Opcode_enum Opcode = new Opcode_enum();
            byte BlockSize = 4;//块大小   
            string tmpInit = initRFID;
            /*if(type == TYPEGONGJIAN.A)
                tmpInit = initA;
            else if(type == TYPEGONGJIAN.B)
                tmpInit = initB;
            else if (type == TYPEGONGJIAN.C)
                tmpInit = initC;*/
       
            byte[] BlockDatas = tool.HexStringToByteArray(tmpInit);

            if (Status_enum.SUCCESS == MainForm.SGreader.WriteMBlock(ReaderID, Opcode, UID, blockStart, blockCnt, BlockSize, BlockDatas))
            {
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0005"));
                dispRfidDataToGrid(curUID, tmpInit, "Init");
                return true;
            }
            else
            {
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0006"));
                return false;
            }
        }
  

        private bool readMblock(ref string blockData)
        {
            byte [] ReceiveDatas = new byte[64];
            byte dataLen = 0;
            Antenna_enum ant = Antenna_enum.ANT_1;
            if (Status_enum.SUCCESS == MainForm.SGreader.ReadMBlock(ReaderID, opcode, UID, blockStart, blockCnt, ref ReceiveDatas, ref dataLen, ant))
            {
                blockData = ConvertMethod.ByteToHexString(ReceiveDatas, 0, dataLen, "");
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0007"));
                return true;
            }
            else
            {
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0008"));
                return false;
            }
        }

        
        private  void dispRfidDataToGrid(string uid, string data , string rwFlag)
        {
            //int index = dataGridViewRFID.Rows.Add();
            //if (index > maxRowsToDispRFID)
            //{
            //    dataGridViewRFID.Rows.Clear();
            //    index = dataGridViewRFID.Rows.Add();
            //    //return;
            //}
            dataGridViewRFID.Rows.Insert(0,1);
            int index = 0;
            if (dataGridViewRFID.Rows.Count > maxRowsToDispRFID)
            {
                dataGridViewRFID.Rows.Clear();
                index = dataGridViewRFID.Rows.Add();
            }

            dataGridViewRFID.Rows[index].Cells[0].Value = (dataGridViewRFID.Rows.Count-1).ToString();//index.ToString();
            dataGridViewRFID.Rows[index].Cells[1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dataGridViewRFID.Rows[index].Cells[2].Value = uid;
            dataGridViewRFID.Rows[index].Cells[3].Value = data;//数据
            dataGridViewRFID.Rows[index].Cells[4].Value = rwFlag;

            string gongjiantype = data.Substring(2, 1); //物料类型 2017.9.9
            if(gongjiantype == "6")
                dataGridViewRFID.Rows[index].Cells[5].Value = "A";
            else if(gongjiantype == "7")
                dataGridViewRFID.Rows[index].Cells[5].Value = "B";
            else if(gongjiantype == "8")
                dataGridViewRFID.Rows[index].Cells[5].Value = "C";

            string gongxuCnt = data.Substring(4, 2);
            dataGridViewRFID.Rows[index].Cells[6].Value = gongxuCnt;//02 2个工序

            string gongxu1 = data.Substring(6, 2);
            if(gongxu1 == "00")
                dataGridViewRFID.Rows[index].Cells[7].Value = ChangeLanguage.GetString(m_Hashtable, "RFIDData0001");
            else
                dataGridViewRFID.Rows[index].Cells[7].Value = ChangeLanguage.GetString(m_Hashtable, "RFIDData0004");

            string gongxu2 = data.Substring(8, 2);
            if (gongxu2 == "00")
                dataGridViewRFID.Rows[index].Cells[8].Value = ChangeLanguage.GetString(m_Hashtable, "RFIDData0001");
            else
                dataGridViewRFID.Rows[index].Cells[8].Value = ChangeLanguage.GetString(m_Hashtable, "RFIDData0004");

            string mes = data.Substring(3, 1); //wwj 质检情况显示 2017.06.20
            if (mes == "2")
                dataGridViewRFID.Rows[index].Cells[9].Value = ChangeLanguage.GetString(m_Hashtable, "RFIDData0005");
            else if(mes == "3")
                dataGridViewRFID.Rows[index].Cells[9].Value = ChangeLanguage.GetString(m_Hashtable, "RFIDData0002");


            //dataGridViewRFID.FirstDisplayedScrollingRowIndex = dataGridViewRFID.RowCount - 1;  //wwj 2017.6.23
        }

        private void AddStatus(string statusText)
        {
            ListViewItem subItem = new ListViewItem();

            int cnt = listView2.Items.Count;
            if (cnt > maxRowsToDispRFID)
            {
                listView2.Items.Clear();
                return;
            }

            subItem.Text = cnt.ToString();
            subItem.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss   ") + statusText);
            listView2.Items.Insert(0,subItem);//Add(subItem);
            //listView2;// = listView2.Items.Count - 1;  //wwj 2017.6.23
        }
        
        private void readHisRfidDataToGrid(string fileName)
        {
            DataSet xmlData = new DataSet();

            if (File.Exists(fileName))
            {
                xmlData.ReadXml(fileName, XmlReadMode.Auto);
                dataGridViewRFID.Columns.Clear();
                dataGridViewRFID.DataSource = xmlData.Tables[3];
            }
            else
            {
                MessageBox.Show(ChangeLanguage.GetString(m_Hashtable, "Messagebox0001"));
            }
        }        

        /// <summary>
        /// 读UID
        /// </summary>
        /// <param name="flag">读写器序列 0:#1RFID  1：#2RFID  2：#3RFID</param>
        /// <param name="rfidUID">UID</param>
        /// <returns></returns>
        private bool readUID(ref string rfidUID)
        {
            if (MainForm.SGreader.ConnectStatus != ConnectStatusEnum.CONNECTED)
            {
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0009"));
                return false;
            }

            int pos = 0;
            int length = UID.Length - 1;

            if (MainForm.SGreader.Inventory(ReaderID, ref UID) == Status_enum.SUCCESS)
            {
                curUID = rfidUID = ConvertMethod.ByteToHexString(UID, pos, length);
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0010"));
                return true;
            }
            else
            {
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0011"));
                /*MainForm.SGreader.DisConnect();
                AddStatus("RFID连接已断开！RFID重连");
                IPAdress = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[13]);//读写器IP地址
                string Port = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[14]);//读写器端口
                ConnectRFID(IPAdress, int.Parse(Port));//连接读写器1  */
                return false;
            }
        }
        /// <summary>
        /// 读多块
        /// </summary>
        /// <param name="flag">读写器系列号 0:#1RFID  1：#2RFID  2：#3RFID</param>
        private string ReadMem(ref string MenStr,bool flag=false)
        {
            Antenna_enum ant = Antenna_enum.ANT_1;
            Opcode_enum Opcode = new Opcode_enum();
            byte[] datas = new byte[9];
            byte len = 0;

            int pos = 0;
            int length = datas.Length - 1;
 //           string MenStr = "";

            if (Status_enum.SUCCESS == MainForm.SGreader.ReadMBlock(0, Opcode, UID, 0, blockCnt, ref datas, ref len, ant))
            {
                MenStr = tool.ByteToHexString(datas, pos, length);
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0012"));
                if(!flag)
                    dispRfidDataToGrid(curUID, MenStr, "Read");
            }
            return MenStr;
        }
        /// <summary>
        /// 写多块
        /// </summary>
        /// <param name="flag">读写器系列号 0:#1RFID  1：#2RFID  2：#3RFID</param>
        private void Writeblock(ref string WriteMenStr)
        {
            Opcode_enum Opcode = new Opcode_enum();
            byte BlockSize = 4;//块大小    
            byte[] BlockDatas = tool.HexStringToByteArray(WriteMenStr);

            if (Status_enum.SUCCESS == MainForm.SGreader.WriteMBlock(ReaderID, Opcode, UID, blockStart, blockCnt, BlockSize, BlockDatas))
            {
                AddStatus(ChangeLanguage.GetString(m_Hashtable, "StatusMessage0003"));
                dispRfidDataToGrid(curUID, WriteMenStr, "Write");
            }
        }
     
        private void btnReadMem_Click(object sender, EventArgs e)
        {
            string str = null;
            readUID(ref str);
            textBoxUID.Text = str;
            str = null;
            ReadMem(ref str);//读多块1
            tBMemData.Text = str;          
        }

        private void btnWriteMem_Click(object sender, EventArgs e)
        {
            string str = null;
            readUID(ref str);
            textBoxUID.Text = str;
            switch(mComboBoxData.SelectedIndex)
            {
                case 1:  //已加工
                    str = CompleteRFID;
                    break;
                default:
                case 0:  //未加工
                    str = initRFID;
                    break;

            }
            Writeblock(ref str);
        }

        private void btnWriteMemB_Click(object sender, EventArgs e)
        {

        }


        private void btnInitMem_Click(object sender, EventArgs e)
        {
            readUID(ref curUID);
            if (initMblock(TYPEGONGJIAN.A))
                tBInitState.Text = ChangeLanguage.GetString(m_Hashtable, "btnInitState1");
            else
                tBInitState.Text = ChangeLanguage.GetString(m_Hashtable, "btnInitState2");
        }
        private void btnInitB_Click(object sender, EventArgs e)
        {

        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            byte nStart, nCnt;
            bool flg1 = Byte.TryParse(tBStart.Text, out nStart);
            bool flg2 = Byte.TryParse(tBCount.Text, out nCnt);
            if (flg1 && flg2)
            {
                blockStart = nStart;
                blockCnt = nCnt;

                switch (comboBoxBlockSize.SelectedIndex)
                {
                    case 0: blockSize = 4;
                        break;
                    case 1: blockSize = 8;
                        break;
                }
            }
            else
            {
                MessageBox.Show(ChangeLanguage.GetString(m_Hashtable, "Messagebox0002"));
                blockStart = 0;
                blockCnt = 2;
            }
        }

        private void btnReadUID_Click(object sender, EventArgs e)
        {
            string tmpUID = null;
            readUID(ref tmpUID);
            textBoxUID.Text = tmpUID;
        }

        private void btnWriteMemB_Click_1(object sender, EventArgs e)
        {
            /*string str = null;
            readUID(ref str);
            textBoxUID.Text = str;
            switch (mComboBoxData.SelectedIndex)
            {
                case 1:  //合格
                    str = initOKRFIDB;
                    break;
                case 2: //不合格
                    str = initNOKRFIDB;
                    break;
                default:
                case 0:  //生料
                    str = initRFIDB;
                    break;

            }
            Writeblock(ref str);*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*string str = null;
            readUID(ref str);
            textBoxUID.Text = str;
            switch (mComboBoxData.SelectedIndex)
            {
                case 1:  //合格
                    str = initOKRFIDC;
                    break;
                case 2: //不合格
                    str = initNOKRFIDC;
                    break;
                default:
                case 0:  //生料
                    str = initRFIDC;
                    break;

            }
            Writeblock(ref str);*/
        }

        private void btnInitB_Click_1(object sender, EventArgs e)
        {
            readUID(ref curUID);
            if (initMblock(TYPEGONGJIAN.B))
                tBInitState.Text = ChangeLanguage.GetString(m_Hashtable, "btnInitState1");
            else
                tBInitState.Text = ChangeLanguage.GetString(m_Hashtable, "btnInitState2");
        }

        private void btnInitC_Click(object sender, EventArgs e)
        {
            readUID(ref curUID);
            if (initMblock(TYPEGONGJIAN.C))
                tBInitState.Text = ChangeLanguage.GetString(m_Hashtable, "btnInitState1");
            else
                tBInitState.Text = ChangeLanguage.GetString(m_Hashtable, "btnInitState2");
        }



    }
}

