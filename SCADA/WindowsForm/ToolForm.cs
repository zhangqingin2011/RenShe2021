using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Forms;
using HNC_MacDataService;
using ScadaHncData;
using HNC.API;

namespace SCADA
{
    public partial class ToolForm : Form
    {
        CNCV2 cncv2;
        AutoSizeFormClass size = new AutoSizeFormClass();

        // public string language = "";
        public DataGridViewTextBoxEditingControl CellEdit = null;
        public static int cncindex = -1;
        public string language = "";
        private string prelanguage; //记录切换语言之前的语言
        Hashtable m_Hashtable;
        private static bool renewtoolflag = true;
        public ToolForm()
        {
            //  size.controllInitializeSize(this);
            InitializeComponent();
            comboBoxcnctool.Items.Clear();
            ushort cncno = 0;
            foreach (var cncv2temp in MainForm.cncv2list)
            {
                string str = "CNC:";
                cncno++;
                str += cncno.ToString();
                comboBoxcnctool.Items.Add(str);
            }
            comboBoxcnctool.SelectedIndex = 0;
        }
        private void ToolForm_Load(object sender, EventArgs e)
        {
            //ChangeLanguage.LoadLanguage(this);//zxl 4.19
            //  language = ChangeLanguage.GetDefaultLanguage();

            //  LoadSetLanguage();
            //  MainForm.languagechangeEvent += LanguageChange;

        }
        void LanguageChange(object sender, string Language)
        {
            LoadSetLanguage();
        }
        public void LoadSetLanguage()
        {
            string lang = ChangeLanguage.GetDefaultLanguage();
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
            if (prelanguage != lang)
            {

                prelanguage = lang;
            }

            dataGridViewtool1.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column2");
            dataGridViewtool1.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column3");
            dataGridViewtool1.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column4");
            dataGridViewtool1.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column5");

            dataGridViewtool1.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column6");
            dataGridViewtool1.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column7");
            dataGridViewtool1.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column8");


        }

        //private void InitdataGridViewtool(CNC cnc)
        //{
        //    int value = 0;
        //    List<String> toolArray = MacDataService.GetInstance().GetHashAllString(cnc.HCNCShareData.sysData.dbNo, "Tool:List");
        //    //获取刀具数目通道参数128号
        //    int countno = MacDataService.GetInstance().HNC_ParamanGetI32((Int32)HNCAPI.HNCDATADEF.PARAMAN_FILE_CHAN, 0, 128, ref value, cnc.HCNCShareData.sysData.dbNo);//P参数从300开始偏移;

        //    if (countno == -1)
        //    {
        //        value = 30;
        //       // return;
        //    }
        //    if (toolArray != null)
        //    {
        //       // dataGridViewtool1.Rows.Clear();
        //        for (int i = 0; i < value;i++ )
        //        {
        //           if(dataGridViewtool1.Rows.Count-1<i)
        //           {
        //               dataGridViewtool1.Rows.Add();
        //               dataGridViewtool1.Rows[i].Cells[0].Value = i + 1;
        //           }
        //        }
        //    }
        //}
        private void InitdataGridViewtool(CNCV2 cncv2)
        {
            int value = 0;

            //获取刀具数目通道参数128号
            int countno = cncv2.ToolNum;

            if (countno == -1)
            {
                value = 30;
            }
            else value = countno;

            // dataGridViewtool1.Rows.Clear();
            for (int i = 0; i < value; i++)
            {
                if (dataGridViewtool1.Rows.Count - 1 < i)
                {
                    dataGridViewtool1.Rows.Add();
                    dataGridViewtool1.Rows[i].Cells[0].Value = i + 1;
                }
            }

        }
        private void cleardataGridViewtool()
        {
            dataGridViewtool1.Rows.Clear();

            dataGridViewtool1.Rows[0].Cells[0].Value = "0";
            dataGridViewtool1.Rows[0].Cells[1].Value = "0.00";
            dataGridViewtool1.Rows[0].Cells[2].Value = "0.00";
            dataGridViewtool1.Rows[0].Cells[3].Value = "0.00";
            dataGridViewtool1.Rows[0].Cells[4].Value = "0.00";
            dataGridViewtool1.Rows[0].Cells[5].Value = "0.00";
            dataGridViewtool1.Rows[0].Cells[6].Value = "0.00";

        }

