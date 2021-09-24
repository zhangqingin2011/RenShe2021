using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HNC_MacDataService;
using System.Collections;
using HNC.API;

namespace SCADA
{

    public partial class MeterForm : Form
    {

        AutoSizeFormClass aotosize = new AutoSizeFormClass();
        public static string MEASURE_VALUE0 = "50040";  //存储测量状态值的用户宏变量
        public static string MEASURE_VALUE1 = "50041";  //存储测量状态值的用户宏变量
        public static string MEASURE_VALUE2 = "50042";  //存储测量状态值的用户宏变量
        public static string MEASURE_VALUE3 = "50043";  //存储测量状态值的用户宏变量
        public static string MEASURE_VALUE4 = "50044";  //存储测量状态值的用户宏变量
        public static string MEASURE_VALUE5 = "50045";  //存储测量状态值的用户宏变量

        public static string BIAODING_VALUE1 = "50046";  //存储测量状态值的用户宏变量
        public static string BIAODING_VALUE2 = "50047";  //存储测量状态值的用户宏变量
        public static string BIAODING_VALUE3 = "50048";  //存储测量状态值的用户宏变量
        public static string BIAODING_VALUE4 = "50049";  //存储测量状态值的用户宏变量
        public static string BIAODING_VALUE5 = "50050";  //存储测量状态值的用户宏变量
        public static String MetersetFilePath1 = "..\\data\\measure\\MeterSetFile1";
        public static String MetersetFilePath2 = "..\\data\\measure\\MeterSetFile2";
        public static String MetersetFilePath3 = "..\\data\\measure\\MeterSetFile3";
        public static String MetersetFilePath4 = "..\\data\\measure\\MeterSetFile4";
        public static String MetetrrecordFilePath = "..\\data\\measure\\MeterRecordFile";

        public static int Textmagno = 0;
        public string language = "";
        private string prelanguage; //记录切换语言之前的语言
        Hashtable m_Hashtable;
        public DataGridViewTextBoxEditingControl CellEdit = null;
        public static bool renewbiaodingfalge = false;
        private static int curMeterMode = 0;// 当前加载的测量模型A-1，B-2，C-3，D-4
        public MeterForm()
        {
            //aotosize.controllInitializeSize(this);
            InitializeComponent();
            initdataGridView2(MetersetFilePath1);
            initdataGridView3(MetetrrecordFilePath);
            if (OrderForm1.ReProcessChoose)
            {
                buttonreprocess.Enabled = true;
                buttontorack.Enabled = true;
            }

        }
        private void MeterForm_Load(object sender, EventArgs e)
        {
            ChangeLanguage.LoadLanguage(this);//zxl 4.19
            language = ChangeLanguage.GetDefaultLanguage();

            LoadSetLanguage();
            MainForm.languagechangeEvent += LanguageChange;

        }
        void LanguageChange(object sender, string Language)
        {
            // LoadSetLanguage();
        }
        public void LoadSetLanguage()
        {
            string lang = ChangeLanguage.GetDefaultLanguage();
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
            if (prelanguage != lang)
            {

                prelanguage = lang;
            }


            dataGridView2.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columna1");
            dataGridView2.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columna2");
            dataGridView2.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columna3");
            dataGridView2.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columna4");
            dataGridView2.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columna5");
            dataGridView2.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columna6");
            dataGridView2.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columna7");

            //dataGridView3.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb1");
            //dataGridView3.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb2");
            //dataGridView3.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb3");
            //dataGridView3.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb4");
            //dataGridView3.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb5");
            //dataGridView3.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb6");
            //dataGridView3.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb7");
            //dataGridView3.Columns[7].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb8");
            //dataGridView3.Columns[8].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb9");
            //dataGridView3.Columns[9].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb10");
            //dataGridView3.Columns[10].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb11");
            //dataGridView3.Columns[11].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb12");
            //dataGridView3.Columns[12].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb13");
            //dataGridView3.Columns[13].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb14");
            //dataGridView3.Columns[14].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb15");
            //dataGridView3.Columns[15].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Columnb16");         
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
            int index2 = 0;
            if (index1 < 0)
            {
                return "null";
            }
            index1 = index1 + indexstring.Length; ;
            index2 = index1;
            int length = 0;
            string str = line.Substring(index2, 1);
            //while (line.ElementAt(index2) != ',' && line.ElementAt(index2) != ';')
            while (str != ",")
            {
                index2++;
                length++;
                str = line.Substring(index2, 1);
            }
            return temp = line.Substring(index1, length);
        }



