using System;
using System.Collections.Generic;

namespace HNCAPI_INTERFACE
{
    public enum NcParam
    {
        PAR_NCU_TYPE,           /*!< NCU控制器类型 */
        PAR_NCU_CYCLE,          /*!< 插补周期 */
        PAR_NCU_PLC2_CMDN,      /*!< PLC2周期执行语句数 */

        PAR_NCU_ANG_RESOL = 5,  /*!< 角度计算分辨率 */
        PAR_NCU_LEN_RESOL,      /*!< 长度计算分辨率 */
        PAR_NCU_TIME_RESOL,     /*!< 时间编程分辨率 */
        PAR_NCU_VEL_RESOL,      /*!< 线速度编程分辨率 */
        PAR_NCU_SPDL_RESOL,     /*!< 角速度编程分辨率 */
        PAR_NCU_ARC_PROFILE,    /*!< 圆弧插补轮廓允许误差 */
        PAR_NCU_MAX_RAD_ERR,    /*!< 圆弧编程端点半径允许偏差 */
        PAR_NCU_G43_SW_MODE,    /*!< 刀具轴选择方式[0,固定z向;1,G17/18/19切换;2,G43指令轴切换] */
        PAR_NCU_G41_G00_G01,    /*!< G00插补使能 */
        PAR_NCU_G53_LEN_BACK,   /*!< G53之后自动恢复刀具长度补偿[0,不恢复 1 恢复] */
        PAR_NCU_CRDS_NUM,       /*!< 允许联动轴数 */
        PAR_NCU_LAN_EN,         /*!< 局域网使能 */
        PAR_NCU_POWER_SAFE,     /*!< 断电保护使能 */
        PAR_NCU_TIME_EN,        /*!< 系统时间显示使能 */
        PAR_NCU_PSW_CHECK,      /*!< 权限检查使能 */
        PAR_NCU_ALARM_POP,      /*!< 报警窗口自动显示使能 */
        PAR_NCU_KBPLC_EN,       /*!< 键盘PLC使能 */
        PAR_NCU_PREVIEW_EN,     /*!< 图形预览使能	*/
        PAR_NCU_FSPD_DISP,      /*!< F进给速度显示方式 */
        PAR_NCU_GLNO_DISP,      /*!< G代码行号显示方式 */
        PAR_NCU_INCH_DISP,      /*!< 公制/英制选择 */
        PAR_NCU_DISP_DIGITS,    /*!< 位置小数点后显示位数 */
        PAR_NCU_FEED_DIGITS,    /*!< 速度小数点后显示位数 */
        PAR_NCU_SPINDLE_DIGITS, /*!< 转速小数点后显示位数 */
        PAR_NCU_LANGUAGE,       /*!< 语言选择 */
        PAR_NCU_LCD_TIME,       /*!< 进入屏保等待时间 */
        PAR_NCU_DISK_TYPE,      /*!< 外置程序存储类型 */
        PAR_NCU_REFRE_INTERV,   /*!< 界面刷新间隔时间 */
        PAR_NCU_SAVE_TYPE,      /*!< 是否外接UPS */
        PAR_NCU_OPERATE_NOTE,   /*!< 操作提示使能[0位：重运行 1位：Tool->相对实际 2位：Tool->当前位置] */
        PAR_NCU_SERVER_NAME,
        PAR_NCU_SERVER_IP1,
        PAR_NCU_SERVER_IP2,
        PAR_NCU_SERVER_IP3,
        PAR_NCU_SERVER_IP4,
        PAR_NCU_SERVER_PORT,    /*!< 服务器端口号 */
        PAR_NCU_SERVER_LOGIN,   /*!< 服务器访问用户名1 */
        PAR_NCU_SERVER_PASSWD,  /*!< 服务器访问密码 */
        PAR_NCU_NET_DISK_DISCON_TIMEOUT,  /*!< 网络断开判断阈值ms */
        PAR_NCU_NET_TYPE = 44,       /*!< 网盘映射类型 */
        PAR_NCU_IP1,            /*!< IP地址段1 */
        PAR_NCU_IP2,            /*!< IP地址段2 */
        PAR_NCU_IP3,            /*!< IP地址段3 */
        PAR_NCU_IP4,            /*!< IP地址段4 */
        PAR_NCU_PORT,           /*!< 本地端口号 */
        PAR_NCU_NET_START,      /*!< 是否开启网络 */
        PAR_NCU_SERIAL_TYPE,    /*!< 串口类型 */
        PAR_NCU_SERIAL_NO = 52, /*!< 串口号 */
        PAR_NCU_DATA_LEN,       /*!< 收发数据长度 */
        PAR_NCU_STOP_BIT,       /*!< 停止位 */
        PAR_NCU_VERIFY_BIT,     /*!< 校验位 */
        PAR_NCU_BAUD_RATE,      /*!< 波特率 */
        PAR_NCU_IP_TYPE,        /*!< 静态IP/动态IP */
        PAR_NCU_ARCS_INTERSECT, /*!< 圆弧交点允差 */
        PAR_NCU_ARCS_ONE_CYCLE, /*!< 两段圆弧的圆心距离小于这个值时，认为两圆同心 */
        PAR_NCU_TOOL_NUM = 60,  /*!< 最大刀具数 */
        PAR_NCU_TOFF_DIGIT,     /*!< 刀补有效位数 */
        PAR_NCU_MAGZ_NUM,       /*!< 最大刀库数 */
        PAR_NCU_TOOL_LOCATION,  /*!< 最大刀座数 */
        PAR_NCU_TABRA_ADD_EN,   /*!< 刀具磨损累加使能 */
        PAR_NCU_TDIA_SHOW_EN,   /*!< 车刀直径显示使能 */
        PAR_NCU_HALF_CYCLE_TOL, /*!< 半圆圆心理论与实际的偏差允许值(mm) */
        PAR_NCU_SMOOTH_SIZE,    /*!< 指令平滑缓冲区大小 */ //wuxuanhui 2019/4/9 此参数作废
        PAR_NCU_CYCLE_OPT,      /*!< 复合循环路径选项【0x00FF: 0 常规  1 退刀段效率优先 2 FANUC兼容  &0xFF00 = =  0x0000 : 45度退刀 0x0100: 径向退刀 */
                                /*!< &0x0200 = =  0x0200时最后一刀直接退到循环起点，凹槽中有台阶时不要用此选项】 （此参数弃用）*/
        PAR_NCU_HOLD_DECODE_EN, /*!< 进给保持后重新解释使能 */
        PAR_NCU_G28_LEN_BACK,   /*!< G28后是否自动恢复刀长补 */
        PAR_NCU_SPEEDUP_EN,     /*!< 解释器周期最大解释段数 */
        PAR_NCU_CUTTIME_EN,     /*!< 加工时间显示使能 */

        PAR_NCU_FOLLOW_TIME,    /*!< 跟踪误差滞后周期 */

        PAR_NCU_SELF_EN = 74,       /*!< 健康保障模块使能,		1:健康保障模块生效，0:关闭   pancl */
        PAR_NCU_SELF_CHE_EN = 75,  /*!< 健康保障监测刀库使能,	1:监测刀库，0:不监测 */
        PAR_NCU_EDIT_AUTO_SAVE,     /*!< G代码编辑后，返回自动保存*/
        PAR_NCU_FAST_VERIFY_STOP_TIME = 77, /*!< 图形校验最大运行时间(秒) */
        PAR_NCU_LOG_IN_CHECK, /*!< 是否开启开机密码 */
        PAR_NCU_NET_CONNECT_TYPE, /*!< 联网设置 */

        PAR_NCU_LOG_SAVE_TYPE = 80, /*!< 日志文件保存类型 */

        PAR_NCU_INTERNET_IP1,   /*!< 网络平台服务器IP */
        PAR_NCU_INTERNET_IP2,
        PAR_NCU_INTERNET_IP3,
        PAR_NCU_INTERNET_IP4,
        PAR_NCU_INTERNET_PORT,  /*!< 网络平台服务器端口 */
        PAR_NCU_GATEWAY1,       /*!< 本机网关 */
        PAR_NCU_GATEWAY2,
        PAR_NCU_GATEWAY3,
        PAR_NCU_GATEWAY4,
        PAR_NCU_CLOUD_TRIGGER,  //云数控上传开关

        PAR_NCU_SUBNET_MASK1 = 91,  // 子网掩码
        PAR_NCU_SUBNET_MASK2,
        PAR_NCU_SUBNET_MASK3,
        PAR_NCU_SUBNET_MASK4,

        PAR_NCU_CLOUD_COMM_TYPE,    //云数控通讯方式 0：http 1：MQTT

        PAR_NCU_SERVER_LOGIN_2 = 97, /*!< 服务器访问用户名2 */
        PAR_NCU_TOOL_WEAR_RANGE,/*!< 车床刀具磨损允许输入范围(mm) */

        PAR_NCU_HMI = 100,                  /*!< 界面设置参数基地址 */
        PAR_NCU_ISSU_EDITION = PAR_NCU_HMI, /*!< 发布版本号 */
        PAR_NCU_TEST_EDITION,               /*!< 测试版本号 */
        PAR_NCU_SHOW_LIST,                  /*!< 示值列，40 */

        PAR_NCU_PROG_PATH = 300,    /*!< 加工代码程序路径 */
        PAR_NCU_PLC_PATH,       /*!< PLC程序路径 */
        PAR_NCU_PLC_NAME,       /*!< PLC程序名 */
        PAR_NCU_DRV_PATH,       /*!< 驱动程序路径 */
        PAR_NCU_DRV_NAME,       /*!< 驱动程序名 */
        PAR_NCU_PARA_PATH,      /*!< 参数文件路径 */
        PAR_NCU_PARA_NAME,      /*!< 参数文件名 */
        PAR_NCU_SIMU_PATH,      /*!< 仿真配置文件路径 */
        PAR_NCU_SIMU_NAME,      /*!< 仿真配置文件名 */
        PAR_NCU_DLGP_PATH,      /*!< 对话编程配置文件路径 */
        PAR_NCU_DLGP_NAME,      /*!< 对话编程配置文件名 */
        PAR_NCU_VIDEO_DEV,      /*!< 视频外设驱动 */

        PAR_NCU_SUB_DBG = 320,  /*!< 宏程序单段使能【WIN】 */
        PAR_NCU_USER_LOGIN = 321,   /*!< 是否开启用户登录? */
        PAR_NCU_GRIND_MODE = 348,   /*!< 上电初始化界面 */
        PAR_NCU_TRIANGEL_FUNC_CTRL = 349,      /*!< 三角函数控制 */
        PAR_NCU_G16_OPT = 350,  /*!< G16的极点定义模式选择 0：FANUC模式 1：HNC-8模式 */
        PAR_NCU_GEDIT_FRAME = 351,  /*!< 编辑界面框架选择 0：HNC-8模式 1：宁江专机模式 */
        PAR_NCU_FTP_MODE = 352, /*!< FTP连接方式 0：HNC-8模式 1：沈飞模式 */
        PAR_NCU_5AXIS_ENABLE = 353,/*!< 是否开启五轴功能应用 0:不开启 1:开启 */
        PAR_NCU_HMI_TYPE,       /*!< HMI类型，0：标准8型；1：沈飞专用； */
        PAR_NCU_CHECK_SYSDISK_FREE, /*!< 系统盘剩余空间不足提示检测阈值 */
        PAR_NCU_MILL_FUN_TYPE,  /*!< 铣床功能类型 */
        PAR_NCU_API_LOG_LEVEL,    /*!< api日志记录级别 */
        PAR_NCU_EXIT_MDI_CLEAR,     /*!< 退出mdi时是否清空mdi内容 */
        PAR_NCU_DEFAULT_RIGHT_LEVEL = 359, /*!< 上电默认用户权限 */

        PAR_NCU_INSERT_SPACE_CLOSE = 361,   /*!< 程序显示分词功能关闭 */
        PAR_NCU_POS_SHOW_2COL,  /*!< 主界面坐标显示2列 */
        PAR_NCU_PROG_SMALL_CHARACTER,   /*!< 程序显示小字符 */
        PAR_NCU_RANDOM_RESTART, /*!< 开启一键偏置功能 */

        PAR_NCU_MAC_TYPE = 368, /*!< 机床类型 */
        PAR_NCU_TOOL_MAG_TYPE, /*!< 刀库类型 */
        PAR_NCU_COMP_ENABLE, /*!< 智能化功能开关 */
        PAR_NCU_MDI_TYPE, /*!< 切换MDI方式 */
        PAR_NCU_TOOL_HEAD_NUM = 372, /*!< 刀沿数目 */
        PAR_NCU_SAMPLE_ST_M = 373, /*!< 伺服调整采样开始M代码 */
        PAR_NCU_SAMPLE_END_M = 374, /*!< 伺服调整采样结束M代码 */
        PAR_NCU_WORKMEAS_AUTO,  /*!< 工件测量自动测量开启 */
        PAR_NCU_HMISHOW = 376,  /*hmi显示参数*/
        PAR_NCU_TEMPERATURE_SENSOR,         /*是否有温度传感器*/
        PAR_NCU_PARAM_CHECK = 378,      /*是否进行参数一致性检查*/
        PAR_NCU_EDITSHOW = 379, /*!< 编辑程序显示参数 */
        PAR_NCU_TOTAL = 2000		/*!< NC参数总数 */
    };

    public enum MachineUserParam
    {
        PAR_MAC_CHAN_NUM,   /*!< 通道数：1~SYS_CHAN_NUM */
        PAR_MAC_CHAN_TYPE,  /*!< 机床通道加工类型【车、铣床、车铣复合】 占用8个参数*/
        PAR_MAC_CHAN_AX_FLAG = 17,  /*!< 通道的轴显示标志，每个通道占用两个参数，共8*2 = 16个参数 */
        PAR_MAC_SHOW_AXES = 41,     /*!< 是否动态显示坐标轴 */
        PAR_MAC_CALIB_TYPE,         /*!< 刀具测量仪类型 */
        PAR_MAC_ALARM_PAUSE,        /*!< 开机自检警告暂停功能 */
        PAR_MAC_CC_CW_SPD_TYPE,     /*!< 半径补偿圆弧速度处理策略 */
        PAR_MAC_CC_WEAR_TYPE,       /*!< 半径补偿=半径减/加磨损 */
        PAR_MAC_CC_CHECK_CTRL,      /*!< 半径补偿干涉控制，0：干涉报警 1：自动处理干涉 */
        PAR_MAC_CC_CHECK_NUM,         /*!< 半径补偿干涉检查段数 */

        PAR_MAC_HOME_BLOCK = 48,    /*!< 机床是否安装回零挡块 */
        PAR_MAC_AXES_NUM,           /*!< 机床总轴数 */
        PAR_MAC_SMX_AXIS_NUM,       /*!< 运动控制通道轴数（耦合从轴+PMC轴） */
        PAR_MAC_SMX_AXIS_IDX,       /*!< PMC及耦合从轴编号，预留32个 */