        /// <summary>
        /// 刷新刀补表
        /// </summary>
        /// <param name="cnctmp"></param> 
        //private void Updatatoolgrid(CNC cnc)
        //{
        //    int value=0;
        //    if (dataGridViewtool1.Visible)
        //    { 
        //        int countno = MacDataService.GetInstance().HNC_ParamanGetI32((Int32)HNCAPI.HNCDATADEF.PARAMAN_FILE_CHAN, 0, 128, ref value, cnc.HCNCShareData.sysData.dbNo);//P参数从300开始偏移;

        //        if (countno == -1)
        //        {
        //            value = 30;
        //            //return;
        //        }
        //         if (value == 0)
        //            {
        //                return;
        //            }


        //            if (value != dataGridViewtool1.Rows.Count)
        //            {
        //                 InitdataGridViewtool( cnc);
        //            }

        //        for (int i = 0; i < dataGridViewtool1.Rows.Count; i++)
        //        {
        //            string toolStr="";
        //            int ret = MacDataService.GetInstance().GetHashKeyValueString(cnc.HCNCShareData.sysData.dbNo, "Tool:List", String.Format("{0:D4}", i + 1), ref toolStr);
        //            if (ret == 0)
        //            {
        //                if (cncindex == 0)//车床刀具长度乘以2
        //                {
        //                    double tempd = 0;
        //                    toolComp tool = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr);
        //                    dataGridViewtool1.Rows[i].Cells[0].Value = i + 1;//刀号
        //                    tempd = tool.GTOOL_LEN1;
        //                    dataGridViewtool1.Rows[i].Cells[2].Value = tempd.ToString("F2");//长度

        //                    //tempd = tool.GTOOL_LEN3 * 2;
        //                    tempd = tool.GTOOL_RAD1;
        //                    dataGridViewtool1.Rows[i].Cells[1].Value = tempd.ToString("F2");//半径

        //                    tempd = tool.WTOOL_LEN1;
        //                    dataGridViewtool1.Rows[i].Cells[4].Value = tempd.ToString("F2"); //长度磨损

        //                    tempd = tool.WTOOL_RAD1;
        //                    dataGridViewtool1.Rows[i].Cells[3].Value = tempd.ToString("F2");//半径磨损;
        //                }
        //                else
        //                {
        //                    toolComp tool = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr);
        //                    dataGridViewtool1.Rows[i].Cells[0].Value = i + 1;//刀号
        //                    dataGridViewtool1.Rows[i].Cells[2].Value = tool.GTOOL_LEN1.ToString("F2");//长度
        //                    dataGridViewtool1.Rows[i].Cells[1].Value = tool.GTOOL_RAD1.ToString("F2");//半径
        //                    dataGridViewtool1.Rows[i].Cells[3].Value = tool.WTOOL_RAD1.ToString("F2");//长度磨损
        //                    dataGridViewtool1.Rows[i].Cells[4].Value = tool.WTOOL_LEN1.ToString("F2");//半径磨损
        //                }
        //            }
        //            else
        //            {
        //                dataGridViewtool1.Rows[i].Cells[0].Value = i + 1;//刀号
        //                dataGridViewtool1.Rows[i].Cells[2].Value ="0.00";//长度
        //                dataGridViewtool1.Rows[i].Cells[1].Value = "0.00"; ;//半径
        //                dataGridViewtool1.Rows[i].Cells[3].Value = "0.00"; ;//长度磨损
        //                dataGridViewtool1.Rows[i].Cells[4].Value = "0.00"; ;//半径磨损
        //            }
        //        }

