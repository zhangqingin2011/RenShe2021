using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using ScadaHncData;
using System.IO;
using System.Collections;

namespace SCADA
{

    public partial class UserControlTaskData : UserControl
    {
        //public ShareData TaskDataObj; //2015.10.24
        const string taskListPathXml = ".\\TaskList"; //2015.10.25
        const string cncTaskListXml = "\\CNCTaskList.xml";
        const string oldcncTaskListXml = "\\oldCNCTaskList.xml";

        const int resendNum = 3; //文件发送失败后的重发次数
        const int maxFileSize = 2; //发送单个文件大小最大默认为2M

       string[] STR_dataGridView_CNCGCode_Columns = { "CNC机台号","下载情况", "备注" };
        string[] STR_dataGridView_GCodeSele_Columns = { "G代码路径", "备注"};
        string[] MessageboxStr_登录 = {"你的操作权限不够，请先登录","提示" };
        private Color sendGcodeBackColor = Color.Chocolate;
        private Color frezenBackColor = Color.BurlyWood;
        //System.Data.DataTable TaskDb;
        System.Data.DataTable dataGridView_CNCGCodeDb = new DataTable();
        System.Data.DataTable dataGridView_GCodeSeleDb = new DataTable();


        public UserControlTaskData()
        {
            InitializeComponent();
            dataGridView_CNCGCode.AllowUserToAddRows = false;
            dataGridView_CNCGCode.ReadOnly = false;
            dataGridView_CNCGCode.RowHeadersVisible = false;

            dataGridView_GCodeSele.AllowUserToAddRows = false;
            dataGridView_GCodeSele.ReadOnly = false;
            dataGridView_GCodeSele.RowHeadersVisible = false;

            DataGridViewCheckBoxColumn dtCheck = new DataGridViewCheckBoxColumn();
            dtCheck.HeaderText = "选择";
            dtCheck.ReadOnly = false;
            dtCheck.Selected = false;

            dtCheck = new DataGridViewCheckBoxColumn();
            dtCheck.HeaderText = "选择";
            dtCheck.ReadOnly = false;
            dtCheck.Selected = false;
            dataGridView_CNCGCode.Columns.Insert(0, dtCheck);
            dataGridView_CNCGCode.Columns[0].Frozen = true;
            this.dataGridView_CNCGCode.DefaultCellStyle.WrapMode = DataGridViewTriState.True;//设置自动换行
            for (int ii = 0; ii < STR_dataGridView_CNCGCode_Columns.Length; ii++)
            {
                dataGridView_CNCGCodeDb.Columns.Add(STR_dataGridView_CNCGCode_Columns[ii]);
            }
            dataGridView_CNCGCode.DataSource = dataGridView_CNCGCodeDb;

            dtCheck = new DataGridViewCheckBoxColumn();
            dtCheck.HeaderText = "选择";
            dtCheck.ReadOnly = false;
            dtCheck.Selected = false;
            dataGridView_GCodeSele.Columns.Insert(0, dtCheck);
            dataGridView_GCodeSele.Columns[0].Frozen = true;
            this.dataGridView_GCodeSele.DefaultCellStyle.WrapMode = DataGridViewTriState.True;//设置自动换行
            this.dataGridView_GCodeSele.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //设置自动调整高度
            for (int ii = 0; ii < STR_dataGridView_GCodeSele_Columns.Length; ii++)
            {
                dataGridView_GCodeSeleDb.Columns.Add(STR_dataGridView_GCodeSele_Columns[ii]);
            }
            dataGridView_GCodeSele.DataSource = dataGridView_GCodeSeleDb;
            ExChangeLanguage();
        }

