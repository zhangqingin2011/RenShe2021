﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HNCAPI;

namespace SCADA.WindowsForm
{
    public partial class LineAlarmForm :Form
    {
        public LineAlarmForm()
        {
            InitializeComponent();
        }
        string[] AlarmContent = { "总控PLC与上位机通讯断开-->请检查通讯线或总控软件是否有问题",
                                  "总控急停-->请检查总控急停按钮",
                                  "RFID读超时-->请检查总控软件或线缆连接是否故障",
                                  "RFID写超时-->请检查总控软件或线缆连接是否故障",
                                  "RFID台上的物料是加工完成件工件",
                                  "RFID读失败-->请检查总控软件或线缆连接是否故障",
                                  "RFID写失败-->请检查总控软件或线缆连接是否故障",
                                  "RFID初始化失败-->请检查总控软件或线缆连接是否故障",
                                  "行走机器人报故障-->请排查行走机器人故障",
                                  "固定机器人报故障-->请排查固定机器人故障",
                                  "车床报警-->请排查车床故障",
                                  "钻攻中心报警-->请排查钻攻中心故障",
                                  "五轴加工单元报警-->请排查五轴加工单元故障",
                                  "总控PLC总线连接不正常"};
        DataTable alarmsource = new DataTable();

        private void UpdateDataGridView()
        {
            alarmsource.Rows.Clear();

            int value = 0;
            if (Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_G, 3010, out value, MainForm.plc_dbNo) == 0)
            {
                //Console.WriteLine("G3010=" + Convert.ToString(value));
                for (int i = 0, no = 1; i < AlarmContent.Length; i++)
                {
                    if ((value & (0x1 << i)) > 0)
                    {
                        alarmsource.Rows.Add(Convert.ToString(no), AlarmContent[i]);
                        no++;
                    }
                }
            }

            if (alarmsource.Rows.Count == 0)
            {
                dataGridView1.RowCount = 1;
                dataGridView1.Rows[0].Cells[0].Value = "";
                dataGridView1.Rows[0].Cells[1].Value = "";
            }
            else
            {
                dataGridView1.RowCount = alarmsource.Rows.Count;
                for (int ii = 0; ii < dataGridView1.Rows.Count; ii++)
                {
                    if (dataGridView1.Rows[ii].Displayed)
                    {
                        for (int jj = 0; jj < alarmsource.Columns.Count; jj++)
                        {
                            if (dataGridView1.Rows[ii].Cells[jj].Value != alarmsource.Rows[ii][jj])
                            {
                                dataGridView1.Rows[ii].Cells[jj].Value = alarmsource.Rows[ii][jj];
                            }
                        }
                    }
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MainForm.plc_dbNo != -1)
                UpdateDataGridView();
        }

        private void LineAlarmForm_Load(object sender, EventArgs e)
        {
            alarmsource.Columns.Add("序号");
            alarmsource.Columns.Add("故障内容");
            for (int i = 0; i < alarmsource.Columns.Count; i++)
            {
                alarmsource.Columns[i].ReadOnly = true;
            }

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
    }
}
