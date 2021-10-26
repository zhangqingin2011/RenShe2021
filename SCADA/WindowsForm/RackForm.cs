using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HNC_MacDataService;
using HNCAPI;

namespace SCADA
{
    public partial class RackForm : Form
    {

        AutoSizeFormClass aotosize = new AutoSizeFormClass();
        
        public static int CANGKUBITS_NUM = 16;   //仓库位数
        private int rack_dbNo = -1;
        public static String BLUEPATH = "..\\picture\\bluewuliao.png";
        public static String BLACKPATH = "..\\picture\\blackwuliao.png";
        public static String REDPATH = "..\\picture\\redwuliao.png";
        public static String WHITEPATH = "..\\picture\\whitewuliao.png";
        public static String YELLOWPATH = "..\\picture\\yellowwuliao.png";
        public static String GREYPATH = "..\\picture\\greywuliao.png";
        public static String GREENPATH = "..\\picture\\greenwuliao.png";
        private String GcodeFilePath = "C:\\Users\\Public\\Documents\\加工程序目录";
        public static bool COMOPENFLAGE = true;
        public static bool COMCLOSEFLAGE = false;
        public static int comstate = 1;//
       public static bool inilight = false;
        //private static int counttemp = 0;
        public string language = "";
        public static bool Inventoryflag = true;//盘点标识
        public static bool rfidreadflag = true;//读rfid标识
        public static bool rfidwriteflag = true;//写rfid标识
        public static bool getrfidflag = false;//获取plc料仓信息
        public static bool setrfidflag = false;//写plc料仓信息
        public static bool magstatesyncflag = false;//写rfid信息后，仓位信息同步标识
        public static bool[] mag_state_change_flage = new bool[30];//料仓状态变化标记
        public static bool codechangeflage = false;
        public static bool meterialchange = false;

        public static bool recivemagmessage = false;//写入或者获取plc整理仓位信息成功标识
        public static int intracktype = 1;

        public enum com_state
        {
            comclose= 1,//串口关闭
            comopen = 2,//串口打开
            comnormal=3,//串口通信正常
            comabnormal= 4,//串口通信异常
           
        }
        public enum LightColor
        {
            light_red = 1,//红色
            light_green,       //   绿色
            light_yellow,//黄色
            light_blue,//蓝色
            light_purple,//紫色
            light_cyan,//青色
            light_write,//白色
        };
        public RackForm()
        {
            aotosize.controllInitializeSize(this);
            InitializeComponent();
          
        }