        PAR_MAC_CYC_TYPE = 83,      /*!< 钻攻中心固定循环类型 */
        PAR_MAC_T_CTRL_TYPE = 89,         /*!< T指令控制方式 */
        PAR_MAC_USR_VAR_WAIT,          /*!< 用户自定义宏变量是否执行G08等待 */
        PAR_MAC_USR_NEW_USR_VAR,      /*!< 兼容FANUC,三菱，#500~999断电保存。0，关闭断电保存功能；1，打开 */
        PAR_MAC_SP_POS_HOLD,        /*!< C轴为速度模式时不刷新坐标 */
        PAR_MAC_PRE_RUN_MASK,             /*!< 按循环启动是否运行预加载程序 */
        PAR_MAC_HOME_CHECK,         /*!< 轴回零完成后进行位置检测，位置偏差较大时提示 */
        PAR_MAC_PWR_TIME_RECORD,          /*!< 断电时间记录诊断功能 */
        PAR_MAC_TINY_LEN_CLEAR = 96,    /*!< 极小长度过滤阈值 */
        PAR_MAC_TINY_INC_CLEAR,         /*!< 极小增量过滤阈值 */
        PAR_MAC_G02_LOSE_PARM,          /*!< G02/G03缺参数时是否转成G01 */

        PAR_MAC_BIG_TOOL_SWITCH,    /*!< 大小刀功能开关 */

        PAR_MAC_SPDL_TYPE = 100,           //排钻机床主轴类型
        PAR_MAC_PAIZUAN_START_NO = 101, //排钻起始刀具号
        PAR_MAC_PAIZUAN_TOOL_NUM = 102, //排钻刀具总数
        PAR_MAC_NEW_FUCTION_TEST = 103,   /*!< 车削中心新加功能调试参数 */
        PAR_MAC_NEW_FUCTION_TEST2,   /*!< 新加功能调试参数 */
        PAR_MAC_TOOLLIFE_TACTICS = 105, /*!< 刀具寿命报警策略 */
        PAR_MAC_TOOL = 106, /*!< 刀具处理参数预留 */


        PAR_MAC_FZONE_IN_MASK = 110,    /*!< 机床保护区内部禁止掩码 */
        PAR_MAC_FZONE_EX_MASK,      /*!< 机床保护区外部禁止掩码 */
        PAR_MAC_FZONE_BND,      /*!< 机床保护区边界-x +x -y +y -z +z 6*6=36 */

        PAR_MAC_TOOL_INTER_ENABLE = 148,
        PAR_MAC_TOOL_INTER_X_LEN,//刀具干涉原点距离
        PAR_MAC_TOOL_INTER_X_DIR,//刀具干涉两轴移动方向
        PAR_MAC_TOOL_INTER_Y_LEN,
        PAR_MAC_TOOL_INTER_Y_DIR,
        PAR_MAC_TOOL_INTER_Z_LEN,
        PAR_MAC_TOOL_INTER_Z_DIR,

        PAR_G95_F_DISP = 160,         /*!<转进给F速度显示*/
        PAR_GCODE_FANUC_ERROR = 161,  /*!<复合循环误差范围*/
        PAR_GCODE_FANUC_HIGH = 162,   /*!<复合循环高度误差范围*///(参数作废)
        PAR_MAC_NEW_FUCTION_TEST3 = 163,/*!< 新加功能调试参数3 */
        PAR_GCODE_FANUC = 164,      /*!< FANUC类型指令支持 */
        PAR_MAC_HOME_DWELL = 165,   /*!< 回参考点延时时间，单位：ms */
        PAR_MAC_PLCACK_CYCLE,       /*!< PLC应答最长时间 */
        PAR_MAC_G32_HOLD_DX,        /*!< 螺纹加工中止的退刀距离 */
        PAR_MAC_G32_HOLD_ANG,       /*!< 螺纹加工中止的退刀角度 */
        PAR_MAC_G64_CORNER_CHK,     /*!< G64拐角准停校验检查使能 */

        PAR_MAC_MCODE_TO_G = 170,   /*!< M代码属性表 */

        PAR_MAC_CHAN_GMODE_SHOW = 220,  /*!< 模态G指令显示定制，每个工位占用3个参数，共8*3 = 24个参数 */

        PAR_MAC_MEAS_SPD = 250, /*!< 测量速度 */
        PAR_MAC_MEAS_DIST,      /*!< 测量最小行程 */

        PAR_MAC_IPSYNC_FUN = 260,   /*!< 插补同步函数注册 */

        PAR_MAC_SPECIAL = 270,      /*!< 专机预留参数起始地址 */

        PAR_MAC_CHECK_ENCRYPT = 298,    /*!< 是否检查文件加密属性 */
        PAR_MAC_PROG_SKEY = 299,        /*!< G代码文件密钥 */

        PAR_MAC_USER = 300, /*!< 用户参数基地址 */

        PAR_MAC_TOTAL = 2000	/*!< 机床用户参数总数 */
    };

    public enum CHParam
    {
        PAR_CH_NAME = 0,    /*!< 名称 */
        PAR_CH_XINDEX,      /*!< X轴编号 */
        PAR_CH_YINDEX,      /*!< Y轴编号 */
        PAR_CH_ZINDEX,      /*!< Z轴编号 */
        PAR_CH_AINDEX,      /*!< A轴编号 */
        PAR_CH_BINDEX,      /*!< B轴编号 */
        PAR_CH_CINDEX,      /*!< C轴编号 */
        PAR_CH_UINDEX,      /*!< U轴编号 */
        PAR_CH_VINDEX,      /*!< V轴编号 */
        PAR_CH_WINDEX,      /*!< W轴编号 */
        PAR_CH_SPDL0,       /*!< 主轴0编号 */
        PAR_CH_SPDL1,       /*!< 主轴1编号 */
        PAR_CH_SPDL2,       /*!< 主轴2编号 */
        PAR_CH_SPDL3,       /*!< 主轴3编号 */
        PAR_CH_X_NAME,      /*!< X轴名 */
        PAR_CH_Y_NAME,      /*!< Y轴名 */
        PAR_CH_Z_NAME,      /*!< Z轴名 */
        PAR_CH_A_NAME,      /*!< A轴名 */
        PAR_CH_B_NAME,      /*!< B轴名 */
        PAR_CH_C_NAME,      /*!< C轴名 */
        PAR_CH_U_NAME,      /*!< U轴名 */
        PAR_CH_V_NAME,      /*!< V轴名 */
        PAR_CH_W_NAME,      /*!< W轴名 */
        PAR_CH_S0_NAME,     /*!< 主轴0名 */
        PAR_CH_S1_NAME,     /*!< 主轴1名 */
        PAR_CH_S2_NAME,     /*!< 主轴2名 */
        PAR_CH_S3_NAME,     /*!< 主轴3名 */
        PAR_CH_SVEL_SHOW,   /*!< 主轴转速显示方式 */
        PAR_CH_S_SHOW,      /*!< 主轴显示定制 */
        PAR_CH_STOP_DELAY,  /*!<急停后增量延时周期 */

        PAR_CH_DEFAULT_F = 30,  /*!< 通道的缺省进给速度 */
        PAR_CH_DRYRUN_SPD,      /*!< 空运行进给速度 */
        PAR_CH_DIAPROG,         /*!< 直径编程使能 */
        PAR_CH_UVW_INC_EN,      /*!< UVW增量编程使能 */
        PAR_CH_CHAMFER_EN,      /*!< 倒角使能 */
        PAR_CH_ANGLEP_EN,       /*!< 角度编程使能*/
        PAR_CH_CYCLE_OPTION,    /*!< 复合循环选项屏蔽字[位]：0x0001 粗加工圆弧转直线 0x0002：凹槽轴向余量报警屏蔽 0x0004: 无精加工 */
        PAR_CH_MPG_ACC_RATE,    /*!< 手摇加速度系数 */
        PAR_CH_MPG_JK_RATE,     /*!< 手摇捷度系数 */
        PAR_CH_MPG_WORK_RATE,   /*!< 手摇加工系数 */

        PAR_CH_MAC_FRAME = 40,  /*!< 机床结构类型【0, 一般直角系机床 1, 通用五轴机床；2+其它机床】 */
        PAR_CH_MACTURN,         /*!< 车床横式立式类型【0, 横式 1, 立式】 */
        PAR_CH_SUBPROG_NAME,    /*!< 共享子程序目录名 */
        PAR_CH_G95_DEFAULT_F = 44,   /*!<通道的缺省转进给速度 */
        PAR_CH_STD_RAD0,        /*!<最小邻域半径L0 */
        PAR_CH_RATIO_SINGLE_POINT,/*!<单点降速角度比例因子 */
        PAR_CH_RATIO_POINT_ANGLE,/*!<转角比判据最小转角比 */
        PAR_CH_RATIO_RELATIVE_ANGLE,/*!<相对长线段判据最小转角比 */
        PAR_CH_MODLE_JUDGE_CONDITION,/*!<判据组合模式【16进制，末位为判据模式(0 1 2)，首位为曲率半径计算模式(0 1)】 */
        PAR_CH_MAX_OVERRIDE = 50,   /*!< 最大进给修调倍数【1.0-2.0】 */
        PAR_CH_ARC_SPEED_DOWN_R,
        PAR_CH_ARC_SPEED_DOWN_V,
        PAR_CH_ARC_SPEED_DOWN_R2,
        PAR_CH_ARC_SPEED_DOWN_V2,
        PAR_CH_ARC_SPEED_DOWN_R3,
        PAR_CH_ARC_SPEED_DOWN_V3,
        PAR_CH_ARC_SPEED_DOWN_R4,
        PAR_CH_ARC_SPEED_DOWN_V4,
        PAR_CYCYLE_BACK_INTP_PROG,
        PAR_CH_FOLLOW_ROTATE_RAD = 60, /*!<工具跟随的摆动半径 */
        PAR_CH_FOLLOW_CHORD_LEN, /*!<弦线跟随的弦长 */
        PAR_CH_FOLLOW_TOOL_AX, /*!<工具旋转的旋转轴号 */
        PAR_CH_FOLLOW_TABLE_AX, /*!<工作台旋转的旋转轴号 */
        PAR_CH_FOLLOW_DX, /*!<工具跟随旋转中心x偏移 */
        PAR_CH_FOLLOW_DY, /*!<工具跟随旋转中心y偏移 */
        PAR_CH_G04_TIME_SHOW,   /*< 显示G04剩余时间 */

        PAR_CH_VPLAN_CRFT_EN = 67,  /*!<第二加工代码工艺参数合并使能 */
        PAR_CH_VPLAN_SPL_EN = 68,  /*!<第二加工代码样条合并使能 */
        PAR_CH_VPLAN_MODE = 69, /*!< 速度规划模式0-9:曲面模式  10+高速模式【激光、木工】  */

        PAR_CH_MICR_MAX_LEN,    /*!< 微线段上限长度 */ /*!< wxh，从这里开始，最多20个参数，Q0/Q1/Q2/Q3参数必须保持一致 */
        PAR_CH_CORNER_MAX_ANG,  /*!< 拐角平滑最小角度 */
        PAR_CH_VEL_FILTER_LEN,  /*!< 微线段速度滤波长度 */
        PAR_CH_PATH_TOLERANCE,  /*!< 轨迹轮廓允差 */
        PAR_CH_CONER_DEC_FACTOR,/*!< 拐角降速比例因子% */
        PAR_CH_HSPL_MIN_LEN,    /*!< 微线段下限长度 */
        PAR_CH_HSPL_MAX_ANG,    /*!< 样条过渡夹角 */
        PAR_CH_HSPL_MAX_RAT,    /*!< 样条平滑的相邻段最大长度比 */
        PAR_CH_HSPL_MAX_LEN,    /*!< 样条平滑的最大线段长度 */
        PAR_CH_ARC2LINE,        /*!< 圆弧离散为直线选择 79 */

        PAR_CH_LOOKAHEAD_NUM,   /*!< 速度规划前瞻段数 */
        PAR_CH_CURVATURE_COEF,  /*!< 曲率半径调整系数【0.3~100.0】 */
        PAR_CH_RECTIFY_NUM,     /*!< 指令速度平滑周期数 */
        PAR_CH_JERK_TIMES,      /*!< 合成捷度时间常数 */
        PAR_CH_MAX_ECEN_ACC,    /*!< 向心加速度 */
        PAR_CH_MAX_TANG_ACC,    /*!<切向加速度 */
        PAR_CH_FEED_ACC_COEF,   /*!< 加速度系数 */
        PAR_CH_FEED_JK_COEF,        /*!< 捷度系数 */
        PAR_CH_PRE_SMOOTH_OFF,  /*!<预处理平滑关闭 */
        PAR_CH_FLAT_ANGLE,      /*!<平角阈值系数 */

        PAR_CH_CYL_RAX = 90,    /*!< 圆柱插补旋转轴号【缺省5、C轴】 */
        PAR_CH_CYL_LAX,         /*!< 圆柱插补直线【轴向】轴号【缺省2、Z轴】 */
        PAR_CH_CYL_PAX,         /*!< 圆柱插补平行【周向、纬线】轴号【缺省1、Y轴】 */
        PAR_CH_TOOL_RETURN_AXNO,
        PAR_CH_POWEROFF_OF_MAC_TYPE = 94,/*!< 断电机床类型 */

        PAR_CH_POLAR_LAX = 95,  /*!< 极坐标插补的直线轴轴号 */
        PAR_CH_POLAR_RAX,       /*!< 极坐标插补的旋转轴轴号 */
        PAR_CH_POLAR_VAX,       /*!< 极坐标插补的假想轴轴号 */
        PAR_CH_POLAR_CX,        /*!< 极坐标插补的旋转中心直线轴坐标 */
        PAR_CH_POLAR_CY,        /*!< 极坐标插补假想轴偏心量 */
        PAR_CH_POLAR_POLETYPE,  /*!< 极点处理模式 */
        PAR_CH_SP_TOOL_NUM,     /*! <主轴上刀具数 */
        PAR_CH_CHANGE_AXIS_NO = 102,/*!<动态切换轴掩码 */
        PAR_CH_G95_DEFAULT = 104,/*!< 系统上电时G95/G94模态设置 */
                                 //PAR_CH_THREAD_TOL = 105,	/*!< 螺纹起点允许偏差 */
                                 //PAR_CH_THREAD_WAY,			/*!< 螺纹加工方式 */

        PAR_CH_G61_DEFAULT = 107,       /*!< 系统上电时G61/G64模态设置 */
        PAR_CH_G00_DEFAULT,     /*!< 系统上电时G00/G01模态设置 */
        PAR_CH_G90_DEFAULT,     /*!< 系统上电时G90/G91模态设置 */
        PAR_CH_G28_ZTRAP_EN,    /*!< G28搜索Z脉冲使能 */
        PAR_CH_G28_POS_EN,      /*!< G28不寻Z脉冲时快移使能 【0 就进给速度定位 1 快移速度定位】 */
        PAR_CH_G28_ONE_SHOT,    /*!< G29 */
        PAR_CH_SKIP_MODE,       /*!< 任意行模式[0,非扫描 1，扫描，恢复单行轴移动模态 2扫描，恢复全部轴移动模态] */
        PAR_CH_AXIS_RETURN_ORDER, /*!<任意行轴返回顺序 */

