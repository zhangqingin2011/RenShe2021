using System;
using System.Collections.Generic;

namespace HNCAPI_INTERFACE
{
    public enum ParaPropType
    {
    	PARA_PROP_VALUE,	// 参数值 参数结构文件定义
    	PARA_PROP_MAXVALUE,	// 最大值 参数结构文件定义
    	PARA_PROP_MINVALUE,	// 最小值 参数结构文件定义
    	PARA_PROP_DFVALUE,	// 缺省值 参数结构文件定义
    	PARA_PROP_NAME,		// 名字  STRING
    	PARA_PROP_ACCESS,	// 权限  INT
    	PARA_PROP_ACT,		// 生效方式 INT
    	PARA_PROP_STORE,	// 存储类型  INT
    	PARA_PROP_ID,		// 参数编号 INT
    	PARA_PROP_SIZE,		// 大小 INT
        PARA_PROP_RELATIONID// 扩展参数
    };

    public enum ParaActType
    {
    	PARA_ACT_SAVE, // 保存生效
    	PARA_ACT_NOW,  // 立即生效
    	PARA_ACT_RST,  // 复位生效
    	PARA_ACT_PWR,  // 重启生效
    	PARA_ACT_HIDE, // 隐藏未启用
        VENDER_RIGHTS
    };

    public enum ParaSubClassProp
    {
    	SUBCLASS_NAME,		// 子类名
    	SUBCLASS_ROWNUM,	// 子类行数
    	SUBCLASS_NUM,		// 子类数
    	SUBCLASS_MAXNUM     //最大行数
    };

    public class HNCPARAMAN
    {
       public const Int32 DATA_STRING_LEN =  64 ;
       public const Int32 PARA_FILE_OPEN_FAIL =  -1  ;//  文件打开失败 
       public const Int32 PARA_FILE_SEEK_FAIL =  -2  ;//  文件seek失败 
       public const Int32 PARA_FILE_READ_FAIL =  -3  ;//  文件读失败 
       public const Int32 PARA_FILE_WRITE_FAIL =  -4  ;//  文件写失败 
       public const Int32 PARA_FILE_DATA_FAIL =  -5  ;//  文件数据错误 
       public const Int32 PARA_POINT_NULL_ERR =  -6  ;//  空指针 
       public const Int32 PARA_FILENO_ERR =  -7  ;//  参数类别错误 
       public const Int32 PARA_SUBNO_ERR =  -8  ;//  子类号越界 
       public const Int32 PARA_ROWNO_ERR =  -9  ;//  行号越界 
       public const Int32 PARA_ROWXNO_ERR =  -10  ;//  总行号越界 
       public const Int32 PARA_INDEX_ERR =  -11  ;//  索引越界 
       public const Int32 PARA_STRING_LIMIT =  -12  ;//  字符串太长 
       public const Int32 PARA_PARMNO_ERR =  -13  ;//  参数号越界 
       public const Int32 PARA_PARMANTYPE_ERR =  -14  ;//  参数类型错误 
       public const Int32 PARM_MAPINDEX_ERR =  -15  ;//  参数索引号错 
       public const Int32 PARM_MAPROWNO_ERR =  -16  ;//  参数行号错 
       public const Int32 PARM_SVWRITE_ERR =  -17  ;//  写伺服参数失败 
       public const Int32 PARM_SVSAVE_ERR =  -18  ;//  保存伺服参数失败 
       public const Int32 PAEM_SVSTATE_ERR =  -19  ;//  总线通讯未准备好,无法写入伺服参数 
       public const Int32 PAEM_SVRESET_ERR =  -20  ;//  总线未成功复位 
       public const Int32 PARM_XML_VERSION_ERR = -21  ;// 参数结构版本不兼容
       public const Int32 PARM_EXPRESION_ERR = -22  ;// 参数计算公式失败
    }

    public enum ParNcu
    {
        PAR_NCU_TYPE,			// NCU控制器类型
        PAR_NCU_CYCLE,			// 插补周期
        PAR_NCU_PLC2_CMDN,		// PLC2周期执行语句数

        PAR_NCU_ANG_RESOL = 5,	// 角度计算分辨率
        PAR_NCU_LEN_RESOL,		// 长度计算分辨率
        PAR_NCU_TIME_RESOL,		// 时间编程分辨率
        PAR_NCU_VEL_RESOL,		// 线速度编程分辨率
        PAR_NCU_SPDL_RESOL,		// 角速度编程分辨率
        PAR_NCU_ARC_PROFILE,	// 圆弧插补轮廓允许误差
        PAR_NCU_MAX_RAD_ERR,	// 圆弧编程端点半径允许偏差
        PAR_NCU_G43_SW_MODE,	// 刀具轴选择方式[0,固定z向;1,G17/18/19切换;2,G43指令轴切换]
        PAR_NCU_G41_G00_G01,	// G00插补使能
        PAR_NCU_G53_LEN_BACK,	// G53之后自动恢复刀具长度补偿[0,不恢复 1 恢复]
        PAR_NCU_CRDS_NUM,	    // 允许联动轴数
        PAR_NCU_LAN_EN,			// 局域网使能
        PAR_NCU_POWER_SAFE,		// 断电保护使能
        PAR_NCU_TIME_EN,		// 系统时间显示使能
        PAR_NCU_PSW_CHECK,		// 权限检查使能
        PAR_NCU_ALARM_POP,		// 报警窗口自动显示使能
        PAR_NCU_KBPLC_EN,       // 键盘PLC使能
        PAR_NCU_GRAPH_ERAS_EN,  // 图形自动擦除使能	
        PAR_NCU_FSPD_DISP,		// F进给速度显示方式	
        PAR_NCU_GLNO_DISP,		// G代码行号显示方式
        PAR_NCU_INCH_DISP,	    // 公制/英制选择
        PAR_NCU_DISP_DIGITS,	// 位置小数点后显示位数
        PAR_NCU_FEED_DIGITS,	// 速度小数点后显示位数
        PAR_NCU_SPINDLE_DIGITS,	// 转速小数点后显示位数
        PAR_NCU_LANGUAGE,		// 语言选择
        PAR_NCU_LCD_TIME,		// 进入屏保等待时间
        PAR_NCU_DISK_TYPE,		// 外置程序存储类型
        PAR_NCU_REFRE_INTERV,   // 界面刷新间隔时间
        PAR_NCU_SAVE_TYPE,      // 是否外接UPS
        PAR_NCU_OPERATE_NOTE,   /*!< 操作提示使能[0位：重运行 1位：Tool->相对实际 2位：Tool->当前位置] */
        PAR_NCU_SERVER_NAME,
        PAR_NCU_SERVER_IP1,
        PAR_NCU_SERVER_IP2,
        PAR_NCU_SERVER_IP3,
        PAR_NCU_SERVER_IP4,
        PAR_NCU_SERVER_PORT,    // 服务器端口号
        PAR_NCU_SERVER_LOGIN,   // 服务器访问用户名
        PAR_NCU_SERVER_PASSWD,  // 服务器访问密码
        PAR_NCU_FTP_ADMIT,      // FTP是否验证权限
        PAR_NCU_NET_TYPE = 44,  // 网盘映射类型
        PAR_NCU_IP1,            // IP地址段1
        PAR_NCU_IP2,            // IP地址段2
        PAR_NCU_IP3,            // IP地址段3
        PAR_NCU_IP4,            // IP地址段4
        PAR_NCU_PORT,           // 本地端口号
        PAR_NCU_NET_START,		// 是否开启网络
        PAR_NCU_SERIAL_TYPE,	// 串口类型
        PAR_NCU_SERIAL_NO = 52,	// 串口号
        PAR_NCU_DATA_LEN,       // 收发数据长度
        PAR_NCU_STOP_BIT,       // 停止位
        PAR_NCU_VERIFY_BIT,     // 校验位
        PAR_NCU_BAUD_RATE,      // 波特率
        PAR_NCU_IP_TYPE,        // 静态IP/动态IP