        private bool initdataGridView2(string path)//文件
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
                if (dataGridView2.Rows.Count < 1)
                {

                    dataGridView2.Rows.Add(6);
                    for (int i = 0; i < 6; i++)
                    {
                        string temp = "";
                        int no = 50040 + i;
                        if (i == 0)
                        {
                            temp = "#" + no.ToString();
                        }
                        else if (i == 1)
                        {
                            temp = "#" + no.ToString();
                        }
                        else temp = "#" + no.ToString();
                        dataGridView2.Rows[i].Cells[0].Value = temp;
                        dataGridView2.Rows[i].Cells[1].Value = "0.000";
                        dataGridView2.Rows[i].Cells[2].Value = "0.000";
                        dataGridView2.Rows[i].Cells[3].Value = "0.000";
                        dataGridView2.Rows[i].Cells[4].Value = "0.000";
                        dataGridView2.Rows[i].Cells[5].Value = "0.000";
                        dataGridView2.Rows[i].Cells[6].Value = "No";
                    }
                }
                while (line != null)
                {
                    item = getvalueformstring(line, "item=");
                    refvalue = getvalueformstring(line, "ref=");
                    uppervalue = getvalueformstring(line, "upper=");
                    lowervalue = getvalueformstring(line, "lower=");





                    if (item != "null")
                    {
                        dataGridView2.Rows[ii].Cells[0].Value = item;
                    }
                    if (refvalue != "null")
                    {

                        dataGridView2.Rows[ii].Cells[1].Value = refvalue;

                    }
                    if (uppervalue != "null")
                    {

                        dataGridView2.Rows[ii].Cells[2].Value = uppervalue;

                    }
                    if (lowervalue != "null")
                    {

                        dataGridView2.Rows[ii].Cells[3].Value = lowervalue;
                    }

                    for (int k = 0; k <= 3; k++)
                    {
                        string temp = dataGridView2.Rows[ii].Cells[k].Value.ToString();
                        for (int l = 0; l < temp.Length; l++)
                        {
                            char char1 = temp.ElementAt(l);
                            if (char1 == '.')
                            {
                                ;
                            }
                            else if (char1 == '#' && k == 0 && l == 0)
                            {
                                ;
                            }
                            else if (char1 == '-' && k != 0 && l == 0)
                            {
                                ;
                            }
                            else if (char1 > '9' || char1 < '0')
                            {
                                if (language == "English")
                                {
                                    MessageBox.Show("Please enter number correct");
                                }
                                else MessageBox.Show("数据不合法");
                                return false;
                            }
                        }
                    }

                    line = sr.ReadLine();
                    ii++;
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
        private bool initdataGridView3(string path)// 文件
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);//读取文件
                string timenows = "";
                // string newlog = "";

