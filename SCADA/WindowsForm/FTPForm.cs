using System;
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
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SCADA
{
    public partial class FTPForm : Form
    {
        private string IP;
        private int port;
      //  private string FTPSentPath;
        private string LocalSavePath;
        private string RemoteLoadpath;
        private string RemodeUpLoadpath;
        private string DirPath;
        private string Dirurl;
        private string RemoteLoadurl;
        private string RemoteUpLoadurl;
        private string name;
        private string key;
        TcpClient client;
        NetworkStream netStream;
        StreamReader sr;
        StreamWriter sw;
        public FTPForm()
        {
            InitializeComponent();

            AddDataGridColumns(STR_Columns, DGVTakeOff1, DGVTakeOffDb1);
            SetDataGridAttribute(DGVTakeOff1);
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
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                IP = textBox3.Text;
                
                name = textBoxname.Text;
                key =textBoxkey.Text ;
               // FTPSentPath = textBoxremoteload.Text;
                RemoteLoadpath = textBoxremoteload.Text;
                RemodeUpLoadpath = textBoxremoteupload.Text;
                if (textBox4.Text == "")
                {
                    RemoteLoadurl = "ftp://" + IP + "/" + RemoteLoadpath + "/";
                    RemoteUpLoadurl = "ftp://" + IP + "/" + RemodeUpLoadpath + "/";
                }
                else
                {
                    port = Convert.ToInt32(textBox4.Text);
                    RemoteLoadurl = "ftp://" + IP + ":" + port + "/" + RemoteLoadpath + "/";
                    RemoteUpLoadurl = "ftp://" + IP + ":" + port + "/" + RemodeUpLoadpath + "/";

                }
            }
            catch
            {
                PushMessage(richTextBox1, "通信或者路径设置错误，连接失败！ ", Color.Black);      
               label1.Text = "通信参数错误";
               return;
            }
            try
            {
               // client = new TcpClient(IP, port);
                DirList.Clear();
                DirList = GetAllList(RemoteLoadurl);
                renewBox(DirList, listBoxDir);
                //DirPath = ;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                buttonlocal.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                buttondiconnect.Enabled = true;
                buttonconnect.Enabled = false;

                PushMessage(richTextBox1, "连接成功！ ", Color.Black);
                label1.Text = "连接成功";
                return;
            }
            catch
            {
                PushMessage(richTextBox1, "连接失败！ ", Color.Black);

                label1.Text = "连接失败";
                return;
            }
            //获取FTP根目录下的子目录和文件列表
            //GetDirAndFiles(@"server:\");

        }
        List<string> DirList = new List<string>();

        private   bool renewBox(List<string>list,ListBox listBoxDir)
        {
            if (list.Count == 0)
            {
                PushMessage(richTextBox1, "目录为空！ ", Color.Black);
               return  false;
            }
            else
            {
                listBoxDir.Items.Clear();
                foreach(var item in list)
                {
                    if(item.Length>0)
                    {
                        this.listBoxDir.Items.Add(item);
                    }
                }
                //获取文件列表
               // this.listBoxFile.Items.Clear();
                return true;
            }

        }
        private List<string> GetAllList(string url)
        {
            List<string> list = new List<string>();
            //string v1 = "ftp://192.168.1.100";//ftp://192.168.1.100/FTP/
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(url));
            req.Credentials = new NetworkCredential(name, key);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            req.UseBinary = true;
            req.UsePassive = true;
            try
            {
                using (FtpWebResponse res = (FtpWebResponse)req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            list.Add(s);
                        }
                    }
                }

                Dirurl = url;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return list;
        }
        private void buttonlocal_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "选择任务存放路径";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(folderBrowser.SelectedPath))
                {
                    PushMessage(richTextBox1, "选择的任务路径为空！ ", Color.Black);
                  
                    return;
                }
                else
                {
                    textBox5.Text = folderBrowser.SelectedPath.ToString();
                    LocalSavePath = textBox5.Text;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
                if (this.listBoxDir.SelectedIndex == -1)
                {
                    //获取当前目录下的子目录和文件列表
                    // GetAllList(this.groupBoxDir.Text);
                }
                else
                {
                    try
                    {
                        string path = Dirurl+listBoxDir.SelectedItem.ToString();
                        //找到最后一个\，截去\
                        //path = path.Substring(0, path.LastIndexOf("\\"));
                        //再从右到左找一个\
                        //int num = path.LastIndexOf("\\");
                        //截去\后面的子串
                     //   path = path.Substring(0, num + 1);
                        DirList.Clear();
                        DirList = GetAllList(path);
                        renewBox(DirList, listBoxDir);
                    }
                    catch
                    {
                        PushMessage(richTextBox1, "下级目录打开失败！ ", Color.Black);
                    }
                }
                //int index = listBoxDir.SelectedIndex;
                ////获取所选择目录下的子目录和文件列表
                //InsertaFile(listBoxDir, index, listBoxFile);
        }

        private void buttondiconnect_Click(object sender, EventArgs e)
        {
            PushMessage(richTextBox1, "连接关闭！ ", Color.Black);
            
            client.Close();
            label1.Text = "连接关闭";
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button7.Enabled = false;
            buttondiconnect.Enabled = false;
            buttonconnect.Enabled = true;
        }
        private void PushMessage(RichTextBox richText, string Msg, Color color)
        {
            string Message = Msg + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
            richText.SelectionColor = color;
            richText.AppendText(Message);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listBoxDir.SelectedIndex == -1)
            {
                //获取当前目录下的子目录和文件列表
               // GetAllList(this.groupBoxDir.Text);
            }
            else
            {
                    string path = listBoxDir.SelectedItem.ToString();
                    path = Dirurl + "//" + path;
                     listBoxFile.Items.Add(path);                                     
            }
        }


        private void button8_Click_1(object sender, EventArgs e)
        {
            if (false)
            {
                //获取当前目录下的子目录和文件列表
                // GetAllList(this.groupBoxDir.Text);
            }
            else
            {
                try
                {

                    string path = Dirurl;
                    //找到最后一个\，截去\
                    path = path.Substring(0, path.LastIndexOf("//"));
                    //再从右到左找一个\
                    int num = path.LastIndexOf("//");
                    //截去\后面的子串
                    path = path.Substring(0, num + 1); 
                    DirList.Clear();
                    DirList = GetAllList(RemoteLoadurl);
                    renewBox(DirList, listBoxDir);
                }

                catch
                {
                    PushMessage(richTextBox1, "上级目录打开失败！ ", Color.Black);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listBoxFile.SelectedIndex == -1)
            {
                //获取当前目录下的子目录和文件列表
                // GetAllList(this.groupBoxDir.Text);
            }
            else
            {
                string path = listBoxFile.SelectedItem.ToString();
                listBoxFile.Items.Remove(path);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listBoxFile.SelectedIndex == -1)
            {
                //获取当前目录下的子目录和文件列表
                // GetAllList(this.groupBoxDir.Text);
            }
            else
            {
                string path = listBoxFile.SelectedItem.ToString();
                int index = path.LastIndexOf("//");
                string name = path.Substring(index+2);
                Download(path,LocalSavePath,name);
            }
        }
        /// <summary>
        /// 从ftp服务器下载文件的功能
        /// </summary>
        /// <param name="ftpfilepath">ftp下载的地址</param>
        /// <param name="filePath">存放到本地的路径</param>
        /// <param name="fileName">保存的文件名称</param>
        /// <returns></returns>
        private  bool Download(string ftpfilepath, string filePath,string filename)
        {
            try
            {

                FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpfilepath));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(name, key);
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                filename = filePath + "\\" + filename;
                FileStream outputStream = new FileStream(filename, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();

                PushMessage(richTextBox1, filename+"下载成功！", Color.Black);
                return true;
            }
            catch (Exception ex)
            {

                PushMessage(richTextBox1, filename+"下载失败！ ", Color.Black);
                //errorinfo = string.Format("因{0},无法下载", ex.Message);
                return false;
            }
        }

        string[] STR_Columns = { "任务文件路径", "加载时间" };

        DataTable DGVTakeOffDb1 = new DataTable();
        private bool IsGridwiewHasFilePath(DataTable table, string filename)
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
        private void button5_Click(object sender, EventArgs e)
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
                    for (int i = 0; i < DGVTakeOff1.Rows.Count; i++)
                    {
                        string[] array = new string[STR_Columns.Length];
                        array[0] = DGVTakeOff1.Rows[i].Cells[1].Value.ToString();
                        array[1] = DGVTakeOff1.Rows[i].Cells[2].Value.ToString();
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
                    DGVTakeOff1.DataSource = null;
                    DGVTakeOff1.DataSource = DGVTakeOffDb1;
                    DGVTakeOff_SizeChanged(DGVTakeOff1, null);
                    DGVTakeOff1.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DeleteDataGridView(STR_Columns, DGVTakeOffDb1, DGVTakeOff1);
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
           
             DGVTakeOff_SizeChanged(gridView, null);
            gridView.ClearSelection();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string str1 = "", str2 = "";
            for (int jj = 0; jj < DGVTakeOff1.Rows.Count; jj++)
            {
                if ((bool)DGVTakeOff1.Rows[jj].Cells[0].EditedFormattedValue)
                {
                  //  string filename = DGVTakeOff1.Rows[jj].Cells[1].Value.ToString().Split('\\');
                    //string s1 = DGVTakeOffDb.Rows[jj][0].ToString() + "\\";
                    //string s2 = "\\" +xmlwr.configDictionary[XmlWR.KEYS.服务器IP.ToString()] + "\\" + xmlwr.configDictionary[XmlWR.KEYS.上传路径.ToString()] + "\\";
                      string path = DGVTakeOff1.Rows[jj].Cells[1].Value.ToString();
                      int index = path.LastIndexOf("\\");
                    string name = path.Substring(index+2);
                    //SendFile(s1,"//192.168.8.98/共享文件夹/");
                    //string name = ;
                    if (UploadFile(RemoteUpLoadurl, path, name))
                    {
                        str1 += name;
                    }
                    else
                    {
                        str2 += name;
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
        /// <summary>
        /// 上传文件到远程ftp
        /// </summary>
        /// <param name="ftpPath">ftp上的文件路径</param>
        /// <param name="path">本地的文件目录</param>
        /// <param name="id">文件名</param>
        /// <returns></returns>
        private  bool UploadFile(string ftpPath, string path, string id)
        {
            string erroinfo = "";
            FileInfo f = new FileInfo(path);
            string v11 = ftpPath + id;// "ftp://192.168.1.100/aaa";// "ftp://192.168.1.100";
            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(v11));
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential(name, key);
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
            reqFtp.ContentLength = f.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = f.OpenRead();
            try
            {
                Stream strm = reqFtp.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                erroinfo = "完成";
                return true;
            }
            catch (Exception ex)
            {
                erroinfo = string.Format("因{0},无法完成上传", ex.Message);
                return false;
            }
        }

        private void buttondiconnect_Click_1(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            buttonlocal.Enabled = true;
            button8.Enabled = false;
            buttondiconnect.Enabled = false;
            buttonconnect.Enabled = true;
        }

     
    }
}
