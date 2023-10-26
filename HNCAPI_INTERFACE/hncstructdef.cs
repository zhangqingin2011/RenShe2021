﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HNCAPI_INTERFACE
{
    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi, Pack = 4)]
    public struct SHncData
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I4)]
        public Int32 i;
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.R8)]
        public Double f;
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 n;    // 变量偏移地址
    }

    // 系统用全局变量、表达式运算的联合数据类型
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct SDataUnion
    {
        public Byte type;
        public Byte g90;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 1)]
        public SHncData v;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct nctime_t
    {
        public Int32 second;    // seconds - [0,59]
        public Int32 minute;    // minutes - [0,59]
        public Int32 hour;  // hours   - [0,23]
        public Int32 hsecond; /* hundredths of seconds */
        public Int32 day;   // [1,31]
        public Int32 month; // [0,11] (January = 0)
        public Int32 year;  // (current year minus 1900)
        public Int32 wday;  // Day of week, [0,6] (Sunday = 0)
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct SEventElement
    {
        [MarshalAs(UnmanagedType.I2)]
        public Int16 src;// 事件来源
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 code;// 事件代码
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public Byte[] buf;
    }

    public class SDataProperty
    {
        public const Int32 PARA_PROP_DATA_LEN = 68;
        public const Int32 PARA_BYTE_ARRAY_LEN = 8;
        public const Int32 PARA_STRING_LEN = 64;
        public const Int32 PARA_VALUE_START_INDEX = 4;

        public SByte type;
        public Int32 integer;
        public Double real;
        public String str;
        public Byte[] array;

        public override String ToString()
        {
            String s = String.Empty;
            switch (type)
            {
                case (SByte)HNCDATATYPE.DTYPE_INTEGER:
                case (SByte)HNCDATATYPE.DTYPE_UINT:
                case (SByte)HNCDATATYPE.DTYPE_BOOL:
                    s = integer.ToString();
                    break;
                case (SByte)HNCDATATYPE.DTYPE_FLOAT:
                    s = real.ToString();
                    break;
                case (SByte)HNCDATATYPE.DTYPE_STRING:      //字符串直接返回0
                    s = str;
                    break;
                case (SByte)HNCDATATYPE.DTYPE_BYTE:
                    for (Int32 i = 0; i < PARA_BYTE_ARRAY_LEN; i++)
                    {
                        SByte b = (SByte)array[i];
                        if (b < 0)
                        {
                            continue;
                        }
                        s += b.ToString();
                        s += " ";
                    }
                    break;
                case (SByte)HNCDATATYPE.DTYPE_HEX4:
                    s = "0x" + integer.ToString("X");
                    break;
                default:
                    break;
            }
            return s;
        }

        public SDataProperty()
        {
            type = 0;
            real = 0;
            str = String.Empty;
            array = new Byte[8];
        }
    }

    public class SParmProperty
    {
        public Int32 id;
        public String name;
        public Int32 access;
        public Int32 actType;
        public Int32 storeType;
        public String defaltVal;
        public String minVal;
        public String maxVal;
        public String value;

        public SParmProperty()
        {
            id = 0;
            access = 0;
            actType = 0;
            storeType = 0;
            name = String.Empty;
            defaltVal = String.Empty;
            minVal = String.Empty;
            maxVal = String.Empty;
            value = String.Empty;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct SAxisVals
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string name;         // 轴名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string svVer;        // 伺服驱动版本
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string svType;       // 伺服类型
        public Int32 type;          // 轴类型
        public Int32 chanNo;        // 获取通道号
        public Int32 chAxisNo;      // 获取在通道中的轴号
        public Int32 leadAxisNo;    // 获取引导轴
        public Int32 actPulse;      // 实际脉冲位置
        public Int32 cmdPulse;      // 指令脉冲位置
        public Int32 encCntr;       // 电机位置
        public Int32 isHomed;       // 回零完成
        public Int32 zSpace1;       // Z脉间距1
        public Int32 zSpace2;       // Z脉间距2
        public Double actPos;       // 机床实际位置
        public Double cmdPos;       // 机床指令位置
        public Double actWcsPos;    // 工件实际位置
        public Double cmdWcsPos;    // 工件指令位置
        public Double actRcsPos;    // 相对实际位置
        public Double cmdRcsPos;    // 相对指令位置
        public Double progPos;      // 编程位置
        public Double cmdVel;       // 指令速度
        public Double actVel;       // 实际速度
        public Double leftToGo;     // 剩余进给
        public Double wcsZero;      // 工件零点 
        public Double wheelOff;     // 手轮中断偏移量
        public Double followErr;    // 跟踪误差
        public Double synErr;       // 同步误差 
        public Double compVal;      // 轴补偿值
        public Double zDist;        // Z脉冲偏移
        public Double relZero;      // 相对零点
        public Double motorRev;     // 电机转速
        public Double loadCurrent;  // 负载电流
        public Double ratedCurrent; // 额定电流
        public Double waveFreq;     // 波形频率
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct SName
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string name;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct SChanVals
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string name;
        public Int32 macType;
        public Int32 axesMask;
        public Int32 axesMask1;
        public Int32 cmdType;
        public Double cmdFeedRate;
        public Double actFeedRate;
        public Double progFeedRate;
        public Int32 feedOverride;
        public Int32 rapidOverride;
        public Int32 mCode;
        public Int32 tCode;
        public Int32 tOffs;
        public Int32 toolUse;
        public Int32 toolRdy;
        public Int32 chanMode;
        public Int32 isMdi;
        public Int32 isCycle;
        public Int32 isHold;
        public Int32 isProgSel;
        public Int32 isProgEnd;
        public Int32 isThreading;
        public Int32 isRigid;
        public Int32 isRewinded;
        public Int32 isEstop;
        public Int32 isReseting;
        public Int32 isRunning;
        public Int32 isHoming;
        public Int32 isMoving;
        public Int32 diameter;
        public Int32 isVerify;
        public Int32 runRow;
        public Int32 dcdRow;
        public Int32 selProg;
        public Int32 runProg;
        public Int32 partCntr;
        public Int32 partTotalNum;
        public Int32 hOff;
        public Int32 dOff;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] progName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] chanLax;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public SName[] chanAxisName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public SName[] chanSpdlName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public Int32[] chanModal;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] chanSpdlLax;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] chanSpdlParaLax;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Double[] cmdSpdlSpeed;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Double[] actSpdlSpeed;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] spdlOverride;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Double[] bpPos;
    }
}