        //PAR_CH_M_GROUP1,             /*!< M代码分组1 */
        // PAR_CH_G95_DEFAULT,		/*!< 系统上电时G95/G94模态设置 */

        PAR_CH_MAG_START_NO = 125,  /*!< 	起始刀库号 */
        PAR_CH_MAG_NUM,             /*!< 	刀库数目 */
        PAR_CH_TOOL_START_NO,       /*!< 	起始刀具号 */
        PAR_CH_TOOL_NUM,            /*!< 	刀具数目 */
        PAR_CH_TOOL_OFF_START_NO,   /*!<	起始刀补号 */
        PAR_CH_LIFE_ON,             /*!< 	刀具寿命功能开启 */

        PAR_CH_TOFF_ON,             /*!<限位与保护区时，刀补开启 */
        PAR_CH_TOFF_LIMIT,          /*!<刀补开启时，Z轴刀尖与负限位的距离 */
        PAR_CH_LIFE_LOSE_NO,        /*!<  T指令寿命管理忽略号	,例如T106，系统将其认为是T(106 - 100) */
        PAR_CH_SYNC_RESET,          /*!<复位是否清除同步 */
        PAR_CH_MILL_LEN,            /*!<铣床刀具组长度补偿 */
        PAR_CH_MILL_ARC,            /*!<铣床刀具组半径补偿 */
        PAR_CH_TOOL_OFF_TYPE,       /*!<是否以坐标偏移方式执行刀具偏置 (此参数弃用)*/

        //  = 140 第2套小线段参数
        PAR_CH_MICR_MAX_LEN2 = 140, /*!< 微线段上限长度 */
        PAR_CH_CORNER_MAX_ANG2, /*!< 拐角平滑最小角度 */
        PAR_CH_VEL_FILTER_LEN2, /*!< 微线段速度滤波长度 */
        PAR_CH_PATH_TOLERANCE2, /*!< 轨迹轮廓允差 */
        PAR_CH_CONER_DEC_FACTOR2,/*!< 拐角降速比例因子% */
        PAR_CH_HSPL_MIN_LEN2,   /*!< 微线段下限长度 */
        PAR_CH_HSPL_MAX_ANG2,   /*!< 样条过渡夹角 */
        PAR_CH_HSPL_MAX_RAT2,   /*!< 样条平滑的相邻段最大长度比 */
        PAR_CH_HSPL_MAX_LEN2,   /*!< 样条平滑的最大线段长度 */
        PAR_CH_ARC2LINE2,       /*!< 圆弧离散为直线选择 79 */

        PAR_CH_LOOKAHEAD_NUM2,  /*!< 速度规划前瞻段数 */
        PAR_CH_CURVATURE_COEF2, /*!< 曲率半径调整系数【0.3~100.0】 */
        PAR_CH_RECTIFY_NUM2,        /*!< 指令速度平滑周期数 */
        PAR_CH_JERK_TIMES2,     /*!< 合成捷度时间常数 */
        PAR_CH_MAX_ECEN_ACC2,   /*!< 向心加速度 */
        PAR_CH_MAX_TANG_ACC2,   /*!<切向加速度 */
        PAR_CH_FEED_ACC_COEF2,  /*!< 加速度系数 */
        PAR_CH_FEED_JK_COEF2,       /*!< 捷度系数 */
        PAR_CH_PRE_SMOOTH_OFF2, /*!<预处理平滑关闭 */
        PAR_CH_FLAT_ANGLE2,     /*!<平角阈值系数 */
                                //  = 160 第3套小线段参数
        PAR_CH_MICR_MAX_LEN3 = 160, /*!< 微线段上限长度 */
        PAR_CH_CORNER_MAX_ANG3, /*!< 拐角平滑最小角度 */
        PAR_CH_VEL_FILTER_LEN3, /*!< 微线段速度滤波长度 */
        PAR_CH_PATH_TOLERANCE3, /*!< 轨迹轮廓允差 */
        PAR_CH_CONER_DEC_FACTOR3,/*!< 拐角降速比例因子% */
        PAR_CH_HSPL_MIN_LEN3,   /*!< 微线段下限长度 */
        PAR_CH_HSPL_MAX_ANG3,   /*!< 样条过渡夹角 */
        PAR_CH_HSPL_MAX_RAT3,   /*!< 样条平滑的相邻段最大长度比 */
        PAR_CH_HSPL_MAX_LEN3,   /*!< 样条平滑的最大线段长度 */
        PAR_CH_ARC2LINE3,       /*!< 圆弧离散为直线选择 79 */

        PAR_CH_LOOKAHEAD_NUM3,  /*!< 速度规划前瞻段数 */
        PAR_CH_CURVATURE_COEF3, /*!< 曲率半径调整系数【0.3~100.0】 */
        PAR_CH_RECTIFY_NUM3,        /*!< 指令速度平滑周期数 */
        PAR_CH_JERK_TIMES3,     /*!< 合成捷度时间常数 */
        PAR_CH_MAX_ECEN_ACC3,   /*!< 向心加速度 */
        PAR_CH_MAX_TANG_ACC3,   /*!<切向加速度 */
        PAR_CH_FEED_ACC_COEF3,  /*!< 加速度系数 */
        PAR_CH_FEED_JK_COEF3,       /*!< 捷度系数 */
        PAR_CH_PRE_SMOOTH_OFF3, /*!<预处理平滑关闭 */
        PAR_CH_FLAT_ANGLE3,     /*!<平角阈值系数 */
                                //  = 180 第4套小线段参数
        PAR_CH_MICR_MAX_LEN4 = 180, /*!< 微线段上限长度 */
        PAR_CH_CORNER_MAX_ANG4, /*!< 拐角平滑最小角度 */
        PAR_CH_VEL_FILTER_LEN4, /*!< 微线段速度滤波长度 */
        PAR_CH_PATH_TOLERANCE4, /*!< 轨迹轮廓允差 */
        PAR_CH_CONER_DEC_FACTOR4,/*!< 拐角降速比例因子% */
        PAR_CH_HSPL_MIN_LEN4,   /*!< 微线段下限长度 */
        PAR_CH_HSPL_MAX_ANG4,   /*!< 样条过渡夹角 */
        PAR_CH_HSPL_MAX_RAT4,   /*!< 样条平滑的相邻段最大长度比 */
        PAR_CH_HSPL_MAX_LEN4,   /*!< 样条平滑的最大线段长度 */
        PAR_CH_ARC2LINE4,       /*!< 圆弧离散为直线选择 79 */

        PAR_CH_LOOKAHEAD_NUM4,  /*!< 速度规划前瞻段数 */
        PAR_CH_CURVATURE_COEF4, /*!< 曲率半径调整系数【0.3~100.0】 */
        PAR_CH_RECTIFY_NUM4,        /*!< 指令速度平滑周期数 */
        PAR_CH_JERK_TIMES4,     /*!< 合成捷度时间常数 */
        PAR_CH_MAX_ECEN_ACC4,   /*!< 向心加速度 */
        PAR_CH_MAX_TANG_ACC4,   /*!<切向加速度 */
        PAR_CH_FEED_ACC_COEF4,  /*!< 加速度系数 */
        PAR_CH_FEED_JK_COEF4,       /*!< 捷度系数 */
        PAR_CH_PRE_SMOOTH_OFF4, /*!<预处理平滑关闭 */
        PAR_CH_FLAT_ANGLE4,     /*!<平角阈值系数 */

        PAR_CH_ROBOT_PARA = 200, /*!<200~299机器人参数区 */
                                 //PAR_CH_WTZONE_NUM = 200,	// 工件及刀具保护区总个数0~10
                                 //PAR_CH_WTZONE_TYPE,			// 工件及刀具保护区类型
                                 //PAR_CH_WTZONE_FLAG,			// 工件及刀具保护区属性
                                 //PAR_CH_WTZONE_BND,			// 工件及刀具保护区边界

        PAR_CH_GRAPH_SIMU_SEL = 265,    /*!< 视角选择，每个通道4个bit(铣) */
        PAR_CH_GRAPH_VIEWXYZ_X_ANGLE = 266,                     /*!< 视角xyz,转动角度X(铣) */
        PAR_CH_GRAPH_VIEWXYZ_Y_ANGLE,                       /*!< 视角xyz,转动角度Y(铣) */
        PAR_CH_GRAPH_VIEWXYZ_Z_ANGLE,                       /*!< 视角xyz,转动角度Z(铣) */
        PAR_CH_GRAPH_VIEWXYZ_ZOOM,                              /*!< 视角xyz,显示比例(铣) */
        PAR_CH_GRAPH_VIEWXYZ_X_CENTER,                          /*!< 视角xyz,X中心坐标(铣) */
        PAR_CH_GRAPH_VIEWXYZ_Y_CENTER,                      /*!< 视角xyz,Y中心坐标(铣) */
        PAR_CH_GRAPH_VIEWXYZ_Z_CENTER,                      /*!< 视角xyz,Z中心坐标(铣) */

        PAR_CH_GRAPH_VIEWXY_ZOOM = PAR_CH_GRAPH_VIEWXYZ_Z_CENTER + HNCDATADEF.CH_GRAPH_EVERY_VIEW_PAR_RESV,    /*!< 视角xy,显示比例(铣) */
        PAR_CH_GRAPH_VIEWXY_X_CENTER,                                   /*!< 视角xy,X中心坐标(铣) */
        PAR_CH_GRAPH_VIEWXY_Y_CENTER,                                   /*!< 视角xy,Y中心坐标(铣) */
        PAR_CH_GRAPH_VIEWXY_Z_CENTER,                                   /*!< 视角xy,Z中心坐标(铣) */

        PAR_CH_GRAPH_VIEWYZ_ZOOM = PAR_CH_GRAPH_VIEWXY_Z_CENTER + HNCDATADEF.CH_GRAPH_EVERY_VIEW_PAR_RESV, /*!< 视角yz,显示比例(铣) */
        PAR_CH_GRAPH_VIEWYZ_X_CENTER,                                   /*!< 视角yz,X中心坐标(铣) */
        PAR_CH_GRAPH_VIEWYZ_Y_CENTER,                                   /*!< 视角yz,Y中心坐标(铣) */
        PAR_CH_GRAPH_VIEWYZ_Z_CENTER,                                   /*!< 视角yz,Z中心坐标(铣) */

        PAR_CH_GRAPH_VIEWXZ_ZOOM = PAR_CH_GRAPH_VIEWYZ_Z_CENTER + HNCDATADEF.CH_GRAPH_EVERY_VIEW_PAR_RESV, /*!< 视角xz,显示比例(铣) */
        PAR_CH_GRAPH_VIEWXZ_X_CENTER,                                   /*!< 视角xz,X中心坐标(铣) */
        PAR_CH_GRAPH_VIEWXZ_Y_CENTER,                                   /*!< 视角xz,Y中心坐标(铣) */
        PAR_CH_GRAPH_VIEWXZ_Z_CENTER,                                   /*!< 视角xz,Z中心坐标(铣) */

        PAR_CH_GRAPH_TURN_ZOOM_WORK = PAR_CH_GRAPH_VIEWXZ_Z_CENTER + HNCDATADEF.CH_GRAPH_EVERY_VIEW_PAR_RESV,  /*!< 最佳显示比例(车) */
        PAR_CH_GRAPH_TURN_ZOOM_USER,                                    /*!< 用户比例缩放系数缩放(车) */
        PAR_CH_GRAPH_TURN_LO,                                           /*!< 毛坯长度(车) */
        PAR_CH_GRAPH_TURN_DO,                                           /*!< 毛坯直径(车) */
        PAR_CH_GRAPH_TURN_DI,                                           /*!< 毛坯内侧直径(车) */
        PAR_CH_GRAPH_TURN_LI,                                       /*!< 对刀点偏移(车) */

        PAR_CH_USER_AD_OFF = 300, /*!< 用户模拟量输入点偏移量。x寄存器，单位字节 */
        PAR_CH_USER_DA_OFF = 301, /*!< 用户模拟量输出点偏移量。y寄存器，单位字节 */

        //PAR_CH_RESONA_DAMP_AMP = 300, // 主轴转速避振波幅【百分比 0.05】
        //PAR_CH_RESONA_DAMP_PRD = 301, // 主轴转速避振周期【秒】

        PAR_CH_TAX_ENABLE = 310,    /*!< 倾斜轴控制使能 */
        PAR_CH_TAX_ORTH_AX_INDEX,   /*!< 正交轴轴号 */
        PAR_CH_TAX_TILT_AX_INDEX,   /*!< 倾斜轴轴号 */
        PAR_CH_TAX_TILT_ANGLE,      /*!< 倾斜角度 */

        PAR_CH_G41_TRANS_PROG_ID = 330,       /*!< G41转换程序号 330*/
        PAR_CH_G43_TRANS_PROG_ID,       /*!< G43转换程序号 */
        PAR_CH_G54_TRANS_PROG_ID,       /*!< G54转换程序号 */
        PAR_CH_M00_TRANS_PROG_ID,

        PAR_CH_EG_MAIN_AXIS = 340,  /*!<主动轴主轴号 */
        PAR_CH_EG_SUB_AXIS,         /*!<从动轴主轴号 */
        PAR_CH_EG_MIAN_RATIO,       /*!<主动轴比例 */
        PAR_CH_EG_SUB_RATIO,        /*!<从动轴比例 */
        PAR_CH_EG_TYPE,             /*!<同步类型，1指令跟随，0实际跟随 */
        PAR_CH_EG_PHASE_ENABLE,     /*!<同步相位开启 */
        PAR_CH_EG_PHASE,            /*!<同步相位差 */

        PAR_CH_EG2_MAIN_AXIS = 347, /*!<主动轴主轴号 */
        PAR_CH_EG2_SUB_AXIS,            /*!<从动轴主轴号 */
        PAR_CH_EG2_MIAN_RATIO,       /*!<主动轴比例 */
        PAR_CH_EG2_SUB_RATIO,        /*!<从动轴比例 */
        PAR_CH_EG2_TYPE,                /*!<同步类型，1指令跟随，0实际跟随 */
        PAR_CH_EG2_PHASE_ENABLE,     /*!<同步相位开启 */
        PAR_CH_EG2_PHASE,           /*!<同步相位差 */

        PAR_CH_EG3_MAIN_AXIS = 354, /*!<主动轴主轴号 */
        PAR_CH_EG3_SUB_AXIS,            /*!<从动轴主轴号 */
        PAR_CH_EG3_MIAN_RATIO,       /*!<主动轴比例 */
        PAR_CH_EG3_SUB_RATIO,        /*!<从动轴比例 */
        PAR_CH_EG3_TYPE,                /*!<同步类型，1指令跟随，0实际跟随 */
        PAR_CH_EG3_PHASE_ENABLE,     /*!<同步相位开启 */
        PAR_CH_EG3_PHASE,           /*!<同步相位差 */

