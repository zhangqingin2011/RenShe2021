﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using Thrift;

namespace HNCAPI_INTERFACE
{
    public delegate void WriteDelegate(String content, Int32 result);
    public delegate void ConnDelegate(String ip, UInt16 port, Boolean result);

    public class HncApi
    {
        #region Net
        public String IP { get { return m_ip; } }
        public UInt16 Port { get { return m_port; } }
        public Int16 MachineNo { get { return m_machineNo; } }
        //public String ClientName { get { return m_clientName; } }
        /*!	@brief 网络初始化
	     * @param[in] ip ：IP地址
	     * @param[in] port ：端口号
	     * @param[in] name ：进程名
	     * @return 
	     * -  0：网络初始化成功；
	     * - -1：网络初始化失败；
	 
	     * @attention 使用HNC_NetConnect函数之前必须先调用本函数
	     * @par 示例:
	     * @code
	       //此处输入的IP为上位机本地IP
	       int ret = HNC_NetInit("10.10.56.40", 10001, "CppTest");
	     * @endcode     
	
	     * @see :: HNC_NetConnect
	     */
        public Int32 HNC_NetInit(String ip, UInt16 port, String name)
        {
            if (m_isInit)
            {
                return 0;
            }
            try
            {
                m_thriftPool = new ThriftPool(ip, port);

                m_dicDelegate = new Dictionary<String, WriteDelegate>();
                m_dicDelegate.Clear();
                m_dicLock = new object();
                m_writeEventScanThreadStart = true;
                m_writeEventScanThread = new Thread(new ThreadStart(WriteEventScanThreadFunc));
                m_writeEventScanThread.Start();

                m_connHandler = null;

                m_machineNo = -1;
                m_clientName = name;

                m_isInit = true;
                //自动连接
                m_AutoConnectThreadStart = false; //线程运行状态
                m_AutoCThread = null; //此线程在接口中创建

            }
            catch (TApplicationException x)
            {
                Log.WriteLine(x.ToString());
                return -1;
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
                return -1;
            }

            Log.WriteLine("hnc init success!");
            return 0;
        }
        /*!	@brief 断开网络
	     *
	     * @attention 注意事项
	     * @par 示例:
	     * @code
	       HNC_NetExit();
	     * @endcode     
	
	     * @see :: HNC_NetConnect
	     */
        public void HNC_NetExit()
        {
            if (!m_isInit)
            {
                return;
            }
            m_writeEventScanThreadStart = false;
            m_writeEventScanThread.Join();

            //判断是否创建重连线程
            if(m_AutoConnectThreadStart)
            {
                m_AutoConnectThreadStart = false;
                m_AutoCThread.Join();//自动连接关闭
            }
            

            if (m_connThread != null && m_connThread.IsAlive)
            {
                m_connThread.Abort();
            }
        }
        /*!	@brief 网络连接
	     * @param[in] ip ：IP地址
	     * @param[in] port ：端口号
         * @param[in] ConnHandler ：连接委托，用来判断连接是否成功
	     * @return 
         * - 0~255: 返回机器号；
	     * - -1：连接失败；
	 
	     * @attention 使用本函数之前必须先调用HNC_NetInit，连接是否成功不能通过返回值判断
	     * @par 示例:
	     * @code
	       //此处输入的IP为要连接的下位机IP
           WriteDelegate deleFunc = new ConnDelegate(CallBack);
	       int ret = HNC_NetConnect("10.10.56.223", 10001, deleFunc);
	     * @endcode     
	
	     * @see :: HNC_NetInit
         */
        public Int16 HNC_NetConnect(String ip, UInt16 port, ConnDelegate ConnHandler)
        {
            m_ip = ip;
            m_port = port;
            if (ConnHandler != null)
            {
            	m_connHandler = ConnHandler;
            }
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                    m_machineNo = autoClient.Client.Connect(ip, port, m_clientName);
                }
                
                if (ConnHandler != null)
                {
                    m_connThread = new Thread(new ThreadStart(ConnThreadFunc));
                    m_connThread.Start();
                }
                
                return m_machineNo;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 网络连接
	     * @param[in] ip ：IP地址
	     * @param[in] port ：端口号
	     * @return 
         * - 0~255: 返回机器号；
	     * - -1：连接失败；
	 
	     * @attention 使用本函数之前必须先调用HNC_NetInit，连接是否成功不能通过返回值判断
	     * @par 示例:
	     * @code
	       //此处输入的IP为要连接的下位机IP
	       int ret = HNC_NetConnect("10.10.56.223", 10001);
	     * @endcode     
	
	     * @see :: HNC_NetInit
         */
        public Int16 HNC_NetConnect(String ip, UInt16 port)
        {
            m_ip = ip;
            m_port = port;

            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                    m_machineNo = autoClient.Client.Connect(ip, port, m_clientName);
                }

                return m_machineNo;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 判断网络是否连接
	     * @param[in] ip ：IP地址
	     * @param[in] port ：端口号
	     * @return 
	     * -  0：已连接；
	     * - -1：未连接；
	 
	     * @attention 使用本函数之前必须先调用HNC_NetInit
	     * @par 示例:
	     * @code
	       //此处输入的IP为下位机IP
	       int ret = HNC_NetIsConnect("10.10.56.223", 10001);
	     * @endcode     
	
	     * @see :: HNC_NetInit
	     */
        public Int16 HNC_NetIsConnect(String ip, UInt16 port)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.IsConnect(ip, port);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 判断网络是否连接
	     * @return 
	     * -  0：已连接；
	     * - -1：未连接；
	 
	     * @attention 使用本函数之前必须先调用HNC_NetInit
	     * @par 示例:
	     * @code
	       //说明
	       int ret = HNC_NetIsConnect(); 
	     * @endcode     
	
