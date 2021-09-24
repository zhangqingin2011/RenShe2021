using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.Runtime;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Windows;
using ScadaHncData;
using HNCAPI;
using System.Management;//需要在项目中添加System.Management引用
using System.Management.Instrumentation;
using HNC_MacDataService;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Net.NetworkInformation;
using HNC.MOHRSS.Model;
using HNC.MOHRSS.Common;
using Hsc3.Comm;
using Hsc3.Proxy;
using HNC.API;
using HNCAPI_INTERFACE;

namespace SCADA
{
    public delegate void LanguageChangeHandle(object sender, string Language);

    public partial class MainForm : Form
    {
        #region    窗体初始化
        //用来记录from是否打开过
        public static string XMLSavePath = "..\\data\\Set\\SCADASet.xml";//设置文件的路径
        public static m_xmlDociment m_xml = null;//设置数据文件
        //public static m_xmlDociment m_Rackxml = null;//设置物料文件
        public static string RFIDXMLSavePath = "..\\data\\Set\\RFIDSave.xml";//设置RFID对应物料信息文件
        public static string RackXMLSavePath = "..\\data\\Set\\RFIDSave.xml";//设置RFID对应物料信息文件

        public static String RackstateFilePath = "..\\data\\Order\\RackstateFile";
        public static String RackstateFilebakPath = "..\\data\\Order\\RackstateFilebak";
        public static String IPSetFilePath = "..\\data\\Set\\IPSetFile";
        public static RFIDSavexml m_RFIDxml = null;

        //public static List<CNC> cnclist = null;//CNC设备列表
        public static List<ROBOT> robotlist = null;//机器人设备列表
        public static UInt32 logHandle = 0;//日记句柄，老日记
                                           // public static CMM cmm = null;
        public static int plc_dbNo = -1;  //总控PLC对应redis数据库号
        public static ShareData shareData = null;//共享内存对象
        public static ModbusTcp sptcp1 = null;
        public static lightcommunicate spport1 = null;
        //public PLCDataShare plc = null;

        public static bool bIsDebug = false;
        public static bool bIsOK = false;  //是否合格 
        public static bool robotconnect = false;
        public static bool Orderinitflage = false;  //是否合格 

        public const int USERMESSAGE = 0x0400;
        public const int LanguageChangeMsg = USERMESSAGE + 10;
        public const int ClosingMsg = USERMESSAGE + 1000;//窗口关闭的自定义消息

        public const int UpdateMessureInfo = USERMESSAGE + 300;//更新测量数据 2017.7.24

        public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
        public static IntPtr cncform_Ptr = IntPtr.Zero;///
        public static IntPtr plcform_Ptr = IntPtr.Zero;///
        //         public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
        //         public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
        //         public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄
        //         public static IntPtr mainform_Ptr = IntPtr.Zero;//保存了MainForm的句柄


        //  private CollectShare ncCollector = null;//CNC数据收集器
        private readonly String LOG_PATH = "..\\data\\ScadaLog";//系统日志路径
        private readonly UInt16 maxLogSize = 2 * 1024;
        private readonly Byte maxFileCnt = 10;
        public static bool InitializeComponentFinish = false;//所有设备初始化状态
        public static bool GetInitRackFinish = false;//所有设备初始化状态
        public static bool InitRackFinish = false;//所有设备初始化状态
        public static bool measureenable = false;//测量使能
        //public static String LinckedText = "通信正常";//界面上连接上的状态颜色
        //public static String UnLinckedText = "离线";//界面上没连接上的状态颜色
        //public static String ENLinckedText = "Normal";
        //public static String ENUnLinckedText = "Offline";

        ////public static RFIDDATAT m_ShowRfidDataTable = null;//RFID信息管理
        //public static HFReader SGreader = new HFReader();//RFID

        private String LogFilePath = "..\\data\\Log\\SystemLog.xml";//log日志路径
        public static LogData m_Log = null;//系统日志
        public static string CurrentLanguage;
        public static bool PLC_SIMES_ON_line = false;
        public static String RFIDDataFilePath = "..\\data\\RFID\\RFIDDataDataTableFile";//RFID数据保存
        public readonly String CNCTaskDataFilePath = "..\\data\\CNCTask\\CNCTask.xml";//派工单数据保存

        public static System.EventHandler<EQUIP_STATE> SendEQUIP_STATEHandler = null;

        public static EquipmentCheck m_CheckHander = null;

        public static LogData m_CCDdata = new LogData();
        public static String CCDdataFilePath = "..\\data\\CCD\\CCDDataDataTableFile";
        public static String RackdataFilePath = "..\\data\\Rack\\RackdataFile";
        public static String RackdataFilebakPath = "..\\data\\Rack\\RackdataFilebak";

        public static String IPdataFilePath = "..\\data\\Set\\IpSetFile";
        public static bool flash_flag = true;
        //private Thread CheckThread;

        public static int[] magprocesss1tate = new int[30];//工序1状态，0,未开始；1，上料中；2上料完成；3下料中；4下料完成；5完成
        public static int[] magprocesss2tate = new int[30];//工序2状态，0,未开始；1，上料中；2上料完成；3下料中；4下料完成；5完成       
        public static int[] magisordered = new int[30];//0表示该仓位没有订单，1，表示该仓位已经生成订单
        public static int[] ordertate = new int[30];//工序2状态，0,未开始；1，进行中；2完成。

        public static bool lineinit = true;
        public static bool linestart = true;
        public static bool linestop = false;
        public static bool linereset = true;
        public static bool linestarting = false;
        public static bool linestoping = false;
        public static bool linereseting = false;

        public static string PCplcAddress = "0.0.0.0";
        public static string PCcadAddress = "0.0.0.0";
        public static string PCAddress = "0.0.0.0";
        public static string PLCAddress = "0.0.0.0";
        public static string LATHEAddress = "0.0.0.0";
        public static string CNCAddress = "0.0.0.0";
        public static string ROBORTAddress = "0.0.0.0";
        // public static string RFIDAddress = "0.0.0.0";
        public static string VIDEOAddress = "0.0.0.0";
        // public static string WATCH1Address = "0.0.0.0";
        // public static string WATCH2Address = "0.0.0.0";
        // public static string MeterAddress = "0.0.0.0";
        public static string TRANSERAddress = "0.0.0.0";
        public static bool refreshIpAddress = false;
        public static bool refreshIpAddressxml = false;
        public static int Linebuttomcur = 2;//当前产线哪个按钮正在按下，初始认为产线停止2，产线启动1，产线复位3

        public int inttt = 0;
        public static bool plcgetconfim = false;
        public string language = "";
        public static int key = 1;//20180306测试临时定义
        //         public static Dmis m_dmis;
        //        Random rand = new Random();
        INIAPI iniapi = new INIAPI();
        private static Form[] m_Formarr = null;
        private static DateTime SystemStartime;

        public static Flathedata Slathedata = new Flathedata();
        public static FCNCdata SCNCdata = new FCNCdata();
        public static FOrderdata SOrderdata = new FOrderdata();
        public static FRobortdata SRobortdata = new FRobortdata();
        public static FStoragebin SStoragebin = new FStoragebin();
        public static FUserdata SUserdata = new FUserdata();
        public static FWorkpiececategorydata SWorkpiececategorydata = new FWorkpiececategorydata();
        public static FGaugedata SGaugedata = new FGaugedata();
        public static FGaugedetaildata SGaugedetaildata = new FGaugedetaildata();
        public static FCutter SCutter = new FCutter();
        public static FIpSetFile SIpSetFile = new FIpSetFile();
        public static List<FEquipment> SEquipmentlist = new List<FEquipment>();
        public static bool SQLonline = false;
        private static int sqlconnectcount = 0;
        public static bool sqlconnectflage = false;
        public static bool renewlathesql = true;

        public static bool renewcncsql = true;

        public static bool paichengformshowflage = false;
        public static bool Orderinsertflage = false;
        public static bool orderformshowflage = false;
        public static bool UserLogin = false;
        public static string UserLoginname = "Gust";
        public static Int32[] Mag_Fun_cur = new Int32[30];//每个仓位工件正在执行的工序编号，1,2,3,4
        public static Int32[] Mag_Check = new Int32[30];//每个仓位工件检测标识，1、不合格，0合格。
        #region 注册
        /// <summary>
        /// 取得设备硬盘的卷标号
        /// </summary>
        /// <returns></returns>
        private string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_PhysicalMedia");
            ManagementObjectCollection moc = mc.GetInstances();
            string strID = null;
            foreach (ManagementObject mo in moc)
            {
                try
                {
                    strID = mo.Properties["SerialNumber"].Value.ToString();
                }
                catch (Exception ex)
                {
                }
                // break;
            }
            if (strID == null)
            {
                return "00000000";
            }
            strID = strID.Replace("_", "");
            int length = strID.Length;
            if (length > 10)
            {
                strID = strID.Substring(length - 10, 9);
                return strID;
            }
            else
            {
                strID = strID + "00000000";
                strID = strID.Substring(0, 8);
                return strID;
            }
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
            for (int ii = 0; ii < SNCreateStr.Length; ii++)//求出字符串
            {
                SNCreateStrii ^= SNCreateStr.Substring(ii, 1).GetHashCode();
                str += SNCreateStrii.ToString();
                int index = ((System.Math.Abs(SNCreateStrii)) / (Int32.MaxValue / 2))
                    * (str.Length / 2);
                str = str.Substring(0, index) + SNCreateStr.Substring(ii, 1) + str.Substring(index, str.Length - index - 1);
            }
            return str;
        }

        string jiemi(string str, string encryptKey)
        {
            byte[] P_byte_key = Encoding.Unicode.GetBytes(encryptKey);//将密钥字符串转换为字节序列
            MemoryStream P_MemoryStream_temp = new MemoryStream();//创建内存流对象
            if (str != string.Empty && str != null)
            {
                byte[] P_byte_data = Convert.FromBase64String(str);//将加密后的字符串转换为字节序列
                MemoryStream P_Stream_MS = new MemoryStream(P_byte_data);//创建内存流对象并写入数据
                //创建加密流对象    
                CryptoStream P_CryptStream_Stream =
                    new CryptoStream(P_Stream_MS, new DESCryptoServiceProvider().
                    CreateDecryptor(P_byte_key, P_byte_key), CryptoStreamMode.Read);
                byte[] P_bt_temp = new byte[200];//创建字节序列对象

                int i = 0;//创建记数器
                while ((i = P_CryptStream_Stream.Read(P_bt_temp, 0, P_bt_temp.Length)) > 0)//使用while循环得到解密数据               
                {
                    P_MemoryStream_temp.Write(P_bt_temp, 0, i);//将解密后的数据放入内存流                    
                }
            }
            return Encoding.Unicode.GetString(P_MemoryStream_temp.ToArray());//方法返回解密后的字符串              
        }

        private String GetzhuceEndtime(String str)
        {
            string miyao = "HNC8";
            return jiemi(str, miyao);

        }
        private bool zhuce()
        {
            String SNpath = "..\\data\\Set\\SN.dll";
            String SNCreateStr = getRNum();
            //SNCreateStr = "EMKEMEMM333936I2K17KME8C";
            String SNCreateStr11 = MakeSingSn(SNCreateStr);
            String DT = String.Empty;

            bool isreg = false;
            if (System.IO.File.Exists(SNpath))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(SNpath, Encoding.Default);
                // System.IO.StreamWriter sr = new System.IO.StreamWriter(fs);
                String Allstr = "";
                String SN = "";
                String line = "";

                /*while ((line = sr.ReadLine()) != null)
                {
                    Allstr += line;
                }
                String[] SArray = Allstr.Split(new char[1]{'#'});*/
                SN = sr.ReadLine();
                //MessageBox.Show(SN,"提示");
                DT = sr.ReadLine();
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
                        sw.WriteLine(SNCreateStr11);
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


            if (DT == String.Empty || DT == null)
            {
                System.IO.File.Open(SNpath, System.IO.FileMode.Create);
                SCADA.WindowsForm.Reg m_reg = new SCADA.WindowsForm.Reg();
                m_reg.RegMunber = SNCreateStr;
                m_reg.ShowDialog();
                isreg = false;
            }
            else
            {
                DT = GetzhuceEndtime(DT);
                DateTime dt = Convert.ToDateTime(DT); //注册到期时间
                DateTime nowtime = DateTime.Now;//当前时间
                TimeSpan u = (dt - nowtime);
                if (u.TotalMinutes < 0)
                {
                    System.IO.File.Open(SNpath, System.IO.FileMode.Create);
                    SCADA.WindowsForm.Reg m_reg = new SCADA.WindowsForm.Reg();
                    m_reg.RegMunber = SNCreateStr;
                    m_reg.ShowDialog();
                    //MessageBox.Show("软件已过期，请重新注册！");
                    isreg = false;
                }
            }
            return isreg;
        }
        #endregion

        //ResourceManager m_resource = new ResourceManager(typeof(MainForm));
        Hashtable m_Hashtable;

        public static LanguageChangeHandle languagechangeEvent;

        private TableLayoutPanel tableLayoutPanelBtn = new TableLayoutPanel();
        //private Button button1 = new Button();
        //private Button button2 = new Button();
        //private Button button3 = new Button();//给plc100命令码
        TabPage tabPageLinealarm = new TabPage();
        //TabPage tabPageAGV = new TabPage();

        #region 新机器人接口
        CommApi cmApi;
        ProxyMotion proMot;
        ProxyVm proxyVm;
        #endregion
        #region 新机床接口

        public static List<CNCV2> cncv2list = null;
        public static CollectDataV2 collectdatav2 = null;
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
            m_RFIDxml = new RFIDSavexml();//创建RFID保存文件的对象
            //cnclist = new List<CNC>();//CNC设备列表

            cncv2list = new List<CNCV2>();

            robotlist = new List<ROBOT>();//机器人设备列表
            //cmm = new CMM();
            sptcp1 = new ModbusTcp();//初始化modbustcp数据
            spport1 = new lightcommunicate();//初始化串口
            //lineCCD = new LineCCD();


            LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();

