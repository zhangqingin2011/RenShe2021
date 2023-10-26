﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using LineDevice;
using System.Xml;
using System.Runtime;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Windows;
using PLC;
using Collector;
using ScadaHncData;
using HNCAPI;
using System.Management;//需要在项目中添加System.Management引用
using System.Management.Instrumentation;
using HNC_MacDataService;
using Sygole.HFReader;

namespace SCADA
{
    public partial class MainForm : Form
    {
        #region    窗体初始化
        public static HFReader SGreader0 = new HFReader();//RFID 1
        //public static HFReader SGreader1 = new HFReader();//RFID 2
        //public static HFReader SGreader2 = new HFReader();//RFID 3

        //用来记录from是否打开过
        public static string XMLSavePath = "..\\data\\Set\\SCADASet.xml";//设置文件的路径
        public static m_xmlDociment m_xml = null;//设置数据文件

        public static List<CNC> cnclist = null;//CNC设备列表
        public static List<ROBOT> robotlist = null;//机器人设备列表
        public static UInt32 logHandle = 0;//日记句柄，老日记
        public static CMM cmm = null;

        public static ShareData shareData = null;//共享内存对象

        public PLCDataShare plc = null;

        public const int USERMESSAGE = 0x0400;
        public const int LanguageChangeMsg = USERMESSAGE + 10;
        public const int ClosingMsg = USERMESSAGE + 1000;//窗口关闭的自定义消息

        public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
        public static IntPtr cncform_Ptr = IntPtr.Zero;///
        public static IntPtr plcform_Ptr = IntPtr.Zero;///
//         public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
//         public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
//         public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
//         public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
   

        private CollectShare ncCollector = null;//CNC数据收集器
        private readonly String LOG_PATH = "..\\data\\ScadaLog";//系统日志路径
        private readonly UInt16 maxLogSize = 2 * 1024;
        private readonly Byte maxFileCnt = 10;
        public static bool InitializeComponentFinish = false;//所有设备初始化状态

        public static String LinckedText = "通信正常";//界面上连接上的状态颜色
        public static String UnLinckedText = "离线";//界面上没连接上的状态颜色

        //public static RFIDDATAT m_ShowRfidDataTable = null;//RFID信息管理

        private String LogFilePath = "..\\data\\Log\\SystemLog.xml";//log日志路径
        public static LogData m_Log = null;//系统日志

        public static String RFIDDataFilePath = "..\\data\\RFID\\RFIDDataDataTableFile";//RFID数据保存
        public readonly String CNCTaskDataFilePath = "..\\data\\CNCTask\\CNCTask.xml";//派工单数据保存

        public static System.EventHandler<EQUIP_STATE> SendEQUIP_STATEHandler = null;

        public static EquipmentCheck m_CheckHander = null;

        public static LogData m_CCDdata = new LogData();
        public static String CCDdataFilePath = "..\\data\\CCD\\CCDDataDataTableFile";

