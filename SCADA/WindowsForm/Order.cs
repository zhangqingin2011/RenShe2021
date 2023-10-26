﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Collector;
using HNC_MacDataService;
using HNCAPI;
using System.IO;
using System.Collections;

namespace SCADA
{
    
    public partial class OrderForm : Form
    {
        //订单的保存格式
        //序号 料仓编号  工序1 工序2  工序3  工序4  订单下发 工序1状态  工序2状态  工序3状态 工序4状态
       //item = 0,magno = 5,fun1 = 1,fun2=2,fun3=0，fun4=0，down = 0，state1= 1，state2=1，state3=0，state4=0；
       //序号
       //工序 0、无 1、车床  2、铣床
       //订单下发 0、未下发，1、已下发
       //工序状态 0、无此工序，1、未开始，2、进行中，3、完成，4、返修中
       public  string language = "";
       static public bool[] valueb = new bool[6];
       static public string[] valuestrarry = new string[6];
       static public double[,] refvalue = new double[6, 3];
       static public double[] resvalue = new double[6];
        private static int nextstep1 = -2;//1车，2铣，0完成，-1错误
        private static int nextstep2 = -2;//1车，2铣，0完成，-1错误
        private static int curfunnext1 = -1;      
        private static int curfunnext2 = -1;
        private static int magnumber = -1;
        private static int maginderx = -1;
        public static bool bIsOK = false;  //是否合格 
        public static bool measureenable = false;//测量使能
        public static bool renewMeterRecordGridview = false;//测量使能
        Hashtable m_Hashtable;
       public static String OrderdataFilePath = "..\\data\\Order\\OrderdataFile";
       public static String OrderstateFilePath = "..\\data\\Order\\OrderstateFile";

        public static  String GcodeFilePath = "C:\\Users\\Public\\Documents\\加工程序目录";

       public static String MetersetFilePath = "..\\data\\measure\\MeterSetFile";
       public static String MetetrrecordFilePath = "..\\data\\measure\\MeterRecordFile";
       public int[] gongxuindex = new int[30];

       public static bool remeterdgv3 = false;
       public static bool firstaddrecord = false;

       public static bool renewMeterDGV2 = false;
       public static bool ReProcessChoose = false;
       public static Int32 ReProcessChooseitem = 0;
       public ShowForm form1 = new ShowForm();
   //    public static Int32 ReProcessChooseitem = 0;

       private string prelanguage; //记录切换语言之前的语言
      

       AutoSizeFormClass aotosize = new AutoSizeFormClass();

       public OrderForm()
        {
            InitializeComponent();
           
            InitOrderdata(OrderdataFilePath);
            InitOrderstate(OrderstateFilePath);
            form1.Visible = false;

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
               comboBoxFun3.Items.Clear();
               comboBoxFun3.Items.Add(Default_linetype_value1);
               comboBoxFun3.Items.Add(Default_linetype_value2);
               comboBoxFun3.Items.Add(Default_linetype_value3);
               comboBoxFun3.SelectedIndex = 0;
               comboBoxFun4.Items.Clear();
               comboBoxFun4.Items.Add(Default_linetype_value1);
               comboBoxFun4.Items.Add(Default_linetype_value2);
               comboBoxFun4.Items.Add(Default_linetype_value3);
               comboBoxFun4.SelectedIndex = 0;
               prelanguage = lang;
           }
           if (language == "English")
           {
               this.Column8.Items.Clear();
               this.Column8.Items.AddRange(new object[] {
                                "None",
                                "Processes1",
                                "Processes2",
                                "Processes3",
                               "Processes4"});

           }
           if (language == "Chinese")
           {
               this.Column8.Items.Clear();
               this.Column8.Items.AddRange(new object[] {
                                        "无",
                                        "工序一",
                                        "工序二",
                                        "工序三",
                                        "工序四"});
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
           dataGridVieworder2.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "dataGridViewtextboxColumn7");
           dataGridVieworder2.Columns[7].HeaderText = ChangeLanguage.GetString(m_Hashtable, "dataGridViewtextboxColumn8");


           
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
            if (MagNoStr=="")
            {
               return ;
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
                    //for (int jj = 0; jj < dataGridVieworder.RowCount - 1; jj++)
                    //{
                    //    if (dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == MagNoStr)
                    //    {
                    //        if (language == "English")
                    //        {
                    //            MessageBox.Show("The Mag have been chosed already,please shoose another");
                    //        }
                    //        else
                    //            MessageBox.Show("当前仓位已经绑定工艺，请选择其他料仓");
                    //        textBox1magno.Focus();
                    //        return;
                    //    }
                    //}
                    // textBox1magno.Focus();

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

            ChangeLanguage.LoadLanguage(this);//zxl 4.19
            language = ChangeLanguage.GetDefaultLanguage();
            comboBoxFun1.SelectedIndex = 0; 
            comboBoxFun2.SelectedIndex = 0;
            comboBoxFun3.SelectedIndex = 0;
            comboBoxFun4.SelectedIndex = 0;


            LoadSetLanguage();
            MainForm.languagechangeEvent += LanguageChange; 
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
     //       int MagNo = Convert.ToInt16(MagNoStr);
            //int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Fun_cur;//料位工艺类型
            //int maglength = (int)ModbusTcp.MagLength;
            //int magsypei = magstart + maglength * MagNo - 1;

            string fun1string =comboBoxFun1.Text  ;
           // int fun1no = Convert.ToInt16(fun1string);
            string fun2string = comboBoxFun2.Text;
           // int fun2no = Convert.ToInt16(fun2string);
            string fun3string = comboBoxFun3.Text;
           // int fun3no = Convert.ToInt16(fun3string);
            string fun4string = comboBoxFun4.Text;
          //  int fun4no = Convert.ToInt16(fun4string);

            int maglength = (int)ModbusTcp.MagLength;

            if (fun1string=="无"
                && fun2string == "无"
                && fun3string == "无"
                && fun4string == "无")
            {             
                MessageBox.Show("工序全部为空，不能生成订单");
                return;
            }

            if (fun1string == "None"
                && fun2string == "None"
                && fun3string == "None"
                && fun4string == "None")
            {
                MessageBox.Show("The processes is null");
                return; 
            }
            //for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)
            //   // for (int jj = 0; jj < dataGridVieworder.RowCount - 1; jj++)
            //{
            //    if (dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == MagNoStr)
            //    {
            //        if (language == "English")
            //        {
            //            MessageBox.Show("The Mag have been chosed already,please shoose another");
            //        }
            //        else
            //            MessageBox.Show("当前仓位已经绑定工艺，请选择其他料仓");
            //        return;
            //    }
            //}
            //插入订单
          // bool result = InsertOrdersave(MagNo, fun1no, fun2no, fun3no, fun4no);
         //  if (result)//插入订单成功。记录仓位工艺
            {
                int index = dataGridVieworder.Rows.Add();
                dataGridVieworder.Rows[index].Cells[0].Value =dataGridVieworder.Rows.Count;
               // dataGridVieworder.Rows[index].Cells[0].Value = dataGridVieworder.Rows.Count - 1;
                dataGridVieworder.Rows[index].Cells[1].Value = textBox1magno.Text;
                dataGridVieworder.Rows[index].Cells[2].Value = comboBoxFun1.Text;
                dataGridVieworder.Rows[index].Cells[3].Value = comboBoxFun2.Text;
                dataGridVieworder.Rows[index].Cells[4].Value = comboBoxFun3.Text;
                dataGridVieworder.Rows[index].Cells[5].Value = comboBoxFun4.Text;
                if (language == "English")
                {
                    dataGridVieworder.Rows[index].Cells[6].Value = "OK";
                    dataGridVieworder.Rows[index].Cells[7].Value = "None";
                    dataGridVieworder.Rows[index].Cells[8].Value = "OK";
                }
                else
                {
                    dataGridVieworder.Rows[index].Cells[6].Value = "确定";
                    dataGridVieworder.Rows[index].Cells[7].Value = "无";
                    dataGridVieworder.Rows[index].Cells[8].Value = "确定";
                }
                

                int index2 = dataGridVieworder2.Rows.Add();

                //dataGridVieworder2.Rows[index2].Cells[0].Value = dataGridVieworder2.Rows.Count - 1;
                dataGridVieworder2.Rows[index2].Cells[0].Value = dataGridVieworder2.Rows.Count;
                dataGridVieworder2.Rows[index2].Cells[1].Value = dataGridVieworder.Rows[index].Cells[1].Value;
                if (language == "English")
                {
                    for (int ii = 2; ii < 6; ii++)
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
                    dataGridVieworder2.Rows[index2].Cells[6].Value = "NotDownload";
                    dataGridVieworder2.Rows[index2].Cells[7].Value = "None";
                }
                else
                {
                    for (int ii = 2; ii < 6; ii++)
                    {
                        string temp = dataGridVieworder.Rows[index].Cells[ii].Value.ToString();
                        if (temp != "无")
                        {
                            dataGridVieworder2.Rows[index2].Cells[ii].Value = "未开始";
                        }
                        else
                        {
                            dataGridVieworder2.Rows[index2].Cells[ii].Value = "无";
                        }
                    }
                    dataGridVieworder2.Rows[index2].Cells[6].Value = "未下发";
                    dataGridVieworder2.Rows[index2].Cells[7].Value = "无";
                }
 
            }
        
        
        }


        private void OrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void OrderForm_SizeChanged(object sender, EventArgs e)
        {
            aotosize.controlAutoSize(this);
        }