            if (!zhuce())
            {
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_security;
                SendParm.LevelIndex = (int)LogData.Node2Level.AUDIT;
                SendParm.DisplayLang = ChangeLanguage.GetDefaultLanguage();
                SendParm.EventID = ((int)LogData.Node2Level.AUDIT).ToString();
                SendParm.Keywords = ChangeLanguage.GetString("LogContentUserRegistration");
                SendParm.EventData = ChangeLanguage.GetString("LogContentUserRegistrationFailure");
                System.Threading.Thread.Sleep(1000);
                Environment.Exit(0);
            }
            else
            {
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_security;
                SendParm.LevelIndex = (int)LogData.Node2Level.AUDIT;
                SendParm.DisplayLang = ChangeLanguage.GetDefaultLanguage();
                SendParm.EventID = ((int)LogData.Node2Level.AUDIT).ToString();
                SendParm.Keywords = ChangeLanguage.GetString("LogContentUserRegistration");
                SendParm.EventData = ChangeLanguage.GetString("LogContentUserRegistrationSuccess");
            }

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
                System.Windows.Forms.MessageBox.Show("配置文件不存在，已经生成默认的配置文件\r\n");
            }
            if (m_RFIDxml.m_load(RFIDXMLSavePath) == 0)
            {
                System.Windows.Forms.MessageBox.Show("RFID对应物料信息文件不存在，已经生成默认的文件\r\n");
            }

            if (LoadRackState(RackdataFilePath) == 0)
            {
                System.Windows.Forms.MessageBox.Show("料仓信息文件不存在，已经生成默认的文件\r\n");
            }
            if (LoadIpSet(IPdataFilePath) == 0)
            {
                System.Windows.Forms.MessageBox.Show("IP配置文件不存在，已经生成默认的文件\r\n");
            }
            if (initrackfunstate(RackstateFilePath) == 0)
            {
                System.Windows.Forms.MessageBox.Show("订单状态文件不存在，已经生成默认的文件\r\n"); ;
            }
            initmeter(MeterForm.MetersetFilePath1);
            initmeter(MeterForm.MetersetFilePath2);
            initmeter(MeterForm.MetersetFilePath3);
            initmeter(MeterForm.MetersetFilePath4);
            initRacktype();
            //初始化自动排程参数
            AotoOrderForm.Fvalue1stop = false;//提示临界值停自动报警
            AotoOrderForm.zhiliangflage = false;//质量优先
            AotoOrderForm.xiaolvflage = true;//效率优先
            AotoOrderForm.Aleavel = 1;//A中物料的等级
            AotoOrderForm.Bleavel = 1;//A中物料的等级
            AotoOrderForm.Cleavel = 1;//A中物料的等级
            AotoOrderForm.Dleavel = 1;//A中物料的等级
            AotoOrderForm.Fvalue1 = 60;//提示临界值
            AotoOrderForm.Fvalue2 = 80;//报警临界值
                                       // initRacktype();
            AddForm();


            // hisoutput = int.Parse(iniapi.loaddata());

            //------------------------------------Modified by zb 20151111--------------------------------start
            // TcpServerChannel tc = new TcpServerChannel("Scada", 9987);
            //------------------------------------Modified by zb 20151111--------------------------------end

            //     ChannelServices.RegisterChannel(tc, false);
            // LifetimeServices.LeaseTime = TimeSpan.Zero;
            //       ObjRef objrefWellKnown = RemotingServices.Marshal(shareData, "ZKService");

            try
            {
                m_CheckHander = new EquipmentCheck();
                InitEquiment();
                Initport();
                //m_ShowRfidDataTable = new RFIDDATAT();
                InitializeComponentFinish = true;
                if (InitializeComponentFinish)
                {
                    //ncCollector.StartCollection();   //开启采集器
                }
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
                SendParm.DisplayLang = ChangeLanguage.GetDefaultLanguage();
                SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
                SendParm.Keywords = ChangeLanguage.GetString("LogContentDeviceInitSuccess");
                SendParm.EventData = ChangeLanguage.GetString("LogContentAllDeviceInitSuccess");
                SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
            }
            catch (System.Exception ex)
            {
                InitializeComponentFinish = false;
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                SendParm.LevelIndex = (int)LogData.Node2Level.FAULT;
                SendParm.DisplayLang = ChangeLanguage.GetDefaultLanguage();
                SendParm.EventID = ((int)LogData.Node2Level.FAULT).ToString();
                SendParm.Keywords = ChangeLanguage.GetString("LogContentDeviceInitFailure");
                SendParm.EventData = ex.ToString();
                SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
            }

            #region 开启机床数据采集
            //获取机床ip
            var Equipmentlist = InitEquiment(true);

            foreach (var eq in Equipmentlist)
            {
                SEquipmentlist.Add(eq);
            }
            collectdatav2 = new CollectDataV2(Equipmentlist);
            collectdatav2.StartCollection();
            foreach (var collectdata in collectdatav2.GetCNCDataLst)
            {

                cncv2list.Add(collectdata.CNC);
            }

            #endregion
            #region 初始化机器人
            cmApi = new CommApi();
            proMot = new ProxyMotion(cmApi);
            proxyVm = new ProxyVm(cmApi);
            #endregion
            #region 设备数据更新
           var  UpdateThread = new System.Threading.Thread(this.UpDataThreadProc);
            UpdateThread.Start();
            #endregion
            //            
            //             LogApi.WriteLogInfo(MainForm.logHandle, (Byte)ENUM_LOG_LEVEL.LEVEL_WARN
            //                 , "******************************Scada init succes!!******************************");
        }

        private void UpDataThreadProc()
        {
            while (true)
            {
                UpdataData();
                Thread.Sleep(500);
            }
        }
        private void UpdataData()
        {
            if (sqlconnectflage == false)
            {
                if (DbHelper.SQL.State == ConnectionState.Closed)
                {
                    try
                    {
                        DbHelper.SQL.Open();
                        SQLonline = true;
                       
                        sqlconnectcount = 0;
                    }
                    catch
                    {

                        SQLonline = false;                 
                        sqlconnectcount++;
                        if (sqlconnectcount > 3)
                        {
                            sqlconnectflage = true;

                            buttonsqlconnect.BackColor = Color.SpringGreen;
                        }
                    }
                }
            }

            if (SQLonline)
            {
                DbHelper.SQL.Close();
                try
                {


                    renewWorkpieceCategory();
                    renewStoragebin();
                    //renewLathedata();
                    //renewCNCdata();
                    foreach (var cnctemp in cncv2list)
                    {
                        if (cnctemp.cnctype == CNCType.CNC)
                        {
                            renewCNCdata(cnctemp);
                            if (renewcncsql)
                            {

                                renewToolCNC(cnctemp);
                            }

                        }
                        else if (cnctemp.cnctype == CNCType.Lathe)
                        {
                            renewLathedata(cnctemp);
                            if (renewlathesql)
                            {
                                renewToolLathe(cnctemp);
                            }
                        }
                    }
                    renewSRobot(true);


                }
                catch (Exception ex)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("SQL operate error!");
                    }
                    else
                    {

                        MessageBox.Show("数据库操作错误!");
                    }
                }
            }
        }
        private void initRacktype()
        {
            int magtype = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Type;
            int maglength = (int)ModbusTcp.MagLength;
            int magtypei = 0;
            int index = 0;
            for (index = 1; index < 13; index++)
            {
                magtypei = magtype + (index - 1) * maglength;
                ModbusTcp.DataMoubus[magtypei] = 0;
            }
            for (index = 13; index < 25; index++)
            {
                magtypei = magtype + (index - 1) * maglength;
                ModbusTcp.DataMoubus[magtypei] = 1;
            }
            for (index = 25; index < 28; index++)
            {
                magtypei = magtype + (index - 1) * maglength;
                ModbusTcp.DataMoubus[magtypei] = 2;
            }
            for (index = 28; index < 31; index++)
            {
                magtypei = magtype + (index - 1) * maglength;
                ModbusTcp.DataMoubus[magtypei] = 3;
            }
        }
        //MetersetFilePath
        private bool initmeter(string path)//文件
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);

                string item = "";
                string refvalue = "";
                string uppervalue = "";
                string lowervalue = "";




                string line;
                int ii = 0;
                line = sr.ReadLine();

                while (line != null)
                {
                    item = getvalueformstring(line, "item=");
                    refvalue = getvalueformstring(line, "ref=");
                    uppervalue = getvalueformstring(line, "upper=");
                    lowervalue = getvalueformstring(line, "lower=");
                    line = sr.ReadLine();
                    ii++;
                }
                if (ii == 0)
                {
                    ;
                }

                sr.Close();
                aFile.Close();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
        private void Initport()
        {

            if (RackForm.COMOPENFLAGE)
            {
                //打开COM口
                if (!MainForm.spport1.portisopen)//串口开启状态
                {
                    if (MainForm.spport1.Open())//串口打开成功
                    {
                        // 开启串口成功
                        button2.BackColor = Color.Gray;
                        button3.BackColor = Color.LightGreen;
                        RackForm.initlightstate();//料架灯初始化一次
                        //comstate =(int) com_state.comopen;
                        RackForm.COMOPENFLAGE = false;
                        RackForm.COMCLOSEFLAGE = true;
                    }
                    else
                    {//串口打开失败
                        RackForm.COMOPENFLAGE = true;
                        RackForm.COMCLOSEFLAGE = false;
                    }
                }

            }
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
        /// <summary>
        /// 从文件中获取料仓状态
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int LoadRackState(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                int statei = -1;
                int jj = 0;
                string line;
                line = sr.ReadLine();
                while (line != null)
                {

                    int i = line.Length - 2;
                    int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
                    int maglength = (int)ModbusTcp.MagLength;
                    int magstatei = magstart + maglength * (jj);
                    jj++;
                    statei = line.ElementAt(i);
                    statei = statei - 48;
                    if (statei > 10 || statei < 0)
                    {
                        sr.Close();
                        aFile.Close();
                        ReLoadRackState();
                        return 1;
                    }
                    ModbusTcp.DataMoubus[magstatei] = statei;//获取料仓状态


                    line = sr.ReadLine();
                }
                sr.Close();
                aFile.Close();
                return 1;

            }
            catch (IOException)
            {
                ReLoadRackState();
                return 0;
            }
        }
        private int ReLoadRackState()
        {
            try
            {
                File.Delete(RackdataFilePath);
                File.Copy(RackdataFilebakPath, RackdataFilePath, true);

                File.Delete(OrderForm1.OrderdataFilePath);
                File.Copy(OrderForm1.OrderdataFilebakPath, OrderForm1.OrderdataFilePath, true);

                File.Delete(OrderForm1.OrderstateFilePath);
                File.Copy(OrderForm1.OrderstateFilebakPath, OrderForm1.OrderstateFilePath, true);

                FileStream aFile = new FileStream(RackdataFilePath, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                int statei = -1;
                int jj = 0;
                string line;
                line = sr.ReadLine();
                while (line != null)
                {
                    int i = line.Length - 2;
                    int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
                    int maglength = (int)ModbusTcp.MagLength;
                    int magstatei = magstart + maglength * (jj);
                    jj++;
                    statei = line.ElementAt(i);
                    statei = statei - 48;
                    ModbusTcp.DataMoubus[magstatei] = statei;//获取料仓状态
                    line = sr.ReadLine();
                }
                sr.Close();
                aFile.Close();
                return 1;
            }
            catch (IOException ex)
            {
                return 0;
            }
        }

        private int LoadIpSet(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                int jj = 0;
                string line;
                line = sr.ReadLine();
                while (line != null)
                {
                    if (line == "")
                    {

                        sr.Close();
                        aFile.Close();
                        return 1;
                    }
                    int index = line.IndexOf(':');
                    if (index == -1)
                    {
                        jj++;
                        line = sr.ReadLine(); ;
                    }
                    string title = line.Substring(0, index);
                    string ip = line.Substring(index + 1, line.Length - index - 2);
                    if (title == "PC")
                    {
                        PCAddress = ip;
                    }
                    if (title == "PLCPC")
                    {
                        PCplcAddress = ip;
                    }
                    if (title == "CADPC")
                    {
                        PCcadAddress = ip;
                    }

                    if (title == "PLC")
                    {
                        PLCAddress = ip;
                    }
                    if (title == "LATHE")
                    {
                        LATHEAddress = ip;
                    }
                    if (title == "CNC")
                    {
                        CNCAddress = ip;
                    }
                    if (title == "ROBORT")
                    {
                        ROBORTAddress = ip;
                    }
                    if (title == "VIDEO")
                    {
                        VIDEOAddress = ip;
                    }
                    //if (title == "RFID")
                    //{
                    //    RFIDAddress = ip;
                    //}
                    //if (title == "WATCH1")
                    //{
                    //    WATCH1Address = ip;
                    //}
                    //if (title == "WATCH2")
                    //{
                    //    WATCH2Address = ip;
                    //}
                    //if (title == "METER")
                    //{
                    //    MeterAddress = ip;
                    //}
                    //if (title == "RFID")
                    //{
                    //    RFIDAddress = ip;
                    //}
                    jj++;
                    line = sr.ReadLine();
                }
                sr.Close();
                aFile.Close();
                return 1;
            }
            catch (IOException e)
            {
                return 0;
            }
        }
        /// <summary>
        /// 初始化时检测实际料位信息和文件读取的信息是否一致
        /// </summary>
        private void CheckInitRackState()
        {
            int jj = 1;
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = -1;
            int statei = -1;
            if (sptcp1 == null)
            {
                return;
            }

            InitRackFinish = true;

            for (jj = 1; jj < 30; jj++)
            {
                magstatei = magstart + maglength * (jj - 1);
                statei = ModbusTcp.DataMoubus[magstatei];
                if (statei == (int)ModbusTcp.Mag_state_config.Statewait
                       || statei == (int)ModbusTcp.Mag_state_config.StateFinishStandard
                       || statei == (int)ModbusTcp.Mag_state_config.StateFailure
                       || statei == (int)ModbusTcp.Mag_state_config.StateFinishNotStandard)
                {
                    if (jj <= 16)
                    {
                        if (!sptcp1.GetMagOnbit(jj))//无料
                        {
                            string temp = jj.ToString();
                            ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statenull;
                            temp = temp + "号物料信息加载失败，恢复默认信息";
                            MessageBox.Show(temp);
                        }
                    }
                    else
                    {
                        if (!sptcp1.GetMagOnbit(jj))//无料
                        {
                            string temp = jj.ToString();
                            ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statenull;
                            temp = temp + "号物料信息加载失败，恢复默认信息";
                            MessageBox.Show(temp);
                        }
                    }

                }
                else if (statei == (int)ModbusTcp.Mag_state_config.Statenull
                    || statei == (int)ModbusTcp.Mag_state_config.StateProcessing)
                {
                    if (jj <= 16)
                    {
                        if (sptcp1.GetMagOnbit(jj))//有料
                        {
                            string temp = jj.ToString();
                            ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statenull;
                            temp = temp + "号物料信息加载失败，恢复默认信息";
                            MessageBox.Show(temp);
                        }
                    }
                    else
                    {
                        if (sptcp1.GetMagOnbit(jj))//有料
                        {
                            string temp = jj.ToString();
                            ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statenull;
                            temp = temp + "号物料信息加载失败，恢复默认信息";
                            MessageBox.Show(temp);
                        }
                    }
                }
            }
        }
        private void OnlanguagechangeEvent(string language)
        {
            if (languagechangeEvent != null)
            {
                languagechangeEvent(this, language);
            }
        }

        //private void registerDialog()
        //{
        //    //             MessageBoxManager.Unregister();
        //    //             MessageBoxManager.OK = Localization.Dialog["201"];
        //    //             MessageBoxManager.Cancel = Localization.Dialog["202"];
        //    //             MessageBoxManager.Yes = Localization.Dialog["203"];
        //    //             MessageBoxManager.No = Localization.Dialog["204"];
        //    //             MessageBoxManager.Retry = Localization.Dialog["205"];
        //    //             MessageBoxManager.Abort = Localization.Dialog["206"];
        //    //             MessageBoxManager.Ignore = Localization.Dialog["207"];
        //    //             MessageBoxManager.Register();
        //}

        /// <summary>
        /// 根据XML文件初始化设备对象
        /// </summary>
        //private void InitEquiment()
        //{
        //    //            m_dmis = new Dmis("http://192.168.2.10:8080/dmis/", "http://192.168.2.90:8886/dmis?command=refresh&machineName=CMM1");
        //    //             m_dmis = new Dmis("http://localhost:8080/dmis/", "http://localhost:8081/dmis/");


        //    LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
        //    string get_str = m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[0]);//SUM
        //    for (int ii = 0; ii < int.Parse(get_str); ii++)
        //    {
        //        CNC m_cnc = new CNC();
        //        m_cnc.InitCNCParam(m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.serial]),
        //             m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.workshop]),
        //             m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.productionline]),
        //             m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.system]),
        //             m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]),
        //             m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.port]),
        //             m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.SN]),
        //             m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.EQUIP_CODE]),
        //             m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.remark]),
        //             CNCTaskDataFilePath);
        //        //net_to_redis
        //        if (MacDataService.GetInstance().GetMachineDbNo(m_cnc.ip, ref m_cnc.dbNo) == 0)
        //            Console.WriteLine(" m_cnc.dbNo = " + m_cnc.dbNo);

        //        cnclist.Add(m_cnc);
        //        SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
        //        SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
        //        SendParm.DisplayLang = ChangeLanguage.GetDefaultLanguage();
        //        SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
        //        SendParm.Keywords = ChangeLanguage.GetString("LogContentCNCInit");
        //        SendParm.EventData = m_cnc.JiTaiHao + ChangeLanguage.GetString("LogContentInitSuccess") + "；IP = " + m_cnc.ip + "  Port = " + m_cnc.port;
        //        SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
        //        if (SCADA.MainForm.SendEQUIP_STATEHandler != null)//添加CNC设备状态数据
        //        {
        //            EQUIP_STATE m_EQUIP_STATE = new EQUIP_STATE();
        //            m_EQUIP_STATE.EQUIP_TYPE = 0;
        //            m_EQUIP_STATE.EQUIP_CODE = m_cnc.BujianID;// VARCHAR2(50),设备ID
        //            m_EQUIP_STATE.EQUIP_CODE_CNC = ""; // VARCHAR2(50),cnc:SN号
        //            m_EQUIP_STATE.STATE_VALUE = -1; // FLOAT(126),-1：离线状态，0：一般状态（空闲状态），1：循环启动（运行状态），2：进给保持（空闲状态），3：急停状态（报警状态）
        //            //SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
        //        }

        //        ////总控PLC只有一个  2017.6.27
        //        //string ip = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
        //        //if (MacDataService.GetInstance().GetMachineDbNo(ip, ref plc_dbNo) == 0)
        //        //    Console.WriteLine(" PLC.dbNo = " + plc_dbNo);
        //    }


        //    /*if (cnclist.Count > 0)
        //    {
        //        int rec = HncApi.HNC_NetInit(m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]), cnclist[0].port);
        //        if (rec != 0)
        //        {
        //            SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
        //            SendParm.LevelIndex = (int)LogData.Node2Level.WARNING;
        //            SendParm.EventID = ((int)LogData.Node2Level.WARNING).ToString();
        //            SendParm.Keywords = "CNC初始化";
        //            SendParm.EventData = "本地网络环境初始化失败；IP = " + m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]) +
        //                               "  端口 = " + cnclist[0].port;
        //            SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
        //            throw new Exception(SendParm.EventData);
        //        }
        //        else
        //        {
        //            SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
        //            SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
        //            SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
        //            SendParm.Keywords = "CNC初始化";
        //            SendParm.EventData = "本地网络环境初始化成功；IP = " + m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]) +
        //                               "  端口 = " + cnclist[0].port;
        //            SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
        //        }
        //    }*/


        //    //get_str = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, -1, m_xmlDociment.Default_Attributes_str1[0]);
        //    //for (int ii = 0; ii < int.Parse(get_str); ii++)
        //    //{
        //    //    PLC_MITSUBISHI_HNC8 m_slplc = new PLC_MITSUBISHI_HNC8();
        //    //    m_slplc.serial = Int32.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Path_str[8]));
        //    //    m_slplc.ID = Int32.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.id]));
        //    //    m_slplc.ip = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
        //    //    m_slplc.port = UInt16.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.port]));
        //    //    m_slplc.remark = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.remark]);
        //    //    m_slplc.system = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.system]);
        //    //    m_slplc.type = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.type]);
        //    //    m_slplc.productionline = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.productionline]);
        //    //    m_slplc.workshop = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.workshop]);
        //    //    m_slplc.EQUIP_CODE = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.EQUIP_CODE]);
        //    //    m_slplc.SN = m_xml.m_Read(m_xmlDociment.PathRoot_PLC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.SN]);

        //    //    if (SCADA.MainForm.SendEQUIP_STATEHandler != null)//添加PLC设备状态数据
        //    //    {
        //    //        EQUIP_STATE m_EQUIP_STATE = new EQUIP_STATE();
        //    //        m_EQUIP_STATE.EQUIP_TYPE = 2;
        //    //        m_EQUIP_STATE.EQUIP_CODE = m_slplc.EQUIP_CODE;// VARCHAR2(50),设备ID
        //    //        m_EQUIP_STATE.EQUIP_CODE_CNC = m_slplc.SN; // VARCHAR2(50),cnc:SN号
        //    //        m_EQUIP_STATE.STATE_VALUE = -1; // FLOAT(126),-1：离线状态，0：一般状态（空闲状态），1：循环启动（运行状态），2：进给保持（空闲状态），3：急停状态（报警状态）
        //    //        //SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
        //    //    }

        //    //    String[] Device;
        //    //    if (m_slplc.system == m_xmlDociment.PLC_System[0])
        //    //    {
        //    //        Device = new String[m_xmlDociment.Default_MITSUBISHI_Device1.Length + m_xmlDociment.Default_MITSUBISHI_Device2.Length];
        //    //        for (int qq = 0; qq < m_xmlDociment.Default_MITSUBISHI_Device1.Length; qq++)
        //    //        {
        //    //            Device[qq] = m_xmlDociment.Default_MITSUBISHI_Device1[qq];
        //    //        }
        //    //        for (int qq = 0; qq < m_xmlDociment.Default_MITSUBISHI_Device2.Length; qq++)
        //    //        {
        //    //            Device[m_xmlDociment.Default_MITSUBISHI_Device1.Length + qq] = m_xmlDociment.Default_MITSUBISHI_Device2[qq];
        //    //        }
        //    //    }
        //    //    else if (m_slplc.system == m_xmlDociment.PLC_System[1])
        //    //    {
        //    //        Device = new String[m_xmlDociment.Default_HNC8_Device1.Length + m_xmlDociment.Default_HNC8_Device2.Length];
        //    //        for (int qq = 0; qq < m_xmlDociment.Default_HNC8_Device1.Length; qq++)
        //    //        {
        //    //            Device[qq] = m_xmlDociment.Default_HNC8_Device1[qq];
        //    //        }
        //    //        for (int qq = 0; qq < m_xmlDociment.Default_HNC8_Device2.Length; qq++)
        //    //        {
        //    //            Device[m_xmlDociment.Default_HNC8_Device1.Length + qq] = m_xmlDociment.Default_HNC8_Device2[qq];
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        Device = new String[0];
        //    //    }
        //    //    //for (int jj = 0; jj < Device.Length; jj++)
        //    //    //{
        //    //    //    string pathstr1 = m_xmlDociment.PathRoot_PLC_Item + ii.ToString();//Root/PLC/Itemii
        //    //    //    string pathstr2 = pathstr1 + "/" + Device[jj];//"";
        //    //    //    if (!m_xml.CheckNodeExist(pathstr2))
        //    //    //    {
        //    //    //        continue;
        //    //    //    }

        //    //    //    Int32 Count = Int32.Parse(m_xml.m_Read(pathstr2, -1, m_xmlDociment.Default_Attributes_str1[0]));
        //    //    //    String[] AddressGeshi = m_xml.m_Read(pathstr2, -1, m_xmlDociment.Default_Attributes_str2[1]).Split('-');

        //    //    //    if (m_slplc.system == m_xmlDociment.PLC_System[0])
        //    //    //    {
        //    //    //        m_slplc.InitMITSUBISHIPLC_Signal(Device[jj], Int32.Parse(AddressGeshi[0]), m_xml.m_Read(pathstr2, -1, m_xmlDociment.Default_Attributes_str2[4]), Count);
        //    //    //        for (int kk = 0; kk < Count; kk++)
        //    //    //        {
        //    //    //            bool jiankong;
        //    //    //            if (m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[3]) == m_xmlDociment.Default_Attributesstr2_value[3])
        //    //    //            {
        //    //    //                jiankong = false;
        //    //    //            }
        //    //    //            else
        //    //    //            {
        //    //    //                jiankong = true;
        //    //    //            }
        //    //    //            m_slplc.AddSignalNode2List(Device[jj], kk,
        //    //    //                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[1]),
        //    //    //                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[2]), 0,
        //    //    //                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[4]),
        //    //    //                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[5]),
        //    //    //                jiankong);
        //    //    //        }
        //    //    //    }
        //    //    //    else if (m_slplc.system == m_xmlDociment.PLC_System[1])
        //    //    //    {
        //    //    //        Int32 HncRegType_i = PLC_MITSUBISHI_HNC8.GetHncRegType(Device[jj]);
        //    //    //        m_slplc.InitHNC8PLC_Signal(Device[jj], Int32.Parse(AddressGeshi[0]), HncRegType_i, Count);
        //    //    //        for (int kk = 0; kk < Count; kk++)
        //    //    //        {
        //    //    //            bool jiankong;
        //    //    //            if (m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[3]) == m_xmlDociment.Default_Attributesstr2_value[3])
        //    //    //            {
        //    //    //                jiankong = false;
        //    //    //            }
        //    //    //            else
        //    //    //            {
        //    //    //                jiankong = true;
        //    //    //            }
        //    //    //            m_slplc.AddSignalNode2List(Device[jj], kk,
        //    //    //                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[1]),
        //    //    //                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[2]), 0,
        //    //    //                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[4]),
        //    //    //                m_xml.m_Read(pathstr2, kk, m_xmlDociment.Default_Attributes_str2[5]),
        //    //    //                jiankong);
        //    //    //        }

        //    //    //    }
        //    //    //}
        //    //    //plc.AddPLC(m_slplc);

        //    //    /////////添加ROBOT信号点到PLC
        //    //    if (m_slplc.serial == 0)//目前只有一个PLC
        //    //    {
        //    //        int robotItemsum = int.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, -1, m_xmlDociment.Default_Attributes_str1[0]));
        //    //        for (int jj = 0; jj < robotItemsum; jj++)
        //    //        {

        //    //            String RobotPathStr = m_xmlDociment.PathRoot_ROBOT_Item + jj.ToString();

        //    //            String DeviceX = m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.X];
        //    //            String DeviceY = m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.Y];
        //    //            String RobotPathXStr = RobotPathStr + "/" + DeviceX;
        //    //            String RobotPathYStr = RobotPathStr + "/" + DeviceY;
        //    //            Int32 StatX = 0;
        //    //            Int32 StatY = 0;
        //    //            int robotItemXsum = int.Parse(m_xml.m_Read(RobotPathXStr, -1, m_xmlDociment.Default_Attributes_str2[0]));
        //    //            int robotItemYsum = int.Parse(m_xml.m_Read(RobotPathYStr, -1, m_xmlDociment.Default_Attributes_str2[0]));
        //    //            m_slplc.InitRobot_Signal(DeviceX, robotItemXsum, ref StatX);
        //    //            m_slplc.InitRobot_Signal(DeviceY, robotItemYsum, ref StatY);
        //    //            for (int xx = 0; xx < robotItemXsum; xx++)
        //    //            {
        //    //                bool jiankong;
        //    //                if (m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[3]) == m_xmlDociment.Default_Attributesstr2_value[3])
        //    //                {
        //    //                    jiankong = false;
        //    //                }
        //    //                else
        //    //                {
        //    //                    jiankong = true;
        //    //                }
        //    //                m_slplc.AddSignalNode2List(DeviceX, StatX + xx,
        //    //                    m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[1]),
        //    //                    m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[2]), 0,
        //    //                    m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[4]),
        //    //                    m_xml.m_Read(RobotPathXStr, xx, m_xmlDociment.Default_Attributes_str2[5]),
        //    //                    jiankong);
        //    //            }
        //    //            for (int xx = 0; xx < robotItemYsum; xx++)
        //    //            {
        //    //                bool jiankong;
        //    //                if (m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[3]) == m_xmlDociment.Default_Attributesstr2_value[3])
        //    //                {
        //    //                    jiankong = false;
        //    //                }
        //    //                else
        //    //                {
        //    //                    jiankong = true;
        //    //                }
        //    //                m_slplc.AddSignalNode2List(DeviceY, StatY + xx,
        //    //                    m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[1]),
        //    //                    m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[2]), 0,
        //    //                    m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[4]),
        //    //                    m_xml.m_Read(RobotPathYStr, xx, m_xmlDociment.Default_Attributes_str2[5]),
        //    //                    jiankong);
        //    //            }
        //    //            ROBOT mrobot = new ROBOT();
        //    //            mrobot.SetRobot(m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Path_str[8]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.id]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.workshop]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.productionline]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.type]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.system]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.port]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.EQUIP_CODE]),
        //    //                m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, jj, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.SN]),
        //    //                StatX, robotItemXsum, StatY, robotItemYsum);
        //    //            robotlist.Add(mrobot);
        //    //        }
        //    //    }

        //    //    //#region 连接RFID
        //    //    /////添加RFID XML文件初始化 2017.4.26 zxl
        //    //    //int RFIDiiSum = int.Parse(m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]));
        //    //    //if (RFIDiiSum != 0)
        //    //    //{
        //    //    //    RFIDConnect(SendParm);

        //    //    //}
        //    //    //#endregion


        //    //}
        //    //plc.StarCollectThread();
        //}

        /// <summary>
        /// 根据XML文件初始化设备对象
        /// </summary>
        private List<FEquipment> InitEquiment(bool t = true)
        {
            List<FEquipment> eqlistcnc = new List<FEquipment>();
            LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
            string get_str = m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[0]);//SUM
            for (int ii = 0; ii < int.Parse(get_str); ii++)
            {
                FEquipment equipmentcnc = new FEquipment();
                equipmentcnc.IP = m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
                equipmentcnc.Port = Convert.ToUInt16(m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.port]));
                equipmentcnc.IsConnect = false;
                if (m_xml.m_Read(m_xmlDociment.PathRoot_CNC, ii, m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.serial]) == "HNC_818A")
                {
                    equipmentcnc.Type = EquipmentType.Lath;
                }
                else equipmentcnc.Type = EquipmentType.CNC;
                eqlistcnc.Add(equipmentcnc);
            }
            return eqlistcnc;
        }

        /// <summary>
        /// 连接RFID 2017.5.02 zxl
        /// </summary>
        /// <param name="flag">读写器序号</param>
        public void RFIDConnect(LogData.EventHandlerSendParm sendParm)
        {
            string[] Attributesstr = new String[m_xmlDociment.Default_Attributes_RFID.Length];

            Attributesstr[13] = m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[13]);//读写器IP地址
            Attributesstr[14] = m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[14]);//读写器端口
        }


        //public static bool PingTestCNC(string ip)
        //{
        //    bool connect = false;
        //    Ping pingtest = new Ping();
        //    try
        //    {
        //        PingReply reply = pingtest.Send(ip);
        //        if (reply.Status == IPStatus.Success)
        //            connect = true;
        //    }
        //    catch
        //    {
        //    }
        //    return connect;
        //}

        public static bool PingTestCNC(string ip, int timeout)
        {
            bool connect = false;
            Ping pingtest = new Ping();
            try
            {
                PingReply reply = pingtest.Send(ip, timeout);
                if (reply.Status == IPStatus.Success)
                    connect = true;
            }
            catch
            {
            }
            return connect;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine("load");
            //groupBox7.Visible = false;
            //初始打开时加载HomeForm
            m_Formarr = new Form[tabPagemain.TabCount];
            CurrentLanguage = ChangeLanguage.GetDefaultLanguage();
            LoadSCADALanguage(CurrentLanguage);
            ChangeLanguage.LoadLanguage(this);//zxl 4.19

            if (tabPagemain.TabCount > 0)
            {
                GenerateForm(0, tabPagemain);
            }
            //总控PLC只有一个  2017.6.27
            //string ip = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
            //if (MacDataService.GetInstance().GetMachineDbNo(ip, ref plc_dbNo) == 0)
            //    Console.WriteLine(" PLC.dbNo = " + plc_dbNo);
        }



        //public string paigongdate = "";
        //public int paigongnum = 0;
        //public static int nowoutput = 0;
        //public static int hisoutput = 0;
        //object paigonglock = new object();
        //public void PaiGongRefresh()
        //{
        //    lock (paigonglock)
        //    {
        //        if (paigongnum <= nowoutput)
        //        {
        //            int addpaigong = 100;
        //            //                    addpaigong = rand.Next(10, 30);
        //            paigongnum += addpaigong;
        //            paigongdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //        }
        //        SetlabTaskFailedNumText(nowoutput.ToString(), paigongnum.ToString(), paigongdate, hisoutput.ToString());
        //    }
        //}


        //在选项卡中生成窗体
        public void GenerateForm(int form_index, object sender)
        {
            // 反射生成窗体//只生成一次
            if (m_Formarr[form_index] == null && form_index >= 0 && form_index < tabPagemain.TabCount)
            {
                string formClassSTR = ((TabControl)sender).SelectedTab.Tag.ToString();
                m_Formarr[form_index] = (Form)Assembly.GetExecutingAssembly().CreateInstance(formClassSTR);
                // m_Formarr[10 ] = { "SCADA.VideoForm, Text: VideoForm"};
                if (m_Formarr[form_index] != null)
                {
                    //设置窗体没有边框 加入到选项卡中
                    m_Formarr[form_index].FormBorderStyle = FormBorderStyle.None;
                    m_Formarr[form_index].TopLevel = false;
                    m_Formarr[form_index].Parent = ((TabControl)sender).SelectedTab;
                    m_Formarr[form_index].ControlBox = false;
                    m_Formarr[form_index].Dock = DockStyle.Fill;
                    if (ChangeLanguage.defaultcolor != Color.White)
                    {
                        ChangeLanguage.LoadSkin(m_Formarr[form_index], ChangeLanguage.defaultcolor);
                    }
                    m_Formarr[form_index].Show();

                }
            }
            if (form_index == 1)
            {
                paichengformshowflage = true;
            }


        }


        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabPagemain.SuspendLayout();
            GenerateForm(tabPagemain.SelectedIndex, sender);
            tabPagemain.ResumeLayout();
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
                    tabPagemain.SelectedIndex = (int)m.WParam;
                    break;
                case USERMESSAGE + 1:
                    //RobotForm.ROBOTIndex = (int)m.LParam;
                    //tabPagemain.SelectedIndex = 2;
                    break;
                case USERMESSAGE + 2:
                    //CMMForm.cmmindex = 1;
                    //tabPagemain.SelectedIndex = 7;
                    break;
                case USERMESSAGE + 13:  //料架
                    tabPagemain.SelectedIndex = 5;
                    break;
                case USERMESSAGE + 14:  //rfid
                    tabPagemain.SelectedIndex = 4;
                    break;
                case USERMESSAGE + 15: //测量
                    tabPagemain.SelectedIndex = 6;
                    break;
                case USERMESSAGE + 300://更新测量结果 2017.7.24
                                       // OrderForm.UpdateMeasureRes();
                    break;

                case USERMESSAGE + 16: //派单
                    tabPagemain.SelectedIndex = 3;
                    break;
                case USERMESSAGE + 100://添加派工单 CNC设备控制数据
                    TaskDataForm.AddTaskData2Form((int)m.WParam);
                    break;
                case USERMESSAGE + 200://Web端获取CNC文本文件

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



        void button1_Click(object sender, EventArgs e)
        {
            if (MainForm.UserLogin == false)
            {
                if (language == "English")
                {
                    MessageBox.Show("User has no permission!");
                }
                else MessageBox.Show("当前用户无此权限!");
                return;
            }
            if (!MainForm.PLC_SIMES_ON_line)
            {
                if (language == "English")
                {
                    MessageBox.Show("PLC is offline");
                }
                else MessageBox.Show("PLC离线");
                return;
            }
            if (ModbusTcp.MES_PLC_comfim_write_flage)//产线正在复位或者盘点
            {
                if (linereseting == true)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The Line is restarting");
                    }
                    else MessageBox.Show("产线复位中,不能开启产线");
                }
                else if (RackForm.Inventoryflag == false)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The rack is Inventory");
                    }
                    else MessageBox.Show("料架盘点中,不能开启产线");
                }
                else if (linestarting == true)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The line is starting");
                    }
                    else MessageBox.Show("产线正在启动中");
                }
                else if (linestoping == true)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The line is stopping");
                    }
                    MessageBox.Show("产线停止中,不能开启产线");
                }

                return;
            }

            if (linestart)
            {

                if (ModbusTcp.MES_PLC_comfim_write_flage == false)
                {
                    Linebuttomcur = 1;
                    ModbusTcp.MES_PLC_comfim_write_flage = true;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComStartSys;
                    linestart = false;
                    linestarting = true;
                    button1.BackColor = Color.LightPink;
                    linereset = false;//产线启动中或者产线启动状态不能点击复位按钮
                    linestop = false;//产线启动中不能点击停止按钮
                    linereseting = false;//产线启动中或者产线启动状态不能点击复位按钮
                    linestoping = false;//产线启动中或者产线启动状态不能点击复位按钮
                    button3.BackColor = Color.Gray;
                    button2.BackColor = Color.Gray;
                    if (language == "English")
                    {

                        button1.Text = "STARTING";
                    }
                    else button1.Text = "产线启动中";
                }
                if (ModbusTcp.MES_PLC_comfim_write_flage == false)
                {
                    button1.BackColor = Color.Gray;

                    linestarting = false;
                    if (language == "English")
                    {

                        button1.Text = "STARTED";
                    }
                    else
                        button1.Text = "产线已开启";
                    //  linestart = true;

                    button2.BackColor = Color.LightGreen;
                    if (language == "English")
                    {
                        linestart = false;
                        button2.Text = "STOP";
                    }
                    else
                    {
                        button2.Text = "产线停止";
                    }
                    linestop = true;
                    linereset = false;
                }

            }
            else
            {
                return;
            }
        }

        void button2_Click(object sender, EventArgs e)
        {
            if (!MainForm.PLC_SIMES_ON_line)
            {
                if (language == "English")
                {
                    MessageBox.Show("PLC is offline");
                }
                else MessageBox.Show("PLC离线");
                return;
            }
            if (RackForm.Inventoryflag == false)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rack is Inventory");
                }
                else MessageBox.Show("料架盘点中,不能开启产线");
                return;
            }
            if (linestop)
            {
                //  if (ModbusTcp.MES_PLC_comfim_write_flage == true)
                //{
                if (linereseting == true)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The line is resetting");
                    }
                    else MessageBox.Show("产线复位进行中，不能停止");
                    return;
                    //if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComStartDevice)
                    //{
                    //    ModbusTcp.MES_PLC_comfim_write_flage = false;
                    //    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                    //    linereseting = false;
                    //    linereset = false;
                    //    button3.BackColor = Color.Gray;
                    //    if (language == "English")
                    //    {

                    //        button3.Text = "RESET";
                    //    }
                    //    else
                    //        button3.Text = "产线复位";
                    //}
                }
                if (linestarting == true)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The line is startting");
                    }
                    else MessageBox.Show("产线启动进行中，不能停止");
                    return;
                    //if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComStartSys)
                    //{
                    //    ModbusTcp.MES_PLC_comfim_write_flage = false;
                    //    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                    //    linestarting = false;
                    //    linestart = false;
                    //    button1.BackColor = Color.Gray;
                    //    if (language == "English")
                    //    {

                    //        button1.Text = "START";
                    //    }
                    //    else
                    //        button1.Text = "产线启动";
                    //}
                }
                if (!RackForm.rfidreadflag)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The Rack is Inventory");
                    }
                    else MessageBox.Show("料仓盘点中，不能停止");
                    return;
                    //if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComReadRfid)
                    //{
                    //    ModbusTcp.MES_PLC_comfim_write_flage = false;
                    //    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;

                    //    RackForm.rfidreadflag = true;
                    //}
                }
                if (!RackForm.rfidwriteflag)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The Rack is Inventory");
                    }
                    else MessageBox.Show("料仓盘点中，不能停止");
                    return;
                    //if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComWriteRfid)
                    //{
                    //    ModbusTcp.MES_PLC_comfim_write_flage = false;
                    //    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                    //    RackForm.rfidwriteflag = true;
                    //}
                }
                if (!RackForm.Inventoryflag)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The Rack is Inventory");
                    }
                    else MessageBox.Show("料仓盘点中，不能停止");
                    return;
                    //if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComReadRfid)
                    //{
                    //    ModbusTcp.MES_PLC_comfim_write_flage = false;
                    //    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                    //    RackForm.Inventoryflag = true;
                    //}
                }

                if (ModbusTcp.MES_PLC_comfim_write_flage == false)
                {

                    Linebuttomcur = 2;
                    ModbusTcp.MES_PLC_comfim_write_flage = true;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComStopSys;
                    linestop = false;
                    linestoping = true;
                    button2.BackColor = Color.LightPink;
                    linereset = false;//产线启动中或者产线启动状态不能点击复位按钮
                    linestart = false;//产线启动中或者产线启动状态不能点击复位按钮
                    linereseting = false;//产线启动中或者产线启动状态不能点击复位按钮
                    linestarting = false;//产线启动中或者产线启动状态不能点击复位按钮
                    button1.BackColor = Color.Gray;
                    button3.BackColor = Color.Gray;
                    if (language == "English")
                    {

                        button2.Text = "STOPING";
                    }
                    else button2.Text = "产线停止中";
                }

                if (ModbusTcp.MES_PLC_comfim_write_flage == false)
                {

                    button1.BackColor = Color.LightGreen;
                    button3.BackColor = Color.LightGreen;
                    button2.BackColor = Color.Gray;
                    linestoping = false;

                    if (language == "English")
                    {

                        button1.Text = "START";
                        button2.Text = "STOPPED";
                    }
                    else
                    {
                        button1.Text = "产线开启";
                        button2.Text = "产线已停止";
                    }


                    linestart = true;
                    linestarting = false;
                    linereset = true;
                    linereseting = false;
                }
            }

            // }
        }

        void button3_Click(object sender, EventArgs e)
        {
            //if (!MainForm.PLC_SIMES_ON_line)
            //{
            //    if (language == "English")
            //    {
            //        MessageBox.Show("PLC is offline");
            //    }
            //    else MessageBox.Show("PLC离线");
            //    return;
            //}
            //if (linereset)
            // {
            //if (linestop)//产线没有停止
            //{
            //    if (language == "English")
            //    {
            //        MessageBox.Show("Please stop theline!");
            //    }
            //    else MessageBox.Show("产线停止后才能复位！");
            //    return;
            //}
            ////获取当前默认语言  
            ////string language = ChangeLanguage.GetDefaultLanguage();
            //if (RackForm.Inventoryflag == false)
            //{
            //    MessageBox.Show("料架盘点中,不能开启产线");
            //    return;
            //}
            //if (ModbusTcp.MES_PLC_comfim_write_flage == false)
            // {

            if (OrderForm1.GcodeSenttoCNC(0, 2) != 1)
            {
                MessageBox.Show("加工中心回零程序下发失败");
                return;
            }
            if (OrderForm1.GcodeSenttoCNC(0, 1) != 1)
            {
                MessageBox.Show("车床回零程序下发失败");
                return;
            }
            renewww = true;
            Linebuttomcur = 3;
            ModbusTcp.MES_PLC_comfim_write_flage = true;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComStartDevice;
            linereset = false;
            linereseting = true;
            button3.BackColor = Color.LightPink;

            linestop = false;
            linestoping = false;
            linestart = false;//产线启动中或者产线启动状态不能点击复位按钮
            linestarting = false;//产线启动中或者产线启动状态不能点击复位按钮
            button1.BackColor = Color.Gray;
            button2.BackColor = Color.Gray;
            if (language == "English")
            {

                button2.Text = "STOP";
            }
            else
                button2.Text = "产线停止";
            if (language == "English")
            {

                button3.Text = "RESETING";
            }
            else button3.Text = "产线复位中";
            // }
            if (ModbusTcp.MES_PLC_comfim_write_flage == false)
            {
                button3.BackColor = Color.LightGreen;
                if (language == "English")
                {

                    button3.Text = "RESET";
                }
                else
                    button3.Text = "产线复位";
                linereset = true;
            }
            // }
            // else
            // {
            //    return;
            //}
        }

        private static void AddForm()
        {

        }
        private void 中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //窗口语言的切换
            ChangeLanguage.SetDefaultLanguage("Chinese");
            CurrentLanguage = ChangeLanguage.GetDefaultLanguage();
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadLanguage(form);
            }


            //读取“任务”窗口中文
            TaskDataForm.ctrlTaskData.ExChangeLanguage();
            LoadSCADALanguage(CurrentLanguage);
            m_CheckHander.LoadEquipLanguage(CurrentLanguage);
            OnlanguagechangeEvent("Chinese");
        }

        private void 英文ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //窗口语言的切换
            ChangeLanguage.SetDefaultLanguage("English");
            CurrentLanguage = ChangeLanguage.GetDefaultLanguage();
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadLanguage(form);
            }

            //读取“任务”窗口英文
            TaskDataForm.ctrlTaskData.ExChangeLanguage();
            LoadSCADALanguage(CurrentLanguage);
            m_CheckHander.LoadEquipLanguage(CurrentLanguage);
            OnlanguagechangeEvent("English");
        }

        private void LoadSCADALanguage(String lang)
        {
            if (lang == "Chinese")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh");
            }
            else if (lang == "English")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            }
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);

            currentText.Text = ChangeLanguage.GetString(m_Hashtable, "currentText.Text");
            totaltimeText.Text = ChangeLanguage.GetString(m_Hashtable, "totaltimeText.Text");
            dayText.Text = ChangeLanguage.GetString(m_Hashtable, "dayText.Text");

            StripMenuItemFile.Text = ChangeLanguage.GetString(m_Hashtable, "StripMenuItemFile.Text");
            StripMenuItemEdit.Text = ChangeLanguage.GetString(m_Hashtable, "StripMenuItemEdit.Text");
            StripMenuItemCheck.Text = ChangeLanguage.GetString(m_Hashtable, "StripMenuItemCheck.Text");
            StripMenuItemParmas.Text = ChangeLanguage.GetString(m_Hashtable, "StripMenuItemParams.Text");
            StripMenuItemHelp.Text = ChangeLanguage.GetString(m_Hashtable, "StripMenuItemHelp.Text");
            版本ToolStripMenuItem.Text = ChangeLanguage.GetString(m_Hashtable, "VersionStripMenuItem.Text");

            changeSkin.Text = ChangeLanguage.GetString(m_Hashtable, "changeSkin.Text");
            changeLanguage.Text = ChangeLanguage.GetString(m_Hashtable, "changeLanguage.Text");

            中文ToolStripMenuItem.Text = ChangeLanguage.GetString(m_Hashtable, "ChineseStripMenuItem.Text");
            // 英文ToolStripMenuItem.Text = ChangeLanguage.GetString(m_Hashtable, "EnglishStripMenuItem.Text");
            toolStripMenuItem2.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem2.Text");
            toolStripMenuItem3.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem3.Text");
            toolStripMenuItem4.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem4.Text");
            toolStripMenuItem5.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem5.Text");
            toolStripMenuItem6.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem6.Text");
            toolStripMenuItem7.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem7.Text");
            toolStripMenuItem8.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem8.Text");
            toolStripMenuItem9.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem9.Text");
            toolStripMenuItem10.Text = ChangeLanguage.GetString(m_Hashtable, "toolStripMenuItem10.Text");
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
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadSkin(form, Color.Blue);
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //             this.skinEngine1.SkinFile = "DeepCyan.ssk";
            //             Localization.WriteDefaultSkin("DeepCyan");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadSkin(form, Color.LightBlue);
            }

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //this.skinEngine1.SkinFile = "DeepGreen.ssk";
            //Localization.WriteDefaultSkin("DeepGreen");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadSkin(form, Color.LightGreen);
            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            //             this.skinEngine1.SkinFile = "DeepOrange.ssk";
            //             Localization.WriteDefaultSkin("DeepOrange");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadSkin(form, Color.Orange);
            }
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            //             this.skinEngine1.SkinFile = "Midsummer.ssk";
            //             Localization.WriteDefaultSkin("Midsummer");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadSkin(form, Color.Green);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            //             this.skinEngine1.SkinFile = "MacOS.ssk";
            //             Localization.WriteDefaultSkin("MacOS");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadSkin(form, Color.SkyBlue);
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            //             this.skinEngine1.SkinFile = "Page.ssk";
            //             Localization.WriteDefaultSkin("Page");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadSkin(form, Color.SteelBlue);
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            //             this.skinEngine1.SkinFile = "Vista.ssk";
            //             Localization.WriteDefaultSkin("Vista");
            foreach (Form form in Application.OpenForms)
            {
                ChangeLanguage.LoadSkin(form, Color.LightYellow);
            }
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
            // iniapi.savedata(hisoutput.ToString());

            /*if (m_ShowRfidDataTable != null)
            {
                m_ShowRfidDataTable.AppExit();
            }*/
            //if (cncform_Ptr != IntPtr.Zero)
            //{
            //    HomeForm.SendMessage(cncform_Ptr, ClosingMsg, 0, 0);
            //}
            //if (plcform_Ptr != IntPtr.Zero)
            //{
            //    HomeForm.SendMessage(plcform_Ptr, ClosingMsg, 0, 0);
            //}
            //foreach (CNC m_cnc in cnclist)
            //{
            //    m_cnc.CNCExit();
            //}
            //plc.ClosePLC_MITSUBISHI();
            //ncCollector.CollectExit();

            LogApi.WriteLogInfo(MainForm.logHandle, (Byte)ENUM_LOG_LEVEL.LEVEL_WARN
                , "******************************Scada exit!!******************************\n");

            // cmm.cmmexit();
            LogApi.LogExit(logHandle);
            //HncApi.HNC_NetExit();
            m_Log.ExitApp();
            m_CCDdata.ExitApp();
            if (cmApi.isConnected())
            {
                cmApi.disconnect();
            }

            collectdatav2.CollectExit();
        }
        public Dictionary<int, string> Alarmss { get; set; }

        /// <summary>
        /// 和云数控的链接状态更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //         DateTime SystemStarTime = DateTime.DaysInMonth(2015,2);
        public static String SystemRuingTimes;
        private void timer_LinckCheck_Tick(object sender, EventArgs e)
        {
            language = ChangeLanguage.GetDefaultLanguage();
            if (ModbusTcp.MES_PLC_comfim_write_flage == false)
            {
                if (Linebuttomcur == 1)
                {
                    if (!linestart)//产线启动完成后  
                                   //else if (!linestarting && !linestart)//产线启动完成后
                    {

                        linestart = false;//切换产线复位按钮
                        linestarting = false;
                        linestop = true;//产线复位时可以停止，产线复位完成后只有开启产线，停止按钮才能有效
                        linereset = false;//产线开启不能复位


                        button1.BackColor = Color.Gray;
                        button3.BackColor = Color.Gray;
                        button2.BackColor = Color.LightGreen;

                        if (language == "English")
                        {

                            button1.Text = "STARTED";
                            button3.Text = "RESET";
                            button2.Text = "STOPPED";
                        }
                        else
                        {
                            button1.Text = "产线已开启";
                            button2.Text = "产线停止";
                            button3.Text = "产线复位";
                        }
                    }
                }
                else if (Linebuttomcur == 2)
                {
                    if (!linestop)//产线停止完成后
                    {

                        linestop = false;//切换产线停止按钮
                        linestoping = false;
                        linestart = true;//切换产线开启按钮
                        linereset = true;//产线开启不能复位


                        button1.BackColor = Color.LightGreen;
                        button3.BackColor = Color.LightGreen;
                        button2.BackColor = Color.Gray;

                        if (language == "English")
                        {

                            button1.Text = "START";
                            button3.Text = "RESET";
                            button2.Text = "STOP";
                        }
                        else
                        {
                            button1.Text = "产线开启";
                            button2.Text = "产线已停止";
                            button3.Text = "产线复位";
                        }
                    }
                }
                else if (Linebuttomcur == 3)
                {
                    if (!linereset)//产线复位完成后

                    //if (!linereseting && !linereset)//产线复位完成后
                    {

                        linereset = true;//切换产线复位按钮

                        linereseting = false;
                        linestart = true;//切换产线复位按钮

                        linestop = false;//产线复位时可以停止，产线复位完成后只有开启产线，停止按钮才能有效


                        button1.BackColor = Color.LightGreen;
                        button2.BackColor = Color.Gray;
                        button3.BackColor = Color.LightGreen;
                        if (language == "English")
                        {

                            button1.Text = "START";
                            button3.Text = "RESET";
                            button2.Text = "STOPPED";
                        }
                        else
                        {
                            button1.Text = "产线开启";
                            button2.Text = "产线停止";
                            button3.Text = "产线复位";
                        }
                    }
                }



            }

            //查询西门子PLC是否在线
            // if (PingTestCNC(ModbusTcp.tcpIp, 300))
            if (sptcp1.Tcpclient != null)
            {
                if (sptcp1.Tcpclient.Client != null)
                {
                    if (sptcp1.Tcpclient.Connected)
                    {
                        PLC_SIMES_ON_line = true;
                        buttonplcconnect.BackColor = Color.Gray;
                        if (language == "English")
                        {
                            labelplcstate.Text = "On-Line";
                        }
                        else
                        {
                            labelplcstate.Text = "在线";
                        }

                    }
                    else
                    {
                        plcdataclear();
                        PLC_SIMES_ON_line = false;
                        buttonplcconnect.BackColor = Color.LightGreen;

                        if (language == "English")
                        {

                            labelplcstate.Text = "Off-Line";
                        }
                        else
                        {
                            labelplcstate.Text = "离线";
                        }
                    }
                }
                else
                {
                    plcdataclear();
                    PLC_SIMES_ON_line = false;
                    buttonplcconnect.BackColor = Color.LightGreen;

                    if (language == "English")
                    {

                        labelplcstate.Text = "Off-Line";
                    }
                    else
                    {
                        labelplcstate.Text = "离线";
                    }
                }
            }
            else
            {
                plcdataclear();
                PLC_SIMES_ON_line = false;
                buttonplcconnect.BackColor = Color.LightGreen;
                if (language == "English")
                {

                    labelplcstate.Text = "Off-Line";
                }
                else
                {
                    labelplcstate.Text = "离线";
                }
            }
            //机床在线判断

            labellathe.Text = "离线";
            labelcnc.Text = "离线";
            labelrobort.Text = "离线";
            // labelplcstate.Text = "离线";


            if (MainForm.cncv2list.Count == 2)
            {
                var temp1 = cncv2list[0];
                var temp2 = cncv2list[1];
                if (temp1.IsConnected())
                {
                    SEquipmentlist[0].IsConnect = true;
                    if (temp1.cnctype == HNC.API.CNCType.CNC)
                    {
                        labelcnc.Text = "在线";
                    }
                    else if (temp1.cnctype == CNCType.Lathe)
                    {
                        labellathe.Text = "在线";
                    }
                }
                else
                {
                    SEquipmentlist[0].IsConnect = false;

                    if (temp1.cnctype == CNCType.CNC)
                    {
                        labelcnc.Text = "离线";
                    }
                    else if (temp1.cnctype == CNCType.Lathe)
                    {
                        labellathe.Text = "离线";
                    }
                }
                if (temp2.IsConnected())
                {
                    SEquipmentlist[1].IsConnect = false;
                    if (temp2.cnctype == CNCType.CNC)
                    {
                        labelcnc.Text = "在线";
                    }
                    else if (temp2.cnctype == CNCType.Lathe)
                    {
                        labellathe.Text = "在线";
                    }

                }
                else
                {
                    SEquipmentlist[1].IsConnect = false;

                    if (temp2.cnctype == CNCType.CNC)
                    {
                        labelcnc.Text = "离线";
                    }
                    else if (temp2.cnctype == CNCType.Lathe)
                    {
                        labellathe.Text = "离线";
                    }
                }

            }

            if (MainForm.cncv2list.Count == 1)
            {
                var temp1 = cncv2list[0];
                var temp2 = cncv2list[1];
                if (temp1.IsConnected())
                {
                    SEquipmentlist[0].IsConnect = true;


                    if (temp1.cnctype == CNCType.CNC)
                    {
                        labelcnc.Text = "在线";
                    }
                    else if (temp1.cnctype == CNCType.Lathe)
                    {
                        labellathe.Text = "在线";
                    }
                }
                else
                {
                    SEquipmentlist[0].IsConnect = false;

                    if (temp1.cnctype == CNCType.CNC)
                    {
                        labelcnc.Text = "离线";
                    }
                    else if (temp1.cnctype == CNCType.Lathe)
                    {
                        labellathe.Text = "离线";
                    }
                }

            }
            //机器人在线判断
            // robotconnect = MainForm.PingTestCNC("192.168.8.103", 300);//
            if (cmApi != null)
            {
                //202108robotconnect = cmApi.isConnected();
            }
            else
            {
                robotconnect = false;
            }
            if (robotconnect)
            {
                if (language == "English")
                {
                    labelrobort.Text = "On-Line";
                }
                else
                {
                    labelrobort.Text = "在线";
                }
            }
            //数据库在线判断
            if(SQLonline)
            {
                labelsqlstate.Text = "在线";
            }
            else
            {
                labelsqlstate.Text = "离线";
            }

            if (ssText.Text != DateTime.Now.ToString())
            {
                String[] timearr = (DateTime.Now - SystemStartime).ToString().Split('.');
                if (timearr.Length == 2)//一天内
                {
                    //SystemRuingTimes = "累计运行时间：0天 " + timearr[0];
                    totaldays.Text = "0";
                    timesText.Text = timearr[0] + "         ";
                }
                else if (timearr.Length == 3)//超过一天
                {
                    //SystemRuingTimes = "累计运行时间：" + timearr[0] + "天 " + timearr[1];
                    totaldays.Text = timearr[0];
                    timesText.Text = timearr[1] + "         ";
                }
                ssText.Text = DateTime.Now.ToString() + "         ";
            }


            //刷新设备状态
            // UpdataCNCStateNum();
            UpdataCNCStateNum(true);
            Updatamagstate();

            //当前有报警信息时，高亮显示报警选项标签
            if (MainForm.cncv2list.Count > 0)
            {
                alarmshow.BackColor = Color.White;
                alarmshow.Text = "";
                foreach (var cncv2temp in cncv2list)
                {
                    if (cncv2temp.Alarms != null)
                    {
                        if (cncv2temp.Alarms.Count > 0)  //目前只有一台机床  2017.6.9  hxb
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
                            alarmshow.Text = cncv2temp.Alarms.First().Value.ToString() + "!";
                        }
                    }

                }

            }
            timer_LinckCheck.Enabled = true;

            saverackfunstate(RackstateFilePath);

        }


        private string getvalueformstring(string line, string indexstring)
        {
            if (line == "")
            {
                return "null";
            }
            string temp = "";
            //取得=后面，前面的字符
            int index1 = line.IndexOf(indexstring);
            if (index1 < 0)
            {
                return "null";
            }
            int index2 = 0;
            index1 = index1 + indexstring.Length;
            index2 = index1;
            string str = line.Substring(index2, 1);
            int length = 0;
            while (str != "," && str != ";")
            {
                index2++;
                length++;
                str = line.Substring(index2, 1);
            }
            return temp = line.Substring(index1, length);
        }
        //rack=0,fun1=-1,fun2=-1;
        /// <summary>
        /// 
        /// </summary>
        private int initrackfunstate(string path)
        {
            try
            {
                File.Delete(RackstateFilePath);
                File.Copy(RackstateFilebakPath, RackstateFilePath, true);
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                string rack = "";
                string fun1 = "";
                string fun2 = "";
                string ordered = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();

                while (line != null && line != "")
                {
                    rack = getvalueformstring(line, "rack=");
                    fun1 = getvalueformstring(line, "fun1=");
                    fun2 = getvalueformstring(line, "fun2=");
                    ordered = getvalueformstring(line, "ordered=");
                    if (rack == "null" || fun1 == "null" || fun2 == "null" || ordered == "null")
                    {
                        line = sr.ReadLine();

                    }
                    else
                    {
                        int magno = Convert.ToInt32(rack);
                        int ifun1 = Convert.ToInt32(fun1);
                        int ifun2 = Convert.ToInt32(fun2);
                        int iordered = Convert.ToInt32(ordered);
                        magprocesss1tate[magno - 1] = ifun1;
                        magprocesss2tate[magno - 1] = ifun2;
                        magisordered[magno - 1] = iordered;
                        line = sr.ReadLine();
                        ii++;
                    }

                }

                sr.Close();
                aFile.Close();
                return 1;
            }
            catch (IOException e)
            {
                return 0;
            }
        }
        private int Reinitrackfunstate(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                string rack = "";
                string fun1 = "";
                string fun2 = "";
                string ordered = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();

                while (line != null && line != "")
                {
                    rack = getvalueformstring(line, "rack=");
                    fun1 = getvalueformstring(line, "fun1=");
                    fun2 = getvalueformstring(line, "fun2=");
                    ordered = getvalueformstring(line, "ordered=");
                    if (rack == "null" || fun1 == "null" || fun2 == "null" || ordered == "null")
                    {
                        line = sr.ReadLine();

                    }
                    else
                    {
                        int magno = Convert.ToInt32(rack);
                        int ifun1 = Convert.ToInt32(fun1);
                        int ifun2 = Convert.ToInt32(fun2);
                        int iordered = Convert.ToInt32(ordered);
                        magprocesss1tate[magno - 1] = ifun1;
                        magprocesss2tate[magno - 1] = ifun2;
                        magisordered[magno - 1] = iordered;
                        line = sr.ReadLine();
                        ii++;
                    }

                }

                sr.Close();
                aFile.Close();
                return 1;
            }
            catch (IOException e)
            {
                return 0;
            }
        }

        private bool saverackfunstate(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Create);
                StreamWriter sr = new StreamWriter(aFile);
                int jj = 0;
                string orderdatas = "";
                string rack = "";
                string fun1 = "";
                string fun2 = "";
                string ordered = "";
                for (int ii = 1; ii < 31; ii++)
                {
                    rack = ii.ToString();
                    fun1 = magprocesss1tate[ii - 1].ToString();
                    fun2 = magprocesss2tate[ii - 1].ToString();
                    ordered = magisordered[ii - 1].ToString();
                    orderdatas = "rack=" + rack;
                    orderdatas = orderdatas + ",fun1=" + fun1;
                    orderdatas = orderdatas + ",fun2=" + fun2;
                    orderdatas = orderdatas + ",ordered=" + ordered;
                    if (orderdatas == "")
                    {
                        ;
                    }
                    orderdatas = orderdatas + ";";
                    if (orderdatas.Length > 20)
                    {

                        sr.WriteLine(orderdatas);
                    }
                }
                sr.Close();
                aFile.Close();
                return true;
            }
            catch (IOException e)
            {
                return false;
            }
        }
        private void Updatamagstate()
        {
            int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型          
            int maglength = (int)ModbusTcp.MagLength;
            int magiostateadress = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state_1;
            bool maginstate = false;
            int magstatei = 0;
            int temp = 0;
            if (!GetInitRackFinish)
            {

                return;
            }

            if (!InitRackFinish)
            {

                CheckInitRackState();
            }
            if (!RackForm.magstatesyncflag)
            {

                Rackstatesave(RackdataFilePath);
                return;
            }
            //ModbusTcp.DataMoubus[magiostateadress] = 48059;
            //ModbusTcp.DataMoubus[magiostateadress+1] = 48059;
            //查询料架上的传感器，将无料刷为有料 ;
            //sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.req, 0, 0, (int)SCADA.ModbusTcp.DataConfigArr.Mag_state_1, 2);//请求61、62号寄存器存储内容是料位有无料信息
            //sptcp1.ReceiveData((byte)SCADA.ModbusTcp.Func_Code.req, 0, 0, (int)SCADA.ModbusTcp.DataConfigArr.Mag_state_1, 2);
            for (int i = 0; i < 30; i++)
            {
                //高科20181014
                //magstatei = magstatestart + maglength * i;

                //int magstartnum = magiostateadress;
                //if (i <= 16)
                //{

                //        temp = 1 << i ;
                //      //  temp = 1 << (i -+8);
                //        if ((ModbusTcp.DataMoubus[magstartnum] & temp) == temp)
                //        {
                //            maginstate = true;
                //        }
                //        else
                //            maginstate = false;

                //}
                //else
                //{

                //    if (i < 24)
                //    {
                //        temp = 1 <<(i-16);
                //        if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                //        {
                //            maginstate = true;
                //        }
                //        else
                //            maginstate = false;
                //    }
                //    else
                //    {
                //        temp = 1 << (i - 16);
                //        if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                //        {
                //            maginstate = true;
                //        }
                //        else
                //            maginstate = false;
                //    }
                //    if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                //    {
                //        maginstate = true;
                //    }
                //    else
                //        maginstate = false;
                //}


                //if (ModbusTcp.DataMoubus[magstatei] == 0 && maginstate)
                //{
                //    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statewait;
                //}


                magstatei = magstatestart + maglength * i;

                int magstartnum = magiostateadress;
                if (i <= 15)
                {
                    if (i < 8)
                    {
                        temp = 1 << (8 + i);
                        if ((ModbusTcp.DataMoubus[magstartnum] & temp) == temp)
                        {
                            maginstate = true;
                        }
                        else
                            maginstate = false;
                    }
                    else
                    {
                        temp = 1 << (i - 8);
                        if ((ModbusTcp.DataMoubus[magstartnum] & temp) == temp)
                        {
                            maginstate = true;
                        }
                        else
                            maginstate = false;
                    }
                }
                else
                {

                    if (i < 24)
                    {
                        temp = 1 << (i - 8);
                        if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                        {
                            maginstate = true;
                        }
                        else
                            maginstate = false;
                    }
                    else
                    {
                        temp = 1 << (i - 24);
                        if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                        {
                            maginstate = true;
                        }
                        else
                            maginstate = false;
                    }
                    if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                    {
                        maginstate = true;
                    }
                    else
                        maginstate = false;
                }

                if (ModbusTcp.DataMoubus[magstatei] == 0 && maginstate)
                {
                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statewait;
                }
                if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statewait && maginstate == false && MainForm.magisordered[i] == 0)
                {
                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statenull;
                }

            }
            //查询料架上的传感器，将有料刷为加工中；

            //查询料架上的传感器，将加工中刷为合格或者不合格；在检测程序中置位检测结果

            //
            Rackstatesave(RackdataFilePath);
        }
        private int Rackstatesave(string path)
        {
            int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型  
            int magstatei = 0;
            int[] magstate = new int[30];
            int maglength = (int)ModbusTcp.MagLength;
            for (int i = 0; i < 30; i++)
            {
                magstatei = magstatestart + maglength * i;
                magstate[i] = ModbusTcp.DataMoubus[magstatei];//获取料仓状态
            }

            try
            {
                FileStream aFile = new FileStream(path, FileMode.OpenOrCreate);
                StreamWriter sr = new StreamWriter(aFile);
                int jj = 0;
                string magnotemp;
                string magstateline;
                for (jj = 1; jj < 30; jj++)
                {
                    magnotemp = jj.ToString();
                    magstateline = magnotemp + "MagState=" + magstate[jj - 1] + ";";
                    sr.WriteLine(magstateline);
                }
                sr.Close();
                aFile.Close();
                return 1;
            }
            catch (IOException e)
            {
                return 0;
            }
        }
        //private void UpdataCNCStateNum()
        //{
        //    int cncruing = 0;
        //    int cnckongxian = 0;
        //    int cncbaojing = 0;
        //    int cnclixian = 0;

        //    if (MainForm.cnclist != null && MainForm.cnclist.Count > 0)
        //    {
        //        cnclixian = MainForm.cnclist.Count;
        //        for (int ii = 0; ii < MainForm.cnclist.Count; ii++)
        //        {
        //            if (MainForm.cnclist[ii] != null && MainForm.cnclist[ii].isConnected())
        //            {
        //                LineDevice.CNC.CNCState CNCStatei = LineDevice.CNC.CNCState.DISCON;
        //                MainForm.cnclist[ii].Checkcnc_state(ref CNCStatei);
        //                if ((int)LineDevice.CNC.CNCState.IDLE == (int)CNCStatei)
        //                {
        //                    if (ii == 0)
        //                    {
        //                        Slathedata.State = EnumLatheState.Running;
        //                        //(int)LineDevice.CNC.CNCState.IDLE ;
        //                    }
        //                    if (ii == 1)
        //                    {

        //                        SCNCdata.State = EnumMachiningCenterState.Running;
        //                        //SCNCdata.State = (int)LineDevice.CNC.CNCState.IDLE;
        //                    }
        //                    cnckongxian++;
        //                }
        //                else if ((int)LineDevice.CNC.CNCState.RUNING == (int)CNCStatei)
        //                {
        //                    cncruing++;
        //                    if (ii == 0)
        //                    {
        //                        Slathedata.State = EnumLatheState.Running; //(int)LineDevice.CNC.CNCState.RUNING;
        //                    }
        //                    if (ii == 1)
        //                    {
        //                        SCNCdata.State = EnumMachiningCenterState.Running;// (int)LineDevice.CNC.CNCState.RUNING;
        //                    }
        //                }
        //                else if ((int)LineDevice.CNC.CNCState.ALARM == (int)CNCStatei)
        //                {
        //                    cncbaojing++;
        //                    if (ii == 0)
        //                    {
        //                        Slathedata.State = EnumLatheState.Running; //(int)LineDevice.CNC.CNCState.ALARM;
        //                    }
        //                    if (ii == 1)
        //                    {
        //                        SCNCdata.State = EnumMachiningCenterState.Running; //(int)LineDevice.CNC.CNCState.ALARM;
        //                    }
        //                }
        //                if (ii == 0)
        //                {
        //                    Slathedata.State = EnumLatheState.Running; //(int)LineDevice.CNC.CNCState.DISCON;
        //                }
        //                if (ii == 1)
        //                {
        //                    SCNCdata.State = EnumMachiningCenterState.Running;// (int)LineDevice.CNC.CNCState.DISCON;
        //                }
        //                cnclixian--;
        //            }
        //        }
        //    }
        //    label_CNCRuningNum.Text = /*"运行数量：" + */cncruing.ToString();
        //    label_CNCkongxianNum.Text = /*"空闲数量：" + */cnckongxian.ToString();
        //    label_CNCbaojingNum.Text = /*"报警数量：" + */cncbaojing.ToString();
        //    label_CNCLiXianNum.Text = /*"离线数量：" + */cnclixian.ToString();
        //}

        private void UpdataCNCStateNum(bool t = true)
        {
            int cncruing = 0;
            int cnckongxian = 0;
            int cncbaojing = 0;
            int cnclixian = 0;

            if (MainForm.cncv2list != null && MainForm.cncv2list.Count > 0)
            {
                cnclixian = MainForm.cncv2list.Count;
                for (int ii = 0; ii < MainForm.cncv2list.Count; ii++)
                {
                    if (MainForm.cncv2list[ii].IsConnected())
                    {
                        if (cncv2list[ii].cnctype == CNCType.CNC)
                        {
                            if (cncv2list[ii].EquipmentState == "空闲")
                            {
                                SCNCdata.State = EnumMachiningCenterState.Idle;
                                cnckongxian++;
                            }
                            else if (cncv2list[ii].EquipmentState == "运行")
                            {
                                SCNCdata.State = EnumMachiningCenterState.Running;
                                cncruing++;
                            }
                            else if (cncv2list[ii].EquipmentState == "急停")
                            {
                                SCNCdata.State = EnumMachiningCenterState.Error;
                                cncbaojing++;

                            }
                            else if (cncv2list[ii].EquipmentState == "复位")
                            {
                                SCNCdata.State = EnumMachiningCenterState.Running;
                                cncruing++;
                            }
                        }
                        if (cncv2list[ii].cnctype == CNCType.Lathe)
                        {
                            if (cncv2list[ii].EquipmentState == "空闲")
                            {
                                Slathedata.State = EnumLatheState.Idle;
                                cnckongxian++;
                            }
                            else if (cncv2list[ii].EquipmentState == "运行")
                            {
                                Slathedata.State = EnumLatheState.Running;
                                cncruing++;
                            }
                            else if (cncv2list[ii].EquipmentState == "急停")
                            {
                                Slathedata.State = EnumLatheState.Error;
                                cncbaojing++;

                            }
                            else if (cncv2list[ii].EquipmentState == "复位")
                            {
                                Slathedata.State = EnumLatheState.Running;
                                cncruing++;
                            }
                        }
                        cnclixian--;
                    }
                    else
                    {
                        if (cncv2list[ii].cnctype == CNCType.CNC)
                        {
                            SCNCdata.State = EnumMachiningCenterState.Offline;
                            cnclixian++;
                        }
                        else if (cncv2list[ii].cnctype == CNCType.Lathe)
                        {
                            Slathedata.State = EnumLatheState.Offline;
                            cnclixian++;
                        }
                    }

                }
            }
            label_CNCRuningNum.Text = /*"运行数量：" + */cncruing.ToString();
            label_CNCkongxianNum.Text = /*"空闲数量：" + */cnckongxian.ToString();
            label_CNCbaojingNum.Text = /*"报警数量：" + */cncbaojing.ToString();
            label_CNCLiXianNum.Text = /*"离线数量：" + */cnclixian.ToString();
        }


        private void alarmshow_Click_1(object sender, EventArgs e)
        {
            if (alarmshow.IsLink == true)
            {
                //PlcForm.plcindex = 2;
                tabPagemain.SelectedIndex = 4;
            }
        }


        void InitPlcData()
        {
            sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 40, 1, 0);// 清除plc信息
            sptcp1.ReceiveData();
        }
        private void timermodbus_Tick(object sender, EventArgs e)
        {


        }



        private void buttonplcconnect_Click(object sender, EventArgs e)
        {
            if (sptcp1.Tcpclient != null)
            {
                if (sptcp1.Tcpclient.Client != null)
                {
                    if (sptcp1.Tcpclient.Connected)
                    {
                        return;
                    }
                }
            }

            ModbusTcp.connectcount = 0;
            plcdataclear();
            buttonplcconnect.BackColor = Color.Gray;
        }
        private void MESToPLCcomfirmfun()
        {
            string temp = "";
            if (ModbusTcp.MES_PLC_comfim_write_flage == true)
            {
                ModbusTcp.MES_PLC_comfim_write_count++;
                if (timer_plc.Interval * ModbusTcp.MES_PLC_comfim_write_count > 60 * 1000 * 2)//指令下达交互时间2min无回应，报错
                {

                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComStartDevice)
                    {
                        temp = "复位超时,请复位PLC！";

                        linereset = false;//切换产线复位按钮
                        linereseting = false;


                        ModbusTcp.MES_PLC_comfim_write_count = 0;
                        ModbusTcp.MES_PLC_comfim_write_flage = false;
                        plcgetconfim = false;

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 1, 1, 0);//给plc写订单信息
                        MainForm.sptcp1.ReceiveData();
                        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 1, 1, 0);//给plc写订单信息
                        MainForm.sptcp1.ReceiveData();
                        MessageBox.Show(temp);
                        return;
                    }
                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComStartSys)
                    {
                        temp = "启动超时,请复位PLC！";

                        linestart = true;//切换产线复位按钮
                        linestarting = false;


                        ModbusTcp.MES_PLC_comfim_write_count = 0;
                        ModbusTcp.MES_PLC_comfim_write_flage = false;
                        plcgetconfim = false;

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 1, 1, 0);//给plc写订单信息
                        MainForm.sptcp1.ReceiveData();
                        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 1, 1, 0);//给plc写订单信息
                        MainForm.sptcp1.ReceiveData();
                        MessageBox.Show(temp);
                        return;
                    }
                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComStopSys)
                    {
                        temp = "停止超时,请复位PLC！";

                        linestop = true;//切换产线复位按钮
                        linestoping = false;


                        ModbusTcp.MES_PLC_comfim_write_count = 0;
                        ModbusTcp.MES_PLC_comfim_write_flage = false;
                        plcgetconfim = false;

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 1, 1, 0);//给plc写订单信息
                        MainForm.sptcp1.ReceiveData();
                        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 1, 1, 0);//给plc写订单信息
                        MainForm.sptcp1.ReceiveData();
                        MessageBox.Show(temp);
                        return;
                    }
                }
                if (timer_plc.Interval * ModbusTcp.MES_PLC_comfim_write_count > 60 * 1000 * 10)//指令下达交互时间2min无回应，报错
                {
                    ModbusTcp.MES_PLC_comfim_write_count = 0;
                    ModbusTcp.MES_PLC_comfim_write_flage = false;
                    plcgetconfim = false;

                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComWriteRfid && !RackForm.rfidwriteflag)
                    {
                        temp = "写入超时,请复位PLC！";
                        RackForm.rfidwriteflag = true;
                        return;
                    }
                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComWriteRfid && !RackForm.Inventoryflag)
                    {
                        temp = "盘点超时,请复位PLC！";
                        RackForm.Inventoryflag = true;
                        return;
                    }
                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComReadRfid)
                    {
                        temp = "HMI写入,请复位PLC！";
                        RackForm.rfidreadflag = true;
                    }

                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] = 0;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 1, 1, 0);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 1, 1, 0);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();
                    MessageBox.Show(temp);
                    return;
                }


                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 1, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();
                Thread.Sleep(2);
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 1);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();
                Thread.Sleep(2);

                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] == ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm])
                {
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                    plcgetconfim = true;
                }
                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] == 0
                    && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == 0
                    && plcgetconfim == true)
                {
                    plcgetconfim = false;
                    ModbusTcp.MES_PLC_comfim_write_flage = false;
                    ModbusTcp.MES_PLC_comfim_write_count = 0;
                    if (!RackForm.rfidwriteflag)
                    {
                        RackForm.rfidwriteflag = true;

                        // RackForm.setrfidflag = true;//写rfid置位料仓信息整体更新
                    }
                    if (!RackForm.rfidreadflag)
                    {
                        RackForm.rfidreadflag = true;
                        RackForm.getrfidflag = true;//度rfid，置位料仓信息整体更新
                    }
                    if (!RackForm.Inventoryflag)
                    {
                        RackForm.Inventoryflag = true;
                        RackForm.getrfidflag = true;//度rfid，置位料仓信息整体更新
                    }

                }
            }
            else
                return;
        }

        private void timer_plc_fun(object sender, EventArgs e)
        {
          
            if (!Orderinitflage)
            {
                //请求料架有无料信息
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm, 10);//查询plc_mes的命令
                MainForm.sptcp1.ReceiveData();
                //MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Meterial, 1, 1, 0);//给plc写订单信息

                MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.req, 1, 0, (int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_status, 28);//请求61、62号寄存器存储内容是料位有无料信息      
                MainForm.sptcp1.ReceiveData();

                MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.writereg, (int)SCADA.ModbusTcp.DataConfigArr.p_MeterValue1, 54, 1, 0);//写机器人位置
                MainForm.sptcp1.ReceiveData();
                //   MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.writereg, (int)SCADA.ModbusTcp.DataConfigArr.p_MeterValue1, 15, 1, 0);//请求61、62号寄存器存储内容是料位有无料信息      
                //if (MeterForm.renewbiaodingfalge)
                //{
                //   // MeterForm.renewbiaodingfalge = false;
                //    MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.writereg, (int)SCADA.ModbusTcp.DataConfigArr.p_MeterValue1, 15, 1, 0);//请求61、62号寄存器存储内容是料位有无料信息      

                //}
                if (RackForm.meterialchange)
                {
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Meterial, 1, 1, 0);//给plc写订单信息
                                                                                                                                 //RackForm.meterialchange = false;
                }//材质修改
                MESToPLCcomfirmfun();

                OrderForm1.RackmessageSync();//铣床测量信号处理
                                             //timer_plc.Enabled = false;
            }
            else
            {
                timer_plc.Enabled = false;
            }

        }

        /// <summary>
        ///       清除plc交互数据去
        /// </summary>
        private void plcdataclear()
        {
            for (int i = 0; i < 60; i++)
            {
                ModbusTcp.DataMoubus[i] = 0;
            }
        }
        public static bool renewww = false;
        private void timersql_Tick(object sender, EventArgs e)
        {
            return;
            if (sqlconnectflage == false)
            {
                if (DbHelper.SQL.State == ConnectionState.Closed)
                {
                    try
                    {
                        DbHelper.SQL.Open();
                        SQLonline = true;
                        if (language == "English")
                        {
                            labelsqlstate.Text = "On-Line";
                        }
                        else
                        {
                            labelsqlstate.Text = "在线";
                        }
                        sqlconnectcount = 0;
                    }
                    catch
                    {

                        SQLonline = false;
                        if (language == "English")
                        {
                            labelsqlstate.Text = "Off-Line";
                        }
                        else
                        {
                            labelsqlstate.Text = "离线";
                        }
                        sqlconnectcount++;
                        if (sqlconnectcount > 3)
                        {
                            sqlconnectflage = true;

                            buttonsqlconnect.BackColor = Color.SpringGreen;
                        }
                    }
                }
            }

            if (SQLonline)
            {
                DbHelper.SQL.Close();
                try
                {


                    renewWorkpieceCategory();
                    renewStoragebin();
                    //renewLathedata();
                    //renewCNCdata();
                    foreach (var cnctemp in cncv2list)
                    {
                        if (cnctemp.cnctype == CNCType.CNC)
                        {
                            renewCNCdata(cnctemp);
                            if (renewcncsql)
                            {

                                renewToolCNC(cnctemp);
                            }

                        }
                        else if (cnctemp.cnctype == CNCType.Lathe)
                        {
                            renewLathedata(cnctemp);
                            if (renewlathesql)
                            {
                                renewToolLathe(cnctemp);
                            }
                        }
                    }
                    renewSRobot(true);
             

                }
                catch (Exception ex)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("SQL operate error!");
                    }
                    else
                    {

                        MessageBox.Show("数据库操作错误!");
                    }
                }
            }
        }
        private static bool connectrobot = false;
        private void UpdataRobotData()
        {
            bool robotconnect = PingTestCNC(MainForm.ROBORTAddress, 300);//
            if (!robotconnect)
            {
                return;
            }

            if (!connectrobot)
            {
                uint ret = 0;
                //robotData = new int[length];
                ret = HSCAPI.HSCAPIDLL_NetInit();//网络初始化
                ret = HSCAPI.HSCAPIDLL_NetConnect(MainForm.ROBORTAddress, 5004);//网络连接
                ret = HSCAPI.HSCAPIDLL_NetIsConnect();//检查网络状态;
                if (ret != 0)
                {
                    return;
                }
                robotconnect = true;
                ret = HSCAPI.HSCAPIDLL_NetIsConnect();
                if (ret != 0)
                {
                    return;
                }

                //获取工作模式信息
                //HSCAPI.WorkMode mode = HSCAPI.WorkMode.WORK_MODE_NONE;
                //HSCAPI.HSCAPIDLL_GetWorkMode(ref mode);
                //robortdate.WorkMode = (int)mode;

                ////获取工作状态信息和程序名称信息
                //HSCAPI.TaskStatus statu = new HSCAPI.TaskStatus();
                //HSCAPI.HSCAPIDLL_GetUserProgStatus(ref statu);
                //robortdate.WorkState = (int)statu.state;
                //robortdate.ProgName = statu.progName;
                //robortdate.CurFileName = statu.currFileName;


                //获取轴位置信息
                Double[] curJointPos6 = new Double[6];
                ret = HSCAPI.HSCAPIDLL_GetJointPos6(ref curJointPos6);//获取当前关节角坐标
                var pos1 = curJointPos6[0];
                var pos2 = curJointPos6[1];
                var pos3 = curJointPos6[2];
                var pos4 = curJointPos6[3];
                var pos5 = curJointPos6[4];
                var pos6 = curJointPos6[5];
                var pos7 = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint7_coor];
                if (pos7 > 16000)
                {
                    pos7 = (~pos7) & 0xffff;
                    pos7 = 0 - (pos7 + 1);
                }
                var spos7 = pos7.ToString("F3");


                var spos1 = pos1.ToString("F3");
                var spos2 = pos2.ToString("F3");
                var spos3 = pos3.ToString("F3");
                var spos4 = pos4.ToString("F3");
                var spos5 = pos5.ToString("F3");
                var spos6 = pos6.ToString("F3");
                //var spos7= pos7.ToString("F3");
                SRobortdata.SiteJ1 = Convert.ToDouble(spos1);
                SRobortdata.SiteJ2 = Convert.ToDouble(spos2);
                SRobortdata.SiteJ3 = Convert.ToDouble(spos3);
                SRobortdata.SiteJ4 = Convert.ToDouble(spos4);
                SRobortdata.SiteJ5 = Convert.ToDouble(spos5);
                SRobortdata.SiteJ6 = Convert.ToDouble(spos6);
                SRobortdata.SiteJ7 = Convert.ToDouble(spos7);
                double[] posarry = new double[7];
                posarry[0] = SRobortdata.SiteJ1;
                posarry[1] = SRobortdata.SiteJ2;
                posarry[2] = SRobortdata.SiteJ3;
                posarry[3] = SRobortdata.SiteJ4;
                posarry[4] = SRobortdata.SiteJ5;
                posarry[5] = SRobortdata.SiteJ6;
                posarry[6] = SRobortdata.SiteJ7;
                for (int i = 0; i < 7; i++)
                {
                    double tempd = Convert.ToDouble(posarry[i]);
                    var temps = tempd.ToString("F3");
                    if (temps != "null")
                    {

                        int index = temps.IndexOf(".");
                        int flage = 1;
                        if (Convert.ToDouble(temps) < 0)
                        {
                            flage = -1;
                        }
                        string refvalue1 = temps.Substring(0, index);//整数部分
                        string refvalue2 = temps.Substring(index + 1);//小数部分
                        if (flage == -1)
                        {

                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + 3 * i] = 1;
                        }
                        else
                        {
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + 3 * i] = 0;
                        }

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 1] = Convert.ToInt32(refvalue1);
                        if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 1] < 0)
                        {
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 1] = (-1) * ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 1];
                        }
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 2] = Convert.ToInt32(refvalue2);
                    }
                }
                //更新mes to plc 信息
            }

        }
        private void UpdataRobotData(bool t = true)
        {
            if (!connectrobot)
            {
                var robotconnect = cmApi.connect(MainForm.ROBORTAddress, 23234);//
                if (robotconnect != 0)
                {
                    SCADA.LogData.EventHandlerSendParm SendParm = new SCADA.LogData.EventHandlerSendParm();
                    SendParm.Node1NameIndex = (int)SCADA.LogData.Node1Name.Equipment_ROBOT;
                    SendParm.LevelIndex = (int)SCADA.LogData.Node2Level.MESSAGE;
                    SendParm.EventID = ((int)SCADA.LogData.Node2Level.MESSAGE).ToString();
                    SendParm.Keywords = "机器人采集器开启";
                    SendParm.EventData = MainForm.ROBORTAddress + "：网络连接成功";
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, null, null);

                    return;
                }
                else
                {
                    SCADA.LogData.EventHandlerSendParm SendParm = new SCADA.LogData.EventHandlerSendParm();
                    SendParm.Node1NameIndex = (int)SCADA.LogData.Node1Name.Equipment_ROBOT;
                    SendParm.LevelIndex = (int)SCADA.LogData.Node2Level.MESSAGE;
                    SendParm.EventID = ((int)SCADA.LogData.Node2Level.MESSAGE).ToString();
                    SendParm.Keywords = "机器人采集器关闭";
                    SendParm.EventData = MainForm.ROBORTAddress + "：网络连接失败";
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, null, null);

                    return;
                }
            }

            var curJointPos6 = new List<double>();
            var pos1 = 0.000;
            var pos2 = 0.000;
            var pos3 = 0.000;
            var pos4 = 0.000;
            var pos5 = 0.000;
            var pos6 = 0.000;
            int pos7 = 0;
            if (!cmApi.isConnected())
            {
                cmApi.disconnect();
                connectrobot = false;

            }
            else
            {
                proMot.getJntData(6, ref curJointPos6);
                pos1 = curJointPos6[0];
                pos2 = curJointPos6[1];
                pos3 = curJointPos6[2];
                pos4 = curJointPos6[3];
                pos5 = curJointPos6[4];
                pos6 = curJointPos6[5];
                pos7 = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint7_coor];
                if (pos7 > 16000)
                {
                    pos7 = (~pos7) & 0xffff;
                    pos7 = 0 - (pos7 + 1);
                }


            }

            //获取轴位置信息
            //获取轴位置信息

            //  ret = HSCAPI.HSCAPIDLL_GetJointPos6(ref curJointPos6);//获取当前关节角坐标



            //var spos7= pos7.ToString("F3");
            SRobortdata.SiteJ1 = pos1;
            SRobortdata.SiteJ2 = pos2;
            SRobortdata.SiteJ3 = pos3;
            SRobortdata.SiteJ4 = pos4;
            SRobortdata.SiteJ5 = pos5;
            SRobortdata.SiteJ6 = pos6;
            SRobortdata.SiteJ7 = pos7;
            double[] posarry = new double[7];
            posarry[0] = SRobortdata.SiteJ1;
            posarry[1] = SRobortdata.SiteJ2;
            posarry[2] = SRobortdata.SiteJ3;
            posarry[3] = SRobortdata.SiteJ4;
            posarry[4] = SRobortdata.SiteJ5;
            posarry[5] = SRobortdata.SiteJ6;
            posarry[6] = SRobortdata.SiteJ7;

            for (int i = 0; i < 7; i++)
            {
                double tempd = Convert.ToDouble(posarry[i]);
                var temps = tempd.ToString("F3");
                var index = 0;
                string refvalue1 = "";
                string refvalue2 = "";
                if (temps != "null")
                {

                    index = temps.IndexOf(".");
                    if (index == -1)
                    {
                        index = temps.Length;
                        refvalue1 = temps;//整数部分
                        refvalue2 = "0";//小数部分

                    }
                    else
                    {
                        refvalue1 = temps.Substring(0, index);//整数部分
                        refvalue2 = temps.Substring(index + 1);//小数部分
                    }
                }
                else
                {
                    refvalue1 = "0";
                    refvalue2 = "0";
                }



                int flage = 1;
                if (Convert.ToDouble(temps) < 0)
                {
                    flage = -1;
                }
                if (flage == -1)
                {

                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + 3 * i] = 1;
                }
                else
                {
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + 3 * i] = 0;
                }

                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 1] = Convert.ToInt32(refvalue1);
                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 1] < 0)
                {
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 1] = (-1) * ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 1];
                }
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_Jpos + i * 3 + 2] = Convert.ToInt32(refvalue2);
            }
        }

        private void renewLathedata(CNCV2 cncv2)
        {
            if (cncv2.EquipmentState == "离线")
            {
                return;
            }
            int cnc1state = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.CNC1_Door_state];//加工中心

            if ((cnc1state & 0x0002) == 0x0002)//CNC_Door_Open	= 0x0002	,//	加工中心自动门打开(0未打开1打开）
            {
                Slathedata.Door = EnumLatheDoorState.Open;
            }
            else
            {
                Slathedata.Door = EnumLatheDoorState.Closed;
            }
            if ((cnc1state & 0x0004) == 0x0004)//L_Chuck_state	= 0x0004	,//	车床卡盘状态(0松开1夹紧)
            {
                Slathedata.Chuck = EnumLatheChuckState.Tight;
            }
            else
            {

                Slathedata.Chuck = EnumLatheChuckState.Loose;
            }
            string Fspeeds = cncv2.SpeedRate.ToString("F1");
            string Spdlspeeds = cncv2.SpindleSpeed.ToString("F1");

            Slathedata.SpindleSpeed = Convert.ToDouble(Spdlspeeds);
            Slathedata.Feedrate = Convert.ToDouble(Fspeeds);
            Slathedata.WorkpieceProgram = cncv2.GProgram;
            Slathedata.instructionsCount = cncv2.InstructionsCount;
            Slathedata.ToolSN = cncv2.InstructionsTool;
            Slathedata.SizeX = cncv2.SiteX;
            Slathedata.SizeZ = cncv2.SiteZ;
            Slathedata.Load_Current_C = cncv2.LoadC;
            Slathedata.Load_Current_X = cncv2.LoadX;
            Slathedata.Load_Current_Z = cncv2.LoadZ;

            //var temp1 = DbHelper.Get<StorageBin>(new { SN = cnclist[0].MagNo });
            var temp1 = DbHelper.Get<StorageBin>(new { SN = 3 });
            if (temp1 == null)
            {
                return;
            }
            else
            {
                Slathedata.StorageBinId = temp1.Id;

                var temp2 = DbHelper.Get<Lathe>(new { });
                if (temp2 == null)
                {
                    DbHelper.Insert(new Lathe
                    {
                        Door = Slathedata.Door,
                        Chuck = Slathedata.Chuck,
                        Feedrate = Slathedata.Feedrate,
                        InstructionsCount = Slathedata.instructionsCount,
                        WorkpieceProgram = Slathedata.WorkpieceProgram,
                        SiteX = Slathedata.SizeX,
                        SiteZ = Slathedata.SizeZ,
                        Load_Current_C = Slathedata.Load_Current_C,
                        Load_Current_X = Slathedata.Load_Current_X,
                        Load_Current_Z = Slathedata.Load_Current_Z,
                        // StorageBinId = SCNCdata.StorageBinId,
                        StorageBinId = Slathedata.StorageBinId,
                        SpindleSpeed = Slathedata.SpindleSpeed,
                        State = Slathedata.State,
                    });
                }

                else
                {
                    temp2.Door = Slathedata.Door;
                    temp2.Chuck = Slathedata.Chuck;
                    temp2.Feedrate = Slathedata.Feedrate;
                    temp2.InstructionsCount = Slathedata.instructionsCount;
                    temp2.WorkpieceProgram = Slathedata.WorkpieceProgram;
                    temp2.SiteX = Slathedata.SizeX;
                    temp2.SiteZ = Slathedata.SizeZ;
                    temp2.Load_Current_C = Slathedata.Load_Current_C;
                    temp2.Load_Current_X = Slathedata.Load_Current_X;
                    temp2.Load_Current_Z = Slathedata.Load_Current_Z;
                    // StorageBinId = SCNCdata.StorageBinId,
                    temp2.StorageBinId = Slathedata.StorageBinId;
                    temp2.SpindleSpeed = Slathedata.SpindleSpeed;
                    temp2.State = Slathedata.State;

                    DbHelper.Update(temp2);
                }

            }



        }
        private void renewCNCdata(CNCV2 cncv2)
        {

            if (cncv2.EquipmentState == "离线")
            {
                return;
            }

            //  GetInsideMeter();
            int cnc1state = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.CNC2_Door_state];//加工中心
            if ((cnc1state & 0x0002) == 0x0002)//CNC_Door_Open	= 0x0002	,//	加工中心自动门打开(0未打开1打开）
            {
                SCNCdata.Door = EnumMachiningCenterDoorState.Open;
            }
            else
            {
                SCNCdata.Door = EnumMachiningCenterDoorState.Closed; ;
            }
            if ((cnc1state & 0x0004) == 0x0004)//L_Chuck_state	= 0x0004	,//	车床卡盘状态(0松开1夹紧)
            {
                SCNCdata.Chuck = EnumMachiningCenterChuckState.Tight;
            }
            else
            {
                SCNCdata.Chuck = EnumMachiningCenterChuckState.Loose;
            }
            if ((cnc1state & 0x0008) == 0x0008)//L_Chuck_state	= 0x0004	,//	车床卡盘状态(0松开1夹紧)
            {
                SCNCdata.Chuck2 = EnumMachiningCenterChuckState.Tight;
            }
            else
            {
                SCNCdata.Chuck2 = EnumMachiningCenterChuckState.Loose;
            }
            string Fspeeds = cncv2.SpeedRate.ToString("F1");
            string Spdlspeeds = cncv2.SpindleSpeed.ToString("F1");

            SCNCdata.SpindleSpeed = Convert.ToDouble(Spdlspeeds);
            SCNCdata.Feedrate = Convert.ToDouble(Fspeeds);
            SCNCdata.WorkpieceProgram = cncv2.GProgram;
            SCNCdata.instructionsCount = cncv2.InstructionsCount;
            SCNCdata.ToolSN = cncv2.InstructionsCount;
            SCNCdata.SizeX = cncv2.SiteX;
            SCNCdata.SizeY = cncv2.SiteY;
            SCNCdata.SizeZ = cncv2.SiteZ;
            SCNCdata.Load_Current_C = cncv2.LoadC;
            SCNCdata.Load_Current_X = cncv2.LoadX;
            SCNCdata.Load_Current_Y = cncv2.LoadX;
            SCNCdata.Load_Current_Z = cncv2.LoadZ;

            // var temp1 = DbHelper.Get<StorageBin>(new { SN = cnclist[1].MagNo });
            var temp1 = DbHelper.Get<StorageBin>(new { SN = 1 });

            if (temp1 == null)
            {
                return;
            }
            else
            {
                SCNCdata.StorageBinId = temp1.Id;
                var temp2 = DbHelper.GetList<MachiningCenter>(new { }).FirstOrDefault();
                if (temp2 == null)
                {
                    DbHelper.Insert(new MachiningCenter
                    {
                        Door = SCNCdata.Door,
                        Chuck = SCNCdata.Chuck,
                        Chuck2 = SCNCdata.Chuck2,
                        Feedrate = SCNCdata.Feedrate,
                        InstructionsCount = SCNCdata.instructionsCount,
                        WorkpieceProgram = SCNCdata.WorkpieceProgram,
                        SiteX = SCNCdata.SizeX,
                        SiteZ = SCNCdata.SizeZ,
                        SiteY = SCNCdata.SizeY,
                        Load_Current_C = SCNCdata.Load_Current_C,
                        Load_Current_X = SCNCdata.Load_Current_X,
                        Load_Current_Z = SCNCdata.Load_Current_Z,
                        Load_Current_Y = SCNCdata.Load_Current_Y,
                        // StorageBinId = SCNCdata.StorageBinId,
                        StorageBinId = SCNCdata.StorageBinId,
                        SpindleSpeed = SCNCdata.SpindleSpeed,
                        State = SCNCdata.State,
                    });
                }
                else
                {
                    temp2.Door = SCNCdata.Door;
                    temp2.Chuck = SCNCdata.Chuck;
                    temp2.Chuck2 = SCNCdata.Chuck2;
                    temp2.Feedrate = Slathedata.Feedrate;
                    temp2.InstructionsCount = SCNCdata.instructionsCount;
                    temp2.WorkpieceProgram = SCNCdata.WorkpieceProgram;
                    temp2.SiteX = SCNCdata.SizeX;
                    temp2.SiteZ = SCNCdata.SizeZ;
                    temp2.SiteY = SCNCdata.SizeY;
                    temp2.Load_Current_C = SCNCdata.Load_Current_C;
                    temp2.Load_Current_X = SCNCdata.Load_Current_X;
                    temp2.Load_Current_Z = SCNCdata.Load_Current_Z;
                    temp2.Load_Current_Y = SCNCdata.Load_Current_Y;
                    // StorageBinId = SCNCdata.StorageBinId,
                    temp2.StorageBinId = SCNCdata.StorageBinId;
                    temp2.SpindleSpeed = SCNCdata.SpindleSpeed;
                    temp2.State = SCNCdata.State;

                    DbHelper.Update(temp2);
                }

            }

        }
        //private void GetInsideMeter()
        //{

        //    string key = "MacroVariables:USER";
        //    string[] metervalus = new string[6];
        //    double[] metervalud = new double[6];


        //    int ret0 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE0, ref metervalus[0]);
        //    int ret1 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE1, ref metervalus[1]);
        //    int ret2 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE2, ref metervalus[2]);
        //    int ret3 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE3, ref metervalus[3]);
        //    int ret4 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE4, ref metervalus[4]);
        //    int ret5 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE5, ref metervalus[5]);

        //    if (ret0 == -1 || ret1 == -1 || ret2 == -1 || ret3 == -1 || ret5 == -1 || ret4 == -1)
        //    {
        //        MessageBox.Show("获取失败，请重试！");
        //        return;
        //    }
        //    double[] temparry = new double[6];
        //    for (int ii = 0; ii < 6; ii++)
        //    {
        //        string str = metervalus[ii];
        //        if (str.Length > 0)
        //        {
        //            int strStart = str.IndexOf("f\":");
        //            int len = str.IndexOf(",", strStart + 3) - (strStart + 3);
        //            string strTmp = str.Substring(strStart + 3, len);
        //            string temps = "";
        //            double tempd = Convert.ToDouble(strTmp);
        //            //  tempd = -1.23;
        //            temps = tempd.ToString("F3");
        //            temparry[ii] = Convert.ToDouble(temps);//测量实际值      
        //        }
        //        else
        //        {
        //            temparry[ii] = 0;
        //        }
        //    }
        //    for (int i = 0; i < 6; i++)
        //    {
        //        double tempd = temparry[i];
        //        var temps = tempd.ToString("F3");
        //        if (temps != "null")
        //        {

        //            int index = temps.IndexOf(".");
        //            int flage = 1;
        //            if (Convert.ToDouble(temps) < 0)
        //            {
        //                flage = -1;
        //            }
        //            string refvalue1 = temps.Substring(0, index);//整数部分
        //            string refvalue2 = temps.Substring(index + 1);//小数部分
        //            if (flage == -1)
        //            {

        //                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + 3 * i] = 1;
        //            }
        //            else
        //            {
        //                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + 3 * i] = 0;
        //            }

        //            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + i * 3 + 1] = Convert.ToInt32(refvalue1);
        //            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + i * 3 + 1] < 0)
        //            {
        //                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + i * 3 + 1] = (-1) * ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + i * 3 + 1];
        //            }
        //            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + i * 3 + 2] = Convert.ToInt32(refvalue2);
        //        }
        //    }

        //}
        private void renewStoragebin()
        {

            for (int ii = 0; ii < 30; ii++)
            {
                int magstate = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
                int magsence = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;
                int magType = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Type;
                //   int magmaterial = (int)SCADA.ModbusTcp.DataConfigArr.Mag_material;
                int maglength = (int)ModbusTcp.MagLength;
                int magstatei = magstate + maglength * ii;
                int magsencei = magsence + maglength * ii;
                int magTypei = magType + maglength * ii;

                SStoragebin.SN = ii + 1;
                var state = EnumBinState.Empty;
                Enum.TryParse<EnumBinState>((ModbusTcp.DataMoubus[magstatei]).ToString(), out state);
                SStoragebin.State = state;

                var state1 = EnumSession.None;

                Enum.TryParse<EnumSession>((ModbusTcp.DataMoubus[magsence]).ToString(), out state1);

                SStoragebin.Session = state1;

                int typei = 65 + ModbusTcp.DataMoubus[magTypei];
                char typec = Convert.ToChar(typei);
                string types = Convert.ToString(typec);

                var temp2 = DbHelper.Get<WorkpieceCategory>(new { Name = types });
                SStoragebin.WorkpieceCategoryId = temp2.Id;
                var temp1 = DbHelper.Get<StorageBin>(new { SN = ii + 1 });
                if (temp1 == null)
                {
                    DbHelper.Insert(new StorageBin
                    {
                        SN = SStoragebin.SN,
                        State = SStoragebin.State,
                        Session = SStoragebin.Session,
                        WorkpieceCategoryId = SStoragebin.WorkpieceCategoryId,
                    });
                }
                else if (temp1.SN != SStoragebin.SN || temp1.State != SStoragebin.State || temp1.Session != SStoragebin.Session
                  || temp1.WorkpieceCategoryId != SStoragebin.WorkpieceCategoryId)
                {

                    temp1.SN = SStoragebin.SN;
                    temp1.State = SStoragebin.State;
                    temp1.Session = SStoragebin.Session;
                    temp1.WorkpieceCategoryId = SStoragebin.WorkpieceCategoryId;

                    DbHelper.Update(temp1);
                }
            }
        }
        private void renewSRobot()
        {

            int J1Posion = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint1_coor];
            int J2Posion = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint2_coor];
            int J3Posion = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint3_coor];
            int J4Posion = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint4_coor];
            int J5Posion = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint5_coor];
            int J6Posion = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint6_coor];
            int J7Posion = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Joint7_coor];


            if (J1Posion > 360)
            {
                J1Posion = (~J1Posion) & 0xffff;
                J1Posion = 0 - (J1Posion + 1);
            }
            if (J2Posion > 360)
            {
                J2Posion = (~J2Posion) & 0xffff;
                J2Posion = 0 - (J2Posion + 1);
            }
            if (J3Posion > 360)
            {
                J3Posion = (~J3Posion) & 0xffff;
                J3Posion = 0 - (J3Posion + 1);
            }
            if (J4Posion > 360)
            {
                J4Posion = (~J4Posion) & 0xffff;
                J4Posion = 0 - (J4Posion + 1);
            }
            if (J5Posion > 360)
            {
                J5Posion = (~J5Posion) & 0xffff;
                J5Posion = 0 - (J5Posion + 1);
            }
            if (J6Posion > 360)
            {
                J6Posion = (~J6Posion) & 0xffff;
                J6Posion = 0 - (J6Posion + 1);
            }
            if (J7Posion > 16000)
            {
                J7Posion = (~J7Posion) & 0xffff;
                J7Posion = 0 - (J7Posion + 1);
            }

            SRobortdata.Mode = EnumHelper.ParseEnum<EnumRobotMode>(ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode]);
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 0)
            {
                SRobortdata.Mode = EnumRobotMode.Manual;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 1)
            {
                SRobortdata.Mode = EnumRobotMode.Manual;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 2)
            {
                SRobortdata.Mode = EnumRobotMode.Automatic;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 3)
            {
                SRobortdata.Mode = EnumRobotMode.External;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_status] == 0)
            {
                SRobortdata.State = EnumRobotState.Running;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_status] == 1)
            {
                SRobortdata.State = EnumRobotState.Error;
            }
            //SRobortdata.State = EnumHelper.ParseEnum<EnumRobotState>(ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_status]);
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm] == 1)
            {
                SRobortdata.OriginalPoint = true;
            }
            else
            {
                SRobortdata.OriginalPoint = false;
            }

            //UpdataRobotData();

            SRobortdata.SiteJ1 = J1Posion * 1.0;
            SRobortdata.SiteJ2 = J2Posion * 1.0;
            SRobortdata.SiteJ3 = J3Posion * 1.0;
            SRobortdata.SiteJ4 = J4Posion * 1.0;
            SRobortdata.SiteJ5 = J5Posion * 1.0;
            SRobortdata.SiteJ6 = J6Posion * 1.0;
            SRobortdata.SiteJ7 = J7Posion * 1.0;

            // var temp1 = DbHelper.GetList<StorageBin>(new { SN = cnclist[1].MagNo }).FirstOrDefault();           
            var temp2 = DbHelper.Get<Robot>(new { });
            if (temp2 == null)
            {
                DbHelper.Insert(new Robot
                {

                    Mode = SRobortdata.Mode,
                    IsOriginalPoint = SRobortdata.OriginalPoint,
                    SiteJ1 = SRobortdata.SiteJ1,
                    SiteJ2 = SRobortdata.SiteJ2,
                    SiteJ3 = SRobortdata.SiteJ3,
                    SiteJ4 = SRobortdata.SiteJ4,
                    SiteJ5 = SRobortdata.SiteJ5,
                    SiteJ6 = SRobortdata.SiteJ6,
                    SiteJ7 = SRobortdata.SiteJ7,
                    State = SRobortdata.State,
                });
            }
            else
            {
                temp2.State = SRobortdata.State;
                temp2.Mode = SRobortdata.Mode;
                temp2.IsOriginalPoint = SRobortdata.OriginalPoint;
                temp2.SiteJ1 = SRobortdata.SiteJ1;
                temp2.SiteJ2 = SRobortdata.SiteJ2;
                temp2.SiteJ3 = SRobortdata.SiteJ3;
                temp2.SiteJ4 = SRobortdata.SiteJ4;
                temp2.SiteJ5 = SRobortdata.SiteJ5;
                temp2.SiteJ6 = SRobortdata.SiteJ6;
                temp2.SiteJ7 = SRobortdata.SiteJ7;

                DbHelper.Update(temp2);
            }

        }
        private void renewSRobot(bool t = true)
        {



            SRobortdata.Mode = EnumHelper.ParseEnum<EnumRobotMode>(ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode]);
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 0)
            {
                SRobortdata.Mode = EnumRobotMode.Manual;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 1)
            {
                SRobortdata.Mode = EnumRobotMode.Manual;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 2)
            {
                SRobortdata.Mode = EnumRobotMode.Automatic;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 3)
            {
                SRobortdata.Mode = EnumRobotMode.External;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_status] == 0)
            {
                SRobortdata.State = EnumRobotState.Running;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_status] == 1)
            {
                SRobortdata.State = EnumRobotState.Error;
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm] == 1)
            {
                SRobortdata.OriginalPoint = true;
            }
            else
            {
                SRobortdata.OriginalPoint = false;
            }

            UpdataRobotData(true);

            // var temp1 = DbHelper.GetList<StorageBin>(new { SN = cnclist[1].MagNo }).FirstOrDefault();           
            var temp2 = DbHelper.Get<Robot>(new { });
            if (temp2 == null)
            {
                DbHelper.Insert(new Robot
                {

                    Mode = SRobortdata.Mode,
                    IsOriginalPoint = SRobortdata.OriginalPoint,
                    SiteJ1 = SRobortdata.SiteJ1,
                    SiteJ2 = SRobortdata.SiteJ2,
                    SiteJ3 = SRobortdata.SiteJ3,
                    SiteJ4 = SRobortdata.SiteJ4,
                    SiteJ5 = SRobortdata.SiteJ5,
                    SiteJ6 = SRobortdata.SiteJ6,
                    SiteJ7 = SRobortdata.SiteJ7,
                    State = SRobortdata.State,
                });
            }
            else
            {
                temp2.State = SRobortdata.State;
                temp2.Mode = SRobortdata.Mode;
                temp2.IsOriginalPoint = SRobortdata.OriginalPoint;
                temp2.SiteJ1 = SRobortdata.SiteJ1;
                temp2.SiteJ2 = SRobortdata.SiteJ2;
                temp2.SiteJ3 = SRobortdata.SiteJ3;
                temp2.SiteJ4 = SRobortdata.SiteJ4;
                temp2.SiteJ5 = SRobortdata.SiteJ5;
                temp2.SiteJ6 = SRobortdata.SiteJ6;
                temp2.SiteJ7 = SRobortdata.SiteJ7;

                DbHelper.Update(temp2);
            }

        }
        private void renewWorkpieceCategory()
        {
            var temp1 = DbHelper.Get<WorkpieceCategory>(new { Name = "A" });
            if (temp1 == null)
            {
                DbHelper.Insert(new WorkpieceCategory
                {
                    Name = "A",
                    Description = "A",
                });
            }
            temp1 = DbHelper.Get<WorkpieceCategory>(new { Name = "B" });
            if (temp1 == null)
            {
                DbHelper.Insert(new WorkpieceCategory
                {
                    Name = "B",
                    Description = "B",
                });
            }
            temp1 = DbHelper.Get<WorkpieceCategory>(new { Name = "C" });
            if (temp1 == null)
            {
                DbHelper.Insert(new WorkpieceCategory
                {
                    Name = "C",
                    Description = "C",
                });
            }
            temp1 = DbHelper.Get<WorkpieceCategory>(new { Name = "D" });
            if (temp1 == null)
            {
                DbHelper.Insert(new WorkpieceCategory
                {
                    Name = "D",
                    Description = "D",
                });
            }
        }

        //private void renewToolLathe()
        //{
        //    //车床表
        //    if (cnclist.Count < 2)
        //    {
        //        return;
        //    }
        //    if (cnclist[0].isConnected() == false)
        //    {
        //        return;
        //    }

        //    int value = 0;
        //    int countno = MacDataService.GetInstance().HNC_ParamanGetI32((Int32)HNCAPI.HNCDATADEF.PARAMAN_FILE_CHAN, 0, 128, ref value, MainForm.cnclist[0].HCNCShareData.sysData.dbNo);//P参数从300开始偏移;

        //    if (countno == -1)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        // var temp0 = DbHelper.Get<MachiningCenter>(new { });
        //        var temp1 = DbHelper.Get<Lathe>(new { });

        //        // var item = DbHelper.Get<Cutter>(new { });
        //        SCutter.MachineId = temp1.Id;

        //        DbHelper.Delete<Cutter>(new { MachineId = temp1.Id });

        //        for (int i = 0; i < value; i++)
        //        {
        //            string toolStr = "";
        //            int ret = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[0].HCNCShareData.sysData.dbNo, "Tool:List", String.Format("{0:D4}", i + 1), ref toolStr);
        //            if (ret == 0)
        //            {

        //                double tempd = 0;
        //                toolComp tool = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr);
        //                SCutter.SN = i + 1;//刀号
        //                tempd = tool.GTOOL_LEN1 * 2;
        //                SCutter.Radius = tempd;//半径

        //                tempd = tool.GTOOL_LEN3;
        //                SCutter.Length = tempd;//长度

        //                tempd = tool.WTOOL_LEN1;
        //                SCutter.LengthAbrasion = tempd;//长度磨损

        //                tempd = tool.WTOOL_RAD1 * 2;
        //                SCutter.RadiusAbrasion = tempd;//半径磨损;
        //                var temp2 = DbHelper.Get<Cutter>(new { SN = SCutter.SN, MachineId = temp1.Id });
        //                if (temp2 == null)
        //                {
        //                    DbHelper.Insert(new Cutter
        //                    {
        //                        SN = SCutter.SN,
        //                        Length = SCutter.Length,
        //                        LengthAbrasion = SCutter.LengthAbrasion,
        //                        Radius = SCutter.Radius,
        //                        RadiusAbrasion = SCutter.RadiusAbrasion,
        //                        MachineId = SCutter.MachineId,
        //                    });
        //                }
        //                else if (SCutter.Length != temp2.Length || SCutter.LengthAbrasion != temp2.LengthAbrasion
        //                    || SCutter.Radius != temp2.Radius || SCutter.RadiusAbrasion != temp2.RadiusAbrasion)
        //                {
        //                    temp2.SN = SCutter.SN;
        //                    temp2.Length = SCutter.Length;
        //                    temp2.LengthAbrasion = SCutter.LengthAbrasion;
        //                    temp2.Radius = SCutter.Radius;
        //                    temp2.RadiusAbrasion = SCutter.RadiusAbrasion;
        //                    temp2.MachineId = SCutter.MachineId;

        //                    DbHelper.Update(temp2);
        //                }
        //                renewlathesql = false;

        //            }
        //        }
        //        // DbHelper.Delete<Cutter>(new { MachineId = temp1.Id });

        //    }
        //}
        //private void renewToolCNC()
        //{
        //    //车床表
        //    if (cnclist.Count < 2)
        //    {
        //        return;
        //    }
        //    if (cnclist[1].isConnected() == false)
        //    {
        //        return;
        //    }

        //    int value = 0;
        //    int countno = MacDataService.GetInstance().HNC_ParamanGetI32((Int32)HNCAPI.HNCDATADEF.PARAMAN_FILE_CHAN, 0, 128, ref value, MainForm.cnclist[1].HCNCShareData.sysData.dbNo);//P参数从300开始偏移;

        //    if (countno == -1)
        //    {
        //        countno = 30;
        //    }
        //    else
        //    {
        //        var temp1 = DbHelper.Get<MachiningCenter>(new { });
        //        //var temp1 = DbHelper.Get<Lathe>(new { });

        //        // var item = DbHelper.Get<Cutter>(new { });
        //        SCutter.MachineId = temp1.Id;

        //        DbHelper.Delete<Cutter>(new { MachineId = temp1.Id });

        //        for (int i = 0; i < value; i++)
        //        {
        //            string toolStr = "";
        //            int ret = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].HCNCShareData.sysData.dbNo, "Tool:List", String.Format("{0:D4}", i + 1), ref toolStr);
        //            if (ret == 0)
        //            {

        //                double tempd = 0;
        //                toolComp tool = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr);
        //                SCutter.SN = i + 1;//刀号
        //                tempd = tool.GTOOL_LEN1;
        //                SCutter.Radius = tempd;//半径

        //                tempd = tool.GTOOL_LEN3;
        //                SCutter.Length = tempd;//长度

        //                tempd = tool.WTOOL_LEN1;
        //                SCutter.LengthAbrasion = tempd;//长度磨损

        //                tempd = tool.WTOOL_RAD1;
        //                SCutter.RadiusAbrasion = tempd;//半径磨损;
        //                var temp2 = DbHelper.Get<Cutter>(new { SN = SCutter.SN, MachineId = temp1.Id });
        //                if (temp2 == null)
        //                {
        //                    DbHelper.Insert(new Cutter
        //                    {
        //                        SN = SCutter.SN,
        //                        Length = SCutter.Length,
        //                        LengthAbrasion = SCutter.LengthAbrasion,
        //                        Radius = SCutter.Radius,
        //                        RadiusAbrasion = SCutter.RadiusAbrasion,
        //                        MachineId = SCutter.MachineId,
        //                    });
        //                }
        //                else if (SCutter.Length != temp2.Length || SCutter.LengthAbrasion != temp2.LengthAbrasion
        //                    || SCutter.Radius != temp2.Radius || SCutter.RadiusAbrasion != temp2.RadiusAbrasion)
        //                {
        //                    temp2.SN = SCutter.SN;
        //                    temp2.Length = SCutter.Length;
        //                    temp2.LengthAbrasion = SCutter.LengthAbrasion;
        //                    temp2.Radius = SCutter.Radius;
        //                    temp2.RadiusAbrasion = SCutter.RadiusAbrasion;
        //                    temp2.MachineId = SCutter.MachineId;

        //                    DbHelper.Update(temp2);
        //                }

        //                renewcncsql = false;

        //            }
        //        }
        //        // DbHelper.Delete<Cutter>(new { MachineId = temp1.Id });

        //    }
        //}
        private void renewToolLathe(CNCV2 cncv2)
        {
            //车床表

            if (cncv2.EquipmentState == "离线")
            {
                return;
            }

            int value = 0;
            int countno = cncv2.ToolNum;

            if (countno == -1)
            {
                return;
            }
            else
            {
                // var temp0 = DbHelper.Get<MachiningCenter>(new { });
                var temp1 = DbHelper.Get<Lathe>(new { });

                // var item = DbHelper.Get<Cutter>(new { });
                SCutter.MachineId = temp1.Id;

                DbHelper.Delete<Cutter>(new { MachineId = temp1.Id });

                for (int i = 0; i < value; i++)
                {
                    //  string toolStr = "";

                    double tempd = 0;
                    SCutter.SN = i + 1;//刀号
                    tempd = cncv2.TOOLData[i].Radius * 2;
                    SCutter.Radius = tempd;//半径

                    tempd = cncv2.TOOLData[i].Length;
                    SCutter.Length = tempd;//长度

                    tempd = cncv2.TOOLData[i].LengthComp;
                    SCutter.LengthAbrasion = tempd;//长度磨损

                    tempd = cncv2.TOOLData[i].RadiusComp * 2;
                    SCutter.RadiusAbrasion = tempd;//半径磨损;
                    var temp2 = DbHelper.Get<Cutter>(new { SN = SCutter.SN, MachineId = temp1.Id });
                    if (temp2 == null)
                    {
                        DbHelper.Insert(new Cutter
                        {
                            SN = SCutter.SN,
                            Length = SCutter.Length,
                            LengthAbrasion = SCutter.LengthAbrasion,
                            Radius = SCutter.Radius,
                            RadiusAbrasion = SCutter.RadiusAbrasion,
                            MachineId = SCutter.MachineId,
                        });
                    }
                    else if (SCutter.Length != temp2.Length || SCutter.LengthAbrasion != temp2.LengthAbrasion
                        || SCutter.Radius != temp2.Radius || SCutter.RadiusAbrasion != temp2.RadiusAbrasion)
                    {
                        temp2.SN = SCutter.SN;
                        temp2.Length = SCutter.Length;
                        temp2.LengthAbrasion = SCutter.LengthAbrasion;
                        temp2.Radius = SCutter.Radius;
                        temp2.RadiusAbrasion = SCutter.RadiusAbrasion;
                        temp2.MachineId = SCutter.MachineId;

                        DbHelper.Update(temp2);
                    }
                    renewlathesql = false;

                }
            }
            // DbHelper.Delete<Cutter>(new { MachineId = temp1.Id });

        }

        private void renewToolCNC(CNCV2 cncv2)
        {
            //车床表
            if (cncv2.EquipmentState == "离线")
            {
                return;
            }

            int value = 0;
            int countno = cncv2.ToolNum;//P参数从300开始偏移;

            if (countno == -1)
            {
                countno = 30;
            }
            else
            {
                var temp1 = DbHelper.Get<MachiningCenter>(new { });
                //var temp1 = DbHelper.Get<Lathe>(new { });

                // var item = DbHelper.Get<Cutter>(new { });
                SCutter.MachineId = temp1.Id;

                DbHelper.Delete<Cutter>(new { MachineId = temp1.Id });

                for (int i = 0; i < value; i++)
                {
                    double tempd = 0;
                    SCutter.SN = i + 1;//刀号
                    tempd = cncv2.TOOLData[i].Radius;
                    SCutter.Radius = tempd;//半径


                    tempd = cncv2.TOOLData[i].Length;
                    SCutter.Length = tempd;//长度

                    tempd = cncv2.TOOLData[i].LengthComp;
                    SCutter.LengthAbrasion = tempd;//长度磨损

                    tempd = cncv2.TOOLData[i].RadiusComp;
                    SCutter.RadiusAbrasion = tempd;//半径磨损;
                    var temp2 = DbHelper.Get<Cutter>(new { SN = SCutter.SN, MachineId = temp1.Id });
                    if (temp2 == null)
                    {
                        DbHelper.Insert(new Cutter
                        {
                            SN = SCutter.SN,
                            Length = SCutter.Length,
                            LengthAbrasion = SCutter.LengthAbrasion,
                            Radius = SCutter.Radius,
                            RadiusAbrasion = SCutter.RadiusAbrasion,
                            MachineId = SCutter.MachineId,
                        });
                    }
                    else if (SCutter.Length != temp2.Length || SCutter.LengthAbrasion != temp2.LengthAbrasion
                        || SCutter.Radius != temp2.Radius || SCutter.RadiusAbrasion != temp2.RadiusAbrasion)
                    {
                        temp2.SN = SCutter.SN;
                        temp2.Length = SCutter.Length;
                        temp2.LengthAbrasion = SCutter.LengthAbrasion;
                        temp2.Radius = SCutter.Radius;
                        temp2.RadiusAbrasion = SCutter.RadiusAbrasion;
                        temp2.MachineId = SCutter.MachineId;

                        DbHelper.Update(temp2);
                    }

                    renewcncsql = false;


                }
                // DbHelper.Delete<Cutter>(new { MachineId = temp1.Id });

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (sqlconnectflage == true)
            {
                sqlconnectflage = false;
                sqlconnectcount = 0;
                buttonsqlconnect.BackColor = Color.Gray;
            }
            else
            {
                return;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tabPagemain_DrawItem(object sender, DrawItemEventArgs e)
        {
            tabPagemain.SuspendLayout();
            //背景色

            Image backImage = Properties.Resources.titleBack;
            Rectangle rec = tabPagemain.ClientRectangle;

            StringFormat StrFormat = new StringFormat();

            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;

            SolidBrush tabBackBrush = new SolidBrush(Color.SkyBlue);
            //文字色
            SolidBrush FrontBrush = new SolidBrush(Color.White);
            //StringFormat stringF = new StringFormat();
            Font wordfont = new Font("微软雅黑", 14.25F);
            e.Graphics.DrawImage(backImage, 0, 0, tabPagemain.Width, tabPagemain.Height);
            for (int i = 0; i < tabPagemain.TabCount; i++)
            {
                //标签工作区
                Rectangle rec1 = tabPagemain.GetTabRect(i);
                //e.Graphics.DrawImage(backImage, 0, 0, tabPagemain.Width, tabPagemain.Height);
                e.Graphics.FillRectangle(tabBackBrush, rec1);
                ////标签头背景色
                e.Graphics.DrawString(tabPagemain.TabPages[i].Text, wordfont, FrontBrush, rec1, StrFormat);
                ////标签头文字
            }
            tabPagemain.ResumeLayout();
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
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x020A)
            {

            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

    }
}



