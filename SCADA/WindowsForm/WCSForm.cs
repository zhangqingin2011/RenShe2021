using HNC.MOHRSS.Model;
using HncDataInterfaces;
using Hsc3.Comm;
using Mono.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Forms;

namespace SCADA
{


    public partial class WCSForm : Form
    {
        public WCSForm()
        {
            InitializeComponent();
            ReNewUI();
        }

  

        private void timer1_Tick(object sender, EventArgs e)
        {
            ReNewUI();
        }

        private void ReNewUI()
        {
            //刷新AGV相关数据
            int StoreState = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.LineStoreBit];
            MainForm.agvdata.LineStoreState = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.LineStoreBit];
            MainForm.agvdata.FinStoreState = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.FinStoreBit];
            MainForm.agvdata.RawStoreState = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.RawStore];
            MainForm.agvdata.Task_Raw_Line = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Task_Raw_Line];
            MainForm.agvdata.Task_Fin_Line = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Task_Fin_Line];
            MainForm.agvdata.Task_Line_Raw = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Task_Line_Raw];
            MainForm.agvdata.Task_Line_Fin = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Task_Line_Fin];
            MainForm.agvdata.Task_Finish = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Task_Finish];
            MainForm.agvdata.AGV_Arriv_RawStore = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.AGV_Arriv_RawStore];
            MainForm.agvdata.AGV_Arriv_FinStore = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.AGV_Arriv_FinStore];
            MainForm.agvdata.AGV_Arriv_LineStore = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.AGV_Arriv_LineStore];
            MainForm.agvdata.AGV_Task_State = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.AGV_Task_State];
            MainForm.agvdata.AGV_Vol = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.AGV_Vol];
            MainForm.agvdata.AGV_Beat = ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.AGV_Beat];
            //更新缓冲区 页面信息
            labelS1State.Text = MainForm.agvdata.RawStoreState == 0? "无托盘" : "有托盘";
            pictureBoxS1.BackColor = MainForm.agvdata.RawStoreState == 0? Color.White : Color.LightGreen;
            labelS2State.Text = MainForm.agvdata.LineStoreState == 0 ? "无托盘" : "有托盘";
            pictureBoxS2.BackColor = MainForm.agvdata.LineStoreState == 0 ? Color.White : Color.LightGreen;
            labelS3State.Text = MainForm.agvdata.FinStoreState == 0? "无托盘" : "有托盘";
            pictureBoxS3.BackColor = MainForm.agvdata.FinStoreState == 0 ? Color.White : Color.LightGreen;
            if (MainForm.agvdata.Task_Raw_Line == 1)
            {
                textBoxAGVTask.Text = "从毛坯库运输到线边库";
            }
            else if (MainForm.agvdata.Task_Fin_Line == 1)
            {
                textBoxAGVTask.Text = "从成品库运输到线边库";
            }
            else if (MainForm.agvdata.Task_Line_Raw == 1)
            {
                textBoxAGVTask.Text = "从线边库运输到毛坯库";
            }
            else if (MainForm.agvdata.Task_Line_Fin == 1)
            {
                textBoxAGVTask.Text = "从线边库运输到成品库";
            }
            else
            {
                textBoxAGVTask.Text = "空闲";
            }
            if (MainForm.agvdata.AGV_Task_State == 0)
            {
                textBoxAGVState.Text = "维护中";
            }
            if (MainForm.agvdata.AGV_Task_State == 1)
            {
                textBoxAGVState.Text = "就绪";
            }
            if (MainForm.agvdata.AGV_Task_State == 2)
            {
                textBoxAGVState.Text = "忙碌";
            }
            if (MainForm.agvdata.AGV_Task_State == 4)
            {
                textBoxAGVState.Text = "故障";
            }
            if (MainForm.agvdata.AGV_Task_State == 10)
            {
                textBoxAGVState.Text = "接口错误";
            }
            if (MainForm.agvdata.AGV_Task_State == 11)
            {
                textBoxAGVState.Text = "急停中";
            }
            textBoxAGVVol.Text = MainForm.agvdata.AGV_Vol.ToString();
            textBoxAGVBeat.Text = MainForm.agvdata.AGV_Beat.ToString();
            //更新料仓页面信息
            renewRack();
            buttonbin1.BackColor = RackBinState[0]== false? Color.White : Color.LightGreen;
            buttonbin2.BackColor = RackBinState[1] == false ? Color.White : Color.LightGreen;
            buttonbin3.BackColor = RackBinState[2] == false ? Color.White : Color.LightGreen;
            buttonbin4.BackColor = RackBinState[3] == false ? Color.White : Color.LightGreen;
            buttonbin5.BackColor = RackBinState[4] == false ? Color.White : Color.LightGreen;
            buttonbin6.BackColor = RackBinState[5] == false ? Color.White : Color.LightGreen;
            buttonbin7.BackColor = RackBinState[6] == false ? Color.White : Color.LightGreen;
            buttonbin8.BackColor = RackBinState[7] == false ? Color.White : Color.LightGreen;
            buttonbin9.BackColor = RackBinState[8] == false ? Color.White : Color.LightGreen;
            buttonbin10.BackColor = RackBinState[9] == false ? Color.White : Color.LightGreen;
            buttonbin11.BackColor = RackBinState[10] == false ? Color.White : Color.LightGreen;
            buttonbin12.BackColor = RackBinState[11] == false ? Color.White : Color.LightGreen;
            buttonbin13.BackColor = RackBinState[12] == false ? Color.White : Color.LightGreen;
            buttonbin14.BackColor = RackBinState[13] == false ? Color.White : Color.LightGreen;
            buttonbin15.BackColor = RackBinState[14] == false ? Color.White : Color.LightGreen;
            buttonbin16.BackColor = RackBinState[15] == false ? Color.White : Color.LightGreen;
            buttonbin17.BackColor = RackBinState[16] == false ? Color.White : Color.LightGreen;
            buttonbin18.BackColor = RackBinState[17] == false ? Color.White : Color.LightGreen;
            buttonbin19.BackColor = RackBinState[18] == false ? Color.White : Color.LightGreen;
            buttonbin20.BackColor = RackBinState[19] == false ? Color.White : Color.LightGreen;
            buttonbin21.BackColor = RackBinState[20] == false ? Color.White : Color.LightGreen;
            buttonbin22.BackColor = RackBinState[21] == false ? Color.White : Color.LightGreen;
            buttonbin23.BackColor = RackBinState[22] == false ? Color.White : Color.LightGreen;
            buttonbin24.BackColor = RackBinState[23] == false ? Color.White : Color.LightGreen;
            buttonbin25.BackColor = RackBinState[24] == false ? Color.White : Color.LightGreen;
            buttonbin26.BackColor = RackBinState[25] == false ? Color.White : Color.LightGreen;
            buttonbin27.BackColor = RackBinState[26] == false ? Color.White : Color.LightGreen;
            buttonbin28.BackColor = RackBinState[27] == false ? Color.White : Color.LightGreen;
            buttonbin29.BackColor = RackBinState[28] == false ? Color.White : Color.LightGreen;
            buttonbin30.BackColor = RackBinState[29] == false ? Color.White : Color.LightGreen;

            //检查AGV任务是否完成
            CheckAGVTaskEnd();

        }
       
        private void CheckAGVTaskEnd()
        {
            if(MainForm.agvdata.Task_Finish == 1)
            {
                //更新AGV任务
                if (MainForm.agvdata.Task_Raw_Line == 1 && MainForm.agvdata.Task_Finish==1)
                {

                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Raw_Line, 1, 1, 0);// 清除plcAGV任务信息
                }
                //更新AGV任务
                if (MainForm.agvdata.Task_Fin_Line == 1 && MainForm.agvdata.Task_Fin_Line == 1)
                {

                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Fin_Line, 1, 1, 0);// 清除plcAGV任务信息
                }
                //更新AGV任务
                if (MainForm.agvdata.Task_Line_Raw == 1 && MainForm.agvdata.Task_Line_Raw == 1)
                {

                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Line_Raw, 1, 1, 0);// 清除plcAGV任务信息
                }
                //更新AGV任务
                if (MainForm.agvdata.Task_Line_Fin == 1 && MainForm.agvdata.Task_Finish == 1)
                {

                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Line_Fin, 1, 1, 0);// 清除plcAGV任务信息
                }
                //毛坯-线边
                if(buttonS1TS2S.Text == "执行中")
                {
                    buttonS1TS2S.Text = "开始";
                    buttonS1TS2S.BackColor = Color.DarkTurquoise;
                    buttonS1TS2S.Enabled = true;
                    buttonS1TS2C.BackColor = Color.Gray;
                    buttonS1TS2C.Enabled = false;
                }
                //成品-线边
                if (buttonS3TS2S.Text == "执行中")
                {
                    buttonS3TS2S.Text = "开始";
                    buttonS3TS2S.BackColor = Color.DarkTurquoise;
                    buttonS3TS2S.Enabled = true;
                    buttonS3TS2C.BackColor = Color.Gray;
                    buttonS3TS2C.Enabled = false;
                } //线边-毛坯
                if (buttonS2TS1S.Text == "执行中")
                {
                    buttonS2TS1S.Text = "开始";
                    buttonS2TS1S.BackColor = Color.DarkTurquoise;
                    buttonS2TS1S.Enabled = true;
                    buttonS2TS1C.BackColor = Color.Gray;
                    buttonS2TS1C.Enabled = false;
                }//线边-成品
                if (buttonS2TS3S.Text == "执行中")
                {
                    buttonS2TS3S.Text = "开始";
                    buttonS2TS3S.BackColor = Color.DarkTurquoise;
                    buttonS2TS3S.Enabled = true;
                    buttonS2TS3C.BackColor = Color.Gray;
                    buttonS2TS3C.Enabled = false;
                }
            }
        }
        private bool[] RackBinState = new bool[30];//立体库仓位是否有料
        private void renewRack()
        {
            int magstatestart = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state;//零件类型          
            int maglength = (int)ModbusTcp.MagLength;
            int magiostateadress = (int)SCADA.ModbusTcp.DataConfigArr.Mag_state_1;
            bool maginstate = false;
            int magstatei = 0;
            int temp = 0;

            for (int i = 0; i < 30; i++)
            {
                magstatei = magstatestart + maglength * i;

                int magstartnum = magiostateadress;
                if (i <= 15)
                {
                    if (i < 8)
                    {
                        temp = 1 << (8 + i);
                        if ((ModbusTcp.DataMoubus[magstartnum] & temp) == temp)
                        {
                            maginstate = true;
                        }
                        else
                            maginstate = false;
                    }
                    else
                    {
                        temp = 1 << (i - 8);
                        if ((ModbusTcp.DataMoubus[magstartnum] & temp) == temp)
                        {
                            maginstate = true;
                        }
                        else
                            maginstate = false;
                    }
                }
                else
                {

                    if (i < 24)
                    {
                        temp = 1 << (i - 8);
                        if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                        {
                            maginstate = true;
                        }
                        else
                            maginstate = false;
                    }
                    else
                    {
                        temp = 1 << (i - 24);
                        if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                        {
                            maginstate = true;
                        }
                        else
                            maginstate = false;
                    }
                    if ((ModbusTcp.DataMoubus[magstartnum + 1] & temp) == temp)
                    {
                        maginstate = true;
                    }
                    else
                        maginstate = false;
                }
                RackBinState[i] = maginstate;
            }

        }

        private void buttonS1TS2S_Click(object sender, EventArgs e)
        {
            if(MainForm.agvdata.AGV_Task_State == 1)
            {
                if( MainForm.agvdata.RawStoreState == 1 && MainForm.agvdata.LineStoreState == 0)
                        {
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Raw_Line, 1, 1, 1);// 下发plcAGV任务信息
                    buttonS1TS2S.Text = "执行中";
                    buttonS1TS2S.Enabled = false;
                    buttonS1TS2S.BackColor = Color.Gray;
                    buttonS1TS2C.BackColor = Color.DarkTurquoise;
                    buttonS1TS2C.Enabled = true;
                }
                else
                {
                    MessageBox.Show("源位置没有托盘或者目标位置已经有托盘！");
                }
             
            }
            else
            {
                MessageBox.Show("AGV未准备就绪！");
            }
        }

        private void buttonS3TS2S_Click(object sender, EventArgs e)
        {
            if (MainForm.agvdata.AGV_Task_State == 1)
            {
                if (MainForm.agvdata.FinStoreState == 1 && MainForm.agvdata.LineStoreState == 0)
                {
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Fin_Line, 1, 1, 1);// 下发plcAGV任务信息
                    buttonS3TS2S.Text = "执行中";
                    buttonS3TS2S.BackColor = Color.Gray;
                    buttonS3TS2S.Enabled = false;
                    buttonS3TS2C.BackColor = Color.DarkTurquoise;
                    buttonS3TS2C.Enabled = true;
                }
                else
                {
                    MessageBox.Show("源位置没有托盘或者目标位置已经有托盘！");
                }

            }
            else
            {
                MessageBox.Show("AGV未准备就绪！");
            }
        }

        private void buttonS2TS1S_Click(object sender, EventArgs e)
        {
            if (MainForm.agvdata.AGV_Task_State == 1)
            {
                if (MainForm.agvdata.RawStoreState == 0&&MainForm.agvdata.LineStoreState ==1)
                {
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Line_Raw, 1, 1, 1);// 下发plcAGV任务信息
                    buttonS2TS1S.Text = "执行中";
                    buttonS2TS1S.BackColor = Color.Gray;
                    buttonS2TS1S.Enabled =false;
                    buttonS2TS1C.BackColor = Color.DarkTurquoise;
                    buttonS2TS1C.Enabled = true;
                }
                else
                {
                    MessageBox.Show("源位置没有托盘或者目标位置已经有托盘！");
                }

            }
            else
            {
                MessageBox.Show("AGV未准备就绪！");
            }
        }

        private void buttonS2TS3S_Click(object sender, EventArgs e)
        {
            if (MainForm.agvdata.AGV_Task_State == 1)
            {
                if (MainForm.agvdata.FinStoreState == 0 || MainForm.agvdata.LineStoreState == 1)
                {
                    MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Line_Fin, 1, 1, 1);// 下发plcAGV任务信息
                    buttonS2TS3S.Text = "执行中";
                    buttonS2TS3S.BackColor = Color.Gray;
                    buttonS2TS3S.Enabled =false;
                    buttonS2TS3C.BackColor = Color.DarkTurquoise;
                    buttonS2TS3C.Enabled = true;
                }
                else
                {
                    MessageBox.Show("源位置没有托盘或者目标位置已经有托盘！");
                }

            }
            else
            {
                MessageBox.Show("AGV未准备就绪！");
            }
        }

        private void buttonS1TS2C_Click(object sender, EventArgs e)
        {
            
                MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Raw_Line, 1, 1, 0);// 清除plcAGV任务信息
                buttonS1TS2S.Text = "开始";
                buttonS1TS2S.BackColor = Color.DarkTurquoise;
                buttonS1TS2S.Enabled = true;
                buttonS1TS2C.BackColor = Color.Gray;
                buttonS1TS2C.Enabled = false;
  
        }

        private void buttonS3TS2C_Click(object sender, EventArgs e)
        {
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Fin_Line, 1, 1, 0);// 清除plcAGV任务信息
            buttonS3TS2S.Text = "开始";
            buttonS3TS2S.BackColor = Color.DarkTurquoise;
            buttonS3TS2S.Enabled = true;
            buttonS3TS2C.BackColor = Color.Gray;
            buttonS3TS2C.Enabled = false;
        }

        private void buttonS2TS1C_Click(object sender, EventArgs e)
        {
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Line_Raw, 1, 1, 0);// 清除plcAGV任务信息
            buttonS2TS1S.Text = "开始";
            buttonS2TS1S.BackColor = Color.DarkTurquoise;
            buttonS2TS1S.Enabled = true;
            buttonS2TS1C.BackColor = Color.Gray;
            buttonS2TS1C.Enabled = false;
        }

        private void buttonS2TS3C_Click(object sender, EventArgs e)
        {
            MainForm.sptcp1.SendData((byte)ModbusTcp.Func_Code.writereg, (int)ModbusTcp.DataConfigArr.Task_Line_Fin, 1, 1, 0);// 清除plcAGV任务信息
            buttonS2TS3S.Text = "开始";
            buttonS2TS3S.BackColor = Color.DarkTurquoise;
            buttonS2TS3S.Enabled = true;
            buttonS2TS3C.BackColor = Color.Gray;
            buttonS2TS3C.Enabled = false;
        }

        private void buttonS2TS4SS_Click(object sender, EventArgs e)
        {
            int sourceBinNum = 0;
          //  int DesBinNum = 0;
            try
            {
                sourceBinNum = Convert.ToInt32(textBoxSource1.Text);
                if (sourceBinNum > 50 || sourceBinNum < 30)
                {
                    MessageBox.Show("线边库库位超出范围！");
                }
            }
            catch
            {
                MessageBox.Show("线边库库位数据错误");
                return;
            }
            //try
            //{
            //    DesBinNum = Convert.ToInt32(textBoxSource1.Text);
            //    if (DesBinNum > 50 || DesBinNum < 30)
            //    {
            //        MessageBox.Show("立体库库位超出范围！");
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("立体库库位数据错误！");
            //    return;
            //}
            if (MainForm.agvdata.LineStoreState == 0)
            {
                MessageBox.Show("线边库没有托盘！");
                return;
            }
            if (RackBinState[sourceBinNum -30- 1] == true)
            {
                MessageBox.Show("立体库目标位置已有料！");
                return;
            }

            if (MainForm.PLC_SIMES_ON_line == false)
            {

                MessageBox.Show("PLC未连接，请稍后下发搬运指令!");
                return;
            }

            if (ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_speed] == 1)
            {
                MessageBox.Show("机器人繁忙，请稍后下发搬运指令!");
                return;
            }
            if (ModbusTcp.DataMoubus[(int)SCADA.ModbusTcp.DataConfigArr.Mesans_Robot_position_comfirm] == 0)
            {

                MessageBox.Show("机器人不在HOME位置无法下发搬运指令！");
                return;
            }
            if (ModbusTcp.Rack_number_write_flage == true)
            {
                MessageBox.Show("机器人有正在执行的任务，请稍后下发搬运指令！");
                return;

            }
            if(Stackfun(true, sourceBinNum))
            {
                buttonS2TS4SS.BackColor = Color.Gray;
                buttonS2TS4SS.Enabled = false;
                buttonS2TS4SC.Enabled = true;
                buttonS2TS4SC.BackColor = Color.DarkTurquoise;
            }
        }

        private void buttonS4TS2SS_Click(object sender, EventArgs e)
        {

        }

        private void buttonS2TS4SC_Click(object sender, EventArgs e)
        {
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] ==3)//车床
            {

                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm] = 0;
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm] = 0;
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;

                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] = 0;
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response_2] = 0;//MES响应信号清零
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response_2] = 0;//MES响应信号清零
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response] = 0;
                buttonS2TS4SS.BackColor = Color.DarkTurquoise;
                buttonS2TS4SS.Enabled = true;
                buttonS2TS4SC.Enabled = false;
                buttonS2TS4SC.BackColor = Color.Gray;
            }
        }

        private void buttonS4TS2SC_Click(object sender, EventArgs e)
        {
            if (ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] == 3)//车床
            {

                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = 0;
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm] = 0;
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm] = 0;
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 0;

                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Mesans_PLC_response] = 0;
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Load_number_response_2] = 0;//MES响应信号清零
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rcak_Unload_number_response_2] = 0;//MES响应信号清零
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Machine_type_response] = 0;
                buttonS4TS2SS.BackColor = Color.DarkTurquoise;
                buttonS4TS2SS.Enabled = true;
                buttonS4TS2SC.Enabled = false;
                buttonS4TS2SC.BackColor = Color.Gray;
            }
        }
        /// <summary>
        /// 码垛任务
        /// </summary>
        /// <param name="WMSInFlag">是否为线边库搬运到立体库</param>
        /// <param name="sourceBin">物料源仓位号/param>
        /// <returns>返回true，命令下发成功，返回-1命令下发不成功</returns>

        private bool Stackfun(bool WMSInFlag, int sourceBin)
        {

            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.MES_PLC_comfirm] = (int)ModbusTcp.MesCommandToPlc.ComProcessManage;//加工命令码
          
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_UnLoad_comfirm] = sourceBin-30;//料仓编号
                ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Rack_number_Load_comfirm] = sourceBin;//料仓编号
          
          
            ModbusTcp.DataMoubus[(int)ModbusTcp.DataConfigArr.Order_type_comfirm] = 3;//加工类型，3标识线边库
            ModbusTcp.Rack_number_write_flage = true;//订单下发标识

            // magprocesss1tate[magno - 1] = (int)Processstate.Loading;
            return true;
        }

    }


    /// <summary>
    /// AGV数据
    /// </summary>
    public class AGVData
        {
            public Int32 LineStoreState;//	线边库状态(0无料盘1有料盘）
            public Int32 FinStoreState;//	成品库状态(0无料盘1有料盘）
            public Int32 RawStoreState;//	毛坯库状态(0无料盘1有料盘)
            public Int32 Task_Raw_Line;//	毛坯库到线边库
            public Int32 Task_Fin_Line;//	成品库到线边库
            public Int32 Task_Line_Raw;//	从线边库到毛坯库
            public Int32 Task_Line_Fin;//	从线边库到成品库
            public Int32 Task_Finish;//	AGV任务完成

            public Int32 AGV_Arriv_RawStore;//AGV 到达毛坯库，1到达，0离开
            public Int32 AGV_Arriv_FinStore;//AGV 到达成品库，1到达，0离开
            public Int32 AGV_Arriv_LineStore;//AGV 到达线边库，1到达，0离开
            public Int32 AGV_Task_State;//0:初始，4:任务创建成功，6:任务创建失败，16:成功完成，24:错误完成
            public Int32 AGV_State;//AGV状态0：维护中、充电中  1：就绪 2：忙碌 4：故障 10：API接口错误  11：急停中
            public Int32 AGV_Vol;//电量，百分比
            public Int32 AGV_Beat;//心跳每秒加1 到1000后回1
        }
    }
