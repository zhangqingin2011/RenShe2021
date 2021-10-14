using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HNC_MacDataService;
using HNCAPI;
using System.IO;
using System.Collections;
using HNC.MOHRSS.Model;
using HNC.MOHRSS.Common;
using HNC.API;

namespace SCADA
{

    public partial class OrderForm1 : Form
    {
        //订单的保存格式
        //序号 料仓编号  工序1 工序2  工序3  工序4  订单下发 工序1状态  工序2状态  工序3状态 工序4状态
        //item = 0,magno = 5,fun1 = 1,fun2=2,fun3=0，fun4=0，down = 0，state1= 1，state2=1，state3=0，state4=0；
        //序号
        //工序 0、无 1、车床  2、铣床
        //订单下发 0、未下发，1、已下发
        //工序状态 0、无此工序，1、未开始，2、进行中，3、完成，4、返修中
        public string language = "";
        static public bool[] valueb = new bool[6];
        static public string[] valuestrarry = new string[6];
        static public double[,] refvalue = new double[6, 3];
        static public double[] resvalue = new double[6];
        public static bool bIsOK = false;  //是否合格 
        public static bool measureenable = false;//测量使能
        public static bool renewMeterRecordGridview = false;//测量使能
        Hashtable m_Hashtable;
        public static String OrderdataFilePath = "..\\data\\Order\\OrderdataFile";
        public static String OrderstateFilePath = "..\\data\\Order\\OrderstateFile";
        public static String OrderdataFilebakPath = "..\\data\\Order\\OrderdatabakFile";
        public static String OrderstateFilebakPath = "..\\data\\Order\\OrderstatebakFile";

        public static String GcodeFilePath = "C:\\Users\\Public\\Documents\\加工程序目录";

        public static String MetersetFilePath = "..\\data\\measure\\MeterSetFile";
        public static String MetetrrecordFilePath = "..\\data\\measure\\MeterRecordFile";
        public int[] gongxuindex = new int[30];

        public static bool remeterdgv3 = false;
        public static bool firstaddrecord = false;

        public static bool renewMeterDGV2 = false;
        public static bool ReProcessChoose = false;
        public static int ReProcessChooseitem = 0;
        public ShowForm form1 = new ShowForm();
        //    public static Int32 ReProcessChooseitem = 0;

        private string prelanguage; //记录切换语言之前的语言
        public int finishmagload = 0;
        public int finishmagunload = 0;
        public int finishmag = 0;
        public string tempstring = "";//完成提示
        AutoSizeFormClass aotosize = new AutoSizeFormClass();
        public static bool rerunningflage = false;
        public static bool aotomode = false;//自动模式标识
        public static bool aotostop = false;//自动模式暂停标识
        public static bool manmode = true;//手动模式标识
        private static int aotorunmag = 0;//自动加工的仓位号
        private static int aotorunmaglathe = 0;//自动加工的仓位号
        private static int aotorunmagcnc = 0;//自动加工的仓位号
        private static int indexlathe = -1;//车床当前自动运行的订单编号
        private static int indexcnc = -1;//加工中心当前自动运行的订单编号
        public static bool IsRerunning = false;
        public static bool SkipMeterflage = false;
        private static bool MessageHasshow1 = true;//车床完成信号仓位错误提示标志
        private static bool MessageHasshow2 = true;//铣床完成信号仓位错误提示标志
        public enum Processstate : int//
        {
            None = -1,//工序为无
            Notstart = 0,//未开始
            Loading = 1,//上料中
            Loaded = 2,//上料完
            Running = 3,//加工中
            Runned = 4,//加工完成
            Rerunning = 5,//返修中
            Rerunned = 6,//返修完成
            Unloading = 7,//下料中
            Unloaded = 8,//下料完
            Alarm = 9,//异常状态
        };
        public enum Orderstate : int//报文的功能码
        {
            Notstart = 0,
            Processing = 1,
            Finish = 2,
            Reback = 3,
            Alarm = 4,//异常状态
        };


        public OrderForm1()
        {
            InitializeComponent();
            comboBoxFun1.SelectedIndex = 0;
            comboBoxFun2.SelectedIndex = 0;
            InitOrderdata(OrderdataFilePath);
            InitOrderstate(OrderstateFilePath);
            form1.Visible = false;

        }
        private bool ReInitOrderdata()
        {

            try
            {

                File.Delete(OrderForm1.OrderdataFilePath);
                File.Copy(OrderForm1.OrderdataFilebakPath, OrderForm1.OrderdataFilePath, true);

                FileStream aFile = new FileStream(OrderdataFilePath, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);

                string item = "";
                string magno = "";
                string fun1 = "";
                string fun2 = "";
                string fun = "";
                string step = "";
                string down = "";
                string top = "";
                string state = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();
                dataGridVieworder.Rows.Clear();
                while (line != null && line != "")
                {
                    dataGridVieworder.Rows.Add();
                    item = getvalueformstring(line, "item=");
                    magno = getvalueformstring(line, "magno=");
                    fun1 = getvalueformstring(line, "fun1=");
                    fun2 = getvalueformstring(line, "fun2=");
                    fun = getvalueformstring(line, "fun=");
                    step = getvalueformstring(line, "step=");
                    down = getvalueformstring(line, "down=");
                    top = getvalueformstring(line, "top=");
                    state = getvalueformstring(line, "state=");
                    if (item == "null" || magno == "null" || fun1 == "null" || fun == "null" || step == "null" || down == "null" || top == "null" || state == "null")
                    {
                        line = sr.ReadLine();

                    }
                    else
                    {
                        dataGridVieworder.Rows[ii].Cells[0].Value = item;
                        dataGridVieworder.Rows[ii].Cells[1].Value = magno;
                        dataGridVieworder.Rows[ii].Cells[2].Value = fun1;
                        dataGridVieworder.Rows[ii].Cells[3].Value = fun2;
                        dataGridVieworder.Rows[ii].Cells[4].Value = fun;
                        dataGridVieworder.Rows[ii].Cells[5].Value = step;
                        dataGridVieworder.Rows[ii].Cells[8].Value = state;
                        if (language == "English")
                        {
                            dataGridVieworder.Rows[ii].Cells[6].Value = "OK";
                            dataGridVieworder.Rows[ii].Cells[7].Value = "Top";
                        }
                        else
                        {
                            dataGridVieworder.Rows[ii].Cells[6].Value = "确定";

                            dataGridVieworder.Rows[ii].Cells[7].Value = "置顶";
                        }

                        dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.LightGreen;
                        dataGridVieworder.Rows[ii].Cells[7].Style.BackColor = Color.LightGreen;
                        int magnoi = Convert.ToInt32(magno);
                        MainForm.magisordered[magnoi - 1] = 1;
                        line = sr.ReadLine();
                        ii++;
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
        private bool InitOrderdata(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);

                string item = "";
                string magno = "";
                string fun1 = "";
                string fun2 = "";
                string fun = "";
                string step = "";
                string down = "";
                string top = "";
                string state = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();
                dataGridVieworder.Rows.Clear();
                while (line != null && line != "")
                {
                    dataGridVieworder.Rows.Add();
                    item = getvalueformstring(line, "item=");
                    magno = getvalueformstring(line, "magno=");
                    fun1 = getvalueformstring(line, "fun1=");
                    fun2 = getvalueformstring(line, "fun2=");
                    fun = getvalueformstring(line, "fun=");
                    step = getvalueformstring(line, "step=");
                    down = getvalueformstring(line, "down=");
                    top = getvalueformstring(line, "top=");
                    state = getvalueformstring(line, "state=");
                    if (item == "null" || magno == "null" || fun1 == "null" || fun == "null" || step == "null" || down == "null" || top == "null" || state == "null")
                    {
                        line = sr.ReadLine();

                    }
                    else
                    {
                        dataGridVieworder.Rows[ii].Cells[0].Value = item;
                        dataGridVieworder.Rows[ii].Cells[1].Value = magno;
                        dataGridVieworder.Rows[ii].Cells[2].Value = fun1;
                        dataGridVieworder.Rows[ii].Cells[3].Value = fun2;
                        dataGridVieworder.Rows[ii].Cells[4].Value = fun;
                        dataGridVieworder.Rows[ii].Cells[5].Value = step;
                        dataGridVieworder.Rows[ii].Cells[8].Value = state;
                        if (language == "English")
                        {
                            dataGridVieworder.Rows[ii].Cells[6].Value = "OK";
                            dataGridVieworder.Rows[ii].Cells[7].Value = "Top";
                        }
                        else
                        {
                            dataGridVieworder.Rows[ii].Cells[6].Value = "确定";

                            dataGridVieworder.Rows[ii].Cells[7].Value = "置顶";
                        }

                        dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.LightGreen;
                        dataGridVieworder.Rows[ii].Cells[7].Style.BackColor = Color.LightGreen;
                        int magnoi = Convert.ToInt32(magno);
                        MainForm.magisordered[magnoi - 1] = 1;
                        line = sr.ReadLine();
                        ii++;
                    }


                }

                sr.Close();
                aFile.Close();
                return true;
            }
            catch (IOException e)
            {

                ReInitOrderdata();
                return false;
            }
        }

        private bool reInitOrderstate()
        {
            try
            {
                File.Delete(OrderForm1.OrderstateFilePath);
                File.Copy(OrderForm1.OrderstateFilebakPath, OrderForm1.OrderstateFilePath, true);
                FileStream aFile = new FileStream(OrderstateFilePath, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                string item = "";
                string magno = "";
                string fun1 = "";
                string fun2 = "";
                string state = "";
                string check = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();

                dataGridVieworder2.Rows.Clear();
                while (line != null && line != "")
                {
                    item = getvalueformstring(line, "item=");
                    magno = getvalueformstring(line, "magno=");
                    fun1 = getvalueformstring(line, "fun1=");
                    fun2 = getvalueformstring(line, "fun2=");
                    state = getvalueformstring(line, "state=");
                    check = getvalueformstring(line, "check=");
                    if (item == "null" || magno == "null" || fun1 == "null" || fun2 == "null" || state == "null" || check == "null")
                    {
                        line = sr.ReadLine();

                    }
                    else
                    {
                        dataGridVieworder2.Rows.Add();
                        dataGridVieworder2.Rows[ii].Cells[0].Value = item;
                        dataGridVieworder2.Rows[ii].Cells[1].Value = magno;
                        dataGridVieworder2.Rows[ii].Cells[2].Value = fun1;
                        dataGridVieworder2.Rows[ii].Cells[3].Value = fun2;
                        dataGridVieworder2.Rows[ii].Cells[4].Value = state;
                        dataGridVieworder2.Rows[ii].Cells[5].Value = check;

                        line = sr.ReadLine();
                        ii++;
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
        private bool InitOrderstate(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                string item = "";
                string magno = "";
                string fun1 = "";
                string fun2 = "";
                string state = "";
                string check = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();

                dataGridVieworder2.Rows.Clear();
                while (line != null && line != "")
                {
                    item = getvalueformstring(line, "item=");
                    magno = getvalueformstring(line, "magno=");
                    fun1 = getvalueformstring(line, "fun1=");
                    fun2 = getvalueformstring(line, "fun2=");
                    state = getvalueformstring(line, "state=");
                    check = getvalueformstring(line, "check=");
                    if (item == "null" || magno == "null" || fun1 == "null" || fun2 == "null" || state == "null" || check == "null")
                    {
                        line = sr.ReadLine();

                    }
                    else
                    {
                        dataGridVieworder2.Rows.Add();
                        dataGridVieworder2.Rows[ii].Cells[0].Value = item;
                        dataGridVieworder2.Rows[ii].Cells[1].Value = magno;
                        dataGridVieworder2.Rows[ii].Cells[2].Value = fun1;
                        dataGridVieworder2.Rows[ii].Cells[3].Value = fun2;
                        dataGridVieworder2.Rows[ii].Cells[4].Value = state;
                        dataGridVieworder2.Rows[ii].Cells[5].Value = check;

                        line = sr.ReadLine();
                        ii++;
                    }

                }

                sr.Close();
                aFile.Close();
                return true;
            }
            catch (IOException e)
            {
                reInitOrderstate();
                return false;
            }
        }

        //rack=0,fun1=-1,fun2=-1;

        /// <summary>
        ///保存订单信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool Orderdatasave(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Create);
                StreamWriter sr = new StreamWriter(aFile);
                StringBuilder sb = new StringBuilder();
                int jj = 0;
                string orderdatas = "";
                //if (dataGridVieworder.Rows.Count<2)
                //{
                //    return;
                //}

                if (dataGridVieworder.Rows.Count == 0)
                {
                    sr.Close();
                    aFile.Close();

                    FileStream wFile = new FileStream(path, FileMode.Create);
                    StreamWriter wsr = new StreamWriter(wFile);
                    wsr.WriteLine(sb.ToString());

                    wsr.Close();
                    wFile.Close();
                    return true;
                }
                if (dataGridVieworder.Rows.Count > 1)
                {
                    if (language == "English")
                    {
                        this.Column5.Items.Clear();
                        this.Column5.Items.AddRange(new object[] {
                                "None",
                                "Processes1",
                                "Processes2",});

                    }
                    if (language == "Chinese")
                    {
                        this.Column5.Items.Clear();
                        this.Column5.Items.AddRange(new object[] {
                                        "无",
                                        "工序一",
                                        "工序二",});
                    }
                    if (language == "English")
                    {
                        this.Column6.Items.Clear();
                        this.Column6.Items.AddRange(new object[] {
                                "Load",
                                "Unload",
                                "Change",
                                "Aoto",});

                    }
                    if (language == "Chinese")
                    {
                        this.Column6.Items.Clear();
                        this.Column6.Items.AddRange(new object[] {
                                        "上料",
                                        "下料",
                                        "换料",
                                        "自动",});
                    }
                }
                for (jj = 0; jj < dataGridVieworder.Rows.Count; jj++)
                {





                    if (dataGridVieworder.Rows[jj].Cells[0].Value == null
                        || dataGridVieworder.Rows[jj].Cells[1].Value == null
                        || dataGridVieworder.Rows[jj].Cells[2].Value == null
                        || dataGridVieworder.Rows[jj].Cells[3].Value == null
                        || dataGridVieworder.Rows[jj].Cells[4].Value == null
                        || dataGridVieworder.Rows[jj].Cells[5].Value == null
                        || dataGridVieworder.Rows[jj].Cells[6].Value == null
                        || dataGridVieworder.Rows[jj].Cells[7].Value == null
                        || dataGridVieworder.Rows[jj].Cells[8].Value == null)
                    {
                        dataGridVieworder.Rows.RemoveAt(jj);
                    }
                    else
                    {
                        if (dataGridVieworder.Rows[jj].Cells[0].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[2].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[3].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[6].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[8].Value.ToString() == "" ||
                           dataGridVieworder.Rows[jj].Cells[0].Value.ToString() == "null" ||
                           dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == "null" ||
                           dataGridVieworder.Rows[jj].Cells[2].Value.ToString() == "null" ||
                           dataGridVieworder.Rows[jj].Cells[3].Value.ToString() == "null" ||
                           dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "null" ||
                           dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "null" ||
                           dataGridVieworder.Rows[jj].Cells[6].Value.ToString() == "null" ||
                           dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "null" ||
                           dataGridVieworder.Rows[jj].Cells[8].Value.ToString() == "null")
                        {
                            ;
                        }
                        else
                        {
                            if (language == "English")
                            {
                                for (int kk = 2; kk < 4; kk++)
                                {
                                    if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "无")
                                    {
                                        dataGridVieworder.Rows[jj].Cells[kk].Value = "None";
                                    }
                                    if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "车工序")
                                    {
                                        dataGridVieworder.Rows[jj].Cells[kk].Value = "Lathe";
                                    }
                                    if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "铣工序")
                                    {
                                        dataGridVieworder.Rows[jj].Cells[kk].Value = "CNC";
                                    }
                                }
                                if (dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "工序二")
                                {
                                    dataGridVieworder.Rows[jj].Cells[4].Value = "Processes2";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "工序一")
                                {
                                    dataGridVieworder.Rows[jj].Cells[4].Value = "Processes1";
                                }

                                if (dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "无")
                                {
                                    dataGridVieworder.Rows[jj].Cells[4].Value = "None";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "上料")
                                {
                                    dataGridVieworder.Rows[jj].Cells[5].Value = "Load";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "下料")
                                {
                                    dataGridVieworder.Rows[jj].Cells[5].Value = "Unload";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "换料")
                                {
                                    dataGridVieworder.Rows[jj].Cells[5].Value = "Change";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "自动")
                                {
                                    dataGridVieworder.Rows[jj].Cells[5].Value = "Aoto";
                                }
                                //DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
                                //     cell = (DataGridViewComboBoxCell)dataGridVieworder.Rows[jj].Cells[7];
                                //ComboBox cell1 = new ComboBox();


                                dataGridVieworder.Rows[jj].Cells[6].Value = "OK";
                                dataGridVieworder.Rows[jj].Cells[7].Value = "Top";

                            }
                            else
                            {
                                for (int kk = 2; kk < 4; kk++)
                                {
                                    if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "None")
                                    {
                                        dataGridVieworder.Rows[jj].Cells[kk].Value = "无";
                                    }
                                    if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "Lathe")
                                    {
                                        dataGridVieworder.Rows[jj].Cells[kk].Value = "车工序";
                                    }
                                    if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "CNC")
                                    {
                                        dataGridVieworder.Rows[jj].Cells[kk].Value = "铣工序";
                                    }
                                }

                                if (dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "Processes2")
                                {
                                    dataGridVieworder.Rows[jj].Cells[4].Value = "工序二";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "Processes1")
                                {
                                    dataGridVieworder.Rows[jj].Cells[4].Value = "工序一";
                                }

                                if (dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "None")
                                {
                                    dataGridVieworder.Rows[jj].Cells[4].Value = "无";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "Load")
                                {
                                    dataGridVieworder.Rows[jj].Cells[5].Value = "上料";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "Unload")
                                {
                                    dataGridVieworder.Rows[jj].Cells[5].Value = "下料";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "Change")
                                {
                                    dataGridVieworder.Rows[jj].Cells[5].Value = "换料";
                                }
                                if (dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "Aoto")
                                {
                                    dataGridVieworder.Rows[jj].Cells[5].Value = "自动";
                                }
                                dataGridVieworder.Rows[jj].Cells[6].Value = "确定";
                                dataGridVieworder.Rows[jj].Cells[7].Value = "置顶";
                            }
                            orderdatas = "item=" + dataGridVieworder.Rows[jj].Cells[0].Value.ToString();
                            orderdatas = orderdatas + ",magno=" + dataGridVieworder.Rows[jj].Cells[1].Value.ToString();
                            orderdatas = orderdatas + ",fun1=" + dataGridVieworder.Rows[jj].Cells[2].Value.ToString();
                            orderdatas = orderdatas + ",fun2=" + dataGridVieworder.Rows[jj].Cells[3].Value.ToString();
                            orderdatas = orderdatas + ",fun=" + dataGridVieworder.Rows[jj].Cells[4].Value.ToString();
                            orderdatas = orderdatas + ",step=" + dataGridVieworder.Rows[jj].Cells[5].Value.ToString();
                            orderdatas = orderdatas + ",down=" + dataGridVieworder.Rows[jj].Cells[6].Value.ToString();
                            orderdatas = orderdatas + ",top=" + dataGridVieworder.Rows[jj].Cells[7].Value.ToString();
                            orderdatas = orderdatas + ",state=" + dataGridVieworder.Rows[jj].Cells[8].Value.ToString();

                            sb.Append(orderdatas);
                            if (orderdatas.Length > 40)
                            {

                                sr.WriteLine(orderdatas);
                            }
                        }
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
        /// <summary>
        /// 保存订单状态信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool Orderstatesave(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Create);
                StreamWriter sr = new StreamWriter(aFile);
                StringBuilder sb = new StringBuilder();
                int jj = 0;
                string orderdatas = "";

                if (dataGridVieworder2.Rows.Count == 0)
                {
                    sr.Close();
                    aFile.Close();

                    FileStream wFile = new FileStream(path, FileMode.Create);
                    StreamWriter wsr = new StreamWriter(wFile);
                    wsr.WriteLine(sb.ToString());

                    wsr.Close();
                    wFile.Close();
                    return true;
                }

                if (dataGridVieworder.Rows.Count > 1)
                {
                    if (language == "English")
                    {
                        this.Column5.Items.Clear();
                        this.Column5.Items.AddRange(new object[] {
                                "None",
                                "Processes1",
                                "Processes2",});

                    }
                    if (language == "Chinese")
                    {
                        this.Column5.Items.Clear();
                        this.Column5.Items.AddRange(new object[] {
                                        "无",
                                        "工序一",
                                        "工序二",});
                    }
                    if (language == "English")
                    {
                        this.Column6.Items.Clear();
                        this.Column6.Items.AddRange(new object[] {
                                "Load",
                                "Unload",
                                "Change",
                                "Aoto",});

                    }
                    if (language == "Chinese")
                    {
                        this.Column6.Items.Clear();
                        this.Column6.Items.AddRange(new object[] {
                                        "上料",
                                        "下料",
                                        "换料",
                                        "自动",});
                    }
                }

                for (jj = 0; jj < dataGridVieworder2.Rows.Count; jj++)
                {
                    if (dataGridVieworder2.Rows[jj].Cells[0].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[1].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[2].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[3].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[4].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[5].Value == null)
                    {
                        dataGridVieworder2.Rows.RemoveAt(jj);
                    }
                    else
                    {
                        if (dataGridVieworder2.Rows[jj].Cells[0].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[1].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[2].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[3].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[4].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[5].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[0].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[1].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[2].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[3].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[4].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[5].Value.ToString() == "null")
                        {
                            ;
                        }
                        else
                        {


                            orderdatas = "item=" + dataGridVieworder2.Rows[jj].Cells[0].Value.ToString();
                            orderdatas = orderdatas + ",magno=" + dataGridVieworder2.Rows[jj].Cells[1].Value.ToString();
                            orderdatas = orderdatas + ",fun1=" + dataGridVieworder2.Rows[jj].Cells[2].Value.ToString();
                            orderdatas = orderdatas + ",fun2=" + dataGridVieworder2.Rows[jj].Cells[3].Value.ToString();
                            orderdatas = orderdatas + ",state=" + dataGridVieworder2.Rows[jj].Cells[4].Value.ToString();
                            orderdatas = orderdatas + ",check=" + dataGridVieworder2.Rows[jj].Cells[5].Value.ToString();
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
        public void LoadSetLanguage()
        {
            string lang = ChangeLanguage.GetDefaultLanguage();
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
            if (prelanguage != lang)
            {

                string Default_linetype_value1 = ChangeLanguage.GetString(m_Hashtable, "comoBoxFun1.Items1");
                string Default_linetype_value2 = ChangeLanguage.GetString(m_Hashtable, "comoBoxFun1.Items2");
                string Default_linetype_value3 = ChangeLanguage.GetString(m_Hashtable, "comoBoxFun1.Items3");
                string Default_Material_1 = ChangeLanguage.GetString(m_Hashtable, "Material_1");
                string Default_Material_2 = ChangeLanguage.GetString(m_Hashtable, "Material_2");
                comboBoxFun1.Items.Clear();
                comboBoxFun1.Items.Add(Default_linetype_value1);
                comboBoxFun1.Items.Add(Default_linetype_value2);
                comboBoxFun1.Items.Add(Default_linetype_value3);
                comboBoxFun1.SelectedIndex = 0;
                comboBoxFun2.Items.Clear();
                comboBoxFun2.Items.Add(Default_linetype_value1);
                comboBoxFun2.Items.Add(Default_linetype_value2);
                comboBoxFun2.Items.Add(Default_linetype_value3);
                comboBoxFun2.SelectedIndex = 0;
                prelanguage = lang;
            }
            if (language == "English")
            {
                this.Column5.Items.Clear();
                this.Column5.Items.AddRange(new object[] {
                                "Processes1",
                                "Processes2"});
                this.Column6.Items.Clear();
                this.Column6.Items.AddRange(new object[] {
                                "Load",
                                "Unload",
                                "Change",
                                "Aoto"});

            }
            if (language == "Chinese")
            {
                this.Column5.Items.Clear();
                this.Column5.Items.AddRange(new object[] {
                                        "工序一",
                                        "工序二"});
                this.Column6.Items.Clear();
                this.Column6.Items.AddRange(new object[] {
                                "上料",
                                "下料",
                                "换料",
                                "自动"});
            }
            dataGridVieworder.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column1");
            dataGridVieworder.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column2");
            dataGridVieworder.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column3");
            dataGridVieworder.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column4");
            dataGridVieworder.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column5");
            dataGridVieworder.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column6");
            dataGridVieworder.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column7");
            dataGridVieworder.Columns[7].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column8");
            dataGridVieworder.Columns[8].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column9");

            dataGridVieworder2.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "dataGridViewtextboxColumn1");
            dataGridVieworder2.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "dataGridViewtextboxColumn2");
            dataGridVieworder2.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "dataGridViewtextboxColumn3");
            dataGridVieworder2.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "dataGridViewtextboxColumn4");
            dataGridVieworder2.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "dataGridViewtextboxColumn5");
            dataGridVieworder2.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "dataGridViewtextboxColumn6");



        }

