﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Collector;
using HFAPItest;
using HNC_MacDataService;
using HNCAPI;
using Sygole.HFReader;

namespace SCADA
{   
    public partial class RfidForm : Form
    {
        /// <summary>
        public static int  m_src = 0;//工件来源
        public static bool m_ReErr = true;//测量仪故障

        public string[] str_size = new string[6];
        public static bool m_IsWrong = true;//只发一次不合格

        private int rfid_dbNo = -1;

        int maxWidth, maxHeight;
        #region 所有自定义类实便化

        byte ReaderID = 0x00;//读写器ID
        bool Rfidflag = false;//#1RFID连接标志
        Opcode_enum opcode = Opcode_enum.ADDRESS_MODE;
        //bool Rfidflag1 = false;//#2RFID连接标志
        //bool Rfidflag2 = false;//#3RFID连接标志
        string IPAdress = "";//#1RFID IP 地址
        //string IPAdress1 = "";//#2RFID IP 地址
        //string IPAdress2 = "";//#3RFID IP 地址

        byte[] UID = new byte[9];
        string curUID = null; 
        const int maxRowsToDispRFID = 50;  //显示RFID记录上限
        byte blockStart = 0, blockCnt = 2 , blockSize = 4; //多块读写  blocksize只能4和8
   
        string initRFID = "3361030000000000";  //初始化标签  336103 
        string initOKRFID = "3361030101000000"; //合格
        string initNOKRFID = "3362030101000000"; //不合格

        private static string data = null, rfidUID = null;
        string rfidDataXMLname = ".RFIDdata.XML";
        string[] columnTitle = { "索引", "时间戳", "标签识别码", "标签数据", "读/写", "工序要求", "工序1", "工序2", "工序3", "工序4" };
        enum GONGJIAN
        {
            JU,
            MA,
            XIANG,
            SHI,
            PAO,
            ZU,
            JIANG
        };

        #endregion

        public RfidForm()
        {
            int i = 0;
            InitializeComponent();
            dataGridViewRFID.AllowUserToAddRows = true;
            dataGridViewRFID.ColumnCount = columnTitle.Length;
            dataGridViewRFID.Columns[0].Name = columnTitle[0];
            dataGridViewRFID.Columns[1].Name = columnTitle[1];
            dataGridViewRFID.Columns[2].Name = columnTitle[2];
            dataGridViewRFID.Columns[3].Name = columnTitle[3];
            dataGridViewRFID.Columns[4].Name = columnTitle[4];
            dataGridViewRFID.Columns[5].Name = columnTitle[5];
            dataGridViewRFID.Columns[6].Name = columnTitle[6];
            dataGridViewRFID.Columns[7].Name = columnTitle[7];
            dataGridViewRFID.Columns[8].Name = columnTitle[8];
            dataGridViewRFID.Columns[9].Name = columnTitle[9];

            maxWidth = Screen.PrimaryScreen.WorkingArea.Width;
            maxHeight = Screen.PrimaryScreen.WorkingArea.Height;

            int RFIDiiSum = int.Parse(MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]));

            //if (File.Exists("standard.ini"))  //读取公差设定文件的数据
            //{
            //    StreamReader strRead = new StreamReader("standard.ini",Encoding.Default);
            //    while(!strRead.EndOfStream)
            //    {
            //        str_size[i] = strRead.ReadLine();
            //        i++;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("公差设定文件standard.ini不存在!");
            //}

