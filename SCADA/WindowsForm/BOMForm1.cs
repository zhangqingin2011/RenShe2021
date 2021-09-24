using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Data.SqlClient;
using System.Diagnostics;

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

            comboBoxFun1.SelectedIndex = 0;
            comboBoxFun2.SelectedIndex = 0;

            //tabControl1.TabPages.Remove(tabPageEBOM);
            //tabControl1.TabPages.Remove(tabPagePBOM);
            //tabControl1.TabPages.Remove(tabPagegongyi);
            tabControl1.TabPages.Remove(tabPageBOM);
            tabControl1.TabPages.Remove(tabPageCARD);


        }
        //public void GenerateForm(int form_index, object sender)
        //{
        //    // 反射生成窗体//只生成一次
        //    if (m_Formarre1[form_index] == null && form_index >= 0 && form_index < tabControl1.TabCount)
        //    {
        //        string formClassSTR = ((TabControl)sender).SelectedTab.Tag.ToString();
        //        m_Formarre1[form_index] = (Form)Assembly.GetExecutingAssembly().CreateInstance(formClassSTR);
        //        // m_Formarr[10 ] = { "SCADA.VideoForm, Text: VideoForm"};
        //        if (m_Formarre1[form_index] != null)
        //        {
        //            //设置窗体没有边框 加入到选项卡中
        //            m_Formarre1[form_index].FormBorderStyle = FormBorderStyle.None;
        //            m_Formarre1[form_index].TopLevel = false;
        //            m_Formarre1[form_index].Parent = ((TabControl)sender).SelectedTab;
        //            m_Formarre1[form_index].ControlBox = false;
        //            m_Formarre1[form_index].Dock = DockStyle.Fill;
        //            if (ChangeLanguage.defaultcolor != Color.White)
        //            {
        //                ChangeLanguage.LoadSkin(m_Formarre1[form_index], ChangeLanguage.defaultcolor);
        //            }
        //            m_Formarre1[form_index].Show();
        //        }
        //    }

        ////}
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //(tabControl1.SelectedIndex, sender);
            
           
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void BOMForm_SizeChanged(object sender, EventArgs e)
        {
            aotosize.controlAutoSize(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileroad = BOMFilePath + "\\f1.xls" ;
           // webBrowser1.Navigate(fileroad); 
        }

        private void buttonmadelist_Click(object sender, EventArgs e)
        {
            int ii=0;//图号数据总行数
            int jj=0;//
            int kk = 1;//textbox写入行行编号
            //string ConnectionSql = "data source = localhost;initial catalog=BOM;usr id = sa;password = sa";
            if (comboBoxchoose.Text == null || comboBoxchoose.Text == "")
            {
                
                MessageBox.Show("请选择文件");
                return;
            }
            if (comboBoxchoose.Text.Length<12)
             {
                 MessageBox.Show("选择文件错误");
                 return;
             }
            string tuhao = comboBoxchoose.Text.Substring(0,11);
           

            if (myConnection.State == ConnectionState.Closed)
            {
                try
                {
                    myConnection.Open();
                }
                catch
                {

                    myConnection.Close();
                    MessageBox.Show("工艺数据库连接失败");
                    return;

                }
            }
            try
            {

                if (myConnection.State == ConnectionState.Open)
                {
                    //获取表格中图号指定图号的数据行数
                     myCommand.CommandText= "Select * from dbo.GY_GONGXUINFOR where TH='" + tuhao + "' ";

                     myReader = myCommand.ExecuteReader();
                     ii = 0;
                    while(myReader.Read())
                    {
                        object temp = myReader.GetValue(2);
                        string str = temp.ToString();
                        if (str == tuhao)
                        {
                            ii++;
                        }
                    }

                    myReader.Close();
                    if(ii<1)
                    {
                        MessageBox.Show("当前文件工序内容为空");

                        myConnection.Close();
                        return;

                    }
                    if(ii>12)
                    {

                        myConnection.Close();
                        return;

                        
                    }
                    FGongxu[] gongxulist = new FGongxu[ii];
                    for (int i = 0; i < gongxulist.Count(); i++)
                    {
                        gongxulist[i] = new FGongxu();
                    }
                    //获取当前图号的所有数据，保存在gongxulist数组中
                    myCommand.CommandText = "Select * from dbo.GY_GONGXUINFOR where TH='" + tuhao + "' ";

                    myReader = myCommand.ExecuteReader();
                    
                    while (myReader.Read())
                    {

                        object temp1 = myReader.GetValue(2);
                        string str = temp1.ToString();
                        if (str == tuhao)
                        {
                            object temp = myReader.GetValue(0);
                            gongxulist[jj].KEYID = temp.ToString();
                            object temp2 = myReader.GetValue(1);
                            gongxulist[jj].CVOLID = temp2.ToString();

                             temp = myReader.GetValue(2);
                             gongxulist[jj].TH = temp.ToString();

                             temp = myReader.GetValue(3);
                             gongxulist[jj].LJMC = temp.ToString();

                             temp = myReader.GetValue(4);
                             gongxulist[jj].CL = temp.ToString();

                             temp = myReader.GetValue(5);
                             gongxulist[jj].MTJS = temp.ToString();

                             temp = myReader.GetValue(6);
                             gongxulist[jj].MPJS = temp.ToString();

                             temp = myReader.GetValue(7);
                             gongxulist[jj].GXH = temp.ToString();

                             temp = myReader.GetValue(8);
                             gongxulist[jj].AZ = temp.ToString();

                            temp = myReader.GetValue(9);
                            
                            gongxulist[jj].GBH = temp.ToString();
                            temp = myReader.GetValue(10);

                            gongxulist[jj].GXMC = temp.ToString();
                            temp = myReader.GetValue(11);

                           gongxulist[jj].GXNR = temp.ToString();
                           temp = myReader.GetValue(12);
                           gongxulist[jj].TSJGLJS = temp.ToString();
                            temp = myReader.GetValue(13);
                            gongxulist[jj].JGFS = temp.ToString();
                            temp = myReader.GetValue(14);
                            gongxulist[jj].ZZZS = temp.ToString();
                            temp = myReader.GetValue(15);
                            gongxulist[jj].JJSD = temp.ToString();
                            temp = myReader.GetValue(16);
                            gongxulist[jj].QXSD = temp.ToString();
                            temp = myReader.GetValue(17);
                            gongxulist[jj].JGYL = temp.ToString();
                            temp = myReader.GetValue(18);
                            gongxulist[jj].JCMC = temp.ToString();
                            temp = myReader.GetValue(19);
                            gongxulist[jj].JJMC = temp.ToString();
                            temp = myReader.GetValue(20);
                            gongxulist[jj].DJMC = temp.ToString();
                            temp = myReader.GetValue(21);
                            gongxulist[jj].DJZJ = temp.ToString();
                            temp = myReader.GetValue(22);
                            gongxulist[jj].WJ =temp.ToString();
                            temp = myReader.GetValue(23);
                            gongxulist[jj].NJ = temp.ToString();
                            temp = myReader.GetValue(24);
                            gongxulist[jj].GSDE = temp.ToString();
                            temp = myReader.GetValue(25);
                            gongxulist[jj].CXMC = temp.ToString();
                            
                            jj++;
                        }
                    }

                    myReader.Close();

                    myConnection.Close();
                    clesrgongyika();

                    //找到工序=1，工步=0的行内容，填入textbox中。
                    for (jj = 0; jj < ii;jj++ )
                    {
                        if (gongxulist[jj].GXH == "1" && gongxulist[jj].GBH == "" && gongxulist[jj].GXMC!="")
                        {
                           
                                textBox0203.Text = gongxulist[jj].LJMC;//零件名称
                                textBox0205.Text = gongxulist[jj].CL;// 材料
                                textBox0208.Text = gongxulist[jj].MTJS;//每台件数
                                textBox0210.Text = gongxulist[jj].MPJS;//每批件数
                                textBox0213.Text = gongxulist[jj].TH;// 图号
                                //textBox0217.Text = DateTime.Today.ToShortDateString();// 日期
                                writepbomtorow(1,1 ,gongxulist[jj]);
                            if(gongxulist[jj].JCMC == "加工中心")
                            {
                                //comboBoxFun1.Text = "加工中心";
                                comboBoxFun1.SelectedIndex = 2;
                            }
                            else if (gongxulist[jj].JCMC == "车床")
                            {
                                //comboBoxFun1.Text = "车床";
                                comboBoxFun1.SelectedIndex = 1;
                            }
                            else
                                comboBoxFun1.SelectedIndex = 0;
                                
                                kk=2;

                        }
                       
                    }
                    for (jj = 0; jj < ii; jj++)
                    {
                        if (gongxulist[jj].GXH == "1" && gongxulist[jj].GBH == "1")
                        {
                            writepbomtorow(0,kk, gongxulist[jj]);
                            kk++;
                        }
                    }
                    for (jj = 0; jj < ii; jj++)
                    {
                        if (gongxulist[jj].GXH == "1" && gongxulist[jj].GBH == "2")
                        {
                            writepbomtorow(0,kk, gongxulist[jj]);
                            kk++;
                        }
                    }
                    for (jj = 0; jj < ii; jj++)
                    {
                        if (gongxulist[jj].GXH == "1" && gongxulist[jj].GBH == "3")
                        {
                            writepbomtorow(0, kk, gongxulist[jj]);
                            kk++;
                        }
                    }
                    for (jj = 0; jj < ii; jj++)
                    {
                        if (gongxulist[jj].GXH == "2" && gongxulist[jj].GBH == "" && gongxulist[jj].GXMC != "")
                        {
                            writepbomtorow(2,kk, gongxulist[jj]);
                            kk++;
                            if (gongxulist[jj].JCMC == "加工中心")
                            {
                               // comboBoxFun2.Text = "加工中心";
                                comboBoxFun2.SelectedIndex = 2;
                            }
                            else if (gongxulist[jj].JCMC == "车床")
                            {
                                //comboBoxFun2.Text = "车床";
                                comboBoxFun2.SelectedIndex = 1;
                            }
                            else
                                comboBoxFun2.SelectedIndex = 0;
                        }
                    }
                    for (jj = 0; jj < ii; jj++)
                    {
                        if (gongxulist[jj].GXH == "2" && gongxulist[jj].GBH == "1")
                        {
                            writepbomtorow(0,kk, gongxulist[jj]);
                            kk++;
                        }
                    } 
                    for (jj = 0; jj < ii; jj++)
                    {
                        if (gongxulist[jj].GXH == "2" && gongxulist[jj].GBH == "2")
                        {
                            writepbomtorow(0,kk, gongxulist[jj]);
                            kk++;
                        }
                    } 
                    for (jj = 0; jj < ii; jj++)
                    {
                        if (gongxulist[jj].GXH == "2" && gongxulist[jj].GBH == "3")
                        {
                            writepbomtorow(0, kk, gongxulist[jj]);
                            kk++;
                        }
                    }

                 
                }
                int length = comboBoxchoose.Text.Length;
                string name = comboBoxchoose.Text.Substring(length-2,2);
                if(name == "连接轴")
                {
                    labeltype1.Text="A";
                }
                else if(name =="中间轴")
                {
                    labeltype1.Text = "B";
                }
                else if(name =="上板")
                {
                    labeltype1.Text = "C";
                }
                else if(name == "下板")
                {
                    labeltype1.Text = "D";
                }

            }
            catch
            {

                myReader.Close();

                myConnection.Close();
                MessageBox.Show("工艺数据库连接失败");
                return;

            }
        }
        private void clesrgongyika()
        {
            //foreach (Control control in this.groupBox2.Controls)
            //{
            //    if (control is TextBox)
            //    {
            //        TextBox temp = (TextBox)control;
            //        string names = temp.Name;
            //        if (names.Length < 11)//textBox0306
            //        {
            //            return;
            //        }
            //        string strno=names.Substring(7,2);
            //        if (names == "0606")
            //        {
            //            int i =9;
            //        }

            //        if (strno == "00" || strno == "01" || strno == "02" || strno == "03" || strno == "03")
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            temp.Text = "";
            //        }
            //        if (strno == "10")
            //        {
            //            int i = 10;
            //        } if (strno == "14")
            //        {
            //            int i = 10;
            //        } if (strno == "15")
            //        {
            //            int i = 10;
            //        }
            //    }
            //}

            textBox0203.Text = "";//零件名称
            textBox0205.Text = "";// 材料
            textBox0208.Text = "";//每台件数
            textBox0210.Text = "";//每批件数
            textBox0213.Text = "";

            textBox0500.Text = "";//
            textBox0501.Text = "";//
            textBox0502.Text = "";//
            textBox0503.Text = "";//
            textBox0504.Text = "";//
            textBox0505.Text = "";//
            textBox0506.Text = "";//
            textBox0507.Text = "";//
            textBox0508.Text = "";//
            textBox0509.Text = "";//
            textBox0510.Text = "";//
            textBox0511.Text = "";//
            textBox0512.Text = "";//
            textBox0513.Text = "";//
            textBox0514.Text = "";//
            textBox0515.Text = "";//
            textBox0516.Text = "";//
            textBox0517.Text = "";//
            textBox0518.Text = "";//;

            textBox0600.Text = "";//
            textBox0601.Text = "";//
            textBox0602.Text = "";//
            textBox0603.Text = "";//
            textBox0604.Text = "";//
            textBox0605.Text = "";//
            textBox0606.Text = "";//
            textBox0607.Text = "";//
            textBox0608.Text = "";//
            textBox0609.Text = "";//
            textBox0610.Text = "";//
            textBox0611.Text = "";//
            textBox0612.Text = "";//
            textBox0613.Text = "";//
            textBox0614.Text = "";//
            textBox0615.Text = "";//
            textBox0616.Text = "";//
            textBox0617.Text = "";//
            textBox0618.Text = "";//;

            textBox0700.Text = "";//
            textBox0701.Text = "";//
            textBox0702.Text = "";//
            textBox0703.Text = "";//
            textBox0704.Text = "";//
            textBox0705.Text = "";//
            textBox0706.Text = "";//
            textBox0707.Text = "";//
            textBox0708.Text = "";//
            textBox0709.Text = "";//
            textBox0710.Text = "";//
            textBox0711.Text = "";//
            textBox0712.Text = "";//
            textBox0713.Text = "";//
            textBox0714.Text = "";//
            textBox0715.Text = "";//
            textBox0716.Text = "";//
            textBox0717.Text = "";//
            textBox0718.Text = "";//;

            textBox0800.Text = "";//
            textBox0801.Text = "";//
            textBox0802.Text = "";//
            textBox0803.Text = "";//
            textBox0804.Text = "";//
            textBox0805.Text = "";//
            textBox0806.Text = "";//
            textBox0807.Text = "";//
            textBox0808.Text = "";//
            textBox0809.Text = "";//
            textBox0810.Text = "";//
            textBox0811.Text = "";//
            textBox0812.Text = "";//
            textBox0813.Text = "";//
            textBox0814.Text = "";//
            textBox0815.Text = "";//
            textBox0816.Text = "";//
            textBox0817.Text = "";//
            textBox0818.Text = "";//;

            textBox0900.Text = "";//
            textBox0901.Text = "";//
            textBox0902.Text = "";//
            textBox0903.Text = "";//
            textBox0904.Text = "";//
            textBox0905.Text = "";//
            textBox0906.Text = "";//
            textBox0907.Text = "";//
            textBox0908.Text = "";//
            textBox0909.Text = "";//
            textBox0910.Text = "";//
            textBox0911.Text = "";//
            textBox0912.Text = "";//
            textBox0913.Text = "";//
            textBox0914.Text = "";//
            textBox0915.Text = "";//
            textBox0916.Text = "";//
            textBox0917.Text = "";//
            textBox0918.Text = "";//;


            textBox1000.Text = "";//
            textBox1001.Text = "";//
            textBox1002.Text = "";//
            textBox1003.Text = "";//
            textBox1004.Text = "";//
            textBox1005.Text = "";//
            textBox1006.Text = "";//
            textBox1007.Text = "";//
            textBox1008.Text = "";//
            textBox1009.Text = "";//
            textBox1010.Text = "";//
            textBox1011.Text = "";//
            textBox1012.Text = "";//
            textBox1013.Text = "";//
            textBox1014.Text = "";//
            textBox1015.Text = "";//
            textBox1016.Text = "";//
            textBox1017.Text = "";//
            textBox1018.Text = "";//;

            textBox1100.Text = "";//
            textBox1101.Text = "";//
            textBox1102.Text = "";//
            textBox1103.Text = "";//
            textBox1104.Text = "";//
            textBox1105.Text = "";//
            textBox1106.Text = "";//
            textBox1107.Text = "";//
            textBox1108.Text = "";//
            textBox1109.Text = "";//
            textBox1110.Text = "";//
            textBox1111.Text = "";//
            textBox1112.Text = "";//
            textBox1113.Text = "";//
            textBox1114.Text = "";//
            textBox1115.Text = "";//
            textBox1116.Text = "";//
            textBox1117.Text = "";//
            textBox1118.Text = "";//;


            textBox1200.Text = "";//
            textBox1201.Text = "";//
            textBox1202.Text = "";//
            textBox1203.Text = "";//
            textBox1204.Text = "";//
            textBox1205.Text = "";//
            textBox1206.Text = "";//
            textBox1207.Text = "";//
            textBox1208.Text = "";//
            textBox1209.Text = "";//
            textBox1210.Text = "";//
            textBox1211.Text = "";//
            textBox1212.Text = "";//
            textBox1213.Text = "";//
            textBox1214.Text = "";//
            textBox1215.Text = "";//
            textBox1216.Text = "";//
            textBox1217.Text = "";//
            textBox1218.Text = "";//;

            textBox1300.Text = "";//
            textBox1301.Text = "";//
            textBox1302.Text = "";//
            textBox1303.Text = "";//
            textBox1304.Text = "";//
            textBox1305.Text = "";//
            textBox1306.Text = "";//
            textBox1307.Text = "";//
            textBox1308.Text = "";//
            textBox1309.Text = "";//
            textBox1310.Text = "";//
            textBox1311.Text = "";//
            textBox1312.Text = "";//
            textBox1313.Text = "";//
            textBox1314.Text = "";//
            textBox1315.Text = "";//
            textBox1316.Text = "";//
            textBox1317.Text = "";//
            textBox1318.Text = "";//;

            textBox1400.Text = "";//
            textBox1401.Text = "";//
            textBox1402.Text = "";//
            textBox1403.Text = "";//
            textBox1404.Text = "";//
            textBox1405.Text = "";//
            textBox1406.Text = "";//
            textBox1407.Text = "";//
            textBox1408.Text = "";//
            textBox1409.Text = "";//
            textBox1410.Text = "";//
            textBox1411.Text = "";//
            textBox1412.Text = "";//
            textBox1413.Text = "";//
            textBox1414.Text = "";//
            textBox1415.Text = "";//
            textBox1416.Text = "";//
            textBox1417.Text = "";//
            textBox1418.Text = "";//;

            textBox1500.Text = "";//
            textBox1501.Text = "";//
            textBox1502.Text = "";//
            textBox1503.Text = "";//
            textBox1504.Text = "";//
            textBox1505.Text = "";//
            textBox1506.Text = "";//
            textBox1507.Text = "";//
            textBox1508.Text = "";//
            textBox1509.Text = "";//
            textBox1510.Text = "";//
            textBox1511.Text = "";//
            textBox1512.Text = "";//
            textBox1513.Text = "";//
            textBox1514.Text = "";//
            textBox1515.Text = "";//
            textBox1516.Text = "";//
            textBox1517.Text = "";//
            textBox1518.Text = "";//;
            
        }


        private void writepbomtorow(int gongxu,int rowindex,FGongxu list)
        {
            if (rowindex == 1)
            {
                textBox0500.Text = "1";
                if (gongxu==0)
                {
                    textBox0501.Text = "0";//
                }
                else if(gongxu==1)
                {
                    textBox0501.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox0501.Text = "2";//
                }
                
                textBox0502.Text = list.GXMC;//
                textBox0503.Text = "";//
                textBox0504.Text = list.GXNR;//
                textBox0505.Text = list.TSJGLJS;//
                textBox0506.Text = list.JGFS;//
                textBox0507.Text = list.ZZZS;//
                textBox0508.Text = list.JJSD;//
                textBox0509.Text = list.QXSD;//
                textBox0510.Text = list.JGYL;//
                textBox0511.Text = list.JCMC;//
                textBox0512.Text = list.JJMC;//
                textBox0513.Text = list.DJMC;//
                textBox0514.Text = list.DJZJ;//
                textBox0515.Text =list.WJ;//
                textBox0516.Text = list.NJ;//
                textBox0517.Text = list.GSDE;//
                textBox0518.Text = list.CXMC;//;
            }
            if (rowindex == 2)
            {
                textBox0600.Text = "2";
                if (gongxu == 0)
                {
                    textBox0601.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox0601.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox0601.Text = "2";//
                }
                if (textBox0502.Text != list.GXMC)
                {
                    textBox0602.Text = list.GXMC;
                }
                if (textBox0503.Text != list.GBH)
                {
                    textBox0603.Text = list.GBH; 
                }
                if (textBox0504.Text != list.GXNR)
                {
                    textBox0604.Text = list.GXNR; 
                }
                
                textBox0605.Text = list.TSJGLJS;//
               
                textBox0606.Text = list.JGFS;//
               
                textBox0607.Text = list.ZZZS;//

                textBox0608.Text = list.JJSD;//
                textBox0609.Text = list.QXSD;//
                textBox0610.Text = list.JGYL;//
                textBox0611.Text = list.JCMC;//
                textBox0612.Text = list.JJMC;//
                textBox0613.Text = list.DJMC;//
                textBox0614.Text = list.DJZJ;//
                textBox0615.Text = list.WJ;//
                textBox0616.Text = list.NJ;//
                textBox0617.Text = list.GSDE;//
                textBox0618.Text = list.CXMC;//;
            }
            if (rowindex == 3)
            {
                textBox0700.Text = "3";
                if (gongxu == 0)
                {
                    textBox0701.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox0701.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox0701.Text = "2";//
                }
                if (textBox0602.Text != list.GXMC)
                {
                    textBox0702.Text = list.GXMC;
                }
                if (textBox0603.Text != list.GBH)
                {
                    textBox0703.Text = list.GBH;
                }
                if (textBox0604.Text != list.GXNR)
                {
                    textBox0704.Text = list.GXNR; ;
                }
                textBox0705.Text = list.TSJGLJS;//
                textBox0706.Text = list.JGFS;//
                textBox0707.Text = list.ZZZS;//
                textBox0708.Text = list.JJSD;//
                textBox0709.Text = list.QXSD;//
                textBox0710.Text = list.JGYL;//
                textBox0711.Text = list.JCMC;//
                textBox0712.Text = list.JJMC;//
                textBox0713.Text = list.DJMC;//
                textBox0714.Text = list.DJZJ;//
                textBox0715.Text = list.WJ;//
                textBox0716.Text = list.NJ;//
                textBox0717.Text = list.GSDE;//
                textBox0718.Text = list.CXMC;//;
            } 
            if (rowindex == 4)
            {
                textBox0800.Text = "4";
                if (gongxu == 0)
                {
                    textBox0801.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox0801.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox0801.Text = "2";//
                }
                if (textBox0702.Text != list.GXMC)
                {
                    textBox0802.Text = list.GXMC;
                }
                if (textBox0703.Text != list.GBH)
                {
                    textBox0803.Text = list.GBH;
                }
                if (textBox0704.Text != list.GXNR)
                {
                    textBox0804.Text = list.GXNR; ;
                }
                textBox0805.Text = list.TSJGLJS;//
                textBox0806.Text = list.JGFS;//
                textBox0807.Text = list.ZZZS;//
                textBox0808.Text = list.JJSD;//
                textBox0809.Text = list.QXSD;//
                textBox0810.Text = list.JGYL;//
                textBox0811.Text = list.JCMC;//
                textBox0812.Text = list.JJMC;//
                textBox0813.Text = list.DJMC;//
                textBox0814.Text = list.DJZJ;//
                textBox0815.Text = list.WJ;//
                textBox0816.Text = list.NJ;//
                textBox0817.Text = list.GSDE;//
                textBox0818.Text = list.CXMC;//;
            } 
            if (rowindex == 5)
            {
                textBox0900.Text = "5";
                if (gongxu == 0)
                {
                    textBox0901.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox0901.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox0901.Text = "2";//
                }
                if (textBox0802.Text != list.GXMC)
                {
                    textBox0902.Text = list.GXMC;
                }
                if (textBox0803.Text != list.GBH)
                {
                    textBox0903.Text = list.GBH;
                }
                if (textBox0804.Text != list.GXNR)
                {
                    textBox0904.Text = list.GXNR; ;
                }
                textBox0905.Text = list.TSJGLJS;//
                textBox0906.Text = list.JGFS;//
                textBox0907.Text = list.ZZZS;//
                textBox0908.Text = list.JJSD;//
                textBox0909.Text = list.QXSD;//
                textBox0910.Text = list.JGYL;//
                textBox0911.Text = list.JCMC;//
                textBox0912.Text = list.JJMC;//
                textBox0913.Text = list.DJMC;//
                textBox0914.Text = list.DJZJ;//
                textBox0915.Text = list.WJ;//
                textBox0916.Text = list.NJ;//
                textBox0917.Text = list.GSDE;//
                textBox0918.Text = list.CXMC;//;
            } 
            if (rowindex == 6)
            {
                textBox1000.Text = "6";
                if (gongxu == 0)
                {
                    textBox1001.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox1001.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox1001.Text = "2";//
                }
                if (textBox0902.Text != list.GXMC)
                {
                    textBox1002.Text = list.GXMC;
                }
                if (textBox0903.Text != list.GBH)
                {
                    textBox1003.Text = list.GBH;
                }
                if (textBox0904.Text != list.GXNR)
                {
                    textBox1004.Text = list.GXNR; ;
                }
                textBox1005.Text = list.TSJGLJS;//
                textBox1006.Text = list.JGFS;//
                textBox1007.Text = list.ZZZS;//
                textBox1008.Text = list.JJSD;//
                textBox1009.Text = list.QXSD;//
                textBox1010.Text = list.JGYL;//
                textBox1011.Text = list.JCMC;//
                textBox1012.Text = list.JJMC;//
                textBox1013.Text = list.DJMC;//
                textBox1014.Text = list.DJZJ;//
                textBox1015.Text = list.WJ;//
                textBox1016.Text = list.NJ;//
                textBox1017.Text = list.GSDE;//
                textBox1018.Text = list.CXMC;//;
            }
            if (rowindex == 7)
            {
                textBox1100.Text = "7";
                if (gongxu == 0)
                {
                    textBox1101.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox1101.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox1101.Text = "2";//
                }
                if (textBox1002.Text != list.GXMC)
                {
                    textBox1102.Text = list.GXMC;
                }
                if (textBox1003.Text != list.GBH)
                {
                    textBox1103.Text = list.GBH;
                }
                if (textBox1004.Text != list.GXNR)
                {
                    textBox1104.Text = list.GXNR; ;
                }
                textBox1105.Text = list.TSJGLJS;//
                textBox1106.Text = list.JGFS;//
                textBox1107.Text = list.ZZZS;//
                textBox1108.Text = list.JJSD;//
                textBox1109.Text = list.QXSD;//
                textBox1110.Text = list.JGYL;//
                textBox1111.Text = list.JCMC;//
                textBox1112.Text = list.JJMC;//
                textBox1113.Text = list.DJMC;//
                textBox1114.Text = list.DJZJ;//
                textBox1115.Text = list.WJ;//
                textBox1116.Text = list.NJ;//
                textBox1117.Text = list.GSDE;//
                textBox1118.Text = list.CXMC;//;
            }
            if (rowindex == 8)
            {
                textBox1200.Text = "8";
                if (gongxu == 0)
                {
                    textBox1201.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox1201.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox1201.Text = "2";//
                }
                if (textBox1102.Text != list.GXMC)
                {
                    textBox1202.Text = list.GXMC;
                }
                if (textBox1103.Text != list.GBH)
                {
                    textBox1203.Text = list.GBH;
                }
                if (textBox1104.Text != list.GXNR)
                {
                    textBox1204.Text = list.GXNR; ;
                }
                textBox1205.Text = list.TSJGLJS;//
                textBox1206.Text = list.JGFS;//
                textBox1207.Text = list.ZZZS;//
                textBox1208.Text = list.JJSD;//
                textBox1209.Text = list.QXSD;//
                textBox1210.Text = list.JGYL;//
                textBox1211.Text = list.JCMC;//
                textBox1212.Text = list.JJMC;//
                textBox1213.Text = list.DJMC;//
                textBox1214.Text = list.DJZJ;//
                textBox1215.Text = list.WJ;//
                textBox1216.Text = list.NJ;//
                textBox1217.Text = list.GSDE;//
                textBox1218.Text = list.CXMC;//;
            } 
            if (rowindex == 9)
            {
                textBox1300.Text = "9";
                if (gongxu == 0)
                {
                    textBox1301.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox1301.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox0801.Text = "2";//
                }
                if (textBox1202.Text != list.GXMC)
                {
                    textBox1302.Text = list.GXMC;
                }
                if (textBox1203.Text != list.GBH)
                {
                    textBox1303.Text = list.GBH;
                }
                if (textBox1204.Text != list.GXNR)
                {
                    textBox1304.Text = list.GXNR; ;
                }
                textBox1305.Text = list.TSJGLJS;//
                textBox1306.Text = list.JGFS;//
                textBox1307.Text = list.ZZZS;//
                textBox1308.Text = list.JJSD;//
                textBox1309.Text = list.QXSD;//
                textBox1310.Text = list.JGYL;//
                textBox1311.Text = list.JCMC;//
                textBox1312.Text = list.JJMC;//
                textBox1313.Text = list.DJMC;//
                textBox1314.Text = list.DJZJ;//
                textBox1315.Text = list.WJ;//
                textBox1316.Text = list.NJ;//
                textBox1317.Text = list.GSDE;//
                textBox1318.Text = list.CXMC;//;
            }
            if (rowindex == 10)
            {
                textBox1400.Text = "10";
                if (gongxu == 0)
                {
                    textBox1401.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox1401.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox1401.Text = "2";//
                }
                if (textBox1302.Text != list.GXMC)
                {
                    textBox1402.Text = list.GXMC;
                }
                if (textBox1303.Text != list.GBH)
                {
                    textBox1303.Text = list.GBH;
                }
                if (textBox1304.Text != list.GXNR)
                {
                    textBox1404.Text = list.GXNR; ;
                }
                textBox1405.Text = list.TSJGLJS;//
                textBox1406.Text = list.JGFS;//
                textBox1407.Text = list.ZZZS;//
                textBox1408.Text = list.JJSD;//
                textBox1409.Text = list.QXSD;//
                textBox1410.Text = list.JGYL;//
                textBox1411.Text = list.JCMC;//
                textBox1412.Text = list.JJMC;//
                textBox1413.Text = list.DJMC;//
                textBox1414.Text = list.DJZJ;//
                textBox1415.Text = list.WJ;//
                textBox1416.Text = list.NJ;//
                textBox1417.Text = list.GSDE;//
                textBox1418.Text = list.CXMC;//;
            } 
            if (rowindex == 11)
            {
                textBox1400.Text = "11";
                if (gongxu == 0)
                {
                    textBox1501.Text = "";//
                }
                else if (gongxu == 1)
                {
                    textBox1501.Text = "1";//
                }
                else if (gongxu == 2)
                {
                    textBox1501.Text = "2";//
                }
                if (textBox1402.Text != list.GXMC)
                {
                    textBox1502.Text = list.GXMC;
                }
                if (textBox1403.Text != list.GBH)
                {
                    textBox1503.Text = list.GBH;
                }
                if (textBox1404.Text != list.GXNR)
                {
                    textBox1504.Text = list.GXNR; ;
                }
                textBox1505.Text = list.TSJGLJS;//
                textBox1506.Text = list.JGFS;//
                textBox1507.Text = list.ZZZS;//
                textBox1508.Text = list.JJSD;//
                textBox1509.Text = list.QXSD;//
                textBox1510.Text = list.JGYL;//
                textBox1511.Text = list.JCMC;//
                textBox1512.Text = list.JJMC;//
                textBox1513.Text = list.DJMC;//
                textBox1514.Text = list.DJZJ;//
                textBox1515.Text = list.WJ;//
                textBox1516.Text = list.NJ;//
                textBox1517.Text = list.GSDE;//
                textBox1518.Text = list.CXMC;//;
            }
        }
        
       

        ////private void toolStripMenuItem1_Click(object sender, EventArgs e)
        ////{

        ////    openFileDialog1.RestoreDirectory = true; //记忆上次浏览路径
        ////    openFileDialog1.Multiselect = true;
        ////    openFileDialog1.Title = ChangeLanguage.GetString("SelectFileTitle");
        ////    openFileDialog1.Filter = "Excel文件|";
        ////    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        ////    {
        ////        webBrowsere.Navigate(openFileDialog1.FileName) ;
        ////    }
        ////}

        private void BOMItemGenerate( )
        {
            //string ConnectionSql = "data source = localhost;initial catalog=BOM;usr id = sa;password = sa";
            //SqlConnection Connection1 = new SqlConnection ();
            //Connection1.ConnectionString = ConnectionSql;
 
            if (myConnection.State == ConnectionState.Closed)
            {
            
                try
                {
                    myConnection.Open();
                }
                catch
                {

                    MessageBox.Show("工艺数据库连接失败");
                    return;
                }
            }
            if (myConnection.State == ConnectionState.Open)
            {
                //string select = "Select [KEYID] ,[CVOLID],[TH,LJMC],[CL],[MTJS],[MPJS],[GXH],[AZ],[GBH],[GXNR],[TSJGLJS],[JGFS],[ZZZS],[JJSD],[QXSD],[JGY]L,[JCMC],[JJMC],[DJMC],[DJZJ],[WJ],[NJ],[GSDE],[CXMC] form BOM_INFORMATION";
                
                myCommand.CommandText="Select * FROM dbo.BOM_INFORMATION";
               // com.Connection = Connection1;
                //com.CommandType = CommandType.Text;
               // com.CommandText="BOM_INFORMATION";

                try
                {
                    myReader = myCommand.ExecuteReader();
                    // DataTable  schema = myReader.GetSchemaTable();
                    //遍历每一行数据
                    while (myReader.Read())
                    {
                        object temp = myReader.GetValue(2);
                        string str = temp.ToString();
                        if (str == "PBOM")
                        {
                            object temp1 =  myReader.GetValue(5) ;
                            object temp2 = myReader.GetValue(7);
                            string filename = temp1.ToString() + temp2.ToString();//图号+零件名称
                            bool hassameitem = false;
                            for (int i = 0; i < comboBoxchoose.Items.Count; i++)
                            {
                                if (filename == comboBoxchoose.Items[i].ToString())
                                {
                                    hassameitem = true;
                                }
                            }
                            if (!hassameitem)
                            {
                                comboBoxchoose.Items.Add(filename);
                            }
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
           
            return;
            
        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (myConnection.State == ConnectionState.Closed)
                {
                   myConnection.Open() ;
                }
                //获取父节点
                myCommand.CommandText = "Select * from dbo.FBOMList ";

                 myReader = myCommand.ExecuteReader();
                int fuidnumber = 0;
                int ziidnumber = 0;
                int jj = 0;
                int kk=0;
                while (myReader.Read())
                {
                    object temp = myReader.GetValue(3);
                    string ss = temp.ToString();
                    if (  ss == "0")
                    { fuidnumber++;
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
                for(int i =0;i<bomlist.Count();i++)
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
                for (kk = 0; kk < bomlist.Count();kk++ )
                {

                    if (bomlist[kk].PRNTNODEID == "0"&bomlist[kk].BOMTYPE == "EBOM")
                    {
                        string Nodename = bomlist[kk].TH+"  " +bomlist[kk].LJMC;//图号+名称
                        TreeNode treeNodefu = new System.Windows.Forms.TreeNode(Nodename);
                        for (jj = 0; jj < bomlist.Count(); jj++)
                        {
                            if (bomlist[jj].PRNTNODEID == bomlist[kk].NODEID && bomlist[jj].BOMTYPE=="EBOM")
                            {
                                string Nodename1 = bomlist[jj].TH + "  " + bomlist[jj].LJMC;//图号+名称
                                TreeNode treeNodefu1 = new System.Windows.Forms.TreeNode(Nodename1);
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
                if (myReader!=null)
                {
                    myReader.Close();
                }
                if(myConnection!=null)
                {
                    myConnection.Close();
                }
                MessageBox.Show("工艺数据获取失败");
                return;
            }
        }

       

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
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
                int index = nodename.IndexOf(' ');
                string tuhao = nodename.Substring(0, index);
                try
                {
                    if (myConnection.State == ConnectionState.Closed)
                    {
                        myConnection.Open();
                    }
                    //获取父节点
                    myCommand.CommandText = "Select * from FBOMList  ";

                    myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {

                        object temp1 = myReader.GetValue(5);
                        object temp2 = myReader.GetValue(2);
                        string ss = temp1.ToString();
                        string type = temp2.ToString();
                        if (ss == tuhao&&type=="EBOM")
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

        private void button1_Click_2(object sender, EventArgs e)
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

                myCommand.CommandText = "Select * from FBOMList  ";
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    //string ss = myReader.GetString(0);
                    //bomlist[jj].BOMTYPE = myReader.GetString(2);
                    //bomlist[jj].PRNTNODEID = myReader.GetString(3);
                    //bomlist[jj].NODEID = myReader.GetString(4);
                    //bomlist[jj].TH = myReader.GetString(5);
                    //bomlist[jj].CSUFFIX = myReader.GetString(6);
                    //bomlist[jj].LJMC = myReader.GetString(7);
                    //bomlist[jj].CRIV = myReader.GetString(8);
                    //bomlist[jj].CNUMBER = myReader.GetString(9);
                    //bomlist[jj].CL = myReader.GetString(10);
                    //bomlist[jj].GG = myReader.GetString(11);
                    //bomlist[jj].XM = myReader.GetString(12);
                    //bomlist[jj].SB = myReader.GetString(13);
                    //bomlist[jj].XMU = myReader.GetString(14);
                    //bomlist[jj].TZBL = myReader.GetString(15);
                    //bomlist[jj].BZ = myReader.GetString(16);
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
                for (kk = 0; kk < bomlist.Count(); kk++)
                {
                    if (bomlist[kk].PRNTNODEID == "0"&&bomlist[kk].BOMTYPE == "PBOM")
                    {
                        string Nodename = bomlist[kk].TH + "  " + bomlist[kk].LJMC;//图号+名称
                        TreeNode treeNodefu = new System.Windows.Forms.TreeNode(Nodename);
                        for (jj = 0; jj < bomlist.Count(); jj++)
                        {
                            if (bomlist[jj].PRNTNODEID == bomlist[kk].NODEID && bomlist[jj].BOMTYPE == "PBOM")
                            {
                                string Nodename1 = bomlist[jj].TH + "  " + bomlist[jj].LJMC;//图号+名称
                                TreeNode treeNodefu1 = new System.Windows.Forms.TreeNode(Nodename1);
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

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null /*&& e.Node.FirstNode != null*/)
            {
                string nodename = e.Node.Text;

                if (nodename == null)
                {
                    return;
                }
                if (nodename.Length < 12)
                {
                    return;
                }
                int index = nodename.IndexOf(' ');
                string tuhao = nodename.Substring(0, index);
                try
                {
                    if (myConnection.State == ConnectionState.Closed)
                    {
                        myConnection.Open();
                    }
                    //获取父节点
                    myCommand.CommandText = "Select * from FBOMList  ";

                    myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        object temp1 = myReader.GetValue(5);
                        object temp2 = myReader.GetValue(2);
                        string ss = temp1.ToString();
                        string type = temp2.ToString();
                        if (ss == tuhao&&type =="PBOM")
                        {
                            //FBOMList bomlist = new FBOMList();

                            //bomlist.KEYID = myReader.GetString(0);
                            //bomlist.KEYID = myReader.GetString(0);
                            //bomlist.CVOLID = myReader.GetString(1);
                            //bomlist.BOMTYPE = myReader.GetString(2);
                            //bomlist.PRNTNODEID = myReader.GetString(3);
                            //bomlist.NODEID = myReader.GetString(4);
                            //bomlist.TH = myReader.GetString(5);
                            //bomlist.CSUFFIX = myReader.GetString(6);
                            //bomlist.LJMC = myReader.GetString(7);
                            //bomlist.CRIV = myReader.GetString(8);
                            //bomlist.CNUMBER = myReader.GetString(9);
                            //bomlist.CL = myReader.GetString(10);
                            //bomlist.GG = myReader.GetString(11);
                            //bomlist.XM = myReader.GetString(12);
                            //bomlist.SB = myReader.GetString(13);
                            //bomlist.XMU = myReader.GetString(14);
                            //bomlist.TZBL = myReader.GetString(15);
                            //bomlist.BZ = myReader.GetString(16);
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
                            textBoxbomtype1.Text = bomlist.BOMTYPE;
                            textBoxhouzhui1.Text = bomlist.CSUFFIX;
                            textBoxmingcheng1.Text = bomlist.LJMC;
                            textBoxbanbenhao1.Text = bomlist.CSUFFIX; 
                            textBoxtuhao1.Text = bomlist.CRIV; ;
                            textBoxxingming1.Text = bomlist.XM;
                            textBoxshebei1.Text = bomlist.SB;
                            textBoxxiangmu1.Text = bomlist.XMU;
                            textBoxbili1.Text = bomlist.TZBL;
                            textBoxshuliang1.Text = bomlist.CNUMBER;
                            textBoxtuhao1.Text = bomlist.TH;
                            textBoxcaizhi1.Text = bomlist.CL;
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

        private void comboBoxchoose_Click(object sender, EventArgs e)
        {
            BOMItemGenerate();

        }


     

       

        

        //private void BOMForm_SizeChanged(object sender, EventArgs e)
        //{
        //   //otosize.controlAutoSize(this);
        //}

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number");
                }
                else
                    MessageBox.Show("请输入数字");
                textBox1.Focus();
            }
        }
        private bool InterOrdertodataFile(string path, int no, string fun1, string fun2)
        {

            try
            {
                FileStream rFile = new FileStream(path, FileMode.Open);
                StreamReader rsr = new StreamReader(rFile);
                int ii = 0;
                string orderdatas = "";
                string items = "";
                string magnos = "";
                string fun1s = "";
                string fun2s = "";
                string funs = "";
                string steps = "";
                string downs = "";
                string tops = "";
                string states = "";
                string line;
                line = rsr.ReadLine();

                while (line != null && line != "")
                {
                    ii++;

                    line = rsr.ReadLine();
                }
                items = (ii + 1).ToString();
                fun1s = fun1;
                fun2s = fun2;
                magnos = no.ToString();
                if (language == "English")
                {
                    steps = "Aoto";
                    downs = "OK";
                    tops = "Top";
                    if (fun1s == "None")
                    {
                        states = "None;Notstart;";
                    }
                    else if (fun2s == "None")
                    {

                        states = "Notstart;None;";
                    }
                    else
                    {
                        states = "Notstart;Notstart;";
                    }
                }
                else
                {
                    steps = "自动";
                    downs = "确定";
                    tops = "置顶";
                    if (fun1s == "无")
                    {
                        states = "无;未开始;";

                        funs = "工序二";
                        MainForm.magprocesss1tate[no - 1] = (int)OrderForm1.Processstate.None;

                        MainForm.magprocesss2tate[no - 1] = (int)OrderForm1.Processstate.Notstart;
                    }
                    else if (fun2s == "无")
                    {

                        states = "未开始;无;";

                        funs = "工序一";
                        MainForm.magprocesss2tate[no - 1] = (int)OrderForm1.Processstate.None;

                        MainForm.magprocesss1tate[no - 1] = (int)OrderForm1.Processstate.Notstart;
                    }
                    else
                    {
                        states = "未开始;未开始;";

                        funs = "工序一";
                        MainForm.magprocesss1tate[no - 1] = (int)OrderForm1.Processstate.Notstart;

                        MainForm.magprocesss2tate[no - 1] = (int)OrderForm1.Processstate.Notstart;
                    }
                }
                MainForm.ordertate[no - 1] = (int)OrderForm1.Orderstate.Notstart;

                MainForm.Mag_Check[no - 1] = 0;//测量结果初始化为0
                MainForm.magisordered[no - 1] = 1;
                orderdatas = "item=" + items;
                orderdatas = orderdatas + ",magno=" + magnos;
                orderdatas = orderdatas + ",fun1=" + fun1s;
                orderdatas = orderdatas + ",fun2=" + fun2s;
                orderdatas = orderdatas + ",fun=" + funs;
                orderdatas = orderdatas + ",step=" + steps;
                orderdatas = orderdatas + ",down=" + downs;
                orderdatas = orderdatas + ",top=" + tops;
                orderdatas = orderdatas + ",state=" + states;

                rsr.Close();
                rFile.Close();
                if (ii > 0)
                {
                    FileStream wFile = new FileStream(path, FileMode.Append);
                    StreamWriter wsr = new StreamWriter(wFile);
                    wsr.WriteLine(orderdatas);

                    wsr.Close();
                    wFile.Close(); ;
                }
                else
                {
                    FileStream wFile = new FileStream(path, FileMode.Create);
                    StreamWriter wsr = new StreamWriter(wFile);
                    wsr.WriteLine(orderdatas);

                    wsr.Close();
                    wFile.Close();
                }
                return true;
            }
            catch (IOException e)
            {
                return false;
            }

        }

        private bool InterOrdertostateFile(string path, int no, string fun1, string fun2)
        {
            try
            {
                FileStream rFile = new FileStream(path, FileMode.Open);
                StreamReader rsr = new StreamReader(rFile);
                int ii = 0;
                string orderdatas = "";
                string items = "";
                string magnos = "";
                string fun1s = "";
                string fun2s = "";
                string states = "";
                string checks = "";
                string line;
                line = rsr.ReadLine();

                while (line != null && line != "")
                {
                    ii++;

                    line = rsr.ReadLine();
                }
                items = (ii + 1).ToString();

                magnos = no.ToString();
                if (language == "English")
                {
                    if (fun1 == "None")
                    {
                        fun1s = "None";
                    }
                    else
                    {
                        fun1s = "Notstart";
                    }
                    if (fun2 == "None")
                    {
                        fun2s = "None";
                    }
                    else
                    {
                        fun2s = "Notstart";
                    }
                    states = "Notstart";
                    checks = "None";
                }
                else
                {
                    if (fun1 == "无")
                    {
                        fun1s = "无";
                    }
                    else
                    {
                        fun1s = "未开始";
                    }
                    if (fun2 == "无")
                    {
                        fun2s = "无";
                    }
                    else
                    {
                        fun2s = "未开始";
                    }
                    states = "未开始";
                    checks = "None";

                }
                MainForm.ordertate[no - 1] = (int)OrderForm1.Orderstate.Notstart;
                orderdatas = "item=" + items;
                orderdatas = orderdatas + ",magno=" + magnos;
                orderdatas = orderdatas + ",fun1=" + fun1s;
                orderdatas = orderdatas + ",fun2=" + fun2s;
                orderdatas = orderdatas + ",state=" + states;
                orderdatas = orderdatas + ",check=" + checks;
                orderdatas = orderdatas + ";";

                rsr.Close();
                rFile.Close();
                if(ii>0)
                {
                    FileStream wFile = new FileStream(path, FileMode.Append);
                    StreamWriter wsr = new StreamWriter(wFile);
                    wsr.WriteLine(orderdatas);

                    wsr.Close();
                    wFile.Close();
                    return true;
                }
                else
                {
                    FileStream wFile = new FileStream(path, FileMode.Create);
                    StreamWriter wsr = new StreamWriter(wFile);
                    wsr.WriteLine(orderdatas);

                    wsr.Close();
                    wFile.Close();
                    return true;
                }
            }
            catch (IOException e)
            {
                return false;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            if (MagNoStr == "")
            {
                return;
            }
            try
            {
                int MagNo = Convert.ToInt32(MagNoStr);
                if (MagNo > 30 || MagNo <= 0)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("Please enter number between 1 to 30");
                    }
                    else
                        MessageBox.Show("请输入1-30之间的数字");
                    textBox1.Focus();
                }
                else
                {

                }
            }
            catch
            {
                if (language == "English")
                {
                    MessageBox.Show("Please enter number between 1 to 30");
                }
                else
                    MessageBox.Show("请输入1-30之间的数字");
                textBox1.Focus();
            }
        }

        private void buttonmadeorder_Click(object sender, EventArgs e)
        {
            string pbomname = labeltype1.Text;
            if (pbomname != "A" && pbomname != "B" && pbomname != "C" && pbomname != "D")
            {
                if (language == "English")
                {
                    MessageBox.Show("Please download the process sheet！");
                }
                else
                    MessageBox.Show("请加载工艺文件！");
                return;
            }
            string MagNoStr = textBox1.Text;
            int MagNo1 = 0;

            if (MagNoStr == "")
            {
                if (language == "English")
                {
                    MessageBox.Show("The Mag is null");
                }
                else
                    MessageBox.Show("料仓编号为空，不能生成订单");
                return;
            }

            try
            {
                MagNo1 = Convert.ToInt16(MagNoStr);
            }
            catch
            {
                if (language == "English")
                {
                    MessageBox.Show("The Mag number is err");
                }
                else
                    MessageBox.Show("料仓编号错误");
                return;
            }

            if (pbomname == "A")
            {
                if (MagNo1 > 12)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The position for Current material must be betwen 1 and 12 ");
                    }
                    else
                        MessageBox.Show("当前类型物料仓位位于1—12之间");
                    return;
                }
            }
            if (pbomname == "B")
            {
                if (MagNo1 < 12 || MagNo1 > 24)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The position for Current material must be  betwen 12 and 24");
                    }
                    else
                        MessageBox.Show("当前类型物料仓位位于12—24之间");
                    return;
                }
            }
            if (pbomname == "C")
            {
                if (MagNo1 < 25 || MagNo1 > 27)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The position for Current material must be betwen 25 and 27");
                    }
                    else
                        MessageBox.Show("当前类型物料仓位位于25—27之间");
                    return;
                }
            }
            if (pbomname == "D")
            {
                if (MagNo1 < 28 || MagNo1 > 30)
                {
                    if (language == "English")
                    {
                        MessageBox.Show("The position for Current material must be  betwen 28 and 30 ");
                    }
                    else
                        MessageBox.Show("当前类型物料仓位位于28—30之间");
                    return;
                }
            }
            string fun1string = comboBoxFun1.Text;
            // int fun1no = Convert.ToInt16(fun1string);
            string fun2string = comboBoxFun2.Text;
            // int fun2no = Convert.ToInt16(fun2string);

            int maglength = (int)ModbusTcp.MagLength;

            if (fun1string == fun2string)
            {
                MessageBox.Show("工序相同，不能生成订单");
                return;
            }

            if (fun1string == "None"
                && fun2string == "None")
            {
                MessageBox.Show("The processes is null");
                return;
            }
            //查询是否已经存在
            if (MainForm.magisordered[MagNo1 - 1] == 1)
            {
                if (language == "English")
                {
                    MessageBox.Show("The Mag have been chosed already,please choose another");
                }
                else
                    MessageBox.Show("当前仓位已经绑定工艺，请选择其他料仓");
                return;
            }
            bool ret1 = InterOrdertodataFile(OrderForm1.OrderdataFilePath, MagNo1, comboBoxFun1.Text, comboBoxFun2.Text);

            bool ret2 = InterOrdertostateFile(OrderForm1.OrderstateFilePath, MagNo1, comboBoxFun1.Text, comboBoxFun2.Text);
            MainForm.Orderinsertflage = true;
            if (ret1 && ret2)
            {
                if (language == "English")
                {
                    MessageBox.Show("The order is made");
                }
                else
                    MessageBox.Show("当前订单生成成功");
                return;
            }
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
                CAPPPprocess = new Process();
                CAPPPprocess.StartInfo.FileName = "C:\\KMSOFT\\KMCAPP\\kmcapp.exe";
                //CAPPPprocess.StartInfo.FileName = "C:\\Program Files (x86)\\FormatFactory\\FormatFactory.exe";
                CAPPPprocess.Start();
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
                //PDMPprocess.StandardInput.WriteLine("C:\\Program Files (x86)\\证照之星5.0");
                PDMPprocess.StandardInput.WriteLine("pdm.exe");
               

            }
            //if (PDMPprocess == null)
            //{
            //    PDMPprocess = new Process();
            //    PDMPprocess.StartInfo.FileName = "C:\\KMSOFT\\KMPDM\\pdm.exe";

            //    PDMPprocess.Start();
            //}
            else
            {
                if (PDMPprocess.HasExited)
                {
                    PDMPprocess.Start();
                    PDMPprocess.StandardInput.WriteLine("C:\\KMSOFT\\KMPDM");
                    PDMPprocess.StandardInput.WriteLine("pdm.exe");
                    //PDMPprocess.StandardInput.WriteLine("C:\\Program Files (x86)\\证照之星5.0");
                    //PDMPprocess.StandardInput.WriteLine("ZZZX.exe");
                }
                else
                {
                    PDMPprocess.StandardInput.WriteLine("pdm.exe");
                    //PDMPprocess.StandardInput.WriteLine("ZZZX.exe");
                }

            }
        }

        private void labelchoose_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxchoose_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

   

      


        

       
    }
}