        //        renewtoolflag = false;
        //    }
        //}
        private void Updatatoolgrid(CNCV2 cncv2)
        {
            int value = 0;
            if (dataGridViewtool1.Visible)
            {
                int countno = cncv2.ToolNum;
                if (countno == -1)
                {
                    value = 30;
                }
                else
                {
                    value = countno;
                }
                if (value == 0)
                {
                    return;
                }
                else
                {
                    if (value != dataGridViewtool1.Rows.Count)
                    {
                        InitdataGridViewtool(cncv2);
                    }
                }
                for (int i = 0; i < dataGridViewtool1.Rows.Count; i++)
                {

                    if (cncv2.cnctype == CNCType.Lathe)//车床刀具长度乘以2
                    {
                        double tempd = 0;
                        dataGridViewtool1.Rows[i].Cells[0].Value = i + 1;//刀号

                        dataGridViewtool1.Rows[i].Cells[2].Value = cncv2.TOOLData[i].Length.ToString("F2");//长度

                        tempd = cncv2.TOOLData[i].Radius * 2;
                        dataGridViewtool1.Rows[i].Cells[1].Value = tempd.ToString("F2");//半径


                        dataGridViewtool1.Rows[i].Cells[4].Value = cncv2.TOOLData[i].LengthComp.ToString("F2"); //长度磨损


                        dataGridViewtool1.Rows[i].Cells[3].Value = cncv2.TOOLData[i].RadiusComp.ToString("F2");//半径磨损;
                    }
                    else
                    {
                        dataGridViewtool1.Rows[i].Cells[0].Value = i + 1;//刀号
                        dataGridViewtool1.Rows[i].Cells[2].Value = cncv2.TOOLData[i].Length.ToString("F2");//长度
                        dataGridViewtool1.Rows[i].Cells[1].Value = cncv2.TOOLData[i].Radius.ToString("F2");//半径
                        dataGridViewtool1.Rows[i].Cells[3].Value = cncv2.TOOLData[i].RadiusComp.ToString("F2");//长度磨损
                        dataGridViewtool1.Rows[i].Cells[4].Value = cncv2.TOOLData[i].LengthComp.ToString("F2");//半径磨损
                    }
                    //var tool1 = MainForm.Repo.GetSingle<Tools>(p => p.ToolID == i + 1);
                    renewtoolflag = false;

                }
            }
        }
        private void comboBoxcnctool_SelectedIndexChanged(object sender, EventArgs e)
        {
            // cnc = MainForm.cnclist[comboBoxcnctool.SelectedIndex];
            cncv2 = MainForm.cncv2list[comboBoxcnctool.SelectedIndex];
            cncindex = comboBoxcnctool.SelectedIndex;

            dataGridViewtool1.Rows.Clear();
            if (cncv2.IsConnected())
            {

                renewtoolflag = true;
                InitdataGridViewtool(cncv2);//初始寄存器列表  
            }

        }
        private double getnumformstring(string str)
        {
            double num;
            double temp1 = 0;
            double temp2 = 0;
            int pointindex = str.IndexOf('.');
            if (pointindex < 1)
            {
                pointindex = str.Length;
            }
            for (int i = 0; i < pointindex; i++)
            {
                char item = str.ElementAt(i);
                if (item <= '9' && item >= '0')
                {
                    temp1 = temp1 * 10 + (int)(item) - (int)('0');
                }
            }
            if (str.Length - 3 >= pointindex)
            {
                char item = str.ElementAt(pointindex + 1);
                if (item <= '9' && item >= '0')
                {
                    temp2 = 0.1 * ((int)(item) - (int)('0'));
                }
                int next = pointindex + 2;
                item = str.ElementAt(next);
                if (item <= '9' && item >= '0')
                {
                    temp2 = temp2 + 0.01 * ((int)(item) - (int)('0'));
                }
            }
            else if (str.Length - 2 == pointindex)
            {
                char item = str.ElementAt(pointindex + 1);
                if (item <= '9' && item >= '0')
                {
                    temp2 = 0.1 * ((int)(item) - (int)('0'));
                }

            }
            else if (str.Length == pointindex)
            {
                temp2 = 0;

            }

            num = temp2 + temp1;
            return num;
        }
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    int setres = 0;
        //    int setres1 = 0;
        //    if (dataGridViewtool1.Rows.Count <= 1)
        //    {
        //        if (language == "English")
        //        {
        //            MessageBox.Show("The CNC if offline or tool number is 0");
        //        }
        //        else MessageBox.Show("机床离线或者当前机床刀具数量为0");
        //        return;
        //    }
        //    for (int i = 0; i < dataGridViewtool1.Rows.Count; i++)
        //    {
        //        string temps = "";
        //        if (dataGridViewtool1.Rows[i].Cells[6].Value == null)
        //        {
        //            dataGridViewtool1.Rows[i].Cells[6].Value = "0.00";
        //        }
        //        if (dataGridViewtool1.Rows[i].Cells[6].Value.ToString() == "")
        //        {
        //            dataGridViewtool1.Rows[i].Cells[6].Value = "0.00";
        //        }
        //        if (dataGridViewtool1.Rows[i].Cells[6].Value.ToString() == "-")
        //        {
        //            dataGridViewtool1.Rows[i].Cells[6].Value = "0.00";
        //        }
        //        temps = dataGridViewtool1.Rows[i].Cells[6].Value.ToString();

