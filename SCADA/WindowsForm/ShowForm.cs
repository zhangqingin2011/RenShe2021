using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA
{
    public partial class ShowForm : Form
    {
        public static string messagestring;

        public static string compmessage;
        public string language = "";
        public ShowForm()
        {
            language = ChangeLanguage.GetDefaultLanguage();
            InitializeComponent();
            textBox1.Text = messagestring;
            textBox2.Text = compmessage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MainForm.PLC_SIMES_ON_line == false)//plc离线
            {
              MessageBox.Show("PLC离线，不允许返修!");
                return;
            }
            if (MainForm.linereseting)//产线复位中不能盘点
            {
                 MessageBox.Show("产线复位进行中，不允许返修!");
                return;
            }
            if (MainForm.linestarting)//产线复位中不能盘点
            {
                 MessageBox.Show("产线启动进行中，不允许返修!");
                return;
            }
            if (MainForm.linestoping)//产线复位中不能盘点
            {
               MessageBox.Show("产线停止进行中，不允许返修!");
                return;
            }
            if ( MainForm.cncv2list[1].IsConnected() == false)//产线复位中不能盘点
            {
                MessageBox.Show("铣床离线，不允许返修!");
                return;
            }
            if (!RackForm.Inventoryflag)
            {
               MessageBox.Show("当前正在盘点，不允许返修!");
                return;
            }
            if (!RackForm.rfidreadflag)
            {
               MessageBox.Show("RFID信息读取中，不允许返修!");
                return;
            }
            if (!RackForm.rfidwriteflag)
            {
                 MessageBox.Show("RFID信息写入中，不允许返修!");
                return;
            }
            if (MainForm.linestart)
            {
                 MessageBox.Show("产线停止，请启动产线!");
                return;
            }
            int Gcodeloadreturn = OrderForm1.GcodeSenttoCNC(MainForm.cncv2list[1].MagNum, 2);
            if (Gcodeloadreturn == -1)
            {
                MessageBox.Show("没有找到匹配的G代码");
                return ;
            }
            else if (Gcodeloadreturn == 0)
            {
                 MessageBox.Show("G代码下发失败");
                return ;
            }
            OrderForm1.IsRerunning = true;
            OrderForm1.ReProcessChoose = false;
            ModbusTcp.MES_PLC_comfim_write_flage = true;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComReProcess;
            OrderForm1.rerunningflage = true;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {        
            OrderForm1.ReProcessChoose = false;
         }

        private void timer1_Tick(object sender, EventArgs e)
        {
            language = ChangeLanguage.GetDefaultLanguage();
            textBox1.Text = messagestring;
            textBox2.Text = compmessage;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool renewtoolflag = false;
            if (MainForm.cncv2list[1].IsConnected() == false)//产线复位中不能盘点
            {
                MessageBox.Show("铣床离线，不能自动补偿!");
                return;
            }
            else//自动补偿
            {
                //获取补偿数据
                for(int i = 0; i < 6; i++)
                {
                    var data =OrderForm1. AotoToolComDataList[i];

                    if (data.toolno != 0 && data.compValue != 0)
                    {
                        if(data.type==0)//长度
                        {

                            MainForm.cncv2list[1].TOOLDataChange[data.toolno].LengthCompAdd = data.compValue;
                            MainForm.cncv2list[1].TOOLDataChange[data.toolno].RadiusCompAdd = 0;
                            MainForm.cncv2list[1].TOOLDataChange[data.toolno].ToolChangeflag = true ;
                            renewtoolflag = true;
                        }
                        else//半径
                        {
                            MainForm.cncv2list[1].TOOLDataChange[data.toolno].LengthCompAdd =0;
                            MainForm.cncv2list[1].TOOLDataChange[data.toolno].RadiusCompAdd = data.compValue;
                            MainForm.cncv2list[1].TOOLDataChange[data.toolno].ToolChangeflag = true;
                            renewtoolflag = true;
                        }

                    }
                    else
                    {
                        MainForm.cncv2list[1].TOOLDataChange[data.toolno].RadiusCompAdd = 0;
                        MainForm.cncv2list[1].TOOLDataChange[data.toolno].LengthCompAdd =0;
                        MainForm.cncv2list[1].TOOLDataChange[data.toolno].ToolChangeflag = false;
                    }
                }
                //下发补偿值
                if(renewtoolflag )
                {
                    var ret = MainForm.collectdatav2.GetCNCDataLst[1].SetTool();
                    if (!ret)
                    {
                        MessageBox.Show("补偿失败，请确认网络是否连接！");
                    }
                    else
                    {
                        MessageBox.Show("自动刀补完成！");
                        renewtoolflag = true;

                    }
                }
            }
        }

        private void GetRoughToolData(int toolno, double Metervalue)
        {
            int Sign = 0;
            int Inter = 0;
            int dec = 0;

            DoubleToInt(MainForm.cncv2list[1].TOOLData[toolno - 1].Radius, ref Sign, ref Inter, ref dec);
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadiusWear_Positive] = Sign;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadius_int] = Inter;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadius_Float] = dec;

            DoubleToInt(MainForm.cncv2list[1].TOOLData[toolno - 1].Radius, ref Sign, ref Inter, ref dec);
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadiusWear_Positive] = Sign;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadiusWear_int] = Inter;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadiusWear_Float] = dec;


            DoubleToInt(Metervalue, ref Sign, ref Inter, ref dec);
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughMeter_Positive] = Sign;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughMeter_int] = Inter;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughMeter_Float] = dec;
      

        }
        /// <summary>
        /// 获取浮点数的符号位、整数、小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <param name="integepartr"></param>
        /// <param name="decimalpart"></param>
        private void DoubleToInt(double value,ref int sign, ref int integepart, ref int decimalpart)
        {
            sign = 0;
            integepart = 0;
            decimalpart = 0;
            if (value>=0)
            {
                sign =0;
            }
            else
            {
                sign = 1;
            }
            string temps = value.ToString("F2");
            if (temps != "null")
            {

                int index = temps.IndexOf(".");
                int flage = 1;
                if (Convert.ToDouble(temps) < 0)
                {
                    flage = -1;
                }
                string integepartrs = temps.Substring(0, index);//整数部分
                string decimalparts = temps.Substring(index + 1);//小数部分
                integepart = Convert.ToInt32(integepartrs);
                decimalpart = Convert.ToInt32(decimalparts);
            }
            else
            {
                sign = 0;
                integepart = 0;
                decimalpart = 0;
            }
        }
        private void GetFineToolData(int toolno,double Mertervalue)
        {
            int Sign = 0;
            int Inter = 0;
            int dec = 0;
            DoubleToInt(MainForm.cncv2list[1].TOOLData[toolno - 1].Radius, ref Sign, ref Inter, ref dec);
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadiusWear_Positive] = Sign;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadius_int] = Inter;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadius_Float] = dec;

            DoubleToInt(MainForm.cncv2list[1].TOOLData[toolno - 1].Radius, ref Sign, ref Inter, ref dec);
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadiusWear_Positive] = Sign;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadiusWear_int] = Inter;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadiusWear_Float] = dec;


            DoubleToInt(Mertervalue, ref Sign, ref Inter, ref dec);
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineMeter_Positive] = Sign;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineMeter_int] = Inter;
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineMeter_Float] = dec;

        }

    }
}
