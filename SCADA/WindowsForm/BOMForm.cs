
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HNCAPI;
using HNC_MacDataService;
using ScadaHncData;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Collections;
using System.Net.NetworkInformation;
using HNC.API;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace SCADA
{
    public partial class BOMForm : Form
    {
        AutoSizeFormClass aotosize = new AutoSizeFormClass();
        public string language = "";
        public static String BOMFilePath = "C:\\Users\\Public\\Documents\\清单目录";
        private static Form[] m_Formarre1 = null;
        private SqlCommand myCommand =null;
        private SqlDataReader myReader;
        private SqlConnection myConnection=null;
        private static Process PDMPprocess;
        private static Process CAPPPprocess;
        private string CardFilePath = "C:\\Users\\Public\\Documents\\工艺卡目录 ";//"C:\\Users\\Public\\Documents\\工艺卡目录";
        private List<string> Cardlist = new List<string>();

        AutoSizeFormClass autosizeebom = new AutoSizeFormClass();
        AutoSizeFormClass autosizepbom = new AutoSizeFormClass();

        AutoSizeFormClass autosizecard = new AutoSizeFormClass();
        public BOMForm()
        {
            InitializeComponent();
            m_Formarre1 = new Form[tabControl1.TabCount];
            myConnection = new SqlConnection();
            string conlink = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
            myConnection.ConnectionString = conlink;
            myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            treeView1.Nodes.Clear();

            //tabControl1.TabPages.Remove(tabPageBOM);
            //tabControl1.TabPages.Remove(tabPageCARD);
            //tabControl1.TabPages.Remove(BOMIN);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //(tabControl1.SelectedIndex, sender);
            
           
        }


        

        private void BOMForm_SizeChanged(object sender, EventArgs e)
        {
           // aotosize.controlAutoSize(this);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Image backImage = Properties.Resources.whiteBack;
            Rectangle rec = tabControl1.ClientRectangle;

            StringFormat StrFormat = new StringFormat();

            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;

            SolidBrush tabBackBrush = new SolidBrush(Color.DarkGray);
            //文字色
            SolidBrush FrontBrush = new SolidBrush(Color.White);
            //StringFormat stringF = new StringFormat();
            Font wordfont = new Font("微软雅黑", 12F);
            e.Graphics.DrawImage(backImage, 0, 0, tabControl1.Width, tabControl1.Height);
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                //标签工作区
                Rectangle rec1 = tabControl1.GetTabRect(i);
                //e.Graphics.DrawImage(backImage, 0, 0, tabPagemain.Width, tabPagemain.Height);
                e.Graphics.FillRectangle(tabBackBrush, rec1);
                ////标签头背景色
                e.Graphics.DrawString(tabControl1.TabPages[i].Text, wordfont, FrontBrush, rec1, StrFormat);
                ////标签头文字
            }
        }

        //private void textBox0416_TextChanged(object sender, EventArgs e)
        //{

        //}

        //private void groupBox2_Enter(object sender, EventArgs e)
        //{

        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (PDMPprocess == null)
        //    {
        //        PDMPprocess = new Process();
        //        PDMPprocess.StartInfo.WorkingDirectory = "C:\\KMSOFT\\KMPDM";
        //        PDMPprocess.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
        //        PDMPprocess.StartInfo.UseShellExecute = false;
        //        PDMPprocess.StartInfo.RedirectStandardInput = true;
        //        PDMPprocess.StartInfo.RedirectStandardOutput = true;
        //        PDMPprocess.StartInfo.RedirectStandardError = true;
        //        PDMPprocess.StartInfo.CreateNoWindow = false;
        //        PDMPprocess.Start();
        //        PDMPprocess.StandardInput.WriteLine("C:\\KMSOFT\\KMPDM");

        //        PDMPprocess.StandardInput.WriteLine("pdm.exe");
        //        //PDMPprocess.StandardInput.WriteLine("exit");
        //        //PDMPprocess.StandardInput.AutoFlush = true;
        //        //string str = PDMPprocess.StandardOutput.ReadToEnd();
        //        //PDMPprocess.WaitForExit();
        //        //PDMPprocess.Close();

        //    }
        //    //if (PDMPprocess == null)
        //    //{
        //    //    PDMPprocess = new Process();
        //    //    PDMPprocess.StartInfo.FileName = "C:\\KMSOFT\\KMPDM\\pdm.exe";

        //    //    PDMPprocess.Start();
        //    //}
        //    else
        //    {
        //        if (PDMPprocess.HasExited)
        //        {
        //            PDMPprocess.Start();
        //            PDMPprocess.StandardInput.WriteLine("pdm.exe");
        //        }

        //    }

        //}

      

        private void button4_Click(object sender, EventArgs e)
        {
            if (CAPPPprocess == null)
            {
                try
                {
                    CAPPPprocess = new Process();
                    CAPPPprocess.StartInfo.FileName = "C:\\KMSOFT\\KMCAPP\\kmcapp.exe";
                    //CAPPPprocess.StartInfo.FileName = "C:\\Program Files (x86)\\FormatFactory\\FormatFactory.exe";
                    CAPPPprocess.Start();
                }
                catch
                {
                    MessageBox.Show("打开工艺卡失败");
                }
                
            }
            else
            {
                if (CAPPPprocess.HasExited)
                {
                    CAPPPprocess.Start();
                }

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (PDMPprocess == null)
            {
                try
                {
                    PDMPprocess = new Process();
                PDMPprocess.StartInfo.WorkingDirectory = "C:\\KMSOFT\\KMPDM";
               // PDMPprocess.StartInfo.WorkingDirectory = "C:\\Program Files (x86)\\证照之星5.0";
                PDMPprocess.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
                PDMPprocess.StartInfo.UseShellExecute = false;
                PDMPprocess.StartInfo.RedirectStandardInput = true;
                PDMPprocess.StartInfo.RedirectStandardOutput = true;
                PDMPprocess.StartInfo.RedirectStandardError = true;
                PDMPprocess.StartInfo.CreateNoWindow = false;
                PDMPprocess.Start();
                PDMPprocess.StandardInput.WriteLine("C:\\KMSOFT\\KMPDM");
                PDMPprocess.StandardInput.WriteLine("pdm.exe");
               
                }
                catch
                {
                   MessageBox.Show("打开PDM程序失败") ;
                }
                

            }         
            else
            {
                if (PDMPprocess.HasExited)
                {
                    PDMPprocess.Start();
                    PDMPprocess.StandardInput.WriteLine("C:\\KMSOFT\\KMPDM");
                    PDMPprocess.StandardInput.WriteLine("pdm.exe");
                }
                else
                {
                    PDMPprocess.StandardInput.WriteLine("pdm.exe");
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.RestoreDirectory = true; //记忆上次浏览路径
                fileDialog.Multiselect = true;
                //fileDialog.Filter = "Excel文件(*.xls)|*.xls";
                fileDialog.Filter = "Excel文件(*.xls)|*.xls|xml格式表格(*.xml)|*.xml|PDF格式(*.pdf)|*.pdf";

                fileDialog.Title = "选择提交任务文件";


                if (fileDialog.ShowDialog() == DialogResult.OK)
                {

                  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //private void Button3_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        OpenFileDialog fileDialog = new OpenFileDialog();
        //        fileDialog.RestoreDirectory = true; //记忆上次浏览路径
        //                                            //fileDialog.Multiselect = true;
        //                                            //  fileDialog.Filter = "Excel文件(*.xls)|*.xls|xml格式表格(*.xml)|*.xml|PDF格式(*.pdf)|*.pdf";
        //        fileDialog.Title = "选择Excel或者PDF文件";


        //        if (fileDialog.ShowDialog() == DialogResult.OK)
        //        {

        //            try
        //            {
        //                // axAcroPDFShow.Show();
        //                axAcroPDFShow.LoadFile(fileDialog.FileName);
        //                axAcroPDFShow.setShowToolbar(false);
        //                axAcroPDFShow.setShowScrollbars(false);
        //                axAcroPDFShow.setPageMode("pages only");
        //                var temp = axAcroPDFShow.IsDisposed;
        //            }
        //            catch (Exception ex)
        //            {
        //                return;

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        private void Buttonmadelist_Click(object sender, EventArgs e)
        {
            int ii = 0;//图号数据总行数
            int jj = 0;//
            int kk = 1;//textbox写入行行编号
            //string ConnectionSql = "data source = localhost;initial catalog=BOM;usr id = sa;password = sa";
            if (comboBoxchoose.Text == null || comboBoxchoose.Text == ""|| comboBoxchoose.Text == "请选择工艺卡")
            {

                MessageBox.Show("请选择文件");
                return;
            }
            if (comboBoxchoose.Text.Length < 1)
            {
                MessageBox.Show("选择文件错误");
                return;
            }
            string tuhao = comboBoxchoose.Text;
            renewCard(tuhao);

        }
        private  void renewCard(string name)
        {
            string path = CardFilePath + "\\" + name ;
            if (!File.Exists(path))
            {
               MessageBox.Show("文件不存在");
                return;
            }
            dataGridView1.Rows.Clear();
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    int ii = 0;

                    dataGridView1.Rows.Clear();
                    IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    DataSet result = reader.AsDataSet();
                    int count = result.Tables[0].Rows.Count;
                    if(count>5)
                    {
                        dataGridView1.Rows.Add(count - 5);
                    }
                   
                    textBoxljmc.Text = result.Tables[0].Rows[1][2].ToString();
                    textBoxcl.Text = result.Tables[0].Rows[1][6].ToString();
                    textBoxjs.Text = result.Tables[0].Rows[1][10].ToString();
                    textBoxmpsl.Text = result.Tables[0].Rows[1][13].ToString();
                    textBoxth.Text = result.Tables[0].Rows[1][16].ToString();
                   
                    for ( ii = 5; ii < count; ii++)
                    {
                        //dataGridView1.Rows.Add();
                        dataGridView1.Rows[ii - 5].Cells[0].Value = result.Tables[0].Rows[ii][0].ToString();
                        dataGridView1.Rows[ii - 5].Cells[1].Value = result.Tables[0].Rows[ii][1].ToString();
                        dataGridView1.Rows[ii - 5].Cells[2].Value = result.Tables[0].Rows[ii][2].ToString();
                        dataGridView1.Rows[ii - 5].Cells[3].Value = result.Tables[0].Rows[ii][3].ToString();
                        dataGridView1.Rows[ii - 5].Cells[4].Value = result.Tables[0].Rows[ii][4].ToString();
                        dataGridView1.Rows[ii - 5].Cells[5].Value = result.Tables[0].Rows[ii][5].ToString();
                        dataGridView1.Rows[ii - 5].Cells[6].Value = result.Tables[0].Rows[ii][6].ToString();
                        dataGridView1.Rows[ii - 5].Cells[7].Value = result.Tables[0].Rows[ii][7].ToString();
                        dataGridView1.Rows[ii - 5].Cells[8].Value = result.Tables[0].Rows[ii][8].ToString();
                        dataGridView1.Rows[ii - 5].Cells[9].Value = result.Tables[0].Rows[ii][9].ToString();
                        dataGridView1.Rows[ii - 5].Cells[10].Value = result.Tables[0].Rows[ii][10].ToString();
                        dataGridView1.Rows[ii - 5].Cells[11].Value = result.Tables[0].Rows[ii][11].ToString();
                        dataGridView1.Rows[ii - 5].Cells[12].Value = result.Tables[0].Rows[ii][12].ToString();
                        dataGridView1.Rows[ii - 5].Cells[13].Value = result.Tables[0].Rows[ii][13].ToString();
                        dataGridView1.Rows[ii - 5].Cells[14].Value = result.Tables[0].Rows[ii][14].ToString();
                        dataGridView1.Rows[ii - 5].Cells[15].Value = result.Tables[0].Rows[ii][15].ToString();
                        dataGridView1.Rows[ii - 5].Cells[16].Value = result.Tables[0].Rows[ii][16].ToString();
                        dataGridView1.Rows[ii - 5].Cells[17].Value = result.Tables[0].Rows[ii][17].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件格式错误");
            }
            
            


        }

        private void ComboBoxchoose_Click(object sender, EventArgs e)
        {
            CardItemGenerate();
        }
        private void CardItemGenerate()
        {
            DirectoryInfo dir = new DirectoryInfo(CardFilePath);
            try
            {
               if(!dir.Exists)
                {
                    MessageBox.Show("获取工艺失败，无工艺文件");
                    return;
                }
                DirectoryInfo dirD = dir as DirectoryInfo;
               if (dirD == null)
                {
                    return;
                }
               else
                {
                    Cardlist.Clear();
                    FileSystemInfo[] files = dirD.GetFileSystemInfos();
                    foreach(FileSystemInfo filetemp in files)
                    {
                        FileInfo file = filetemp as FileInfo;
                        if(file !=null)
                        {
                           if(file.Name.Substring(file.Name.Length-5,5) == ".xlsx")
                            {
                                string item = file.Name;
                               Cardlist.Add(item);
                            }
                            else if (file.Name.Substring(file.Name.Length - 4, 4) == ".xls")
                            {
                                string item = file.Name;
                                Cardlist.Add(item);
                            }
                        }
                        else
                        {//xlsx
                            ;
                        }
                    }
                }
                comboBoxchoose.Items.Clear();
                foreach (var temp in Cardlist)
                {
                    comboBoxchoose.Items.Add(temp);
                }
            }
            catch (Exception ex)
            {
                Cardlist.Clear();
                comboBoxchoose.Items.Clear();
                foreach (var temp in Cardlist)
                {
                    comboBoxchoose.Items.Add(temp);
                }
                MessageBox.Show("获取工艺失败，请检查文件是否存在");
            }
            
        }

        private void BOMForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(10);
            //初始化表头

        }
        private void BOMTitleInit(DataGridView dgv)
        {
            int title = 0;
            DataGridViewRow gvr = dgv.Rows[title];
           
            //合并行 3,0-4,0 3,1-4,1 3,2-4,2 
        }
        private void RowandCloumAdd(DataGridView dgv,int rowstart,int rownumber,int cloumnumber)
        {
            ;
        }


        private void DataGridView1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Buttongetebom_Click(object sender, EventArgs e)
        {
            try
            {
                if (myConnection.State == ConnectionState.Closed)
                {
                    myConnection.Open();
                }
                //获取父节点
                myCommand.CommandText = "Select * from FBOMList ";

                myReader = myCommand.ExecuteReader();
                int fuidnumber = 0;
                int ziidnumber = 0;
                int jj = 0;
                int kk = 0;
                while (myReader.Read())
                {
                    object temp = myReader.GetValue(3);
                    string ss = temp.ToString();
                    if (ss == "0")
                    {
                        fuidnumber++;
                    }
                    else
                        ziidnumber++;
                }
                // myReader.Close();

                if (fuidnumber == 0)
                {
                    MessageBox.Show("工艺数据库连接失败");
                    return;
                }
                FBOMList[] bomlist = new FBOMList[fuidnumber + ziidnumber];
                for (int i = 0; i < bomlist.Count(); i++)
                {
                    bomlist[i] = new FBOMList();
                }
                myReader.Close();
                // TreeNode[] treeNode = new System.Windows.Forms.TreeNode[fuidnumber + ziidnumber];
                //int[,] treeid = new int[fuidnumber,ziidnumber];

                myCommand.CommandText = "Select * from FBOMList  ";
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    object temp = myReader.GetValue(0);
                    string ss = temp.ToString();
                    temp = myReader.GetValue(2);
                    bomlist[jj].BOMTYPE = temp.ToString();
                    temp = myReader.GetValue(3);
                    bomlist[jj].PRNTNODEID = temp.ToString();
                    temp = myReader.GetValue(4);
                    bomlist[jj].NODEID = temp.ToString();
                    temp = myReader.GetValue(5);
                    bomlist[jj].TH = temp.ToString();
                    temp = myReader.GetValue(6);
                    bomlist[jj].CSUFFIX = temp.ToString();
                    temp = myReader.GetValue(7);
                    bomlist[jj].LJMC = temp.ToString();
                    temp = myReader.GetValue(8);
                    bomlist[jj].CRIV = temp.ToString();
                    temp = myReader.GetValue(9);
                    bomlist[jj].CNUMBER = temp.ToString();
                    temp = myReader.GetValue(10);
                    bomlist[jj].CL = temp.ToString();
                    temp = myReader.GetValue(11);
                    bomlist[jj].GG = temp.ToString();
                    temp = myReader.GetValue(12);
                    bomlist[jj].XM = temp.ToString();
                    temp = myReader.GetValue(13);
                    bomlist[jj].SB = temp.ToString();
                    temp = myReader.GetValue(14);
                    bomlist[jj].XMU = temp.ToString();
                    temp = myReader.GetValue(15);
                    bomlist[jj].TZBL = temp.ToString();
                    temp = myReader.GetValue(16);
                    bomlist[jj].BZ = temp.ToString();
                    jj++;
                }
                myReader.Close();
                string[] Key1 = new string[fuidnumber];
                for (int lev1 = 0; lev1 < bomlist.Count(); lev1++)
                {

                    if (bomlist[lev1].PRNTNODEID == "0" && bomlist[lev1].BOMTYPE == "EBOM")
                    {
                        string Nodename = bomlist[lev1].TH + bomlist[lev1].LJMC;//图号+名称
                        TreeNode treeNodefu = new System.Windows.Forms.TreeNode(Nodename);
                        for (int lev2 = 0; lev2 < bomlist.Count(); lev2++)
                        {
                            if (bomlist[lev2].PRNTNODEID == bomlist[lev1].NODEID && bomlist[lev2].BOMTYPE == "EBOM")
                            {
                                string Nodename1 = bomlist[lev2].TH + bomlist[lev2].LJMC;//图号+名称
                                TreeNode treeNodefu1 = new System.Windows.Forms.TreeNode(Nodename1);
                                for(int lev3 = 0; lev3 < bomlist.Count(); lev3++)
                                {
                                    if(bomlist[lev3].PRNTNODEID == bomlist[lev2].NODEID && bomlist[lev3].BOMTYPE == "EBOM")
                                    {
                                        string Nodename2 = bomlist[lev3].TH + bomlist[lev3].LJMC;//图号+名称
                                        TreeNode treeNodefu2 = new System.Windows.Forms.TreeNode(Nodename2);
                                        treeNodefu1.Nodes.Add(Nodename2);
                                    }
                                }

                                treeNodefu.Nodes.Add(treeNodefu1);
                            }
                        }
                        treeView1.Nodes.Clear();
                        treeView1.Nodes.Add(treeNodefu);
                    }

                }
                myReader.Close();
                myConnection.Close();
            }
            catch
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }
                MessageBox.Show("工艺数据获取失败");
                return;
            }

        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (myConnection.State == ConnectionState.Closed)
                {
                    myConnection.Open();
                }
                //获取父节点
                myCommand.CommandText = "Select * from FBOMList  ";

                myReader = myCommand.ExecuteReader();
                int fuidnumber = 0;
                int ziidnumber = 0;
                int jj = 0;
                int kk = 0;
                while (myReader.Read())
                {
                    object temp = myReader.GetValue(3);
                    string ss = temp.ToString();
                    if (ss == "0")
                    {
                        fuidnumber++;
                    }
                    else
                        ziidnumber++;
                }
                // myReader.Close();

                if (fuidnumber == 0)
                {
                    MessageBox.Show("工艺数据库连接失败");
                    return;
                }
                FBOMList[] bomlist = new FBOMList[fuidnumber + ziidnumber];
                for (int i = 0; i < bomlist.Count(); i++)
                {
                    bomlist[i] = new FBOMList();
                }
                myReader.Close();
                // TreeNode[] treeNode = new System.Windows.Forms.TreeNode[fuidnumber + ziidnumber];
                //int[,] treeid = new int[fuidnumber,ziidnumber];

                myCommand.CommandText = "Select * from FBOMList ";
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                     object temp = myReader.GetValue(0);
                    string ss = temp.ToString();
                    temp = myReader.GetValue(2);
                    bomlist[jj].BOMTYPE = temp.ToString();
                    temp = myReader.GetValue(3);
                    bomlist[jj].PRNTNODEID = temp.ToString();
                    temp = myReader.GetValue(4);
                    bomlist[jj].NODEID = temp.ToString();
                    temp = myReader.GetValue(5);
                    bomlist[jj].TH = temp.ToString();
                    temp = myReader.GetValue(6);
                    bomlist[jj].CSUFFIX = temp.ToString();
                    temp = myReader.GetValue(7);
                    bomlist[jj].LJMC = temp.ToString();
                    temp = myReader.GetValue(8);
                    bomlist[jj].CRIV = temp.ToString();
                    temp = myReader.GetValue(9);
                    bomlist[jj].CNUMBER = temp.ToString();
                    temp = myReader.GetValue(10);
                    bomlist[jj].CL = temp.ToString();
                    temp = myReader.GetValue(11);
                    bomlist[jj].GG = temp.ToString();
                    temp = myReader.GetValue(12);
                    bomlist[jj].XM = temp.ToString();
                    temp = myReader.GetValue(13);
                    bomlist[jj].SB = temp.ToString();
                    temp = myReader.GetValue(14);
                    bomlist[jj].XMU = temp.ToString();
                    temp = myReader.GetValue(15);
                    bomlist[jj].TZBL = temp.ToString();
                    temp = myReader.GetValue(16);
                    bomlist[jj].BZ = temp.ToString();
                    jj++;
                }
                myReader.Close();
                string[] Key1 = new string[fuidnumber];
          
                for (int lev1 = 0; lev1 < bomlist.Count(); lev1++)
                {

                    if (bomlist[lev1].PRNTNODEID == "0" && bomlist[lev1].BOMTYPE == "PBOM")
                    {
                        string Nodename = bomlist[lev1].TH  + bomlist[lev1].LJMC;//图号+名称
                        TreeNode treeNodefu = new System.Windows.Forms.TreeNode(Nodename);
                        for (int lev2 = 0; lev2 < bomlist.Count(); lev2++)
                        {
                            if (bomlist[lev2].PRNTNODEID == bomlist[lev1].NODEID && bomlist[lev2].BOMTYPE == "PBOM")
                            {
                                string Nodename1 = bomlist[lev2].TH + bomlist[lev2].LJMC;//图号+名称
                                TreeNode treeNodefu1 = new System.Windows.Forms.TreeNode(Nodename1);
                                for (int lev3 = 0; lev3 < bomlist.Count(); lev3++)
                                {
                                    if (bomlist[lev3].PRNTNODEID == bomlist[lev2].NODEID && bomlist[lev3].BOMTYPE == "PBOM")
                                    {
                                        string Nodename2 = bomlist[lev3].TH + bomlist[lev3].LJMC;//图号+名称
                                        TreeNode treeNodefu2 = new System.Windows.Forms.TreeNode(Nodename2);
                                        treeNodefu1.Nodes.Add(Nodename2);
                                    }
                                }

                                treeNodefu.Nodes.Add(treeNodefu1);
                            }
                        }
                        treeView2.Nodes.Clear();
                        treeView2.Nodes.Add(treeNodefu);
                    }

                }
                myReader.Close();
                myConnection.Close();
            }
            catch
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }
                MessageBox.Show("工艺数据获取失败");
                return;
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GroupBox1_SizeChanged(object sender, EventArgs e)
        {
            GroupBox grb = sender as GroupBox;
            autosizeebom.controlAutoSize(grb);
        }

        //private void GroupBox2_SizeChanged(object sender, EventArgs e)
        //{
        //    GroupBox grb = sender as GroupBox;
        //    autosizeebom.controlAutoSize(grb);
        //}

        private void GroupBox3_SizeChanged(object sender, EventArgs e)
        {

            GroupBox grb = sender as GroupBox;
            autosizecard.controlAutoSize(grb);
        }
        private void GroupBox2_SizeChanged_1(object sender, EventArgs e)
        {
            GroupBox grb = sender as GroupBox;
            autosizepbom.controlAutoSize(grb);
        }
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null /*&& e.Node.FirstNode != null*/)
            {
                string nodename = e.Node.Text;

                if (nodename == null)
                {
                    return;
                }
                if (nodename.Length < 1)
                {
                    return;
                }
                //int index = nodename.IndexOf(' ');
                string tuhao = nodename;

                try
                {
                    if (myConnection.State == ConnectionState.Closed)
                    {
                        myConnection.Open();
                    }
                    //获取父节点
                    myCommand.CommandText = "Select * from FBOMList   ";

                    myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {

                        object temp1 = myReader.GetValue(5);
                        object temp2 = myReader.GetValue(2);
                        object temp3 = myReader.GetValue(7);
                        string ss = temp1.ToString()+ temp3.ToString();
                        string type = temp2.ToString();
                        if (ss == tuhao && type == "EBOM")
                        {
                            FBOMList bomlist = new FBOMList();
                            temp1 = myReader.GetValue(0);
                            bomlist.KEYID = temp1.ToString();
                            temp1 = myReader.GetValue(1);
                            bomlist.CVOLID = temp1.ToString();
                            temp1 = myReader.GetValue(2);
                            bomlist.BOMTYPE = temp1.ToString();
                            temp1 = myReader.GetValue(3);
                            bomlist.PRNTNODEID = temp1.ToString();
                            temp1 = myReader.GetValue(4);
                            bomlist.NODEID = temp1.ToString();
                            temp1 = myReader.GetValue(5);
                            bomlist.TH = temp1.ToString();
                            temp1 = myReader.GetValue(6);
                            bomlist.CSUFFIX = temp1.ToString();
                            temp1 = myReader.GetValue(7);
                            bomlist.LJMC = temp1.ToString();
                            temp1 = myReader.GetValue(8);
                            bomlist.CRIV = temp1.ToString();
                            temp1 = myReader.GetValue(9);
                            bomlist.CNUMBER = temp1.ToString();
                            temp1 = myReader.GetValue(10);
                            bomlist.CL = temp1.ToString();
                            temp1 = myReader.GetValue(11);
                            bomlist.GG = temp1.ToString();
                            temp1 = myReader.GetValue(12);
                            bomlist.XM = temp1.ToString();
                            temp1 = myReader.GetValue(13);
                            bomlist.SB = temp1.ToString();
                            temp1 = myReader.GetValue(14);
                            bomlist.XMU = temp1.ToString();
                            temp1 = myReader.GetValue(15);
                            bomlist.TZBL = temp1.ToString();
                            temp1 = myReader.GetValue(16);
                            bomlist.BZ = temp1.ToString();
                            textBoxbomtype.Text = bomlist.BOMTYPE;
                            textBoxdaihao.Text = bomlist.CSUFFIX;
                            textBoxmingcheng.Text = bomlist.LJMC;
                            textBoxbanben.Text = bomlist.CSUFFIX; ;
                            textBoxtuhao.Text = bomlist.CRIV; ;
                            textBoxxingming.Text = bomlist.XM;
                            textBoxshebei.Text = bomlist.SB;
                            textBoxxiangmu.Text = bomlist.XMU;
                            textBoxbili.Text = bomlist.TZBL;
                            textBoxshuliang.Text = bomlist.CNUMBER;
                            textBoxtuhao.Text = bomlist.TH;
                            textBoxcaizhi.Text = bomlist.CL;
                            break;
                        }
                    }
                    if (myReader != null)
                    {
                        myReader.Close();
                    }
                    if (myConnection != null)
                    {
                        myConnection.Close();
                    }
                }
                catch
                {
                    if (myReader != null)
                    {
                        myReader.Close();
                    }
                    if (myConnection != null)
                    {
                        myConnection.Close();
                    }
                    MessageBox.Show("工艺数据获取失败");
                    return;
                }
            }
        }

        private void TreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node != null /*&& e.Node.FirstNode != null*/)
            {
                string nodename = e.Node.Text;

                if (nodename == null)
                {
                    return;
                }
                if (nodename.Length < 1)
                {
                    return;
                }
                //int index = nodename.IndexOf(' ');
                string tuhao = nodename;
                try
                {
                    if (myConnection.State == ConnectionState.Closed)
                    {
                        myConnection.Open();
                    }
                    //获取父节点
                    myCommand.CommandText = "Select * from FBOMList ";

                    myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        object temp1 = myReader.GetValue(5);
                        object temp2 = myReader.GetValue(2);
                        object temp3 = myReader.GetValue(7);
                        string ss = temp1.ToString()+ temp3.ToString();
                        string type = temp2.ToString();
                        if (ss == tuhao && type == "PBOM")
                        {
                     
                            FBOMList bomlist = new FBOMList();
                            temp1 = myReader.GetValue(0);
                            bomlist.KEYID = temp1.ToString();
                            temp1 = myReader.GetValue(1);
                            bomlist.CVOLID = temp1.ToString();
                            temp1 = myReader.GetValue(2);
                            bomlist.BOMTYPE = temp1.ToString();
                            temp1 = myReader.GetValue(3);
                            bomlist.PRNTNODEID = temp1.ToString();
                            temp1 = myReader.GetValue(4);
                            bomlist.NODEID = temp1.ToString();
                            temp1 = myReader.GetValue(5);
                            bomlist.TH = temp1.ToString();
                            temp1 = myReader.GetValue(6);
                            bomlist.CSUFFIX = temp1.ToString();
                            temp1 = myReader.GetValue(7);
                            bomlist.LJMC = temp1.ToString();
                            temp1 = myReader.GetValue(8);
                            bomlist.CRIV = temp1.ToString();
                            temp1 = myReader.GetValue(9);
                            bomlist.CNUMBER = temp1.ToString();
                            temp1 = myReader.GetValue(10);
                            bomlist.CL = temp1.ToString();
                            temp1 = myReader.GetValue(11);
                            bomlist.GG = temp1.ToString();
                            temp1 = myReader.GetValue(12);
                            bomlist.XM = temp1.ToString();
                            temp1 = myReader.GetValue(13);
                            bomlist.SB = temp1.ToString();
                            temp1 = myReader.GetValue(14);
                            bomlist.XMU = temp1.ToString();
                            temp1 = myReader.GetValue(15);
                            bomlist.TZBL = temp1.ToString();
                            temp1 = myReader.GetValue(16);
                            textBoxptype.Text = bomlist.BOMTYPE;
                            textBoxphouzui.Text = bomlist.CSUFFIX;
                            textBoxpname.Text = bomlist.LJMC;
                            textBoxpbanben.Text = bomlist.CRIV;
                          //  textBox5.Text = bomlist.CRIV; ;
                            textBoxxingmingp.Text = bomlist.XM;
                            //textBoxpnumber.Text = bomlist.SB;
                            textBoxpbili.Text = bomlist.TZBL;
                            textBoxpguige.Text = bomlist.GG;
                            textBoxpcailiao.Text = bomlist.CL;
                            textBoxpnumber.Text = bomlist.CNUMBER;
                            textBox12.Text = bomlist.CL;
                            break;
                        }
                    }
                    if (myReader != null)
                    {
                        myReader.Close();
                    }
                    if (myConnection != null)
                    {
                        myConnection.Close();
                    }
                }
                catch
                {
                    if (myReader != null)
                    {
                        myReader.Close();
                    }
                    if (myConnection != null)
                    {
                        myConnection.Close();
                    }
                    MessageBox.Show("工艺数据获取失败");
                    return;
                }
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }
    }
}
