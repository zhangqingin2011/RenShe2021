using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using Sygole.HFReader;
using HFAPItest;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Collections;
using SCADA.WindowsForm;
using HNC.MOHRSS.Model;
using HNC.MOHRSS.Common;

namespace SCADA
{
    public partial class SetForm : Form
    {
        public string language = "";
        private enum fangwei : int
        {
            left = 1,
            right,
            top,
            down,

        };
        private UserIpset userIpset1;
        private UserIpset userIpset2;
        private UserIpset userIpset3;
        private UserIpset userIpset4;
        private UserIpset userIpset5;
        private UserIpset userIpset6;
        private UserIpset userIpset7;
        private UserIpset userIpset8;

        

        #region   定义窗体类需要的成员和构造函数
        //调整窗体类

        //  AutoSizeFormClass asc = new AutoSizeFormClass();
        public class Dgv_Xml_PathType
        {
            public DataGridView Dgv;
            public string PathStr;
            public bool DgvDatChange;
        }

        //窗体切换时数据是否更改的标志
        List<Dgv_Xml_PathType> DgvLis = new List<Dgv_Xml_PathType>();
        //保持标记
        bool DGVRowsAddFlag = false;
        //         public static IntPtr setFormPtr;
        List<Binding> m_bindingList = new List<Binding>();
        int dgvPLCSet_CurrentRow_Index_Old = -1;
        int dgvRobotSet_CurrentRow_Index_Old = -1;
        String[] UserPasswordStrArr = { "登陆", "注销", "取消", "修改密码", "确定" };
        String PXMessage = "你的操作权限不够！";
        String LogInMessage = "用户未登陆！";
        private System.EventHandler<String> MsgTipsHandler;

        //2017.4.25
        public static string[] ip = new string[9];//读写器Ip
        public static string[] port = new string[9];//读写器端口
        public static byte[] ReaderID = new byte[9];//读写器ID
     //   public static HFReader SGreader = new HFReader();

        //ResourceManager m_resource = new ResourceManager(typeof(SetForm));
        private string prelanguage; //记录切换语言之前的语言
        Hashtable m_Hashtable;
        MessageForm message;


        public static String ONLINE = "..\\picture\\top_bar_green.png";
        public static String OFFLINE = "..\\picture\\top_bar_black.png";

        Point pointnode1 = new Point();
        Point pointnode2 = new Point();
        Point pointnode3 = new Point();
        Point pointnode4 = new Point();
        Point pointnode5 = new Point();
        Point pointnode6 = new Point();
        Point pointnode7 = new Point();
        Point pointnode8 = new Point();
        Point pointnode9 = new Point();
        Point pointnode10 = new Point();
        Point pointnode11 = new Point();
        Point pointnode12 = new Point();
        Point pointnode13right = new Point();
        Point pointnode13left = new Point();
        Point pointnode13top = new Point();
        Point pointnode13bottom = new Point();
        enum UserPasswordStrArrIndex
        {
            登陆 = 0,
            注销,
            取消,
            修改密码,
            确定
        }
        //构造函数初始化界面
        public SetForm()
        {
            InitializeComponent();


            colDataSet.Items.Add("HNC_818A");
            colDataSet.Items.Add("HNC_818B");
            colDataSet.Items.Add("HNC_848B");
            colDataSet.Items.Add("HNC_848C");
            Dgv_Xml_PathType m_DgvLisNode = new Dgv_Xml_PathType();
            m_DgvLisNode.Dgv = dgvCNCSet;
            m_DgvLisNode.PathStr = m_xmlDociment.PathRoot_CNC;
            m_DgvLisNode.DgvDatChange = false;
            DgvLis.Add(m_DgvLisNode);

            m_DgvLisNode = new Dgv_Xml_PathType();
            m_DgvLisNode.Dgv = dgvRobotSet;
            m_DgvLisNode.PathStr = m_xmlDociment.PathRoot_ROBOT;
            m_DgvLisNode.DgvDatChange = false;
            DgvLis.Add(m_DgvLisNode);

            //m_DgvLisNode = new Dgv_Xml_PathType();
            //m_DgvLisNode.Dgv = DGVRobotInputSignalS;
            //m_DgvLisNode.PathStr = m_xmlDociment.PathRoot_ROBOT_Item + "0" + "/" + m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.X];
            //m_DgvLisNode.DgvDatChange = false;
            //DgvLis.Add(m_DgvLisNode);

            //m_DgvLisNode = new Dgv_Xml_PathType();
            //m_DgvLisNode.Dgv = DGVRobotOutputSignalS;
            //m_DgvLisNode.PathStr = m_xmlDociment.PathRoot_ROBOT_Item + "0" + "/" + m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.Y];
            //m_DgvLisNode.DgvDatChange = false;
            //DgvLis.Add(m_DgvLisNode);

            m_DgvLisNode = new Dgv_Xml_PathType();
            m_DgvLisNode.Dgv = dgvRFIDSet;
            m_DgvLisNode.PathStr = m_xmlDociment.PathRoot_RFID;
            m_DgvLisNode.DgvDatChange = false;
            DgvLis.Add(m_DgvLisNode);

            m_DgvLisNode = new Dgv_Xml_PathType();
            m_DgvLisNode.Dgv = dgvMeasureSet;
            m_DgvLisNode.PathStr = m_xmlDociment.PathRoot_Measure;
            m_DgvLisNode.DgvDatChange = false;
            DgvLis.Add(m_DgvLisNode);

            Column6.Items.Add("HNC8");
            Column6.Items.Add("MITSUBISHI");
            Column6.Items.Add("SIEMENS");
            m_DgvLisNode = new Dgv_Xml_PathType();
            m_DgvLisNode.Dgv = dgvPLCSet;
            m_DgvLisNode.PathStr = m_xmlDociment.PathRoot_PLC;
            m_DgvLisNode.DgvDatChange = false;
            DgvLis.Add(m_DgvLisNode);


            Binding m_Binding = new Binding("Text", dgvCNCSet, "RowCount", false);
            m_Binding.Format += new ConvertEventHandler(OnCountryFromFormat);
            m_Binding.Parse += new ConvertEventHandler(OnCountryFromParse);
            textBox_CNCNUM.DataBindings.Add(m_Binding);
            m_bindingList.Add(m_Binding);

            m_Binding = new Binding("Text", dgvRobotSet, "RowCount", false);
            m_Binding.Format += new ConvertEventHandler(OnCountryFromFormat);
            m_Binding.Parse += new ConvertEventHandler(OnCountryFromParse);
            textBox_ROBOTNum.DataBindings.Add(m_Binding);
            m_bindingList.Add(m_Binding);

            //m_Binding = new Binding("Text", DGVRobotInputSignalS, "RowCount", false);
            //m_Binding.Format += new ConvertEventHandler(OnCountryFromFormat);
            //m_Binding.Parse += new ConvertEventHandler(OnCountryFromParse);
            //textBox_SeleRobotIntSNum.DataBindings.Add(m_Binding);
            //m_bindingList.Add(m_Binding);


            //m_Binding = new Binding("Text", DGVRobotOutputSignalS, "RowCount", false);
            //m_Binding.Format += new ConvertEventHandler(OnCountryFromFormat);
            //m_Binding.Parse += new ConvertEventHandler(OnCountryFromParse);
            //textBox_SeleRobotOutSNum.DataBindings.Add(m_Binding);
            //m_bindingList.Add(m_Binding);

            m_Binding = new Binding("Text", dgvRFIDSet, "RowCount", false);
            m_Binding.Format += new ConvertEventHandler(OnCountryFromFormat);
            m_Binding.Parse += new ConvertEventHandler(OnCountryFromParse);
            textBox_RFidDgvRowsNum.DataBindings.Add(m_Binding);
            m_bindingList.Add(m_Binding);

            m_Binding = new Binding("Text", dgvMeasureSet, "RowCount", false);
            m_Binding.Format += new ConvertEventHandler(OnCountryFromFormat);
            m_Binding.Parse += new ConvertEventHandler(OnCountryFromParse);
            textBox_MeasureDgvRowsNum.DataBindings.Add(m_Binding);
            m_bindingList.Add(m_Binding);

            m_Binding = new Binding("Text", dgvPLCSet, "RowCount", false);
            m_Binding.Format += new ConvertEventHandler(OnCountryFromFormat);
            m_Binding.Parse += new ConvertEventHandler(OnCountryFromParse);
            textBox_PLCDgvRowsNum.DataBindings.Add(m_Binding);
            m_bindingList.Add(m_Binding);

            label_Tisp.Text = "";
            label_Tisp.ForeColor = Color.Red;
            label_ChangeUserTips.Text = "";
            label_ChangeUserTips.ForeColor = Color.Red;
            comboBox_UserName.DataSource = MainForm.m_xml.GetUserNameStrArr();
            if (comboBox_UserName.Items.Count > 0)
            {
                comboBox_UserName.SelectedIndex = 0;
                label_CurrentUsername.Text = comboBox_UserName.Text;
                button_UserOnOrOff.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.注销];//注销
                button_ChangeUserPassword.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.修改密码];//修改密码
            }

