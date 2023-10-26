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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using HNCAPI;

namespace SCADA
{
    public partial class MyServer : Form
    {
         public MyServer()
         {
             InitializeComponent();
             TextBox.CheckForIllegalCrossThreadCalls = false;
             textBox1.Text = "192.168.2.99";
             textBox2.Text = "10000";
             autolink();
         }
        bool sendpassword = true;
        Thread threadWatch = null;
        Socket socketWatch = null;
        Dictionary<string, Socket> dict = new Dictionary<string, Socket>();
        Dictionary<string, Thread> dictThread = new Dictionary<string, Thread>();
        string Password = "01010101";
        int index = HncApi.GetHncRegType("R");
        bool printmsg = true;
        bool senden = true;
        public int type = 0;

        public void autolink()
        {
            // 创建负责监听的套接字，注意其中的参数；
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 获得文本框中的IP对象；
            IPAddress address = IPAddress.Parse(textBox1.Text.Trim());
            // 创建包含ip和端口号的网络节点对象；
            IPEndPoint endPoint = new IPEndPoint(address, int.Parse(textBox2.Text.Trim()));
            try
            {
                // 将负责监听的套接字绑定到唯一的ip和端口上；
                socketWatch.Bind(endPoint);
            }
            catch (SocketException se)
            {
                MessageBox.Show("异常：" + se.Message);
                return;
            }
            // 设置监听队列的长度；
            socketWatch.Listen(10);
            // 创建负责监听的线程；
            threadWatch = new Thread(WatchConnecting);
            threadWatch.IsBackground = true;
            threadWatch.Start();
            ShowMsg("服务器启动监听成功！");
            //}
        }

        /// <summary>
        /// 监听客户端请求的方法；
        /// </summary>
        void WatchConnecting()
        {
            HncApi.HNC_RegClrBit(index, 26, 4, Collector.CollectHNCPLC.m_clientNo);
            while (true) // 持续不断的监听客户端的连接请求；
            {
                // 开始监听客户端连接请求，Accept方法会阻断当前的线程；
                Socket sokConnection = socketWatch.Accept(); // 一旦监听到一个客户端的请求，就返回一个与该客户端通信的 套接字；
                // 想列表控件中添加客户端的IP信息；
                listBox1.Items.Add(sokConnection.RemoteEndPoint.ToString());
                // 将与客户端连接的 套接字 对象添加到集合中；
                dict.Add(sokConnection.RemoteEndPoint.ToString(), sokConnection);
                ShowMsg("客户端连接成功！");
                Thread thr = new Thread(RecMsg);
                thr.IsBackground = true;
                thr.Start(sokConnection);
                dictThread.Add(sokConnection.RemoteEndPoint.ToString(), thr); // 将新建的线程 添加 到线程的集合中去。
                if (sendpassword == true)
                {
                    HncApi.HNC_RegSetBit(index, 26, 4, Collector.CollectHNCPLC.m_clientNo);
                    string initpassword = ":," + Password;
                    btnSendString(initpassword);
                    sendpassword = false;
                }
            }
        }

