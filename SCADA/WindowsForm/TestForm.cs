using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HNC_MacDataService;
using HNC.API;

namespace SCADA
{
    public partial class TestForm : Form
    {
        public string language = "";
        private static bool testbegin1 = false;//加工中心
        private static bool testbegin2 = false;//车床
        private static bool testbegin3 = false;//机器人
        public static bool testbegin4 = false; //料仓
        public static bool testbegin5 = false;//试切
        public static int[] magstate = new int[30];
        public static String BLUEPATH = "..\\picture\\bluewuliao.png";
        public static String BLACKPATH = "..\\picture\\blackwuliao.png";
        public static String REDPATH = "..\\picture\\redwuliao.png";
        public static String WHITEPATH = "..\\picture\\whitewuliao.png";
        public static String YELLOWPATH = "..\\picture\\yellowwuliao.png";
        public static String GREYPATH = "..\\picture\\greywuliao.png";
        public static String GREENPATH = "..\\picture\\greenwuliao.png";
        public DataGridViewTextBoxEditingControl CellEdit = null;
        //public string language = "";
        private string prelanguage; //记录切换语言之前的语言
        Hashtable m_Hashtable;

        public TestForm()
        {
            InitializeComponent();

            comboBoxopen1.Items.Add("开门");
            comboBoxopen1.Items.Add("关门");
            comboBoxopen1.SelectedIndex = 0;

            comboBoxopen2.Items.Add("开门");
            comboBoxopen2.Items.Add("关门");
            comboBoxopen2.SelectedIndex = 0;

            comboBoxpan1.Items.Add("夹紧");
            comboBoxpan1.Items.Add("松开");
            comboBoxpan1.SelectedIndex = 0;

            comboBoxpanb.Items.Add("夹紧");
            comboBoxpanb.Items.Add("松开");
            comboBoxpanb.SelectedIndex = 0;

            comboBoxpan2.Items.Add("夹紧");
            comboBoxpan2.Items.Add("松开");
            comboBoxpan2.SelectedIndex = 0;
            initcomboBoxaitems();//初始化料架选择下拉框
            textBoxspeed1.Text = "0.0";
            textBoxspeed2.Text = "0.0";

            textBoxj6.Text = "0.0";
            textBoxj7.Text = "0.0";

            initdataGridViewcnc2();
            initdataGridViewcnc1();                
            initdataGridViewrobort();
            initdataGridViewcut();
            initdataGridViewbiaoding();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            //ChangeLanguage.LoadLanguage(this);//zxl 4.19
            //language = ChangeLanguage.GetDefaultLanguage();

            //LoadSetLanguage();

            //MainForm.languagechangeEvent += LanguageChange;
        }
        void LanguageChange(object sender, string Language)
        {
            LoadSetLanguage();

            initcomboBoxaitems();//初始化料架选择下拉框
            initdataGridViewcnc2();
            initdataGridViewcnc1();
            initdataGridViewrobort();
        }
        public void LoadSetLanguage()
        {
            string lang = ChangeLanguage.GetDefaultLanguage();
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
            if (prelanguage != lang)
            {
                comboBoxopen1.Items.Clear();
                comboBoxopen2.Items.Clear();
                comboBoxpan1.Items.Clear();
                comboBoxpanb.Items.Clear();
                comboBoxpan2.Items.Clear();
                string comboBoxopen_value1 = ChangeLanguage.GetString(m_Hashtable, "comboBoxopen.Items01");
                string comboBoxopen_value2 = ChangeLanguage.GetString(m_Hashtable, "comboBoxopen.Items02");
                string comboBoxpan_value1 = ChangeLanguage.GetString(m_Hashtable, "comboBoxpan.Items01");
                string comboBoxpan_value2 = ChangeLanguage.GetString(m_Hashtable, "comboBoxpan.Items02");
                comboBoxopen1.Items.Add(comboBoxopen_value1);
                comboBoxopen1.Items.Add(comboBoxopen_value2);
                comboBoxpan2.Items.Add(comboBoxpan_value1);
                comboBoxpan2.Items.Add(comboBoxpan_value2);
                comboBoxpanb.Items.Add(comboBoxpan_value1);
                comboBoxpanb.Items.Add(comboBoxpan_value2); 
                comboBoxopen2.Items.Add(comboBoxopen_value1);
                comboBoxopen2.Items.Add(comboBoxopen_value2);
                comboBoxpan1.Items.Add(comboBoxpan_value1);
                comboBoxpan1.Items.Add(comboBoxpan_value2);
                comboBoxopen1.SelectedIndex = 0;
                comboBoxpan1.SelectedIndex = 0;
                comboBoxopen2.SelectedIndex = 0;
                comboBoxpan2.SelectedIndex = 0;

                prelanguage = lang;
            }

            dataGridViewcnc2.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column1");
            dataGridViewcnc2.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column2");
            dataGridViewcnc2.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column3");
            dataGridViewcnc1.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column1a");
            dataGridViewcnc1.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column2a");
            dataGridViewcnc1.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column3a");
            dataGridViewrobort.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column1c");
            dataGridViewrobort.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column2c");
            dataGridViewrobort.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column3c");
            dataGridViewcut.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column1b");
            dataGridViewcut.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column2b");
            dataGridViewcut.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column3b");
            dataGridViewcut.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column4b");
            dataGridViewcut.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column5b");
            dataGridViewcut.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "Column6b");


        }
        private void initcomboBoxaitems()
        {
            foreach (Control control in this.tableLayoutPanel19.Controls)
            {
                if(control is TableLayoutPanel)
                {
                   subcombox(control.Controls);               
                }
            }
        }
        private void subcombox(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is TableLayoutPanel)
                        {
                            subsubcombox(control.Controls);
                        }
            }
        }
        private void subsubcombox(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is ComboBox)
                {

                    language = ChangeLanguage.GetDefaultLanguage();
                    ComboBox temp = (ComboBox)control;
                    //if (language == "English")
                    //{
                    //    temp.Items.Clear();
                    //    temp.Items.Add("None");
                    //    temp.Items.Add("Waiting");
                    //    temp.Items.Add("Processing");
                    //    temp.Items.Add("Qualified");
                    //    temp.Items.Add("NotQualified");
                    //    temp.Items.Add("LatheFinish");
                    //    temp.Items.Add("CNCFinish");
                    //    temp.Items.Add("Error");

                    //    temp.SelectedIndex = 0;
                    //}
                    //else
                    //{

                        temp.Items.Clear();
                        temp.Items.Add("无料");
                        temp.Items.Add("待加工");
                        temp.Items.Add("正在加工");
                        temp.Items.Add("合格品");
                        temp.Items.Add("不合格品");
                        temp.Items.Add("车加工完成");
                        temp.Items.Add("铣加工完成");
                        temp.Items.Add("异常");

                        temp.SelectedIndex = 0;
                    //}
                    

                }
            }
        }
        private void initdataGridViewcnc2()
        {
             //if (dataGridViewcnc2.Rows.Count<1)
             //   {

                    dataGridViewcnc2.Rows.Clear();
                    dataGridViewcnc2.Rows.Add(4);

                    int hight = (dataGridViewcnc2.Height - dataGridViewcnc2.ColumnHeadersHeight) / 3;
                    for (int i = 0; i < 4;i++ )
                    {
                        string temp = "";
                        int no = i+1;
                        temp = no.ToString();
                        dataGridViewcnc2.Rows[i].Height = hight;

                        dataGridViewcnc2.Rows[i].Cells[0].Value = temp;
                        if(i==0)
                        {
                            //if (language == "English")
                            //{
                            //    dataGridViewcnc2.Rows[i].Cells[1].Value = "Door State"; 
                            //}
                            //else
                        dataGridViewcnc2.Rows[i].Cells[1].Value = "开关门状态";
                        }
                        if (i == 1)
                        {
                            //if (language == "English")
                            //{
                            //    dataGridViewcnc2.Rows[i].Cells[1].Value = "Chuck1 State";
                            //}
                            //else 
                        dataGridViewcnc2.Rows[i].Cells[1].Value = "液压卡盘状态";
                        }
                        if (i == 2)
                        {
                            //if (language == "English")
                            //{
                            //    dataGridViewcnc2.Rows[i].Cells[1].Value = "Chuck2 State";
                            //}
                            //else 
                        dataGridViewcnc2.Rows[i].Cells[1].Value = "零点卡盘状态";
                        }
                        if (i == 3)
                        {
                            //if (language == "English")
                            //{
                            //    dataGridViewcnc2.Rows[i].Cells[1].Value = "Principal Speed";
                            //}
                            //else
                        dataGridViewcnc2.Rows[i].Cells[1].Value = "主轴转速";
                        }
                        dataGridViewcnc2.Rows[i].Cells[2].Value = "";
                    }
                //}
        }
        private void initdataGridViewcnc1()
        {
            //if (dataGridViewcnc1.Rows.Count < 1)
            //{
                dataGridViewcnc1.Rows.Clear();

                dataGridViewcnc1.Rows.Add(3);

                int hight = (dataGridViewcnc1.Height - dataGridViewcnc1.ColumnHeadersHeight) / 3;

                for (int i = 0; i < 3; i++)
                {
                    string temp = "";
                    int no = i + 1;
                    temp = no.ToString();
                    dataGridViewcnc1.Rows[i].Height = hight;
                    dataGridViewcnc1.Rows[i].Cells[0].Value = temp;
                    if (i == 0)
                    {
                        //if (language == "English")
                        //{
                        //    dataGridViewcnc1.Rows[i].Cells[1].Value = "Door State";
                        //}
                        //else 
                        dataGridViewcnc1.Rows[i].Cells[1].Value = "开关门状态";
                    }
                    if (i == 1)
                    {
                        //if (language == "English")
                        //{
                        //    dataGridViewcnc1.Rows[i].Cells[1].Value = "Chuck State";
                        //}
                        //else
                        dataGridViewcnc1.Rows[i].Cells[1].Value = "卡盘状态";
                    }
                    if (i == 2)
                    {
                        //if (language == "English")
                        //{
                        //    dataGridViewcnc1.Rows[i].Cells[1].Value = "Principal Speed";
                        //}
                        //else
                        dataGridViewcnc1.Rows[i].Cells[1].Value = "主轴转速";
                    }
                    dataGridViewcnc1.Rows[i].Cells[2].Value = "";
                }
            //}
        }
        private void initdataGridViewrobort()
        {
            //if (dataGridViewrobort.Rows.Count < 2)
            //{

                dataGridViewrobort.Rows.Clear();
                dataGridViewrobort.Rows.Add(2);

                int hight = (dataGridViewrobort.Height - dataGridViewrobort.ColumnHeadersHeight) / 2;
                for (int i = 0; i < 2; i++)
                {
                    string temp = "";
                    int no = i + 1;
                    temp = no.ToString();
                    dataGridViewrobort.Rows[i].Height = hight;

                    dataGridViewrobort.Rows[i].Cells[0].Value = temp;
                    if (i == 0)
                    {
                        //if (language == "English")
                        //{
                        //    dataGridViewrobort.Rows[i].Cells[1].Value = "J6 Position";
                        //}
                        //else
                        dataGridViewrobort.Rows[i].Cells[1].Value = "J6 位置";
                    }
                    if (i == 1)
                    {
                        //if (language == "English")
                        //{
                        //    dataGridViewrobort.Rows[i].Cells[1].Value = "J7 Position";
                        //}
                        //else 
                        dataGridViewrobort.Rows[i].Cells[1].Value = "J7 位置";
                    }              
                }
            //}
        }
        private void initdataGridViewcut()
        {
            if (dataGridViewcut.Rows.Count < 6)
            {
                dataGridViewcut.Rows.Clear();
                dataGridViewcut.Rows.Add(6);

                int hight = (dataGridViewcut.Height - dataGridViewcut.ColumnHeadersHeight) / 6;
                for (int i = 0; i <6; i++)
                {
                    string temp = "";
                    int no = 50040 + i;
                    temp = no.ToString();
                    if (i == 0)
                    {
                        temp =  no.ToString();
                    }
                    if (i == 1)
                    {
                        temp =  no.ToString();
                    }
                    dataGridViewcut.Rows[i].Height = hight;
                    dataGridViewcut.Rows[i].Cells[0].Value = "#" + temp;

                    dataGridViewcut.Rows[i].Cells[1].Value = "0.00";
                    dataGridViewcut.Rows[i].Cells[2].Value = "0.00";
                    dataGridViewcut.Rows[i].Cells[3].Value = "0.00";
                    dataGridViewcut.Rows[i].Cells[4].Value = "";
                    dataGridViewcut.Rows[i].Cells[5].Value = "";
                }
            }
        }
        private void initdataGridViewbiaoding()
        {
            if (dataGridViewBiaoding.Rows.Count < 5)
            {
                dataGridViewBiaoding.Rows.Clear();
                dataGridViewBiaoding.Rows.Add(5);

                int hight = (dataGridViewBiaoding.Height - dataGridViewBiaoding.ColumnHeadersHeight) / 5;
                for (int i = 0; i < 5; i++)
                {
                    string temp = "";
                    int no = 50046 + i;
                    temp = no.ToString();
                    if (i == 0)
                    {
                        temp = no.ToString();
                    }
                    if (i == 1)
                    {
                        temp = no.ToString();
                    }
                    var str= "标定值"+(i+1).ToString();
                    dataGridViewBiaoding.Rows[i].Height = hight;
                    dataGridViewBiaoding.Rows[i].Cells[0].Value = "#" + temp;

                    dataGridViewBiaoding.Rows[i].Cells[1].Value = str;
                    dataGridViewBiaoding.Rows[i].Cells[2].Value = "0.00";
                }
            }
        }
        private void  RenewdataGridViewcnc2()
        {
            if (dataGridViewcnc2.Rows.Count < 4)
            {
                dataGridViewcnc2.Rows.Clear();
                dataGridViewcnc2.Rows.Add(4);
            }
            int hight = (dataGridViewcnc2.Height-dataGridViewcnc2.ColumnHeadersHeight) /3;

            int cnc1state = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.CNC2_Door_state];//加工中心
            bool cnc1dooropenstate = false;
            bool cnc1panloosestate = false;
            bool cnc1panbloosestate = false;
            string cnc1doorstate = "开门";
            string cnc1panstate = "松开";
            string cnc1panbstate = "松开";
            string speeds = "0.0";
            double speed = -1;
            if ((cnc1state & 0x0002) == 0x0002)//CNC_Door_Open	= 0x0002	,//	加工中心自动门打开(0未打开1打开）
            {
                cnc1dooropenstate = true;
            }
            if ((cnc1state & 0x0004) == 0x0004)//L_Chuck_state	= 0x0004	,//	车床卡盘状态(0松开1夹紧)
            {
                cnc1panloosestate = true;
            }
            if ((cnc1state & 0x0008) == 0x0008)//L_Chuck_state	= 0x0004	,//	车床卡盘状态(0松开1夹紧)
            {
                cnc1panbloosestate = true;
            }

       
                if (cnc1dooropenstate)
                {
                    cnc1doorstate = "开门";
                }
                else cnc1doorstate = "关门";

                if (cnc1panloosestate)
                {
                    cnc1panstate = "夹紧";
                }
                else cnc1panstate = "松开";

                if (cnc1panbloosestate)
                {
                    cnc1panbstate = "夹紧";
                }
                else cnc1panbstate = "松开";
       
            
            for (int i = 0; i < 4; i++)
            {
                string temp = "";
                int no = i + 1;
                temp = no.ToString();
                dataGridViewcnc2.Rows[i].Height = hight;
                dataGridViewcnc2.Rows[i].Cells[0].Value = temp;
                if (i == 0)
                {
                    
                   dataGridViewcnc2.Rows[i].Cells[1].Value = "开关门状态";
                    dataGridViewcnc2.Rows[i].Cells[2].Value = cnc1doorstate;
                    DataGridViewCell aa = dataGridViewcnc2.Rows[i].Cells[2];
                    if (comboBoxopen1.Text == cnc1doorstate)
                    {
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
                if (i == 1)
                {
                    dataGridViewcnc2.Rows[i].Cells[1].Value = "液压卡盘状态";
                    dataGridViewcnc2.Rows[i].Cells[2].Value = cnc1panstate;
                    DataGridViewCell aa = dataGridViewcnc2.Rows[i].Cells[2];
                    if (comboBoxpan1.Text == cnc1panstate)
                    {
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
                if (i ==2)
                {
                    dataGridViewcnc2.Rows[i].Cells[1].Value = "零点卡盘状态";
                    dataGridViewcnc2.Rows[i].Cells[2].Value = cnc1panbstate;
                    DataGridViewCell aa = dataGridViewcnc2.Rows[i].Cells[2];
                    if (comboBoxpanb.Text == cnc1panbstate)
                    {
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
                if (i == 3)
                {
                    dataGridViewcnc2.Rows[i].Cells[1].Value = "主轴转速";
                    if (MainForm.cncv2list == null)
                    {
                        speeds = "0.0" ;
                    }
                    else if (MainForm.cncv2list.Count<2)
                    {
                        speeds = "0.0";
                    }
                    else
                    {
                        if (MainForm.cncv2list[1]==null)
                        {
                            speeds = "0.0";
                        }
                        else  speeds = MainForm.cncv2list[1].SpindleSpeed.ToString("F3");
                        if (speeds == null)
                        {
                            speeds = "0.0" ;
                        }
                    }
                   
                    speed = Convert.ToDouble(speeds);
                    dataGridViewcnc2.Rows[i].Cells[2].Value = speed.ToString("F3");
                    DataGridViewCell aa = dataGridViewcnc2.Rows[i].Cells[2];
                    double standspeed = Convert.ToDouble(textBoxspeed1.Text);
                    if (((standspeed - speed) > -1) &&( (standspeed - speed)<1))
                    {
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
                
            }
        }
        private void RenewdataGridViewcnc1()
        {
            if (dataGridViewcnc1.Rows.Count < 3)
            {
                dataGridViewcnc1.Rows.Clear();
                dataGridViewcnc1.Rows.Add(3);
            }

            int hight = (dataGridViewcnc1.Height - dataGridViewcnc1.ColumnHeadersHeight) / 3;

            int cnc2state =ModbusTcp.DataMoubus[ (int)SCADA.ModbusTcp.DataConfigArr.CNC1_Door_state];//车床 
            bool cnc2dooropenstate = false;
            bool cnc2panloosestate = false;
            string cnc2doorstate = "";
            string cnc2panstate = "";
            string speeds = "";
            double speed = -1;
            if ((cnc2state & 0x0002) == 0x0002)//CNC_Door_Open	= 0x0002	,//	加工中心自动门打开(0未打开1关闭）
            {
                cnc2dooropenstate = true;
            }
            if ((cnc2state & 0x0004) == 0x0004)//L_Chuck_state	= 0x0004	,//	车床卡盘状态(0松开1夹紧)
            {
                cnc2panloosestate = true;
            }
             if (cnc2dooropenstate)
                {
                    cnc2doorstate = "开门";
                }
                else cnc2doorstate = "关门";

                if (cnc2panloosestate)
                {
                    cnc2panstate = "夹紧";
                }
                else cnc2panstate = "松开";
          
            for (int i = 0; i < 3; i++)
            {
                string temp = "";
                int no = i + 1;
                temp = no.ToString();
                dataGridViewcnc1.Rows[i].Cells[0].Value = temp;

                dataGridViewcnc1.Rows[i].Height = hight;
                if (i == 0)
                {
                   dataGridViewcnc1.Rows[i].Cells[1].Value = "开关门状态";

                    dataGridViewcnc1.Rows[i].Cells[2].Value = cnc2doorstate;
                    DataGridViewCell aa = dataGridViewcnc1.Rows[i].Cells[2];
                    if (comboBoxopen2.Text== cnc2doorstate)
                    {                       
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
                if (i == 1)
                {
                    dataGridViewcnc1.Rows[i].Cells[1].Value = "卡盘状态";
                    dataGridViewcnc1.Rows[i].Cells[2].Value = cnc2panstate;

                    DataGridViewCell aa = dataGridViewcnc1.Rows[i].Cells[2];
                    if (comboBoxpan2.Text == cnc2panstate)
                    {
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
                if (i == 2)
                {
                   dataGridViewcnc1.Rows[i].Cells[1].Value = "主轴转速";
                    if (MainForm.cncv2list == null)
                    {
                        speeds = "0.0";
                    }
                    else if (MainForm.cncv2list.Count<1)
                    {
                       
                        speeds = "0.0";
                    }
                    else
                    {
                        if (MainForm.cncv2list[0] == null)
                        {
                            speeds = "0.000";
                        }
                        else speeds = MainForm.cncv2list[0].SpindleSpeed.ToString("F3");
                        if (speeds == null)
                        {
                            speeds = "0.000";
                        }
                    }
                    
                    speed = Convert.ToDouble(speeds);
                    dataGridViewcnc1.Rows[i].Cells[2].Value = speed.ToString("F3");
                    DataGridViewCell aa = dataGridViewcnc1.Rows[i].Cells[2];
                    double standspeed = Convert.ToDouble(textBoxspeed2.Text);
                    if (((standspeed - speed) > -1) && ((standspeed - speed) < 1))
                    //if (textBoxspeed2.Text == speed.ToString("F1"))
                    {
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
            }
        }

        private void RenewdataGridViewrobort()
        {
            if (dataGridViewrobort.Rows.Count <2)
            {
                dataGridViewrobort.Rows.Clear();
                dataGridViewrobort.Rows.Add(2);
            }
            int hight = (dataGridViewrobort.Height - dataGridViewrobort.ColumnHeadersHeight) / 2;
          
            string J6Posions = "";
            string J7Posions = "";
            J6Posions =  MainForm.SRobortdata.SiteJ6.ToString("F2");
            J7Posions =  MainForm.SRobortdata.SiteJ7.ToString("F2");
            for (int i = 0; i < 2; i++)
            {
                string temp = "";
                int no = i + 1;
                temp = no.ToString();
                dataGridViewrobort.Rows[i].Height = hight;
                dataGridViewrobort.Rows[i].Cells[0].Value = temp;
                if (i == 0)
                {
                    dataGridViewrobort.Rows[i].Cells[1].Value = "J6 位置";

                    dataGridViewrobort.Rows[i].Cells[2].Value = J6Posions;
                    DataGridViewCell aa = dataGridViewrobort.Rows[i].Cells[2];

                    double J6 = Convert.ToDouble(J6Posions);
                    double standspeed = Convert.ToDouble(textBoxj6.Text);
                    if (((standspeed - J6) > -1) && ((standspeed - J6) < 1))
                   // if (textBoxj6.Text == J6Posions)
                    {
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
                if (i == 1)
                {
                    dataGridViewrobort.Rows[i].Cells[1].Value = "J7 位置";
                    dataGridViewrobort.Rows[i].Cells[2].Value = J7Posions;
                    DataGridViewCell aa = dataGridViewrobort.Rows[i].Cells[2];

                    double J7 = Convert.ToDouble(J7Posions);
                    double standspeed = Convert.ToDouble(textBoxj7.Text);
                    if (((standspeed - J7) > -1) && ((standspeed - J7) < 1))
                    //if (textBoxj7.Text == J7Posions)
                    {
                        aa.Style.ForeColor = Color.Green;
                    }
                    else aa.Style.ForeColor = Color.Red;
                }
            }
        }

        //private void RenewdataGridViewcut()
        //{

        //    string key = "MacroVariables:USER";
        //    string[] metervalus = new string[6];
        //    double[] metervalud = new double[6];
        //    if (dataGridViewcut.RowCount < 6)
        //    {
        //        MessageBox.Show("获取失败，请重试！");
        //        return;
        //    }

        //    int ret0 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE0, ref metervalus[0]);
        //    int ret1 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE1, ref metervalus[1]);
        //    int ret2 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE2, ref metervalus[2]);
        //    int ret3 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE3, ref metervalus[3]);
        //    int ret4 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE4, ref metervalus[4]);
        //    int ret5 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.MEASURE_VALUE5, ref metervalus[5]);
       
        //    if (ret0 == -1 || ret1 == -1 || ret2 == -1 || ret3 == -1 || ret5 == -1 || ret4 == -1 || ret5 == -1 )
        //    {
        //        MessageBox.Show("获取失败，请重试！");
        //        return;
        //    }

        //    for (int ii = 0; ii < 6; ii++)
        //    {
        //        string str = metervalus[ii];
        //        if (str.Length > 0)
        //        {
        //            int strStart = str.IndexOf("f\":");
        //            int len = str.IndexOf(",", strStart + 3) - (strStart + 3);
        //            string strTmp = str.Substring(strStart + 3, len);
        //            string temps = "";
        //            double tempd =  Convert.ToDouble(strTmp);
        //            string  refvalues =dataGridViewcut.Rows[ii].Cells[1].Value.ToString() ;
        //            string uprefs= dataGridViewcut.Rows[ii].Cells[2].Value.ToString() ;
        //            string downrefs = dataGridViewcut.Rows[ii].Cells[3].Value.ToString() ;
        //            double refvalued = Convert.ToDouble(refvalues);//参考值
        //            double uprefd = Convert.ToDouble(uprefs);
        //            double downrefd = Convert.ToDouble(downrefs);
        //            temps= tempd.ToString("F3");
        //            dataGridViewcut.Rows[ii].Cells[4].Value = temps;
        //            metervalud[ii] = Convert.ToDouble(temps);//测量实际值

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


        //            if (refvalued>=0)
        //            {
        //                if (metervalud[ii] <= (refvalued + uprefd) && metervalud[ii] >= (refvalued + downrefd))
        //                {
        //                    if (language == "English")
        //                    {
        //                        dataGridViewcut.Rows[ii].Cells[5].Value = "Qualified";
        //                    }
        //                    else dataGridViewcut.Rows[ii].Cells[5].Value = "合格 ";

        //                   // ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.REFINT1 + ii * (int)ModbusTcp.MeterLength + 8] = 0;
        //                }
        //                else
        //                {
        //                    if (language == "English")
        //                    {
        //                        dataGridViewcut.Rows[ii].Cells[5].Value = "Unqualified";
        //                    }
        //                    else dataGridViewcut.Rows[ii].Cells[5].Value = "不合格 ";

        //                    //ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.REFINT1 + ii * (int)ModbusTcp.MeterLength + 8] = 1;

        //                }
        //            }
                   
        //            else {
        //                if (metervalud[ii] >= (refvalued - uprefd) && metervalud[ii] <= (refvalued - downrefd))
        //                {
        //                    if (language == "English")
        //                    {
        //                        dataGridViewcut.Rows[ii].Cells[5].Value = "Qualified";
        //                    }
        //                    else dataGridViewcut.Rows[ii].Cells[5].Value = "合格 ";

        //                   // ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.REFINT1 + ii * (int)ModbusTcp.MeterLength + 8] = 0;
        //                }
        //                else
        //                {
        //                    if (language == "English")
        //                    {
        //                        dataGridViewcut.Rows[ii].Cells[5].Value = "Unqualified";
        //                    }
        //                    else dataGridViewcut.Rows[ii].Cells[5].Value = "不合格 ";
        //                }
        //            }      
        //        }
        //        else
        //        {
        //        }
        //    }
       
        //}
        private void RenewdataGridViewcut(bool t = true)
        {

            //   string key = "MacroVariables:USER";
            string[] metervalus = new string[6];
            double[] metervalud = new double[6];
            int zerosum = 1;
            if (dataGridViewcut.RowCount < 6)
            {
                return;
            }

            metervalud[0] = MainForm.cncv2list[1].MeterValue[0];
            metervalud[1] = MainForm.cncv2list[1].MeterValue[1];
            metervalud[2] = MainForm.cncv2list[1].MeterValue[2];
            metervalud[3] = MainForm.cncv2list[1].MeterValue[3];
            metervalud[4] = MainForm.cncv2list[1].MeterValue[4];
            metervalud[5] = MainForm.cncv2list[1].MeterValue[5];


            for (int ii = 0; ii < 6; ii++)
            {

                string refvalues = dataGridViewcut.Rows[ii].Cells[1].Value.ToString();
                string uprefs = dataGridViewcut.Rows[ii].Cells[2].Value.ToString();
                string downrefs = dataGridViewcut.Rows[ii].Cells[3].Value.ToString();
                double refvalued = Convert.ToDouble(refvalues);//参考值
                double uprefd = Convert.ToDouble(uprefs);
                double downrefd = Convert.ToDouble(downrefs);
               
                dataGridViewcut.Rows[ii].Cells[4].Value = metervalud[ii].ToString("F3");
                metervalud[ii] = metervalud[ii];//测量实际值

                string temps = metervalud[ii].ToString("F3");

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
                    zerosum = 1;
                    if (refvalue2.Length == 1)
                    {
                        zerosum = 100;
                    }
                    else if (refvalue2.Length == 2)
                    {
                        zerosum = 10;
                    }
                    //if (refvalue2.Substring(0, 1) == "0")
                    //{
                    //    if (refvalue2.Substring(1, 1) == "0")
                    //    {
                    //        zerosum = 100;
                    //    }
                    //    else
                    //    {
                    //        zerosum = 10;
                    //    }
                    //}
                    //else
                    //{
                    //    zerosum = 1;
                    //}
                    if (flage == -1)
                    {

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3] = 1;
                    }
                    else

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3] = 0;

                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 1] = Convert.ToInt32(refvalue1);
                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 1] < 0)
                    {
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 1] = (-1) * ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 1];
                    }
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_InsideMeterpos + ii * 3 + 2] = Convert.ToInt32(refvalue2) * zerosum;
                }



                if (refvalued >= 0)
                {
                    if (metervalud[ii] <= (refvalued + uprefd) && metervalud[ii] >= (refvalued + downrefd))
                    {
                        dataGridViewcut.Rows[ii].Cells[5].Value = "合格 ";

                        // ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.REFINT1 + ii * (int)ModbusTcp.MeterLength + 8] = 0;
                    }
                    else
                    {
                        dataGridViewcut.Rows[ii].Cells[5].Value = "不合格 ";

                        //ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.REFINT1 + ii * (int)ModbusTcp.MeterLength + 8] = 1;

                    }
                }

                else
                {
                    if (metervalud[ii] >= (refvalued - uprefd) && metervalud[ii] <= (refvalued - downrefd))
                    {
                        dataGridViewcut.Rows[ii].Cells[5].Value = "合格 ";

                        // ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.REFINT1 + ii * (int)ModbusTcp.MeterLength + 8] = 0;
                    }
                    else
                    {
                        dataGridViewcut.Rows[ii].Cells[5].Value = "不合格 ";

                    }
                }




            }
            testbegin5 = false;

        }
        //private void RenewdataGridViewBiaoding()
        //{

        //    string key = "MacroVariables:USER";
        //    string[] metervalus = new string[5];
        //    double[] metervalud = new double[5];
        //    if (dataGridViewBiaoding.RowCount < 5)
        //    {
        //        return;
        //    }

        //    int ret0 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.BIAODING_VALUE1, ref metervalus[0]);
        //    int ret1 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.BIAODING_VALUE2, ref metervalus[1]);
        //    int ret2 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.BIAODING_VALUE3, ref metervalus[2]);
        //    int ret3 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.BIAODING_VALUE4, ref metervalus[3]);
        //    int ret4 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeterForm.BIAODING_VALUE5, ref metervalus[4]);

        //    if (ret0 == -1 || ret1 == -1 || ret2 == -1 || ret3 == -1 || ret4 == -1 )
        //    {
        //        MessageBox.Show("获取失败，请重试！");
        //        return;
        //    }

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
        //            temps = tempd.ToString("F3");
        //            dataGridViewBiaoding.Rows[ii].Cells[2].Value = temps;

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
        private void RenewdataGridViewBiaoding(bool t = true)
        {
            double[] metervalud = new double[5];
            if (dataGridViewBiaoding.RowCount < 5)
            {
                return;
            }

            metervalud[0] = MainForm.cncv2list[1].MeterValue[6];
            metervalud[1] = MainForm.cncv2list[1].MeterValue[7];
            metervalud[2] = MainForm.cncv2list[1].MeterValue[8];
            metervalud[3] = MainForm.cncv2list[1].MeterValue[9];
            metervalud[4] = MainForm.cncv2list[1].MeterValue[10];
            // metervalud[5] = MainForm.cncv2list[0].MeterValue[5];

            int zerosum = 1;
            for (int ii = 0; ii < 5; ii++)
            {
                zerosum = 1;
                string temps = metervalud[ii].ToString("F3");
                if (temps.Length > 0)
                {
                    dataGridViewBiaoding.Rows[ii].Cells[2].Value = temps;
                    if (temps != "null")
                    {

                        int index = temps.IndexOf(".");
                        int flage = 1;
                        if (Convert.ToDouble(temps) < 0)
                        {
                            flage = -1;
                        }
                        if (index == -1)
                        {
                            index = temps.Length;
                        }
                        string refvalue1 = temps.Substring(0, index);//整数部分
                        string refvalue2 = temps.Substring(index + 1);//小数部分
                        zerosum = 1;

                        //if (refvalue2.Substring(0, 1) == "0")
                        //{
                        //    if (refvalue2.Substring(1, 1) == "0")
                        //    {
                        //        zerosum = 100;
                        //    }
                        //    else
                        //    {
                        //        zerosum = 10;
                        //    }
                        //}
                        //else
                        //{
                        //    zerosum = 1;
                        //}
                        int i = ii;
                        if (flage == -1)
                        {

                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + 3 * i] = 1;
                        }
                        else
                        {
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + 3 * i] = 0;
                        }

                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + i * 3 + 1] = Convert.ToInt32(refvalue1);
                        if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + i * 3 + 1] < 0)
                        {
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + i * 3 + 1] = (-1) * ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + i * 3 + 1];
                        }
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.p_MeterValue1 + i * 3 + 2] = Convert.ToInt32(refvalue2) * zerosum;
                    }

                }
            }

        }
        private void textBoxspeed2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Delete)
            {
                
                e.Handled = false;
            }
            else
            {
                //if (language == "English")
                //{
                //    MessageBox.Show("Please enter number");
                //}
                //else
                    MessageBox.Show("请输入数字");
                textBoxspeed2.Focus();
            }
        }

        private void textBoxspeed2_Leave(object sender, EventArgs e)
        {
            string Speeds = ((TextBox)sender).Text;
            double Speedd = Convert.ToDouble(Speeds);
            int pointcount = 0;
            char temp  ;
              if (Speeds=="")
            {
                return;
            }
              else
            {
                 for(int i=0;i<Speeds.Length;i++)
                 {
                     temp = Speeds.ElementAt(i);
                    if (temp == '.')
                     {
                             pointcount++;
                     }
                }
                 if (pointcount > 1)
                 {
                     //if (language == "English")
                     //{
                     //    MessageBox.Show("Please enter number correct");
                     //}
                     //else
                         MessageBox.Show("请输入正确数字");
                     textBoxspeed2.Focus();
                 }
                 else
                 {
                     textBoxspeed2.Text = Speedd.ToString("F1") ;
                 }
            }
           
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Delete)
            {

                e.Handled = false;
            }
            else
            {
                //if (language == "English")
                //{
                //    MessageBox.Show("Please enter number");
                //}
                //else
                    MessageBox.Show("请输入数字");
                textBoxspeed1.Focus();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string Speeds = ((TextBox)sender).Text;
            int pointcount = 0;
            char temp ;
            if (Speeds == "")
            {
                return;
            }
            else
            {
                for (int i = 0; i < Speeds.Length; i++)
                {
                    temp = Speeds.ElementAt(i);
                    if (temp == '.')
                    {
                        pointcount++;
                    }
                }
                if (pointcount > 1)
                {
                    //if (language == "English")
                    //{
                    //    MessageBox.Show("Please enter number correct");
                    //}
                    //else
                        MessageBox.Show("请输入正确数字");
                    textBoxspeed1.Focus();
                }
                else
                {

                    double Speedd = Convert.ToDouble(Speeds);
                    textBoxspeed1.Text = Speedd.ToString("F1");
                }
            }

        }

        private void cleardataGridViewcnc2()
        {
            for (int i = 0; i <=dataGridViewcnc2.RowCount - 1; i++)
            {

                dataGridViewcnc2.Rows[i].Cells[2].Value = "";
            }
        }
        private void cleardataGridViewcnc1()
        {
            for (int i = 0; i <= dataGridViewcnc1.RowCount - 1; i++)
            {

                dataGridViewcnc1.Rows[i].Cells[2].Value = "";
            }
        }
        private void cleardataGridViewrobort()
        {
            for (int i = 0; i <= dataGridViewrobort.RowCount - 1; i++)
            {

                dataGridViewrobort.Rows[i].Cells[2].Value = "";
            }
        }

        private void cleardataGridViewcut()
        {
            for (int i = 0; i <= dataGridViewcut.RowCount - 1; i++)
            {
                dataGridViewcut.Rows[i].Cells[5].Value = "";
                dataGridViewcut.Rows[i].Cells[4].Value = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            language = ChangeLanguage.GetDefaultLanguage();
            getmagindex();
            renewpicture();
            if (testbegin1)
            {
                RenewdataGridViewcnc2();
            }
            else
            {
                cleardataGridViewcnc2() ;
            }
            if (testbegin2)
            {
                RenewdataGridViewcnc1();
            }
            else
            {
                cleardataGridViewcnc1() ;
            }
            if (testbegin3)
            {
               RenewdataGridViewrobort() ;
            }
            else
            {
                cleardataGridViewrobort();
            }
            if (testbegin5)
            {
                RenewdataGridViewcut(true);
                RenewdataGridViewBiaoding(true);
                testbegin5 = false;
            }
            else
            {
                button3.BackColor = Color.DarkTurquoise;
               // cleardataGridViewcut();
            }
            if (language == "English" )
            {
                if (comboBoxopen1.Text == "开门" || comboBoxopen1.Text == "关门" || comboBoxpan1.Text == "夹紧" || comboBoxpan1.Text == "松开")
                {
                    comboBoxopen1.Items.Clear();
                    comboBoxopen1.Items.Add("Open");
                    comboBoxopen1.Items.Add("Close");

                    comboBoxpan1.Items.Clear();
                    comboBoxpan1.Items.Add("Clamping");
                    comboBoxpan1.Items.Add("Loosen"); 
                }
                if (comboBoxopen2.Text == "开门" || comboBoxopen2.Text == "关门" || comboBoxpan2.Text == "夹紧" || comboBoxpan2.Text == "松开")
                {
                    comboBoxopen2.Items.Clear();
                    comboBoxopen2.Items.Add("Open");
                    comboBoxopen2.Items.Add("Close");

                    comboBoxpan2.Items.Clear();
                    comboBoxpan2.Items.Add("Clamping");
                    comboBoxpan2.Items.Add("Loosen"); 
                }
                
            }
            if (language == "Chinese")
            {
                if (comboBoxopen1.Text == "Open" || comboBoxopen1.Text == "Close" || comboBoxpan1.Text == "Clamping" || comboBoxpan1.Text == "Loosen")
                {
                     comboBoxopen1.Items.Clear();
                    comboBoxopen1.Items.Add("开门");
                    comboBoxopen1.Items.Add("关门");
                    comboBoxpan1.Items.Clear();
                    comboBoxpan1.Text = "夹紧";
                    comboBoxpan1.Text = "松开";
                }
                if (comboBoxopen2.Text == "Open" || comboBoxopen2.Text == "Close" || comboBoxpan2.Text == "Clamping" || comboBoxpan2.Text == "Loosen")
                {
                    comboBoxopen2.Items.Clear();
                    comboBoxopen2.Items.Add("开门");
                    comboBoxopen2.Items.Add("关门");
                    comboBoxpan2.Items.Clear();
                    comboBoxpan2.Text = "夹紧";
                    comboBoxpan2.Text = "松开";
                }
                
            }

        }

        private void buttonbegin1_Click(object sender, EventArgs e)
        {
            if (!testbegin1)
            {
                testbegin1 = true;
                buttonbegin1.BackColor = Color.Gray;
                comboBoxopen1.Enabled = false;
                comboBoxpan1.Enabled = false;
                textBoxspeed1.Enabled = false;
            }
            else
            {
                testbegin1 = false;
                buttonbegin1.BackColor = Color.DarkTurquoise;
                comboBoxopen1.Enabled = true;
                comboBoxpan1.Enabled = true;
                textBoxspeed1.Enabled = true;
            }
        }
/// <summary>
///开始测试
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void buttonbegin2_Click(object sender, EventArgs e)
        {
            if (!testbegin2)
            {
                
                testbegin2 = true;
                buttonbegin2.BackColor = Color.Gray;
                comboBoxopen2.Enabled = false;
                comboBoxpan2.Enabled = false;
                textBoxspeed2.Enabled = false;
            }
            else
            {
                testbegin2 = false;
                buttonbegin2.BackColor = Color.DarkTurquoise;
                comboBoxopen2.Enabled = true;
                comboBoxpan2.Enabled = true;
                textBoxspeed2.Enabled = true;
            }
        }

        /// <summary>
        /// J6轴目标位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Delete)
            {

                e.Handled = false;
            }
            else
            {
                //if (language == "English")
                //{
                //    MessageBox.Show("Please enter number");
                //}
                //else
                    MessageBox.Show("请输入数字");
                textBoxj6.Focus();
            }
        }
        /// <summary>
        /// J6轴目标位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox3_Leave(object sender, EventArgs e)
        {
            string Positions = ((TextBox)sender).Text;
            int pointcount = 0;
            char temp;
            if (Positions == "")
            {
                return;
            }
            else
            {
                for (int i = 0; i < Positions.Length; i++)
                {
                    temp = Positions.ElementAt(i);
                    if (temp == '.')
                    {
                        pointcount++;
                    }
                }
                if (pointcount > 1)
                {
                    //if (language == "English")
                    //{
                    //    MessageBox.Show("Please enter number correct");
                    //}
                    //else
                        MessageBox.Show("请输入正确数字");
                    textBoxj6.Focus();
                }
                else
                {

                    double Positiond = Convert.ToDouble(Positions);
                    textBoxj6.Text = Positiond.ToString("F1");
                }
            }
        }
        /// <summary>
        /// J7轴目标位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Delete)
            {

                e.Handled = false;
            }
            else
            {
                //if (language == "English")
                //{
                //    MessageBox.Show("Please enter number");
                //}
                //else
                    MessageBox.Show("请输入数字");
                textBoxj7.Focus();
            }
        }
        /// <summary>
        /// J7轴目标位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox4_Leave(object sender, EventArgs e)
        {
            string Positions = ((TextBox)sender).Text;
            int pointcount = 0;
            char temp;
            if (Positions == "")
            {
                return;
            }
            else
            {
                for (int i = 0; i < Positions.Length; i++)
                {
                    temp = Positions.ElementAt(i);
                    if (temp == '.')
                    {
                        pointcount++;
                    }
                }
                if (pointcount > 1)
                {
                    //if (language == "English")
                    //{
                    //    MessageBox.Show("Please enter number correct");
                    //}
                    //else
                        MessageBox.Show("请输入正确数字");
                    textBoxj7.Focus();
                }
                else
                {
                    double Positiond = Convert.ToDouble(Positions);
                    textBoxj7.Text = Positiond.ToString("F1");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!testbegin3)
            {
                testbegin3 = true;
                button2.BackColor = Color.Gray;
                textBoxj6.Enabled = false;
                textBoxj7.Enabled = false;
            }
            else
            {
                testbegin3 = false;
                button2.BackColor = Color.DarkTurquoise;
                textBoxj6.Enabled = true;
                textBoxj7.Enabled = true;
            }
        }

     
        private void getmagindex()
        {
            foreach (Control control in this.tableLayoutPanel19.Controls)
            {
                if (control is TableLayoutPanel)
                {
                    submagindex(control.Controls);
                }
            }
        }
        private void submagindex(Control.ControlCollection controls)
        {
             foreach (Control control in controls )
            {
                if (control is TableLayoutPanel)
                {
                    subsubmagindex(control.Controls);
                }
             }
        }
        private void subsubmagindex(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is ComboBox)
                {
                    ComboBox temp = (ComboBox)control;
                    string names = temp.Name;
                    if (names=="")
                    {
                        return ;
                    }
                    string numbers = "";
                    int number = -1;
                    int index = -1;
                    int length = 0;                  
                   index =  names.IndexOf('a');
                    length = names.Length-index;
                    numbers = names.Substring(index+1, length-1);
                    number = Convert.ToInt32(numbers);
                    if (temp.SelectedIndex<=7)
                    {
                        magstate[number - 1] = temp.SelectedIndex;
                    }
                    
                }
            }
        }
        private void renewpicture()
        {
            foreach (Control control in this.tableLayoutPanel19.Controls)
            {
                if (control is TableLayoutPanel)
                {
                    subpicture(control.Controls);
                }
            }
        }
        private void subpicture(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is PictureBox)
                {
                    PictureBox temp = (PictureBox)control;
                    string names = temp.Name;
                    if (names == "")
                    {
                        return;
                    }
                    string numbers = "";
                    int number = -1;
                    int index = -1;
                    int length = 0;
                    index = names.IndexOf('a');
                    length = names.Length - index;
                    numbers = names.Substring(index + 1, length - 1);
                    number = Convert.ToInt32(numbers);
     
                    if (magstate[number - 1] == 0)
                    {
                        temp.Load(WHITEPATH);
                    }
                    else if (magstate[number - 1] == 1)
                    {
                        temp.Load(GREYPATH);
                    }
                    else if (magstate[number - 1] ==2)
                    {
                        temp.Load(BLUEPATH);
                    }
                    else if (magstate[number - 1] ==3)
                    {
                        temp.Load(GREENPATH);
                    }
                    else if (magstate[number - 1] ==4)
                    {
                        temp.Load(YELLOWPATH);
                    }
                    else if (magstate[number - 1] == 5)
                    {
                        temp.Load(BLUEPATH);
                    }
                    else if (magstate[number - 1] == 6)
                    {
                        temp.Load(BLUEPATH);
                    }
                    else if (magstate[number - 1] == 7)
                    {
                        temp.Load(REDPATH);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!testbegin4)
            {
               
                testbegin4 = true;
                button4.BackColor = Color.Gray;
            }
            else
            {
                testbegin4 = false;
                button4.BackColor = Color.DarkTurquoise;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!testbegin5)
            {
                if (MainForm.cncv2list[1].IsConnected() == false)
                {
                    //if (language == "English")
                    //{
                    //    //  ShowForm.messagestring = "Cnc is off line";
                    //    MessageBox.Show("Cnc is off line");
                    //}
                    //else
                        //ShowForm.messagestring = "加工中心离线，无法获取测量数据";
                        //ShowForm form1 = new ShowForm();
                        //  form1.Show();
                        MessageBox.Show("加工中心离线，无法获取测量数据");
                    return;
                }//20180519

                int jj = 0;
                double lowerdatad = 0.0;
                double uppervalued = 0.0;
                double refvalued = 0.0;
                string tempvalues = "";
                string temps = "";

                for (jj = 0; jj < dataGridViewcut.Rows.Count; jj++)
                {
                    for (int k = 0; k <= 3; k++)
                    {
                        if (dataGridViewcut.Rows[jj].Cells[k].Value==null)
                        {
                            dataGridViewcut.Rows[jj].Cells[k].Value="0.00";
                        }
                        string temp = dataGridViewcut.Rows[jj].Cells[k].Value.ToString();
                        int pointnum = 0;
                        for (int l = 0; l < temp.Length; l++)
                        {
                            char char1 = temp.ElementAt(l);
                            if (char1 == '.')
                            {
                                pointnum++;
                            }
                            else if (char1 == '#' && k == 0 && l == 0)
                            {
                                ;
                            }
                            else if (char1 == '-' && k != 0)
                            {
                                ;
                            }
                            else if (char1 > '9' || char1 < '0')
                            {

                                MessageBox.Show("数据不合法");
                                return;
                            }

                            if (pointnum > 1)
                            {
                                MessageBox.Show("数据不合法");
                                return;
                            }
                        }
                    }

                    tempvalues = dataGridViewcut.Rows[jj].Cells[3].Value.ToString();
                    lowerdatad = Convert.ToDouble(tempvalues);
                    tempvalues = dataGridViewcut.Rows[jj].Cells[2].Value.ToString();
                    uppervalued = Convert.ToDouble(tempvalues);
                    tempvalues = dataGridViewcut.Rows[jj].Cells[1].Value.ToString();
                    refvalued = Convert.ToDouble(tempvalues);

                    if (refvalued >= 0)
                    {
                        if ((uppervalued + uppervalued) < (lowerdatad + uppervalued))
                        {
                            temps = dataGridViewcut.Rows[jj].Cells[0].Value.ToString();

                            tempvalues = "测量上下限设置错误";
                            tempvalues = temps + tempvalues;

                            MessageBox.Show(tempvalues);
                            return;
                        }
                    }
                    else
                    {
                        if ((uppervalued-uppervalued) < (lowerdatad - uppervalued))
                        {
                            temps = dataGridViewcut.Rows[jj].Cells[0].Value.ToString();

                            tempvalues = "测量上下限设置错误";
                            tempvalues = temps + tempvalues;

                            MessageBox.Show(tempvalues);
                            return;
                        }
                    }
                   
                }
                testbegin5 = true;
                button3.BackColor = Color.Gray;
            }
            else
            {
                testbegin5 = false;
                button3.BackColor = Color.DarkTurquoise;
            }
        }


        private void dataGridViewcut_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dataGridViewcut.CurrentCell.ColumnIndex == 1 || this.dataGridViewcut.CurrentCell.ColumnIndex == 2 | this.dataGridViewcut.CurrentCell.ColumnIndex ==3)
            {
                CellEdit = (DataGridViewTextBoxEditingControl)e.Control;
                CellEdit.SelectAll();
                CellEdit.KeyPress += Cells_KeyPress;
                CellEdit.Leave += Cells_Leave;
            }
        }
        private void Cells_KeyPress(object sender, KeyPressEventArgs e)
        {
             if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter
                || e.KeyChar == (char)Keys.Delete || e.KeyChar == '-' )
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
            string values = ((TextBox)sender).Text;
            int pointcount = 0;
            char temp;
            if (values == "")
            {
                ((TextBox)sender).Text = "0.00";
                return;
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    temp = values.ElementAt(i);
                    if (temp=='-')
                    {
                        if (i != 0)
                        {
                            pointcount = 99;//负号不在最开始报错
                        }
                    }
                    if (temp == '.')
                    {
                        pointcount++;
                    }
                }
                if (pointcount > 1)
                {
                    //if (language == "English")
                    //{
                    //    MessageBox.Show("Please enter number correct");
                    //}
                    //else
                        MessageBox.Show("请输入正确数字");
                    ((TextBox)sender).Text = "0.00";
                }
                else
                {
                    double valued = Convert.ToDouble(values);
                    ((TextBox)sender).Text = valued.ToString("F2");
                }
            }
        }
    }
}
