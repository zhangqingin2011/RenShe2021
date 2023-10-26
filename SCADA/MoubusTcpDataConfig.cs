using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace SCADA
{
   public partial class ModbusTcp
    {
        public static Int32 ModbusBufferHeadSize = 7;//7个字节的MBAP表头 其中只有长度需要服务端写190，其他的直接复制客户端数据
        public static Int32 ModbusBufferSize = 322;//190
        public static Int32 ModbusBufferFunCodeSizeReq = 5;
        public static Int32 ModbusBufferFunCodeSizeWrite = 6;
        public static Int32 ResModbusBufferFunCodeSize = 2;
        public static Int32 DataMoubusByteSize = 2 * (ModbusBufferSize) + ModbusBufferHeadSize + ModbusBufferFunCodeSizeWrite;//通讯数据+报文+功能码
        public static int MagAllNum = 30;
        public static int MagLength = 4;
        public static int MagStartNum = 61;
        public static int MeterLength = 9;
        public static int MeterNum = 6;
      //  public static int tcpPort = 502;   //端口号
      //  public static string tcpIp = "192.168.8.10";//西门子PLC地址

     //   public static IPAddress tcpIpAddress = new IPAddress(new byte[] { 192, 168, 8,10 });
        public static IPAddress localIpAddress = new IPAddress(new byte[] { 192, 168, 8, 99 });
        public static Int32[] DataMoubus = new Int32[ModbusBufferSize];//与西门子通讯数据
        public static Int32[] OldDataMoubus = new Int32[ModbusBufferSize];//与西门子通讯数据

        public static Int32[] ReadDataMoubus = new Int32[ModbusBufferSize];//与西门子通讯数据
           
       
        //public  Byte[]  DataMoubusByte = new Byte[2*ModbusBufferSize];//与西门子通讯数据
        public static Byte[] DataMoubusByte= new Byte[ModbusBufferSize*2];//与西门子通讯数据
        public static Byte[] DataMoubusHeadByte = new Byte[ModbusBufferHeadSize];//与西门子通讯数据表头
        public static Byte[] DataMoubusFuncByteReq = new Byte[ModbusBufferFunCodeSizeReq];//与西门子通讯数据请求功能码
        public static Byte[] DataMoubusFuncByteWrite = new Byte[ModbusBufferFunCodeSizeWrite];//与西门子通讯数据写入功能码

       public static Byte[] ReadDataMoubusByteBufferHead = new Byte[ModbusBufferHeadSize];//与西门子通讯数据
       // public static Byte[] ReadDataMoubusByteBufferFunc = new Byte[ResModbusBufferFunCodeSize];//与西门子通讯数据
        //public static Byte[] ReadDataMoubusByteBufferData= new Byte[DataMoubusByteSize];//与西门子通讯数据

        public ModbusTcp()
        {
            for(int i=0;i<ModbusBufferSize;i++)
            {
                DataMoubus[i]= 0;
            }
         }
        public enum DaraHeadArr : int//报文的表头
        {
            Event_Flage = 0,//Modbus请求、响应事物处理的识别码，客户端生成
            Agreement_Flage = 2,//0=Modbus协议，客户端生成
            Data_Length = 4,//以下字节的数量，客户端生成
            Unit_Flage = 6,//从站识别码，客户端生成
        };
        public enum Func_Code :byte//报文的功能码
        {
            req = 3,//保持型寄存器读取
            writereg = 16,//写多寄存器
        };

        public enum DataConfigArr: int
        {
            //mes请求plc
            MES_PLC_comfirm = 1,//	MES发给PLC自动化系统的各种状态（System）
            Rack_number_UnLoad_comfirm,//MES发给PLC机床料放回料仓
            Order_type_comfirm = 3,//机床类型  1车床，2加工中心
            Rack_number_Load_comfirm,//MES发给PLC的取料仓的料到机床

            //mes相应plc
            Mesans_PLC_response=11,//	MES-PLC响应码
            Rcak_Load_number_response,//	MES响应仓位
            Rcak_Unload_number_response,//	MES响应仓位
            //Result_response,//上传结果
            Machine_type_response = 14,//	MES响应设备类型


            //mes相应plc
            Mesans_PLC_response_2 = 16,//	MES-PLC响应码
            Rcak_Load_number_response_2,//	MES响应仓位
            Rcak_Unload_number_response_2,//	MES响应仓位
            //Result_response,//上传结果
            Machine_type_response_2 = 19,//	MES响应设备类型

            PLC_MES_comfirm = 21,//PLC向MES发送命令
            Rcak_Load_number_comfirm,//PLC向MES发送的上料位值
            Rcak_Unload_number_comfirm,//PLC向MES发送的下料位值
            Machine_type_comfirm = 24,//PLC向MES发送的设备号

            PLC_MES_comfirm_2 = 26,//PLC向MES发送命令
            Rcak_Load_number_comfirm_2,//PLC向MES发送的上料位值
            Rcak_Unload_number_comfirm_2,//PLC向MES发送的下料位值
            Machine_type_comfirm_2 = 29,//PLC向MES发送的设备号

            PLC_MES_respone = 31,//PLC响应MES命令
            Rack_number_UnLoad_respone,//PLC响应MES下料位
            Order_type_respone,//PLC响应MES放料位
            Rack_number_Load_respone,//PLC响应上料位

             Mesans_Robot_status=41	,//机械手的状态
            Mesans_Robot_position_comfirm	,//机械手是否在HOME位置确认
            Mesans_Robot_mode	,//机械手运行模式
            Mesans_Robot_speed	,//机器人速度百分比 机器人有任务为1，机器人空闲为0
            Mesans_Joint1_coor	,//机械手关节1的坐标值
            Mesans_Joint2_coor	,//机械手关节2的坐标值
            Mesans_Joint3_coor	,//机械手关节3的坐标值
            Mesans_Joint4_coor	,//机械手关节4的坐标值
            Mesans_Joint5_coor	,//机械手关节5的坐标值
            Mesans_Joint6_coor	,//机械手关节6的坐标值
            Mesans_Joint7_coor,//机械手关节7的坐标值
            Robot_clamp_number,//机械手料抓编号
            Lathe_finish_state,//车床加工完成状态（1为完成）
            Cnc_finish_state,//加工中心加工完成状态


            //料仓有无料状态
            Mag_state_1=61,//
            Mag01_state = 0x0001	,//	仓位1状态(0无料 1有料)
            Mag02_state = 0x0002	,//	仓位2状态(0无料 1有料)
            Mag03_state = 0x0004	,//	仓位3状态(0无料 1有料)
             Mag04_state = 0x0008	,//	仓位4状态(0无料 1有料)
             Mag05_state = 0x0010	,//	仓位5状态(0无料 1有料)
             Mag06_state = 0x0020	,//	仓位6状态(0无料 1有料)
             Mag07_state = 0x0040	,//	仓位7状态(0无料 1有料)
             Mag08_state = 0x0080	,//	仓位8状态(0无料 1有料)
             Mag09_state = 0x0100	,//	仓位9状态(0无料 1有料)
             Mag10_state = 0x0200	,//	仓位10状态(0无料 1有料)
             Mag11_state = 0x0400	,//	仓位11状态(0无料 1有料)
             Mag12_state = 0x0800	,//	仓位10状态(0无料 1有料)
             Mag13_state = 0x1000	,//	仓位13状态(0无料 1有料)
             Mag14_state = 0x2000	,//	仓位14状态(0无料 1有料)
             Mag15_state = 0x4000	,//	仓位15状态(0无料 1有料)
             Mag16_state = 0x8000	,//	仓位16状态(0无料 1有料)
             Mag_state_2=62,//
            Mag17_state = 0x0001	,//	仓位17状态(0无料 1有料)
            Mag18_state = 0x0002	,//	仓位18状态(0无料 1有料)
            Mag19_state = 0x0004	,//	仓位19状态(0无料 1有料)
             Mag20_state = 0x0008	,//	仓位20状态(0无料 1有料)
             Mag21_state = 0x0010	,//	仓位21状态(0无料 1有料)
             Mag22_state = 0x0020	,//	仓位22状态(0无料 1有料)
             Mag23_state = 0x0040	,//	仓位23状态(0无料 1有料)
             Mag24_state = 0x0080	,//	仓位24状态(0无料 1有料)
             Mag25_state = 0x0100	,//	仓位25状态(0无料 1有料)
             Mag26_state = 0x0200	,//	仓位26状态(0无料 1有料)
             Mag27_state = 0x0400	,//	仓位27状态(0无料 1有料)
             Mag28_state = 0x0800	,//	仓位28状态(0无料 1有料)
             Mag29_state = 0x1000	,//	仓位29状态(0无料 1有料)
             Mag30_state = 0x2000	,//	仓位30状态(0无料 1有料)

            CNC1_Door_state = 66,//	车床开关门状态
             L_Door_Close	= 0x0001	,//	车床自动门关闭(0未关闭1关闭）
              L_Door_Open	= 0x0002	,//	车床自动门打开(0未打开1打开）
             L_Chuck_state	= 0x0004	,//	车床卡盘状态(0松开1夹紧)

            CNC2_Door_state = 67,//	加工中心开关门状态
            CNC_Door_Close	= 0x0001	,//	加工中心自动门关闭(0未关闭1关闭）
            CNC_Door_Open	= 0x0002	,//	加工中心自动门打开(0未打开1关闭）
            CNC_Chuck_state	= 0x0004	,//	加工中心卡盘状态(0松开1夹紧)

            //从71 开始每个仓位4个Int数据，一共30个仓位，工120个数据， 
            //Mag_Scene = 71	,//	仓位1场次 A-Z
            //Mag_Fun_cur	,//	正在执行的工序号
            //Mag_Check	,//物料检测标识，1、不合格，0合格。
            Meterial=70	,//	0-铝件，1-钢件



            Mag_Scene = 71,//	仓位1场次 A-Z
            Mag_Type,//	零件类型,零件的编号（0_A,1-B,2-C,3-D）
            Mag_material,//零件材质，零件的类型（0-铝件，1-钢件）
            Mag_state,//	零件状态0空，1待加工，2正在加工，3合格品，4不合格品，5车床加工完成，6加工中心加工完成，7异常状态

            p_MeterValue1 = 191,//测量标定值符号位0-正数，1负数
            i_MeterValue1,//测量标定值1整数部分
            f_MeterValue1,//测量标定值1小数部分
            p_MeterValue2,//测量标定值符号位0-正数，1负数
            i_MeterValue2,//测量标定值2整数部分
            f_MeterValue2,//测量标定值2小数部分
            p_MeterValue3,//测量标定值符号位0-正数，1负数
            i_MeterValue3 ,//测量标定值3整数部分
            f_MeterValue3,//测量标定值3小数部分
            p_MeterValue4,//测量标定值符号位0-正数，1负数
            i_MeterValue4,//测量标定值4整数部分
            f_MeterValue4,//测量标定值4小数部分
            p_MeterValue5,//测量标定值符号位0-正数，1负数
            i_MeterValue5,//测量标定值5整数部分
            f_MeterValue5,//测量标定值5小数部分
            //机器人轴位置，3*7=21个 206-227
            p_Jpos = 206,
            i_Jpos ,
            f_Jpos,
            //机器人轴位置，3*6=18个 228-244
            p_InsideMeterpos = 227,
            i_InsideMeterpos,
            f_InsideMeterpos,
            //刀补补偿 3*10=30个，245-274
            RoughLength_Positive = 245,
            RoughLength_int,
            RoughLength_Float,
            RoughLengthWear_Positive,
            RoughLengthWear_int,
            RoughLengthWear_Float,
            RoughRadius_Positive ,
            RoughRadius_int,
            RoughRadius_Float,
            RoughRadiusWear_Positive,
            RoughRadiusWear_int,
            RoughRadiusWear_Float,
            RoughMeter_Positive,
            RoughMeter_int,
            RoughMeter_Float,

            FineLength_Positive,
            FineLength_int,
            FineLength_Float,
            FineLengthWear_Positive,
            FineLengthWear_int,
            FineLengthWear_Float,
            FineRadius_Positive,
            FineRadius_int,
            FineRadius_Float,
            FineRadiusWear_Positive,
            FineRadiusWear_int,
            FineRadiusWear_Float,
            FineMeter_Positive,
            FineMeter_int,
            FineMeter_Float,
            //料仓对应的模型编号，共30各0-1号模型，1-2号模型，2-3号模型，3-4号模型，275-314
            Mag1_Sheet_No = 275,//仓位图纸模型编号

              LineStoreBit = 305,//	线边库状态(0无料盘1有料盘）
              FinStoreBit = 306,//	成品库状态(0无料盘1有料盘）
              RawStore ,//	毛坯库状态(0无料盘1有料盘)
              Task_Raw_Line ,//	毛坯库到线边库
              Task_Fin_Line ,//	成品库到线边库
              Task_Line_Raw ,//	从线边库到毛坯库
              Task_Line_Fin ,//	从线边库到成品库
              Task_Finish ,//	AGV任务完成

            AGV_Arriv_RawStore =313,//AGV 到达毛坯库，1到达，0离开
            AGV_Arriv_FinStore ,//AGV 到达成品库，1到达，0离开
            AGV_Arriv_LineStore,//AGV 到达线边库，1到达，0离开
            AGV_Task_State ,//0:初始，4:任务创建成功，6:任务创建失败，16:成功完成，24:错误完成
            AGV_State,//AGV状态0：维护中、充电中  1：就绪 2：忙碌 4：故障 10：API接口错误  11：急停中
            AGV_Vol,//电量，百分比
            AGV_Beat,//心跳每秒加1 到1000后回1

        };
    
 
        public enum Mag_state_config
        {
            Statenull=0,//无料
            Statewait=1,//待加工
            StateProcessing = 2,//加工中      
            StateFinishStandard = 3,//加工完成合格
            StateFinishNotStandard = 4,//加工完成不合格    
            Statecnc1Finish = 5,//数车加工完成
            Statecnc2Finish = 6,//加工中心加工完成
            StateFailure = 7,//加工失败
        }
        public enum MesResponseToPlc//d011
        {
            ResUpGcode = 201,//请求上传程式		
            ResMachining = 202,//加工反馈 		
            ReswriteRfidDown = 203,//写RFID信息完成		
            ResreadRfidDown = 204,// 读RFID信息完成		
            ResMeterReq = 205,// 测量请求		
        }
        public enum PlcCommandToMes//d021
        {
            ComUpGcode = 201,//请求上传程式		
            ComMachining = 202,//加工反馈 		
            ComwriteRfidDown = 203,//写RFID信息完成		
            ComreadRfidDown = 204,// 读RFID信息完成		
            ComMeterReq= 205,// 测量请求	
        }
        public enum MesCommandToPlc//d001
        {		
            ComStartSys = 98,//98启动系统
            ComStopSys = 99,//99停止系统
            ComStartDevice = 100,//100启动设备
            ComProcessManage = 102,//102加工调度
            ComWriteRfid= 103,//103写RFID信息
            ComReadRfid = 104,//104 读RFID信息
            ComReProcess= 105,//105返修
            ComToRack = 106,//106 取料
        }
        public enum PlcResponseToMes//d031
        {
            ResStartSys = 98,//98启动系统
            ResStopSys = 99,//99停止系统
            ResStartDevice = 100,//100启动设备
            ResProcessManage = 102,//102加工调度
            ResWriteRfid = 103,//103写RFID信息
            ResReadRfid = 104,//104 读RFID信息
            ResReProcess = 105,//105返修
            ResToRack = 106,//106 取料
        }
  /// <summary>
        /// 获取数据
  /// </summary>
  /// <param name="Arrnum">数据编号</param>
  /// <param name="result">获取成功标识</param>
  /// <returns></returns>
        public int GetIntValue(int Arrnum,out bool result )
        {
            int value  = 0;
            if(Arrnum>=0&&Arrnum<ModbusBufferSize)
            {
               value =  DataMoubus[Arrnum];        
                result = true;
               return value ;
            }
            else
            {
                result = false ;
                return value = -1;
            }
            
        }

        /// <summary>
        ///  设置数据
        /// </summary>
        /// <param name="Arrnum">数据编号</param>
        /// <param name="value">设置值</param>
        /// <param name="result">设置成功标识</param>
         public void SetIntValue(int Arrnum,int value,out bool result  )
        {
             if(Arrnum>=0&&Arrnum<ModbusBufferSize)
            {
               DataMoubus[Arrnum] = value  ;        
                result = true;
                return;
            }
            else
            {
                result = false ;
                return;
            }
        }
 
        /// <summary>
         /// 获取点位信息
        /// </summary>
        /// <param name="Arrnum">数据编号</param>
        /// <param name="bit">点位</param>
        /// <param name="result">获取成功的标识</param>
        /// <returns></returns>
        public bool GetBoolValue(int Arrnum,int bit,out bool result)
        {
            bool Bitvalue = false ;
            Int32 temp = 0; 
            if(bit<0 ||bit>31)
            {
               result = false ;
                return Bitvalue = false;
            }
            else
            {
            if(Arrnum>=0&&Arrnum<ModbusBufferSize)
            {
               temp =  DataMoubus[Arrnum]&(1>>bit);  
                if(temp >0)
                {
                   Bitvalue= true;
                }
                else
                {
                    Bitvalue= false;
                }
                result = true;
               return Bitvalue ;
            }
            else
            {
                result = false ;
                return Bitvalue = false;
            }
            }
        }   
        
        /// <summary>
        /// 设置点位信息
        /// </summary>
        /// <param name="Arrnum">数据编号</param>
        /// <param name="bit">点位</param>
        /// <param name="result">设置成功标识</param>
        public void SetBoolValue(int Arrnum,int bit,out bool result)
        {
            bool Bitvalue = false ;
            Int32 temp = 0; 
            if(bit<0 ||bit>31)
            {
               result = false ;
               return;
            }
            else
            {
                if (Arrnum >= 0 && Arrnum < ModbusBufferSize)
                {
                    temp = DataMoubus[Arrnum] | ((1 >> bit));
                    if (temp > 0)
                    {
                        Bitvalue = true;
                    }
                    else
                    {
                        Bitvalue = false;
                    }
                    result = true;
                    return;
                }
                else
                {
                    result = false;
                    return;
                }
            }
        }
        /// <summary>
        /// 清除点位信息
        /// </summary>
        /// <param name="Arrnum"></param>
        /// <param name="bit"></param>
        /// <param name="result"></param>
         public void ClrBoolValue(int Arrnum,int bit,out bool result)
        {

            if(bit<0 ||bit>31)
            {
               result = false ;
            }
            else
            {
                if(Arrnum>=0&&Arrnum<ModbusBufferSize)
                {
                  DataMoubus[Arrnum] =  DataMoubus[Arrnum]&( ~(1>>bit))  ;
                  result = true;
                }
                else
                {
                    result = false ;
                }
            }
        }
        
       /// <summary>
         /// 获取料仓信息
       /// </summary>
       /// <param name="MagNo"> 料仓编号</param>
       /// <param name="item">料仓具体信息</param>
       /// <param name="result">获取成功标识</param>
       /// <returns></returns>
        public  Int32 GetMagValue(int MagNo,int item,out bool result)
        {
           int value  = 0;
           int magstartnum = Convert.ToInt32( DataConfigArr.Mag_Scene);
            if(MagNo<0 ||MagNo>=30||item<0||item>=4)
            {
                result =false ;
                return value= -1;
            }
            else 
            {
                value = DataMoubus[magstartnum + MagNo * 4 + item];        
                result = true;
               return value ;
            }
        }
      
         /// <summary>
        /// 设置料仓数据
         /// </summary>
        /// <param name="MagNo">料仓编号</param>
        /// <param name="item">料仓具体信息</param>
         /// <param name="value">设置的数据</param>
        /// <param name="result">获取成功标识</param>
          public void SetMagValue(int MagNo,int item,int value,out bool result)
        {
            int magstartnum = Convert.ToInt32(DataConfigArr.Mag_Scene);
            if (MagNo < 0 || MagNo >= 30 || item < 0 || item >= 4)
            {
                result = false;
                return ;
            }
            else
            {
                DataMoubus[magstartnum + MagNo * 4 + item]= value ;
                result = true;
                return ;
            }
        }
          public bool GetMagOnbit(int MagNo)
          {
              int magstartnum = Convert.ToInt32(DataConfigArr.Mag_state_1);
              if (MagNo <= 0 || MagNo >30)
              {
                  return false;
              }
              else
              {
                  if (MagNo <= 16)
                  {
                      int temp = 1 << (MagNo - 1);
                      if ((DataMoubus[magstartnum] & temp) == temp)
                      {
                          return true;
                      }
                      else
                          return false;
                  }
                  else
                  {
                      int temp = 1 << (MagNo - 17);
                      if ((DataMoubus[magstartnum+1] & temp) == temp)
                      {
                          return true;
                      }
                      else
                          return false;
                  }
              }
          }


          //private void SetPLCS7IP()
          //{//20180112
          //    tcpPort = 502;
          //    tcpIp = "";
          //}

    }
}
