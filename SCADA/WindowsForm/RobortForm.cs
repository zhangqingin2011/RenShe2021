using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace SCADA
{
    public partial class RobortForm : Form
    {

        public string language = "";
        private string prelanguage; //记录切换语言之前的语言

        public static String HOMEPATH = "..\\picture\\top_bar_green.png";
        public static String NOHOMEPATH = "..\\picture\\top_bar_black.png";
        Hashtable m_Hashtable;
        public RobortForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            language = ChangeLanguage.GetDefaultLanguage();
            if (MainForm.robotconnect&&MainForm.PLC_SIMES_ON_line)
            {
               
                     labelroboton.Text = "在线";
            
            }
            else
            {
                 labelroboton.Text = "离线";
              
                pictureBoxhome.Load(NOHOMEPATH);
                labelstate.Text  = "";
                labelmode.Text  = "";
                textBoxj1.Text = "";
                textBoxj2.Text = "";
                textBoxj3.Text = "";
                textBoxj4.Text = "";
                textBoxj5.Text = "";
                textBoxj6.Text = "";
                textBoxj7.Text = "";
                return ;
            }

            if(ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_status] ==1)
            {
                //if (language == "English")
                // {
                //     labelstate.Text = "alarm";
                // }
                // else
                // {
                     labelstate.Text = "报警";
                //}
            }
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_status] == 0)
            {
                //if (language == "English")
                //{
                //    labelstate.Text = "run";
                //}
                //else
                //{
                  labelstate.Text = "运行";
                //}
            }

            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] == 0)
            {
                //if (language == "English")
                //{
                //    labelmode.Text = "null";
                //}
                //else
                //{
                    labelmode.Text = "无";
                //}
            }
            if(ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] ==1)
            {
                //if (language == "English")
                // {
                //     labelmode.Text = "HAND";
                // }
                // else
                // {
                    labelmode.Text = "手动";
                //}
            }
            if(ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] ==2)
            {
                //if (language == "English")
                // {
                //     labelmode.Text = "AUTO";
                // }
                // else
                // {
                     labelmode.Text = "自动";
                //}
            }
            if(ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_mode] ==3)
            {
                //if (language == "English")
                // {
                //     labelmode.Text = "EXTERNAL";
                // }
                // else
                // {
                     labelmode.Text = "外部";
                //}
            }

            string clampno = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Robot_clamp_number].ToString();
            if (clampno == "0")
            {
                //if (language == "English")
                //{
                //    labelno.Text = "No clamp";
                //}
                //else
                //{
                    labelno.Text = "无料抓";
                //}
            }
            else
            {
                //if (language == "English")
                //{
                //    labelno.Text = "clamp NO." + clampno;
                //}
                //else
                //{
                    labelno.Text = clampno+"号料抓";
                //}
            }
            
            
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm] == 1)
            {
               
                pictureBoxhome.Load(HOMEPATH);
            }
            else
            {
                pictureBoxhome.Load(NOHOMEPATH);
            }
            textBoxj1.Text = MainForm.SRobortdata.SiteJ1.ToString("F3");
            textBoxj2.Text = MainForm.SRobortdata.SiteJ2.ToString("F3");
            textBoxj3.Text = MainForm.SRobortdata.SiteJ3.ToString("F3");
            textBoxj4.Text = MainForm.SRobortdata.SiteJ4.ToString("F3");
            textBoxj5.Text = MainForm.SRobortdata.SiteJ5.ToString("F3");
            textBoxj6.Text = MainForm.SRobortdata.SiteJ6.ToString("F3");
            textBoxj7.Text = MainForm.SRobortdata.SiteJ7.ToString("F3");
        }

        private void RobortForm_Load(object sender, EventArgs e)
        {
            //ChangeLanguage.LoadLanguage(this);//zxl 4.19
           // language = ChangeLanguage.GetDefaultLanguage();
           // LoadSetLanguage();
           // MainForm.languagechangeEvent += LanguageChange; 
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
            //if(lang=="English")
            //{
                
            //    labelrobot1.Text = "communication :";
            //    robortpos.Text = "robot position :";
            //    labelstates.Text = "State :";
            //    labelmodes.Text = "Mode :";
            //    labelhomes.Text = "Home :";
            //}
            //else
            //{
                labelrobot1.Text = "机器人通信：";
                robortpos.Text = "机器人实际位置：";
                labelstates.Text = "状态：";
                labelmodes.Text = "模式：";
                labelhomes.Text = "Home点：";
            //}
        }

       
      
    }
}
