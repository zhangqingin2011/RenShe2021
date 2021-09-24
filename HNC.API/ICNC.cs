
using System;
using HNCAPI_INTERFACE;
using System.Collections.Generic;

namespace HNC.API
{
    /// <summary>
    /// 数控系统基类
    /// </summary>
    public abstract class CNCBase : ICNC
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; protected set; }
        
        /// <summary>
        /// 端口号
        /// </summary>
        public ushort Port { get; protected set; }
        /// <summary>
        /// 机床类型
        /// </summary>
        public CNCType cnctype { get;  set; }
        /// <summary>
        /// CF卡SN号
        /// </summary>
        public string MachineSN { get;  set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public string  EquipmentState { get;  set; }
        /// <summary>
        /// 机床上的物料好
        /// </summary>
        public int MagNum { get; set; }
        /// <summary>
        /// 主轴速度
        /// </summary>
        public double SpindleSpeed { get; set; }
        /// <summary>
        /// 进给速度
        /// </summary>
        public double FeedRate { get; set; }
        /// <summary>
        /// 当前运行G代码名称
        /// </summary>
        public string GProgram { get; set; }
        /// <summary>
        /// 当前刀具号
        /// </summary>
        public int InstructionsTool { get; set; }
        /// <summary>
        /// 当前运行指令行数
        /// </summary>
        public int InstructionsCount { get; set; }
        /// <summary>
        /// 机床正在加工的仓位编号
        /// </summary>
        public short strogeId { get; set; }
        /// <summary>
        /// 卡盘1开关状态
        /// </summary>
        public bool Chuck1 { get; set; }
        /// <summary>
        /// 卡盘2开关状态
        /// </summary>
        public bool Chuck2 { get; set; }
        /// <summary>
        /// 安全门开关
        /// </summary>
        public bool Door { get; set; }
        /// <summary>
        /// X轴位置
        /// </summary>
        public double SiteX { get; set; }
        /// <summary>
        /// Y轴位置
        /// </summary>
        public double SiteY { get; set; }
        /// <summary>
        /// Z轴位置
        /// </summary>
        public double SiteZ { get; set; }
        /// <summary>
        /// C轴位置
        /// </summary>
        public double SiteC { get; set; }
        /// <summary>
        /// X轴指令位置
        /// </summary>
        public double SiteXF { get; set; }
        /// <summary>
        /// Y轴指令位置
        /// </summary>
        public double SiteYF { get; set; }
        /// <summary>
        /// Z轴指令位置
        /// </summary>
        public double SiteZF { get; set; }
        /// <summary>
        /// C轴指令位置
        /// </summary>
        public double SiteCF { get; set; }
        /// <summary>
        /// X负载
        /// </summary>
        public double LoadX { get; set; }
        /// <summary>
        /// Y负载
        /// </summary>
        public double LoadY { get; set; }
        /// <summary>
        /// Z负载
        /// </summary>
        public double LoadZ { get; set; }
        /// <summary>
        /// C负载
        /// </summary>
        public double LoadC { get; set; }
        /// <summary>
        /// 刀具数量
        /// </summary>
        public Int32 ToolNum { get; set; }
        /// <summary>
        ///主轴修调百分比
        /// </summary>
        public double SpdlRate { get; set; }
        /// <summary>
        /// 进给百分比
        /// </summary>
        public double SpeedRate { get; set; }
        /// <summary>
        /// 快移百分比
        /// </summary>
        public double RapidRate { get; set; }
        /// <summary>
        /// 机床的刀具数据
        /// </summary>
        public ToolDataConfig[] TOOLData = new ToolDataConfig[100];
        /// <summary>
        /// 刀补补偿修改值
        /// </summary>
        public ToolDataConfigAdd[] TOOLDataChange = new ToolDataConfigAdd[100];
        /// <summary>
        /// Y寄存器480
        /// </summary>
        public Int32 CNCButtonY480 { get; set; }
        /// <summary>
        /// Y寄存器481
        /// </summary>
        public Int32 CNCButtonY481 { get; set; }
        /// <summary>
        /// Y寄存器481
        /// </summary>
        public Int32 CNCButtonY482 { get; set; }  
        /// <summary>
        /// Y寄存器481
        /// </summary>
        public Int32 CNCButtonY483{ get; set; }
        /// <summary>
        /// Y寄存器481
        /// </summary>
        public Int32 CNCButtonY484 { get; set; }
        /// <summary>
        /// Y寄存器481
        /// </summary>
        public Int32 CNCButtonY485 { get; set; }
        /// <summary>
        /// Y寄存器481
        /// </summary>
        public Int32 CNCButtonY486 { get; set; }

        /// <summary>
        /// R寄存器29
        /// </summary>
        public Int32 CNCButtonR29{ get; set; }

        public Dictionary<int, string> Alarms{ get; set; }
    ///// <summary>
    ///// 长度补偿增量
    ///// </summary>
    //public double[] LengthCompAdd { get; set; }
    ///// <summary>
    ///// 半径补偿增量
    ///// </summary>
    //public double[] RadiusCompAdd { get; set; }
    ///// <summary>
    ///// MES 系统下发刀补标志
    ///// </summary>
    //public bool[] CompSentFlage { get; set; }
    /// <summary>
    /// 获取测量数据标识
    ///// </summary>
    //public bool GetMeterValueFlage;
    /// <summary>
    /// 测头测量值
    /// </summary>
    public double[] MeterValue = new Double[50];

        /// <summary>
        /// 当前运行程序内容
        /// </summary>
        public string[] CurProgContentLine { get; set; }
        

        /// <summary>
        /// 下发机床程序的路径
        /// </summary>
        public string SetProgPath;

        /// <summary>
        /// 下发机床程序成功
        /// </summary>
        public TaskState SetProgFlage;

        /// <summary>
        /// 刀补下发任务
        /// </summary>
        public TaskState SetToolTaskFlage;

     
        /// <summary>
        /// IP:Port
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{IP}:{Port}";

        /// <summary>
        /// 查询连接
        /// </summary>
        /// <returns>操作成功则返回TRUE</returns>
        public abstract bool IsConnected();

        /// <summary>
        /// 查询信号
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号
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
        /// <returns>信号值为1则返回TRUE</returns>
        public bool RegGetBit(HncRegType type, int index, int bit) => RegGetValue(type, index, out int value) ? ((value >> bit) & 1) == 1 : false;

        /// <summary>
        /// 设置信号
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="bit">Bit</param>
        /// <returns>操作成功则返回TRUE</returns>
        public abstract bool RegSetBit(HncRegType type, int index, int bit);

        /// <summary>
        /// 清除信号
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="bit">Bit</param>
        /// <returns>操作成功则返回TRUE</returns>
        public abstract bool RegClearBit(HncRegType type, int index, int bit);

        /// <summary>
        /// 触发信号（如果信号存在则清除）
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号
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
        /// <returns>如果存在且清除成功则返回TRUE</returns>
        public bool RegTrigger(HncRegType type, int index, int bit) => RegGetBit(type, index, bit) && RegClearBit(type, index, bit);

        /// <summary>
        /// 获取寄存器的值
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="value">寄存器值</param>
        /// <returns>操作成功则返回TRUE</returns>
        public abstract bool RegGetValue(HncRegType type, int index, out int value);

        /// <summary>
        /// 获取寄存器的值
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <returns>寄存器值</returns>
        public (bool, int) RegGetValue(HncRegType type, int index) => (RegGetValue(type, index, out int value), value);

        /// <summary>
        /// 获取寄存器的值
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="value">寄存器值</param>
        /// <returns>操作成功则返回TRUE</returns>
        public abstract bool RegSetValue(HncRegType type, int index, int value);

        /// <summary>
        /// 系统版本信息
        /// </summary>
        public string NCKVersion { get; set; }
        public string DRVVersion { get; set; }
        public string PLCVersion { get; set; }
        public string NCVersion { get; set; }
    
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICNC
    {
        /// <summary>
        /// IP地址
        /// </summary>
        string IP { get; }

        /// <summary>
        /// 端口号
        /// </summary>
        ushort Port { get; }

        /// <summary>
        /// CF卡SN号
        /// </summary>
        string MachineSN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string ToString();

        /// <summary>
        /// 查询连接
        /// </summary>
        /// <returns>操作成功则返回TRUE</returns>
        bool IsConnected();

        /// <summary>
        /// 查询信号
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="bit">Bit</param>
        /// <returns>信号值为1则返回TRUE</returns>
        bool RegGetBit(HncRegType type, int index, int bit);

        /// <summary>
        /// 设置信号
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="bit">Bit</param>
        /// <returns>操作成功则返回TRUE</returns>
        bool RegSetBit(HncRegType type, int index, int bit);

        /// <summary>
        /// 清除信号
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="bit">Bit</param>
        /// <returns>操作成功则返回TRUE</returns>
        bool RegClearBit(HncRegType type, int index, int bit);

        /// <summary>
        /// 触发信号
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="bit">Bit</param>
        /// <returns>如果存在且清除成功则返回TRUE</returns>
        bool RegTrigger(HncRegType type, int index, int bit);

        /// <summary>
        /// 获取寄存器的值
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="value">寄存器值</param>
        /// <returns>操作成功则返回TRUE</returns>
        bool RegGetValue(HncRegType type, int index, out int value);

        /// <summary>
        /// 获取寄存器的值
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <returns>寄存器值</returns>
        (bool, int) RegGetValue(HncRegType type, int index);

        /// <summary>
        /// 获取寄存器的值
        /// </summary>
        /// <param name="type">寄存器类型</param>
        /// <param name="index">寄存器序号</param>
        /// <param name="value">寄存器值</param>
        /// <returns>操作成功则返回TRUE</returns>
        bool RegSetValue(HncRegType type, int index, int value);
    }
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// 无任务
        /// </summary>
        NULL = 0,
        /// <summary>
        /// 任务未开始
        /// </summary>
        NotStatrt = 1,
        /// <summary>
        /// 任务启动
        /// </summary>
        Start = 2,
        /// <summary>
        /// 任务成功
        /// </summary>
        Succed= 3,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 4,
    }
    /// <summary>
    /// 机床类型
    /// </summary>
    public enum CNCType
    {    ///车床
        Lathe = 0,
        ///加工中心
        CNC = 1,
    };
    /// <summary>
    /// 刀具修改值
    /// </summary>
    public struct ToolDataConfigAdd
    {
        /// <summary>
        /// 刀具ID号
        /// </summary>
        public Int32 ToolID;
        /// <summary>
        /// 刀具修改标识，false本刀具不修改，true本刀具要修改
        /// </summary>
        public bool ToolChangeflag;
        /// <summary>
        /// 刀具长度补偿
        /// </summary>
        public double LengthCompAdd;
        /// <summary>
        /// 刀具半径补偿
        /// </summary>
        public double RadiusCompAdd;

    };
    /// <summary>
    /// 刀具数据结构
    /// </summary>
    public struct ToolDataConfig
    {
        /// <summary>
        /// 刀具ID号
        /// </summary>
        public Int32 ToolID;
        /// <summary>
        /// 刀具长度
        /// </summary>
        public double Length;
        /// <summary>
        /// 刀具半径
        /// </summary>
        public double Radius ;
        /// <summary>
        /// 刀具长度补偿
        /// </summary>
        public double LengthComp;
        /// <summary>
        /// 刀具半径补偿
        /// </summary>
        public double RadiusComp ;

        /// <summary>
        /// Z偏置
        /// </summary>
        public double Zlength;
    }
}