        private void textBox1magno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number");
                }
                else
                    MessageBox.Show("请输入数字");
                textBox1magno.Focus();
            }
        }

        private void textBox1magno_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            if (MagNoStr == "")
            {
                return;
            }
            try
            {
                int MagNo = Convert.ToInt32(MagNoStr);
                if (MagNo > 30 || MagNo <= 0)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("Please enter number between 1 to 30");
                    }
                    else
                        MessageBox.Show("请输入1-30之间的数字");
                    textBox1magno.Focus();
                }
                else
                {

                }
            }
            catch
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number between 1 to 30");
                }
                else
                    MessageBox.Show("请输入1-30之间的数字");
                textBox1magno.Focus();
            }

        }

        #region

        private void Order_Load(object sender, EventArgs e)
        {

            //  ChangeLanguage.LoadLanguage(this);//zxl 4.19
            // language = ChangeLanguage.GetDefaultLanguage();
            comboBoxFun1.SelectedIndex = 0;
            comboBoxFun2.SelectedIndex = 0;


            // LoadSetLanguage();
            //MainForm.languagechangeEvent += LanguageChange;
        }
        #endregion

        void LanguageChange(object sender, string Language)
        {
            LoadSetLanguage();
        }

        private void buttonmadeorder_Click(object sender, EventArgs e)
        {
            string MagNoStr = textBox1magno.Text;
            int MagNo1 = 0;

            if (MagNoStr == "")
            {
                if (language == "English")
                {
                    MessageBox.Show("The Mag is null");
                }
                else
                    MessageBox.Show("料仓编号为空，不能生成订单");
                return;
            }

            try
            {
                MagNo1 = Convert.ToInt16(MagNoStr);
            }
            catch
            {
                if (language == "English")
                {
                    MessageBox.Show("The Mag number is err");
                }
                else
                    MessageBox.Show("料仓编号错误");
                return;
            }
            for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)
            {
                if (dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == MagNoStr)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The Mag have been chosed already,please choose another");
                    }
                    else
                        MessageBox.Show("当前仓位已经绑定工艺，请选择其他料仓");
                    return;
                }
            }

            string fun1string = comboBoxFun1.Text;
            // int fun1no = Convert.ToInt16(fun1string);
            string fun2string = comboBoxFun2.Text;
            // int fun2no = Convert.ToInt16(fun2string);

            int maglength = (int)ModbusTcp.MagLength;

            if (fun1string == fun2string)
            {
                MessageBox.Show("工序相同，不能生成订单");
                return;
            }

            if (fun1string == "None"
                && fun2string == "None")
            {
                MessageBox.Show("The processes is null");
                return;
            }

            int index = dataGridVieworder.Rows.Add();
            dataGridVieworder.Rows[index].Cells[0].Value = dataGridVieworder.Rows.Count;
            // dataGridVieworder.Rows[index].Cells[0].Value = dataGridVieworder.Rows.Count - 1;
            dataGridVieworder.Rows[index].Cells[1].Value = textBox1magno.Text;
            dataGridVieworder.Rows[index].Cells[2].Value = comboBoxFun1.Text;
            dataGridVieworder.Rows[index].Cells[3].Value = comboBoxFun2.Text;
            MainForm.magisordered[MagNo1 - 1] = 1;

            MainForm.ordertate[MagNo1 - 1] = (int)Orderstate.Notstart;
            MainForm.Mag_Check[MagNo1 - 1] = 0;//测量结果初始化为0
            if (language == "English")
            {
                dataGridVieworder.Rows[index].Cells[4].Value = "Processes1";
                dataGridVieworder.Rows[index].Cells[5].Value = "Load";
                dataGridVieworder.Rows[index].Cells[6].Value = "OK";
                dataGridVieworder.Rows[index].Cells[7].Value = "TOP";
                dataGridVieworder.Rows[index].Cells[8].Value = "None";
                dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.LightGreen;
                dataGridVieworder.Rows[index].Cells[7].Style.BackColor = Color.LightGreen;
                if (comboBoxFun1.Text == "None")
                {
                    MainForm.magprocesss1tate[MagNo1 - 1] = (int)Processstate.None;
                }
                else
                {
                    MainForm.magprocesss1tate[MagNo1 - 1] = (int)Processstate.Notstart;
                }
                if (comboBoxFun2.Text == "None")
                {
                    MainForm.magprocesss2tate[MagNo1 - 1] = (int)Processstate.None;
                }
                else
                {
                    MainForm.magprocesss2tate[MagNo1 - 1] = (int)Processstate.Notstart;
                }
            }
            else
            {
                dataGridVieworder.Rows[index].Cells[4].Value = "工序一";
                dataGridVieworder.Rows[index].Cells[5].Value = "上料";
                dataGridVieworder.Rows[index].Cells[6].Value = "确定";
                dataGridVieworder.Rows[index].Cells[7].Value = "置顶";
                dataGridVieworder.Rows[index].Cells[8].Value = "无";

                dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.LightGreen;
                dataGridVieworder.Rows[index].Cells[7].Style.BackColor = Color.LightGreen;
                if (comboBoxFun1.Text == "无")
                {
                    MainForm.magprocesss1tate[MagNo1 - 1] = (int)Processstate.None;
                }
                else
                {
                    MainForm.magprocesss1tate[MagNo1 - 1] = (int)Processstate.Notstart;
                }
                if (comboBoxFun2.Text == "无")
                {
                    MainForm.magprocesss2tate[MagNo1 - 1] = (int)Processstate.None;
                }
                else
                {
                    MainForm.magprocesss2tate[MagNo1 - 1] = (int)Processstate.Notstart;
                }
            }


            int index2 = dataGridVieworder2.Rows.Add();

            //dataGridVieworder2.Rows[index2].Cells[0].Value = dataGridVieworder2.Rows.Count - 1;
            dataGridVieworder2.Rows[index2].Cells[0].Value = dataGridVieworder2.Rows.Count;
            dataGridVieworder2.Rows[index2].Cells[1].Value = dataGridVieworder.Rows[index].Cells[1].Value;
            if (language == "English")
            {
                for (int ii = 2; ii < 4; ii++)
                {
                    string temp = dataGridVieworder.Rows[index].Cells[ii].Value.ToString();
                    if (temp != "None")
                    {
                        dataGridVieworder2.Rows[index2].Cells[ii].Value = "NotStarted";

                    }
                    else
                    {
                        dataGridVieworder2.Rows[index2].Cells[ii].Value = "None";
                    }
                }
                dataGridVieworder2.Rows[index2].Cells[4].Value = "NotDownload";
                dataGridVieworder2.Rows[index2].Cells[5].Value = "None";
            }
            else
            {
                for (int ii = 2; ii < 4; ii++)
                {
                    string temp = dataGridVieworder.Rows[index].Cells[ii].Value.ToString();
                    if (temp != "无")
                    {
                        dataGridVieworder2.Rows[index2].Cells[ii].Value = "未下发";

                    }
                    else
                    {
                        dataGridVieworder2.Rows[index2].Cells[ii].Value = "无";
                    }
                }
                dataGridVieworder2.Rows[index2].Cells[4].Value = "未开始";
                dataGridVieworder2.Rows[index2].Cells[5].Value = "None";
            }

            if (language == "English")
            {
                MessageBox.Show("The order is made");
            }
            else
                MessageBox.Show("当前订单生成成功");
            return;


        }




        /// <summary>
        /// 机床上料
        /// </summary>
        /// <param name="magno">仓位好</param>
        /// <param name="cncno">1车床，2铣床</param>
        /// <returns>返回0，命令下发成功，返回-1命令下发不成功</returns>
        private int CNCLoadfun(int magno, int cncno, ref string message)
        {

            // int ret = -1;

            int index = getmagnorowindex(magno);
            int MesReq_order = (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm;
            int MesReq_order_code = (int)ModbusTcp.MesCommandToPlc.ComProcessManage;
            int Mesreq_Rack_Unload_number = (int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm;
            int Mesreq_Rack_Load_number = (int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm;
            int Mesreq_Order_type = (int)ModbusTcp.DataConfigArr.Order_type_comfirm;
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = magstart + maglength * (magno - 1);

            string language1 = ChangeLanguage.GetDefaultLanguage();
            int magchecki = magstatei - 1;
            if (cncno == 1)
            {
                //if (MainForm.cnclist[0].isConnected() == true)//20180312
                {
                    if (MainForm.cncv2list[0].MagNum == 0 || MainForm.cncv2list[0].MagNum == magno)//机床状态判定，如果机床占用，不可下单 //20180312  
                    {

                        int Gcodeloadreturn = GcodeSenttoCNC(magno, 1);
                        if (Gcodeloadreturn == 1)
                        {

                        }
                        else if (Gcodeloadreturn == -1)
                        {
                            if (language1 == "English")
                            {
                                message = "The Gcode name err!";
                            }
                            else message = "没有找到匹配的G代码!";
                            return -1;
                        }
                        else
                        {
                            if (language1 == "English")
                            {
                                message = "The Gcode download err!";
                            }
                            else message = "G代码下发失败!";
                            return -1;
                        }
                        if (ModbusTcp.Rack_number_write_flage)
                        {
                            if (language1 == "English")
                            {
                                message = "The order before is sentting,please wait a miniter!";
                            }
                            else message = "前一个订单正在下发，请稍后下发下一订单！";
                            return -1;
                        }
                        MainForm.cncv2list[0].MagNum = magno;//机床绑定料仓号 
                        ModbusTcp.Rack_number_write_flage = true;
                        ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                        ModbusTcp.DataMoubus[Mesreq_Rack_Unload_number] = 0;//料仓编号
                        ModbusTcp.DataMoubus[Mesreq_Rack_Load_number] = magno;//料仓编号
                        ModbusTcp.DataMoubus[Mesreq_Order_type] = 1;//加工类型
                        ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                        ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;

                        dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.Gray;
                        if (language1 == "English")
                        {
                            message = "Load succeed！";
                        }
                        else message = "上料命令下发成功！";
                        // magprocesss1tate[magno - 1] = (int)Processstate.Loading;
                        return 0;
                    }
                    else
                    {
                        if (language1 == "English")
                        {
                            message = "The Lathe is processing,can not download！";
                        }
                        else message = "车床有未完成订单，本次订单下达失败！";    //20180312              
                        return -1;
                    }
                }
                //else
                //{
                //    if (language1 == "English")
                //    {
                //        message="The Lathe is off line,can not download";
                //    }
                //    else message="车床离线，上料下达失败!";    //20180312   
                //    return -1;
                //}
            }
            else if (cncno == 2)
            {
                if (MainForm.cncv2list[1].EquipmentState!="离线")//20180312
                {
                    if (MainForm.cncv2list[1].MagNum == 0 /*|| MainForm.cnclist[0].MagNo == magno*/)//机床状态判定，如果机床占用，不可下单 //20180312  
                    {

                        int Gcodeloadreturn = GcodeSenttoCNC(magno, 2);
                        if (Gcodeloadreturn == 1)
                        {

                        }
                        else if (Gcodeloadreturn == -1)
                        {
                            if (language1 == "English")
                            {
                                message = "The Gcode name err!";
                            }
                            else message = "没有找到匹配的G代码!";
                            return -1;
                        }
                        else
                        {
                            if (language1 == "English")
                            {
                                message = "The Gcode download err!";
                            }
                            else message = "G代码下发失败!";
                            return -1;
                        }

                        if (ModbusTcp.Rack_number_write_flage)
                        {
                            if (language1 == "English")
                            {
                                message = "The order before is sentting,please wait a miniter!";
                            }
                            else message = "前一个订单正在下发，请稍后下发下一订单！";
                            return -1;
                        }
                        MainForm.cncv2list[1].MagNum = magno;//机床绑定料仓号 
                        ModbusTcp.Rack_number_write_flage = true;
                        ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                        ModbusTcp.DataMoubus[Mesreq_Rack_Unload_number] = 0;//料仓编号
                        ModbusTcp.DataMoubus[Mesreq_Rack_Load_number] = magno;//料仓编号
                        ModbusTcp.DataMoubus[Mesreq_Order_type] = 2;//加工类型
                        ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                        ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;

                        dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.Gray;
                        if (language1 == "English")
                        {
                            message = "Load succeed！";
                        }
                        else message = "上料命令下发成功！";
                        return 0;
                    }
                    else
                    {
                        if (language1 == "English")
                        {
                            message = "The Lathe is processing,can not download！";
                        }
                        else message = "加工中心有未完成订单，本次订单下达失败！";    //20180312              
                        return -1;
                    }
                }
                else
                {
                    if (language1 == "English")
                    {
                        message = "The Lathe is off line,can not download";
                    }
                    else message = "加工中心离线，上料下达失败!";    //20180312   
                    return -1;
                }
            }
            else
            {
                return -1; ;
            }

            //  return ret;
        }
        /// <summary>
        /// 机床下料
        /// </summary>
        /// <param name="magno"></param>
        /// <param name="cncno"></param>
        /// <returns>返回0，命令下发成功，返回-1命令下发不成功</returns>
        private int CNCUnLoadfun(int magno, int cncno, ref string message)
        {

            int index = getmagnorowindex(magno);

            int MesReq_order = (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm;
            int MesReq_order_code = (int)ModbusTcp.MesCommandToPlc.ComProcessManage;
            int Mesreq_Rack_Unload_number = (int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm;
            int Mesreq_Rack_Load_number = (int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm;
            int Mesreq_Order_type = (int)ModbusTcp.DataConfigArr.Order_type_comfirm;
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = magstart + maglength * (magno - 1);

            string language1 = ChangeLanguage.GetDefaultLanguage();
            int magchecki = magstatei - 1;
            if (cncno == 1)//车床
            {
                if (MainForm.cncv2list[0].EquipmentState != "离线")//20180312
                {
                    if (MainForm.cncv2list[0].MagNum == 0 || MainForm.cncv2list[0].MagNum == magno)//机床状态判定，如果机床占用，不可下单 //20180312  
                    {

                        if (ModbusTcp.Rack_number_write_flage)
                        {
                            if (language1 == "English")
                            {
                                message = "The order before is sentting,please wait a miniter!";
                            }
                            else message = "前一个订单正在下发，请稍后下发下一订单！";
                            return -1;
                        }
                        MainForm.cncv2list[0].MagNum = magno;//机床绑定料仓号 
                        ModbusTcp.Rack_number_write_flage = true;
                        ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                        ModbusTcp.DataMoubus[Mesreq_Rack_Unload_number] = magno;//料仓编号
                        ModbusTcp.DataMoubus[Mesreq_Rack_Load_number] = 0;//料仓编号
                        ModbusTcp.DataMoubus[Mesreq_Order_type] = 1;//加工类型
                        ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                        ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                                                            //  ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;

                        // magprocesss1tate[magno - 1] = (int)Processstate.Unloading;
                        dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.Gray;
                        if (language1 == "English")
                        {
                            message = "Unload succeed！";
                        }
                        else message = "下料命令下发成功！";
                        return 0;
                    }
                    else
                    {
                        if (language1 == "English")
                        {
                            message = "The Lathe is processing,can not download！";
                        }
                        else message = "车床有未完成订单，本次订单下达失败！";    //20180312              
                        return -1;
                    }
                }
                else
                {
                    if (language1 == "English")
                    {
                        message = "The Lathe is off line,can not download";
                    }
                    else message = "车床离线，下料下达失败!";    //20180312   
                    return -1;
                }
            }
            else if (cncno == 2)
            {
                if (MainForm.cncv2list[1].EquipmentState != "离线")//20180312
                {
                    if (MainForm.cncv2list[1].MagNum == 0 || MainForm.cncv2list[1].MagNum == magno)//机床状态判定，如果机床占用，不可下单 //20180312  
                    {

                        if (ModbusTcp.Rack_number_write_flage)
                        {
                            if (language1 == "English")
                            {
                                message = "The order before is sentting,please wait a miniter!";
                            }
                            else message = "前一个订单正在下发，请稍后下发下一订单！";
                            return -1;
                        }
                        MainForm.cncv2list[1].MagNum = magno;//机床绑定料仓号 
                        ModbusTcp.Rack_number_write_flage = true;
                        ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                        ModbusTcp.DataMoubus[Mesreq_Rack_Unload_number] = magno;//料仓编号
                        ModbusTcp.DataMoubus[Mesreq_Rack_Load_number] = 0;//料仓编号
                        ModbusTcp.DataMoubus[Mesreq_Order_type] = 2;//加工类型
                        ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                        ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   

                        OrderForm1.ReProcessChoose = false;

                        OrderForm1.rerunningflage = false;
                        dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.Gray;
                        if (language1 == "English")
                        {
                            message = "Load succeed！";
                        }
                        else message = "下料命令下发成功！";
                        return 0;
                    }
                    else
                    {
                        if (language1 == "English")
                        {
                            message = "The Lathe is processing,can not download！";
                        }
                        else message = "加工中心有未完成订单，本次订单下达失败！";    //20180312              
                        return -1;
                    }
                }
                else
                {
                    if (language1 == "English")
                    {
                        message = "The Lathe is off line,can not download";
                    }
                    else message = "加工中心离线，下料下达失败!";    //20180312   
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 机床换料
        /// </summary>
        /// <param name="magno">要换上的料位号</param>
        /// <param name="cncno">1车床，2铣床</param>
        /// <returns>返回0，命令下发成功，返回-1命令下发不成功</returns>
        private int CNCChangefun(int magno, int cncno, ref string message)
        {
            // int ret = -1;

            int index = getmagnorowindex(magno);

            int MesReq_order = (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm;
            int MesReq_order_code = (int)ModbusTcp.MesCommandToPlc.ComProcessManage;
            int Mesreq_Rack_Unload_number = (int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm;
            int Mesreq_Rack_Load_number = (int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm;
            int Mesreq_Order_type = (int)ModbusTcp.DataConfigArr.Order_type_comfirm;
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = magstart + maglength * (magno - 1);

            string language1 = ChangeLanguage.GetDefaultLanguage();
            int magchecki = magstatei - 1;
            if (cncno == 1)
            {
                if (MainForm.cncv2list[0].EquipmentState != "离线")//20180312
                {
                    int cnccurmag = MainForm.cncv2list[0].MagNum;//当前机床的料位号
                    string cnccurmags = cnccurmag.ToString();
                    int rowindex = -1;
                    if (MainForm.cncv2list[0].MagNum == 0)
                    {
                        if (language1 == "English")
                        {
                            message = "The Lathe has no order,please choose Load!";
                        }
                        else message = "当前车床没有订单，请选择上料!";
                        return -1;
                    }
                    for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)
                    {
                        if (dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == cnccurmags)
                        {
                            rowindex = jj;
                        }
                    }
                    if (rowindex == -1)
                    {
                        if (language1 == "English")
                        {
                            message = "message err!";
                        }
                        else message = "车床与订单信息不符";
                        return -1;
                    }
                    if (dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString() == "车工序" || dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString() == "Lathe")
                    {

                        if (MainForm.magprocesss1tate[cnccurmag - 1] == (int)Processstate.Runned)//上料完成
                        {
                            int Gcodeloadreturn = GcodeSenttoCNC(magno, 1);
                            if (Gcodeloadreturn == 1)
                            {
                                ;
                            }
                            else if (Gcodeloadreturn == -1)
                            {
                                if (language1 == "English")
                                {
                                    message = "The Gcode name err!";
                                }
                                else message = "没有找到匹配的G代码!";
                                return -1;

                            }
                            else
                            {
                                if (language1 == "English")
                                {
                                    message = "The Gcode download err!";
                                }
                                else message = "G代码下发失败!";
                                return -1;
                            }

                            if (ModbusTcp.Rack_number_write_flage)
                            {
                                if (language1 == "English")
                                {
                                    message = "The order before is sentting,please wait a miniter!";
                                }
                                else message = "前一个订单正在下发，请稍后下发下一订单！";
                                return -1;
                            }
                            ModbusTcp.Rack_number_write_flage = true;
                            ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                            ModbusTcp.DataMoubus[Mesreq_Rack_Unload_number] = cnccurmag;//料仓编号
                            ModbusTcp.DataMoubus[Mesreq_Rack_Load_number] = magno;//料仓编号
                            ModbusTcp.DataMoubus[Mesreq_Order_type] = 1;//加工类型
                            ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                            ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                            ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;

                            dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.Gray;
                            if (MainForm.magprocesss1tate[cnccurmag - 1] == (int)Processstate.Runned)
                            {
                                MainForm.magprocesss1tate[cnccurmag - 1] = (int)Processstate.Unloading;
                            }
                            else if (MainForm.magprocesss2tate[cnccurmag - 1] == (int)Processstate.Runned)
                            {

                                MainForm.magprocesss2tate[cnccurmag - 1] = (int)Processstate.Unloading;
                            }
                            //  magprocesss1tate[magno - 1] = (int)Processstate.Loading;

                            MainForm.cncv2list[0].MagNum = magno;//机床绑定料仓号 
                            if (language1 == "English")
                            {
                                message = "Load succeed！";
                            }
                            else message = "换料命令下发成功！";

                            return 0;
                        }
                        else
                        {
                            if (language1 == "English")
                            {
                                message = "The Lathe is not loaded state!";
                            }
                            else message = "车床不是加工完成状态，不允许换料!";
                            return -1;
                        }

                    }
                    else if (dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString() == "车工序" || dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString() == "Lathe")
                    {

                        if (MainForm.magprocesss2tate[cnccurmag - 1] == (int)Processstate.Runned)//上料完成
                        {
                            int Gcodeloadreturn = GcodeSenttoCNC(magno, 1);
                            if (Gcodeloadreturn == 1)
                            {
                                ;
                            }
                            else if (Gcodeloadreturn == -1)
                            {
                                if (language1 == "English")
                                {
                                    message = "The Gcode name err!";
                                }
                                else message = "没有找到匹配的G代码!";
                                return -1;

                            }
                            else
                            {
                                if (language1 == "English")
                                {
                                    message = "The Gcode download err!";
                                }
                                else message = "G代码下发失败!";
                                return -1;
                            }

                            if (ModbusTcp.Rack_number_write_flage)
                            {
                                if (language1 == "English")
                                {
                                    message = "The order before is sentting,please wait a miniter!";
                                }
                                else message = "前一个订单正在下发，请稍后下发下一订单！";
                                return -1;
                            }
                            ModbusTcp.Rack_number_write_flage = true;
                            ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                            ModbusTcp.DataMoubus[Mesreq_Rack_Unload_number] = cnccurmag;//料仓编号
                            ModbusTcp.DataMoubus[Mesreq_Rack_Load_number] = magno;//料仓编号
                            ModbusTcp.DataMoubus[Mesreq_Order_type] = 1;//加工类型
                            ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                            ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                            ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;

                            dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.Gray;
                            MainForm.cncv2list[0].MagNum = magno;//机床绑定料仓号 
                            if (language1 == "English")
                            {
                                message = "Load succeed！";
                            }
                            else message = "换料命令下发成功！";
                            if (MainForm.magprocesss1tate[cnccurmag - 1] == (int)Processstate.Runned)
                            {
                                MainForm.magprocesss1tate[cnccurmag - 1] = (int)Processstate.Unloading;
                            }
                            else if (MainForm.magprocesss2tate[cnccurmag - 1] == (int)Processstate.Runned)
                            {

                                MainForm.magprocesss2tate[cnccurmag - 1] = (int)Processstate.Unloading;
                            }

                            //  magprocesss2tate[magno - 1] = (int)Processstate.Loading;
                            return 0;
                        }
                        else
                        {
                            if (language1 == "English")
                            {
                                message = "The Lathe is not loaded state!";
                            }
                            else message = "车床不是加工完成状态，不允许换料!";
                            return -1;
                        }

                    }
                    else
                    {
                        if (language1 == "English")
                        {
                            message = "message err!";
                        }
                        else message = "车床与订单信息不符";
                        return -1;
                    }
                }
                else
                {
                    if (language1 == "English")
                    {
                        message = "The Lathe is off line,can not download";
                    }
                    else message = "车床离线，上料下达失败!";    //20180312   
                    return -1;
                }
            }
            else if (cncno == 2)
            {
                if (MainForm.cncv2list[1].EquipmentState != "离线")//20180312
                {
                    int cnccurmag = MainForm.cncv2list[1].MagNum;//当前机床的料位号
                    string cnccurmags = cnccurmag.ToString();
                    int rowindex = -1;
                    if (MainForm.cncv2list[1].MagNum == 0)
                    {
                        if (language1 == "English")
                        {
                            message = "The CNChas no order,please choose Load!";
                        }
                        else message = "当前加工中心没有订单，请选择上料!";
                        return -1;
                    }
                    for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)
                    {
                        if (dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == cnccurmags)
                        {
                            rowindex = jj;
                        }
                    }
                    if (rowindex == -1)
                    {
                        if (language1 == "English")
                        {
                            message = "message err!";
                        }
                        else message = "加工中心与订单信息不符";
                        return -1;
                    }
                    if (dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString() == "铣工序" || dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString() == "CNC")
                    {

                        if (MainForm.magprocesss1tate[cnccurmag - 1] == (int)Processstate.Runned)//上料完成
                        {
                            int Gcodeloadreturn = GcodeSenttoCNC(magno, 2);
                            if (Gcodeloadreturn == 1)
                            {
                                ;
                            }
                            else if (Gcodeloadreturn == -1)
                            {
                                if (language1 == "English")
                                {
                                    message = "The Gcode name err!";
                                }
                                else message = "没有找到匹配的G代码!";
                                return -1;

                            }
                            else
                            {
                                if (language1 == "English")
                                {
                                    message = "The Gcode download err!";
                                }
                                else message = "G代码下发失败!";
                                return -1;
                            }

                            if (ModbusTcp.Rack_number_write_flage)
                            {
                                if (language1 == "English")
                                {
                                    message = "The order before is sentting,please wait a miniter!";
                                }
                                else message = "前一个订单正在下发，请稍后下发下一订单！";
                                return -1;
                            }
                            ModbusTcp.Rack_number_write_flage = true;
                            ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                            ModbusTcp.DataMoubus[Mesreq_Rack_Unload_number] = cnccurmag;//料仓编号
                            ModbusTcp.DataMoubus[Mesreq_Rack_Load_number] = magno;//料仓编号
                            ModbusTcp.DataMoubus[Mesreq_Order_type] = 2;//加工类型
                            ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                            ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                            ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;


                            OrderForm1.ReProcessChoose = false;

                            OrderForm1.rerunningflage = false;
                            dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.Gray;
                            MainForm.cncv2list[1].MagNum = magno;//机床绑定料仓号 
                            if (language1 == "English")
                            {
                                message = "Load succeed！";
                            }
                            else message = "换料料命令下发成功！";

                            if (MainForm.magprocesss1tate[cnccurmag - 1] == (int)Processstate.Runned)
                            {
                                MainForm.magprocesss1tate[cnccurmag - 1] = (int)Processstate.Unloading;
                            }
                            else if (MainForm.magprocesss2tate[cnccurmag - 1] == (int)Processstate.Runned)
                            {

                                MainForm.magprocesss2tate[cnccurmag - 1] = (int)Processstate.Unloading;
                            }

                            return 0;

                        }
                        else
                        {
                            if (language1 == "English")
                            {
                                message = "The Lathe is not loaded state!";
                            }
                            else message = "加工中心不是加工完成状态，不允许换料!";
                            return -1;
                        }
                    }
                    else if (dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString() == "铣工序" || dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString() == "CNC")
                    {

                        if (MainForm.magprocesss2tate[cnccurmag - 1] == (int)Processstate.Runned)//上料完成
                        {
                            int Gcodeloadreturn = GcodeSenttoCNC(magno, 2);
                            if (Gcodeloadreturn == 1)
                            {
                                ;
                            }
                            else if (Gcodeloadreturn == -1)
                            {
                                if (language1 == "English")
                                {
                                    message = "The Gcode name err!";
                                }
                                else message = "没有找到匹配的G代码!";
                                return -1;

                            }
                            else
                            {
                                if (language1 == "English")
                                {
                                    message = "The Gcode download err!";
                                }
                                else message = "G代码下发失败!";
                                return -1;
                            }

                            if (ModbusTcp.Rack_number_write_flage)
                            {
                                if (language1 == "English")
                                {
                                    message = "The order before is sentting,please wait a miniter!";
                                }
                                else message = "前一个订单正在下发，请稍后下发下一订单！";
                                return -1;
                            }
                            ModbusTcp.Rack_number_write_flage = true;
                            ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                            ModbusTcp.DataMoubus[Mesreq_Rack_Unload_number] = cnccurmag;//料仓编号
                            ModbusTcp.DataMoubus[Mesreq_Rack_Load_number] = magno;//料仓编号
                            ModbusTcp.DataMoubus[Mesreq_Order_type] = 2;//加工类型
                            ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                            ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                            ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;

                            dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.Gray;
                            MainForm.cncv2list[1].MagNum = magno;//机床绑定料仓号 
                            if (language1 == "English")
                            {
                                message = "Load succeed！";
                            }
                            else message = "换料命令下发成功！";
                            if (MainForm.magprocesss1tate[cnccurmag - 1] == (int)Processstate.Runned)
                            {
                                MainForm.magprocesss1tate[cnccurmag - 1] = (int)Processstate.Unloading;
                            }
                            else if (MainForm.magprocesss2tate[cnccurmag - 1] == (int)Processstate.Runned)
                            {

                                MainForm.magprocesss2tate[cnccurmag - 1] = (int)Processstate.Unloading;
                            }

                            OrderForm1.ReProcessChoose = false;

                            OrderForm1.rerunningflage = false;

                            return 0;
                        }
                        else
                        {
                            if (language1 == "English")
                            {
                                message = "The Lathe is not loaded state!";
                            }
                            else message = "加工中心不是加工完成状态，不允许换料!";


                            return -1;
                        }

                    }
                    else
                    {
                        if (language1 == "English")
                        {
                            message = "message err!";
                        }
                        else message = "加工中心与订单信息不符";
                        return -1;
                    }
                }
                else
                {
                    if (language1 == "English")
                    {
                        message = "The Lathe is off line,can not download";
                    }
                    else message = "加工中心离线，上料下达失败!";    //20180312   
                    return -1;
                }
            }
            else
            {
                return -1;
            }

        }

        private void dataGridVieworder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dataGridVieworder.Rows.Count)
            {
                DataGridViewColumn column = dataGridVieworder.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                    //行列都是0开始计算，表头不算， MessageBox.Show("x: " + e.RowIndex.ToString() + ",y:" + e.ColumnIndex.ToString());
                    if (e.ColumnIndex == 6)//确定按钮按下//BackColor = System.Drawing.Color.LightGreen;
                    {
                        int ret = -1;
                        string messagestring = "";
                        //dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gray;
                        string MagNoStr = dataGridVieworder.Rows[e.RowIndex].Cells[1].Value.ToString();
                        int MagNo = Convert.ToInt16(MagNoStr);
                        int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
                        int maglength = (int)ModbusTcp.MagLength;
                        int magstatei = magstart + maglength * (MagNo - 1);
                        int robortishome = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm];

                        if (manmode == false)
                        {
                            if (language == "English")
                            {
                                MessageBox.Show("Please turn off the aotomatic scheduling");
                            }
                            else MessageBox.Show("请关闭自动排程功能！");
                            return;
                        }
                        if (MainForm.PLC_SIMES_ON_line == false)
                        {
                            if (language == "English")
                            {
                                MessageBox.Show("Load failure,because PLC is off line！");
                            }
                            else MessageBox.Show("PLC未连接，不能下达订单!");
                            return;
                        }
                        if (!MainForm.linestop)//产线停止状态
                        {
                            if (language == "English")
                            {
                                MessageBox.Show("Load failure,Please start line!");
                            }
                            else MessageBox.Show("订单下达失败，请先启动产线!");
                            return;
                        }
                        if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statenull)
                        {
                            if (language == "English")
                            {
                                MessageBox.Show("Current Mag is empty ,Please make a new Order!");
                            }
                            else MessageBox.Show("指定仓位无料，请重新下达订单!");
                            return;
                        }
                        if (ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_speed] == 1)
                        {
                            if (language == "English")
                            {
                                MessageBox.Show("The robot is busy,please down the order later!");
                            }
                            else MessageBox.Show("机器人繁忙，请稍后下订单!");
                            return;
                        }
                        if (robortishome == 0)
                        {
                            if (language == "English")
                            {
                                MessageBox.Show("Robort is not at home!");
                            }
                            else MessageBox.Show("机器人不在HOME位置无法下达订单！");
                            return;
                        }
                        if (language == "English")
                        {
                            if (dataGridVieworder.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.LightGreen)//订单未下达
                            {
                                string magnumbers = dataGridVieworder2.Rows[e.RowIndex].Cells[1].Value.ToString();
                                int magnumber = Convert.ToInt32(magnumbers);
                                if (dataGridVieworder.Rows[e.RowIndex].Cells[4].Value.ToString() == "Processes1")
                                {
                                    //订单逻辑是否满足
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "Load")
                                    {
                                        if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Notstart)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "Lathe")
                                            {

                                                ret = CNCLoadfun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Loading;
                                                }

                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "CNC")
                                            {
                                                ret = CNCLoadfun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "Process 1 is null";
                                            }

                                        }
                                        else
                                        {
                                            messagestring = "Process 1 already Loaded";
                                        }
                                    }

                                    else if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "UnLoad")
                                    {
                                        if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Runned)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "Lathe")
                                            {
                                                ret = CNCUnLoadfun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Unloading;
                                                }

                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "CNC")
                                            {
                                                ret = CNCUnLoadfun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Unloading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "Process 1 is null";
                                            }

                                        }
                                        else if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Notstart
                                            || MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Loading)
                                        {
                                            messagestring = "Process 1 have not loaded";
                                        }
                                        else if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Running)
                                        {
                                            messagestring = "Process 1 is running";
                                        }
                                        else if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Unloading
                                           || MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Unloaded)
                                        {
                                            messagestring = "Process 1 already Unloaded";
                                        }
                                    }
                                    else if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "Change")
                                    {
                                        if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Notstart)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "Lathe")
                                            {
                                                ret = CNCChangefun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "CNC")
                                            {
                                                ret = CNCChangefun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "Process 1 is null";
                                            }
                                        }
                                        else
                                        {
                                            messagestring = "Process 1 already Loaded";
                                        }
                                    }

                                }
                                else if (dataGridVieworder.Rows[e.RowIndex].Cells[4].Value.ToString() == "Processes2")
                                {

                                    //订单逻辑是否满足
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "Load")
                                    {
                                        if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Notstart)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "Lathe")
                                            {
                                                ret = CNCLoadfun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "CNC")
                                            {
                                                ret = CNCLoadfun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Loading;
                                                }

                                            }
                                            else
                                            {
                                                messagestring = "Processes 2 is null";
                                            }

                                        }
                                        else
                                        {
                                            messagestring = "Processes 2 already Loaded";
                                        }
                                    }

                                    else if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "UnLoad")
                                    {
                                        if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Runned)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "Lathe")
                                            {
                                                ret = CNCUnLoadfun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Unloading;
                                                }

                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "CNC")
                                            {
                                                ret = CNCUnLoadfun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Unloading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "Processes 2 is null";
                                            }

                                        }
                                        else if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Notstart
                                            || MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Loading)
                                        {
                                            messagestring = "Processes 2 have not loaded";
                                        }
                                        else if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Running)
                                        {
                                            messagestring = "Processes 2 is running";
                                        }
                                        else if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Unloading
                                           || MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Unloaded)
                                        {
                                            messagestring = "Processes 2 already Unloaded";
                                        }
                                    }
                                    else if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "Change")
                                    {
                                        if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Notstart)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "Lathe")
                                            {
                                                ret = CNCChangefun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "CNC")
                                            {
                                                ret = CNCChangefun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "Processes 2 is null";
                                            }
                                        }
                                        else
                                        {
                                            messagestring = "Processes 2 already Loaded";
                                        }
                                    }
                                }

                                //for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)


                                if (ret == 0)//操作成功
                                {
                                    dataGridVieworder.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Gray;

                                }
                                if (ret == -1)
                                {
                                    MessageBox.Show(messagestring);
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else  //中文
                        {
                            Color color1;
                            color1 = dataGridVieworder.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
                            if (dataGridVieworder.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.LightGreen)//订单未下达
                            {
                                string magnumbers = dataGridVieworder2.Rows[e.RowIndex].Cells[1].Value.ToString();
                                int magnumber = Convert.ToInt32(magnumbers);
                                if (dataGridVieworder.Rows[e.RowIndex].Cells[4].Value.ToString() == "工序一")
                                {
                                    //订单逻辑是否满足
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "上料")
                                    {
                                        if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Notstart)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "车工序")
                                            {

                                                ret = CNCLoadfun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Loading;
                                                }

                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "铣工序")
                                            {
                                                ret = CNCLoadfun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "工序1为空，操作不成功";
                                            }

                                        }
                                        else
                                        {
                                            messagestring = "工序1已经上料，操作不成功";
                                        }
                                    }

                                    else if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "下料")
                                    {
                                        if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Runned)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "车工序")
                                            {
                                                ret = CNCUnLoadfun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Unloading;
                                                }

                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "铣工序")
                                            {
                                                ret = CNCUnLoadfun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Unloading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "工序1为空，操作不成功";
                                            }

                                        }
                                        else if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Notstart
                                            || MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Loading)
                                        {
                                            messagestring = "工序1未上料完成，操作不成功";
                                        }
                                        else if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Running)
                                        {
                                            messagestring = "工序1正在加工";
                                        }
                                        else if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Unloading
                                           || MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Unloaded)
                                        {
                                            messagestring = "工序1已经下料，操作不成功";
                                        }
                                    }
                                    else if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "换料")
                                    {
                                        if (MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Notstart)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "车工序")
                                            {
                                                ret = CNCChangefun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "铣工序")
                                            {
                                                ret = CNCChangefun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss1tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "工序1为空，操作不成功";
                                            }
                                        }
                                        else
                                        {
                                            messagestring = "工序1已经上料，操作不成功";
                                        }
                                    }

                                }
                                else if (dataGridVieworder.Rows[e.RowIndex].Cells[4].Value.ToString() == "工序二")
                                {

                                    //订单逻辑是否满足
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "上料")
                                    {
                                        if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Notstart)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "车工序")
                                            {
                                                ret = CNCLoadfun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "铣工序")
                                            {
                                                ret = CNCLoadfun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Loading;
                                                }

                                            }
                                            else
                                            {
                                                messagestring = "工序2为空，操作不成功";
                                            }

                                        }
                                        else
                                        {
                                            messagestring = "工序2已经上料，操作不成功";
                                        }
                                    }

                                    else if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "下料")
                                    {
                                        if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Runned)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "车工序")
                                            {
                                                ret = CNCUnLoadfun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Unloading;
                                                }

                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "铣工序")
                                            {
                                                ret = CNCUnLoadfun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Unloading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "工序2为空，操作不成功";
                                            }

                                        }
                                        else if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Notstart
                                            || MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Loading)
                                        {
                                            messagestring = "工序2未上料完成，操作不成功";
                                        }
                                        else if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Running)
                                        {
                                            messagestring = "工序2正在加工";
                                        }
                                        else if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Unloading
                                           || MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Unloaded)
                                        {
                                            messagestring = "工序2已经下料，操作不成功";
                                        }
                                    }
                                    else if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "换料")
                                    {
                                        if (MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Notstart)
                                        {
                                            if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "车工序")
                                            {
                                                ret = CNCChangefun(magnumber, 1, ref messagestring);//车床上料 ;//机床是否能执行
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "铣工序")
                                            {
                                                ret = CNCChangefun(magnumber, 2, ref messagestring);//铣床上料 ;
                                                if (ret == 0)
                                                {
                                                    MainForm.magprocesss2tate[magnumber - 1] = (int)Processstate.Loading;
                                                }
                                            }
                                            else
                                            {
                                                messagestring = "工序2为空，操作不成功";
                                            }
                                        }
                                        else
                                        {
                                            messagestring = "工序2已经上料，操作不成功";
                                        }
                                    }
                                }

                                if (ret == 0)//操作成功
                                {
                                    dataGridVieworder.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Gray;
                                    MessageBox.Show(messagestring);
                                }
                                if (ret == -1)
                                {
                                    MessageBox.Show(messagestring);
                                }
                            }
                            else if (e.ColumnIndex == 7)//置顶
                            {
                                return;
                            }
                        }

                    }
                    else if (e.ColumnIndex == 7)//置顶
                    {
                        for (int ii = 0; ii < dataGridVieworder.RowCount; ii++)
                        {
                            if (MainForm.ordertate[ii] == (int)Orderstate.Processing)
                            {
                                return; //如果预订单正在进行，不允许置顶
                            }
                        }
                        //订单置顶，其他顺序往下
                        TopRangeDataGridView(e.RowIndex);
                    }
                }

            }
        }


        private void TopRangeDataGridView(int rowindex)
        {
            string OrderNoStr = dataGridVieworder.Rows[rowindex].Cells[1].Value.ToString();
            if (OrderNoStr == "")
            {
                return;
            }
            int OrderNo = Convert.ToInt16(OrderNoStr);

            string item0 = dataGridVieworder.Rows[rowindex].Cells[0].Value.ToString();
            string item1 = dataGridVieworder.Rows[rowindex].Cells[1].Value.ToString();
            string item2 = dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString();
            string item3 = dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString();
            string item4 = dataGridVieworder.Rows[rowindex].Cells[4].Value.ToString();
            string item5 = dataGridVieworder.Rows[rowindex].Cells[5].Value.ToString();
            string item6 = dataGridVieworder.Rows[rowindex].Cells[6].Value.ToString();
            string item7 = dataGridVieworder.Rows[rowindex].Cells[7].Value.ToString();
            string item8 = dataGridVieworder.Rows[rowindex].Cells[8].Value.ToString();


            string itema0 = dataGridVieworder2.Rows[rowindex].Cells[0].Value.ToString();
            string itema1 = dataGridVieworder2.Rows[rowindex].Cells[1].Value.ToString();
            string itema2 = dataGridVieworder2.Rows[rowindex].Cells[2].Value.ToString();
            string itema3 = dataGridVieworder2.Rows[rowindex].Cells[3].Value.ToString();
            string itema4 = dataGridVieworder2.Rows[rowindex].Cells[4].Value.ToString();
            string itema5 = dataGridVieworder2.Rows[rowindex].Cells[5].Value.ToString();
            dataGridVieworder2.Rows.RemoveAt(rowindex);
            dataGridVieworder.Rows.RemoveAt(rowindex);
            int itemcount = 0;
            for (int jj = 0; jj < dataGridVieworder.Rows.Count; jj++)
            {
                itemcount = jj + 2;
                dataGridVieworder.Rows[jj].Cells[0].Value = itemcount.ToString();

            }
            for (int jj = 0; jj < dataGridVieworder2.Rows.Count; jj++)
            {
                itemcount = jj + 2;
                dataGridVieworder2.Rows[jj].Cells[0].Value = itemcount.ToString();
            }

            dataGridVieworder2.Rows.Add();
            dataGridVieworder.Rows.Add();
            for (int jj = dataGridVieworder.Rows.Count - 1; jj > 0; jj--)
            {
                itemcount = jj;
                dataGridVieworder.Rows[jj].Cells[0].Value = (jj + 1).ToString();
                dataGridVieworder.Rows[jj].Cells[1].Value = dataGridVieworder.Rows[jj - 1].Cells[1].Value.ToString();
                dataGridVieworder.Rows[jj].Cells[2].Value = dataGridVieworder.Rows[jj - 1].Cells[2].Value.ToString();
                dataGridVieworder.Rows[jj].Cells[3].Value = dataGridVieworder.Rows[jj - 1].Cells[3].Value.ToString();
                dataGridVieworder.Rows[jj].Cells[4].Value = dataGridVieworder.Rows[jj - 1].Cells[4].Value.ToString();
                dataGridVieworder.Rows[jj].Cells[5].Value = dataGridVieworder.Rows[jj - 1].Cells[5].Value.ToString();
                dataGridVieworder.Rows[jj].Cells[6].Value = dataGridVieworder.Rows[jj - 1].Cells[6].Value.ToString();
                dataGridVieworder.Rows[jj].Cells[7].Value = dataGridVieworder.Rows[jj - 1].Cells[7].Value.ToString();
                dataGridVieworder.Rows[jj].Cells[8].Value = dataGridVieworder.Rows[jj - 1].Cells[8].Value.ToString();

            }
            dataGridVieworder.Rows[0].Cells[0].Value = "1";
            dataGridVieworder.Rows[0].Cells[1].Value = item1;
            dataGridVieworder.Rows[0].Cells[2].Value = item2;
            dataGridVieworder.Rows[0].Cells[3].Value = item3;
            dataGridVieworder.Rows[0].Cells[4].Value = item4;
            dataGridVieworder.Rows[0].Cells[5].Value = item5;
            dataGridVieworder.Rows[0].Cells[6].Value = item6;
            dataGridVieworder.Rows[0].Cells[7].Value = item7;
            dataGridVieworder.Rows[0].Cells[8].Value = item8;
            for (int jj = dataGridVieworder2.Rows.Count - 1; jj > 0; jj--)
            {
                itemcount = jj;
                dataGridVieworder2.Rows[jj].Cells[0].Value = (jj + 1).ToString();
                dataGridVieworder2.Rows[jj].Cells[1].Value = dataGridVieworder2.Rows[jj - 1].Cells[1].Value.ToString();
                dataGridVieworder2.Rows[jj].Cells[2].Value = dataGridVieworder2.Rows[jj - 1].Cells[2].Value.ToString();
                dataGridVieworder2.Rows[jj].Cells[3].Value = dataGridVieworder2.Rows[jj - 1].Cells[3].Value.ToString();
                dataGridVieworder2.Rows[jj].Cells[4].Value = dataGridVieworder2.Rows[jj - 1].Cells[4].Value.ToString();
                dataGridVieworder2.Rows[jj].Cells[5].Value = dataGridVieworder2.Rows[jj - 1].Cells[5].Value.ToString();

            }
            dataGridVieworder2.Rows[0].Cells[0].Value = "1";
            dataGridVieworder2.Rows[0].Cells[1].Value = itema1;
            dataGridVieworder2.Rows[0].Cells[2].Value = itema2;
            dataGridVieworder2.Rows[0].Cells[3].Value = itema3;
            dataGridVieworder2.Rows[0].Cells[4].Value = itema4;
            dataGridVieworder2.Rows[0].Cells[5].Value = itema5;

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

        //private bool InitOrderdata(string path)
        //{
        //    try
        //    {
        //        FileStream aFile = new FileStream(path, FileMode.Open);
        //        StreamReader sr = new StreamReader(aFile);

        //        string item = "";
        //        string magno = "";
        //        string fun1 = "";
        //        string fun2 = "";
        //        string fun3 = "";
        //        string fun4 = "";
        //        string down = "";
        //        string backfun = "";
        //        string backdown = "";
        //        string line;
        //        int ii = 0;
        //        line = sr.ReadLine();

        //        while (line != null && line != "")
        //        {
        //            dataGridVieworder.Rows.Add();
        //            item = getvalueformstring(line, "item=");
        //            magno = getvalueformstring(line, "magno=");
        //            fun1 = getvalueformstring(line, "fun1=");
        //            fun2 = getvalueformstring(line, "fun2=");
        //            fun3 = getvalueformstring(line, "fun3=");
        //            fun4 = getvalueformstring(line, "fun4=");
        //            down = getvalueformstring(line, "down=");
        //            backfun = getvalueformstring(line, "backfun=");
        //            backdown = getvalueformstring(line, "backdown=");
        //            if (item == "null" || magno == "null" || fun1 == "null" || fun2 == "null" || fun3 == "null" || fun4 == "null" || down == "null" || backfun == "null" || backdown == "null")
        //            {
        //                line = sr.ReadLine();

        //            }
        //            else
        //            {
        //                dataGridVieworder.Rows[ii].Cells[0].Value = item;
        //                dataGridVieworder.Rows[ii].Cells[1].Value = magno;
        //                dataGridVieworder.Rows[ii].Cells[2].Value = fun1;
        //                dataGridVieworder.Rows[ii].Cells[3].Value = fun2;
        //                dataGridVieworder.Rows[ii].Cells[4].Value = fun3;
        //                dataGridVieworder.Rows[ii].Cells[5].Value = fun4;
        //                if (language == "English")
        //                {
        //                    dataGridVieworder.Rows[ii].Cells[6].Value = "OK";
        //                    if (down == "Yes")
        //                    {
        //                        dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.Gray;
        //                    }
        //                    dataGridVieworder.Rows[ii].Cells[7].Value = backfun;
        //                    dataGridVieworder.Rows[ii].Cells[8].Value = "OK";
        //                    if (backdown == "Yes")
        //                    {
        //                        dataGridVieworder.Rows[ii].Cells[8].Style.BackColor = Color.Gray;
        //                    }
        //                }
        //                else
        //                {
        //                    dataGridVieworder.Rows[ii].Cells[6].Value = "确定";
        //                    if (down == "是")
        //                    {
        //                        dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.Gray;
        //                    }
        //                    dataGridVieworder.Rows[ii].Cells[7].Value = backfun;
        //                    dataGridVieworder.Rows[ii].Cells[8].Value = "确定";
        //                    if (backdown == "是")
        //                    {
        //                        dataGridVieworder.Rows[ii].Cells[8].Style.BackColor = Color.Gray;
        //                    }
        //                }


        //                line = sr.ReadLine();
        //                ii++;
        //            }


        //        }

        //        sr.Close();
        //        aFile.Close();
        //        return true;
        //    }
        //    catch (IOException e)
        //    {
        //        return false;
        //    }
        //}

        public static int GcodeSenttoCNC(int MagNo, int gongyiNo)
        {
            string gcoderoad = GcodeFilePath;
            string gcodename = "";
            if (MagNo == 0)
            {
                if (gongyiNo == 1)//车床
                {
                    gcodename = "OHOMEL.nc";
                    gcoderoad = GcodeFilePath + "\\" + gcodename;
                    if (!File.Exists(@gcoderoad))
                    {
                        return -1;
                    }
                    MainForm.cncv2list[0].SetProgPath = gcoderoad;
                    if (MainForm.collectdatav2.GetCNCDataLst[0].SetProgFile())
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                    //if (MainForm.cnclist[0].sendFile(gcoderoad, "h/lnc8/prog/" + gcodename, 0, false) == 0)
                    //{
                    //    return 1;
                    //}
                    //else
                    //{
                    //    return 0;
                    //}
                }
                else if (gongyiNo == 2)//铣床
                {
                    gcodename = "OHOMECNC.nc";
                    gcoderoad = GcodeFilePath + "\\" + gcodename;

                    if (!File.Exists(@gcoderoad))
                    {
                        return -1;
                    }
                    MainForm.cncv2list[1].SetProgPath = gcoderoad;
                    if (MainForm.collectdatav2.GetCNCDataLst[1].SetProgFile())
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                    //if (MainForm.cnclist[1].sendFile(gcoderoad, "h/lnc8/prog/" + gcodename, 0, false) == 0)
                    //{
                    //    return 1;
                    //}
                    //else
                    //{
                    //    return 0;
                    //}
                }
                else
                {
                    return 0;
                }

            }

            //A011CNC,场次1位，工件类型2位，材料1位，L车，CNC铣
            int magscenestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;//零件类型  

            int maglength = (int)ModbusTcp.MagLength;
            int magSecenei = magscenestart + maglength * (MagNo - 1);//场次
            int magSecene = ModbusTcp.DataMoubus[magSecenei];//场次
                                                             //  int magmeterial = ModbusTcp.DataMoubus[magSecenei + 2];//材质
            int magtypestart = ModbusTcp.DataMoubus[magSecenei + 1];//零件类型
            char magSeceneic;
            string magSeceneis = "";
            //  string magmeterialis = ""; 
            string magtypestartis = "";
            string magnos = MagNo.ToString();
            //A的ascii码为41大写
            magSecenei = magSecene + 65;
            //magtypestart

            magSeceneic = Convert.ToChar(magSecenei);
            magSeceneis = Convert.ToString(magSeceneic);//零件场次0-A，1-B，2-C，3-D，4-E,5_F
            magtypestartis = Convert.ToString(magtypestart);//零件类型，0-A，1-B，2-C，3-D，
            if (MagNo < 10)
            {
                magnos = "0" + magnos;//仓位编号01-30
            }

            //O A -场次11-仓位号2-类型 CNC-机床.nc
            if (gongyiNo == 1)//车床
            {
                gcodename = "O" + magSeceneis + magnos + magtypestartis + "L.nc";
                gcoderoad = GcodeFilePath + "\\" + gcodename;
                if (!File.Exists(@gcoderoad))
                {
                    return -1;
                }
                // if (MainForm.cnclist[0].sendFile(gcoderoad, "..prog/" + gcodename, 0, false) == 0)
                MainForm.cncv2list[0].SetProgPath = gcoderoad;
                if (MainForm.collectdatav2.GetCNCDataLst[0].SetProgFile())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
             
            }


            //}
            else if (gongyiNo == 2)//铣床
            {
                gcodename = "O" + magSeceneis + magnos + magtypestartis + "CNC.nc";
                gcoderoad = GcodeFilePath + "\\" + gcodename;
                if (!File.Exists(@gcoderoad))
                {
                    return -1;
                }
                MainForm.cncv2list[1].SetProgPath = gcoderoad;
                if (MainForm.collectdatav2.GetCNCDataLst[1].SetProgFile())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
                //if (MainForm.cnclist[1].sendFile(gcoderoad, "../prog/" + gcodename, 0, false) == 0)
                //if (MainForm.cnclist[1].sendFile(gcoderoad, "h/lnc8/prog/" + gcodename, 0, false) == 0)
                //{
                //    return 1;
                //}
                //else
                //{
                //    return 0;
                //}
            }
            else
            {
                return 0;
            }

        }
        //private bool InitOrderstate(string path)
        //{
        //    try
        //    {
        //        FileStream aFile = new FileStream(path, FileMode.Open);
        //        StreamReader sr = new StreamReader(aFile);
        //        string item = "";
        //        string magno = "";
        //        string fun1 = "";
        //        string fun2 = "";
        //        string fun3 = "";
        //        string fun4 = "";
        //        string down = "";
        //        string check = "";
        //        string line;
        //        int ii = 0;
        //        line = sr.ReadLine();

        //        while (line != null && line != "")
        //        {
        //            item = getvalueformstring(line, "item=");
        //            magno = getvalueformstring(line, "magno=");
        //            fun1 = getvalueformstring(line, "fun1=");
        //            fun2 = getvalueformstring(line, "fun2=");
        //            fun3 = getvalueformstring(line, "fun3=");
        //            fun4 = getvalueformstring(line, "fun4=");
        //            down = getvalueformstring(line, "down=");
        //            check = getvalueformstring(line, "check=");
        //            if (item == "null" || magno == "null" || fun1 == "null" || fun2 == "null" || fun3 == "null" || fun4 == "null" || down == "null" || check == "null")
        //            {
        //                line = sr.ReadLine();

        //            }
        //            else
        //            {
        //                dataGridVieworder2.Rows.Add();
        //                dataGridVieworder2.Rows[ii].Cells[0].Value = item;
        //                dataGridVieworder2.Rows[ii].Cells[1].Value = magno;
        //                dataGridVieworder2.Rows[ii].Cells[2].Value = fun1;
        //                dataGridVieworder2.Rows[ii].Cells[3].Value = fun2;
        //                dataGridVieworder2.Rows[ii].Cells[4].Value = fun3;
        //                dataGridVieworder2.Rows[ii].Cells[5].Value = fun4;
        //                dataGridVieworder2.Rows[ii].Cells[6].Value = down;
        //                dataGridVieworder2.Rows[ii].Cells[7].Value = check;

        //                line = sr.ReadLine();
        //                ii++;
        //            }

        //        }

        //        sr.Close();
        //        aFile.Close();
        //        return true;
        //    }
        //    catch (IOException e)
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        ///保存订单信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //private bool Orderdatasave(string path)
        //{
        //    try
        //    {
        //        FileStream aFile = new FileStream(path, FileMode.Create);
        //        StreamWriter sr = new StreamWriter(aFile);
        //        int jj = 0;
        //        string orderdatas = "";
        //        string odersdownflage = "否";
        //        string odersbackdownflage = "否";
        //        //if (dataGridVieworder.Rows.Count<2)
        //        //{
        //        //    return;
        //        //}

        //        if (dataGridVieworder.Rows.Count > 1)
        //        {
        //            if (language == "English")
        //            {
        //                this.Column5.Items.Clear();
        //                this.Column5.Items.AddRange(new object[] {
        //                        "None",
        //                        "Processes1",
        //                        "Processes2"});
        //                this.Column6.Items.Clear();
        //                this.Column6.Items.AddRange(new object[] {
        //                        "Load",
        //                        "Unload",
        //                        "Change"});

        //            }
        //            if (language == "Chinese")
        //            {
        //                this.Column5.Items.Clear();
        //                this.Column5.Items.AddRange(new object[] {
        //                                "无",
        //                                "工序一",
        //                                "工序二",});
        //                this.Column6.Items.Clear();
        //                this.Column6.Items.AddRange(new object[] {
        //                        "上料",
        //                        "下料",
        //                        "换料"});
        //            }
        //        }
        //        for (jj = 0; jj < dataGridVieworder.Rows.Count; jj++)
        //        {

        //            if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "None"
        //                    || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "无")
        //            {
        //                gongxuindex[jj] = 0;
        //            }
        //            if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "Processes1"
        //                    || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "工序一")
        //            {
        //                gongxuindex[jj] = 1;
        //            }
        //            if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "Processes2"
        //                   || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "工序二")
        //            {
        //                gongxuindex[jj] = 2;
        //            }
        //            if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "Processes3"
        //                   || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "工序三")
        //            {
        //                gongxuindex[jj] = 3;
        //            }
        //            if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "Processes4"
        //                   || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "工序四")
        //            {
        //                gongxuindex[jj] = 4;
        //            }



        //            if (dataGridVieworder.Rows[jj].Cells[6].Style.BackColor == Color.Gray)
        //            {
        //                if (language == "English")
        //                {
        //                    odersdownflage = "Yes";
        //                }
        //                else odersdownflage = "是";
        //            }
        //            else
        //            {
        //                if (language == "English")
        //                {
        //                    odersdownflage = "No";
        //                }
        //                else odersdownflage = "否";
        //            }
        //            if (dataGridVieworder.Rows[jj].Cells[8].Style.BackColor == Color.Gray)
        //            {
        //                if (language == "English")
        //                {
        //                    odersbackdownflage = "Yes";
        //                }
        //                else odersbackdownflage = "是";
        //            }
        //            else
        //            {
        //                if (language == "English")
        //                {
        //                    odersbackdownflage = "No";
        //                }
        //                else odersbackdownflage = "否";
        //            }
        //            if (dataGridVieworder.Rows[jj].Cells[0].Value == null
        //                || dataGridVieworder.Rows[jj].Cells[1].Value == null
        //                || dataGridVieworder.Rows[jj].Cells[2].Value == null
        //                || dataGridVieworder.Rows[jj].Cells[3].Value == null
        //                || dataGridVieworder.Rows[jj].Cells[4].Value == null
        //                || dataGridVieworder.Rows[jj].Cells[5].Value == null
        //                || dataGridVieworder.Rows[jj].Cells[6].Value == null
        //                || dataGridVieworder.Rows[jj].Cells[7].Value == null
        //                || dataGridVieworder.Rows[jj].Cells[8].Value == null)
        //            {
        //                dataGridVieworder.Rows.RemoveAt(jj);
        //            }
        //            else
        //            {
        //                if (dataGridVieworder.Rows[jj].Cells[0].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[2].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[3].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[6].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[8].Value.ToString() == "" ||
        //                   dataGridVieworder.Rows[jj].Cells[0].Value.ToString() == "null" ||
        //                   dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == "null" ||
        //                   dataGridVieworder.Rows[jj].Cells[2].Value.ToString() == "null" ||
        //                   dataGridVieworder.Rows[jj].Cells[3].Value.ToString() == "null" ||
        //                   dataGridVieworder.Rows[jj].Cells[4].Value.ToString() == "null" ||
        //                   dataGridVieworder.Rows[jj].Cells[5].Value.ToString() == "null" ||
        //                   dataGridVieworder.Rows[jj].Cells[6].Value.ToString() == "null" ||
        //                   dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "null" ||
        //                   dataGridVieworder.Rows[jj].Cells[8].Value.ToString() == "null")
        //                {
        //                    ;
        //                }
        //                else
        //                {
        //                    if (language == "English")
        //                    {
        //                        for (int kk = 2; kk < 6; kk++)
        //                        {
        //                            if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "无")
        //                            {
        //                                dataGridVieworder.Rows[jj].Cells[kk].Value = "None";
        //                            }
        //                            if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "车工序")
        //                            {
        //                                dataGridVieworder.Rows[jj].Cells[kk].Value = "Lathe";
        //                            }
        //                            if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "铣工序")
        //                            {
        //                                dataGridVieworder.Rows[jj].Cells[kk].Value = "CNC";
        //                            }
        //                        }
        //                        //DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
        //                        //     cell = (DataGridViewComboBoxCell)dataGridVieworder.Rows[jj].Cells[7];
        //                        //ComboBox cell1 = new ComboBox();
        //                        if (gongxuindex[jj] == 0)
        //                        {
        //                            // cell1.SelectedIndex = 0;
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "None";
        //                        }
        //                        if (gongxuindex[jj] == 1)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "Processes1";
        //                        }
        //                        if (gongxuindex[jj] == 2)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "Processes2";
        //                        }
        //                        if (gongxuindex[jj] == 3)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "Processes3";
        //                        }
        //                        if (gongxuindex[jj] == 4)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "Processes4";
        //                        }

        //                        dataGridVieworder.Rows[jj].Cells[6].Value = "OK";
        //                        dataGridVieworder.Rows[jj].Cells[8].Value = "OK";

        //                    }
        //                    else
        //                    {
        //                        for (int kk = 2; kk < 6; kk++)
        //                        {
        //                            if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "None")
        //                            {
        //                                dataGridVieworder.Rows[jj].Cells[kk].Value = "无";
        //                            }
        //                            if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "Lathe")
        //                            {
        //                                dataGridVieworder.Rows[jj].Cells[kk].Value = "车工序";
        //                            }
        //                            if (dataGridVieworder.Rows[jj].Cells[kk].Value.ToString() == "CNC")
        //                            {
        //                                dataGridVieworder.Rows[jj].Cells[kk].Value = "铣工序";
        //                            }
        //                        }
        //                        if (gongxuindex[jj] == 0)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "无";
        //                        }
        //                        if (gongxuindex[jj] == 1)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "工序一";
        //                        }
        //                        if (gongxuindex[jj] == 2)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "工序二";
        //                        }
        //                        if (gongxuindex[jj] == 3)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "工序三";
        //                        }
        //                        if (gongxuindex[jj] == 4)
        //                        {
        //                            dataGridVieworder.Rows[jj].Cells[7].Value = "工序四";
        //                        }

        //                        dataGridVieworder.Rows[jj].Cells[6].Value = "确定";
        //                        dataGridVieworder.Rows[jj].Cells[8].Value = "确定";
        //                    }
        //                    orderdatas = "item=" + dataGridVieworder.Rows[jj].Cells[0].Value.ToString();
        //                    orderdatas = orderdatas + ",magno=" + dataGridVieworder.Rows[jj].Cells[1].Value.ToString();
        //                    orderdatas = orderdatas + ",fun1=" + dataGridVieworder.Rows[jj].Cells[2].Value.ToString();
        //                    orderdatas = orderdatas + ",fun2=" + dataGridVieworder.Rows[jj].Cells[3].Value.ToString();
        //                    orderdatas = orderdatas + ",fun3=" + dataGridVieworder.Rows[jj].Cells[4].Value.ToString();
        //                    orderdatas = orderdatas + ",fun4=" + dataGridVieworder.Rows[jj].Cells[5].Value.ToString();
        //                    orderdatas = orderdatas + ",down=" + odersdownflage;
        //                    orderdatas = orderdatas + ",backfun=" + dataGridVieworder.Rows[jj].Cells[7].Value.ToString();
        //                    orderdatas = orderdatas + ",backdown=" + odersbackdownflage;
        //                    orderdatas = orderdatas + ";";
        //                    if (orderdatas.Length > 40)
        //                    {

        //                        sr.WriteLine(orderdatas);
        //                    }
        //                }
        //            }

        //        }
        //        sr.Close();
        //        aFile.Close();
        //        return true;
        //    }
        //    catch (IOException e)
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// 保存订单状态信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //private bool Orderstatesave(string path)
        //{
        //    try
        //    {
        //        FileStream aFile = new FileStream(path, FileMode.Create);
        //        StreamWriter sr = new StreamWriter(aFile);
        //        int jj = 0;
        //        string orderdatas = "";

        //        for (jj = 0; jj < dataGridVieworder2.Rows.Count; jj++)
        //        {
        //            if (dataGridVieworder2.Rows[jj].Cells[0].Value == null
        //                || dataGridVieworder2.Rows[jj].Cells[1].Value == null
        //                || dataGridVieworder2.Rows[jj].Cells[2].Value == null
        //                || dataGridVieworder2.Rows[jj].Cells[3].Value == null
        //                || dataGridVieworder2.Rows[jj].Cells[4].Value == null
        //                || dataGridVieworder2.Rows[jj].Cells[5].Value == null
        //                || dataGridVieworder2.Rows[jj].Cells[6].Value == null
        //                || dataGridVieworder2.Rows[jj].Cells[7].Value == null)
        //            {
        //                dataGridVieworder2.Rows.RemoveAt(jj);
        //            }
        //            else
        //            {
        //                if (dataGridVieworder2.Rows[jj].Cells[0].Value.ToString() == "" ||
        //                   dataGridVieworder2.Rows[jj].Cells[1].Value.ToString() == "" ||
        //                   dataGridVieworder2.Rows[jj].Cells[2].Value.ToString() == "" ||
        //                   dataGridVieworder2.Rows[jj].Cells[3].Value.ToString() == "" ||
        //                   dataGridVieworder2.Rows[jj].Cells[4].Value.ToString() == "" ||
        //                   dataGridVieworder2.Rows[jj].Cells[5].Value.ToString() == "" ||
        //                   dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "" ||
        //                   dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "" ||
        //                   dataGridVieworder2.Rows[jj].Cells[0].Value.ToString() == "null" ||
        //                   dataGridVieworder2.Rows[jj].Cells[1].Value.ToString() == "null" ||
        //                   dataGridVieworder2.Rows[jj].Cells[2].Value.ToString() == "null" ||
        //                   dataGridVieworder2.Rows[jj].Cells[3].Value.ToString() == "null" ||
        //                   dataGridVieworder2.Rows[jj].Cells[4].Value.ToString() == "null" ||
        //                   dataGridVieworder2.Rows[jj].Cells[5].Value.ToString() == "null" ||
        //                   dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "null" ||
        //                   dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "null")
        //                {
        //                    ;
        //                }
        //                else
        //                {
        //                    if (language == "English")
        //                    {
        //                        for (int kk = 2; kk < 6; kk++)
        //                        {
        //                            if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "无")
        //                            {
        //                                dataGridVieworder2.Rows[jj].Cells[kk].Value = "None";
        //                            }
        //                            if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "未开始")
        //                            {
        //                                dataGridVieworder2.Rows[jj].Cells[kk].Value = "NotStarted";
        //                            }
        //                            if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "进行中")
        //                            {
        //                                dataGridVieworder2.Rows[jj].Cells[kk].Value = "Processing";
        //                            }
        //                            if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "完成")
        //                            {
        //                                dataGridVieworder2.Rows[jj].Cells[kk].Value = "Finish";
        //                            }


        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "未下发")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[6].Value = "NotDownload";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "进行中")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[6].Value = "Processing";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "完成")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[6].Value = "Finish";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "待返修")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[6].Value = "NeedReProcesses";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "无")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[7].Value = "None";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "合格")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[7].Value = "Qualified";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "不合格")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[7].Value = "Unqualified";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        for (int kk = 2; kk < 6; kk++)
        //                        {
        //                            if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "None")
        //                            {
        //                                dataGridVieworder2.Rows[jj].Cells[kk].Value = "无";
        //                            }
        //                            if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "NotStarted")
        //                            {
        //                                dataGridVieworder2.Rows[jj].Cells[kk].Value = "未开始";
        //                            }
        //                            if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "Processing")
        //                            {
        //                                dataGridVieworder2.Rows[jj].Cells[kk].Value = "进行中";
        //                            }
        //                            if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "Finish")
        //                            {
        //                                dataGridVieworder2.Rows[jj].Cells[kk].Value = "完成";
        //                            }
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "NotDownload")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[6].Value = "未下发";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "Processing")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[6].Value = "进行中";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "Finish")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[6].Value = "完成";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "NeedReProcesses")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[6].Value = "待返修";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "None")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[7].Value = "无";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "Qualified")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[7].Value = "合格";
        //                        }
        //                        if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "Unqualified")
        //                        {
        //                            dataGridVieworder2.Rows[jj].Cells[7].Value = "不合格";
        //                        }

        //                    }

        //                    orderdatas = "item=" + dataGridVieworder2.Rows[jj].Cells[0].Value.ToString();
        //                    orderdatas = orderdatas + ",magno=" + dataGridVieworder2.Rows[jj].Cells[1].Value.ToString();
        //                    orderdatas = orderdatas + ",fun1=" + dataGridVieworder2.Rows[jj].Cells[2].Value.ToString();
        //                    orderdatas = orderdatas + ",fun2=" + dataGridVieworder2.Rows[jj].Cells[3].Value.ToString();
        //                    orderdatas = orderdatas + ",fun3=" + dataGridVieworder2.Rows[jj].Cells[4].Value.ToString();
        //                    orderdatas = orderdatas + ",fun4=" + dataGridVieworder2.Rows[jj].Cells[5].Value.ToString();
        //                    orderdatas = orderdatas + ",down=" + dataGridVieworder2.Rows[jj].Cells[6].Value.ToString();
        //                    orderdatas = orderdatas + ",check=" + dataGridVieworder2.Rows[jj].Cells[7].Value.ToString();
        //                    if (orderdatas == "")
        //                    {
        //                        ;
        //                    }
        //                    orderdatas = orderdatas + ";";
        //                    if (orderdatas.Length > 40)
        //                    {

        //                        sr.WriteLine(orderdatas);
        //                    }
        //                }
        //            }

        //        }
        //        sr.Close();
        //        aFile.Close();
        //        return true;
        //    }
        //    catch (IOException e)
        //    {
        //        return false;
        //    }
        //}

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    language = ChangeLanguage.GetDefaultLanguage();
        //    if (!ReProcessChoose)
        //    {
        //        form1.Visible = false;
        //    }
        //    if (MainForm.Orderinitflage == false)
        //    {
        //        MainForm.Orderinitflage = true;
        //    }

        //    //接收数据，更新数据
        //    //sptcp1.ReceiveData();
        //    //linereset = true;//切换产线复位按钮
        //    //命令码MES_PLC_comfirm，plc的响应码为PLC_MES_respone
        //    //下指令时，MES_PLC_comfim_write_flage=true,命令码=98，
        //    //如果MES_PLC_comfim_write_flage==true，那么发送MES_PLC_comfirm，同时发送请求PLC_MES_respone，         
        //    //如果MES_PLC_comfirm==98&&PLC_MES_respone==98，那么MES_PLC_comfirm=0，
        //    //如果PLC_MES_respone==0；那么MES_PLC_comfim_write_flage=false；交互完成

        //    //下达命令交互
        //    MESToPLCcomfirmfun();
        //    //订单信息Rack_number_comfirm，Order_type_comfirm，PLC响应为Rack_number_respone，Order_type_respone
        //    //下指令时Rack_number_write_flage= true，Rack_number_comfirm=m，Order_type_comfirm=n
        //    //如果Rack_number_write_flage==true，那么发送订单信息，同时发送请求订单反馈
        //    //如果Rack_number_respone=m，那么Rack_number_comfirm=0
        //    // 如果Order_type_respone=0, 那么Order_type_comfirm=0；
        //    //Rack_number_respone==0&&Order_type_respone==0；那么Rack_number_write_flage = false；交互完成

        //    //下达订单交互
        //    MESToPLCorderfun();
        //    //机床加工状态信息PLC给MES的命令PLC_MES_comfirm，Rcak_number_comfirm，Result_comfirm，Machine_type_comfirm
        //    //Mes反馈给机床的信息MES_PLC_response，Rcak_number_response，Result_response，Machine_type_response
        //    //发送请求加工信息
        //    //如果PLC_MES_comfirm=202，&&MES_PLC_response==0，PLC_MES_comfim_req_flage=true，
        //    //如果PLC_MES_comfim_req_flage==true，那么发送MES_PLC_response，
        //    //那么确认获取机床加工结果。更新机床占用信息，置位MES_PLC_response=202，plc方自行清除PLC_MES_comfirm
        //    //如果PLC_MES_comfirm=0，那么MES_PLC_response= 0，发送MES_PLC_response，置位PLC_MES_comfim_back_flage，
        //    //如果PLC_MES_comfim_back_flage == true，那么请求MES_PLC_response
        //    //如果请求到的MES_PLC_response=0，那么PLC_MES_comfim_req_flage=false，PLC_MES_comfim_back_flage=false，交互完成
        //    //请求加工信息交互
        //    PLCToMEScncfun();//车床完成信号处理

        //    PLCToMEScncfun2();//铣床完成信号处理

        //    PLCToMEScncfun3();//铣床测量信号处理


        //    RackmessageSync();//同步料仓数据

        //    //请求料架有无料信息
        //    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm, 10);//查询plc_mes的命令
        //    MainForm.sptcp1.ReceiveData();
        //    MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.req, 1, 0, (int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_status, 28);//请求61、62号寄存器存储内容是料位有无料信息      
        //    MainForm.sptcp1.ReceiveData();
        //}
        /// <summary>
        /// 获取料仓对应的行号
        /// </summary>
        /// <param name="magno">料仓编号</param>
        /// <returns>行号</returns>
        private int getmagnorowindex(int magno)
        {
            int temp = -1;
            for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
            {
                if (dataGridVieworder.Rows[ii].Cells[1].Value.ToString() == magno.ToString())
                {
                    temp = ii;
                }
            }
            if (temp == -1)
            {
                return -1;
            }
            else
                return temp;
        }

        /// <summary>
        /// 获取下一工序
        /// </summary>
        /// <param name="magno">料仓号</param>
        /// <param name="curfun">当前料的工序编号</param>
        /// <returns>返回1-车工序，返回2-铣工序，返回0-所有工序完成，没有新的工序 ,返回-1，错误</returns>
        //public int getnextorderfun(int magno, ref int curfun)//获取下一工序内容，
        //{
        //    int index = -1;
        //    int nextfun = -2;
        //    index = getmagnorowindex(magno);
        //    maginderx = index;
        //    if (index == -1)
        //    {
        //        return -1;
        //    }
        //    if (curfun == 4)
        //    {
        //        return 0;//所有工序完成，没有下一工序
        //    }
        //    for (int ii = curfun; ii < 5; ii++)
        //    {
        //        if (nextfun == -2)
        //        {
        //            if (language == "English")
        //            {
        //                if (dataGridVieworder2.Rows[index].Cells[1 + ii].Value.ToString() == "NotStarted")
        //                {
        //                    if (dataGridVieworder.Rows[index].Cells[1 + ii].Value.ToString() == "Lathe")
        //                    {
        //                        nextfun = 1;
        //                        curfun = ii;
        //                    }
        //                    else if (dataGridVieworder.Rows[index].Cells[1 + ii].Value.ToString() == "CNC")
        //                    {
        //                        nextfun = 2;
        //                        curfun = ii;
        //                    }
        //                    else
        //                    {
        //                        nextfun = -1;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (dataGridVieworder2.Rows[index].Cells[1 + ii].Value.ToString() == "未开始")
        //                {
        //                    if (dataGridVieworder.Rows[index].Cells[1 + ii].Value.ToString() == "车工序")
        //                    {
        //                        nextfun = 1;
        //                        curfun = ii;
        //                    }
        //                    else if (dataGridVieworder.Rows[index].Cells[1 + ii].Value.ToString() == "铣工序")
        //                    {
        //                        nextfun = 2;
        //                        curfun = ii;
        //                    }
        //                    else
        //                    {
        //                        nextfun = -1;
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    if (nextfun == -2)
        //    {
        //        nextfun = 0;
        //    }
        //    return nextfun;
        //}
        //测量返回值:true:值合法；false:无效
        public static bool checkedInputValue(double value, double[] arry)
        {
            double refer = arry[0];
            double max = arry[1];
            double min = arry[2];

            //超出临界值判断
            if (refer >= 0)
            {
                //AotoOrderForm.Fvalue1 = 40;

                if ((value - refer) < min * AotoOrderForm.Fvalue2 * 0.01 || (value - refer) > max * AotoOrderForm.Fvalue2 * 0.01)//实际值小于参考值，并且误差大于临街值2
                {
                    AotoOrderForm.Fvalue2over = true;
                }
                else if ((value - refer) < min * AotoOrderForm.Fvalue1 * 0.01 || (value - refer) > max * AotoOrderForm.Fvalue1 * 0.01)//实际值小于参考值，并且误差大于临街值1
                {
                    AotoOrderForm.Fvalue1over = true;
                }

            }
            if (refer < 0)
            {
                if (((value - refer) > -1 * max * AotoOrderForm.Fvalue2 * 0.01) || ((value - refer) < -1 * min * AotoOrderForm.Fvalue2 * 0.01))
                {

                    AotoOrderForm.Fvalue2over = true;

                }
                else if (((value - refer) >= -1 * max * AotoOrderForm.Fvalue1 * 0.01) || ((value - refer) <= -1 * min * AotoOrderForm.Fvalue1 * 0.01))
                {

                    AotoOrderForm.Fvalue1over = true;

                }
            }

            if (refer >= 0)
            {
                if (value >= (refer + min) && value <= (refer + max))
                    return true;
                else
                    return false;
            }
            else
            {
                if (value >= (refer - max) && value <= (refer - min))
                    return true;
                else
                    return false;

            }

        }


        private void getrefvalue(string path, ref double[,] arry)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);

                string refvalue = "";
                string uppervalue = "";
                string lowervalue = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();
                while (line != null && line != "")
                {
                    refvalue = getvalueformstring(line, "ref=");
                    uppervalue = getvalueformstring(line, "upper=");
                    lowervalue = getvalueformstring(line, "lower=");

                    arry[ii, 0] = Convert.ToDouble(refvalue);
                    arry[ii, 1] = Convert.ToDouble(uppervalue);
                    arry[ii, 2] = Convert.ToDouble(lowervalue);
                    line = sr.ReadLine();
                    ii++;
                }

                sr.Close();
                aFile.Close();
            }
            catch (IOException e)
            {
                ;
            }

        }

        //public void UpdateMeasureRes()  //测量合格、不合格
        //{
        //    if (!measureenable)
        //    {
        //        return;
        //    }

        //    AotoOrderForm.Fvalue2over = false;

        //    AotoOrderForm.Fvalue1over = false;
        //    measureenable = false;
        //    string key = "MacroVariables:USER";

        //    //   int magcheckstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Check;//零件不合格标识
        //    //   int maglength = (int)ModbusTcp.MagLength;
        //    int i = MainForm.cnclist[1].MagNo;
        //    //  int magchecki = magcheckstart + maglength * (i - 1);
        //    //    int magchecki = MainForm.Mag_Check[i - 1];

        //    double[] valuearry = new double[6];

        //    string[] valuearrys = new string[6];
        //    if (MainForm.cnclist[1].MagNo < 1)
        //    {
        //        return;
        //    }
        //    if (MainForm.cnclist[1].MagNo < 12)
        //    {
        //        getrefvalue(MeterForm.MetersetFilePath1, ref refvalue);//获取参考值
        //    }
        //    else if (MainForm.cnclist[1].MagNo < 24)
        //    {
        //        getrefvalue(MeterForm.MetersetFilePath2, ref refvalue);//获取参考值
        //    }
        //    else if (MainForm.cnclist[1].MagNo < 28)
        //    {
        //        getrefvalue(MeterForm.MetersetFilePath3, ref refvalue);//获取参考值
        //    }
        //    else {
        //        getrefvalue(MeterForm.MetersetFilePath4, ref refvalue);//获取参考值
        //    }
        //    // getrefvalue(MetersetFilePath, ref refvalue);//获取参考值

        //    int ret0 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE0, ref valuestrarry[0]);
        //    int ret1 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE1, ref valuestrarry[1]);
        //    int ret2 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE2, ref valuestrarry[2]);
        //    int ret3 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE3, ref valuestrarry[3]);
        //    int ret4 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE4, ref valuestrarry[4]);
        //    int ret5 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE5, ref valuestrarry[5]);

        //    if (ret0 == -1 || ret1 == -1 || ret2 == -1 || ret3 == -1 || ret5 == -1 || ret4 == -1)
        //    {
        //        MessageBox.Show("获取失败，请重试！");
        //        return;
        //    }

        //    for (int ii = 0; ii < 6; ii++)
        //    {
        //        string str = valuestrarry[ii];
        //        if (str.Length > 0)
        //        {
        //            double temp = 0;
        //            int strStart = str.IndexOf("f\":");
        //            int len = str.IndexOf(",", strStart + 3) - (strStart + 3);
        //            string strTmp = str.Substring(strStart + 3, len);
        //            temp = Convert.ToDouble(strTmp);
        //            valuearrys[ii] = temp.ToString("F3");
        //            string temps = temp.ToString("F3");
        //            resvalue[ii] = Convert.ToDouble(valuearrys[ii]);

        //            if (temps != "null")
        //            {

        //                int index = temps.IndexOf(".");
        //                int flage = 1;
        //                if (Convert.ToDouble(temps) < 0)
        //                {
        //                    flage = -1;
        //                }
        //                string refvalue1 = temps.Substring(0, index);//整数部分
        //                string refvalue2 = temps.Substring(index + 1);//小数部分
        //                if (flage == -1)
        //                {

        //                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3] = 1;
        //                }
        //                else

        //                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3] = 0;

        //                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 1] = Convert.ToInt32(refvalue1);
        //                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 1] < 0)
        //                {
        //                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 1] = (-1) * ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 1];
        //                }
        //                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 2] = Convert.ToInt32(refvalue2);
        //            }

        //            double[] arrytemp = new double[3];
        //            arrytemp[0] = refvalue[ii, 0];
        //            arrytemp[1] = refvalue[ii, 1];
        //            arrytemp[2] = refvalue[ii, 2];

        //            if (checkedInputValue(resvalue[ii], arrytemp))
        //            {
        //                valueb[ii] = true;//检测合格       

        //            }
        //            else
        //            {
        //                valueb[ii] = false;//检测不合格    

        //            }
        //        }
        //        else
        //        {


        //        }
        //    }
        //    if (valueb[0] && valueb[1] && valueb[2] && valueb[3] && valueb[4] && valueb[5])
        //    {
        //        bIsOK = true;
        //        // ModbusTcp.DataMoubus[magchecki] = 0;//检测合格 
        //        MainForm.Mag_Check[i - 1] = 0;
        //        MeterlogFileadd(MetetrrecordFilePath);
        //        renewMeterDGV2 = true;
        //        MeterForm.Textmagno = MainForm.cnclist[1].MagNo;

        //    }
        //    else
        //    {
        //        bIsOK = false;
        //        //ModbusTcp.DataMoubus[magchecki] = 1;//检测不合格 
        //        MainForm.Mag_Check[i - 1] = 1;
        //        MeterlogFileadd(MetetrrecordFilePath);

        //        renewMeterDGV2 = true;
        //        MeterForm.Textmagno = MainForm.cnclist[1].MagNo;

        //    }

        //}

        public void UpdateMeasureRes(bool t = true)  //测量合格、不合格
        {

            if (!measureenable)
            {
                return;
            }
            measureenable = false;
            CNCV2 cncv2 = null;
            foreach ( var temp  in MainForm.cncv2list)
            {
                if(temp.cnctype == CNCType.CNC)
                {
                    cncv2 = temp;
                }
            }
            int i = cncv2.MagNum;
            //  int magchecki = magcheckstart + maglength * (i - 1);
            //    int magchecki = MainForm.Mag_Check[i - 1];
            // double[] valuearry = new double[6];
            if (cncv2.MagNum < 1)
            {
                return;
            }

            getrefvalue(MetersetFilePath, ref refvalue);//获取参考值

            valuestrarry[0] = cncv2.MeterValue[0].ToString();
            valuestrarry[1] = cncv2.MeterValue[1].ToString();
            valuestrarry[2] = cncv2.MeterValue[2].ToString();
            valuestrarry[3] = cncv2.MeterValue[3].ToString();
            valuestrarry[4] = cncv2.MeterValue[4].ToString();
            valuestrarry[5] = cncv2.MeterValue[5].ToString();


            for (int ii = 0; ii < 6; ii++)
            {
                string str = valuestrarry[ii];
                if (str.Length > 0)
                {
                    resvalue[ii] = Convert.ToDouble(str);

                    double[] arrytemp = new double[3];
                    arrytemp[0] = refvalue[ii, 0];
                    arrytemp[1] = refvalue[ii, 1];
                    arrytemp[2] = refvalue[ii, 2];

                    if (checkedInputValue(resvalue[ii], arrytemp))
                    {
                        valueb[ii] = true;//检测合格       

                    }
                    else
                    {
                        valueb[ii] = false;//检测不合格    

                    }
                }
                else
                {


                }
            }
            if (valueb[0] && valueb[1] && valueb[2] && valueb[3] && valueb[4] && valueb[5])
            {
                bIsOK = true;
                // ModbusTcp.DataMoubus[magchecki] = 0;//检测合格 
                MainForm.Mag_Check[i - 1] = 0;
                MeterlogFileadd(MetetrrecordFilePath);
                renewMeterDGV2 = true;
                //MeterForm.Textmagno = MainForm.cnclist[1].MagNo;
                MeterForm.Textmagno = MainForm.cncv2list[1].MagNum;

            }
            else
            {
                bIsOK = false;
                //ModbusTcp.DataMoubus[magchecki] = 1;//检测不合格 
                MainForm.Mag_Check[i - 1] = 1;
                MeterlogFileadd(MetetrrecordFilePath);

                renewMeterDGV2 = true;
               // MeterForm.Textmagno = MainForm.cnclist[1].MagNo;
                MeterForm.Textmagno = MainForm.cncv2list[1].MagNum;

            }

        }
        private void MeterlogFileadd(string path)
        {
            try
            {
                if (MainForm.cncv2list.Count < 2)
                {
                    return;
                }
                FileStream aFile = new FileStream(path, FileMode.Append);
                StreamWriter sr = new StreamWriter(aFile);//追加文件
                string timenow = "";
                string newlog = "";

                string res = "";
                if (bIsOK)
                {
                    if (language == "English")
                    {
                        res = "Yes";
                    }
                    else res = "Yes";
                }
                else
                {
                    if (language == "English")
                    {
                        res = "No";
                    }
                    else res = "No";
                }

                string type = "";
                if (MainForm.cncv2list[1].MagNum <= 12)
                {
                    type = "A";
                }
                else if (MainForm.cncv2list[1].MagNum <= 24)
                {
                    type = "B";
                }
                else if (MainForm.cncv2list[1].MagNum <= 27)
                {
                    type = "C";
                }
                else
                {
                    type = "D";
                }

                string magno = MainForm.cncv2list[1].MagNum.ToString();
                timenow = DateTime.Now.ToString();
                string ref1 = refvalue[0, 0].ToString();
                string res1 = resvalue[0].ToString();
                string ref2 = refvalue[1, 0].ToString();
                string res2 = resvalue[1].ToString();
                string ref3 = refvalue[2, 0].ToString();
                string res3 = resvalue[2].ToString();
                string ref4 = refvalue[3, 0].ToString();
                string res4 = resvalue[3].ToString();
                string ref5 = refvalue[4, 0].ToString();
                string res5 = resvalue[4].ToString();
                string ref6 = refvalue[5, 0].ToString();
                string res6 = resvalue[5].ToString();

                newlog = "type=" + type + ",magno=" + magno + ",res=" + res + ",timenow=" + timenow + ",ref1=" + ref1 + ",res1=" + res1 + ",ref2=" + ref2 + ",res2=" + res2
                    + ",ref3=" + ref3 + ",res3=" + res3 + ",ref4=" + ref4 + ",res4=" + res4 + ",ref5=" + ref5 + ",res5=" + res5 + ",ref6=" + ref6 + ",res6=" + res6 + ",";

                sr.WriteLine(newlog);
                sr.Close();

                aFile.Close();
                MeterlogFiledelet(path);
                renewMeterRecordGridview = true;
                if (MainForm.SQLonline)
                {
                    try
                    {
                        //MainForm.cnclist[1].MagNo = 2;
                        var temp1 = DbHelper.Get<StorageBin>(new { SN = MainForm.cncv2list[1].MagNum });
                        if (temp1 != null)
                        {
                            MainForm.SGaugedata.StorageBinId = temp1.Id;
                        }
                        else
                        {
                            return;
                        }
                        //    MainForm.cnclist[1].MagNo = 0;
                        var temp2 = DbHelper.Get<User>(new { Username = MainForm.UserLoginname });
                        if (temp2 != null)
                        {
                            MainForm.SGaugedata.UserId = temp2.Id;
                        }
                        else
                        {
                            return;
                        }
                        res = "Yes";
                        System.DateTime date;
                        date = DateTime.Now;
                        DbHelper.Insert(new Gauge
                        {
                            Result = res,
                            UpdatedTime = date,
                            StorageBinId = MainForm.SGaugedata.StorageBinId,
                            UserId = MainForm.SGaugedata.UserId,
                        });

                        var temp3 = DbHelper.Get<Gauge>("Order By UpdatedTime desc");

                        if (temp3 == null)
                        {
                            return;
                        }
                        MainForm.SGaugedetaildata.GaugeId = temp3.Id;
                        for (int i = 0; i < 6; i++)
                        {
                            DbHelper.Insert(new GaugeDetail
                            {
                                ActualValue = refvalue[i, 0],
                                GaugeId = temp3.Id,
                                ReferenceValue = resvalue[i],
                                SN = i + 1,
                            });
                        }
                        for (int i = 6; i < 20; i++)
                        {
                            DbHelper.Insert(new GaugeDetail
                            {
                                ActualValue = 0,
                                GaugeId = temp3.Id,
                                ReferenceValue = 0,
                                SN = i + 1,
                            });
                        }

                    }
                    catch (Exception ex)
                    {
                    
                    }
                }
            }
            catch (IOException e)
            {
                ;
            }
        }
        private void MeterlogFiledelet(string path)
        {
            try
            {
                //FileStream aFile = new FileStream(path, FileMode.Append);
                //StreamWriter sr = new StreamWriter(aFile);//追加文件
                List<string> lines = new List<string>(File.ReadAllLines(path));
                int linecount = lines.Count();
                if (linecount > 40)
                {
                    for (int i = 0; i < (linecount - 40); i++)
                    {
                        lines.RemoveAt(i);
                    }
                    File.WriteAllLines(path, lines.ToArray());

                }
                //sr.WriteLine(newlog);
                //sr.Close();

                //aFile.Close();
                // renewMeterRecordGridview = true;
            }
            catch (IOException e)
            {
                ;
            }
        }
        /// <summary>
        /// mes与plc料仓信息同步
        /// </summary>
        static public void RackmessageSync()
        {

            if (RackForm.setrfidflag)//mes整体料仓信息写给plc
            {
                if (RackForm.recivemagmessage)
                {
                    RackForm.setrfidflag = false;
                    RackForm.recivemagmessage = false;
                    for (int ii = 0; ii < 30; ii++)
                    {
                        RackForm.mag_state_change_flage[ii] = false;
                    }

                    return;
                }
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mag_Scene, 120, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();
            }
            if (RackForm.getrfidflag)//获取plc的料仓整体信息
            {
                if (RackForm.recivemagmessage)
                {
                    RackForm.getrfidflag = false;
                    RackForm.recivemagmessage = false;
                    return;
                }
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mag_Scene, 120);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();
            }
            if (RackForm.magstatesyncflag)//只有在mes执行写rfid功能后，才同步仓位信息给plc
            {
                for (int ii = 0; ii < 30; ii++)
                {
                    if (RackForm.mag_state_change_flage[ii])//仓位有变化,写信息给plc
                    {
                        int maglength = (int)ModbusTcp.MagLength;
                        int startreg = (int)ModbusTcp.DataConfigArr.Mag_Scene + ii * maglength;
                        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, startreg, maglength, 1, 0);//给plc单个仓位信息
                        MainForm.sptcp1.ReceiveData();
                        RackForm.mag_state_change_flage[ii] = false;
                    }
                }
            }


        }


        //下达订单或者下达返修取料命令
        private void MESToPLCorderfun()
        {
            if (ModbusTcp.Rack_number_write_flage == true)
            {
                ModbusTcp.Rack_number_write_count++;
                //if (timermodbus.Interval * ModbusTcp.Rack_number_write_count > 60 * 1000)//指令下达交互时间1min无回应，报错
                //{
                //    MessageBox.Show("订单下达超时，订单撤回！");
                //    ModbusTcp.Rack_number_write_count = 0;
                //    ModbusTcp.Rack_number_write_flage = false;
                //    RebackOrder();
                //    return;
                //}
                //if (ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_speed] == 1)
                //{
                //    return;
                //}
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 4, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();

                Thread.Sleep(20);
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 4);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();

                Thread.Sleep(20);
                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] == ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm])
                {
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm] = 0;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm] = 0;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;
                }
                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == 0
                    && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] == 0)
                {
                    ModbusTcp.Rack_number_write_flage = false;
                    ModbusTcp.Rack_number_write_count = 0;

                }
            }
            else
                return;
        }
        public static void RebackOrder()
        {
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm] = 0;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm] = 0;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] = 0;
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 4, 1, 0);//给plc写订单信息
            MainForm.sptcp1.ReceiveData();

            Thread.Sleep(2);
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 4, 1, 0);//给plc写订单信息
            MainForm.sptcp1.ReceiveData();
            Thread.Sleep(2);

            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 4, 1, 0);//给plc写订单信息
            MainForm.sptcp1.ReceiveData();

            Thread.Sleep(2);

            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 4, 1, 0);//给plc写订单信息
            MainForm.sptcp1.ReceiveData();
            return;
        }
        private void MESToPLCcomfirmfun()
        {
            if (ModbusTcp.MES_PLC_comfim_write_flage == true)
            {
                string temp = "";
                ModbusTcp.MES_PLC_comfim_write_count++;
                if (timer1.Interval * ModbusTcp.MES_PLC_comfim_write_count > 80 * 1000)//指令下达交互时间2min无回应，报错
                {

                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == (int)ModbusTcp.MesCommandToPlc.ComStartDevice)
                    {
                        temp = "复位超时,请复位PLC！";

                        MainForm.linereset = false;//切换产线复位按钮
                        MainForm.linereseting = false;


                        ModbusTcp.MES_PLC_comfim_write_count = 0;
                        ModbusTcp.MES_PLC_comfim_write_flage = false;
                        MainForm.plcgetconfim = false;

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

                        MainForm.linestart = true;//切换产线复位按钮
                        MainForm.linestarting = false;


                        ModbusTcp.MES_PLC_comfim_write_count = 0;
                        ModbusTcp.MES_PLC_comfim_write_flage = false;
                        MainForm.plcgetconfim = false;

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

                        MainForm.linestop = true;//切换产线复位按钮
                        MainForm.linestoping = false;


                        ModbusTcp.MES_PLC_comfim_write_count = 0;
                        ModbusTcp.MES_PLC_comfim_write_flage = false;
                        MainForm.plcgetconfim = false;

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
                if (timer1.Interval * ModbusTcp.MES_PLC_comfim_write_count > 60 * 1000 * 10)//指令下达交互时间2min无回应，报错
                {
                    ModbusTcp.MES_PLC_comfim_write_count = 0;
                    ModbusTcp.MES_PLC_comfim_write_flage = false;
                    MainForm.plcgetconfim = false;

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
                    MainForm.plcgetconfim = true;
                }
                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] == 0
                    && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] == 0
                    && MainForm.plcgetconfim == true)
                {
                    MainForm.plcgetconfim = false;
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
                    if (MainForm.linereseting)//产线复位完成后
                    {
                        MainForm.linereseting = false;
                    }
                    else if (MainForm.linestarting)//产线启动完成后
                    {
                        MainForm.linestarting = false;
                    }
                    else if (MainForm.linestoping)//产线停止完成后
                    {
                        MainForm.linestoping = false;
                    }

                }
            }
            else
                return;
        }

        private void PLCToMEScncfun()//机器人放料完成回到home点后，给出完成信号
        {

            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm] == (int)ModbusTcp.MesResponseToPlc.ResMachining //plc202指令
                && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] == 0//mes响应为0
                && ModbusTcp.PLC_MES_comfim_req_flage == false)//互换标识为false
            {

                finishmag = 0;
                finishmagload = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_comfirm]; ;
                finishmagunload = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_comfirm];

                if (finishmagload > 0)
                {

                    finishmag = finishmagload;
                }
                else if (finishmagunload > 0)
                {

                    finishmag = finishmagunload;
                }


                string finishmags = finishmag.ToString();
                // int cncmagno = finishmag;
                if (finishmag < 1)
                {
                    tempstring = "无料位号反馈";
                    if (language == "English")
                    {
                        tempstring = finishmags + "is error";
                    }
                    return;
                }


                ModbusTcp.PLC_MES_comfim_req_flage = true;//置位互换标识，开启互换

                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm];//置位MES响应信号

                int index = getmagnorowindex(finishmag);
                if (index == -1)
                {
                    if (MessageHasshow1 == true)
                    {
                        MessageHasshow1 = false;
                        MessageBox.Show("无法匹配完成信号的仓位信息！");
                    }
                    return;
                }
                MessageHasshow1 = true;
                if (dataGridVieworder.Rows[index].Cells[2].Value.ToString() == "车工序" || dataGridVieworder.Rows[index].Cells[2].Value.ToString() == "Lathe")
                {
                    if (MainForm.magprocesss1tate[finishmag - 1] == (int)Processstate.Loading)
                    {
                        MainForm.magprocesss1tate[finishmag - 1] = (int)Processstate.Loaded;
                        tempstring = finishmags + "号料上料完成";
                        if (language == "English")
                        {
                            tempstring = finishmags + "Load finished";
                        }
                    }
                    else if (MainForm.magprocesss1tate[finishmag - 1] == (int)Processstate.Unloading)
                    {
                        int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
                        int maglength = (int)ModbusTcp.MagLength;
                        int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statecnc1Finish;
                        MainForm.magprocesss1tate[finishmag - 1] = (int)Processstate.Unloaded;
                        dataGridVieworder2.Rows[index].Cells[2].Value = "完成";
                        tempstring = finishmags + "号料下料完成";
                        if (language == "English")
                        {
                            dataGridVieworder2.Rows[index].Cells[2].Value = "Finish";
                            tempstring = finishmags + "unload finished";
                        }
                        if (MainForm.cncv2list[0].MagNum == finishmag) // 更新机床完成信号
                        {
                            MainForm.cncv2list[0].MagNum = 0;
                        }
                    }
                    dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.LightGreen;
                }
                else if (dataGridVieworder.Rows[index].Cells[3].Value.ToString() == "车工序" || dataGridVieworder.Rows[index].Cells[3].Value.ToString() == "Lathe")
                {
                    if (MainForm.magprocesss2tate[finishmag - 1] == (int)Processstate.Loading)
                    {
                        MainForm.magprocesss2tate[finishmag - 1] = (int)Processstate.Loaded;
                        tempstring = finishmags + "号料上料完成";
                        if (language == "English")
                        {
                            tempstring = finishmags + "load finished";
                        }
                    }
                    else if (MainForm.magprocesss2tate[finishmag - 1] == (int)Processstate.Unloading)
                    {
                        int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
                        int maglength = (int)ModbusTcp.MagLength;
                        int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态
                        MainForm.magprocesss2tate[finishmag - 1] = (int)Processstate.Unloaded;

                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statecnc1Finish;
                        dataGridVieworder2.Rows[index].Cells[3].Value = "完成";
                        tempstring = finishmags + "号料下料完成";
                        if (language == "English")
                        {
                            dataGridVieworder2.Rows[index].Cells[3].Value = "Finish";
                            tempstring = finishmags + "Unload finished";
                        }
                        if (MainForm.cncv2list[0].MagNum == finishmag) // 更新机床完成信号
                        {
                            MainForm.cncv2list[0].MagNum = 0;
                        }
                        dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.LightGreen;
                    }
                }

            }
            if (ModbusTcp.PLC_MES_comfim_req_flage == true)
            {
                ModbusTcp.PLC_MES_comfim_req_count++;

                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response, 4, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();


                Thread.Sleep(2);
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm, 4);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();


                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm] == 0)
                {
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] = 0;//MES响应信号清零
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response] = 0;//MES响应信号清零
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response] = 0;//MES响应信号清零
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response] = 0;//MES响应信号清零
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response, 4, 1, 0);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();
                    if (ModbusTcp.PLC_MES_comfim_back_flage == false)
                    {
                        ModbusTcp.PLC_MES_comfim_back_flage = true;//置位请求plc响应信号
                    }
                }
                if (ModbusTcp.PLC_MES_comfim_back_flage == true)
                {

                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response, 4);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();

                    Thread.Sleep(2);
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response, 4);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();


                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm] == 0
                        && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response] == 0
                     && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response] == 0//MES响应信号清零
                     && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response] == 0//MES响应信号清零
                    && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] == 0)
                    {
                        ModbusTcp.PLC_MES_comfim_req_flage = false;
                        ModbusTcp.PLC_MES_comfim_back_flage = false;

                        MessageBox.Show(tempstring);

                    }
                }
            }

        }
        private void PLCToMEScncfun2()
        {

            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == (int)ModbusTcp.MesResponseToPlc.ResMachining //plc202指令
                && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] == 0//mes响应为0
                && ModbusTcp.PLC_MES_comfim_req_flage_2 == false)//互换标识为false
            {
                finishmag = 0;
                finishmagload = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_comfirm_2];
                finishmagunload = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_comfirm_2];
                if (finishmagload > 0)
                {

                    finishmag = finishmagload;
                }
                else if (finishmagunload > 0)
                {

                    finishmag = finishmagunload;
                }

                string finishmags = finishmag.ToString();
                // int cncmagno = finishmag;
                if (finishmag < 1)
                {
                    tempstring = "无料位号反馈";
                    if (language == "English")
                    {
                        tempstring = finishmags + "Load finished";
                    }
                    return;
                }

                ModbusTcp.PLC_MES_comfim_req_flage_2 = true;//置位互换标识，开启互换

                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2];//置位MES响应信号
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_comfirm_2];//置位MES响应信号
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_comfirm_2];//MES响应信号清零
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_comfirm_2];//MES响应信号清零


                int index = getmagnorowindex(finishmag);
                if (index == -1)
                {
                    if (MessageHasshow2 == true)
                    {
                        MessageHasshow2 = false;
                        MessageBox.Show("无法匹配完成信号的仓位信息！");
                    }
                    return;
                }
                MessageHasshow2 = true;
                if (dataGridVieworder.Rows[index].Cells[2].Value.ToString() == "铣工序" || dataGridVieworder.Rows[index].Cells[2].Value.ToString() == "CNC")
                {
                    if (MainForm.magprocesss1tate[finishmag - 1] == (int)Processstate.Loading)
                    {
                        MainForm.magprocesss1tate[finishmag - 1] = (int)Processstate.Loaded;
                        tempstring = finishmags + "号料上料完成";
                        if (language == "English")
                        {
                            tempstring = finishmags + "load finished";
                        }
                    }
                    else if (MainForm.magprocesss1tate[finishmag - 1] == (int)Processstate.Unloading)
                    {
                        int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
                        int maglength = (int)ModbusTcp.MagLength;
                        int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态

                        MainForm.magprocesss1tate[finishmag - 1] = (int)Processstate.Unloaded;
                        dataGridVieworder2.Rows[index].Cells[2].Value = "完成";
                        tempstring = finishmags + "号料下料完成";

                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statecnc2Finish;

                        if (language == "English")
                        {
                            dataGridVieworder2.Rows[index].Cells[2].Value = "Finish";
                            tempstring = finishmags + "Unload finished";
                        }
                        if (MainForm.cncv2list[1].MagNum == finishmag) // 更新机床完成信号
                        {
                            MainForm.cncv2list[1].MagNum = 0;
                        }
                    }
                }
                else if (dataGridVieworder.Rows[index].Cells[3].Value.ToString() == "铣工序" || dataGridVieworder.Rows[index].Cells[3].Value.ToString() == "CNC")
                {
                    if (MainForm.magprocesss2tate[finishmag - 1] == (int)Processstate.Loading)
                    {
                        MainForm.magprocesss2tate[finishmag - 1] = (int)Processstate.Loaded;
                        tempstring = finishmags + "号料上料完成";
                        if (language == "English")
                        {
                            tempstring = finishmags + "load finished";
                        }

                    }
                    else if (MainForm.magprocesss2tate[finishmag - 1] == (int)Processstate.Unloading)
                    {

                        int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
                        int maglength = (int)ModbusTcp.MagLength;
                        int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态

                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statecnc2Finish;
                        MainForm.magprocesss2tate[finishmag - 1] = (int)Processstate.Unloaded;
                        dataGridVieworder2.Rows[index].Cells[3].Value = "完成";
                        tempstring = finishmags + "号料下料完成";
                        if (language == "English")
                        {
                            dataGridVieworder2.Rows[index].Cells[3].Value = "Finish";
                            tempstring = finishmags + "Unload finished";
                        }
                        if (MainForm.cncv2list[1].MagNum == finishmag) // 更新机床完成信号
                        {
                            MainForm.cncv2list[1].MagNum = 0;
                        }
                    }
                }


                dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.LightGreen;
            }
            if (ModbusTcp.PLC_MES_comfim_req_flage_2 == true)
            {
                ModbusTcp.PLC_MES_comfim_req_count_2++;

                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();

                Thread.Sleep(2);
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();

                Thread.Sleep(2);
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2, 4);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();

                Thread.Sleep(2);
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2, 4);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();


                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == 0)
                {
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = 0;//MES响应信号清零
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response_2] = 0;//MES响应信号清零
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response_2] = 0;//MES响应信号清零
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response_2] = 0;//MES响应信号清零
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4, 1, 0);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();
                    if (ModbusTcp.PLC_MES_comfim_back_flage_2 == false)
                    {
                        ModbusTcp.PLC_MES_comfim_back_flage_2 = true;//置位请求plc响应信号
                    }
                }
                if (ModbusTcp.PLC_MES_comfim_back_flage_2 == true)
                {

                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();

                    Thread.Sleep(2);
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();


                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == 0
                    && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] == 0)
                    {
                        ModbusTcp.PLC_MES_comfim_req_flage_2 = false;
                        ModbusTcp.PLC_MES_comfim_back_flage_2 = false;

                        int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
                        int maglength = (int)ModbusTcp.MagLength;
                        int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态
                        int magchecki = MainForm.Mag_Check[finishmag - 1];
                        int index = getmagnorowindex(finishmag);

                        //   ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.Statecnc2Finish;
                        //if (magchecki == 1)//不合格
                        //{
                        //    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishNotStandard;

                        //    dataGridVieworder2.Rows[index].Cells[5].Value = "No";

                        //}
                        //else
                        //{
                        //    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;

                        //    dataGridVieworder2.Rows[index].Cells[5].Value = "Yes";

                        //}
                        MessageBox.Show(tempstring);
                    }
                }
            }
        }

        /// <summary>
        /// plc给mes测量请求信号
        /// </summary>
        //private void PLCToMEScncfun3()
        //{
        //    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == (int)ModbusTcp.MesResponseToPlc.ResMeterReq //plc202指令
        //        && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] == 0//mes响应为0
        //        && ModbusTcp.PLC_MES_comfim_req_meter == false)//互换标识为false
        //    {
        //        int cncno = 2;

        //        finishmag = MainForm.cnclist[1].MagNo;
        //        if (finishmag == 0)
        //        {
        //            return;
        //        }
        //        // int cncmagno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_comfirm_2];//获取完成信号绑定的料仓号
        //       // magnumber = cncmagno;
        //        ModbusTcp.PLC_MES_comfim_req_meter = true;//置位互换标识，开启互换
        //        //一号机床车床，2号机床加工中心

        //        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2];//置位MES响应信号

        //        string finishmags = finishmag.ToString();
        //        string cncnos = cncno.ToString();
        //        int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//料仓状态     
        //        int maglength = (int)ModbusTcp.MagLength;
        //        int curfuntemp = MainForm.Mag_Fun_cur[finishmag - 1];//当前正在执行的工序编号
        //        int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态
        //        int index = getmagnorowindex(finishmag);
        //        measureenable = true;//置位测量使能信号
        //        UpdateMeasureRes();//更新测量结果

        //    }
        //    if (ModbusTcp.PLC_MES_comfim_req_meter == true)
        //    {

        //        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4, 1, 0);//给plc写订单信息
        //        MainForm.sptcp1.ReceiveData();
        //        Thread.Sleep(2);

        //        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2, 4);//给plc写订单信息
        //        MainForm.sptcp1.ReceiveData();
        //        Thread.Sleep(2);

        //        if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == 0)
        //        {
        //            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = 0;//MES响应信号清零
        //            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response_2] = 0;//MES响应信号清零
        //            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response_2] = 0;//MES响应信号清零
        //            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response_2] = 0;//MES响应信号清零
        //            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4, 1, 0);//给plc写订单信息
        //            MainForm.sptcp1.ReceiveData();
        //            if (ModbusTcp.PLC_MES_comfim_back_meter == false)
        //            {
        //                ModbusTcp.PLC_MES_comfim_back_meter = true;//置位请求plc响应信号
        //            }
        //        }
        //        if (ModbusTcp.PLC_MES_comfim_back_meter == true)
        //        {

        //            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4);//给plc写订单信息
        //            MainForm.sptcp1.ReceiveData();

        //            Thread.Sleep(2);
        //            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4);//给plc写订单信息
        //            MainForm.sptcp1.ReceiveData();


        //            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == 0
        //            && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] == 0)
        //            {
        //                ModbusTcp.PLC_MES_comfim_req_meter = false;
        //                ModbusTcp.PLC_MES_comfim_back_meter = false;

        //                finishmag = MainForm.cnclist[1].MagNo;
        //                int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
        //                int maglength = (int)ModbusTcp.MagLength;
        //                int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态
        //                int magchecki = MainForm.Mag_Check[finishmag - 1];
        //                int index = getmagnorowindex(finishmag);

        //                if (language == "English")
        //                {
        //                    if (magchecki == 1)//不合格
        //                    {
        //                        dataGridVieworder2.Rows[index].Cells[6].Value = "Unqualified";
        //                        ShowForm.messagestring = "NO." + finishmag.ToString() + "is unqualified ,Please choose next step !";

        //                        //生成测量结果提示和选择
        //                        ReProcessChoose = true;
        //                        if (form1 == null)
        //                        {
        //                            form1 = new ShowForm();
        //                            form1.Visible = true;
        //                        }
        //                        else if (!form1.Visible)
        //                        {
        //                            form1.Visible = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dataGridVieworder2.Rows[index].Cells[6].Value = "Qualified";
        //                        ShowForm.messagestring = "NO." + finishmag.ToString() + "is qualified  ,Please choose next step !";

        //                        //生成测量结果提示和选择
        //                        ReProcessChoose = true;
        //                        if (form1 == null)
        //                        {
        //                            form1 = new ShowForm();
        //                            form1.Visible = true;
        //                        }
        //                        else if (!form1.Visible)
        //                        {
        //                            form1.Visible = true;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (magchecki == 1)//不合格
        //                    {
        //                        //dataGridVieworder2.Rows[index].Cells[7].Value = "不合格";
        //                        ShowForm.messagestring = finishmag.ToString() + "仓位工件不合格，请选择下一步 !";

        //                        //生成测量结果提示和选择
        //                        ReProcessChoose = true;

        //                        if (form1 == null)
        //                        {
        //                            form1 = new ShowForm();
        //                            form1.Visible = true;
        //                        }
        //                        else if (!form1.Visible)
        //                        {
        //                            form1.Visible = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dataGridVieworder2.Rows[index].Cells[6].Value = "合格";
        //                        ShowForm.messagestring = finishmag.ToString() + "仓位工件合格，请选择下一步  !";
        //                        // ShowForm form1 = new ShowForm();


        //                        //生成测量结果提示和选择
        //                        ReProcessChoose = true;
        //                        if (form1 == null)
        //                        {
        //                            form1 = new ShowForm();
        //                            form1.Visible = true;
        //                        }
        //                        else if (!form1.Visible)
        //                        {
        //                            form1.Visible = true;
        //                        }

        //                    }
        //                }

        //                dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.LightGreen;
        //            }
        //        }
        //    }
        //}



        //private void MessaureFun()
        //{
        //    int cncno = 2;
        //    finishmag = MainForm.cncv2list[1].MagNo;
        //    if (finishmag == 0)
        //    {
        //        return;
        //    }
        //    // int cncmagno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_comfirm_2];//获取完成信号绑定的料仓号
        //    // magnumber = cncmagno;
        //    //一号机床车床，2号机床加工中心
        //    string finishmags = finishmag.ToString();
        //    string cncnos = cncno.ToString();
        //    int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//料仓状态     
        //    int maglength = (int)ModbusTcp.MagLength;
        //    int curfuntemp = MainForm.Mag_Fun_cur[finishmag - 1];//当前正在执行的工序编号
        //    int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态
        //    int index = getmagnorowindex(finishmag);
        //    measureenable = true;//置位测量使能信号

        //    UpdateMeasureRes();//更新测量结果

        //    int magchecki = MainForm.Mag_Check[finishmag - 1];
        //    if (language == "English")
        //    {
        //        if (magchecki == 1)//不合格
        //        {
        //            dataGridVieworder2.Rows[index].Cells[5].Value = "NO";
        //            ShowForm.messagestring = "NO." + finishmag.ToString() + "is unqualified ,Please choose next step !";

        //            //生成测量结果提示和选择
        //            ReProcessChoose = true;
        //            if (form1 == null)
        //            {
        //                form1 = new ShowForm();
        //                form1.Visible = true;
        //            }
        //            else if (!form1.Visible)
        //            {
        //                form1.Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            dataGridVieworder2.Rows[index].Cells[5].Value = "Yes";
        //            ShowForm.messagestring = "NO." + finishmag.ToString() + "is qualified  ,Please choose next step !";
        //            //生成测量结果提示和选择
        //            ReProcessChoose = true;
        //            if (form1 == null)
        //            {
        //                form1 = new ShowForm();
        //                form1.Visible = true;
        //            }
        //            else if (!form1.Visible)
        //            {
        //                form1.Visible = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (magchecki == 1)//不合格
        //        {
        //            //dataGridVieworder2.Rows[index].Cells[7].Value = "不合格";
        //            ShowForm.messagestring = finishmag.ToString() + "仓位工件不合格，请选择下一步 !";

        //            //生成测量结果提示和选择
        //            ReProcessChoose = true;

        //            if (form1 == null)
        //            {
        //                form1 = new ShowForm();
        //                form1.Visible = true;
        //            }
        //            else if (!form1.Visible)
        //            {
        //                form1.Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            dataGridVieworder2.Rows[index].Cells[5].Value = "Yes";
        //            ShowForm.messagestring = finishmag.ToString() + "仓位工件合格，请选择下一步  !";
        //            // ShowForm form1 = new ShowForm();


        //            //生成测量结果提示和选择
        //            ReProcessChoose = true;
        //            if (form1 == null)
        //            {
        //                form1 = new ShowForm();
        //                form1.Visible = true;
        //            }
        //            else if (!form1.Visible)
        //            {
        //                form1.Visible = true;
        //            }

        //        }
        //    }
        //    dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.LightGreen;


        //}
        private void MessaureFun(bool t = true)
        {
            int cncno = 2;
           
            if (finishmag == 0)
            {
                return;
            }
        
            // int cncmagno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_comfirm_2];//获取完成信号绑定的料仓号
            // magnumber = cncmagno;
            //一号机床车床，2号机床加工中心
            string finishmags = finishmag.ToString();
            string cncnos = cncno.ToString();
            int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//料仓状态     
            int maglength = (int)ModbusTcp.MagLength;
            int curfuntemp = MainForm.Mag_Fun_cur[finishmag - 1];//当前正在执行的工序编号
            int magstatei = magstatestart + maglength * (finishmag - 1);//料仓状态
            int index = getmagnorowindex(finishmag);
            measureenable = true;//置位测量使能信号

            UpdateMeasureRes(true);//更新测量结果

            int magchecki = MainForm.Mag_Check[finishmag - 1];
            if (language == "English")
            {
                if (magchecki == 1)//不合格
                {
                    dataGridVieworder2.Rows[index].Cells[5].Value = "NO";
                    ShowForm.messagestring = "NO." + finishmag.ToString() + "is unqualified ,Please choose next step !";

                    //生成测量结果提示和选择
                    ReProcessChoose = true;
                    if (form1 == null)
                    {
                        form1 = new ShowForm();
                        form1.Visible = true;
                    }
                    else if (!form1.Visible)
                    {
                        form1.Visible = true;
                    }
                }
                else
                {
                    dataGridVieworder2.Rows[index].Cells[5].Value = "Yes";
                    ShowForm.messagestring = "NO." + finishmag.ToString() + "is qualified  ,Please choose next step !";
                    //生成测量结果提示和选择
                    ReProcessChoose = true;
                    if (form1 == null)
                    {
                        form1 = new ShowForm();
                        form1.Visible = true;
                    }
                    else if (!form1.Visible)
                    {
                        form1.Visible = true;
                    }
                }
            }
            else
            {
                if (magchecki == 1)//不合格
                {
                    //dataGridVieworder2.Rows[index].Cells[7].Value = "不合格";
                    ShowForm.messagestring = finishmag.ToString() + "仓位工件不合格，请选择下一步 !";

                    //生成测量结果提示和选择
                    ReProcessChoose = true;

                    if (form1 == null)
                    {
                        form1 = new ShowForm();
                        form1.Visible = true;
                    }
                    else if (!form1.Visible)
                    {
                        form1.Visible = true;
                    }
                }
                else
                {
                    dataGridVieworder2.Rows[index].Cells[5].Value = "Yes";
                    ShowForm.messagestring = finishmag.ToString() + "仓位工件合格，请选择下一步  !";
                    // ShowForm form1 = new ShowForm();


                    //生成测量结果提示和选择
                    ReProcessChoose = true;
                    if (form1 == null)
                    {
                        form1 = new ShowForm();
                        form1.Visible = true;
                    }
                    else if (!form1.Visible)
                    {
                        form1.Visible = true;
                    }

                }
            }
            dataGridVieworder.Rows[index].Cells[6].Style.BackColor = Color.LightGreen;


        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxorderno_Leave(object sender, EventArgs e)
        {
            string OrderNoStr = ((TextBox)sender).Text;
            if (OrderNoStr == "")
            {
                return;
            }
            try
            {
                int OrderNo = Convert.ToInt16(OrderNoStr);
                if (OrderNo > 30 || OrderNo <= 0)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("Please enter number between 1 to 30");
                    }
                    else
                        MessageBox.Show("请输入1-30之间的数字");
                    textBoxorderno.Focus();
                }
                else
                {
                    // textBoxorderno.Focus();           
                }
            }
            catch
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number between 1 to 30");
                }
                else
                    MessageBox.Show("请输入1-30之间的数字");
                textBoxorderno.Focus();
            }
        }

        private void textBoxorderno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number between 1 to 30");
                }
                else
                    MessageBox.Show("请输入1-30之间的数字");
                textBoxorderno.Focus();
            }

        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            string OrderNoStr = ((TextBox)sender).Text;
            if (OrderNoStr == "")
            {
                return;
            }
            try
            {
                int OrderNo = Convert.ToInt16(OrderNoStr);

                if (OrderNo > 30 || OrderNo < 0)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("Please enter number between 1 to 30");
                    }
                    else
                        MessageBox.Show("请输入1-30之间的数字");
                    textBox1.Focus();
                }
                else
                {
                    //  textBox1.Focus();
                }
            }
            catch
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number between 1 to 30");
                }
                else
                    MessageBox.Show("请输入1-30之间的数字");
                textBox1.Focus();
            }

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number");
                }
                else
                    MessageBox.Show("请输入数字");
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string OrderNoStr = textBoxorderno.Text;
            string magno = "";
            int magnoi = -1;
            if (OrderNoStr == "")
            {
                return;
            }
            int OrderNo = Convert.ToInt16(OrderNoStr) - 1;
            if (OrderNo > dataGridVieworder.RowCount - 1)
            {
                if (language == "English")
                {
                    MessageBox.Show("The Order has not been generated");
                }
                else
                    MessageBox.Show("当前订单号还没有生成");
                return;
            }
            else
            {
                magno = dataGridVieworder2.Rows[OrderNo].Cells[1].Value.ToString();
                magnoi = Convert.ToInt16(magno);
                if (magnoi < 1)
                {
                    return;
                }
                if (dataGridVieworder2.Rows[OrderNo].Cells[4].Value.ToString() == "进行中" ||
                    dataGridVieworder2.Rows[OrderNo].Cells[4].Value.ToString() == "Processing")
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The Order  can't be delete,becaude the order is producing");
                    }
                    else
                        MessageBox.Show("当前订单正在生产，无法删除");
                }
                else if (dataGridVieworder2.Rows[OrderNo].Cells[4].Value.ToString() == "报警" ||
               dataGridVieworder2.Rows[OrderNo].Cells[4].Value.ToString() == "Alarm")
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The Order  can't be delete,becaude the order is producing");
                    }
                    else
                        MessageBox.Show("当前订单正在生产，无法删除");
                }
                else
                {
                    dataGridVieworder2.Rows.RemoveAt(OrderNo);
                    dataGridVieworder.Rows.RemoveAt(OrderNo);
                    int itemcount = 0;
                    for (int jj = 0; jj < dataGridVieworder.Rows.Count; jj++)
                    {
                        itemcount = jj + 1;
                        dataGridVieworder.Rows[jj].Cells[0].Value = itemcount.ToString();

                    }
                    for (int jj = 0; jj < dataGridVieworder2.Rows.Count; jj++)
                    {
                        itemcount = jj + 1;
                        dataGridVieworder2.Rows[jj].Cells[0].Value = itemcount.ToString();

                    }
                    MainForm.magprocesss1tate[magnoi - 1] = 0;
                    MainForm.magprocesss2tate[magnoi - 1] = 0;
                    MainForm.ordertate[magnoi - 1] = 0;
                    //MainForm.magisordered
                    MainForm.magisordered[magnoi - 1] = 0;
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string OrderNoStr = textBox3.Text;
            if (OrderNoStr == "")
            {
                return;
            }
            int OrderNo = Convert.ToInt16(OrderNoStr);

            if (OrderNo > dataGridVieworder.RowCount)
            //  if (OrderNo > dataGridVieworder.RowCount - 1)
            {
                if (language == "English")
                {
                    MessageBox.Show("The Order has not been generated");
                }
                else
                    MessageBox.Show("当前订单号还没有生成");
            }
            else
            {
                if (dataGridVieworder2.Rows[OrderNo - 1].Cells[4].Value.ToString() == "进行中" ||
                    dataGridVieworder2.Rows[OrderNo - 1].Cells[4].Value.ToString() == "Processing" ||
                    dataGridVieworder2.Rows[OrderNo - 1].Cells[4].Value.ToString() == "报警" ||
                    dataGridVieworder2.Rows[OrderNo - 1].Cells[4].Value.ToString() == "Alarm")
                {
                    string magnos = dataGridVieworder2.Rows[OrderNo - 1].Cells[1].Value.ToString();
                    int magoi = Convert.ToInt16(magnos);
                    int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;

                    int maglength = (int)ModbusTcp.MagLength;

                    int magstatei = magstart + maglength * (magoi - 1);

                    if (magoi == MainForm.cncv2list[0].MagNum)//车床
                    {
                        MainForm.cncv2list[0].MagNum = 0;
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFailure;//加工异常

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response_2] = 0;//MES响应信号清零
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response_2] = 0;//MES响应信号清零
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response] = 0;

                        if (language == "English")
                        {
                            MessageBox.Show("The cnc1 process call back");
                        }
                        else
                            MessageBox.Show("车床工序取消");
                    }
                    if (magoi == MainForm.cncv2list[1].MagNum)//车床
                    {
                        MainForm.cncv2list[1].MagNum = 0;
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFailure; ;//加工异常

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response_2] = 0;//MES响应信号清零
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response_2] = 0;//MES响应信号清零
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response_2] = 0;

                        if (language == "English")
                        {
                            MessageBox.Show("The cnc2 process call back");
                        }
                        else
                            MessageBox.Show("加工中心工序取消");
                    }
                    if (magoi == MainForm.cncv2list[0].MagNum)//车床
                    {
                        MainForm.cncv2list[0].MagNum = 0;
                    }
                    if (magoi == MainForm.cncv2list[1].MagNum)//车床
                    {
                        MainForm.cncv2list[1].MagNum = 0; ;
                    }

                    if (language == "English")
                    {
                        dataGridVieworder2.Rows[OrderNo - 1].Cells[4].Value = "Reback";
                    }
                    else dataGridVieworder2.Rows[OrderNo - 1].Cells[4].Value = "撤销";

                    MainForm.ordertate[magoi - 1] = (int)Orderstate.Reback;
                    //if (aotorunmag == magoi)
                    //{
                    aotorunmag = 0;
                    aotorunmaglathe = 0;
                    aotorunmagcnc = 0;
                    //}
                }
                else
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The order is not processing");
                    }
                    else
                        MessageBox.Show("当前订单不是正在生产中，不用撤回");
                }

            }

        }

        private void orderstaterecord()
        {
            string magnos = "";
            int magno = -1;
            for (int jj = 0; jj < dataGridVieworder.Rows.Count; jj++)
            {
                magnos = dataGridVieworder.Rows[jj].Cells[1].Value.ToString();
                magno = Convert.ToInt32(magnos);
                if (dataGridVieworder.Rows[jj].Cells[2].Value.ToString() == "无" || dataGridVieworder.Rows[jj].Cells[2].Value.ToString() == "None")
                {
                    MainForm.magprocesss1tate[magno - 1] = (int)Processstate.None;
                }
                else if (dataGridVieworder.Rows[jj].Cells[3].Value.ToString() == "无" || dataGridVieworder.Rows[jj].Cells[3].Value.ToString() == "None")
                {

                    MainForm.magprocesss2tate[magno - 1] = (int)Processstate.None;
                }
                if (MainForm.ordertate[magno - 1] == (int)Orderstate.Reback)
                {
                    MainForm.ordertate[magno - 1] = (int)Orderstate.Reback;
                    dataGridVieworder2.Rows[jj].Cells[4].Value = "撤销";

                    dataGridVieworder.Rows[jj].Cells[8].Value = "撤销";
                    if (language == "English")
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "Reback";
                        dataGridVieworder.Rows[jj].Cells[8].Value = "Reback";
                    }

                }
                else if ((MainForm.magprocesss1tate[magno - 1] == (int)Processstate.None && MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Unloaded)
                || (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Unloaded && MainForm.magprocesss2tate[magno - 1] == (int)Processstate.None)
                || (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Unloaded && MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Unloaded))
                {
                    int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//料仓状态     
                    int maglength = (int)ModbusTcp.MagLength;
                    int magstatei = magstatestart + maglength * (magno - 1);//料仓状态

                    MainForm.ordertate[magno - 1] = (int)Orderstate.Finish;
                    if (MainForm.Mag_Check[magno - 1] == 1)//不合格
                    {
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishNotStandard;

                        dataGridVieworder2.Rows[jj].Cells[5].Value = "No";
                    }
                    else
                    {
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;

                        dataGridVieworder2.Rows[jj].Cells[5].Value = "Yes";
                    }


                }
                else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.None && MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Notstart)
                {
                    MainForm.ordertate[magno - 1] = (int)Orderstate.Notstart;

                }
                else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Notstart && MainForm.magprocesss2tate[magno - 1] == (int)Processstate.None)
                {

                    MainForm.ordertate[magno - 1] = (int)Orderstate.Notstart;
                }
                else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Notstart && MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Notstart)
                {
                    MainForm.ordertate[magno - 1] = (int)Orderstate.Notstart;
                }
                else
                {
                    MainForm.ordertate[magno - 1] = (int)Orderstate.Processing;
                }
                if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Alarm || MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Alarm)
                {
                    if (MainForm.ordertate[magno - 1] != (int)Orderstate.Reback)
                    {
                        MainForm.ordertate[magno - 1] = (int)Orderstate.Alarm; ;
                    }
                }
                if (MainForm.ordertate[magno - 1] == (int)Orderstate.Reback)
                {

                }
                else if (language == "Chinese")
                {
                    if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.None)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "无";

                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Notstart)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "未开始";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Loading)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "上料中";

                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Loaded)
                    {

                        dataGridVieworder2.Rows[jj].Cells[2].Value = "上料完成";

                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Running)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "加工中";

                    }

                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Rerunning)
                    {

                        dataGridVieworder2.Rows[jj].Cells[2].Value = "返修中";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Runned)
                    {

                        dataGridVieworder2.Rows[jj].Cells[2].Value = "加工完成";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Unloading)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "下料中";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Unloaded)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "下料完成";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Alarm)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "报警";
                    }



                    if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.None)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "无";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Notstart)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "未开始";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Loading)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "上料中";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Loaded)
                    {

                        dataGridVieworder2.Rows[jj].Cells[3].Value = "上料完成";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Running)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "加工中";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Runned)
                    {

                        dataGridVieworder2.Rows[jj].Cells[3].Value = "加工完成";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Rerunning)
                    {

                        dataGridVieworder2.Rows[jj].Cells[3].Value = "返修中";

                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Unloading)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "下料中";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Unloaded)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "下料完成";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Alarm)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "报警";
                    }


                    if (MainForm.ordertate[magno - 1] == (int)Orderstate.Notstart)
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "未开始";
                    }
                    else if (MainForm.ordertate[magno - 1] == (int)Orderstate.Processing)
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "进行中";
                    }
                    else if (MainForm.ordertate[magno - 1] == (int)Orderstate.Finish)
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "完成";
                    }
                    else if (MainForm.ordertate[magno - 1] == (int)Orderstate.Alarm)
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "报警";
                    }

                    // 订单状态
                    //if (MainForm.ordertate[magno - 1] == (int)Orderstate.Notstart)
                    //{
                    //    dataGridVieworder2.Rows[jj].Cells[4].Value = "未开始";
                    //}
                    //else if (MainForm.ordertate[magno - 1] == (int)Orderstate.Processing)
                    //{
                    //    dataGridVieworder2.Rows[jj].Cells[4].Value = "进行中";
                    //}
                    //else if (MainForm.ordertate[magno - 1] == (int)Orderstate.Finish)
                    //{
                    //    dataGridVieworder2.Rows[jj].Cells[4].Value = "完成";
                    //}
                }
                else
                {
                    if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.None)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "None";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Notstart)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Notstart";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Loading)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Loading";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Loaded)
                    {

                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Loaded";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Running)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Running";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Rerunning)
                    {

                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Rerunning";

                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Runned)
                    {

                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Runned";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Unloading)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Unloading";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Unloaded)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Unloaded";
                    }
                    else if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Alarm)
                    {
                        dataGridVieworder2.Rows[jj].Cells[2].Value = "Alarm";
                    }

                    if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.None)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "None";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Notstart)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Notstart";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Loading)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Loading";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Loaded)
                    {

                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Loaded";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Running)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Running";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Rerunning)
                    {

                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Rerunning";

                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Runned)
                    {

                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Runned";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Unloading)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Unloading";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Unloaded)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Unloaded";
                    }
                    else if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Alarm)
                    {
                        dataGridVieworder2.Rows[jj].Cells[3].Value = "Alarm";
                    }

                    // 订单状态
                    if (MainForm.ordertate[magno - 1] == (int)Orderstate.Notstart)
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "Notstart";
                    }
                    else if (MainForm.ordertate[magno - 1] == (int)Orderstate.Processing)
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "Processing";
                    }
                    else if (MainForm.ordertate[magno - 1] == (int)Orderstate.Finish)
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "Finish";
                    }
                    else if (MainForm.ordertate[magno - 1] == (int)Orderstate.Alarm)
                    {
                        dataGridVieworder2.Rows[jj].Cells[4].Value = "Alarm";
                    }
                }

                string str1 = dataGridVieworder2.Rows[jj].Cells[2].Value.ToString();
                string str2 = dataGridVieworder2.Rows[jj].Cells[3].Value.ToString();
                string str3 = str1 + ";" + str2 + ";";
                dataGridVieworder.Rows[jj].Cells[8].Value = str3;
            }
        }
        private void rerunningmeter()
        {
            if (OrderForm1.rerunningflage)//返修开始
            {
                int magno = MainForm.cncv2list[1].MagNum;//返修仓位号

                int index = getmagnorowindex(magno);
                if (index < 0)
                {
                    OrderForm1.rerunningflage = false;
                    return;
                }

              
                if (dataGridVieworder.Rows[index].Cells[2].Value.ToString() == "铣工序" || dataGridVieworder.Rows[index].Cells[2].Value.ToString() == "CNC")
                {
                    if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Runned)
                    {
                        if (MainForm.cncv2list[1].EquipmentState == "运行")
                        {
                            MainForm.magprocesss1tate[magno - 1] = (int)Processstate.Rerunning;
                            IsRerunning = false;
                        }
                    }
                    if (MainForm.magprocesss1tate[magno - 1] == (int)Processstate.Rerunning && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Cnc_finish_state] == 1)
                    {
                        if (MainForm.cncv2list[1].EquipmentState == "空闲")
                        {
                            MainForm.magprocesss1tate[magno - 1] = (int)Processstate.Runned;

                            MessaureFun(true);
                            OrderForm1.rerunningflage = false;
                        }
                    }
                }
                else if (dataGridVieworder.Rows[index].Cells[3].Value.ToString() == "铣工序" || dataGridVieworder.Rows[index].Cells[3].Value.ToString() == "CNC")
                {
                    if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Runned)
                    {
                        if (MainForm.cncv2list[1].EquipmentState == "运行")
                        {
                            MainForm.magprocesss2tate[magno - 1] = (int)Processstate.Rerunning;
                            IsRerunning = false;
                        }
                    }
                    if (MainForm.magprocesss2tate[magno - 1] == (int)Processstate.Rerunning)
                    {
                        if (MainForm.cncv2list[1].EquipmentState == "空闲" && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Cnc_finish_state] == 1)
                        {
                            MainForm.magprocesss2tate[magno - 1] = (int)Processstate.Runned;

                            MessaureFun(true);
                            OrderForm1.rerunningflage = false;
                        }
                    }
                }

            }
        }
        private void cncprocessstate()
        {
            if (MainForm.cncv2list.Count < 2)
            {
                return;
            }
            int magno1 = MainForm.cncv2list[0].MagNum;
            int magno2 = MainForm.cncv2list[1].MagNum;

            int index1 = getmagnorowindex(magno1);
            int index2 = getmagnorowindex(magno2);
       
            //if (MainForm.cnclist != null)
            //{
                //if (MainForm.cnclist.Count >= 2)
                //{
                //    cncalarmcount1 = MainForm.cnclist[0].HCNCShareData.currentAlarmList.Count;
                //    cncalarmcount2 = MainForm.cnclist[1].HCNCShareData.currentAlarmList.Count;
                //}
                //else if (MainForm.cnclist.Count >= 1)
                //{
                //    cncalarmcount1 = MainForm.cnclist[0].HCNCShareData.currentAlarmList.Count;
                //}
            //}

            if (magno1 > 0 && index1 < 0)
            {
                MainForm.cncv2list[0].MagNum = 0;
                return;
            }
            if (magno2 < 0 && index2 < 0)
            {
                MainForm.cncv2list[0].MagNum = 0;
                return;
            }

           string CNCState0 ="";

            string CNCState1 ="";
             CNCState0 = MainForm.cncv2list[0].EquipmentState;
          

             CNCState1 = MainForm.cncv2list[1].EquipmentState;
         


            if (magno1 != 0)
            {
                if (dataGridVieworder.Rows[index1].Cells[2].Value.ToString() == "车工序" || dataGridVieworder.Rows[index1].Cells[2].Value.ToString() == "Lathe")
                {
                    //if (cncalarmcount1 > 0)//车床报警
                    //{

                    //    MainForm.magprocesss1tate[magno1 - 1] = (int)Processstate.Alarm;
                    //}
                    if (MainForm.magprocesss1tate[magno1 - 1] == (int)Processstate.Loaded)
                    {
                        if (CNCState0 == "运行")
                        {
                            MainForm.magprocesss1tate[magno1 - 1] = (int)Processstate.Running;
                        }
                    }
                    if (MainForm.magprocesss1tate[magno1 - 1] == (int)Processstate.Running)
                    {
                        if (CNCState0 == "空闲" && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Lathe_finish_state] ==1 )
                        {
                            MainForm.magprocesss1tate[magno1 - 1] = (int)Processstate.Runned;

                            dataGridVieworder.Rows[index1].Cells[6].Style.BackColor = Color.LightGreen;
                        }
                    }
                }
                if (dataGridVieworder.Rows[index1].Cells[3].Value.ToString() == "车工序" || dataGridVieworder.Rows[index1].Cells[2].Value.ToString() == "Lathe")
                {
                    //if (cncalarmcount1 > 0)//车床报警
                    //{

                    //    MainForm.magprocesss2tate[magno1 - 1] = (int)Processstate.Alarm;
                    //}
                    if (MainForm.magprocesss2tate[magno1 - 1] == (int)Processstate.Loaded)
                    {
                        if (CNCState0 == "运行")
                        {
                            MainForm.magprocesss2tate[magno1 - 1] = (int)Processstate.Running;
                        }
                    }
                    if (MainForm.magprocesss2tate[magno1 - 1] == (int)Processstate.Running)
                    {
                        if (CNCState0 == "空闲" && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Lathe_finish_state] ==1 )
                        {
                            MainForm.magprocesss2tate[magno1 - 1] = (int)Processstate.Runned;

                            dataGridVieworder.Rows[index1].Cells[6].Style.BackColor = Color.LightGreen;
                        }
                    }
                }
            }
            if (magno2 != 0)
            {
                if (dataGridVieworder.Rows[index2].Cells[2].Value.ToString() == "铣工序" || dataGridVieworder.Rows[index2].Cells[2].Value.ToString() == "CNC")
                {
                  
                    if (MainForm.magprocesss1tate[magno2 - 1] == (int)Processstate.Loaded)
                    {
                        if (CNCState1 == "运行")
                        {
                            MainForm.magprocesss1tate[magno2 - 1] = (int)Processstate.Running;
                        }
                    }
                    if (MainForm.magprocesss1tate[magno2 - 1] == (int)Processstate.Running)
                    {
                        if (CNCState1 == "空闲" && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Cnc_finish_state] == 1)
                        {
                            MainForm.magprocesss1tate[magno2 - 1] = (int)Processstate.Runned;
                            MessaureFun(true);
                        }
                    }
                }
                if (dataGridVieworder.Rows[index2].Cells[3].Value.ToString() == "铣工序" || dataGridVieworder.Rows[index2].Cells[2].Value.ToString() == "CNC")
                {
                    //if (cncalarmcount2 > 0)//车床报警
                    //{

                    //    MainForm.magprocesss2tate[magno2 - 1] = (int)Processstate.Alarm;
                    //}
                    if (MainForm.magprocesss2tate[magno2 - 1] == (int)Processstate.Loaded)
                    {
                        if (CNCState1 == "运行")
                        {
                            MainForm.magprocesss2tate[magno2 - 1] = (int)Processstate.Running;
                        }
                    }
                    if (MainForm.magprocesss2tate[magno2 - 1] == (int)Processstate.Running)
                    {
                        if (CNCState1 == "空闲" && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Cnc_finish_state] == 1)
                        {
                            MainForm.magprocesss2tate[magno2 - 1] = (int)Processstate.Runned;
                            MessaureFun(true);
                        }
                    }
                }
            }
        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {

            //MeterlogFileadd(MetetrrecordFilePath);

            //if (MainForm.paichengformshowflage)
            //{
            //    if (MainForm.orderformshowflage)
            //    {
            //        InitOrderdata(OrderdataFilePath);
            //        InitOrderstate(OrderstateFilePath);
            //        MainForm.paichengformshowflage = false;
            //        MainForm.Orderinsertflage = false;
            //    }
            //}

            cncprocessstate();
            orderstaterecord();

            if (aotomode)
            {
                labelmode.Text = "自动模式";
                if (language == "English")
                    labelmode.Text = "Aoto mode";
                buttonstart1.BackColor = Color.Gray;
                buttonstop1.BackColor = Color.LightGreen;
                for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
                {
                    dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.Gray;
                    dataGridVieworder.Rows[ii].Cells[7].Style.BackColor = Color.Gray;
                }
            }
            else if (aotostop)
            {

                OrderForm1.IsRerunning = false;
                labelmode.Text = "自动模式暂停";
                if (language == "English")
                    labelmode.Text = "Stop mode";
                buttonstart1.BackColor = Color.LightGreen;
                buttonstop1.BackColor = Color.Gray;
                for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
                {
                    dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.Gray;
                    dataGridVieworder.Rows[ii].Cells[7].Style.BackColor = Color.Gray;
                }
            }
            else
            {
                // buttonman.BackColor = Color.Gray;                 
                // buttonaoto.BackColor = Color.LightGreen;

                OrderForm1.IsRerunning = false;
                labelmode.Text = "手动模式";
                if (language == "English")
                    labelmode.Text = "mannul mode";
                for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
                {
                    dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.LightGreen;
                    dataGridVieworder.Rows[ii].Cells[7].Style.BackColor = Color.LightGreen;
                }
            }
            if (MainForm.cncv2list.Count < 2)
            {
                textBox1.Text = "0";
                textBox2.Text = "0";
            }
            else
            {
                textBox1.Text = MainForm.cncv2list[0].MagNum.ToString();
                textBox2.Text = MainForm.cncv2list[1].MagNum.ToString();

            }
            language = ChangeLanguage.GetDefaultLanguage();
            if (!ReProcessChoose)
            {
                form1.Visible = false;
            }
            if (MainForm.Orderinitflage == false)
            {
                MainForm.Orderinitflage = true;
            }
            if (dataGridVieworder != null)
            {
                if (dataGridVieworder.Rows.Count >= 0 && MainForm.Orderinsertflage == false)
                {
                    Orderdatasave(OrderdataFilePath); //20180502
                }
            }


            if (dataGridVieworder2 != null)
            {
                if (dataGridVieworder2.Rows.Count >= 0 && MainForm.Orderinsertflage == false)
                {
                    Orderstatesave(OrderstateFilePath); //20180502
                }
            }

            //下达命令交互
            MESToPLCcomfirmfun();
            //下达订单交互
            MESToPLCorderfun(); //请求加工信息交互
            PLCToMEScncfun();//车床完成信号处理

            PLCToMEScncfun2();//铣床完成信号处理

            //  PLCToMEScncfun3();//铣床测量信号处理
            rerunningmeter();

            RackmessageSync();//同步料仓数据//请求料架有无料信息
           if (RackForm.meterialchange)
           {
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Meterial, 1, 1, 0);//给plc写订单信息

                MainForm.sptcp1.ReceiveData();
               //RackForm.meterialchange = false;
            }//材质修改
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm, 10);//查询plc_mes的命令
            MainForm.sptcp1.ReceiveData();
            MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.req, 1, 0, (int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_status, 28);//请求61、62号寄存器存储内容是料位有无料信息      
            MainForm.sptcp1.ReceiveData();
            //MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.writereg, (int)SCADA.ModbusTcp.DataConfigArr.f_Jpos, 21, 1, 0);//写机器人位置
               
            //if (MeterForm.renewbiaodingfalge)
            //{
                //MeterForm.renewbiaodingfalge =false;
                MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.writereg, (int)SCADA.ModbusTcp.DataConfigArr.p_MeterValue1, 54, 1, 0);//请求61、62号寄存器存储内容是料位有无料信息      
                MainForm.sptcp1.ReceiveData();
            //}
             
            if (aotomode)
            {
                AotoOrderRun();
            }
        }

        /// <summary>
        ///  获取当前车床正在执行的仓位号和订单号
        /// </summary>
        private void Getlathenumber()
        {
           // aotorunmaglathe = 0;
            if (aotorunmaglathe > 0)
            {
                //查询仓位对应的订单序号
                indexlathe = -1;
                string strlathe = aotorunmaglathe.ToString();
                for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
                {
                    if (dataGridVieworder.Rows[ii].Cells[1].Value.ToString() == strlathe)
                    {
                        indexlathe = ii;
                    }
                }
                if (indexlathe == -1)
                {

                    aotomode = false;
                    aotostop = true;
                    if (language == "English")
                    {
                        MessageBox.Show("The Order message is err!");
                    }
                    else MessageBox.Show(" 订单信息读取错误，自动加工暂停!");
                    return;
                }
            

                if (dataGridVieworder.Rows[indexlathe].Cells[2].Value.ToString() == "Lathe" ||
                   dataGridVieworder.Rows[indexlathe].Cells[2].Value.ToString() == "车工序")//查询订单中车步骤为第一还是第二工序
                {
                    if (MainForm.magprocesss1tate[aotorunmaglathe - 1] == (int)Processstate.Unloaded)
                    {
                        aotorunmaglathe = 0;
                    };
                }
                if (dataGridVieworder.Rows[indexlathe].Cells[3].Value.ToString() == "Lathe" ||
                   dataGridVieworder.Rows[indexlathe].Cells[3].Value.ToString() == "车工序")//查询订单中车步骤为第一还是第二工序
                {
                    if (MainForm.magprocesss2tate[aotorunmaglathe - 1] == (int)Processstate.Unloaded)
                    {
                        aotorunmaglathe = 0;
                    };
                }

            }
            if (aotorunmaglathe == 0)
            {
                fideaotorunmaglathe();
            }
        }
        private void Getcncnumber()
        {
            if (aotorunmagcnc > 0)
            {
                indexcnc = -1;
                string strcnc = aotorunmagcnc.ToString();
                for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
                {
                    if (dataGridVieworder.Rows[ii].Cells[1].Value.ToString() == strcnc)
                    {
                        indexcnc = ii;
                    }
                }
                if (indexcnc == -1)
                {

                    aotomode = false;
                    aotostop = true;
                    if (language == "English")
                    {
                        MessageBox.Show("The Order message is err!");
                    }
                    else MessageBox.Show(" 订单信息读取错误，自动加工暂停!");
                    return;
                }

                if (dataGridVieworder.Rows[indexcnc].Cells[2].Value.ToString() == "Cnc" ||
                  dataGridVieworder.Rows[indexcnc].Cells[2].Value.ToString() == "铣工序")//查询订单中车步骤为第一还是第二工序
                {
                    if (MainForm.magprocesss1tate[aotorunmagcnc - 1] == (int)Processstate.Unloaded)
                    {
                        aotorunmagcnc = 0;
                    };
                }
                if (dataGridVieworder.Rows[indexcnc].Cells[3].Value.ToString() == "Cnc" ||
                   dataGridVieworder.Rows[indexcnc].Cells[3].Value.ToString() == "铣工序")//查询订单中车步骤为第一还是第二工序
                {
                    if (MainForm.magprocesss2tate[aotorunmagcnc - 1] == (int)Processstate.Unloaded)
                    {
                        aotorunmagcnc = 0;
                    }
                }
            }

            if (aotorunmagcnc == 0)
            {
                fideaotorunmagcnc();
            }

        }
        /// <summary>
        /// 下发任务
        /// </summary>
        /// <param name="number">仓位编号</param>
        /// <param name="rowindex">订单编号</param>
        /// <returns>1-下发了新的任务，0-没有下发新的任务，2-下发订单时出现错误</returns>
        private int AotoRunLathefun(int number, int rowindex,ref string messagestring)
        {
            messagestring = "";
            if (dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString() == "Lathe" ||
                    dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString() == "车工序")//车床工序
            {
                if (!(MainForm.magprocesss1tate[number - 1] == (int)Processstate.None
                || MainForm.magprocesss1tate[number - 1] == (int)Processstate.Unloaded))
                {
                    int ret = 0;
                    if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Alarm)
                    {

                        aotomode = false;
                        aotostop = true;
                        messagestring = messagestring + "车床有报警或者提示，自动加工暂停";
                        if (language == "English")
                        {
                            messagestring = messagestring + "The order is err,change to aotostop mode ";
                        }
                        return 2;
                    }
                    if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Notstart)//上料
                    {
                        ret = CNCLoadfun(number, 1, ref messagestring);//车床上料 ;//机床是否能执行
                        if (ret == 0)
                        {
                            MainForm.magprocesss1tate[number - 1] = (int)Processstate.Loading;
                            TopRangeDataGridView(rowindex);
                            return 1;
                        }
                        else
                        {


                            aotomode = false;
                            aotostop = true;
                            messagestring =messagestring+ "订单下发失败,自动加工暂停";
                            if (language == "English")
                            {
                                messagestring = messagestring + "The order down err,change to manmode";
                            }

                            return 2;

                        }
                    }
                    if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Runned)//下料
                    {
                        ret = CNCUnLoadfun(number, 1, ref messagestring);//车床上料 ;//机床是否能执行
                        if (ret == 0)
                        {
                            MainForm.magprocesss1tate[number - 1] = (int)Processstate.Unloading;
                            return 1;
                        }
                        else
                        {

                            aotomode = false;
                            aotostop = true;
                            messagestring = "订单下发失败,自动加工暂停";
                            if (language == "English")
                            {
                                messagestring = "The order down err,change to manmode";
                            }

                            MessageBox.Show(messagestring);
                            return 2;
                        }
                    }
                    return 0;
                }
                return 0;
                   
            }
            else if (dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString() == "Lathe" ||
                    dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString() == "车工序")//车床工序
            {
                if (!(MainForm.magprocesss2tate[number - 1] == (int)Processstate.None
                || MainForm.magprocesss2tate[number - 1] == (int)Processstate.Unloaded))
                {
                    int ret = 0;
                    if (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Alarm)
                    {

                        aotomode = false;
                        aotostop = true;
                        messagestring = messagestring + "车床有报警或者提示，自动加工暂停";
                        if (language == "English")
                        {
                            messagestring = messagestring + "The order is err,change to aotostop mode ";
                        }
                        return 2;
                    }
                    if (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Notstart)//上料
                    {
                        ret = CNCLoadfun(number, 1, ref messagestring);//车床上料 ;//机床是否能执行
                        if (ret == 0)
                        {
                            MainForm.magprocesss2tate[number - 1] = (int)Processstate.Loading; 
                            TopRangeDataGridView(rowindex);
                            return 1;
                        }
                        else
                        {


                            aotomode = false;
                            aotostop = true;
                            messagestring = messagestring + "订单下发失败,自动加工暂停";
                            if (language == "English")
                            {
                                messagestring = messagestring + "The order down err,change to manmode";
                            }

                            return 2;

                        }
                    }
                    if (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Runned)//下料
                    {
                        ret = CNCUnLoadfun(number, 1, ref messagestring);//车床上料 ;//机床是否能执行
                        if (ret == 0)
                        {
                            MainForm.magprocesss2tate[number - 1] = (int)Processstate.Unloading;
                            return 1;
                        }
                        else
                        {

                            aotomode = false;
                            aotostop = true;
                            messagestring = "订单下发失败,自动加工暂停";
                            if (language == "English")
                            {
                                messagestring = "The order down err,change to manmode";
                            }

                            MessageBox.Show(messagestring);
                            return 2;
                        }
                    }
                    return 0;
                }
                return 0;

            }
            else
            {
                aotomode = false;
                aotostop = true;
                messagestring = "订单下发失败,自动加工暂停";
                if (language == "English")
                {
                    messagestring = messagestring + "The order down err,change to manmode";
                }

                return 2;
            }
        }
        
        
        private int AotoRunCncfun(int number, int rowindex,ref string messagestring)
        {
            messagestring = "";
            if (dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString() == "CNC" ||
                 dataGridVieworder.Rows[rowindex].Cells[2].Value.ToString() == "铣工序")//车床工序
            {
                if (!(MainForm.magprocesss1tate[number - 1] == (int)Processstate.None
                || MainForm.magprocesss1tate[number - 1] == (int)Processstate.Unloaded))
                {
                    int ret = 0;
                    if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Alarm)
                    {

                        aotomode = false;
                        aotostop = true;
                        messagestring = messagestring + "机床有报警或者提示，自动加工暂停";
                        if (language == "English")
                        {
                            messagestring = messagestring + "The order is err,change to aotostop mode ";
                        }
                        return 2;
                    }
                    if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Notstart)//上料
                    {
                        ret = CNCLoadfun(number, 2, ref messagestring);//车床上料 ;//机床是否能执行
                        if (ret == 0)
                        {
                            MainForm.magprocesss1tate[number - 1] = (int)Processstate.Loading; 
                            TopRangeDataGridView(rowindex);
                            return 1;
                        }
                        else
                        {


                            aotomode = false;
                            aotostop = true;
                            messagestring = messagestring + "订单下发失败,自动加工暂停";
                            if (language == "English")
                            {
                                messagestring = messagestring + "The order down err,change to manmode";
                            }

                            return 2;

                        }
                    }
                  // OrderForm1.IsRerunning = false;//下料

                    if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Runned && OrderForm1.IsRerunning == false)//下料
                    {
                        //处理测量信息
                        //if()
                        if (SkipMeterflage == true)
                        {
                            SkipMeterflage = false;
                        }
                       
                        if (AotoOrderForm.zhiliangflage == true)//质量为优先,等待用户选择
                        {
                            if (OrderForm1.ReProcessChoose == true)//等待用户选择返修或者取料
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            OrderForm1.ReProcessChoose = false;
                        }
                            //如果不是质量优先，在CNCUnLoadfun函数自动关闭用户选择弹框
                       // return 0;
                        ret = CNCUnLoadfun(number, 2, ref messagestring);//车床上料 ;//机床是否能执行
                        if (ret == 0)
                        {
                            MainForm.magprocesss1tate[number - 1] = (int)Processstate.Unloading;
                            return 1;
                        }
                        else
                        {

                            aotomode = false;
                            aotostop = true;
                            messagestring =messagestring+ "订单下发失败,自动加工暂停";
                            if (language == "English")
                            {
                                messagestring = messagestring+"The order down err,change to manmode";
                            }

                            return 2;
                        }
                    }
                    return 0;
                }
                return 0;

            }
            else if (dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString() == "CNC" ||
                 dataGridVieworder.Rows[rowindex].Cells[3].Value.ToString() == "铣工序")//车床工序
            {
                if (!(MainForm.magprocesss2tate[number - 1] == (int)Processstate.None
                || MainForm.magprocesss2tate[number - 1] == (int)Processstate.Unloaded))
                {
                    int ret = 0;
                    if (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Alarm)
                    {

                        aotomode = false;
                        aotostop = true;
                        messagestring = messagestring + "机床有报警或者提示，自动加工暂停";
                        if (language == "English")
                        {
                            messagestring = messagestring + "The order is err,change to aotostop mode ";
                        }
                        return 2;
                    }
                    if (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Notstart)//上料
                    {
                        ret = CNCLoadfun(number, 2, ref messagestring);//车床上料 ;//机床是否能执行
                        if (ret == 0)
                        {
                            MainForm.magprocesss2tate[number - 1] = (int)Processstate.Loading;
                            TopRangeDataGridView(rowindex);
                            return 1;
                        }
                        else
                        {
                            aotomode = false;
                            aotostop = true;
                            messagestring = messagestring + "订单下发失败,自动加工暂停";
                            if (language == "English")
                            {
                                messagestring = messagestring + "The order down err,change to manmode";
                            }

                            return 2;

                        }
                    }
                    if (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Runned && OrderForm1.IsRerunning == false)//下料
                    {
                        //处理测量信息
                        //
                        //if (MainForm.Mag_Check[number - 1] == 1)//测量不合格
                        //{
                        //    messagestring = "测量不合格，自动加工暂停";
                        //    aotomode = false;
                        //    aotostop = true;

                        //    MessageBox.Show(messagestring);
                        //    return 2;
                        //}
                        //if (AotoOrderForm.Fvalue2over == true)
                        //{
                        //    messagestring = "测量误差大于报警临界值,自动加工暂停";
                        //    aotomode = false;
                        //    aotostop = true;

                        //    MessageBox.Show(messagestring);
                        //    return 2;
                        //}
                        //else if (AotoOrderForm.Fvalue1over == true && AotoOrderForm.Fvalue1stop == true)
                        //{
                        //    messagestring = "测量误差大于提示临界值,自动加工暂停";
                        //    aotomode = false;
                        //    aotostop = true;

                        //    MessageBox.Show(messagestring);
                        //    return 2;
                        //}
                        if (SkipMeterflage == true)
                        {
                            SkipMeterflage = false;
                        }
                       
                        if (AotoOrderForm.zhiliangflage == true)//质量为优先,等待用户选择
                        {
                            if (OrderForm1.ReProcessChoose == true)//等待用户选择返修或者取料
                            {
                                return 0;
                            }
                        }//如果不是质量优先，在CNCUnLoadfun函数自动关闭用户选择弹框
                        else
                        {
                            OrderForm1.ReProcessChoose = false;
                        }
                        ret = CNCUnLoadfun(number, 2, ref messagestring);//车床上料 ;//机床是否能执行
                        if (ret == 0)
                        {
                            MainForm.magprocesss2tate[number - 1] = (int)Processstate.Unloading;
                            return 1;
                        }
                        else
                        {

                            aotomode = false;
                            aotostop = true;
                            messagestring = messagestring + "订单下发失败,自动加工暂停";
                            if (language == "English")
                            {
                                messagestring = messagestring + "The order down err,change to manmode";
                            }

                            return 2;
                        }
                    }
                    return 0;
                }
                return 0;

            }
            else
            {
                aotomode = false;
                aotostop = true;
                messagestring = "订单下发失败,自动加工暂停";
                if (language == "English")
                {
                    messagestring = messagestring + "The order down err,change to manmode";
                }

                return 2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">仓位编号</param>
        /// <param name="messagestring">返回信息</param>
        /// <returns>返回false，没有测量提示，返回true有测量超过限制值</returns>
        private bool renewmeterresult( int number,ref string messagestring)
        {
            if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Runned && OrderForm1.IsRerunning == false)//下料
            {
                //处理测量信息
                //if()
                if (SkipMeterflage == true)
                {
                    
                    return false;
                }
                else
                {
                    if (MainForm.Mag_Check[number - 1] == 1)//测量不合格
                    {
                        messagestring = "测量不合格，自动加工暂停";
                        aotomode = false;
                        aotostop = true;

                        MessageBox.Show(messagestring);
                        return true;
                    }
                    if (AotoOrderForm.Fvalue2over == true)
                    {
                        messagestring = "测量误差大于报警临界值,自动加工暂停";
                        aotomode = false;
                        aotostop = true;
                        MessageBox.Show(messagestring);
                        return true;
                    }
                    else if (AotoOrderForm.Fvalue1over == true && AotoOrderForm.Fvalue1stop == true)
                    {
                        messagestring = "测量误差大于提示临界值,自动加工暂停";
                        aotomode = false;
                        aotostop = true;

                        MessageBox.Show(messagestring);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;;
            }
        }
        private void AotoOrderRun()
        {
            if (aotomode == false)
            {
                return;
            }
            //MainForm.cnclist[1].MagNo = 0;
            if (MainForm.linestart)
            {
                aotomode = false;

                return;
            }
            if (MainForm.PLC_SIMES_ON_line == false)
            {
                aotomode = false;
                aotostop = true;

                if (language == "English")
                {
                    MessageBox.Show("Load failure,because PLC is off line，");
                }
                else MessageBox.Show("PLC未连接，不能下达订单!");

                return;
            }
            if (!MainForm.linestop)//产线停止状态
            {

                aotomode = false;
                aotostop = true;
                if (language == "English")
                {
                    MessageBox.Show("Load failure,Please start line!");
                }
                else MessageBox.Show("订单下达失败，请先启动产线!");

                return;
            }


            int robortishome = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm];
            
            Getlathenumber();
            Getcncnumber();
            if (aotorunmagcnc == 0 && aotorunmaglathe == 0)//如果自动加工仓位铣床为0，那么自动加工停止
            {

                //MessageBox.Show("当前没有自动状态的待加工订单!");
                aotomode = false;
                aotostop = true;
                MessageBox.Show("当前没有自动状态的待加工订单!");
                return;
            }
            
            
            string message1=  "";
            if (aotorunmagcnc>0)
            {
                if (AotoOrderForm.xiaolvflage == true)//效率为优先
                {
                    OrderForm1.ReProcessChoose = false;

                }
                else
                {
                    bool ret = renewmeterresult(aotorunmagcnc, ref message1);
                    if (ret == true)
                    {
                        aotomode = false;
                        aotostop = true;
                        MessageBox.Show(message1);
                        return;
                    }
                    //else
                    //{
                    //    if (AotoOrderForm.xiaolvflage == true)//质量为优先,等待用户选择
                    //    {
                    //        OrderForm1.ReProcessChoose = false;

                    //    }
                    //}
                }
               
            }
            // bool ret = renewmeterresult(aotorunmagcnc, ref message1);
            if (ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_speed] == 0)
            {
                int iii = 0; ;
            }
          
            //机器人繁忙状态不能匹配下一步骤
            if (ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_speed] == 1||ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm] == 0 || ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_speed] == 1|| ModbusTcp.Rack_number_write_flage == true)
            {
                return;
            }

            //机器人繁忙状态不能匹配下一步骤
            //  if (ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_speed] == 1)
            //{
            //    return;
            //}
            //查询车床是否有新的任务下发
            string message=  "";
            int retcnc = -1;
            int retlathe = -1;
            if (aotorunmaglathe >0)
            {
                retlathe = AotoRunLathefun(aotorunmaglathe, indexlathe, ref message);//下发车床任务
            }
            else
            {
                retlathe = 0;
            }
            //车床没有下发新的任务，那么铣床查询是否有新的任务下发
            if (retlathe == 2)//订单下发时出错，切换暂停状态
            {
                aotomode = false;
                aotostop = true;
                MessageBox.Show(message);
                
                return; 
            }
            if (retlathe == 0)//当前没有下发新的车床任务
            {
                if (aotorunmagcnc > 0)
                {
                    retcnc = AotoRunCncfun(aotorunmagcnc, indexcnc, ref  message);
                }
                else
                {
                    retcnc=0;
                }
            }
            if (retcnc == 2)//订单下发时出错，切换暂停状态
            {
                aotomode = false;
                aotostop = true;
                MessageBox.Show(message);
                return; 
            }

        }


        /// <summary>
        /// 查找下一个车床任务仓位编号，如果查找到的仓位没有物料提示缺料并继续查找下一订单，如果没有符合条件的订单车床不继续增加任务，
        /// </summary>
        private void fideaotorunmaglathe()
        {
            aotorunmaglathe =0;
            int leavel = -1;
            int leavelold =5;
            for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
            {
                if (dataGridVieworder.Rows[ii].Cells[5].Value.ToString() == "自动"
                     || dataGridVieworder.Rows[ii].Cells[5].Value.ToString() == "Aoto")
                {
                    string numbers = dataGridVieworder.Rows[ii].Cells[1].Value.ToString();
                    int number = Convert.ToInt32(numbers);
                    int MagNo = number;
                    int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
                    int maglength = (int)ModbusTcp.MagLength;
                    int magstatei = magstart + maglength * (MagNo - 1);
                    string message = "";
                    if (number<13)
                    {
                        leavel = AotoOrderForm.Aleavel;
                    }
                    else if(number<25)
                    {
                        leavel = AotoOrderForm.Bleavel;
                    }
                    else if(number<28)
                    {
                        leavel = AotoOrderForm.Cleavel;
                    }
                    else if (number < 31)
                    {
                        leavel = AotoOrderForm.Dleavel;
                    }
                    if (dataGridVieworder.Rows[ii].Cells[2].Value.ToString() == "Lathe" ||
                    dataGridVieworder.Rows[ii].Cells[2].Value.ToString() == "车工序")//工序一为车工序
                    {
                        if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Notstart
                           || MainForm.magprocesss1tate[number - 1] == (int)Processstate.Runned)
                        {
                            if (leavel < leavelold)
                            {

                                if (ModbusTcp.DataMoubus[magstatei] != (int)ModbusTcp.Mag_state_config.Statenull)
                                {
                                    aotorunmaglathe = number;
                                    indexlathe = ii;
                                    leavelold = leavel;
                                }
                                else
                                {
                                    message = number.ToString()+"号仓位缺料";
                                    MessageBox.Show(message); 
                                }
                            }
                        }                       
                    }
                    else if (dataGridVieworder.Rows[ii].Cells[3].Value.ToString() == "Lathe" ||
                    dataGridVieworder.Rows[ii].Cells[3].Value.ToString() == "车工序")//工序二为车工序
                    {
                        if ((MainForm.magprocesss1tate[number - 1] == (int)Processstate.Unloaded 
                            || MainForm.magprocesss1tate[number - 1] == (int)Processstate.None)
                            && (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Notstart
                           || MainForm.magprocesss2tate[number - 1] == (int)Processstate.Runned))
                        {
                            if (leavel < leavelold)
                            {
                                if (ModbusTcp.DataMoubus[magstatei] != (int)ModbusTcp.Mag_state_config.Statenull)
                                {
                                    aotorunmaglathe = number;
                                    indexlathe = ii;
                                    leavelold = leavel;
                                }
                                else
                                {
                                    message = number.ToString() + "号仓位缺料";
                                    MessageBox.Show(message); 
                                }
                            }
                        }
                    }
                }
            }
           
        }
        /// <summary>
        /// 查找下一个铣床任务仓位编号
        /// </summary>
        private void fideaotorunmagcnc()
        {
           aotorunmagcnc = 0;
           int leavel = -1;
           int leavelold = 5;
           for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
           {
               if (dataGridVieworder.Rows[ii].Cells[5].Value.ToString() == "自动"
                    || dataGridVieworder.Rows[ii].Cells[5].Value.ToString() == "Aoto")
               {
                   string numbers = dataGridVieworder.Rows[ii].Cells[1].Value.ToString();
                   int number = Convert.ToInt32(numbers);
                   int MagNo = number;
                   int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
                   int maglength = (int)ModbusTcp.MagLength;
                   int magstatei = magstart + maglength * (MagNo - 1);
                   string message = "";
                   if (number < 13)
                   {
                       leavel = AotoOrderForm.Aleavel;
                   }
                   else if (number < 25)
                   {
                       leavel = AotoOrderForm.Bleavel;
                   }
                   else if (number < 28)
                   {
                       leavel = AotoOrderForm.Cleavel;
                   }
                   else if (number < 31)
                   {
                       leavel = AotoOrderForm.Dleavel;
                   }
                   if (dataGridVieworder.Rows[ii].Cells[2].Value.ToString() == "Cnc" ||
               dataGridVieworder.Rows[ii].Cells[2].Value.ToString() == "铣工序")//工序一为车工序
                   {
                       if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Notstart
                           || MainForm.magprocesss1tate[number - 1] == (int)Processstate.Runned)
                       {
                           if (MainForm.magprocesss1tate[number - 1] == (int)Processstate.Runned)
                           {
                               SkipMeterflage = true ;
                           }
                           if (leavel < leavelold)
                           {

                               if (ModbusTcp.DataMoubus[magstatei] != (int)ModbusTcp.Mag_state_config.Statenull)
                               {
                                   aotorunmagcnc = number;

                                   indexcnc = ii;
                                   leavelold = leavel;
                               }
                               else
                               {
                                   message = number.ToString() + "号仓位缺料";
                                   MessageBox.Show(message);
                               }
                           }
                       }
                   }
                   else if (dataGridVieworder.Rows[ii].Cells[3].Value.ToString() == "Cnc" ||
               dataGridVieworder.Rows[ii].Cells[3].Value.ToString() == "铣工序")  //工序二为车工序
                   {
                       if ((MainForm.magprocesss1tate[number - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss1tate[number - 1] == (int)Processstate.None)
                           && (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Notstart
                           || MainForm.magprocesss2tate[number - 1] == (int)Processstate.Runned))
                       {
                           if (MainForm.magprocesss2tate[number - 1] == (int)Processstate.Runned)
                           {
                               SkipMeterflage = true;
                           }
                           if (leavel < leavelold)
                           {
                               if (ModbusTcp.DataMoubus[magstatei] != (int)ModbusTcp.Mag_state_config.Statenull)
                               {
                                   aotorunmagcnc = number;
                                   leavelold = leavel;
                                   indexcnc = ii;
                               }
                               else
                               {
                                   message = number.ToString() + "号仓位缺料";
                                   MessageBox.Show(message);
                               }
                           }
                       }
                   }
               }
           }                    
         
        }

        private void OrderForm1_SizeChanged(object sender, EventArgs e)
        {
            aotosize.controlAutoSize(this);
        }

        private void buttonstart_Click(object sender, EventArgs e)
        {
            int ret = -1;
            string messagestring = "";
            //dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gray;
            //string MagNoStr = dataGridVieworder.Rows[e.RowIndex].Cells[1].Value.ToString();
            //int MagNo = Convert.ToInt16(MagNoStr);
            int magstart = 0;//零件类型
            int maglength = 0;
            int magstatei = 0;
            int robortishome = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm];

            if (aotomode)
            {
                return;
            }

            if (MainForm.PLC_SIMES_ON_line == false)
            {
                if (language == "English")
                {
                    MessageBox.Show("Load failure,because PLC is off line，");
                }
                else MessageBox.Show("PLC未连接，不能自动加工!");
                return;
            }
            if (!MainForm.linestop)//产线停止状态
            {
                if (language == "English")
                {
                    MessageBox.Show("Load failure,Please start line!");
                }
                else MessageBox.Show("自动加工失败，请先启动产线!");
                return;
            }


            //没有正在执行的订单
            for (int ii = 0; ii < dataGridVieworder2.Rows.Count; ii++)
            {
                if (dataGridVieworder2.Rows[ii].Cells[4].Value.ToString() == "进行中"
                    || dataGridVieworder2.Rows[ii].Cells[4].Value.ToString() == "Processing")
                {
                    int magnumber  = -1;
                    string magnumbers = dataGridVieworder2.Rows[ii].Cells[1].Value.ToString() ;
                    try
                    {
                         magnumber = Convert.ToInt32(magnumbers);
                    }
                    catch
                    {
                       MessageBox.Show("订单仓位标号读取错误!");
                       return;
                    }
                    //if ((MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Unloaded
                    //       || MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Notstart
                    //       || MainForm.magprocesss1tate[magnumber - 1] == (int)Processstate.Unloaded)
                    //    &&(MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Unloaded
                    //       || MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Notstart
                    //       || MainForm.magprocesss2tate[magnumber - 1] == (int)Processstate.Unloaded))
                    //{
                    //    ;
                    //}
                    //else MessageBox.Show("产线正在加工，请稍后开启自动加工!");
                    //return;
                }
            }
            if (MainForm.cncv2list.Count < 2)
            {
                if (language == "English")
                {
                    MessageBox.Show("The lathe or cnc is offline!");
                }
                else MessageBox.Show("车床或铣床离线!");
                return;
            }
            else if (MainForm.cncv2list[0].MagNum != 0 || MainForm.cncv2list[1].MagNum != 0)
            {
                int magnumber1 = MainForm.cncv2list[0].MagNum ;
                int magnumber2 = MainForm.cncv2list[1].MagNum;
               
                if(magnumber1!= 0 )
                {
                    int index =-1;
                    try
                    {
                         index = getmagnorowindex(magnumber1);
                    }
                    catch
                    {
                        MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");return;
                    }
                    if(index==-1)
                    {
                        MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");;return;
                    }
                    if(dataGridVieworder.Rows[index].Cells[5].Value.ToString() == "自动"
                    || dataGridVieworder.Rows[index].Cells[5].Value.ToString() == "Aoto")
                    {
                         if ((MainForm.magprocesss1tate[magnumber1 - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss1tate[magnumber1 - 1] == (int)Processstate.Notstart
                           || MainForm.magprocesss1tate[magnumber1 - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss1tate[magnumber1 - 1] == (int)Processstate.Runned
                           || MainForm.magprocesss1tate[magnumber1 - 1] == (int)Processstate.None)
                        &&(MainForm.magprocesss2tate[magnumber1 - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss2tate[magnumber1 - 1] == (int)Processstate.Notstart
                           || MainForm.magprocesss2tate[magnumber1 - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss2tate[magnumber1 - 1] == (int)Processstate.Runned
                           || MainForm.magprocesss2tate[magnumber1 - 1] == (int)Processstate.None))
                        {
                            ;
                        }
                         else 
                         {
                              MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");return;
                         }
                    }
                    else
                    {
                        MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");return;
                        ;
                    }
                }
                if (magnumber2!=0)
                {
                   int index =-1;
                    try
                    {
                        index = getmagnorowindex(magnumber2);
                    }
                    catch
                    {
                        MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");return;
                    }
                    if(index==-1)
                    {
                        MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");;return;
                    }
                    if(dataGridVieworder.Rows[index].Cells[5].Value.ToString() == "自动"
                    || dataGridVieworder.Rows[index].Cells[5].Value.ToString() == "Aoto")
                    {
                       

                        if ((MainForm.magprocesss1tate[magnumber2 - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss1tate[magnumber2 - 1] == (int)Processstate.Notstart
                           || MainForm.magprocesss1tate[magnumber2 - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss1tate[magnumber2 - 1] == (int)Processstate.Runned
                           || MainForm.magprocesss1tate[magnumber2 - 1] == (int)Processstate.None)
                        && (MainForm.magprocesss2tate[magnumber2 - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss2tate[magnumber2 - 1] == (int)Processstate.Notstart
                           || MainForm.magprocesss2tate[magnumber2 - 1] == (int)Processstate.Unloaded
                           || MainForm.magprocesss2tate[magnumber2 - 1] == (int)Processstate.Runned
                           || MainForm.magprocesss2tate[magnumber2 - 1] == (int)Processstate.None))
                        {
                            ;
                        }
                         else 
                         {
                              MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");return;
                         }
                    }
                    else
                    {
                        MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");return;
                        ;
                    }
                }
                //if (language == "English")
                //{
                //    MessageBox.Show("The lathe or cnc is offline!");
                //}
                //else MessageBox.Show("车床或铣床有工件正在加工，请稍后在开启自动加工!");
                //return;
            }
            bool hasaotoorder = false;//有未开始订单为自动状态
            for (int ii = 0; ii < dataGridVieworder2.Rows.Count; ii++)
            {
                //if (dataGridVieworder2.Rows[ii].Cells[4].Value.ToString() == "未开始"
                //   || dataGridVieworder2.Rows[ii].Cells[4].Value.ToString() == "Notstart")
                //{
                    if (dataGridVieworder.Rows[ii].Cells[5].Value.ToString() == "自动"
                    || dataGridVieworder.Rows[ii].Cells[5].Value.ToString() == "Aoto")
                    {
                        string magnos = dataGridVieworder.Rows[ii].Cells[1].Value.ToString();

                        int magno = Convert.ToInt32(magnos) - 1;
                        if (magnos == "null" || magno < 0 || magno > 30)
                        {
                            if (language == "English")
                            {
                                MessageBox.Show("Mag message error ,Please make a new Order!");
                            }
                            else MessageBox.Show("仓位信息错误，请重新下达订单!");
                            return;
                        }
                        magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
                        maglength = (int)ModbusTcp.MagLength;
                        magstatei = magstart + maglength * magno;

                        if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statenull)
                        {
                            if (language == "English")
                            {
                                MessageBox.Show("Current Mag is empty ,Please make a new Order!");
                            }
                            else MessageBox.Show("指定仓位无料，请重新下达订单!");
                            return;
                        }
                        else
                            hasaotoorder = true;
                    }
                }
            //}
            if (hasaotoorder == false)
            {
                if (language == "English")
                {
                    MessageBox.Show("None of the order is aoto state!");
                }
             //   else MessageBox.Show("没有订单是自动状态!");
                return;
            }
            if (ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_speed] == 1)
            {
                if (language == "English")
                {
                    MessageBox.Show("The robot is busy,please hold on!");
                }
                else MessageBox.Show("机器人繁忙状态，请等待!");
                return;
            }
            //批量加工条件满足

            aotomode = true;
            aotostop = false;

            aotorunmaglathe = 0;

            aotorunmagcnc = 0;
            buttonstop1.Enabled = true;
            buttonstop1.BackColor = Color.LightGreen;
            //批量加工开始后，不允许置顶，不允许下发订单
            for (int ii = 0; ii < dataGridVieworder.Rows.Count; ii++)
            {
                dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.Gray;
                dataGridVieworder.Rows[ii].Cells[7].Style.BackColor = Color.Gray;
            }
            /*搜索数据表，从第一行往下找到第一个自动状态的订单，取到料位号，根据料位号逐个下发操作，
            直到该料位加工完成后寻找下一个自动订单，在寻找下一个自动状态的订单，取料位号下发和跟踪，直到所有自动订单完成*/
            /*自动状态，加工中的订单不允许删除或者撤销*/
            /*当切换到手动状态时，当前正在执行的订单走完当前步骤后不再自动执行下一步动作*/


        }

        private void buttonstop_Click(object sender, EventArgs e)
        {
            if (aotostop)
            {
                return;
            }

            aotomode = false;
            aotostop = true;
            buttonstop1.Enabled = false;
            buttonstop1.BackColor = Color.Gray;
            //buttonstart1.Enabled = false;
            buttonstart1.BackColor = Color.LightGreen;

        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {

               buttonstart1.Enabled = true;
                buttonstart1.BackColor = Color.LightGreen;
                aotomode = false;
                aotostop = false;
                manmode = false;
            }
            if (checkBox1.CheckState == CheckState.Unchecked)
            {
                buttonstart1.Enabled = false;
                buttonstop1.Enabled = false;

                buttonstart1.BackColor = Color.Gray;
                buttonstop1.BackColor = Color.Gray;
                aotomode = false;
                aotostop = false;
                manmode = true;

            }
        }



    }
}

