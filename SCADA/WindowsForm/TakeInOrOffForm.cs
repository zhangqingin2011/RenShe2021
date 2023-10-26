﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace SCADA
{
    public partial class TakeInOrOffForm : Form
    {
        public TakeInOrOffForm()
        {
            InitializeComponent();
            InitConfig();
        }

        XmlWR xmlwr = new XmlWR();
        string[] STR_Columns = { "任务文件路径", "加载时间" };
        DataTable DGVTakeInDb = new DataTable();
        DataTable DGVTakeOffDb = new DataTable();

        void InitConfig()
        {
            xmlwr.ReadXml(xmlwr.Path);
            InitUI();
        }

        void InitUI()
        {
            textBox1.Text = xmlwr.configDictionary[XmlWR.KEYS.服务器IP.ToString()];
            textBox2.Text = xmlwr.configDictionary[XmlWR.KEYS.下载路径.ToString()];
            textBox3.Text = xmlwr.configDictionary[XmlWR.KEYS.上传路径.ToString()];
            textBox4.Text = xmlwr.configDictionary[XmlWR.KEYS.本地接收路径.ToString()];
            SetDataGridAttribute(DGVTakeIn);
            AddDataGridColumns(STR_Columns, DGVTakeIn, DGVTakeInDb);
            SetDataGridAttribute(DGVTakeOff);
            AddDataGridColumns(STR_Columns, DGVTakeOff, DGVTakeOffDb);
        }

        private void Buttonsave_Click(object sender, EventArgs e)
        {
            xmlwr.WriteNodeValue((int)XmlWR.KEYS.服务器IP, textBox1.Text);
            xmlwr.WriteNodeValue((int)XmlWR.KEYS.下载路径, textBox2.Text);
            xmlwr.WriteNodeValue((int)XmlWR.KEYS.上传路径, textBox3.Text);
        }

        private void Buttonfresh_Click(object sender, EventArgs e)
        {
            xmlwr.ReadNodeValue((int)XmlWR.KEYS.服务器IP);
            xmlwr.ReadNodeValue((int)XmlWR.KEYS.下载路径);
            xmlwr.ReadNodeValue((int)XmlWR.KEYS.上传路径);
            textBox1.Text = xmlwr.configDictionary[XmlWR.KEYS.服务器IP.ToString()];
            textBox2.Text = xmlwr.configDictionary[XmlWR.KEYS.下载路径.ToString()];
            textBox3.Text = xmlwr.configDictionary[XmlWR.KEYS.上传路径.ToString()];
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "选择任务存放路径";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(folderBrowser.SelectedPath))
                {
                    MessageBox.Show("选择的任务路径为空！");
                    return;
                }
                else
                {
                    //Console.WriteLine(folderBrowser.SelectedPath);
                    xmlwr.WriteNodeValue((int)XmlWR.KEYS.本地接收路径, folderBrowser.SelectedPath);
                    xmlwr.ReadNodeValue((int)XmlWR.KEYS.本地接收路径);
                    textBox4.Text = xmlwr.configDictionary[XmlWR.KEYS.本地接收路径.ToString()];
                    PushMessage(richTextBox1, string.Format("本地接收路径变更为: {0}", folderBrowser.SelectedPath), Color.Black);
                }
            }
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            RichTextBox rich = sender as RichTextBox;
            rich.SelectionStart = rich.Text.Length;
            rich.ScrollToCaret();
        }

        private void RichTextBox2_TextChanged(object sender, EventArgs e)
        {
            RichTextBox rich = sender as RichTextBox;
            rich.SelectionStart = rich.Text.Length;
            rich.ScrollToCaret();
        }

        private void PushMessage(RichTextBox richText , string Msg, Color color)
        {
            string Message = Msg + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
            richText.SelectionColor = color;
            richText.AppendText(Message);
        }

        private void SetDataGridAttribute(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void AddDataGridColumns(string[] STR_Colums, DataGridView dgv, DataTable table)
        {
            for (int ii = 0; ii < STR_Colums.Length; ii++)
            {
                table.Columns.Add(STR_Colums[ii]);
            }
            dgv.DataSource = table;
            DataGridViewCheckBoxColumn dtCheck = new DataGridViewCheckBoxColumn();
            dtCheck.HeaderText = "选择";
            dtCheck.ReadOnly = false;
            dtCheck.Selected = false;
            dgv.Columns.Insert(0, dtCheck);
        }

        private void DGVTakeIn_SizeChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                int minwith = dgv.Width / dgv.Columns.Count;
                if (i == 0)
                {
                    dgv.Columns[i].Width = minwith / 3;
                }
                else if (i == 1)
                {
                    dgv.Columns[i].Width = minwith / 4 + minwith * 5 / 3;
                }
                else if (i == 2)
                {
                    dgv.Columns[i].Width = minwith * 3 / 4;
                }
            }
        }

        private void DGVTakeOff_SizeChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                int minwith = dgv.Width / dgv.Columns.Count;
                if (i == 0)
                {
                    dgv.Columns[i].Width = minwith / 3;
                }
                else if (i == 1)
                {
                    dgv.Columns[i].Width = minwith * 5 / 3;
                }
                else
                {
                    dgv.Columns[i].Width = minwith;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string initfilepath = "//"+ xmlwr.configDictionary[XmlWR.KEYS.服务器IP.ToString()]+"//"+ xmlwr.configDictionary[XmlWR.KEYS.下载路径.ToString()]+"//";
                OpenFileDialog fileDialog = new OpenFileDialog();
               fileDialog.InitialDirectory = initfilepath;
                fileDialog.RestoreDirectory = true; //记忆上次浏览路径
                fileDialog.Multiselect = true;
                fileDialog.Title = "选择接收任务文件";

                DataTable DGVTakeInDb1 = new DataTable();
                for (int ii = 0; ii < STR_Columns.Length; ii++)
                {
                    DGVTakeInDb1.Columns.Add(STR_Columns[ii]);
                }
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    // DGVTakeIn.DataSource = null;
                    //DGVTakeInDb = DGVTakeIn.DataSource;
                    //DGVTakeInDb.Clear();
                    for (int i=0;i< DGVTakeIn.Rows.Count; i++)
                    {
                        string[] array = new string[STR_Columns.Length];
                        array[0] = DGVTakeIn.Rows[i].Cells[1].Value.ToString();
                        array[1] = DGVTakeIn.Rows[i].Cells[2].Value.ToString();
                        DGVTakeInDb1.Rows.Add(array); 
                    }
                    foreach (string filename in fileDialog.FileNames)
                    {
                        
                        if (IsGridwiewHasFilePath(DGVTakeInDb1, filename))
                        {
                            MessageBox.Show(ChangeLanguage.GetString("MessageFilePathExist") + filename);
                        }
                        else
                        {
                            string[] array = new string[STR_Columns.Length];
                            array[0] = filename;
                            array[1] = DateTime.Now.ToString();
                            DGVTakeInDb1.Rows.Add(array);
                        }
                    }
                    DGVTakeIn.DataSource = null;
                    DGVTakeIn.DataSource = DGVTakeInDb1;
                    DGVTakeIn_SizeChanged(DGVTakeIn, null);
                    DGVTakeIn.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //System.Diagnostics.Process.Start("explorer.exe", "C:\\");
        }

        private bool IsGridwiewHasFilePath(DataTable table ,string filename)
        {
            bool result = false;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (filename == table.Rows[i][0].ToString())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DeleteDataGridView(STR_Columns, DGVTakeInDb, DGVTakeIn);
        }

        void DeleteDataGridView(string[] STR_Columns, DataTable dataTable, DataGridView gridView)
        {
            DataTable Db = new DataTable();
            for (int ii = 0; ii < STR_Columns.Length; ii++)
            {
                Db.Columns.Add(STR_Columns[ii]);
            }
            //dataTable.Clear();
            for (int ii = 0; ii < gridView.Rows.Count; ii++)
            {
                
               // dataTable.Rows.Add(array);
                if (!(bool)gridView.Rows[ii].Cells[0].EditedFormattedValue)
                {
                    string[] array = new string[STR_Columns.Length];
                    array[0] = gridView.Rows[ii].Cells[1].Value.ToString();
                    array[1] = gridView.Rows[ii].Cells[2].Value.ToString();
                    Db.Rows.Add(array);
                }
            }
            //dataTable = Db;
            gridView.DataSource = null;
            gridView.DataSource = Db;
            if(gridView == DGVTakeIn)
                DGVTakeIn_SizeChanged(gridView, null);
            else
                DGVTakeOff_SizeChanged(gridView, null);
            gridView.ClearSelection();
        }

        bool SendFile(string sourefile, string destifolder)
        {
            bool res = false;
            if (string.IsNullOrEmpty(sourefile) || string.IsNullOrEmpty(destifolder))
                return res;

            byte[] content = LoadFile(sourefile);
            string[] strs = sourefile.Split('\\');
            string sourename = strs[strs.Length - 1];
            string destifile = destifolder + "\\" + sourename;
            res = WriteFile(content, destifile);
            return res;
        }
     
        private byte[] LoadFile(string path)
        {           
            byte[] content = null;
            try
            {
                content = File.ReadAllBytes(path);
            }
            catch (Exception)
            {
            }
            return content;
        }

        private bool WriteFile(byte[] content, string path)
        {
            bool res = false;
            if (content == null)
                return res;

            try
            {
                File.WriteAllBytes(path, content);
                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            string str1 = "", str2 = "";
            //DataTable DGVTakeInDb1 = new DataTable();
            //DGVTakeInDb1.Clear();
            //for (int i = 0; i < DGVTakeIn.Rows.Count; i++)
            //{
            //    string[] array = new string[STR_Columns.Length];
            //    array[0] = DGVTakeIn.Rows[i].Cells[0].ToString();
            //    array[1] = DGVTakeIn.Rows[i].Cells[1].ToString();
            //    DGVTakeInDb1.Rows.Add(array);
            //}
            for (int jj = 0; jj < DGVTakeIn.Rows.Count; jj++)
            {
                if ((bool)DGVTakeIn.Rows[jj].Cells[0].EditedFormattedValue)
                {
                    string[] filename = DGVTakeIn.Rows[jj].Cells[1].Value.ToString().Split('\\');
                    var t = DGVTakeIn.Rows[jj].Cells[1].Value.ToString();
                    if (SendFile(DGVTakeIn.Rows[jj].Cells[1].Value.ToString(), xmlwr.configDictionary[XmlWR.KEYS.本地接收路径.ToString()]))
                    {
                        str1 += filename[filename.Length - 1] + "；";
                    }
                    else
                    {
                        str2 += filename[filename.Length - 1] + "；";
                    }
                }
            }
            if (str1.Length > 0)
            {
                //textBox1.Text = string.Format("上传成功: {0}", str1);
                //textBox2.Text = DateTime.Now.ToString();
                PushMessage(richTextBox1, string.Format("下载成功: {0}", str1), Color.Black);
            }
            if (str2.Length > 0)
            {
                //textBox1.Text = string.Format("上传失败: {0}", str2);
                //textBox2.Text = DateTime.Now.ToString();
                PushMessage(richTextBox1, string.Format("下载失败: {0}", str2), Color.Red);
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.RestoreDirectory = true; //记忆上次浏览路径
                fileDialog.Multiselect = true;
                fileDialog.Title = "选择提交任务文件";
                DataTable DGVTakeOffDb1 = new DataTable();
                for (int ii = 0; ii < STR_Columns.Length; ii++)
                {
                    DGVTakeOffDb1.Columns.Add(STR_Columns[ii]);
                }
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    //DGVTakeInDb.Clear();
                    for (int i = 0; i < DGVTakeOff.Rows.Count; i++)
                    {
                        string[] array = new string[STR_Columns.Length];
                        array[0] = DGVTakeOff.Rows[i].Cells[1].Value.ToString();
                        array[1] = DGVTakeOff.Rows[i].Cells[2].Value.ToString();
                        DGVTakeOffDb1.Rows.Add(array);
                    }
                    //DGVTakeOff.DataSource = null;
                    foreach (string filename in fileDialog.FileNames)
                    {
                        if (IsGridwiewHasFilePath(DGVTakeOffDb1, filename))
                        {
                            MessageBox.Show(ChangeLanguage.GetString("MessageFilePathExist") + filename);
                        }
                        else
                        {
                            string[] array = new string[STR_Columns.Length];
                            array[0] = filename;
                            array[1] = DateTime.Now.ToString();
                            DGVTakeOffDb1.Rows.Add(array);
                        }
                    }
                    DGVTakeOff.DataSource = null;
                    DGVTakeOff.DataSource = DGVTakeOffDb1;
                    DGVTakeOff_SizeChanged(DGVTakeOff, null);
                    DGVTakeOff.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            DeleteDataGridView(STR_Columns, DGVTakeOffDb, DGVTakeOff);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            string str1 = "", str2 = "";
            //DGVTakeOffDb.Clear();
            //for (int i = 0; i < DGVTakeOff.RowCount; i++)
            //{
            //    string[] array = new string[STR_Columns.Length];
            //    array[0] = DGVTakeOff.Rows[i].Cells[0].ToString();
            //    array[1] = DGVTakeOff.Rows[i].Cells[1].ToString();
            //    DGVTakeOffDb.Rows.Add(array);
            //}
            for (int jj = 0; jj < DGVTakeOff.Rows.Count; jj++)
            {
                if ((bool)DGVTakeOff.Rows[jj].Cells[0].EditedFormattedValue)
                {
                    string[] filename = DGVTakeOff.Rows[jj].Cells[1].Value.ToString().Split('\\');
                    //string s1 = DGVTakeOffDb.Rows[jj][0].ToString() + "\\";
                    //string s2 = "\\" +xmlwr.configDictionary[XmlWR.KEYS.服务器IP.ToString()] + "\\" + xmlwr.configDictionary[XmlWR.KEYS.上传路径.ToString()] + "\\";
                    //SendFile(s1,"//192.168.8.98/共享文件夹/");
                    if (SendFile(DGVTakeOff.Rows[jj].Cells[1].Value.ToString(), "\\\\" + xmlwr.configDictionary[XmlWR.KEYS.服务器IP.ToString()] + "\\" +xmlwr.configDictionary[XmlWR.KEYS.上传路径.ToString()]))
                    {
                        str1 += filename[filename.Length - 1] + "；";
                    }
                    else
                    {
                        str2 += filename[filename.Length - 1] + "；";
                    }
                }
            }
            if (str1.Length > 0)
            {
                PushMessage(richTextBox2, string.Format("上传成功: {0}", str1), Color.Black);
            }
            if (str2.Length > 0)
            {
                PushMessage(richTextBox2, string.Format("上传失败: {0}", str2), Color.Red);
            }
        }
    }
}