        public static bool flash_flag = true;

//         public static Dmis m_dmis;
//        Random rand = new Random();
        INIAPI iniapi = new INIAPI();
		private static Form[] m_Formarr = null;
		private static DateTime SystemStartime;
        #region 注册
        /// <summary>
        /// 取得设备硬盘的卷标号
        /// </summary>
        /// <returns></returns>
        private string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }
        /// <summary>
        /// 获得CPU的序列号
        /// </summary>
        /// <returns></returns>
        private string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <returns></returns>
        private string getMNum()
        {
            string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
            string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码
            return strMNum;
        }
        private int[] intCode = new int[127];//存储密钥
        private int[] intNumber = new int[25];//存机器码的Ascii值
        private char[] Charcode = new char[25];//存储机器码字
        private void setIntCode()//给数组赋值小于10的数
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <returns></returns>
        private string getRNum()
        {
            setIntCode();//初始化127位数组
            string MNum = this.getMNum();//获取注册码
            for (int i = 1; i < Charcode.Length; i++)//把机器码存入数组中
            {
                Charcode[i] = Convert.ToChar(MNum.Substring(i - 1, 1));
            }
            for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。
            {
                intNumber[j] = intCode[Convert.ToInt32(Charcode[j])] + Convert.ToInt32(Charcode[j]);
            }
            string strAsciiName = "";//用于存储注册码
            for (int j = 1; j < intNumber.Length; j++)
            {
                if (intNumber[j] >= 48 && intNumber[j] <= 57)//判断字符ASCII值是否0－9之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 65 && intNumber[j] <= 90)//判断字符ASCII值是否A－Z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 97 && intNumber[j] <= 122)//判断字符ASCII值是否a－z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else//判断字符ASCII值不在以上范围内
                {
                    if (intNumber[j] > 122)//判断字符ASCII值是否大于z
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
                    }
                    else
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
                    }
                }
            }
            return strAsciiName;//返回注册码
        }
        public static String MakeSingSn(String SNCreateStr)
        {
            String str = "";
            Int32 SNCreateStrii = SNCreateStr.GetHashCode();
            for (int ii = 0; ii < SNCreateStr.Length;ii++ )//求出字符串
            {
                SNCreateStrii ^= SNCreateStr.Substring(ii, 1).GetHashCode();
                str += SNCreateStrii.ToString();
                int index = ((System.Math.Abs(SNCreateStrii)) / (Int32.MaxValue / 2))
                    * (str.Length / 2);
                str = str.Substring(0, index) + SNCreateStr.Substring(ii, 1) + str.Substring(index, str.Length - index - 1);
            }
            return str;
        }
        private bool zhuce()
        {
            String SNpath = "..\\data\\Set\\SN.txt";
            String SNCreateStr = getRNum();
            String SNCreateStr11 = MakeSingSn(SNCreateStr);
		    
            
            bool isreg = false;
            if (System.IO.File.Exists(SNpath))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(SNpath, Encoding.Default);
                String SN = "";
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    SN += line;
                }
                sr.Close();
                if (SN == SNCreateStr11)
                {
                    isreg = true;
                }
                else
                {
                    SCADA.WindowsForm.Reg m_reg = new SCADA.WindowsForm.Reg();
                    m_reg.RegMunber = SNCreateStr;
                    m_reg.ShowDialog(); 
                    if (m_reg.RegSet == SNCreateStr11)
                    {
                        isreg = true;
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(SNpath);
                        sw.Write(SNCreateStr11);
                        sw.Close();
                    }
                }
            }
            else
            {
                System.IO.File.Open(SNpath, System.IO.FileMode.Create);
                SCADA.WindowsForm.Reg m_reg = new SCADA.WindowsForm.Reg();
                m_reg.RegMunber = SNCreateStr;
                m_reg.ShowDialog();
                if (m_reg.RegSet == SNCreateStr11)
                {
                    isreg = true;
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(SNpath);
                    sw.Write(SNCreateStr11);
                    sw.Close();
                }
            }
            return isreg;
        }
        #endregion

        public MainForm()
        {
            SystemStartime = DateTime.Now;
            if (mainform_Ptr != IntPtr.Zero)
            {
                MessageBox.Show("系统异常重启！");
                Environment.Exit(0);
            }
            m_Log = new LogData();//系统日志
            m_Log.m_load(LogFilePath, true);
            m_xml = new m_xmlDociment();//设置数据文件
            cnclist = new List<CNC>();//CNC设备列表
            robotlist = new List<ROBOT>();//机器人设备列表
            cmm = new CMM();

            //lineCCD = new LineCCD();

            LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();

/*            if (!zhuce())
            {
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_security;
                SendParm.LevelIndex = (int)LogData.Node2Level.AUDIT;
                SendParm.EventID = ((int)LogData.Node2Level.AUDIT).ToString();
                SendParm.Keywords = "用户注册";
                SendParm.EventData = "用户注册失败！";
                System.Threading.Thread.Sleep(1000);
                Environment.Exit(0);
            }
            else
            {
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_security;
                SendParm.LevelIndex = (int)LogData.Node2Level.AUDIT;
                SendParm.EventID = ((int)LogData.Node2Level.AUDIT).ToString();
                SendParm.Keywords = "用户注册";
                SendParm.EventData = "用户注册成功！";
            }
 */
            SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, 
                new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");

            SendEQUIP_STATEHandler += new System.EventHandler<EQUIP_STATE>(this.SendEQUIP_STATEHandlerFuc);//设备状态发生变化
            InitializeComponent();
            Boolean ret = LogApi.LogInitialize(ref logHandle, LOG_PATH, maxLogSize, maxFileCnt);
            if (!ret)
            {
                MessageBox.Show("日志初始化失败！");
            }

            mainform_Ptr = this.Handle;
            if (m_xml.m_load(XMLSavePath) == 0)
            {
                System.Windows.Forms.MessageBox.Show("配置文件不存在，已经生成默认的配置文件\r\n" );
            }
            AddForm();
            TaskDataForm.ctrlTaskData = new UserControlTaskData();

            ShareData._gShareData = shareData = new ShareData(mainform_Ptr);
            ncCollector = new CollectShare(shareData, cnclist);
            plc = new PLCDataShare(shareData);
            TaskDataForm.ctrlTaskData.TaskDataObj = shareData;

            hisoutput = int.Parse(iniapi.loaddata());

            //------------------------------------Modified by zb 20151111--------------------------------start
            TcpServerChannel tc = new TcpServerChannel("Scada", 9987); 
            //------------------------------------Modified by zb 20151111--------------------------------end

            ChannelServices.RegisterChannel(tc, false);
            LifetimeServices.LeaseTime = TimeSpan.Zero;
            ObjRef objrefWellKnown = RemotingServices.Marshal(shareData, "ZKService");

            try
            {
                m_CheckHander = new EquipmentCheck();                
                InitEquiment();
                
                //m_ShowRfidDataTable = new RFIDDATAT();
                InitializeComponentFinish = true;
                if (InitializeComponentFinish)
                {
                    ncCollector.StartCollection();   //开启采集器
                }
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
                SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
                SendParm.Keywords = "设备初始化成功";
                SendParm.EventData = "所有设备初始化成功！";
                SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
            }
            catch (System.Exception ex)
            {
                InitializeComponentFinish = false;
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                SendParm.LevelIndex = (int)LogData.Node2Level.FAULT;
                SendParm.EventID = ((int)LogData.Node2Level.FAULT).ToString();
                SendParm.Keywords = "设备初始化失败";
                SendParm.EventData = ex.ToString();
                SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
            }

//             LogApi.WriteLogInfo(MainForm.logHandle, (Byte)ENUM_LOG_LEVEL.LEVEL_WARN
//                 , "******************************Scada init succes!!******************************");
        }
        public void SendEQUIP_STATEHandlerFuc(object obj, EQUIP_STATE m_EQUIP_STATE)
        {
            if (m_EQUIP_STATE.STATE_VALUE != null)
            {
                switch ((int)m_EQUIP_STATE.STATE_VALUE)// FLOAT(126),-1：离线状态，0：一般状态（空闲状态），1：循环启动（运行状态），2：进给保持（空闲状态），3：急停状态（报警状态）
                {
                    case -1:
                        m_EQUIP_STATE.EQUIP_STATE_Column = "离线";
                        break;
                    case 0:
                        m_EQUIP_STATE.EQUIP_STATE_Column = "空闲";
                        break;
                    case 1:
                        m_EQUIP_STATE.EQUIP_STATE_Column = "运行";
                        break;
                    case 2:
                        m_EQUIP_STATE.EQUIP_STATE_Column = "保持";
                        break;
                    case 3:
                        m_EQUIP_STATE.EQUIP_STATE_Column = "急停";
                        break;
                    default:
                        break;
                }
            }
            m_EQUIP_STATE.SWTICH_TIME = DateTime.Now;// DATE,时间戳
            lock (shareData.m_lockequipstate)
            {
                shareData.m_equipstate.Add(m_EQUIP_STATE);
                if (shareData.m_equipstate.Count > shareData.ncDatas_CountMax)
                {
                    shareData.m_equipstate.RemoveAt(0);
                }
            }
    }

        private void registerDialog() 
        {
//             MessageBoxManager.Unregister();
//             MessageBoxManager.OK = Localization.Dialog["201"];
//             MessageBoxManager.Cancel = Localization.Dialog["202"];
//             MessageBoxManager.Yes = Localization.Dialog["203"];
//             MessageBoxManager.No = Localization.Dialog["204"];
//             MessageBoxManager.Retry = Localization.Dialog["205"];
//             MessageBoxManager.Abort = Localization.Dialog["206"];
//             MessageBoxManager.Ignore = Localization.Dialog["207"];
//             MessageBoxManager.Register();
        }

        /// <summary>
        /// 根据XML文件初始化设备对象
        /// </summary>
        private void InitEquiment()
        {
//            m_dmis = new Dmis("http://192.168.2.10:8080/dmis/", "http://192.168.2.90:8886/dmis?command=refresh&machineName=CMM1");
//             m_dmis = new Dmis("http://localhost:8080/dmis/", "http://localhost:8081/dmis/");


            LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
            string get_str = m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[0]);//SUM
            for (int ii = 0; ii < int.Parse(get_str); ii++)
            {
                CNC m_cnc = new CNC();
                m_cnc.InitCNCParam(m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.serial]),
                     m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.workshop]),
                     m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.productionline]),
                     m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]),
                     m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.port]),
                     m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.SN]),
                     m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.EQUIP_CODE]),
                     m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.remark]),
                     CNCTaskDataFilePath);
                //net_to_redis
                if (MacDataService.GetInstance().GetMachineDbNo(m_cnc.ip, ref m_cnc.dbNo) == 0)
                    Console.WriteLine(" m_cnc.dbNo = " + m_cnc.dbNo);

                cnclist.Add(m_cnc);
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
                SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
                SendParm.Keywords = "CNC初始化";
                SendParm.EventData = m_cnc.JiTaiHao + "初始化成功；IP = " + m_cnc.ip + "  端口 = " + m_cnc.port;
                SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
                if (SCADA.MainForm.SendEQUIP_STATEHandler != null)//添加CNC设备状态数据
                {
                    EQUIP_STATE m_EQUIP_STATE = new EQUIP_STATE();
                    m_EQUIP_STATE.EQUIP_TYPE = 0;
                    m_EQUIP_STATE.EQUIP_CODE = m_cnc.BujianID;// VARCHAR2(50),设备ID
                    m_EQUIP_STATE.EQUIP_CODE_CNC = ""; // VARCHAR2(50),cnc:SN号
                    m_EQUIP_STATE.STATE_VALUE = -1; // FLOAT(126),-1：离线状态，0：一般状态（空闲状态），1：循环启动（运行状态），2：进给保持（空闲状态），3：急停状态（报警状态）
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
                }
            }
                       

            /*if (cnclist.Count > 0)
            {
                int rec = HncApi.HNC_NetInit(m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]), cnclist[0].port);
                if (rec != 0)
                {
                    SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                    SendParm.LevelIndex = (int)LogData.Node2Level.WARNING;
                    SendParm.EventID = ((int)LogData.Node2Level.WARNING).ToString();
                    SendParm.Keywords = "CNC初始化";
                    SendParm.EventData = "本地网络环境初始化失败；IP = " + m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]) +
                                       "  端口 = " + cnclist[0].port;
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
                    throw new Exception(SendParm.EventData);
                }
                else
                {
                    SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                    SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
                    SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
                    SendParm.Keywords = "CNC初始化";
                    SendParm.EventData = "本地网络环境初始化成功；IP = " + m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]) +
                                       "  端口 = " + cnclist[0].port;
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
                }
            }*/


            get_str = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, -1, m_xmlDociment.Default_Attributes_str1[0]);
            for (int ii = 0; ii < int.Parse(get_str); ii++)
            {
                PLC_MITSUBISHI_HNC8 m_slplc = new PLC_MITSUBISHI_HNC8();
                m_slplc.serial = Int32.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Path_str[8]));
                m_slplc.ID = Int32.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.id]));
                m_slplc.ip = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
                m_slplc.port = UInt16.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.port]));
                m_slplc.remark = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.remark]);
                m_slplc.system = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.system]);
                m_slplc.type = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.type]);
                m_slplc.productionline = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.productionline]);
                m_slplc.workshop = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.workshop]);
                m_slplc.EQUIP_CODE = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.EQUIP_CODE]);
                m_slplc.SN = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.SN]);

                if (SCADA.MainForm.SendEQUIP_STATEHandler != null)//添加PLC设备状态数据
                {
                    EQUIP_STATE m_EQUIP_STATE = new EQUIP_STATE();
                    m_EQUIP_STATE.EQUIP_TYPE = 2;
                    m_EQUIP_STATE.EQUIP_CODE = m_slplc.EQUIP_CODE;// VARCHAR2(50),设备ID
                    m_EQUIP_STATE.EQUIP_CODE_CNC = m_slplc.SN; // VARCHAR2(50),cnc:SN号
                    m_EQUIP_STATE.STATE_VALUE = -1; // FLOAT(126),-1：离线状态，0：一般状态（空闲状态），1：循环启动（运行状态），2：进给保持（空闲状态），3：急停状态（报警状态）
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
                }

                String[] Device;
                if (m_slplc.system == m_xmlDociment.PLC_System[0])
                {
                    Device = new String[m_xmlDociment.Default_MITSUBISHI_Device1.Length + m_xmlDociment.Default_MITSUBISHI_Device2.Length];
                    for (int qq = 0; qq < m_xmlDociment.Default_MITSUBISHI_Device1.Length; qq++)
                    {
                        Device[qq] = m_xmlDociment.Default_MITSUBISHI_Device1[qq];
                    }
                    for (int qq = 0; qq < m_xmlDociment.Default_MITSUBISHI_Device2.Length; qq++)
                    {
                        Device[m_xmlDociment.Default_MITSUBISHI_Device1.Length + qq] = m_xmlDociment.Default_MITSUBISHI_Device2[qq];
                    }
                }
                else if (m_slplc.system == m_xmlDociment.PLC_System[1])
                {
                    Device = new String[m_xmlDociment.Default_HNC8_Device1.Length + m_xmlDociment.Default_HNC8_Device2.Length];
                    for (int qq = 0; qq < m_xmlDociment.Default_HNC8_Device1.Length; qq++)
                    {
                        Device[qq] = m_xmlDociment.Default_HNC8_Device1[qq];
                    }
                    for (int qq = 0; qq < m_xmlDociment.Default_HNC8_Device2.Length; qq++)
                    {
                        Device[m_xmlDociment.Default_HNC8_Device1.Length + qq] = m_xmlDociment.Default_HNC8_Device2[qq];
                    }
                }
                else
                {
                    Device = new String[0];
                }
                for (int jj = 0; jj < Device.Length; jj++)
                {
                    string pathstr1 = m_xmlDociment.PathRoot_PLC_Item + ii.ToString();//Root/PLC/Itemii
                    string pathstr2 = pathstr1 + "/" + Device[jj];//"";
                    if (!m_xml.CheckNodeExist(pathstr2))
                    {
                        continue;
                    }

                    Int32 Count = Int32.Parse(m_xml.m_Read(pathstr2, -1, m_xmlDociment.Default_Attributes_str1[0]));
                    String[] AddressGeshi = m_xml.m_Read(pathstr2, -1, m_xmlDociment.Default_Attributes_str2[1]).Split('-');

                    if (m_slplc.system == m_xmlDociment.PLC_System[0])
                    {
                        m_slplc.InitMITSUBISHIPLC_Signal(Device[jj], Int32.Parse(AddressGeshi[0]), m_xml.m_Read(pathstr2, -1, m_xmlDociment.Default_Attributes_str2[4]), Count);
                        for (int kk = 0; kk < Count; kk++)
                        {
                            bool jiankong;
                            if (m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[3]) == m_xmlDociment.Default_Attributesstr2_value[3])
                            {
                                jiankong = false;
                            }
                            else
                            {
                                jiankong = true;
                            }
                            m_slplc.AddSignalNode2List(Device[jj], kk,
                                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[1]),
                                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[2]), 0,
                                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[4]),
                                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[5]),
                                jiankong);
                        }
                    }
                    else if (m_slplc.system == m_xmlDociment.PLC_System[1])
                    {
                        Int32 HncRegType_i = PLC_MITSUBISHI_HNC8.GetHncRegType(Device[jj]);
                        m_slplc.InitHNC8PLC_Signal(Device[jj],Int32.Parse(AddressGeshi[0]), HncRegType_i,Count);
                        for (int kk = 0; kk < Count; kk++)
                        {
                            bool jiankong;
                            if (m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[3]) == m_xmlDociment.Default_Attributesstr2_value[3])
                            {
                                jiankong = false;
                            }
                            else
                            {
                                jiankong = true;
                            }
                            m_slplc.AddSignalNode2List(Device[jj], kk,
                                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[1]),
                                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[2]), 0,
                                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[4]),
                                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[5]),
                                jiankong);
                        }

                    }
                }
                plc.AddPLC(m_slplc);
                                
                /////////添加ROBOT信号点到PLC
                if (m_slplc.serial == 0)//目前只有一个PLC
                {
                    int robotItemsum = int.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, -1, m_xmlDociment.Default_Attributes_str1[0]));
                    for (int jj = 0; jj < robotItemsum; jj++)
                    {

                        String RobotPathStr = m_xmlDociment.PathRoot_ROBOT_Item + jj.ToString();

                        String DeviceX = m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.X];
                        String DeviceY = m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.Y];
                        String RobotPathXStr = RobotPathStr + "/" + DeviceX;
                        String RobotPathYStr = RobotPathStr + "/" + DeviceY;
                        Int32 StatX = 0;
                        Int32 StatY = 0;
                        int robotItemXsum = int.Parse(m_xml.m_Read(RobotPathXStr, -1, m_xmlDociment.Default_Attributes_str2[0]));
                        int robotItemYsum = int.Parse(m_xml.m_Read(RobotPathYStr, -1, m_xmlDociment.Default_Attributes_str2[0]));
                        m_slplc.InitRobot_Signal(DeviceX, robotItemXsum, ref StatX);
                        m_slplc.InitRobot_Signal(DeviceY, robotItemYsum, ref StatY);
                        for (int xx = 0; xx < robotItemXsum; xx++)
                        {
                            bool jiankong;
                            if (m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[3]) == m_xmlDociment.Default_Attributesstr2_value[3])
                            {
                                jiankong = false;
                            }
                            else
                            {
                                jiankong = true;
                            }
                            m_slplc.AddSignalNode2List(DeviceX, StatX + xx,
                                m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[1]),
                                m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[2]), 0,
                                m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[4]),
                                m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[5]),
                                jiankong);
                        }
                        for (int xx = 0; xx < robotItemYsum; xx++)
                        {
                            bool jiankong;
                            if (m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[3]) == m_xmlDociment.Default_Attributesstr2_value[3])
                            {
                                jiankong = false;
                            }
                            else
                            {
                                jiankong = true;
                            }
                            m_slplc.AddSignalNode2List(DeviceY, StatY + xx,
                                m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[1]),
                                m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[2]), 0,
                                m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[4]),
                                m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[5]),
                                jiankong);
                        }
                        ROBOT mrobot = new ROBOT();
                        mrobot.SetRobot(m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Path_str[8]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.id]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.workshop]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.productionline]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.type]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.system]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.port]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.EQUIP_CODE]),
                            m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.SN]),
                            StatX, robotItemXsum, StatY, robotItemYsum);
                        robotlist.Add(mrobot);
                    }
                }

           #region 连接RFID
