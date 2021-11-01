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
    public partial class ToolCompForm : Form
    {
        public ToolCompForm()
        {
            InitializeComponent();
            
            textBoxA1.Text = "0.000";
            textBoxA2.Text = "0.000";
            textBoxA3.Text = "0.000";
            textBoxB1.Text = "0.000";
            textBoxB2.Text = "0.000";
            textBoxB3.Text = "0.000";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadius_Positive];
            int Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadius_int];
            int dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadius_Float];
            double val = 0.0;
            val = Inter + 0.001 * dec;
            if(Sign==1)
            {
                val = -1 * val;
            }
            textBoxA1.Text = val.ToString("F3");
             Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadiusWear_Positive];
             Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadiusWear_int];
             dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughRadiusWear_Float];
             val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxA2.Text = val.ToString("F3");

            Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughMeter_Positive];
            Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughMeter_int];
            dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughMeter_Float];
            val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxA3.Text = val.ToString("F3");

            Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughLength_Positive];
            Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughLength_int];
            dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughLength_Float];
            val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxA4.Text = val.ToString("F3");
            Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughLengthWear_Positive];
            Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughLengthWear_int];
            dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.RoughLengthWear_Float];
            val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxA5.Text = val.ToString("F3");

            Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadius_Positive];
            Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadius_int];
            dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadius_Float];
            val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxB1.Text = val.ToString("F3");

            Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadiusWear_Positive];
            Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadiusWear_int];
            dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineRadiusWear_Float];
            val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxB2.Text = val.ToString("F3");

            Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineMeter_Positive];
            Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineMeter_int];
            dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineMeter_Float];
            val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxB2.Text = val.ToString("F3");
            Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineLength_Positive];
            Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineLength_int];
            dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineLength_Float];
            val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxB4.Text = val.ToString("F3");
            Sign = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineLengthWear_Positive];
            Inter = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineLengthWear_int];
            dec = ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.FineLengthWear_Float];
            val = 0.0;
            val = Inter + 0.001 * dec;
            if (Sign == 1)
            {
                val = -1 * val;
            }
            textBoxB5.Text = val.ToString("F3");
        }

     
    }
}