        PAR_CH_CARRY_MAIN_AXIS = 361,           /*!<乘载主动主轴号 */
        PAR_CH_CARRY_SUB_AXIS,          /*!<乘载从动主轴号 */
        PAR_CH_CARRY_RATIO,             /*!<乘载比例 */

        PAR_CH_FREQTAP_SPDL_ACCE_COEF = 364,  /*!<变频主轴刚性攻丝主轴加速公式系数 */
        PAR_CH_FREQTAP_SPDL_REDC_COEF,  /*!<变频主轴刚性攻丝主轴减速公式系数 */
        PAR_CH_FREQTAP_SPDL_DELAY,      /*!<变频主轴刚性攻丝主轴延时时间 */
        PAR_CH_FREQTAP_VEL_COEF,        /*!<变频主轴刚性攻丝速度补偿系数 */
        PAR_CH_FREQTAP_ACC_DIS,         /*!<变频主轴刚性攻丝加速度补偿值（微米） */

        PAR_CH_FREQTAP2_SPDL_ACCE_COEF = 369,  /*!<变频主轴刚性攻丝主轴加速公式系数 */
        PAR_CH_FREQTAP2_SPDL_REDC_COEF,  /*!<变频主轴刚性攻丝主轴减速公式系数 */
        PAR_CH_FREQTAP2_SPDL_DELAY,      /*!<变频主轴刚性攻丝主轴延时时间 */
        PAR_CH_FREQTAP2_VEL_COEF,       /*!<变频主轴刚性攻丝速度补偿系数 */
        PAR_CH_FREQTAP2_ACC_DIS,            /*!<变频主轴刚性攻丝加速度补偿值（微米） */

        PAR_CH_FREQTAP3_SPDL_ACCE_COEF = 374,  /*!<变频主轴刚性攻丝主轴加速公式系数 */
        PAR_CH_FREQTAP3_SPDL_REDC_COEF,  /*!<变频主轴刚性攻丝主轴减速公式系数 */
        PAR_CH_FREQTAP3_SPDL_DELAY,      /*!<变频主轴刚性攻丝主轴延时时间 */
        PAR_CH_FREQTAP3_VEL_COEF,       /*!<变频主轴刚性攻丝速度补偿系数 */
        PAR_CH_FREQTAP3_ACC_DIS,            /*!<变频主轴刚性攻丝加速度补偿值（微米） */

        PAR_CH_FREQTAP4_SPDL_ACCE_COEF = 379,  /*!<变频主轴刚性攻丝主轴加速公式系数 */
        PAR_CH_FREQTAP4_SPDL_REDC_COEF,  /*!<变频主轴刚性攻丝主轴减速公式系数 */
        PAR_CH_FREQTAP4_SPDL_DELAY,      /*!<变频主轴刚性攻丝主轴延时时间 */
        PAR_CH_FREQTAP4_VEL_COEF,       /*!<变频主轴刚性攻丝速度补偿系数 */
        PAR_CH_FREQTAP4_ACC_DIS,            /*! 383 <变频主轴刚性攻丝加速度补偿值（微米） */

        PAR_CH_HEAT_TYPE = 384,     /*!< 五轴热误差补偿类型 */
        PAR_CH_HEAT_WARP_START,     /*!< 五轴热误差表起始温度 */
        PAR_CH_HEAT_WARP_NUM,       /*!< 五轴热误差表温度点数 */
        PAR_CH_HEAT_WARP_STEP,      /*!< 五轴热误差表温度间隔 */
        PAR_CH_HEAT_WARP_SENSOR,    /*!< 五轴热误差表传感器编号 */
        PAR_CH_HEAT_WARP_TABLE,     /*!< 五轴热误差表起始参数号 */

        /*! 
         * @name 五轴参数，占用400~600，共200个。其他模块禁止使用。zouj 2020/1/25
         * @{
         */
        PAR_CH_RTCPARA_OFF = 50,        /*!<RTCP参数偏移值 */

        PAR_CH_TOOL_INIT_DIR_X = 400,   /*!<刀具初始方向(X) */
        PAR_CH_TOOL_INIT_DIR_Y,     /*!<刀具初始方向(Y) */
        PAR_CH_TOOL_INIT_DIR_Z,     /*!<刀具初始方向(Z) */
        PAR_CH_TOOL_G54_METHOD,     /*!<RTCP对刀方式 */
        PAR_CH_WCOMP_ENABLE,        /*!<W轴补偿 */
        PAR_CH_ANG_OUTPUT_MODE = 405,   /*!<旋转轴角度输出判定方式 */
        PAR_CH_ANG_OUTPUT_ORDER,    /*!<旋转轴角度输出判定顺序 */
        PAR_CH_POLE_TOLERANCE,      /*!<极点角度范围 */
        PAR_CH_SWIVEL_DIVISION,     /*!<摆头分度使能 */
        PAR_CH_SWIVEL_DOUBLE_TOOLCOMP, /*!<直角头双向刀长补 */
        PAR_CH_RTCP_SWIVEL_TYPE = 410,  /*!<摆头结构类型 */
        PAR_CH_RTCP_SWIVEL_RAX1_DIR_X,  /*!<摆头第1旋转轴方向(X) */
        PAR_CH_RTCP_SWIVEL_RAX1_DIR_Y,  /*!<摆头第1旋转轴方向(Y) */
        PAR_CH_RTCP_SWIVEL_RAX1_DIR_Z,  /*!<摆头第1旋转轴方向(Z) */
        PAR_CH_RTCP_SWIVEL_RAX2_DIR_X,  /*!<摆头第2旋转轴方向(X) */
        PAR_CH_RTCP_SWIVEL_RAX2_DIR_Y,  /*!<摆头第2旋转轴方向(Y) */
        PAR_CH_RTCP_SWIVEL_RAX2_DIR_Z,  /*!<摆头第2旋转轴方向(Z) */
        PAR_CH_RTCP_SWIVEL_RAX1_OFF_X,  /*!<摆头第1旋转轴偏移矢量(X) */
        PAR_CH_RTCP_SWIVEL_RAX1_OFF_Y,  /*!<摆头第1旋转轴偏移矢量(Y) */
        PAR_CH_RTCP_SWIVEL_RAX1_OFF_Z,  /*!<摆头第1旋转轴偏移矢量(Z) */
        PAR_CH_RTCP_SWIVEL_RAX2_OFF_X,  /*!<摆头第2旋转轴偏移矢量(X) */
        PAR_CH_RTCP_SWIVEL_RAX2_OFF_Y,  /*!<摆头第2旋转轴偏移矢量(Y) */
        PAR_CH_RTCP_SWIVEL_RAX2_OFF_Z,  /*!<摆头第2旋转轴偏移矢量(Z) */

        PAR_CH_RTCP_TABLE_TYPE = 425,       /*!<转台结构类型 */
        PAR_CH_RTCP_TABLE_RAX1_DIR_X,   /*!<转台第1旋转轴方向(X) */
        PAR_CH_RTCP_TABLE_RAX1_DIR_Y,   /*!<转台第1旋转轴方向(Y) */
        PAR_CH_RTCP_TABLE_RAX1_DIR_Z,   /*!<转台第1旋转轴方向(Z) */
        PAR_CH_RTCP_TABLE_RAX2_DIR_X,   /*!<转台第2旋转轴方向(X) */
        PAR_CH_RTCP_TABLE_RAX2_DIR_Y,   /*!<转台第2旋转轴方向(Y) */
        PAR_CH_RTCP_TABLE_RAX2_DIR_Z,   /*!<转台第2旋转轴方向(Z) */
        PAR_CH_RTCP_TABLE_RAX1_OFF_X,   /*!<转台第1旋转轴偏移矢量(X) */
        PAR_CH_RTCP_TABLE_RAX1_OFF_Y,   /*!<转台第1旋转轴偏移矢量(Y) */
        PAR_CH_RTCP_TABLE_RAX1_OFF_Z,   /*!<转台第1旋转轴偏移矢量(Z) */
        PAR_CH_RTCP_TABLE_RAX2_OFF_X,   /*!<转台第2旋转轴偏移矢量(X) */
        PAR_CH_RTCP_TABLE_RAX2_OFF_Y,   /*!<转台第2旋转轴偏移矢量(Y) */
        PAR_CH_RTCP_TABLE_RAX2_OFF_Z,   /*!<转台第2旋转轴偏移矢量(Z) */
        PAR_CH_IIPDIS_MODE,             /*!<插补行程计算方式 */

        PAR_CH_RTCP_7AXIS_ENABLE = 588,     /*!<7轴RTCP使能 */
        PAR_CH_RTCP_SWIVEL_RAX3_DIR_X,  /*!<摆头第3旋转轴方向(X) */
        PAR_CH_RTCP_SWIVEL_RAX3_DIR_Y,  /*!<摆头第3旋转轴方向(Y) */
        PAR_CH_RTCP_SWIVEL_RAX3_DIR_Z,  /*!<摆头第3旋转轴方向(Z) */
        PAR_CH_RTCP_SWIVEL_RAX3_OFF_X,  /*!<摆头第3旋转轴偏移矢量(X) */
        PAR_CH_RTCP_SWIVEL_RAX3_OFF_Y,  /*!<摆头第3旋转轴偏移矢量(Y) */
        PAR_CH_RTCP_SWIVEL_RAX3_OFF_Z,  /*!<摆头第3旋转轴偏移矢量(Z) */
        PAR_CH_RTCP_TABLE_RAX3_DIR_X,   /*!<转台第3旋转轴方向(X) */
        PAR_CH_RTCP_TABLE_RAX3_DIR_Y,   /*!<转台第3旋转轴方向(Y) */
        PAR_CH_RTCP_TABLE_RAX3_DIR_Z,   /*!<转台第3旋转轴方向(Z) */
        PAR_CH_RTCP_TABLE_RAX3_OFF_X,   /*!<转台第3旋转轴偏移矢量(X) */
        PAR_CH_RTCP_TABLE_RAX3_OFF_Y,   /*!<转台第3旋转轴偏移矢量(Y) */
        PAR_CH_RTCP_TABLE_RAX3_OFF_Z,   /*!<转台第3旋转轴偏移矢量(Z) */


        /*!@}*/

        PAR_CH_TOTAL = 1000
    };

    public enum AxisParam
    {
        PAR_AX_NAME = 0,    /*!< 轴名[显示用] */
        PAR_AX_TYPE,        /*!< 轴类型[直线、摆动、回转、主轴] */
        PAR_AX_INDEX,       /*!< 轴编号 暂时预留 */
        PAR_AX_MODN,        /*!< 设备号 暂时预留 */
        PAR_AX_DEV_I = PAR_AX_MODN,
        PAR_AX_PM_MUNIT,    /*!< 电子齿轮比分子(位移量)[每转位移量nm] */
        PAR_AX_PM_PULSE,    /*!< 电子齿轮比分母(脉冲数)[每转指令脉冲数] */
        PAR_AX_PLMT,        /*!< 正软极限 */
        PAR_AX_NLMT,        /*!< 负软极限 */
        PAR_AX_PLMT2,       /*!< 第2正软极限 */
        PAR_AX_NLMT2,       /*!< 第2负软极限 */

        PAR_AX_HOME_WAY = 10,   /*!< 回参考点方式 */
        PAR_AX_HOME_DIR,        /*!< 回参考点方向 */
        PAR_AX_ENC_OFF,         /*!< 编码器反馈偏置量【手动零点、绝对式编码器】 */
        PAR_AX_HOME_OFF,        /*!< 回参考点后的偏移量 */
        PAR_AX_HOME_MASK,       /*!< Z脉冲屏蔽角度 */
        PAR_AX_HOME_HSPD,       /*!< 回参考点高速 */
        PAR_AX_HOME_LSPD,       /*!< 回参考点低速 */
        PAR_AX_HOME_CRDS,       /*!< 参考点坐标值 */
        PAR_AX_HOME_CODSPACE,   /*!< 距离码参考点间距 */
        PAR_AX_HOME_CODOFF,     /*!< 间距编码偏差 */

        PAR_AX_HOME_RANGE = 20, /*!< 搜Z脉冲最大移动距离 */
        PAR_AX_HOME_CRDS2,      /*!< 第2参考点坐标值 */
        PAR_AX_HOME_CRDS3,      /*!< 第3参考点坐标值 */
        PAR_AX_HOME_CRDS4,      /*!< 第4参考点坐标值 */
        PAR_AX_HOME_CRDS5,      /*!< 第5参考点坐标值 */
        PAR_AX_REF_RANGE,       /*!< 参考点范围偏差 */
        PAR_AX_HOME_CYCLE_OFF,  /*!< 非整传动比回转轴偏差 */
        PAR_AX_ENC2_OFF,        /*!< 第2编码器反馈偏置量【手动零点、绝对式编码器】 */
        PAR_AX_PM2_MUNIT,       /*!< 第2编码器电子齿轮比分子(位移量)[每转位移量nm] */
        PAR_AX_PM2_PULSE,       /*!< 第2编码器电子齿轮比分母(脉冲数)[每转指令脉冲数] */

        PAR_AX_G60_OFF = 30,    /*!< 单向定位(G60)偏移量 */
        PAR_AX_ROT_RAD,         /*!< 转动轴当量半径 */
        PAR_AX_JOG_LOWSPD,      /*!< 慢速点动速度 */
        PAR_AX_JOG_FASTSPD,     /*!< 快速点动速度 */
        PAR_AX_RAPID_SPD,       /*!< 快移速度 */
        PAR_AX_FEED_SPD,        /*!< 最高进给速度 */
        PAR_AX_RAPID_ACC,       /*!< 快移加速度 */
        PAR_AX_RAPID_JK,        /*!< 快移捷度 */
        PAR_AX_FEED_ACC,        /*!< 进给加速度 */
        PAR_AX_FEED_JK,         /*!< 进给捷度 */
        PAR_AX_THREAD_ACC,      /*!< 螺纹加速度 */
        PAR_AX_THREAD_DEC,      /*!< 螺纹减速度 */
        PAR_AX_MPG_UNIT_SPD,    /*!< 手摇单位速度比例系数 */
        PAR_AX_MPG_RESOL,       /*!< 手摇脉冲分辨率 */
        PAR_AX_MPG_INTE_RATE,   /*!< 手摇缓冲系数 */
        PAR_AX_MPG_INTE_TIME,   /*!< 手摇缓冲周期 [45] */
        PAR_AX_MPG_OVER_RATE,   /*!< 手摇过冲系数 */
        PAR_AX_MPG_SPD,         /*!< 手摇进给速度 */
        PAR_AX_RAPID_RATE,      /*!< 超速报警系数 */
        PAR_AX_FOLLOW_RATE,     /*!< 轴跟踪误差系数，1000mm/min的跟踪误差 */

