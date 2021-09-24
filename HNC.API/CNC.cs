using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentFTP;
using System.IO;
using HNCAPI_INTERFACE;

namespace HNC.API
{
    /// <summary>
    /// 数控系统（ServerWindow）
    /// </summary>
    public class CNCV2 : CNCBase
    {
       
        /// <summary>
        /// 数控系统（ServerWindow）
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="apiIP">API的IP地址</param>
        /// <param name="apiPort">API的端口号</param>
        public CNCV2(string ip, ushort port = 10001, string apiIP = "127.0.0.1", ushort apiPort = 9090)
        {
            if (IPAddress.TryParse(ip, out var address))
            {
                IP = ip;
            }
            else
            {
                throw new ArgumentException($"IP地址：{ip}不合法");
            }
            Port = port;
            HncApi  machine= new HncApi();
            if (machine.HNC_NetInit(apiIP, apiPort, IP) != 0)
            {
                return; throw new Exception("网络初始化失败");
            }
            Machine = machine;
            var machineNo = Machine.HNC_NetConnect(IP, Port);
            if (machineNo < 0 || machineNo >= 255)
            {
                return; throw new Exception("网络连接失败");
            }
            Machine.HNC_AlarmSubscribe(false);
            Machine.HNC_EventSubscribe(false);
            Machine.HNC_ClientRequestWriteToken();
            Machine.HNC_NetAutoConnect(true);
            //FTP root:111111 lnc8:111111
            FtpClient = new FtpClient(IP, 10021, "lnc8", "111111");
        }


        /// <summary>
        /// 数控系统（ServerWindow）(192.168.1.[*])
        /// </summary>
        /// <param name="ip">IP地址(192.168.1.[*])</param>
        /// <param name="port">端口号</param>
        public CNCV2(byte ip, ushort port = 10001) : this($"192.168.1.{ip}", port) { }

        /// <summary>
        /// 数控系统（ServerWindow）
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        public CNCV2(IPAddress ip, ushort port = 10001) : this(ip?.ToString(), port) { }

        /// <summary>
        /// 获取当前通道内加载得程序名称
        /// </summary>
        /// <param name="progName"></param>
        /// <returns></returns>
        public string CNCCurGrogName()
        {
            var progName = "";
            var ret = Machine.HNC_FprogGetFullName(0, ref progName);
            return progName;
        }
        /// <summary>
        /// HncApi
        /// </summary>
     
        public HncApi Machine { get; } = new HncApi();
      
        /// <summary>
        /// FTP客户端
        /// </summary>
        public FtpClient FtpClient { get; }

