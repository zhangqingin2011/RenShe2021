﻿using System;
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

namespace SCADA
{
    public partial class MeterSetForm : Form
    {

        AutoSizeFormClass aotosize = new AutoSizeFormClass();
        public static String MetersetFilePath1_1 = "..\\data\\measure\\MeterSetFile1_1";
        public static String MetersetFilePath1_2 = "..\\data\\measure\\MeterSetFile1_2";
        public static String MetersetFilePath1_3 = "..\\data\\measure\\MeterSetFile1_3";
        public static String MetersetFilePath1_4 = "..\\data\\measure\\MeterSetFile1_4";
        public static String MetersetFilePath2_1 = "..\\data\\measure\\MeterSetFile2_1";
        public static String MetersetFilePath2_2 = "..\\data\\measure\\MeterSetFile2_2";
        public static String MetersetFilePath2_3 = "..\\data\\measure\\MeterSetFile2_3";
        public static String MetersetFilePath2_4 = "..\\data\\measure\\MeterSetFile2_4";
        public static String MetersetFilePath3_1 = "..\\data\\measure\\MeterSetFile3_1";
        public static String MetersetFilePath3_2 = "..\\data\\measure\\MeterSetFile3_2";
        public static String MetersetFilePath3_3 = "..\\data\\measure\\MeterSetFile3_3";
        public static String MetersetFilePath3_4 = "..\\data\\measure\\MeterSetFile3_4";
        public static String MetersetFilePath4_1 = "..\\data\\measure\\MeterSetFile4_1";
        public static String MetersetFilePath4_2 = "..\\data\\measure\\MeterSetFile4_2";
        public static String MetersetFilePath4_3 = "..\\data\\measure\\MeterSetFile4_3";
        public static String MetersetFilePath4_4 = "..\\data\\measure\\MeterSetFile4_4";

        public DataGridViewTextBoxEditingControl CellEdit = null;
        public static int refreshmeterdata = -1;//1,A类型表格参数变更，2-B，3-C，4-D
        public string language = "";
        private string prelanguage; //记录切换语言之前的语言
        Hashtable m_Hashtable;
        private int MeterModeIndex = 0;
        public MeterSetForm()
        {
            InitializeComponent();
            comboBoxModeSel.SelectedIndex = 0;

            initdataGridView(MetersetFilePath1_1, dataGridView1);
            initdataGridView(MetersetFilePath1_2, dataGridView2);
            initdataGridView(MetersetFilePath1_3, dataGridView3);
            initdataGridView(MetersetFilePath1_4, dataGridView4);

        }

        private void buttonsub1_Click(object sender, EventArgs e)
        {
            if(comboBoxModeSel.SelectedIndex == 0 )
            {
                meterdatasave(MetersetFilePath1_1, dataGridView1);
                meterdatasave(MetersetFilePath1_2, dataGridView2);
                meterdatasave(MetersetFilePath1_3, dataGridView3);
                meterdatasave(MetersetFilePath1_4, dataGridView4);
            }
            else if(comboBoxModeSel.SelectedIndex == 1)
            {
                meterdatasave(MetersetFilePath2_1, dataGridView1); 
                meterdatasave(MetersetFilePath2_2, dataGridView2);
                meterdatasave(MetersetFilePath2_3, dataGridView3);
                meterdatasave(MetersetFilePath2_4, dataGridView4);
            }
            else if (comboBoxModeSel.SelectedIndex == 2)
            {
                meterdatasave(MetersetFilePath3_1, dataGridView1);
                meterdatasave(MetersetFilePath3_2, dataGridView2);
                meterdatasave(MetersetFilePath3_3, dataGridView3);
                meterdatasave(MetersetFilePath3_4, dataGridView4);
            }
           else if (comboBoxModeSel.SelectedIndex == 3)
            {
                meterdatasave(MetersetFilePath4_1, dataGridView1);
                meterdatasave(MetersetFilePath4_2, dataGridView2);
                meterdatasave(MetersetFilePath4_3, dataGridView3);
                meterdatasave(MetersetFilePath4_4, dataGridView4);
            }
            freshdataGridView(1);
            freshdataGridView(2);
            freshdataGridView(3);
            freshdataGridView(4);
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
        private bool initdataGridView(string path,DataGridView dgv)//文件
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);

