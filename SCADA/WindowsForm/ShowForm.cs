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
        public string language = "";
        public ShowForm()
        {
            language = ChangeLanguage.GetDefaultLanguage();
            InitializeComponent();
            textBox1.Text = messagestring;       
        }

        private void button1_Click(object sender, EventArgs e)
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
            if ( MainForm.cncv2list[1].IsConnected() == false)//产线复位中不能盘点
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
                    MessageBox.Show("The line is stop，please start the line!");
                }
                else MessageBox.Show("产线停止，请启动产线!");
                return;
            }
            int Gcodeloadreturn = OrderForm1.GcodeSenttoCNC(MainForm.cncv2list[1].MagNum, 2);
            if (Gcodeloadreturn == -1)
            {
                if (language == "English")
                {
                    MessageBox.Show("The Gcode name err");
                }
                else MessageBox.Show("没有找到匹配的G代码");
                return ;
            }
            else if (Gcodeloadreturn == 0)
            {
                if (language == "English")
                {
                    MessageBox.Show("The Gcode download err");
                }
                else MessageBox.Show("G代码下发失败");
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
           
        }

     
    }
}