            tabControlSet.TabPages.Remove(tabPageUserManager);  //隐藏用户管理界面
            tabControlSet.TabPages.Remove(tabPageRobotSet);  //隐藏用户管理界面
            tabControlSet.TabPages.Remove(tabPageRFIDSet);
            tabControlSet.TabPages.Remove(tabPage2);
            tabControlSet.TabPages.Remove(tabPage1);
            tabControlSet.TabPages.Remove(tabPageNetset);
            SetUserPasswoedItemShowOrHide();
            //LogIn = true;   //打开操作权限
            LogIn = true;   //打开操作权限
            MsgTipsHandler = new System.EventHandler<string>(this.MsgTipsHandlerFuc);
        }
        #endregion

        #region   数量改变响应入口
        void OnCountryFromFormat(object sender, ConvertEventArgs e)
        {
            try
            {
                DataGridView DGV = (DataGridView)(((Binding)sender).DataSource);
                int ii = int.Parse(e.Value.ToString());
                int conut_min = 1;
                //if (DGV == PLCInputSignalSDefine || DGV == PLCOutputSignalSDefine)//PLC寄存器监控可以为0
                //{
                //    conut_min = 0;
                //}

                if (e.Value == null || e.Value == DBNull.Value)
                {
                    return;
                }
                else if (ii > conut_min)
                {
                    if (DGV.RowCount == ii && DGVRowsAddFlag)
                    {
                        for (int jj = 0; jj < ii; jj++)
                        {
                            if (DGV.Rows[jj].Cells[0].Value == null || DGV.Rows[jj].Cells[0].Value.ToString() == "")
                            {
                                if (DGV.ColumnCount == m_xmlDociment.Default_Attributesstr1_value.Length)
                                {
                                    if (tabControlSet.SelectedIndex == 1)
                                    {
                                        DGV.Rows[jj].SetValues(m_xmlDociment.Default_Attributesstr1_value);
                                    }
                                    if (tabControlSet.SelectedIndex == 2)
                                    {
                                        DGV.Rows[jj].SetValues(m_xmlDociment.Default_Attributesstr1_value_robot);
                                    }
                                    if (tabControlSet.SelectedIndex == 3)
                                    {

                                        DGV.Rows[jj].SetValues(m_xmlDociment.Default_Attributesstr1_value_plc);
                                    }
                                }
                                else if (DGV.ColumnCount == m_xmlDociment.Default_Attributesstr2_value.Length)
                                {
                                    DGV.Rows[jj].SetValues(m_xmlDociment.Default_Attributesstr2_value);
                                }
                                else if (DGV.ColumnCount == m_xmlDociment.Default_Attributes_RFID_value.Length)
                                {
                                    DGV.Rows[jj].SetValues(m_xmlDociment.Default_Attributes_RFID_value);
                                }
                                else if (DGV.ColumnCount == m_xmlDociment.Default_Attributesstr3_value.Length)
                                {
                                    DGV.Rows[jj].SetValues(m_xmlDociment.Default_Attributesstr3_value);
                                }
                                DGV.Rows[jj].Cells[0].Value = jj.ToString();
                            }
                        }
                        DGVRowsAddFlag = false;
                    }
                    e.Value = ii.ToString();
                }
                else
                {
                    e.Value = conut_min.ToString();//"1";
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        void OnCountryFromParse(object sender, ConvertEventArgs e)
        {
            int ii = 0;
            try
            {
                DataGridView DGV = (DataGridView)(((Binding)sender).DataSource);
                ii = int.Parse(e.Value.ToString());
                if (ii >= 0)
                {
                    if (ii == 0)
                    {
                        //if (DGV != PLCInputSignalSDefine && DGV != PLCOutputSignalSDefine)//PLC寄存器监控也许为0
                        //{
                        //    ii++;
                        //}
                    }
                    if (DGV.RowCount < ii)//
                    {
                        DGVRowsAddFlag = true;
                    }
                    else
                    {
                        DGVRowsAddFlag = false;
                    }
                    if (DGV.RowCount != ii)//数据被改变
                    {
                        RefreshDgvDataChangeF(DGV);
                    }
                    e.Value = ii.ToString();
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        #endregion

        #region DataGridView 复制粘贴保存快捷键
        private void DataGirdViewCopy(DataGridView DGV)
        {
            try
            {
                if (DGV.GetCellCount(DataGridViewElementStates.Selected) > 0)
                {
                    Clipboard.SetDataObject(DGV.GetClipboardContent());
                }
            }
            catch
            {
                // 不处理
            }
        }
        private bool DataGirdViewPaste(DataGridView DGV)
        {
            bool 是否只有一个单元格 = true;
            try
            {
                int RowIndex = DGV.CurrentCell.RowIndex;
                int ColumnIndex = DGV.CurrentCell.ColumnIndex;

                // 获取剪切板的内容，并按行分割
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return 是否只有一个单元格;
                pasteText = pasteText.Replace('\r', '\0');
                string[] lines = pasteText.Split('\n');
                for (int jj = RowIndex; jj < RowIndex + lines.Length; jj++)
                {
                    if (jj >= DGV.Rows.Count)
                    {
                        break;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(lines[jj - RowIndex].Trim()))
                            continue;
                        string[] vals = lines[jj - RowIndex].Split('\t');// 按 Tab 分割数据
                        for (int ii = ColumnIndex; ii < vals.Length + ColumnIndex; ii++)
                        {
                            if (DGV.ColumnCount <= ii)
                            {
                                break;
                            }
                            else
                            {
                                DGV.Rows[jj].Cells[ii].Value = vals[ii - ColumnIndex].Trim("\0".ToCharArray());
                            }
                        }
                        if (vals.Length > 1)
                        {
                            是否只有一个单元格 = false;
                        }
                    }
                }
                if (lines.Length > 1)
                {
                    是否只有一个单元格 = false;
                }
            }
            catch
            {
                // 不处理
            }
            return 是否只有一个单元格;
        }


        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto,
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control focusedControl = null;
            IntPtr focusedHandle = GetFocus();
            if (focusedHandle != IntPtr.Zero)
                focusedControl = Control.FromChildHandle(focusedHandle);
            focusedControl = focusedControl.Parent.Parent;
            if (focusedControl != null && focusedControl.GetType() == typeof(DataGridView))
            {
                if (keyData == (Keys.Control | Keys.V))//Ctrl+V
                {
                    if (!DataGirdViewPaste((DataGridView)focusedControl))
                    {
                        return true;
                    }
                }
                else if (keyData == (Keys.Control | Keys.C))//Ctrl+C
                {
                    DataGirdViewCopy((DataGridView)focusedControl);
                }
            }
            if (keyData == (Keys.Control | Keys.S))//Ctrl+S
            {

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
        /// <summary>
        /// 刷新DgvLi中的数据改变标记
        /// </summary>
        /// <param name="dgv"></param>
        void RefreshDgvDataChangeF(DataGridView dgv)
        {
            for (int ii = 0; ii < DgvLis.Count; ii++)
            {
                if (DgvLis[ii].Dgv == dgv)
                {
                    DgvLis[ii].DgvDatChange = true;
                }
            }
        }

        /// <summary>
        /// 刷新DgvLi数据在Xml中的路径
        /// </summary>
        /// <param name="dgv"></param>
        void RefreshDgvXmlPath(DataGridView dgv, Int32 Index)
        {
            //if (dgv.Name == DGVRobotInputSignalS.Name)
            //{
            //    DgvLis[2].PathStr = m_xmlDociment.PathRoot_ROBOT_Item + Index.ToString() + "/" + m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.X];
            //}
            //else if (dgv.Name == DGVRobotOutputSignalS.Name)
            //{
            //    DgvLis[3].PathStr = m_xmlDociment.PathRoot_ROBOT_Item + Index.ToString() + "/" + m_xmlDociment.Default_Path_str[(int)m_xmlDociment.Path_str.Y];
            //}
            //else if (dgv.Name == PLCInputSignalSDefine.Name)
            //{
            //    DgvLis[5].PathStr = m_xmlDociment.PathRoot_PLC_Item + Index.ToString() + "/" + comboBoxPLCDevice1.Text;//MainForm.m_xml.Default_Path_str[6];
            //}
            //else if (dgv.Name == PLCOutputSignalSDefine.Name)
            //{
            //    DgvLis[6].PathStr = m_xmlDociment.PathRoot_PLC_Item + Index.ToString() + "/" + comboBoxPLCDevice2.Text;//MainForm.m_xml.Default_Path_str[7];
            //}
        }

        /// <summary>
        /// 获取DgvLi数据再Xml中的路径
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        string GetDgvXmlLisPath(DataGridView dgv)
        {
            string path = "";
            for (int ii = 0; ii < DgvLis.Count; ii++)
            {
                if (DgvLis[ii].Dgv == dgv)
                {
                    path = DgvLis[ii].PathStr;
                }
            }
            return path;
        }

        /// <summary>
        /// 刷新列表行数
        /// </summary>
        /// <param name="ReadValue_name"></param>
        void BindingLisReadValue(DataGridView dgv)
        {
            foreach (Binding m_b in m_bindingList)
            {
                if (((DataGridView)(m_b.DataSource)) == dgv)
                {
                    m_b.ReadValue();
                }
            }
        }

        public void LoadSetLanguage()
        {
            string lang = ChangeLanguage.GetDefaultLanguage();
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
            if (prelanguage != lang)
            {
                comboBoxLineType.Items.Clear();
                string Default_linetype_value1 = ChangeLanguage.GetString(m_Hashtable, "comoBoxLineType.Items01");
                string Default_linetype_value2 = ChangeLanguage.GetString(m_Hashtable, "comoBoxLineType.Items02");
                string Default_linetype_value3 = ChangeLanguage.GetString(m_Hashtable, "comoBoxLineType.Items03");
                comboBoxLineType.Items.Add(Default_linetype_value1);
                comboBoxLineType.Items.Add(Default_linetype_value2);
                comboBoxLineType.Items.Add(Default_linetype_value3);

                comboBoxLineType.SelectedIndex = 2;
                prelanguage = lang;
            }
            dgvCNCSet.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn001");
            dgvCNCSet.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn002");
            dgvCNCSet.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn003");
            dgvCNCSet.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn004");
            dgvCNCSet.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn005");
            dgvCNCSet.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn006");
            dgvCNCSet.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn007");
            dgvCNCSet.Columns[7].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn008");
            dgvCNCSet.Columns[8].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn009");
            dgvCNCSet.Columns[9].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn010");
            dgvCNCSet.Columns[10].HeaderText = ChangeLanguage.GetString(m_Hashtable, "CNCSetColumn011");

            dgvRobotSet.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0001");
            dgvRobotSet.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0002");
            dgvRobotSet.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0003");
            dgvRobotSet.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0004");
            dgvRobotSet.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0005");
            dgvRobotSet.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0006");
            dgvRobotSet.Columns[8].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0007");
            dgvRobotSet.Columns[9].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0008");
            dgvRobotSet.Columns[10].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RobotSetColumn0009");



            dgvPLCSet.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm00");
            dgvPLCSet.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm01");
            dgvPLCSet.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm02");
            dgvPLCSet.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm03");
            dgvPLCSet.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm04");
            dgvPLCSet.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm05");
            dgvPLCSet.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm06");
            dgvPLCSet.Columns[7].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm07");
            dgvPLCSet.Columns[8].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm08");
            dgvPLCSet.Columns[9].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm09");
            dgvPLCSet.Columns[10].HeaderText = ChangeLanguage.GetString(m_Hashtable, "PLCSetColumm10");


            dgvRFIDSet.ColumnCount = 16;
            dgvRFIDSet.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn01");
            dgvRFIDSet.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn02");
            dgvRFIDSet.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn03");
            dgvRFIDSet.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn04");
            dgvRFIDSet.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn05");
            dgvRFIDSet.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn06");
            dgvRFIDSet.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn07");
            dgvRFIDSet.Columns[7].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn08");
            dgvRFIDSet.Columns[8].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn09");
            dgvRFIDSet.Columns[9].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn10");
            dgvRFIDSet.Columns[10].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn11");
            dgvRFIDSet.Columns[11].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn12");
            dgvRFIDSet.Columns[12].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn13");
            dgvRFIDSet.Columns[13].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn14");
            dgvRFIDSet.Columns[14].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn15");
            dgvRFIDSet.Columns[15].HeaderText = ChangeLanguage.GetString(m_Hashtable, "RFIDSetColumn16");
            dgvRFIDSet.Columns[0].ReadOnly = true;
            dgvRFIDSet.Columns[4].Visible = false;
            dgvRFIDSet.Columns[5].Visible = false;
            dgvRFIDSet.Columns[6].Visible = false;
            dgvRFIDSet.Columns[7].Visible = false;
            dgvRFIDSet.Columns[8].Visible = false;
            dgvRFIDSet.Columns[9].Visible = false;
            dgvRFIDSet.Columns[10].Visible = false;
            dgvRFIDSet.Columns[11].Visible = false;
            dgvRFIDSet.Columns[12].Visible = false;
            dgvMeasureSet.ColumnCount = 10;
            dgvMeasureSet.Columns[0].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn01");
            dgvMeasureSet.Columns[1].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn02");
            dgvMeasureSet.Columns[2].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn03");
            dgvMeasureSet.Columns[3].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn04");
            dgvMeasureSet.Columns[4].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn05");
            dgvMeasureSet.Columns[5].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn06");
            dgvMeasureSet.Columns[6].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn07");
            dgvMeasureSet.Columns[7].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn08");
            dgvMeasureSet.Columns[8].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn09");
            dgvMeasureSet.Columns[9].HeaderText = ChangeLanguage.GetString(m_Hashtable, "MeasureColumn10");
        }

     

        #region   SetForm窗体装载
        private void SetForm_Load(object sender, EventArgs e)
        {
            userIpset1 = new UserIpset("NO1编程电脑:") { Dock = DockStyle.Top };
            userIpset2 = new UserIpset("NO2管控电脑:") { Dock = DockStyle.Top };
            userIpset3 = new UserIpset("NO3工艺电脑:") { Dock = DockStyle.Top };
            userIpset4 = new UserIpset("NO4车床:") { Dock = DockStyle.Top };
            userIpset5 = new UserIpset("NO5加工中心:") { Dock = DockStyle.Top };
            userIpset6 = new UserIpset("NO6PLC:") { Dock = DockStyle.Top };
            userIpset7 = new UserIpset("NO7机器人:") { Dock = DockStyle.Top };
            userIpset8 = new UserIpset("NO8录像机:") { Dock = DockStyle.Top };
            flowLayoutPanel1.Controls.Add(userIpset1);
            flowLayoutPanel1.Controls.Add(userIpset2);
            flowLayoutPanel1.Controls.Add(userIpset3);
            flowLayoutPanel1.Controls.Add(userIpset4);
            flowLayoutPanel1.Controls.Add(userIpset5);
            flowLayoutPanel1.Controls.Add(userIpset6);
            flowLayoutPanel1.Controls.Add(userIpset7);
            flowLayoutPanel1.Controls.Add(userIpset8);

            IPSetLoad();
           // ChangeLanguage.LoadLanguage(this);//zxl 4.19
            //prelanguage = ChangeLanguage.GetDefaultLanguage();

            //改变datagridview的编辑属性
            dgvCNCSet.AllowUserToAddRows = false;
            dgvCNCSet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            //dgvPLCSet.AllowUserToAddRows = false;
            //dgvPLCSet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dgvRobotSet.AllowUserToAddRows = false;
            dgvRobotSet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dgvRFIDSet.AllowUserToAddRows = false;
            dgvRFIDSet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dgvMeasureSet.AllowUserToAddRows = false;
            dgvMeasureSet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dgvPLCSet.AllowUserToAddRows = false;
            dgvPLCSet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            //DGVRobotInputSignalS.AllowUserToAddRows = false;
            //DGVRobotInputSignalS.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            //DGVRobotOutputSignalS.AllowUserToAddRows = false;
            //DGVRobotOutputSignalS.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            //PLCInputSignalSDefine.AllowUserToAddRows = false;
            //PLCInputSignalSDefine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            //PLCOutputSignalSDefine.AllowUserToAddRows = false;
            //PLCOutputSignalSDefine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            //dgv_PLCAlarmTb.AllowUserToAddRows = false;
            //dgv_PLCAlarmTb.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            //设置列的只读属性         
            dgvCNCSet.Columns[0].ReadOnly = true;
            dgvCNCSet.Columns[2].ReadOnly = true;
            dgvCNCSet.Columns[3].ReadOnly = true;
            dgvCNCSet.Columns[4].ReadOnly = true;//数控系统类型
            //dgvCNCSet.Columns[5].ReadOnly = true;

            //dgvPLCSet.Columns[0].ReadOnly = true;
            //dgvPLCSet.Columns[2].ReadOnly = true;
            //dgvPLCSet.Columns[3].ReadOnly = true;
            //dgvPLCSet.Columns[4].ReadOnly = true;
            //dgvPLCSet.Columns[5].ReadOnly = true;

            dgvRobotSet.Columns[0].ReadOnly = true;
            dgvRobotSet.Columns[2].ReadOnly = true;
            dgvRobotSet.Columns[3].ReadOnly = true;
            dgvRobotSet.Columns[4].ReadOnly = true;
            dgvRobotSet.Columns[5].ReadOnly = true;

            //DGVRobotInputSignalS.Columns[0].ReadOnly = true;
            //DGVRobotInputSignalS.Columns[3].ReadOnly = true;
            //DGVRobotOutputSignalS.Columns[0].ReadOnly = true;
            //DGVRobotOutputSignalS.Columns[3].ReadOnly = true;

            //PLCInputSignalSDefine.Columns[0].ReadOnly = true;
            //PLCInputSignalSDefine.Columns[3].ReadOnly = true;
            //PLCOutputSignalSDefine.Columns[0].ReadOnly = true;
            //PLCOutputSignalSDefine.Columns[3].ReadOnly = true;

            //设置列不能自动排序       
            for (int i = 0; i < dgvCNCSet.Columns.Count; i++)
            {
                dgvCNCSet.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //for (int i = 0; i < dgvPLCSet.Columns.Count; i++)
            //{
            //    dgvPLCSet.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
            for (int i = 0; i < dgvRobotSet.Columns.Count; i++)
            {
                dgvRobotSet.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //for (int i = 0; i < DGVRobotInputSignalS.Columns.Count; i++)
            //{
            //    DGVRobotInputSignalS.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
            //for (int i = 0; i < DGVRobotOutputSignalS.Columns.Count; i++)
            //{
            //    DGVRobotOutputSignalS.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
            //for (int i = 0; i < PLCInputSignalSDefine.Columns.Count; i++)
            //{
            //    PLCInputSignalSDefine.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
            //for (int i = 0; i < PLCOutputSignalSDefine.Columns.Count; i++)
            //{
            //    PLCOutputSignalSDefine.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //}

            ///////////CNC本地IP选择处理
            System.Net.IPAddress CNCLocalIp;
            if (!System.Net.IPAddress.TryParse(MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]), out CNCLocalIp))
            {
                MainForm.m_xml.m_UpdateAttribute(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0], m_xmlDociment.Default_CNCLocalIVvalue[0]);
                MainForm.m_xml.SaveXml2File(MainForm.XMLSavePath);
            }
            comboBox_LocalIpAddr.SelectedText = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]);

            //////////////////////PLC系统
            //comboBoxSelePLCSystem.DataSource = m_xmlDociment.PLC_System;
            /////////产线类型选择
            comboBoxLineType.Items.Clear();
            m_Hashtable = ChangeLanguage.LoadOtherLanguage(this);
            string Default_linetype_value1 = ChangeLanguage.GetString(m_Hashtable, "comoBoxLineType.Items01");
            string Default_linetype_value2 = ChangeLanguage.GetString(m_Hashtable, "comoBoxLineType.Items02");
            string Default_linetype_value3 = ChangeLanguage.GetString(m_Hashtable, "comoBoxLineType.Items03");
            comboBoxLineType.Items.Add(Default_linetype_value1);
            comboBoxLineType.Items.Add(Default_linetype_value2);
            comboBoxLineType.Items.Add(Default_linetype_value3);

            comboBoxLineType.SelectedIndex = 2;
            /*comboBoxLineType.DataSource = m_xmlDociment.Default_linetype_value;
            for(int ii = 0;ii < m_xmlDociment.Default_linetype_value.Length;ii++)
            {
                if (MainForm.m_xml.m_Read(m_xmlDociment.Path_linetype, -1, m_xmlDociment.Default_Attributes_linetype[0]) == m_xmlDociment.Default_linetype_value[ii])
                {
                    comboBoxLineType.SelectedIndex = ii;
                }
            }*/
            //comboBoxPLCDevice1.DataSource = m_xmlDociment.Default_MITSUBISHI_Device1;
            //comboBoxPLCDevice2.DataSource = m_xmlDociment.Default_MITSUBISHI_Device2;



            //             string str = "";
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            int jj = 0, kk = 0;
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection allAddress = adapterProperties.UnicastAddresses;
                if (allAddress.Count > 0)
                {
                    //                     str += "interface   " + jj + "description:\n\t " + adapter.Description + "\n ";
                    foreach (UnicastIPAddressInformation addr in allAddress)
                    {
                        if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            //comboBox_LocalIpAddr.Items.Add(addr.Address);
                        }
                        if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            comboBox_LocalIpAddr.Items.Add(addr.Address);
                            if (addr.Address.ToString() == MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]))
                            {
                                kk = jj;
                            }
                            jj++;
                        }
                    }
                }
            }
            comboBox_LocalIpAddr.SelectedIndex = kk;

            //textBoxStartIO.Text = "0";

            InitdgvSet(ref dgvRFIDSet, ref m_xmlDociment.Default_RFID_STR);
            dgvRFIDSet.Columns[0].ReadOnly = true;
            dgvRFIDSet.Columns[4].Visible = false;
            dgvRFIDSet.Columns[5].Visible = false;
            dgvRFIDSet.Columns[6].Visible = false;
            dgvRFIDSet.Columns[7].Visible = false;
            dgvRFIDSet.Columns[8].Visible = false;
            dgvRFIDSet.Columns[9].Visible = false;
            dgvRFIDSet.Columns[10].Visible = false;
            dgvRFIDSet.Columns[11].Visible = false;
            dgvRFIDSet.Columns[12].Visible = false;

            for (int i = 0; i < dgvRFIDSet.Columns.Count; i++)
            {
                dgvRFIDSet.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //InitdgvSet(ref dgv_PLCAlarmTb, ref m_xmlDociment.Default_PLCAlarmTb_STR);
            //dgv_PLCAlarmTb.Columns[0].ReadOnly = true;

            InitdgvSet(ref dgvMeasureSet, ref m_xmlDociment.Default_Equement_STR);
            dgvMeasureSet.Columns[0].ReadOnly = true;
            dgvMeasureSet.Columns[2].ReadOnly = true;
            dgvMeasureSet.Columns[3].ReadOnly = true;
            //dgvMeasureSet.Columns[2].Visible = false;
            //dgvMeasureSet.Columns[3].Visible = false;
            for (int i = 0; i < dgvMeasureSet.Columns.Count; i++)
            {
                dgvMeasureSet.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //设置列的只读属性         
            //InitdgvSet(ref dgvPLCSet, ref m_xmlDociment.Default_PLC_STR);
            dgvPLCSet.Columns[0].ReadOnly = true;
            dgvPLCSet.Columns[2].ReadOnly = true;
            dgvPLCSet.Columns[3].ReadOnly = true;
            dgvPLCSet.Columns[4].ReadOnly = true;
            //dgvPLCSet.Columns[5].ReadOnly = true;
            LoadSetLanguage();
            MainForm.languagechangeEvent += LanguageChange;
            InitIPAddress();
            InitIPtext();
            //Initlinepoint();
        }
        #endregion


        
        void InitIPtext()
        {
        }

        Graphics graphics;
        IList<LayerData> layerDatas = new List<LayerData>();
        IList<Button> buttons = new List<Button>();
        IList<String> IPstring = new List<String>();
        IList<Image> Imagelist = new List<Image>();
        void renewIPlist()
        {
            IPstring.Add(MainForm.PCplcAddress);
            IPstring.Add(MainForm.PCAddress);
            IPstring.Add(MainForm.PCcadAddress);
            IPstring.Add(MainForm.LATHEAddress);
            IPstring.Add(MainForm.CNCAddress);
            IPstring.Add(MainForm.PLCAddress);
            IPstring.Add(MainForm.ROBORTAddress);
            IPstring.Add(MainForm.VIDEOAddress);
            //IPstring.Add(MainForm.MeterAddress);
            //IPstring.Add(MainForm.RFIDAddress);
            //IPstring.Add(MainForm.WATCH1Address);
            //IPstring.Add(MainForm.WATCH2Address);
        }
        void IPSetLoad()
        {        
            renewIPlist();
            Imagelist.Add(Properties.Resources.P1);
            Imagelist.Add(Properties.Resources.P2);
            Imagelist.Add(Properties.Resources.P3);
            Imagelist.Add(Properties.Resources.P4);
            Imagelist.Add(Properties.Resources.P5);
            Imagelist.Add(Properties.Resources.P6);
            Imagelist.Add(Properties.Resources.P7);
            Imagelist.Add(Properties.Resources.P8);
            //Imagelist.Add(Properties.Resources.P9);
            //Imagelist.Add(Properties.Resources.P10);
            //Imagelist.Add(Properties.Resources.P11);
            //Imagelist.Add(Properties.Resources.P12);

            pictureBoxIP.Image = new Bitmap(pictureBoxIP.Width, pictureBoxIP.Height);
            graphics = Graphics.FromImage(pictureBoxIP.Image);
            var list = new List<Button>();
            var count = 8;
            var points = GetPoints(pictureBoxIP.Width / 2, pictureBoxIP.Height / 2, 300, count);
            for (int i = 0; i < count; i++)
            {
                var btn = new Button { Location = new Point(points[i].X - 33, points[i].Y - 25), Width = 75, Height = 50, Text = "",
                                       BackgroundImage = Imagelist[i],
                                       BackgroundImageLayout = ImageLayout.Stretch,
                                       FlatStyle = FlatStyle.Popup,
                                       Tag = IPstring[i]};
                btn.Click += Button_Click;
                list.Add(btn);
            }

            pictureBoxIP.Controls.AddRange(list.ToArray());
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    list[i].PerformClick();
                    list[j].PerformClick();
                }
            }
           
        }
         private IList<Point> GetPoints(int ox, int oy, int r, int count)
        {
            var list = new List<Point>();
            var radians = (Math.PI / 180) * Math.Round(360.0 / count);
            for (int i = 0; i < count; i++)
            {
                var x = ox + r * Math.Sin(radians * i);
                var y = oy + r * Math.Cos(radians * i);
                list.Add(new Point((int)x, (int)y));
            }
            return list;
        }
        private void Button_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (!buttons.Contains(btn))
            {
                buttons.Add(btn);
                if (buttons.Count == 2)
                {
                    bool found = false;
                    for (int i = 0; i < layerDatas.Count; i++)
                    {
                        var item = layerDatas[i];
                        if (item.IsThis(buttons))
                        {
                            found = true;
                            layerDatas.Remove(item);
                            break;
                        }
                    }
                    if (!found)
                    {
                        layerDatas.Add(new LayerData(buttons[0], buttons[1]));
                        DrawLine(Color.LightBlue, buttons);
                    }
                    else
                    {
                        DrawLine(pictureBoxIP.BackColor, buttons);
                        for (int i = 0; i < layerDatas.Count; i++)
                        {
                            DrawLine(Color.LightBlue, layerDatas[i].Buttons);
                        }
                    }
                    pictureBoxIP.Refresh();
                    buttons.Clear();
                }
            }
        }

        private void DrawLine(Color color, IList<Button> buttons)
        {
            graphics.DrawLine(
                new Pen(color,3),
                new Point(buttons[0].Location.X + buttons[0].Width / 2, buttons[0].Location.Y + buttons[0].Height / 2),
                new Point(buttons[1].Location.X + buttons[1].Width / 2, buttons[1].Location.Y + buttons[1].Height / 2)
                );
            graphics.Save();
        }


        private void buttonnettest_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < layerDatas.Count; i++)
            {
                IList<Button> buttons1 = layerDatas[i].Buttons;
                string ip1 = buttons1[0].Tag.ToString();
                string ip2 = buttons1[1].Tag.ToString();
                //RFID连接逻辑
                //if (ip1 == MainForm.RFIDAddress )
                //{
                //    if(ip2 == MainForm.PLCAddress)
                //    {
                //        DrawLine(Color.Green, layerDatas[i].Buttons);
                       
                //    }
                //    else
                //    {
                //        DrawLine(Color.Red, layerDatas[i].Buttons);;
                //    }
                //}
                //else if (ip2 == MainForm.RFIDAddress )
                //{
                //    if(ip1 == MainForm.PLCAddress)
                //    {
                //        DrawLine(Color.Green, layerDatas[i].Buttons);
                //    }
                //    else
                //    {
                //        DrawLine(Color.Red, layerDatas[i].Buttons);;
                //    }
                //}
                ////摄像头连接逻辑
                //else if(ip1== MainForm.WATCH1Address)
                //{
                //    if(ip2 == MainForm.VIDEOAddress)                  
                //    {
                //        DrawLine(Color.Green, layerDatas[i].Buttons);
                        
                //    }
                //    else
                //    {
                //        DrawLine(Color.Red, layerDatas[i].Buttons);;
                //    }
                //}
                //else  if (ip2 == MainForm.WATCH1Address)
                //{
                //    if(ip1 == MainForm.VIDEOAddress)
                //    {
                //        DrawLine(Color.Green, layerDatas[i].Buttons);
                //    }
                //    else
                //    {
                //        DrawLine(Color.Red, layerDatas[i].Buttons);;
                //    }
                //}
                // else if(ip1== MainForm.WATCH2Address)
                //{
                //    if(ip2 == MainForm.VIDEOAddress)
                //    {
                //        DrawLine(Color.Green, layerDatas[i].Buttons);
                        
                //    }
                //    else
                //    {
                //        DrawLine(Color.Red, layerDatas[i].Buttons);;
                //    }
                //}
                //else if (ip2 == MainForm.WATCH2Address)
                //{
                //    if(ip1 == MainForm.VIDEOAddress)
                //    {
                //        DrawLine(Color.Green, layerDatas[i].Buttons);
                //    }
                //    else
                //    {
                //        DrawLine(Color.Red, layerDatas[i].Buttons);;
                //    }
                //}

                ////测量逻辑
                //else if (ip1 == MainForm.MeterAddress)
                //{
                //    if(ip2 == MainForm.CNCAddress)
                //    {
                //        DrawLine(Color.Green, layerDatas[i].Buttons);
                //    }
                //    else
                //    {
                //        DrawLine(Color.Red, layerDatas[i].Buttons);;
                //    }
                //}
                //else if (ip2 == MainForm.MeterAddress)
                //{
                //    if(ip1 == MainForm.CNCAddress)
                //    {
                //        DrawLine(Color.Green, layerDatas[i].Buttons);
                //    }
                //    else
                //    {
                //        DrawLine(Color.Red, layerDatas[i].Buttons);;
                //    }
                //}
                
                //包含总控电脑的逻辑
               // else if (ip1 == MainForm.PCAddress || ip2 == MainForm.PCAddress)
                    if (ip1 == MainForm.PCAddress || ip2 == MainForm.PCAddress)
                {
                    if(ip1==MainForm.PCAddress)
                    {
                        bool connect = MainForm.PingTestCNC(ip2, 100);
                        if(connect)
                        {
                            DrawLine(Color.Green, layerDatas[i].Buttons);
                        }
                        else
                        {
                            DrawLine(Color.Red, layerDatas[i].Buttons);
                        }
                    }
                    else
                    {
                         bool connect = MainForm.PingTestCNC(ip1, 100);
                        if(connect)
                        {
                            DrawLine(Color.Green, layerDatas[i].Buttons);
                        }
                        else
                        {
                            DrawLine(Color.Red, layerDatas[i].Buttons);
                        }
                    }
                }               
                else//总控电脑，RFID，侧头，摄像头之外的设备连接
                {
                    bool connect1 = MainForm.PingTestCNC(ip1, 100);
                    bool connect2 = MainForm.PingTestCNC(ip2, 100);
                    if(connect1&&connect2)
                    {
                        DrawLine(Color.Green, layerDatas[i].Buttons);
                    }
                    else
                    {
                       DrawLine(Color.Red, layerDatas[i].Buttons);
                    }
                }


            }
                   
            pictureBoxIP.Refresh();
            buttons.Clear();
            
        }

        //private void Button_Click(object sender, EventArgs e)
        //{
        //    //PictureBox1.Image = new Bitmap(PictureBox1.Width, PictureBox1.Height);
        //    //Graphics graphics = Graphics.FromImage(PictureBox1.Image);
        //    //Pen pen = new Pen(Color.Red, 5);
        //    //graphics.DrawLine(pen,
        //    //    new Point(0, 0),
        //    //    new Point(50, 05));
        //    //graphics.Save();
        //    //return;
        //    var btn = sender as Button;
        //    if (!buttons.Contains(btn))
        //    {
        //        buttons.Add(btn);
        //    }
        //    if (buttons.Count == 2)
        //    {
        //        for (int i = 0; i < layerDatas.Count; i++)
        //        {
        //            var item = layerDatas[i];
        //            if (item.IsThis(buttons))
        //            {
        //                item.Draw();
        //                break;
        //            }
        //        }
        //        buttons.Clear();
        //    }
        //}
        //void Initlinepoint()
        //{
        //    pointnode1.X = pictureBoxn1.Location.X + pictureBoxn1.Size.Width / 2;
        //    pointnode1.Y = pictureBoxn1.Location.Y + pictureBoxn1.Size.Height;
        //    pointnode2.X = pictureBoxn2.Location.X + pictureBoxn2.Size.Width / 2;
        //    pointnode2.Y = pictureBoxn2.Location.Y + pictureBoxn2.Size.Height;
        //    pointnode3.X = pictureBoxn3.Location.X + pictureBoxn3.Size.Width;
        //    pointnode3.Y = pictureBoxn3.Location.Y + pictureBoxn3.Size.Height / 2;
        //    pointnode4.X = pictureBoxn4.Location.X + pictureBoxn4.Size.Width;
        //    pointnode4.Y = pictureBoxn4.Location.Y + pictureBoxn4.Size.Height / 2;
        //    pointnode5.X = pictureBoxn5.Location.X + pictureBoxn5.Size.Width;
        //    pointnode5.Y = pictureBoxn5.Location.Y + pictureBoxn5.Size.Height / 2;
        //    pointnode6.X = pictureBoxn6.Location.X + pictureBoxn6.Size.Width / 2;
        //    pointnode6.Y = pictureBoxn6.Location.Y;

        //    pointnode7.X = pictureBoxn6.Location.X + pictureBoxn7.Size.Width / 2;
        //    pointnode7.Y = pictureBoxn6.Location.Y;
        //    pointnode8.Y = pictureBoxn5.Location.Y;
        //    pointnode8.X = pictureBoxn6.Location.X + pictureBoxn8.Size.Width / 2;
        //    pointnode9.Y = pictureBoxn9.Location.Y + pictureBoxn9.Size.Height / 2;
        //    pointnode9.X = pictureBoxn9.Location.X;
        //    pointnode10.Y = pictureBoxn10.Location.Y + pictureBoxn10.Size.Height / 2;
        //    pointnode10.X = pictureBoxn10.Location.X;
        //    pointnode11.Y = pictureBoxn11.Location.Y + pictureBoxn11.Size.Height / 2;
        //    pointnode11.X = pictureBoxn11.Location.X;
        //    pointnode12.Y = pictureBoxn12.Location.Y + pictureBoxn12.Size.Height;
        //    pointnode12.X = pictureBoxn12.Location.X + pictureBoxn12.Size.Width / 2;

        //    pointnode13right.Y = pictureBoxn9.Location.Y + pictureBoxn5.Size.Height;
        //    pointnode13right.X = pictureBoxn9.Location.X + pictureBoxn6.Size.Width / 2;
        //    userIpset1.SetIPName("编程电脑:");
        //    userIpset2.SetIPName("管控电脑:");
        //    userIpset3.SetIPName("工艺电脑:");
        //    userIpset4.SetIPName("车床:");
        //    userIpset5.SetIPName("加工中心:");
        //    userIpset6.SetIPName("PLC:");
        //    userIpset7.SetIPName("机器人:");
        //    userIpset8.SetIPName("录像机:");
        //    userIpset9.SetIPName("侧头:");
        //    userIpset10.SetIPName("RFID:");
        //    userIpset11.SetIPName("摄像头1:");
        //    userIpset12.SetIPName("摄像头2:");
        //    userIpset13.SetIPName("交换机:");
        //}
        /// <summary>
        /// 初始化IP设置界面
        /// </summary>
        void InitIPAddress()
        {
            int[] arry = new int[4];

            Ipstringtoarry(MainForm.PCplcAddress, ref arry);
            userIpset1.SetIPAdress(arry);

            Ipstringtoarry(MainForm.PCAddress, ref arry);
            userIpset2.SetIPAdress(arry);

            Ipstringtoarry(MainForm.PCcadAddress, ref arry);
            userIpset3.SetIPAdress(arry);

            Ipstringtoarry(MainForm.LATHEAddress, ref arry);
            userIpset4.SetIPAdress(arry);

            Ipstringtoarry(MainForm.CNCAddress, ref arry);
            userIpset5.SetIPAdress(arry);

            Ipstringtoarry(MainForm.PLCAddress, ref arry);
            userIpset6.SetIPAdress(arry);


            Ipstringtoarry(MainForm.ROBORTAddress, ref arry);
            userIpset7.SetIPAdress(arry);

            Ipstringtoarry(MainForm.VIDEOAddress, ref arry);
            userIpset8.SetIPAdress(arry);

            //Ipstringtoarry(MainForm.MeterAddress, ref arry);
            //userIpset9.SetIPAdress(arry);

            //Ipstringtoarry(MainForm.RFIDAddress, ref arry);
            //userIpset10.SetIPAdress(arry);

            //Ipstringtoarry(MainForm.WATCH1Address, ref arry);
            //userIpset11.SetIPAdress(arry);

            //Ipstringtoarry(MainForm.WATCH2Address, ref arry);
            //userIpset12.SetIPAdress(arry);
        }
        /// <summary>
        /// 解析IP
        /// </summary>
        /// <param name="ipstr"></param>
        /// <param name="arry"></param>
        void Ipstringtoarry(string ipstr, ref int[] arry)
        {
            int index = 0;
            int i = 0;
            string tempstr = ipstr;
            index = ipstr.IndexOf('.');
            for (i = 0; i < 4; i++)
            {
                if (index < 0)
                {

                    arry[i] = Convert.ToInt32(tempstr);
                }
                else
                {
                    string title = tempstr.Substring(0, index);
                    arry[i] = Convert.ToInt32(title);
                    tempstr = tempstr.Substring(index + 1, tempstr.Length - index - 1);
                    index = tempstr.IndexOf('.');
                }


            }
        }
        void LanguageChange(object sender, string Language)
        {
            LoadSetLanguage();
        }
        #region   表格标题行语音切换注册
        private void changeDGVSetting()
        {
            //             dgvCNCSet.Columns[0].HeaderText = Localization.Forms["SetForm"]["colNumberCNCSet"];
            //             dgvCNCSet.Columns[1].HeaderText = Localization.Forms["SetForm"]["colIDSet"];
            //             dgvCNCSet.Columns[2].HeaderText = Localization.Forms["SetForm"]["colWorkshopSet"];
            //             dgvCNCSet.Columns[3].HeaderText = Localization.Forms["SetForm"]["colProductionLineSet"];
            //             dgvCNCSet.Columns[4].HeaderText = Localization.Forms["SetForm"]["colTypeSet"];
            //             dgvCNCSet.Columns[5].HeaderText = Localization.Forms["SetForm"]["colDataSet"];
            //             dgvCNCSet.Columns[6].HeaderText = Localization.Forms["SetForm"]["colIPSet"];
            //             dgvCNCSet.Columns[7].HeaderText = Localization.Forms["SetForm"]["colPortSet"];
            // 
            //             dgvRobotSet.Columns[0].HeaderText = Localization.Forms["SetForm"]["colNumberRobotSet"];
            //             dgvRobotSet.Columns[1].HeaderText = Localization.Forms["SetForm"]["colRobotIDSet"];
            //             dgvRobotSet.Columns[2].HeaderText = Localization.Forms["SetForm"]["colWorkShopRobotSet"];
            //             dgvRobotSet.Columns[3].HeaderText = Localization.Forms["SetForm"]["colProductionLineRobotSet"];
            //             dgvRobotSet.Columns[4].HeaderText = Localization.Forms["SetForm"]["colTypeRobotSet"];
            //             dgvRobotSet.Columns[5].HeaderText = Localization.Forms["SetForm"]["colModelRobotSet"];
            //             dgvRobotSet.Columns[6].HeaderText = Localization.Forms["SetForm"]["colModelRobotIp"];
            //             dgvRobotSet.Columns[7].HeaderText = Localization.Forms["SetForm"]["colModelRobotPort"];
            // 
            //             dgvPLCSet.Columns[0].HeaderText = Localization.Forms["SetForm"]["colNumberPLCSet"];
            //             dgvPLCSet.Columns[1].HeaderText = Localization.Forms["SetForm"]["colPLCIDSet"];
            //             dgvPLCSet.Columns[2].HeaderText = Localization.Forms["SetForm"]["colWorkShopPLCSet"];
            //             dgvPLCSet.Columns[3].HeaderText = Localization.Forms["SetForm"]["colProductionLinePLCSet"];
            //             dgvPLCSet.Columns[4].HeaderText = Localization.Forms["SetForm"]["colTypePLCSet"];
            //             dgvPLCSet.Columns[5].HeaderText = Localization.Forms["SetForm"]["colModelPLCSet"];
            //             dgvPLCSet.Columns[6].HeaderText = Localization.Forms["SetForm"]["colIPPLCSet"];
            //             dgvPLCSet.Columns[7].HeaderText = Localization.Forms["SetForm"]["colPortPLCSet"];
            // 
            //             dgvRFIDSet.Columns[0].HeaderText = Localization.Forms["SetForm"]["colNumberRFIDSet"];
            //             dgvRFIDSet.Columns[1].HeaderText = Localization.Forms["SetForm"]["colIDRFIDSet"];
            //             dgvRFIDSet.Columns[2].HeaderText = Localization.Forms["SetForm"]["colWorkShopRFIDSet"];
            //             dgvRFIDSet.Columns[3].HeaderText = Localization.Forms["SetForm"]["colProductionLineRFIDSet"];
            //             dgvRFIDSet.Columns[4].HeaderText = Localization.Forms["SetForm"]["colTypeRFIDSet"];
            //             dgvRFIDSet.Columns[5].HeaderText = Localization.Forms["SetForm"]["colModelRFIDSet"];
            //             dgvRFIDSet.Columns[6].HeaderText = Localization.Forms["SetForm"]["colIPRFIDSet"];
            //             dgvRFIDSet.Columns[7].HeaderText = Localization.Forms["SetForm"]["colPortRFIDSet"];
            //             dgvRFIDSet.Columns[8].HeaderText = Localization.Forms["SetForm"]["colControlRFIDSet"];


            //             DGVRobotSignalSDefine.Columns[0].HeaderText = Localization.Forms["SetForm"]["robotNum"];
            //             DGVRobotSignalSDefine.Columns[1].HeaderText = Localization.Forms["SetForm"]["RobotSignalNum"];
            //             DGVRobotSignalSDefine.Columns[2].HeaderText = Localization.Forms["SetForm"]["RobotAddress"];
            //             DGVRobotSignalSDefine.Columns[3].HeaderText = Localization.Forms["SetForm"]["RobotSignalName"];
            // 
            //             DGVPLCSignalSDefine.Columns[0].HeaderText = Localization.Forms["SetForm"]["PLCNum"];
            //             DGVPLCSignalSDefine.Columns[1].HeaderText = Localization.Forms["SetForm"]["PLCSignalNum"];
            //             DGVPLCSignalSDefine.Columns[2].HeaderText = Localization.Forms["SetForm"]["plcAddress"];
            //             DGVPLCSignalSDefine.Columns[3].HeaderText = Localization.Forms["SetForm"]["plcSignalName"];

        }
        #endregion

        //protected override void DefWndProc(ref Message m)
        //{
        //    //switch (m.Msg)
        //    //{
        //    //    case MainForm.LanguageChangeMsg:
        //    //        //                     Localization.RefreshLanguage(this);
        //    //        //                     changeDGVSetting();
        //    //        break;
        //    //    default:
        //    //        base.DefWndProc(ref m);
        //    //        break;
        //    //}
        //}
        #region   窗体自动调整布局设置
        private void SetForm_SizeChanged(object sender, EventArgs e)
        {
            //             asc.controllInitializeSize(this);
            //   asc.controlAutoSize(this);
            this.WindowState = (System.Windows.Forms.FormWindowState)(2);
        }
        #endregion

        #region   数据显示
        //整个窗体可见时显示的处理
        private void SetForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((SetForm)sender).Visible)
            {
                tabControlSet.SelectedIndex = 0; //设置默认显示
                txtCNCSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[0]);
                txtRobotSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, -1, m_xmlDociment.Default_Attributes_str1[0]);
              //  txtPLCSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, -1, m_xmlDociment.Default_Attributes_str1[0]);
              //  txtRFIDSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]);

                //车间和产线的显示  
               // txtWSSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[2]);
               // txtPLSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[3]);
            }
        }
        #endregion

        #region   判断输入参数是否合法
        private void txtCNCSet_Leave(object sender, EventArgs e)
        {
            //初始化正则表达式
            Regex digitregex = new Regex(@"^(?:\d|[123]\d|40)$");
            //判断文本框内容是否符合正则表达式
            if (digitregex.IsMatch(txtCNCSet.Text) == true)
            { }
            else
            {
                MessageBox.Show("请输入0-40的数字", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCNCSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[0]);
                this.txtCNCSet.Focus();
            }
        }

        private void txtRobotSet_Leave(object sender, EventArgs e)
        {
            //初始化正则表达式
            Regex digitregex = new Regex(@"^(?:\d|[1]\d|20)$");
            //判断文本框内容是否符合正则表达式
            if (digitregex.IsMatch(txtRobotSet.Text) == true)
            { }
            else
            {
                MessageBox.Show("请输入0-20的数字", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRobotSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, -1, m_xmlDociment.Default_Attributes_str1[0]);
                this.txtRobotSet.Focus();
            }
        }

        private void txtPLCSet_Leave(object sender, EventArgs e)
        {
            //初始化正则表达式
            Regex digitregex = new Regex(@"^(?:\d|[1]\d|20)$");
            //判断文本框内容是否符合正则表达式
            if (digitregex.IsMatch(txtPLCSet.Text) == true)
            { }
            else
            {
                MessageBox.Show("请输入0-20的数字", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPLCSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, -1, m_xmlDociment.Default_Attributes_str1[0]);
                this.txtPLCSet.Focus();
            }
        }

        private void txtRFIDSet_Leave(object sender, EventArgs e)
        {
            //初始化正则表达式
            Regex digitregex = new Regex(@"^(?:\d|[1]\d|20)$");
            //判断文本框内容是否符合正则表达式
            if (digitregex.IsMatch(txtRFIDSet.Text) == true)
            { }
            else
            {
                MessageBox.Show("请输入0-20的数字", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRFIDSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]);
                this.txtRFIDSet.Focus();
            }

        }

        private void txtWSSet_Leave(object sender, EventArgs e)
        {
            //初始化正则表达式
            //             Regex digitregex = new Regex(@"^#(\d?|10)$");
            //判断文本框内容是否符合正则表达式
            //             if (txtWSSet.Text != "")
            //             {
            //             }
            //             else
            //             {
            //                 MessageBox.Show("请输入字符串", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                 txtWSSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[2]);
            //                 this.txtWSSet.Focus();
            //             }
        }

        private void txtPLSet_Leave(object sender, EventArgs e)
        {
        }

        #endregion

        #region    保存配置

        /// <summary>
        /// 保存
        /// </summary>
        private void SaveDgvData2XmlAndFile(int flag_i, bool flag_message)
        {
            if (LogIn)
            {
                bool SaveF = true;
                //setSave_Click(null, null);
                bool MessageBoxPosFla = false;
                for (int ii = 0; ii < DgvLis.Count; ii++)
                {
                    
                    if (DgvLis[ii].DgvDatChange)
                    {
                        if (flag_message && !MessageBoxPosFla)
                        {
                            string PosMessageStr = ChangeLanguage.GetString("MessageFormContent002");
                            if (flag_i == 1)
                            {
                                PosMessageStr = ChangeLanguage.GetString("MessageFormContent003");
                            }
                            DialogResult select;
                            //select = MessageBox.Show(PosMessageStr, " 提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            message = new MessageForm(ChangeLanguage.GetString("MessageFormTitle01"), PosMessageStr, ChangeLanguage.GetString("MessageFormButton01"), ChangeLanguage.GetString("MessageFormButton02"));
                            message.ShowDialog();
                            select = message.DialogResult;
                            //判断用户的选择
                            if (select != DialogResult.OK)
                            {
                                if (flag_i == 0)
                                {
                                    for (int jj = ii; jj < DgvLis.Count; jj++)
                                    {
                                        DgvLis[jj].DgvDatChange = false;
                                    }
                                }
                                //if (comboBoxPLCDevice1.Text != m_xmlDociment.Default_MITSUBISHI_Device1[2])//Buffer
                                //{
                                //    return;
                                //}
                                SaveF = false;
                            }
                            else
                            {
                                SaveF = true;
                            }
                            MessageBoxPosFla = true;
                        }
                        refreshDgvData2Xml(DgvLis[ii].Dgv, DgvLis[ii].PathStr);
                        //if(SaveF)
                        //MessageBox.Show("文件保存成功！");
                        if (SaveF)
                        {
                            message = new MessageForm("", ChangeLanguage.GetString("MessageFormContent004"), ChangeLanguage.GetString("MessageFormButton01"));
                            message.ShowDialog();
                        }
                        DgvLis[ii].DgvDatChange = false;
                    }
                }
                //if (comboBoxPLCDevice1.Text == m_xmlDociment.Default_MITSUBISHI_Device1[2] && dgvPLCSet != null)//Buffer
                //{
                //    String iStartIOStr = textBoxStartIO_Old.ToString();
                //    String pathstr = m_xmlDociment.PathRoot_PLC_Item + dgvPLCSet.CurrentRow.Index.ToString()
                //                        + "/" + comboBoxPLCDevice1.Text;
                //    if (MainForm.m_xml.CheckNodeExist(pathstr))
                //    {
                //        if (iStartIOStr != MainForm.m_xml.m_Read(pathstr, -1, m_xmlDociment.Default_Attributes_str2[4]))
                //        {
                //            MainForm.m_xml.m_UpdateAttribute(pathstr, -1, m_xmlDociment.Default_Attributes_str2[4], iStartIOStr);
                //            SaveF = true;
                //        }
                //    }
                //}
                if (SaveF)
                {
                    MainForm.m_xml.SaveXml2File(MainForm.XMLSavePath);
                    LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
                    SendParm.Node1NameIndex = (int)LogData.Node1Name.System_security;
                    SendParm.LevelIndex = (int)LogData.Node2Level.MESSAGE;
                    SendParm.EventID = ((int)LogData.Node2Level.MESSAGE).ToString();
                    SendParm.Keywords = "配置参数保存";
                    SendParm.EventData = "配置参数保存功";
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
                }
            }
        }
        private void btnSaveCNC_Click(object sender, EventArgs e)
        {
            SaveDgvData2XmlAndFile(0, false);
        }
        private void btnSaveROBOT_Click(object sender, EventArgs e)
        {
            SaveDgvData2XmlAndFile(0, false);
        }
        private void btnSavePLC_Click(object sender, EventArgs e)
        {
            SaveDgvData2XmlAndFile(0, false);
        }

        #region 获取和设置RFID参数
        /// <summary>
        /// 获取RFID通讯参数信息
        /// </summary>
        /// <param name="ReaderID">读写器ID</param>
        /// <param name="ipAddr">读写器ip地址</param>
        /// <param name="mark">子网掩码</param>
        /// <param name="gateway">网关</param>
        /// <param name="mac">MAC</param>
        //public void GetCommcfg()
        //{
        //    CommCfg cfg = new CommCfg();
        //    for (int jj = 0; jj < dgvRFIDSet.Rows.Count; jj++)
        //    {
        //        ReaderID[jj] = ConvertMethod.HexStringToSingleByte(dgvRFIDSet.Rows[jj].Cells[1].Value.ToString(), 0);
        //        if (SGreader.GetCommCfg(ReaderID[jj], ref cfg) == Status_enum.SUCCESS)
        //        {
        //            dgvRFIDSet.Rows[jj].Cells[2].Value = cfg.Mask; ;//子网掩码
        //            dgvRFIDSet.Rows[jj].Cells[3].Value = cfg.GateWay;//网关               
        //            dgvRFIDSet.Rows[jj].Cells[4].Value = cfg.IPAddr;//IP
        //            dgvRFIDSet.Rows[jj].Cells[6].Value = ConvertMethod.MacToString(cfg.MAC);//MAC
        //        }
        //    }
        //}
        ///// <summary>
        /// 设置RFID通讯参数
        /// </summary>
        //public void SetCommcfg()
        //{
        //    CommCfg cfg = new CommCfg();
        //    for (int jj = 0; jj < dgvRFIDSet.Rows.Count; jj++)
        //    {
        //        ReaderID[jj] = ConvertMethod.HexStringToSingleByte(dgvRFIDSet.Rows[jj].Cells[1].Value.ToString(), 0);
        //        if (SGreader.SetCommCfg(ReaderID[jj], cfg) == Status_enum.SUCCESS)
        //        {
        //            cfg.Mask = dgvRFIDSet.Rows[jj].Cells[2].Value.ToString();//子网掩码
        //            cfg.GateWay = dgvRFIDSet.Rows[jj].Cells[3].Value.ToString();//网关                
        //            cfg.IPAddr = dgvRFIDSet.Rows[jj].Cells[4].Value.ToString();//IP
        //            cfg.MAC = ConvertMethod.StringToMac(dgvRFIDSet.Rows[jj].Cells[6].Value.ToString());//MAC
        //        }
        //    }
        //}
        #endregion
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //2017.4.25 zxl
        //    for (int i = 0; i < dgvRFIDSet.Rows.Count; i++)
        //    {
        //        ip[i] = dgvRFIDSet.Rows[i].Cells[13].Value.ToString();//读写器ip
        //        port[i] = dgvRFIDSet.Rows[i].Cells[14].Value.ToString();//端口
        //        if (SGreader.Connect(ip[i], int.Parse(port[i])))
        //        {
        //            MessageBox.Show("读写器连接成功!");
        //            dgvRFIDSet.Rows[i].Cells[i].Value = "";
        //            //GetCommcfg();//获取通讯参数
        //            SetCommcfg();//设置通讯参数
        //        }
        //        else
        //        {
        //            MessageBox.Show("读写器连接失败.请确保参数地址和线缆连接正确.");
        //        }
        //    }
        //}

        private void btnSaveRFID_Click(object sender, EventArgs e)
        {
            SaveDgvData2XmlAndFile(0, false);
        }

        private void btnSaveMeasure_Click(object sender, EventArgs e)
        {
            SaveDgvData2XmlAndFile(0, false);
        }
        //private void button_savePLCAlarmTb_Click(object sender, EventArgs e)
        //{
        //    SaveDgvData2XmlAndFile(0);
        //}
        //private void btnSaveCheckEqment_Click(object sender, EventArgs e)
        //{
        //    SaveDgvData2XmlAndFile(0);
        //}

        //总数的修改和保存
        private void setSave_Click(object sender, EventArgs e)
        {
            if (MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[2]) != txtWSSet.Text ||
                MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[3]) != txtPLSet.Text ||
                MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_RFID[2]) != txtWSSet.Text ||
                MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_RFID[3]) != txtPLSet.Text ||
                MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, -1, m_xmlDociment.Default_Attributes_str3[2]) != txtWSSet.Text ||
                MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, -1, m_xmlDociment.Default_Attributes_str3[3]) != txtPLSet.Text ||
                //(dgvPLCSet.CurrentRow != null && MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC_Item + dgvPLCSet.CurrentRow.Index.ToString(),
                //                            -1, m_xmlDociment.Default_Attributes_str1[5]) != comboBoxSelePLCSystem.Text) ||
               MainForm.m_xml.m_Read(m_xmlDociment.Path_linetype, -1, m_xmlDociment.Default_Attributes_linetype[0]) != comboBoxLineType.Text ||
                MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]) != comboBox_LocalIpAddr.Text)
            {
                //if (MessageBox.Show("设置数据已经更新，是否确认保存？", " 提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                message = new MessageForm(ChangeLanguage.GetString("MessageFormTitle01"), ChangeLanguage.GetString("MessageFormContent001"), ChangeLanguage.GetString("MessageFormButton01"), ChangeLanguage.GetString("MessageFormButton02"));
                message.ShowDialog();
                if (message.DialogResult == DialogResult.OK)
                {
                    if (MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[2]) != txtWSSet.Text)
                    {
                        MainForm.m_xml.ChangeDefault_AllAttributes(m_xmlDociment.Default_Attributes_str1[2], txtWSSet.Text);
                    }
                    if (MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[3]) != txtPLSet.Text)
                    {
                        MainForm.m_xml.ChangeDefault_AllAttributes(m_xmlDociment.Default_Attributes_str1[3], txtPLSet.Text);
                    }
                    if (MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, -1, m_xmlDociment.Default_Attributes_str3[2]) != txtWSSet.Text)
                    {
                        MainForm.m_xml.ChangeDefault_AllAttributes(m_xmlDociment.Default_Attributes_str3[2], txtWSSet.Text);
                    }
                    if (MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_Measure, -1, m_xmlDociment.Default_Attributes_str3[3]) != txtPLSet.Text)
                    {
                        MainForm.m_xml.ChangeDefault_AllAttributes(m_xmlDociment.Default_Attributes_str3[3], txtPLSet.Text);
                    }
                    //if (dgvPLCSet.CurrentRow != null)
                    //{
                    //    if (MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC_Item + dgvPLCSet.CurrentRow.Index.ToString(),
                    //                        -1, m_xmlDociment.Default_Attributes_str1[5]) != comboBoxSelePLCSystem.Text)//说明PLC系统被改变
                    //    {
                    //        MainForm.m_xml.ChangeDefault_PlcSystemAttributes(dgvPLCSet.CurrentRow.Index, comboBoxSelePLCSystem.Text);
                    //        refreshXmlData2DGV(m_xmlDociment.PathRoot_PLC, dgvPLCSet);
                    //        BindingLisReadValue(dgvPLCSet);
                    //        UpDataPLCItemMessge(dgvPLCSet.CurrentRow.Index);
                    //    }
                    //}
                    if (MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0]) != comboBox_LocalIpAddr.Text)//CNC使用的本地
                    {
                        MainForm.m_xml.m_UpdateAttribute(m_xmlDociment.PathRoot_CNCLocalIp, -1, m_xmlDociment.Default_Attributes_CNCLocalIp[0], comboBox_LocalIpAddr.Text);
                    }
                    if (MainForm.m_xml.m_Read(m_xmlDociment.Path_linetype, -1, m_xmlDociment.Default_Attributes_linetype[0]) != comboBoxLineType.Text)
                    {
                        MainForm.m_xml.m_UpdateAttribute(m_xmlDociment.Path_linetype, -1, m_xmlDociment.Default_Attributes_linetype[0], comboBoxLineType.Text);
                    }

                    MainForm.m_xml.SaveXml2File(MainForm.XMLSavePath);
                }
                else
                {
                }
                //                 MainForm.m_xml.SaveXml2File(MainForm.XMLSavePath);
            }
            return;
        }
        #endregion

        #region      客户切换界面
        private void tabControlSet_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // SaveDgvData2XmlAndFile(0,true);
        }
        private void tabControlSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlSet.SelectedIndex)
            {
                case 1:
                    txtCNCSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[0]);//CNC数量
                    txtRobotSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_ROBOT, -1, m_xmlDociment.Default_Attributes_str1[0]);//机器人数量
                    txtPLCSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, -1, m_xmlDociment.Default_Attributes_str1[0]);//PLC数量
                    txtRFIDSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_RFID, -1, m_xmlDociment.Default_Attributes_str1[0]);//RFIDs数量
                    //车间和产线的显示  
                    txtWSSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[2]);//车间标识
                    txtPLSet.Text = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_CNC, -1, m_xmlDociment.Default_Attributes_str1[3]);//产线标识
                    break;
                case 2:
                    refreshXmlData2DGV(m_xmlDociment.PathRoot_CNC, dgvCNCSet);
                    BindingLisReadValue(dgvCNCSet);
                    break;
                    //case 2:
                    //    refreshXmlData2DGV(m_xmlDociment.PathRoot_ROBOT, dgvRobotSet);
                    //    BindingLisReadValue(dgvRobotSet);

                    //String pathstr = m_xmlDociment.PathRoot_ROBOT_Item + dgvRobotSet.CurrentRow.Index.ToString()
                    //                + "/" + m_xmlDociment.Default_Path_str[6];
                    //refreshXmlData2DGV(pathstr, DGVRobotInputSignalS);
                    //BindingLisReadValue(DGVRobotInputSignalS);

                    //pathstr = m_xmlDociment.PathRoot_ROBOT_Item + dgvRobotSet.CurrentRow.Index.ToString()
                    //           + "/" + m_xmlDociment.Default_Path_str[7];
                    //refreshXmlData2DGV(pathstr, DGVRobotOutputSignalS);
                    //BindingLisReadValue(DGVRobotOutputSignalS);
                    break;
                //case 3:
                //    refreshXmlData2DGV(m_xmlDociment.PathRoot_PLC, dgvPLCSet);
                //    BindingLisReadValue(dgvPLCSet);

                //    //    if (dgvPLCSet.CurrentRow != null)
                //    //    {
                //    //        UpDataPLCItemMessge(dgvPLCSet.CurrentRow.Index);
                //    //    }
                //    break;
                case 4:
                    refreshXmlData2DGV(m_xmlDociment.PathRoot_RFID, dgvRFIDSet);
                    BindingLisReadValue(dgvRFIDSet);
                    break;
                case 5:
                    refreshXmlData2DGV(m_xmlDociment.PathRoot_Measure, dgvMeasureSet);
                    BindingLisReadValue(dgvMeasureSet);
                    break;
                //case 6:
                //    refreshXmlData2DGV(m_xmlDociment.PathRoot_PLCAlarmTb, dgv_PLCAlarmTb);
                //    BindingLisReadValue(dgv_PLCAlarmTb);
                //    break;
                default:
                    break;
            }
        }

        #endregion

        #region   dgvCNCSet数据合法性检查
        string dgvCNCSet_Cell_Old;
        private void dgvCNCSet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgvCNCSet_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                dgvCNCSet_Cell_Old = "";
            }
        }
        private void dgvCNCSet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, btnSaveCNC))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvCNCSet_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvCNCSet_Cell_Old;
                    return;
                }
                if (e.ColumnIndex == (int)m_xmlDociment.Attributes_str1.port)//端口合法性检查
                {
                    if (int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 1024)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvCNCSet_Cell_Old;
                        return;
                    }
                }
                //                 else if (e.ColumnIndex == 1)//ID合法性检查
                //                 {
                //                     if (int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 0)
                //                     {
                //                         dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvCNCSet_Cell_Old;
                //                         return;
                //                     }
                //                 }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_str1.ip)//IP合法性检查
                {
                    System.Net.IPAddress ip;
                    if (!System.Net.IPAddress.TryParse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out ip))
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvCNCSet_Cell_Old;
                        return;
                    }
                }
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.Equals(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, dgvCNCSet_Cell_Old))
                {
                    RefreshDgvDataChangeF(dgv);
                }
            }
            catch (System.Exception ex)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvCNCSet_Cell_Old;
            }
        }
        #endregion

        #region   dgvRobotSet数据合法性检查
        string dgvRobotSet_Cell_Old;
        private void dgvRobotSet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgvRobotSet_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                dgvRobotSet_Cell_Old = "";
            }
        }
        private void dgvRobotSet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, btnSaveROBOT))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRobotSet_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRobotSet_Cell_Old;
                    return;
                }

                if (e.ColumnIndex == (int)m_xmlDociment.Attributes_str1.port)//端口合法性检查
                {
                    if (int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 1024)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRobotSet_Cell_Old;
                        return;
                    }
                }
                //                 else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_str1.id)//ID合法性检查
                //                 {
                //                     if (int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 0)
                //                     {
                //                         dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRobotSet_Cell_Old;
                //                         return;
                //                     }
                //                 }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_str1.ip)//IP合法性检查
                {
                    System.Net.IPAddress ip;
                    if (!System.Net.IPAddress.TryParse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out ip))
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRobotSet_Cell_Old;
                        return;
                    }
                }
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.Equals(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, dgvRobotSet_Cell_Old))
                {
                    RefreshDgvDataChangeF(dgv);
                }
            }
            catch (System.Exception ex)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRobotSet_Cell_Old;
            }
        }
        #endregion

        #region   dgvPLCSet数据合法性检查
        string dgvPLCSet_Cell_Old;
        private void dgvPLCSet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvPLCSet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, btnSavePLC))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCSet_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCSet_Cell_Old;
                    return;
                }

                if (e.ColumnIndex == (int)m_xmlDociment.Attributes_str1.port)//端口合法性检查
                {
                    if (int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 1024)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCSet_Cell_Old;
                        return;
                    }
                }
                //                 else if (e.ColumnIndex == 1)//ID合法性检查
                //                 {
                //                     if (int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 0)
                //                     {
                //                         dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCSet_Cell_Old;
                //                         return;
                //                     }
                //                 }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_str1.ip)//IP合法性检查
                {
                    System.Net.IPAddress ip;
                    if (!System.Net.IPAddress.TryParse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out ip))
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCSet_Cell_Old;
                        return;
                    }
                }
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.Equals(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, dgvPLCSet_Cell_Old))
                {
                    RefreshDgvDataChangeF(dgv);
                }
            }
            catch (System.Exception ex)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCSet_Cell_Old;
            }
        }
        #endregion

        #region   dgvRFIDSet数据合法性检查
        string dgvRFIDSet_Cell_Old;
        private void dgvRFIDSet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgvRFIDSet_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                dgvRFIDSet_Cell_Old = "";
            }
        }
        private void dgvRFIDSet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, btnSaveRFID))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                    return;
                }
                String PLCSystem = MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, int.Parse(dgv.Rows[e.RowIndex].Cells[4].Value.ToString()), m_xmlDociment.Default_Attributes_str1[5]);
                if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.PLCserial)//所属PLC的序号合法性检查
                {
                    Int32 PLCSel = int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    if (PLCSel < 0 || PLCSel >= Int32.Parse(MainForm.m_xml.m_Read(m_xmlDociment.PathRoot_PLC, -1, m_xmlDociment.Default_Attributes_str1[0])))
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                        return;
                    }
                }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.ReadDevice
                    || e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.WriteDevice)//地址类型合法性检查
                {
                    bool find = false;
                    if (PLCSystem == m_xmlDociment.PLC_System[0])
                    {
                        if (m_xmlDociment.Default_MITSUBISHI_Device1.Contains(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                        {
                            find = true;
                        }
                        if (m_xmlDociment.Default_MITSUBISHI_Device2.Contains(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                        {
                            find = true;
                        }
                    }
                    else if (PLCSystem == m_xmlDociment.PLC_System[1])
                    {
                        if (m_xmlDociment.Default_HNC8_Device1.Contains(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                        {
                            find = true;
                        }
                        if (m_xmlDociment.Default_HNC8_Device2.Contains(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                        {
                            find = true;
                        }
                    }

                    if (!find)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                        return;
                    }
                }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.ReadAddressStar ||
                    e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.WriteAddressStar)//起始地址
                {
                    String PLCDevice = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString();
                    String[] AdressType = MainForm.m_xml.FindDeviceAddress(ref PLCDevice, ref PLCSystem).Split('-');
                    if (PLCSystem == m_xmlDociment.PLC_System[0])//三菱地址合法性检查
                    {
                        //                         Int32 Adreaaii = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), Int32.Parse(AdressType[0]));
                        //                         Int32 Adreaaiinim = Convert.ToInt32(AdressType[1], Int32.Parse(AdressType[0]));
                        //                         Int32 Adreaaiimax = Convert.ToInt32(AdressType[2], Int32.Parse(AdressType[0]));
                        // 
                        //                         if (Adreaaii < Adreaaiinim || Adreaaii >= Adreaaiimax)
                        //                         {
                        //                             dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                        //                             return;
                        //                         }
                    }
                    else if (PLCSystem == m_xmlDociment.PLC_System[1])
                    {
                        //                         String[] AdressSplit = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split('.');
                        //                         if (AdressSplit.Length == 1)
                        //                         {
                        //                             if (Int32.Parse(AdressSplit[0]) < Int32.Parse(AdressType[1]) ||
                        //                                 Int32.Parse(AdressSplit[0]) >= Int32.Parse(AdressType[2]))
                        //                             {
                        //                                 dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                        //                                 return;
                        //                             }
                        //                         }
                        //                         else if (AdressSplit.Length == 2)
                        //                         {
                        //                             if (Int32.Parse(AdressSplit[1]) < 0 || Int32.Parse(AdressSplit[1]) >= Int32.Parse(AdressType[0]) ||
                        //                                 Int32.Parse(AdressSplit[0]) < Int32.Parse(AdressType[1]) ||
                        //                                 Int32.Parse(AdressSplit[0]) >= Int32.Parse(AdressType[2]))
                        //                             {
                        //                                 dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                        //                                 return;
                        //                             }
                        //                         }
                        //                         else
                        //                         {
                        //                             dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                        //                             return;
                        //                         }
                    }
                }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.ReadAddressSet
                    || e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.WriteAddressSet)//地址地址格式
                {
                    String[] str = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split(',');
                    String PLCDevice = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex - 2].Value.ToString();
                    String[] AdressType = MainForm.m_xml.FindDeviceAddress(ref PLCDevice, ref PLCSystem).Split('-');
                    Int32 Adreaaiinim = Convert.ToInt32(AdressType[1], Int32.Parse(AdressType[0]));
                    Int32 Adreaaiimax = Convert.ToInt32(AdressType[2], Int32.Parse(AdressType[0]));
                    Int32 add = 0;
                    for (int ii = 0; ii < str.Length; ii++)
                    {
                        int strii = Int32.Parse(str[ii]);
                        add += strii;
                        if (strii < Adreaaiinim || strii >= Adreaaiimax || add >= Adreaaiimax)
                        {
                            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                            return;
                        }
                    }
                }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.MonitorBit)//监控位
                {

                }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.ip)//IP合法性检查
                {
                    System.Net.IPAddress ip;
                    if (!System.Net.IPAddress.TryParse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out ip))
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                        return;
                    }
                }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.port)//端口合法性检查
                {
                    if (int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 1024)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
                        return;
                    }
                }
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.Equals(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, dgvRFIDSet_Cell_Old))
                {
                    RefreshDgvDataChangeF(dgv);
                }
            }
            catch (System.Exception ex)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvRFIDSet_Cell_Old;
            }
        }
        #endregion

        #region   dgvMeasureSet数据合法性检查
        string dgvMeasureSet_Cell_Old;
        private void dgvMeasureSet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgvMeasureSet_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                dgvMeasureSet_Cell_Old = "";
            }
        }
        private void dgvMeasureSet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, btnSaveMeasure))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvMeasureSet_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvMeasureSet_Cell_Old;
                    return;
                }
                if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.ip)//IP合法性检查
                {
                    System.Net.IPAddress ip;
                    if (!System.Net.IPAddress.TryParse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out ip))
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvMeasureSet_Cell_Old;
                        return;
                    }
                }
                else if (e.ColumnIndex == (int)m_xmlDociment.Attributes_RFID.port)//端口合法性检查
                {
                    if (int.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 1024)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvMeasureSet_Cell_Old;
                        return;
                    }
                }

                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.Equals(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, dgvMeasureSet_Cell_Old))
                {
                    RefreshDgvDataChangeF(dgv);
                }
            }
            catch (System.Exception ex)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvMeasureSet_Cell_Old;
            }
        }
        #endregion

        #region   DGVRobotInputSignalS数据合法性检查
        string DGVRobotInputSignalS_Cell_Old;
        private void DGVRobotInputSignalS_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, btnSaveROBOT))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotInputSignalS_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotInputSignalS_Cell_Old;
                    return;
                }

                //                 if (e.ColumnIndex == 1)//16进制地址合法性检查
                //                 {
                //                     if (Convert.ToInt64(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), 16) < 0)
                //                     {
                //                         dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotInputSignalS_Cell_Old;
                //                         return;
                //                     }
                //                 }
                //                 else if (e.ColumnIndex == 4 || e.ColumnIndex == 5)//ID合法性检查
                //                 {
                //                     if (Int64.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 0)
                //                     {
                //                         dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotInputSignalS_Cell_Old;
                //                         return;
                //                     }
                //                 }
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.Equals(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, DGVRobotInputSignalS_Cell_Old))
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split('\t')[0];
                    RefreshDgvDataChangeF(dgv);
                }
            }
            catch (System.Exception ex)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotInputSignalS_Cell_Old;
            }
        }
        private void DGVRobotInputSignalS_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                DGVRobotInputSignalS_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                DGVRobotInputSignalS_Cell_Old = "";
            }
        }
        #endregion

        #region   DGVRobotOutputSignalS数据合法性检查
        string DGVRobotOutputSignalS_Cell_Old;
        private void DGVRobotOutputSignalS_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                DGVRobotOutputSignalS_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                DGVRobotOutputSignalS_Cell_Old = "";
            }
        }
        private void DGVRobotOutputSignalS_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, btnSaveROBOT))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotOutputSignalS_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotOutputSignalS_Cell_Old;
                    return;
                }

                //                 if (e.ColumnIndex == 1)//16进制地址合法性检查
                //                 {
                //                     if (Convert.ToInt64(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), 16) < 0)
                //                     {
                //                         dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotOutputSignalS_Cell_Old;
                //                         return;
                //                     }
                //                 }
                //                 else if (e.ColumnIndex == 4 || e.ColumnIndex == 5)//ID合法性检查
                //                 {
                //                     if (Int64.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 0)
                //                     {
                //                         dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotOutputSignalS_Cell_Old;
                //                         return;
                //                     }
                //                 }
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DGVRobotOutputSignalS_Cell_Old && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    RefreshDgvDataChangeF(dgv);
                }
            }
            catch (System.Exception ex)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGVRobotOutputSignalS_Cell_Old;
            }
        }
        #endregion

        /*
        #region   PLCInputSignalSDefine数据合法性检查
        string PLCInputSignalSDefine_Cell_Old;
        private void PLCInputSignalSDefine_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                PLCInputSignalSDefine_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                PLCInputSignalSDefine_Cell_Old = "";
            }
        }
        private void PLCInputSignalSDefine_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, btnSavePLC))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCInputSignalSDefine_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCInputSignalSDefine_Cell_Old;
                    return;
                }

                if (e.ColumnIndex == 1)//地址合法性检查
                {*/
        /*
        String PLCDevice = comboBoxPLCDevice1.Text;
        String PLCSystem = comboBoxSelePLCSystem.Text;
        String[] AdressType = MainForm.m_xml.FindDeviceAddress(ref PLCDevice, ref PLCSystem).Split('-');
        if (PLCSystem == m_xmlDociment.PLC_System[0])//三菱地址合法性检查
        {
            Int32 Adreaaii = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), Int32.Parse(AdressType[0]));
            Int32 Adreaaiinim = Convert.ToInt32(AdressType[1], Int32.Parse(AdressType[0]));
            Int32 Adreaaiimax = Convert.ToInt32(AdressType[2], Int32.Parse(AdressType[0]));

            if (Adreaaii < Adreaaiinim || Adreaaii >= Adreaaiimax)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCInputSignalSDefine_Cell_Old;
                return;
            }
        }
        else if (PLCSystem == m_xmlDociment.PLC_System[1])
        {
            String[] AdressSplit = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split('.');
            if (AdressSplit.Length == 1)
            {
                if (Int32.Parse(AdressSplit[0]) < Int32.Parse(AdressType[1]) ||
                    Int32.Parse(AdressSplit[0]) >= Int32.Parse(AdressType[2]))
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCInputSignalSDefine_Cell_Old;
                    return;
                }
            }
            else if (AdressSplit.Length == 2)
            {
                if (Int32.Parse(AdressSplit[1]) < 0 || Int32.Parse(AdressSplit[1]) >= Int32.Parse(AdressType[0])||
                    Int32.Parse(AdressSplit[0]) < Int32.Parse(AdressType[1]) || 
                    Int32.Parse(AdressSplit[0]) >= Int32.Parse(AdressType[2]))
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCInputSignalSDefine_Cell_Old;
                    return;
                }
            }
            else
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCInputSignalSDefine_Cell_Old;
                return;
            }
        }*/
        /*
 }
//                 else if (e.ColumnIndex == 4 || e.ColumnIndex == 5)//ID合法性检查
//                 {
//                     if (Int64.Parse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 0)
//                     {
//                         dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCInputSignalSDefine_Cell_Old;
//                         return;
//                     }
//                 }
 if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != PLCInputSignalSDefine_Cell_Old && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
 {
     RefreshDgvDataChangeF(dgv);
 }
}
catch (System.Exception ex)
{
 dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCInputSignalSDefine_Cell_Old;
}
}
#endregion
*/
        /*
                #region   PLCOutputSignalSDefine数据合法性检查
                string PLCOutputSignalSDefine_Cell_Old;
                private void PLCOutputSignalSDefine_CellEnter(object sender, DataGridViewCellEventArgs e)
                {
                    if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        PLCOutputSignalSDefine_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                    else
                    {
                        PLCOutputSignalSDefine_Cell_Old = "";
                    }
                }
        
                private void PLCOutputSignalSDefine_CellEndEdit(object sender, DataGridViewCellEventArgs e)
                {
                    DataGridView dgv = (DataGridView)sender;

                    if (!CheckUserPassWord(sender, btnSavePLC))
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCOutputSignalSDefine_Cell_Old;
                        return;
                    }

                    try
                    {
                        if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                        {
                            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCOutputSignalSDefine_Cell_Old;
                            return;
                        }
                        if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != PLCOutputSignalSDefine_Cell_Old && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            RefreshDgvDataChangeF(dgv);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PLCOutputSignalSDefine_Cell_Old;
                    }
                }
                #endregion*/

        /*
        #region   dgvPLCAlarmTb数据合法性检查
        string dgvPLCAlarmTb_Cell_Old;
        private void dgv_PLCAlarmTb_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgvPLCAlarmTb_Cell_Old = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                dgvPLCAlarmTb_Cell_Old = "";
            }
        }
        private void dgv_PLCAlarmTb_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!CheckUserPassWord(sender, button_savePLCAlarmTb))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCAlarmTb_Cell_Old;
                return;
            }

            try
            {
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCAlarmTb_Cell_Old;
                    return;
                }
                if (e.ColumnIndex == 1)//报警号
                {
                    int outint = 0;
                    if (!int.TryParse(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),out outint))
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCAlarmTb_Cell_Old;
                        return;
                    }
                }
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != dgvPLCAlarmTb_Cell_Old && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    RefreshDgvDataChangeF(dgv);
                }

            }
            catch (System.Exception ex)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvPLCAlarmTb_Cell_Old;
            }
        }

        #endregion
        */

        #region   dgv初始化
        private void InitdgvSet(ref DataGridView dgv, ref String[] dgvRFIDSetColumnAttributes_str)
        {
            dgv.ColumnCount = dgvRFIDSetColumnAttributes_str.Length;
            for (int ii = 0; ii < dgv.ColumnCount; ii++)
            {
                dgv.Columns[ii].HeaderText = dgvRFIDSetColumnAttributes_str[ii];
            }
        }
        #endregion

        private void dgvRobotSet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRobotSet.CurrentRow.Index != dgvRobotSet_CurrentRow_Index_Old)
            {
                String pathstr = m_xmlDociment.PathRoot_ROBOT_Item + dgvRobotSet.CurrentRow.Index.ToString()
                    + "/" + m_xmlDociment.Default_Path_str[6];
                if (dgvRobotSet.CurrentRow.Index < dgvRobotSet.RowCount - 1)
                {
                    if (!MainForm.m_xml.CheckNodeExist(pathstr))
                    {
                        SaveDgvData2XmlAndFile(1, true);
                    }
                }
                //refreshXmlData2DGV(pathstr, DGVRobotInputSignalS);
                //RefreshDgvXmlPath(DGVRobotInputSignalS, dgvRobotSet.CurrentRow.Index);
                //BindingLisReadValue(DGVRobotInputSignalS);

                pathstr = m_xmlDociment.PathRoot_ROBOT_Item + dgvRobotSet.CurrentRow.Index.ToString()
                   + "/" + m_xmlDociment.Default_Path_str[7];
                //refreshXmlData2DGV(pathstr, DGVRobotOutputSignalS);
                //RefreshDgvXmlPath(DGVRobotOutputSignalS, dgvRobotSet.CurrentRow.Index);
                //BindingLisReadValue(DGVRobotOutputSignalS);

                //label_SeleRobotIntS.Text = "序号为" + dgvRobotSet.CurrentRow.Index.ToString() + "的机器人的输入信号数量：";
                //label_SeleRobotOutS.Text = "序号为" + dgvRobotSet.CurrentRow.Index.ToString() + "的机器人的输出信号数量：";
                dgvRobotSet_CurrentRow_Index_Old = dgvRobotSet.CurrentRow.Index;
            }
        }

        /*
        private void dgvPLCSet_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPLCSet.CurrentRow != null && dgvPLCSet_CurrentRow_Index_Old != e.RowIndex)
            {
                UpDataPLCItemMessge(e.RowIndex);
                dgvPLCSet_CurrentRow_Index_Old = e.RowIndex;
            }
        }
        private void comboBoxSelePLCSystem_Leave(object sender, EventArgs e)
        {
            if(dgvPLCSet.CurrentRow != null)
            {
                SaveDgvData2XmlAndFile(0);
            }
        }

        private Int32 PLCIdex_old = -1;
        private void UpDataPLCItemMessge(Int32 PLCIdex)
        {
            String Path = m_xmlDociment.PathRoot_PLC_Item +  PLCIdex;
            if(MainForm.m_xml.CheckNodeExist(Path))
            {
                String PlcSystem = MainForm.m_xml.m_Read(Path, -1, m_xmlDociment.Default_Attributes_str1[5]);
                UpDataPLCItemystemShow(PlcSystem);
                RefreshDgvXmlPath(PLCInputSignalSDefine, PLCIdex);
                RefreshDgvXmlPath(PLCOutputSignalSDefine, PLCIdex);

                String pathstr = Path + "/" + comboBoxPLCDevice1.Text;//MainForm.m_xml.Default_Path_str[6];
                refreshXmlData2DGV(pathstr, PLCInputSignalSDefine);
                BindingLisReadValue(PLCInputSignalSDefine);

                pathstr = Path + "/" + comboBoxPLCDevice2.Text;//MainForm.m_xml.Default_Path_str[7];
                refreshXmlData2DGV(pathstr, PLCOutputSignalSDefine);
                BindingLisReadValue(PLCOutputSignalSDefine);

                LablPLCXuHao1.Text = "PLC序号为: " + PLCIdex.ToString();
                if (PLCIdex_old != PLCIdex)
                {
                    comboBoxPLCDevice1_SelectedIndex_Old = 0;
                    comboBoxPLCDevice2_SelectedIndex_Old = 0;
                    PLCIdex_old = PLCIdex;
                }

                comboBoxPLCDevice1.SelectedIndex = comboBoxPLCDevice1_SelectedIndex_Old;
                comboBoxPLCDevice2.SelectedIndex = comboBoxPLCDevice2_SelectedIndex_Old;

                if (PlcSystem == m_xmlDociment.PLC_System[0])///初始化comboBoxPLCDevice1、comboBoxPLCDevice2
                {
                    comboBoxPLCDevice1.DataSource = m_xmlDociment.Default_MITSUBISHI_Device1;
                    comboBoxPLCDevice2.DataSource = m_xmlDociment.Default_MITSUBISHI_Device2;
                }
                else if (PlcSystem == m_xmlDociment.PLC_System[1])
                {
                    comboBoxPLCDevice1.DataSource = m_xmlDociment.Default_HNC8_Device1;
                    comboBoxPLCDevice2.DataSource = m_xmlDociment.Default_HNC8_Device2;
                }
                 
                if (comboBoxPLCDevice1.Text == m_xmlDociment.Default_MITSUBISHI_Device1[2])//Buffer
                {
                    labeliStartIO.Visible = true;
                    textBoxStartIO.Visible = true;
                }
                else
                {
                    labeliStartIO.Visible = false;
                    textBoxStartIO.Visible = false;
                }
            }
            else
            {
                SaveDgvData2XmlAndFile(0);
                UpDataPLCItemMessge(PLCIdex);
            }
        }

        /// <summary>
        /// 更新comboBoxSelePLCSystem选择数据
        /// </summary>
        /// <param name="PLCIdex"></param>
        private void UpDataPLCItemystemShow(String PLCSystem)
        {
            for (int ii = 0; ii < comboBoxSelePLCSystem.Items.Count; ii++)
            {
                comboBoxSelePLCSystem.SelectedIndex = ii;
                if (PLCSystem == comboBoxSelePLCSystem.Text)
                    break;
            }
        }
        */

        /// <summary>
        /// 将Xml数据刷新到DGV
        /// </summary>
        /// <param name="Pathstr"></param>
        /// <param name="DGV"></param>
        private void refreshXmlData2DGV(string Pathstr, DataGridView DGV)
        {
            DGV.Rows.Clear();
            if (MainForm.m_xml.CheckNodeExist(Pathstr))
            {
                int ITSUM = int.Parse(MainForm.m_xml.m_Read(Pathstr, -1, m_xmlDociment.Default_Attributes_str1[0]));//SUM
                for (int ii = 0; ii < ITSUM; ii++)
                {
                    DataGridViewRow InserRows = new DataGridViewRow();
                    InserRows.CreateCells(DGV);
                    string inserPath = Pathstr + "/" + m_xmlDociment.Default_Path_str[5] + ii.ToString();
                    MainForm.m_xml.Attributes2GridViewRow(inserPath, ref InserRows);
                    DGV.Rows.Add(InserRows);
                }
            }
        }


        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="Pathstr"></param>
        private void refreshDgvData2Xml(DataGridView DGV, string Pathstr)
        {
            if (!MainForm.m_xml.CheckNodeExist(Pathstr))// && (DGV == PLCInputSignalSDefine || DGV == PLCOutputSignalSDefine))//节点不存在，新建
            {
                MainForm.m_xml.MakeXmlPLCDevicePath(Pathstr);
            }
            int ITemSUM = int.Parse(MainForm.m_xml.m_Read(Pathstr, -1, m_xmlDociment.Default_Attributes_str1[0]));//SUM
            if (DGV.Rows.Count > ITemSUM)//插入
            {
                for (int kk = 0; kk < (DGV.Rows.Count - ITemSUM); kk++)
                {
                    MainForm.m_xml.InserNode(Pathstr);
                }
            }
            else if (DGV.Rows.Count < ITemSUM)
            {
                for (int kk = 0; kk < (ITemSUM - DGV.Rows.Count); kk++)
                {
                    MainForm.m_xml.DeleNode(Pathstr, int.Parse(MainForm.m_xml.m_Read(Pathstr, -1, m_xmlDociment.Default_Attributes_str1[0])) - 1);
                }
            }
            for (int jj = 0; jj < DGV.Rows.Count; jj++)
            {
                string Itempas = Pathstr + "/" + m_xmlDociment.Default_Path_str[5] + jj.ToString();
                DataGridViewRow r = DGV.Rows[jj];
                MainForm.m_xml.GridViewRow2XmlAttributes(Itempas, ref r);
            }
        }

        #region   右击弹出快捷菜单的响应函数
        /// <summary>
        /// 右击弹出快捷菜单的响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip cms = (ContextMenuStrip)sender;
            DataGridView dgv = (DataGridView)(this.Controls.Find(cms.Name, true)[0]);
            int OpCount = dgv.SelectedRows.Count;
            if (OpCount > 0)
            {
                int OpBegin = 0;
                for (int ii = 0; ii < dgv.Rows.Count; ii++)
                {
                    if (dgv.Rows[ii].Selected == true)
                    {
                        OpBegin = ii;
                        break;
                    }
                }


                //if (dgv.Name == DGVRobotInputSignalS.Name || dgv.Name == DGVRobotOutputSignalS.Name)
                //{
                //    RefreshDgvXmlPath(dgv, dgvRobotSet.CurrentRow.Index);
                //}
                //else if (dgv.Name == PLCInputSignalSDefine.Name || dgv.Name == PLCOutputSignalSDefine.Name)
                //{
                //    RefreshDgvXmlPath(dgv, dgvPLCSet.CurrentRow.Index);
                //}
                if (cms.Items[0] == e.ClickedItem)//添加
                {
                    DataGridViewRow InserRows = new DataGridViewRow();
                    InserRows.CreateCells(dgv);
                    if (dgv.ColumnCount == m_xmlDociment.Default_Attributesstr1_value.Length)
                    {
                        InserRows.SetValues(m_xmlDociment.Default_Attributesstr1_value);
                    }
                    else if (dgv.ColumnCount == m_xmlDociment.Default_Attributesstr2_value.Length)
                    {
                        InserRows.SetValues(m_xmlDociment.Default_Attributesstr2_value);
                    }
                    else if (dgv.ColumnCount == m_xmlDociment.Default_Attributes_RFID_value.Length)
                    {
                        InserRows.SetValues(m_xmlDociment.Default_Attributes_RFID_value);
                    }
                    else if (dgv.ColumnCount == m_xmlDociment.Default_Attributesstr3_value.Length)
                    {
                        InserRows.SetValues(m_xmlDociment.Default_Attributesstr3_value);
                    }

                    dgv.Rows.Insert(OpBegin, InserRows);
                    for (int ii = OpBegin; ii < dgv.Rows.Count; ii++)
                    {
                        dgv.Rows[ii].Cells[0].Value = ii.ToString();
                    }
                    RefreshDgvDataChangeF(dgv);
                }
                else if (cms.Items[1] == e.ClickedItem && dgv.RowCount != 1)//删除
                {
                    if (OpCount == dgv.RowCount)
                    {
                        dgv.Rows[OpBegin].Selected = false;
                    }
                    for (int ii = dgv.SelectedRows.Count; ii > 0; ii--)
                    {
                        dgv.Rows.RemoveAt(dgv.SelectedRows[ii - 1].Index);
                    }
                    for (int ii = OpBegin; ii < dgv.RowCount; ii++)
                    {
                        dgv.Rows[ii].Cells[0].Value = ii.ToString();
                    }
                    RefreshDgvDataChangeF(dgv);
                }
                BindingLisReadValue(dgv);
            }
            dgv.ClearSelection();
        }


        /// <summary>
        /// 鼠标右击时调用函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = ((DataGridView)sender);
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                for (int ii = 0; ii < dgv.Rows.Count; ii++)
                {
                    for (int jj = 0; jj < dgv.ColumnCount; jj++)
                    {
                        if (dgv.Rows[ii].Cells[jj].Selected)
                        {
                            dgv.Rows[ii].Selected = true;
                            break;
                        }
                    }
                }
                ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
                contextMenuStrip1.Items.Add("添加");
                contextMenuStrip1.Items.Add("删除");
                contextMenuStrip1.Name = dgv.Name;
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            }
            else if (e.Button == MouseButtons.Left && e.RowIndex > -1
                && e.ColumnIndex == 3 && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null
                /*&& (//dgv.Name == PLCInputSignalSDefine.Name || dgv.Name == PLCOutputSignalSDefine.Name ||
                dgv.Name == DGVRobotInputSignalS.Name || dgv.Name == DGVRobotOutputSignalS.Name)*/)
            {
                //if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "否")
                //{
                //    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "是";
                //}
                //else
                //{
                //    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "否";
                //}
                RefreshDgvDataChangeF(dgv);
            }

        }

        /// <summary>
        /// 右击列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PLCInputSignalSDefine_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvMouseClick(sender, e);
        }

        private void PLCOutputSignalSDefine_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvMouseClick(sender, e);
        }

        private void dgvCNCSet_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvMouseClick(sender, e);
        }

        private void dgvRobotSet_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //             dgvMouseClick(sender, e);
        }

        private void DGVRobotInputSignalS_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvMouseClick(sender, e);
        }

        private void DGVRobotOutputSignalS_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvMouseClick(sender, e);
        }

        //        private void dgvPLCSet_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //        {
        ////             dgvMouseClick(sender, e);
        //        }

        private void dgvRFIDSet_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvMouseClick(sender, e);
        }
        private void dgvMeasureSet_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvMouseClick(sender, e);
        }
        //private void dgv_PLCAlarmTb_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    dgvMouseClick(sender, e);
        //}
        //private void dgvCheckEquementSet_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    dgvMouseClick(sender, e);
        //}

        #endregion
        /// <summary>
        /// 离开设置时检查是否需要保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetForm_Leave(object sender, EventArgs e)
        {
            SaveDgvData2XmlAndFile(0, true);
        }

        /*
        #region comboBoxPLCDevice操作
        private int comboBoxPLCDevice1_SelectedIndex_Old = -1;
        private void comboBoxPLCDevice1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvPLCSet.CurrentRow != null && comboBoxPLCDevice1_SelectedIndex_Old != comboBoxPLCDevice1.SelectedIndex)
            {
                String pathstr = m_xmlDociment.PathRoot_PLC_Item + dgvPLCSet.CurrentRow.Index.ToString()
                                + "/" + comboBoxPLCDevice1.Text;
                if (comboBoxPLCDevice1.Text == m_xmlDociment.Default_MITSUBISHI_Device1[2])//Buffer
                {
                    labeliStartIO.Visible = true;
                    textBoxStartIO.Visible = true;
                    if (MainForm.m_xml.CheckNodeExist(pathstr))
                    {
                        if (textBoxStartIO_Old.ToString() != MainForm.m_xml.m_Read(pathstr, -1, m_xmlDociment.Default_Attributes_str2[4]))
                        {
                            textBoxStartIO.Text = MainForm.m_xml.m_Read(pathstr, -1, m_xmlDociment.Default_Attributes_str2[4]);
                        }
                    }
                }
                else
                {
                    labeliStartIO.Visible = false;
                    textBoxStartIO.Visible = false;
                }

                SaveDgvData2XmlAndFile(0);
                refreshXmlData2DGV(pathstr, PLCInputSignalSDefine);
                BindingLisReadValue(PLCInputSignalSDefine);
                RefreshDgvXmlPath(PLCInputSignalSDefine, dgvPLCSet.CurrentRow.Index);
            }
            comboBoxPLCDevice1_SelectedIndex_Old = comboBoxPLCDevice1.SelectedIndex;
        }
        private int comboBoxPLCDevice2_SelectedIndex_Old = -1;
        private void comboBoxPLCDevice2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvPLCSet.CurrentRow != null && comboBoxPLCDevice2_SelectedIndex_Old != comboBoxPLCDevice2.SelectedIndex)
            {
                SaveDgvData2XmlAndFile(0);
                String pathstr = m_xmlDociment.PathRoot_PLC_Item + dgvPLCSet.CurrentRow.Index.ToString()
                           + "/" + comboBoxPLCDevice2.Text;
                refreshXmlData2DGV(pathstr, PLCOutputSignalSDefine);
                BindingLisReadValue(PLCOutputSignalSDefine);
                RefreshDgvXmlPath(PLCOutputSignalSDefine, dgvPLCSet.CurrentRow.Index);
            }
            comboBoxPLCDevice2_SelectedIndex_Old = comboBoxPLCDevice2.SelectedIndex;
        }
        #endregion

        private int textBoxStartIO_Old = 0;
        private void textBoxStartIO_TextChanged(object sender, EventArgs e)
        {
            if (textBoxStartIO.Text != textBoxStartIO_Old.ToString())
            {
                try
                {
                    int StartIO = int.Parse(textBoxStartIO.Text);
                    if (StartIO >= 0)
                    {
                        textBoxStartIO_Old = StartIO;
                    }
                    else
                    {
                        textBoxStartIO.Text = textBoxStartIO_Old.ToString();
                    }
                }
                catch
                {
                    textBoxStartIO.Text = textBoxStartIO_Old.ToString();
                }
            }
        }
        */

        #region 数量改变按回车事件
        private void textBox_CNCNUM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveCNC.Focus();
            }
        }
        private void textBox_ROBOTNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveROBOT.Focus();
            }
        }
        private void textBox_SeleRobotIntSNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveROBOT.Focus();
            }
        }
        private void textBox_SeleRobotOutSNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveROBOT.Focus();
            }
        }
        /*
        private void textBox_PLCDVGROSNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSavePLC.Focus();
            }
        }
        private void textBox__SelePLCIntDVGROSNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSavePLC.Focus();
            }
        }
        private void textBox__SelePLCOutDVGROSNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSavePLC.Focus();
            }
        }
        */
        private void textBox_RFidDgvRowsNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveRFID.Focus();
            }
        }

        private void textBox_MeasureDgvRowsNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveRFID.Focus();
            }
        }
        //private void textBox_CheckEqDgvRowsNum_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        btnSaveCheckEqment.Focus();
        //    }
        //}

        /*
        private void textBox_PLCAlarmNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_savePLCAlarmTb.Focus();
            }
        }
        */
        #endregion

        #region 用户权限管理操作
        /// <summary>
        /// 登陆或者注销响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_UserOnOrOff_Click(object sender, EventArgs e)
        {
            if (button_UserOnOrOff.Text == UserPasswordStrArr[(int)UserPasswordStrArrIndex.登陆])
            {
                if (textBox_UserPassword1.Visible)
                {
                    if (MainForm.m_xml.CheckUserPassword(comboBox_UserName.Text, textBox_UserPassword1.Text))
                    {
                        label_CurrentUsername.Text = comboBox_UserName.Text;
                        button_UserOnOrOff.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.注销];
                        MsgTipsHandler.BeginInvoke(label_Tisp, comboBox_UserName.Text + "：登陆成功！", null, null);
                        LogIn = true;
                        LogInUserName = comboBox_UserName.Text;
                    }
                    else
                    {
                        MsgTipsHandler.BeginInvoke(label_Tisp, comboBox_UserName.Text + "：登陆失败！", null, null);
                    }
                }
                else
                {
                    if (comboBox_UserName.SelectedIndex > 0)
                    {
                        label_CurrentUsername.Text = comboBox_UserName.Items[comboBox_UserName.SelectedIndex - 1].ToString();
                    }
                    else
                    {
                        label_CurrentUsername.Text = comboBox_UserName.Items[0].ToString();
                    }
                    MsgTipsHandler.BeginInvoke(label_Tisp, comboBox_UserName.Text + "：登陆成功！", null, null);
                    if (comboBox_UserName.SelectedIndex == 1)
                    {
                        LogIn = false;
                    }
                }
            }
            else if (button_UserOnOrOff.Text == UserPasswordStrArr[(int)UserPasswordStrArrIndex.注销])
            {
                label_CurrentUsername.Text = comboBox_UserName.Items[0].ToString();
                button_UserOnOrOff.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.登陆];
                MsgTipsHandler.BeginInvoke(label_Tisp, comboBox_UserName.Text + "：注销成功！", null, null);
                LogIn = false;
            }
            else if (button_UserOnOrOff.Text == UserPasswordStrArr[(int)UserPasswordStrArrIndex.取消])//取消修改密码
            {
                button_ChangeUserPassword.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.修改密码];
                button_UserOnOrOff.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.注销];
            }
            SetUserPasswoedItemShowOrHide();
        }

        private void textBox_UserPassword1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox_UserPassword2.Visible)
                {
                    textBox_UserPassword2.Focus();
                }
                else
                {
                    button_UserOnOrOff_Click(null, null);
                }
            }
        }
        private void textBox_UserPassword2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_UserOnOrOff_Click(null, null);
            }
        }


        /// <summary>
        /// 修改用户密码响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ChangeUserPassword_Click(object sender, EventArgs e)
        {
            if (button_ChangeUserPassword.Text == UserPasswordStrArr[(int)UserPasswordStrArrIndex.修改密码])
            {
                button_ChangeUserPassword.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.确定];
                button_UserOnOrOff.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.取消];
            }
            else if (button_ChangeUserPassword.Text == UserPasswordStrArr[(int)UserPasswordStrArrIndex.确定])
            {
                if (textBox_UserPassword1.Text == textBox_UserPassword2.Text && textBox_UserPassword1.Text.Length >= 8)
                {
                    button_ChangeUserPassword.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.修改密码];
                    button_UserOnOrOff.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.注销];
                    MainForm.m_xml.SetUserPassword(comboBox_UserName.Text, textBox_UserPassword1.Text);
                    MainForm.m_xml.SaveXml2File(MainForm.XMLSavePath);
                    MsgTipsHandler.BeginInvoke(label_Tisp, comboBox_UserName.Text + "：修改密码成功！", null, null);
                }
                else
                {
                    if (textBox_UserPassword1.Text == textBox_UserPassword2.Text && textBox_UserPassword1.Text.Length < 8)
                    {
                        MsgTipsHandler.BeginInvoke(label_Tisp, comboBox_UserName.Text + "：密码位数必须大于等于8！", null, null);
                    }
                    else
                    {
                        MsgTipsHandler.BeginInvoke(label_Tisp, comboBox_UserName.Text + "：第一次输入的密码和第二次输入的密码必须相同！", null, null);
                    }
                }
            }
            SetUserPasswoedItemShowOrHide();
        }

        private void SetUserPasswoedItemShowOrHide()
        {
            if (comboBox_UserName.Items.Count > 0)
            {
                if (label_CurrentUsername.Text == comboBox_UserName.Text)
                {
                    if (comboBox_UserName.SelectedIndex == 0)
                    {
                        label_UserName.Visible = true;
                        comboBox_UserName.Visible = true;

                        label_UserPasswor1.Visible = false;
                        label_UserPasswor2.Visible = false;
                        textBox_UserPassword1.Visible = false;
                        textBox_UserPassword2.Visible = false;
                        button_UserOnOrOff.Visible = false;
                        button_ChangeUserPassword.Visible = false;
                    }
                    else
                    {
                        if (button_ChangeUserPassword.Text == UserPasswordStrArr[(int)UserPasswordStrArrIndex.修改密码])//可修改密码状态
                        {
                            label_UserName.Visible = true;
                            comboBox_UserName.Visible = true;

                            label_UserPasswor1.Visible = false;
                            label_UserPasswor2.Visible = false;
                            textBox_UserPassword1.Visible = false;
                            textBox_UserPassword2.Visible = false;
                            if (comboBox_UserName.SelectedIndex == 0)
                            {
                                button_UserOnOrOff.Visible = false;
                            }
                            else
                            {
                                button_UserOnOrOff.Visible = true;
                            }
                            button_ChangeUserPassword.Visible = true;
                        }
                        else if (button_ChangeUserPassword.Text == UserPasswordStrArr[(int)UserPasswordStrArrIndex.确定])//修改密码中
                        {
                            label_UserName.Visible = false;
                            comboBox_UserName.Visible = false;

                            label_UserPasswor1.Visible = true;
                            label_UserPasswor2.Visible = true;
                            textBox_UserPassword1.Visible = true;
                            textBox_UserPassword2.Visible = true;
                            textBox_UserPassword1.Text = "";
                            textBox_UserPassword2.Text = "";

                            button_UserOnOrOff.Visible = true;
                            button_ChangeUserPassword.Visible = true;
                        }
                    }
                }
                else
                {
                    //                     int ii = 0 ;
                    //                     for(;ii < comboBox_UserNmae.Items.Count;ii++)
                    //                     {
                    //                         if(comboBox_UserNmae.Items[ii].ToString() == label_CurrentUsername.Text)
                    //                         {
                    //                             break;
                    //                         }
                    //                     }


                    if (comboBox_UserName.SelectedIndex == 0)
                    {
                        label_UserName.Visible = true;
                        comboBox_UserName.Visible = true;

                        label_UserPasswor1.Visible = false;
                        label_UserPasswor2.Visible = false;
                        textBox_UserPassword1.Visible = false;
                        textBox_UserPassword2.Visible = false;
                        button_UserOnOrOff.Visible = false;
                        button_ChangeUserPassword.Visible = false;
                    }
                    else
                    {
                        label_UserName.Visible = true;
                        comboBox_UserName.Visible = true;

                        label_UserPasswor1.Visible = true;
                        label_UserPasswor2.Visible = false;
                        textBox_UserPassword1.Visible = true;
                        textBox_UserPassword2.Visible = false;
                        textBox_UserPassword1.Text = "";
                        button_UserOnOrOff.Visible = true;
                        button_ChangeUserPassword.Visible = false;
                    }

                    //                     if (ii < comboBox_UserNmae.SelectedIndex)//权限提升
                    //                     {
                    //                         label_UserName.Visible = true;
                    //                         comboBox_UserNmae.Visible = true;
                    // 
                    //                         label_UserPasswor1.Visible = true;
                    //                         label_UserPasswor2.Visible = false;
                    //                         textBox_UserPassword1.Visible = true;
                    //                         textBox_UserPassword2.Visible = false;
                    //                         textBox_UserPassword1.Text = "";
                    //                         button_UserOnOrOff.Visible = true;
                    //                         button_ChangeUserPassword.Visible = false;
                    // 
                    //                     }
                    //                     else
                    //                     {
                    //                         label_UserName.Visible = true;
                    //                         comboBox_UserNmae.Visible = true;
                    // 
                    //                         label_UserPasswor1.Visible = false;
                    //                         label_UserPasswor2.Visible = false;
                    //                         textBox_UserPassword1.Visible = false;
                    //                         textBox_UserPassword2.Visible = false;
                    //                         button_UserOnOrOff.Visible = true;
                    //                         button_ChangeUserPassword.Visible = false;
                    //                     }

                }

                //
                if (label_CurrentUsername.Text == m_xmlDociment.Default_Username_value[1])//管理者
                {
                    groupBox_UserManerge.Visible = true;
                }
                else
                {
                    groupBox_UserManerge.Visible = false;
                }
            }
            else
            {
                label_UserName.Visible = false;
                comboBox_UserName.Visible = false;
                label_UserPasswor1.Visible = false;
                label_UserPasswor2.Visible = false;
                textBox_UserPassword1.Visible = false;
                textBox_UserPassword2.Visible = false;
                button_UserOnOrOff.Visible = false;
                button_ChangeUserPassword.Visible = false;

            }
        }

        private void comboBox_UserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_UserName.Text == label_CurrentUsername.Text)//选择当前用户
            {
                button_ChangeUserPassword.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.修改密码];
                button_UserOnOrOff.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.注销];
            }
            else
            {
                button_UserOnOrOff.Text = UserPasswordStrArr[(int)UserPasswordStrArrIndex.登陆];
            }
            SetUserPasswoedItemShowOrHide();
        }

        private void MsgTipsHandlerFuc(object ob, String Str)
        {
            Label lb = (Label)ob;
            ThreaSetLaBText(lb, Str);
            System.Threading.Thread.Sleep(1800);
            try
            {
                if (lb.Text == Str)//最后一次才清空
                {
                    ThreaSetLaBText(lb, "");
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 委托设置文本控件显示
        /// </summary>
        /// <param name="LB"></param>
        /// <param name="str"></param>
        private void ThreaSetLaBText(Label LB, String str)
        {
            // if (LB.Text != str)
            {
                if (LB.InvokeRequired)//等待异步
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<string> actionDelegate = (x) =>
                    {
                        LB.Text = x;
                    };
                    LB.Invoke(actionDelegate, str);
                }
                else
                {
                    LB.Text = str;
                }
                if (str.Length != 0)
                {
                    String[] arrstr =  {LogData.LogDataNode1Name[(int)LogData.Node1Name.System_security],
                            LogData.LogDataNode2Level[(int)LogData.Node2Level.AUDIT], ((int)LogData.Node2Level.AUDIT).ToString(), "用户管理操作",
                            str};
                    LogData.EventHandlerSendParm SendParm = new LogData.EventHandlerSendParm();
                    SendParm.Node1NameIndex = (int)LogData.Node1Name.System_security;
                    SendParm.LevelIndex = (int)LogData.Node2Level.AUDIT;
                    SendParm.EventID = ((int)LogData.Node2Level.AUDIT).ToString();
                    SendParm.Keywords = "用户管理操作";
                    SendParm.EventData = str;
                    SCADA.MainForm.m_Log.AddLogMsgHandler.BeginInvoke(this, SendParm, new AsyncCallback(SCADA.MainForm.m_Log.AddLogMsgHandlerFinished), "AddLogMsgHandlerFinished!");
                }
            }
        }
        #endregion

        #region 用户权限限制和提醒
        public static bool LogIn = true;
        public static String LogInUserName = "";
        private bool CheckUserPassWord(object ob, Button bt)
        {
            if (!LogIn)
            {
                MessageBox.Show(LogInMessage, " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bt.Focus();
                return false;
            }
            return true;
        }

        private void txtWSSet_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, setTest);
        }

        private void txtPLSet_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, setTest);
        }

        private void comboBoxLineType_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, setTest);
        }

        private void comboBox_LocalIpAddr_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, setTest);
        }
        private void textBox_CNCNUM_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSaveCNC);
        }

        private void dgvCNCSet_Enter(object sender, EventArgs e)
        {
            //             CheckUserPassWord(sender, btnSaveCNC);
        }

        private void dgvRobotSet_Enter(object sender, EventArgs e)
        {
            //             CheckUserPassWord(sender, btnSaveROBOT);
        }

        private void textBox_ROBOTNum_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSaveROBOT);
        }

        private void textBox_SeleRobotIntSNum_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSaveROBOT);
        }

        private void textBox_SeleRobotOutSNum_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSaveROBOT);
        }

        private void DGVRobotInputSignalS_Enter(object sender, EventArgs e)
        {
            //             CheckUserPassWord(sender, btnSaveROBOT);
        }

        private void DGVRobotOutputSignalS_Enter(object sender, EventArgs e)
        {
            //             CheckUserPassWord(sender, btnSaveROBOT);
        }

        private void dgvPLCSet_Enter(object sender, EventArgs e)
        {
            //             CheckUserPassWord(sender, btnSavePLC);
        }
        /*
        private void comboBoxSelePLCSystem_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSavePLC);
        }

        private void textBox_PLCDVGROSNum_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSavePLC);
        }

        private void textBoxStartIO_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSavePLC);
        }

        private void comboBoxPLCDevice1_Enter(object sender, EventArgs e)
        {
//             CheckUserPassWord(sender, btnSavePLC);
        }

        private void textBox__SelePLCIntDVGROSNum_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSavePLC);
        }

        private void comboBoxPLCDevice2_Enter(object sender, EventArgs e)
        {
//             CheckUserPassWord(sender, btnSavePLC);
        }

        private void textBox__SelePLCOutDVGROSNum_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSavePLC);
        }

        private void PLCInputSignalSDefine_Enter(object sender, EventArgs e)
        {
//             CheckUserPassWord(sender, btnSavePLC);
        }

        private void PLCOutputSignalSDefine_Enter(object sender, EventArgs e)
        {
//             CheckUserPassWord(sender, btnSavePLC);
        }
        */
        private void textBox_RFidDgvRowsNum_Enter(object sender, EventArgs e)
        {
            CheckUserPassWord(sender, btnSaveRFID);
        }
        //private void textBox_CheckEqDgvRowsNum_Enter(object sender, EventArgs e)
        //{
        //    CheckUserPassWord(sender, btnSaveCheckEqment);
        //}


        //private void textBox_PLCAlarmNum_Enter(object sender, EventArgs e)
        //{
        //    CheckUserPassWord(sender, button_savePLCAlarmTb);
        //}


        private void radioButton_AddUser_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_AddUser.Checked)//添加用户
            {
                textBox_ChangeUserName.Visible = true;
                textBox_ChangeUserName.Text = "";
                label_ChangeUserPasswor1.Visible = true;
                textBox_ChangeUserPassword1.Visible = true;
                textBox_ChangeUserPassword1.Text = "";
                label_ChangeUserPasswor2.Visible = true;
                textBox_ChangeUserPassword1.Visible = true;
                textBox_ChangeUserPassword1.Text = "";
                mComboBox_DeleUser.Visible = false;
                button_EditUserOK.Text = "添加";
            }
            else
            {
                textBox_ChangeUserName.Visible = false;
                label_ChangeUserPasswor1.Visible = false;
                textBox_ChangeUserPassword1.Visible = false;
                label_ChangeUserPasswor2.Visible = false;
                textBox_ChangeUserPassword1.Visible = false;
                mComboBox_DeleUser.Visible = true;
                button_EditUserOK.Text = "删除";
                String[] str = MainForm.m_xml.GetUserNameStrArr();
                if (str.Length > 2)
                {
                    String[] str1 = new String[str.Length - 2];
                    for (int ii = 2; ii < str.Length; ii++)
                    {
                        str1[ii - 2] = str[ii];
                    }
                    mComboBox_DeleUser.DataSource = str1;
                }

            }
        }

        private void button_EditUserOK_Click(object sender, EventArgs e)//添加或者删除用户按钮相应
        {
            if (radioButton_AddUser.Checked)//添加用户
            {
                if (CheckNewUserNameAndPassword())
                {
                    MainForm.m_xml.InserNode(m_xmlDociment.PathRoot_User);
                    MainForm.m_xml.m_UpdateAttribute(m_xmlDociment.PathRoot_User, -2, m_xmlDociment.Default_Attributes_User[0], textBox_ChangeUserName.Text);//用户名
                    MainForm.m_xml.m_UpdateAttribute(m_xmlDociment.PathRoot_User, -2, m_xmlDociment.Default_Attributes_User[1],
                        MainForm.MakeSingSn(textBox_ChangeUserPassword1.Text));//用户密码
                    if (comboBox1.Text == "普通用户")
                    {
                        MainForm.m_xml.m_UpdateAttribute(m_xmlDociment.PathRoot_User, -2, m_xmlDociment.Default_Attributes_User[2], "1");//quanxian
                    }
                    else if (comboBox1.Text == "高级用户")
                    {
                        MainForm.m_xml.m_UpdateAttribute(m_xmlDociment.PathRoot_User, -2, m_xmlDociment.Default_Attributes_User[2], "2");//quanxian
                    }
                    MainForm.m_xml.SaveXml2File(MainForm.XMLSavePath);
                    comboBox_UserName.DataSource = MainForm.m_xml.GetUserNameStrArr();
                    MsgTipsHandler.BeginInvoke(label_ChangeUserTips, "添加用户成功：用户名 = " + textBox_ChangeUserName.Text, null, null);
                }
            }
            else
            {
                if (mComboBox_DeleUser.Items.Count > 0)
                {
                    MainForm.m_xml.DeleNode(m_xmlDociment.PathRoot_User, mComboBox_DeleUser.SelectedIndex + 2);
                    MainForm.m_xml.SaveXml2File(MainForm.XMLSavePath);
                    String[] str = MainForm.m_xml.GetUserNameStrArr();
                    comboBox_UserName.DataSource = str;
                    MsgTipsHandler.BeginInvoke(label_ChangeUserTips, "删除用户成功：用户名 = " + mComboBox_DeleUser.Text, null, null);

                    if (str.Length > 2)
                    {
                        String[] str1 = new String[str.Length - 2];
                        for (int ii = 2; ii < str.Length; ii++)
                        {
                            str1[ii - 2] = str[ii];
                        }
                        mComboBox_DeleUser.DataSource = str1;
                    }
                }
            }
        }


        private bool CheckNewUserNameAndPassword()//检查新添加的用户合法性
        {
            bool flg = false;
            if (textBox_ChangeUserName.Text.Length < 1)
            {
                MsgTipsHandler.BeginInvoke(label_ChangeUserTips, "添加用户失败：用户名长度不能小于1位！", null, null);
            }
            else if (textBox_ChangeUserPassword1.Text.Length < 6)
            {
                MsgTipsHandler.BeginInvoke(label_ChangeUserTips, "添加用户失败：密码长度不能小于6位！", null, null);
            }
            else if (textBox_ChangeUserPassword1.Text != textBox_ChangeUserPassword1.Text)
            {
                MsgTipsHandler.BeginInvoke(label_ChangeUserTips, "添加用户失败：两次输入密码不一致！", null, null);
            }
            else if (comboBox_UserName.Items.Count > 100)
            {
                MsgTipsHandler.BeginInvoke(label_ChangeUserTips, "添加用户失败：用户数量已经达到上限！", null, null);
            }
            else
            {
                int ii = 0;
                for (; ii < comboBox_UserName.Items.Count; ii++)
                {
                    if (textBox_ChangeUserName.Text == comboBox_UserName.Items[ii].ToString())
                    {
                        break;
                    }
                }
                if (ii < comboBox_UserName.Items.Count)
                {
                    MsgTipsHandler.BeginInvoke(label_ChangeUserTips, "添加用户失败：用户名已经存在！", null, null);
                }
                else
                {
                    flg = true;
                }
            }

            return flg;
        }


        #endregion

        private void Settimer_Tick(object sender, EventArgs e)
        {
            if (dgvCNCSet.RowCount == 1)
            {
                if (dgvCNCSet.Rows[0].Cells[6].Value == null)
                {
                    ;
                }
                else if (dgvCNCSet.Rows[0].Cells[6].Value.ToString() != MainForm.LATHEAddress)
                {
                    dgvCNCSet.Rows[0].Cells[6].Value = MainForm.LATHEAddress;

                    //SaveDgvData2XmlAndFile(0, false);
                }
            }
            else if (dgvCNCSet.RowCount == 2)
            {
                if (dgvCNCSet.Rows[0].Cells[6].Value == null)
                {
                    ;
                }
                else if (dgvCNCSet.Rows[0].Cells[6].Value.ToString() != MainForm.LATHEAddress
                    || dgvCNCSet.Rows[1].Cells[6].Value.ToString() != MainForm.CNCAddress)
                {
                    dgvCNCSet.Rows[0].Cells[6].Value = MainForm.LATHEAddress;

                    dgvCNCSet.Rows[1].Cells[6].Value = MainForm.CNCAddress;

                    //SaveDgvData2XmlAndFile(0, false);
                }
            }
            //LoadSetLanguage();
        }


        private void buttonNetSave_Click(object sender, EventArgs e)
        {

            language = ChangeLanguage.GetDefaultLanguage();
            //foreach (Control control in this.flowLayoutPanel1.Controls)
            //{
            //    if (control is UserIpset)
            //    {
            //        UserIpset temp = (UserIpset)control;
            //        string names = temp.IP;
            //        if (names == string.Empty)
            //        {
            //            ;
            //        }
            //        foreach (Control control1 in control.Controls)
            //        {
            //            if (control1 is TableLayoutPanel)
            //            {
            //                foreach (Control control2 in control1.Controls)
            //                {
            //                    if (control2 is TableLayoutPanel)
            //                    {
            //                        foreach (Control control3 in control2.Controls)
            //                        {
            //                            if (control3 is TextBox)
            //                            {
            //                                TextBox temp = (TextBox)control3;
            //                                string names = temp.Text;
            //                                for (int ii = 0; ii < names.Length; ii++)
            //                                {

            //                                    if (names[ii] < '0' || names[ii] > '9')
            //                                    {
            //                                        if (language == "English")
            //                                        {
            //                                            MessageBox.Show("IP set is err");
            //                                        }
            //                                        else MessageBox.Show("IP设置错误");
            //                                        return;
            //                                    }
            //                                }
            //                                int namei = Convert.ToInt32(names);
            //                                if (namei > 255 || namei < 0)
            //                                {
            //                                    if (language == "English")
            //                                    {
            //                                        MessageBox.Show("IP set is err");
            //                                    }
            //                                    else MessageBox.Show("IP设置错误");
            //                                    return;
            //                                }
            //                            }
            //                        }

            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            string strCP1 = textBoxPC1.Text;
            string strCP2 = textBoxPC2.Text;
            string strCP3 = textBoxPC3.Text;
            string strCP4 = textBoxPC4.Text;
            string strPLC1 = textBoxPLCIP1.Text;
            string strPLC2 = textBoxPLCIP2.Text;
            string strPLC3 = textBoxPLCIP3.Text;
            string strPLC4 = textBoxPLCIP4.Text;
            string strLa1 = textBoxLaIP1.Text;
            string strLa2 = textBoxLaIP2.Text;
            string strLa3 = textBoxLaIP3.Text;
            string strLa4 = textBoxLaIP4.Text;
            string strCNC1 = textBoxCNCIP1.Text;
            string strCNC2 = textBoxCNCIP2.Text;
            string strCNC3 = textBoxCNCIP3.Text;
            string strCNC4 = textBoxCNCIP4.Text;
            string strRo1 = textBoxRoIP1.Text;
            string strRo2 = textBoxRoIP2.Text;
            string strRo3 = textBoxRoIP3.Text;
            string strRo4 = textBoxRoIP4.Text;
            string strvi1 = textBoxViIP1.Text;
            string strvi2 = textBoxViIP2.Text;
            string strvi3 = textBoxViIP3.Text;
            string strvi4 = textBoxViIP4.Text;
            string strrf1 = textBoxRFIP1.Text;
            string strrf2 = textBoxRFIP2.Text;
            string strrf3 = textBoxRFIP3.Text;
            string strrf4 = textBoxRFIP4.Text;
            if (strCP4 == strPLC4 || strCP4 == strLa4
                || strCP4 == strCNC4 || strCP4 == strRo4
                || strCP4 == strvi4 || strCP4 == strrf4
                || strPLC4 == strLa4 || strPLC4 == strCNC4
                || strPLC4 == strRo4 || strPLC4 == strvi4
                || strPLC4 == strrf4
                || strLa4 == strCNC4 || strLa4 == strRo4
                || strLa4 == strvi4 || strLa4 == strrf4
                || strLa4 == strCNC4 || strLa4 == strRo4
                || strCNC4 == strRo4 || strCNC4 == strvi4
                || strCNC4 == strrf4
                || strRo4 == strvi4 || strRo4 == strrf4
                || strvi4 == strrf4)
            {
                if (language == "English")
                {
                    MessageBox.Show("IP Address can not be the same!");
                }
                else MessageBox.Show("IP地址重复");
                return;
            }
            if (strCP1 == strPLC1 && strLa1 == strPLC1
                && strCNC1 == strPLC1 && strRo1 == strPLC1
                && strvi1 == strPLC1 && strrf1 == strPLC1
                && strCP2 == strPLC2 && strLa2 == strPLC2
                && strCNC2 == strPLC2 && strRo2 == strPLC2
                && strvi2 == strPLC2 && strrf2 == strPLC2
                && strCP3 == strPLC3 && strLa3 == strPLC3
                && strCNC3 == strPLC3 && strRo3 == strPLC3
                && strvi3 == strPLC3 && strrf3 == strPLC3)
            {
                FileStream aFile = new FileStream(MainForm.IPSetFilePath, FileMode.OpenOrCreate);
                StreamWriter sr = new StreamWriter(aFile);
                string strPC = strCP1 + "." + strCP2 + "." + strCP3 + "." + strCP4;
                string strPLC = strPLC1 + "." + strPLC2 + "." + strPLC3 + "." + strPLC4;
                string strLa = strLa1 + "." + strLa2 + "." + strLa3 + "." + strLa4;
                string strCNC = strCNC1 + "." + strCNC2 + "." + strCNC3 + "." + strCNC4;
                string strRo = strRo1 + "." + strRo2 + "." + strRo3 + "." + strRo4;
                string strvi = strvi1 + "." + strvi2 + "." + strvi3 + "." + strvi4;
                string strrf = strrf1 + "." + strrf2 + "." + strrf3 + "." + strrf4;
                string line1 = "PC:" + strPC + ";";
                string line2 = "PLC:" + strPLC + ";";
                string line3 = "LATHE:" + strLa + ";";
                string line4 = "CNC:" + strCNC + ";";
                string line5 = "ROBORT:" + strRo + ";";
                string line6 = "VIDEO:" + strvi + ";";
                string line7 = "RFID:" + strrf + ";";

                sr.WriteLine(line1);
                sr.WriteLine(line2);
                sr.WriteLine(line3);
                sr.WriteLine(line4);
                sr.WriteLine(line5);
                sr.WriteLine(line6);
                sr.WriteLine(line7);

                sr.Close();
                aFile.Close();
                MainForm.refreshIpAddress = true;


            }
            else
            {
                if (language == "English")
                {
                    MessageBox.Show("IP Address is not at same segment");
                }
                else MessageBox.Show("IP不在一个网段");
                return;
            }








        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox4.Visible)
            {
                textBox4.Focus();
            }
            else
            {
                button_UserOnOrOff_Click(null, null);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox3.Visible)
            {
                textBox3.Focus();
            }
            else
            {
                button_UserOnOrOff_Click(null, null);
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Visible)
            {
                textBox1.Focus();
            }
            else
            {
                button_UserOnOrOff_Click(null, null);
            }
        }

        private void buttonzhuce_Click(object sender, EventArgs e)
        {
            string name = textBox4.Text;
            string key1 = textBox3.Text;
            string key2 = textBox1.Text;
            if (key1 != key2)
            {
                MessageBox.Show("密码不一致");
                if (language == "English")
                {
                    MessageBox.Show("Key is not the same");
                }
                return;
            }
            if (name == "")
            {
                MessageBox.Show("用户名不能为空!");
                if (language == "English")
                {
                    MessageBox.Show("Username can not be null!");
                }

                return;
            }
            try
            {
                if (MainForm.SQLonline)
                {
                    var temp2 = DbHelper.Get<User>(new { Username = name });
                    if (temp2 == null)
                    {
                        DbHelper.Insert(new User
                        {
                            Username = name,
                            Password = AesEncryption.Encrypt(key2),
                        });
                        MessageBox.Show("注册成功!");
                        if (language == "English")
                        {
                            MessageBox.Show("User add correct!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("用户名已存在!");
                        if (language == "English")
                        {
                            MessageBox.Show("Username already exist!");
                        }

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("数据库未连接，用户信息操作失败!");
                    if (language == "English")
                    {
                        MessageBox.Show("database connect failure!");
                    }
                    return;
                }
            }
            catch
            {
                MessageBox.Show("数据库操作失败!");
                if (language == "English")
                {
                    MessageBox.Show("database connect failure!");
                }

                return;
            }


        }

        private void buttonlogin_Click(object sender, EventArgs e)
        {
            if (textBoxname.Text == "" || textBoxkey.Text == "")
            {
                MessageBox.Show("请正确输入用户名和密码!");
                if (language == "English")
                {
                    MessageBox.Show("Please enter the name and key correct!");
                }

                return;
            }
            try
            {
                var temp2 = DbHelper.Get<User>(new { Username = textBoxname.Text });
                if (temp2 == null)
                {

                    if (language == "English")
                    {
                        MessageBox.Show("User is not exist!");
                    }
                    else
                    {
                        MessageBox.Show("用户不存在!");
                    }


                    return;
                }
                else
                {
                    if (temp2.Username == textBoxname.Text && temp2.Password == AesEncryption.Encrypt(textBoxkey.Text))
                    {
                        MainForm.UserLogin = true;
                        MessageBox.Show(" 登陆成功!");
                        if (language == "English")
                        {
                            MessageBox.Show("Login!");
                        }
                        labelusername.Text = textBoxname.Text;
                        MainForm.UserLoginname = labelusername.Text;
                        return;
                    }
                    else
                    {

                        if (language == "English")
                        {
                            MessageBox.Show("Key is not error!");
                        }
                        else
                        {
                            MessageBox.Show("用户密码错误!");
                        }

                        return;
                    }
                }

            }
            catch
            {
                MessageBox.Show("数据库操作失败!");
                if (language == "English")
                {
                    MessageBox.Show("database connect failure!");
                }

                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //车床连接测试

            if (MainForm.cncv2list[0].IsConnected())
            {
                pictureBoxchecklathe.Load(ONLINE);
            }
            else
            {
                pictureBoxchecklathe.Load(OFFLINE);
            }
            //铣床连接测试
            if (MainForm.cncv2list[1].IsConnected())
            {
                pictureBoxcheckcnc.Load(ONLINE);
            }
            else
            {
                pictureBoxcheckcnc.Load(OFFLINE);
            }
            //PLC 连接测试
            if (MainForm.PLC_SIMES_ON_line)
            {
                pictureBoxchecklathe.Load(ONLINE);
                pictureBoxcheckrfid.Load(ONLINE);
            }
            else
            {
                pictureBoxcheckplc.Load(OFFLINE);
                pictureBoxcheckrfid.Load(OFFLINE);
            }
            //机器人连接测试
            bool robotconnect = MainForm.PingTestCNC(MainForm.ROBORTAddress, 300);//
            if (robotconnect == true)
            {
                pictureBoxcheckrobot.BackColor = Color.LightGreen;
            }
            else
            {
                pictureBoxcheckrobot.Load(OFFLINE);
            }
            //录像机连接测试
            bool videoconnect = MainForm.PingTestCNC(MainForm.VIDEOAddress, 300);//
            if (videoconnect == true)
            {
                pictureBoxcheckvideo.BackColor = Color.LightGreen;
            }
            else
            {
                pictureBoxcheckvideo.Load(OFFLINE);
            }

            //RFID连接测试
            try
            {

                Ping ping = new Ping();
                PingReply pingReply = ping.Send(MainForm.PCAddress, 200);
                if (pingReply.Status == IPStatus.Success)
                {
                    pictureBoxcheckpc.Load(ONLINE);
                }
                else
                {
                    pictureBoxcheckpc.Load(OFFLINE);
                }
            }
            catch
            {
                pictureBoxcheckpc.Load(OFFLINE);
            }

        }

        //private void textBoxshebei1_Leave(object sender, EventArgs e)
        //{

        //    string textBoxshebei1str = ((TextBox)sender).Text;
        //    if (textBoxshebei1str == "")
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        int shebeiNo = Convert.ToInt32(textBoxshebei1str);
        //        if (shebeiNo > 12 || shebeiNo <= 0)
        //        {
        //            if (language == "English")
        //            {
        //                MessageBox.Show("Please enter number between 1 to 13");
        //            }
        //            else
        //                MessageBox.Show("请输入设备编号1-12");
        //            (sender as TextBox).Focus();
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch
        //    {
        //        if (language == "English")
        //        {
        //            MessageBox.Show("Please enter number between 1 to 13");
        //        }
        //        else
        //            MessageBox.Show("请输入设备编号1-13");
        //        (sender as TextBox).Focus();
        //    }
        //}

    

        //private void textBoxshebei2_Leave(object sender, EventArgs e)
        //{
        //    string textBoxshebei2str = ((TextBox)sender).Text;
        //    if (textBoxshebei2str == "")
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        int shebeiNo = Convert.ToInt32(textBoxshebei2str);
        //        if (shebeiNo > 12 || shebeiNo <= 0)
        //        {
        //            if (language == "English")
        //            {
        //                MessageBox.Show("Please enter number between 1 to 13");
        //            }
        //            else
        //                MessageBox.Show("请输入设备编号1-13");
        //            (sender as TextBox).Focus();
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch
        //    {
        //        if (language == "English")
        //        {
        //            MessageBox.Show("Please enter number between 1 to 13");
        //        }
        //        else
        //            MessageBox.Show("请输入设备编号1-13");
        //        (sender as TextBox).Focus();
        //    }
        //}



        //private void buttonconnect_Click(object sender, EventArgs e)
        //{
        //    //if (textBoxshebei1.Text == "" || textBoxshebei2.Text == "" || (textBoxshebei1.Text == textBoxshebei2.Text))
        //    {
        //        MessageBox.Show("请检查设备编号");
        //        return;
        //    }
        //    int node1 = Convert.ToInt32(textBoxshebei1.Text);
        //    int node2 = Convert.ToInt32(textBoxshebei2.Text);
        //    PictureBox picturenode1 = new PictureBox();
        //    PictureBox picturenode2 = new PictureBox();
        //    foreach (Control control in this.tabPageNet.Controls)
        //    {
        //        getpictureitem(control, node1, node2, ref picturenode1, ref picturenode2);

        //    }
        //    if (picturenode1.Location == null || picturenode2.Location == null)
        //    {
        //        MessageBox.Show("请检查设备编号");
        //        return;
        //    }
        //    Point point1 = PointToScreen(picturenode1.Location);
        //    Point point2 = PointToScreen(picturenode2.Location);
        //    int x1 = point1.X;
        //    int y1 = point1.Y;
        //    int x2 = point2.X;
        //    int y2 = point2.Y;
        //    if (node1 < 4 && node2 < 4)//都在第一级
        //    {
        //        if ((node1 == 1 && node2 == 3) || (node1 == 3 && node2 == 1))//折线
        //        {
        //            int point1x = x1 + picturenode1.Width / 2;
        //            int point1y = y1;
        //            int point2x = x2 + picturenode2.Width / 2;
        //            int point2y = y2;
        //            Point pointA = new Point(point1x, point1y);
        //            Point pointB = new Point(point1x, point1y - 10);
        //            Point pointC = new Point(point2x, point1y - 10);
        //            Point pointD = new Point(point2x, point2y);
        //            bool node1connect = MainForm.PingTestCNC(MainForm.PCplcAddress, 300);
        //            bool node3connect = MainForm.PingTestCNC(MainForm.PCcadAddress, 300);
        //            pictureBoxline.Image = new Bitmap(pictureBoxline.Width, pictureBoxline.Height);
        //            Graphics g = Graphics.FromImage(pictureBoxline.Image);

        //            if (node1connect && node3connect)
        //            {
        //                Pen apen = new Pen(Color.Green, 5);
        //                g.DrawLine(apen, pointC, pointB);
        //                g.Save();
        //                // g.DrawLine(apen, pointB, pointC);
        //                // g.DrawLine(apen, pointC, pointD);
        //            }
        //            else
        //            {
        //                Pen apen = new Pen(Color.Red, 10);
        //                g.DrawLine(apen, pointnode1, pointnode2);
        //                //g.DrawLine(apen, pointB, pointC);
        //                //g.DrawLine(apen, pointC, pointD);
        //                g.Save();
        //            }

        //        }
        //        if ((node1 == 1 && node2 == 2) || (node1 == 2 && node2 == 1))//横线
        //        {
        //            ;
        //        }
        //        if ((node1 == 2 && node2 == 3) || (node1 == 3 && node2 == 2))//横线
        //        {
        //            ;
        //        }
        //    }
        //    else if (4 < node1 && node1 < 9 && 4 < node2 && node2 < 9)//都在第二级
        //    {
        //        ;
        //    }
        //    else if (8 < node1 && node1 < 13 && 8 < node2 && node2 < 13)//都在第三一级
        //    {
        //        ;
        //    }


        //}

        private void getpictureitem(Control control, int no1, int no2, ref PictureBox p1, ref PictureBox p2)
        {
            if (control is PictureBox)
            {
                PictureBox temp = (PictureBox)control;
                string names = temp.Name;
                if (names == "")
                {
                    return;
                }
                string numbers = "";
                int number = -1;
                int length = names.Length;
                if (length == 12)
                {
                    numbers = names.Substring(length - 1, 1);
                    number = Convert.ToInt32(numbers);
                }
                if (length == 13)
                {
                    numbers = names.Substring(length - 2, 2);
                    number = Convert.ToInt32(numbers);
                }
                if (number == no1)
                {
                    p1 = temp;
                }
                if (number == no2)
                {
                    p2 = temp;
                }

            }

        }

       

        private void buttondisconnect_Click(object sender, EventArgs e)
        {

        }

        private void buttonsave_Click(object sender, EventArgs e)
        {
            language = ChangeLanguage.GetDefaultLanguage();
            foreach (Control control in this.flowLayoutPanel1.Controls)
            {
                if (control is UserIpset)
                {
                    UserIpset temp = (UserIpset)control;
                    string names = temp.IP;
                    int[] arry = new int[4];
                    Ipstringtoarry(names, ref arry);

                    if (names == string.Empty)
                    {
                        ;
                    }
                    for(int ii =0;ii<4;ii++)
                    {
                        if(arry[ii]>255||arry[ii]<0)
                        {
                            MessageBox.Show("IP设置错误");
                            return;
                        }
                     }
                }
            }
                   
            int[,] arryip=new int[8,4];       
            int[] arry1 = new int[4];

            string PCplcIP = userIpset1.IP;    
            Ipstringtoarry(PCplcIP, ref arry1);
            arryip[0,0] = arry1[0];arryip[0,1] = arry1[1];arryip[0,2] = arry1[2];arryip[0,3] = arry1[3];

            string PCIP = userIpset2.IP;
            Ipstringtoarry(PCIP, ref arry1);         
            arryip[1,0] = arry1[0];arryip[1,1] = arry1[1];arryip[1,2] = arry1[2];arryip[1,3] = arry1[3];

            string PCcadIP = userIpset3.IP;
            Ipstringtoarry(PCcadIP, ref arry1);
            arryip[2,0] = arry1[0];arryip[2,1] = arry1[1];arryip[2,2] = arry1[2];arryip[2,3] = arry1[3];

            string LATHEIP = userIpset4.IP;
            Ipstringtoarry(LATHEIP, ref arry1);
            arryip[3, 0] = arry1[0]; arryip[3, 1] = arry1[1]; arryip[3, 2] = arry1[2]; arryip[3, 3] = arry1[3];

            string CNCIP = userIpset5.IP;
            Ipstringtoarry(CNCIP, ref arry1);
            arryip[4, 0] = arry1[0]; arryip[4, 1] = arry1[1]; arryip[4, 2] = arry1[2]; arryip[4, 3] = arry1[3];

            string PLCIP = userIpset6.IP;
            Ipstringtoarry(PLCIP, ref arry1);
            arryip[5,0] = arry1[0];arryip[5,1] = arry1[1];arryip[5,2] = arry1[2];arryip[5,3] = arry1[3];


            string ROBORTIP = userIpset7.IP;
            Ipstringtoarry(ROBORTIP, ref arry1);
            arryip[6,0] = arry1[0];arryip[6,1] = arry1[1];arryip[6,2] = arry1[2];arryip[6,3] = arry1[3];



            string VIDEOIP = userIpset8.IP;
            Ipstringtoarry(VIDEOIP, ref arry1);
            arryip[7, 0] = arry1[0]; arryip[7, 1] = arry1[1]; arryip[7, 2] = arry1[2]; arryip[7, 3] = arry1[3];

            for (int ii = 0; ii < 8; ii++)
            {
                for (int jj = 0; jj < ii; jj++)
                {
                    if (arryip[ii, 0] != arryip[jj, 0] || arryip[ii, 1] != arryip[jj, 1] || arryip[ii, 2] != arryip[jj, 2])
                    {
                        MessageBox.Show("1-8号设备IP不在一个网段");
                        return;
                    }

                }
            }
            for (int ii = 0; ii < 8; ii++)
            {
                for (int jj = 0; jj < ii; jj++)
                {
                    if (arryip[ii, 3] == arryip[jj, 3])
                    {
                        MessageBox.Show("IP地址重复");
                        return;
                    }
                }
            }
            
                
            string[,] arryipstr=new string[8,4];
            for (int ii = 0; ii < 8;ii++ )
            {
                arryipstr[ii, 0] = arryip[ii, 0].ToString();
                arryipstr[ii, 1] = arryip[ii, 1].ToString(); 
                arryipstr[ii, 2] = arryip[ii, 2].ToString(); 
                arryipstr[ii, 3] = arryip[ii, 3].ToString(); 
            }

                FileStream aFile = new FileStream(MainForm.IPSetFilePath, FileMode.OpenOrCreate);
                StreamWriter sr = new StreamWriter(aFile);
                string IP1 = arryipstr[0, 0] + "." + arryipstr[0, 1] + "." + arryipstr[0, 2] + "." + arryipstr[0, 3];
                string IP2 = arryipstr[1, 0] + "." + arryipstr[1, 1] + "." + arryipstr[1, 2] + "." + arryipstr[1, 3];
                string IP3 = arryipstr[2, 0] + "." + arryipstr[2, 1] + "." + arryipstr[2, 2] + "." + arryipstr[2, 3];
                string IP4 = arryipstr[3, 0] + "." + arryipstr[3, 1] + "." + arryipstr[3, 2] + "." + arryipstr[3, 3];
                string IP5 = arryipstr[4, 0] + "." + arryipstr[4, 1] + "." + arryipstr[4, 2] + "." + arryipstr[4, 3];
                string IP6= arryipstr[5, 0] + "." + arryipstr[5, 1] + "." + arryipstr[5, 2] + "." + arryipstr[5, 3];
                string IP7 = arryipstr[6, 0] + "." + arryipstr[6, 1] + "." + arryipstr[6, 2] + "." + arryipstr[6, 3];
                string IP8= arryipstr[7, 0] + "." + arryipstr[7, 1] + "." + arryipstr[7, 2] + "." + arryipstr[7, 3];

            //for(int ii=0;ii<8;ii++)
            //{
            //    ;
            //}
            //   MainForm.SIpSetFile.id = ;            
            //   MainForm.SIpSetFile.Name =        
            //   MainForm.SIpSetFile.IP = ;
            // var temp1 = DbHelper.Get<IpSetFile>(new { SN = SCutter.SN, MachineId = temp1.Id });
            //            if (temp2 == null)
            //            {
            //                DbHelper.Insert(new Cutter
            //                {
            //                    SN = SCutter.SN,
            //                    Length = SCutter.Length,
            //                    LengthAbrasion = SCutter.LengthAbrasion,
            //                    Radius = SCutter.Radius,
            //                    RadiusAbrasion = SCutter.RadiusAbrasion,
            //                    MachineId = SCutter.MachineId,
            //                });
            //            }
            //            else if (SCutter.Length != temp2.Length || SCutter.LengthAbrasion != temp2.LengthAbrasion
            //                || SCutter.Radius != temp2.Radius || SCutter.RadiusAbrasion != temp2.RadiusAbrasion)
            //            {
            //                temp2.SN = SCutter.SN;
            //                temp2.Length = SCutter.Length;
            //                temp2.LengthAbrasion = SCutter.LengthAbrasion;
            //                temp2.Radius = SCutter.Radius;
            //                temp2.RadiusAbrasion = SCutter.RadiusAbrasion;
            //                temp2.MachineId = SCutter.MachineId;

            //                DbHelper.Update(temp2);
            //            }
                string line1 = "CADPC:" + IP3 + ";";        
                string line2= "PC:" +  IP2 + ";";
                string line3 = "PLCPC:" +  IP1 + ";";
                string line4 = "LATHE:" +  IP4 + ";";
                string line5 = "CNC:" +  IP5 + ";";                     
                string line6 = "PLC:" +  IP6 + ";";
                string line7 = "ROBORT:" +  IP7 + ";";
                string line8 = "VIDEO:" +  IP8 + ";";       

                sr.WriteLine(line1);
                sr.WriteLine(line2);
                sr.WriteLine(line3);
                sr.WriteLine(line4);
                sr.WriteLine(line5);
                sr.WriteLine(line6);
                sr.WriteLine(line7);
                sr.WriteLine(line8);

                sr.Close();
                aFile.Close();
                MainForm.refreshIpAddress = true;
                renewCNCIPxml(IP4, IP5);

            

        }

        private void renewCNCIPxml(string latheip,string cncip)
        {
            XmlDataDocument doc = new XmlDataDocument() ;
            doc.Load(MainForm.XMLSavePath);
            doc.SelectSingleNode("Root/CNC/Item0").Attributes["ip"].InnerXml = latheip;
            doc.SelectSingleNode("Root/CNC/Item1").Attributes["ip"].InnerXml = cncip;
            doc.Save(MainForm.XMLSavePath);
        }
    


    }


}