/********************************************************************
	created:	2017/05/02
	created:	24:3:2017   17:20
	filename: 	20170323
    file path:	从XML文件直接读取RFID的IP地址和端口进行连接
	file ext:	cs
	author:		zhangxinglan	
	purpose:	连接RFID
*********************************************************************/
                ///添加RFID XML文件初始化 2017.4.26 zxl
                int RFIDiiSum = int.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]));
                if (RFIDiiSum != 0)
                {
                    RFIDConnect(0, SendParm);
                    //RFIDConnect(1, SendParm);
                    //RFIDConnect(2, SendParm);
                }
               #endregion


                ///////添加RFID
 /*               if (m_slplc.system == m_xmlDociment.PLC_System[0])//三菱
                {
                    int RFIDiiSum = int.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]));
                    for (int RFIDii = 0; RFIDii < RFIDiiSum; RFIDii++)
                    {
                        if (m_slplc.serial.ToString() == m_xml.m_Read(m_xmlDociment.PathRoot_RFID, RFIDii, m_xmlDociment.Default_Attributes_RFID[(int)m_xmlDociment.Attributes_RFID.PLCserial]))
                        {
                            String[] Attributesstr = new String[m_xmlDociment.Default_Attributes_RFID.Length];
                            Attributesstr[0] = m_xml.m_Read(m_xmlDociment.PathRoot_RFID, RFIDii, m_xmlDociment.Default_Path_str[8]);
                            for(int Attributesstrii = 1;Attributesstrii < m_xmlDociment.Default_Attributes_RFID.Length;Attributesstrii++)
                            {
                                Attributesstr[Attributesstrii] = m_xml.m_Read(m_xmlDociment.PathRoot_RFID, RFIDii, m_xmlDociment.Default_Attributes_RFID[Attributesstrii]);
                            }
//                             if (SCADA.MainForm.SendEQUIP_STATEHandler != null)//添加CNC设备状态数据
//                             {
//                                 EQUIP_STATE m_EQUIP_STATE = new EQUIP_STATE();
//                                 m_EQUIP_STATE.EQUIP_TYPE = 3;
//                                 m_EQUIP_STATE.EQUIP_CODE = m_xml.m_Read(m_xmlDociment.PathRoot_RFID, RFIDii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_RFID.remark]);// VARCHAR2(50),设备ID
//                                 m_EQUIP_STATE.EQUIP_CODE_CNC = ""; // VARCHAR2(50),cnc:SN号
//                                 m_EQUIP_STATE.STATE_VALUE = -1; // FLOAT(126),-1：离线状态，0：一般状态（空闲状态），1：循环启动（运行状态），2：进给保持（空闲状态），3：急停状态（报警状态）
//                                 SCADA.MainForm.SendEQUIP_STATEHandler.BeginInvoke(this, m_EQUIP_STATE, null, null);
//                             }                            
                            //m_slplc.AddRFIDItem2List(ref Attributesstr, ref RfidForm.RFIDReadDataStruct);
                        }
                    }
                }*/
            }
            plc.StarCollectThread();

            //2017.3.30  hxb
            /*cmm.iptext = m_xml.m_Read(m_xmlDociment.PathRoot_CheckEq, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
            cmm.porttext = m_xml.m_Read(m_xmlDociment.PathRoot_CheckEq, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.port]);
            cmm.autolink();*/
        }

        /// <summary>
        /// 连接RFID 2017.5.02 zxl
        /// </summary>
        /// <param name="flag">读写器序号</param>
        public void RFIDConnect(int flag, LogData.EventHandlerSendParm sendParm)
        {
            string[] Attributesstr = new String[m_xmlDociment.Default_Attributes_RFID.Length];
            bool Rfidflag0 = false;
            bool Rfidflag1 = false;
            bool Rfidflag2 = false;

            Attributesstr[13] = m_xml.m_Read(m_xmlDociment.PathRoot_RFID, flag, m_xmlDociment.Default_Attributes_RFID[13]);//读写器IP地址
            Attributesstr[14] = m_xml.m_Read(m_xmlDociment.PathRoot_RFID, flag, m_xmlDociment.Default_Attributes_RFID[14]);//读写器端口
            switch (flag)
            {
                case 0: Rfidflag0 = SGreader0.Connect(Attributesstr[13], int.Parse(Attributesstr[14]));break;
                //case 1: Rfidflag1 = SGreader1.Connect(Attributesstr[13], int.Parse(Attributesstr[14])); break;
               // case 2: Rfidflag2 = SGreader2.Connect(Attributesstr[13], int.Parse(Attributesstr[14])); break;
            }
            if (Rfidflag0 || Rfidflag1 || Rfidflag2)
            {
                sendParm.Keywords = "RFID连接状态";
                sendParm.EventData = "连接成功；IP = " + Attributesstr[13] + " 端口 = " + Attributesstr[14];
                SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, sendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
            }
            else
            {
                if (SCADA.MainForm.SendEQUIP_STATEHandler != null)
                {
                    sendParm.Keywords = "RFID连接状态";
                    sendParm.EventData = "连接失败；IP = " + Attributesstr[13] + " 端口 = " + Attributesstr[14];
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, sendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
                }
            }         
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine("load");
            //初始打开时加载HomeForm
            m_Formarr = new Form[tabPage1.TabCount];
            ChangeLanguage.LoadLanguage(this);//zxl 4.19

            if (tabPage1.TabCount > 0)
            {
                GenerateForm(0, tabPage1);
            }

//             string skin = Localization.ReadDefaultSkin();
            //程序加载时在状态栏显示时钟

//              if (Localization.HasLang)
//              {
//                  Localization.RefreshLanguage(this);
//              }
//              foreach (ToolStripMenuItem topItem in this.menuStrip1.Items)
//              {
//                  topItem.Text = Localization.Menu[topItem.Name];
//                  menu(topItem);
//              }
//              registerDialog();
            //产线字符串
             labProductionLine.Text = m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.productionline]);
       
        }

        public void SetlabTaskFailedNumText(String TaskFailedNumText, String labTaskNumText, String labTaskDateText, String TaskHisNumText)
        {
            labTaskDate.Text = labTaskDateText;
            labTaskNum.Text = labTaskNumText;
            dangqianchanliang.Text = TaskFailedNumText;
            label_HisChanliang.Text = TaskHisNumText;
        }

        public string paigongdate = "";
        public int paigongnum = 0;
        public static int nowoutput = 0;
        public static int hisoutput = 0;
        object paigonglock = new object();
        public void PaiGongRefresh()
        {
            lock (paigonglock)
            {
                if (paigongnum <= nowoutput)
                {
                    int addpaigong = 100;
//                    addpaigong = rand.Next(10, 30);
                    paigongnum += addpaigong;
                    paigongdate = DateTime.Now.ToString();
                }
                SetlabTaskFailedNumText(nowoutput.ToString(), paigongnum.ToString(), paigongdate, hisoutput.ToString());
            }            
        }


        //在选项卡中生成窗体
        public void GenerateForm(int form_index, object sender)
        {
            // 反射生成窗体//只生成一次
            if(m_Formarr[form_index] == null && form_index >= 0 && form_index < tabPage1.TabCount)
            {
                string formClassSTR = ((TabControl)sender).SelectedTab.Tag.ToString();
                m_Formarr[form_index] = (Form)Assembly.GetExecutingAssembly().CreateInstance(formClassSTR);
                if (m_Formarr[form_index] != null)
                {
                    //设置窗体没有边框 加入到选项卡中
                    m_Formarr[form_index].FormBorderStyle = FormBorderStyle.None;
                    m_Formarr[form_index].TopLevel = false;
                    m_Formarr[form_index].Parent = ((TabControl)sender).SelectedTab;
                    m_Formarr[form_index].ControlBox = false;
                    m_Formarr[form_index].Dock = DockStyle.Fill;
                    m_Formarr[form_index].Show();
                }
            }
            
        }


        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateForm(tabPage1.SelectedIndex, sender);
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="m"></param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0002:
                    break;
                case 0x0010:
                    break;
                case 0x0011:
                    break;
                case USERMESSAGE:
                    CncForm.tab_index = (int)m.LParam;
                    tabPage1.SelectedIndex = (int)m.WParam;
                    break;
                case USERMESSAGE + 1:
                    RobotForm.ROBOTIndex = (int)m.LParam;
                    tabPage1.SelectedIndex = 2;
                    break;
                case USERMESSAGE + 2:
                    CMMForm.cmmindex = 1;
                    tabPage1.SelectedIndex = 7;
                    break;
                case USERMESSAGE + 13:  //料架
                    tabPage1.SelectedIndex = 5;
                    break;
                case USERMESSAGE + 14:  //rfid
                    tabPage1.SelectedIndex = 4;
                    break;
                case USERMESSAGE + 15: //测量
                    tabPage1.SelectedIndex = 6;
                    break;

                case USERMESSAGE + 16: //派单
                    tabPage1.SelectedIndex = 3;
                    break;
                case USERMESSAGE + 100://添加派工单 CNC设备控制数据
                    TaskDataForm.AddTaskData2Form((int)m.WParam);
                    break;
                case USERMESSAGE + 200://Web端获取CNC文本文件
                    for (int ii = 0; ii < cnclist.Count;ii++ )
                    {
                        if (cnclist[ii].BujianID == shareData.m_getcncEQUIP_CODE && cnclist[ii].isConnected())
                        {
                            /*if (cnclist[ii].netFileGet(".\\tempfile", shareData.m_getcnctextfilename) == 0)
                            {
                                System.IO.FileStream f = new System.IO.FileStream(".\\tempfile", System.IO.FileMode.Open);
                                System.IO.StreamReader reader = new System.IO.StreamReader(f,Encoding.Default);
                                while(!reader.EndOfStream)
                                {
                                    shareData.m_getcnctextfilestr += reader.ReadLine().ToString();
                                    shareData.m_getcnctextfilestr += Environment.NewLine;
                                }
                                reader.Close();
                                f.Close();
                            }*/
                        }
                    }
                    break;
                case 161:
                    if ((int)m.WParam == 20)
                    {
//                         if (SetForm.LogIn)
                        {
//                             if (MessageBox.Show("是否确定退出！", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.Close();
                            }
                        }