        //        for (int j = 0; j < temps.Length; j++)
        //        {
        //            char item = temps.ElementAt(j);
        //            if (item == '.')
        //            {
        //                ;
        //            }
        //            else if (item == '-')
        //            {
        //                ;
        //            }
        //            else if (item > '9' || item < '0')
        //            {
        //                if (language == "English")
        //                {
        //                    MessageBox.Show("The compensation  is not correct");
        //                }
        //                else MessageBox.Show("补偿值设置不合理");
        //                return;
        //            }
        //        }

        //        string temps1 = "";
        //        if (dataGridViewtool1.Rows[i].Cells[5].Value == null)
        //        {
        //            dataGridViewtool1.Rows[i].Cells[5].Value = "0.00";
        //        }
        //        if (dataGridViewtool1.Rows[i].Cells[5].Value.ToString() == "")
        //        {
        //            dataGridViewtool1.Rows[i].Cells[5].Value = "0.00";
        //        }
        //        if (dataGridViewtool1.Rows[i].Cells[5].Value.ToString() == "-")
        //        {
        //            dataGridViewtool1.Rows[i].Cells[5].Value = "0.00";
        //        }
        //        temps1 = dataGridViewtool1.Rows[i].Cells[5].Value.ToString();

        //        for (int j = 0; j < temps1.Length; j++)
        //        {
        //            char item = temps1.ElementAt(j);
        //            if (item == '.')
        //            {
        //                ;
        //            }
        //            else if (item == '-')
        //            {
        //                ;
        //            }
        //            else if (item > '9' || item < '0')
        //            {
        //                if (language == "English")
        //                {
        //                    MessageBox.Show("The compensation  is not correct");
        //                }
        //                else MessageBox.Show("补偿值设置不合理");
        //                return;
        //            }
        //        }

        //        double longeadd = Convert.ToDouble(dataGridViewtool1.Rows[i].Cells[6].Value.ToString());
        //        double radioadd = Convert.ToDouble(dataGridViewtool1.Rows[i].Cells[5].Value.ToString());
        //        if (longeadd > 0.0 || longeadd < 0.0)
        //        {
        //            string toolStr = "";
        //            int ret = MacDataService.GetInstance().GetHashKeyValueString(cnc.HCNCShareData.sysData.dbNo, "Tool:List", String.Format("{0:D4}", i + 1), ref toolStr);
        //            if (ret == 0)
        //            {
        //                toolComp tool = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr);

        //                //if (cncindex == 0)//车床刀具长度乘以2
        //                //{
        //                //    longeadd = longeadd * 0.5;
        //                //}
        //                //getnumformstring(dataGridViewtool1.Rows[i].Cells[5].Value.ToString());
        //                // double radadd = Convert.ToDouble(dataGridViewtool1.Rows[i].Cells[6].Value.ToString());
        //                // getnumformstring(dataGridViewtool1.Rows[i].Cells[6].Value.ToString());
        //                if (longeadd > 0.0 || longeadd < 0.0)
        //                {

