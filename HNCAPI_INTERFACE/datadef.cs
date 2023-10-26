﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace HNCAPI_INTERFACE
{
    public enum SysVarDef
    {
        VAR_DECODER_SIZE,
        VAR_CHANCTRL_SIZE,
        VAR_AXISCTRL_SIZE,
        VAR_SMXCTRL_SIZE,
        VAR_SYS_CRDS_CHAN = 4,//Monitor正在设置的坐标系所属通道[最多4个Monitor]
        VAR_SYS_DISP_CHAN = 8,//Monitor的显示通道记录[最多4个]
        VAR_DECODER_BAK_SIZE = 12,
        //	VAR_SYS_ACT_CHAN=12,//8个工位的活动通道
        //VAR_SYS_MUTEX_FLAG=20,//系统全局锁4*32=128个
        VAR_SYS_SEMPHORE = 20,//系统全局信号量40个【16位】

        VAR_SYS_G5EX_IDCH = 40, //60个扩展工件坐标系的ID及通道号,低16位为轴掩码，高16位为通道号
        VAR_SYS_G5EX_ZERO = 100,    //60个扩展工件坐标系零点60*18[64位]=1080

        VAR_SYS_G68_POINT = 1180, //12*20 G68的两个辅助点
        VAR_SYS_G68_ZERO = 1420, //6*20 [Bit64]
        VAR_SYS_G68_VCT = 1540, //18*20个[fBit64] x y z三个轴的向量

        //MDI 信息
        VAR_SYS_MDI_MODE = 1900, //MDI模式:mode ret
        VAR_SYS_MDI_ROW,        //MDI输入程序行数
        VAR_SYS_MDI_CHAN,       //MDI运行的通道
        VAR_SYS_SMX_NUM0,       //静态耦合轴个数
        VAR_SYS_SMX_NUM,        //总耦合轴个数
        VAR_DCDVAR_OFFSET,      //解释器中的局部变量偏移量

        VAR_SYS_ALARM_COPY = 1910,  //1910~1917
        VAR_SYS_ALARM_FLAG = 1918,  //1918~1925
        VAR_SYS_NOTE_COPY = 1926,   //1926~1937
        VAR_SYS_NOTE_FLAG = 1938,   //1938~1949

        VAR_SYS_CHG_WCS_N = 1950,   //进给保持时，修改的工件坐标系数
        VAR_SYS_CHG_WCS_I,  //1951~1958，进给保持时，修改的工件坐标系所属的通道号及工件坐标编号
        VAR_SYS_CHG_TOOL_N = 1959,  //进给保持时，修改的刀具个数
        VAR_SYS_CHG_TOOL_I = 1960,  //1960~1969 进给保持时，修改的刀具编号 
        VAR_SYS_G5X_TEMP = 1970,        //1970~2113 八个进给保持时的临时工件坐标系 8*18 = 144

        VAR_SYS_YMD = 2114,//记录系统时间 年+月+日
        VAR_SYS_HMS,//系统时间，小时+分钟+秒
        VAR_SYS_POWER_OFF_PERIOD, // 系统上次关机到本次开机的时间差，单位：秒，未读取到时间时为0

        //列表信息
        VAR_SYS_TAB_NUM = 2144, //列表个数 50个列表的信息
        VAR_SYS_TAB_COL,        //第一个列表的列数及主列数
        VAR_SYS_TAB_OFF,        //第一个列表数据的起始地址偏移
        //	VAR_SYS_TAB_COL,		//第n个列表的列数及主列数
        //	VAR_SYS_TAB_OFF,		//2145 ~ 2244 第n个列表数据的起始地址偏移

        VAR_SYS_G31_8 = 2245,       //G31.8测量点缓存 #42245 ~ #42499	
        VAR_SYS_DEBUG_INF = 2500,   //调试信息42500 ~ 43499 

        VAR_SMPL_PERIOD = 3500,     //采样周期设置,为插补周期的自然数倍，16个采样通道共用
        VAR_SMPL_LMT,               //采样截止条件，采样通道共用
        VAR_SMPL_IDX,               //采样通道写指针，采样通道共用
        VAR_SMPL_READ_PT,           //采样通道读指针，采样通道共用
        VAR_SMPL_TYPE = 3504,       //3504~3535 采样通道类型，共32个
        VAR_SMPL_AXIS = 3536,           //3536~3567	采样通道轴号，共32个
        VAR_SMPL_OFFSET = 3568,     //3568~3599 采样通道偏移量，共32个
        VAR_SMPL_DATA_LEN = 3600,       //3600~3631 采样通道数据长度，共32个
        VAR_SMPL_USED_CHAN_NUM = 3632,  //已使用的采样通道数
        VAR_EVENT_SUBSCRIBED,       //是否订阅事件数据
        VAR_ALARM_SUBSCRIBED,		//是否订阅报警数据

        VAR_TOOLMEAS_CALIBATION_TYPE = 3700,    // 激光式测量(Bit32)   3700~3749刀具测量使用
        VAR_TOOLMEAS_BLOW_OPEN,                 // 吹气开M指令(Bit32)
        VAR_TOOLMEAS_BLOW_CLOSE,                // 吹气关M指令(Bit32)
        VAR_TOOLMEAS_LASER_OPEN,                // 激光开M指令(Bit32)
        VAR_TOOLMEAS_LASER_CLOSE,               // 激光关M指令(Bit32)
        VAR_TOOLMEAS_LASER_DOOR_OPEN,           // 激光门开M指令(Bit32)
        VAR_TOOLMEAS_LASER_DOOR_CLOSE,          // 激光门关M指令(Bit32)
        VAR_TOOLMEAS_M_NUM,                     // 测量次数(Bit32)
        VAR_TOOLMEAS_SAVE_HEIGTH,               // 安全距离高度(Bit32*100000)
        VAR_TOOLMEAS_M_F2,                      // 测量速度(Bit32*100000)
        VAR_TOOLMEAS_M_F3,                      // 触发速度(Bit32*100000)
        VAR_TOOLMEAS_M_S,                       // 主轴转速(Bit32*100000)
        VAR_TOOLMEAS_M_THINKNESS,               // 量仪厚度(Bit32*100000)
        VAR_STARTNO,                            // 起始刀具号(Bit32)

        VAR_TOOLMEAR_T_F1,                      //车床测量速度(Bit32*100000)
        VAR_TOOLMEAR_T_F2,                      //车床接触速度(Bit32*100000)
        VAR_TOOLMEAR_T_LENGTH,                  //车床量仪长度(Bit32*100000)
        VAR_TOOLMEAR_T_WIDTH,                   //车床量仪宽度(Bit32*100000)
        VAR_TOOLMEAR_T_NUM,                     //车床测量次数(Bit32)
        VAR_TOOLMEAS_M_BASE_TOOL_LENGTH,        //铣床标准刀具长度(Bit32*100000)

        VAR_TOOLMEAS_T_REF_DIR_X = 3730,        //标刀方向X	
        VAR_TOOLMEAS_T_REF_DIR_Z,               //标刀方向Z

        VAR_G12_POLOR_LEFTRIGHT = 3740, //极点位置，在极点左边还是右边
        VAR_G12_POLOR_TYPE, //极点类型
        VAR_RIGHT_CONFIG = 3742, // 3742~3748 当前权限mcp面板按钮使能

        VAR_SYS_TEACH_IN = 5000,    //示教记录 #45000 ~ #49999	

        VAR_SYS_SPDL_SYNC_ERR = 6000,  //变频器攻丝时的同步误差

        VAR_SYS_TOTAL = 10000 //10000
    }
}