        PAR_AX_DEFAULT_S = 50,  /*!< 主轴缺省转速 */
        PAR_SPDL_MAX_SPEED,     /*!< 主轴最大转速 */
        PAR_SPDL_SPD_TOL,       /*!< 主轴转速允许转速波动率 */
        PAR_SPDL_SPD_TIME,      /*!< 主轴转速到达允许最大时间 */
        PAR_SPDL_THREAD_TOL,    /*!< 螺纹加工时的转速允差 */
        PAR_AX_SP_ORI_POS,      /*!< 进给主轴定向角度 */
        PAR_AX_SP_ZERO_TOL,     /*!< 进给主轴零速允差【脉冲】 */
        PAR_AX_MAX_EXT_PINC,    /*!< 外部指令最大周期叠加量 */

        PAR_AX_EXT_LOAD_EN,     /*!< 负载由外部导入 */

        PAR_AX_POS_TOL = 60,    /*!< 定位允差 */
        PAR_AX_MAX_LAG,         /*!< 最大跟随误差 */
        PAR_AX_LAG_CMP_EN,      /*!< 龙门轴同步误差补偿使能 */
        PAR_AX_LAG_CMP_COEF,    /*!< 跟随误差补偿调整系数 */
        PAR_AX_LAG_CMP_CNT,     /*!< 动态补偿系数整定周期数 */

        PAR_AX_ATEETH = 65, /*!< 传动比分子[轴侧齿数] */
        PAR_AX_MTEETH,      /*!< 传动比分母[电机侧齿数] */
        PAR_AX_MT_PPR,      /*!< 电机每转脉冲数 */
        PAR_AX_PITCH,       /*!< 丝杆导程 */
        PAR_AX_RACK_NUM,    /*!< 齿条齿数 */
        PAR_AX_RACK_SPACE,  /*!< 齿条齿间距 */
        PAR_AX_WORM_NUM,    /*!< 蜗杆头数 */
        PAR_AX_WORM_SPACE,  /*!< 蜗杆齿距 */

        PAR_AX_RATING_CUR = 74,  /*!< 额定电流 */
        PAR_AX_POWER_RATE,  /*!< 功率系数 */
        PAR_AX_ENC2_PPR,    /*!< 第2编码器每转脉冲数 */
        PAR_AX_INDEX_TYPE, /*!<分度轴类型：1、鼠牙盘；2，分度轴 */
        PAR_AX_INDEX_POS,/*!<分度起点 */
        PAR_AX_INDEX_DIVIDE, /*!<分度间隔 */
        PAR_AX_INDEX_LOCK_M, /*!<分度加锁M代码 */
        PAR_AX_INDEX_UNLOCK_M, /*!<分度解锁M代码 */
                               //PAR_ZAX_LOCK_EN = 80,	/*!< Z轴锁允许使能 */
                               //PAR_RAX_ROLL_EN,		/*!< 旋转轴循环使能 */
        PAR_RAX_PATH_MODE,      /*!< 旋转轴路径模式：0、正常；1、短路径；2、一直正向；3、一直负向 */
        PAR_RAX_CYCLE_RANGE,    /*!< 旋转轴循环行程 */
        PAR_RAX_DISP_RANGE,     /*!< 旋转轴显示角度范围 */
        PAR_LAX_PROG_UNIT,      /*!< 直线轴编程指令最小单位 */
        PAR_RAX_PROG_UNIT,      /*!< 旋转轴编程指令最小单位 */
        PAR_OVERLOAD_JUDGE,     /*!< 轴过载判定百分比 */

        PAR_AX_ENC_MODE = 90,   /*!< 编码器工作模式\n
        PAR_AX_EC1_TYPE,		/*!< 1号编码器类型【增量、距离码、绝对】 */
        PAR_AX_EC1_OUTP,        /*!< 反馈电子齿轮比分子[输出脉冲数] */
        PAR_AX_EC1_FBKP,        /*!< 反馈电子齿轮比分母[反馈脉冲数] */
        PAR_AX_EC1_BIT_N,       /*!< 1号编码器计数位数【绝对式必填】 */
        PAR_AX_EC2_TYPE,        /*!< 2号编码器类型【增量、距离码、绝对】 */
        PAR_AX_EC2_OUTP,        /*!< 反馈电子齿轮比分子[输出脉冲数]  */
        PAR_AX_EC2_FBKP,        /*!< 反馈电子齿轮比分母[反馈脉冲数] */
        PAR_AX_EC2_BIT_N,       /*!< 2号编码器计数位数【绝对式必填】 */

        PAR_AX_SMX_TYPE = 100,  /*!< 运动控制(MC)轴类型  */
        PAR_AX_SMX_LEAD_IDX,
        PAR_AX_COMPEN_LAG = 106,
        PAR_AX_ALARM_LAG,
        PAR_AX_ALARM_VDIFF,
        PAR_AX_ALARM_CDIFF,
        PAR_AX_SMX_PARA,        /*!< MC轴运动系数，16 */

        PAR_AX_WCS_DISP = 126,  /*!< 同步时，从轴工件坐标系显示方式 */
        PAR_AX_SYNC_MIRROR,     /*!< 是否镜像 */
        PAR_AX_COORD_DIR,       /*!< 主从轴坐标系的正方向 */
        PAR_AX_ZERO_OFFSET,     /*!< 同步轴机床零点偏差 */

        PAR_AX_COMP_MAX_COEF = 130, /*!< 最大误差补偿率 */
        PAR_AX_COMP_MAX_VALUE,      /*!< 最大误差补偿值 */
        PAR_AX_CODOFF_VALUE,        /*!< 轴反馈偏差 */

        PAR_AX_TANG_WAIT = 134,     /*!< 切线控制引导轴是否等待 */
        PAR_AX_TANG_NO = 135,       /*!< 切线控制随动轴轴号0，1，2代表A，B，C轴，此参数有150移动到此 */
        PAR_AX_TANG_ANGLE,          /*!< 切线控制偏移角 */
        PAR_AX_TANG_OFFSET,         /*!< 切线跟随偏差值 */
        PAR_AX_HALF_ALL,            /*!< 全闭环、半闭环差值报警阈值 */
        PAR_AX_STOC_ABC,            /*!< 主轴CS切换的轴号 */

        //wangxu
        PAR_AX_ADJUST_COE = 140, /*!< 蛙跳调节系数 */
        PAR_AX_FOL_COE, /*!< 随动比例增益 */
        PAR_AX_FOL_ADR, /*!< 随动模拟量地址 */
        PAR_AX_FOL_POS_DOWN, /*!< 随动位置下限 */
        PAR_AX_FOL_SPD, /*!< 随动最大转速 */
        PAR_AX_FOL_OFF, /*!< 随动到位信号误差范围 */
        PAR_AX_JUMP_HIGH, /*!< 蛙跳高度 */
        PAR_AX_FOL_POS_UP, /*!< 随动位置上限 */
        PAR_AX_JUMP_TO_FOL, /*!< 蛙跳切换到随动的高度 */
        PAR_AX_FOL_NONLINER, /*!< 随动电压非线性，需要先标定 */
        PAR_AX_DEM_SPACE, /*!< 标定的间距 */
        PAR_AX_DEM_VOL, /*!< 碰板电压 */

        PAR_AX_VIB_STOP = 153, /*!< 震荡是否立刻停止 */
        PAR_AX_VIB_OVERRIDE = 154, /*!< 震荡是否受修调控制 */
        PAR_AX_S_ACK = 155,       /*!< S指令需要响应 */
        PAR_AX_OUT_DA,          /*!< 主轴输出模拟量 */
        PAR_AX_MAX_MOTOR,       /*!< 主轴电机最大转速 */
        PAR_AX_SHIFT_NUM,       /*!< 主轴挡位数 */
        PAR_AX_LOW_SPEED1,      /*!< 主轴1档最低转速 */
        PAR_AX_HIGH_SPEED1,     /*!< 主轴1档最高转速 */
        PAR_AX_ATEETH1,         /*!< 主轴1档传动比分子[轴侧齿数] */
        PAR_AX_MTEETH1,         /*!< 主轴1档传动比分母[电机侧齿数] */
        PAR_AX_EC_OUTP1,        /*!< 主轴1档反馈电子齿轮比分子[输出脉冲数] */
        PAR_AX_EC_FBKP1,        /*!< 主轴1档反馈电子齿轮比分母[反馈脉冲数] */
        PAR_AX_LOW_SPEED2,      /*!< 主轴2档最低转速 */
        PAR_AX_HIGH_SPEED2,     /*!< 主轴2档最高转速 */
        PAR_AX_ATEETH2,         /*!< 主轴2档传动比分子[轴侧齿数] */
        PAR_AX_MTEETH2,         /*!< 主轴2档传动比分母[电机侧齿数] */
        PAR_AX_EC_OUTP2,        /*!< 主轴2档反馈电子齿轮比分子[输出脉冲数] */
        PAR_AX_EC_FBKP2,        /*!< 主轴2档反馈电子齿轮比分母[反馈脉冲数] */
        PAR_AX_LOW_SPEED3,      /*!< 主轴3档最低转速 */
        PAR_AX_HIGH_SPEED3,     /*!< 主轴3档最高转速 */
        PAR_AX_ATEETH3,         /*!< 主轴3档传动比分子[轴侧齿数] */
        PAR_AX_MTEETH3,         /*!< 主轴3档传动比分母[电机侧齿数] */
        PAR_AX_EC_OUTP3,        /*!< 主轴3档反馈电子齿轮比分子[输出脉冲数] */
        PAR_AX_EC_FBKP3,        /*!< 主轴3档反馈电子齿轮比分母[反馈脉冲数] */
        PAR_AX_LOW_SPEED4,      /*!< 主轴4档最低转速 */
        PAR_AX_HIGH_SPEED4,     /*!< 主轴4档最高转速 */
        PAR_AX_ATEETH4,         /*!< 主轴4档传动比分子[轴侧齿数] */
        PAR_AX_MTEETH4,         /*!< 主轴4档传动比分母[电机侧齿数] */
        PAR_AX_EC_OUTP4,        /*!< 主轴4档反馈电子齿轮比分子[输出脉冲数] */
        PAR_AX_EC_FBKP4,        /*!< 主轴4档反馈电子齿轮比分母[反馈脉冲数] */
        PAR_AX_SWITCH_SPEED,    /*!< 启用切换点转速 */
        PAR_AX_1S2_SPEED,       /*!< 档位一与档位二切换点转速 */
        PAR_AX_2S3_SPEED,       /*!< 档位二与档位三切换点转速 */
        PAR_AX_3S4_SPEED,       /*!< 档位三与档位四切换点转速 */
        PAR_AX_SHIGT_SPEED,     /*!< 主轴换挡时电机转速 */
        PAR_AX_SHIGT_HOME,      /*!< 主轴换挡后需要重新回零 */

        PAR_FEED_AX_SHIFT_NUM = 189,  /*!< 进给轴挡位数 */
        PAR_FEED_AX_EC_OUTP1,   /*!< 进给轴1档传动比分子*/
        PAR_FEED_AX_EC_FBKP1,   /*!< 进给轴1档传动比分母*/
        PAR_FEED_AX_EC_OPUT2,   /*!< 进给轴2档传动比分子*/
        PAR_FEED_AX_EC_FBKP2,   /*!< 进给轴2档传动比分母*/

        PAR_AX_ACT_PULSE_TOLERANCE = 196,   /*!< 断电反馈脉冲位置允差 */
        PAR_AX_ENC_TOLERANCE = 197, /*!< 断电位置允差 */
        PAR_AX_OUT_ACTVEL = 198,    /*!< 实际速度超速判断周期 */
        PAR_AX_INTEG_PRD = 199,     /*!< 显示速度积分周期数 */
        PAR_AX_DRIVE_SHAFT_TYPE, /*!< 传动类型 */
        PAR_AX_SLIDE_WAY_TYPE, /*!< 导轨类型 */


        PAR_AX_PLMT3,       /*!< 第3正软极限 */
        PAR_AX_NLMT3,       /*!< 第3负软极限 */
        PAR_AX_PLMT4,       /*!< 第4正软极限 */
        PAR_AX_NLMT4,       /*!< 第4负软极限 */
        PAR_AX_PLMT5,       /*!< 第5正软极限 */
        PAR_AX_NLMT5,       /*!< 第5负软极限 */

        PAR_ECAT_MOTOR_RATING_CUR_RATE = 498,   /*!< EtherCat额定电流系数 */
        PAR_ECAT_MOTOR_RATING_CUR = 499,    /*!< EtherCat额定电流 */

        /*! 
         * @name 伺服参数（进给轴，预留500个）
         * @{
         */
        PAR_SV_POSITION_GAIN = HNCDATADEF.SERVO_PARM_START_IDX,    /*!< 位置比例增益 */
        PAR_SV_POS_FF_GAIN,             /*!< 位置前馈增益 */
        PAR_SV_SPEED_GAIN,              /*!< 速度比例增益 */
        PAR_SV_SPEED_KI,                /*!< 速度积分时间常数 */
        PAR_SV_SPEED_FB_FILTER,         /*!< 速度反馈滤波因子 */
        PAR_SV_MOTOR_TYPE = (HNCDATADEF.SERVO_PARM_START_IDX + 43),    /*!< 驱动器电机类型代码 */
        PAR_SV_MOTOR_RATING_CUR = (HNCDATADEF.SERVO_PARM_START_IDX + 86),  /*!< 电机额定电流 */

        /*!@}*/

        /*! 
         * @name 伺服参数（主轴，预留500个）
         * @{
         */
        PAR_SP_POSITION_GAIN = HNCDATADEF.SERVO_PARM_START_IDX,    /*!< 位置控制位置比例增益 */
        PAR_SP_MOTOR_RATING_CUR = (HNCDATADEF.SERVO_PARM_START_IDX + 53),  /*!< IM电机额定电流 */
        PAR_SP_MOTOR_TYPE = (HNCDATADEF.SERVO_PARM_START_IDX + 59),        /*!< 驱动器电机类型代码 */
                                                                /*!@}*/

        PAR_AX_TOTAL = 1000
    };

    public enum ErrorCmpParam
    {
        PAR_CMP_BL_ENABLE = 0,      /*!< 反向间隙补偿类型 */
        PAR_CMP_BL_VALUE,           /*!< 反向间隙补偿值 */
        PAR_CMP_BL_RATE,            /*!< 反向间隙补偿率 */
        PAR_CMP_BL_VALUE2,          /*!< 第2反向间隙补偿值（快移反向间隙补偿值） */