        private void RackForm_Load(object sender, EventArgs e)
        {
          
            for (int ii = 0; ii < 30;ii++ )
            {
                mag_state_change_flage[ii]=true;
            }
            comboBoxmeterial.Items.Clear();
            comboBoxmeterial.Items.Add("A");
            comboBoxmeterial.Items.Add("B");
            comboBoxmeterial.Items.Add("C");
            comboBoxmeterial.Items.Add("D");
            comboBoxmeterial.Items.Add("E");
            comboBoxmeterial.Items.Add("F");
            comboBoxmeterial.Items.Add("G");
            comboBoxmeterial.Items.Add("H");
            comboBoxmeterial.SelectedIndex = 0;
            comboBoxcaizhi.SelectedIndex = 0;
            comboBoxq1.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 0];
            comboBoxq2.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 1];
            comboBoxq3.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 2];
            comboBoxq4.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 3];
            comboBoxq5.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 4];
            comboBoxq6.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 5];
            comboBoxq7.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 6];
            comboBoxq8.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 7];
            comboBoxq9.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 8];
            comboBoxq10.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 9];
            comboBoxq11.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 10];
            comboBoxq12.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 11];
            comboBoxq13.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 12];
            comboBoxq14.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 13];
            comboBoxq15.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 14];
            comboBoxq16.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 15];
            comboBoxq17.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 16];
            comboBoxq18.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 17];
            comboBoxq19.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 18];
            comboBoxq20.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 19];
            comboBoxq21.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 20];
            comboBoxq22.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 21];
            comboBoxq23.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 22];
            comboBoxq24.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 23];
            comboBoxq25.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 24];
            comboBoxq26.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 25];
            comboBoxq27.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 26];
            comboBoxq28.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 27];
            comboBoxq29.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 28];
            comboBoxq30.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag_Type + (int)ModbusTcp.MagLength * 29];
            comboBoxp1.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 0];
            comboBoxp2.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 1]; 
            comboBoxp3.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 2]; 
            comboBoxp4.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 3]; 
            comboBoxp5.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 4]; 
            comboBoxp6.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 5]; 
            comboBoxp7.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No +6];
            comboBoxp8.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 7];
            comboBoxp9.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 8];
            comboBoxp10.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 9];
            comboBoxp11.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 10];
            comboBoxp12.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 11];
            comboBoxp13.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 12];
            comboBoxp14.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 13];
            comboBoxp15.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 14];
            comboBoxp16.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 15];
            comboBoxp17.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 16];
            comboBoxp18.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 17];
            comboBoxp19.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 18];
            comboBoxp20.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 19];
            comboBoxp21.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 20];
            comboBoxp22.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 21];
            comboBoxp23.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 22];
            comboBoxp24.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 23];
            comboBoxp25.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 24];
            comboBoxp26.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 25];
            comboBoxp27.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 26];
            comboBoxp28.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 27];
            comboBoxp29.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 28];
            comboBoxp30.SelectedIndex = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + 29];
        }
        public static void initlightstate()
        {
            //更新五色灯状态
            if (MainForm.spport1.portisopen)//端口打开
            {
                byte key = 0;
                byte color = 0;
                int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
                int magnum = (int)ModbusTcp.MagAllNum;
                int maglength = (int)ModbusTcp.MagLength;

                
                for (byte MagNo = 0; MagNo < 30; MagNo++)
                {
                    int magstatei = magstart + maglength * MagNo;
                    key = (byte)(MagNo + (MagNo / 6) * 2);
                    if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statenull)//无料
                    {
                        color = (byte)LightColor.light_write;//
                    }
                    else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statewait)//毛培
                    {
                        color = (byte)LightColor.light_cyan;//
                    }
                    else if (ModbusTcp.DataMoubus[magstatei] ==(int)ModbusTcp.Mag_state_config.Statewait)//加工中
                    {
                        color = (byte)LightColor.light_blue;//
                    }
                    else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFailure )//加工异常
                    {
                        color = (byte)LightColor.light_red;//
                    }
                    else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFinishStandard)//加工合格
                    {
                        color = (byte)LightColor.light_green;//
                    }
                    else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFinishNotStandard)//加工不合格
                    {
                        color = (byte)LightColor.light_yellow;//
                    }
                    MainForm.spport1.SendMessage(color, key);//写数
                    MainForm.spport1.SendMessage(color, key);//写数据
                }
                inilight = true;//初始化信息置位

            }
        }

        private void closelight()
        {
            //更新五色灯状态
            if (MainForm.spport1.portisopen)//端口打开
            {
                byte key = 0;
                byte color = 0;
                int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
                int magnum = (int)ModbusTcp.MagAllNum;
                int maglength = (int)ModbusTcp.MagLength;


                for (byte MagNo = 0; MagNo < 30; MagNo++)
                {
                    int magstatei = magstart + maglength * MagNo;
                    key = (byte)(MagNo + (MagNo / 6) * 2);
                    color = 0;
                    MainForm.spport1.SendMessage(color, key);//写数据
                    MainForm.spport1.SendMessage(color, key);//写数据
                }
                inilight = true;//初始化信息置位
            }
        }

        private void refreshMagState(PictureBox PictureBoxtemp, int MagNo)
        {
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
            int magnum =    (int)ModbusTcp.MagAllNum;
            int maglength = (int)ModbusTcp.MagLength;
         
            int magstatei = magstart + maglength * MagNo;
             int magscenei =magstatei-3 ;
             int magmateriali =magstatei-1;
             int magtypei=magstatei-2;
             //ModbusTcp.DataMoubus[magtypei] = MagNo+1;
            if (TestForm.testbegin4)
            {
                ModbusTcp.DataMoubus[magstatei] = TestForm.magstate[MagNo];
                //if (TestForm.magstate[MagNo]>2)
                //{
                //    ModbusTcp.DataMoubus[magstatei] = TestForm.magstate[MagNo] + 1;
                //}             
            }
            if (ModbusTcp.DataMoubus[magscenei] != ModbusTcp.OldDataMoubus[magscenei])
            {
                RackForm.mag_state_change_flage[MagNo] = true;//将仓位信息写给plc的标识;
                ModbusTcp.OldDataMoubus[magscenei] = ModbusTcp.DataMoubus[magscenei];
                codechangeflage = true;
            }
            if (ModbusTcp.DataMoubus[magmateriali] != ModbusTcp.OldDataMoubus[magmateriali])
            {
                RackForm.mag_state_change_flage[MagNo] = true;//将仓位信息写给plc的标识;
                ModbusTcp.OldDataMoubus[magmateriali] = ModbusTcp.DataMoubus[magmateriali];

               // codechangeflage = true;
            }
            if (ModbusTcp.DataMoubus[magtypei] != ModbusTcp.OldDataMoubus[magtypei])
            {
                RackForm.mag_state_change_flage[MagNo] = true;//将仓位信息写给plc的标识;
                ModbusTcp.OldDataMoubus[magtypei] = ModbusTcp.DataMoubus[magtypei];

                codechangeflage = true;
            }
          if (ModbusTcp.DataMoubus[magstatei] != ModbusTcp.OldDataMoubus[magstatei])
            {
                  RackForm.mag_state_change_flage[MagNo] = true;//将仓位信息写给plc的标识

                    //更新五色灯状态
                    if (MainForm.spport1.portisopen)//端口打开
                    {
                        byte key =0;
                        byte color=0;
                        key = (byte )(MagNo + (MagNo / 6) * 2);
                        if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statenull)//无料
                        {
                            color = (byte)LightColor.light_write;//蓝色
                        }
                        else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statewait)//毛培
                        {
                            color = (byte)LightColor.light_cyan;//
                        }
                        else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateProcessing)//加工中
                        {
                            color = (byte)LightColor.light_blue;//
                        }
                        else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statecnc1Finish)//加工中
                        {
                            color = (byte)LightColor.light_blue;//
                        }
                        else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statecnc2Finish)//加工中
                        {
                            color = (byte)LightColor.light_blue;//
                        }
                        else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFailure)//加工异常
                        {
                            color = (byte)LightColor.light_red;//
                        }
                        else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFinishStandard)//加工合格
                        {
                            color = (byte)LightColor.light_green;//
                        }
                        else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFinishNotStandard)//加工不合格
                        {
                            color = (byte)LightColor.light_yellow;//
                        }
                        MainForm.spport1.SendMessage(color,key)  ;//写数据
                        MainForm.spport1.SendMessage(color, key);//写数据
                        MainForm.spport1.SendMessage(color, key);//写数据SS
                       
                 }


                    if(ModbusTcp.DataMoubus[magstatei]  == (int)ModbusTcp.Mag_state_config.Statenull)
                    {
                         PictureBoxtemp.Load(WHITEPATH);
                    }
                     else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statewait)
                   {
                        PictureBoxtemp.Load(GREYPATH);
                   }
                    else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateProcessing)
                  {
                       PictureBoxtemp.Load(BLUEPATH);
                  }
                    else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statecnc1Finish)
                    {
                        PictureBoxtemp.Load(BLUEPATH);
                    }
                    else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.Statecnc2Finish)
                    {
                        PictureBoxtemp.Load(BLUEPATH);
                    }
                 else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFinishStandard)
                {
                    PictureBoxtemp.Load(GREENPATH);
                }
                else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFinishNotStandard)
                {
                    PictureBoxtemp.Load(YELLOWPATH);
                }
                else if (ModbusTcp.DataMoubus[magstatei] == (int)ModbusTcp.Mag_state_config.StateFailure)
                {
                    PictureBoxtemp.Load(REDPATH);
                }
                ModbusTcp.OldDataMoubus[magstatei] = ModbusTcp.DataMoubus[magstatei];//测试使用
           }
         
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            language = ChangeLanguage.GetDefaultLanguage(); 
            if(language == "English")
            {
               if (label8.Text == "串口开启")
              {
                  label8.Text = "port open";
              }
            }
            else
            {
                if (label8.Text == "port open")
                {
                    label8.Text = "串口开启";
                }
            }
            if (COMCLOSEFLAGE)
            {
                        button2.BackColor = Color.Gray;
                        button3.BackColor = Color.LightGreen;
                        //comstate =(int) com_state.comopen;
                         if (language == "English")
                        {
                            label8.Text = "port open";
                        }
                         else 
                             label8.Text = "串口开启";
            }
            if (COMOPENFLAGE)
            {
                button3.BackColor = Color.Gray;
                button2.BackColor = Color.LightGreen;
                //comstate = (int)com_state.comclose;
                if (language == "English")
                {
                    label8.Text = "port close";
                }
                else
                    label8.Text = "串口关闭";
            }
             //如果已经取得了料架点位信息，初始无色等等的信息
            if (MainForm.InitRackFinish )
            {
                if(!inilight)
                {
                    initlightstate();//料架灯初始化一次
                }         
               
            }

            if (ModbusTcp.MES_PLC_comfim_write_flage == false )
            {   //  string language = ChangeLanguage.GetDefaultLanguage();
                if (Inventoryflag)
                {
                    button5.BackColor = Color.LightGreen;
                    if (language == "English")
                    {

                        button5.Text = "Inventory";
                    }
                    else
                        button5.Text = "料仓盘点";
                }
                if (rfidreadflag)
                {
                    button4.BackColor = Color.LightGreen;
                    if (language == "English")
                    {

                        button4.Text = "HMIWrite";
                    }
                    else
                        button4.Text = "HMI写入";
                }
                if (rfidwriteflag)
                {
                    button6.BackColor = Color.LightGreen;
                    if (language == "English")
                    {

                        button6.Text = "MESWrite";
                    }
                    else
                        button6.Text = "MES写入";
                }
            }
            if (Inventoryflag == false)//料架盘点中，所哟无料为无料
            {
               //for(int i = 1;i<=30;i++)
               //{
                 //int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;
               //    int magnum = (int)ModbusTcp.MagAllNum;
               //    int maglength = (int)ModbusTcp.MagLength;

               //    int magstatei = magstart + maglength * (i-1);
               //     ModbusTcp.DataMoubus[magstatei] = 0;
               //}
            }
            Getallmagsceneandmaterial();


            refreshMagState(this.pictureBoxNo1, 0);
            refreshMagState(this.pictureBoxNo2, 1);
            refreshMagState(this.pictureBoxNo3, 2);
            refreshMagState(this.pictureBoxNo4, 3);
            refreshMagState(this.pictureBoxNo5, 4);
            refreshMagState(this.pictureBoxNo6, 5);
            refreshMagState(this.pictureBoxNo7, 6);
            refreshMagState(this.pictureBoxNo8, 7);
            refreshMagState(this.pictureBoxNo9, 8);
            refreshMagState(this.pictureBoxNo10, 9);
            refreshMagState(this.pictureBoxNo11, 10);
            refreshMagState(this.pictureBoxNo12, 11);
            refreshMagState(this.pictureBoxNo13, 12);
            refreshMagState(this.pictureBoxNo14, 13);
            refreshMagState(this.pictureBoxNo15, 14);
            refreshMagState(this.pictureBoxNo16, 15);
            refreshMagState(this.pictureBoxNo17, 16);
            refreshMagState(this.pictureBoxNo18, 17);
            refreshMagState(this.pictureBoxNo19, 18);
            refreshMagState(this.pictureBoxNo20,19);
            refreshMagState(this.pictureBoxNo21, 20);
            refreshMagState(this.pictureBoxNo22, 21);
            refreshMagState(this.pictureBoxNo23, 22);
            refreshMagState(this.pictureBoxNo24, 23);
            refreshMagState(this.pictureBoxNo25, 24);
            refreshMagState(this.pictureBoxNo26, 25);
            refreshMagState(this.pictureBoxNo27, 26);
            refreshMagState(this.pictureBoxNo28, 27);
            refreshMagState(this.pictureBoxNo29, 28);
            refreshMagState(this.pictureBoxNo30, 29);
            if(codechangeflage == true)
            {              
                Getallmaggcodel();
                codechangeflage = false;
            }
            
            //更新串口状态
      
            ////hxb  2017.4.10
            //if (HncApi.HNC_NetIsConnect(Collector.CollectHNCPLC.dbNo) >= 0)

        }
        private void Getallmaggcodel()
        {
            getmaggcode();
        }
        private void Getallmagsceneandmaterial()
        {
            getmagindex();
        }
        private void getmaggcode()
        {
            foreach (Control control in this.tableLayoutPanel5.Controls)
            {
                if (control is TableLayoutPanel)
                {
                    submagindex1(control.Controls);
                }
            }
        }
        private void submagindex1(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is TableLayoutPanel)
                {
                    subsubmagindex11(control.Controls);
                }
            }
        }
        private void subsubmagindex11(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is TableLayoutPanel)
                {
                    subsubmagindex111(control.Controls);
                }
            }
        }
        private void subsubmagindex111(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                //OA
                if (control is Label)
                {
                    Label temp = (Label)control;
                    string names = temp.Name;
                    if (names == "")
                    {
                        return;
                    }
                    string numbers = "";
                    string type = "";
                    int number = -1;
                    int index = 5;
                    int length = 0;
                    type = names.Substring(5, 1);
                    if (type != "c" && type != "l")
                    {
                        return;
                    }
                    length = names.Length - index;
                    numbers = names.Substring(index + 1, length - 1);
                    number = Convert.ToInt32(numbers);
                    
                    int magscenestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;//零件类型  

                    int maglength = (int)ModbusTcp.MagLength;
                    int magSecenei = magscenestart + maglength * (number - 1);//场次
                    int magSecene = ModbusTcp.DataMoubus[magSecenei];
                    //int magmeterial = ModbusTcp.DataMoubus[magSecenei + 2];
                    int magtypestart = ModbusTcp.DataMoubus[magSecenei + 1];
                    char magSeceneic;
                    string magSeceneis = "";
                    //string magmeterialis = Convert.ToString(magmeterial);
                    string magtypestartis = Convert.ToString(magtypestart);//零件类型0-6，A-F
                    string MagNoS = "";
                    string gcoderoad = GcodeFilePath;
                    string gcodename = "";
                    //A的ascii码为41大写
                    magSecenei = magSecene + 65;

                    magSeceneic = Convert.ToChar(magSecenei);
                    magSeceneis = Convert.ToString(magSeceneic);//场次信息
                    if (number < 10)
                    {
                        MagNoS = "0"+Convert.ToString(number);
                    }
                    else
                    {
                        MagNoS =  Convert.ToString(number);
                    }

                    type = names.Substring(5, 1);
                    length = names.Length - index - 1;
                    numbers = names.Substring(index + 1, length );
                    number = Convert.ToInt32(numbers);
                    if (type == "l")
                    {
                        gcodename = "O" + magSeceneis +MagNoS+ magtypestartis + "L.nc";
                        gcoderoad = GcodeFilePath + "\\" + gcodename;
                        if (File.Exists(@gcoderoad))
                        {
                           temp.Text= "L:"+gcodename ;
                        }
                        else
                        {
                            temp.Text = "L:";
                        }
                    }
                    else if (type == "c")
                    {

                        gcodename = "O" + magSeceneis +MagNoS + magtypestartis + "CNC.nc";
                        gcoderoad = GcodeFilePath + "\\" + gcodename;
                        if (File.Exists(@gcoderoad))
                        {
                            temp.Text = "C:"+gcodename;
                        }
                        else
                        {
                            temp.Text = "C:";
                        }
                    }
                }
            }
        }
        
        private void getmagindex()
        {
            foreach (Control control in this.tableLayoutPanel5.Controls)
            {
                if (control is TableLayoutPanel)
                {
                    submagindex(control.Controls);
                }
            }
        }
        private void submagindex(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
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
                    if (names == "")
                    {
                        return;
                    }
                    string numbers = "";
                    string type = "";
                    int number = -1;
                    int index = -1;
                    int length = 0;
                    int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;
                    int magnum =    (int)ModbusTcp.MagAllNum;
                    int maglength = (int)ModbusTcp.MagLength;
                    int magtypei=0;
                    int magmodenum=0;
         
                    index = names.IndexOf('x');
                    type = names.Substring(index + 1, 1);                    
                    length = names.Length - index-1;
                    numbers = names.Substring(index + 2, length - 1);
                    number = Convert.ToInt32(numbers);
                    if(type =="p")
                    {
                        magmodenum = (int)SCADA.ModbusTcp.DataConfigArr.Mag1_Sheet_No + number-1;
                        if (temp.SelectedIndex<0)
                        {
                            temp.SelectedIndex=0;
                        }
                        ModbusTcp.DataMoubus[magmodenum] = temp.SelectedIndex;
                    }
                    else if(type =="q")
                    {

                        magtypei = magstart + maglength * (number - 1) + 1;
                        if (temp.SelectedIndex < 0)
                        {
                            temp.SelectedIndex = 0;
                        }
                        ModbusTcp.DataMoubus[magtypei] = temp.SelectedIndex;
                        if (intracktype<30)
                        {
                            intracktype++;
                            if (temp.SelectedIndex < 0)
                            {
                                temp.SelectedIndex = 0;
                            }
                            if (number < 13)
                            {
                               // temp.SelectedIndex = 0;
                                ModbusTcp.DataMoubus[magtypei] = temp.SelectedIndex;
                            }
                            else if (number < 25)
                            {
                                //temp.SelectedIndex = 1;
                                ModbusTcp.DataMoubus[magtypei] = temp.SelectedIndex;
                            }
                            else if (number < 28)
                            {
                               // temp.SelectedIndex = 2;
                                ModbusTcp.DataMoubus[magtypei] = temp.SelectedIndex;
                            }
                            else
                            {
                                //temp.SelectedIndex = 3;
                                ModbusTcp.DataMoubus[magtypei] = temp.SelectedIndex;
                            }
                        }
                        
                       
                    }                  
                }
            }
        }
        private int getnumformstring(string str)
        {
            int num;
            int[] str1= {0,0};
            for (int i = 0; i < str.Length; i++)
            {
                char item = str.ElementAt(i);
                if (item<='9'&&item>='0')
                {
                    str1[1] = str1[0];
                    str1[0] = (int)(item)-(int)('0');
                }
            }
            return num = str1[1] * 10 + str1[0];
        }
        private void comboBoxNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string comboboxname = ((ComboBox ) sender).Name;
           int boxno =  getnumformstring(comboboxname);
             int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;
            int maglength = (int)ModbusTcp.MagLength;          
             int magstatei = magstart + maglength * (boxno-1);

             ModbusTcp.DataMoubus[magstatei] = ((ComboBox ) sender).SelectedIndex;//更改料仓状态

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
                    MessageBox.Show("Please enter number") ;
                }
                else 
                MessageBox.Show("请输入数字");
                textBox1.Focus();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            int MagNo = getnumformstring(MagNoStr);
            if (MagNo > 30 || MagNo < 0)
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
                int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;
                int maglength = (int)ModbusTcp.MagLength;
                int magstatei = magstart + maglength * MagNo - 1;
                if (ModbusTcp.DataMoubus[magstatei] == (int)SCADA.ModbusTcp.Mag_state_config.Statewait)//仓位待加工
                {
                    return;
                }
                else
                {
                    //MessageBox.Show("当前仓位不是待加工，请重新派单");
                    //textBox1.Focus();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string MagNoStr = textBox1.Text;
            int MagNo = getnumformstring(MagNoStr);
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;
            int maglength = (int)ModbusTcp.MagLength;
            int magstatei = magstart + maglength * MagNo - 1;
            int magchecki = magstatei-1;
            //当前料位正在加工无法初始化
            if (MainForm.cncv2list[0].MagNum == MagNo
                || MainForm.cncv2list[1].MagNum == MagNo)
            {
                if (language == "English")
                {
                    MessageBox.Show("Current rack is processing , the rack can't be initialized .");
                }
                else
                    MessageBox.Show("当前仓位订单未完成，无法初始化为无料状态！");
                textBox1.Focus();
            }
            else //仓位待加工
            {
                ModbusTcp.DataMoubus[magstatei] = (int)SCADA.ModbusTcp.Mag_state_config.Statenull;
                //ModbusTcp.DataMoubus[magstatei-1] = 0;
               // ModbusTcp.DataMoubus[magstatei-2] = 0;
               // ModbusTcp.DataMoubus[magstatei-3] = 0;
              

                string temp = "";
                if (language == "English")
                {
                   temp = MagNoStr + " the rack initialized successful .";
                }
                else 
                    temp = MagNoStr + "仓位初始化为无料成功！";
                MessageBox.Show(temp);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (COMOPENFLAGE)
            {
                //打开COM口
                if ( !MainForm.spport1.portisopen)//串口开启状态
                {
                    if (MainForm.spport1.Open())//串口打开成功
                    {
                          // 开启串口成功
                        button2.BackColor = Color.Gray;
                        button3.BackColor = Color.LightGreen;
                        initlightstate();//料架灯初始化一次
                        //comstate =(int) com_state.comopen;
                         if (language == "English")
                        {
                            label8.Text = "port open";
                        }
                         else 
                             label8.Text = "串口开启";
                       COMOPENFLAGE = false;
                       COMCLOSEFLAGE = true;      
                    }
                    else
                    {//串口打开失败
                        if (language == "English")
                        {
                            label8.Text = "port open err";
                        }
                        else  
                            label8.Text = "串口开启失败";
                        COMOPENFLAGE = true;
                        COMCLOSEFLAGE = false;   
                    }
                }

            }

        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            if (COMCLOSEFLAGE)
            {
                //关闭COM口
                if (MainForm.spport1.portisopen)//串口关闭状态
                {
                    closelight();//关闭所有灯
                    if (MainForm.spport1.Close())//串口打开成功
                    {
                        button3.BackColor = Color.Gray;
                        button2.BackColor = Color.LightGreen;
                        //comstate = (int)com_state.comclose;
                        if (language == "English")
                        {
                            label8.Text = "port close";
                        }
                        else
                            label8.Text = "串口关闭";
                        COMOPENFLAGE = true;
                        COMCLOSEFLAGE = false;
                    }
                    else
                    {
                        if (language == "English")
                        {
                            label8.Text = "port close err";
                        }
                        else  
                            label8.Text = "串口关闭失败";
                        COMOPENFLAGE = false;
                        COMCLOSEFLAGE = true;
                    }
                }
                else 
                    return;
            }
            return;
        }

        private void RackForm_SizeChanged(object sender, EventArgs e)
        {
        //    aotosize.controlAutoSize(this);
        }
                
        /// <summary>
        /// 料架盘点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (MainForm.PLC_SIMES_ON_line==false)//plc离线
            {
                if (language == "English")
                {
                    MessageBox.Show("PLC is off-line,can't inventory!");
                }
                else  MessageBox.Show("PLC离线，不允许盘点!");
                return ;
            }
            if (MainForm.linereseting )//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is resetting,can't inventory!");
                }
                else  MessageBox.Show("产线复位进行中，不允许盘点!");
                return;
            }
            if (MainForm.linestarting)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is starting,can't inventory!");
                }
                else MessageBox.Show("产线启动进行中，不允许盘点!");
                return;
            }
            if (MainForm.linestoping)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is stopping,can't inventory!");
                }
                else MessageBox.Show("产线停止进行中，不允许盘点!");
                return;
            }
            if (MainForm.cncv2list[0].MagNum!=0 && MainForm.cncv2list[1].MagNum != 0 )//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is processing,can't inventory!");
                }
                else MessageBox.Show("产线生产中，不允许盘点!");
                    return;
            }
            if (!Inventoryflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rackis Inventorying!");
                }
                else MessageBox.Show("当前正在盘点!");
            }
            if(!rfidreadflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is reading ,can't inventory!");
                }
                else  MessageBox.Show("HMI信息写入中，不允许盘点!");
                return;
            }
            if (!rfidwriteflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is writting,can't inventory!");
                }
                else MessageBox.Show("MES信息写入中，不允许盘点!");
                return;
            }
            if (MainForm.linestart)//产线没开启，不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The line is stop，please start the line!");
                }
                else MessageBox.Show("产线停止，请启动产线!");
                return;
            }
      
            if(Inventoryflag)//Inventoryflag盘点执行中为false，没有盘点为true
            {
                if(ModbusTcp.MES_PLC_comfim_write_flage == false)
                {
                    //获取当前默认语言  
                    //string language = ChangeLanguage.GetDefaultLanguage();
                    button5.BackColor = Color.LightPink;
                    if (language == "English")
                    {

                        button5.Text = "On Inventory ";
                    }
                    else button5.Text = "料架盘点中";
                    ModbusTcp.MES_PLC_comfim_write_flage = true;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComWriteRfid;
                    Inventoryflag = false;
                }
            }
 
        }

        private void panel3_SizeChanged(object sender, EventArgs e)
        {
            aotosize.controlAutoSize(this);

        }
        /// <summary>
        /// 读取RFID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (MainForm.PLC_SIMES_ON_line == false)//plc离线
            {
                if (language == "English")
                {
                    MessageBox.Show("PLC is off-line,can't inventory!");
                }
                else MessageBox.Show("PLC离线，不允许HMI信息写入!");
                return;
            }
          //  if (!MainForm.linereset)//产线复位中不能盘点
            if (MainForm.linereseting)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is resetting,can't inventory!");
                }
                else MessageBox.Show("产线复位进行中，不允许HMI信息写入");
                return;
            }
            if (MainForm.linestarting)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is starting,can't inventory!");
                }
                else MessageBox.Show("产线启动进行中，不允许HMI信息写入!");
                return;
            }
            if (MainForm.linestoping)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is stopping,can't writting!");
                }
                else MessageBox.Show("产线停止进行中，不允许HMI信息写入!");
                return;
            }
          
            if (MainForm.cncv2list[0].MagNum != 0 && MainForm.cncv2list[1].MagNum != 0)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Order is processing!");
                }
                else MessageBox.Show("产线生产中，不允许HMI信息写入!");
                return;
            }
            if (!Inventoryflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rackis writting!");
                }
                else MessageBox.Show("当前正在盘点!");
                return;
            }
            if (!rfidreadflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("HMI message is writting!");
                }
                else MessageBox.Show("HMI信息正在写入!");
            }
            if (!rfidwriteflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is writting,can't writting!");
                }
                else MessageBox.Show("MES信息写入中，不允许HMI写入!!");
                return;
            }
            if (MainForm.linestart)//产线没开启，不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The line is stop，please start the line!");
                }
                else MessageBox.Show("产线停止，请启动产线!");
                return;
            }
            if (rfidreadflag)//Inventoryflag盘点执行中为false，没有盘点为true
            {
                if (ModbusTcp.MES_PLC_comfim_write_flage == false)
                {
                    //获取当前默认语言  
                    //string language = ChangeLanguage.GetDefaultLanguage();
                    button4.BackColor = Color.LightPink;
                    //RackForm.magstatesyncflag = false;
                    if (language == "English")
                    {

                        button4.Text = "HMIWritting";
                    }
                    else button4.Text = "HMI写入中";
                    ModbusTcp.MES_PLC_comfim_write_flage = true;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComReadRfid;
                    rfidreadflag = false;
                }
            }
        }
 /// <summary>
 /// 写RFID
 /// </summary>
 /// <param name="sender"></param>
 /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (MainForm.PLC_SIMES_ON_line == false)//plc离线
            {
                MessageBox.Show("PLC离线，不允许MES信息写入!");
                return;
            }
            if (MainForm.linereseting)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is resetting,can't writting!");
                }
                else MessageBox.Show("产线复位进行中，不允许MES信息写入");
                return;
            }
            if (MainForm.linestarting)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is starting,can't writting!");
                }
                else MessageBox.Show("产线启动进行中，不允许MES信息写入!");
                return;
            }
            if (MainForm.linestoping)//产线复位中不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The Line is stopping,can't writting!");
                }
                else MessageBox.Show("产线停止进行中，不允许MES信息写入!");
                return;
            }
            if (MainForm.cncv2list[0].MagNum != 0 && MainForm.cncv2list[1].MagNum != 0)//产线复位中不能盘点
            {
                MessageBox.Show("产线生产中，不允许MES信息写入!");
                return;
            }
            if (!Inventoryflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rackis Inventorying!");
                }
                else MessageBox.Show("当前正在盘点!");
                return;
            }
            if (!rfidreadflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is writting,can't writting!");
                }
                else MessageBox.Show("HMI信息写入中，不允许再次写入!");
                return;
            }
            if (!rfidwriteflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is writting!");
                }
                else MessageBox.Show("MES信息正在写入!");
                return;
            }
            if (MainForm.linestart)//产线没开启，不能盘点
            {
                if (language == "English")
                {
                    MessageBox.Show("The line is stop，please start the line!");
                }
                else MessageBox.Show("产线停止，请启动产线!");
                return;
            }
            if (rfidwriteflag)//Inventoryflag盘点执行中为false，没有盘点为true
            {
                if (ModbusTcp.MES_PLC_comfim_write_flage == false)
                {
                    RackForm.setrfidflag = true;//写rfid置位料仓信息整体更新
                    //if (RackForm.getrfidflag)//获取plc的料仓整体信息 RackForm.rfidreadflag
                    //{
                    //    if (RackForm.recivemagmessage)
                    //    {
                    //        RackForm.getrfidflag = false;
                    //        RackForm.recivemagmessage = false;
                    //        return;
                    //    }
                    //    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.req, 1, 0, (int)ModbusTcp.DataConfigArr.Mag_Scene, 120);//给plc写订单信息
                    //    MainForm.sptcp1.ReceiveData();
                    //}
                    //获取当前默认语言  
                    string language = ChangeLanguage.GetDefaultLanguage();
                    button6.BackColor = Color.LightPink;
                    if (language == "English")
                    {

                        button6.Text = "MESWritting";
                    }
                    else button6.Text = "MES写入中";
                    ModbusTcp.MES_PLC_comfim_write_flage = true;
                    ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComWriteRfid;
                    rfidwriteflag = false;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int ii = 1; ii <= 30;ii++ )
            {

                int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;
                int maglength = (int)ModbusTcp.MagLength;
                int magScene = magstart + maglength * (ii-1);
                if (MainForm.cncv2list[0].MagNum == ii
               || MainForm.cncv2list[1].MagNum == ii)
                {
                    string temp = "";
                    if (language == "English")
                    {
                        temp = "No." + ii.ToString() + "is processing , the rack can't be initialized !";
                        MessageBox.Show(temp );
                    }
                    else
                    {
                        temp = "No." + ii.ToString()+"仓位订单未完成，无法初始化！";
                        MessageBox.Show(temp);
                    }
  
                }
                else //仓位待加工
                {
                    ModbusTcp.DataMoubus[magScene] = 0;
                    ModbusTcp.DataMoubus[magScene + 1] = ii;
                    //ModbusTcp.DataMoubus[magScene + 2] = 0;
                    ModbusTcp.DataMoubus[magScene + 3] = 0;
                    intracktype = 1;
                    
                }
            }

            //当前料位正在加工无法初始化
           
        }
        /// <summary>
        /// 同步仓位信息给plc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
             if (MainForm.PLC_SIMES_ON_line == false)//plc离线
            {
                MessageBox.Show("PLC离线，不允许同步信息!");
                return;
            }
         
            if (!Inventoryflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rackis Inventorying!");
                }
                else MessageBox.Show("当前正在盘点!");
                return;
            }
            if (!rfidreadflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is writting,can't inventory!");
                }
                else MessageBox.Show("HMI信息写入中，不允许写入!");
                return;
            }
            if (!rfidwriteflag)
            {
                if (language == "English")
                {
                    MessageBox.Show("The rfid is writting!");
                }
                else MessageBox.Show("MES信息正在写入!");
                return;
            }

            if (!magstatesyncflag)
            {
                magstatesyncflag = true;
                button8.BackColor = Color.Gray;
                if (language == "English")
                {

                    button8.Text = "MessageSteping";
                }
                else button8.Text = "信息同步中";
            }
            else
            {
                magstatesyncflag =false ;
                button8.BackColor = Color.LightGreen;
                if (language == "English")
                {

                    button8.Text = "MessageStep";
                }
                else button8.Text = "信息同步";
            }
        }
        //场次信息变更
        private void comboBoxmeterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxmeterial.SelectedIndex >= 0)
            {
                int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_Scene;
                int maglength = (int)ModbusTcp.MagLength;
                for (int i = 0; i < 30; i++)
                {

                    int magScene = magstart + maglength * i;
                    ModbusTcp.DataMoubus[magScene] = comboBoxmeterial.SelectedIndex;
                }
            }
            codechangeflage = true;
            
          //  meterialchange = true;
        }
        //材质变化
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Meterial] = comboBoxcaizhi.SelectedIndex;
            int magstart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_material;
         
            int maglength = (int)ModbusTcp.MagLength;
            for(int i=0;i<30;i++)
            {
                int meterialnum = magstart + maglength * i;
                ModbusTcp.DataMoubus[meterialnum] = comboBoxcaizhi.SelectedIndex; ;
            }
            codechangeflage = true;
        }

    
    }

}