        public void ExChangeLanguage()
        {
            button_AddGCode.Text = ChangeLanguage.GetString("button_AddGCode");
            button_DeletGCode.Text = ChangeLanguage.GetString("button_DeletGCode");
            button_DowLoadGCode.Text = ChangeLanguage.GetString("button_DowLoadGCode");
           // buttondingdanquxiao.Text = ChangeLanguage.GetString("buttondingdanquxiao");
            dataGridView_CNCGCode.Columns[0].HeaderText = ChangeLanguage.GetString("CNCGcodeColumn01");
            dataGridView_CNCGCode.Columns[1].HeaderText = ChangeLanguage.GetString("CNCGcodeColumn02");
            dataGridView_CNCGCode.Columns[2].HeaderText = ChangeLanguage.GetString("CNCGcodeColumn03");
            dataGridView_CNCGCode.Columns[3].HeaderText = ChangeLanguage.GetString("CNCGcodeColumn04");
            dataGridView_GCodeSele.Columns[0].HeaderText = ChangeLanguage.GetString("GcodeSeleColumn01");
            dataGridView_GCodeSele.Columns[1].HeaderText = ChangeLanguage.GetString("GcodeSeleColumn02");
            dataGridView_GCodeSele.Columns[2].HeaderText = ChangeLanguage.GetString("GcodeSeleColumn03");

        }

 

        private void UserControlTaskData_Loadv2(object sender, EventArgs e)
        {
            dataGridView_CNCGCode.Columns[0].Width = this.dataGridView_CNCGCode.Width / 5;
            dataGridView_CNCGCode.Columns[1].Width = this.dataGridView_CNCGCode.Width / 5;
            dataGridView_CNCGCode.Columns[2].Width = this.dataGridView_CNCGCode.Width * 2 / 5;
            dataGridView_CNCGCode.Columns[3].Width = this.dataGridView_CNCGCode.Width - this.dataGridView_CNCGCode.Width * 4 / 5;

            dataGridView_GCodeSele.Columns[0].Width = this.dataGridView_GCodeSele.Width / 5;
            dataGridView_GCodeSele.Columns[1].Width = this.dataGridView_GCodeSele.Width * 2 / 5;
            dataGridView_GCodeSele.Columns[2].Width = this.dataGridView_GCodeSele.Width - this.dataGridView_GCodeSele.Width * 3 / 5;

            if (MainForm.cncv2list != null && MainForm.cncv2list.Count > 0)
            {
                for (int ii = 0; ii < MainForm.cncv2list.Count; ii++)
                {
                    string[] array = new string[STR_dataGridView_CNCGCode_Columns.Length];
                    array[0] = MainForm.cncv2list[ii].MachineSN;
                    array[1] = "";
                    array[2] = "";
                    dataGridView_CNCGCodeDb.Rows.Add(array);

                }
            }
        }

        private void button_DeletGCode_Click(object sender, EventArgs e)
         {
             System.Data.DataTable Db = new DataTable();
             STR_dataGridView_GCodeSele_Columns[0] = ChangeLanguage.GetString("GcodeSeleColumn02");
             STR_dataGridView_GCodeSele_Columns[1] = ChangeLanguage.GetString("GcodeSeleColumn03");
             for (int ii = 0; ii < STR_dataGridView_GCodeSele_Columns.Length; ii++)
             {
                 Db.Columns.Add(STR_dataGridView_GCodeSele_Columns[ii]);
             }
             for (int ii = 0; ii < dataGridView_GCodeSeleDb.Rows.Count; ii++)
             {
                 if (!(bool)dataGridView_GCodeSele.Rows[ii].Cells[0].EditedFormattedValue)
                 {
                     string[] array = new string[STR_dataGridView_GCodeSele_Columns.Length];
                     array[0] = dataGridView_GCodeSeleDb.Rows[ii][0].ToString();
                     array[1] = dataGridView_GCodeSeleDb.Rows[ii][1].ToString();
                     Db.Rows.Add(array);
                 }
             }
             dataGridView_GCodeSeleDb = Db;
             dataGridView_GCodeSele.DataSource = null;
             dataGridView_GCodeSele.DataSource = dataGridView_GCodeSeleDb;
         }
         private void button_AddGCode_Click(object sender, EventArgs e)
         {
             try
             {
                 OpenFileDialog fileDialog = new OpenFileDialog();
                 fileDialog.RestoreDirectory = true; //记忆上次浏览路径
                 fileDialog.Multiselect = true;
                 fileDialog.Title = ChangeLanguage.GetString("SelectFileTitle");
                 fileDialog.Filter = ChangeLanguage.GetString("SelectFileFilter");

                 if (fileDialog.ShowDialog() == DialogResult.OK)
                 {
                     dataGridView_GCodeSele.DataSource = null;
                     foreach (string filename in fileDialog.FileNames)
                     {
                         if (IsGridwiewHasFilePath(filename))
                         {
                             MessageBox.Show(ChangeLanguage.GetString("MessageFilePathExist") + filename);
                         }
                         else
                         {
                             string[] array = new string[STR_dataGridView_GCodeSele_Columns.Length];
                             array[0] = filename;
                             array[1] = "";
                             dataGridView_GCodeSeleDb.Rows.Add(array);
                         }
                     }
                     dataGridView_GCodeSele.DataSource = null;
                     dataGridView_GCodeSele.DataSource = dataGridView_GCodeSeleDb;
                 }
             }
             catch
             {

             }
         }

