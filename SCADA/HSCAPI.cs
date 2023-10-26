﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace Csharpdlltest
{
    public class HSCAPI
    {      
        public enum enable
        {
            DISABLE,
            ENABLE,
        };
       
        public enum WorkMode
        {
            WORK_MODE_AUT,
            WORK_MODE_T1,
            WORK_MODE_T2,
            WORK_MODE_EXT,
            WORK_MODE_NONE,
        };
       public enum FrameType
        {
            COORD_TYPE_JOINT,
            COORD_TYPE_BASE,
            COORD_TYPE_TOOL,
            COORD_TYPE_WORLD,
            COORD_TYPE_NONE,
        };

       public struct HscDevice
       {
           public string strName; //控制器名
           public string strIP; //IP地址
           public string strSN; //SN序列号
       };

       public struct StateData
       {
           public int en; //0-disable 1-enable
           public WorkMode workMode;
           public FrameType coord_type;
           public int vord;
           public int tool_frame_num;
           public int base_frame_num;
           public int tp_increment; //增量模式,目前没有用到
       }
        public enum TpScreen
        {
            SCREEN_JOG,
            SCREEN_PRG,
            SCREEN_NONE,
        };
        public enum Direction
        {
        DIRECTION_POSITIVE = 0,
        DIRECTION_NEGTIVE = 1,
        };
        public enum CALIBRATIONTYPE
        {
          SINGLE_POINT, //单点
          MULTI_POINT, //多点
          ORIENTATION_POINT, //
          OTHER_POINT, 
        };    
 
       public enum VarName
       {
         EXT_PRG,
         REF,
         TOOL_FRAME,
         BASE_FRAME,
         IR,
         DR,
         JR,
         LR,
         ER,
       }
       public enum WorkGroup
      {
         ROBOT,
         EXT_AXES,
      };
       public enum MoveType
      {
         MOVE,
         MOVES,
      };
            //组类型
      public enum GROUP_TYPE
     {
        GROUP_TYPE_ROBOT,
        GROUP_TYPE_EXT_AXES,
     };
      public enum IOType
      {
          IO_TYPE_DIN,
          IO_TYPE_DOUT,
      };
  
        //关节坐标(4轴)
        public struct JointPos4
        {
            public Double a1;
            public Double a2;
            public Double a3;
            public Double a4;
        };

        //关节坐标(6轴)
        public struct JointPos6
        {
            public Double a1;
            public Double a2;
            public Double a3;
            public Double a4;
            public Double a5;
            public Double a6;
        };

        //笛卡尔坐标(4轴)
        public struct DcartPos4
        {
            public Double x;
            public Double y;
            public Double z;
            public Double a;
        };

        //笛卡尔坐标(6轴)
        public struct DcartPos6
        {
            public Double x;
            public Double y;
            public Double z;
            public Double a;
            public Double b;
            public Double c;
        };
     
        public enum ProgState
        {
            STATE_NONE,
            STATE_RUNNING = 1,
            STATE_STOPPED,
            STATE_ERROR = 4,
            STATE_TERMINATED,
            STATE_READY = 7,
            STATE_KILLSTART = 9,
            STATE_KILLED,
        };
        public struct TaskStatus
        {
            public ProgState state; //取模256(258%256 = 2(STATE_STOPPED))，即2和258都是stopped状态
            public int error; //错误代码
            public int source; //当前运行行号
            public String progName; //主程序名
            public String currFileName; //当前运行程序名
        };
        public enum uctype
        {
            LIST,
            FILE,
        };
       public struct MFileInfo
        {
            public string strName;//文件名称
            public UInt32 ulSize;// 大小
            public uctype ucType; //类型 0表示文件，1表示目录
            public String strModifyTime; //修改时间
        };

        //关节属性
        //public List<int> JointConfig;

        //网络初始化。
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_NetInit", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_NetInit();
        
        //扫描网络
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_NetScanDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_NetScanDevice(ref UInt32 ret);

        //网络连接, rIp: IPC的地址；rPort: 5001、5004、5005可用，5003被示教器占用。
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_NetConnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_NetConnect(String rIp, UInt16 rPort);

        //检查网络状态，为0连接正常，非0连接不正常。
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_NetIsConnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_NetIsConnect();

        //网络退出，在系统关闭时调用，释放资源。
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_NetExit", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_NetExit();
        
        //控制器重启   
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SysReboot", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SysReboot();
         
        //控制器启动状态 
         [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SysIsBootComplete", CallingConvention = CallingConvention.Cdecl)]
         public static extern UInt32 HSCAPIDLL_SysIsBootComplete(ref UInt16 bootState);

        //获取状态数据，模式、运行程序名字、倍率等 
         [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getStateData", CallingConvention = CallingConvention.Cdecl)]
         public static extern UInt32 HSCAPIDLL_getStateData(ref int en,ref WorkMode workMode,ref FrameType coord_type,ref int vord,ref int tool_frame_num,ref int base_frame_num,ref int tp_increment);//获取状态数据 

        //设置使能 
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetEnableState", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetEnableState(enable en);

        //获取机器人使能状态（全部轴上使能本状态为使能，任何一个轴非使能本状态非使能）
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetEnableState", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetEnableState(ref enable en);

        //设置工作模式（手动、自动、外部）
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetWorkMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetWorkMode(WorkMode mode);

        //获取运行模式（手动、自动、外部）
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetWorkMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetWorkMode(ref WorkMode mode);

        //设置坐标系（关节坐标、世界坐标、基坐标、工具坐标） 
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setFrame(FrameType frame);

        //获取坐标系（基坐标、工具坐标、世界坐标、关节坐标）
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetFrameType", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetFrameType(ref FrameType frame);

        //设置窗口，在手动时加载程序需要切换到prgscreen
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setScreen", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setScreen(TpScreen screen);

        //获取窗口（JOG窗口、PRG窗口）
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetTpScreen", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetTpScreen(ref TpScreen screen);

        //设置倍率（范围1-100）
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetOverride", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetOverride(UInt32 nOverride);

        //获取当前倍率
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetOverride", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetOverride(ref UInt32 nOverride);        

        //设置IR寄存器的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetIR", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetIR(UInt32 index, Int32 val);

        //获取IR寄存器的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetIR", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetIR(UInt32 index, ref Int32 val);

        //设置DR寄存器的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetDR", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetDR(UInt32 index, Double val);

        //获取DR寄存器的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetDR", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetDR(UInt32 index, ref Double val);

        //设置4轴机器人的LR寄存器的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetLR4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetLR4(UInt32 index, IntPtr pDoubleArray);

        //设置六轴机器人的LR寄存器的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetLR6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetLR6(UInt32 index, IntPtr pDoubleArray);

        //获取4轴机器人的LR寄存器的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetLR4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetLR4(UInt32 index, IntPtr pDoubleArray);

        //获取六轴机器人的LR寄存器的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetLR6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetLR6(UInt32 index, IntPtr pDoubleArray);
        //还不能使用JR寄存器的接口
        
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetJR4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetJR4(UInt32 index, IntPtr pDoubleArray);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetJR6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetJR6(UInt32 index, IntPtr pDoubleArray);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetJR4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetJR4(UInt32 index, IntPtr pDoubleArray);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetJR6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetJR6(UInt32 index, IntPtr pDoubleArray);

        //获取机器人当前关节坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetJointPos4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetJointPos4(IntPtr pJointPosDoubleArray);

        //获取机器人当前关节坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetJointPos6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetJointPos6(IntPtr pJointPosDoubleArray);

        //获取机器人当前笛卡尔坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetDcartPos4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetDcartPos4(IntPtr pDcartPosDoubleArray);

        //获取机器人当前笛卡尔坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetDcartPos6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetDcartPos6(IntPtr pDcartPosDoubleArray);

        //获取外部轴坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetExtAxesPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetExtAxesPos(IntPtr pExtAxesPosDoubleArray);

        //获取外部轴坐标
        //[DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetExtAxesPos6", CallingConvention = CallingConvention.Cdecl)]
        //public static extern UInt32 HSCAPIDLL_GetExtAxesPos6(IntPtr pExtAxesPosDoubleArray);

        //设置工具坐标 
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setToolFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setToolFrame(UInt32 index, string data);

        ////获取工具坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getToolFrame4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getToolFrame4(UInt32 index, IntPtr pDoubleArray);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getToolFrame6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getToolFrame6(UInt32 index, IntPtr pDoubleArray);

        //设置基坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setBaseFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setBaseFrame(UInt32 index, string data);

        //获取基坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getBaseFrame4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getBaseFrame4(UInt32 index, IntPtr pDoubleArray);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getBaseFrame6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getBaseFrame6(UInt32 index, IntPtr pDoubleArray);

        //设置工具坐标系号
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetToolFrameNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetToolFrameNum(UInt32 nToolNum);

        //获取工具坐标系号
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetToolFrameNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetToolFrameNum(ref UInt32 nToolNum);

        //设置工件坐标系号
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetBaseFrameNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetBaseFrameNum(UInt32 nBaseNum);

        //获取工件坐标系号
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetBaseFrameNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetBaseFrameNum(ref UInt32 nBaseNum);

        //变量
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetLong", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetLong(string vName, ref UInt32 value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetDouble", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetDouble(string vName, ref double value);
         
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetString", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_GetString(string vName,ref UInt32 value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetJoint", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_GetJoint(string vName, ref UInt32 value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetLocation", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_GetLocation(string vName, ref UInt32 value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetJoint4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetJoint4(String vName, IntPtr pDoubleArray);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetJoint6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetJoint6(String vName, IntPtr pDoubleArray);
        
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetLocation4", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetLocation4(String vName, IntPtr pDoubleArray);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetLocation6", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetLocation6(String vName, IntPtr pDoubleArray);

        //获取机器人内部轴轴数
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetRobotAxesNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetRobotAxesNumber(ref UInt32 nRobotAxesNumber);

        //获取机器人外部轴轴数
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetExtAxesNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_GetExtAxesNumber(ref UInt32 nExtAxesNumber);

        //文件	
        //文件发送
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_NetSendFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_NetSendFile(string srcFilename, string dstFilename);

        //文件获取
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_NetGetFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_NetGetFile(string srcFilename, string dstFilename);

        //手动模式运行
        //手动模式jogscreen下，每200ms调用一次
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_tpJogFeedDog", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_tpJogFeedDog();

        //手动运动
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_StartMoving", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_StartMoving(UInt32 nAxis, Direction nDirect);

        //手动停止
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_StopMoving", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_StopMoving();
 
        //自动模式运行
        //发送主程序到控制器
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SendUserProgFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SendUserProgFile(string filePath, string fileName);

        //加载程序。从IPC磁盘中加载已有的程序。（该函数实际上加载的是上述的预加载程序）
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_ProgLoad", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_ProgLoad(string progName);

        //获取机器人程序名
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetUserProgName", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_GetUserProgName(ref UInt32 ret);

        //清空lib数组
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_resetSubFuncArray", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_resetSubFuncArray();

        //最后发送主程序
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SendSubFuncFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SendSubFuncFile(string srcFilePath, string dstFileName);

        //加载子程序到数组中
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_loadSubLib", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_loadSubLib(string subName);

        //程序开始
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_ProgStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_ProgStart();

        //程序暂停
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_ProgPause", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_ProgPause();

        //停止已加载的程序并卸载
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_ProgStop", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_ProgStop();

        //获取已加载程序的状态
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetUserProgStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_GetUserProgStatus(ref ProgState sta,ref int err,ref int sour,ref UInt32 re);

        //单步
        //单步进入。程序已加载，但还未连续运行时可调用该函数
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_StepIn", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_StepIn();

        //单步跳出。程序已加载，但还未连续运行时可调用该函数。
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_StepOut", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_StepOut();

        //单步执行。程序已加载，但还未连续运行时可调用该函数
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_StepOver", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_StepOver();

        //运行到任意行
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_runArbitraryLine", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_runArbitraryLine(UInt32 no);

        //标定
        //工具坐标标定
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_tpToolCalibrationType", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_tpToolCalibrationType(CALIBRATIONTYPE type);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_tpCalibratePoint", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_tpCalibratePoint(UInt32 pointNum);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_tpRemoveCalibrationPoint", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_tpRemoveCalibrationPoint(UInt32 pointNum);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_tpToolCalibration", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_tpToolCalibration(UInt32 index);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_saveTpToolCalibration", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_saveTpToolCalibration();

        //保存变量到文件
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_saveVar", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_saveVar(VarName var);
      
        //设置变量
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setVar", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setVar(string varName, string varValue);

        //基坐标标定
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setCalibrateBaseValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setCalibrateBaseValue(UInt32 index, string value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_calibrateBase", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_calibrateBase(UInt16 index);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_saveCalibrateBase", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_saveCalibrateBase();

        //运动设置
        //运动到点
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_moveToPoint", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_moveToPoint(string point, WorkGroup groupNo, MoveType type);
       
	  //设置零点，data只能是关节坐标
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setHomePositionValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setHomePositionValue(GROUP_TYPE group, string data); 
 
        //保存零点
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_saveHomePosition", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_saveHomePosition(GROUP_TYPE group);

        //设置轴限位信息
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setLimitInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setLimitInfo(GROUP_TYPE group, UInt32 index, double negtive, double positive, bool en);
	
        //获取轴限位信息
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getLimitInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getLimitInfo(GROUP_TYPE group, UInt32 index,ref Double negtive, ref Double positive,ref bool en);

        //保存内部轴限位信息
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_saveRobPlimit", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_saveRobPlimit();
	
        //保存外部轴限位信息
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_saveEaPlimit", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_saveEaPlimit();
	
        //设置运动距离
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setMoveDistance", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setMoveDistance(UInt32 moveIncrementLevel);

        //获取运动距离
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getMoveDistance", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getMoveDistance(ref int moveIncrementLevel);

        //IO设置
        //设置虚拟IO
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetVinFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetVinFlag(UInt16 inputId, bool stat);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetVoutFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_SetVoutFlag(UInt16 outputId, bool stat);
        
        //设置IO值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setIOValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setIOValue(IOType io, UInt16 ioId, bool value);

        //设置外部控制输入标志
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setInputSigMap", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setInputSigMap(int lSysSigIndex, int lInputIndex);

        //设置外部控制输出标志
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setOutputSigMap", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setOutputSigMap(int lSysSigIndex, int lOutputIndex);

        //保存设置
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_HSCAPIDLL_saveSignalReg", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_saveSignalReg();

        //设置组，内部或者外部
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setMotionGroup", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setMotionGroup(GROUP_TYPE group);

        //设置模拟量输出，id为索引，value为电压取值范围
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setAOutputValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_setAOutputValue(int id, double value);

        //从数据低位到高位分别对应IO的低位到高位
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getIOGroup", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getIOGroup(IOType io, int group,ref int value);

        //获取IO的虚拟状态，io表示类型DI或者DO；group表示组号，目前有16组；value获取的值（32位，对应8个IO位的虚拟标志值）
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getIOGroupVirtualFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getIOGroupVirtualFlag(IOType io, int group,ref int value);

        //获取所有数字I/O的值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getallIOValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_getallIOValue(IOType io,ref UInt32 re);

        //获取所有数字I/O的状态
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getallIOState", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_getallIOState(IOType io,ref UInt32 re);

        //获取din总数
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getDinNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getDinNum(ref int value);

        //获取dout总数
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getDoutNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getDoutNum(ref int value);

        //获取ain总数
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAinNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getAinNum(ref int value);

        //获取aout总数
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAoutNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getAoutNum(ref int value);

        //获取DI输入起始id
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getInputStartId", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getInputStartId(ref int value);

        //获取DO输出起始id
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getOutputStartId", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 HSCAPIDLL_getOutputStartId(ref int value);

        //获取每个通道输入多少个，输入IO总数=INPUT_SLICE_COUNT*PORT_COUNT_PER_SLICE
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getInputSliceCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getInputSliceCount(ref int value);

        //获取每个通道输出多少个，计算方法同上
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getOutputSliceCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getOutputSliceCount(ref int value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getPortCountPerSlice", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getPortCountPerSlice(ref int value);

        //模拟量IO
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAInputStartId", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAInputStartId(ref int value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAOutputStartId", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAOutputStartId(ref int value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAInputPortCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAInputPortCount(ref int value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAOutputPortCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAOutputPortCount(ref int value);

        //获取输入输出电压最大最小值
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAInputMaxValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAInputMaxValue(ref double value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAOutputMaxValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAOutputMaxValue(ref double value);
     
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAInputMinValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAInputMinValue(ref double value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAOutputMinValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAOutputMinValue(ref double value);
    
        //获取模拟量精度
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAInputAccValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAInputAccValue(ref double value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getAOutputAccValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getAOutputAccValue(ref double value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetAInputValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_GetAInputValue(int id,ref double value);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetAOutputValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_GetAOutputValue(int id,ref double value);

        //使能用户plc,0 disable, 1&other enable
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setUserPlcState", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_setUserPlcState(int en);

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getUserPlcState", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_getUserPlcState(ref int en);

        //判断用户plc运行状态，不为0则为启动状态
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_userPlcIsRunning", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_userPlcIsRunning(ref int run);

        //启动plc
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_startUserPlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_startUserPlc(int state);

        //停止plc，以上用户plc接口都是立即生效
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_stopUserPlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_stopUserPlc(int state);

        //清除错误
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_clearFault", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_clearFault();

        //设置语言(0-english 1-chinese)
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_SetMessageLang", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_SetMessageLang(int no);

        //开启打印
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setPrintDebug", CallingConvention = CallingConvention.Cdecl)]
        public static extern void  HSCAPIDLL_setPrintDebug(bool en);

        //获取版本信息
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetSOVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_GetSOVersion(ref UInt32 re);

        //获取HMCAPI库版本
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getHMCVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern string  HSCAPIDLL_getHMCVersion();

        //获取错误
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetTpError", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_GetTpError(ref string errMsg);

        //设置网络超时时间
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_setSyncMessageWaitTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_setSyncMessageWaitTime(UInt16 usTime);

        //获取文件列表
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_getFileList", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr  HSCAPIDLL_getFileList(string strDirPath,ref UInt32 ret);

        //删除文件
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_NetDelFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32  HSCAPIDLL_NetDelFile(string strFilePath);

        //授权指令
       //生成序列号
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_genSN", CallingConvention = CallingConvention.Cdecl)]
         public static extern IntPtr  HSCAPIDLL_genSN(ref UInt32 retsn);
        
        //授权
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_authorization", CallingConvention = CallingConvention.Cdecl)]
        public static extern int  HSCAPIDLL_authorization(string authCode);

        //计算授权剩余时间
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_authorizationRemainingTime()", CallingConvention = CallingConvention.Cdecl)]
        public static extern Double HSCAPIDLL_authorizationRemainingTime();


        //通用接口
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_executeCmdResponse", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_executeCmdResponse(string cmdStr, ref UInt32 ret, int priority);

        //获取系统信息
        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetSysMessage", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_GetSysMessage(ref Int32 nType, ref Int32 nNum, ref UInt32 retMsg, UInt32 nwaitTime);

        //HSCAPIDLL_SetLR和HSCAPIDLL_GetLR函数使用下面重新封装的来调用
        //其中作为参数的pDcartDataArray = new Double[4]
        public static UInt32 HSCAPIDLL_SetLR4(UInt32 index, Double[] pDcartDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            DcartPos4 dcartPos4;
            Int32 DcartArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            dcartPos4.x = pDcartDataArray[0];
            dcartPos4.y = pDcartDataArray[1];
            dcartPos4.z = pDcartDataArray[2];
            dcartPos4.a = pDcartDataArray[3];

            Marshal.StructureToPtr(dcartPos4, ptr, true);
            ret = HSCAPIDLL_SetLR4(index, ptr);

            Marshal.FreeHGlobal(ptr);

            return ret;
        }

        //其中作为参数的pDcartDataArray = new Double[6]
        public static UInt32 HSCAPIDLL_SetLR6(UInt32 index, Double[] pDcartDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            DcartPos6 dcartPos6;
            Int32 DcartArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            dcartPos6.x = pDcartDataArray[0];
            dcartPos6.y = pDcartDataArray[1];
            dcartPos6.z = pDcartDataArray[2];
            dcartPos6.a = pDcartDataArray[3];
            dcartPos6.b = pDcartDataArray[4];
            dcartPos6.c = pDcartDataArray[5];

            Marshal.StructureToPtr(dcartPos6, ptr, true);
            ret = HSCAPIDLL_SetLR6(index, ptr);

            Marshal.FreeHGlobal(ptr);

            return ret;
        }       

        public static UInt32 HSCAPIDLL_GetLR4(UInt32 index, ref Double[] pDcartDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            DcartPos4 dcartPos4;
            Int32 DcartArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetLR4(index, ptr);
            if (ret == 0)/*OK*/
            {
                dcartPos4 = (DcartPos4)Marshal.PtrToStructure(ptr, typeof(DcartPos4));
                pDcartDataArray[0] = dcartPos4.x;
                pDcartDataArray[1] = dcartPos4.y;
                pDcartDataArray[2] = dcartPos4.z;
                pDcartDataArray[3] = dcartPos4.a;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetLR6(UInt32 index, ref Double[] pDcartDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            DcartPos6 dcartPos6;
            Int32 DcartArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetLR6(index, ptr);
            if (ret == 0)/*OK*/
            {
                dcartPos6 = (DcartPos6)Marshal.PtrToStructure(ptr, typeof(DcartPos6));
                pDcartDataArray[0] = dcartPos6.x;
                pDcartDataArray[1] = dcartPos6.y;
                pDcartDataArray[2] = dcartPos6.z;
                pDcartDataArray[3] = dcartPos6.a;
                pDcartDataArray[4] = dcartPos6.b;
                pDcartDataArray[5] = dcartPos6.c;
            }
            
            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_SetJR4(UInt32 index, Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos4 jointPos4;
            Int32 JointArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            jointPos4.a1 = pJointDataArray[0];
            jointPos4.a2 = pJointDataArray[1];
            jointPos4.a3 = pJointDataArray[2];
            jointPos4.a4 = pJointDataArray[3];

            Marshal.StructureToPtr(jointPos4, ptr, true);
            ret = HSCAPIDLL_SetJR4(index, ptr);

            Marshal.FreeHGlobal(ptr);

            return ret; 
        }

        public static UInt32 HSCAPIDLL_SetJR6(UInt32 index, Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos6 jointPos6;
            Int32 JointArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            jointPos6.a1 = pJointDataArray[0];
            jointPos6.a2 = pJointDataArray[1];
            jointPos6.a3 = pJointDataArray[2];
            jointPos6.a4 = pJointDataArray[3];
            jointPos6.a5 = pJointDataArray[4];
            jointPos6.a6 = pJointDataArray[5];

            Marshal.StructureToPtr(jointPos6, ptr, true);
            ret = HSCAPIDLL_SetJR6(index, ptr);

            Marshal.FreeHGlobal(ptr);

            return ret;
        }

        public static UInt32 HSCAPIDLL_GetJR4(UInt32 index, ref Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos4 jointpos4;
            Int32 JointArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            ret = HSCAPIDLL_GetJR4(index, ptr);
            if (ret == 0)/*OK*/
            {
                jointpos4 = (JointPos4)Marshal.PtrToStructure(ptr, typeof(JointPos4));
                pJointDataArray[0] = jointpos4.a1;
                pJointDataArray[1] = jointpos4.a2;
                pJointDataArray[2] = jointpos4.a3;
                pJointDataArray[3] = jointpos4.a4;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetJR6(UInt32 index, ref Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos6 jointpos6;
            Int32 JointArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            ret = HSCAPIDLL_GetJR6(index, ptr);
            if (ret == 0)/*OK*/
            {
                jointpos6 = (JointPos6)Marshal.PtrToStructure(ptr, typeof(JointPos6));
                pJointDataArray[0] = jointpos6.a1;
                pJointDataArray[1] = jointpos6.a2;
                pJointDataArray[2] = jointpos6.a3;
                pJointDataArray[3] = jointpos6.a4;
                pJointDataArray[4] = jointpos6.a5;
                pJointDataArray[5] = jointpos6.a6;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetJoint4(String vName, ref Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos4 jointpos4;
            Int32 JointArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            ret = HSCAPIDLL_GetJoint4(vName, ptr);
            if (ret == 0)/*OK*/
            {
                jointpos4 = (JointPos4)Marshal.PtrToStructure(ptr, typeof(JointPos4));
                pJointDataArray[0] = jointpos4.a1;
                pJointDataArray[1] = jointpos4.a2;
                pJointDataArray[2] = jointpos4.a3;
                pJointDataArray[3] = jointpos4.a4;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;   
        }

        public static UInt32 HSCAPIDLL_GetJoint6(String vName, ref Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos6 jointpos6;
            Int32 JointArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            ret = HSCAPIDLL_GetJoint6(vName, ptr);
            if (ret == 0)/*OK*/
            {
                jointpos6 = (JointPos6)Marshal.PtrToStructure(ptr, typeof(JointPos6));
                pJointDataArray[0] = jointpos6.a1;
                pJointDataArray[1] = jointpos6.a2;
                pJointDataArray[2] = jointpos6.a3;
                pJointDataArray[3] = jointpos6.a4;
                pJointDataArray[4] = jointpos6.a5;
                pJointDataArray[5] = jointpos6.a6;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetLocation4(String vName, ref Double[] pDcartDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            DcartPos4 dcartPos4;
            Int32 DcartArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetLocation4(vName, ptr);
            if (ret == 0)/*OK*/
            {
                dcartPos4 = (DcartPos4)Marshal.PtrToStructure(ptr, typeof(DcartPos4));
                pDcartDataArray[0] = dcartPos4.x;
                pDcartDataArray[1] = dcartPos4.y;
                pDcartDataArray[2] = dcartPos4.z;
                pDcartDataArray[3] = dcartPos4.a;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetLocation6(String vName, ref Double[] pDcartDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            DcartPos6 dcartPos6;
            Int32 DcartArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetLocation6(vName, ptr);
            if (ret == 0)/*OK*/
            {
                dcartPos6 = (DcartPos6)Marshal.PtrToStructure(ptr, typeof(DcartPos6));
                pDcartDataArray[0] = dcartPos6.x;
                pDcartDataArray[1] = dcartPos6.y;
                pDcartDataArray[2] = dcartPos6.z;
                pDcartDataArray[3] = dcartPos6.a;
                pDcartDataArray[4] = dcartPos6.b;
                pDcartDataArray[5] = dcartPos6.c;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetJointPos4(ref Double[] pJointPosDoubleArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos4 jPos4;
            Int32 DcartArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetJointPos4(ptr);
            if (ret == 0)/*OK*/
            {
                jPos4 = (JointPos4)Marshal.PtrToStructure(ptr, typeof(JointPos4));
                pJointPosDoubleArray[0] = jPos4.a1;
                pJointPosDoubleArray[1] = jPos4.a2;
                pJointPosDoubleArray[2] = jPos4.a3;
                pJointPosDoubleArray[3] = jPos4.a4;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetJointPos6(ref Double[] pJointPosDoubleArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos6 jPos6;
            Int32 DcartArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetJointPos6(ptr);
            if (ret == 0)/*OK*/
            {
                jPos6 = (JointPos6)Marshal.PtrToStructure(ptr, typeof(JointPos6));
                pJointPosDoubleArray[0] = jPos6.a1;
                pJointPosDoubleArray[1] = jPos6.a2;
                pJointPosDoubleArray[2] = jPos6.a3;
                pJointPosDoubleArray[3] = jPos6.a4;
                pJointPosDoubleArray[4] = jPos6.a5;
                pJointPosDoubleArray[5] = jPos6.a6;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetDcartPos4(ref Double[] pDcartPosDoubleArray)
        {
            UInt32 ret = UInt32.MaxValue;
            DcartPos4 dPos4;
            Int32 DcartArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetDcartPos4(ptr);
            if (ret == 0)/*OK*/
            {
                dPos4 = (DcartPos4)Marshal.PtrToStructure(ptr, typeof(DcartPos4));
                pDcartPosDoubleArray[0] = dPos4.x;
                pDcartPosDoubleArray[1] = dPos4.y;
                pDcartPosDoubleArray[2] = dPos4.z;
                pDcartPosDoubleArray[3] = dPos4.a;

            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetDcartPos6(ref Double[] pDcartPosDoubleArray)
        {
            UInt32 ret = UInt32.MaxValue;
            DcartPos6 dPos6;
            Int32 DcartArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetDcartPos6(ptr);
            if (ret == 0)/*OK*/
            {
                dPos6 = (DcartPos6)Marshal.PtrToStructure(ptr, typeof(DcartPos6));
                pDcartPosDoubleArray[0] = dPos6.x;
                pDcartPosDoubleArray[1] = dPos6.y;
                pDcartPosDoubleArray[2] = dPos6.z;
                pDcartPosDoubleArray[3] = dPos6.a;
                pDcartPosDoubleArray[4] = dPos6.b;
                pDcartPosDoubleArray[5] = dPos6.c;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetExtAxesPos(ref Double[] pExtAxesPosDoubleArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos4 jPos4;
            Int32 DcartArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(DcartArraySize);

            ret = HSCAPIDLL_GetExtAxesPos(ptr);
            if (ret == 0)/*OK*/
            {
                jPos4 = (JointPos4)Marshal.PtrToStructure(ptr, typeof(JointPos4));
                pExtAxesPosDoubleArray[0] = jPos4.a1;
                pExtAxesPosDoubleArray[1] = jPos4.a2;
                pExtAxesPosDoubleArray[2] = jPos4.a3;
                pExtAxesPosDoubleArray[3] = jPos4.a4;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }       

        public static UInt32 HSCAPIDLL_getToolFrame4(UInt32 index, ref Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos4 jointpos4;
            Int32 JointArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            ret = HSCAPIDLL_getToolFrame4(index, ptr);
            if (ret == 0)/*OK*/
            {
                jointpos4 = (JointPos4)Marshal.PtrToStructure(ptr, typeof(JointPos4));
                pJointDataArray[0] = jointpos4.a1;
                pJointDataArray[1] = jointpos4.a2;
                pJointDataArray[2] = jointpos4.a3;
                pJointDataArray[3] = jointpos4.a4;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_getToolFrame6(UInt32 index, ref Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos6 jointpos6;
            Int32 JointArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            ret = HSCAPIDLL_getToolFrame6(index, ptr);
            if (ret == 0)/*OK*/
            {
                jointpos6 = (JointPos6)Marshal.PtrToStructure(ptr, typeof(JointPos6));
                pJointDataArray[0] = jointpos6.a1;
                pJointDataArray[1] = jointpos6.a2;
                pJointDataArray[2] = jointpos6.a3;
                pJointDataArray[3] = jointpos6.a4;
                pJointDataArray[4] = jointpos6.a5;
                pJointDataArray[5] = jointpos6.a6;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_getBaseFrame4(UInt32 index, ref Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos4 jointpos4;
            Int32 JointArraySize = 4 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            ret = HSCAPIDLL_getBaseFrame4(index, ptr);
            if (ret == 0)/*OK*/
            {
                jointpos4 = (JointPos4)Marshal.PtrToStructure(ptr, typeof(JointPos4));
                pJointDataArray[0] = jointpos4.a1;
                pJointDataArray[1] = jointpos4.a2;
                pJointDataArray[2] = jointpos4.a3;
                pJointDataArray[3] = jointpos4.a4;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_getBaseFrame6(UInt32 index, ref Double[] pJointDataArray)
        {
            UInt32 ret = UInt32.MaxValue;
            JointPos6 jointpos6;
            Int32 JointArraySize = 6 * Marshal.SizeOf(typeof(Double));
            IntPtr ptr = Marshal.AllocHGlobal(JointArraySize);

            ret = HSCAPIDLL_getBaseFrame6(index, ptr);
            if (ret == 0)/*OK*/
            {
                jointpos6 = (JointPos6)Marshal.PtrToStructure(ptr, typeof(JointPos6));
                pJointDataArray[0] = jointpos6.a1;
                pJointDataArray[1] = jointpos6.a2;
                pJointDataArray[2] = jointpos6.a3;
                pJointDataArray[3] = jointpos6.a4;
                pJointDataArray[4] = jointpos6.a5;
                pJointDataArray[5] = jointpos6.a6;
            }

            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        public static UInt32 HSCAPIDLL_NetScanDevice(ref HscDevice[] device)
        {
            string Cmdstr;
            IntPtr Device;
            //HscDevice[] deva= new HscDevice[size];
            UInt32 ret = UInt32.MaxValue;
            Device = HSCAPIDLL_NetScanDevice(ref ret);//扫描网络
            Cmdstr = Marshal.PtrToStringAnsi(Device);
            String[] devstrarry = Cmdstr.Split(',');
            
            for(int i=0;i<devstrarry.Length-1;i++)
            {
                String[] devi = devstrarry[i].Split('|');
                device[i].strName = devi[0];
                device[i].strIP = devi[1];
                device[i].strSN = devi[2];
            }
            return ret;
        }

        [DllImport("HSCAPIDLL.dll", EntryPoint = "HSCAPIDLL_GetJointPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HSCAPIDLL_GetJointPos(ref UInt32 ret);

        public static UInt32 HSCAPIDLL_GetJointPos(ref Double[] Joint)
        {
            UInt32 ret = UInt32.MaxValue;
            string Cmdstr;
            IntPtr Jot;
            Jot = HSCAPIDLL_GetJointPos(ref ret);//扫描网络
            Cmdstr = Marshal.PtrToStringAnsi(Jot);
            String[] strJot = Cmdstr.Split('|');
            for (int i = 0; i < strJot.Length-1; i++)
            {
                Joint[i]=double.Parse(strJot[i]);
            }
            return ret;
            
        }

        public static  UInt32 HSCAPIDLL_GetUserProgName(ref String progName)
        {
            UInt32 ret = UInt32.MaxValue;
            IntPtr pname;
            pname = HSCAPIDLL_GetUserProgName(ref ret);//"获当取前主程序"
            progName = Marshal.PtrToStringAnsi(pname);
            return ret;
        }

        public static UInt32 HSCAPIDLL_executeCmdResponse(string cmdStr, ref String response, int priority)
        {
            UInt32 ret = UInt32.MaxValue;    
            IntPtr rett;
            rett = HSCAPI.HSCAPIDLL_executeCmdResponse(cmdStr, ref ret, priority);
            response = Marshal.PtrToStringAnsi(rett);

            return ret;
        }

        public static UInt32 HSCAPIDLL_getAllIOValue(IOType io, ref bool[] avalueArray)
        {
            UInt32 ret = UInt32.MaxValue;
            string rettt = null;
            IntPtr avalue;
            avalue = HSCAPI.HSCAPIDLL_getallIOValue(io, ref ret);//获取所有IO的值
            rettt = Marshal.PtrToStringAnsi(avalue);
            string[] AvalueArray = rettt.Split('|');
            for (int i = 0; i < AvalueArray.Length-1; i++)
            {
                avalueArray[i] = Convert.ToBoolean(AvalueArray[i]);  
            }
           return ret;        
        }

        public static UInt32 HSCAPIDLL_getAllIOState(IOType io, ref bool[] stateArray)
        {
            UInt32 ret = UInt32.MaxValue;
            IntPtr sta;
            string rettt = null;
            sta = HSCAPI.HSCAPIDLL_getallIOState(io, ref ret);//获取所有IO状态
            rettt = Marshal.PtrToStringAnsi(sta);
            string[] StateArray = rettt.Split('|');
            for (int i = 0; i < StateArray.Length-1; i++)
            {
                stateArray[i] = Convert.ToBoolean(StateArray[i]);   
            }
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetSOVersion(ref String osVer)
        {
            UInt32 ret = UInt32.MaxValue;
              
            IntPtr ov;
            ov = HSCAPI.HSCAPIDLL_GetSOVersion(ref ret);
            osVer = Marshal.PtrToStringAnsi(ov);

            return ret;
           
        }

        public static UInt32 HSCAPIDLL_genSN(ref String snstr)
        {
            UInt32 ret = UInt32.MaxValue;        
            IntPtr Sn;
            Sn = HSCAPI.HSCAPIDLL_genSN(ref ret);
            snstr = Marshal.PtrToStringAnsi(Sn);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetSysMessage(ref Int32 nType, ref Int32 nNum,ref String strMsg, UInt32 nwaitTime)
        {
            UInt32 ret = UInt32.MaxValue;
            IntPtr robotMsg = HSCAPI.HSCAPIDLL_GetSysMessage(ref nType, ref nNum, ref ret, 500);
            strMsg = Marshal.PtrToStringAnsi(robotMsg);
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetString(string vName, ref String value)
        {
            UInt32 ret = UInt32.MaxValue;
            IntPtr getstr = HSCAPI.HSCAPIDLL_GetString(vName, ref ret);
            value = Marshal.PtrToStringAnsi(getstr);

            return ret; 
        }
       
        public static UInt32 HSCAPIDLL_GetJoint(string vName, ref Double[] value)
        {
            UInt32 ret = UInt32.MaxValue;
            String Jval = null;
            
            IntPtr getstr = HSCAPI.HSCAPIDLL_GetJoint(vName, ref ret);
            Jval = Marshal.PtrToStringAnsi(getstr);
            String[] JvalArray = Jval.Split('|');
            for (int i=0; i < JvalArray.Length - 1; i++)
            {
                value[i] = double.Parse(JvalArray[i]); 
            }
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetLocation(string vName, ref Double[] value)
        {
            UInt32 ret = UInt32.MaxValue;
            String Jval = null;

            IntPtr getstr = HSCAPI.HSCAPIDLL_GetLocation(vName, ref ret);
            Jval = Marshal.PtrToStringAnsi(getstr);
            String[] JvalArray = Jval.Split('|');
            for (int i = 0; i < JvalArray.Length - 1; i++)
            {
                value[i] = double.Parse(JvalArray[i]); 
            }
            return ret;
        }

        public static UInt32 HSCAPIDLL_GetUserProgStatus(ref TaskStatus statu)
        {
            UInt32 ret = UInt32.MaxValue;

            ProgState sta = ProgState.STATE_NONE;
            int err = 0;
            int sour = 0;
            String Spstr = null; 
            IntPtr str;
            str = HSCAPI.HSCAPIDLL_GetUserProgStatus(ref sta,ref err,ref sour,ref ret);
            Spstr = Marshal.PtrToStringAnsi(str);
            String[] proname = Spstr.Split('|');
            statu.error = err;
            statu.source = sour;          
            statu.currFileName = proname[0];
            statu.progName = proname[1];
            if(statu.progName == ""&&sta ==ProgState.STATE_READY)
            {
                sta = ProgState.STATE_NONE;
            }
            statu.state = sta;
            return ret;

        }

        public static UInt32 HSCAPIDLL_getStateData(ref StateData statu)
        {
            UInt32 ret = UInt32.MaxValue;
            int en =0;
            //TaskStatus prgState = new TaskStatus();
            WorkMode workMode = WorkMode.WORK_MODE_NONE;
            FrameType coord_type = FrameType.COORD_TYPE_NONE;
            int vord = 0;
            int tool_frame_num = 0;
            int base_frame_num =0;
            int tp_increment =0;

            ret = HSCAPIDLL_getStateData(ref en, ref workMode, ref coord_type, ref vord, ref tool_frame_num, ref base_frame_num, ref tp_increment);
            statu.en = en;
  
            statu.workMode = workMode;
            statu.coord_type = coord_type;
            statu.vord = vord;
            statu.tool_frame_num = tool_frame_num;
            statu.base_frame_num = base_frame_num;
            statu.tp_increment = tp_increment;
            return ret;
        }

        public static UInt32 HSCAPIDLL_getFileList(string strDirPath, ref MFileInfo[] flielist,ref int size)
        {
            UInt32 ret = UInt32.MaxValue;
            IntPtr str;
            str = HSCAPIDLL_getFileList(strDirPath,ref ret);
            String Spstr = Marshal.PtrToStringAnsi(str);
            String[] strlist = Spstr.Split(';');
            size = strlist.Length - 1;
            for (int i = 0; i < strlist.Length - 1; i++)
            {
               
                String[] Strfile = strlist[i].Split('|');
                flielist[i].strName = Strfile[0];
                flielist[i].ulSize = Convert.ToUInt32(Strfile[1]);
                flielist[i].strModifyTime = Strfile[3];
                if (Strfile[2] == "1")
                {
                    flielist[i].ucType = uctype.LIST;
                }
                else
                {
                   flielist[i].ucType = uctype.FILE;
                }
            }
            return ret;
        }
    }    
}