            tabControl1.TabPages.Remove(tabPageSet);  //隐藏参数设置界面
            mComboBoxData.SelectedIndex = 0;
        }


        private void RfidForm_Load(object sender, EventArgs e)
        {
            ChangeLanguage.LoadLanguage(this);
            string[] STR_PARM_IN = { "序号", "RFID标签读写状态" };
            int offset = 200;
            //布局设计

            this.Height = (maxHeight - 28) * 9 / 10 * 8 / 9 - 35;

            this.tabControl1.Width = this.Width / 2 - 2 - offset;
            this.dataGridViewRFID.Width = this.Width / 2 + offset;
            this.dataGridViewRFID.Left = this.tabControl1.Left + this.Width / 2 + 2 - offset;
            this.listView2.Width = this.Width;

            this.tabControl1.Height = this.Height / 2;
            this.dataGridViewRFID.Height = this.tabControl1.Height;

            this.listView2.Height = this.tabControl1.Height;
            this.listView2.Top = this.tabControl1.Top + this.tabControl1.Height + 5;

            //this.panel1.Width = this.Width;
            //this.panel1.Height = (((MainForm)this.MdiParent).rightForm.Height - 6) / 2;
            

            int[] columnHeadWidth1 ={this.listView2.Width / 8,this.listView2.Width*7 / 8 };
            for (int col = 0; col < 2; col++)
            {
                listView2.Columns.Add(STR_PARM_IN[col], columnHeadWidth1[col]);
            }

           // btnConnect_Click(sender, e); 

            //总控PLC只有一个  hxb  2017.4.27
            string ip = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, 0, m_xmlDociment.Default_Attributes_str1[(int)m_xmlDociment.Attributes_str1.ip]);
            MacDataService.GetInstance().GetMachineDbNo(ip, ref rfid_dbNo);

            ///添加RFID XML文件初始化 2017.4.26 zxl,再次连接
            int RFIDiiSum = int.Parse(MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]));
            string[] Attributesstr = new String[m_xmlDociment.Default_Attributes_RFID.Length];
            IPAdress = Attributesstr[13] = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[13]);//读写器IP地址
            Attributesstr[14] = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, 0, m_xmlDociment.Default_Attributes_RFID[14]);//读写器端口
            ConnectRFID(Attributesstr[13], int.Parse(Attributesstr[14]));//连接读写器1
       
        }

        private void RfidForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainForm.SGreader0.ConnectStatus == ConnectStatusEnum.CONNECTED)
            {
                try
                {
                    MainForm.SGreader0.DisConnect();
                }
                catch (Exception ex)
                {
                      //连接按钮旁的状态标签
                     AddStatus(ex.Message);
                }
            }

            this.Dispose();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            monitorRFIDReadAndWrite(IPAdress);
        }