         private bool IsGridwiewHasFilePath(string filename)
         {
             bool result = false;
             for (int i = 0; i < dataGridView_GCodeSeleDb.Rows.Count; i++)
             {
                 if (filename == dataGridView_GCodeSeleDb.Rows[i][0].ToString())
                 {
                     result = true;
                     break;
                 }
             }
             return result;
         }

         private void button_jitaiquanxuan_Click(object sender, EventArgs e)
         {
             for (int ii = 0; ii < dataGridView_CNCGCode.Rows.Count; ii++)
             {
                 dataGridView_CNCGCode.Rows[ii].Cells[0].Value = true;
             }
         }
         private void button_jitaiquanbuxuan_Click(object sender, EventArgs e)
         {
             for (int ii = 0; ii < dataGridView_CNCGCode.Rows.Count; ii++)
             {
                 dataGridView_CNCGCode.Rows[ii].Cells[0].Value = false;
             }
         }
         private void button_GCodequanbuxuan_Click(object sender, EventArgs e)
         {
             for (int ii = 0; ii < dataGridView_GCodeSele.Rows.Count; ii++)
             {
                 dataGridView_GCodeSele.Rows[ii].Cells[0].Value = false;
             }
         }
         private void button_GCodequanxuan_Click(object sender, EventArgs e)
         {
             for (int ii = 0; ii < dataGridView_GCodeSele.Rows.Count; ii++)
             {
                 dataGridView_GCodeSele.Rows[ii].Cells[0].Value = true;
             }
         }

