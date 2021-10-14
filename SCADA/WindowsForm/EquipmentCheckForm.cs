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
    public partial class EquipmentCheckForm : Form
    {

        Dictionary<int, string> RobortAlarmDic = new Dictionary<int, string>();
        Dictionary<int, string> LatheAlarmDic = new Dictionary<int, string>();
        Dictionary<int, string> CNCAlarmDic = new Dictionary<int, string>();
        Dictionary<int, string> PLCAlarmDic = new Dictionary<int, string>();

        public EquipmentCheckForm()
        {
            InitializeComponent();
            timer1.Enabled = true;
        }

        private void EquipmentCheckForm_Load(object sender, EventArgs e)
        {
            ChangeLanguage.LoadLanguage(this);//zxl 4.19
        }

        private void timer1_Tick(object sender, EventArgs e)
        {          
                UpDatadataGridView();
        }

   

        private void button_UpDataTable_Click(object sender, EventArgs e)
        {
            UpDatadataGridView();
        }

        private void UpDatadataGridView()
        {
            Dictionary<int, string> TempDic = new Dictionary<int, string>();
            if (!MainForm.robotconnect)
            {
                TempDic.Add(1, "离线");
            }
            else
            {
                RobortAlarmDic.Add(1, "在线");
            }
            if(RobortAlarmDic!= TempDic)
            {
                RobortAlarmDic.Clear();
                RobortAlarmDic = TempDic;
            }
            TempDic = new Dictionary<int, string>();

            if (!MainForm.SEquipmentlist[0].IsConnect)
            {
                TempDic.Add(1, "离线");
            }
            else
            {
                TempDic.Add(1, "在线");
                if(MainForm.cncv2list[0] != null && MainForm.cncv2list[0].Alarms.Count>0)
                {
             
                    foreach( var item in MainForm.cncv2list[0].Alarms)
                    {
                        TempDic.Add(1, item.Value);
                    }
                }
            }
            if (LatheAlarmDic != TempDic)
            {
                LatheAlarmDic.Clear();
                LatheAlarmDic = TempDic;
            }
            TempDic = new Dictionary<int, string>();
            if (!MainForm.SEquipmentlist[1].IsConnect)
            {
                TempDic.Add(1, "离线");
            }
            else
            {
                TempDic.Add(1, "在线");
                if (MainForm.cncv2list[1] != null&& MainForm.cncv2list[1].Alarms.Count > 0)
                {
         
                    foreach (var item in MainForm.cncv2list[0].Alarms)
                    {
                        TempDic.Add(1, item.Value);
                    }
                }
            }
            if (CNCAlarmDic != TempDic)
            {
                CNCAlarmDic.Clear();
                CNCAlarmDic = TempDic;
            }
            TempDic = new Dictionary<int, string>();
            if (!MainForm.PLC_SIMES_ON_line)
            {
                TempDic.Add(1, "离线");
            }
            else
            {
                TempDic.Add(1, "在线");
            }
            if (PLCAlarmDic != TempDic)
            {
                PLCAlarmDic.Clear();
                PLCAlarmDic = TempDic;
            }
           var TempDic1 = new Dictionary<string, string>();
            int i = 1;
            foreach( var item in RobortAlarmDic)
            {
                TempDic1.Add("机器人",item.Value);
            }
            foreach (var item in LatheAlarmDic)
            {
                TempDic1.Add("车床", item.Value);
            }
            foreach (var item in CNCAlarmDic)
            {
                TempDic1.Add("加工中心", item.Value);
            }
            foreach (var item in PLCAlarmDic)
            {
                TempDic1.Add("PLC", item.Value);
            }

            if (dataGridViewalarmdata.Visible)
            {
                int k = 0;
                if (dataGridViewalarmdata.Rows.Count != TempDic1.Count)
                {

                    dataGridViewalarmdata.Rows.Clear();
                    dataGridViewalarmdata.Rows.Add(TempDic1.Count);
                }
                foreach (var item in TempDic1)
                {
                    dataGridViewalarmdata.Rows[k].Cells[0].Value = k+1;
                    dataGridViewalarmdata.Rows[k].Cells[1].Value = item.Key.ToString();
                    dataGridViewalarmdata.Rows[k].Cells[2].Value = item.Value.ToString();
                    k++;

                }
             
            }
        }

 
      


        
    }
}