                string items = "";
                string types = "";
                string magnos = "";
                string ress = "";
                string ref1s = "";
                string res1s = "";
                string ref2s = "";
                string res2s = "";
                string ref3s = "";
                string res3s = "";
                string ref4s = "";
                string res4s = "";
                string ref5s = "";
                string res5s = "";
                string ref6s = "";
                string res6s = "";
                int ii = 0;
                int itemi = 0;
                string line = sr.ReadLine();
                dataGridView3.Rows.Clear();
                while (line != null)
                {
                    itemi = ii + 1;
                    items = itemi.ToString();

                    types = getvalueformstring(line, "type=");
                    magnos = getvalueformstring(line, "magno=");
                    ress = getvalueformstring(line, "res=");
                    timenows = getvalueformstring(line, "timenow=");
                    ref1s = getvalueformstring(line, "ref1=");
                    res1s = getvalueformstring(line, "res1=");
                    ref2s = getvalueformstring(line, "ref2=");
                    res2s = getvalueformstring(line, "res2=");
                    ref3s = getvalueformstring(line, "ref3=");
                    res3s = getvalueformstring(line, "res3=");
                    ref4s = getvalueformstring(line, "ref4=");
                    res4s = getvalueformstring(line, "res4=");
                    ref5s = getvalueformstring(line, "ref5=");
                    res5s = getvalueformstring(line, "res5=");
                    ref6s = getvalueformstring(line, "ref6=");
                    res6s = getvalueformstring(line, "res6=");

                    dataGridView3.Rows.Add();
                    dataGridView3.Rows[ii].Cells[0].Value = items;

                    if (types != "null")
                    {

                        dataGridView3.Rows[ii].Cells[1].Value = types;
                    }
                    if (magnos != "null")
                    {

                        dataGridView3.Rows[ii].Cells[2].Value = magnos;
                    }
                    if (ress != "null")
                    {

                        dataGridView3.Rows[ii].Cells[3].Value = ress;
                    }
                    if (timenows != "null")
                    {

                        dataGridView3.Rows[ii].Cells[4].Value = timenows;
                    }
                    if (ref1s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[5].Value = ref1s;
                    }
                    if (res1s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[6].Value = res1s;
                    }
                    if (ref2s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[7].Value = ref2s;
                    }
                    if (res2s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[8].Value = res2s;
                    }
                    if (ref3s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[9].Value = ref3s;
                    }
                    if (res3s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[10].Value = res3s;
                    }
                    if (ref4s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[11].Value = ref4s;
                    }
                    if (res4s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[12].Value = res4s;
                    }
                    if (ref5s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[13].Value = ref5s;
                    }
                    if (res5s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[14].Value = res5s;
                    }
                    if (ref6s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[15].Value = ref6s;
                    }
                    if (res6s != "null")
                    {

                        dataGridView3.Rows[ii].Cells[16].Value = res6s;
                    }

                    ii++;
                    line = sr.ReadLine();

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
        private void UpdatadataGridView3(string path)
        {
            if (OrderForm1.renewMeterRecordGridview)
            {
                initdataGridView3(path);
                OrderForm1.renewMeterRecordGridview = false;
            }

        }
        private void MeterForm_SizeChanged(object sender, EventArgs e)
        {
            aotosize.controlAutoSize(this);
        }





        //public void UpdateMeasurevalue()  //测量合格、不合格
        //{
        //    if (!OrderForm1.renewMeterDGV2)
        //    {
        //        return;
        //    }
        //    if (dataGridView2.RowCount < 6)
        //    {
        //        return;
        //    }
        //    OrderForm1.renewMeterDGV2 = false;


        //    if (MainForm.cnclist[1].isConnected() == false)
        //    {
        //        return;
        //    }

        //    int maglength = (int)ModbusTcp.MagLength;
        //    int i = MainForm.cnclist[1].MagNo;

        //    double[] valuearry = new double[6];
        //    double[,] refvalue = new double[6, 3];


        //    for (int ii = 0; ii < 6; ii++)
        //    {
        //        string str = OrderForm1.valuestrarry[ii];
        //        if (str == null)
        //        {
        //            return;
        //        }
        //        if (str.Length > 0)
        //        {
        //            int strStart = str.IndexOf("f\":");
        //            int len = str.IndexOf(",", strStart + 3) - (strStart + 3);
        //            string strTmp = str.Substring(strStart + 3, len);//获取到测量值
        //            string strref = dataGridView2.Rows[ii].Cells[1].Value.ToString();
        //            double dact = Convert.ToDouble(strTmp);
        //            double dref = Convert.ToDouble(strref);
        //            double dif = dact - dref;
        //            string sdif = dif.ToString("F2");
        //            string strTempshort = dact.ToString("F2");

        //            dataGridView2.Rows[ii].Cells[4].Value = strTempshort;


        //            dataGridView2.Rows[ii].Cells[5].Value = sdif;
        //            if (OrderForm1.valueb[ii] == false)
        //            {
        //                dataGridView2.Rows[ii].Cells[6].Value = "No";
        //            }
        //            else
        //            {
        //                dataGridView2.Rows[ii].Cells[6].Value = "Yes";
        //            }
        //        }

        //    }

        //}

        public void UpdateMeasurevalue(CNCV2 cnctemp)  //测量合格、不合格
        {
            if (!OrderForm1.renewMeterDGV2)
            {
                return;
            }
            if (dataGridView2.RowCount < 6)
            {
                return;
            }
            OrderForm1.renewMeterDGV2 = false;


            if (cnctemp.EquipmentState == "离线")
            {
                return;
            }

            int maglength = (int)ModbusTcp.MagLength;
            int i = cnctemp.MagNum;

            double[] valuearry = new double[6];
            double[,] refvalue = new double[6, 3];


            for (int ii = 0; ii < 6; ii++)
            {
                string str = OrderForm1.valuestrarry[ii];
                if (str == null)
                {
                    return;
                }
                if (str.Length > 0)
                {
                    int strStart = str.IndexOf("f\":");
                    int len = str.IndexOf(",", strStart + 3) - (strStart + 3);
                    string strTmp = str.Substring(strStart + 3, len);//获取到测量值
                    string strref = dataGridView2.Rows[ii].Cells[1].Value.ToString();
                    double dact = Convert.ToDouble(strTmp);
                    double dref = Convert.ToDouble(strref);
                    double dif = dact - dref;
                    string sdif = dif.ToString("F2");
                    string strTempshort = dact.ToString("F2");

                    dataGridView2.Rows[ii].Cells[4].Value = strTempshort;


                    dataGridView2.Rows[ii].Cells[5].Value = sdif;
                    if (OrderForm1.valueb[ii] == false)
                    {
                        dataGridView2.Rows[ii].Cells[6].Value = "No";
                    }
                    else
                    {
                        dataGridView2.Rows[ii].Cells[6].Value = "Yes";
                    }
                }

            }

        }
        private bool freshdataGridView2(string path)//文件
        {
            bool res = initdataGridView2(path);
            return res;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Textmagno < 13)
            {
                textBox3.Text = "A";
                if (MeterSetForm.refreshmeterdata != 0)
                {
                    initdataGridView2(MetersetFilePath1);
                    MeterSetForm.refreshmeterdata = 0;
                }
            }
            else if (Textmagno < 25)
            {
                textBox3.Text = "B";
                if (MeterSetForm.refreshmeterdata != 1)
                {
                    initdataGridView2(MetersetFilePath2);
                    MeterSetForm.refreshmeterdata = 1;
                }
            }
            else if (Textmagno < 28)
            {
                textBox3.Text = "C";
                if (MeterSetForm.refreshmeterdata != 2)
                {
                    initdataGridView2(MetersetFilePath3);
                    MeterSetForm.refreshmeterdata = 2;
                }
            }
            else if (Textmagno < 31)
            {
                textBox3.Text = "D";
                if (MeterSetForm.refreshmeterdata != 3)
                {
                    initdataGridView2(MetersetFilePath4);
                    MeterSetForm.refreshmeterdata = 3;
                }
            }
            foreach (var cnctemp in MainForm.cncv2list)
            {
                if (cnctemp.cnctype == CNCType.CNC)
                {
                    UpdateMeasurevalue(cnctemp);
                    UpdatadataGridView3(MetetrrecordFilePath);
                }
            }


            if (OrderForm1.ReProcessChoose)
            {
                buttonreprocess.Enabled = true;
                buttontorack.Enabled = true;
            }
            else
            {
                buttonreprocess.Enabled = false;
                buttontorack.Enabled = false;
            }
            if (MainForm.cncv2list[1].EquipmentState!="离线")
            {
                if (language == "English")
                {
                    textBox2.Text = "OnLine";
                }
                else textBox2.Text = "在线";
            }
            else
            {
                if (language == "English")
                {
                    textBox2.Text = "OffLine";
                }
                else textBox2.Text = "离线";
            }
            textBox1.Text = Textmagno.ToString();

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (MainForm.PLC_SIMES_ON_line == false)//plc离线
            {
                if (language == "English")
                {
                    MessageBox.Show("PLC is off-line,can't reprocess!");
                }
                else MessageBox.Show("PLC离线，不允许返修!");
                return;
            }
            if (MainForm.linereseting)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is resetting,can't reprocess!");
                }
                else MessageBox.Show("产线复位进行中，不允许返修!");
                return;
            }
            if (MainForm.linestarting)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is starting,can't reprocess!");
                }
                else MessageBox.Show("产线启动进行中，不允许返修!");
                return;
            }
            if (MainForm.linestoping)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is stopping,can't reprocess!");
                }
                else MessageBox.Show("产线停止进行中，不允许返修!");
                return;
            }
            if (!MainForm.cncv2list[1].IsConnected())//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The CNC is off line,can't reprocess!");
                }
                else MessageBox.Show("铣床离线，不允许返修!");
                return;
            }
            if (!RackForm.Inventoryflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rackis Inventorying!");
                }
                else MessageBox.Show("当前正在盘点，不允许返修!");
            }
            if (!RackForm.rfidreadflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is reading ,can't reprocess!");
                }
                else MessageBox.Show("RFID信息读取中，不允许返修!");
                return;
            }
            if (!RackForm.rfidwriteflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is writting,can't reprosessing!");
                }
                else MessageBox.Show("RFID信息写入中，不允许返修!");
                return;
            }
            if (MainForm.linestart)
            {
                if (language == "English")
                {
                    MessageBox.Show("The line is stop!");
                }
                else MessageBox.Show("产线未开启!");
                return;// 产线没启动
            }
            OrderForm1.ReProcessChoose = false;
            ModbusTcp.MES_PLC_comfim_write_flage = true;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComReProcess;
            OrderForm1.rerunningflage = true;
            buttonreprocess.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OrderForm1.rerunningflage = false;
            OrderForm1.ReProcessChoose = false;
            buttontorack.Enabled = false;
        }

        /// <summary>
        /// 侧头标定值上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void buttonbiaoding_Click(object sender, EventArgs e)
        //{
        //    string key = "MacroVariables:USER";
        //    string[] metervalus = new string[5];


        //    int ret0 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, BIAODING_VALUE1, ref metervalus[0]);
        //    int ret1 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, BIAODING_VALUE2, ref metervalus[1]);
        //    int ret2 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, BIAODING_VALUE3, ref metervalus[2]);
        //    int ret3 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, BIAODING_VALUE4, ref metervalus[3]);
        //    int ret4 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, BIAODING_VALUE5, ref metervalus[4]);

        //    if (ret0 == -1 || ret1 == -1 || ret2 == -1 || ret3 == -1 || ret4 == -1)
        //    {
        //        MessageBox.Show("获取标定值失败，请重新获取！");
        //        return;
        //    }

        //    renewbiaodingfalge = true;

        //    for (int ii = 0; ii < 5; ii++)
        //    {
        //        string str = metervalus[ii];
        //        if (str.Length > 0)
        //        {
        //            int strStart = str.IndexOf("f\":");
        //            int len = str.IndexOf(",", strStart + 3) - (strStart + 3);

        //            string strTmp = str.Substring(strStart + 3, len);
        //            string temps = "";
        //            double tempd = Convert.ToDouble(strTmp);
        //            // tempd = 12.22;
        //            temps = tempd.ToString("F3");

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

        //                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3] = 1;
        //                }
        //                else

        //                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3] = 0;

        //                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 1] = Convert.ToInt32(refvalue1);
        //                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 1] < 0)
        //                {
        //                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 1] = (-1) * ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 1];
        //                }
        //                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 2] = Convert.ToInt32(refvalue2);
        //            }

        //        }
        //    }
        //}
        /// <summary>
        /// 侧头标定值上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonbiaoding_Clickv2(object sender, EventArgs e)
        {
            // string key = "MacroVariables:USER";
            string[] metervalus = new string[5];

            //50046开始-50050
            metervalus[0] = MainForm.cncv2list[0].MeterValue[6].ToString("F4");
            metervalus[1] = MainForm.cncv2list[0].MeterValue[7].ToString("F4");
            metervalus[2] = MainForm.cncv2list[0].MeterValue[8].ToString("F4");
            metervalus[3] = MainForm.cncv2list[0].MeterValue[9].ToString("F4");
            metervalus[4] = MainForm.cncv2list[0].MeterValue[10].ToString("F4");
            renewbiaodingfalge = true;

            for (int ii = 0; ii < 5; ii++)
            {
                string str = metervalus[ii];
                if (str.Length > 0)
                {
                    string temps = "";
                    temps = str;

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

                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3] = 1;
                        }
                        else

                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3] = 0;

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 1] = Convert.ToInt32(refvalue1);
                        if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 1] < 0)
                        {
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 1] = (-1) * ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 1];
                        }
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + ii * 3 + 2] = Convert.ToInt32(refvalue2);
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonbiaoding_Clickv2(sender, e);
        }


    }
}