         //private void button_DowLoadGCode_Click(object sender, EventArgs e)
         //{
         //    if (SetForm.LogIn)
         //    {
         //        string str3 = "";
         //        for (int ii = 0; ii < dataGridView_CNCGCode.Rows.Count;ii++ )
         //        {
         //            if ((bool)dataGridView_CNCGCode.Rows[ii].Cells[0].EditedFormattedValue)
         //            {
         //                string str1 = "",str2 = "";
         //                for (int jj = 0; jj < dataGridView_GCodeSele.Rows.Count; jj++)
         //                {
         //                    if ((bool)dataGridView_GCodeSele.Rows[jj].Cells[0].EditedFormattedValue)
         //                    {
         //                        string[] filename = dataGridView_GCodeSeleDb.Rows[jj][0].ToString().Split('\\');
         //                        if (MainForm.cnclist[ii].sendFile(dataGridView_GCodeSeleDb.Rows[jj][0].ToString(), "h/lnc8/prog/" + filename[filename.Length - 1], 0, false) == 0)
         //                        {
         //                            str1 += filename[filename.Length - 1] + "；";
         //                        }
         //                        else
         //                        {
         //                            str2 += filename[filename.Length - 1] + "；";
         //                        }
         //                    }
         //                }
         //                if (str1.Length > 0)
         //                {
         //                    str1 = ChangeLanguage.GetString("LoadSuccess01") + str1;
         //                }
         //                if (str2.Length > 0)
         //                {
         //                    str2 = ChangeLanguage.GetString("LoadFailed01") + str2;
         //                }
         //                dataGridView_CNCGCodeDb.Rows[ii][1] = str1 + "\r\n" + str2;
         //                if(str2.Length > 0)
         //                {
         //                    dataGridView_CNCGCodeDb.Rows[ii][2] = DateTime.Now.ToString() + ":" + ChangeLanguage.GetString("LoadFailed02");
         //                    str3 += dataGridView_CNCGCodeDb.Rows[ii][0] + ":" + str2;
         //                }
         //                else
         //                {
         //                    dataGridView_CNCGCodeDb.Rows[ii][2] = DateTime.Now.ToString() + ":" + ChangeLanguage.GetString("LoadSuccess02");
         //                }
         //            }
         //        }
         //        if (str3.Length > 0)
         //        {
         //            MessageBox.Show(str3, MessageboxStr_登录[1], MessageBoxButtons.OK, MessageBoxIcon.Warning);
         //        }
         //    }
         //    else
         //    {
         //        MessageBox.Show(MessageboxStr_登录[0], MessageboxStr_登录[1], MessageBoxButtons.OK, MessageBoxIcon.Warning);
         //    }
         //}

        private void button_DowLoadGCode_Clickv2(object sender, EventArgs e)
        {
            if (SetForm.LogIn)
            {
                string str3 = "";
                for (int ii = 0; ii < dataGridView_CNCGCode.Rows.Count; ii++)
                {
                    if ((bool)dataGridView_CNCGCode.Rows[ii].Cells[0].EditedFormattedValue)
                    {
                        string str1 = "", str2 = "";
                        for (int jj = 0; jj < dataGridView_GCodeSele.Rows.Count; jj++)
                        {
                            if ((bool)dataGridView_GCodeSele.Rows[jj].Cells[0].EditedFormattedValue)
                            {

                                string[] filename = dataGridView_GCodeSeleDb.Rows[jj][0].ToString().Split('\\');

                                MainForm.cncv2list[ii].SetProgPath = dataGridView_GCodeSeleDb.Rows[jj][0].ToString();
                               
                                MainForm.cncv2list[ii].SetProgPath = dataGridView_GCodeSeleDb.Rows[jj][0].ToString();

                                if (MainForm.cncv2list[ii] != null)
                                {
                                    if (MainForm.collectdatav2.GetCNCDataLst[ii].SetProgFile())
                                    {
                                        str1 += filename[filename.Length - 1] + "；";
                                    }
                                    else
                                    {
                                        str2 += filename[filename.Length - 1] + "；";

                                    }
                                }
                                else
                                {
                                    str2 += filename[filename.Length - 1] + "；";
                                }

                            }
                        }
                        if (str1.Length > 0)
                        {
                            str1 = ChangeLanguage.GetString("LoadSuccess01") + str1;
                        }
                        if (str2.Length > 0)
                        {
                            str2 = ChangeLanguage.GetString("LoadFailed01") + str2;
                        }
                        dataGridView_CNCGCodeDb.Rows[ii][1] = str1 + "\r\n" + str2;
                        if (str2.Length > 0)
                        {
                            dataGridView_CNCGCodeDb.Rows[ii][2] = DateTime.Now.ToString() + ":" + ChangeLanguage.GetString("LoadFailed02");
                            str3 += dataGridView_CNCGCodeDb.Rows[ii][0] + ":" + str2;
                        }
                        else
                        {
                            dataGridView_CNCGCodeDb.Rows[ii][2] = DateTime.Now.ToString() + ":" + ChangeLanguage.GetString("LoadSuccess02");
                        }
                    }
                }
                if (str3.Length > 0)
                {
                    MessageBox.Show(str3, MessageboxStr_登录[1], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(MessageboxStr_登录[0], MessageboxStr_登录[1], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    } 
}


