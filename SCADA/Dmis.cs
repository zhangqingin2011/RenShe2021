﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;


namespace SCADA
{
    public class Dmis
    {
        bool ExitFlag = false;
        Thread threalister;
        Thread threaGet;
        HttpListener lister = new HttpListener();
        public EventHandler<string> GetStrHandler;
        public bool initfg = false;
        public Dmis(string url_post, string url_Get)
        {
            try
            { 
                threaGet = new Thread(new ThreadStart(delegate
                {
                    loopGet(url_Get);
                }
                     ));
                threaGet.Start();

                lister.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                lister.Prefixes.Add(url_post);
                lister.Start();

                threalister = new Thread(new ThreadStart(delegate
                {
                    looplister(lister);
                }
                     ));

                threalister.Start();
                initfg = true;
            }
            catch (Exception e)
            {
                initfg = false;
                LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
                SendParm.Node1NameIndex = (int)LogData.Node1Name.System_runing;
                SendParm.LevelIndex = (int)LogData.Node2Level.ERROR;
                SendParm.EventID = ((int)LogData.Node2Level.ERROR).ToString();
                SendParm.Keywords = "三坐标设备接口网络初始化错";
                SendParm.EventData = e.ToString();
                SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, null, null);
            }

        }
        private void looplister(HttpListener m_lister)
        {
            while (!ExitFlag)
            {
                try 
                {
                    HttpListenerContext context = m_lister.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    Servlet m_server = new MyServlet();
                    m_server.onCreate();
                    if (request.HttpMethod == "POST")
                    {
                        m_server.onPost(request, response);
                        byte[] bytes = new byte[1024];
                        request.InputStream.Read(bytes, 0, 1024);
                        LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
                        SendParm.Node1NameIndex = (int)LogData.Node1Name.Equipment_CCD;
                        SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
                        SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
                        SendParm.Keywords = "合格";
                        SendParm.EventData = Encoding.Default.GetString(bytes).TrimEnd('\0');
//                         SendParm.EventData = Encoding.Unicode.GetString(bytes).TrimEnd('\0');
                        SCADA.MainForm.m_CCDdata.AddLogMsgHandler.BeginInvoke(this, SendParm, null, null);
                    }
                    else if (request.HttpMethod == "GET")
                    {
                        m_server.onGet(request, response);
                    }
                    response.Close();
                }
                catch(Exception ex)
                {
                    break;
                }
            }
        }
        HttpWebRequest request;
        private void loopGet(string url)
        {

            while (!ExitFlag)
            {
                try
                {
                    request = HttpWebRequest.Create(url) as HttpWebRequest;
                    request.Method = "GET";                            //请求方法
//                     request.ProtocolVersion = new Version(1, 1);   //Http/1.1版本

                    HttpWebResponse response =
                                 request.GetResponse() as HttpWebResponse;

                    //如果主体信息不为空，则接收主体信息内容
                    if (response.ContentLength <= 0)
                        continue;
                    //接收响应主体信息
                    if (GetStrHandler != null)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            int totalLength = (int)response.ContentLength;
                            int numBytesRead = 0;
                            byte[] bytes = new byte[totalLength + 1024];
                            //通过一个循环读取流中的数据，读取完毕，跳出循环
                            while (numBytesRead < totalLength)
                            {
                                int num = stream.Read(bytes, numBytesRead, 1024);  //每次希望读取1024字节
                                if (num == 0)   //说明流中数据读取完毕
                                    break;
                                numBytesRead += num;
                            }
                            string str = Encoding.Default.GetString(bytes).TrimEnd('\0');
                            GetStrHandler.BeginInvoke(null, str, null, null);
                        }
                    }
                    response.Close();
                    Thread.Sleep(3000);
                }
                catch (Exception ex)
                {
                    //break;
                }
            }
        }

        public void Exit()
        {
            ExitFlag = true;
            if(initfg)
            {
                lister.Stop();
                Thread.Sleep(3000);
            }
        }

    }
    public class Servlet
    {
        public virtual void onGet(System.Net.HttpListenerRequest request,
            System.Net.HttpListenerResponse response) { }
        public virtual void onPost(System.Net.HttpListenerRequest request,
            System.Net.HttpListenerResponse response) { }
        public virtual void onCreate()
        {

        }
    }
    public class MyServlet : Servlet
    {
        public override void onCreate()
        {
            base.onCreate();
        }

        public override void onGet(HttpListenerRequest request, HttpListenerResponse response)
        {
//             Console.WriteLine("GET:" + request.Url);
            byte[] buffer = Encoding.UTF8.GetBytes("OK");

            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        public override void onPost(HttpListenerRequest request, HttpListenerResponse response)
        {
//             Console.WriteLine("POST:" + request.Url);
            byte[] buffer = Encoding.UTF8.GetBytes("OK");
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }

}