        void RecMsg(object sokConnectionparn)
        {
            Socket sokClient = sokConnectionparn as Socket;
            while (true)
            {
                // 定义一个2M的缓存区；
                byte[] arrMsgRec = new byte[1024 * 1024 * 2];
                // 将接受到的数据存入到输入 arrMsgRec中；
                int length = -1;
                try
                {
                    length = sokClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度；
                }
                catch (SocketException se)
                {
                    ShowMsg("异常：" + se.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；
                    dict.Remove(sokClient.RemoteEndPoint.ToString());

                    // 从通信线程集合中删除被中断连接的通信线程对象；
                    dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从列表中移除被中断的连接IP
                    listBox1.Items.Remove(sokClient.RemoteEndPoint.ToString());
                    HncApi.HNC_RegClrBit(index, 26, 4, Collector.CollectHNCPLC.m_clientNo);
                    sendpassword = true;
                    printmsg = true;
                    return;
                }
                catch (Exception e)
                {
                    ShowMsg("异常：" + e.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；
                    dict.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从通信线程集合中删除被中断连接的通信线程对象；
                    dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从列表中移除被中断的连接IP
                    listBox1.Items.Remove(sokClient.RemoteEndPoint.ToString());
                    return;
                }
                try
                {
                    if (length >= 4)
                    {
                        string strData = System.Text.Encoding.Default.GetString(arrMsgRec);// 将接受到的字节数据转化成字符串；
                        strData = strData.Substring(0, length);
                        string[] strDatas = strData.Split(':');
//                        ShowMsg("数据：" + strData);

                        if (strDatas.Length >= 4)
                        {
                            if (strDatas[0] == Password && strDatas[1] == "CMMResult" && strDatas[2] == "XML")
                            {
                                string xmlData = strDatas[3];
                                if (xmlData.Substring(xmlData.Length - 8, 8) == Password)
                                {
                                    xmlData = xmlData.Substring(0, xmlData.Length - 8);
                                    strDatas[0] = Password;
                                    strDatas[1] = strDatas[4];
                                    strDatas[2] = strDatas[5];
                                    strDatas[3] = strDatas[6];
                                }
                                byte[] xmlStream = System.Text.Encoding.Default.GetBytes(xmlData);
                                ShowMsg("数据：" + Password + ":CMMResult:XML:" + System.Text.Encoding.UTF8.GetString(xmlStream) + "\n");
                                MemoryStream memoryStream = new MemoryStream(xmlStream);
                                XmlSerializer ser = new XmlSerializer(typeof(XmlDocument));
                                XmlDocument m_XmlDoc = (XmlDocument)ser.Deserialize(memoryStream);
                                GetResponseMsg_PlcChanged(m_XmlDoc);
                            }
                            else if (strDatas[0] == Password && strDatas[1] == "CMMState" && strDatas[2] == "String")
                            {
                                string CmmState = strDatas[3];
                                if(HncApi.HNC_NetIsConnect(Collector.CollectHNCPLC.m_clientNo)>=0)
                                {
                                    if (CmmState == "1" || CmmState == ("1" + Password))        //空闲
                                    {
                                        HncApi.HNC_RegClrBit(index, 26, 2, Collector.CollectHNCPLC.m_clientNo);
                                        HncApi.HNC_RegSetBit(index, 26, 1, Collector.CollectHNCPLC.m_clientNo);
                                        senden = true;
                                        ShowMsg("测量机状态:空闲");
                                    }
                                    else if (CmmState == "2" || CmmState == ("2" + Password))  //忙碌
                                    {
                                        HncApi.HNC_RegClrBit(index, 26, 1, Collector.CollectHNCPLC.m_clientNo);
                                        HncApi.HNC_RegSetBit(index, 26, 2, Collector.CollectHNCPLC.m_clientNo);

                                        senden = true;
                                        ShowMsg("测量机状态:忙碌");
                                    }
                                    else if (CmmState == "3" || CmmState == ("3" + Password))  //完成
                                    {
                                        HncApi.HNC_RegClrBit(index, 26, 2, Collector.CollectHNCPLC.m_clientNo);
                                        HncApi.HNC_RegSetBit(index, 26, 1, Collector.CollectHNCPLC.m_clientNo);

                                        senden = true;
                                        ShowMsg("测量机状态:完成");
                                    }
                                    else if (CmmState == "4" || CmmState == ("4" + Password))  //出错
                                    {
                                        HncApi.HNC_RegClrBit(index, 26, 2, Collector.CollectHNCPLC.m_clientNo);
                                        HncApi.HNC_RegClrBit(index, 26, 1, Collector.CollectHNCPLC.m_clientNo);
                                        HncApi.HNC_RegSetBit(index, 26, 3, Collector.CollectHNCPLC.m_clientNo);
                                        ShowMsg("测量机状态:出错");
                                    }
                                }
                            }
                            else if (strDatas[0] == Password && strDatas[1] == "tickt" && strDatas[2] == "int"&& strDatas[3] == "0")
                            {
                                ShowMsg("数据：" + strData);
                                btnSendString(Password + ":tickt:int:1");
                            }
                        }

                    }

                    if (checkBox1.Checked && length >= 4)
                    {

                        byte[] xmlStream = new byte[length - 4];
                        Buffer.BlockCopy(arrMsgRec, 4, xmlStream, 0, xmlStream.Length);
                        XmlDocument xmlFile = (XmlDocument)MyXmlUnit.DeSerializeFromXmlBytes(xmlStream);

                        SaveFileDialog sfd = new SaveFileDialog();
                        if (sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            xmlFile.Save(sfd.FileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowMsg("异常：" + ex.Message);
                }
            }
        }

        int i = 0;
        void ShowMsg(string str)
        {
            if (str == ("数据：" + Password + ":tickt:int:0") || str == ("Send:" + Password + ":tickt:int:1"))
            {
                if (printmsg == false)
                {
                    return;
                }
                i++;
                if (i == 2)
                {
                    printmsg = false;
                    i = 0;
                }
            }
            textBox3.AppendText(str + "\r\n");
        }

        private void btnSendString(string strCmd)
        {
            try
            {
                string strKey = "";
                listBox1.SelectedIndex = 0;
                strKey = listBox1.Text.Trim();
                string[] datablock = strCmd.Split(':');

                if (string.IsNullOrEmpty(strKey)) // 判断是不是选择了发送的对象；
                {
                    MessageBox.Show("请选择你要发送的好友！！！");
                }
                else
                {
                    byte[] arrMsg = System.Text.Encoding.Default.GetBytes(strCmd);
                    dict[strKey].Send(arrMsg);
                    if (datablock[1] == "," + Password)
                    {
                        ShowMsg("Send:" + strCmd);
                        return;
                    }
                    else if (datablock[3] == "0" || datablock[3] == "1")
                    {
                        ShowMsg("Send:" + strCmd);
                    }
                    else if (datablock[3] == "1,0002")
                    {
                        ShowMsg("测量A料正面");
                    }
                    else if (datablock[3] == "1,0001")
                    {
                        ShowMsg("测量A料反面");
                    }
                    else if (datablock[3] == "1,0003")
                    {
                        ShowMsg("测量B料正面");
                    }
                    else if (datablock[3] == "1,0004")
                    {
                        ShowMsg("测量B料反面");
                    }
                    else
                    {
                        ShowMsg("Error");
                    }
                }
            }
            catch (SocketException ex)
            {
                ShowMsg("异常：" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strCmd = Password + ":CMD:int:1,0001";
            btnSendString(strCmd);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string strKey = "";
                strKey = listBox1.Text.Trim();

                if (string.IsNullOrEmpty(strKey)) // 判断是不是选择了发送的对象；
                {
                    MessageBox.Show("请选择你要发送的好友！！！");
                }
                else
                {
                    string strMsg = textBox4.Text.Trim();
                    byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);

                    byte[] arrLength = new byte[4];
                    arrLength = BitConverter.GetBytes(arrMsg.Length + 4);
                    byte[] arrSendMsg = new byte[arrMsg.Length + 4];
                    arrSendMsg[0] = arrLength[3];
                    arrSendMsg[1] = arrLength[2];
                    arrSendMsg[2] = arrLength[1];
                    arrSendMsg[3] = arrLength[0];
                    Buffer.BlockCopy(arrMsg, 0, arrSendMsg, 4, arrMsg.Length);
                    int aa = BitConverter.ToInt32(arrSendMsg, 0);
                    int bb = aa;

                    dict[strKey].Send(arrSendMsg);    // 解决了 sokConnection是局部变量，不能再本函数中引用的问题；
                    ShowMsg(strMsg);
                    textBox4.Clear();
                }
            }
            catch (SocketException ex)
            {
                ShowMsg("异常：" + ex.Message);
            }
        }

        void GetResponseMsg_PlcChanged(XmlDocument xmlDoc)
        {
            int ii = 0;
            for (; ii < Collector.CollectHNCPLC.m_cmmresultlist.Count; ii++)
            {
                if (Collector.CollectHNCPLC.m_cmmresultlist[ii].type == type)
                {
                    try
                    {
                        XmlNode rootNode = xmlDoc.SelectSingleNode("root");

                        foreach (XmlNode node in rootNode.ChildNodes)
                        {
                            switch (node.Name)
                            {
                                //                         case "header":
                                #region Header
                                //                             foreach (XmlAttribute attri in node.Attributes)
                                //                             {
                                //                                 switch (attri.Name)
                                //                                 {
                                //                                     //case "eventId": m_PlcChange_Res.eventId = attri.Value; break;
                                //                                     //case "eventName": m_PlcChange_Res.eventName = attri.Value; break;
                                //                                     //case "eventSwitch": m_PlcChange_Res.eventSwitch = attri.Value; break;
                                //                                     default: break;
                                //                                 }
                                //                             }
                                //                             if (node.HasChildNodes)
                                //                             {
                                //                                 foreach (XmlNode m_Node in node)
                                //                                 {
                                //                                     if (m_Node.Name == "location")
                                //                                     {
                                //                                         foreach (XmlAttribute attri in m_Node.Attributes)
                                //                                         {
                                //                                             switch (attri.Name)
                                //                                             {
                                //                                                 //case "lineNo": m_PlcChange_Res.lineNo = attri.Value; break;
                                //                                                 //case "statNo": m_PlcChange_Res.statNo = attri.Value; break;
                                //                                                 //case "statIdx": m_PlcChange_Res.statIdx = attri.Value; break;
                                //                                                 //case "fuNo": m_PlcChange_Res.fuNo = attri.Value; break;
                                //                                                 //case "workPos": m_PlcChange_Res.workPos = attri.Value; break;
                                //                                                 //case "toolPos": m_PlcChange_Res.toolPos = attri.Value; break;
                                //                                                 //case "processName": m_PlcChange_Res.processName = attri.Value; break;
                                //                                                 //case "processNo": m_PlcChange_Res.processNo = attri.Value; break;
                                //                                                 default: break;
                                //                                             }
                                //                                         }
                                //                                     }
                                //                                 }
                                //                             }
                                //                             break;
                                #endregion
                                //                         case "event":
                                #region Event
                                //                             if (node.HasChildNodes)
                                //                             {
                                //                                 foreach (XmlNode m_Node in node)
                                //                                 {
                                //                                     if (m_Node.Name == "result")
                                //                                     {
                                //                                         foreach (XmlAttribute attri in m_Node.Attributes)
                                //                                         {
                                //                                             //if (attri.Name == "returnCode")
                                //                                             //m_PlcChange_Res.returnCode = attri.Value;
                                //                                         }
                                //                                     }
                                //                                     else if (m_Node.Name == "trace")
                                //                                     {
                                //                                         if (m_Node.HasChildNodes)
                                //                                         {
                                //                                             foreach (XmlNode m_TraceNode in m_Node)
                                //                                             {
                                //                                                 //ResponseMsg_UAES.trace m_Trace = new ResponseMsg_UAES.trace();
                                //                                                 //foreach (XmlAttribute attri in m_TraceNode.Attributes)
                                //                                                 //{
                                //                                                 //    switch (attri.Name)
                                //                                                 //    {
                                //                                                 //        case "level": m_Trace.lavel = attri.Value; break;
                                //                                                 //        case "code": m_Trace.code = attri.Value; break;
                                //                                                 //        case "text": m_Trace.text = attri.Value; break;
                                //                                                 //        case "source": m_Trace.source = attri.Value; break;
                                //                                                 //        default: break;
                                //                                                 //    }
                                //                                                 //}
                                //                                                 //m_PlcChange_Res.traces.Add(m_Trace);
                                //                                             }
                                //                                         }
                                //                                     }
                                //                                 }
                                //                             }
                                //                             break;
                                #endregion
                                case "Results":
                                    #region Body-Items
                                    if (node.HasChildNodes)
                                    {
                                        foreach (XmlNode m_Node in node)
                                        {
                                            if (m_Node.Name == "values")
                                            {
                                                if (m_Node.HasChildNodes)
                                                {
                                                    foreach (XmlNode m_ItemNode in m_Node)
                                                    {
                                                        //ResponseMsg_UAES.item m_Item = new ResponseMsg_UAES.item();
                                                        string[] m_Item = new string[6];
                                                        foreach (XmlAttribute attri in m_ItemNode.Attributes)
                                                        {
                                                            switch (attri.Name)
                                                            {
                                                                case "Dimension": m_Item[0] = attri.Value; break;
                                                                case "Nominal": m_Item[1] = attri.Value; break;
                                                                case "UpTOL": m_Item[2] = attri.Value; break;
                                                                case "LowTOL": m_Item[3] = attri.Value; break;
                                                                case "Measure": m_Item[4] = attri.Value; break;
                                                                case "Result": m_Item[5] = attri.Value; break;
                                                                default: break;
                                                            }
                                                            //    if (m_Item.name == "ProgramNO")
                                                            //    {
                                                            //        m_PlcChange_Res.ProgramNo = m_Item.value;
                                                            //    }
                                                        }                                                      
                                                            int jj = 0;
                                                            for (; jj < 6; jj++)
                                                            {
                                                                while (true)
                                                                {
                                                                    if (Collector.CollectHNCPLC.m_cmmresultlist[ii].mearesult1[jj] == null)
                                                                    {
                                                                        Collector.CollectHNCPLC.m_cmmresultlist[ii].mearesult1[jj] = m_Item[jj];
                                                                        break;
                                                                    }
                                                                    else if (Collector.CollectHNCPLC.m_cmmresultlist[ii].mearesult2[jj] == null)
                                                                    {
                                                                        Collector.CollectHNCPLC.m_cmmresultlist[ii].mearesult2[jj] = m_Item[jj];
                                                                        break;
                                                                    }
                                                                    else if (Collector.CollectHNCPLC.m_cmmresultlist[ii].mearesult3[jj] == null)
                                                                    {
                                                                        Collector.CollectHNCPLC.m_cmmresultlist[ii].mearesult3[jj] = m_Item[jj];
                                                                        break;
                                                                    }
                                                                    else if (Collector.CollectHNCPLC.m_cmmresultlist[ii].mearesult4[jj] == null)
                                                                    {
                                                                        Collector.CollectHNCPLC.m_cmmresultlist[ii].mearesult4[jj] = m_Item[jj];
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        break;
                                                                    }
                                                                }    
                                                            }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                    #endregion
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                
            }               
            
        }

    }

    
}