        PAR_NCU_TOOL_NUM = 60,	// 最大刀具数
        PAR_NCU_TOFF_DIGIT,     // 刀补有效位数
        PAR_NCU_MAGZ_NUM,		// 最大刀库数
        PAR_NCU_TOOL_LOCATION,	// 最大刀座数
        PAR_NCU_TABRA_ADD_EN,   // 刀具磨损累加使能
        PAR_NCU_TDIA_SHOW_EN,   // 车刀直径显示使能
        PAR_NCU_SUB_PROG_EN,    // 全局子程序调用使能
        PAR_NCU_TRANS_ORDER,	// 镜像缩放旋转嵌套次序
        // 【0 旋转->缩放->镜像 1 镜像->缩放->旋转 2 自由编程，自动整理成按镜像->缩放->旋转的次序实施变换 
        // 3 按照实际的编程次序实施变换； 0/1/2三种选择时，都会按照镜像->缩放->旋转的次序实施变换】
        PAR_NCU_CYCLE_OPT,		// 复合循环路径选项【0x00FF: 0 常规  1 退刀段效率优先 2 FANUC兼容  &0xFF00 = =  0x0000 : 45度退刀 0x0100: 径向退刀 
        // &0x0200 = =  0x0200时最后一刀直接退到循环起点，凹槽中有台阶时不要用此选项】
        PAR_NCU_HOLD_DECODE_EN,	// 进给保持后重新解释使能
        PAR_NCU_G28_LEN_BACK,	// G28后是否自动恢复刀长补
        PAR_NCU_SPEEDUP_EN,    // 内部调试用，加速使能

        PAR_NCU_LOG_SAVE_TYPE = 80, // 日志文件保存类型

        PAR_NCU_INTERNET_IP1,	// 网络平台服务器IP
        PAR_NCU_INTERNET_IP2,
        PAR_NCU_INTERNET_IP3,
        PAR_NCU_INTERNET_IP4,
        PAR_NCU_INTERNET_PORT,	//网络平台服务器端口

        PAR_NCU_HMI = 100,					// 界面设置参数基地址
        PAR_NCU_ISSU_EDITION = PAR_NCU_HMI,	// 发布版本号
        PAR_NCU_TEST_EDITION,				// 测试版本号
        PAR_NCU_SHOW_LIST,					// 示值列，40
        PAR_NCU_GRAPH = PAR_NCU_SHOW_LIST + 40,	// 图形参数，90

        PAR_NCU_ALARM_LOG_NUM_LIMIT = 280,	//	日志条目限制
        PAR_NCU_WORKINFO_LOG_NUM_LIMIT,
        PAR_NCU_FILECHANGE_LOG_NUM_LIMIT,
        PAR_NCU_PANEL_LOG_NUM_LIMIT,
        PAR_NCU_DEFINE_LOG_NUM_LIMIT,
        PAR_NCU_EVENT_LOG_NUM_LIMIT,

        PAR_NCU_ALARM_LOG_TIME_LIMIT = 290,	//	日志时间限制
        PAR_NCU_WORKINFO_LOG_TIME_LIMIT,
        PAR_NCU_FILECHANGE_LOG_TIME_LIMIT,
        PAR_NCU_PANEL_LOG_TIME_LIMIT,
        PAR_NCU_DEFINE_LOG_TIME_LIMIT,
        PAR_NCU_EVENT_LOG_TIME_LIMIT,

        PAR_NCU_PROG_PATH = 300,	// 加工代码程序路径
        PAR_NCU_PLC_PATH,		// PLC程序路径
        PAR_NCU_PLC_NAME,		// PLC程序名
        PAR_NCU_DRV_PATH,		// 驱动程序路径
        PAR_NCU_DRV_NAME,		// 驱动程序名
        PAR_NCU_PARA_PATH,		// 参数文件路径
        PAR_NCU_PARA_NAME,		// 参数文件名
        PAR_NCU_SIMU_PATH,		// 仿真配置文件路径
        PAR_NCU_SIMU_NAME,		// 仿真配置文件名
        PAR_NCU_DLGP_PATH,		// 对话编程配置文件路径
        PAR_NCU_DLGP_NAME,		// 对话编程配置文件名
        PAR_NCU_VIDEO_DEV,      // 视频外设驱动

        PAR_NCU_SUB_DBG = 320,	// 宏程序单段使能【WIN】
        PAR_NCU_USER_LOGIN = 321,	// 	是否开启用户登录?
        PAR_NCU_G16_OPT = 350,	// G16的极点定义模式选择 0：FANUC模式 1：HNC-8模式
        PAR_NCU_GEDIT_FRAME = 351,	// 编辑界面框架选择 0：HNC-8模式 1：宁江专机模式
        PAR_NCU_TOTAL = 500		// NC参数总数
    }