        //                    longeadd = tool.WTOOL_LEN1 + longeadd;
        //                    int res = MacDataService.GetInstance().HNC_ToolSetValue((int)MacDataService.ToolParaIndex.WTOOL_LEN1, i + 1, longeadd, cnc.HCNCShareData.sysData.dbNo);//长度磨损
        //                    if (res != 0 && setres == 0)
        //                        setres = -1;
        //                    //MessageBox.Show("补偿失败，请确认网络是否连接！");
        //                }
        //                renewtoolflag = true;
        //                MainForm.renewcncsql = true;
        //                MainForm.renewlathesql = true;
        //            }

        //        }
        //        if (radioadd > 0.0 || radioadd < 0.0)
        //        {
        //            string toolStr1 = "";
        //            int ret1 = MacDataService.GetInstance().GetHashKeyValueString(cnc.HCNCShareData.sysData.dbNo, "Tool:List", String.Format("{0:D4}", i + 1), ref toolStr1);
        //            if (ret1 == 0)
        //            {
        //                toolComp tool1 = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr1);

        //                if (cncindex == 0)//车床刀具长度乘以2
        //                {
        //                    radioadd = radioadd;
        //                }
        //                //getnumformstring(dataGridViewtool1.Rows[i].Cells[5].Value.ToString());
        //                // double radadd = Convert.ToDouble(dataGridViewtool1.Rows[i].Cells[6].Value.ToString());
        //                // getnumformstring(dataGridViewtool1.Rows[i].Cells[6].Value.ToString());
        //                if (radioadd > 0.0 || radioadd < 0.0)
        //                {

        //                    radioadd = tool1.WTOOL_RAD1 + radioadd;
        //                    int res1 = MacDataService.GetInstance().HNC_ToolSetValue((int)MacDataService.ToolParaIndex.WTOOL_RAD1, i + 1, radioadd, cnc.HCNCShareData.sysData.dbNo);//长度磨损
        //                    if (res1 != 0 && setres1 == 0)
        //                        setres1 = -1;
        //                    //MessageBox.Show("补偿失败，请确认网络是否连接！");
        //                }

        //                renewtoolflag = true;
        //                MainForm.renewcncsql = true;
        //                MainForm.renewlathesql = true;
        //            }
        //        }
        //        //if (radadd > 0.0 || radadd < 0.0)
        //        //{
        //        //    radadd = tool.WTOOL_RAD1 + radadd;
        //        //    int res = MacDataService.GetInstance().HNC_ToolSetValue((int)MacDataService.ToolParaIndex.GTOOL_RAD1, i + 1, radadd, cnc.HCNCShareData.sysData.dbNo);//半径磨损
        //        //    if (res != 0 && setres == 0)
        //        //        setres = -1;
        //        //}

        //        //设置刀具参数


        //        //MessageBox.Show("补偿失败，请确认网络是否连接！");

        //        if (setres == -1 || setres1 == -1)
        //        {
        //            if (language == "English")
        //            {
        //                MessageBox.Show("It's Failure to set compensation value ,please check the connection");
        //            }
        //            else MessageBox.Show("补偿失败，请确认网络是否连接！");
        //        }
        //    }

        //    for (int ii = 0; ii < dataGridViewtool1.Rows.Count; ii++)
        //    {
        //        dataGridViewtool1.Rows[ii].Cells[6].Value = "0.00";
        //        dataGridViewtool1.Rows[ii].Cells[5].Value = "0.00";
        //        //dataGridViewtool1.Rows[ii].Cells[6].Value = "0.00";
        //    }