//                         else
//                         {
//                             MessageBox.Show("你的操作权限不够！", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                         }
                    }
                    else
                    {
                        base.DefWndProc(ref m);
                    }
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }
        #endregion

        private static void AddForm()
        {         
//             Localization.AddForm("RobotForm");
//             Localization.AddForm("RfidForm");
//             Localization.AddForm("CncForm");
//             Localization.AddForm("PlcForm");
//             Localization.AddForm("SetForm");
//             Localization.AddForm("MainForm");
//             Localization.AddForm("LogForm");
//             string defaultLanguage=  Localization.ReadDefaultLanguage();
//             if (!Localization.Load(defaultLanguage))
//             {
//                 LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
//                 SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
//                 SendParm.LevelIndex = (int)LogData.Node2Level.严重;
//                 SendParm.EventID = ((int)LogData.Node2Level.严重).ToString();
//                 SendParm.Keywords = "语言初始化";
//                 SendParm.EventData = "错误: 无法加载默认语言配置文件";
//                 SendParm.Provider = "MainForm";
//                 SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(null, SendParm, null, null);
// 
//                 Localization.HasLang = false;
//             }
//             else
//                 Localization.HasLang = true;
        }
        private void 中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //主菜单的切换
            menuLanguage("Chinese");

            //读取“任务”窗口中文
            TaskDataForm.ctrlTaskData.ExChangeLanguage("Chinese");

            //窗口语言的切换
            ChangeLanguage.SetDefaultLanguage("Chinese");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadLanguage(form);
            }     
        }
       
        private void 英文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //主菜单的切换
            menuLanguage("English");

            //读取“任务”窗口英文
            TaskDataForm.ctrlTaskData.ExChangeLanguage("English");

            //窗口语言的切换
            ChangeLanguage.SetDefaultLanguage("English");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadLanguage(form);
            }
        }
        /// <summary>
        /// 更换菜单语言函数
        /// </summary>
        /// <param name="lang">要加载的语言</param>
        private void menuLanguage(String lang)
        {
            if (lang == "Chinese")
            {
                StripMenuItemFile.Text = "文件";
                StripMenuItemEdit.Text = "编辑";
                StripMenuItemCheck.Text = "查看";
                StripMenuItemParmas.Text = "参数";
                StripMenuItemHelp.Text = "帮助";
                StripMenuItemEdit.Text = "编辑";
                changeSkin.Text = "更换皮肤";
                changeLanguage.Text = "更换语言";

                中文ToolStripMenuItem.Text = "中文";
                英文ToolStripMenuItem.Text = "英文";
                toolStripMenuItem2.Text = "深蓝";
                toolStripMenuItem3.Text = "浅蓝";
                toolStripMenuItem4.Text = "绿色";
                toolStripMenuItem5.Text = "橙色";
                toolStripMenuItem6.Text = "深绿";
                toolStripMenuItem7.Text = "海蓝";
                toolStripMenuItem8.Text = "海蓝2";
                toolStripMenuItem9.Text = "浅黄";
                toolStripMenuItem10.Text = "其他";
            }
            else if (lang == "English")
            {
                StripMenuItemFile.Text = "File";
                StripMenuItemEdit.Text = "Edition";
                StripMenuItemCheck.Text = "Check";
                StripMenuItemParmas.Text = "Parmas";
                StripMenuItemHelp.Text = "Help";
                StripMenuItemEdit.Text = "Edit";
                changeSkin.Text = "changeSkin";
                changeLanguage.Text = "changeLanguage";

                中文ToolStripMenuItem.Text = "Chinese";
                英文ToolStripMenuItem.Text = "English";
                toolStripMenuItem2.Text = "DarkBlue";
                toolStripMenuItem3.Text = "WathetBlue";
                toolStripMenuItem4.Text = "Green";
                toolStripMenuItem5.Text = "Orange";
                toolStripMenuItem6.Text = "DarkGreen";
                toolStripMenuItem7.Text = "OceanBlue";
                toolStripMenuItem8.Text = "OceanBlue2";
                toolStripMenuItem9.Text = "Buff";
                toolStripMenuItem10.Text = "Other";
            }
        }
        private void menu(ToolStripMenuItem item)
        {
            //foreach (ToolStripMenuItem subitem in item.DropDownItems)
            //{
            //    if (subitem is ToolStripMenuItem)
            //    {
            //        string text = "";
            //        if (Localization.Menu.TryGetValue(subitem.Name, out text))
            //            subitem.Text = text;
            //        if (subitem.HasDropDownItems)
            //            menu(subitem);
            //    }
            //}
        }
        #region  编辑-->皮肤更换
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //this.skinEngine1.SkinFile = "Calmness.ssk";
            //Localization.WriteDefaultSkin("Calmness");
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "DeepCyan.ssk";
//             Localization.WriteDefaultSkin("DeepCyan");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "DeepGreen.ssk";
//             Localization.WriteDefaultSkin("DeepGreen");
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "DeepOrange.ssk";
//             Localization.WriteDefaultSkin("DeepOrange");
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "Midsummer.ssk";
//             Localization.WriteDefaultSkin("Midsummer");
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "MacOS.ssk";
//             Localization.WriteDefaultSkin("MacOS");
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "Page.ssk";
//             Localization.WriteDefaultSkin("Page");
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "Vista.ssk";
//             Localization.WriteDefaultSkin("Vista");
        }
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "DiamondBlue.ssk";
//             Localization.WriteDefaultSkin("DiamondBlue");
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "Eighteen.ssk";
//             Localization.WriteDefaultSkin("Eighteen");
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "Emerald.ssk";
//             Localization.WriteDefaultSkin("Emerald");
        }
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "Vista2_color1.ssk";
//             Localization.WriteDefaultSkin("Vista2_color1");
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "Wave.ssk";
//             Localization.WriteDefaultSkin("Wave");
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "WaveColor1.ssk";
//             Localization.WriteDefaultSkin("WaveColor1");
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
//             this.skinEngine1.SkinFile = "WaveColor2.ssk";
//             Localization.WriteDefaultSkin("WaveColor2");
        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