        /// <summary>
        /// 程序结束前调用此方法
        /// </summary>
        public void Close()
        {
            FtpClient?.Disconnect();
            Machine.HNC_ClientReleaseWriteToken();
            Machine.HNC_NetExit();
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        public override bool IsConnected() => 0 == Machine.HNC_NetIsConnect();

        /// <summary>


        #region 寄存器 Reg

        /// <summary>
        /// 寄存器Clear
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">
        /// X,Y-[0,511]
        /// F,G-[0,3119]
        /// R-[0,399]
        /// B-[0,1721]
        /// </param>
        /// <param name="bit">
        /// X,Y,R-[0,7] 
        /// F,G,W-[0,15]
        /// D,B,P-[0,31]
        /// </param>
        /// <returns></returns>
        public override bool RegClearBit(HncRegType type, int index, int bit) => 0 == Machine.HNC_RegClrBit((int)type, index, bit);

        /// <summary>
        /// 寄存器Get(支持X,Y,R,F,G)
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">
        /// X,Y-[0,511]
        /// F,G-[0,3119]
        /// R-[0,399]
        /// B-[0,1721]!
        /// </param>
        /// <param name="value">
        /// X,Y,R-[0,255]
        /// F,G-[0,65535]
        /// </param>
        /// <returns></returns>
        public override bool RegGetValue(HncRegType type, int index, out int value)
        {
            value = 0;
            var r = 0 == Machine.HNC_RegGetValue((int)type, index, ref value);
            switch (type)
            {
                case HncRegType.REG_TYPE_X:
                case HncRegType.REG_TYPE_Y:
                case HncRegType.REG_TYPE_R:
                    value = Convert.ToInt32((byte)value);
                    break;
                case HncRegType.REG_TYPE_F:
                case HncRegType.REG_TYPE_G:
                    value = Convert.ToInt32((ushort)value);
                    break;
                default:
                    value = Convert.ToInt32((ushort)value);
                    break;
            }
            return r;
        }

        /// <summary>
        /// 寄存器Set
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">
        /// X,Y-[0,511]
        /// F,G-[0,3119]
        /// R-[0,399]
        /// B-[0,1721]
        /// </param>
        /// <param name="bit">
        /// X,Y,R-[0,7] 
        /// F,G,W-[0,15]
        /// D,B,P-[0,31]
        /// </param>
        /// <returns></returns>
        public override bool RegSetBit(HncRegType type, int index, int bit) => 0 == Machine.HNC_RegSetBit((int)type, index, bit);

        /// <summary>
        /// 寄存器SetValue
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">
        /// X,Y-[0,511]
        /// F,G-[0,3119]
        /// R-[0,399]
        /// B-[0,1721]
        /// </param>
        /// <param name="value">
        /// X,Y,R-[0,255]
        /// F,G-[0,65535]
        /// </param>
        /// <returns></returns>
        public override bool RegSetValue(HncRegType type, int index, int value) => 0 == Machine.HNC_RegSetValue((int)type, index, value);

        /// <summary>
        /// 寄存器GetMulti(支持X,Y,R,F,G)
        /// </summary>
        /// <param name="index">
        /// X,Y-[0,511]
        /// F,G-[0,3119]
        /// R-[0,399]
        /// B-[0,1721]!
        /// </param>
        /// <param name="num">要获取的寄存器个数</param>
        /// <param name="values">多个寄存器值</param>
        /// <param name="type">寄存器类型</param>
        /// <returns></returns>
        public bool RegGetMultiValues(int index, ushort num, out IList<int> values, HncRegType type)
        {
            var bytes = new byte[num];
            var r = 0 == Machine.HNC_RegGetMultiValues((int)type, index, num, ref bytes);
            values = bytes.Select(s => Convert.ToInt32(s)).ToList();
            return r;
        }

        /// <summary>
        /// 寄存器GetNum获取总组数(支持X,Y,R,F,G)
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="num">寄存器组数</param>
        /// <returns></returns>
        public bool RegGetNum(HncRegType type, out int num)
        {
            num = 0;
            return 0 == Machine.HNC_RegGetNum((int)type, ref num);
        }

        #endregion


        #region 变量 Var

        /// <summary>
        /// 按类型获取变量的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool VarGetValue(HncVarType type, out string dataType, out object dataValue) => VarGetValue(type, out dataType, out dataValue, 0, 0);

        /// <summary>
        /// 按类型获取变量的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <param name="no"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool VarGetValue(HncVarType type, out string dataType, out object dataValue, int no = 0, int index = 0)
        {
            var r = false;
            if (type == HncVarType.VAR_TYPE_SYSTEM_F)
            {
                dataType = typeof(double).FullName;
                double vd = 0;
                r = 0 == Machine.HNC_VarGetValue((int)type, no, index, ref vd);
                dataValue = vd;
            }
            else
            {
                dataType = typeof(int).FullName;
                int vi = 0;
                r = 0 == Machine.HNC_VarGetValue((int)type, no, index, ref vi);
                dataValue = vi;
            }
            return r;
        }

        /// <summary>
        /// 按类型设置变量的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public bool VarSetValue(HncVarType type, int no, int index, object value) => 0 == Machine.HNC_VarSetValue((int)type, no, index, type == HncVarType.VAR_TYPE_SYSTEM_F ? (double)value : (int)value);

        /// <summary>
        /// 按类型设置变量的位
        /// </summary>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="index"></param>
        /// <param name="bit"></param>
        public bool VarSetBit(HncVarType type, int no, int index, short bit) => 0 == Machine.HNC_VarSetBit((int)type, no, index, bit);

        /// <summary>
        /// 按类型清除变量的位
        /// </summary>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="index"></param>
        /// <param name="bit"></param>
        public bool VarClrBit(HncVarType type, int no, int index, short bit) => 0 == Machine.HNC_VarClrBit((int)type, no, index, bit);

        #endregion

        #region 宏变量 MacroVar

        /// <summary>
        /// 按索引号获取宏变量的值 
        /// </summary>
        /// <param name="no">
        /// 变量编号 [40000, 59999]: 系统变量；其中，[50000, 54999]：用户自定义变量
        /// </param>
        /// <param name="value">宏变量值</param>
        /// <returns></returns>
        public bool MacroVarGetValue(int no, out SDataUnion value)
        {
            value = default(SDataUnion);
            return 0 == Machine.HNC_MacroVarGetValue(no, ref value);
        }

        /// <summary>
        /// 按索引号设置宏变量的值
        /// </summary>
        /// <param name="no">
        /// 变量编号 [40000, 59999]: 系统变量；其中，[50000, 54999]：用户自定义变量
        /// </param>
        /// <param name="value">宏变量值</param>
        /// <returns></returns>
        public bool MacroVarSetValue(int no, SDataUnion value) => 0 == Machine.HNC_MacroVarSetValue(no, value);

        #endregion

        #region 参数 Parameter

        /// <summary>
        /// 设置参数属性的值
        /// </summary>
        /// <param name="parmId">参数编号</param>
        /// <param name="type">参数属性的类型</param>
        /// <param name="value">参数属性的值</param>
        public void ParamanSetParaProp(int parmId, ParaPropType type, SDataProperty value) => Machine.HNC_ParamanSetParaProp(parmId, (sbyte)type, value);

        /// <summary>
        /// 获取参数属性值
        /// </summary>
        /// <param name="parmId">参数编号</param>
        /// <param name="type">参数属性的类型</param>
        /// <param name="value">参数属性的值</param>
        /// <returns></returns>
        public bool ParamanGetParaProp(int parmId, ParaPropType type, out SDataProperty value)
        {
            value = default(SDataProperty);
            return 0 == Machine.HNC_ParamanGetParaProp(parmId, (sbyte)type, ref value);
        }

        /// <summary>
        /// 批量获取参数属性值（按parmId参数编号获取）
        /// </summary>
        /// <param name="parmId">参数编号</param>
        /// <param name="type">参数属性的类型</param>
        /// <param name="props">参数属性值结构体变量</param>
        /// <returns></returns>
        public bool ParamanGetProps(int parmId, ParaPropType type, out SParmProperty props)
        {
            props = default(SParmProperty);
            return 0 == Machine.HNC_ParamanGetProps(parmId, (sbyte)type, ref props);
        }

        /// <summary>
        /// 保存参数数据文件
        /// </summary>
        /// <returns></returns>
        public bool ParamanSave() => 0 == Machine.HNC_ParamanSave();

        /// <summary>
        /// 保存参数结构文件
        /// </summary>
        /// <returns></returns>
        public bool ParamanSaveStrFile() => 0 == Machine.HNC_ParamanSaveStrFile();

        #endregion

        #region 轴 Axis

        /// <summary>
        /// 获取轴数据的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool AxisGetValue(HncAxis type, out string dataType, out object dataValue) => AxisGetValue(type, out dataType, out dataValue, 0);
       // public bool AxisGetValue(HncAxis type, out string dataType, out object dataValue) => Machine.HNC_AxisGetValue(type, ax, out dataValue);

        /// <summary>
        /// 获取轴数据的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <param name="axisAx"></param>
        /// <returns></returns>
        public bool AxisGetValue(HncAxis type, out string dataType, out object dataValue, int axisAx = 0)
        {
            var r = false;
            int vi = 0;
            double vd = 0;
            string vs = string.Empty;
            switch (type)
            {
                case HncAxis.HNC_AXIS_TYPE:           // 轴类型 {Get(Bit32)}
                case HncAxis.HNC_AXIS_CHAN:           // 获取通道号 {Get(Bit32)}
                case HncAxis.HNC_AXIS_CHAN_INDEX:     // 获取在通道中的轴号 {Get(Bit32)}
                case HncAxis.HNC_AXIS_CHAN_SINDEX:    // 获取在通道中的主轴号 {Get(Bit32)}
                case HncAxis.HNC_AXIS_LEAD:           // 获取引导轴 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_ACT_POS:        // 机床实际位置 {Get(Bit32)}
                case HncAxis.HNC_AXIS_ACT_POS2:       // 机床实际位置2 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_CMD_POS:        // 机床指令位置 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_ACT_POS_WCS:    // 工件实际位置 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_CMD_POS_WCS:    // 工件指令位置 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_ACT_POS_RCS:    // 相对实际位置 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_CMD_POS_RCS:    // 相对指令位置 {Get(Bit32)}
                case HncAxis.HNC_AXIS_ACT_PULSE:      // 实际脉冲位置 {Get(Bit32)}
                case HncAxis.HNC_AXIS_CMD_PULSE:      // 指令脉冲位置 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_PROG_POS:       // 编程位置 {Get(Bit32)}
                case HncAxis.HNC_AXIS_ENC_CNTR:       // 电机位置 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_CMD_VEL:        // 指令速度 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_LEFT_TOGO:      // 剩余进给 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_WCS_ZERO:       // 工件零点 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_WHEEl_OFF:      // 手轮中断偏移量 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_FOLLOW_ERR:     // 跟踪误差 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_SYN_ERR:        // 同步误差	{Get(Bit32)}
                //case HncAxis.HNC_AXIS_COMP:           // 轴补偿值 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_ZSW_DIST:       // Z脉冲偏移 {Get(Bit32)}
                //case HncAxis.HNC_AXIS_REAL_ZERO:      // 相对零点 {Get(Bit32)}
                case HncAxis.HNC_AXIS_IS_HOMEF:       // 回零完成 {Get(Bit32)}
                case HncAxis.HNC_AXIS_MOTOR_TYPE_FLAG:// 伺服类型出错标志 {Get(Bit32)}
                    r = 0 == Machine.HNC_AxisGetValue((int)type, axisAx, ref vi);
                    dataType = vi.GetType().FullName;
                    dataValue = vi;
                    return r;
                case HncAxis.HNC_AXIS_ACT_POS:
                case HncAxis.HNC_AXIS_CMD_POS:
                case HncAxis.HNC_AXIS_ACT_POS_WCS:
                case HncAxis.HNC_AXIS_CMD_POS_WCS:
                case HncAxis.HNC_AXIS_ACT_POS_RCS:
                case HncAxis.HNC_AXIS_CMD_POS_RCS:
                case HncAxis.HNC_AXIS_PROG_POS:
                case HncAxis.HNC_AXIS_LEFT_TOGO:
                case HncAxis.HNC_AXIS_WHEEl_OFF:
                case HncAxis.HNC_AXIS_COMP:
                case HncAxis.HNC_AXIS_REAL_ZERO:
                case HncAxis.HNC_AXIS_ZSW_DIST:
                case HncAxis.HNC_AXIS_SYN_ERR:
                case HncAxis.HNC_AXIS_FOLLOW_ERR:
                case HncAxis.HNC_AXIS_WCS_ZERO:
                case HncAxis.HNC_AXIS_CMD_VEL:
                case HncAxis.HNC_AXIS_ACT_VEL:        // 实际速度 {Get(fBit64)}
                case HncAxis.HNC_AXIS_MOTOR_REV:      // 电机转速 {Get(fBit64)}
                case HncAxis.HNC_AXIS_DRIVE_CUR:      // 驱动单元电流 {Get(fBit64)}
                case HncAxis.HNC_AXIS_LOAD_CUR:       // 负载电流 {Get(fBit64)}
                case HncAxis.HNC_AXIS_RATED_CUR:      // 额定电流 {Get(fBit64)}
                case HncAxis.HNC_AXIS_WAVE_FREQ:      // 波形频率 {Get(fBit64)}
                    r = 0 == Machine.HNC_AxisGetValue((int)type, axisAx, ref vd);
                    dataType = vd.GetType().FullName;
                    dataValue = vd;
                    return r;
                case HncAxis.HNC_AXIS_NAME:           // 轴名 {Get(Bit8[PARAM_STR_LEN])}
                case HncAxis.HNC_AXIS_DRIVE_VER:      // 伺服驱动版本 {Get(Bit8[32])}
                case HncAxis.HNC_AXIS_MOTOR_TYPE:     // 伺服类型 {Get(Bit8[32])}
                    r = 0 == Machine.HNC_AxisGetValue((int)type, axisAx, ref vs);
                    dataType = vs.GetType().FullName;
                    dataValue = vs;
                    return r;
                default:
                    dataType = vs.GetType().FullName;
                    dataValue = vs;
                    return r;
            }
        }

        /// <summary>
        /// 批量取轴数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axisAx"></param>
        /// <returns></returns>
        public bool AxisGetMultiValues(out SAxisVals value, int axisAx = 0)
        {
            value = default(SAxisVals);
            return 0 == Machine.HNC_AxisGetMultiValues(axisAx, ref value);
        }

        #endregion

        #region 通道 Channel

        /// <summary>
        /// 获取通道数据的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool ChannelGetValue(HncChannel type, out string dataType, out object dataValue) => ChannelGetValue(type, out dataType, out dataValue, 0, 0);

        /// <summary>
        /// 获取通道数据的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <param name="chanCh"></param>
        /// <param name="chanIndex"></param>
        /// <returns></returns>
        public bool ChannelGetValue(HncChannel type, out string dataType, out object dataValue, int chanCh = 0, int chanIndex = 0)
        {
            var r = false;
            int vi = 0;
            double vd = 0;
            string vs = string.Empty;
            switch (type)
            {
                case HncChannel.HNC_CHAN_IS_EXIST:                   //"通道是否存在"
                case HncChannel.HNC_CHAN_MAC_TYPE:                   //"通道的机床类型"
                case HncChannel.HNC_CHAN_AXES_MASK:                  //"轴掩码"
                case HncChannel.HNC_CHAN_AXES_MASK1:                 //"轴掩码1"               
                case HncChannel.HNC_CHAN_CMD_TYPE:                   //"读取当前G代码的标志"               
                case HncChannel.HNC_CHAN_FEED_OVERRIDE:              //"进给修调"
                case HncChannel.HNC_CHAN_RAPID_OVERRIDE:             //"快移修调"
                case HncChannel.HNC_CHAN_MCODE:                      //"通道的M指令"
                case HncChannel.HNC_CHAN_TCODE:                      //"通道的T指令"
                case HncChannel.HNC_CHAN_TOFFS:                      //"通道中的刀偏号"
                case HncChannel.HNC_CHAN_TOOL_USE:                   //"当前刀具"
                case HncChannel.HNC_CHAN_TOOL_RDY:                   //"准备好交换的刀具"
                case HncChannel.HNC_CHAN_MODE:                       //"模式"
                case HncChannel.HNC_CHAN_IS_MDI:                     //"MDI"
                case HncChannel.HNC_CHAN_CYCLE:                      //"循环启动"
                case HncChannel.HNC_CHAN_HOLD:                       //"进给保持"
                case HncChannel.HNC_CHAN_IS_PROGSEL:                 //"已选程序"
                case HncChannel.HNC_CHAN_IS_PROGEND:                 //"程序运行完成"
                case HncChannel.HNC_CHAN_IS_THREADING:               //"螺纹加工"
                case HncChannel.HNC_CHAN_IS_RIGID:                   //"刚性攻丝"
                case HncChannel.HNC_CHAN_IS_REWINDED:                //"重运行复位状态"
                case HncChannel.HNC_CHAN_IS_ESTOP:                   //"急停"
                case HncChannel.HNC_CHAN_IS_RESETTING:               //"复位"
                case HncChannel.HNC_CHAN_IS_RUNNING:                 //"运行中"
                case HncChannel.HNC_CHAN_IS_HOMING:                  //"回零中"
                case HncChannel.HNC_CHAN_IS_MOVING:                  //"轴移动中"
                case HncChannel.HNC_CHAN_DIAMETER:                   //"直半径编程"
                case HncChannel.HNC_CHAN_VERIFY:                     //"校验"
                case HncChannel.HNC_CHAN_RUN_ROW:                    //"运行行"
                case HncChannel.HNC_CHAN_DCD_ROW:                    //"译码行"
                case HncChannel.HNC_CHAN_SEL_PROG:                   //"选择程序的编号"
                case HncChannel.HNC_CHAN_RUN_PROG:                   //"运行程序的编号"
                case HncChannel.HNC_CHAN_PART_CNTR:                  //"加工计数"
                case HncChannel.HNC_CHAN_PART_STATI:                 //"工件总数"
                //case HncChannel.HNC_CHAN_HMI_RESET:                
                //case HncChannel.HNC_CHAN_CHG_PROG:                 
                //case HncChannel.HNC_CHAN_PERIOD_TOTAL:             
                case HncChannel.HNC_CHAN_LAX:                         //"通道轴对应的逻辑轴号"              
                case HncChannel.HNC_CHAN_MODAL:                       //"通道模态 共80组"
                case HncChannel.HNC_CHAN_SPDL_LAX:                    //"通道主轴对应的逻辑轴号，动态"
                case HncChannel.HNC_CHAN_SPDL_PARA_LAX:               //"通道主轴对应的逻辑轴号，静态"      
                case HncChannel.HNC_CHAN_SPDL_OVERRIDE:               //"主轴修调"
                //case HncChannel.HNC_CHAN_DO_HOLD:                  
                case HncChannel.HNC_CHAN_PROG_FLOW:                   //"正在调用手动子程序"
                case HncChannel.HNC_CHAN_H_OFF:                       //"当前使用刀具长度补偿号"
                case HncChannel.HNC_CHAN_D_OFF:                       //"当前使用刀具半径补偿号"
                                                                      //case HncChannel.HNC_CHAN_BP_POS_EX:
                                                                      //case HncChannel.HNC_CHAN_IS_SBL:
                                                                      //case HncChannel.HNC_CHAN_PART_NEED:
                                                                      //case HncChannel.HNC_CHAN_TOTAL:
                    r = 0 == Machine.HNC_ChannelGetValue((int)type, chanCh, chanIndex, ref vi);
                    dataType = vi.GetType().FullName;
                    dataValue = vi;
                    return r;
                case HncChannel.HNC_CHAN_BP_POS:                    //"断点位置"
                case HncChannel.HNC_CHAN_CMD_FEEDRATE:              //"指令进给速度"
                case HncChannel.HNC_CHAN_ACT_FEEDRATE:              //"实际进给速度"
                case HncChannel.HNC_CHAN_PROG_FEEDRATE:             //"编程指令速度"
                case HncChannel.HNC_CHAN_CMD_SPDL_SPEED:            //"主轴指令速度"
                case HncChannel.HNC_CHAN_ACT_SPDL_SPEED:            //"主轴实际速度"
                    r = 0 == Machine.HNC_ChannelGetValue((int)type, chanCh, chanIndex, ref vd);
                    dataType = vd.GetType().FullName;
                    dataValue = vd;
                    return r;
                case HncChannel.HNC_CHAN_NAME:                      //"通道名"
                case HncChannel.HNC_CHAN_AXIS_NAME:                 //"编程轴名"
                case HncChannel.HNC_CHAN_SPDL_NAME:                 //"编程主轴名"
                    r = 0 == Machine.HNC_ChannelGetValue((int)type, chanCh, chanIndex, ref vs);
                    dataType = vs.GetType().FullName;
                    dataValue = vs;
                    return r;
                default:
                    dataType = vs.GetType().FullName;
                    dataValue = vs;
                    return r;
            }
        }

        #endregion

        #region 系统 System

        /// <summary>
        /// 获取系统数据的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool SystemGetValue(HncSystem type, out string dataType, out object dataValue)
        {
            var r = false;
            int vi = 0;
            string vs = string.Empty;
            switch (type)
            {
                case HncSystem.HNC_SYS_CHAN_NUM:
                case HncSystem.HNC_SYS_MOVE_UNIT:
                case HncSystem.HNC_SYS_TURN_UNIT:
                case HncSystem.HNC_SYS_METRIC_DISP:
                case HncSystem.HNC_SYS_SHOW_TIME:
                case HncSystem.HNC_SYS_POP_ALARM:
                case HncSystem.HNC_SYS_GRAPH_ERASE:
                //case HncSystem.HNC_SYS_MAC_TYPE:
                case HncSystem.HNC_SYS_PREC:
                case HncSystem.HNC_SYS_F_PREC:
                case HncSystem.HNC_SYS_S_PREC:
                case HncSystem.HNC_SYS_CNC_VER:
                case HncSystem.HNC_SYS_MCP_KEY:
                case HncSystem.HNC_SYS_ACTIVE_CHAN:
                case HncSystem.HNC_SYS_REQUEST_CHAN:
                case HncSystem.HNC_SYS_MDI_CHAN:
                case HncSystem.HNC_SYS_REQUEST_CHAN_MASK:
                //case HncSystem.HNC_SYS_CHAN_MASK:
                //case HncSystem.HNC_SYS_PLC_STOP:
                //case HncSystem.HNC_SYS_POWEROFF_ACT:
                case HncSystem.HNC_SYS_IS_HOLD_REDECODE:
                case HncSystem.HNC_SYS_ACCESS_LEVEL:
                //case HncSystem.HNC_SYS_MUL_CHAN_RESET:
                case HncSystem.HNC_SYS_RIGHTS_KEY:
                case HncSystem.HNC_SYS_REG_DAYS_REMANING:
                    r = 0 == Machine.HNC_SystemGetValue((int)type, ref vi);
                    dataType = typeof(int).FullName;
                    dataValue = vi;
                    break;
                //case HncSystem.HNC_SYS_PLC2_CUR_CYCLE:
                //case HncSystem.HNC_SYS_PLC2_MIN_CYCLE:
                //case HncSystem.HNC_SYS_PLC2_MAX_CYCLE:
                //case HncSystem.HNC_SYS_PLC_ONLINE:
                //case HncSystem.HNC_SYS_TOTAL:
                //    break;
                case HncSystem.HNC_SYS_NCK_VER:
                case HncSystem.HNC_SYS_DRV_VER:
                case HncSystem.HNC_SYS_PLC_VER:
                case HncSystem.HNC_SYS_NC_VER:
                case HncSystem.HNC_SYS_SN_NUM:
                case HncSystem.HNC_SYS_MACHINE_TYPE:
                case HncSystem.HNC_SYS_MACHINE_INFO:
                case HncSystem.HNC_SYS_MACFAC_INFO:
                case HncSystem.HNC_SYS_USER_INFO:
                case HncSystem.HNC_SYS_MACHINE_NUM:
                case HncSystem.HNC_SYS_EXFACTORY_DATE:
                    r = 0 == Machine.HNC_SystemGetValue((int)type, ref vs);
                    dataType = typeof(string).FullName;
                    dataValue = vs;
                    break;
                default:
                    dataType = typeof(string).FullName;
                    dataValue = vs;
                    break;
            }
            return r;
        }

        #endregion

        #region 警报 Alarm

        /// <summary>
        /// 报警类型：系统(SY)、通道(CH)、轴(AX)、伺服(SV)、PLC(PC)、设备(DV)、语法(PS)、用户PLC(UP)、HMI(HM)；
        /// 报警级别：报警(ERR)、提示(MSG)；
        /// 报警号9位，
        /// 通道、语法：(1位报警类型)+(1位报警级别)+(3位通道号)+(4位报警内容)；
        /// 轴、伺服：(1位报警类型)+(1位报警级别)+(3位轴号) +(4位报警内容)；
        /// 其它：(1位报警类型)+(1位报警级别)+(7位报警内容)；
        /// 如：急停报警 CH_ERR_0000003
        /// </summary>
        /// <returns>警报字典（Key：编号，Value：内容）</returns>
        public IDictionary<int, string> AlarmGetData()
        {
            int num = 0;
            var dict = new Dictionary<int, string>();
            if (0 == Machine.HNC_AlarmGetNum(ref num))
            {
                for (int i = 0; i < num; i++)
                {
                    int alarmNo = 0; string alarmText = string.Empty;
                    if (0 == Machine.HNC_AlarmGetData(i, ref alarmNo, ref alarmText))
                    {
                        dict.Add(alarmNo, alarmText);
                    }
                }
            }
            return dict;
        }

        #endregion

        #region 事件 Event

        /// <summary>
        /// 事件Get
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        public bool EventGetSysEv(out SEventElement ev)
        {
            ev = default(SEventElement);
            return 0 == Machine.HNC_EventGetSysEv(ref ev);
        }

        /// <summary>
        /// 向下位机推送消息
        /// </summary>
        /// <param name="msg"></param>
        public void PutMessage(string msg)
        {
            if (msg.Length > EVENTDEF.MAX_PUT_MSG_LEN)
            {
                //推送消息长度超过上限
            }
            else
            {
                Machine.HNC_PutMessage(msg);
            }
        }

        #endregion

        #region 刀具 Tool

        /// <summary>
        /// 刀具文件保存
        /// </summary>
        /// <returns></returns>
        public bool ToolSave() => 0 == Machine.HNC_ToolSave();

        /// <summary>
        /// 获取系统最大刀具数目 
        /// </summary>
        /// <returns></returns>
        public int ToolGetMaxToolNum() => Machine.HNC_ToolGetMaxToolNum();

        /// <summary>
        /// 获取刀具参数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <param name="no">刀具号1~1024</param>
        /// <returns></returns>
        public bool ToolGetToolPara(ToolParaIndex index, out string dataType, out object dataValue, int no = 1)
        {
            if (index >= ToolParaIndex.INFTOOL_ID && index <= ToolParaIndex.INFTOOL_G64MODE)
            {
                int vi = 0;
                var r = Machine.HNC_ToolGetToolPara(no, (int)index, ref vi);
                dataType = typeof(int).FullName;
                dataValue = vi;
                return 0 == r;
            }
            else
            {
                double vd = 0;
                var r = Machine.HNC_ToolGetToolPara(no, (int)index, ref vd);
                dataType = typeof(double).FullName;
                dataValue = vd;
                return 0 == r;
            }
        }

        /// <summary>
        /// 设置刀具参数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="no">刀具号1~1024</param>
        /// <returns></returns>
        public bool ToolSetToolPara(ToolParaIndex index, object value, int no = 1)
        {
            if (index >= ToolParaIndex.INFTOOL_ID && index <= ToolParaIndex.INFTOOL_G64MODE)
            {
                return 0 == Machine.HNC_ToolSetToolPara(no, (int)index, (int)value);
            }
            else
            {
                return 0 == Machine.HNC_ToolSetToolPara(no, (int)index, (double)value);
            }
        }

        /// <summary>
        /// 保存刀库表 
        /// </summary>
        /// <returns></returns>
        public bool ToolMagSave() => 0 == Machine.HNC_ToolMagSave();

        /// <summary>
        /// 获取系统最大刀库数目
        /// </summary>
        /// <returns></returns>
        public int ToolGetMaxMagNum() => Machine.HNC_ToolGetMaxMagNum();

        /// <summary>
        /// 获取刀库数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="num"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        public bool ToolGetMagBase(MagzHeadIndex index, out int num, int no = 1)
        {
            num = 0;
            return 0 == Machine.HNC_ToolGetMagBase(no, (int)index, ref num);
        }

        /// <summary>
        /// 设置刀库数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        public bool ToolSetMagBase(MagzHeadIndex index, int value, int no = 1) => 0 == Machine.HNC_ToolSetMagBase(no, (int)index, value);

        /// <summary>
        /// 订阅刀库中刀位的刀具号
        /// </summary>
        /// <param name="potNo"></param>
        /// <param name="magNo"></param>
        public void ToolMagGetToolNo_Subscribe(int potNo, int magNo = 1) => Machine.HNC_ToolMagGetToolNo_Subscribe(magNo, potNo, false);

        /// <summary>
        /// 取消订阅刀库中刀位的刀具号
        /// </summary>
        /// <param name="potNo"></param>
        /// <param name="magNo"></param>
        public void ToolMagGetToolNo_UnSubscribe(int potNo, int magNo = 1) => Machine.HNC_ToolMagGetToolNo_Subscribe(magNo, potNo, true);

        /// <summary>
        /// 获取刀库中刀位的刀具号
        /// </summary>
        /// <param name="potNo"></param>
        /// <param name="toolNo"></param>
        /// <param name="magNo"></param>
        /// <returns></returns>
        public bool ToolMagGetToolNo(int potNo, out int toolNo, int magNo = 1)
        {
            toolNo = 0;
            return 0 == Machine.HNC_ToolMagGetToolNo(magNo, potNo, ref toolNo);
        }

        /// <summary>
        /// 设置刀库中刀位的刀具号
        /// </summary>
        /// <param name="potNo"></param>
        /// <param name="value"></param>
        /// <param name="magNo"></param>
        /// <returns></returns>
        public bool ToolMagSetToolNo(int potNo, int value, int magNo = 1) => 0 == Machine.HNC_ToolMagSetToolNo(magNo, potNo, value);

        /// <summary>
        /// 获取刀具数目 
        /// </summary>
        /// <param name="toolStartNo">刀具起始号</param>
        /// <returns>刀具数目</returns>
        public int ToolGetSysToolNum(int toolStartNo)
        {
            var toolNum = 0;
            Machine.HNC_ToolGetSysToolNum(ref toolStartNo, ref toolNum);
            return toolNum;
        }

        #endregion

        #region 坐标系 CRDS

        /// <summary>
        /// 获取坐标系数据的
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool CrdsGetValue(HncCRDS type, out string dataType, out object dataValue) => CrdsGetValue(type, out dataType, out dataValue, 0, 0, 0);

        /// <summary>
        /// 获取坐标系数据的
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <param name="crdsAx"></param>
        /// <param name="crdsCh"></param>
        /// <param name="crdsCrds"></param>
        /// <returns></returns>
        public bool CrdsGetValue(HncCRDS type, out string dataType, out object dataValue, int crdsAx = 0, int crdsCh = 0, int crdsCrds = 0)
        {
            var r = false;
            int vi = 0;
            double vd = 0;
            string vs = string.Empty;
            switch (type)
            {
                case HncCRDS.HNC_CRDS_G68_VCT:
                    r = 0 == Machine.HNC_CrdsGetValue((int)type, crdsAx, ref vd, crdsCh, crdsCrds);
                    dataType = vd.GetType().FullName;
                    dataValue = vd;
                    return r;
                default:
                    r = 0 == Machine.HNC_CrdsGetValue((int)type, crdsAx, ref vi, crdsCh, crdsCrds);
                    dataType = vi.GetType().FullName;
                    dataValue = vi;
                    return r;
            }
        }

        /// <summary>
        /// 设置坐标系数据的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataValue"></param>
        /// <param name="crdsAx"></param>
        /// <param name="crdsCh"></param>
        /// <param name="crdsCrds"></param>
        /// <returns></returns>
        public bool CrdsSetValue(HncCRDS type, object dataValue, int crdsAx = 0, int crdsCh = 0, int crdsCrds = 0)
        {
            switch (type)
            {
                case HncCRDS.HNC_CRDS_G68_VCT:
                    return 0 == Machine.HNC_CrdsSetValue((int)type, crdsAx, (double)dataValue, crdsCh, crdsCrds);
                default:
                    return 0 == Machine.HNC_CrdsSetValue((int)type, crdsAx, (int)dataValue, crdsCh, crdsCrds);
            }
        }

        /// <summary>
        /// 取系统定义的各类坐标系数目
        /// </summary>
        /// <param name="type">各类坐标系数目类型</param>
        /// <returns></returns>
        public int CrdsGetMaxNum(HncCRDS type) => Machine.HNC_CrdsGetMaxNum((int)type);

        #endregion

        #region 采样 SAMPL

        /// <summary>
        /// 获取采样通道
        /// </summary>
        /// <param name="ch">采样通道（0~31）</param>
        /// <param name="type">采样对象的数据类型</param>
        /// <param name="axis">逻辑轴号</param>
        /// <param name="offset">偏移量</param>
        /// <param name="dataLen">单个采样数据长度</param>
        /// <returns></returns>
        public bool SamplGetChannel(int ch, out HncSampleType type, out int axis, out int offset, out int dataLen)
        {
            var t = 0; axis = 0; offset = 0; dataLen = 0;
            var r = 0 == Machine.HNC_SamplGetChannel(ch, ref t, ref axis, ref offset, ref dataLen);
            type = (HncSampleType)t;
            return r;
        }

        #endregion

        #region G代码 GCode

        /// <summary>
        /// 获取机床程序ID映射表
        /// </summary>
        /// <param name="progName"></param>
        /// <returns></returns>
      public  int  HNC_FprogGetProgIdMap(ref Dictionary<uint, string> progName)
        {

            return Machine.HNC_FprogGetProgIdMap(ref progName);
        }
        /// <summary>
        /// 根据ID映射编号获取G代码名称
        /// </summary>
        /// <param name="progId"></param>
        /// <param name="progName"></param>
        /// <returns></returns>
        public int HNC_FprogGetProgById(uint progId, ref string progName)
        {
            return Machine.HNC_FprogGetProgById(progId,ref progName);
        }
        /// <summary>
        /// 根据id获取G代码路径
        /// </summary>
        /// <param name="progId"></param>
        /// <param name="progName"></param>
        /// <returns></returns>
        public int HNC_FprogGetProgPath(int progId, ref string progName)
        {
            return Machine.HNC_FprogGetProgPathByIdx(progId, ref progName);
        }
        /// <summary>
        /// 从下位机加载G代码程序
        /// </summary>
        /// <param name="filePath">路径需包括"../prog/"</param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool SysCtrlSelectProg(string filePath, int ch = 0) => 0 == Machine.HNC_SysCtrlSelectProg(ch, filePath);

        /// <summary>
        /// 重新运行停止的程序
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool SysCtrlResetProg(int ch = 0) => 0 == Machine.HNC_SysCtrlResetProg(ch);

        /// <summary>
        /// 停止通道正在运行的程序
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool SysCtrlStopProg(int ch = 0) => 0 == Machine.HNC_SysCtrlStopProg(ch);

        /// <summary>
        /// 从下位机加载G代码子程序
        /// </summary>
        /// <param name="subProgNo"></param>
        /// <param name="name"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool FprogLoadSubProg(int subProgNo, string name, int ch = 0) => 0 == Machine.HNC_FprogLoadSubProg(ch, subProgNo, name);
        /// <summary>
        /// 上载程序到机床，并加载
        /// </summary>
        /// <param name="remotepath">h/lnc8/prog/</param>
        /// <param name="localpath">C:\\</param>
        /// <param name="name">O0001.nc</param>
        /// <param name="remoteip"></param>
        /// <returns>true成功，false失败</returns>
        public bool SysCtrlUpLoadFile(string remotepath, string localpath, string remoteip)
        {
            string FTPAddress = remoteip;//My.Repo.GetSingle<Equipment>(p => p.Type == EquipmentType.EDA).IP;
            FileInfo f = new FileInfo(localpath);
            string FileName = f.Name;
            string ftpRemotePath = remotepath;
            //string LocalPath = EDADesPath; //待上传文件
            //FileInfo f = new FileInfo(LocalPath);
            //string FileName = f.Name;
            //string ftpRemotePath = "/Work File/";//"ftp://192.168.1.200/h/lnc8/prog/OMDI"
            string FTPPath = "ftp://" + FTPAddress + ftpRemotePath + FileName; //上传到ftp路径,如ftp://***.***.***.**:21/home/test/test.txt
                                                                               //实现文件传输协议 (FTP) 客户端

            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTPPath));
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential("root", "111111"); //设置通信凭据
            reqFtp.KeepAlive = false; //请求完成后关闭ftp连接
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
            reqFtp.ContentLength = f.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            //读本地文件数据并上传
            FileStream fs = f.OpenRead();
            try
            {
                Stream strm = reqFtp.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                string path1 = "../prog/" + FileName;
                Machine.HNC_SysCtrlSelectProg(0, path1);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }


        }
        #endregion