        //}
        private void button1_Click_2(object sender, EventArgs e)
        {
            int setres = 0;
            int setres1 = 0;
            if (dataGridViewtool1.Rows.Count <= 1)
            {
                MessageBox.Show("机床离线或者当前机床刀具数量为0");
                return;
            }
            for (int i = 0; i < dataGridViewtool1.Rows.Count; i++)
            {
                string temps = "";
                if (dataGridViewtool1.Rows[i].Cells[6].Value == null)
                {
                    dataGridViewtool1.Rows[i].Cells[6].Value = "0.00";
                }
                if (dataGridViewtool1.Rows[i].Cells[6].Value.ToString() == "")
                {
                    dataGridViewtool1.Rows[i].Cells[6].Value = "0.00";
                }
                if (dataGridViewtool1.Rows[i].Cells[6].Value.ToString() == "-")
                {
                    dataGridViewtool1.Rows[i].Cells[6].Value = "0.00";
                }
                temps = dataGridViewtool1.Rows[i].Cells[6].Value.ToString();

                for (int j = 0; j < temps.Length; j++)
                {
                    char item = temps.ElementAt(j);
                    if (item == '.')
                    {
                        ;
                    }
                    else if (item == '-')
                    {
                        ;
                    }
                    else if (item > '9' || item < '0')
                    {
                        MessageBox.Show("补偿值设置不合理");
                        return;
                    }
                }

                string temps1 = "";
                if (dataGridViewtool1.Rows[i].Cells[5].Value == null)
                {
                    dataGridViewtool1.Rows[i].Cells[5].Value = "0.00";
                }
                if (dataGridViewtool1.Rows[i].Cells[5].Value.ToString() == "")
                {
                    dataGridViewtool1.Rows[i].Cells[5].Value = "0.00";
                }
                if (dataGridViewtool1.Rows[i].Cells[5].Value.ToString() == "-")
                {
                    dataGridViewtool1.Rows[i].Cells[5].Value = "0.00";
                }
                temps1 = dataGridViewtool1.Rows[i].Cells[5].Value.ToString();

                for (int j = 0; j < temps1.Length; j++)
                {
                    char item = temps1.ElementAt(j);
                    if (item == '.')
                    {
                        ;
                    }
                    else if (item == '-')
                    {
                        ;
                    }
                    else if (item > '9' || item < '0')
                    {
                        MessageBox.Show("补偿值设置不合理");
                        return;
                    }
                }

                double longeadd = Convert.ToDouble(dataGridViewtool1.Rows[i].Cells[6].Value.ToString());
                double radioadd = Convert.ToDouble(dataGridViewtool1.Rows[i].Cells[5].Value.ToString());
                if ((radioadd > 0.0 || radioadd < 0.0) && (radioadd > 0.0 || radioadd < 0.0) == false)
                {
                    var aa = "0.00";
                    cncv2.TOOLDataChange[i].RadiusCompAdd = Convert.ToDouble(aa);

                    cncv2.TOOLDataChange[i].LengthCompAdd = Convert.ToDouble(aa);
                    cncv2.TOOLDataChange[i].ToolChangeflag = false;
                    renewtoolflag = true;
                }
                if (longeadd > 0.0 || longeadd < 0.0)
                {
                    string temp = longeadd.ToString("f2");
                    cncv2.TOOLDataChange[i].LengthCompAdd = Convert.ToDouble(temp);
                    cncv2.TOOLDataChange[i].ToolChangeflag = true;
                    renewtoolflag = true;
                }

                if (radioadd > 0.0 || radioadd < 0.0)
                {
                    string toolStr1 = "";
                    var temp = radioadd.ToString("f2");
                    cncv2.TOOLDataChange[i].RadiusCompAdd = Convert.ToDouble(temp);
                    cncv2.TOOLDataChange[i].ToolChangeflag = true;
                    renewtoolflag = true;

                }

            }


            //设置刀具参数
            if (renewtoolflag)
            {
                var ret = MainForm.collectdatav2.GetCNCDataLst[0].SetTool();
                if (!ret)
                {
                    MessageBox.Show("补偿失败，请确认网络是否连接！");
                }
                else
                {
                    MainForm.renewcncsql = true;
                }
            }


            for (int ii = 0; ii < dataGridViewtool1.Rows.Count; ii++)
            {
                dataGridViewtool1.Rows[ii].Cells[6].Value = "0.00";
                dataGridViewtool1.Rows[ii].Cells[5].Value = "0.00";
            }
        }
        private void timerupdata_Tick(object sender, EventArgs e)
        {
            if (cncv2.EquipmentState != "离线")
            {
                if (language == "English")
                {
                    textBox1.Text = "Online";
                }
                else textBox1.Text = "在线";
                if (renewtoolflag)
                {

                    Updatatoolgrid(cncv2);
                }
            }
            else
            {
                if (language == "English")
                {
                    textBox1.Text = "Offline";
                }
                else textBox1.Text = "离线";
            }


        }

