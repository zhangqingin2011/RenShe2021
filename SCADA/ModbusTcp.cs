﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace SCADA
{
    public partial   class ModbusTcp
    {
        public enum ConnectStatusEnum { CONNECTED, DISCONNECTED, CONNECTING, CONNECTLOST };
        public delegate void EventHandler<AutoReadEventArgs>(object sender, AutoReadEventArgs Args);

        public TcpClient Tcpclient = null;
        public bool isStarted = false;
        private  NetworkStream ModbusplcStream;
        private static bool IsConnect = false;
        private static int MBAPNUM = 1;
        public Int16 reaponseiserrcode = 0x80;
        public static bool MES_PLC_comfim_write_flage = false;//mes发送plc启动码标识
        public static int MES_PLC_comfim_write_count= 0;//mes发送plc启动码计时
        public static bool Rack_number_write_flage = false;//mes发送plc订单信息
        public static int Rack_number_write_count = 0;//mes发送plc订单计时
        public static bool PLC_MES_comfim_req_flage = false;//mes请求机床加工信息
        public static int PLC_MES_comfim_req_count = 0;//mes请求机床加工信息计时
        public static bool PLC_MES_comfim_back_flage = false;//mes请求机床加工信息反馈
        public static int PLC_MES_comfim_back_count = 0;//mes请求机床加工信息反馈计时
        public static bool PLC_MES_comfim_req_flage_2 = false;
        public static bool PLC_MES_comfim_back_flage_2 = false;
        public static int PLC_MES_comfim_req_count_2 = 0;//mes请求机床加工信息计时
        public static int PLC_MES_comfim_back_count_2 = 0;//mes请求机床加工信息反馈计时

        public static bool PLC_MES_comfim_req_meter = false;
        public static bool PLC_MES_comfim_back_meter= false;//测量处理标记



        public static bool Start_write_flage = false;
        public static int connectcount = 0;

        public static bool ReStart_write_flage = false;
        /// <summary>
        /// 检查网络是否连接
        /// </summary>
        /// <param name="IP">地址</param>
        /// <param name="Port">端口号</param>
        /// <returns>true，在线。false，离线</returns>
        public  bool CheckS7PLCConnect(String IP, int Port)
        {

            //return true;
           // IPAddress ip = IPAddress.Parse(tcpIp);
            IPAddress ip = IPAddress.Parse(MainForm.PLCAddress);
            IPAddress[] Localip = Dns.GetHostAddresses("");

            if (Tcpclient == null)
            {
                try
                {
                    Tcpclient = new TcpClient();
                    if (!MainForm.PingTestCNC(MainForm.PLCAddress, 300))
                        //if (!MainForm.PingTestCNC("192.168.8.10", 300))
                    {
                        return false; ;
                    }
                    //Tcpclient.Connect("192.168.8.10", 502);
                    Tcpclient.Connect(MainForm.PLCAddress, 502);
                    ModbusplcStream = Tcpclient.GetStream();
                    IsConnect = true;
              WriteStream((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 40, 1, 0);//开机时先清除一次命令
                    return true; 
                }
                catch (Exception ex)
                {
                    return false; 
                }
            }
            else if (Tcpclient.Client == null)
            {
                if (connectcount > 1)
                {
                    Stop();
                    return false;
                }
                try
                {
                    connectcount++;
                    Tcpclient = new TcpClient();

                   // if (!MainForm.PingTestCNC("192.168.8.10", 300))
                    if (!MainForm.PingTestCNC(MainForm.PLCAddress, 300))
                    {
                        return false; ;
                    }
                   // Tcpclient.Connect(MainForm.PLCAddress, 502);
                    Tcpclient.Connect("192.168.8.10", 502);
                    ModbusplcStream = Tcpclient.GetStream();
                    IsConnect = true;
                            WriteStream((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 40, 1, 0);//开机时先清除一次命令
                    return true;
                }
                catch (Exception ex)
                {
                    return false;

                }
            }
            else
            {
                if (!Tcpclient.Connected)
                {
                    if (connectcount > 1)
                    {
                        Stop();
                        return false;
                    }
                    try
                    {
                        connectcount++;
                        Tcpclient = new TcpClient();
                        //if (!MainForm.PingTestCNC("192.168.8.10", 300))

                        if (!MainForm.PingTestCNC(MainForm.PLCAddress, 300))
                        {
                            return false; ;
                        }
                       // Tcpclient.Connect("192.168.8.10", 502);

                        Tcpclient.Connect(MainForm.PLCAddress, 502);
                        ModbusplcStream = Tcpclient.GetStream();
                        IsConnect = true;
                             WriteStream((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.MES_PLC_comfirm, 40, 1, 0);//开机时先清除一次命令
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;

                    }
                }
            }

            return true; 
        }

        /// <summary>
        /// 开启发送数据线程
        /// </summary>
          //public void Start()
          //{
          //    isStarted = true;

          //    serverThread = new Thread(new ThreadStart(SendData));
          //    serverThread.Start();
          //}

       /// <summary>
       /// 关闭侦听线程
       /// </summary>
          public  void Stop()
          {
              try
              {
                  if (Tcpclient != null)
                      Tcpclient.Close();
                      IsConnect = false;

              }
              finally
              {
              }
          }
        private bool ReadDataBuffer(byte[] buffer)
        {
            int MBAPIndex = 0;//报文开始位置索引
            //解析第一个报文
            int backfun = buffer[7];//第一个报文功能码
            int transflage = 0;
            while (MBAPIndex>=0)
            {
                if (backfun == 0x80)
                {
                   MBAPIndex= MBAPIndex+9 ;//错误报文长度是9,下一个报文的索引
                   transflage =  buffer[MBAPIndex] *256+buffer[MBAPIndex+1] ;//下一个报文传输标识
                    if(transflage== 0)//下一个报文传输标识为0，那么数据包解析完成
                    {
                        MBAPIndex = -1;
                    }
                    else
                    {
                        backfun = buffer[MBAPIndex+7] ;
                    }
                }
                else if(backfun == 0x03)
                {
                    //获取寄存器的值
                    int regnum = buffer[MBAPIndex+8] / 2;//传输的寄存器个数
                    int regstart = buffer[MBAPIndex + 2]*256+buffer[MBAPIndex+3];//从协议标识位获取请求寄存器起始地址
                    int datastart = MBAPIndex+9;//buffer中，数据开始位置
                    if (MainForm.GetInitRackFinish == false)
                    {

                        if (buffer[MBAPIndex + 2] == 0 && buffer[MBAPIndex + 3] == 41)
                        {
                            MainForm.GetInitRackFinish = true;
                        }
                    }
                    if (RackForm.recivemagmessage == false)
                    {
                        if (buffer[MBAPIndex + 2] == 0 && buffer[MBAPIndex + 3] == 71)
                        {
                            RackForm.recivemagmessage = true;
                        }
                    }
                    if (MeterForm.renewbiaodingfalge == true)
                    {
                        if (buffer[MBAPIndex + 2] == 0 && buffer[MBAPIndex + 3] == (int)SCADA.ModbusTcp.DataConfigArr.p_MeterValue1)
                        {
                            MeterForm.renewbiaodingfalge = false;
                        }
                    }
                    if (RackForm.meterialchange == false)
                    {
                        if (buffer[MBAPIndex + 2] == 0 && buffer[MBAPIndex + 3] == (int)ModbusTcp.DataConfigArr.Meterial)
                        {
                            RackForm.meterialchange = true;
                        }
                    }
                    for (int i = 0; i < regnum; i++)
                    {
                        DataMoubus[regstart + i] = buffer[datastart + 2 * i] * 256 + buffer[datastart + 2 * i + 1];
                    }

                    //读取下一个报文的信息
                    MBAPIndex = MBAPIndex + 9 + buffer[MBAPIndex + 8];//请求报文长度是9,下一个报文的索引
                    transflage = buffer[MBAPIndex] * 256 + buffer[MBAPIndex + 1];//下一个报文传输标识
                   
                    if (transflage == 0)//下一个报文传输标识为0，那么数据包解析完成
                    {
                        MBAPIndex = -1;
                    }
                    else
                    {
                        backfun = buffer[MBAPIndex + 7];
                    }
                }
                else if (backfun == 0x10)
                {
                    //获取寄存器的值
                   
                    if (RackForm.recivemagmessage == false)
                    {
                        if (buffer[MBAPIndex + 2] == 0 && buffer[MBAPIndex + 3] == 71)
                        {
                            RackForm.recivemagmessage = true;
                        }
                    }
                    MBAPIndex = MBAPIndex + 12;//写多个寄存器返回报文长度是12,下一个报文的索引
                    transflage = buffer[MBAPIndex] * 256 + buffer[MBAPIndex + 1];//下一个报文传输标识
                    if (transflage == 0)//下一个报文传输标识为0，那么数据包解析完成
                    {
                        MBAPIndex = -1;
                    }
                    else
                    {
                        backfun = buffer[MBAPIndex + 7];
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public  void ReceiveData()
          {
            bool receiveflage = true;
            int count = 0;
            if (/*IsConnect||*/CheckS7PLCConnect(MainForm.PLCAddress, 502))

                 // if (/*IsConnect||*/CheckS7PLCConnect(tcpIp, tcpPort))
              {
                  try
                  {
                      while (receiveflage)
                      {
                          if (ModbusplcStream.DataAvailable)
                          {

                              Byte[] ReadDataMoubusByteBuffer = new Byte[DataMoubusByteSize];//与西门子通讯数据
                               //await ModbusplcStream.ReadAsync(ReadDataMoubusByteBuffer, 0, DataMoubusByteSize);
                              ModbusplcStream.Read(ReadDataMoubusByteBuffer, 0, DataMoubusByteSize);

                              if (ReadDataBuffer(ReadDataMoubusByteBuffer) == true)
                              {
                                  receiveflage = false;
                                  //return true;
                              }
                              else
                              {
                                  receiveflage = false;
                                  //return false;
                              }
                          }
                          else
                          {
                              Thread.Sleep(1);
                              count++;
                              if (count>100)
                              {
                                  receiveflage = false;
                                  //return false;
                              }
                             
                          }
                      }
                     
                  }
                  catch
                  {
                      if (Tcpclient != null)
                          Tcpclient.Close();
                      //return false;
                  }
              }
              else
              {
                  //return false;
              }
       
          }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="funcode">功能码</param>
        /// <param name="sendstart">数据表中的字节起始地址</param>
        /// <param name="sendlength">发送数据 字节长度</param>
        /// <param name="receivestart">请求plc中寄存器数据起始地址字</param>
        /// <param name="receivenum">请求plc中寄存器的长度字</param>
          public bool  SendData(byte funcode, int sendstart, int sendlength, int receivestart, int receivenum)
          {

             // if (CheckS7PLCConnect(tcpIp, tcpPort))//(MainForm.PCAddress, 502);
              if (CheckS7PLCConnect(MainForm.PLCAddress, 502))
              {
                  try
                  {

                      if (WriteStream(funcode, sendstart, sendlength, receivestart, receivenum))
                      {
                          //  newclient.SendTo(DataMoubusByteBuffer, Tclient);//往服务器发送数据
                          return true;
                      }
                      else
                          return false;
                  }
                  catch
                  {
                      if (Tcpclient != null)
                          Tcpclient.Close();
                      return false;
                  }
              }
              else return false;
          }

 

          
         //报文头
        /// <summary>
        /// 写报文头
        /// </summary>
          /// <param name="senddatalength56">报文头5、6字节，后续字节长度</param>
          /// <param name="senddataprotocol">报文2、3字节，写时为0，请求时为请求起始地址/param>
        public void  AddModbusHeadArr(int senddatalength56,int senddataprotocol)
        {
            int startH = MBAPNUM / 256;
            int startL = MBAPNUM % 256;
            MBAPNUM++;
            if(MBAPNUM>10000)
            {
                MBAPNUM = 1;
                startH = 0;
                startL = 1;
            }
            int FollowlengthH = senddatalength56/256;
            int FollowlengthL = senddatalength56%256;
            DataMoubusHeadByte[0] = (byte)startH; // 传输标识1
            DataMoubusHeadByte[1] = (byte)startL;//传输标识2//0表示modbus协议
            DataMoubusHeadByte[2] = (byte)(senddataprotocol / 256);//协议标识1
            DataMoubusHeadByte[3] = (byte)(senddataprotocol % 256);//协议标识2
            //DataMoubusHeadByte[4] =1;//后续字节长度1//高字节在前392
            //DataMoubusHeadByte[5] = 136;//后续字节长度2//紧跟其后的字节数据长度
            DataMoubusHeadByte[4] = (byte)FollowlengthH;//后续字节长度1//高字节在前392
            DataMoubusHeadByte[5] = (byte)FollowlengthL;//后续字节长度2//紧跟其后的字节数据长度
            DataMoubusHeadByte[6] =1;//单位标识//从站地址

        }

        // 功能码
        public void AddModbusFuncReqArr(Byte funcode, int receivestart, int receivenum)
        {
            int startH = receivestart / 256;
            int startL = receivestart % 256-1;
            DataMoubusFuncByteReq[0] = funcode;
            DataMoubusFuncByteReq[1] = (byte)startH;//起始寄存器基址，高字节在前
            DataMoubusFuncByteReq[2] = (byte)startL;
            DataMoubusFuncByteReq[3] = (byte)(receivenum / 256);//寄存器个数，高字节在前
            DataMoubusFuncByteReq[4] = (byte)(receivenum %256);//20
            //DataMoubusFuncByte[3] = 1;//寄存器个数，高字节在前
            //DataMoubusFuncByte[4] = 124;//380

           
        }
        public void AddModbusFuncWriteArr(Byte funcode, int sendstart, int sendlength)
        {
            int startH = sendstart / 256;
            int startL = sendstart % 256-1;
            int lengthH=sendlength/256 ;
            int lengthL = sendlength%256;
            DataMoubusFuncByteWrite[0] = funcode;
            DataMoubusFuncByteWrite[1] = (byte)startH;//起始寄存器基址，高字节在前
            DataMoubusFuncByteWrite[2] = (byte)startL;
            DataMoubusFuncByteWrite[3] = (byte)lengthH;//寄存器个数，高字节在前
            DataMoubusFuncByteWrite[4] = (byte)lengthL;//20
            DataMoubusFuncByteWrite[5] = (byte)(sendlength*2);//20
            //DataMoubusFuncByte[3] = 1;//寄存器个数，高字节在前
            //DataMoubusFuncByte[4] = 124;//380


        }
        // 传输数据
        public void AddModbusDataArr()
        {
        
            for (int i = 0; i < ModbusBufferSize; i++)
            {
                DataMoubusByte[i * 2] = (Byte)(DataMoubus[i ]/256);
                DataMoubusByte[i * 2+1 ] = (Byte)(DataMoubus[i ]%256);
            }
        }
        /// <summary>
        /// 获取传输数据总长度
        /// </summary>
        /// <param name="funcode">功能码</param>
        /// <param name="sendlength">写入寄存器个数</param>
        /// <param name="receivenum">请求寄存器个数</param>
        /// <returns></returns>
        protected int GetSendbyteLegth(byte funcode, int sendlength, int receivenum)
        {
            int SendbyteLegth = 0;
            if (funcode == 3)
          {
              SendbyteLegth = 7;//+报文长度
              SendbyteLegth += 5;//+功能码长度
              SendbyteLegth += 0;//要请求不用附加数据内容
              return SendbyteLegth;
          }
          else
          {
              SendbyteLegth = 7;//+报文长度
              SendbyteLegth += 6;//+功能码长度
              SendbyteLegth += (sendlength * 2);//写入附加要写入的数据内容，寄存器是字，发送数据是字节
              return SendbyteLegth;
          }
        }
        /// <summary>
        /// 获取报文头中后续字节长度
        /// </summary>
        /// <param name="funcode">功能码</param>
        /// <param name="sendlength">写入寄存器个数</param>
        /// <param name="receivenum">请求寄存器个数</param>
        /// <returns></returns>
        protected int GetSendbyteLegth56(byte funcode, int sendlength, int receivenum)
        {
            int SendbyteLegth56 = 1;
            if (funcode == 3)
            {
                SendbyteLegth56 += 5;//+功能码长度
                SendbyteLegth56 += 0;//要请求不用附加数据内容
                return SendbyteLegth56;
            }
            else
            {
                SendbyteLegth56 += 6;//+功能码长度
                SendbyteLegth56 += (sendlength * 2);//写入附加要写入的数据内容，寄存器是字，发送数据是字节
                return SendbyteLegth56;
            }
        }
          //客户端写数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcode">功能码，3请求服务端数据，16写入数据到服务器</param>
        /// <param name="sendstart">写入寄存器的起始地址</param>
        /// <param name="sendlength">需要写入的寄存器个数，<128</param>
        /// <param name="receivestart">请求寄存器的起始地址</param>
        /// <param name="receivenum">请求寄存器的个数</param>
        /// <returns></returns>
        protected bool WriteStream(byte funcode, int sendstart, int sendlength, int receivestart, int receivenum)
          {
              int  sendbytebufferlength = GetSendbyteLegth(funcode,sendlength,receivenum);
              int SendbyteLegth56 = GetSendbyteLegth56(funcode,sendlength,receivenum);
              byte[] sendbytebuffer=new byte[sendbytebufferlength];
              try
              {
                  if(funcode == 0x03)
                  {

                      AddModbusHeadArr(SendbyteLegth56, receivestart);
                  }
                  else if (funcode == 0x10)
                  {

                      AddModbusHeadArr(SendbyteLegth56, sendstart);
                  }
                  if (funcode == 3)
                  {
                      AddModbusFuncReqArr(funcode, receivestart, receivenum);
                      for (int i = 0; i < 7; i++)
                      {
                          sendbytebuffer[i] = DataMoubusHeadByte[i];
                      }
                      for (int i = 0; i < 5; i++)
                      {
                          sendbytebuffer[7+i] = DataMoubusFuncByteReq[i];
                      }
                      //await ModbusplcStream.WriteAsync(sendbytebuffer, 0, sendbytebuffer.Length);
                       ModbusplcStream.Write(sendbytebuffer, 0, sendbytebuffer.Length);
                  }
                  else
                  {
                      AddModbusFuncWriteArr(funcode, sendstart, sendlength);
                      AddModbusDataArr();
                      for (int i = 0; i < 7;i++ )
                      {
                          sendbytebuffer[i] = DataMoubusHeadByte[i];
                      }
                      for (int i = 0; i < 6; i++)
                      {
                          sendbytebuffer[i+7] = DataMoubusFuncByteWrite[i];
                      }
                      for(int i=0;i<sendlength;i++)
                      {
                          if (i == 52)
                          {
                              ;
                          }
                          sendbytebuffer[13 + 2*i ] = DataMoubusByte[(sendstart + i) * 2];
                          sendbytebuffer[13 +2* i  + 1] = DataMoubusByte[(sendstart + i) * 2+1];
                      }

                      //await ModbusplcStream.WriteAsync(sendbytebuffer, 0, sendbytebuffer.Length);
                      ModbusplcStream.Write(sendbytebuffer, 0, sendbytebuffer.Length);
                  }
                 
              }
              catch (Exception exp)
              {
                  exp.ToString();
                  return false;
              }
              return true;
          }
      


        // 数据表打包

        //发送数据包到客户端



    }
}