        PAR_CMP_HEAT_TYPE = 5,      /*!< 热误差补偿类型 */
        PAR_CMP_HEAT_REFN,          /*!< 热误差补偿参考点坐标 */
        PAR_CMP_HEAT_WARP_START,    /*!< 热误差偏置表起始温度 */
        PAR_CMP_HEAT_WARP_NUM,      /*!< 热误差偏置表温度点数 */
        PAR_CMP_HEAT_WARP_STEP,     /*!< 热误差偏置表温度间隔 */
        PAR_CMP_HEAT_WARP_SENSOR,   /*!< 热误差偏置表传感器编号 */
        PAR_CMP_HEAT_WARP_TABLE,    /*!< 热误差偏置表起始参数号 */
        PAR_CMP_HEAT_SLOPE_START,   /*!< 热误差斜率表起始温度 */
        PAR_CMP_HEAT_SLOPE_NUM,     /*!< 热误差斜率表温度点数 */
        PAR_CMP_HEAT_SLOPE_STEP,    /*!< 热误差斜率表温度间隔 */
        PAR_CMP_HEAT_SLOPE_SENSOR,  /*!< 热误差斜率表传感器编号 */
        PAR_CMP_HEAT_SLOPE_TABLE,   /*!< 热误差斜率表起始参数号 */
        PAR_CMP_HEAT_RATE,          /*!< 热误差补偿率 */
        PAR_CMP_HEAT_DOUBLE_TIME,   // 热误差双曲线过渡时间

        PAR_CMP_PITCH_TYPE = 20,    /*!< 螺距误差补偿类型 */
        PAR_CMP_PITCH_START,        /*!< 螺距误差补偿起点坐标 */
        PAR_CMP_PITCH_NUM,          /*!< 螺距误差补偿点数 */
        PAR_CMP_PITCH_STEP,         /*!< 螺距误差补偿点间距 */
        PAR_CMP_PITCH_MODULO,       /*!< 螺距误差取模补偿使能 */
        PAR_CMP_PITCH_FACTOR,       /*!< 螺距误差补偿倍率 */
        PAR_CMP_PITCH_TABLE,        /*!< 螺距误差补偿表起始参数号 */

        PAR_CMP_SQU1_ENABLE = 30,   /*!< 第1项垂直度补偿使能 */
        PAR_CMP_SQU1_INPUT_AX,      /*!< 第1项垂直度补偿基准轴号 */
        PAR_CMP_SQU1_REFN,          /*!< 第1项垂直度补偿基准点坐标 */
        PAR_CMP_SQU1_ANG,           /*!< 第1项垂直度补偿角度 */

        PAR_CMP_SQU2_ENABLE = 40,   /*!< 第2项垂直度补偿使能 */
        PAR_CMP_SQU2_INPUT_AX,      /*!< 第2项垂直度补偿基准轴号 */
        PAR_CMP_SQU2_REFN,          /*!< 第2项垂直度补偿基准点坐标 */
        PAR_CMP_SQU2_ANG,           /*!< 第2项垂直度补偿角度 */

        PAR_CMP_STRA1_INPUT_AX = 50,    /*!< 第1项直线度补偿基准轴号 */
        PAR_CMP_STRA1_TYPE,         /*!< 第1项直线度补偿类型 */
        PAR_CMP_STRA1_START,        /*!< 第1项直线度补偿起点坐标 */
        PAR_CMP_STRA1_NUM,          /*!< 第1项直线度补偿点数 */
        PAR_CMP_STRA1_STEP,         /*!< 第1项直线度补偿点间距 */
        PAR_CMP_STRA1_MODULO,       /*!< 第1项直线度取模补偿使能 */
        PAR_CMP_STRA1_FACTOR,       /*!< 第1项直线度补偿倍率 */
        PAR_CMP_STRA1_TABLE,        /*!< 第1项直线度补偿表起始参数号 */

        PAR_CMP_STRA2_INPUT_AX = 65,    /*!< 第2项直线度补偿基准轴号 */
        PAR_CMP_STRA2_TYPE,         /*!< 第2项直线度补偿类型 */
        PAR_CMP_STRA2_START,        /*!< 第2项直线度补偿起点坐标 */
        PAR_CMP_STRA2_NUM,          /*!< 第2项直线度补偿点数 */
        PAR_CMP_STRA2_STEP,         /*!< 第2项直线度补偿点间距 */
        PAR_CMP_STRA2_MODULO,       /*!< 第2项直线度取模补偿使能 */
        PAR_CMP_STRA2_FACTOR,       /*!< 第2项直线度补偿倍率 */
        PAR_CMP_STRA2_TABLE,        /*!< 第2项直线度补偿表起始参数号 */

        PAR_CMP_ANG1_INPUT_AX = 80, /*!< 第1项角度补偿基准轴号 */
        PAR_CMP_ANG1_ASSO_AX,       /*!< 第1项角度补偿关联轴号 */
        PAR_CMP_ANG1_REFN,          /*!< 第1项角度补偿参考点坐标 */
        PAR_CMP_ANG1_TYPE,          /*!< 第1项角度补偿类型 */
        PAR_CMP_ANG1_START,         /*!< 第1项角度补偿起点坐标 */
        PAR_CMP_ANG1_NUM,           /*!< 第1项角度补偿点数 */
        PAR_CMP_ANG1_STEP,          /*!< 第1项角度补偿点间距 */
        PAR_CMP_ANG1_MODULO,        /*!< 第1项角度取模补偿使能 */
        PAR_CMP_ANG1_FACTOR,        /*!< 第1项角度补偿倍率 */
        PAR_CMP_ANG1_TABLE,         /*!< 第1项角度补偿表起始参数号 */

        PAR_CMP_ANG2_INPUT_AX = 95, /*!< 第2项角度补偿基准轴号 */
        PAR_CMP_ANG2_ASSO_AX,       /*!< 第2项角度补偿关联轴号 */
        PAR_CMP_ANG2_REFN,          /*!< 第2项角度补偿参考点坐标 */
        PAR_CMP_ANG2_TYPE,          /*!< 第2项角度补偿类型 */
        PAR_CMP_ANG2_START,         /*!< 第2项角度补偿起点坐标 */
        PAR_CMP_ANG2_NUM,           /*!< 第2项角度补偿点数 */
        PAR_CMP_ANG2_STEP,          /*!< 第2项角度补偿点间距 */
        PAR_CMP_ANG2_MODULO,        /*!< 第2项角度取模补偿使能 */
        PAR_CMP_ANG2_FACTOR,        /*!< 第2项角度补偿倍率 */
        PAR_CMP_ANG2_TABLE,         /*!< 第2项角度补偿表起始参数号 */

        PAR_CMP_ANG3_INPUT_AX = 110,    /*!< 第3项角度补偿基准轴号 */
        PAR_CMP_ANG3_ASSO_AX,       /*!< 第3项角度补偿关联轴号 */
        PAR_CMP_ANG3_REFN,          /*!< 第3项角度补偿参考点坐标 */
        PAR_CMP_ANG3_TYPE,          /*!< 第3项角度补偿类型 */
        PAR_CMP_ANG3_START,         /*!< 第3项角度补偿起点坐标 */
        PAR_CMP_ANG3_NUM,           /*!< 第3项角度补偿点数 */
        PAR_CMP_ANG3_STEP,          /*!< 第3项角度补偿点间距 */
        PAR_CMP_ANG3_MODULO,        /*!< 第3项角度取模补偿使能 */
        PAR_CMP_ANG3_FACTOR,        /*!< 第3项角度补偿倍率 */
        PAR_CMP_ANG3_TABLE,         /*!< 第3项角度补偿表起始参数号 */

        PAR_CMP_QUAD_ENABLE = 125,  /*!< 过象限突跳补偿类型 */
        PAR_CMP_QUAD_VALUE,         /*!< 过象限突跳补偿值 */
        PAR_CMP_QUAD_DELAY_T,       /*!< 过象限突跳补偿延时时间，单位：ms */
        PAR_CMP_QUAD_MIN_VEL,       /*!< 过象限突跳补偿最低速度 */
        PAR_CMP_QUAD_MAX_VEL,       /*!< 过象限突跳补偿最高速度 */
        PAR_CMP_QUAD_ACC_T,         /*!< 过象限突跳补偿加速时间，单位：ms */
        PAR_CMP_QUAD_DEC_T,         /*!< 过象限突跳补偿减速时间，单位：ms */
        PAR_CMP_QUAD_TRQ_VAL,       /*!< 静摩擦补偿扭矩值，取值范围：-10000~10000 */
        PAR_CMP_QUAD_TRQ_VAL2,      /*!< 静摩擦补偿扭矩值2，取值范围：-10000~10000 */
        PAR_CMP_QUAD_TRQ_MAX,       /*!< 静摩擦补偿扭矩值补偿值，取值范围：-10000~10000 */

        PAR_CMP_MULHT_TYPE = 135,       /*!< 多元线性补偿类型 */
        PAR_CMP_MULHT_REFN,             /*!< 多元线性补偿参考点坐标 */
        PAR_CMP_MULHT_BASE_WARP,        /*!< 主轴偏置模型常量 */
        PAR_CMP_MULHT_WARP_SEN_NUM,     /*!< 主轴偏置模型传感器接入个数 */
        PAR_CMP_MULHT_WARP_SEN_LIST,    /*!< 主轴偏置模型传感器编号序列 */
        PAR_CMP_MULHT_WARP_COEF_TABLE,  /*!< 主轴偏置模型系数表起始参数号 */
        PAR_CMP_MULHT_BASE_SLOPE,       /*!< 丝杆斜率模型常量 */
        PAR_CMP_MULHT_SLOPE_SEN_NUM,    /*!< 丝杆斜率模型传感器接入个数 */
        PAR_CMP_MULHT_SLOPE_SEN_LIST,   /*!< 丝杆斜率模型传感器编号序列 */
        PAR_CMP_MULHT_SLOPE_COEF_TABLE, /*!< 丝杆斜率模型系数表起始参数号 */

        PAR_CMP_RATE_ENABLE = 150,      /*!<机床反向的补偿率，在反向间隙与双向螺补时生效 */
        PAR_CMP_RATE_LOW,               /*!<低速时的补偿率，半径100，F1000时的补偿率 */
        PAR_CMP_RATE_HIGH,              /*!<高速时的补偿率，半径100，F5000时的补偿率 */
        PAR_CMP_RATE_MAX,               /*!<最大补偿率 */

        PAR_CMP_FEEDFOR_ENABLE,         /*!<前馈补偿 */
        PAR_CMP_FEEDFOR_RATE,           /*!<前馈补偿补偿系数 */
        PAR_CMP_FEEDFOR_CYCLE,          /*!<前馈补偿提前周期 */

        PAR_CMP_TT_TYPE = 157,
        Par_CMP_TT_RATE,
        PAR_CMP_TT_MAX,
        PAR_CMP_TT_UP_TIME,
        PAR_CMP_TT_DELAY,
        PAR_CMP_TT_DOWN_TIME,

        PAR_CMP_NOSENSORHEAT_TYPE,          /*!< 无温度传感器误差补偿类型 */
        PAR_CMP_NOSENSORHEAT_ONE,           /*!< 无温度传感器误差补偿系数1 */
        PAR_CMP_NOSENSORHEAT_TWO,           /*!< 无温度传感器误差补偿系数2 */
        PAR_CMP_NOSENSORHEAT_THREE,         /*!< 无温度传感器误差补偿系数3 */
        PAR_CMP_NOSENSORHEAT_FOUR,          /*!< 无温度传感器误差补偿系数4 */
        PAR_CMP_NOSENSORHEAT_SPDL,          /*!< 无温度传感器误差补偿关联主轴 */
        PAR_CMP_NOSENSORHEAT_SPDL_ONE,      /*!< 无温度传感器误差补偿主轴系数1 */
        PAR_CMP_NOSENSORHEAT_SPDL_TWO,      /*!< 无温度传感器误差补偿主轴系数2 */
        PAR_CMP_NOSENSORHEAT_SPDL_THREE,    /*!< 无温度传感器误差补偿主轴系数3 */
        PAR_CMP_NOSENSORHEAT_REFN,          /*!< 无温度传感器误差补偿膨胀零点 */
        PAR_CMP_NOSENSORHEAT_DST,           /*!< 无温度传感器误差补偿目标点 */

        PAR_CMP_SPECIAL = 160,  /*!< 专机预留 */ /*!<扩展参数起始地址 */
        PAR_CMP_TOTAL = 500		/*!< 轴补偿参数总个数 */
    };

    public enum EquipmentParam
    {
        PAR_DEV_NAME,               /*!< 设备名称 */
        PAR_DEV_INDEX,              /*!< 设备的系统序号，在系统全部设备中的序号 */
        PAR_DEV_TYPE,               /*!< 设备类型 */
        PAR_DEV_GRP_IDX,            /*!< 在同组设备中的序号 */
        PAR_DEV_ID,                 /*!< 设备ID[生产唯一] */
        PAR_DEV_VENDOR,             /*!< 生产商 */
        PAR_DEV_READONLY_NUM = 8,   /*!< 保留 */
        PAR_DEV_MODE,               /*!< 设备数据字长 */
        PAR_DEV_GNL_NUM				/*!< [设备通用参数的个数] */
    };
    public enum EquipmentMasterParam
    {
        /*! 
         * @name 系统固化参数
         * @{
         */
        PAR_DEV_BRD_FPGA_VER = EquipmentParam.PAR_DEV_GNL_NUM, /*!< FPGA固件程序版本号 */
        PAR_DEV_BRD_CARD_VER, /*!< 主站卡版本号 */
                              /*!@}*/

        PAR_DEV_BRD_SYS_OBJ_NUM = (EquipmentParam.PAR_DEV_GNL_NUM + 10),   /*!< 本地控制对象个数，保留+追加 */
        PAR_DEV_BRD_NET_OBJ_NUM,    /*!< 总线从站控制对象个数 */
        PAR_DEV_BRD_OBJ_NUM,        /*!< 控制对象总个数，本地+总线从站 */

        /*! 
         * @name 用户配置参数
         * @{
         */
        PAR_DEV_BRD_BUS_CYCLE = (EquipmentParam.PAR_DEV_GNL_NUM + 40), /*!< 总线通讯周期 */
        PAR_DEV_BRD_BUS_REQ_TIMES,  /*!< 总线通讯请求次数 */
        PAR_DEV_BRD_BUS_TOPO,       /*!< 拓扑结构（保留） */
                                    /*!@}*/

        PAR_DEV_BRD_SP_ADD_NUM = (EquipmentParam.PAR_DEV_GNL_NUM + 50),    /*!< 追加模拟量主轴数 */

        PAR_DEV_BRD_RESV_TYPE = (EquipmentParam.PAR_DEV_GNL_NUM + 60)	/*!< 本地保留设备类型，预留10个，可扩展 */
    };

    public enum EquipmentAXParam
    {
        PAR_DEV_AX_MODE = EquipmentParam.PAR_DEV_GNL_NUM,  /*!< 工作模式 */
        PAR_DEV_AX_IDX,         /*!< 设备对应的逻辑轴号 */
        PAR_DEV_AX_ENCOD_DIR,   /*!< 编码器反馈取反标志 */
        PAR_DEV_AX_CMD_TYPE,    /*!< 主轴DA输出类型 */
        PAR_DEV_AX_CYC_EN,      /*!< 反馈位置循环使能 */
        PAR_DEV_AX_MT_PPR,      /*!< 反馈位置循环脉冲数 */
        PAR_DEV_AX_ENCOD_TYPE,  /*!< 编码器类型 */
        PAR_DEV_AX_RESERVE1,    /*!< 保留1 */
        PAR_DEV_AX_RESERVE2,    /*!< 保留2 */
        PAR_DEV_AX_RESERVE3     /*!< 保留3 */
    };