//            m_dmis.Exit();
            iniapi.savedata(hisoutput.ToString());

            /*if (m_ShowRfidDataTable != null)
            {
                m_ShowRfidDataTable.AppExit();
            }*/
            if (cncform_Ptr != IntPtr.Zero)
            {
                HomeForm.SendMessage(cncform_Ptr, ClosingMsg, 0, 0);
            }
            if (plcform_Ptr != IntPtr.Zero)
            {
                HomeForm.SendMessage(plcform_Ptr, ClosingMsg, 0, 0);
            }
            foreach (CNC m_cnc in cnclist)
            {
                m_cnc.CNCExit();
            }
            plc.ClosePLC_MITSUBISHI();
            ncCollector.CollectExit();

            LogApi.WriteLogInfo(MainForm.logHandle, (Byte)ENUM_LOG_LEVEL.LEVEL_WARN
                , "******************************Scada exit!!******************************\n");

            cmm.cmmexit();
            LogApi.LogExit(logHandle);
            //HncApi.HNC_NetExit();
            m_Log.ExitApp();
            m_CCDdata.ExitApp();
        }

        /// <summary>
        /// 和云数控的链接状态更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
//         DateTime SystemStarTime = DateTime.DaysInMonth(2015,2);
        public static String SystemRuingTimes;        
        private void timer_LinckCheck_Tick(object sender, EventArgs e)
        {            
            if (shareData != null && shareData.Conneted)
            {
                if (labelLinckText.Text != LinckedText)
                {
                    pictureBox_YunShuKongLinckState.Image = SCADA.Properties.Resources.top_bar_green;
                    labelLinckText.Text = LinckedText;
                }
                shareData.Conneted = false;
            }
            else
            {
                if (labelLinckText.Text != UnLinckedText)
                {
                    pictureBox_YunShuKongLinckState.Image = SCADA.Properties.Resources.top_bar_black;
                    labelLinckText.Text = UnLinckedText;
                }
            }
            if (ssText.Text != DateTime.Now.ToString())
            {
                String[] timearr = (DateTime.Now - SystemStartime).ToString().Split('.');
                if (timearr.Length == 2)//一天内
                {
                    SystemRuingTimes = "累计运行时间：0天 " + timearr[0];
                }
                else if (timearr.Length == 3)//超过一天
                {
                    SystemRuingTimes = "累计运行时间：" + timearr[0] + "天 " + timearr[1];
                }
                ssText.Text = "当前时间：" + DateTime.Now.ToString()
                    + "         " + SystemRuingTimes + "         ";
            }

            ///显示产量
            if (PLCDataShare.m_plclist != null &&
                PLCDataShare.m_plclist.Count > 0 &&
                PLCDataShare.m_plclist[0].system == m_xmlDociment.PLC_System[1] &&
                PLCDataShare.m_plclist[0].m_hncPLCCollector != null)
            {
//                 dangqianchanliang.Text = PLCDataShare.m_plclist[0].m_hncPLCCollector.dangqianchanliang.ToString();
//                 label_HisChanliang.Text = PLCDataShare.m_plclist[0].m_hncPLCCollector.lishichanliang.ToString();
            }
            PaiGongRefresh();
            //刷新设备状态
            UpdataCNCStateNum();

            /*string alarmstatus = "";
            if (PLCDataShare.m_plclist != null && PLCDataShare.m_plclist.Count > 0 && PLCDataShare.m_plclist[0].m_hncPLCCollector != null)
            {
                alarmstatus = PLCDataShare.m_plclist[0].m_hncPLCCollector.show_alarm_status();
            }
            else
            {
                alarmstatus = "PLC未连接！";
            }

            string[] AlarmStatuS = alarmstatus.Split('"');
            if (AlarmStatuS.Length > 1)
            {
                 if (PlcForm.isinalarmform == false)
                 {
                     alarmshow.IsLink = true;
                     alarmshow.BackColor = Color.Red;
                 }
                 else
                 {
                     alarmshow.IsLink = false;
                     alarmshow.BackColor = System.Drawing.Color.FromArgb(255, 240, 240);
                 }
                 alarmshow.Text = AlarmStatuS[1] + "!";
             }
             else
             {
                 alarmshow.IsLink = false;
                 alarmshow.BackColor = System.Drawing.Color.FromArgb(255, 240, 240);
                 alarmshow.Text = alarmstatus;
             }
            */
            
            //当前有报警信息时，高亮显示报警选项标签
            if (cnclist[0].HCNCShareData.currentAlarmList.Count > 0)  //目前只有一台机床  2017.6.9  hxb
            {
                if (flash_flag)
                {
                    alarmshow.BackColor = Color.Red;
                    flash_flag = false;
                }
                else
                {
                    alarmshow.BackColor = Color.Yellow;
                    flash_flag = true;
                }
                alarmshow.Text = cnclist[0].HCNCShareData.currentAlarmList[cnclist[0].HCNCShareData.currentAlarmList.Count - 1].alarmTxt+"!";
            }
            else
            {
                alarmshow.BackColor = Color.White;
                alarmshow.Text = "";
            }
        }

        private void 版本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String str = "程序集版本：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n";
            str += "文件版本：" + Application.ProductVersion.ToString() + "\n";