        private void dataGridVieworder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (!MainForm.PLC_SIMES_ON_line)//产线不是启动状态
            //{
            //    if (language == "English")
            //    {
            //        MessageBox.Show("PLC Off Line");
            //    }
            //    else MessageBox.Show("PLC离线");
            //    return;
            //}
            //if (MainForm.linestart)//产线不是启动状态
            //{
            //    if (language == "English")
            //    {
            //        MessageBox.Show("Line not Started yet");
            //    }
            //    MessageBox.Show("产线未启动");
            //    return;
            //}
            if (e.RowIndex >= 0 && e.RowIndex < dataGridVieworder.Rows.Count)
            {
                DataGridViewColumn column = dataGridVieworder.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                   //行列都是0开始计算，表头不算， MessageBox.Show("x: " + e.RowIndex.ToString() + ",y:" + e.ColumnIndex.ToString());
                    if (e.ColumnIndex == 6)//订单派发按钮按下//BackColor = System.Drawing.Color.LightGreen;
                    {                                     
                        //dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gray;
                        if (language == "English")
                        {
                            if (dataGridVieworder2.Rows[e.RowIndex].Cells[6].Value.ToString() == "NotDownload")//订单未下达
                            {
                                string magnumbers=dataGridVieworder2.Rows[e.RowIndex].Cells[1].Value.ToString();
                            
                                 //for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)
                                for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)
                                {
                                    if (dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == magnumbers
                                       &&
                                         (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "进行中"
                                          || dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "Processing"
                                          || dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "返修中"
                                         || dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "ReProcessing"))
                                    {
                                        if (language == "English")
                                        {
                                            MessageBox.Show("The Mag is processing ,please shoose another");
                                        }
                                        else
                                            MessageBox.Show("当前订单的仓位正在加工，请选择其他订单");
                                        return;
                                    }
                                }

                               
                                //  if( dataGridVieworderdown(e.RowIndex, e.ColumnIndex))//订单下发成功
                                dataGridVieworderdown(e.RowIndex, e.ColumnIndex);
                                dataGridVieworder.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (dataGridVieworder2.Rows[e.RowIndex].Cells[6].Value.ToString() == "未下发")//订单未下达
                            {
                                string magnumbers = dataGridVieworder2.Rows[e.RowIndex].Cells[1].Value.ToString();

                                //for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)
                                for (int jj = 0; jj < dataGridVieworder.RowCount; jj++)
                                {
                                    if (dataGridVieworder.Rows[jj].Cells[1].Value.ToString() == magnumbers
                                       &&
                                         (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "进行中"
                                          || dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "Processing"
                                          || dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "返修中"
                                         || dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "ReProcessing"))
                                    {
                                        if (language == "English")
                                        {
                                            MessageBox.Show("The Mag is processing ,please shoose another");
                                        }
                                        else
                                            MessageBox.Show("当前订单的仓位正在加工，请选择其他订单");
                                        return;
                                    }
                                }

                                //  if( dataGridVieworderdown(e.RowIndex, e.ColumnIndex))//订单下发成功
                                dataGridVieworderdown(e.RowIndex, e.ColumnIndex);
                                dataGridVieworder.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                return;
                            }
                        }
                        
                       // e.RowIndex，行号
                        // dataGridVieworder.Rows[e.RowIndex].Cells[1].Value //仓位
                        //查询该订单的执行情况，订单执行有另外一张表格
                        //下达任务，按钮失效。
                    }
                    if (e.ColumnIndex == 8)//返修按钮按下
                    {
                        if (language == "English")
                        {
                            if (dataGridVieworder2.Rows[e.RowIndex].Cells[6].Value.ToString() == "Finish" || dataGridVieworder2.Rows[e.RowIndex].Cells[6].Value.ToString() == "NeedReProcesses")//订单未下达
                            {
                                bool flage = false;
                                string temp = dataGridVieworder.Rows[e.RowIndex].Cells[7].Value.ToString();
                                if (temp == "None")
                                {
                                    MessageBox.Show("Please choose reback processes");
                                    return;
                                }
                                else if (temp == "Processes1")
                                {
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "None")
                                    {
                                        MessageBox.Show("Current Processes is none");
                                        return;
                                    }
                                    flage = dataGridViewbackorderdown(e.RowIndex, 2);
                                }
                                else if (temp == "Processes2")
                                {
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "None")
                                    {
                                        MessageBox.Show("Current Processes is none");
                                        return;
                                    }
                                    flage = dataGridViewbackorderdown(e.RowIndex, 3);
                                }
                                else if (temp == "Processes3")
                                {
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[4].Value.ToString() == "None")
                                    {
                                        MessageBox.Show("Current Processes is none");
                                        return;
                                    }
                                    flage = dataGridViewbackorderdown(e.RowIndex, 4);
                                }
                                else if (temp == "Processes4")
                                {
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "None")
                                    {
                                        MessageBox.Show("Current Processes is none");
                                        return;
                                    }
                                    flage = dataGridViewbackorderdown(e.RowIndex, 5);
                                }
                                if (flage == true)
                                {
                                    dataGridVieworderdown(e.RowIndex, e.ColumnIndex);
                                    dataGridVieworder.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gray;

                                }
                            }
                            else
                            {
                                MessageBox.Show("Current Order is not finish or needreback");
                                return;
                            }
                        }
                        else
                        {
                            if (dataGridVieworder2.Rows[e.RowIndex].Cells[6].Value.ToString() == "完成" || dataGridVieworder2.Rows[e.RowIndex].Cells[6].Value.ToString() == "待返修")//订单未下达
                            {
                                bool flage = false;
                                string temp = dataGridVieworder.Rows[e.RowIndex].Cells[7].Value.ToString();
                                if (temp == "无")
                                {
                                    MessageBox.Show("请选择返修工序");
                                    return;
                                }
                                else if (temp == "工序一")
                                {
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[2].Value.ToString() == "无")
                                    {
                                        MessageBox.Show("当前工序无动作，请确认返修工序");
                                        return;
                                    }
                                    flage = dataGridViewbackorderdown(e.RowIndex, 2);
                                }
                                else if (temp == "工序二")
                                {
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[3].Value.ToString() == "无")
                                    {
                                        MessageBox.Show("当前工序无动作，请确认返修工序");
                                        return;
                                    }
                                    flage = dataGridViewbackorderdown(e.RowIndex, 3);
                                }
                                else if (temp == "工序三")
                                {
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[4].Value.ToString() == "无")
                                    {
                                        MessageBox.Show("当前工序无动作，请确认返修工序");
                                        return;
                                    }
                                    flage = dataGridViewbackorderdown(e.RowIndex, 4);
                                }
                                else if (temp == "工序四")
                                {
                                    if (dataGridVieworder.Rows[e.RowIndex].Cells[5].Value.ToString() == "无")
                                    {
                                        MessageBox.Show("当前工序无动作，请确认返修工序");
                                        return;
                                    }
                                    flage = dataGridViewbackorderdown(e.RowIndex, 5);
                                }
                                if (flage == true)
                                {
                                    dataGridVieworderdown(e.RowIndex, e.ColumnIndex);
                                    dataGridVieworder.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gray;

                                }
                            }
                            else
                            {
                                MessageBox.Show("当前订单不是完成状态或者返修状态，不能执行单休流程");
                                return;
                            }
                        }
                       
                       //查询对应订单派发按钮是否按下，查询该仓位没有在加工
                        //查询当前工序对应机床没有正在加工
                        //订单下达
                        //工件状态修改为加工中
                    }
                }
            }
        }

        private bool dataGridVieworderdown(int RowIndex ,int ColumnIndex)
        {
            string MagNoStr = dataGridVieworder.Rows[RowIndex].Cells[1].Value.ToString();
            int MagNo = Convert.ToInt16(MagNoStr);
             int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
             int maglength = (int)ModbusTcp.MagLength;
             int magstatei = magstart + maglength * (MagNo-1) ;
             int i = MainForm.cnclist[1].MagNo;
             int magchecki = magstatei - 1;
             int magfuni = magstatei-2;//当前加工工序号
             int robortishome = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm];
             bool gongxucnc1flage = false;
             bool gongxucnc2flage = false;         
             string firstgongxu = "";

             int curfun = 0;//当前加工工序
           
             if (MainForm.PLC_SIMES_ON_line == false)
             {
                 if (language == "English")
                 {
                     MessageBox.Show("Load failure,because PLC is off line，");
                 }
                 else MessageBox.Show("PLC未连接，不能下达订单!");
                 return false;
             }
             if (!MainForm.linestop )//产线停止状态
             {
                 if (language == "English")
                 {
                     MessageBox.Show("Load failure,Please start line!");
                 }
                 else  MessageBox.Show("订单下达失败，请先启动产线!");
                   return false;
             }
             if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statenull)
             {
                 if (language == "English")
                 {
                     MessageBox.Show("Current Mag is empty ,Please make a new Order!");
                 }
                 else MessageBox.Show("指定仓位无料，请重新下达订单!");
                    return false;
             }
             if (robortishome == 0)
             {
                 if (language == "English")
                 {
                     MessageBox.Show("Robort is not at home!");
                 }
                 else  MessageBox.Show("机器人不在HOME位置无法下达订单！");
                   return false;
             }
             for(int ii=2;ii<6;ii++)
             {
                 if (language == "English")
                 {
                     if (dataGridVieworder.Rows[RowIndex].Cells[ii].Value.ToString() == "Lathe")
                     {
                         if (curfun == 0)
                         {
                             curfun = ii - 1;//获取工序号，1、2、3、4
                         }
                         if (firstgongxu == "")
                         {
                             firstgongxu = "Lathe";
                         }
                         gongxucnc1flage = true;
                     }
                     else if (dataGridVieworder.Rows[RowIndex].Cells[ii].Value.ToString() == "CNC")
                     {
                         if (curfun == 0)
                         {
                             curfun = ii - 1;//获取工序号，1、2、3、4
                         }
                         if (firstgongxu == "")
                         {
                             firstgongxu = "CNC";
                         }
                         gongxucnc2flage = true;
                     }
                 }
                 else
                 {
                     if (dataGridVieworder.Rows[RowIndex].Cells[ii].Value.ToString() == "车工序")
                     {
                         if (curfun == 0)
                         {
                             curfun = ii - 1;//获取工序号，1、2、3、4
                         }
                         if (firstgongxu == "")
                         {
                             firstgongxu = "车工序";
                         }
                         gongxucnc1flage = true;
                     }
                     else if (dataGridVieworder.Rows[RowIndex].Cells[ii].Value.ToString() == "铣工序")
                     {
                         if (curfun == 0)
                         {
                             curfun = ii - 1;//获取工序号，1、2、3、4
                         }
                         if (firstgongxu == "")
                         {
                             firstgongxu = "铣工序";
                         }
                         gongxucnc2flage = true;
                     }
                 }
                
             }
             
             //单车工序
             if(gongxucnc1flage == true&&gongxucnc2flage==false)
             {
                 if (cnc0orderdown(MagNo))
                   {
                      // ModbusTcp.DataMoubus[magfuni] = curfun;
                       MainForm.Mag_Fun_cur[MagNo - 1] = curfun;
                       if (language == "English")
                       {
                           if (dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value.ToString() == "NotStarted")
                           {
                               dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "Processing";
                           }

                           if (dataGridVieworder2.Rows[RowIndex].Cells[6].Value.ToString() == "NotDownload")
                           {
                               dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "Processing";
                           }
                       
                       }
                       else
                       {
                           if (dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value.ToString() == "未开始")
                           {
                               dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "进行中";
                           }

                           if (dataGridVieworder2.Rows[RowIndex].Cells[6].Value.ToString() == "未下发")
                           {
                               dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "进行中";
                           }
                       
                       }

                      
                        
                        return true;
                   }
                 return false;
             }
             //单铣工序
             else   if (gongxucnc2flage == true && gongxucnc1flage == false)
             {
                 if (cnc1orderdown(MagNo))
                 {

                    // ModbusTcp.DataMoubus[magfuni] = curfun;
                     MainForm.Mag_Fun_cur[MagNo - 1] = curfun;
                     if (language == "English")
                     {
                         if (dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value.ToString() == "NotStarted")
                         {
                             dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "Processing";
                         }

                         if (dataGridVieworder2.Rows[RowIndex].Cells[6].Value.ToString() == "NotDownload")
                         {
                             dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "Processing";
                         }
                     }
                     else
                     {
                         if (dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value.ToString() == "未开始")
                         {
                             dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "进行中";
                         }

                         if (dataGridVieworder2.Rows[RowIndex].Cells[6].Value.ToString() == "未下发")
                         {
                             dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "进行中";
                         }
                        
                     }
                     return true;
                 }
                 return false;
             }
             //车铣复合工序
             else if (gongxucnc2flage == true && gongxucnc1flage == true)
             {
                 if (cnc0and1orderdown(MagNo, firstgongxu))
                 {

                     //ModbusTcp.DataMoubus[magfuni] = curfun;
                     MainForm.Mag_Fun_cur[MagNo - 1] = curfun;
                     if (language == "English")
                     {
                         if (dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value.ToString() == "NotStarted")
                         {
                             dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "Processing";
                         }

                         if (dataGridVieworder2.Rows[RowIndex].Cells[6].Value.ToString() == "NotDownload")
                         {
                             dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "Processing";
                         }
                     }
                     else
                     {
                         if (dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value.ToString() == "未开始")
                         {
                             dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "进行中";
                         }

                         if (dataGridVieworder2.Rows[RowIndex].Cells[6].Value.ToString() == "未下发")
                         {
                             dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "进行中";
                         }
                     }
                     
                     return true;
                 }
                 return false;
             }
             else
             {
                 if (language == "English")
                 {
                     MessageBox.Show("The processes is none，Order can not download！");
                 }
                 else  MessageBox.Show("没有设定加工工序，订单无法下达！");
                 return false;
             }
        
        }

        private bool dataGridViewbackorderdown(int RowIndex, int  fun)
        {
            string MagNoStr = dataGridVieworder.Rows[RowIndex].Cells[1].Value.ToString();
            int MagNo = Convert.ToInt16(MagNoStr);
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = magstart + maglength * (MagNo - 1);
            int i = MainForm.cnclist[1].MagNo;
            int magchecki = magstatei - 1;
            int magfuni = magstatei - 2;//当前加工工序号
            int robortishome = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm];
            bool gongxucnc1flage = false;
            bool gongxucnc2flage = false;
            string firstgongxu = "";

            int curfun = 0;//当前加工工序

            if (MainForm.PLC_SIMES_ON_line == false)
            {
                if (language == "English")
                {
                    MessageBox.Show("Load failure,because PLC is off line，");
                }
                else MessageBox.Show("PLC未连接，不能下达订单!");
                return false;
            }
            if (!MainForm.linestop)//产线停止状态
            {
                if (language == "English")
                {
                    MessageBox.Show("Load failure,Please start line!");
                }
                else MessageBox.Show("订单下达失败，请先启动产线!");
                return false;
            }
            if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statenull)
            {
                if (language == "English")
                {
                    MessageBox.Show("Current Mag is empty ,Please make a new Order!");
                }
                else MessageBox.Show("指定仓位无料，请重新下达订单!");
                return false;
            }
            if (robortishome == 0)
            {
                if (language == "English")
                {
                    MessageBox.Show("Robort is not at home!");
                }
                else MessageBox.Show("机器人不在HOME位置无法下达订单！");
                return false;
            }
            if (language == "English")
            {
                if (dataGridVieworder.Rows[RowIndex].Cells[fun].Value.ToString() == "Lathe")
                {
                    if (curfun == 0)
                    {
                        curfun = fun - 1;//获取工序号，1、2、3、4
                    }
                    if (firstgongxu == "")
                    {
                        firstgongxu = "Lathe";
                    }
                    gongxucnc1flage = true;
                }
                else if (dataGridVieworder.Rows[RowIndex].Cells[fun].Value.ToString() == "CNC")
                {
                    if (curfun == 0)
                    {
                        curfun = fun - 1;//获取工序号，1、2、3、4
                    }
                    if (firstgongxu == "")
                    {
                        firstgongxu = "CNC";
                    }
                    gongxucnc2flage = true;
                }

                //单车工序
                if (gongxucnc1flage == true && gongxucnc2flage == false)
                {
                    if (cnc0orderdown(MagNo))
                    {
                        //ModbusTcp.DataMoubus[magfuni] = curfun;
                        MainForm.Mag_Fun_cur[MagNo - 1] = curfun;
                        dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "Processing";
                        dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "ReProcessing";
                        return true;
                    }
                    return false;
                }
                //单铣工序
                else if (gongxucnc2flage == true && gongxucnc1flage == false)
                {
                    if (cnc1orderdown(MagNo))
                    {

                        //ModbusTcp.DataMoubus[magfuni] = curfun;
                        MainForm.Mag_Fun_cur[MagNo - 1] = curfun;
                        dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "Processing";
                        dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "ReProcessing";
                        return true;
                    }
                    return false;
                }

                else
                {
                    MessageBox.Show("The processes is none，Order can not download！");
                    return false;
                }
            }
            else
            {
                if (dataGridVieworder.Rows[RowIndex].Cells[fun].Value.ToString() == "车工序")
                {
                    if (curfun == 0)
                    {
                        curfun = fun - 1;//获取工序号，1、2、3、4
                    }
                    if (firstgongxu == "")
                    {
                        firstgongxu = "车工序";
                    }
                    gongxucnc1flage = true;
                }
                else if (dataGridVieworder.Rows[RowIndex].Cells[fun].Value.ToString() == "铣工序")
                {
                    if (curfun == 0)
                    {
                        curfun = fun - 1;//获取工序号，1、2、3、4
                    }
                    if (firstgongxu == "")
                    {
                        firstgongxu = "铣工序";
                    }
                    gongxucnc2flage = true;
                }

                //单车工序
                if (gongxucnc1flage == true && gongxucnc2flage == false)
                {
                    if (cnc0orderdown(MagNo))
                    {
                        //ModbusTcp.DataMoubus[magfuni] = curfun;
                        MainForm.Mag_Fun_cur[MagNo - 1] = curfun;
                        dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "进行中";
                        dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "返修中";
                        return true;
                    }
                    return false;
                }
                //单铣工序
                else if (gongxucnc2flage == true && gongxucnc1flage == false)
                {
                    if (cnc1orderdown(MagNo))
                    {

                        //ModbusTcp.DataMoubus[magfuni] = curfun;
                        MainForm.Mag_Fun_cur[MagNo - 1] = curfun;
                        dataGridVieworder2.Rows[RowIndex].Cells[1 + curfun].Value = "进行中";
                        dataGridVieworder2.Rows[RowIndex].Cells[6].Value = "返修中";
                        return true;
                    }
                    return false;
                }

                else
                {
                    MessageBox.Show("没有设定加工工序，订单无法下达！");
                    return false;
                }
            }

        }

        private bool cnc0orderdown(int MagNo)
        {
            int MesReq_order = (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm;
            int MesReq_order_code = (int)ModbusTcp.MesCommandToPlc.ComProcessManage;
            int Mesreq_Rack_number = (int)ModbusTcp.DataConfigArr.Rack_number_comfirm;
            int Mesreq_Order_type = (int)ModbusTcp.DataConfigArr.Order_type_comfirm;
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = magstart + maglength * (MagNo - 1);
       
           string  language1 = ChangeLanguage.GetDefaultLanguage();
            int magchecki = magstatei - 1;
            if (MainForm.cnclist[0].isConnected() == true)//20180312
            {
                if (MainForm.cnclist[0].MagNo == 0 || MainForm.cnclist[0].MagNo == MagNo)//机床状态判定，如果机床占用，不可下单 //20180312  
                {
                    
                    int Gcodeloadreturn = GcodeSenttoCNC(MagNo, 1);
                    if (Gcodeloadreturn==1)
                    {

                    }
                    else if (Gcodeloadreturn==-1)
                    {
                        if (language1 == "English")
                        {
                            MessageBox.Show("The Gcode name err");
                        }
                        else MessageBox.Show("没有找到匹配的G代码");
                        return false;
                    }
                    else
                    {
                        if (language1 == "English")
                        {
                            MessageBox.Show("The Gcode download err");
                        }
                        else MessageBox.Show("G代码下发失败");  
                        return false;
                    }
                    if(ModbusTcp.Rack_number_write_flage)
                    {
                        if (language1 == "English")
                        {
                            MessageBox.Show("");
                        }
                        else MessageBox.Show("前一个订单正在下发，请稍后下发下一订单");
                        return false;
                    }
                    MainForm.cnclist[0].MagNo = MagNo;//机床绑定料仓号 
                    ModbusTcp.Rack_number_write_flage = true;
                    ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                    ModbusTcp.DataMoubus[Mesreq_Rack_number] = MagNo;//料仓编号
                    ModbusTcp.DataMoubus[Mesreq_Order_type] = 1;//加工类型
                    ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                    ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;
                    return true;
                }
                else
                {
                    if (language1 == "English")
 
                    {
                       MessageBox.Show("The Lathe is processing,can not download");
                    }
                    else MessageBox.Show("车床有未完成订单，本次订单下达失败");    //20180312              
                    return false;
                }
            }
            else
            {
                if (language1 == "English")
                {
                    MessageBox.Show("The Lathe is off line,can not download");
                 }
                else  MessageBox.Show("车床离线，本次订单下达失败");    //20180312   
                return false;
            }
        }
        private bool cnc1orderdown(int MagNo)
        {
            int MesReq_order = (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm;
            int MesReq_order_code = (int)ModbusTcp.MesCommandToPlc.ComProcessManage;
            int Mesreq_Rack_number = (int)ModbusTcp.DataConfigArr.Rack_number_comfirm;
            int Mesreq_Order_type = (int)ModbusTcp.DataConfigArr.Order_type_comfirm;
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = magstart + maglength * (MagNo - 1);
            
           string  language1 = ChangeLanguage.GetDefaultLanguage();
            int magchecki = magstatei - 1;
            if (MainForm.cnclist[1].isConnected() == true)//20180312
            {
                if (MainForm.cnclist[1].MagNo == 0 || MainForm.cnclist[1].MagNo == MagNo)//机床状态判定，如果机床占用，不可下单 //20180312  
                {
                    int Gcodeloadreturn = GcodeSenttoCNC(MagNo, 2);
                    if (Gcodeloadreturn == 1)
                    {

                    }
                    else if (Gcodeloadreturn == -1)
                    {
                        if (language1 == "English")
                        {
                            MessageBox.Show("The Gcode name err");
                        }
                        else MessageBox.Show("没有找到匹配的G代码");
                        return false;
                    }
                    else
                    {
                        if (language1 == "English")
                        {
                            MessageBox.Show("The Gcode download err");
                        }
                        else MessageBox.Show("G代码下发失败");
                        return false;
                    }
                    if (ModbusTcp.Rack_number_write_flage)
                    {
                        if (language1 == "English")
                        {
                            MessageBox.Show("");
                        }
                        else MessageBox.Show("前一个订单正在下发，请稍后下发下一订单");
                        return false;
                    }
                    MainForm.cnclist[1].MagNo = MagNo;//机床绑定料仓号
                    ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                    ModbusTcp.DataMoubus[Mesreq_Rack_number] = MagNo;//料仓编号
                    ModbusTcp.DataMoubus[Mesreq_Order_type] = 2;//加工类型
                    ModbusTcp.Rack_number_write_flage = true;//订单下发标识
                    ModbusTcp.DataMoubus[magchecki] = 1;//检测默认为正常   
                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;
                    return true;                  
                    
                }
                else
                {
                    if (language1 == "English")
 
                    {
                       MessageBox.Show("The CNC is processing,the order download failure");
                    }
                    else MessageBox.Show("加工中心有未完成订单，本次订单下达失败");    //20180312              
                    return false;
                }
            }
            else
            {
                if (language1 == "English")
                {
                    MessageBox.Show("The CNC is off line,can not download");
                 }
                else MessageBox.Show("加工中心离线，本次订单下达失败");    //20180312   
                return false;
            }
        }
        private bool cnc0and1orderdown(int MagNo,string firstgongxu)
        {
            int MesReq_order = (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm;
            int MesReq_order_code = (int)ModbusTcp.MesCommandToPlc.ComProcessManage;
            int Mesreq_Rack_number = (int)ModbusTcp.DataConfigArr.Rack_number_comfirm;
            int Mesreq_Order_type = (int)ModbusTcp.DataConfigArr.Order_type_comfirm;
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = magstart + maglength * (MagNo - 1);

            int magchecki = magstatei - 1;
            int gongyiNo = 0;
            if (MainForm.cnclist[0].isConnected() == true && MainForm.cnclist[1].isConnected())
            {
                if (MainForm.cnclist[0].MagNo != 0 && MainForm.cnclist[1].MagNo != 0)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("Lathe and cnc is processing，the order do");
                    }
                    else MessageBox.Show("车床和铣床有未完成订单，本次订单下达失败");
                    return false;
                }
                else if (MainForm.cnclist[0].MagNo != 0 && MainForm.cnclist[1].MagNo == 0)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("Lathe  is processing，the order do");
                    }
                    else MessageBox.Show("车床有未完成订单，本次订单下达失败");
                    return false;
                }
                else if (MainForm.cnclist[0].MagNo == 0 && MainForm.cnclist[1].MagNo != 0)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("Cnc is processing，the order down failure");
                    }
                    else MessageBox.Show("加工中心有未完成订单，本次订单下达失败");
                    return false;
                }
                else if (MainForm.cnclist[1].MagNo == 0 && MainForm.cnclist[0].MagNo == 0)//机床状态判定，如果机床占用，不可下单
                {
                    if (language == "English")
                    {
                        if (firstgongxu == "Lathe")
                        {
                            gongyiNo = 1;
                        }
                        if (firstgongxu == "CNC")
                        {
                            gongyiNo = 2;
                        }
                    }
                    else
                    {
                        if (firstgongxu == "车工序")
                        {
                            gongyiNo = 1;
                        }
                        if (firstgongxu == "铣工序")
                        {
                            gongyiNo = 2;
                        }
                    }
                    int Gcodeloadreturn1 = GcodeSenttoCNC(MagNo, 1);
                    int Gcodeloadreturn2 = GcodeSenttoCNC(MagNo, 2);
                    if (Gcodeloadreturn1 != 1 ||Gcodeloadreturn1!= 1)
                    {
                        if (language == "English")
                        {
                            MessageBox.Show("The CNC and Lathe can download Gcode err");
                        }
                        else MessageBox.Show("下传G代码失败");
                        return false;
                    }
                    if (ModbusTcp.Rack_number_write_flage)
                    {
                        if (language== "English")
                        {
                            MessageBox.Show("");
                        }
                        else MessageBox.Show("前一个订单正在下发，请稍后下发下一订单");
                        return false;
                    }
                    MainForm.cnclist[1].MagNo = MagNo;//机床绑定料仓号
                    MainForm.cnclist[0].MagNo = MagNo;//机床绑定料仓号

                    ModbusTcp.DataMoubus[MesReq_order] = MesReq_order_code;//加工命令码
                    ModbusTcp.DataMoubus[Mesreq_Rack_number] = MagNo;//料仓编号
                    ModbusTcp.DataMoubus[Mesreq_Order_type] = gongyiNo;//加工类型
                    ModbusTcp.DataMoubus[magchecki] = 0;//检测默认为0   
                    ModbusTcp.Rack_number_write_flage = true;//订单下发标识           
                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateProcessing;
                    return true;
                   
                   
                }
                else
                {
                    if (language == "English")
                    {
                        MessageBox.Show("Lathe and cnc is processing，the order do");
                    }
                    else MessageBox.Show("车床和加工中心有未完成订单，本次订单下达失败");
                    return false;
                }
            }
            else
            {
                if (language == "English")
                {
                    MessageBox.Show("Lathe or cnc is off line，the order download failure");
                }
                else MessageBox.Show("车床或者加工中心离线，本次订单下达失败");
                return false;
            }
        }
        private string getvalueformstring(string line,string indexstring)
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
            int length= 0;
            while (str != "," && str !=";")
            {
                index2++;
                length++;
                str = line.Substring(index2, 1);
            }
            return temp = line.Substring(index1, length);
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
                string fun3 = "";
                string fun4 = "";
                string down = "";
                string backfun = "";
                string backdown = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();

                while (line != null&&line!="")
                {
                    dataGridVieworder.Rows.Add();
                    item = getvalueformstring(line, "item=");
                    magno = getvalueformstring(line, "magno=");
                    fun1 = getvalueformstring(line, "fun1=");
                    fun2 = getvalueformstring(line, "fun2=");
                    fun3 = getvalueformstring(line, "fun3=");
                     fun4 = getvalueformstring(line, "fun4=");
                    down = getvalueformstring(line, "down=");
                    backfun = getvalueformstring(line, "backfun=");
                    backdown = getvalueformstring(line, "backdown=");
                    if (item == "null" || magno == "null" || fun1 == "null" || fun2 == "null" || fun3 == "null" || fun4 == "null" || down == "null" || backfun == "null" || backdown == "null")
                    {
                        line = sr.ReadLine();
                       
                    }
                    else
                    {
                        dataGridVieworder.Rows[ii].Cells[0].Value = item;     
                        dataGridVieworder.Rows[ii].Cells[1].Value = magno;
                        dataGridVieworder.Rows[ii].Cells[2].Value = fun1;
                        dataGridVieworder.Rows[ii].Cells[3].Value = fun2;
                        dataGridVieworder.Rows[ii].Cells[4].Value = fun3;
                        dataGridVieworder.Rows[ii].Cells[5].Value = fun4;
                        if (language == "English")
                        {
                            dataGridVieworder.Rows[ii].Cells[6].Value = "OK";
                            if (down == "Yes")
                            {
                                dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.Gray;
                            }
                            dataGridVieworder.Rows[ii].Cells[7].Value = backfun;
                            dataGridVieworder.Rows[ii].Cells[8].Value = "OK";
                            if (backdown == "Yes")
                            {
                                dataGridVieworder.Rows[ii].Cells[8].Style.BackColor = Color.Gray;
                            }
                        }
                        else
                        {
                            dataGridVieworder.Rows[ii].Cells[6].Value = "确定";
                            if (down == "是")
                            {
                                dataGridVieworder.Rows[ii].Cells[6].Style.BackColor = Color.Gray;
                            }
                            dataGridVieworder.Rows[ii].Cells[7].Value = backfun;
                            dataGridVieworder.Rows[ii].Cells[8].Value = "确定";
                            if (backdown == "是")
                            {
                                dataGridVieworder.Rows[ii].Cells[8].Style.BackColor = Color.Gray;
                            }
                        }
                        

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

        public static int GcodeSenttoCNC(int MagNo, int gongyiNo)
        {
            string gcoderoad = GcodeFilePath;
            string gcodename = "";
           if (MagNo == 0)
           {
               if (gongyiNo==1)//车床
               {
                    gcodename = "OHOMEL.nc";
                    gcoderoad = GcodeFilePath + "\\" + gcodename;
                    if (MainForm.cnclist[0].sendFile(gcoderoad, "h/lnc8/prog/" + gcodename, 0, false) == 0)
                   {
                       return 1;
                   }
                   else
                   {
                       return 0;
                   }
                }
               else  if (gongyiNo == 2)//铣床
                {
                    gcodename = "OHOMECNC.nc";
                    gcoderoad = GcodeFilePath + "\\" + gcodename;
                    if (MainForm.cnclist[1].sendFile(gcoderoad, "h/lnc8/prog/" + gcodename, 0, false) == 0)
                   {
                      return 1;
                   }
                   else
                   {
                       return 0;
                   }
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
            int  magSecene = ModbusTcp.DataMoubus[magSecenei];
            int  magmeterial = ModbusTcp.DataMoubus[magSecenei+2];
            int  magtypestart = ModbusTcp.DataMoubus[magSecenei+1];
            char magSeceneic;
            string magSeceneis = "";
            string  magmeterialis = Convert.ToString(magmeterial);
            string magtypestartis = "";
            //A的ascii码为41大写
            magSecenei = magSecene + 64;

            magSeceneic = Convert.ToChar(magSecenei);
            magSeceneis = Convert.ToString(magSeceneic);
            if (magtypestart<10)
            {               
                magtypestartis = Convert.ToString(magtypestart);
                magtypestartis = "0" + magtypestartis;
            }
            else
            {
                magtypestartis = Convert.ToString(magtypestart);
            }
           
            if (gongyiNo==1)//车床
            {
                gcodename = "O" + magSeceneis + magtypestartis + magmeterialis + "L.nc";
                gcoderoad = GcodeFilePath + "\\" + gcodename;
                if (!File.Exists(@gcoderoad))
                {
                    return -1;
                }
               // if (MainForm.cnclist[0].sendFile(gcoderoad, "..prog/" + gcodename, 0, false) == 0)
                    if (MainForm.cnclist[0].sendFile(gcoderoad, "h/lnc8/prog/" + gcodename, 0, false) == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else  if (gongyiNo == 2)//铣床
            {
                gcodename = "O" + magSeceneis + magtypestartis + magmeterialis + "CNC.nc";
                gcoderoad = GcodeFilePath + "\\" + gcodename;
                if (!File.Exists(@gcoderoad))
                {
                    return -1;
                }
               //if (MainForm.cnclist[1].sendFile(gcoderoad, "../prog/" + gcodename, 0, false) == 0)
                    if (MainForm.cnclist[1].sendFile(gcoderoad, "h/lnc8/prog/" + gcodename, 0, false) == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
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
                string fun3 = "";
                string fun4 = "";
                string down = "";
                string check = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();

                while (line != null && line != "")
                {
                    item = getvalueformstring(line, "item=");
                    magno = getvalueformstring(line, "magno=");
                    fun1 = getvalueformstring(line, "fun1=");
                    fun2 = getvalueformstring(line, "fun2=");
                    fun3 = getvalueformstring(line, "fun3=");
                     fun4 = getvalueformstring(line, "fun4=");
                    down = getvalueformstring(line, "down=");
                    check = getvalueformstring(line, "check=");
                    if (item == "null" || magno == "null" || fun1 == "null" || fun2 == "null" || fun3 == "null" || fun4 == "null" || down == "null" || check == "null")
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
                        dataGridVieworder2.Rows[ii].Cells[4].Value = fun3;
                        dataGridVieworder2.Rows[ii].Cells[5].Value = fun4;
                        dataGridVieworder2.Rows[ii].Cells[6].Value = down;
                        dataGridVieworder2.Rows[ii].Cells[7].Value = check;

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
                int jj = 0;
                string orderdatas = "";
                string odersdownflage = "否";
                string odersbackdownflage = "否";
                //if (dataGridVieworder.Rows.Count<2)
                //{
                //    return;
                //}

                if (dataGridVieworder.Rows.Count>1)
                {
                    if ( language == "English")
                    {
                            this.Column8.Items.Clear();
                            this.Column8.Items.AddRange(new object[] {
                                "None",
                                "Processes1",
                                "Processes2",
                                "Processes3",
                               "Processes4"});
           
                    }
                    if ( language == "Chinese")
                    {
                         this.Column8.Items.Clear();
                            this.Column8.Items.AddRange(new object[] {
                                        "无",
                                        "工序一",
                                        "工序二",
                                        "工序三",
                                        "工序四"});
                    }
                }
                for (jj = 0; jj < dataGridVieworder.Rows.Count; jj++)
                {
                   
                        if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "None"
                                || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "无")
                        {
                            gongxuindex[jj] = 0;
                        }
                        if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "Processes1"
                                || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "工序一")
                        {
                            gongxuindex[jj] = 1;
                        }
                        if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "Processes2"
                               || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "工序二")
                        {
                            gongxuindex[jj] = 2;
                        }
                        if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "Processes3"
                               || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "工序三")
                        {
                            gongxuindex[jj] = 3;
                        }
                        if (dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "Processes4"
                               || dataGridVieworder.Rows[jj].Cells[7].Value.ToString() == "工序四")
                        {
                            gongxuindex[jj] = 4;
                        }

                    
                    
                    if (dataGridVieworder.Rows[jj].Cells[6].Style.BackColor == Color.Gray)
                    {
                        if (language == "English")
                        {
                            odersdownflage = "Yes";
                        }
                        else odersdownflage = "是";
                    }
                    else
                    {
                        if (language == "English")
                        {
                            odersdownflage = "No";
                        }
                        else odersdownflage = "否";
                    }
                   if (dataGridVieworder.Rows[jj].Cells[8].Style.BackColor == Color.Gray)
                    {
                        if (language == "English")
                        {
                            odersbackdownflage = "Yes";
                        }
                        else odersbackdownflage = "是";
                    }
                   else
                   {
                       if (language == "English")
                       {
                           odersbackdownflage = "No";
                       }
                       else odersbackdownflage = "否";
                   }
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
                               for(int kk=2;kk<6;kk++)
                               {
                                   if(dataGridVieworder.Rows[jj].Cells[kk].Value.ToString()=="无")
                                   {
                                       dataGridVieworder.Rows[jj].Cells[kk].Value="None";
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
                               //DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
                               //     cell = (DataGridViewComboBoxCell)dataGridVieworder.Rows[jj].Cells[7];
                               //ComboBox cell1 = new ComboBox();
                               if (gongxuindex[jj]  ==0)
                               {
                                  // cell1.SelectedIndex = 0;
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "None";
                               }
                               if (gongxuindex[jj] == 1)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "Processes1";
                               }
                               if (gongxuindex[jj] == 2)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "Processes2";
                               }
                               if (gongxuindex[jj] == 3)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "Processes3";
                               }
                               if (gongxuindex[jj] == 4)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "Processes4";
                               }

                               dataGridVieworder.Rows[jj].Cells[6].Value = "OK";
                               dataGridVieworder.Rows[jj].Cells[8].Value = "OK";                       

                           }
                           else
                           {
                               for (int kk = 2; kk < 6; kk++)
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
                               if (gongxuindex[jj] == 0)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "无";
                               }
                               if (gongxuindex[jj] == 1)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "工序一";
                               }
                               if (gongxuindex[jj] == 2)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "工序二";
                               }
                               if (gongxuindex[jj] == 3)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "工序三";
                               }
                               if (gongxuindex[jj] == 4)
                               {
                                   dataGridVieworder.Rows[jj].Cells[7].Value = "工序四";
                               }

                               dataGridVieworder.Rows[jj].Cells[6].Value ="确定";
                               dataGridVieworder.Rows[jj].Cells[8].Value = "确定";
                           }
                           orderdatas = "item=" + dataGridVieworder.Rows[jj].Cells[0].Value.ToString();
                           orderdatas = orderdatas + ",magno=" + dataGridVieworder.Rows[jj].Cells[1].Value.ToString();
                           orderdatas = orderdatas + ",fun1=" + dataGridVieworder.Rows[jj].Cells[2].Value.ToString();
                           orderdatas = orderdatas + ",fun2=" + dataGridVieworder.Rows[jj].Cells[3].Value.ToString();
                           orderdatas = orderdatas + ",fun3=" + dataGridVieworder.Rows[jj].Cells[4].Value.ToString();
                           orderdatas = orderdatas + ",fun4=" + dataGridVieworder.Rows[jj].Cells[5].Value.ToString();
                           orderdatas = orderdatas + ",down=" + odersdownflage;
                           orderdatas = orderdatas + ",backfun=" + dataGridVieworder.Rows[jj].Cells[7].Value.ToString();
                           orderdatas = orderdatas + ",backdown=" + odersbackdownflage;
                           orderdatas = orderdatas + ";";
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
                int jj = 0;
                string orderdatas = "";

                for (jj = 0; jj < dataGridVieworder2.Rows.Count; jj++)
                {
                    if (dataGridVieworder2.Rows[jj].Cells[0].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[1].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[2].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[3].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[4].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[5].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[6].Value == null
                        || dataGridVieworder2.Rows[jj].Cells[7].Value == null)
                    {
                        dataGridVieworder2.Rows.RemoveAt(jj);
                    }
                    else
                    {
                        if(dataGridVieworder2.Rows[jj].Cells[0].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[1].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[2].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[3].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[4].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[5].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "" ||
                           dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "" || 
                           dataGridVieworder2.Rows[jj].Cells[0].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[1].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[2].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[3].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[4].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[5].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "null" ||
                           dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "null")
                        {
                            ;
                        }
                        else
                        {
                            if (language == "English")
                            {
                                for (int kk = 2; kk < 6; kk++)
                                {
                                    if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "无")
                                    {
                                        dataGridVieworder2.Rows[jj].Cells[kk].Value = "None";
                                    }
                                    if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "未开始")
                                    {
                                        dataGridVieworder2.Rows[jj].Cells[kk].Value = "NotStarted";
                                    }
                                    if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "进行中")
                                    {
                                        dataGridVieworder2.Rows[jj].Cells[kk].Value = "Processing";
                                    }
                                    if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "完成")
                                    {
                                        dataGridVieworder2.Rows[jj].Cells[kk].Value = "Finish";
                                    }
                                   
                                   
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "未下发")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[6].Value = "NotDownload";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "进行中")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[6].Value = "Processing";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "完成")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[6].Value = "Finish";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "待返修")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[6].Value = "NeedReProcesses";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "无")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[7].Value = "None";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "合格")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[7].Value = "Qualified";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "不合格")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[7].Value = "Unqualified";
                                }
                            }
                            else
                            {
                                for (int kk = 2; kk < 6; kk++)
                                {
                                    if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "None")
                                    {
                                        dataGridVieworder2.Rows[jj].Cells[kk].Value = "无";
                                    }
                                    if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "NotStarted")
                                    {
                                        dataGridVieworder2.Rows[jj].Cells[kk].Value = "未开始";
                                    }
                                    if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "Processing")
                                    {
                                        dataGridVieworder2.Rows[jj].Cells[kk].Value = "进行中";
                                    }
                                    if (dataGridVieworder2.Rows[jj].Cells[kk].Value.ToString() == "Finish")
                                    {
                                        dataGridVieworder2.Rows[jj].Cells[kk].Value = "完成";
                                    }
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "NotDownload")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[6].Value = "未下发";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "Processing")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[6].Value = "进行中";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "Finish")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[6].Value = "完成";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[6].Value.ToString() == "NeedReProcesses")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[6].Value = "待返修";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "None")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[7].Value = "无";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "Qualified")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[7].Value = "合格";
                                }
                                if (dataGridVieworder2.Rows[jj].Cells[7].Value.ToString() == "Unqualified")
                                {
                                    dataGridVieworder2.Rows[jj].Cells[7].Value = "不合格";
                                }

                            }

                            orderdatas = "item=" + dataGridVieworder2.Rows[jj].Cells[0].Value.ToString();
                            orderdatas = orderdatas + ",magno=" + dataGridVieworder2.Rows[jj].Cells[1].Value.ToString();
                            orderdatas = orderdatas + ",fun1=" + dataGridVieworder2.Rows[jj].Cells[2].Value.ToString();
                            orderdatas = orderdatas + ",fun2=" + dataGridVieworder2.Rows[jj].Cells[3].Value.ToString();
                            orderdatas = orderdatas + ",fun3=" + dataGridVieworder2.Rows[jj].Cells[4].Value.ToString();
                            orderdatas = orderdatas + ",fun4=" + dataGridVieworder2.Rows[jj].Cells[5].Value.ToString();
                            orderdatas = orderdatas + ",down=" + dataGridVieworder2.Rows[jj].Cells[6].Value.ToString();
                            orderdatas = orderdatas + ",check=" + dataGridVieworder2.Rows[jj].Cells[7].Value.ToString();
                            if (orderdatas=="")
                            {
                                ;
                            }
                            orderdatas = orderdatas + ";";
                            if (orderdatas.Length>40)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            language = ChangeLanguage.GetDefaultLanguage();
            if (!ReProcessChoose)
            {
                form1.Visible = false;
            }
            if (MainForm.Orderinitflage == false)
            {
                MainForm.Orderinitflage=true;
            }
            if (dataGridVieworder != null)
            {
                if (dataGridVieworder.Rows.Count > 1)
                {
                   Orderdatasave(OrderdataFilePath); //20180502
                }
            }
           
            //if(ReProcessChoose == false)
            //{
            //   if(form1.Visible)
            //   {
            //       form1.Visible =false;
            //   }
            //}
            if (dataGridVieworder2 != null)
            {
                if (dataGridVieworder2.Rows.Count > 1)
                {
                    Orderstatesave(OrderstateFilePath); //20180502
                }
            }
            //接收数据，更新数据
            //sptcp1.ReceiveData();
            //linereset = true;//切换产线复位按钮
            //命令码MES_PLC_comfirm，plc的响应码为PLC_MES_respone
            //下指令时，MES_PLC_comfim_write_flage=true,命令码=98，
            //如果MES_PLC_comfim_write_flage==true，那么发送MES_PLC_comfirm，同时发送请求PLC_MES_respone，         
            //如果MES_PLC_comfirm==98&&PLC_MES_respone==98，那么MES_PLC_comfirm=0，
            //如果PLC_MES_respone==0；那么MES_PLC_comfim_write_flage=false；交互完成
            
            //下达命令交互
            MESToPLCcomfirmfun();
            //订单信息Rack_number_comfirm，Order_type_comfirm，PLC响应为Rack_number_respone，Order_type_respone
            //下指令时Rack_number_write_flage= true，Rack_number_comfirm=m，Order_type_comfirm=n
            //如果Rack_number_write_flage==true，那么发送订单信息，同时发送请求订单反馈
            //如果Rack_number_respone=m，那么Rack_number_comfirm=0
            // 如果Order_type_respone=0, 那么Order_type_comfirm=0；
            //Rack_number_respone==0&&Order_type_respone==0；那么Rack_number_write_flage = false；交互完成

            //下达订单交互
            MESToPLCorderfun();
            //机床加工状态信息PLC给MES的命令PLC_MES_comfirm，Rcak_number_comfirm，Result_comfirm，Machine_type_comfirm
            //Mes反馈给机床的信息MES_PLC_response，Rcak_number_response，Result_response，Machine_type_response
            //发送请求加工信息
            //如果PLC_MES_comfirm=202，&&MES_PLC_response==0，PLC_MES_comfim_req_flage=true，
            //如果PLC_MES_comfim_req_flage==true，那么发送MES_PLC_response，
            //那么确认获取机床加工结果。更新机床占用信息，置位MES_PLC_response=202，plc方自行清除PLC_MES_comfirm
            //如果PLC_MES_comfirm=0，那么MES_PLC_response= 0，发送MES_PLC_response，置位PLC_MES_comfim_back_flage，
            //如果PLC_MES_comfim_back_flage == true，那么请求MES_PLC_response
            //如果请求到的MES_PLC_response=0，那么PLC_MES_comfim_req_flage=false，PLC_MES_comfim_back_flage=false，交互完成
            //请求加工信息交互
            PLCToMEScncfun();//车床完成信号处理

            PLCToMEScncfun2();//铣床完成信号处理

            PLCToMEScncfun3();//铣床测量信号处理

       
            RackmessageSync();//同步料仓数据

            //请求料架有无料信息
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm, 10);//查询plc_mes的命令
            MainForm.sptcp1.ReceiveData();
            MainForm.sptcp1.SendData((byte)SCADA.ModbusTcp.Func_Code.req, 1, 0, (int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_status, 28);//请求61、62号寄存器存储内容是料位有无料信息      
            MainForm.sptcp1.ReceiveData();
        }
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
                if (dataGridVieworder.Rows[ii].Cells[1].Value.ToString() == magno.ToString() 
                    &&
                    (dataGridVieworder2.Rows[ii].Cells[6].Value.ToString()=="进行中"
                ||dataGridVieworder2.Rows[ii].Cells[6].Value.ToString() == "Processing"
                ||dataGridVieworder2.Rows[ii].Cells[6].Value.ToString() == "返修中"
                || dataGridVieworder2.Rows[ii].Cells[6].Value.ToString() == "ReProcessing"))
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
        public int getnextorderfun(int magno, ref int curfun)//获取下一工序内容，
        {
            int index = -1;
            int nextfun = -2;
            index = getmagnorowindex(magno);
            maginderx = index;
            if (index == -1)
            {
                return -1;
            }
            if (curfun == 4)
            {
                return 0;//所有工序完成，没有下一工序
            }
            for (int ii = curfun; ii < 5; ii++)
            {
                if (nextfun == -2)
                {
                    if (language == "English")
                    {
                        if (dataGridVieworder2.Rows[index].Cells[1 + ii].Value.ToString() == "NotStarted")
                        {
                            if (dataGridVieworder.Rows[index].Cells[1 + ii].Value.ToString() == "Lathe")
                            {
                                nextfun = 1;
                                curfun = ii;
                            }
                            else if (dataGridVieworder.Rows[index].Cells[1 + ii].Value.ToString() == "CNC")
                            {
                                nextfun = 2;
                                curfun = ii;
                            }
                            else
                            {
                                nextfun = -1;
                            }
                        }
                    }
                    else
                    {
                        if (dataGridVieworder2.Rows[index].Cells[1 + ii].Value.ToString() == "未开始")
                        {
                            if (dataGridVieworder.Rows[index].Cells[1 + ii].Value.ToString() == "车工序")
                            {
                                nextfun = 1;
                                curfun = ii;
                            }
                            else if (dataGridVieworder.Rows[index].Cells[1 + ii].Value.ToString() == "铣工序")
                            {
                                nextfun = 2;
                                curfun = ii;
                            }
                            else
                            {
                                nextfun = -1;
                            }
                        }
                    }
                   
                }
            }
            if (nextfun==-2)
            {
                nextfun=0;
            }
            return nextfun;
        }
         //测量返回值:true:值合法；false:无效
        public static bool checkedInputValue(double value,double[] arry)
        {
            double refer = arry[0];
            double max = arry[1];
            double min = arry[2];

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
                 while (line != null&&line!="")
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

        public void UpdateMeasureRes()  //测量合格、不合格
        {
           if (!measureenable)
            {
                return;
            }
           measureenable = false;
            string key = "MacroVariables:USER";

         //   int magcheckstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Check;//零件不合格标识
         //   int maglength = (int)ModbusTcp.MagLength;
            int i = MainForm.cnclist[1].MagNo;
          //  int magchecki = magcheckstart + maglength * (i - 1);
        //    int magchecki = MainForm.Mag_Check[i - 1];
        
            double[] valuearry = new double[6];
            if( MainForm.cnclist[1].MagNo<1)
            {
                return;
            }

            getrefvalue(MetersetFilePath, ref refvalue);//获取参考值

            int ret0 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeasureForm.MEASURE_VALUE0, ref valuestrarry[0]);
            int ret1 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeasureForm.MEASURE_VALUE1, ref valuestrarry[1]);
            int ret2 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeasureForm.MEASURE_VALUE2, ref valuestrarry[2]);
            int ret3 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeasureForm.MEASURE_VALUE3, ref valuestrarry[3]);
            int ret4 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeasureForm.MEASURE_VALUE4, ref valuestrarry[4]);
            int ret5 = MacDataService.GetInstance().GetHashKeyValueString(MainForm.cnclist[1].dbNo, key, MeasureForm.MEASURE_VALUE5, ref valuestrarry[5]);

            if (ret0 == -1 || ret1 == -1 || ret2 == -1 || ret3 == -1 || ret5 == -1 || ret4 == -1)
            {
                return ;
            }

            for (int ii = 0; ii < 6; ii++)
            {
                string str = valuestrarry[ii];
                if (str.Length > 0)
                {
                    int strStart = str.IndexOf("f\":");
                    int len = str.IndexOf(",", strStart + 3) - (strStart + 3);
                    string strTmp = str.Substring(strStart + 3, len);
                    resvalue[ii] = Convert.ToDouble(strTmp);

                    double[] arrytemp = new double[3];
                    arrytemp[0]=refvalue[ii,0];
                    arrytemp[1]=refvalue[ii,1];
                    arrytemp[2]=refvalue[ii,2];

                    if (checkedInputValue(resvalue[ii], arrytemp))
                    {
                        valueb[ii] = true;//检测合格       
                        
                    }
                    else
                    {
                        valueb[ii]= false;//检测不合格    
                         
                    }
                }
                else
                {


                }
            }
            if (valueb[0] && valueb[1] && valueb[2] && valueb[3] && valueb[4] && valueb[5] )
            {
                bIsOK = true;
               // ModbusTcp.DataMoubus[magchecki] = 0;//检测合格 
                MainForm.Mag_Check[i - 1] = 0;
                MeterlogFileadd(MetetrrecordFilePath);
                renewMeterDGV2 = true;
                MeterForm.Textmagno = MainForm.cnclist[1].MagNo;
                
            }
            else
            {
                bIsOK = false;
                //ModbusTcp.DataMoubus[magchecki] = 1;//检测不合格 
                MainForm.Mag_Check[i - 1] = 1;
                MeterlogFileadd(MetetrrecordFilePath);

                renewMeterDGV2 = true;
                MeterForm.Textmagno = MainForm.cnclist[1].MagNo;
                
            }
             
        }
        private void MeterlogFileadd(string path)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Append);
                StreamWriter sr = new StreamWriter(aFile);//追加文件
                string timenow = "";
                string newlog = "";

                string res ="";
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
                    else  res = "No";
                }
                string magno = MainForm.cnclist[1].MagNo.ToString();
                 timenow = DateTime.Now.ToString();
                string ref1 = refvalue[0,0].ToString();
                string res1 = resvalue[0].ToString();
                string ref2 = refvalue[1,0].ToString();
                string res2 = resvalue[1].ToString();
                string ref3 = refvalue[2,0].ToString();
                string res3 = resvalue[2].ToString();
                string ref4 = refvalue[3,0].ToString();
                string res4 = resvalue[3].ToString();
                string ref5 = refvalue[4,0].ToString();
                string  res5 = resvalue[4].ToString();
                string ref6 = refvalue[5,0].ToString();
                string res6 = resvalue[5].ToString();
      
                newlog ="magno=" + magno+",res=" + res + ",timenow=" + timenow + ",ref1=" + ref1 + ",res1=" + res1 + ",ref2="+ ref2 + ",res2=" + res2
                    + ",ref3=" + ref3 + ",res3=" + res3 + ",ref4=" + ref4 + ",res4=" + res4 + ",ref5=" + ref5 + ",res5=" + res5 + ",ref6=" + ref6 + ",res6=" + res6+ ",";
              
                sr.WriteLine(newlog);
                sr.Close();

                aFile.Close();
                MeterlogFiledelet(path);
                renewMeterRecordGridview = true;
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
                List<string> lines= new List<string>(File.ReadAllLines(path));
                int linecount = lines.Count();
                if (linecount > 20)
                {
                    for (int i = 0; i < (linecount - 20); i++)
                    {
                        lines.RemoveAt(i);
                    }
                    File.WriteAllLines(path,lines.ToArray());
                   
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
           
            if(RackForm.setrfidflag)//mes整体料仓信息写给plc
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
                        int  maglength = (int)ModbusTcp.MagLength;
                        int startreg = (int)ModbusTcp.DataConfigArr.Mag_Scene + ii * maglength;
                        MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg,startreg, maglength, 1, 0);//给plc单个仓位信息
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
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 3, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();

                Thread.Sleep(20);
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 3);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();

                Thread.Sleep(20);
                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] == ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm])
                {
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_comfirm] = 0;
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
        public static  void RebackOrder()
        {
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_comfirm] = 0;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_respone] = 0;
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 3, 1, 0);//给plc写订单信息
            MainForm.sptcp1.ReceiveData();

            Thread.Sleep(2);
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 3, 1, 0);//给plc写订单信息
            MainForm.sptcp1.ReceiveData();
            Thread.Sleep(2);

            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.PLC_MES_respone, 3, 1, 0);//给plc写订单信息
            MainForm.sptcp1.ReceiveData();

            Thread.Sleep(2);

            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.PLC_MES_respone,3, 1, 0);//给plc写订单信息
            MainForm.sptcp1.ReceiveData();
            return; 
        }
        private void MESToPLCcomfirmfun()
        {
            if (ModbusTcp.MES_PLC_comfim_write_flage == true)
            {
                string temp = "";
                ModbusTcp.MES_PLC_comfim_write_count++;
                if (timer1.Interval * ModbusTcp.MES_PLC_comfim_write_count > 60 * 1000 * 2)//指令下达交互时间2min无回应，报错
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
            //MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0,(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm, 4);//查询plc_mes的命令
            //MainForm.sptcp1.ReceiveData();

            //Thread.Sleep(2);
            //MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm, 4);//查询plc_mes的命令
            //MainForm.sptcp1.ReceiveData();
            //}
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm] == (int)ModbusTcp.MesResponseToPlc.ResMachining //plc202指令
                && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] == 0//mes响应为0
                && ModbusTcp.PLC_MES_comfim_req_flage ==false)//互换标识为false
            {
                //int cncno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_comfirm];//获取完成信号绑定的机床编号
                //int cncmagno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_comfirm];//获取完成信号绑定的料仓号
                //if (cncno <= 0 || cncmagno <= 0)
                //{

                //    //ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm] = 0;
                //    //ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                //    if (language == "English")
                //    {
                //        MessageBox.Show("Mag message is err");
                //    }
                //    else MessageBox.Show("机床与料仓信息不符合,加工完成信号传输失败");
                //    ModbusTcp.PLC_MES_comfim_req_flage = false;
                //    return;
                //}
                int cncmagno = MainForm.cnclist[0].MagNo;
                if (cncmagno == 0)
                {
                    return;
                }
                ModbusTcp.PLC_MES_comfim_req_flage = true;//置位互换标识，开启互换
               //一号机床车床，2号机床加工中心
               
               // if (cncmagno == MainForm.cnclist[cncno - 1].MagNo)
                //{
                   //MainForm.cnclist[0].MagNo = 0; // 更新机床完成信号
                   
                   ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm];//置位MES响应信号
               
                //  int magfunstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Fun_cur;//零件类型
                 int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
                 int maglength = (int)ModbusTcp.MagLength;
                // int magfuni = magfunstart + maglength * (cncmagno - 1);
                 //  ModbusTcp.DataMoubus[magfuni];//当前料为正在执行的工序号
                 int curfuntemp = MainForm.Mag_Fun_cur[cncmagno - 1];//  ModbusTcp.DataMoubus[magfuni];//当前料为正在执行的工序号
                 int magstatei = magstatestart + maglength * (cncmagno - 1);//料仓状态
                 int index = getmagnorowindex(cncmagno);

                 dataGridVieworder2.Rows[index].Cells[1 + curfuntemp].Value = "完成";//更新当前工序的加工状态20180611此处curfuntemp有问题，需修正

                 nextstep1 = getnextorderfun(cncmagno, ref curfuntemp);//获取下一道工序的内容和工序编号，静态
                 if (dataGridVieworder2.Rows[index].Cells[6].Value.ToString() == "返修中")
                {
                    nextstep1 = 0;
                }
                 curfunnext1 = curfuntemp;//下一步工序编号
                 magnumber = cncmagno;//获取料仓编号，静态，仓位编号


            }
            if (ModbusTcp.PLC_MES_comfim_req_flage == true)
            {
                ModbusTcp.PLC_MES_comfim_req_count++;
                //if (timermodbus.Interval * ModbusTcp.PLC_MES_comfim_req_count > 60 * 1000)//指令下达交互时间1min无回应，报错
                //{
                //    MessageBox.Show("请求超时！");
                //    ModbusTcp.PLC_MES_comfim_req_flage = false;
                //    ModbusTcp.PLC_MES_comfim_req_count = 0;
                //    return;
                //}
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response, 4, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();


                Thread.Sleep(2);
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm, 4);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();


                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm] ==0)
                {
                     ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] =0;//MES响应信号清零
                   ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response] = 0;//MES响应信号清零
                   ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_response] = 0;//MES响应信号清零
                   MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response, 4, 1, 0);//给plc写订单信息
                   MainForm.sptcp1.ReceiveData();
                  if(ModbusTcp.PLC_MES_comfim_back_flage==false)
                    {
                        ModbusTcp.PLC_MES_comfim_back_flage = true;//置位请求plc响应信号
                    }
                }
                if(ModbusTcp.PLC_MES_comfim_back_flage==true)
                {

                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response, 4);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();

                    Thread.Sleep(2);
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response, 4);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();


                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm] == 0
                        && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response] == 0
                        && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_response] == 0
                    &&ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] == 0)
                    {
                        ModbusTcp.PLC_MES_comfim_req_flage = false;
                        ModbusTcp.PLC_MES_comfim_back_flage = false;

                     //   int magfunstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Fun_cur;//零件类型
                        int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
                        int maglength = (int)ModbusTcp.MagLength;
                      //  int magfuni = magfunstart + maglength * (magnumber - 1);
                        int curfun = MainForm.Mag_Fun_cur[magnumber - 1];//当前正在行进的机床号
                       // int curfun = ModbusTcp.DataMoubus[magfuni];//当前料为正在执行的工序号
                        int magstatei = magstatestart + maglength * (magnumber - 1);//料仓状态
                        int index = getmagnorowindex(magnumber);
                        if (nextstep1 == 0)//工序完成
                        {
                            string cncmagnos = magnumber.ToString();//仓位编号
                            string temp = "";

                            //temp = cncmagnos + "号料" + "车削工序加工完成";
                            //MessageBox.Show(temp);

                           ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;

                           if (language == "English")
                           {
                               dataGridVieworder2.Rows[index].Cells[6].Value = "Finish";
                               temp = cncmagnos + "is Finish";
                           }
                           else
                           {
                               dataGridVieworder2.Rows[index].Cells[6].Value = "完成";
                               temp = cncmagnos + "号料加工完成";
                           }

                           MainForm.cnclist[0].MagNo = 0; // 更新机床完成信号
                            MessageBox.Show(temp);
                        }
                        else  if (nextstep1  ==1)//下一步工序是车
                        {
                            string cncmagnos = magnumber.ToString();
                            string temp = "";
                            if (cnc0orderdown(magnumber))
                            {
                                MainForm.Mag_Fun_cur[magnumber - 1] = curfunnext1;//当前正在执行的工序编号
                               // ModbusTcp.DataMoubus[magfuni] = curfunnext1;
                                if (language == "English")
                                {
                                    dataGridVieworder2.Rows[index].Cells[1 + curfunnext1].Value = "Processing";
                                    temp = cncmagnos + "'s Lathe process is compelet";
                                }
                                else
                                {
                                    dataGridVieworder2.Rows[index].Cells[1 + curfunnext1].Value = "进行中";
                                    temp = cncmagnos + "号料" + "车削工序加工完成";
                                }


                                MainForm.cnclist[0].MagNo = 0; // 更新机床完成信号
                                MessageBox.Show(temp);
                            }
                        }
                        else if (nextstep1 == 2)//下一步工序是铣
                        {
                            string cncmagnos = magnumber.ToString();
                            string temp = "";
                            if (cnc1orderdown(magnumber))
                            {
                                MainForm.Mag_Fun_cur[magnumber - 1] = curfunnext1;//当前正在执行的工序编号
                               // ModbusTcp.DataMoubus[magfuni] = curfunnext1;
                                if (language == "English")
                                {
                                    dataGridVieworder2.Rows[index].Cells[1 + curfunnext1].Value = "Processing";
                                    temp = cncmagnos + "'s Lathe process is compelet";
                                }
                                else
                                {
                                    dataGridVieworder2.Rows[index].Cells[1 + curfunnext1].Value = "进行中";
                                    temp = cncmagnos + "号料" + "车削工序加工完成";
                                }

                                MainForm.cnclist[0].MagNo = 0; // 更新机床完成信号
                                MessageBox.Show(temp);

                            }
                        }

                    }
                }               
            }

        }
        private void PLCToMEScncfun2()
        {
            //MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2, 4);//查询plc_mes的命令
            //MainForm.sptcp1.ReceiveData();

            //Thread.Sleep(2);
            //MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2, 4);//查询plc_mes的命令
            //MainForm.sptcp1.ReceiveData();
            //}



            //measureenable = true;//置位测量使能信号20180502
            //UpdateMeasureRes();//更新测量结果20180502

            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == (int)ModbusTcp.MesResponseToPlc.ResMachining //plc202指令
                && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] == 0//mes响应为0
                && ModbusTcp.PLC_MES_comfim_req_flage_2 == false)//互换标识为false
            {
                //int cncno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_comfirm_2];//获取完成信号绑定的机床编号
                //int cncmagno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_comfirm_2];//获取完成信号绑定的料仓号
                //if (cncno <= 0 || cncmagno <= 0)
                //{

                //    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm] = 0;
                //    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                //    if (language == "English")
                //    {
                //        MessageBox.Show("Mag message is err"); 
                //    }
                //    else  MessageBox.Show("机床与料仓信息不符合,加工完成信号传输失败");
                //    ModbusTcp.PLC_MES_comfim_req_flage_2 = false;
                //    return;
                //}

                int cncmagno = MainForm.cnclist[1].MagNo;
                int cncno = 2;
                if (cncmagno == 0)
                {
                    return;
                }
                ModbusTcp.PLC_MES_comfim_req_flage_2 = true;//置位互换标识，开启互换
                //一号机床车床，2号机床加工中心

                // if (cncmagno == MainForm.cnclist[cncno - 1].MagNo)
                //{
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2];//置位MES响应信号
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_comfirm_2];//置位MES响应信号
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_comfirm_2];//置位MES响应信号

                string cncmagnos = cncmagno.ToString();
                string cncnos = cncno.ToString();
                string temp = "";
                int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//料仓状态     
                int maglength = (int)ModbusTcp.MagLength;
                int curfuntemp = MainForm.Mag_Fun_cur[magnumber - 1] ;//当前正在执行的工序编号
                int magstatei = magstatestart + maglength * (cncmagno - 1);//料仓状态
                int index = getmagnorowindex(cncmagno);
                if (language == "English")
                {
                    dataGridVieworder2.Rows[index].Cells[1 + curfuntemp].Value = "Finish";//更新当前工序的加工状态

                    nextstep2 = getnextorderfun(cncmagno, ref curfuntemp);//获取下一道工序的内容和工序编号，静态
                    //if (dataGridVieworder2.Rows[index].Cells[6].Value.ToString() == "ReProcessing")
                    //{
                    //    nextstep2 = 0;
                    //}
                }
                else
                {
                    dataGridVieworder2.Rows[index].Cells[1 + curfuntemp].Value = "完成";//更新当前工序的加工状态

                    nextstep2 = getnextorderfun(cncmagno, ref curfuntemp);//获取下一道工序的内容和工序编号，静态
                    //if (dataGridVieworder2.Rows[index].Cells[6].Value.ToString() == "返修中")
                    //{
                    //    nextstep2 = 0;
                    //}
                }
               
                curfunnext2 = curfuntemp;
                magnumber = cncmagno;//获取料仓编号，静态
            }
            if (ModbusTcp.PLC_MES_comfim_req_flage_2 == true)
            {
                ModbusTcp.PLC_MES_comfim_req_count_2++;
                //if (timermodbus.Interval * ModbusTcp.PLC_MES_comfim_req_count > 60 * 1000)//指令下达交互时间1min无回应，报错
                //{
                //    MessageBox.Show("请求超时！");
                //    ModbusTcp.PLC_MES_comfim_req_flage = false;
                //    ModbusTcp.PLC_MES_comfim_req_count = 0;
                //    return;
                //}
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
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_response_2] = 0;//MES响应信号清零
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
                        int magstatei = magstatestart + maglength * (magnumber - 1);//料仓状态
                          int magchecki = MainForm.Mag_Check[magnumber - 1];
                        int index = getmagnorowindex(magnumber);
                        if (nextstep2 == 0)//工序完成
                        {
                            string cncmagnos = magnumber.ToString();
                            string temp = "";
                            if (language == "English")
                            {
                                dataGridVieworder2.Rows[index].Cells[6].Value = "Finish";
                                temp = cncmagnos + "'s cnc processes is compelet";
                                MessageBox.Show(temp);
                                if (magchecki == 1)//不合格
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishNotStandard;
                                    temp = cncmagnos + "is finish and unqualified";
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "Unqualified";
                                    dataGridVieworder2.Rows[index].Cells[6].Value = "NeedReProcesses";
                                }
                                else
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;
                                    temp = cncmagnos + "is finish and qualified";
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "Qualified";
                                    dataGridVieworder2.Rows[index].Cells[6].Value = "Finish";
                                }
                            }
                            else
                            {
                                dataGridVieworder2.Rows[index].Cells[6].Value = "完成";
                                temp = cncmagnos + "号料" + "铣削工序加工完成";
                                MessageBox.Show(temp);
                                if (magchecki == 1)//不合格
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishNotStandard;
                                    temp = cncmagnos + "号料加工完成" + "工件测量不合格";
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "不合格";
                                    dataGridVieworder2.Rows[index].Cells[6].Value = "待返修";
                                }
                                else
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;
                                    temp = cncmagnos + "号料加工完成" + "工件测量合格";
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "合格";
                                    dataGridVieworder2.Rows[index].Cells[6].Value = "完成";
                                }
                            }
                            

                            MainForm.cnclist[1].MagNo = 0; // 更新机床完成信号
                            MessageBox.Show(temp);
                        }  
                        else   if (nextstep2 == 1)//下一步工序是车
                        {
                            string cncmagnos = magnumber.ToString();
                            string temp = "";
                            //temp = cncmagnos + "号料" + "铣削工序加工完成";
                            if (language == "English")
                            {
                                if (ModbusTcp.DataMoubus[magchecki] == 1)//不合格
                                {
                                    temp = cncmagnos + "'s cnc processes is finish and unqualified";
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishNotStandard;
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "Unqualified";

                                    if (cnc0orderdown(magnumber))
                                    {
                                      //  ModbusTcp.DataMoubus[magfuni] = curfunnext2;
                                        MainForm.Mag_Fun_cur[magnumber - 1] = curfunnext2;
                                        dataGridVieworder2.Rows[index].Cells[1 + curfunnext2].Value = "Processing";
                                    }

                                }
                                else
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;

                                    temp = cncmagnos + "'s cnc processes is finish and qualified";
                                    if (cnc0orderdown(magnumber))
                                    {
                                       MainForm.Mag_Fun_cur[magnumber - 1] = curfunnext2;
                                    //    ModbusTcp.DataMoubus[magfuni] = curfunnext2;
                                        dataGridVieworder2.Rows[index].Cells[1 + curfunnext2].Value = "Processing";
                                        dataGridVieworder2.Rows[index].Cells[7].Value = "Qualified";
                                    }
                                }
                            }
                            else
                            {
                                if (magchecki == 1)//不合格
                                {
                                    temp = cncmagnos + "号料铣削工序加工完成" + ",测量不合格";
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishNotStandard;
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "不合格";

                                    if (cnc0orderdown(magnumber))
                                    {
                                        MainForm.Mag_Fun_cur[magnumber - 1] = curfunnext2;
                                        dataGridVieworder2.Rows[index].Cells[1 + curfunnext2].Value = "进行中";
                                    }

                                }
                                else
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;

                                    temp = cncmagnos + "号料铣削工序加工完成" + ",测量合格";
                                    if (cnc0orderdown(magnumber))
                                    {
                                        MainForm.Mag_Fun_cur[magnumber - 1] = curfunnext2;
                                        dataGridVieworder2.Rows[index].Cells[1 + curfunnext2].Value = "进行中";
                                        dataGridVieworder2.Rows[index].Cells[7].Value = "合格";
                                    }
                                }
                            }
                            
                           /////////////
                            MainForm.cnclist[1].MagNo = 0; // 更新机床完成信号
                            MessageBox.Show(temp);
                        }
                        else if (nextstep2 == 2)//下一步工序是铣
                        {
                            string cncmagnos = magnumber.ToString();
                            string temp = "";
                            if (language == "English")
                            {
                                if (magchecki == 1)//不合格
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishNotStandard;
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "Unqualified";
                                    temp = cncmagnos + "'s cnc processes is finish and unqualified";

                                }
                                else
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "Qualified";
                                    temp = cncmagnos + "'s cnc processes is finish and qualified";

                                }
                                /////////////
                                if (cnc1orderdown(magnumber))
                                {
                                    MainForm.Mag_Fun_cur[magnumber - 1] = curfunnext2;
                                    dataGridVieworder2.Rows[index].Cells[1 + curfunnext2].Value = "Processing";

                                }
                            }
                            else
                            {
                                if (magchecki == 1)//不合格
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishNotStandard;
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "不合格";
                                    temp = cncmagnos + "号料铣削工序加工完成" + "工件测量不合格";

                                }
                                else
                                {
                                    ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFinishStandard;
                                    dataGridVieworder2.Rows[index].Cells[7].Value = "合格";
                                    temp = cncmagnos + "号铣削工序加工工完成" + "工件测量合格";

                                }
                                /////////////
                                if (cnc1orderdown(magnumber))
                                {
                                    MainForm.Mag_Fun_cur[magnumber - 1] = curfunnext2;
                                    dataGridVieworder2.Rows[index].Cells[1 + curfunnext2].Value = "进行中";

                                }
                            }
                            
                            MainForm.cnclist[1].MagNo = 0; // 更新机床完成信号
                            MessageBox.Show(temp);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// plc给mes测量请求信号
        /// </summary>
        private void PLCToMEScncfun3()
        {
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == (int)ModbusTcp.MesResponseToPlc.ResMeterReq //plc202指令
                && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] == 0//mes响应为0
                && ModbusTcp.PLC_MES_comfim_req_meter == false)//互换标识为false
            {
                int cncno = 2;

                int cncmagno = MainForm.cnclist[1].MagNo;
                if (cncmagno == 0)
                {
                   return ;
                }
               // int cncmagno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_comfirm_2];//获取完成信号绑定的料仓号
                magnumber = cncmagno;
                ModbusTcp.PLC_MES_comfim_req_meter = true;//置位互换标识，开启互换
                //一号机床车床，2号机床加工中心

                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2];//置位MES响应信号

                string cncmagnos = cncmagno.ToString();
                string cncnos = cncno.ToString();
                int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//料仓状态     
                int maglength = (int)ModbusTcp.MagLength;
                int curfuntemp = MainForm.Mag_Fun_cur[cncmagno - 1];//当前正在执行的工序编号
                int magstatei = magstatestart + maglength * (cncmagno - 1);//料仓状态
                int index = getmagnorowindex(cncmagno);
                measureenable = true;//置位测量使能信号
                UpdateMeasureRes();//更新测量结果
            }
            if (ModbusTcp.PLC_MES_comfim_req_meter == true)
            {
              
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4, 1, 0);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();
                Thread.Sleep(2);

                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2, 4);//给plc写订单信息
                MainForm.sptcp1.ReceiveData();
                Thread.Sleep(2);

                if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == 0)
                {
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = 0;//MES响应信号清零
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response_2] = 0;//MES响应信号清零
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_response_2] = 0;//MES响应信号清零
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4, 1, 0);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();
                    if (ModbusTcp.PLC_MES_comfim_back_meter == false)
                    {
                        ModbusTcp.PLC_MES_comfim_back_meter = true;//置位请求plc响应信号
                    }
                }
                if (ModbusTcp.PLC_MES_comfim_back_meter == true)
                {

                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();

                    Thread.Sleep(2);
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2, 4);//给plc写订单信息
                    MainForm.sptcp1.ReceiveData();


                    if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.PLC_MES_comfirm_2] == 0
                    && ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] == 0)
                    {
                        ModbusTcp.PLC_MES_comfim_req_meter = false;
                        ModbusTcp.PLC_MES_comfim_back_meter = false;

                        int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型       
                        int maglength = (int)ModbusTcp.MagLength;
                        int magstatei = magstatestart + maglength * (magnumber - 1);//料仓状态
                        int magchecki = MainForm.Mag_Check[magnumber - 1];
                        int index = getmagnorowindex(magnumber);

                        if (language == "English")
                        {
                            if (magchecki == 1)//不合格
                            {
                                dataGridVieworder2.Rows[index].Cells[7].Value = "Unqualified";
                                ShowForm.messagestring = "NO." + magnumber.ToString() + "is unqualified ,Please choose next step !";

                                //生成测量结果提示和选择
                                ReProcessChoose = true;
                                if (form1==null)
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
                                dataGridVieworder2.Rows[index].Cells[7].Value = "Qualified";
                                ShowForm.messagestring = "NO." + magnumber.ToString() + "is qualified  ,Please choose next step !";

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
                                ShowForm.messagestring = magnumber.ToString() + "仓位工件不合格，请选择下一步 !";

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
                                dataGridVieworder2.Rows[index].Cells[7].Value = "合格";
                                ShowForm.messagestring =  magnumber.ToString() + "仓位工件合格，请选择下一步  !";
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
                    }
                }
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

        private void textBoxorderno_Leave(object sender, EventArgs e)
        {
            string OrderNoStr = ((TextBox)sender).Text;
            if (OrderNoStr=="")
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

        private void button1_Click(object sender, EventArgs e)
        {

            string OrderNoStr = textBoxorderno.Text;
            if (OrderNoStr == "")
            {return ;
            }
            int OrderNo = Convert.ToInt16(OrderNoStr)-1;
            if (OrderNo > dataGridVieworder.RowCount-1)
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
               
                if (dataGridVieworder2.Rows[OrderNo].Cells[6].Value.ToString() == "进行中"||
                    dataGridVieworder2.Rows[OrderNo].Cells[6].Value.ToString() == "Processing")
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
                    for (int jj = 0; jj < dataGridVieworder.Rows.Count ; jj++)
                    {
                        itemcount = jj + 1;
                        dataGridVieworder.Rows[jj].Cells[0].Value = itemcount.ToString();
                      
                    }
                    for (int jj = 0; jj < dataGridVieworder2.Rows.Count; jj++)
                    {
                        itemcount = jj + 1;
                        dataGridVieworder2.Rows[jj].Cells[0].Value = itemcount.ToString();

                    }

                }
            }   
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox1_Leave(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            string OrderNoStr = textBox1.Text;
            if (OrderNoStr=="")
            {
               return ;
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
                if (dataGridVieworder2.Rows[OrderNo-1].Cells[6].Value.ToString() == "进行中" ||
                    dataGridVieworder2.Rows[OrderNo - 1].Cells[6].Value.ToString() == "Processing")
                {
                    string magnos = dataGridVieworder2.Rows[OrderNo-1].Cells[1].Value.ToString();
                    int magoi = Convert.ToInt16(magnos);
                    int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
  
                    int maglength = (int)ModbusTcp.MagLength;

                     int magstatei = magstart + maglength * (magoi-1);

                     if (magoi == MainForm.cnclist[0].MagNo)//车床
                    {
                        MainForm.cnclist[0].MagNo = 0;
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFailure ;//加工异常
                        if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_comfirm] == OrderNo)
                        {

                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_comfirm] = 0;
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;
                        }
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_response] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response] = 0;
               
                        if (language == "English")
                        {
                            MessageBox.Show("The cnc1 process call back");
                        }
                        else
                            MessageBox.Show("车床工序取消");
                    }
                     if (magoi == MainForm.cnclist[1].MagNo)//车床
                    {
                        MainForm.cnclist[1].MagNo = 0;
                        ModbusTcp.DataMoubus[magstatei] = (int)ModbusTcp.Mag_state_config.StateFailure; ;//加工异常
                        if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_comfirm] == OrderNo)
                        {

                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_comfirm] = 0;
                            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;
                        }
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response_2] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_number_response_2] = 0;
                        ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response_2] = 0;
                
                        if (language == "English")
                        {
                            MessageBox.Show("The cnc2 process call back");
                        }
                        else
                            MessageBox.Show("加工中心工序取消");
                    }
                     if (magoi == MainForm.cnclist[0].MagNo)//车床
                     {
                         MainForm.cnclist[0].MagNo = 0;
                     }
                     if (magoi == MainForm.cnclist[1].MagNo)//车床
                     {
                         MainForm.cnclist[1].MagNo = 0; ;
                     }

                     if (language == "English")
                     {
                         dataGridVieworder2.Rows[OrderNo - 1].Cells[6].Value = "Reback";
                     }
                    else dataGridVieworder2.Rows[OrderNo-1].Cells[6].Value = "撤销";
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

   
    }
}