    // 机床用户参数
    public enum ParMac
    {
        PAR_MAC_CHAN_NUM,	// 通道数：1~SYS_CHAN_NUM
        PAR_MAC_CHAN_TYPE,	// 机床通道加工类型【车、铣床、磨】
        PAR_MAC_CHAN_FLAG = PAR_MAC_CHAN_TYPE + 8,	// 通道选择标志
        PAR_MAC_CHAN_AX_FLAG = PAR_MAC_CHAN_FLAG + 8,	// 通道的轴显示标志，每个通道占用两个参数，共8*2 = 16个参数
        PAR_MAC_CHAN_CUR_FLAG = PAR_MAC_CHAN_AX_FLAG + 16,	// 通道的负载电流显示轴定制
        PAR_MAC_SHOW_AXES = 41,		// 是否动态显示坐标轴
        PAR_MAC_CALIB_TYPE,			// 刀具测量仪类型
        PAR_MAC_HOME_BLOCK = 48,	// 机床是否安装回零挡块
        PAR_MAC_AXES_NUM,			// 机床总轴数
        PAR_MAC_SMX_AXIS_NUM,		// 运动控制通道轴数（耦合从轴+PMC轴）
        PAR_MAC_SMX_AXIS_IDX,		// PMC及耦合从轴编号，预留32个

        PAR_MAC_TOOL = 100,	// 刀具处理参数预留

        PAR_MAC_FZONE_IN_MASK = 110,	//机床保护区内部禁止掩码
        PAR_MAC_FZONE_EX_MASK,		//机床保护区外部禁止掩码
        PAR_MAC_FZONE_BND,		//机床保护区边界-x +x -y +y -z +z 6*6=36

        PAR_MAC_HOME_DWELL = 165,	// 回参考点延时时间，单位：ms
        PAR_MAC_PLCACK_CYCLE,		// PLC应答最长时间
        PAR_MAC_G32_HOLD_DX,		// 螺纹加工中止的退刀距离
        PAR_MAC_G32_HOLD_ANG,		// 螺纹加工中止的退刀角度
        PAR_MAC_G64_CORNER_CHK,		// G64拐角准停校验检查使能

        PAR_MAC_MCODE_FLAG = 170,	// M代码属性表

        PAR_MAC_WKP_GMODE_SHOW = 220,	// 模态G指令显示定制，每个工位占用3个参数，共8*3 = 24个参数

        PAR_MAC_MEAS_SPD = 250,	// 测量速度
        PAR_MAC_MEAS_DIST,		// 测量最小行程

        PAR_MAC_IPSYNC_FUN = 260,	// 插补同步函数注册

        PAR_MAC_SPECIAL = 270,		// 专机预留参数起始地址

        PAR_MAC_CHECK_ENCRYPT = 298,	// 是否检查文件加密属性
        PAR_MAC_PROG_SKEY = 299,		// G代码文件密钥

        PAR_MAC_USER = 300,	// 用户参数基地址

        PAR_MAC_TOTAL = 500	// 机床用户参数总数
    }

    // 通道参数
    public enum ParCh
    {
        PAR_CH_NAME = 0,	// 名称
        PAR_CH_XINDEX,		// X轴编号
        PAR_CH_YINDEX,		// Y轴编号
        PAR_CH_ZINDEX,		// Z轴编号
        PAR_CH_AINDEX,		// A轴编号
        PAR_CH_BINDEX,		// B轴编号
        PAR_CH_CINDEX,		// C轴编号
        PAR_CH_UINDEX,		// U轴编号
        PAR_CH_VINDEX,		// V轴编号
        PAR_CH_WINDEX,		// W轴编号
        PAR_CH_SPDL0,		// 主轴0编号
        PAR_CH_SPDL1,		// 主轴1编号
        PAR_CH_SPDL2,		// 主轴2编号
        PAR_CH_SPDL3,		// 主轴3编号
        PAR_CH_X_NAME,		// X轴名
        PAR_CH_Y_NAME,		// Y轴名
        PAR_CH_Z_NAME,		// Z轴名
        PAR_CH_A_NAME,		// A轴名
        PAR_CH_B_NAME,		// B轴名
        PAR_CH_C_NAME,		// C轴名
        PAR_CH_U_NAME,		// U轴名
        PAR_CH_V_NAME,		// V轴名
        PAR_CH_W_NAME,		// W轴名
        PAR_CH_S0_NAME,		// 主轴0名
        PAR_CH_S1_NAME,		// 主轴1名
        PAR_CH_S2_NAME,		// 主轴2名
        PAR_CH_S3_NAME,		// 主轴3名
        PAR_CH_SVEL_SHOW,	// 主轴转速显示方式
        PAR_CH_S_SHOW,		// 主轴显示定制

        PAR_CH_DEFAULT_F = 30,	// 通道的缺省进给速度
        PAR_CH_DRYRUN_SPD,		// 空运行进给速度
        PAR_CH_DIAPROG,			// 直径编程使能
        PAR_CH_UVW_INC_EN,		// UVW增量编程使能
        PAR_CH_CHAMFER_EN,		// 倒角使能
        PAR_CH_ANGLEP_EN,		// 角度编程使能
        PAR_CH_CYCLE_OPTION,	// 复合循环选项屏蔽字[位]：0x0001 粗加工圆弧转直线 0x0002：凹槽轴向余量报警屏蔽 0x0004: 无精加工

        PAR_CH_MAC_FRAME = 40,	// 机床结构类型【0, 一般直角系机床 1, 通用五轴机床；2+其它机床】

        PAR_CH_FOLLOW_ROTATE_RAD = 60, //工具跟随的摆动半径
        PAR_CH_FOLLOW_CHORD_LEN, //弦线跟随的弦长

        PAR_CH_VPLAN_MODE = 69,	// 速度规划模式0-9:曲面模式  10+高速模式【激光、木工】 

        PAR_CH_MICR_MAX_LEN,	// 微线段上限长度
        PAR_CH_CORNER_MAX_ANG,	// 工艺尖角最大夹角
        PAR_CH_VEL_FILTER_LEN,	// 微线段速度滤波长度
        PAR_CH_PATH_TOLERANCE,	// 轨迹轮廓允差
        PAR_CH_GEO_CNTR_NUM,	// 微线段特征滤波段数
        PAR_CH_HSPL_MIN_LEN,	// 微线段下限长度
        PAR_CH_HSPL_MAX_ANG,	// 样条过渡夹角
        PAR_CH_HSPL_MAX_RAT,	// 样条平滑的相邻段最大长度比
        PAR_CH_HSPL_MAX_LEN,	// 样条平滑的最大线段长度
        PAR_CH_PATH_TYPE,		// 切削轨迹类型 79

