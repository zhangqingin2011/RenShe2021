using System;
using System.Collections.Generic;

namespace HNCAPI_INTERFACE
{
    public enum HncChannel
    {
        HNC_CHAN_IS_EXIST = 0,      /*!< 通道是否存在 {Get(Bit32)} */
        HNC_CHAN_MAC_TYPE,          /*!< 通道的机床类型 {Get(Bit32)} */
        HNC_CHAN_AXES_MASK,         /*!< 轴掩码 {Get(Bit32)} */
        HNC_CHAN_AXES_MASK1,        /*!< 轴掩码1 {Get(Bit32)} */
        HNC_CHAN_NAME,              /*!< 通道名 {Get(Bit8[PARAM_STR_LEN])} */
        HNC_CHAN_CMD_TYPE,          /*!< 读取当前G代码的标志 {Get(Bit32)} */
        HNC_CHAN_CMD_FEEDRATE,      /*!< 指令进给瞬时速度 {Get(fBit64)} */
        HNC_CHAN_ACT_FEEDRATE,      /*!< 实际进给速度 {Get(fBit64)} */
        HNC_CHAN_PROG_FEEDRATE,     /*!< 编程指令速度 {Get(fBit64)} */
        HNC_CHAN_FEED_OVERRIDE,     /*!< 进给修调 {Get(Bit32)} */
        HNC_CHAN_RAPID_OVERRIDE,    /*!< 快移修调 {Get(Bit32)} */
        HNC_CHAN_MCODE,             /*!< 通道的M指令 {Get(Bit32)} */
        HNC_CHAN_TCODE,             /*!< 通道的T指令 {Get(Bit32)} */
        HNC_CHAN_TOFFS,             /*!< 通道中的刀偏号 {Get(Bit32)} */
        HNC_CHAN_TOOL_USE,          /*!< 当前刀具 {Get(Bit32)} */
        HNC_CHAN_TOOL_RDY,          /*!< 准备好交换的刀具 {Get(Bit32)} */
        HNC_CHAN_MODE,              /*!< 模式(返回值数据定义见下面) {Get(Bit32)} */
        HNC_CHAN_IS_MDI,            /*!< MDI {Get(Bit32)} */
        HNC_CHAN_CYCLE,             /*!< 循环启动 {Get(Bit32), Set(NULL)} */
        HNC_CHAN_HOLD,              /*!< 进给保持 {Get(Bit32), Set(NULL)} */
        HNC_CHAN_IS_PROGSEL,        /*!< 已选程序 {Get(Bit32)} */
        HNC_CHAN_IS_PROGEND,        /*!< 程序运行完成 {Get(Bit32)} */
        HNC_CHAN_IS_THREADING,      /*!< 螺纹加工 {Get(Bit32)} */
        HNC_CHAN_IS_RIGID,          /*!< 刚性攻丝 {Get(Bit32)} */
        HNC_CHAN_IS_REWINDED,       /*!< 重运行复位状态 {Get(Bit32)} */
        HNC_CHAN_IS_ESTOP,          /*!< 急停 {Get(Bit32)} */
        HNC_CHAN_IS_RESETTING,      /*!< 复位 {Get(Bit32)} */
        HNC_CHAN_IS_RUNNING,        /*!< 运行中 {Get(Bit32)} */
        HNC_CHAN_IS_HOMING,         /*!< 回零中 {Get(Bit32)} */
        HNC_CHAN_IS_MOVING,         /*!< 轴移动中 {Get(Bit32)} */
        HNC_CHAN_DIAMETER,          /*!< 直半径编程 {Get(Bit32)} */
        HNC_CHAN_VERIFY,            /*!< 校验 {Get(Bit32), Set(Bit32)} */
        HNC_CHAN_RUN_ROW,           /*!< 运行行 {Get(Bit32)} */
        HNC_CHAN_DCD_ROW,           /*!< 译码行 {Get(Bit32)} */
        HNC_CHAN_SEL_PROG,          /*!< 选择程序的编号 {Get(Bit32)} */
        HNC_CHAN_RUN_PROG,          /*!< 运行程序的编号 {Get(Bit32)} */
        HNC_CHAN_PART_CNTR,         /*!< 加工计数 {Get(Bit32), Set(Bit32)} */
        HNC_CHAN_PART_STATI,        /*!< 工件总数 {Get(Bit32), Set(Bit32)} */
        HNC_CHAN_HMI_RESET,         /*!< HMI复位 {Set(NULL)} */
        HNC_CHAN_CHG_PROG,          /*!< 程序修改标志 {Set(NULL)} */
            HNC_CHAN_PERIOD_TOTAL,      /*!< 过时的 */
        HNC_CHAN_LAX,               /*!< 通道轴对应的逻辑轴号 {Get(Bit32)} */
        HNC_CHAN_AXIS_NAME,         /*!< 编程轴名 {Get(Bit8[PARAM_STR_LEN])} */
        HNC_CHAN_SPDL_NAME,         /*!< 编程主轴名 {Get(Bit8[PARAM_STR_LEN])} */
        HNC_CHAN_MODAL,             /*!< 通道模态 共80组 {Get(Bit32)} */
        HNC_CHAN_SPDL_LAX,          /*!< 通道主轴对应的逻辑轴号，动态 {Get(Bit32)} */
        HNC_CHAN_SPDL_PARA_LAX,     /*!< 通道主轴对应的逻辑轴号，静态 {Get(Bit32)} */
        HNC_CHAN_CMD_SPDL_SPEED,    /*!< 主轴指令速度 {Get(fBit64)} */
        HNC_CHAN_ACT_SPDL_SPEED,    /*!< 主轴实际速度 {Get(fBit64)} */
        HNC_CHAN_SPDL_OVERRIDE,     /*!< 主轴修调 {Get(Bit32)} */
        HNC_CHAN_DO_HOLD,           /*!< 设置进给保持  */
        HNC_CHAN_BP_POS,            /*!< 断点位置 {Get(Bit32)} */
        HNC_CHAN_PROG_FLOW,         /*!< 正在调用手动子程序 {Get(Bit32)} */
        HNC_CHAN_H_OFF,             /*!< 当前使用刀具长度补偿号 {Get(Bit32)} */
        HNC_CHAN_D_OFF,             /*!< 当前使用刀具半径补偿号 {Get(Bit32)} */
        HNC_CHAN_BP_POS_EX,
        HNC_CHAN_IS_SBL,            /*!< 是否单段模式 {Get(Bit32)} */
        HNC_CHAN_PART_NEED,         /*!< 加工需求数 {Get(Bit32), Set(Bit32)} */
        HNC_CHAN_S_CODE,            /*!< 通道中主轴转速 {Get(Bit32), Set(Bit32)} */
        HNC_CHAN_F_G94_SPEED,           /*!< 通道中记录的F分进给模态速度 {Get(Bit32), Set(Bit32)} */
        HNC_CHAN_F_G95_SPEED,           /*!< 通道中记录的F转进给模态速度 {Get(Bit32), Set(Bit32)} */
        HNC_CHAN_EXT_FILE_STATE,    /*< 界面告诉内核文件写/删状态(仿FANUC的DPRINT指令用) */
        HNC_CHAN_REFRESH_RUN_INFO,  /*!< 刷新程序行信息 */
        HNC_CHAN_MDI_IN_EXIT_ABLE, /*!< 是否可以进入/退出MDi状态 */
        HNC_CHAN_G50_ZERO,           /*!< G50零点偏置坐标 {Get(fBit64)}*/
        HNC_CHAN_CMD_FEEDRATE_MEAN,     /*!< 指令进给平均速度 {Get(fBit64)} */
        HNC_CHAN_TOTAL
    };

    public class HNCCHAN
    {
       public const Int32 CHAN_MODE_RESET =  0 ;
       public const Int32 CHAN_MODE_AUTO =  1 ;
       public const Int32 CHAN_MODE_JOG =  2 ;
       public const Int32 CHAN_MODE_STEP =  3 ;
       public const Int32 CHAN_MODE_MPG =  4 ;
       public const Int32 CHAN_MODE_HOME =  5 ;
       public const Int32 CHAN_MODE_PMC =  6 ;
       public const Int32 CHAN_MODE_SBL =  7 ;
    }
}
