using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HNC.MOHRSS.Model;
using HNC.MOHRSS.Common;
using System.Data.SqlClient;
using System.Data;
namespace SCADA
{
    /// <summary>
    /// IP配置
    /// </summary>
    public class FIpSetFile
    {
        public int id;
        public string Name;
        public string IP;
    }
    /// <summary>
    /// 车床数据
    /// </summary>
    public class Flathedata
    {
        public EnumLatheChuckState Chuck;
        public EnumLatheDoorState Door;
        public EnumLatheState State;
        public double SizeX;
        public double SizeZ;
        public double SpindleSpeed;
        public double Feedrate;
        public double Load_Current_C;
        public double Load_Current_X;
        public double Load_Current_Z;
        public string WorkpieceProgram;
        public Int32 instructionsCount;
        public Guid StorageBinId;
        public Int32 ToolSN;
    }
    /// <summary>
    /// 加工中心
    /// </summary>
    public class FCNCdata
    {

        // 摘要:
        //     平口钳卡盘
        public EnumMachiningCenterChuckState Chuck;

        //     快换卡盘
        public EnumMachiningCenterChuckState Chuck2;

        //     开关门
        public EnumMachiningCenterDoorState Door;

        public EnumMachiningCenterState State;
        public double SizeX;
        public double SizeY;
        public double SizeZ;
        public double Load_Current_C;
        public double Load_Current_X;
        public double Load_Current_Z;
        public double Load_Current_Y;
        public double SpindleSpeed;
        public double Feedrate;
        public string WorkpieceProgram;
        public Int32 instructionsCount;
        public Guid StorageBinId;
        public Int32 ToolSN;
    }
    /// <summary>
    /// AGV数据
    /// </summary>
    public class FAGVdata
    {
        public Guid MachineId;//机器码
        public bool LineStoreBit;//	线边库状态(0无料盘1有料盘）
        public bool FinStoreBit;//	成品库状态(0无料盘1有料盘）
        public bool RawStore;//	毛坯库状态(0无料盘1有料盘)
        public bool Task_Raw_Line;//	毛坯库到线边库
        public bool Task_Fin_Line;//	成品库到线边库
        public bool Task_Line_Raw;//	从线边库到毛坯库
        public bool Task_Line_Fine;//	从线边库到成品库
        public Int32 AGV_Task_State;//0:初始，4:任务创建成功，6:任务创建失败，16:成功完成，24:错误完成
        public Int32 AGV_State;//AGV状态0：维护中、充电中  1：就绪 2：忙碌 4：故障 10：API接口错误  11：急停中
        public Int32 AGV_Vol;//电量，百分比
        public Int32 AGV_Beat;//心跳每秒加1 到1000后回1
    }
    /// <summary>
    /// 工单数据
    /// </summary>
    public class FOrderdata
    {
        public string UserId;
        public Guid StorageBinId;
        public Int32 WorkpieceCategoryId;
        public Int32 Operation;
    }
    public class FRobortdata
    {
        public EnumRobotState State;
        public EnumRobotMode Mode;
        public bool OriginalPoint;
        public double SiteJ1;
        public double SiteJ2;
        public double SiteJ3;
        public double SiteJ4;
        public double SiteJ5;
        public double SiteJ6;
        public double SiteJ7;
    }
    public class FStoragebin
    {
        public Int32 SN;//仓位编号
        public Guid WorkpieceCategoryId;//类型
        public EnumBinState State;//状态
        public EnumSession Session;//场次
    }
    public class FUserdata
    {
        public string Username;//
        public string Password;//
    }
    public class FWorkpiececategorydata
    {
        public string Name;//
        public string Description;//
    }
    public class FGaugedata
    {
        public Guid UserId;//
        public Guid StorageBinId;//
        public string Result;//合格不合格
        public DateTime UpdatedTime;
    }
    public class FGaugedetaildata
    {
        public Guid GaugeId;//
        public double ReferenceValue;//
        public double ActualValue;//
        public Int32 SN;
    }
    public class FCutter
    {
        public Guid MachineId;//机器码
        public double Length;//长度
        public double LengthAbrasion;//长度补偿
        public double Radius;//半径
        public double RadiusAbrasion;//半径补偿
        public Int32 SN;//刀具编号
    }

    public class FGongxu
    {
        public string KEYID;//主键字段
        public string CVOLID;//文卷ID
        public string TH;//图号
        public string LJMC;//零件名称
        public string CL;//材料
        public string MTJS;//每台件数
        public string MPJS;//每批件数
        public string GXH;//工序号
        public string AZ;//安装
        public string GBH;//工步号
        public string GXMC; //工序内容
        public string GXNR; //工序内容
        public string TSJGLJS;//同时加工零件数
        public string JGFS;//加工方式
        public string ZZZS;//主轴转读
        public string JJSD;//进给转速
        public string QXSD;//切削深度
        public string JGYL;//加工余量
        public string JCMC;//机床名称
        public string JJMC;//夹具名称
        public string DJMC;//刀具名称
        public string DJZJ;//机床名称
        public string WJ;//外径
        public string NJ;//内径
        public string GSDE;//工时定额
        public string CXMC;//程序名称
    }
    public class FBOMList
    {
        public string KEYID;//主键
        public string CVOLID;//问卷id
        public string BOMTYPE;//BOM类型
        public string PRNTNODEID;//父ID
        public string NODEID;//子ID
        public string TH;//图号
        public string CSUFFIX;//代号后缀
        public string LJMC;//零件名称
        public string CRIV;//版本号
        public string CNUMBER;//数量
        public string CL;//材料
        public string GG;//规格
        public string XM;//姓名
        public string SB;//设备
        public string XMU;//项目
        public string TZBL;//图纸比例
        public string BZ;//备注
    }

    public class FEquipment
    {
        /// <summary>
        /// IP地址
        /// </summary>
        //[Display(Name = "IP地址")]
        public string IP { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        //[Display(Name = "端口号")]
        public ushort Port { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        //[Display(Name = "设备类型")]
        //[Required]
        public EquipmentType Type { get; set; }

        /// <summary>
        /// 设备是否在线
        /// </summary>
        public bool IsConnect { get; set; }
    }


    /// <summary>
    /// 设备类型
    /// </summary>
    public enum EquipmentType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 车床
        /// </summary>
        Lath,
        /// <summary>
        /// 加工中心
        /// </summary>
        CNC,
        /// <summary>
        /// 机器人
        /// </summary>
        Robot,
        /// <summary>
        /// 冲压机
        /// </summary>
        Punch,
        /// <summary>
        /// 火花机
        /// </summary>
        EDA,
        /// <summary>
        /// PLC
        /// </summary>
        PLC,
        /// <summary>
        /// 三坐标
        /// </summary>
        Coord,
        /// <summary>
        /// 录像机
        /// </summary>
        Video,
        /// <summary>
        /// MES电脑
        /// </summary>
        ComputerMES,
        /// <summary>
        /// CAD电脑
        /// </summary>
        ComputerCAD,
        /// <summary>
        /// PLC电脑
        /// </summary>
        ComputerPLC,
    }
}