        PAR_CH_LOOKAHEAD_NUM,	// 速度规划前瞻段数
        PAR_CH_CURVATURE_COEF,	// 曲率半径调整系数【0.3~100.0】
        PAR_CH_RECTIFY_NUM,		// 速度整定段数
        PAR_CH_POS_SMTH_NUM,	// 平滑段数
        PAR_CH_MAX_ECEN_ACC,	// 向心加速度
        PAR_CH_MAX_TANG_ACC,	//切向加速度

        PAR_CH_CYL_RAX = 90,	// 圆柱插补旋转轴号【缺省5、C轴】
        PAR_CH_CYL_LAX,			// 圆柱插补直线【轴向】轴号【缺省2、Z轴】
        PAR_CH_CYL_PAX,			// 圆柱插补平行【周向、纬线】轴号【缺省1、Y轴】

        PAR_CH_POLAR_LAX = 95,	// 极坐标插补的直线轴轴号
        PAR_CH_POLAR_RAX,		// 极坐标插补的旋转轴轴号
        PAR_CH_POLAR_VAX,		// 极坐标插补的假想轴轴号
        PAR_CH_POLAR_CX,		// 极坐标插补的旋转中心直线轴坐标
        PAR_CH_POLAR_CY,		// 极坐标插补假想轴偏心量

        PAR_CH_THREAD_TOL = 105,	// 螺纹起点允许偏差
        PAR_CH_THREAD_WAY,			// 螺纹加工方式


        PAR_CH_G61_DEFAULT,		// 系统上电时G61/G64模态设置
        PAR_CH_G00_DEFAULT,		// 系统上电时G00/G01模态设置
        PAR_CH_G90_DEFAULT,		// 系统上电时G90/G91模态设置
        PAR_CH_G28_ZTRAP_EN,	// G28搜索Z脉冲使能
        PAR_CH_G28_POS_EN,		// G28不寻Z脉冲时快移使能 【0 就进给速度定位 1 快移速度定位】
        PAR_CH_G28_ONE_SHOT,	// G29
        PAR_CH_SKIP_MODE,		// 任意行模式[0,扫描 1,跳转]

        // PAR_CH_G95_DEFAULT,		// 系统上电时G95/G94模态设置

        PAR_CH_MAG_START_NO = 125,	// 	起始刀库号
        PAR_CH_MAG_NUM,				// 	刀库数目
        PAR_CH_TOOL_START_NO,		// 	起始刀具号
        PAR_CH_TOOL_NUM,			// 	刀具数目
        PAR_CH_LIFE_ON,				// 	刀具寿命功能开启

        //  = 140 第2套小线段参数
        PAR_CH_MICR_MAX_LEN2 = 140,	// 微线段上限长度
        PAR_CH_CORNER_MAX_ANG2,		// 工艺尖角最大夹角
        PAR_CH_VEL_FILTER_LEN2,		// 微线段速度滤波长度
        PAR_CH_PATH_TOLERANCE2,		// 轨迹轮廓允差
        PAR_CH_GEO_CNTR_NUM2,		// 微线段特征滤波段数
        PAR_CH_HSPL_MIN_LEN2,		// 微线段下限长度
        PAR_CH_HSPL_MAX_ANG2,		// 样条过渡夹角
        PAR_CH_HSPL_MAX_RAT2,		// 样条平滑的相邻段最大长度比
        PAR_CH_HSPL_MAX_LEN2,		// 样条平滑的最大线段长度
        PAR_CH_PATH_TYPE2,			// 切削轨迹类型
        PAR_CH_LOOKAHEAD_NUM2,		// 速度规划前瞻段数
        PAR_CH_CURVATURE_COEF2,		// 曲率半径调整系数【0.3~100.0】
        PAR_CH_RECTIFY_NUM2,		// 速度整定段数
        PAR_CH_POS_SMTH_NUM2,		// 位置平滑段数

        //  = 160 第3套小线段参数
        PAR_CH_MICR_MAX_LEN3 = 160,	// 微线段上限长度
        PAR_CH_CORNER_MAX_ANG3,		// 工艺尖角最大夹角
        PAR_CH_VEL_FILTER_LEN3,		// 微线段速度滤波长度
        PAR_CH_PATH_TOLERANCE3,		// 轨迹轮廓允差
        PAR_CH_GEO_CNTR_NUM3,		// 微线段特征滤波段数
        PAR_CH_HSPL_MIN_LEN3,		// 微线段下限长度
        PAR_CH_HSPL_MAX_ANG3,		// 样条过渡夹角
        PAR_CH_HSPL_MAX_RAT3,		// 样条平滑的相邻段最大长度比
        PAR_CH_HSPL_MAX_LEN3,		// 样条平滑的最大线段长度
        PAR_CH_PATH_TYPE3,			// 切削轨迹类型
        PAR_CH_LOOKAHEAD_NUM3,		// 速度规划前瞻段数
        PAR_CH_CURVATURE_COEF3,		// 曲率半径调整系数【0.3~100.0】
        PAR_CH_RECTIFY_NUM3,		// 速度整定段数
        PAR_CH_POS_SMTH_NUM3,		// 位置平滑段数

        //  = 180 第4套小线段参数
        PAR_CH_MICR_MAX_LEN4 = 180,	// 微线段上限长度
        PAR_CH_CORNER_MAX_ANG4,		// 工艺尖角最大夹角
        PAR_CH_VEL_FILTER_LEN4,		// 微线段速度滤波长度
        PAR_CH_PATH_TOLERANCE4,		// 轨迹轮廓允差
        PAR_CH_GEO_CNTR_NUM4,		// 微线段特征滤波段数
        PAR_CH_HSPL_MIN_LEN4,		// 微线段下限长度
        PAR_CH_HSPL_MAX_ANG4,		// 样条过渡夹角
        PAR_CH_HSPL_MAX_RAT4,		// 样条平滑的相邻段最大长度比
        PAR_CH_HSPL_MAX_LEN4,		// 样条平滑的最大线段长度
        PAR_CH_PATH_TYPE4,			// 切削轨迹类型
        PAR_CH_LOOKAHEAD_NUM4,		// 速度规划前瞻段数
        PAR_CH_CURVATURE_COEF4,		// 曲率半径调整系数【0.3~100.0】
        PAR_CH_RECTIFY_NUM4,		// 速度整定段数
        PAR_CH_POS_SMTH_NUM4,		// 位置平滑段数

