﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Collector;
using HNC_MacDataService;
using HNCAPI;
using ScadaHncData;
using System.IO;

namespace SCADA
{
    public partial class MeasureForm : Form
    {
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
        //public string[] strMeasureStates = { "连接正常", "测量开始", "测量中", "测量结束" };
        int measure_dbNo = -1;
        public MeasureForm()
        {
            InitializeComponent();
            comboBoxToolNo.SelectedIndex = 0;
        }
        public  void UpdateStandardValue()
        {
             //2017.7.24  处理方式使用定时器不好
            tBRefer.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, 0, m_xmlDociment.Default_Attributes_str3[(int)m_xmlDociment.Attributes_MEAS.refe]);
            tBMax.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, 0, m_xmlDociment.Default_Attributes_str3[(int)m_xmlDociment.Attributes_MEAS.max]);
            tBMin.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, 0, m_xmlDociment.Default_Attributes_str3[(int)m_xmlDociment.Attributes_MEAS.min]);
        }
        private void MeasureForm_Load(object sender, EventArgs e)
        {
            ChangeLanguage.LoadLanguage(this);//4.19

            tBRefer.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, 0, m_xmlDociment.Default_Attributes_str3[(int)m_xmlDociment.Attributes_MEAS.refe]);
            tBMax.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, 0, m_xmlDociment.Default_Attributes_str3[(int)m_xmlDociment.Attributes_MEAS.max]);
            tBMin.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, 0, m_xmlDociment.Default_Attributes_str3[(int)m_xmlDociment.Attributes_MEAS.min]);

            //默认测量与第一个机床通讯  hxb  2017.4.27
            string ip = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
            MacDataService.GetInstance().GetMachineDbNo(ip, ref measure_dbNo);
            //measure_dbNo = MainForm.cnclist[0].dbNo; wwj
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Visible && measure_dbNo >= 0 && MacDataService.GetInstance().IsNCConnectToDatabase(measure_dbNo))
            {
                UpdateStandardValue(); //2017.7.24

                //Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_B, MEASURE_VALUE, out value, measure_dbNo);
                string key = "MacroVariables:USER";
                string macVarStr = "";
                int ret = MacDataService.GetInstance().GetHashKeyValueString(measure_dbNo, key, MEASURE_VALUE0, ref macVarStr);
                if (ret == 0 && macVarStr.Length>0)
                {
                   // SDataUnion macVar = Newtonsoft.Json.JsonConvert.DeserializeObject<SDataUnion>(macVarStr);  //"{\"Value\":{\"type\":0,\"g90\":0,\"v\":{\"i\":0,\"f\":0.0,\"n\":0}}}
                   // tBDepth.Text = Convert.ToString(macVar.v.f);   //解析并显示测量结果
                    int strStart = macVarStr.IndexOf("f\":");
                    int len = macVarStr.IndexOf(",", strStart + 3) - (strStart + 3);
                    string strTmp = macVarStr.Substring(strStart + 3, len);      
                   // Console.WriteLine("Start= "+ strStart);
                   // Console.WriteLine("Value=  "+ strTmp);
                    tBDepth.Text = (Convert.ToDouble(strTmp)).ToString("f3");
                }
            }
        }

        //将测量结果的补偿值设入刀补表，完成刀补(+-1)
        private void btnSetCompen_Click(object sender, EventArgs e)
        {
            double compenVal = Convert.ToDouble(tBCompen.Text);

            //从设置表格中获取参考值，公差上限、下限
            double refer = Convert.ToDouble(tBRefer.Text);
            double max = Convert.ToDouble(tBMax.Text);
            double min = Convert.ToDouble(tBMin.Text);
            //if (!checkedInputValue(compenVal))
            //{
            //    MessageBox.Show("输入非法！");
            //    tBCompen.Text = "0.000";
            //    return;
            //}

            if (this.Visible && measure_dbNo >= 0 && MacDataService.GetInstance().IsNCConnectToDatabase(measure_dbNo))
            {
                double d_wucha = 0.1;  //误差范围 -0.1mm~ +0.1mm；之间需要补偿
                double d_DepthNor = Convert.ToDouble(tBRefer.Text);  //参考值
                //if (Math.Abs(d_DepthNor - Convert.ToDouble(tBDepth.Text)) < d_wucha)
                {
                    double toolVal = 0.0;
                    string key = "Channel:" + MainForm.cnclist[0].HCNCShareData.chanDataLst[0].chNo;
                    int Tool_NO = comboBoxToolNo.SelectedIndex+1;
                    string toolStr = "";
                    int ret = MacDataService.GetInstance().GetHashKeyValueString(measure_dbNo, "Tool:List", String.Format("{0:D4}", Tool_NO), ref toolStr);
                    if (ret == 0)
                    {
                        toolComp tool = Newtonsoft.Json.JsonConvert.DeserializeObject<toolComp>(toolStr);
                        int res = MacDataService.GetInstance().HNC_ToolSetValue(2, Tool_NO, compenVal, measure_dbNo);//腔体深度，Z轴方向补偿  2
                        if(res != 0)
                            MessageBox.Show("补偿失败，请确认网络是否连接！");
                    }
                }
            }
        }

          
  

        private void btnStartAgent_Click(object sender, EventArgs e)
        {
            //判断文件的存在 
            string filePath = "..\\HNC-iScope\\HNC-iScope.exe";  //??
            string procName = "HNC-iScope";
            if (File.Exists(filePath))
            {
                //System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
                //Info.FileName = filePath;
                if (System.Diagnostics.Process.GetProcessesByName(procName).ToList().Count > 0)
                {
                    int num = System.Diagnostics.Process.GetProcessesByName(procName).ToList().Count;
                    Console.WriteLine("num"+num);
                    return;
                }

                System.Diagnostics.Process.Start(filePath);
                System.Threading.Thread.Sleep(200);
                //tBFile1.Text = file1;
            }
            else
            {
                MessageBox.Show("数据采集失败！");
            }
        }


    }
}
