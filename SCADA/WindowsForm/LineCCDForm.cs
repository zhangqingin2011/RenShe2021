﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HNCAPI;
using HNC_MacDataService;

namespace SCADA
{
    public partial class LineCCDForm : Form
    {
        int maxWidth, maxHeight;
        int maxRowsToDispCCD = 50;
        string[] columnTitle = { "索引", "时间戳", "CCD拍照指令", "自动识别数据", "读/写", "工序要求","  " };
        string[] resultStr = { "车",  "马", "相", "士", "炮", "卒", "将", "象", "兵"};
        private static Socket socketCCD;

        private int ccd_dbNo = -1;

        public LineCCDForm()
        {
            InitializeComponent();

            dataGridViewCCD.AllowUserToAddRows = true;
            dataGridViewCCD.ColumnCount = columnTitle.Length;
            dataGridViewCCD.Columns[0].Name = columnTitle[0];
            dataGridViewCCD.Columns[1].Name = columnTitle[1];
            dataGridViewCCD.Columns[2].Name = columnTitle[2];
            dataGridViewCCD.Columns[3].Name = columnTitle[3];
            dataGridViewCCD.Columns[4].Name = columnTitle[4];
            dataGridViewCCD.Columns[5].Name = columnTitle[5];
            dataGridViewCCD.Columns[6].Name = columnTitle[6];

            //hxb   2017.4.6
            maxWidth = Screen.PrimaryScreen.WorkingArea.Width;
            maxHeight = Screen.PrimaryScreen.WorkingArea.Height;

            textBoxCCDIP.Text = "161.254.1.120";
            textBoxCCDPort.Text = "1001";
            textBoxCCDSend.Text = "CC,0";
            textBoxCCDReceive.Text = "";
            socketCCD = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void LineCCDForm_Load(object sender, EventArgs e)
        {
            string[] STR_PARM_IN = { "序号", "CCD读写状态" };
            ChangeLanguage.LoadLanguage(this);

            int offset = 200;
            //布局设计
            this.Height = (maxHeight - 28) * 9 / 10 * 8 / 9 - 35;

            this.tabControl1.Width = this.Width / 2 - 2 - offset;
            this.dataGridViewCCD.Width = this.Width / 2 + offset;
            this.dataGridViewCCD.Left = this.tabControl1.Left + this.Width / 2 + 2 - offset;
            this.listViewCCDHisAlarm.Width = this.Width;

           this.tabControl1.Height = this.Height / 2;
            this.dataGridViewCCD.Height = this.tabControl1.Height;

            this.listViewCCDHisAlarm.Height = this.tabControl1.Height;
            this.listViewCCDHisAlarm.Top = this.tabControl1.Top + this.tabControl1.Height + 5;

            int[] columnHeadWidth1 = { this.listViewCCDHisAlarm.Width / 8, this.listViewCCDHisAlarm.Width * 7 / 8 };
            for (int col = 0; col < 2; col++)
            {
                listViewCCDHisAlarm.Columns.Add(STR_PARM_IN[col], columnHeadWidth1[col]);
            }

           // btnCCDConnect_Click(sender, e);

            //总控PLC只有一个  hxb  2017.4.27
            string ip = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
            MacDataService.GetInstance().GetMachineDbNo(ip, ref ccd_dbNo);
        }
/********************************************************************
	created:	2017/03/28
	created:	28:3:2017   8:49
	filename: 	D:\LatheLine\Latheline-宁夏职院-20170325\LatheLine\LineCCD.cs
	file path:	D:\LatheLine\Latheline-宁夏职院-20170325\LatheLine
	file base:	LineCCD
	file ext:	cs
	author:		wang wenjiang
	
	purpose:	连接CCD相机
*********************************************************************/
        private void btnCCDConnect_Click(object sender, EventArgs e)
        {
            if (btnCCDConnect.Text == "连接")
            {
                IPHostEntry iphost = Dns.Resolve(textBoxCCDIP.Text);
                IPAddress ipadress = iphost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipadress, Convert.ToInt32(textBoxCCDPort.Text));

                try
                {
                    socketCCD.Connect(ipEndPoint);
                }
                catch (System.Exception ex)
                {
                    //AddStatus( ex.ToString() );
                    AddStatus("CCD相机连接失败");
                }

                if (socketCCD.Connected)
                {
                    btnCCDConnect.Text = "断开连接";
                    AddStatus("CCD相机连接成功");
                }
            }
            else
            {
                btnCCDConnect.Text = "连接";
                socketCCD.Disconnect(false);//断开连接
                AddStatus("CCD相机已经断开连接.");
            }
        }

/********************************************************************
	created:	2017/03/28
	created:	28:3:2017   8:49
	filename: 	D:\LatheLine\Latheline-宁夏职院-20170325\LatheLine\LineCCD.cs
	file path:	D:\LatheLine\Latheline-宁夏职院-20170325\LatheLine
	file base:	LineCCD
	file ext:	cs
	author:		wang wenjiang
	
	purpose:	发送拍照指令
*********************************************************************/
        private void btnCCDSend_Click(object sender, EventArgs e)
        {
            ccdSend();  
        }