        PAR_CH_WTZONE_NUM = 200,	// 工件及刀具保护区总个数0~10
        PAR_CH_WTZONE_TYPE,			// 工件及刀具保护区类型
        PAR_CH_WTZONE_FLAG,			// 工件及刀具保护区属性
        PAR_CH_WTZONE_BND,			// 工件及刀具保护区边界

        PAR_CH_RESONA_DAMP_AMP = 300, // 主轴转速避振波幅【百分比 0.05】
        PAR_CH_RESONA_DAMP_PRD = 301, // 主轴转速避振周期【秒】

        PAR_CH_TAX_ENABLE = 310,	// 倾斜轴控制使能
        PAR_CH_TAX_ORTH_AX_INDEX,	// 正交轴轴号
        PAR_CH_TAX_TILT_AX_INDEX,	// 倾斜轴轴号
        PAR_CH_TAX_TILT_ANGLE,		// 倾斜角度

        //五轴参数
        PAR_CH_RTCPARA_OFF = 50,		//RTCP参数偏移值

        PAR_CH_TOOL_INIT_DIR_X = 400,	//刀具初始方向(X)
        PAR_CH_TOOL_INIT_DIR_Y,		//刀具初始方向(Y)
        PAR_CH_TOOL_INIT_DIR_Z,		//刀具初始方向(Z)
        PAR_CH_ANG_OUTPUT_MODE = 405,	//旋转轴角度输出判定方式
        PAR_CH_ANG_OUTPUT_ORDER,	//旋转轴角度输出判定顺序
        PAR_CH_POLE_TOLERANCE,		//极点角度范围

        PAR_CH_RTCP_SWIVEL_TYPE = 410,	//摆头结构类型
        PAR_CH_RTCP_SWIVEL_RAX1_DIR_X,	//摆头第1旋转轴方向(X)
        PAR_CH_RTCP_SWIVEL_RAX1_DIR_Y,	//摆头第1旋转轴方向(Y)
        PAR_CH_RTCP_SWIVEL_RAX1_DIR_Z,	//摆头第1旋转轴方向(Z)
        PAR_CH_RTCP_SWIVEL_RAX2_DIR_X,	//摆头第2旋转轴方向(X)
        PAR_CH_RTCP_SWIVEL_RAX2_DIR_Y,	//摆头第2旋转轴方向(Y)
        PAR_CH_RTCP_SWIVEL_RAX2_DIR_Z,	//摆头第2旋转轴方向(Z)
        PAR_CH_RTCP_SWIVEL_RAX1_OFF_X,	//摆头第1旋转轴偏移矢量(X)
        PAR_CH_RTCP_SWIVEL_RAX1_OFF_Y,	//摆头第1旋转轴偏移矢量(Y)
        PAR_CH_RTCP_SWIVEL_RAX1_OFF_Z,	//摆头第1旋转轴偏移矢量(Z)
        PAR_CH_RTCP_SWIVEL_RAX2_OFF_X,	//摆头第2旋转轴偏移矢量(X)
        PAR_CH_RTCP_SWIVEL_RAX2_OFF_Y,	//摆头第2旋转轴偏移矢量(Y)
        PAR_CH_RTCP_SWIVEL_RAX2_OFF_Z,	//摆头第2旋转轴偏移矢量(Z)

        PAR_CH_RTCP_TABLE_TYPE = 425,		//转台结构类型
        PAR_CH_RTCP_TABLE_RAX1_DIR_X,	//转台第1旋转轴方向(X)
        PAR_CH_RTCP_TABLE_RAX1_DIR_Y,	//转台第1旋转轴方向(Y)
        PAR_CH_RTCP_TABLE_RAX1_DIR_Z,	//转台第1旋转轴方向(Z)
        PAR_CH_RTCP_TABLE_RAX2_DIR_X,	//转台第2旋转轴方向(X)
        PAR_CH_RTCP_TABLE_RAX2_DIR_Y,	//转台第2旋转轴方向(Y)
        PAR_CH_RTCP_TABLE_RAX2_DIR_Z,	//转台第2旋转轴方向(Z)
        PAR_CH_RTCP_TABLE_RAX1_OFF_X,	//转台第1旋转轴偏移矢量(X)
        PAR_CH_RTCP_TABLE_RAX1_OFF_Y,	//转台第1旋转轴偏移矢量(Y)
        PAR_CH_RTCP_TABLE_RAX1_OFF_Z,	//转台第1旋转轴偏移矢量(Z)
        PAR_CH_RTCP_TABLE_RAX2_OFF_X,	//转台第2旋转轴偏移矢量(X)
        PAR_CH_RTCP_TABLE_RAX2_OFF_Y,	//转台第2旋转轴偏移矢量(Y)
        PAR_CH_RTCP_TABLE_RAX2_OFF_Z,	//转台第2旋转轴偏移矢量(Z)

        PAR_CH_TOTAL = 500
    }

    // 坐标轴参数
    public enum ParAxis
    {
        PAR_AX_NAME = 0,	// 轴名[显示用]
        PAR_AX_TYPE,		// 轴类型[直线、摆动、回转、主轴]
        PAR_AX_INDEX,		// 轴编号 暂时预留
        PAR_AX_MODN,		// 设备号 暂时预留
        PAR_AX_DEV_I = PAR_AX_MODN,
        PAR_AX_PM_MUNIT,	// 电子齿轮比分子(位移量)[每转位移量nm]
        PAR_AX_PM_PULSE,	// 电子齿轮比分母(脉冲数)[每转指令脉冲数]
        PAR_AX_PLMT,		// 正软极限
        PAR_AX_NLMT,		// 负软极限
        PAR_AX_PLMT2,		// 第2正软极限
        PAR_AX_NLMT2,		// 第2负软极限

