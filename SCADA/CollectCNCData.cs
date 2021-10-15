using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HNC.API;
using System.Threading;
using FluentFTP;
using HNCAPI_INTERFACE;

namespace SCADA
{
    public class CollectCNCData
    {
        public CNCV2 CNC;
        public bool bCollect;
        private Thread CNCCollectThread;
        public bool threaFucRuningF_OK = true;
        public System.Threading.AutoResetEvent Get_Reg_threaFucEvent = new System.Threading.AutoResetEvent(true);

        public CollectCNCData(string ip)
        {
            CNC = new CNCV2(ip, 10001, "127.0.0.1", 9090);

            bCollect = true;
            CollectVersion();//版本信息值采集一次
        }
        public void ThreadStart()
        {
            //collectThread = new Thread(new ThreadStart(DataCollectThread));
            CNCCollectThread = new Thread(new ThreadStart(DataCollectThreadProc));// net_to_redis
            CNCCollectThread.Start();
        }

        public void ThreadStop()
        {
            bCollect = false;
            if (CNCCollectThread != null && System.Threading.ThreadState.Unstarted != CNCCollectThread.ThreadState)
            {
                if (!threaFucRuningF_OK)
                {
                    threaFucRuningF_OK = true;
                    Get_Reg_threaFucEvent.Set();
                }
                CNCCollectThread.Join();
                CNCCollectThread = null;
            }

        }
        private bool flag = true;
        public void DataCollectThreadProc()
        {
            while (bCollect)
            {

                if (!CNC.IsConnected())
                {
                    if(flag)
                    {
                        CNC.EquipmentState = "离线";
                        threaFucRuningF_OK = false;
                        CollectDataV2.threaFucEvent.Set();
                        SCADA.LogData.EventHandlerSendParm SendParm = new SCADA.LogData.EventHandlerSendParm();
                        SendParm.Node1NameIndex = (int)SCADA.LogData.Node1Name.Equipment_CNC;
                        SendParm.LevelIndex = (int)SCADA.LogData.Node2Level.MESSAGE;
                        SendParm.EventID = ((int)SCADA.LogData.Node2Level.MESSAGE).ToString();
                        SendParm.Keywords = "CNC采集器关闭";
                        SendParm.EventData = CNC.IP.ToString() + "：网络连接失败";
                        SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, null, null);
                        flag = false;
                    }
                  
                }
                else
                {
                    if(!flag)
                    {
                        flag = true;
                    }
                    CollectCNCStateData();
                    CollectAlarm();
                    CollectTool();
                    CollectProgFile();
                    CollectMeter();
                   // CollectVersion();
                }


            }
        }
        /// <summary>
        /// 采集机床状态
        /// </summary>
        private void CollectCNCStateData()
        {
            try
            {
                if (true)
                {
                    string datatype;
                    object value;
                    object value1;
                    DateTime t12 = DateTime.Now;
                    CNC.EquipmentState = "空闲";
                    if (CNC.IsRuning())
                    {
                        CNC.EquipmentState = "运行";

                    }
                    else if (CNC.IsEmergency())
                    {
                        CNC.EquipmentState = "急停";

                    }
                    else if (CNC.IsReseting())
                    {
                        CNC.EquipmentState = "复位";

                    }
                    //当前程序行号
                    if (CNC.ChannelGetValue(HncChannel.HNC_CHAN_RUN_ROW, out datatype, out value))
                    {
                        CNC.InstructionsCount = Convert.ToInt32(value);
                    }
                    //当前刀号
                    if (CNC.ChannelGetValue(HncChannel.HNC_CHAN_TOOL_USE, out datatype, out value))
                    {
                        CNC.InstructionsTool = Convert.ToInt32(value);
                    }
                    //主轴速度
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_VEL, out datatype, out value))
                    {
                        CNC.SpindleSpeed = Convert.ToDouble(value);
                    }
                    //进给
                    if (CNC.ChannelGetValue(HncChannel.HNC_CHAN_FEED_OVERRIDE, out datatype, out value))
                    {
                        CNC.FeedRate = Convert.ToDouble(value);
                    }
                    //当前程序
                    if (CNC.ChannelGetValue(HncChannel.HNC_CHAN_RUN_PROG, out datatype, out value))
                    {
                        var str1 = CNC.CNCCurGrogName();
                        int index = str1.LastIndexOf('/');
                        if (index >= 0)
                        {
                            CNC.GProgram = str1.Substring(index + 1);
                        }
                        else
                        {
                            CNC.GProgram = str1;
                        }
                        //CNC.GProgram = str1.Substring(index);
                    }
                    //if (CNC.ChannelGetValue(HncChannel.HNC_CHAN_RUN_PROG, out datatype, out value))
                    //{
                    //    CNC.GProgram = value.ToString();
                    //}


                    //工件指令位置

                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_POS_WCS, out datatype, out value, 0))
                    {
                        CNC.SiteXF = Convert.ToDouble(value);
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_POS_WCS, out datatype, out value, 1))
                    {
                        CNC.SiteYF = Convert.ToDouble(value);
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_POS_WCS, out datatype, out value, 2))
                    {
                        CNC.SiteZF = Convert.ToDouble(value);
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_POS_WCS, out datatype, out value, 5))
                    {
                        CNC.SiteCF = Convert.ToDouble(value);
                    }
                    //工件实际位置

                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_POS, out datatype, out value, 0))
                    {
                        CNC.SiteX = Convert.ToDouble(value);
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_POS, out datatype, out value, 0))
                    {
                        CNC.SiteY = Convert.ToDouble(value);
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_POS, out datatype, out value, 2))
                    {
                        CNC.SiteZ = Convert.ToDouble(value);
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_ACT_POS, out datatype, out value, 5))
                    {
                        CNC.SiteC = Convert.ToDouble(value);
                    }

                    //轴负载
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_LOAD_CUR, out datatype, out value, 0))
                    {
                        if (CNC.AxisGetValue(HncAxis.HNC_AXIS_RATED_CUR, out datatype, out value1, 0))
                        {
                            if (Convert.ToDouble(value1) != 0)
                            {
                                CNC.LoadX = (int)((Convert.ToDouble(value) / Convert.ToDouble(value1)) * 100);
                                if(CNC.LoadX<0)
                                {
                                    CNC.LoadX = (-1) * CNC.LoadX;
                                }
                            }

                        }
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_LOAD_CUR, out datatype, out value, 1))
                    {
                        if (CNC.AxisGetValue(HncAxis.HNC_AXIS_RATED_CUR, out datatype, out value1, 1))
                        {
                            if (Convert.ToDouble(value1) != 0)
                            {
                                CNC.LoadY = (int)((Convert.ToDouble(value) / Convert.ToDouble(value1)) * 100);
                                if (CNC.LoadY < 0)
                                {
                                    CNC.LoadY = (-1) * CNC.LoadY;
                                }
                            }

                        }
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_LOAD_CUR, out datatype, out value, 2))
                    {
                        if (CNC.AxisGetValue(HncAxis.HNC_AXIS_RATED_CUR, out datatype, out value1, 2))
                        {
                            if (Convert.ToDouble(value1) != 0)
                            {
                                CNC.LoadZ = (int)((Convert.ToDouble(value) / Convert.ToDouble(value1)) * 100);
                                if (CNC.LoadZ < 0)
                                {
                                    CNC.LoadZ = (-1)*CNC.LoadZ;
                                }
                            }

                        }
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_LOAD_CUR, out datatype, out value, 5))
                    {
                        if (CNC.AxisGetValue(HncAxis.HNC_AXIS_RATED_CUR, out datatype, out value1, 5))
                        {
                            if (Convert.ToDouble(value1) != 0)
                            {
                                CNC.LoadC = (int)((Convert.ToDouble(value) / Convert.ToDouble(value1)) * 100);
                                if (CNC.LoadC < 0)
                                {
                                    CNC.LoadC = (-1) * CNC.LoadC;
                                }
                            }

                        }
                    }
                    if (CNC.ChannelGetValue(HncChannel.HNC_CHAN_FEED_OVERRIDE, out datatype, out value))
                    {
                        CNC.SpeedRate = Convert.ToDouble(value);
                    }
                    if (CNC.ChannelGetValue(HncChannel.HNC_CHAN_RAPID_OVERRIDE, out datatype, out value))
                    {
                        CNC.RapidRate = Convert.ToDouble(value);
                    }
                    if (CNC.ChannelGetValue(HncChannel.HNC_CHAN_SPDL_OVERRIDE, out datatype, out value))
                    {
                        CNC.SpdlRate = Convert.ToDouble(value);
                    }
                    if (CNC.AxisGetValue(HncAxis.HNC_AXIS_TYPE, out datatype, out value, 1))
                    {
                        if (Convert.ToString(value) == "0" || Convert.ToString(value) == "")
                        {
                            CNC.cnctype = CNCType.Lathe;
                        }
                        else
                        {
                            CNC.cnctype = CNCType.CNC;
                        }
                    }
                    if (!CNC.AxisGetValue(HncAxis.HNC_AXIS_TYPE, out datatype, out value, 1))
                    {
                        CNC.cnctype = CNCType.Lathe;

                    }
                    //获取进给百分比fspeedrate


                    //获取主轴修调spldrate


                    //获取系统快移修调值rapidrate

                    //获取按键面板数据
                    var temp1 = 0;
                    if (CNC.RegGetValue(HncRegType.REG_TYPE_Y, 480, out temp1))
                    {
                        CNC.CNCButtonY480 = temp1;
                    }

                    if (CNC.RegGetValue(HncRegType.REG_TYPE_Y, 481, out temp1))
                    {
                        CNC.CNCButtonY481 = temp1;
                    }
                    //获取按键面板数据
                    temp1 = 0;
                    if (CNC.RegGetValue(HncRegType.REG_TYPE_Y, 482, out temp1))
                    {
                        CNC.CNCButtonY482 = temp1;
                    }
                    //获取按键面板数据
                    temp1 = 0;
                    if (CNC.RegGetValue(HncRegType.REG_TYPE_Y, 483, out temp1))
                    {
                        CNC.CNCButtonY483 = temp1;
                    }
                    //获取按键面板数据
                    temp1 = 0;
                    if (CNC.RegGetValue(HncRegType.REG_TYPE_Y, 484, out temp1))
                    {
                        CNC.CNCButtonY484 = temp1;
                    }
                    //获取按键面板数据
                    temp1 = 0;
                    if (CNC.RegGetValue(HncRegType.REG_TYPE_Y, 485, out temp1))
                    {
                        CNC.CNCButtonY485 = temp1;
                    }
                    //获取按键面板数据
                    temp1 = 0;
                    if (CNC.RegGetValue(HncRegType.REG_TYPE_Y, 486, out temp1))
                    {
                        CNC.CNCButtonY486 = temp1;
                    }
                    //获取按键面板数据
                    temp1 = 0;
                    if (CNC.RegGetValue(HncRegType.REG_TYPE_R, 29, out temp1))
                    {
                        CNC.CNCButtonR29 = temp1;
                    }
                    var Num = CNC.ToolGetSysToolNum(1);
                    if (Num < 1)
                    {
                        CNC.ToolNum = 30;
                    }
                    if (Num > 100)
                    {
                        CNC.ToolNum = 100;
                    }
                    else
                    {
                        CNC.ToolNum = Num;
                    }
                }

            }
            catch
            {
                ;
            }

        }
        /// <summary>
        /// 采集机床报警
        /// </summary>
        private void CollectAlarm()
        {
            try
            {
                if (CNC.Alarms == null)
                {
                    CNC.Alarms = new Dictionary<int, string>();
                }
                var dataDict = CNC.AlarmGetData();
                if (CNC.Alarms != dataDict)
                {
                    CNC.Alarms.Clear();

                    foreach (var temp in dataDict)
                    {
                        CNC.Alarms.Add(temp.Key, temp.Value);
                    }
                }


            }
            catch
            {
                ;
            }

        }
        /// 采集机床版本信息
        /// </summary>
        private void CollectVersion()
        {
            try
            {
                if (CNC.IsConnected())
                {
                    var datatType = "";
                    object datavalue = -1;

                    if (CNC.SystemGetValue(HncSystem.HNC_SYS_NCK_VER, out datatType, out datavalue))
                    {
                        CNC.NCKVersion = Convert.ToString(datavalue);
                    }
                    if (CNC.SystemGetValue(HncSystem.HNC_SYS_DRV_VER, out datatType, out datavalue))
                    {
                        CNC.DRVVersion = Convert.ToString(datavalue);
                    }
                    if (CNC.SystemGetValue(HncSystem.HNC_SYS_PLC_VER, out datatType, out datavalue))
                    {
                        CNC.PLCVersion = Convert.ToString(datavalue);
                    }
                    if (CNC.SystemGetValue(HncSystem.HNC_SYS_NC_VER, out datatType, out datavalue))
                    {
                        CNC.NCVersion = Convert.ToString(datavalue);
                    }
                  
                    if (CNC.SystemGetValue(HncSystem.HNC_SYS_MACHINE_NUM, out datatType, out datavalue))
                    {
                        CNC.MachineSN = Convert.ToString(datavalue);
                    }
                    if(CNC.AxisGetValue(HncAxis.HNC_AXIS_TYPE, out datatType, out datavalue, 1))
                    {
                        if(Convert.ToString(datavalue) == "0"|| Convert.ToString(datavalue) == "")
                        {
                            CNC.cnctype = CNCType.Lathe;
                        }
                        else
                        {
                            CNC.cnctype = CNCType.CNC;
                        }
                    }
                    if (!CNC.AxisGetValue(HncAxis.HNC_AXIS_TYPE, out datatType, out datavalue, 1))
                    {
                         CNC.cnctype = CNCType.Lathe;
                     
                    }
                }


            }
            catch (System.Exception ex)
            {

            }

        }

        /// <summary>
        /// 采集机床刀具信息
        /// </summary>
        private void CollectTool()
        {
            try
            {

                CNC.TOOLData = new ToolDataConfig[CNC.ToolNum];
                string type = "";
                object value = -1;
                object value1 = -1;
                object value2 = -1;
                object value3 = -1;
                object value4 = -1;

                for (int i = 0; i < CNC.ToolNum; i++)
                {
                    //CNC获取刀具数据
                    CNC.ToolGetToolPara(ToolParaIndex.INFTOOL_ID, out type, out value, i);
                    CNC.TOOLData[i].ToolID = Convert.ToInt32(i);
                    CNC.ToolGetToolPara(ToolParaIndex.GTOOL_LEN1, out type, out value, i + 1);//长度


                    CNC.ToolGetToolPara(ToolParaIndex.GTOOL_RAD1, out type, out value1, i + 1);//半径

                    CNC.ToolGetToolPara(ToolParaIndex.WTOOL_LEN1, out type, out value2, i + 1);//长度磨损
                    CNC.ToolGetToolPara(ToolParaIndex.WTOOL_RAD1, out type, out value3, i + 1);//半径磨损
                    CNC.ToolGetToolPara(ToolParaIndex.GTOOL_LEN3, out type, out value4, i + 1);//Z偏置
                    CNC.TOOLData[i].Length = Convert.ToDouble(value);
                    CNC.TOOLData[i].Radius = Convert.ToDouble(value1);
                    CNC.TOOLData[i].LengthComp = Convert.ToDouble(value2);
                    CNC.TOOLData[i].RadiusComp = Convert.ToDouble(value3);
                    CNC.TOOLData[i].Zlength = Convert.ToDouble(value4);
                }
            }
            catch
            {
                ;
            }

        }

        /// <summary>
        /// 采集机床当前加载的程序
        /// </summary>
        private void CollectProgFile()
        {
            try
            {
                var v = CNC.FtpClient.GetListing();
                foreach (var item in CNC.FtpClient.GetListing())
                {

                    if (item.Type == FtpFileSystemObjectType.File)
                    {
                        var hash = CNC.FtpClient.GetChecksum(item.FullName)?.Value;
                        var f_name = item.FullName;

                        if (item.Name == CNC.GProgram)
                        {
                            var content = string.Empty;
                            try
                            {
                                using (var sr = new StreamReader(CNC.FtpClient.OpenRead(item.FullName)))
                                {

                                    List<string> gcodelist = new List<string>();
                                    int ii = 0;
                                    var line = sr.ReadLine();
                                    if (line == null)
                                    {
                                        CNC.CurProgContentLine = null;
                                        return;
                                    }
                                    while (line != null)
                                    {
                                        gcodelist.Add(line);
                                        line = sr.ReadLine();
                                    }
                                    CNC.CurProgContentLine = gcodelist.ToArray();

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void CollectMeter()
        {
            try
            {
                //获取测量数据
                SDataUnion MacroVale;

                SDataUnion v1 = new SDataUnion();
                if (CNC.MeterValue == null)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        CNC.MeterValue[i] = 0.0;
                    }
                }
                for (int i = 0; i < 20; i++)
                {
                    CNC.MacroVarGetValue(50040 + i, out MacroVale);

                    var temps = MacroVale.v.f;
                    var temp = temps.ToString("F3");
                    CNC.MeterValue[i] = Convert.ToDouble(temp);


                }
            }
            catch
            {
                ;
            }
        }

        public bool SetTool()
        {

            try
            {
                if (!CNC.IsConnected())
                {
                    return false;
                }
                for (int i = 0; i < CNC.ToolNum; i++)
                {
                    if (CNC.TOOLDataChange[i].ToolChangeflag)
                    {
                        double WTOOL_LEN1 = CNC.TOOLDataChange[i].LengthCompAdd + CNC.TOOLData[i].LengthComp;//长度磨损值
                        double WTOOL_RAD1 = CNC.TOOLDataChange[i].RadiusCompAdd + CNC.TOOLData[i].RadiusComp;//半径磨损值
                        int j = Convert.ToInt16(i);
                        if (CNC.ToolSetToolPara(ToolParaIndex.WTOOL_LEN1, WTOOL_LEN1, i + 1) && CNC.ToolSetToolPara(ToolParaIndex.WTOOL_RAD1, WTOOL_RAD1, i + 1))
                        {
                            CNC.TOOLDataChange[i].LengthCompAdd = 0.0;

                            CNC.TOOLDataChange[i].RadiusCompAdd = 0.0;

                            CNC.TOOLDataChange[i].ToolChangeflag = false;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 程序下发
        /// </summary>
        public bool SetProgFile()
        {
            if (!CNC.IsConnected())
            {
                return false;
            }
            try
            {
                string path = CNC.SetProgPath;// "C:\\Users\\Public\\MESFile\\CncProg\\OMDI";
                string remote = "/h/lnc8/prog/";
                //string name = "OMDI";
                string ip = CNC.IP + ":10021";
                // using (var sr = new StreamWriter(CNC.FtpClient.OpenWrite(cncEvent.GCodeUrl)))
                //var ret = CNC.FtpClient.UploadFile( path, "h/lnc8/prog/");
                if (CNC.SysCtrlUpLoadFile(remote, path, ip))
                {
                    return true;
                }
                else
                {

                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }



        }
    }
}