        #region Sys Sys

        /// <summary>
        /// 系统升级
        /// </summary>
        /// <param name="flag">HNCUPDATE</param>
        /// <param name="pathName"></param>
        public void SysUpdate(int flag, string pathName) => Machine.HNC_SysUpdate(flag, pathName);

        /// <summary>
        /// 系统备份
        /// </summary>
        /// <param name="flag">HNCUPDATE</param>
        /// <param name="pathName"></param>
        public void SysBackup(int flag, string pathName) => Machine.HNC_SysBackup(flag, pathName);

        /// <summary>
        /// 复位
        /// </summary>
        public void SysReset() => Machine.HNC_SysReset();

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="type">特获取的系统配置类型</param>
        /// <param name="config">系统配置</param>
        /// <returns></returns>
        public bool SysCtrlGetConfig(int type, out string config)
        {
            config = string.Empty;
            return 0 == Machine.HNC_SysCtrlGetConfig(type, ref config);
        }

        /// <summary>
        /// 设置校验状态
        /// </summary>
        /// <param name="stat">校验状态</param>
        /// <param name="ch">通道号</param>
        /// <returns></returns>
        public bool VerifySetStatus(bool stat, int ch = 0) => 0 == Machine.HNC_VerifySetStatus(ch, stat ? 1 : 0);