        private void dispRfidDataToGrid(string sendStr, string receiveData, string rwFlag)
        {
            int index = dataGridViewCCD.Rows.Add();
            if (index > maxRowsToDispCCD)
            {
                dataGridViewCCD.Rows.Clear();
                index = dataGridViewCCD.Rows.Add();
                //return;
            }
            dataGridViewCCD.Rows[index].Cells[0].Value = index.ToString();
            dataGridViewCCD.Rows[index].Cells[1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dataGridViewCCD.Rows[index].Cells[2].Value = sendStr;
            dataGridViewCCD.Rows[index].Cells[3].Value = receiveData;
            dataGridViewCCD.Rows[index].Cells[4].Value = rwFlag;

            string gongxuCnt = receiveData.Substring(4, 2);
            dataGridViewCCD.Rows[index].Cells[5].Value = gongxuCnt;

            //string gongxu1 = receiveData.Substring(6, 2);
            //if (gongxu1 == "00")
            //    dataGridViewCCD.Rows[index].Cells[6].Value = "未加工";
            //else
            //    dataGridViewCCD.Rows[index].Cells[6].Value = "已加工";

            //dataGridViewRFID.DataSource = 
        }

        private void AddStatus(string statusText)
        {
            ListViewItem subItem = new ListViewItem();

            int cnt = listViewCCDHisAlarm.Items.Count;
            if (cnt > maxRowsToDispCCD)
            {
                listViewCCDHisAlarm.Items.Clear();
                return;
            }
            subItem.Text = cnt.ToString();
            subItem.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss   ") + statusText);
            listViewCCDHisAlarm.Items.Add(subItem);
        }

/********************************************************************
	created:	2017/03/28
	created:	28:3:2017   9:57
	filename: 	D:\LatheLine\Latheline-宁夏职院-20170325\LatheLine\LineCCD.cs
	file path:	D:\LatheLine\Latheline-宁夏职院-20170325\LatheLine
	file base:	LineCCD
	file ext:	cs
	author:		wang wenjiang
	
	purpose:	
*********************************************************************/
        private void ccdSend( )
        {
            if (socketCCD.Connected != true)
            {
                //AddStatus("已断开连接，请重新连接");
                return;
            }
            string sendingMessage = textBoxCCDSend.Text;
            byte[] forwardMessage = Encoding.ASCII.GetBytes(sendingMessage);
            socketCCD.Send(forwardMessage);//发送指令 
            Thread.Sleep(1000); //延时接收
            try
            {
                string str = "";
                byte[] recivedBytes = new byte[1024];
                int totalByteRecive = socketCCD.Receive(recivedBytes, recivedBytes.Length, 0);
                str += Encoding.ASCII.GetString(recivedBytes, 0, totalByteRecive);
                textBoxCCDReceive.Text = str;
                
            }
            catch (System.Exception ex)
            {
                AddStatus("读取CCD识别数据失败.");  //ex.ToString()
            }

            //接收到的图片
            if (textBoxCCDReceive.Text == "")
                AddStatus("CCD相机数据读取失败!");
            else
            {
                int index = Convert.ToInt32(textBoxCCDReceive.Text);
                if (index > resultStr.Length) //没有建立模板，无法识别
                {
                    dispRfidDataToGrid(sendingMessage, "CCD相机无法识别", "Read");
                }
                else
                {
                    int regIndexB = 100;  //// B100 保存CCD识别物体类型   
                    MacDataService.GetInstance().HNC_RegSetValue((int)HncRegType.REG_TYPE_R, regIndexB, index, ccd_dbNo);

                    dispRfidDataToGrid(sendingMessage, resultStr[index], "Read");

                    dispRfidDataToGrid(sendingMessage, resultStr[index], "Read");

                    //switch (tet)
                    //{
                    //    case 0: pictureBox1.Image = xiangim; break;
                    //    case 1: pictureBox1.Image = maim; break;
                    //    case 2: pictureBox1.Image = shuaiim; break;
                    //    case 3: pictureBox1.Image = jiangim; break;
                    //    default: pictureBox1.Image = im; break;
                    //}
                }
            }     
        }

        private void monitorCCDSendAndReceive( )
        {
            int value = 0;
            int regIndexRead = 8;  //// 发送拍照指令R8.4   读取完成R8.5

            //HncApi.HNC_RegGetValue((int)HncRegType.REG_TYPE_R, regIndexRead, ref value, 2);
            Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_R, regIndexRead, out value, ccd_dbNo);

            if ((value & 0x10) == 0x10) //.0   
            {
                ccdSend( );
                Thread.Sleep(2000); //由于CCD处理需要一定时间，延时后再发送完成指令
                value &= 0x00; //清除读写标志
                if (!MainForm.bIsDebug)
                {
                    MacDataService.GetInstance().HNC_RegClrBit((int)HncRegType.REG_TYPE_R, regIndexRead, 4 , ccd_dbNo);//8.4 0x10
                    Thread.Sleep(200);
                    MacDataService.GetInstance().HNC_RegSetBit((int)HncRegType.REG_TYPE_R, regIndexRead, 5, ccd_dbNo);//8.5 0x20
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            monitorCCDSendAndReceive();
        }


    }
}
