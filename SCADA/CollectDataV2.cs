using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HNC.API;
using System.Threading;

namespace SCADA
{
   public  class CollectDataV2
    {
        public List<CollectCNCData> GetCNCDataLst = new List<CollectCNCData>();
        private List<FEquipment> cncv2list;
        private Thread cnc_connectThread;
        public CollectDataV2( List<FEquipment> cnclist)
        {
            cncv2list = cnclist;
        }
    
        /// <summary>
        /// 开启采集线程的线程
        /// </summary>
        public void StartConnectThread()
        {
            for (int ii = 0; ii < cncv2list.Count; ii++)
            {
                var ip = cncv2list[ii].IP;
                CollectCNCData m_hcncdatanade = new CollectCNCData(ip);
                GetCNCDataLst.Add(m_hcncdatanade);
            }

            for (int ii = 0; ii < GetCNCDataLst.Count; ii++)
            {
                GetCNCDataLst[ii].ThreadStart();
            }
            //m_bEventThreadRunning = true;
            threaFucEvent.Set();
        }
        /// <summary>
        /// 采集器退出
        /// </summary>
        public void CollectExit()
        {
            //m_bEventThreadRunning = false;
            threaFucEvent.Set();
            foreach (CollectCNCData item in GetCNCDataLst)
            {
                item.ThreadStop();
            }
            //  LogApi.WriteLogInfo(MainForm.logHandle, (Byte)ENUM_LOG_LEVEL.LEVEL_WARN, "Collect Exit!");
        }

        public static System.Threading.AutoResetEvent threaFucEvent = new System.Threading.AutoResetEvent(true);
        /// <summary>
        /// 开始采集
        /// </summary>
        public void StartCollection()
        {
            StartConnectThread();
            //StartEventThread();
        }

    }

}