        PAR_AX_HOME_WAY = 10,	// 回参考点方式
        PAR_AX_HOME_DIR,		// 回参考点方向
        PAR_AX_ENC_OFF,			// 编码器反馈偏置量【手动零点、绝对式编码器】
        PAR_AX_HOME_OFF,		// 回参考点后的偏移量
        PAR_AX_HOME_MASK,		// Z脉冲屏蔽角度
        PAR_AX_HOME_HSPD,		// 回参考点高速
        PAR_AX_HOME_LSPD,		// 回参考点低速
        PAR_AX_HOME_CRDS,		// 参考点坐标值
        PAR_AX_HOME_CODSPACE,	// 距离码参考点间距
        PAR_AX_HOME_CODOFF,		// 间距编码偏差

        PAR_AX_HOME_RANGE = 20,	// 搜Z脉冲最大移动距离
        PAR_AX_HOME_CRDS2,		// 第2参考点坐标值
        PAR_AX_HOME_CRDS3,		// 第3参考点坐标值
        PAR_AX_HOME_CRDS4,		// 第4参考点坐标值
        PAR_AX_HOME_CRDS5,		// 第5参考点坐标值
        PAR_AX_REF_RANGE,		// 参考点范围偏差
        PAR_AX_HOME_CYCLE_OFF,	// 非整传动比回转轴偏差
        PAR_AX_ENC2_OFF,		// 第2编码器反馈偏置量【手动零点、绝对式编码器】
        PAR_AX_PM2_MUNIT,		// 第2编码器电子齿轮比分子(位移量)[每转位移量nm]
        PAR_AX_PM2_PULSE,		// 第2编码器电子齿轮比分母(脉冲数)[每转指令脉冲数]

        PAR_AX_G60_OFF = 30,	// 单向定位(G60)偏移量
        PAR_AX_ROT_RAD,			// 转动轴当量半径
        PAR_AX_JOG_LOWSPD,		// 慢速点动速度
        PAR_AX_JOG_FASTSPD,		// 快速点动速度
        PAR_AX_RAPID_SPD,		// 快移速度
        PAR_AX_FEED_SPD,		// 最高进给速度
        PAR_AX_RAPID_ACC,		// 快移加速度
        PAR_AX_RAPID_JK,		// 快移捷度
        PAR_AX_FEED_ACC,		// 进给加速度
        PAR_AX_FEED_JK,			// 进给捷度
        PAR_AX_THREAD_ACC,		// 螺纹加速度
        PAR_AX_THREAD_DEC,		// 螺纹减速度
        PAR_AX_MPG_UNIT_SPD,	// 手摇单位速度比例系数
        PAR_AX_MPG_RESOL,		// 手摇脉冲分辨率
        PAR_AX_MPG_INTE_RATE,	// 手摇缓冲系数
        PAR_AX_MPG_INTE_TIME,	// 手摇缓冲周期 [45]
        PAR_AX_MPG_OVER_RATE,	// 手摇过冲系数
        PAR_AX_MPG_VEL_GAIN,	// 手摇速度调节系数

        PAR_AX_DEFAULT_S = 50,	// 主轴缺省转速
        PAR_SPDL_MAX_SPEED,		// 主轴最大转速
        PAR_SPDL_SPD_TOL,		// 主轴转速允许转速波动率
        PAR_SPDL_SPD_TIME,		// 主轴转速到达允许最大时间
        PAR_SPDL_THREAD_TOL,	// 螺纹加工时的转速允差
        PAR_AX_SP_ORI_POS,		// 进给主轴定向角度
        PAR_AX_SP_ZERO_TOL,		// 进给主轴零速允差【脉冲】
        PAR_AX_MAX_EXT_PINC,	// 外部指令最大周期叠加量

        PAR_AX_POS_TOL = 60,	// 定位允差
        PAR_AX_MAX_LAG,			// 最大跟随误差
        PAR_AX_LAG_CMP_EN,		// 龙门轴同步误差补偿使能
        PAR_AX_LAG_CMP_COEF,	// 跟随误差补偿调整系数
        PAR_AX_LAG_CMP_CNT,		// 动态补偿系数整定周期数

        PAR_AX_ATEETH = 65,	// 传动比分子[轴侧齿数]
        PAR_AX_MTEETH,		// 传动比分母[电机侧齿数]
        PAR_AX_MT_PPR,		// 电机每转脉冲数
        PAR_AX_PITCH,		// 丝杆导程
        PAR_AX_RACK_NUM,	// 齿条齿数
        PAR_AX_RACK_SPACE,	// 齿条齿间距
        PAR_AX_WORM_NUM,	// 蜗杆头数
        PAR_AX_WORM_SPACE,	// 蜗杆齿距
        PAR_RAX_VEL_RATE,   // 旋转轴速度系数
        PAR_AX_RATING_CUR,  // 额定电流
        PAR_AX_POWER_RATE,  // 功率系数
        PAR_AX_ENC2_PPR,	// 第2编码器每转脉冲数
        PAR_AX_INDEX_TYPE, //分度轴类型：1、鼠牙盘；2，分度轴
        PAR_AX_INDEX_POS,//分度起点
        PAR_AX_INDEX_DIVIDE, //分度间隔

        PAR_ZAX_LOCK_EN = 80,	// Z轴锁允许使能
        PAR_RAX_ROLL_EN,		// 旋转轴循环使能
        PAR_RAX_SHORTCUT,		// 旋转轴短路径选择使能
        PAR_RAX_CYCLE_RANGE,	// 旋转轴循环行程
        PAR_RAX_DISP_RANGE,		// 旋转轴显示角度范围
        PAR_LAX_PROG_UNIT,		// 直线轴编程指令最小单位
        PAR_RAX_PROG_UNIT,		// 旋转轴编程指令最小单位

        PAR_AX_ENC_MODE = 90,	// 编码器工作模式

        PAR_AX_EC1_TYPE,		// 1号编码器类型【增量、距离码、绝对】
        PAR_AX_EC1_OUTP,		// 反馈电子齿轮比分子[输出脉冲数]
        PAR_AX_EC1_FBKP,		// 反馈电子齿轮比分母[反馈脉冲数]
        PAR_AX_EC1_BIT_N,		// 1号编码器计数位数【绝对式必填】
        PAR_AX_EC2_TYPE,		// 2号编码器类型【增量、距离码、绝对】
        PAR_AX_EC2_OUTP,		// 反馈电子齿轮比分子[输出脉冲数] 
        PAR_AX_EC2_FBKP,		// 反馈电子齿轮比分母[反馈脉冲数]
        PAR_AX_EC2_BIT_N,		// 2号编码器计数位数【绝对式必填】