        #endregion

        #region MDI

        /// <summary>
        /// MDI开启
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool MdiOpen(int ch = 0) => IsCycling() ? false : IsEmergency() ? false : IsReseting() ? false : 1 == Machine.HNC_MdiOpen(ch);

        /// <summary>
        /// 设置MDI的文本
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool MdiSetContent(string content) => IsCycling() ? false : 0 == Machine.HNC_MdiSetContent(content);

        /// <summary>
        /// MDI关闭
        /// </summary>
        public bool MdiClose() => 1 == Machine.HNC_MdiClose();

        #endregion


        #region 状态 State

     
        /// <summary>
        /// 加工
        /// </summary>
        public bool IsCycling() => RegGetBit(HncRegType.REG_TYPE_F, 2560, 5);

        /// <summary>
        /// 急停
        /// </summary>
        public bool IsEmergency() => ChannelGetValue(HncChannel.HNC_CHAN_IS_ESTOP, out string t, out object v) ? 1 == (int) v : false;
        /// <summary>
        /// 复位
        /// </summary>
        public bool IsReseting() => ChannelGetValue(HncChannel.HNC_CHAN_IS_RESETTING, out string t, out object v) ? 1 == (int)v : false;
        /// <summary>
        /// 运行
        /// </summary>
        public bool IsRuning() => ChannelGetValue(HncChannel.HNC_CHAN_IS_RUNNING, out string t, out object v) ? 1 == (int)v : false;
      

        #endregion

    }
}
