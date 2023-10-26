﻿using System;
using System.Runtime.InteropServices;

namespace HNCAPI_INTERFACE
{
    public enum LadCmd
    {
        LAD_RELOAD_DIT = 0, //	重新加载
        LAD_SAVE_DIT,       //	保存梯图文件
        LAD_DISABLE_BLK,    //	强制禁止元件导通
        LAD_ENABLE_BLK,     //	强制允许元件导通
        LAD_RESTORE_BLK,    //	恢复元件强制状态
        LAD_ADD_REGLOCK,    //	增加一组锁定寄存器
        LAD_DEL_REGLOCK,    //	删除一组锁定寄存器
        LAD_ADD_IOCONTRAST, //	增加一组IO对照表
        LAD_DEL_IOCONTRAST, //	删除一组IO对照表
        LAD_DEL_SYMBOL,     //	删除一组符号表
        LAD_SET_PLC_STOP,   //	停止PLC运行
        LAD_ADD_SUB,        //	增加一个子程序
        LAD_DEL_SUB,        //	删除一个子程序
        LAD_LOAD_CFG,       //	加载配置
        LAD_SAVE_CFG,       //	保存配置

        LAD_GET_COPY_NUM = 50,      //	获取剪切板中梯图行数
        LAD_GET_CHANGE_FLAG,        //	获取梯图修改标志
        LAD_GET_REGLOCK_NUM,        //	获取锁定寄存器数目
        LAD_GET_IO_CONTRAST_NUM,    //	获取IO对照表数目
        LAD_GET_SYMBOL_NUM,         //	获取符号表数目
        LAD_GET_DIT_VERIFY,         //	获取DIT文件校验码
        LAD_GET_PLC_STATE,          //	获取PLC运行状态
        LAD_GET_BLK_REAL_STATE,     //	获取元件状态
        LAD_GET_REG_CACHE_STATUS,   //	获取寄存器锁存数据
        LAD_GET_BLK_FORCE_STATE,    //	获取元件强制状态
        LAD_GET_TIMER_TYPE,         //	获取计时器类型
        LAD_GET_COUNTER_TYPE,       //	获取计数器类型
        LAD_GET_REG_SYMBOL_INDEX,   //	获取寄存器在符号表中的索引

        LAD_CMD_TOTAL
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct SLadReg
    {
        [MarshalAs(UnmanagedType.I4)]
        public Int32 index;
        [MarshalAs(UnmanagedType.I1)]
        public SByte reg_type;
        [MarshalAs(UnmanagedType.I1)]
        public SByte bit;
        [MarshalAs(UnmanagedType.I1)]
        public SByte reserve1;
        [MarshalAs(UnmanagedType.I1)]
        public SByte reserve2;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct SLadRegLock
    {
        [MarshalAs(UnmanagedType.Struct)]
        public SLadReg reg;
        [MarshalAs(UnmanagedType.I1)]
        public SByte lockflag;
        [MarshalAs(UnmanagedType.I1)]
        public SByte newflag;
        [MarshalAs(UnmanagedType.I1)]
        public SByte format;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public SByte[] reserve;
        [MarshalAs(UnmanagedType.I4)]
        public Int32 newval;
    }
}