    public enum EquipmentMPGParam
    {
        PAR_DEV_MPG_TYPE = EquipmentParam.PAR_DEV_GNL_NUM, /*!< MPG类型 */
        PAR_DEV_MPG_IDX,                    /*!< MPG编号 */
        PAR_DEV_MPG_IN,                     /*!< 档位输入点组号 */
        PAR_DEV_MPG_DIR_FLAG,               /*!< 各轴方向取反标志 */
        PAR_DEV_MPG_MULT_FACTOR,            /*!< 倍率放大系数 */
        PAR_DEV_MPG_PAR_NUM					/*!< MPG实际配置参数数目 */
    };

    public enum EquipmentMCP_LocAndMCP_NetParam
    {
        PAR_DEV_MCP_TYPE = EquipmentParam.PAR_DEV_GNL_NUM,	/*!< MCP类型：1-A / 2-B /3-C */
        PAR_DEV_MCP_MPG_IDX,	/*!< 手摇编号 */
        PAR_DEV_MCP_X_BASE,     /*!< 输入点X寄存器起始组号(PAR_DEV_GNL_NUM+2) */
        PAR_DEV_MCP_X_GRPN,     /*!< 输入点占用X寄存器组数 */
        PAR_DEV_MCP_Y_BASE,     /*!< 输出点寄存器起址 */
        PAR_DEV_MCP_Y_GRPN,     /*!< 输出点占用Y寄存器组数 */
        PAR_DEV_MCP_MPG_DIR,    /*!< 手摇方向取反标志 */
        PAR_DEV_MCP_MPG_MULT,   /*!< 手摇倍率放大系数 */
        PAR_DEV_MCP_CODE_TYPE,  /*!< 波段开关编码类型 */
        PAR_DEV_MCP_SPDL_NUM    /*!< 追加模拟量主轴数（temp） */
    };

    public enum EquipmentSerialParam
    {
        PAR_SERIAL_BIT_LEN = EquipmentParam.PAR_DEV_GNL_NUM,   /*!< 收发数据位长度 */
        PAR_SERIAL_STOP,        /*!< 停止位 */
        PAR_SERIAL_PARITY,      /*!< 奇偶校验位 */
        PAR_SERIAL_BAUDRATE,    /*!< 波特率 */
        PAR_SERIAL_PAR_NUM		/*!< SERIAL实际配置参数数目 */
    };

    public enum EquipmentLanParam
    {
        PAR_LAN_IP0 = EquipmentParam.PAR_DEV_GNL_NUM, /*!< IP0 */
                                       // PAR_LAN_IP1,	// IP1
                                       // PAR_LAN_IP2,	// IP2
                                       // PAR_LAN_IP3,	// IP3
        PAR_LAN_GATE0,  /*!< GATE0 */
                        // PAR_LAN_GATE1,	// GATE1
                        // PAR_LAN_GATE2,	// GATE2
                        // PAR_LAN_GATE3,	// GATE3
        PAR_LAN_MASK0,      /*!< MASK0 */
                            // PAR_LAN_MASK1,	// MASK1
                            // PAR_LAN_MASK2,	// MASK2
                            // PAR_LAN_MASK3,	// MASK3
        PAR_LAN_PAR_NUM     /*!< LAN实际配置参数数目 */
    };

    public enum EquipmentWComParam
    {
        PAR_WCOM_TYPE = EquipmentParam.PAR_DEV_GNL_NUM, /*!< WCOM类型 */
        PAR_WCOM_RESERVE,   /*!< 保留 */
        PAR_WCOM_PAR_NUM	/*!< WCOM实际配置参数数目 */
    };

    public enum EquipmentGatherParam
    {
        PAR_GATHER_TYPE = EquipmentParam.PAR_DEV_GNL_NUM,  /*!< 采集卡类型 1:KZM-6000 */
        PAR_GATHER_SERIAL_IDX,    /*!< 串口设备号 */
        PAR_GATHER_SENSOR_NUM,    /*!< 传感器数 */
        PAR_GATHER_IN_X_BASE,     /*!< 输入点起始组号 */
        PAR_GATHER_PAR_NUM        /*!< GATHER实际配置参数数目 */
    };

    public enum DevNcobjType
    {
    	DEV_NCOBJ_NULL_LOC = 1000,// 本地设备--非网络设备
    	DEV_NCOBJ_SPDL_LOC,
    	DEV_NCOBJ_AXIS_LOC,
    	DEV_NCOBJ_IN_LOC,
    	DEV_NCOBJ_OUT_LOC,
    	DEV_NCOBJ_AD_LOC,
    	DEV_NCOBJ_DA_LOC,
    	DEV_NCOBJ_IOMD_LOC,		// NCUC总线的IO集成模块
    	DEV_NCOBJ_MCP_LOC,
    	DEV_NCOBJ_MPG_LOC,
    	DEV_NCOBJ_NCKB_LOC,
    	DEV_NCOBJ_SENSOR_LOC,	// 传感器设备
    	DEV_NCOBJ_SERIAL_LOC,	// 串口设备
    	DEV_NCOBJ_GATHER_LOC,	// 温度采集卡
    
    	DEV_NCOBJ_NULL_NET = 2000,// 网络设备--ncuc\ethercat\syqnet...
    	DEV_NCOBJ_SPDL_NET,
    	DEV_NCOBJ_AXIS_NET,
    	DEV_NCOBJ_IN_NET,
    	DEV_NCOBJ_OUT_NET,
    	DEV_NCOBJ_AD_NET,
    	DEV_NCOBJ_DA_NET,
    	DEV_NCOBJ_IOMD_NET,		// NCUC总线的IO集成模块
    	DEV_NCOBJ_MCP_NET,
    	DEV_NCOBJ_MPG_NET,
    	DEV_NCOBJ_NCKB_NET,
    	DEV_NCOBJ_SENSOR_NET,	// 传感器
    	DEV_NCOBJ_PIDC_NET,		// 位控板
    
    	// 此处扩展新的类型
    	DEV_NCOBJ_ENCOD_NET		// 编码器
    };

    public enum ToolParaIndex
    {
        /*! 
	 * @name 刀具几何相关参数索引
	 * @{
	 */
        GTOOL_DIR = 0,  /*!< 方向  */
        GTOOL_LEN1, /*!< 长度1(铣：刀具长度z；车：X偏置) */
        GTOOL_LEN2, /*!< 长度2(铣：刀具长度x；车：Y偏置) */
        GTOOL_LEN3, /*!< 长度3(铣：刀具长度y；车：Z偏置) */
        GTOOL_LEN4, /*!< 长度4 */
        GTOOL_LEN5, /*!< 长度5 */
        GTOOL_RAD1, /*!< 半径1(铣：刀具半径；车：刀尖半径) */
        GTOOL_RAD2, /*!< 半径2 */
        GTOOL_ANG1, /*!< 角度1 */
        GTOOL_ANG2, /*!< 角度2 */
                    /*!@}*/

        GTOOL_BASE_LEN1,/*!<长度1(铣：刀具基体长度z) */
        GTOOL_BASE_LEN2,/*!<长度2(铣：刀具基体长度x) */
        GTOOL_BASE_LEN3,/*!<长度3(铣：刀具基体长度y) */

        GTOOL_BASE_XDONE,/*!<X试切标志 */
        GTOOL_BASE_YDONE,/*!<Y试切标志 */
        GTOOL_BASE_ZDONE,/*!<Z试切标志 */
        GTOOL_TOTAL,

        /*! 
	    * @name 刀具磨损相关参数索引
	    * @{
	    */
        WTOOL_LEN1 = (Int32)HNCDATADEF.MAX_GEO_PARA,/*!< (铣：长度磨损z；车：Z磨损) */
        WTOOL_LEN2, /*!< 长度2(铣：长度磨损x) */
        WTOOL_LEN3, /*!< 长度3(铣：长度磨损y) */
        WTOOL_LEN4, /*!< 长度4 */
        WTOOL_LEN5, /*!< 长度5 */
        WTOOL_RAD1, /*!< 半径1(铣：半径磨损；车：X磨损) */
        WTOOL_RAD2, /*!< 半径2 */
        WTOOL_ANG1, /*!< 角度1 */
        WTOOL_ANG2, /*!< 角度2 */
                    /*!@}*/
        WTOOL_TOOL_LEN_SET_X,   /*!< 走心机刀具刀长设定X */
        WTOOL_TOOL_LEN_SET_Y,   /*!< 走心机刀具刀长设定Y */
        WTOOL_TOOL_LEN_SET_Z,   /*!< 走心机刀具刀长设定Z */
        WTOOL_TOOL_LEN_FIX_X,   /*!< 走心机刀具刀长修正X */
        WTOOL_TOOL_LEN_FIX_Y,   /*!< 走心机刀具刀长修正Y */
        WTOOL_TOOL_LEN_FIX_Z,	/*!< 走心机刀具刀长修正Z */

        WTOOL_TOTAL,

        /*! 
	    * @name 刀具扩展参数--刀具管理参数，各刀具类型通用
	    * @{
	    */
        TETOOL_PARA0 = (Int32)HNCDATADEF.MAX_GEO_PARA +  (Int32)HNCDATADEF.MAX_WEAR_PARA,
    	TETOOL_PARA1,
    	TETOOL_PARA2,
    	TETOOL_PARA3,
    	TETOOL_PARA4,
    	TETOOL_PARA5,
    	TETOOL_PARA6,
    	TETOOL_PARA7,
    	TETOOL_PARA8,
    	TETOOL_PARA9,

        METOOL_PARA0, /*!< 测量参数0～9 */
        METOOL_PARA1,
        METOOL_PARA2,
        METOOL_PARA3,
        METOOL_PARA4,
        METOOL_PARA5,
        METOOL_PARA6,
        METOOL_PARA7,
        METOOL_PARA8,
        METOOL_PARA9,
        /*!@}*/

        EXTOOL_TOTAL,

        /*! 
	    * @name 刀具监控参数
	    * @{
	    */
        MOTOOL_TYPE = (Int32)HNCDATADEF.MAX_GEO_PARA +  (Int32)HNCDATADEF.MAX_WEAR_PARA + (Int32)HNCDATADEF.MAX_TOOL_EXPARA,/*!< 刀具监控类型，按位有效，可选多种监控方式同时监控 0:关闭 0x01:安装次数 0x02:切削时间 0x04:磨损 0x08:切削里程 0x10:切削能耗 0x20:主轴转数 */
        MOTOOL_GROUP,       /*!< 刀具所属分组号 */
        MOTOOL_RESAULT_LIFE,/*!< 综合寿命(0~100) */
        MOTOOL_WAR_PER,     /*!< 预警比例(0~100) */
        MOTOOL_ALM_PER,     /*!< 报警比例(0~100) */

        MOTOOL_MAX_LIFE1,   /*!< 最大寿命 */
        MOTOOL_ALM_LIFE1,   /*!< 预警寿命 */
        MOTOOL_ACT_LIFE1,   /*!< 实际寿命 */
        MOTOOL_ACT_LIFE_LO1,/*!< 实际寿命(保留) */
        MOTOOL_CNT_UNIT1,   /*!< 计数单位 切削里程0:米 1:百米 2:千米，切削能耗0:瓦时 1:千瓦时，主轴转数0:默认 1:千 2:万 3:十万 4:百万 5:千万 6:亿 */
        MOTOOL_WEIGHT1,     /*!< 权重(0~100) */

        MOTOOL_MAX_LIFE2,   /*!< 最大寿命 */
        MOTOOL_ALM_LIFE2,   /*!< 预警寿命 */
        MOTOOL_ACT_LIFE2,   /*!< 实际寿命 */
        MOTOOL_ACT_LIFE_LO2,/*!< 实际寿命(保留) */
        MOTOOL_CNT_UNIT2,   /*!< 计数单位 切削里程0:米 1:百米 2:千米，切削能耗0:瓦时 1:千瓦时，主轴转数0:默认 1:千 2:万 3:十万 4:百万 5:千万 6:亿 */
        MOTOOL_WEIGHT2,     /*!< 权重(0~100) */

        MOTOOL_MAX_LIFE3,   /*!< 最大寿命 */
        MOTOOL_ALM_LIFE3,   /*!< 预警寿命 */
        MOTOOL_ACT_LIFE3,   /*!< 实际寿命 */
        MOTOOL_ACT_LIFE_LO3,/*!< 实际寿命(保留) */
        MOTOOL_CNT_UNIT3,   /*!< 计数单位 切削里程0:米 1:百米 2:千米，切削能耗0:瓦时 1:千瓦时，主轴转数0:默认 1:千 2:万 3:十万 4:百万 5:千万 6:亿 */
        MOTOOL_WEIGHT3,     /*!< 权重(0~100) */

        MOTOOL_MAX_LIFE4,   /*!< 最大寿命 */
        MOTOOL_ALM_LIFE4,   /*!< 预警寿命 */
        MOTOOL_ACT_LIFE4,   /*!< 实际寿命 */
        MOTOOL_ACT_LIFE_LO4,/*!< 实际寿命(保留) */
        MOTOOL_CNT_UNIT4,   /*!< 计数单位 切削里程0:米 1:百米 2:千米，切削能耗0:瓦时 1:千瓦时，主轴转数0:默认 1:千 2:万 3:十万 4:百万 5:千万 6:亿 */
        MOTOOL_WEIGHT4,     /*!< 权重(0~100) */

        MOTOOL_MAX_LIFE5,   /*!< 最大寿命 */
        MOTOOL_ALM_LIFE5,   /*!< 预警寿命 */
        MOTOOL_ACT_LIFE5,   /*!< 实际寿命 */
        MOTOOL_ACT_LIFE_LO5,/*!< 实际寿命(保留) */
        MOTOOL_CNT_UNIT5,   /*!< 计数单位 切削里程0:米 1:百米 2:千米，切削能耗0:瓦时 1:千瓦时，主轴转数0:默认 1:千 2:万 3:十万 4:百万 5:千万 6:亿 */
        MOTOOL_WEIGHT5,     /*!< 权重(0~100) */
                            /*! 最多可同时设置监控5种方式，若超过5种则仅监控前五个 */
                            /*!@}*/

        MOTOOL_TOTAL,