        PAR_AX_SMX_TYPE = 100,	// 运动控制(MC)轴类型
        PAR_AX_SMX_LEAD_IDX,
        PAR_AX_COMPEN_LAG = 106,
        PAR_AX_ALARM_LAG,
        PAR_AX_ALARM_VDIFF,
        PAR_AX_ALARM_CDIFF,
        PAR_AX_SMX_PARA,		// MC轴运动系数，16



        PAR_AX_COMP_MAX_COEF = 130,	// 最大误差补偿率
        PAR_AX_COMP_MAX_VALUE,		// 最大误差补偿值
        PAR_AX_CODOFF_VALUE,		// 轴反馈偏差

        PAR_AX_DYNAMIC_PM,			//允许动态切换电子齿轮比
        PAR_AX_SHELF1_ATEETH,		//1档齿轮比分子
        PAR_AX_SHELF1_MTEETH,		//1档齿轮比分母
        PAR_AX_SHELF2_ATEETH,		//2档齿轮比分子
        PAR_AX_SHELF2_MTEETH,		//2档齿轮比分母
        PAR_AX_SHELF3_ATEETH,		//3档齿轮比分子
        PAR_AX_SHELF3_MTEETH,		//3档齿轮比分母
        PAR_AX_SHELF4_ATEETH,		//4档齿轮比分子
        PAR_AX_SHELF4_MTEETH,		//4档齿轮比分母
        PAR_AX_SHELF1_ORI_POS,		//1档定向角度
        PAR_AX_SHELF2_ORI_POS,		//2档定向角度
        PAR_AX_SHELF3_ORI_POS,		//3档定向角度
        PAR_AX_SHELF4_ORI_POS,		//4档定向角度

        PAR_AX_TANG_NO,				//切线控制随动轴轴号0，1，2代表A，B，C轴
        PAR_AX_TANG_ANGLE,			//切线控制偏移角

        PAR_AX_ENC_TOLERANCE = 197,	// 断电位置允差
        PAR_AX_OUT_ACTVEL = 198,	// 实际速度超速判断周期
        PAR_AX_INTEG_PRD = 199,		// 显示速度积分周期数

        // 伺服参数（进给轴，预留100个）
        PAR_SV_POSITION_GAIN = HNCDATADEF.SERVO_PARM_START_IDX,	// 位置比例增益
        PAR_SV_POS_FF_GAIN,				// 位置前馈增益
        PAR_SV_SPEED_GAIN,				// 速度比例增益
        PAR_SV_SPEED_KI,				// 速度积分时间常数
        PAR_SV_SPEED_FB_FILTER,			// 速度反馈滤波因子
        PAR_SV_MOTOR_TYPE = HNCDATADEF.SERVO_PARM_START_IDX+43,		// 驱动器电机类型代码
        PAR_SV_MOTOR_RATING_CUR = HNCDATADEF.SERVO_PARM_START_IDX+86,	// 电机额定电流

        // 伺服参数（主轴，预留100个）
        PAR_SP_POSITION_GAIN = HNCDATADEF.SERVO_PARM_START_IDX,	// 位置控制位置比例增益
        PAR_SP_MOTOR_RATING_CUR = HNCDATADEF.SERVO_PARM_START_IDX+53,	// IM电机额定电流
        PAR_SP_MOTOR_TYPE = HNCDATADEF.SERVO_PARM_START_IDX+59,		// 驱动器电机类型代码

        PAR_AX_TOTAL = 1000
    }

    // 误差补偿参数
    public enum ParCmp
    {
        PAR_CMP_BL_ENABLE = 0,		// 反向间隙补偿类型
        PAR_CMP_BL_VALUE,			// 反向间隙补偿值
        PAR_CMP_BL_RATE,			// 反向间隙补偿率
        PAR_CMP_BL_VALUE2,			// 第2反向间隙补偿值（快移反向间隙补偿值）

        PAR_CMP_HEAT_TYPE = 5,		// 热误差补偿类型
        PAR_CMP_HEAT_REFN,			// 热误差补偿参考点坐标
        PAR_CMP_HEAT_WARP_START,	// 热误差偏置表起始温度
        PAR_CMP_HEAT_WARP_NUM,		// 热误差偏置表温度点数
        PAR_CMP_HEAT_WARP_STEP,		// 热误差偏置表温度间隔
        PAR_CMP_HEAT_WARP_SENSOR,	// 热误差偏置表传感器编号
        PAR_CMP_HEAT_WARP_TABLE,	// 热误差偏置表起始参数号
        PAR_CMP_HEAT_SLOPE_START,	// 热误差斜率表起始温度
        PAR_CMP_HEAT_SLOPE_NUM,		// 热误差斜率表温度点数
        PAR_CMP_HEAT_SLOPE_STEP,	// 热误差斜率表温度间隔
        PAR_CMP_HEAT_SLOPE_SENSOR,	// 热误差斜率表传感器编号
        PAR_CMP_HEAT_SLOPE_TABLE,	// 热误差斜率表起始参数号
        PAR_CMP_HEAT_RATE,			// 热误差补偿率

        PAR_CMP_PITCH_TYPE = 20,	// 螺距误差补偿类型
        PAR_CMP_PITCH_START,		// 螺距误差补偿起点坐标
        PAR_CMP_PITCH_NUM,			// 螺距误差补偿点数
        PAR_CMP_PITCH_STEP,			// 螺距误差补偿点间距
        PAR_CMP_PITCH_MODULO,		// 螺距误差取模补偿使能
        PAR_CMP_PITCH_FACTOR,		// 螺距误差补偿倍率
        PAR_CMP_PITCH_TABLE,		// 螺距误差补偿表起始参数号

        PAR_CMP_SQU1_ENABLE = 30,	// 第1项垂直度补偿使能
        PAR_CMP_SQU1_INPUT_AX,	    // 第1项垂直度补偿基准轴号
        PAR_CMP_SQU1_REFN,	        // 第1项垂直度补偿基准点坐标
        PAR_CMP_SQU1_ANG,	        // 第1项垂直度补偿角度

        PAR_CMP_SQU2_ENABLE = 40,	// 第2项垂直度补偿使能
        PAR_CMP_SQU2_INPUT_AX,	    // 第2项垂直度补偿基准轴号
        PAR_CMP_SQU2_REFN,	        // 第2项垂直度补偿基准点坐标
        PAR_CMP_SQU2_ANG,	        // 第2项垂直度补偿角度