//             str += "部署版本：" + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            MessageBox.Show(str);
//             MessageBox.Show("版本号：V1.2\n更新日期：2016/4/24");
        }
        private void UpdataCNCStateNum()
        {
            int cncruing = 0;
            int cnckongxian = 0;
            int cncbaojing = 0;
            int cnclixian = 0;

            if(MainForm.cnclist != null && MainForm.cnclist.Count > 0)
            {
                cnclixian = MainForm.cnclist.Count;
                for (int ii = 0; ii < MainForm.cnclist.Count; ii++)
                {
                    if (MainForm.cnclist[ii] != null && MainForm.cnclist[ii].isConnected())
                    {
                        LineDevice.CNC.CNCState CNCStatei = LineDevice.CNC.CNCState.DISCON;
                        MainForm.cnclist[ii].Checkcnc_state(ref CNCStatei);
                        if ((int)LineDevice.CNC.CNCState.IDLE == (int)CNCStatei)
                        {
                            cnckongxian++;
                        }
                        else if ((int)LineDevice.CNC.CNCState.RUNING == (int)CNCStatei)
                        {
                            cncruing++;
                        }
                        else if ((int)LineDevice.CNC.CNCState.ALARM == (int)CNCStatei)
                        {
                            cncbaojing++;
                        }
                        cnclixian--;
                    }
                }
            }
            label_CNCRuningNum.Text = "运行数量：" + cncruing.ToString();
            label_CNCkongxianNum.Text = "空闲数量：" + cnckongxian.ToString();
            label_CNCbaojingNum.Text = "报警数量：" + cncbaojing.ToString();
            label_CNCLiXianNum.Text = "离线数量：" + cnclixian.ToString();
        }

        private void alarmshow_Click(object sender, EventArgs e)
        {
            if (alarmshow.IsLink == true)
            {
                PlcForm.plcindex = 2;
                tabPage1.SelectedIndex = 4;
            }  
        }
        
    }


    /// <summary>
    /// 使ComboBox不响应鼠标管轮滑动事件
    /// </summary>
    public class mComboBox : ComboBox
    {
        public mComboBox()
        {
            InitializeComponent();
//             this.VisibleChanged += new System.EventHandler(this.mComboBox_VisibleChanged);
//             this.SelectedIndexChanged += new System.EventHandler(this.mComboBox_SelectedIndexChanged);
        }
        protected override void WndProc(ref   Message m)
        {
            if (m.Msg == 0x020A)
            {

            }
            else
            {
                base.WndProc(ref   m);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }

}