        /*! 
	    * @name 刀具一般信息
	    * @{
	    */
        INFTOOL_ID = (Int32)HNCDATADEF.MAX_GEO_PARA +  (Int32)HNCDATADEF.MAX_WEAR_PARA + (Int32)HNCDATADEF.MAX_TOOL_EXPARA + (Int32)HNCDATADEF.MAX_TOOL_MONITOR,/*!< 刀具索引号 */
        INFTOOL_MAGZ,       /*!< 	刀具所属刀库号 */
        INFTOOL_CH,         /*!< 	刀具所属通道号 */
        INFTOOL_TYPE,       /*!< 	刀具类型 */
        INFTOOL_STATE,      /*!< 	刀具状态字 */
        INFTOOL_G64MODE,    /*!< 	刀具高速高精加工模式 */
        INFTOOL_SIZE,       /*!<    刀具规格*/
        TOOL_INFO_NAME,     /*!<	刀具名称 */
        EXTOOL_S_LIMIT,     /*!< S转速限制 */
        EXTOOL_F_LIMIT,     /*!< F转速限制 */
        EXTOOL_LARGE_LEFT,  /*!< 大刀具干涉左刀位 */
        EXTOOL_LARGE_RIGHT, /*!< 大刀具干涩右刀位 */
        EXTOOL_LTOOL_PARM, /*!< 车刀,0x01:旋转方向,0x02刀具编辑锁定  */
                           /*!@}*/
        INFTOOL_TOTAL,

        /*! 
	 * @name 刀具断刀检测
	 * @{
	 */
        BKTOOL_COUNT = (Int32)HNCDATADEF.MAX_GEO_PARA + (Int32)HNCDATADEF.MAX_WEAR_PARA + (Int32)HNCDATADEF.MAX_TOOL_EXPARA + (Int32)HNCDATADEF.MAX_TOOL_MONITOR + (Int32)HNCDATADEF.MAX_TOOL_BASE, /*!< 检测次数 */
        BKTOOL_TRIGGER, /*!< 检测到断刀触发 0:正常 1:断刀 */
        BKTOOL_BACK, /*!< 反馈 0:等待反馈 1:正确 2:误报 */
        BKTOOL_POWER,
        BKTOOL_POWER20 = BKTOOL_POWER + 19, /*!< 共5组20个值,4个一组，分为上阈值，下阈值，前段时间常数和后段时间常数 */
                                            /*!@}*/

        BKTOOL_TOTAL,

        TOOL1_X1,  /*!< 刀具形状数据 */
        TOOL1_Y1,
        TOOL1_Z1,
        TOOL1_I1,
        TOOL1_J1,
        TOOL1_K1,
        TOOL1_X2,
        TOOL1_Y2,
        TOOL1_Z2,
        TOOL1_I2,
        TOOL1_J2,
        TOOL1_K2,

        TOOL_PARA_TOTAL /*!< MAX_TOOL_PARA */
    };

    public enum MagzHeadIndex
    {
        MAGZTAB_HEAD = 0,   /*!< 刀库表起始偏移地址（刀具号段+刀位属性段） */
        MAGZTAB_TOOL_NUM,   /*!< 刀库表中刀具数 */
        MAGZTAB_CUR_TOOL,   /*!< 当前刀具号 */
        MAGZTAB_CUR_POT,    /*!< 当前刀位号 */
        MAGZTAB_REF_TOOL,   /*!< 标刀号 */
        MAGZTAB_CHAN,       /*!< 刀库所属通道号 */
        MAGZTAB_TYPE,       /*!< 刀库类型 */
        SWAP_LEFT_TOOL,     /*!< 机械手左刀位刀具号 */
        SWAP_RIGHT_TOOL,    /*!< 机械手右刀位刀具号 */
        MAGZTAB_RETURN,     /*!< 刀库还刀方式 0:定点 1:随机 2:大刀定点小刀随机 */
        MAGZTAB_WAIT,       /*!< 中间待刀位 */
        MAGZTAB_CUR_TOOL_SEC,/*!< 第二个当前刀具号(车铣复合用) */
        // 预留

        MAGZTAB_TOTAL
    };

    public enum HncSampleType
    {
    	SAMPL_NULL_TYPE = 0,// 空类型
    	SAMPL_AXIS_CMD = 1,// 轴的指令位置
    	SAMPL_AXIS_ACT,		// 轴的实际位置
    	SAMPL_FOLLOW_ERR,	// 轴的跟随误差
    	SAMPL_CMD_INC,		// 轴的指令速度
    	SAMPL_ACT_VEL,		// 轴的实际速度
    	SAMPL_ACT_TRQ,		// 轴的负载电流
    	SAMPL_CMD_POS,		// 指令电机位置
    	SAMPL_CMD_PULSE,	// 指令脉冲位置
    	SAMPL_ACT_POS,		// 实际电机位置
    	SAMPL_ACT_PULSE,	// 实际脉冲位置
    	SAMPL_TOL_COMP,		// 补偿值
    	SAMPL_SYS_VAL = 101,// 系统变量
    	SAMPL_CHAN_VAL,		// 通道变量
    	SAMPL_AXIS_VAL,		// 轴变量
    	SAMPL_X_REG,		// X寄存器
    	SAMPL_Y_REG,		// Y寄存器
    	SAMPL_F_AXIS_REG,	// 轴F寄存器
    	SAMPL_G_AXIS_REG,	// 轴G寄存器
    	SAMPL_F_CHAN_REG,	// 通道F寄存器
    	SAMPL_G_CHAN_REG,	// 通道G寄存器
    	SAMPL_F_SYS_REG,	// 系统F寄存器
    	SAMPL_G_SYS_REG,	// 系统G寄存器
    	SAMPL_R_REG,		// R寄存器
    	SAMPL_B_REG,		// B寄存器
        SAMPL_I_REG,        /*!< I寄存器 */
        SAMPL_Q_REG,        /*!< Q寄存器 */
        SAMPL_K_REG,        /*!< K寄存器 */
        SAMPL_W_REG,        /*!< W寄存器 */
        SAMPL_D_REG,        /*!< D寄存器 */
        SAMPL_T_REG,        /*!< T寄存器 */
        SAMPL_C_REG,        /*!< C寄存器 */
        SAMPL_TOTAL
    };

    public class HNCDATADEF
    {
        public const Int32 SYS_CHAN_NUM =  4  ;//  系统最大通道数 
        public const Int32 SYS_AXES_NUM =  24  ;//  系统最大进给轴数 
        public const Int32 SYS_SPDL_NUM =  8  ;//  系统最大主轴数 
        public const Int32 SYS_NCBRD_NUM =  4  ;//  主控制设备数 
        public const Int32 SYS_NCOBJ_NUM =  80  ;//  从设备控制对象（部件）数 
        public const Int32 CHAN_AXES_NUM =  9  ;//  通道最大轴数 
        public const Int32 CHAN_SPDL_NUM =  4  ;//  通道最大主轴数 
        public const Int32 MAX_SMC_AXES_NUM =  16  ;//  最多运控轴数 
        public const Int32 TOTAL_AXES_NUM =  SYS_AXES_NUM +  SYS_SPDL_NUM  ;//  系统最大逻辑轴数 
        public const Int32 SYS_PART_NUM =  SYS_NCOBJ_NUM  ;//  系统设备接口数 
        public const Int32 CH_GRAPH_EVERY_VIEW_PAR_RESV = 3;/*! NC参数中图形参数,每个视角的保留参数数量 */
        public const Int32 NCU_PARAM_ID_BASE =  0 ;
        public const Int32 MAC_PARAM_ID_BASE =  10000 ;
        public const Int32 CHAN_PARAM_ID_BASE =  40000 ;
        public const Int32 AXIS_PARAM_ID_BASE =  100000 ;
        public const Int32 COMP_PARAM_ID_BASE =  300000 ;
        public const Int32 DEV_PARAM_ID_BASE =  500000 ;
        public const Int32 TABLE_PARAM_ID_BASE =  700000  ;//  数据表参数 
        public const Int32 NCU_PARAM_ID_NUM = 2000;
        public const Int32 MAC_PARAM_ID_NUM = 2000;
        public const Int32 CHAN_PARAM_ID_NUM =  1000 ;
        public const Int32 AXIS_PARAM_ID_NUM =  1000 ;
        public const Int32 COMP_PARAM_ID_NUM =  1000 ;
        public const Int32 DEV_PARAM_ID_NUM =  1000 ;
        public const Int32 TABLE_PARAM_ID_NUM =  100000  ;//  分配给数据表参数的ID数 
        public const Int32 PARAMAN_FILE_NCU =  0  ;//  NC参数 
        public const Int32 PARAMAN_FILE_MAC =  1  ;//  机床用户参数 
        public const Int32 PARAMAN_FILE_CHAN =  2  ;//  通道参数 
        public const Int32 PARAMAN_FILE_AXIS =  3  ;//  坐标轴参数 
        public const Int32 PARAMAN_FILE_ACMP =  4  ;//  误差补偿参数 
        public const Int32 PARAMAN_FILE_CFG =  5  ;//  设备接口参数 
        public const Int32 PARAMAN_FILE_TABLE =  6  ;//  数据表参数 
        public const Int32 PARAMAN_FILE_BOARD =  7  ;//  主站参数 
        public const Int32 PARAMAN_MAX_FILE_LIB =  7  ;//  参数结构文件最大分类数 
        public const Int32 PARAMAN_MAX_PARM_PER_LIB =  1000  ;//  各类参数最大条目数 
        public const Int32 PARAMAN_MAX_PARM_EXTEND =  1000  ;//  分支扩展参数最大条目数 
        public const Int32 PARAMAN_LIB_TITLE_SIZE =  16  ;//  分类名字符串最大长度 
        public const Int32 PARAMAN_REC_TITLE_SIZE =  16  ;//  子类名字符串最大长度 
        public const Int32 PARAMAN_ITEM_NAME_SIZE =  64  ;//  参数条目字符串最大长度 
        public const Int32 SERVO_PARM_START_IDX =  500  ;//  伺服参数起始参数号 
        public const Int32 SERVO_PARM_TOTAL_NUM =  100;//伺服参数个数 
        public const Int32 AX_ENCODER_MASK =  0x00FF ;
        public const Int32 AX_NC_CMD_MASK =  0x00F0 ;
        public const Int32 AX_NC_TRACK_ERR =  0x0100 ;
        public const Int32 AX_NC_CYC64_MODE =  0x1000 ;
        public const Int32 MAX_GEO_PARA = 24;//  刀具几何参数个数 
        public const Int32 MAX_WEAR_PARA = 24;//  刀具磨损参数个数 
        public const Int32 MAX_TOOL_EXPARA = 24;//  刀具扩展参数个数 
        public const Int32 MAX_TOOL_MONITOR = 36;//  刀具监控参数个数 
        public const Int32 MAX_TOOL_BASE = 24  ;//  刀具一般信息参数个数 
        public const Int32 MAX_TOOL_BREAK = 24;     /*!< 刀具断刀检测参数个数 */
        public const Int32 MAX_TOOL_PARA = 200  ;//  刀具基本参数个数  (24 +  24 +  24 +  24 +  24 +  24 +  24  =  168) 
        public const Int32 MAGZ_HEAD_SIZE = 16  ;//  刀库数据表头大小 
        public const Int32 MTOOL_RAD = (Int32)ToolParaIndex.GTOOL_RAD1  ;//  刀具半径 
        public const Int32 MTOOL_LEN = (Int32)ToolParaIndex.GTOOL_LEN1  ;//  刀具长度 
        public const Int32 MTOOL_RAD_WEAR = (Int32)ToolParaIndex.WTOOL_RAD1  ;//  铣刀:半径磨损补偿（径向） 
        public const Int32 MTOOL_LEN_WEAR = (Int32)ToolParaIndex.WTOOL_LEN1  ;//  铣刀:长度磨损补偿（轴向） 
        public const Int32 LTOOL_RAD = (Int32)ToolParaIndex.GTOOL_RAD1  ;//  刀尖半径 
        public const Int32 LTOOL_DIR = (Int32)ToolParaIndex.GTOOL_DIR  ;//  刀尖方向 
        public const Int32 LTOOL_RAD_WEAR = (Int32)ToolParaIndex.WTOOL_RAD1  ;//  车刀:刀具磨损值（径向）（相对值） 
        public const Int32 LTOOL_LEN_WEAR = (Int32)ToolParaIndex.WTOOL_LEN1  ;//  车刀:刀具磨损值（轴向）（相对值） 
        public const Int32 LTOOL_XOFF =  (Int32)ToolParaIndex.GTOOL_LEN1  ;//  车刀：刀具偏置值（径向）（绝对值）  =  试切时X值  -  试切直径/2 
        public const Int32 LTOOL_YOFF =  (Int32)ToolParaIndex.GTOOL_LEN2 ;
        public const Int32 LTOOL_ZOFF =  (Int32)ToolParaIndex.GTOOL_LEN3  ;//  车刀：刀具偏置值（轴向）（绝对值）  =  试切时Z值  -  试切长度 
        public const Int32 LTOOL_XDONE =  (Int32)ToolParaIndex.TETOOL_PARA0  ;//  X试切标志 
        public const Int32 LTOOL_YDONE =  (Int32)ToolParaIndex.TETOOL_PARA0 ;
        public const Int32 LTOOL_ZDONE =  (Int32)ToolParaIndex.TETOOL_PARA1  ;//  Z试切标志 
        public const Int32 SPDL_RESOLUTION =  1000  ;//  主轴转速分辨率 
        public const Int32 MAX_ROW_IN_ONE_SCAN =  200  ;
        public const Int32 MAX_SCAN_ROW_NUM_RANDOM =  1000 ;
        public const Int32 GIVEN_ROW_IDLE =  0 ;
        public const Int32 GIVEN_ROW_WAIT_PROG_OK =  1  ;//等待任意行程序准备好 
        public const Int32 GIVEN_ROW_SCANING =  2  ;//任意行扫描中 
        public const Int32 GIVEN_ROW_WAIT_SCAN_ACK =  3  ;//任意行等待界面给出应答后再向后继续扫描 
        public const Int32 GIVEN_ROW_WAIT_SUBPROG_OK =  4  ;//等待任意行子程序准备好 
        public const Int32 GIVEN_ROW_SUBPROG_OK =  5  ;//任意行子程序准备好 
        public const Int32 GIVEN_ROW_SUBPROG_EXCUTING =  6  ;//任意行子程序执行中 
        public const Int32 GIVEN_ROW_SCAN_ERR =  7  ;//任意行扫描中发现语法错误 
        public const Int32 GIVEN_ROW_TYPE1_WAIT_Z_MOVE =  8  ;//任意行扫描模式1，等待Z轴移动指令 
        public const Int32 SMPL_MAX_CHAN_NUM = 64;// 最大采样通道数。PS：内存上预留64个通道，但目前仅可用32个
        public const Int32 SMPL_CHAN_NUM =  32  ;//  采样通道数 
        public const Int32 SMPL_DATA_NUM =  10000  ;//  每采样通道的采样点数 
        public const Int32 T_CTRL_CHANGE_DIRECT =  0X1 ;
        public const Int32 T_CTRL_TOOL_MODE =  0X2 ;
        public const Int32 VAR_WRITE_WAIT =  0X1 ;
        public const Int32 VAR_READ_WAIT =  0X2 ;
    }
}