/********************************************************************
	created:	2017/04/26
	created:	24:3:2017   17:20
	filename: 	D:\LatheLine\Latheline-武汉城市学院-20170323\LatheLine\LineDetect.cs
	file path:	从XML文件直接读取RFID的IP地址和端口连接
	file base:	LineDetect
	file ext:	cs
	author:		zhangxinglan
	
	purpose:	连接RFID
*********************************************************************/
        private void ConnectRFID(string IPAdress,int Port)
        {
             Rfidflag = MainForm.SGreader0.Connect(IPAdress, Port);
            if (Rfidflag)
            {
                //tabControl1.Enabled = true;
                //btnConnect.Text = "断开";
                //textBoxIPRfid.Enabled = false;
                //textBoxPort.Enabled = false;
                //textBoxIPRfid2.Enabled = false;
                //textBoxPort2.Enabled = false;
                //textBoxIPRfid3.Enabled = false;
                //textBoxPort3.Enabled = false;
                       
                //IPAdress0 = GetIPAdress(0);//读写器1 IP地址
                //IPAdress1 = GetIPAdress(1);//读写器2 IP地址
                //IPAdress2 = GetIPAdress(2);//读写器3 IP地址    
                AddStatus("RFID连接成功！  IP=" + IPAdress+"   Port=3001");                
            }
            else
            {
                AddStatus("读写器连接失败.请确保参数地址和线缆连接正确.");
            }                 
        }

 

        /// <summary>
        /// 写多块
        /// </summary>
        /// <param name="flag">读写器系列号 0:#1RFID  1：#2RFID  2：#3RFID</param>
        private void Writeblock00(int flag)
        {
            Opcode_enum Opcode = new Opcode_enum();
            byte BlockSize = 4;//块大小  
            string str=null;

            string MenStr=ReadMem(ref str);
            //AddStatus(MenStr);
            string str1 = MenStr.Remove(6, 2);
            string str2 = str1.Insert(6, "01");
            byte[] BlockDatas0 = tool.HexStringToByteArray(str2);//00 替换成 01

            if (Status_enum.SUCCESS == MainForm.SGreader0.WriteMBlock(ReaderID, Opcode, UID, blockStart, blockCnt, BlockSize, BlockDatas0))
            {
                AddStatus("写多块成功");
            }
        }

        private void monitorRFIDReadAndWrite(string Adress)
        {
            int value = 0;
            byte len = 0;
   
            int regIndexRead = 5;  // R5.0 为RFID读生料信息    R5.1 为RFID读生料完成
            string tmpUID = null;
            Antenna_enum ant = Antenna_enum.ANT_1;
            Opcode_enum Opcode = new Opcode_enum();
            byte[] datas = new byte[9];
            int pos = 0;
            int length = datas.Length - 1;

             Collector.CollectShare.GetRegValue((int)HncRegType.REG_TYPE_R, regIndexRead, out value, rfid_dbNo);

            //initMblock(0);//初始化

            if ((value & 0x01) == 0x01)//R5.0为RFID读生料信息
            {
                readUID(0, ref tmpUID);//读取UID
                if (Status_enum.SUCCESS == MainForm.SGreader0.ReadMBlock(0, Opcode, UID, blockStart, blockCnt, ref datas, ref len, ant))
                {
                    //获取标签内存
                    string str = tool.ByteToHexString(datas, pos, length);
                    string MemSum = str.Substring(0, 4);//0~4内存数据 即3361
                    string gongxu1 = str.Substring(6, 2);//6~7 工序1数据  00 为生料 01 为熟料

                    if (MemSum == "0000")// //若标签为全为空 进行初始化为 3361030000000000 即3361为空
                    {
                        initMblock(0);//进行初始化
                    }
                    else if (MemSum != "0000")//已经初始化
                    {
                        if (gongxu1 == "01")
                        {
                            initMblock(0);//重新初始化           
                        }
                        dispRfidDataToGrid(curUID, str, "Read");
                    }

                    //AddStatus("#1RFID初始化成功");
                }
                //熟料 R35.0 为0
                //MacDataService.GetInstance().HNC_RegClrBit((int)HncRegType.REG_TYPE_R, regSecondRfid, 0, rfid_dbNo);
                //Thread.Sleep(100);   
                //R5.1 读取完成标志
                MacDataService.GetInstance().HNC_RegSetBit((int)HncRegType.REG_TYPE_R, regIndexRead, 1, rfid_dbNo); 
            }
            else if ((value & 0x04) == 0x04)//R5.2为RFID写熟料信息
            {
                readUID(0, ref tmpUID);//读取UID
                if (Status_enum.SUCCESS == MainForm.SGreader0.ReadMBlock(0, Opcode, UID, blockStart, blockCnt, ref datas, ref len, ant))
                {              
                    //获取标签内存
                    string str = tool.ByteToHexString(datas, pos, length);
                    string MemSum = str.Substring(0, 4);//0~4内存数据 即3361
                    string gongxu1 = str.Substring(6, 2);//8~9 工序2数据  00 为生料 01 为熟料

                    if (MemSum == "0000")// //若标签为全为空 进行初始化为 3361030000000000 即3361为空
                    {
                        initMblock(0);//进行初始化
                    }
                    else if (MemSum != "0000")
                    {
                        if (gongxu1 == "00")
                        {
                            dispRfidDataToGrid(curUID, str, "Write");
                            Writeblock00(0);//写进去
                        }
                        else if (gongxu1 == "01")  //已写过
                        {
                            dispRfidDataToGrid(curUID, str, "Write");
                        }
                    }
                    //AddStatus("#2RFID初始化成功");
                }
               //R5.3 熟料写完成标记
               MacDataService.GetInstance().HNC_RegSetBit((int)HncRegType.REG_TYPE_R, regIndexRead, 3, rfid_dbNo);

            }

        }

        private bool readRFID()
        {
            if (readUID(0,ref rfidUID))
            {
                //textBoxUID.text
                if (readMblock(ref data))
                {
                    if (data.Length < 16) //初始化RFID标签
                    {
                        initMblock(0);
                        data = initRFID;
                    }
                    dispRfidDataToGrid(rfidUID, data, "Read" );
                }
            }

            return true;
        }


        /********************************************************************
            created:	2017/05/03
            author:		zhangxinglan 	
            purpose:	初始化RFID标签
         *  格式：16位
         *  0~4：物料类型,默认 3361
         *  5~6:工序数,   默认 03，表示有3个工序
         *  7~8:工序1加工情况，默认00：表示未加工；    01：表示已加工
         *  9~10:工序2加工情况，默认00：表示未加工；   01：表示已加工
         *  11~12:工序3加工情况，默认00：表示未加工；  01：表示已加工
         *  13~14:工序4加工情况，默认00：表示未加工；  01：表示已加工
         *  15~16:工序5加工情况，默认00：表示未加工；  01：表示已加工
        *********************************************************************/
        /// <summary>
        /// 初始化块
        /// </summary>
        /// <param name="flag"></param>
        private bool initMblock(int flag)
        {
            Opcode_enum Opcode = new Opcode_enum();
            byte BlockSize = 4;//块大小    
            byte[] BlockDatas = tool.HexStringToByteArray(initRFID);

            if (Status_enum.SUCCESS == MainForm.SGreader0.WriteMBlock(ReaderID, Opcode, UID, blockStart, blockCnt, BlockSize, BlockDatas))
            {
                AddStatus("RFID 初始化多块：成功！");
                dispRfidDataToGrid(curUID, initRFID, "Init");
                return true;
            }
            return false;
        }
  

        private bool readMblock(ref string blockData)
        {
            byte [] ReceiveDatas = new byte[64];
            byte dataLen = 0;
            Antenna_enum ant = Antenna_enum.ANT_1;
            if (Status_enum.SUCCESS == MainForm.SGreader0.ReadMBlock(ReaderID, opcode, UID, blockStart, blockCnt, ref ReceiveDatas, ref dataLen, ant))
            {
                blockData = ConvertMethod.ByteToHexString(ReceiveDatas, 0, dataLen, "");
                 AddStatus("读标签多块：成功！");
                 return true;
             }
            else
                  return false;                
        }

        
        private  void dispRfidDataToGrid(string uid, string data , string rwFlag)
        {
            int index = dataGridViewRFID.Rows.Add();
            if (index > maxRowsToDispRFID)
            {
                dataGridViewRFID.Rows.Clear();
                index = dataGridViewRFID.Rows.Add();
                //return;
            }
            dataGridViewRFID.Rows[index].Cells[0].Value = index.ToString();
            dataGridViewRFID.Rows[index].Cells[1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dataGridViewRFID.Rows[index].Cells[2].Value = uid;
            dataGridViewRFID.Rows[index].Cells[3].Value = data;//数据
            dataGridViewRFID.Rows[index].Cells[4].Value = rwFlag;//工序需求

            string gongxuCnt = data.Substring(4, 2);
            dataGridViewRFID.Rows[index].Cells[5].Value = gongxuCnt;//03 3个工序

            string gongxu1 = data.Substring(6, 2);//#1RFID
            if(gongxu1 == "00")
                dataGridViewRFID.Rows[index].Cells[6].Value = "生料";
            else
                dataGridViewRFID.Rows[index].Cells[6].Value = "熟料";

            //string gongxu2 = data.Substring(8, 2);//#2RFID
            //if (gongxu2 == "00")
            //    dataGridViewRFID.Rows[index].Cells[7].Value = "生料";
            //else
                dataGridViewRFID.Rows[index].Cells[7].Value = "熟料";

        }

        private void AddStatus(string statusText)
        {
            ListViewItem subItem = new ListViewItem();

            int cnt = listView2.Items.Count;
            if (cnt > maxRowsToDispRFID)
            {
                listView2.Items.Clear();
                return;
            }

            subItem.Text = cnt.ToString();
            subItem.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss   ") + statusText);
            listView2.Items.Add(subItem);
        }
        
        private void readHisRfidDataToGrid(string fileName)
        {
            DataSet xmlData = new DataSet();

            if (File.Exists(fileName))
            {
                xmlData.ReadXml(fileName, XmlReadMode.Auto);
                dataGridViewRFID.Columns.Clear();
                dataGridViewRFID.DataSource = xmlData.Tables[3];
            }
            else
            {
                MessageBox.Show("RFID历史数据的xml文件不存在");
            }
        }        

        /// <summary>
        /// 读UID
        /// </summary>
        /// <param name="flag">读写器序列 0:#1RFID  1：#2RFID  2：#3RFID</param>
        /// <param name="rfidUID">UID</param>
        /// <returns></returns>
        private bool readUID(int flag, ref string rfidUID)
        {
            if (MainForm.SGreader0.ConnectStatus != ConnectStatusEnum.CONNECTED)
                return false;

            int pos = 0;
            int length = UID.Length - 1;

            if (MainForm.SGreader0.Inventory(ReaderID, ref UID) == Status_enum.SUCCESS)
            {
                curUID = rfidUID = ConvertMethod.ByteToHexString(UID, pos, length);
                AddStatus("读标签UID：成功！");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读多块
        /// </summary>
        /// <param name="flag">读写器系列号 0:#1RFID  1：#2RFID  2：#3RFID</param>
        private string ReadMem(ref string MenStr)
        {
            Antenna_enum ant = Antenna_enum.ANT_1;
            Opcode_enum Opcode = new Opcode_enum();
            byte[] datas = new byte[9];
            byte len = 0;
            int pos = 0;
            int length = datas.Length - 1;
 //           string MenStr = "";

            if (Status_enum.SUCCESS == MainForm.SGreader0.ReadMBlock(0, Opcode, UID, 0, blockCnt, ref datas, ref len, ant))
            {
                MenStr = tool.ByteToHexString(datas, pos, length);
                AddStatus("读多块成功");
            }
            return MenStr;
        }
        /// <summary>
        /// 写多块
        /// </summary>
        /// <param name="flag">读写器系列号 0:#1RFID  1：#2RFID  2：#3RFID</param>
        private void Writeblock(ref string WriteMenStr)
        {
            Opcode_enum Opcode = new Opcode_enum();
            byte BlockSize = 4;//块大小    
            byte[] BlockDatas = tool.HexStringToByteArray(WriteMenStr);

            if (Status_enum.SUCCESS == MainForm.SGreader0.WriteMBlock(ReaderID, Opcode, UID, blockStart, blockCnt, BlockSize, BlockDatas))
                AddStatus("写多块成功");
        }
     
        private void btnReadMem_Click(object sender, EventArgs e)
        {
            string str = null;
            readUID(0, ref str);
            textBoxUID.Text = str;
            str = null;
            ReadMem(ref str);//读多块1
            tBMemData.Text = str;          
        }

        private void btnWriteMem_Click(object sender, EventArgs e)
        {
            string str = null;
            readUID(0, ref str);
            textBoxUID.Text = str;
            switch(mComboBoxData.SelectedIndex)
            {
                case 1:  //合格
                    str = initOKRFID;
                    break;
                case 2: //不合格
                    str = initNOKRFID;
                    break;
                default:
                case 0:  //生料
                    str = initRFID;
                    break;

            }
            Writeblock(ref str);
        }

        private void btnInitMem_Click(object sender, EventArgs e)
        {
            if (initMblock(0))
                tBInitState.Text = "初始化成功！";
            else
                tBInitState.Text = "初始化失败！";
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            byte nStart, nCnt;
            bool flg1 = Byte.TryParse(tBStart.Text, out nStart);
            bool flg2 = Byte.TryParse(tBCount.Text, out nCnt);
            if (flg1 && flg2)
            {
                blockStart = nStart;
                blockCnt = nCnt;

                switch (comboBoxBlockSize.SelectedIndex)
                {
                    case 0: blockSize = 4;
                        break;
                    case 1: blockSize = 8;
                        break;
                }
            }
            else
            {
                MessageBox.Show("请输入整数！");
                blockStart = 0;
                blockCnt = 2;
            }
        }

        private void btnReadUID_Click(object sender, EventArgs e)
        {
            string tmpUID = null;
            readUID(0, ref tmpUID);
            textBoxUID.Text = tmpUID;
        }
    }
}