	     * @see :: HNC_NetInit
	     */
        public Int16 HNC_NetIsConnect()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.IsMachineConnect(m_machineNo);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 是否开启自动重连,默认为不重连
         *         调用本函数前必须调用HNC_NetInit();
         * - true：开启；
         * - false：关闭；	  
         *调用:
         *  HNC_NetAutoConnect(true);//开启自动重连
         */
        public void HNC_NetAutoConnect(Boolean connStatus)
        {
            if (connStatus && m_AutoConnectThreadStart == false)//防止多次初始化
            {//创建线程并开启
                m_AutoConnectThreadStart = true;
                m_AutoCThread = new Thread(new ThreadStart(AutoCThreadFunc));
                m_AutoCThread.Start(); //每30s检测一次
            }
            else if(!connStatus && m_AutoConnectThreadStart == true)
            {
                m_AutoConnectThreadStart = false;
                m_AutoCThread.Join();//释放线程资源
                m_AutoCThread = null;
            }
        }
        /*!	@brief 写令牌请求
         * 内部函数，在调用写值接口时前内部调用该函数
         * @return 
         * -  true：开启写令牌成功；
         * - false：开启写令牌失败；	   
         */
        public bool HNC_ClientRequestWriteToken()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.ClientRequestWriteToken(m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return false;
            }
        }
        /*!	@brief 写令牌释放	 
	     * 内部函数，在调用写值接口时前内部调用该函数    
	     */
        public void HNC_ClientReleaseWriteToken()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ClientReleaseWriteToken(m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Reg
        /*!	@brief 获取寄存器的值
         * 
         * @param[in] type ：寄存器类型
         * @param[in] index ：寄存器索引号
         * @param[out] value ：寄存器值
         * @return 
         * -  0：取值成功；
         * - -1：取值失败；

         * @attention X,Y,R,F,G寄存器取值后需要转为正值
         * @par 示例:
         * @code
           //获取寄存器X[1]的值
           int type = REG_TYPE_X;
           int index = 1;
           int value = 0;
           int ret = HNC_RegGetValue(type, index, ref value);
         * @endcode     

         * @see :: 
         */
        public Int32 HNC_RegGetValue(Int32 type, Int32 index, ref Int32 value)
        {            
            try
            {
                StRegGetVal st = new StRegGetVal();

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.RegGetValue(type, index, m_machineNo);
                }
                
                if(st.Ret != 0)
                {
                    return st.Ret;
                }
                value = st.Data;
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }            
        }
        /*!	@brief 获取多个寄存器的值
	     * 
	     * @param[in] type ：寄存器类型
	     * @param[in] index ：寄存器索引号
	     * @param[in] num ：要获取的寄存器个数
	     * @param[out] value ：要获取的寄存器字节数组
	     * @return 
	     * -  0：取值成功；
	     * - -1：取值失败；
	 
	     * @attention X,Y,R,F,G寄存器取值后需要转为正值
	     * @par 示例:
	     * @code
	       //获取寄存器X[1]及之后的5个值
	       byte value[5];
	       Int32 ret = HNC_RegGetMultiValues(REG_TYPE_X, 1, 5, ref value);
	     * @endcode     
	
	     * @see :: HNC_RegGetValue
	     */
        public Int32 HNC_RegGetMultiValues(Int32 type, Int32 index, Int32 num, ref byte[] value)
        {
            try
            {
                StRegGetMulVal st = new StRegGetMulVal();

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.RegGetMultiValues(type, index, num, m_machineNo);
                }
                
                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = st.Data;
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置寄存器的值
	    * 
	    * @param[in] type ：寄存器类型
	    * @param[in] index ：寄存器索引号
	    * @param[in] value ：待写入的寄存器值
	    * @return 
        * -  0：只返回0

	    * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	    * @par 示例:
	    * @code
	    //设置寄存器X[1]的值为2
	    int type = REG_TYPE_X;
	    int index = 1;
	    int value = 2;
	    HNC_RegSetValue(type, index, value);
	    * @endcode     
	    */
        public Int32 HNC_RegSetValue(Int32 type, Int32 index, Int32 value)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.RegSetValue(type, index, value, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置寄存器的值
	    * 
	    * @param[in] type ：寄存器类型
	    * @param[in] index ：寄存器索引号
	    * @param[in] value ：待写入的寄存器值
	    * @param[in] DelegateFunc ：写值回调函数指针
	 
	    * @attention
	    * @par 示例:
	    * @code
	    //设置寄存器X[1]的值为2
	    void CallBack(String content, Int32 result) { ... }
	    int type = REG_TYPE_X;
	    int index = 1;
	    int value = 2;
        WriteDelegate deleFunc = new WriteDelegate(CallBack);
	    HNC_RegSetValue(type, index, value, deleFunc);
	    * @endcode     
	    */
        public void HNC_RegSetValue(Int32 type, Int32 index, Int32 value, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var st = new
                    {
                        func = "RegSetValue",
                        type = type.ToString(),
                        index = index.ToString(),
                        data = value.ToString()
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.RegSetValue(type, index, value, m_machineNo, m_clientName);
                }
            }
            catch(Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 获取寄存器的总组数
	     * 
	     * @param[in] type ：寄存器类型
	     * @param[out] num ：寄存器组数
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention num = -1表示寄存器类型不匹配
	     * @par 示例:
	     * @code
	       //获取寄存器X的总组数
	       int type = REG_TYPE_X;
	       int num = 0;
	       HNC_RegGetNum(type, ref num);
	     * @endcode      
	 */
        public Int32 HNC_RegGetNum(Int32 type, ref Int32 num)
        {
            try
            {
                StRegGetNum st = new StRegGetNum();

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.RegGetNum(type, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                num = st.Num;
                return st.Ret;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }            
        }
        /*!	@brief 获取FG寄存器的基址
	     * 
	     * @param[in] baseType ：FG寄存器基址类型
	     * @param[out] baseIndex ：基址寄存器组号
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取FG寄存器系统数据基址
	       int baseType = REG_FG_SYS_BASE;
	       int baseIndex = 0;
	       HNC_RegGetFGBase(baseTye, ref baseIndex);
	     * @endcode     
	     */
        public Int32 HNC_RegGetFGBase(Int32 baseType, ref Int32 baseIndex)
        {
            try
            {
	            StRegGetFGBase st = new StRegGetFGBase();

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.RegGetFGBase(baseType, m_machineNo);
                }
                
                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                baseIndex = st.BaseIndex;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;	
            }
        }
        /*!	@brief 将寄存器数据某一位置1
	     * 
	     * @param[in] type ：寄存器类型
	     * @param[in] index ：寄存器索引号
	     * @param[in] bit ：X, Y, R：-1~7，F, G, W：-1~31，D, B, P：-1~31，I,Q,K：-1~7
	     * @return 
         * -  0：只返回0

	     * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //将寄存器X[1]的第2位置1
	       int type = REG_TYPE_X;
	       int index = 1;
	       int bit = 2;
	       HNC_RegSetBit(type, index, bit);
	     * @endcode     
	     */
        public Int32 HNC_RegSetBit(Int32 type, Int32 index, Int32 bit)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.RegSetBit(type, index, bit, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 将寄存器数据某一位置1
	     * 
	     * @param[in] type ：寄存器类型
	     * @param[in] index ：寄存器索引号
	     * @param[in] bit ：X, Y, R：-1~7，F, G, W：-1~31，D, B, P：-1~31，I,Q,K：-1~7
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //将寄存器X[1]的第2位置1
	       void CallBack(String content, Int32 result) { ... }
	       int type = REG_TYPE_X;
	       int index = 1;
	       int bit = 2;
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_RegSetBit(type, index, bit, deleFunc);
	     * @endcode     
	     */
        public void HNC_RegSetBit(Int32 type, Int32 index, Int32 bit, WriteDelegate DelegateFunc)
        {
            try
            {
                if(DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var st = new
                    {
                        func = "RegSetBit",
                        type = type.ToString(),
                        index = index.ToString(),
                        bit = bit.ToString()
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.RegSetBit(type, index, bit, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 将寄存器数据某一位清0
	     * 
	     * @param[in] type ：寄存器类型
	     * @param[in] index ：寄存器索引号
	     * @param[in] bit ：X, Y, R：-1~7，F, G, W：-1~31，D, B, P：-1~31，I,Q,K：-1~7
         * @return 
         * -  0：只返回0

	     * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //将寄存器X[1]的第2位清零
	       int type = REG_TYPE_X;
	       int index = 1;
	       int bit = 2;
	       HNC_RegClrBit(type, index, bit);
	     * @endcode      
	     */
        public Int32 HNC_RegClrBit(Int32 type, Int32 index, Int32 bit)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.RegClrBit(type, index, bit, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 将寄存器数据某一位清0
	     * 
	     * @param[in] type ：寄存器类型
	     * @param[in] index ：寄存器索引号
	     * @param[in] bit ：X, Y, R：-1~7，F, G, W：-1~31，D, B, P：-1~31，I,Q,K：-1~7
	     * @param[in] DelegateFunc ：写值回调函数指针

	     * @attention 
	     * @par 示例:
	     * @code
	       //将寄存器X[1]的第2位清零
	       void CallBack(String content, Int32 result) { ... }
	       int type = REG_TYPE_X;
	       int index = 1;
	       int bit = 2;
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_RegClrBit(type, index, bit, deleFunc);
	     * @endcode      
	     */
        public void HNC_RegClrBit(Int32 type, Int32 index, Int32 bit, WriteDelegate DelegateFunc)
        {
            try
            {
                if(DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var st = new
                    {
                        func = "RegClrBit",
                        type = type.ToString(),
                        index = index.ToString(),
                        bit = bit.ToString()
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.RegClrBit(type, index, bit, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Var
        /*!	@brief 按类型获取变量的值
	     * 
	     * @param[in] type ：变量类型(VAR_TYPE_AXIS, VAR_TYPE_CHANNEL, VAR_TYPE_SYSTEM)
	     * @param[in] no ：轴号或者通道号
	 				       VAR_TYPE_AXIS: 0~31
	 				       VAR_TYPE_CHANNEL: 0~3
	 				       VAR_TYPE_SYSTEM: no无效
	     * @param[in] index ：索引号
	 					      VAR_TYPE_AXIS: 0~399
	 					      VAR_TYPE_CHANNEL: 0~1999
	 					      VAR_TYPE_SYSTEM: 0~9999
	     * @param[out] value ：变量值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 1.当type为VAR_TYPE_AXIS, VAR_TYPE_CHANNEL, VAR_TYPE_SYSTEM时调用
                      2.轴变量个数由100扩容到400
				      3.取轴变量时，未配置轴的轴变量无法取值
	     * @par 示例:
	     * @code
	       //获取轴变量0通道1索引的值
	       int value = 0;
	       int ret = HNC_VarGetValue(VAR_TYPE_AXIS, 0, 1, ref value);
	     * @endcode     
	     */
        public Int32 HNC_VarGetValue(Int32 type, Int32 no, Int32 index, ref Int32 value)    //Bit32
        {
            try
            {
	            StVarGetVal st = new StVarGetVal();

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.VarGetValue(type, no, index, m_machineNo);
                }
                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = BitConverter.ToInt32(st.Data, 0);
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 按类型获取变量的值
	     * 
	     * @param[in] type ：变量类型(VAR_TYPE_SYSTEM_F)
	     * @param[in] no ：轴号或者通道号
	 				       VAR_TYPE_SYSTEM_F: no无效
	     * @param[in] index ：索引号
	 					      VAR_TYPE_SYSTEM_F: 0~9999
	     * @param[out] value ：变量值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 当type为VAR_TYPE_SYSTEM_F时调用
	     * @par 示例:
	     * @code
	       //获取系统变量0通道1索引的值
	       double value = 0;
	       int ret = HNC_VarGetValue(VAR_TYPE_SYSTEM_F, 0, 1, ref value);
	     * @endcode      
	     */
        public Int32 HNC_VarGetValue(Int32 type, Int32 no, Int32 index, ref Double value)   //fBit64
        {
            try
            {
	            StVarGetVal st = new StVarGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.VarGetValue(type, no, index, m_machineNo);
                }
                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = BitConverter.ToDouble(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 批量获取变量值
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] index ：索引号
	     * @param[in] num ：要获取的变量值数目
	     * @param[out] value ：要获取的变量值数组
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 不能获取浮点型系统变量。轴变量总数：轴数*400；通道变量总数：通道数*2000；系统变量总数100000
	     * @par 示例:
	     * @code
	       //获取通道变量通道2索引3的5个变量值
           Int32[] value = new Int32[5];
	       Bit32 ret = HNC_VarGetMultiValues(VAR_TYPE_CHANNEL, 2*2000+3, 5, ref value);
	     * @endcode     
	
	     * @see :: HNC_VarGetValue
	     */
        public Int32 HNC_VarGetMultiValues(Int32 type, Int32 index, Int32 num, ref Int32[] value)
        {
            try
            {
                StVarGetMulVal st = new StVarGetMulVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.VarGetMultiValues(type, index, num, m_machineNo);
                }
                if (st.Ret != 0)
                {
                    return st.Ret;
                }

                //检查数据长度是否匹配
                Int32 size = Marshal.SizeOf(value[0]) * num;
                System.Diagnostics.Debug.Assert(size == st.Data.Length);

                IntPtr arrayPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy(st.Data, 0, arrayPtr, size);
                Marshal.Copy(arrayPtr, value, 0, num);
                Marshal.FreeHGlobal(arrayPtr);

                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 按类型设置变量的值
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] no ：轴号或通道号，参见HNC_VarGetValue
	     * @param[in] index ：索引号，参见HNC_VarGetValue
	     * @param[in] value ：变量值
         * @return 
         * -  0：只返回0

	     * @attention 1.当type为VAR_TYPE_AXIS, VAR_TYPE_CHANNEL, VAR_TYPE_SYSTEM时调用
         *            2.写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //设置轴变量0通道1索引的值为2
	       HNC_VarSetValue(VAR_TYPE_AXIS, 0, 1, 2);
	     * @endcode   

	     * @see :: HNC_VarGetValue
	     */
        public Int32 HNC_VarSetValue(Int32 type, Int32 no, Int32 index, Int32 value)    //Bit32
        {
            try
            {
                Byte[] val = new Byte[sizeof(Double)];  //VAR_DATA_LEN
                Byte[] temp = System.BitConverter.GetBytes(value);
                Array.Copy(temp, val, sizeof(Int32));

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VarSetValue(type, no, index, val, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 按类型设置变量的值
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] no ：轴号或通道号，参见HNC_VarGetValue
	     * @param[in] index ：索引号，参见HNC_VarGetValue
	     * @param[in] value ：变量值
         * @return 
         * -  0：只返回0

	     * @attention 1.当type为VAR_TYPE_SYSTEM_F时调用；
         *            2.写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //设置系统变量0通道1索引的值为2
	       HNC_VarSetValue(VAR_TYPE_SYSTEM_F, 0, 1, 2);
	     * @endcode   

	     * @see :: HNC_VarGetValue
	     */
        public Int32 HNC_VarSetValue(Int32 type, Int32 no, Int32 index, Double value)   //fBit64
        {
            try
            {
	            Byte[] val = System.BitConverter.GetBytes(value);

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VarSetValue(type, no, index, val, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 按类型设置变量的值
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] no ：轴号或通道号，参见HNC_VarGetValue
	     * @param[in] index ：索引号，参见HNC_VarGetValue
	     * @param[in] value ：变量值
	     * @param[in] DelegateFunc ：写值回调函数指针

	     * @attention 当type为VAR_TYPE_AXIS, VAR_TYPE_CHANNEL, VAR_TYPE_SYSTEM时调用
	     * @par 示例:
	     * @code
	       //设置轴变量0通道1索引的值为2
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_VarSetValue(VAR_TYPE_AXIS, 0, 1, 2, deleFunc);
	     * @endcode   

	     * @see :: HNC_VarGetValue
	     */
        public void HNC_VarSetValue(Int32 type, Int32 no, Int32 index, Int32 value, WriteDelegate DelegateFunc)    //Bit32
        {
            try
            {                
                Byte[] val = new Byte[sizeof(Double)];  //VAR_DATA_LEN
                Byte[] temp = System.BitConverter.GetBytes(value);
                Array.Copy(temp, val, sizeof(Int32));

                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    
                    var st = new
                    {
                        func = "VarSetValue",
                        type = type.ToString(),
                        no = no.ToString(),
                        index = index.ToString(),
                        value = value.ToString()
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VarSetValue(type, no, index, val, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 按类型设置变量的值
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] no ：轴号或通道号，参见HNC_VarGetValue
	     * @param[in] index ：索引号，参见HNC_VarGetValue
	     * @param[in] value ：变量值
	     * @param[in] DelegateFunc ：写值回调函数指针

	     * @attention 当type为VAR_TYPE_SYSTEM_F时调用
	     * @par 示例:
	     * @code
	       //设置系统变量0通道1索引的值为2
	       void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_VarSetValue(VAR_TYPE_SYSTEM_F, 0, 1, 2, deleFunc);
	     * @endcode   

	     * @see :: HNC_VarGetValue
	     */
        public void HNC_VarSetValue(Int32 type, Int32 no, Int32 index, Double value, WriteDelegate DelegateFunc)
        {
            try
            {                
	            Byte[] val = System.BitConverter.GetBytes(value);

                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "VarSetValue",
                        type = type.ToString(),
                        no = no.ToString(),
                        index = index.ToString(),
                        value = value.ToString("f6")
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VarSetValue(type, no, index, val, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 按类型设置变量数据的一位（置1)
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] no ：轴号或通道号，参见HNC_VarGetValue
	     * @param[in] index ：索引号，参见HNC_VarGetValue
	     * @param[in] bit ：变量位号-1~31
	     * @return 
         * -  0：只返回0

	     * @attention 1.当bit=-1，设置变量值为1；当bit>-1，将变量的第bit位设置为1
	     *			  2.VAR_TYPE_SYSTEM_F不支持位设置
         *            3.写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //轴变量0通道1索引第2位置1
	       HNC_VarSetBit(VAR_TYPE_AXIS, 0, 1, 2);
	     * @endcode  

	     * @see :: HNC_VarGetValue
	     */
        public Int32 HNC_VarSetBit(Int32 type, Int32 no, Int32 index, Int16 bit)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VarSetBit(type, no, index, bit, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 按类型设置变量数据的一位（置1)
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] no ：轴号或通道号，参见HNC_VarGetValue
	     * @param[in] index ：索引号，参见HNC_VarGetValue
	     * @param[in] bit ：变量位号-1~31
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 1.当bit=-1，设置变量值为1；当bit>-1，将变量的第bit位设置为1
	     *			  2.VAR_TYPE_SYSTEM_F不支持位设置
	     * @par 示例:
	     * @code
	       //轴变量0通道1索引第2位置1
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_VarSetBit(VAR_TYPE_AXIS, 0, 1, 2, deleFunc);
	     * @endcode  

	     * @see :: HNC_VarGetValue
	     */
        public void HNC_VarSetBit(Int32 type, Int32 no, Int32 index, Int16 bit, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "VarSetBit",
                        type = type.ToString(),
                        no = no.ToString(),
                        index = index.ToString(),
                        bit = bit.ToString()
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VarSetBit(type, no, index, bit, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 按类型清除变量数据的一位（置0)
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] no ：轴号或通道号，参见HNC_VarGetValue
	     * @param[in] index ：索引号，参见HNC_VarGetValue
	     * @param[in] bit ：变量位号-1~31
	     * @return 
         * -  0：只返回0

	     * @attention 1.当bit=-1，设置变量值为0；当bit>-1，将变量的第bit位设置为0
	     *			  2.VAR_TYPE_SYSTEM_F不支持位清零
         *            3.写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //轴变量0通道1索引第2位清零
	       HNC_VarClrBit(VAR_TYPE_AXIS, 0, 1, 2);
	     * @endcode  

	     * @see :: HNC_VarGetValue
	     */
        public Int32 HNC_VarClrBit(Int32 type, Int32 no, Int32 index, Int16 bit)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VarClrBit(type, no, index, bit, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 按类型清除变量数据的一位（置0)
	     * 
	     * @param[in] type ：变量类型
	     * @param[in] no ：轴号或通道号，参见HNC_VarGetValue
	     * @param[in] index ：索引号，参见HNC_VarGetValue
	     * @param[in] bit ：变量位号-1~31
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 1.当bit=-1，设置变量值为0；当bit>-1，将变量的第bit位设置为0
	     *			  2.VAR_TYPE_SYSTEM_F不支持位清零
	     * @par 示例:
	     * @code
	       //轴变量0通道1索引第2位清零
	       void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_VarClrBit(VAR_TYPE_AXIS, 0, 1, 2, deleFunc);
	     * @endcode  

	     * @see :: HNC_VarGetValue
	     */
        public void HNC_VarClrBit(Int32 type, Int32 no, Int32 index, Int16 bit, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "VarClrBit",
                        type = type.ToString(),
                        no = no.ToString(),
                        index = index.ToString(),
                        bit = bit.ToString()
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VarClrBit(type, no, index, bit, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 按索引号获取宏变量的值
	     * 
	     * @param[in] no ：变量编号
	  				       [0, 2999]: 通道变量
	  				       [40000, 59999]: 系统变量；其中，[50000, 54999]：用户自定义变量
	  				       [60000, 69999]: 轴变量
	  				       [70000, 99999]: 刀具变量
	     * @param[out] value ：宏变量值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention
	     * @par 示例:
	     * @code
	       //获取宏变量54000的值
	       SDataUnion val = new SDataUnion();
	       int ret = HNC_MacroVarGetValue(54000, ref val);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_MacroVarGetValue(Int32 no, ref SDataUnion value)   //SDataUnion
        {
            try
            {
	            StMacVarGetVal st = new StMacVarGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.MacroVarGetValue(no, m_machineNo);
                }
                
                if (st.Ret != 0)
                {
                    return st.Ret;
                }

                //检查数据长度是否匹配
                Int32 size = Marshal.SizeOf(value);
	            System.Diagnostics.Debug.Assert(size == st.Data.Length);
	
	            //将字节数组转为SDataUnion结构体
	            IntPtr structPtr = Marshal.AllocHGlobal(size);
	            Marshal.Copy(st.Data, 0, structPtr, size);
	            value = (SDataUnion)Marshal.PtrToStructure(structPtr, typeof(SDataUnion));
	            Marshal.FreeHGlobal(structPtr);
	
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 批量获取宏变量的值
         * 
         * @param[in] startNo ：宏变量起始编号
	  				            [0, 2999]: 通道变量
	  				            [40000, 59999]: 系统变量；其中，[50000, 54999]：用户自定义变量
	  				            [60000, 69999]: 轴变量
	  				            [70000, 99999]: 刀具变量
         * @param[in] num ：要获取的宏变量个数
         * @param[out] value ：宏变量值
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 只能获取用户自定义宏变量[50000, 54999]
         * @par 示例:
         * @code
           //获取54000到54004的宏变量值
           int num = 5;
	       SDataUnion[] val = new SDataUnion[num];
	       int ret = HNC_MacroVarGetMultiValues(54000, num, ref val);
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_MacroVarGetMultiValues(Int32 startNo, Int32 num, ref SDataUnion[] value)   //SDataUnion
        {
            try
            {
                StMacVarGetMulVals st = new StMacVarGetMulVals();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.MacroVarGetMultiValues(startNo, num, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }

                //检查数据长度是否匹配
                Int32 size = Marshal.SizeOf(value[0]);
                System.Diagnostics.Debug.Assert(value.Length * size == st.Data.Length);

                //将字节数组转为SDataUnion结构体
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                for(int i = 0; i < num; i++)
                {
                    Marshal.Copy(st.Data, i * size, structPtr, size);
                    value[i] = (SDataUnion)Marshal.PtrToStructure(structPtr, typeof(SDataUnion));
                }                
                Marshal.FreeHGlobal(structPtr);

                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 按索引号设置宏变量的值
	     * 
	     * @param[in] no ：变量编号，参见HNC_MacroVarGetValue
	     * @param[in] value ：宏变量值
	     * @return 
         * -  0：只返回0

	     * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //设置宏变量54000的值
	       SDataUnion val = new SDataUnion();
	       memset(&val, 0, sizeof(SDataUnion));
	       val.g90 = 0;
	       val.type = 1;
	       val.v.i = 2;
	       HNC_MacroVarSetValue(54000, val);
	     * @endcode  
	     * @see :: HNC_MacroVarGetValue
	     */
        public Int32 HNC_MacroVarSetValue(Int32 no, SDataUnion value)
        {
            try
            {
	            Int32 size = Marshal.SizeOf(value);
	
	            //将SDataUnion结构体转为字节数组
	            Byte[] arrData = new Byte[size];
	            IntPtr structPtr = Marshal.AllocHGlobal(size);
	            Marshal.StructureToPtr(value, structPtr, false);
	            Marshal.Copy(structPtr, arrData, 0, size);
	            Marshal.FreeHGlobal(structPtr);

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.MacroVarSetValue(no, arrData, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 按索引号设置宏变量的值
	     * 
	     * @param[in] no ：变量编号，参见HNC_MacroVarGetValue
	     * @param[in] value ：宏变量值
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //设置宏变量54000的值
	       SDataUnion val = new SDataUnion();
	       memset(&val, 0, sizeof(SDataUnion));
	       val.g90 = 0;
	       val.type = 1;
	       val.v.i = 2;
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_MacroVarSetValue(54000, val, deleFunc);
	     * @endcode  
	     * @see :: HNC_MacroVarGetValue
	     */
        public void HNC_MacroVarSetValue(Int32 no, SDataUnion value, WriteDelegate DelegateFunc)
        {
            try
            {
                Int32 size = Marshal.SizeOf(value);

                //将SDataUnion结构体转为字节数组
                Byte[] arrData = new Byte[size];
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(value, structPtr, false);
                Marshal.Copy(structPtr, arrData, 0, size);
                Marshal.FreeHGlobal(structPtr);

                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    String str = String.Empty;
                    switch(value.type)
                    {
                        case HNCDATATYPE.DTYPE_INTEGER:
                            str = value.v.i.ToString();
                            break;
                        case HNCDATATYPE.DTYPE_FLOAT:
                            str = value.v.f.ToString("f6");
                            break;
                        default:
                            str = value.v.n.ToString();
                            break;
                    }

                    var stMac = new
                    {
                        type = value.type.ToString(),
                        g90 = value.g90.ToString(),
                        data = str
                    };

                    var st = new
                    {
                        func = "MacroVarSetValue",
                        no = no.ToString(),
                        SDataUnion = stMac
                    };
                    String key = js.Serialize(st);
                    
                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.MacroVarSetValue(no, arrData, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Parm
        /*!	@brief 获取参数属性值（按parmId参数编号获取）
	     * 
	     * @param[in] parmId ：参数编号
	     * @param[in] propType ：参数属性的类型
	     * @param[out] propValue ：参数属性的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //示例
	       SDataProperty prop = new SDataProperty();
	       int ret = HNC_ParamanGetParaProp(105210, PARA_PROP_NAME, ref prop);
	       string name = prop.value.val_string; //"最大转矩电流限幅"
	       ret = HNC_ParamanGetParaProp(105210, PARA_PROP_DFVALUE, ref prop);
	       int def = prop.value.val_int; //200
	       ret = HNC_ParamanGetParaProp(105210, PARA_PROP_VALUE, ref prop);
	       int value = prop.value.val_int; //200
	     * @endcode     
	
	     * @see 
	     */
        public Int32 HNC_ParamanGetParaProp(Int32 parmId, sbyte propType, ref SDataProperty propValue)
        {
            try
            {
	            StParaGetParaProp st = new StParaGetParaProp();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ParamanGetParaProp(parmId, propType, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }

                propValue.type = (SByte)st.Data[0];
	            switch (propValue.type)
	            {
	                case (SByte)HNCDATATYPE.DTYPE_INTEGER:
	                case (SByte)HNCDATATYPE.DTYPE_UINT:
	                case (SByte)HNCDATATYPE.DTYPE_BOOL:
	                case (SByte)HNCDATATYPE.DTYPE_HEX4:
	                    propValue.integer = BitConverter.ToInt32(st.Data, SDataProperty.PARA_VALUE_START_INDEX);
	                    break;
	                case (SByte)HNCDATATYPE.DTYPE_FLOAT:
	                    propValue.real = BitConverter.ToDouble(st.Data, SDataProperty.PARA_VALUE_START_INDEX);
	                    break;
	                case (SByte)HNCDATATYPE.DTYPE_STRING:      //字符串直接返回0
	                    propValue.str = Encoding.Default.GetString(st.Data, SDataProperty.PARA_VALUE_START_INDEX, SDataProperty.PARA_STRING_LEN).TrimEnd('\0');
                        propValue.str = propValue.str.Split('\0')[0];
                        break;
	                case (SByte)HNCDATATYPE.DTYPE_BYTE:
	                    Array.Copy(st.Data, SDataProperty.PARA_VALUE_START_INDEX, propValue.array, 0, SDataProperty.PARA_BYTE_ARRAY_LEN);
	                    break;
	                default:
	                    break;
	            }
	
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 批量获取参数属性值（按parmId参数编号获取）
	     * 
	     * @param[in] parmId ：参数编号
	     * @param[in] propType ：参数属性的类型，该参数无效
	     * @param[out] props ：参数属性值结构体变量
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention propType参数无效
	     * @par 示例:
	     * @code
	       //批量获取参数属性值
	       SParmProperty props = new props();
	       Bit32 ret = HNC_ParamanGetProps(10033, 0, ref props);
	     * @endcode     
	
	     * @see :: HNC_ParamanGetParaProp
	     */
        public Int32 HNC_ParamanGetProps(Int32 parmId, sbyte propType, ref SParmProperty props)
        {
            try
            {
                StParaGetProps st = new StParaGetProps();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ParamanGetProps(parmId, propType, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                props.id = st.Id;
                props.name = st.Name;
                props.access = st.Access;
                props.actType = st.ActType;
                props.value = st.Value;
                props.defaltVal = st.DefaltVal;
                props.maxVal = st.MaxVal;
                props.minVal = st.MinVal;
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置参数属性的值（包括参数值、最大值、最小值、缺省值名称）
	     * 
	     * @param[in] parmId ：参数编号
	     * @param[in] propType ：参数属性的类型
	     * @param[in] propValue ：参数属性的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 1.当修改PARA_PROP_VALUE参数值后，必须调用保存接口HNC_ParamanSave，否则重启后修改无效。
				      2.当修改PARA_PROP_MAXVALUE、PARA_PROP_MINVALUE、PARA_PROP_DFVALUE、PARA_PROP_NAME后，
				      必须调用保存接口HNC_ParamanSaveStrFile，否则重启后修改无效。
	     * @par 示例:
	     * @code
	       //设置参数属性的值
	       SDataProperty prop = new SDataProperty();
	       HNC_ParamanSetParaProp(105210, PARA_PROP_NAME, prop);
	     * @endcode     
	
	     * @see :: HNC_ParamanSaveStrFile
	     */
        public void HNC_ParamanSetParaProp(Int32 parmId, sbyte propType, SDataProperty propValue)
        {
            try
            {
	            Byte[] arrData = new Byte[SDataProperty.PARA_PROP_DATA_LEN];
	
	            arrData[0] = (Byte)propValue.type;
	            arrData[1] = 0;
	            arrData[2] = 0;
	            arrData[3] = 0;
	            switch (propValue.type)
	            {
	                case (SByte)HNCDATATYPE.DTYPE_INTEGER:
	                case (SByte)HNCDATATYPE.DTYPE_UINT:
	                case (SByte)HNCDATATYPE.DTYPE_BOOL:
	                    BitConverter.GetBytes(propValue.integer).CopyTo(arrData, SDataProperty.PARA_VALUE_START_INDEX);
	                    break;
	                case (SByte)HNCDATATYPE.DTYPE_FLOAT:
	                    BitConverter.GetBytes(propValue.real).CopyTo(arrData, SDataProperty.PARA_VALUE_START_INDEX);
	                    break;
	                case (SByte)HNCDATATYPE.DTYPE_STRING:      //字符串直接返回0
	                    Encoding.Default.GetBytes(propValue.str).CopyTo(arrData, SDataProperty.PARA_VALUE_START_INDEX);
	                    break;
	                case (SByte)HNCDATATYPE.DTYPE_BYTE:
	                    propValue.array.CopyTo(arrData, SDataProperty.PARA_VALUE_START_INDEX);
	                    break;
	                default:
	                    break;
	            }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ParamanSetParaProp(parmId, propType, arrData, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }            
        }
        /*!	@brief 设置参数属性的值（包括参数值、最大值、最小值、缺省值名称）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[in] propValue ：参数属性的值
         * @param[in] DelegateFunc ：写值回调函数指针
         * @return 
         * -  0：成功；
         * - -1：失败；

         * @attention 1.当修改PARA_PROP_VALUE参数值后，必须调用保存接口HNC_ParamanSave，否则重启后修改无效。
                      2.当修改PARA_PROP_MAXVALUE、PARA_PROP_MINVALUE、PARA_PROP_DFVALUE、PARA_PROP_NAME后，
                      必须调用保存接口HNC_ParamanSaveStrFile，否则重启后修改无效。
         * @par 示例:
         * @code
           //设置参数属性的值
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
           SDataProperty prop = new SDataProperty();
           HNC_ParamanSetParaProp(105210, PARA_PROP_NAME, prop, deleFunc);
         * @endcode     

         * @see :: HNC_ParamanSaveStrFile
         */
        public void HNC_ParamanSetParaProp(Int32 parmId, sbyte propType, SDataProperty propValue, WriteDelegate DelegateFunc)
        {
            try
            {
                Byte[] arrData = new Byte[SDataProperty.PARA_PROP_DATA_LEN];

                arrData[0] = (Byte)propValue.type;
                arrData[1] = 0;
                arrData[2] = 0;
                arrData[3] = 0;
                String str = String.Empty;
                switch (propValue.type)
                {
                    case (SByte)HNCDATATYPE.DTYPE_INTEGER:
                    case (SByte)HNCDATATYPE.DTYPE_UINT:
                    case (SByte)HNCDATATYPE.DTYPE_BOOL:
                        BitConverter.GetBytes(propValue.integer).CopyTo(arrData, SDataProperty.PARA_VALUE_START_INDEX);
                        str = propValue.integer.ToString();
                        break;
                    case (SByte)HNCDATATYPE.DTYPE_FLOAT:
                        BitConverter.GetBytes(propValue.real).CopyTo(arrData, SDataProperty.PARA_VALUE_START_INDEX);
                        str = propValue.real.ToString("f6");
                        break;
                    case (SByte)HNCDATATYPE.DTYPE_STRING:      //字符串直接返回0
                        Encoding.Default.GetBytes(propValue.str).CopyTo(arrData, SDataProperty.PARA_VALUE_START_INDEX);
                        str = propValue.str;
                        break;
                    case (SByte)HNCDATATYPE.DTYPE_BYTE:
                        propValue.array.CopyTo(arrData, SDataProperty.PARA_VALUE_START_INDEX);
                        for (Int32 i = 0; i < SDataProperty.PARA_BYTE_ARRAY_LEN; i++)
                        {
                            SByte b = (SByte)propValue.array[i];
                            if (b < 0)
                            {
                                continue;
                            }
                            str += b.ToString();
                            str += "#";
                        }
                        str = str.TrimEnd('#');
                        break;
                    default:
                        break;
                }

                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var stParm = new
                    {
                        type = propValue.type.ToString(),
                        data = str
                    };

                    var st = new
                    {
                        func = "ParamanSetParaProp",
                        id = parmId.ToString(),
                        type = propType.ToString(),
                        SDataProperty = stParm
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ParamanSetParaProp(parmId, propType, arrData, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 保存参数数据文件
	     * 
	     * @return 
         * -  0：只返回0

	     * @attention 1.调用二次开发接口修改参数值后，必须调用该接口保存数据。否则重启后修改无效。
         *            2.不支持最大值、最小值、缺省值、名字的修改保存。
         *            3.写值成功与否都返回0
	     * @par 示例:
	     * @code
	       //保存参数数据文件
	       HNC_ParamanSave();
	     * @endcode     
	
	     * @see :: HNC_ParamanSetParaProp
	     */
        public Int32 HNC_ParamanSave()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ParamanSave(m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 保存参数结构文件
	     * 
	     * @return 
         * -  0：只返回0

	     * @attention 1.当修改PARA_PROP_MAXVALUE、PARA_PROP_MINVALUE、PARA_PROP_DFVALUE、PARA_PROP_NAME后，
				      必须调用保存接口HNC_ParamanSaveStrFile，否则重启后修改无效。
         *            2.写值成功与否都返回0
	     * @par 示例:
	     * @code
	       //保存参数结构文件
	       HNC_ParamanSaveStrFile();
	     * @endcode     
	
	     * @see :: HNC_ParamanSetParaProp
	     */
        public Int32 HNC_ParamanSaveStrFile()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ParamanSaveStrFile(m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region System, Channel, Axis, Crds
        /*!	@brief 获取系统数据的值
	     * 
	     * @param[in] type ：系统数据的类型
	     * @param[out] propValue ：系统数据的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获得当前通道
	       Bit32 ch = 0;
	       Bit32 ret = HNC_SystemGetValue(HNC_SYS_ACTIVE_CHAN, ref ch，0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SystemGetValue(Int32 type, ref Int32 propValue) //Bit32
        {
            try
            {
	            StSysGetVal st = new StSysGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.SystemGetValue(type, m_machineNo);
                }
                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                propValue = BitConverter.ToInt32(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取系统数据的值
	     * 
	     * @param[in] type ：系统数据的类型
	     * @param[out] propValue ：系统数据的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获得NC版本
	       string str = "";
	       Bit32 ret = HNC_SystemGetValue(HNC_SYS_NC_VER, ref str);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SystemGetValue(Int32 type, ref String propValue) //Bit8[32], Bit8[48]
        {
            try
            {
	            StSysGetVal st = new StSysGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.SystemGetValue(type, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                propValue = System.Text.Encoding.Default.GetString(st.Data).TrimEnd('\0');
                propValue = propValue.Split('\0')[0];
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }        
        /*!	@brief 获取通道数据的值
	     * 
	     * @param[in] type ：通道数据的类型
	     * @param[in] ch ：通道号
	     * @param[in] index ：通道轴号/通道主轴号
	     * @param[out] value ：通道数据的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //机床当前类型
	       Bit32 nmacType = 0;
	       Bit32 ret = HNC_ChannelGetValue(HNC_CHAN_MAC_TYPE, 0, 0, ref nmacType，0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ChannelGetValue(Int32 type, Int32 ch, Int32 index, ref Int32 value)    //Bit32
        {
            try
            {
	            StChanGetVal st = new StChanGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ChannelGetValue(type, ch, index, m_machineNo);
                }
                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = BitConverter.ToInt32(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取通道数据的值
	     * 
	     * @param[in] type ：通道数据的类型
	     * @param[in] ch ：通道号
	     * @param[in] index ：通道轴号/通道主轴号
	     * @param[out] value ：通道数据的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //指令进给速度
	       fBit64 spd = 0;
	       Bit32 ret = HNC_ChannelGetValue(HNC_CHAN_CMD_FEEDRATE, 0, 0, ref spd，0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ChannelGetValue(Int32 type, Int32 ch, Int32 index, ref Double value)   //fBit64
        {
            try
            {
	            StChanGetVal st = new StChanGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ChannelGetValue(type, ch, index, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = BitConverter.ToDouble(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取通道数据的值
	     * 
	     * @param[in] type ：通道数据的类型
	     * @param[in] ch ：通道号
	     * @param[in] index ：通道轴号/通道主轴号
	     * @param[out] value ：通道数据的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //通道名
	       string str = "";
	       Bit32 ret = HNC_ChannelGetValue(HNC_CHAN_NAME, 0, 0, ref str，0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ChannelGetValue(Int32 type, Int32 ch, Int32 index, ref String value) //Bit8[]
        {
            try
            {
	            StChanGetVal st = new StChanGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ChannelGetValue(type, ch, index, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = System.Text.Encoding.Default.GetString(st.Data).TrimEnd('\0');
                value = value.Split('\0')[0];
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 批量取通道数据
         * @param[in] ch ：通道号
         * @param[out] value ：通道数据结构体
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //取通道0所有数据
           SChanVals value = new SChanVals();
           HNC_ChannelGetMultiValues(0, ref value);
         * @endcode    
        
         * @see ::
         */
        public Int32 HNC_ChannelGetMultiValues(Int32 ch, ref SChanVals value)
        {
            {
                try
                {
                    StChannelGetVals st = new StChannelGetVals();
                    using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                    {
                    	st = autoClient.Client.ChannelGetMultiValues(ch, m_machineNo);
                    }

                    if (st.Ret != 0)
                    {
                        return st.Ret;
                    }

                    //检查数据长度是否匹配
                    Int32 size = Marshal.SizeOf(value);
                    System.Diagnostics.Debug.Assert(size == st.Data.Length);
                    IntPtr structPtr = Marshal.AllocHGlobal(size);
                    Marshal.Copy(st.Data, 0, structPtr, size);
                    value = (SChanVals)Marshal.PtrToStructure(structPtr, typeof(SChanVals));
                    Marshal.FreeHGlobal(structPtr);

                    return st.Ret;
                }
                catch (System.Exception ex)
                {
                    Log.WriteLine(ex.ToString());
                    return -1;
                }
            }
        }
        /*!	@brief 获取轴数据的值
	     * 
	     * @param[in] type ：轴数据的类型
	     * @param[in] ax ：轴号
	     * @param[out] value ：轴数据的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 当轴没有配时，与参数文件配置保持一致。比如取轴名，若该轴未配，取出来的值是AX。
	     * @par 示例:
	     * @code
	       //轴类型
	       Bit32 type = 0;
	       Bit32 ret = HNC_AxisGetValue(HNC_AXIS_TYPE, 0, ref type);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_AxisGetValue(Int32 type, Int32 ax, ref Int32 value)    //Bit32
        {
            try
            {
	            StAxisGetVal st = new StAxisGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.AxisGetValue(type, ax, m_machineNo);   
                }
                                 

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                //Byte[] arr = System.Text.Encoding.Default.GetBytes(st.Data);
                value = BitConverter.ToInt32(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取轴数据的值
	     * 
	     * @param[in] type ：轴数据的类型
	     * @param[in] ax ：轴号
	     * @param[out] value ：轴数据的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 当轴没有配时，与参数文件配置保持一致。比如取轴名，若该轴未配，取出来的值是AX。
	     * @par 示例:
	     * @code
	       //机床实际位置
	       fBit64 pos = 0;
	       Bit32 ret = HNC_AxisGetValue(HNC_AXIS_ACT_POS, 0, ref pos);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_AxisGetValue(Int32 type, Int32 ax, ref Double value)   //fBit64
        {
            try
            {
	            StAxisGetVal st = new StAxisGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.AxisGetValue(type, ax, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                //Byte[] arr = System.Text.Encoding.Default.GetBytes(st.Data);
                value = BitConverter.ToDouble(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取轴数据的值
	     * 
	     * @param[in] type ：轴数据的类型
	     * @param[in] ax ：轴号
	     * @param[out] value ：轴数据的值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 当轴没有配时，与参数文件配置保持一致。比如取轴名，若该轴未配，取出来的值是AX。
	     * @par 示例:
	     * @code
	       //轴名
	       string name = "";
	       Bit32 ret = HNC_AxisGetValue(HNC_AXIS_NAME, 0, ref str);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_AxisGetValue(Int32 type, Int32 ax, ref String value)   //Bit8[]
        {
            try
            {
	            StAxisGetVal st = new StAxisGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.AxisGetValue(type, ax, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = System.Text.Encoding.Default.GetString(st.Data).TrimEnd('\0');
                value = value.Split('\0')[0];
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 批量取轴数据
         * @param[in] ax ：轴号
         * @param[out] value ：轴数据结构体
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //取x轴所有数据
           SAxisVals value = new SAxisVals();
           HNC_AxisGetMultiValues(0, ref value);
         * @endcode    
        
         * @see ::
         */
        public Int32 HNC_AxisGetMultiValues(Int32 ax, ref SAxisVals value)
        {
            try
            {
                StAxisGetVals st = new StAxisGetVals();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.AxisGetMultiValues(ax, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }

                //检查数据长度是否匹配
                Int32 size = Marshal.SizeOf(value);
                System.Diagnostics.Debug.Assert(size == st.Data.Length);
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy(st.Data, 0, structPtr, size);
                value = (SAxisVals)Marshal.PtrToStructure(structPtr, typeof(SAxisVals));
                Marshal.FreeHGlobal(structPtr);

                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取坐标系数据的值
	     * 
	     * @param[in] type ：坐标系数据的类型
	     * @param[in] ax ：轴号
	     * @param[out] value ：坐标系数据的值
	     * @param[in] ch ：通道号
	     * @param[in] crds ：坐标系编号
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取指定0通道x轴G54坐标系零点
	       Bit32 axisValue = 0;
	       Bit32 ret = HNC_CrdsGetValue(HNC_CRDS_CH_G5X_ZERO, 0, ref axisValue, 0, 54);
	     * @endcode     
	
	     * @see :: HNC_CrdsSetValue
	     */
        public Int32 HNC_CrdsGetValue(Int32 type, Int32 ax, ref Int32 value, Int32 ch, Int32 crds)    //Bit32
        {
            try
            {
	            StCrdsGetVal st = new StCrdsGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.CrdsGetValue(type, ax, ch, crds, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                //Byte[] arr = System.Text.Encoding.Default.GetBytes(st.Data);
                value = BitConverter.ToInt32(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取坐标系数据的值
	     * 
	     * @param[in] type ：坐标系数据的类型
	     * @param[in] ax ：轴号
	     * @param[out] value ：坐标系数据的值
	     * @param[in] ch ：通道号
	     * @param[in] crds ：坐标系编号
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取指定0通道x轴G68特性坐标系向量
	       fBit64 val = 0;
	       Bit32 ret = HNC_CrdsGetValue(HNC_CRDS_G68_VCT0, ref val, 0, 68);
	     * @endcode     
	
	     * @see :: HNC_CrdsSetValue
	     */
        public Int32 HNC_CrdsGetValue(Int32 type, Int32 ax, ref Double value, Int32 ch, Int32 crds)   //fBit64
        {
            try
            {
	            StCrdsGetVal st = new StCrdsGetVal();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.CrdsGetValue(type, ax, ch, crds, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                //Byte[] arr = System.Text.Encoding.Default.GetBytes(st.Data);
                value = BitConverter.ToDouble(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置坐标系数据的值
	     * 
	     * @param[in] type ：坐标系数据的类型
	     * @param[in] ax ：轴号
	     * @param[in] value ：坐标系数据的值
	     * @param[in] ch ：通道号
	     * @param[in] crds ：坐标系编号
         * @return 
         * -  0：只返回0
	 
	     * @attention 1.不支持HNC_CRDS_CH_WCS_ZERO类型设置
         *            2.写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //设置指定0通道x轴G54坐标系零点
	       HNC_CrdsSetValue(HNC_CRDS_CH_G5X_ZERO, 0, axisValue, 0, 54);
	     * @endcode     
	
	     * @see :: HNC_CrdsGetValue
	     */
        public Int32 HNC_CrdsSetValue(Int32 type, Int32 ax, Int32 value, Int32 ch, Int32 crds)    //Bit32
        {
            try
            {
                Byte[] val = new Byte[sizeof(Double)];  //CRDS_DATA_LEN
                Byte[] temp = System.BitConverter.GetBytes(value);
                Array.Copy(temp, val, sizeof(Int32));
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.CrdsSetValue(type, ax, val, ch, crds, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置坐标系数据的值
	     * 
	     * @param[in] type ：坐标系数据的类型
	     * @param[in] ax ：轴号
	     * @param[in] value ：坐标系数据的值
	     * @param[in] ch ：通道号
	     * @param[in] crds ：坐标系编号
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 不支持HNC_CRDS_CH_WCS_ZERO类型设置
	     * @par 示例:
	     * @code
	       //设置指定0通道x轴G54坐标系零点
	       void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_CrdsSetValue(HNC_CRDS_CH_G5X_ZERO, 0, axisValue, 0, 54, deleFunc);
	     * @endcode     
	
	     * @see :: HNC_CrdsGetValue
	     */
        public void HNC_CrdsSetValue(Int32 type, Int32 ax, Int32 value, Int32 ch, Int32 crds, WriteDelegate DelegateFunc)    //Bit32
        {
            try
            {
                Byte[] val = new Byte[sizeof(Double)];  //CRDS_DATA_LEN
                Byte[] temp = System.BitConverter.GetBytes(value);
                Array.Copy(temp, val, sizeof(Int32));

                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    
                    var st = new
                    {
                        func = "CrdsSetValue",
                        type = type.ToString(),
                        ax = ax.ToString(),
                        ch = ch.ToString(),
                        crds = crds.ToString(),
                        value = value.ToString()
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.CrdsSetValue(type, ax, val, ch, crds, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 设置坐标系数据的值
	     * 
	     * @param[in] type ：坐标系数据的类型
	     * @param[in] ax ：轴号
	     * @param[in] value ：坐标系数据的值
	     * @param[in] ch ：通道号
	     * @param[in] crds ：坐标系编号
         * @return 
         * -  0：只返回0
	 
	     * @attention 1.不支持HNC_CRDS_CH_WCS_ZERO类型设置
         *            2.写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //设置指定0通道x轴G68特性坐标系向量
	       fBit64 val = 0;
	       HNC_CrdsSetValue(HNC_CRDS_G68_VCT0, val, 0, 68);
	     * @endcode     
	
	     * @see :: HNC_CrdsGetValue
	     */
        public Int32 HNC_CrdsSetValue(Int32 type, Int32 ax, Double value, Int32 ch, Int32 crds)    //fBit64
        {
            try
            {
	            Byte[] val = System.BitConverter.GetBytes(value);
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.CrdsSetValue(type, ax, val, ch, crds, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }           
        }
        /*!	@brief 设置坐标系数据的值
	     * 
	     * @param[in] type ：坐标系数据的类型
	     * @param[in] ax ：轴号
	     * @param[in] value ：坐标系数据的值
	     * @param[in] ch ：通道号
	     * @param[in] crds ：坐标系编号
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 不支持HNC_CRDS_CH_WCS_ZERO类型设置
	     * @par 示例:
	     * @code
	       //设置指定0通道x轴G68特性坐标系向量
	       fBit64 val = 0;
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_CrdsSetValue(HNC_CRDS_G68_VCT0, val, 0, 68, deleFunc);
	     * @endcode     
	
	     * @see :: HNC_CrdsGetValue
	     */
        public void HNC_CrdsSetValue(Int32 type, Int32 ax, Double value, Int32 ch, Int32 crds, WriteDelegate DelegateFunc)    //fBit64
        {
            try
            {
                Byte[] val = System.BitConverter.GetBytes(value);

                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "CrdsSetValue",
                        type = type.ToString(),
                        ax = ax.ToString(),
                        ch = ch.ToString(),
                        crds = crds.ToString(),
                        value = value.ToString("f6")
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.CrdsSetValue(type, ax, val, ch, crds, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 取系统定义的各类坐标系数目
	     * 
	     * @param[in] type ：各类坐标系数目类型
	     * @return 
	     * 返回系统定义的各类坐标系数目。
	     * - -1：类型越界或指针为空；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //取系统定义的各类坐标系数目
	       Bit32 num = HNC_CrdsGetMaxNum(G5EXT_MAX_NUM);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_CrdsGetMaxNum(Int32 type)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.CrdsGetMaxNum(type, m_machineNo);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }            
        }

        public Int32 HNC_CrdsSave()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                    autoClient.Client.CrdsSave(m_machineNo, m_clientName);
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region Tool
        /*!	@brief 刀具文件保存
	     * 
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0
	     * @par 示例:
	     * @code
	       //刀具文件保存
	       HNC_ToolSave();
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ToolSave()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolSave(m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }            
        }
        /*!	@brief 获取系统最大刀具数目
	     * 
	     * @return 
	     * 系统的刀具最大数目
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //取系统最大刀具数目
	       Bit32 ret = HNC_ToolGetMaxToolNum();
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ToolGetMaxToolNum()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.ToolGetMaxToolNum(m_machineNo);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }           
        }
        /*!	@brief 获取刀具参数
	     * 
	     * @param[in] toolNo ：刀具号1~1024
	     * @param[in] index ：索引号
	     * @param[out] value ：参数值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention HNC_ToolGetToolPara的index对应enum ToolParaIndex，
	     其中，刀具一般信息（INFTOOL_ID到INFTOOL_STATE）的数据类型为Bit32，其它均为fBit64
	     * @par 示例:
	     * @code
	       //取1号刀所属通道
	       Bit32 temp=0;
	       Bit32 ret = HNC_ToolGetToolPara(1, INFTOOL_CH, ref temp);
	     * @endcode     
	
	     * @see :: HNC_ToolGetToolPara_Subscribe
	     */
        public Int32 HNC_ToolGetToolPara(Int32 toolNo, Int32 index, ref Int32 value)    //Bit32
        {
            try
            {
	            StToolGetToolPara st = new StToolGetToolPara();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ToolGetToolPara(toolNo, index, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                //Byte[] arr = System.Text.Encoding.Default.GetBytes(st.Data);    //将string转换为字符数组
                value = BitConverter.ToInt32(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取刀具参数
	     * 
	     * @param[in] toolNo ：刀具号1~1024
	     * @param[in] index ：索引号
	     * @param[out] value ：参数值
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention HNC_ToolGetToolPara的index对应enum ToolParaIndex，
	     其中，刀具一般信息（INFTOOL_ID到INFTOOL_STATE）的数据类型为Bit32，其它均为fBit64
	     * @par 示例:
	     * @code
	       //取1号刀S转速限制
	       fBit64 = 0;
	       Bit32 ret = HNC_ToolGetToolPara(1, EXTOOL_S_LIMIT, ref temp);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ToolGetToolPara(Int32 toolNo, Int32 index, ref Double value)   //fBit64
        {
            try
            {
	            StToolGetToolPara st = new StToolGetToolPara();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ToolGetToolPara(toolNo, index, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                //Byte[] arr = System.Text.Encoding.Default.GetBytes(st.Data);
                value = BitConverter.ToDouble(st.Data, 0);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置刀具参数
	     * 
	     * @param[in] toolNo ：刀具号1~1024
	     * @param[in] index ：索引号
	     * @param[in] value ：参数值
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //设置1号刀所属通道为3
	       HNC_ToolSetToolPara(1, INFTOOL_CH, 3);
	     * @endcode     
	
	     * @see :: HNC_ToolGetToolPara
	     */
        public Int32 HNC_ToolSetToolPara(Int32 toolNo, Int32 index, Int32 value)    //Bit32
        {
            try
            {
                Byte[] val = new byte[sizeof(Double)]; //TOOL_DATA_LEN
                Byte[] temp = System.BitConverter.GetBytes(value);
                Array.Copy(temp, val, sizeof(Int32));
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolSetToolPara(toolNo, index, val, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }          
        }
        /*!	@brief 设置刀具参数
	     * 
	     * @param[in] toolNo ：刀具号1~1024
	     * @param[in] index ：索引号
	     * @param[in] value ：参数值
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //设置1号刀S转速限制
	       HNC_ToolSetToolPara(1, EXTOOL_S_LIMIT, 0.0);
	     * @endcode     
	
	     * @see :: HNC_ToolGetToolPara
	     */
        public Int32 HNC_ToolSetToolPara(Int32 toolNo, Int32 index, Double value)   //fBit64
        {
            try
            {
	            //string val = value.ToString();
	            Byte[] val = System.BitConverter.GetBytes(value);
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolSetToolPara(toolNo, index, val, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }           
        }
        /*!	@brief 设置刀具参数
	     * 
	     * @param[in] toolNo ：刀具号1~1024
	     * @param[in] index ：索引号
	     * @param[in] value ：参数值
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //设置1号刀所属通道为3
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_ToolSetToolPara(1, INFTOOL_CH, 3, deleFunc);
	     * @endcode     
	
	     * @see :: HNC_ToolGetToolPara
	     */
        public void HNC_ToolSetToolPara(Int32 toolNo, Int32 index, Int32 value, WriteDelegate DelegateFunc)    //Bit32
        {
            try
            {
                Byte[] val = new byte[sizeof(Double)]; //TOOL_DATA_LEN
                Byte[] temp = System.BitConverter.GetBytes(value);
                Array.Copy(temp, val, sizeof(Int32));

                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "ToolSetToolPara",
                        no = toolNo.ToString(),
                        index = index.ToString(),
                        value = value.ToString()
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolSetToolPara(toolNo, index, val, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 设置刀具参数
	     * 
	     * @param[in] toolNo ：刀具号1~1024
	     * @param[in] index ：索引号
	     * @param[in] value ：参数值
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //设置1号刀S转速限制
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_ToolSetToolPara(1, EXTOOL_S_LIMIT, 0.0, deleFunc);
	     * @endcode     
	
	     * @see :: HNC_ToolGetToolPara
	     */
        public void HNC_ToolSetToolPara(Int32 toolNo, Int32 index, Double value, WriteDelegate DelegateFunc)   //fBit64
        {
            try
            {
                Byte[] val = System.BitConverter.GetBytes(value);

                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    
                    var st = new
                    {
                        func = "ToolSetToolPara",
                        no = toolNo.ToString(),
                        index = index.ToString(),
                        value = value.ToString("f6")
                    };
                    String key = js.Serialize(st);
                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolSetToolPara(toolNo, index, val, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 保存刀库表
	     * 
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0
	     * @par 示例:
	     * @code
	       //刀库文件保存
	       HNC_ToolMagSave();
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ToolMagSave()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolMagSave(m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }          
        }
        /*!	@brief 获取系统刀库表头起始地址
	     * 
	     * @return 
	     * 返回系统刀库表头的起始地址
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取系统刀库表头起始地址
	       Bit32 addr = HNC_ToolGetMagHeadBase();
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ToolGetMagHeadBase()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.ToolGetMagHeadBase(m_machineNo);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }           
        }
        /*!	@brief 获取系统最大刀库数目
	     * 
	     * @return 
	     * 返回系统定义的最大刀库数目
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //返回系统定义的最大刀库数目
	       Bit32 num = HNC_ToolGetMaxMagNum();
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_ToolGetMaxMagNum()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.ToolGetMaxMagNum(m_machineNo);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }            
        }
        /*!	@brief 获取系统刀位数据起始地址
	     * 
	     * @return 
	     * 返回系统刀位数据的起始地址
	 
	     * @attention 在调用本函数之前必须先调用HNC_ToolGetPotDataBase_Subscribe
	     * @par 示例:
	     * @code
	       //获取系统刀位数据起始地址
	       Bit32 addr = HNC_ToolGetPotDataBase();
	     * @endcode     
	
	     * @see :: HNC_ToolGetPotDataBase_Subscribe
	     */
        public Int32 HNC_ToolGetPotDataBase()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.ToolGetPotDataBase(m_machineNo);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }          
        }
        /*!	@brief 订阅系统刀位数据起始地址
	     * 
	     * @param[in] cancel ：是否订阅，false表示订阅，true表示取消订阅
	 
	     * @attention 在获取系统刀位数据起始地址之前必须先调用本函数
	     * @par 示例:
	     * @code
	       //订阅系统刀位数据起始地址
	       HNC_ToolGetPotDataBase_Subscribe(0, false);
	     * @endcode     
	
	     * @see :: HNC_ToolGetPotDataBase
	     */
        public void HNC_ToolGetPotDataBase_Subscribe(Boolean cancel)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolGetPotDataBase_Subscribe(m_machineNo, cancel);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }            
        }
        /*!	@brief 获取刀库数据
	     * 
	     * @param[in] magNo ：刀库号1~32
	     * @param[in] index ：索引号
	     * @param[out] value ：刀库数据
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取获取刀库表中刀具数
	       Bit32 num=0;
	       Bit32 ret = HNC_ToolGetMagBase(1, MAGZTAB_TOOL_NUM, ref num,0);
	     * @endcode     
	
	     * @see :: HNC_ToolSetMagBase
	     */
        public Int32 HNC_ToolGetMagBase(Int32 magNo, Int32 index, ref Int32 value)
        {
            try
            {
	            StToolGetMagBase st = new StToolGetMagBase();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ToolGetMagBase(magNo, index, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = st.Data;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置刀库数据
	     * 
	     * @param[in] magNo ：刀库号1~32
	     * @param[in] index ：索引号
	     * @param[in] value ：刀库数据
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //设置刀库表中刀具数
	       HNC_ToolSetMagBase(1, MAGZTAB_TOOL_NUM, num);
	     * @endcode     
	
	     * @see :: HNC_ToolGetMagBase
	     */
        public Int32 HNC_ToolSetMagBase(Int32 magNo, Int32 index, Int32 value)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolSetMagBase(magNo, index, value, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }            
        }
        /*!	@brief 设置刀库数据
	     * 
	     * @param[in] magNo ：刀库号1~32
	     * @param[in] index ：索引号
	     * @param[in] value ：刀库数据
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //设置刀库表中刀具数
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_ToolSetMagBase(1, MAGZTAB_TOOL_NUM, num, deleFunc);
	     * @endcode     
	
	     * @see :: HNC_ToolGetMagBase
	     */
        public void HNC_ToolSetMagBase(Int32 magNo, Int32 index, Int32 value, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "ToolSetMagBase",
                        no = magNo.ToString(),
                        index = index.ToString(),
                        data = value.ToString()
                    };
                    String key = js.Serialize(st);

                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolSetMagBase(magNo, index, value, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 获取刀库中刀位的刀具号
	     * 
	     * @param[out] magNo ：刀库号1~32
	     * @param[out] potNo ：刀位号
	     * @param[out] value ：刀具号
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 在调用本函数之前必须先调用HNC_ToolMagGetToolNo_Subscribe
	     * @par 示例:
	     * @code
	       //设置刀库中刀位的刀具号
	       Bit32 magToolNo =0;
	       Bit32 ret = HNC_ToolMagGetToolNo(1, 1, ref magToolNo);
	     * @endcode     
	
	     * @see :: HNC_ToolMagGetToolNo_Subscribe
	     */
        public Int32 HNC_ToolMagGetToolNo(Int32 magNo, Int32 potNo, ref Int32 value)
        {
            try
            {
	            StToolMagGetToolNo st = new StToolMagGetToolNo();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ToolMagGetToolNo(magNo, potNo, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = st.Data;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 订阅刀库中刀位的刀具号
	     * 
	     * @param[in] magNo ：刀库号1~32
	     * @param[in] potNo ：刀位号
	     * @param[in] cancel ：是否订阅，false表示订阅，true表示取消订阅
	 
	     * @attention 在获取刀库中刀位的刀具号之前必须先调用本函数
	     * @par 示例:
	     * @code
	       //订阅刀库中刀位的刀具号
	       HNC_ToolMagGetToolNo_Subscribe(1, 1, 0, false);
	     * @endcode     
	
	     * @see :: HNC_ToolMagGetToolNo
	     */
        public void HNC_ToolMagGetToolNo_Subscribe(Int32 magNo, Int32 potNo, Boolean cancel)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolMagGetToolNo_Subscribe(magNo, potNo, m_machineNo, cancel);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }            
        }
        /*!	@brief 设置刀库中刀位的刀具号
	     * 
	     * @param[in] magNo ：刀库号1~32
	     * @param[in] potNo ：刀位号
	     * @param[in] value ：刀具号
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0
	     * @par 示例:
	     * @code
	       //设置刀库中刀具的刀位号
	       Bit32 tempI = 1;
	       HNC_ToolMagSetToolNo(1, 1, tempI);
	     * @endcode     
	
	     * @see :: HNC_ToolMagGetToolNo
	     */
        public Int32 HNC_ToolMagSetToolNo(Int32 magNo, Int32 potNo, Int32 value)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolMagSetToolNo(magNo, potNo, value, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }          
        }
        /*!	@brief 获取刀库中刀位的刀位属性
	     * 
	     * @param[in] magNo ：刀库号 1~32
	     * @param[in] potNo ：刀位号
	     * @param[out] value ：刀位属性
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 在获取刀库中刀位的刀位属性之前必须先调用HNC_ToolGetPotAttri_Subscribe
	     * @par 示例:
	     * @code
	       //获取刀库中刀位属性
	       Bit32tempI = 1;
	       Bit32 ret = HNC_ToolGetPotAttri(1, 1, ref tempI);
	     * @endcode     
	
	     * @see :: HNC_ToolGetPotAttri_Subscribe
	     */
        public Int32 HNC_ToolGetPotAttri(Int32 magNo, Int32 potNo, ref Int32 value)
        {
            try
            {
	            StToolGetPotAttri st = new StToolGetPotAttri();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.ToolGetPotAttri(magNo, potNo, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                value = st.Data;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 订阅刀库中刀位的刀位属性
	     * 
	     * @param[in] magNo ：刀库号 1~32
	     * @param[in] potNo ：刀位号
	     * @param[in] cancel ：是否订阅，false表示订阅，true表示取消订阅
	 
	     * @attention 在获取刀库中刀位的刀位属性之前必须先调用本函数
	     * @par 示例:
	     * @code
	       //订阅刀库中刀位属性
	       HNC_ToolGetPotAttri_Subscribe(1, 1, 0, false);
	     * @endcode     
	
	     * @see :: HNC_ToolGetPotAttri
	     */
        public void HNC_ToolGetPotAttri_Subscribe(Int32 magNo, Int32 potNo, Boolean cancel)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolGetPotAttri_Subscribe(magNo, potNo, m_machineNo, cancel);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }            
        }
        /*!	@brief 设置刀库中刀位的刀位属性
	     * 
	     * @param[in] magNo ：刀库号 1~32
	     * @param[in] potNo ：刀位号
	     * @param[in] value ：刀位属性
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0
	     * @par 示例:
	     * @code
	       //设置刀库表中刀位属性
	       Bit32 tempI = 1;
	       HNC_ToolSetPotAttri(1, 1, tempI);
	     * @endcode     
	
	     * @see :: HNC_ToolGetPotAttri
	     */
        public Int32 HNC_ToolSetPotAttri(Int32 magNo, Int32 potNo, Int32 value)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.ToolSetPotAttri(magNo, potNo, value, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }           
        }
        /*!	@brief 获取刀具数目
         * @param[in] toolStartNo ：刀具起始号
         * @param[out] toolNum ：刀具数目
         
         * @attention 
         * @par 示例:
         * @code
           //获取刀具数目
           int toolNum = 0;
           HNCAPI_INTERFACE.HNC_ToolGetSysToolNum(0, 10, ref toolNum);
         * @endcode    
        
         * @see ::
         */
        public void HNC_ToolGetSysToolNum(ref Int32 toolStartNo, ref Int32 toolNum)
        {
            try
            {
                StSysToolNum st = new StSysToolNum();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))         
                {
                	st = autoClient.Client.SysGetToolNum(m_machineNo);
                }
                if (st.Ret != 0)
                {
                    return;
                }
                toolStartNo = st.ToolStartNo;
                toolNum = st.ToolNum;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Alarm
        /*!	@brief 订阅报警信息
	     * 
	     * @param[in] cancel ：是否订阅，false表示订阅，true表示取消订阅
	 
	     * @attention 在获取报警信息之前必须先调用本函数
	     * @par 示例:
	     * @code
	       //订阅报警信息
	       HNC_AlarmSubscribe(0, false);
	     * @endcode     
	
	     * @see :: 
	     */
        public void HNC_AlarmSubscribe(Boolean cancel)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.AlarmSubscribe(m_machineNo, cancel);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 获取报警的数目
	     * 
	     * @param[out] num ：报警数目
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 调用本函数之前必须先调用HNC_AlarmSubscribe
	     * @par 示例:
	     * @code
	       //获取报警的数目
	       Bit32 errNum = 0;
	       Bit32 ret = HNC_AlarmGetNum(ref errNum);
	     * @endcode     
	
	     * @see :: HNC_AlarmSubscribe
	     */
        public Int32 HNC_AlarmGetNum(ref Int32 num)
        {
            try
            {
	            StAlarmGetNum st = new StAlarmGetNum();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.AlarmGetNum(m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                num = st.Num;
				return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取当前报警的报警号和报警文本
	     * 
	     * @param[in] index ：报警索引号
	     * @param[out] alarmNo ：输出报警号
	     * @param[out] alarmText ：输出报警文本，最少字符串长度为64
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 调用本函数之前必须先调用HNC_AlarmSubscribe
	     * @par 示例:
	     * @code
	       //获取当前报警的报警号和报警文本
	       Bit32 index = 0;
	       Bit32 alarmNo = 0;
	       string alarmText = "";
	       Bit32 ret = HNC_AlarmGetData(0, ref alarmNo, ref alarmText);
	     * @endcode     
	
	     * @see :: HNC_AlarmSubscribe
	     */
        public Int32 HNC_AlarmGetData(Int32 index, ref Int32 alarmNo, ref String alarmText)
        {
            try
            {
	            StAlarmGetData st = new StAlarmGetData();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.AlarmGetData(index, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                alarmNo = st.AlarmNo;
	            alarmText = Encoding.Default.GetString(st.AlarmText, 0, st.AlarmText.Length).TrimEnd('\0');
                alarmText = alarmText.Split('\0')[0];
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region Event
        /*!	@brief 订阅消息队列中的事件
	     * 
	     * @param[in] cancel ：是否订阅，false表示订阅，true表示取消订阅
	 
	     * @attention 从消息队列中获取事件之前必须先调用本函数
	     * @par 示例:
	     * @code
	       //订阅事件
	       HNC_EventSubscribe(0, false);
	     * @endcode     
	
	     * @see :: HNC_EventGetSysEv
	     */
        public void HNC_EventSubscribe(Boolean cancel)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.EventSubscribe(m_machineNo, m_clientName, cancel);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 从消息队列中获取事件
	     * 
	     * @param[out] ev ：事件
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 调用本函数之前必须先调用HNC_EventSubscribe
	     * @par 示例:
	     * @code
	       //从消息队列中获取
	       SEventElement ev1 = {0, kbNoKey};
	       Bit32 ret = HNC_EventGetSysEv(ref ev1);
	     * @endcode     
	
	     * @see :: HNC_EventSubscribe
	     */
        public Int32 HNC_EventGetSysEv(ref SEventElement ev)
        {
            try
            {
	            StEventData st = new StEventData();

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.EventGetSysEv(m_machineNo, m_clientName);
                }
	
	            if (st.Ret != 0)
	            {
	                return st.Ret;
	            }
	
	            //检查数据长度是否匹配
	            Int32 size = Marshal.SizeOf(ev);
	            System.Diagnostics.Debug.Assert(size == st.EventData.Length);
	
	            //将字节数组转为SDataUnion结构体
	            IntPtr structPtr = Marshal.AllocHGlobal(size);
	            Marshal.Copy(st.EventData, 0, structPtr, size);
	            ev = (SEventElement)Marshal.PtrToStructure(structPtr, typeof(SEventElement));
	            Marshal.FreeHGlobal(structPtr);
	
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region Sampl
        /*!	@brief 获取采样通道
	     * 
	     * @param[in] ch ：采样通道（0~31）
	     * @param[out] type ：采样对象的数据类型
	     * @param[out] axis ：逻辑轴号
	     * @param[out] offset ：偏移量
	     * @param[out] dataLen ：单个采样数据长度
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取采样通道
	       Bit32 type = 0;
	       Bit32 axisNo = 0;
	       Bit32 offset = 0;
	       Bit32 dataLen = 0;
	       Bit32 ret = HNC_SamplGetChannel(0, ref type, ref axisNo, ref offset, ref dataLen);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SamplGetChannel(Int32 ch, ref Int32 type, ref Int32 axis, ref Int32 offset, ref Int32 dataLen)
        {
            try
            {
	            StSamplGetChan st = new StSamplGetChan();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.SamplGetChannel(ch, m_machineNo, m_clientName);
                }
	            
	            if (st.Ret != 0)
	            {
	                return st.Ret;
	            }
	
	            type = st.Type;
	            axis = st.AxisNo;
	            offset = st.Offset;
	            dataLen = st.DataLen;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置采样通道
         * 
         * @param[in] ch ：采样通道（0~31）
         * @param[in] type ：采样对象的数据类型
         * @param[in] axis ：逻辑轴号
         * @param[in] offset ：偏移量
         * @param[in] dataLen ：单个采样数据长度

         * @attention 
         * @par 示例:
         * @code
           //设置采样通道
           ...
           HNC_SamplSetChannel(0, type, axis, offset, dataLen);
         * @endcode     

         * @see :: 
         */
        public void HNC_SamplSetChannel(Int32 ch, Int32 type, Int32 axis, Int32 offset, Int32 dataLen)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.SamplSetChannel(ch, type, axis, offset, dataLen, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 移除采样客户端的某个采样配置
         * 
         * @param[in] type ：采样类型
         * @param[in] axis ：轴号
         * @param[in] offset ：偏移量
         * @param[in] dataLen ：数据长度
         * @return 
         * -  0：成功；
         * - -1：失败；

         * @attention 
         * @par 示例:
         * @code
           //移除采样客户端的某个采样配置
           ...
           Bit32 ret = HNC_SamplRemoveConfig(type, axis, offset, dataLen);
         * @endcode     

         * @see :: 
         */
        public void HNC_SamplRemoveConfig(Int32 type, Int32 axis, Int32 offset, Int32 dataLen)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.SamplRemoveConfig(type, axis, offset, dataLen, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 获取采样数据
         * 
         * @param[out] data ：采样数据
         * @return 
         * -  0：成功；
         * - -1：失败；

         * @attention data中的数据按通道排列，排列顺序为：0通道数据、1通道数据、3通道数据（如果2通道未设置，1通道后面会接3通道数据）
         * @par 示例:
         * @code
           //获取采样数据
           list<list<Bit32>> data
           Bit32 ret = HNC_SamplGetData(data);
         * @endcode     

         * @see :: 
         */
        public Int32 HNC_SamplGetData(List<List<Int32>> data)
        {
            try
            {
	            System.Diagnostics.Debug.Assert(data != null);
	
	            StSamplData st = new StSamplData();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.SamplGetData(m_machineNo, m_clientName);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }

                Int32 dataSize = st.ChNum * st.NumPerCh * sizeof(Int32);
                System.Diagnostics.Debug.Assert(st.Data.Length == dataSize);

                Int32 d = 0;
                for (Int32 i = 0; i < st.ChNum; i++)
                {
                    List<Int32> tmpData = new List<Int32>();
                    for (Int32 j = 0; j < st.NumPerCh; j++)
                    {
                        d = BitConverter.ToInt32(st.Data, (i * st.NumPerCh + j) * sizeof(Int32));
                        tmpData.Add(d);
                    }
                    data.Add(tmpData);
                    //if (i == 16)
                    //{
                    //    Check(tmpData);
                    //}
                }
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }

        private void Check(List<Int32> data)
        {
            for (Int32 i = 0; i < data.Count - 1; i++)
            {
                Int32 tmp = data[i + 1] - data[i];
                if (tmp != 1 && tmp != -255)
                {
                    String msg = "check failed! " + data[i + 1].ToString() + "," + data[i].ToString();
                    Log.WriteLine(msg);
                }
            }
        }
        /*!	@brief 获取采样周期
         * 
         * @param[out] tick ：采样周期（插补周期的整数倍）
         * @return 
         * -  0：成功；
         * - -1：失败；

         * @attention 
         * @par 示例:
         * @code
           //采样周期（插补周期的整数倍）
           Bit32 tick = 0;
           Bit32 ret = HNC_SamplGetPeriod(ref tick);
         * @endcode     

         * @see :: 
         */
        public Int32 HNC_SamplGetPeriod(ref Int32 tick)
        {
            try
            {
            	return HNC_VarGetValue((Int32)HncVarType.VAR_TYPE_SYSTEM, 0, (Int32)SysVarDef.VAR_SMPL_PERIOD, ref tick);
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置采样周期
         * 
         * @param[in] tick ：采样周期（插补周期的整数倍）

         * @attention 
         * @par 示例:
         * @code
           //设置采样周期
           Bit32 tick = 100;
           HNC_SamplSetPeriod(tick);
         * @endcode     

         * @see :: 
         */
        public void HNC_SamplSetPeriod(Int32 tick)
        {
            try
            {
            	HNC_VarSetValue((Int32)HncVarType.VAR_TYPE_SYSTEM, 0, (Int32)SysVarDef.VAR_SMPL_PERIOD, tick);
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 开启采样功能
         * 

         * @attention 开始采样前，必须设置采样通道数目
         * @par 示例:
         * @code
           //说明
           HNC_SamplTriggerOn();
         * @endcode     

         * @see :: 
         */
        public void HNC_SamplTriggerOn()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.SamplSetStat(true, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 关闭采样功能
         * 

         * @attention 
         * @par 示例:
         * @code
           //说明
           HNC_SamplTriggerOff();
         * @endcode     

         * @see :: 
         */
        public void HNC_SamplTriggerOff()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.SamplSetStat(false, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 获取当前采样的状态
         * 
         * @return 
         * - 1：正在采样；
         * - 0：没有采样；

         * @attention 
         * @par 示例:
         * @code
           //获取当前采样的状态
           Bit32 ret = HNC_SamplGetStat();
         * @endcode     

         * @see :: 
         */
        public Int32 HNC_SamplGetStat()
        {
            try
            {
	            Int32 value = 0;
	            Int32 ret = HNC_RegGetValue((Int32)HncRegType.REG_TYPE_G, REG_SYS_G_BASE, ref value);
	            if (ret != 0)
	            {
	                return -1;
	            }
	
	            if (((value >> SMPL_CRTL_BIT) & 0x01) == 1)
	            {
	                return 1;
	            }
	            return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置采样截止的方式和采样个数
         * 
         * @param[in] type ：采样停止的方式
         * @param[in] n ：采样个数[1-10000] 。n仅在type=1时生效。当输入n超过10000按10000处理。

         * @attention 
         * @par 示例:
         * @code
           //设置采样截止的方式和采样个数
           HNC_SamplSetLmt(1, 500);
         * @endcode     

         * @see :: 
         */
        public void HNC_SamplSetLmt(Int32 type, Int32 n)
        {
            try
            {
                const Int32 SMPL_DATA_NUM = 10000;
                if ( n > SMPL_DATA_NUM)
                {
                    n = SMPL_DATA_NUM;
                }

                if (type > 0)
                {
                    HNC_VarSetValue((Int32)HncVarType.VAR_TYPE_SYSTEM, 0, (Int32)SysVarDef.VAR_SMPL_LMT, n);
                }
                else
                {
                    HNC_VarSetValue((Int32)HncVarType.VAR_TYPE_SYSTEM, 0, (Int32)SysVarDef.VAR_SMPL_LMT, type);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Mdi
        /*!	@brief MDI开启
	     * 
	     * @param[in] ch ：通道号
	     * @return 
	     * - 1：成功；
	     * - 0：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //开启0通道的MDI
	       Bit32 ret = HNC_MdiOpen(0);
	     * @endcode     
	
	     * @see :: HNC_MdiClose
	     */
        public Int32 HNC_MdiOpen(Int32 ch)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.MdiOpen(ch, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief MDI开启
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] DelegateFunc ：写值回调函数指针
	     * @return 
	     * - 1：成功；
	     * - 0：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //开启0通道的MDI
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       Bit32 ret = HNC_MdiOpen(0, deleFunc);
	     * @endcode     
	
	     * @see :: HNC_MdiClose
	     */
        public Int32 HNC_MdiOpen(Int32 ch, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "MdiOpen"
                    };
                    String key = js.Serialize(st);

                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.MdiOpen(ch, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置MDI的文本
	     * 
	     * @param[in] content ：设置的文本
	     * @return 
	     * - 1：成功；
	     * - 0：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //设置MDI的文本
	       Bit32 ret = HNC_MdiSetContent("G00X100Y200Z300");
	     * @endcode     
	
	     * @see :: HNC_MdiOpen
	     */
        public Int32 HNC_MdiSetContent(String content)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.MdiSetContent(content, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置MDI的文本
	     * 
	     * @param[in] content ：设置的文本
	     * @param[in] DelegateFunc ：写值回调函数指针
	     * @return 
	     * - 1：成功；
	     * - 0：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //设置MDI的文本
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       Bit32 ret = HNC_MdiSetContent("G00X100Y200Z300", deleFunc);
	     * @endcode     
	
	     * @see :: HNC_MdiOpen
	     */
        public Int32 HNC_MdiSetContent(String content, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "MdiSetContent",
                        content = content
                    };
                    String key = js.Serialize(st);

                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.MdiSetContent(content, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief MDI关闭
	     * 
	     * @return 
	     * - 1：成功；
	     * - 0：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //关闭0通道的MDI
	       Bit32 ret = HNC_MdiClose();
	     * @endcode     
	
	     * @see :: HNC_MdiOpen
	     */
        public Int32 HNC_MdiClose()
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.MdiClose(m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief MDI关闭
	     * 
	     * @param[in] DelegateFunc ：写值回调函数指针
	     * @return 
	     * - 1：成功；
	     * - 0：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //关闭0通道的MDI
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       Bit32 ret = HNC_MdiClose(0, deleFunc);
	     * @endcode     
	
	     * @see :: HNC_MdiOpen
	     */
        public Int32 HNC_MdiClose(WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "MdiClose"
                    };
                    String key = js.Serialize(st);

                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.MdiClose(m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region PLC
        /*!	@brief 强制允许元件导通
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //强制允许元件导通
	       Bit32 ret = HNC_LadEnableBlk(0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadEnableBlk(Int32 index)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.LadSetStatus((Int32)LadCmd.LAD_ENABLE_BLK, index, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }          
        }
        /*!	@brief 强制禁止元件导通
         * 
         * @param[in] index ：在命令表中的索引位置
         * @return 
         * -  0：成功；
         * - -1：失败；

         * @attention 
         * @par 示例:
         * @code
           //强制禁止元件导通
           Bit32 ret = HNC_LadDisableBlk(0);
         * @endcode     

         * @see :: 
         */
        public Int32 HNC_LadDisableBlk(Int32 index)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.LadSetStatus((Int32)LadCmd.LAD_DISABLE_BLK, index, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 恢复元件强制状态
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //恢复元件强制状态
	       Bit32 ret = HNC_LadRestoreBlk(0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadRestoreBlk(Int32 index)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.LadSetStatus((Int32)LadCmd.LAD_RESTORE_BLK, index, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 增加一组锁定寄存器
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //增加一组锁定寄存器
	       Bit32 ret = HNC_LadAddRegLockList(0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadAddRegLockList(Int32 index)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.LadSetStatus((Int32)LadCmd.LAD_ADD_REGLOCK, index, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 删除一组锁定寄存器
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //删除一组锁定寄存器
	       Bit32 ret = HNC_LadDelRegLockList(0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadDelRegLockList(Int32 index)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.LadSetStatus((Int32)LadCmd.LAD_DEL_REGLOCK, index, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 重新加载
	     * 
	     * @param[in] flag ：flag标记
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //重新加载
	       Bit32 ret = HNC_LadReload('\000');
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadReload(Byte flag)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.LadSetStatus((Int32)LadCmd.LAD_RELOAD_DIT, (Int32)flag, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取元件强制状态
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @param[out] status ：元件强制状态
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取元件强制状态
	       Bit32 status = 0;
	       Bit32 ret = HNC_LadGetBlkForceState(0, ref status);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadGetBlkForceState(Int32 index, ref Int32 status)
        {
            try
            {
	            StLadGetStatus st = new StLadGetStatus();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.LadGetStatus((Int32)LadCmd.LAD_GET_BLK_FORCE_STATE, index, 0, m_machineNo);
                }

	            if (st.Ret != 0)
	            {
	                return st.Ret;
	            }
	            status = st.Status;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取寄存器锁存数据
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @param[in] num ：行号
	     * @param[out] status ：寄存器锁存数据
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 在调用本函数之前必须先调用HNC_LadGetRegCacheStatus函数订阅
	     * @par 示例:
	     * @code
	       //获取寄存器锁存数据
	       Bit32 status = 0;
	       Bit32 ret = HNC_LadGetRegCacheStatus(0, 0, ref status);
	     * @endcode     
	
	     * @see :: HNC_LadGetStatus_Subscribe
	     */
        public Int32 HNC_LadGetRegCacheStatus(Int32 index, Int32 num, ref Int32 status)
        {
            try
            {
	            StLadGetStatus st = new StLadGetStatus();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.LadGetStatus((Int32)LadCmd.LAD_GET_REG_CACHE_STATUS, index, num, m_machineNo);
                }

	            if (st.Ret != 0)
	            {
	                return st.Ret;
	            }
	            status = st.Status;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取DIT文件校验码
	     * 
	     * @param[out] verify ：DIT校验码
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取DIT文件校验码
	       Bit32 verity = 0;
	       Bit32 ret = HNC_LadGetFileVerify(ref verity);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadGetFileVerify(ref Int32 verify)
        {
            try
            {
	            StLadGetStatus st = new StLadGetStatus();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.LadGetStatus((Int32)LadCmd.LAD_GET_DIT_VERIFY, 0, 0, m_machineNo);
                }

	            if (st.Ret != 0)
	            {
	                return st.Ret;
	            }
	            verify = st.Status;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取锁定寄存器数目
	     * 
	     * @param[out] num ：锁定寄存器数目
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取锁定寄存器数目
	       Bit32 num = 0;
	       Bit32 ret = HNC_LadGetRegLockNum(ref num);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadGetRegLockNum(ref Int32 num)
        {
            try
            {
	            StLadGetStatus st = new StLadGetStatus();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.LadGetStatus((Int32)LadCmd.LAD_GET_REGLOCK_NUM, 0, 0, m_machineNo);
                }

	            if (st.Ret != 0)
	            {
	                return st.Ret;
	            }
	            num = st.Status;
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 获取寄存器锁存数据
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @param[out] value ：寄存器锁存数据
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 在调用本函数之前必须先调用HNC_LadGetRegLockStatus_Subscribe函数订阅
	     * @par 示例:
	     * @code
	       //获取寄存器锁存数据
	       SLadRegLick value;
	       Bit32 ret = HNC_LadGetRegLockStatus(0, (Bit8*)&SLadRegLock);
	     * @endcode     
	
	     * @see :: HNC_LadGetRegLockStatus_Subscribe
	     */
        public Int32 HNC_LadGetRegLockStatus(Int32 index, IntPtr value)
        {
            try
            {
	            StLadGetRegLock st = new StLadGetRegLock();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.LadGetRegLock(index, m_machineNo);
                }
         
	            if (st.Ret != 0)
	            {
	                return st.Ret;
	            }
	
	            //检查数据长度是否匹配
                Int32 size = Marshal.SizeOf(typeof(SLadRegLock));
	            System.Diagnostics.Debug.Assert(size == st.Data.Length);

                Marshal.Copy(st.Data, 0, value, size);
	            return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置寄存器锁定状态
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @param[in] value ：寄存器锁定状态
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //设置寄存器锁定状态
	       SLadRegLick value;
	       ...
	       Bit32 ret = HNC_LadSetRegLockStatus(0, (Bit8*)&SLadRegLock);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_LadSetRegLockStatus(Int32 index, IntPtr value)
        {
            try
            {
                Int32 size = Marshal.SizeOf(typeof(SLadRegLock));
	
	            Byte[] arrData = new Byte[size];
                Marshal.Copy(value, arrData, 0, size);
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.LadSetRegLock(index, arrData, m_machineNo, m_clientName);
                }

            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 订阅寄存器锁存数据
	     * 
	     * @param[in] cmd ：在命令表中的索引位置
	     * @param[in] index ：行号
	     * @param[in] num ：数目
	     * @param[in] cancel ：是否订阅，false表示订阅，true表示取消订阅
	 
	     * @attention 在获取寄存器锁存数据之前必须先调用本函数
	     * @par 示例:
	     * @code
	       //订阅寄存器锁存数据
	       HNC_LadGetStatus_Subscribe(0, 0, 5, 0, false);
	     * @endcode     
	
	     * @see :: HNC_LadGetRegCacheStatus
	     */
        public void HNC_LadGetStatus_Subscribe(Int32 cmd, Int32 index, Int32 num, Boolean cancel)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.LadGetStatus_Subscribe(cmd, index, num, m_machineNo, cancel);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 订阅寄存器锁定状态
	     * 
	     * @param[in] index ：在命令表中的索引位置
	     * @param[in] cancel ：是否订阅，false表示订阅，true表示取消订阅
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 在获取寄存器锁定状态之前必须先调用本函数订阅
	     * @par 示例:
	     * @code
	       //订阅寄存器锁定状态
	       Bit32 ret = HNC_LadGetRegLockStatus_Subscribe(0, 0, false);
	     * @endcode     
	
	     * @see :: HNC_LadGetRegLockStatus
	     */
        public Int32 HNC_LadGetRegLockStatus_Subscribe(Int32 index, bool cancel)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	return autoClient.Client.LadGetRegLock_Subscribe(index, m_machineNo, cancel);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region Other
        /*!	@brief 系统备份
	     * 
	     * @param[in] flag ：备份内容
	     * @param[in] pathName ：备份文件名，最大长度为128
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //备份当前下位机G代码到文件backup
	       HNC_SysBackup(0x0080, "../backup.tar");
	     * @endcode     
	
	     * @see :: HNC_SysUpdate
	     */
        public Int32 HNC_SysBackup(Int32 flag, String pathName)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.SysBackup(flag, pathName, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 系统备份
	     * 
	     * @param[in] flag ：备份内容
	     * @param[in] pathName ：备份文件名，最大长度为128
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //备份当前下位机G代码到文件backup
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_SysBackup(0x0080, "../backup.tar", deleFunc);
	     * @endcode     
	
	     * @see :: HNC_SysUpdate
	     */
        public void HNC_SysBackup(Int32 flag, String pathName, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "SysBackup",
                        flag = flag.ToString(),
                        path = pathName
                    };
                    String key = js.Serialize(st);

                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.SysBackup(flag, pathName, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 系统升级
	     * 
	     * @param[in] flag ：升级内容
	     * @param[in] pathName ：升级文件名，最大长度为128
         * @return 
         * -  0：只返回0
	 
	     * @attention 写值成功与否都返回0，如要判断请调用带回调函数参数的重载接口
	     * @par 示例:
	     * @code
	       //升级update.tar文件到当前下位机G代码
	       HNC_SysUpdate(0x0080, "../update.tar");
	     * @endcode     
	
	     * @see :: HNC_SysBackup
	     */
        public Int32 HNC_SysUpdate(Int32 flag, String pathName)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.SysUpdate(flag, pathName, m_machineNo, m_clientName);
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 系统升级
	     * 
	     * @param[in] flag ：升级内容
	     * @param[in] pathName ：升级文件名，最大长度为128
	     * @param[in] DelegateFunc ：写值回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //升级update.tar文件到当前下位机G代码
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_SysUpdate(0x0080, "../update.tar", deleFunc);
	     * @endcode     
	
	     * @see :: HNC_SysBackup
	     */
        public void HNC_SysUpdate(Int32 flag, String pathName, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "SysUpdate",
                        flag = flag.ToString(),
                        path = pathName
                    };
                    String key = js.Serialize(st);

                    AddToTable(key, DelegateFunc);
                }

                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.SysUpdate(flag, pathName, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 向下位机推送消息
	    *
	    * @param[in] msg ：推送的消息

	    * @attention
	    * @par 示例:
	    * @code
	    //向下位机推送消息
	    HNC_PutMessage("putmsg");
	    * @endcode

	    * @see ::
	    */
        public void HNC_PutMessage(String msg)
        {
            if (msg.Length > EVENTDEF.MAX_PUT_MSG_LEN)
            {
                return;
            }

            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.PutMessage(Encoding.Default.GetBytes(msg), m_machineNo);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 复位
         
         * @attention 
         * @par 示例:
         * @code
           //说明
           HNC_SysReset();
         * @endcode    
        
         * @see ::
         */
        public void HNC_SysReset()
        {            
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.PutResetMessage(m_machineNo);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }
        /*!	@brief 执行任意行功能
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] line ：行号
	     * @param[in] flag ：flag标记
	     * @param[in] mst ：
	     * @param[in] bNthLine ：true表示G代码N号，false表示G代码行号
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //将0通道G代码以行号跳转到300行
	       HNC_RandomRow(0, 300, 0, mstContent, false);
	     * @endcode     
	
	     * @see :: 
	     */
        public void HNC_RandomRow(Int32 ch, Int32 line, Int32 flag, String mst, Boolean bNthLine)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.RandomRow(ch, line, flag, mst, bNthLine, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return;
            }
        }
        /*!	@brief 执行任意行功能
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] line ：行号
	     * @param[in] flag ：flag标记
	     * @param[in] mst ：
	     * @param[in] bNthLine ：true表示G代码N号，false表示G代码行号
	     * @param[in] DelegateFunc ：回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //将0通道G代码以行号跳转到300行
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_RandomRow(0, 300, 0, mstContent, false, deleFunc);
	     * @endcode     
	
	     * @see :: 
	     */
        public void HNC_RandomRow(Int32 ch, Int32 line, Int32 flag, String mst, Boolean bNthLine, WriteDelegate DelegateFunc)
        {
            try
            {
                if (DelegateFunc != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    var st = new
                    {
                        func = "RandomRow",
                        ch = ch.ToString(),
                        line = line.ToString(),
                        flag = flag.ToString(),
                        bNthLine = System.Convert.ToInt32(bNthLine).ToString()
                    };
                    String key = js.Serialize(st);

                    AddToTable(key, DelegateFunc);
                }
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.RandomRow(ch, line, flag, mst, bNthLine, m_machineNo, m_clientName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return;
            }
        }
        /*!	@brief 获取程序的完整名（含路径）
	     * 
	     * @param[in] ch ：通道号0~4
	     * @param[out] progName ：文件名，程序路径名最大长度60
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //取通道加载的程序的名字（含路径）
	       Bit8 filename[PATH_NAME_LEN] ={0};
	       Bit32 ret = HNC_FprogGetFullName(0, ref filename);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_FprogGetFullName(Int32 ch, ref String progName)
        {
            try
            {
                StProgGetName st = new StProgGetName();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.FprogGetFullName(ch, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                progName = Encoding.Default.GetString(st.Name).TrimEnd('\0');
                progName = progName.Split('\0')[0];
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 挂载网盘
	     * 
	     * @param[in] ip ：要挂载网盘的IP地址,16位字符串
	     * @param[in] progPath ：要挂载网盘的文件夹名称，32位字符串
	     * @param[in] userName ：网盘用户名，32位字符串
	     * @param[in] password ：网盘用户密码，9位字符串
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //挂载网盘，网盘用户名为guest
	       Bit32 ret = HNC_NetDiskMount("10.10.56.56", "net", "guest", "123456");
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_NetDiskMount(String ip, String progPath, String userName, String password)
        {
            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.NetDiskMount(ip, progPath, userName, password, m_machineNo, m_clientName);
            }

            return 0;
        }
        /*!	@brief 挂载网盘
	     * 
	     * @param[in] ip ：要挂载网盘的IP地址,16位字符串
	     * @param[in] progPath ：要挂载网盘的文件夹名称，32位字符串
	     * @param[in] userName ：网盘用户名，32位字符串
	     * @param[in] password ：网盘用户密码，9位字符串
	     * @param[in] DelegateFunc ：回调函数指针
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //挂载网盘，网盘用户名为guest
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       Bit32 ret = HNC_NetDiskMount("10.10.56.56", "net", "guest", "123456", deleFunc);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_NetDiskMount(String ip, String progPath, String userName, String password, WriteDelegate DelegateFunc)
        {
            if (DelegateFunc != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();

                var st = new
                {
                    func = "NetDiskMount",
                    ip = ip,
                    path = progPath,
                    user = userName,
                    password = password
                };
                String key = js.Serialize(st);

                AddToTable(key, DelegateFunc);
            }

            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.NetDiskMount(ip, progPath, userName, password, m_machineNo, m_clientName);
            }

            return 0;
        }
        /*!	@brief 停止通道正在运行的程序
	     * 
	     * @param[in] ch ：通道号
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //停止0通道正在运行的程序
	       HNC_SysCtrlStopProg(0);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SysCtrlStopProg(Int32 ch)
        {
            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.SysCtrlStopProg(ch, m_machineNo, m_clientName);
            }

            return 0;
        }
        /*!	@brief 停止通道正在运行的程序
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] DelegateFunc ：回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //停止0通道正在运行的程序
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_SysCtrlStopProg(0, deleFunc);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SysCtrlStopProg(Int32 ch, WriteDelegate DelegateFunc)
        {
            if (DelegateFunc != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var st = new
                {
                    func = "SysCtrlStopProg",
                    ch = ch.ToString()
                };
                String key = js.Serialize(st);

                AddToTable(key, DelegateFunc);
            }
            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.SysCtrlStopProg(ch, m_machineNo, m_clientName);
            }

            return 0;
        }
        /*!	@brief 重新运行停止的程序
	     * 
	     * @param[in] ch ：通道号
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //重新运行停止的程序
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_SysCtrlResetProg(0, 0, deleFunc);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SysCtrlResetProg(Int32 ch)
        {
            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.SysCtrlResetProg(ch, m_machineNo, m_clientName);
            }

            return 0;
        }
        /*!	@brief 重新运行停止的程序
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] DelegateFunc ：回调函数指针
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //重新运行停止的程序
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       HNC_SysCtrlResetProg(0, deleFunc);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SysCtrlResetProg(Int32 ch, WriteDelegate DelegateFunc)
        {
            if (DelegateFunc != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var st = new
                {
                    func = "SysCtrlResetProg",
                    ch = ch.ToString()
                };
                String key = js.Serialize(st);

                AddToTable(key, DelegateFunc);
            }
            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.SysCtrlResetProg(ch, m_machineNo, m_clientName);
            }

            return 0;
        }
        /*!	@brief 从下位机加载G代码程序
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] name ：下位机G代码文件完整路径文件名，程序路径名最大长度60
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //从下位机加载G代码
	       Bit32 ret = HNC_SysCtrlSelectProg(0, "../prog/O00F1");
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SysCtrlSelectProg(Int32 ch, String name)
        {
            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.SysCtrlSelectProg(ch, name, m_machineNo, m_clientName);
            }

            return 0;
        }
        /*!	@brief 从下位机加载G代码程序
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] name ：下位机G代码文件完整路径文件名，程序路径名最大长度60
	     * @param[in] DelegateFunc ：回调函数指针
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //从下位机加载G代码
           void CallBack(String content, Int32 result) { ... }
           WriteDelegate deleFunc = new WriteDelegate(CallBack);
	       Bit32 ret = HNC_SysCtrlSelectProg(0, "../prog/O00F1", deleFunc);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SysCtrlSelectProg(Int32 ch, String name, WriteDelegate DelegateFunc)
        {
            if (DelegateFunc != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var st = new
                {
                    func = "SysCtrlSelectProg",
                    ch = ch.ToString(),
                    prog = name
                };
                String key = js.Serialize(st);
                key = key.Replace("/", "\\/");
                AddToTable(key, DelegateFunc);
            }

            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.SysCtrlSelectProg(ch, name, m_machineNo, m_clientName);
            }

            return 0;
        }
        /*!	@brief 从下位机加载G代码子程序
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] subProgNo ：子程序号
	     * @param[in] name ：要加载的子程序名
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //从下位机加载G代码子程序
	       Bit32 ret = HNC_FprogLoadSubProg(0, 5, "/net/O00F1");
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_FprogLoadSubProg(Int32 ch, Int32 subProgNo, String name)
        {
            using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
            {
            	autoClient.Client.FprogLoadSubProg(ch, subProgNo, name, m_machineNo, m_clientName);
            }                    

	        return 0;
        }
        /*!	@brief 获取系统配置
	     * 
	     * @param[in] type ：特获取的系统配置类型
	     * @param[out] config ：系统配置
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取系统配置
	       String config = "";
	       Bit32 ret = HNC_SysCtrlGetConfig(HNC_SYS_CFG_BIN_PATH, ref config);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_SysCtrlGetConfig(Int32 type, ref String config)
        {
            try
            {
                StSysCtrlGetConfig st = new StSysCtrlGetConfig();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	st = autoClient.Client.SysCtrlGetConfig(type, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                config = st.Data.TrimEnd('\0');
                config = config.Split('\0')[0];
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        /*!	@brief 设置校验状态为开启
	     * 
	     * @param[in] ch ：通道号
	     * @param[in] stat ：校验状态
	     * @return 
	     * -  0：成功；
	     * - -1：失败；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //设置校验状态为开启
	       Bit32 ret = HNC_VerifySetStatus(0, 1);
	     * @endcode     
	
	     * @see :: 
	     */
        public Int32 HNC_VerifySetStatus(Int32 ch, Int32 stat)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                	autoClient.Client.VerifySetStatus(ch, stat, m_machineNo, m_clientName);
                }

                return 0;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }

        /*!	@brief 订阅根据索引号获取程序名
         * 
         * @param[in] pindex ：程序索引号
         * @param[in] cancel ：是否订阅，false表示订阅，true表示取消订阅
         
         * @attention 根据索引号获取程序名必须先调用本函数
         * @par 示例:
         * @code
           //订阅根据索引号获取程序名
           HncApi.HNC_FprogGetProgPathByIdx_Subscribe(54, false);
	       string progName = "";
	       Bit32 ret = HncApi.HNC_FprogGetProgPathByIdx(54, ref progName);
         * @endcode     
        
         * @see :: HNC_FprogGetProgPathByIdx
         */
        public void HNC_FprogGetProgPathByIdx_Subscribe(Int32 pindex, Boolean cancel)
        {
            try
            {
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                    autoClient.Client.FprogGetProgPathByIdx_Subscribe(pindex, m_machineNo, cancel);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
            }
        }

        /*!	@brief 根据索引号获取程序名
         * 
         * @param[in] pindex ：程序索引号
         * @param[out] progName ：程序名，最大长度为60
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 使用本函数之前必须先调用HNC_FprogGetProgPathByIdx_Subscribe
         本接口与HNC_FprogGetProgById接口的区别：
	     HNC_FprogGetProgPathByIdx中的pindex参数是程序在运行时的索引编号，可能会出现不同的程序占用同一个编号的情况。
	     HNC_FprogGetProgById中的progId是程序在系统中的唯一编号。系统运行、端点重启等操作均不会改变此ID。只有在修改程序内容以后，其对应的ID才可能发生变化。
         * @par 示例:
         * @code
           //根据索引号获取程序名
           HncApi.HNC_FprogGetProgPathByIdx_Subscribe(54, false);
	       string progName = "";
	       Bit32 ret = HncApi.HNC_FprogGetProgPathByIdx(54, ref progName);
         * @endcode     
        
         * @see :: HNC_FprogGetProgPathByIdx_Subscribe
         */
        public Int32 HNC_FprogGetProgPathByIdx(Int32 pindex, ref String progName)
        {
            try
            {
                StFrogGetProgPathByIdx st = new StFrogGetProgPathByIdx();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                    st = autoClient.Client.FprogGetProgPathByIdx(pindex, m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                progName = Encoding.Default.GetString(st.Data).TrimEnd('\0');
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }

        /*!	@brief 根据程序唯一ID获取程序名
         * @param[in] progId ：程序唯一ID
         * @param[out] progName ：程序名（包含路径）
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //根据程序ID获取程序名
           String progName = "";
           HNC_FprogGetProgById(0, ref progName);
         * @endcode    
        
         * @see ::
         */
        public Int32 HNC_FprogGetProgById(UInt32 progId, ref String progName)
        {
            try
            {
                StProgGetProgById st = new StProgGetProgById();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                    st = autoClient.Client.ProgGetProgById((Int32)progId, m_machineNo);
                }
                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                progName = st.Data;
                progName = progName.Split('\0')[0];
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }

        /*!	@brief 获取程序名与程序唯一ID映射表
         * @param[in/out] progName ：程序名-程序唯一ID映射表
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 当下位机载入程序后程序名ID表才刷新
         * @par 示例:
         * @code
           //获取程序名ID表
           Dictionary<uint, String> progIdMap = new Dictionary<uint, string>();
	       HNC_FprogGetProgIdMap(ref progIdMap);
         * @endcode    
        
         * @see ::
         */
        public Int32 HNC_FprogGetProgIdMap(ref Dictionary<UInt32, String> progName)
        {
            try
            {
                StProgGetProgMap st = new StProgGetProgMap();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                    st = autoClient.Client.ProgGetProgMap(m_machineNo);
                }
                if (st.Ret != 0)
                {
                    return st.Ret;
                }
                foreach(var item in st.Data)
                {
                    progName.Add((uint)item.Key, item.Value); //thrift不能传递无符号数据，所以需要转换
                }
                //progName = st.Data;
                return st.Ret;
            }
            catch (System.Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }
        #endregion

#if _OLD_VER_API_
        #region OldVerApi
        /*!	@brief 获取参数属性值（按parmId参数编号获取）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[out] propValue ：参数属性的值
         * @return 
         * -  0：成功；
         * - -1：失败；

         * @attention 
         * @par 示例:
         * @code
           //示例
           int prop = 0;
	       int ret = HNC_ParamanGetParaPropEx(105210, PARA_PROP_DFVALUE, ref prop);
         * @endcode     

         * @see :: 
         */
        public Int32 HNC_ParamanGetParaPropEx(Int32 parmId, Byte propType, ref Int32 propValue)
        {
            SDataProperty temp = new SDataProperty();
            Int32 ret = HNC_ParamanGetParaProp(parmId, (sbyte)propType, ref temp);
            if (ret == 0)
                propValue = temp.integer;
            return ret;
        }
        /*!	@brief 获取参数属性值（按parmId参数编号获取）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[out] propValue ：参数属性的值
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //说明
           Bit32 ret = HNC_ParamanGetParaPropEx();
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_ParamanGetParaPropEx(Int32 parmId, Byte propType, ref Double propValue)
        {
            SDataProperty temp = new SDataProperty();
            Int32 ret = HNC_ParamanGetParaProp(parmId, (sbyte)propType, ref temp);
            if (ret == 0)
                propValue = temp.real;
            return ret;
        }
        /*!	@brief 获取参数属性值（按parmId参数编号获取）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[out] propValue ：参数属性的值
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //说明
           Bit32 ret = HNC_ParamanGetParaPropEx();
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_ParamanGetParaPropEx(Int32 parmId, Byte propType, SByte[] propValue)
        {
            if (propValue.Length != SDataProperty.PARA_BYTE_ARRAY_LEN)
            {
                return -1;
            }

            SDataProperty temp = new SDataProperty();
            Int32 ret = HNC_ParamanGetParaProp(parmId, (sbyte)propType, ref temp);
            if (ret == 0)
                Array.Copy(temp.array, 0, propValue, 0, SDataProperty.PARA_BYTE_ARRAY_LEN);
            return ret;
        }
        /*!	@brief 获取参数属性值（按parmId参数编号获取）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[out] propValue ：参数属性的值
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //获取最大转矩电流限幅
           String prop;
	       int ret = HNC_ParamanGetParaPropEx(105210, PARA_PROP_NAME, ref prop);
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_ParamanGetParaPropEx(Int32 parmId, Byte propType, ref String propValue)
        {
            SDataProperty temp = new SDataProperty();
            Int32 ret = HNC_ParamanGetParaProp(parmId, (sbyte)propType, ref temp);
            if (ret == 0)
                propValue = temp.str;
            return ret;
        }
        /*!	@brief 设置参数属性值（按parmId参数编号设置）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[out] propValue ：参数属性的值
         * @return 
         * -  0：只返回0；
         
         * @attention 
         * @par 示例:
         * @code
           //说明
           Bit32 ret = HNC_ParamanSetParaPropEx();
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_ParamanSetParaPropEx(Int32 parmId, Byte propType, Int32 propValue)
        {
            SDataProperty temp = new SDataProperty();
            temp.integer = propValue;
            HNC_ParamanSetParaProp(parmId, (sbyte)propType, temp);
            return 0;
        }
        /*!	@brief 设置参数属性值（按parmId参数编号设置）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[in] propValue ：参数属性的值
         * @return 
         * -  0：只返回0；
         
         * @attention 
         * @par 示例:
         * @code
           //说明
           Bit32 ret = HNC_ParamanSetParaPropEx();
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_ParamanSetParaPropEx(Int32 parmId, Byte propType, Double propValue)
        {
            SDataProperty temp = new SDataProperty();
            temp.real = propValue;
            HNC_ParamanSetParaProp(parmId, (sbyte)propType, temp);
            return 0;
        }
        /*!	@brief 设置参数属性值（按parmId参数编号设置）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[in] propValue ：参数属性的值
         * @return 
         * -  0：只返回0；
         
         * @attention 
         * @par 示例:
         * @code
           //说明
           Bit32 ret = HNC_ParamanSetParaPropEx();
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_ParamanSetParaPropEx(Int32 parmId, Byte propType, SByte[] propValue)
        {
            if (propValue.Length != SDataProperty.PARA_BYTE_ARRAY_LEN)
            {
                return -1;
            }

            SDataProperty temp = new SDataProperty();
            Array.Copy(propValue, 0, temp.array, 0, SDataProperty.PARA_BYTE_ARRAY_LEN);
            HNC_ParamanSetParaProp(parmId, (sbyte)propType, temp);

            return 0;
        }
        /*!	@brief 设置参数属性值（按parmId参数编号设置）
         * 
         * @param[in] parmId ：参数编号
         * @param[in] propType ：参数属性的类型
         * @param[in] propValue ：参数属性的值
         * @return 
         * -  0：只返回0；
         
         * @attention 
         * @par 示例:
         * @code
           //说明
           Bit32 ret = HNC_ParamanSetParaPropEx();
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_ParamanSetParaPropEx(Int32 parmId, Byte propType, String propValue)
        {
            SDataProperty temp = new SDataProperty();
            temp.str = propValue;
            HNC_ParamanSetParaProp(parmId, (sbyte)propType, temp);
            return 0;
        }
        /*!	@brief 获取报警的数目
         * 
         * @param[in] type ：报警类型（无效参数）
         * @param[in] level ：报警等级（无效参数）
         * @param[out] num ：报警数目
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 调用本函数之前必须先调用HNC_AlarmSubscribe
         * @par 示例:
         * @code
           //获取报警的数目
	       Bit32 errNum = 0;
           Bit32 ret = HNC_AlarmGetNum(0, 0, ref errNum, 0));
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_AlarmGetNum(Int32 type, Int32 level, ref Int32 num)
        {
            return HNC_AlarmGetNum(ref num);
        }
        /*!	@brief 获取当前报警的报警号和报警文本
         * 
         * @param[in] type ：报警类型（无效参数）
         * @param[in] level ：报警等级（无效参数）
         * @param[in] index ：报警索引号
         * @param[out] alarmNo ：输出报警号
         * @param[out] alarmText ：输出报警文本，最少字符串长度为64
         * @return 
         * -  0：成功；
         * - -1：失败；
         
         * @attention 调用本函数之前必须先调用HNC_AlarmSubscribe
         * @par 示例:
         * @code
           //说明
           Bit32 ret = HNC_AlarmGetData(0, 0, 0, ref alarmNo, ref alarmText);
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_AlarmGetData(Int32 type, Int32 level, Int32 index, ref Int32 alarmNo, ref String alarmText)
        {
            return HNC_AlarmGetData(index, ref alarmNo, ref alarmText);
        }
        /*!	@brief 向下位机推送事件
         * 
         * @param[in] ev ：推送的事件
         * @return 
         * -  0：只返回0；
         
         * @attention 该接口在1.26.04上已弃用
         * @par 示例:
         * @code
           //向下位机推送事件
           向下位机推送事件
	       SEventElement ev = new SEventElement();
	       ...
           Bit32 ret = HNC_EventPut(ev);
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_EventPut(SEventElement ev)
        {
            String str = ev.buf.ToString();
            HNC_PutMessage(str);
            return 0;
        }        
        /*!	@brief MDI开启
         * 
         * @return 
         * -  1：成功；
	     * -  0：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //开启0通道的MDI
           Bit32 ret = HNC_SysCtrlMdiOpen(0);
         * @endcode     
        
         * @see :: HNC_MdiClose
         */
        public Int32 HNC_SysCtrlMdiOpen()
        {
            return HNC_MdiOpen(0);
        }
        /*!	@brief MDI关闭
         * 
         * @return 
         * -  1：成功；
	     * -  0：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //关闭MDI
           Bit32 ret = HNC_SysCtrlMdiClose(0);
         * @endcode     
        
         * @see :: HNC_MdiOpen
         */
        public Int32 HNC_SysCtrlMdiClose()
        {
            return HNC_MdiClose();
        }
        /*!	@brief 设置MDI的文本
         * 
         * @param[in] txt ：设置的文本
         * @param[in] txtLen ：文本长度（无效参数）
         * @return 
         * -  1：成功；
	     * -  0：失败；
         
         * @attention 
         * @par 示例:
         * @code
           //设置MDI的文本
           Bit32 ret = HNC_FprogMdiSetContent("G00X100Y200Z300");
         * @endcode     
        
         * @see :: 
         */
        public Int32 HNC_FprogMdiSetContent(String txt, Int32 txtLen)
        {
            return HNC_MdiSetContent(txt);
        }
        #endregion
#endif

        #region private_filed
        /*!	@brief 获取进程名
	     * 
	     * @return 
	     * 返回进程名；
	 
	     * @attention 
	     * @par 示例:
	     * @code
	       //获取进程名
	       string ClientName = GetClientName();
	     * @endcode     
	
	     * @see :: 
	     */
        private String GetClientName()
        {
            System.Diagnostics.Process processes = System.Diagnostics.Process.GetCurrentProcess();
            String name = processes.ProcessName + ":" + processes.Id.ToString();
            return name;
        }
        /*!	@brief 将机器号、key和回调函数指针存入字典m_map中
	     * @param[in] key ：字符串
	     * @param[in] DelegateFunc ：回调函数指针，参见delegate void WriteDelegate(String content, Int32 result);
	 
	     * @attention 内部函数，主要在带回调函数指针参数的写值接口函数中使用
	     * @par 示例:
	     * @code
	       //说明
	       string key = "RegSetValue:" + type + ":" + index + ":" + value;
	       AddToTable(key, DelegateFunc);
	       AddToTable();
	     * @endcode     
	
	     * @see :: HNC_RegSetValue
	     */
        private void AddToTable(String key, WriteDelegate DelegateFunc)
        {
            lock (m_dicLock)
            {
                if (!m_dicDelegate.ContainsKey(key))
                {
                    m_dicDelegate.Add(key, DelegateFunc);
                }
                else
                {
                    m_dicDelegate[key] = DelegateFunc;
                }
            }
        }
        /*!	@brief 写值事件扫描线程函数。
	     * 内部函数，在:: HNC_NetInit中调用
	
	     * @see :: HNC_NetInit
	     */
        private void WriteEventScanThreadFunc()
        {
            String key = String.Empty;
            Int32 result = 0;
            Int32 ret = 0;

            while (m_writeEventScanThreadStart)
            {
                Thread.Sleep(100);

                lock (m_dicLock)
                {
                    ret = GetWriteEvent(ref key, ref result);
                    if (ret != 0)
                    {
                        continue;
                    }

                    WriteDelegate delegateFunc;
                    if (m_dicDelegate.TryGetValue(key.TrimEnd(), out delegateFunc))
                    {
                        delegateFunc(key, result);
                        m_dicDelegate.Remove(key);
                    }
                }
            }
        }
        /*!	@brief 获取写值事件信息。
	     * 内部函数，在WriteEventScanThreadFunc中调用
	     * @param[out] key ：写值事件内容
	     * @param[out] result ：写值事件结果

	     * @see :: WriteEventScanThreadFunc
	     */
        private Int32 GetWriteEvent(ref String key, ref Int32 result)
        {
            try
            {
                StWriteResult st = new StWriteResult();
                using (AutoDeleteClient autoClient = new AutoDeleteClient(m_thriftPool))
                {
                    st = autoClient.Client.GetWriteResult(m_machineNo);
                }

                if (st.Ret != 0)
                {
                    return st.Ret;
                }

                key = st.Key;
                result = st.Result;
                return st.Ret;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.ToString());
                return -1;
            }
        }

        private void ConnThreadFunc()
        {
            if (m_connHandler == null)
            {
                return;
            }

            Int16 stat = -1;
            const Int32 TIME_OUT_CNT = 1000;
            Int32 count = 0;
            while ((stat = HNC_NetIsConnect()) != 0)
            {
                Thread.Sleep(10);

                if (count >= TIME_OUT_CNT)
                {
                    m_connHandler(m_ip, m_port, false);
                    return;
                }
                count++;
            }
            if (stat == 0)
            {
                m_connHandler(m_ip, m_port, true);
            }
        }
        private void AutoCThreadFunc()
        {
            Int16 connectStatus = -1;

            while (m_AutoConnectThreadStart)
            {
                connectStatus = HNC_NetIsConnect();
                if (connectStatus != 0)//不是连接状态
                {
                    try
                    {
                        HNC_NetConnect(m_ip, m_port);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine(ex.ToString());
                        return;
                    }
                }
                Thread.Sleep(30 * 1000);//30S捕捉一次
            }
        }

        const Int32 REG_SYS_F_BASE = 2960;
        const Int32 REG_SYS_G_BASE = 2960;
        const Int32 SMPL_CRTL_BIT = 12;
        //private TTransport m_transport;
        //private TProtocol m_protocol;
        //private HncRpcService.Client m_client;
        private String m_clientName;
        private String m_ip;
        private UInt16 m_port;
        private Int16 m_machineNo;
        //private Object m_lock;
        private Dictionary<String, WriteDelegate> m_dicDelegate;
        private object m_dicLock;
        private Boolean m_writeEventScanThreadStart;
        private Thread m_writeEventScanThread;
        private Boolean m_isInit = false;
        private Thread m_connThread;
        private ConnDelegate m_connHandler;
        private ThriftPool m_thriftPool;

        private Thread m_AutoCThread;
        private Boolean m_AutoConnectThreadStart;//线程启动判断
#endregion
    }
}
