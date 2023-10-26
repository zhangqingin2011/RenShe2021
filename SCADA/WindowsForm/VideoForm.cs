﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using HNC_MacDataService;
using System.Collections;

namespace SCADA
{
    public partial class VideoForm : Form
    {

        private static Int32 m_lUserID = -1;
        private Int32 m_lRealHandle1 = -1;
        private Int32 m_lRealHandle2 = -1;
        private bool m_bInitSDK = false;
        private  uint iLastErr = 0;
        private string strErr;
        private  string str = "";
        private static int curchano = -1;//当前通道号
        public CHCNetSDK.NET_DVR_DEVICEINFO_V30 m_struDeviceInfo;
        public CHCNetSDK.NET_DVR_DEVICECFG_V40 m_struDeviceCfg;
        AutoSizeFormClass ascsize = new AutoSizeFormClass();
        public CHCNetSDK.NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40;
        CHCNetSDK.REALDATACALLBACK RealData = null;

       // public string language = "";
        public struct CHAN_INFO
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.U4)]
            public Int32[] lChannelNo;
            public void Init()
            {
                lChannelNo = new Int32[256];
                for (int i = 0; i < 256; i++)
                    lChannelNo[i] = -1;
            }
        }
        public CHAN_INFO m_struChanNoInfo = new CHAN_INFO();


        public VideoForm()
        {
            InitializeComponent();
            m_bInitSDK = CHCNetSDK.NET_DVR_Init();

            string language = ChangeLanguage.GetDefaultLanguage();
            if (m_bInitSDK == false)
            {

                string strtemp = ("设备初始化失败!");
                //if (language == "English")
                //{
                //    strtemp = ("It's failure to initialize!");
                //}
                renewtextBoxOprate(strtemp);
                return;
            }
            else
            {
                //保存SDK日志 To save the SDK log
                string strtemp = ("设备初始化成功!");
                //if (language == "English")
                //{
                //    strtemp = ("It's Succeed to initialize!");
                //}
                renewtextBoxOprate(strtemp);
                return;
            }
        }

        private void VideoForm_Load(object sender, EventArgs e)
        {
            ChangeLanguage.LoadLanguage(this);
            //textBoxIP.Text = "192.168.8.30";

            textBoxIP.Text = MainForm.VIDEOAddress;
            textBoxport.Text = "8000";
            textBoxname.Text = "admin";
            comboBoxchannel.SelectedIndex = -1;
            comboBoxchannel.Text = "";
            comboBoxstream.Items.Add("Streamcode1");//码流类型：0-主码流，1-子码流，2-码流3，3-码流4
            comboBoxstream.Items.Add("Streamcode2");
            comboBoxstream.Items.Add("Streamcode3");
            comboBoxstream.Items.Add("Streamcode4");
            comboBoxstream.SelectedIndex = 0;
            //标识符中英文字体切换

        }
 

        private void textBoxIP_KeyPress(object sender, KeyPressEventArgs e)//.的ascii码是46
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {

               string language = ChangeLanguage.GetDefaultLanguage();
                string strtemp = ("设置IP时，请输入数字或者'.'!");
                //if (language == "English")
                //{
                //    strtemp = ("Please set number or '.'!");
                //}
                renewtextBoxOprate(strtemp);
                   // Message.Show("请输入数字或者'.'");
                textBoxIP.Focus();
            }
        }

        private void renewtextBoxOprate(string str)
        {
            string timenow = "";
            timenow = DateTime.Now.ToString();
            textBoxOprate.AppendText(timenow + ":  "+ str + "\r\n");
            textBoxOprate.ScrollToCaret();
        }
        private void renewtextBoxEvent(string str)
        {
            string timenow = "";
            timenow = DateTime.Now.ToString();
            textBoxEvent.AppendText(timenow + ":  " + str + "\r\n");
            textBoxEvent.ScrollToCaret();
        }
        private int getiparrformstring(string str)
        {
            int num;
            int[] str1 = { 0, 0,0 };
            for (int i = 0; i < str.Length; i++)
            {
                char item = str.ElementAt(i);
                if (item <= '9' && item >= '0')
                {
                    str1[2] = str1[1];
                    str1[1] = str1[0];
                    str1[0] = (int)(item) - (int)('0');
                }
                else
                    return -1;
            }
            return num =str1[2]*100+ str1[1] * 10 + str1[0];
        }
        private int[] getipformstring(string str,out bool result)
        {
            int[] ip = { 0, 0,0,0 };
            string[] ipstr = { "", "", "", "" };
            int arrlength = str.Length;
            char[] ipchar = new char[arrlength];
            int j = 0;
            int notelength = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char item = str.ElementAt(i);
                 ipchar[i] = item;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (ipchar[i]!='.')
                {
                    notelength++;
                    string temp =ipchar[i].ToString();
                    ipstr[j] = ipstr[j] + temp;
                   // ipstr[j].Insert(ipstr[j].Length-1, temp);
                }
                else
                {
                    if (notelength > 3)
                    {
                        result = false;
                        return ip;
                    }
                    else
                    {
                        notelength = 0;
                        j++;
                        if (j > 4)
                        {
                            result = false;
                            return ip;
                        }
                    }
                }
            }
            for (int i = 0; i< 4;i++ )
            {
                ip[i] = getiparrformstring(ipstr[i]);
            }
            result = true;
            return ip;
        }
        private void textBoxIP_Leave(object sender, EventArgs e)
        {
            bool result = true;
            string textBoxIPStr = ((TextBox)sender).Text;

            string language = ChangeLanguage.GetDefaultLanguage();
            if (textBoxIPStr == "")
            {
                return;
            }
            int[] ip = getipformstring(textBoxIPStr, out result);
            bool right = true;
            if (!result)
            {
                string strtemp = "ip地址格式错误，请重新填写正确的ip地址!";
                //if (language == "English")
                //{
                //    strtemp = ("Ip set error,Please set correctly!");
                //}
                renewtextBoxOprate(strtemp);
                textBoxIP.Clear();
                textBoxIP.Focus();
                return;
            }
            for (int i = 0; i < 4;i++ )
            {
                if (ip[i] > 255 || ip[i] < 0)
                {
                    right = false;
                }
            }
            if (!right)
            {
                string strtemp = "ip地址超出范围错误，请重新填写正确的ip地址";
                //if (language == "English")
                //{
                //    strtemp = ("Ip address is error,Please set correctly!");
                //}
                renewtextBoxOprate(strtemp);
                textBoxIP.Clear();
                textBoxIP.Focus();
            }
            else
            {
               //输入正确的处理
            }
        }

        private void textBoxport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter )
            {
                e.Handled = false;
            }
            else
            {

                string language = ChangeLanguage.GetDefaultLanguage();
                string strtemp = "请输入数字";
                //if (language == "English")
                //{
                //    strtemp = ("Please enter number!");
                //}
                renewtextBoxOprate(strtemp);
                textBoxport.Focus();
            }
        }

        private void textBoxport_Leave(object sender, EventArgs e)
        {
            string textBoxportStr = ((TextBox)sender).Text;
            if (textBoxportStr == "")
            {
                return;
            }
            bool right = true;
            for (int i = 0; i < textBoxportStr.Length; i++)
            {
                char temp = textBoxportStr.ElementAt(i);
                if (temp < '0' || temp > '9')
                {
                    right = false;
                }
            }
            if (!right)
            {

                string language = ChangeLanguage.GetDefaultLanguage();
                string strtemp = "端口错误，请输入数字";
                //if (language == "English")
                //{
                //    strtemp = ("The Port si error,Please enter number!");
                //}
                renewtextBoxOprate(strtemp); 
                textBoxport.Focus();
            }
            else
            {
                //输入正确的处理
            }
        }

        /***获取通道***/
        public void GetDevChanList()
        {
            int i = 0, j = 0;
            string str;

            string language = ChangeLanguage.GetDefaultLanguage();
            m_struChanNoInfo.Init();
            comboBoxchannel.Items.Clear();
            uint dwDChanTotalNum = (uint)m_struDeviceInfo.byIPChanNum + 256 * (uint)m_struDeviceInfo.byHighDChanNum;

            if (dwDChanTotalNum > 0)
            {
                uint dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40);

                IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
                Marshal.StructureToPtr(m_struIpParaCfgV40, ptrIpParaCfgV40, false);

                uint dwReturn = 0;
                int iGroupNo = 0;  //该Demo仅获取第一组64个通道，如果设备IP通道大于64路，需要按组号0~i多次调用NET_DVR_GET_IPPARACFG_V40获取

                if (!CHCNetSDK.NET_DVR_GetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    strErr = "获取IP通道信息失败, 错误号" + iLastErr;
                    //if (language == "English")
                    //{
                    //    strErr = "Get IP channel failure,error No." + iLastErr;
                    //}
                    //获取IP通道信息失败，输出错误号 Failed to Get IP Channel info and output the error code

                    renewtextBoxOprate(strErr); 
                }
                else
                {
                    m_struIpParaCfgV40 = (CHCNetSDK.NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(CHCNetSDK.NET_DVR_IPPARACFG_V40));

                    //获取可用的模拟通道
                    for (i = 0; i < m_struIpParaCfgV40.dwAChanNum; i++)
                    {
                        if (m_struIpParaCfgV40.byAnalogChanEnable[i] == 1)
                        {
                            str = String.Format("CH{0}", i + 1);
                            comboBoxchannel.Items.Add(str);
                            m_struChanNoInfo.lChannelNo[j] = i + m_struDeviceInfo.byStartChan;
                            j++;
                        }
                    }

                    //获取前64个IP通道中的在线通道
                    uint iDChanNum = 64;

                    if (dwDChanTotalNum < 64)
                    {
                        iDChanNum = dwDChanTotalNum; //如果设备IP通道小于64路，按实际路数获取
                    }

                    byte byStreamType;
                    for (i = 0; i < iDChanNum; i++)
                    {
                        byStreamType = m_struIpParaCfgV40.struStreamMode[i].byGetStreamType;
                        CHCNetSDK.NET_DVR_STREAM_MODE m_struStreamMode = new CHCNetSDK.NET_DVR_STREAM_MODE();
                        dwSize = (uint)Marshal.SizeOf(m_struStreamMode);
                        switch (byStreamType)
                        {
                            //0- 直接从设备取流 0- get stream from device directly
                            case 0:
                                IntPtr ptrChanInfo = Marshal.AllocHGlobal((Int32)dwSize);
                                Marshal.StructureToPtr(m_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfo, false);
                                CHCNetSDK.NET_DVR_IPCHANINFO m_struChanInfo = new CHCNetSDK.NET_DVR_IPCHANINFO();
                                m_struChanInfo = (CHCNetSDK.NET_DVR_IPCHANINFO)Marshal.PtrToStructure(ptrChanInfo, typeof(CHCNetSDK.NET_DVR_IPCHANINFO));

                                //列出IP通道 List the IP channel
                                if (m_struChanInfo.byEnable == 1)
                                {
                                    str = String.Format("IP CH{0}", i + 1);
                                    comboBoxchannel.Items.Add(str);
                                    m_struChanNoInfo.lChannelNo[j] = i + (int)m_struIpParaCfgV40.dwStartDChan;
                                    j++;
                                }
                                Marshal.FreeHGlobal(ptrChanInfo);
                                break;
                            //6- 直接从设备取流扩展 6- get stream from device directly(extended)
                            case 6:
                                IntPtr ptrChanInfoV40 = Marshal.AllocHGlobal((Int32)dwSize);
                                Marshal.StructureToPtr(m_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfoV40, false);
                                CHCNetSDK.NET_DVR_IPCHANINFO_V40 m_struChanInfoV40 = new CHCNetSDK.NET_DVR_IPCHANINFO_V40();
                                m_struChanInfoV40 = (CHCNetSDK.NET_DVR_IPCHANINFO_V40)Marshal.PtrToStructure(ptrChanInfoV40, typeof(CHCNetSDK.NET_DVR_IPCHANINFO_V40));

                                //列出IP通道 List the IP channel
                                if (m_struChanInfoV40.byEnable == 1)
                                {
                                    str = String.Format("IP CH{0}", i + 1);
                                    comboBoxchannel.Items.Add(str);
                                    m_struChanNoInfo.lChannelNo[j] = i + (int)m_struIpParaCfgV40.dwStartDChan;
                                    j++;
                                }
                                Marshal.FreeHGlobal(ptrChanInfoV40);
                                break;
                            default:
                                break;
                        }
                    }
                }
                Marshal.FreeHGlobal(ptrIpParaCfgV40);
            }
            else
            {
                for (i = 0; i < m_struDeviceInfo.byChanNum; i++)
                {
                    str = String.Format("CH{0}", i + 1);
                    comboBoxchannel.Items.Add(str);
                    m_struChanNoInfo.lChannelNo[j] = i + m_struDeviceInfo.byStartChan;
                    j++;
                }
            }
            if (comboBoxchannel.Items.Count>1)
            {
                comboBoxchannel.SelectedIndex = 0 ;
            }
            
        }
        private void btmlogin_Click(object sender, EventArgs e)
        {

            string language = ChangeLanguage.GetDefaultLanguage();
            if (textBoxIP.Text == "" || textBoxport.Text == "" ||
                textBoxname.Text == "" || textBoxkey.Text == "")
            {

               // string language = ChangeLanguage.GetDefaultLanguage();
                string strtemp = "请输入登录参数!";
                //if (language == "English")
                //{
                //    strtemp = "Please enter login parameters";
                //}
                renewtextBoxOprate(strtemp); 
                //MessageBox.Show("请输入登录参数!");
                return;
            }
            if (m_lUserID < 0)
            {
                string DVRIPAddress = textBoxIP.Text; //设备IP地址或者域名
                Int16 DVRPortNumber = Int16.Parse(textBoxport.Text);//设备服务端口号
                string DVRUserName = textBoxname.Text;//设备登录用户名
                string DVRPassword = textBoxkey.Text;//设备登录密码

                //CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
                m_struDeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

                //登录设备 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref m_struDeviceInfo);

            //    m_lUserID = 1;

                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    
                    str = "登录失败，错误号：" + iLastErr; //登录失败，输出错误号
                    //if (language == "English")
                    //{
                    //    str = "Login failure,errorNo." + iLastErr;
                    //}
                    renewtextBoxEvent(str); 
                    //MessageBox.Show(str);
                    return;
                }
                else
                {
                    //登录成功
                    renewtextBoxEvent("登录成功!"); 
                    //MessageBox.Show("登录成功!");
                    labelstat1.Text = "已登录";
                    //if (language == "English")
                    //{
                    //    renewtextBoxEvent("Login succeed!");
                    //    labelstat1.Text = "Already login" ;
                    //}
                    labelstat1.ForeColor = System.Drawing.Color.FromArgb(0, 255, 0);
                    //picturelog.BackColor = System.Drawing.Color.FromArgb(0, 255, 0);
           GetDevChanList(); 
                }

            }
            else
            {
                //注销登录 Logout the device
                if (m_lRealHandle1 >= 0)
                {
                   
                    string strtemp = "通道1：请先关闭预览!";
                    //if (language == "English")
                    //{
                    //    strtemp = "Ch1:Please close preview";
                    //}
                    renewtextBoxOprate(strtemp); 
                   // MessageBox.Show("请先关闭预览");
                    return;
                }
                else if (m_lRealHandle2 >= 0)
                {
                    string strtemp = "通道2：请先关闭预览!";
                    //if (language == "English")
                    //{
                    //    strtemp = "Ch2:Please close preview";
                    //}
                    renewtextBoxOprate(strtemp); 
                  //  MessageBox.Show("请先关闭预览");
                    return;
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "退出登录失败, 错误号" + iLastErr;
                    //if (language == "English")
                    //{
                    //    str = " Exit login failure,error No." + iLastErr;
                    //}
                    renewtextBoxEvent(str);
                    // MessageBox.Show(str);
                    return;
                }
                m_lUserID = -1;

                labelstat1.Text = "未登录";
                //if (language == "English")
                //{
                //    labelstat1.Text = "Notlogin";
                //}
                labelstat1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                //picturelog.BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
            }
            return;
        }

        private void comboBoxchannel_SelectedIndexChanged(object sender, EventArgs e)
        {

            string language = ChangeLanguage.GetDefaultLanguage();

            if ((m_lUserID < 0) || (comboBoxchannel.SelectedIndex < 0))
            {
                string strtemp = "请先登录设备获取通道!";
                //if (language == "English")
                //{
                //    strtemp = "Please login and get channel!";
                //}
                renewtextBoxOprate(strtemp); 
                //MessageBox.Show("请先登录设备获取通道！");
                return;
            }

            if (comboBoxchannel.SelectedIndex < 0)
            {
                string strtemp = "没有获取到设备通道!";
                //if (language == "English")
                //{
                //    strtemp = "There is no channel!";
                //}
                renewtextBoxOprate(strtemp); 
                //MessageBox.Show("没有获取到设备通道！");
                return;
            }

            curchano = m_struChanNoInfo.lChannelNo[comboBoxchannel.SelectedIndex];
        }

        private void btmstartview_Click(object sender, EventArgs e)
        {
            string language = ChangeLanguage.GetDefaultLanguage();
            if (m_lUserID < 0)
            {
                string strtemp = "请先登录!";
                //if (language == "English")
                //{
                //    strtemp = "Please login!";
                //}
                renewtextBoxOprate(strtemp); 
             //    MessageBox.Show("Please login the device firstly");
                return;
            }
            if (comboBoxstream.SelectedIndex < 0 || comboBoxstream.SelectedIndex>3)
            {
               string strtemp = "请先选择码流!";
                //if (language == "English")
                //{
                //    strtemp = "Please choose stream code!";
                //}
                
                renewtextBoxOprate(strtemp); 
             //    MessageBox.Show("Please login the device firstly");
                return;
            }
            if (curchano == m_struChanNoInfo.lChannelNo[0])
            {
                if (m_lRealHandle1 < 0)
                {
                    CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo1 = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                    lpPreviewInfo1.hPlayWnd = pictureBox1.Handle;//预览窗口
                    lpPreviewInfo1.lChannel = curchano;//预te览的设备通道
                    lpPreviewInfo1.dwStreamType = (uint)comboBoxstream.SelectedIndex;//(uint)comboBoxcode.SelectedIndex;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                    lpPreviewInfo1.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                    lpPreviewInfo1.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                    lpPreviewInfo1.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数  
                    if (RealData == null)
                    {
                        RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                    }
                    IntPtr pUser = new IntPtr();//用户数据
                    //打开预览 Start live view 
                    m_lRealHandle1 = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo1, null/*RealData*/, pUser);
                    if (m_lRealHandle1 < 0)
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "通道1：预览失败，输出错误号 " + iLastErr; //预览失败，输出错误号
                        //if (language == "English")
                        //{
                        //    str = "CH1：Preview failure,error No. " + iLastErr;
                        //}
                        renewtextBoxEvent(str); 
                        //MessageBox.Show(str);
                        return;
                    }
                    else
                    {
                        //预览成功
                        // btnPreview.Text = "Stop Live View";
                    }
                }
            }
            else if (curchano == m_struChanNoInfo.lChannelNo[1])
            {

                if (m_lRealHandle2 < 0)
                {
                    CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo2 = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                    lpPreviewInfo2.hPlayWnd = pictureBox2.Handle;//预览窗口
                    lpPreviewInfo2.lChannel = curchano;//预te览的设备通道
                    lpPreviewInfo2.dwStreamType = (uint)comboBoxstream.SelectedIndex;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                    lpPreviewInfo2.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                    lpPreviewInfo2.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                    lpPreviewInfo2.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数
                    


                    if (RealData == null)
                    {
                        RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                    }

                    IntPtr pUser = new IntPtr();//用户数据

                    //打开预览 Start live view 
                    m_lRealHandle2 = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo2, null/*RealData*/, pUser);
                    if (m_lRealHandle2 < 0)
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "通道2：预览失败，输出错误号" + iLastErr; //预览失败，输出错误号
                        //if (language == "English")
                        //{
                        //    str = "CH2：Preview failure,error No. " + iLastErr;
                        //}
                        renewtextBoxEvent(str); 
                     
                        //MessageBox.Show(str);
                        return;
                    }
                    else
                    {
                        //预览成功
                        // btnPreview.Text = "Stop Live View";
                    }
                }
            }
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {

            if (dwBufSize > 0)
            {
                byte[] sData = new byte[dwBufSize];
                Marshal.Copy(pBuffer, sData, 0, (Int32)dwBufSize);

                string str = "..\\data\\video\\实时流数据.ps";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwBufSize;
                fs.Write(sData, 0, iLen);
                fs.Close();
            }
        }

        private void btmstopview_Click(object sender, EventArgs e)
        {
            //停止预览 Stop live view 

            string language = ChangeLanguage.GetDefaultLanguage();
            if (curchano == m_struChanNoInfo.lChannelNo[0])
            {
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle1))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "通道1：当前通道预览关闭错误, 错误号 " + iLastErr;
                    //if (language == "English")
                    //{
                    //    str = "CH1：Close failure,error No. " + iLastErr;
                    //}
                    renewtextBoxEvent(str); 
                    //MessageBox.Show(str);
                    return;
                }
                m_lRealHandle1 = -1;
                pictureBox1.Invalidate();//刷新窗口    
            }
            else if (curchano == m_struChanNoInfo.lChannelNo[1])
            {
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle2))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "通道2：当前通道预览关闭错误,  错误号  " + iLastErr;
                    //if (language == "English")
                    //{
                    //    str = "CH2：Close failure,error No. " + iLastErr;
                    //}
                    renewtextBoxEvent(str); 
                    //MessageBox.Show(str);
                    return;
                }
                m_lRealHandle2 = -1;
                pictureBox2.Invalidate();//刷新窗口    ;
            }
        }

    

        private void btmgetjpeg_Click(object sender, EventArgs e)
        {
            string sJpegPicFileName;
            //图片保存路径和文件名 the path and file name to save..\\data\\video\\
            //sJpegPicFileName = "../data/jpeg/";
            string timeday = DateTime.Now.ToString();
            timeday = timeday.Replace('/', '-');
            timeday = timeday.Replace(':', '-');
            timeday = timeday.Replace(' ', '-');

            string language = ChangeLanguage.GetDefaultLanguage();
          // string  timehour = DateTime.Now.ToShortTimeString();



              sJpegPicFileName ="../data/jpeg/"+timeday+".jpg";

            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = 0; //图像质量 Image quality
            lpJpegPara.wPicSize = 0xff; //抓图分辨率 Picture size: 2- 4CIF，0xff- Auto(使用当前码流分辨率)，抓图分辨率需要设备支持，更多取值请参考SDK文档

            //JPEG抓图 Capture a JPEG picture
            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, curchano, ref lpJpegPara, sJpegPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = comboBoxchannel.Text+"：当前通道获取JPEG失败,  错误号 " + iLastErr;
                //if (language == "English")
                //{
                //    str = comboBoxchannel.Text + "Get JPEG failure,error No. " + iLastErr;
                //}
                renewtextBoxEvent(str); 
                //MessageBox.Show(str);
                return;
            }
            else
            {
                str = comboBoxchannel.Text + "当前通道获取JPEG成功" + sJpegPicFileName;
                //if (language == "English")
                //{
                //    str = comboBoxchannel.Text + "Get JPEG succeed! " + sJpegPicFileName;
                //}
                renewtextBoxEvent(str); 
                //MessageBox.Show(str);
            }
            return;

        }

        private void btmgetbmp_Click(object sender, EventArgs e)
        {

            string language = ChangeLanguage.GetDefaultLanguage();
            if (m_lRealHandle1 < 0)
            {
                renewtextBoxOprate("请先打开预览!"); //BMP抓图需要先打开预览
                //if (language == "English")
                //{
                //    renewtextBoxOprate("Please Open preview!"); //BMP抓图需要先打开预览
                //}
                return;
            }
            string sBmpPicFileName;
            //图片保存路径和文件名 the path and file name to save
            //sBmpPicFileName = "../data/bmp/";
            string timeday = DateTime.Now.ToString();
            timeday = timeday.Replace('/', '-');
            timeday = timeday.Replace(':', '-');
            timeday = timeday.Replace(' ', '-');
            sBmpPicFileName = "../data/bmp/" + timeday + ".bmp";

            if (curchano == m_struChanNoInfo.lChannelNo[0])
            {
                if (!CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle1, sBmpPicFileName))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = comboBoxchannel.Text + "抓取图片失败,  错误号 " + iLastErr;
                    //if (language == "English")
                    //{
                    //    str = comboBoxchannel.Text + "Get picture failure,error No. " + iLastErr;
                    //}
                    renewtextBoxEvent(str); 
                    //MessageBox.Show(str);
                    return;
                }
                else
                {
                    str = comboBoxchannel.Text + "抓取图片成功" + sBmpPicFileName;
                    //if (language == "English")
                    //{
                    //    str = comboBoxchannel.Text + "Get picture succed!" + sBmpPicFileName;
                    //}
                    renewtextBoxEvent(str); 
                    //MessageBox.Show(str);
                }
            }
            else
            {
                if (!CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle2, sBmpPicFileName))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = comboBoxchannel.Text + "抓取图片失败,  错误号" + iLastErr;
                    //if (language == "English")
                    //{
                    //    str = comboBoxchannel.Text + "Get picture failure,error No. " + iLastErr;
                    //}
                    renewtextBoxEvent(str); 
                    //MessageBox.Show(str);
                    return;
                }
                else
                {
                    str = comboBoxchannel.Text + "抓取图片成功 " + sBmpPicFileName;
                    //if (language == "English")
                    //{
                    //    str = comboBoxchannel.Text + "Get picture succed! " + sBmpPicFileName;
                    //}
                    renewtextBoxEvent(str); 
                    //MessageBox.Show(str);
                }
            }
            //BMP抓图 Capture a BMP picture
         
            return;
        }

        private void VideoForm_SizeChanged(object sender, EventArgs e)
        {
            ascsize.controlAutoSize(this);
        }

    }
}