        //private void OrderForm_SizeChanged(object sender, EventArgs e)
        //{
        //    size.controlAutoSize(this);
        //    if (dataGridViewtool1.ColumnCount > 0)
        //    {
        //        dataGridViewtool1.Columns[0].Width = dataGridViewtool1.Width / 6 - 100;
        //        dataGridViewtool1.Columns[1].Width = dataGridViewtool1.Width / 6 - 40;
        //        dataGridViewtool1.Columns[2].Width = dataGridViewtool1.Width / 6;
        //        dataGridViewtool1.Columns[3].Width = dataGridViewtool1.Width / 6 - 100;
        //        dataGridViewtool1.Columns[4].Width = dataGridViewtool1.Width / 6;
        //        dataGridViewtool1.Columns[5].Width = dataGridViewtool1.Width / 6 + 220;
        //    }
        //}

        private void ToolForm_SizeChanged(object sender, EventArgs e)
        {
            size.controlAutoSize(this);
            //if (dataGridViewtool1.ColumnCount > 0)
            //{
            //    dataGridViewtool1.Columns[0].Width = dataGridViewtool1.Width / 6 - 100;
            //    dataGridViewtool1.Columns[1].Width = dataGridViewtool1.Width / 6 - 40;
            //    dataGridViewtool1.Columns[2].Width = dataGridViewtool1.Width / 6;
            //    dataGridViewtool1.Columns[3].Width = dataGridViewtool1.Width / 6 - 100;
            //    dataGridViewtool1.Columns[4].Width = dataGridViewtool1.Width / 6;
            //    dataGridViewtool1.Columns[5].Width = dataGridViewtool1.Width / 6 + 220;
            //}
        }

        private void dataGridViewtool1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dataGridViewtool1.CurrentCell.ColumnIndex == 5)
            {
                CellEdit = (DataGridViewTextBoxEditingControl)e.Control;
                CellEdit.SelectAll();
                CellEdit.KeyPress += Cells_KeyPress;
                CellEdit.Leave += Cells_Leave;
            }
            if (this.dataGridViewtool1.CurrentCell.ColumnIndex == 6)
            {
                CellEdit = (DataGridViewTextBoxEditingControl)e.Control;
                CellEdit.SelectAll();
                CellEdit.KeyPress += Cells_KeyPress;
                CellEdit.Leave += Cells_Leave;
            }
        }
        private void Cells_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Delete || e.KeyChar == (char)Keys.Insert)
            {

                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void Cells_Leave(object sender, EventArgs e)
        {
            language = ChangeLanguage.GetDefaultLanguage();
            string values = ((TextBox)sender).Text;
            int pointcount = 0;
            int pointcount1 = 0;
            char temp;
            if (values == "")
            {
                values = "0.00";
            }
            else if (values == "-")
            {
                values = "0.00";
            }
            //else
            //{
            for (int i = 0; i < values.Length; i++)
            {
                temp = values.ElementAt(i);
                if (temp == '.')
                {
                    pointcount++;
                }
            }
            for (int i = 1; i < values.Length; i++)
            {
                temp = values.ElementAt(i);
                if (temp == '-')
                {
                    pointcount1++;
                }
            }
            if (pointcount > 1 || pointcount1 > 0)
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number correct");
                }
                else
                    MessageBox.Show("请输入正确数字");
                ((TextBox)sender).Text = "";
            }
            else
            {
                double valued = Convert.ToDouble(values);
                ((TextBox)sender).Text = valued.ToString("F2");
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {

            renewtoolflag = true;
        }
        //}





    }
}