                string item = "";
                string type = "";
                string refvalue = "";
                string uppervalue = "";
                string lowervalue = "";
                string toolno = "";
                string comptype = "";
                string line;
                int ii = 0;
                line = sr.ReadLine();
                if (dgv.Rows.Count < 1)
                {

                    dgv.Rows.Add(6);
                    for (int i = 0; i < 6; i++)
                    {
                        string temp = "";
                        int no = 50040 + i;

                        temp = "#" + no.ToString();

                        dgv.Rows[i].Cells[0].Value = temp;
                        dgv.Rows[i].Cells[1].Value = "无";
                        dgv.Rows[i].Cells[2].Value = "0.000";
                        dgv.Rows[i].Cells[3].Value = "0.000";
                        dgv.Rows[i].Cells[4].Value = "0.000";
                        dgv.Rows[i].Cells[5].Value = "0";
                        dgv.Rows[i].Cells[6].Value = "长度";
                    }
                }
                while (line != null)
                {
                    item = getvalueformstring(line, "item=");
                    type = getvalueformstring(line, "type=");
                    refvalue = getvalueformstring(line, "ref=");
                    uppervalue = getvalueformstring(line, "upper=");
                    lowervalue = getvalueformstring(line, "lower=");
                    toolno = getvalueformstring(line, "toolno=");
                    comptype = getvalueformstring(line, "comptype=");
                    if (item != "null")
                    {
                        dgv.Rows[ii].Cells[0].Value = item;
                    }
                    if (type != "null")
                    {
                        if(type == "0")
                        {
                            dgv.Rows[ii].Cells[1].Value = "无";
                        }
                        else if (type == "0")
                        {
                            dgv.Rows[ii].Cells[1].Value = "无";
                        }
                       else if (type == "1")
                        {
                            dgv.Rows[ii].Cells[1].Value = "高度";
                        }
                       else if (type == "2")
                        {
                            dgv.Rows[ii].Cells[1].Value = "外轮廓";
                        }
                        else if (type == "3")
                        {
                            dgv.Rows[ii].Cells[1].Value = "内轮廓";
                        }
                        else dgv.Rows[ii].Cells[1].Value = "无";
                    }
                    if (refvalue != "null")
                    {

                        dgv.Rows[ii].Cells[2].Value = refvalue;
                    }
                    if (uppervalue != "null")
                    {

                        dgv.Rows[ii].Cells[3].Value = uppervalue;
                    }
                    if (lowervalue != "null")
                    {

                        dgv.Rows[ii].Cells[4].Value = lowervalue;
                    }
                    if (toolno != "null")
                    {

                        dgv.Rows[ii].Cells[5].Value = toolno;
                    }
                    if (comptype != "null")
                    {
                         if (comptype == "0")
                        {
                            dgv.Rows[ii].Cells[6].Value = "长度";
                        }
                        else if (comptype == "1")
                        {
                            dgv.Rows[ii].Cells[6].Value = "半径";
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
        private void freshdataGridView(int dgvname)//文件
        {
            int refreshmeterdata = dgvname;
            return ;
        }
        private bool meterdatasave(string path,DataGridView dgv)
        {
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Create);
                StreamWriter sr = new StreamWriter(aFile);
                int jj = 0;
                string type = "";
                double lowerdatad = 0.0;
                double uppervalued = 0.0;
                double refvalued = 0.0;
                string StValues = "";
                string UpValues = "";
                    string LowValues = "";
                string tempvalues = "";
                string temps = "";
                string meterdatas = ""; 
                string toolno = "";
                string comptype = "";
                for (jj = 0; jj < dgv.Rows.Count; jj++)
                {
                    for (int k = 2; k <= 4; k++)
                    {
                        if (dgv.Rows[jj].Cells[k].Value == null)
                        {
                            dgv.Rows[jj].Cells[k].Value = "0.000";
                        }
                        string temp = dgv.Rows[jj].Cells[k].Value.ToString();
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

                                sr.Close();
                                aFile.Close();
                                 MessageBox.Show("数据不合法");
                                return false;
                            }

                            if (pointnum > 1)
                            {
                                sr.Close();
                                aFile.Close();
                               MessageBox.Show("数据不合法");
                                return false;
                            }
                        }
                    }
                    if (dgv.Rows[jj].Cells[1].Value.ToString() =="无")
                    {
                        type = "0";
                    }
                    else if (dgv.Rows[jj].Cells[1].Value.ToString() == "高度")
                    {
                        type = "1";
                    }
                    else if (dgv.Rows[jj].Cells[1].Value.ToString() == "外轮廓")
                    {
                        type = "2";
                    }
                    if (dgv.Rows[jj].Cells[1].Value.ToString() == "内轮廓")
                    {
                        type = "3";
                    }
                    tempvalues = dgv.Rows[jj].Cells[4].Value.ToString();
                    lowerdatad = Convert.ToDouble(tempvalues);
                    LowValues = lowerdatad.ToString("F3");
                    tempvalues = dgv.Rows[jj].Cells[3].Value.ToString();
                    uppervalued = Convert.ToDouble(tempvalues);
                    UpValues = uppervalued.ToString("F3");
                    tempvalues = dgv.Rows[jj].Cells[2].Value.ToString();
                    refvalued = Convert.ToDouble(tempvalues);
                    StValues = refvalued.ToString("F3");
                    toolno = dgv.Rows[jj].Cells[5].Value.ToString();
              
                    if (dgv.Rows[jj].Cells[6].Value.ToString() == "长度")
                    {
                        comptype = "0";
                    }
                    else if (dgv.Rows[jj].Cells[6].Value.ToString() == "半径")
                    {
                        comptype = "1";
                    }
                    
                    if (refvalued >= 0)
                    {
                        if ((uppervalued + refvalued) < (lowerdatad + refvalued))
                        {
                            temps = dgv.Rows[jj].Cells[0].Value.ToString();

                            tempvalues = "测量上下限设置错误";

                            tempvalues = temps + tempvalues;
                            sr.Close();
                            aFile.Close();
                            MessageBox.Show(tempvalues);
                            return false;
                        }
                    }
                    else
                    {
                        if ((refvalued - uppervalued) > (refvalued - lowerdatad))
                        {
                            temps = dgv.Rows[jj].Cells[0].Value.ToString();
                           tempvalues = "测量上下限设置错误";
                            tempvalues = temps + tempvalues;
                            sr.Close();
                            aFile.Close();
                            MessageBox.Show(tempvalues);
                            return false;
                        }
                    }

                    meterdatas = "item=" + dgv.Rows[jj].Cells[0].Value.ToString();
                    meterdatas = meterdatas +",type=" + type;
                    meterdatas = meterdatas + ",ref=" + StValues;
                    meterdatas = meterdatas + ",upper=" + UpValues;
                    meterdatas = meterdatas + ",lower=" + LowValues;
                    meterdatas = meterdatas + ",toolno=" + toolno; 
                    meterdatas = meterdatas + ",comptype=" + comptype;
                    meterdatas = meterdatas + ",";
                    sr.WriteLine(meterdatas);
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

        private void MeterSet_Load(object sender, EventArgs e)
        {

            ChangeLanguage.LoadLanguage(this);//zxl 4.19
            language = ChangeLanguage.GetDefaultLanguage();

            LoadSetLanguage();
            MainForm.languagechangeEvent += LanguageChange; 
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

        
        }
        
        private void MeterSet_SizeChanged(object sender, EventArgs e)
        {
            return;  aotosize.controlAutoSize(this);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if  (e.Control is DataGridViewTextBoxEditingControl)
            {
                CellEdit = (DataGridViewTextBoxEditingControl)e.Control;
                DataGridView dgv = CellEdit.EditingControlDataGridView;
                if (dgv.CurrentCell.ColumnIndex == 2 || dgv.CurrentCell.ColumnIndex == 3 || dgv.CurrentCell.ColumnIndex == 4)
                {
                    CellEdit = (DataGridViewTextBoxEditingControl)e.Control;
                    CellEdit.SelectAll();
                    CellEdit.KeyPress += Cells_KeyPress;

                    //CellEdit.Leave += Cells_Leave;
                }
            }
            

        }
        private void Cells_KeyPresstool(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter
                || e.KeyChar == (char)Keys.Delete || e.KeyChar == '-')
            {

                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void Cells_Leavetool(object sender, EventArgs e)
        {
            language = ChangeLanguage.GetDefaultLanguage();
            string values = ((TextBox)sender).Text;
            int pointcount = 0;
            char temp;
            if (values == "")
            {

                ((TextBox)sender).Text = "0";
                return;
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    temp = values.ElementAt(i);
                    if (temp == '-')
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

                    MessageBox.Show("请输入正确数字");
                    ((TextBox)sender).Text = "0";
                }
                else
                {
                    double valued = Convert.ToDouble(values);
                    ((TextBox)sender).Text = valued.ToString("F0");
                }
            }
        }
        private void Cells_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter
                || e.KeyChar == (char)Keys.Delete || e.KeyChar == '-')
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
            char temp;
            if (values == "")
            {

                ((TextBox)sender).Text = "0.000";
                return;
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    temp = values.ElementAt(i);
                    if (temp == '-')
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
                  
                   MessageBox.Show("请输入正确数字");
                    ((TextBox)sender).Text = "0.000";
                }
                else
                {
                    double valued = Convert.ToDouble(values);
                    ((TextBox)sender).Text = valued.ToString("F3");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(true)
            {
                if(comboBoxModeSel.SelectedIndex ==0)
                {
                    initdataGridView(MetersetFilePath1_1, dataGridView1);
                    initdataGridView(MetersetFilePath1_2, dataGridView2);
                    initdataGridView(MetersetFilePath1_3, dataGridView3);
                    initdataGridView(MetersetFilePath1_4, dataGridView4);
                }
                else if (comboBoxModeSel.SelectedIndex == 1)
                {
                    initdataGridView(MetersetFilePath2_1, dataGridView1);
                    initdataGridView(MetersetFilePath2_2, dataGridView2);
                    initdataGridView(MetersetFilePath2_3, dataGridView3);
                    initdataGridView(MetersetFilePath2_4, dataGridView4);
                }
                else if (comboBoxModeSel.SelectedIndex == 2)
                {
                    initdataGridView(MetersetFilePath3_1, dataGridView1);
                    initdataGridView(MetersetFilePath3_2, dataGridView2);
                    initdataGridView(MetersetFilePath3_3, dataGridView3);
                    initdataGridView(MetersetFilePath3_4, dataGridView4);
                }
                else if (comboBoxModeSel.SelectedIndex == 3)
                {
                    initdataGridView(MetersetFilePath4_1, dataGridView1);
                    initdataGridView(MetersetFilePath4_2, dataGridView2);
                    initdataGridView(MetersetFilePath4_3, dataGridView3);
                    initdataGridView(MetersetFilePath4_4, dataGridView4);
                }
            }

        }
    }
}