        PAR_CMP_STRA1_INPUT_AX = 50,	// 第1项直线度补偿基准轴号
        PAR_CMP_STRA1_TYPE,	        // 第1项直线度补偿类型
        PAR_CMP_STRA1_START,	    // 第1项直线度补偿起点坐标
        PAR_CMP_STRA1_NUM,	        // 第1项直线度补偿点数
        PAR_CMP_STRA1_STEP,	        // 第1项直线度补偿点间距
        PAR_CMP_STRA1_MODULO,	    // 第1项直线度取模补偿使能
        PAR_CMP_STRA1_FACTOR,	    // 第1项直线度补偿倍率
        PAR_CMP_STRA1_TABLE,	    // 第1项直线度补偿表起始参数号

        PAR_CMP_STRA2_INPUT_AX = 65,	// 第2项直线度补偿基准轴号
        PAR_CMP_STRA2_TYPE,	        // 第2项直线度补偿类型
        PAR_CMP_STRA2_START,	    // 第2项直线度补偿起点坐标
        PAR_CMP_STRA2_NUM,	        // 第2项直线度补偿点数
        PAR_CMP_STRA2_STEP,	        // 第2项直线度补偿点间距
        PAR_CMP_STRA2_MODULO,	    // 第2项直线度取模补偿使能
        PAR_CMP_STRA2_FACTOR,	    // 第2项直线度补偿倍率
        PAR_CMP_STRA2_TABLE,    	// 第2项直线度补偿表起始参数号

        PAR_CMP_ANG1_INPUT_AX = 80,	// 第1项角度补偿基准轴号
        PAR_CMP_ANG1_ASSO_AX,	    // 第1项角度补偿关联轴号
        PAR_CMP_ANG1_REFN,          // 第1项角度补偿参考点坐标
        PAR_CMP_ANG1_TYPE,	        // 第1项角度补偿类型
        PAR_CMP_ANG1_START,	        // 第1项角度补偿起点坐标
        PAR_CMP_ANG1_NUM,	        // 第1项角度补偿点数
        PAR_CMP_ANG1_STEP,	        // 第1项角度补偿点间距
        PAR_CMP_ANG1_MODULO,	    // 第1项角度取模补偿使能
        PAR_CMP_ANG1_FACTOR,	    // 第1项角度补偿倍率
        PAR_CMP_ANG1_TABLE,	        // 第1项角度补偿表起始参数号

        PAR_CMP_ANG2_INPUT_AX = 95,	// 第2项角度补偿基准轴号
        PAR_CMP_ANG2_ASSO_AX,	    // 第2项角度补偿关联轴号
        PAR_CMP_ANG2_REFN,          // 第2项角度补偿参考点坐标
        PAR_CMP_ANG2_TYPE,	        // 第2项角度补偿类型
        PAR_CMP_ANG2_START,	        // 第2项角度补偿起点坐标
        PAR_CMP_ANG2_NUM,	        // 第2项角度补偿点数
        PAR_CMP_ANG2_STEP,	        // 第2项角度补偿点间距
        PAR_CMP_ANG2_MODULO,	    // 第2项角度取模补偿使能
        PAR_CMP_ANG2_FACTOR,	    // 第2项角度补偿倍率
        PAR_CMP_ANG2_TABLE,	        // 第2项角度补偿表起始参数号

        PAR_CMP_ANG3_INPUT_AX = 110,	// 第3项角度补偿基准轴号
        PAR_CMP_ANG3_ASSO_AX,	    // 第3项角度补偿关联轴号
        PAR_CMP_ANG3_REFN,          // 第3项角度补偿参考点坐标
        PAR_CMP_ANG3_TYPE,	        // 第3项角度补偿类型
        PAR_CMP_ANG3_START,	        // 第3项角度补偿起点坐标
        PAR_CMP_ANG3_NUM,	        // 第3项角度补偿点数
        PAR_CMP_ANG3_STEP,	        // 第3项角度补偿点间距
        PAR_CMP_ANG3_MODULO,	    // 第3项角度取模补偿使能
        PAR_CMP_ANG3_FACTOR,	    // 第3项角度补偿倍率
        PAR_CMP_ANG3_TABLE,	        // 第3项角度补偿表起始参数号

        PAR_CMP_QUAD_ENABLE = 125,	// 过象限突跳补偿类型
        PAR_CMP_QUAD_VALUE,         // 过象限突跳补偿值
        PAR_CMP_QUAD_DELAY_T,		// 过象限突跳补偿延时时间，单位：ms
        PAR_CMP_QUAD_MIN_VEL,		// 过象限突跳补偿最低速度
        PAR_CMP_QUAD_MAX_VEL,		// 过象限突跳补偿最高速度
        PAR_CMP_QUAD_ACC_T,			// 过象限突跳补偿加速时间，单位：ms
        PAR_CMP_QUAD_DEC_T,			// 过象限突跳补偿减速时间，单位：ms
        PAR_CMP_QUAD_TRQ_VAL,		// 静摩擦补偿扭矩值，取值范围：-10000~10000

        PAR_CMP_MULHT_TYPE = 135,		// 多元线性补偿类型
        PAR_CMP_MULHT_REFN,				// 多元线性补偿参考点坐标
        PAR_CMP_MULHT_BASE_WARP,		// 主轴偏置模型常量
        PAR_CMP_MULHT_WARP_SEN_NUM,		// 主轴偏置模型传感器接入个数
        PAR_CMP_MULHT_WARP_SEN_LIST,    // 主轴偏置模型传感器编号序列
        PAR_CMP_MULHT_WARP_COEF_TABLE,  // 主轴偏置模型系数表起始参数号
        PAR_CMP_MULHT_BASE_SLOPE,       // 丝杆斜率模型常量
        PAR_CMP_MULHT_SLOPE_SEN_NUM,    // 丝杆斜率模型传感器接入个数
        PAR_CMP_MULHT_SLOPE_SEN_LIST,   // 丝杆斜率模型传感器编号序列
        PAR_CMP_MULHT_SLOPE_COEF_TABLE, // 丝杆斜率模型系数表起始参数号

        PAR_CMP_SPECIAL = 150,	// 专机预留/扩展参数起始地址
        PAR_CMP_TOTAL = 200		// 轴补偿参数总个数
    }
}
