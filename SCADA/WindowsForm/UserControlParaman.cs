using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HNCAPI;
using HNC_MacDataService;
using ScadaHncData;

namespace SCADA
{
    public partial class UserControlParaman : UserControl
    {
        const string STR_NO_CONNECTION = "未连接";

        //int[] columnHeadWidth = {100, 200, 100, 120, 100, 100, 100 };

        //private short ClientNo = -1;
        private int dbNo = -1;
        int rowNum = 0;

        int m_fileNo = 0;
        int m_recNo = 0;

        public UserControlParaman()
        {
            InitializeComponent();

            dataGridViewParam.AllowUserToAddRows = false;
            dataGridViewParam.ReadOnly = true;
            dataGridViewParam.ColumnCount = 7;
            dataGridViewParam.RowHeadersVisible = false;

            dataGridViewParam.Columns[0].Width = dataGridViewParam.Width / 7 - 60;
            dataGridViewParam.Columns[1].Width = dataGridViewParam.Width / 7 + 100;
            dataGridViewParam.Columns[3].Width = dataGridViewParam.Width / 7 - 40;
            dataGridViewParam.Columns[2].Width = dataGridViewParam.Width / 7;
            dataGridViewParam.Columns[4].Width = dataGridViewParam.Width / 7;
            dataGridViewParam.Columns[5].Width = dataGridViewParam.Width / 7;

            LoadParamanLanguage();
            timerUpdataShow.Enabled = true;
        }

        public void LoadParamanLanguage()
        {
            dataGridViewParam.Columns[0].HeaderText = ChangeLanguage.GetString("ParamColumn01");
            dataGridViewParam.Columns[1].HeaderText = ChangeLanguage.GetString("ParamColumn02");
            dataGridViewParam.Columns[2].HeaderText = ChangeLanguage.GetString("ParamColumn03");
            dataGridViewParam.Columns[3].HeaderText = ChangeLanguage.GetString("ParamColumn04");
            dataGridViewParam.Columns[4].HeaderText = ChangeLanguage.GetString("ParamColumn05");
            dataGridViewParam.Columns[5].HeaderText = ChangeLanguage.GetString("ParamColumn06");
            dataGridViewParam.Columns[6].HeaderText = ChangeLanguage.GetString("ParamColumn07");
        }

        public void updateActLanguage(Int32 actType, string[] strPar)
        {
            switch (actType)
            {
                case (sbyte)ParaActType.PARA_ACT_SAVE:
                    strPar[3] = ChangeLanguage.GetString("ParamEffect01");
                    break;
                case (sbyte)ParaActType.PARA_ACT_NOW:
                    strPar[3] = ChangeLanguage.GetString("ParamEffect02");
                    break;
                case (sbyte)ParaActType.PARA_ACT_RST:
                    strPar[3] = ChangeLanguage.GetString("ParamEffect03");
                    break;
                case (sbyte)ParaActType.PARA_ACT_PWR:
                    strPar[3] = ChangeLanguage.GetString("ParamEffect04");
                    break;
            }
        }

        public void InitParamanTreeView(int dbNo)
        {
            this.dbNo = dbNo;
            //if (HncApi.HNC_NetIsConnect(clientNo) != 0)
            if (!MacDataService.GetInstance().IsNCConnectToDatabase(dbNo))
            {
                return;
            }
            treeViewParaman.Nodes.Clear();

            //short clientNo = 0;
            //if (HncApi.HNC_NetIsConnect(dbNo) != 0)
            if (!MacDataService.GetInstance().IsNCConnectToDatabase(dbNo))
            {
                treeViewParaman.Nodes.Add(new TreeNode(STR_NO_CONNECTION));
            }
            else
            {
                int ret = AddParamanItemFrmNet(dbNo);
                if (ret < 0)
                {
                    MessageBox.Show("网络读取参数结构错误！");
                }
            }

            //2017.3.31   hxb
            if (ParameterDef.InitParams(dbNo) != 0)
            {
                //MessageBox.Show("redis数据库建立中！参数显示不全，请稍后再读！");
            }

           UpdatalistViewParm();
        }

        //2017.3.31   hxb
        public void ReinitParameterContent()
        {
            timerUpdataShow.Enabled = true;
            timerUpdataShow.Start();
            if (ParameterDef.InitParams(dbNo) != 0)
            {
                //MessageBox.Show("redis数据库建立中！参数不全，请稍后再读！");
            }
        }

        private int AddParamanItemFrmNet(int clientNo)
        {
            int fileNum = HNCDATADEF.PARAMAN_MAX_FILE_LIB;
            string fileName = "";
            int ret = 0;
            for (int i = 0; i < fileNum; i++)
            {
 /*               ret = HncApi.HNC_ParamanGetFileName(i, ref fileName, clientNo);
                if (ret < 0)
                {
                    break;
                }

                TreeNode rootNode = new TreeNode(fileName);
                treeViewParaman.Nodes.Add(rootNode);

                int recNum = -1;
                ret = HncApi.HNC_ParamanGetSubClassProp(i, (Byte)ParaSubClassProp.SUBCLASS_NUM, ref recNum, clientNo);
*/
                fileName = ParameterDef.names[i];
                TreeNode rootNode = new TreeNode(fileName);
                treeViewParaman.Nodes.Add(rootNode);

                int recNum = ParameterDef.subClass[i];

                if (recNum < 0)
                {
                    ret = recNum;
                    break;
                }
                else if (recNum > 1)
                {
                    string recName = ParameterDef.subClassName[i];
/*                 string recName = "";
                    ret = HncApi.HNC_ParamanGetSubClassProp(i, (Byte)ParaSubClassProp.SUBCLASS_NAME, ref recName, clientNo);
                    if (ret < 0)
                    {
                        break;
                    }
*/
                    for (int j = 0; j < recNum; j++)
                    {
                        rootNode.Nodes.Add(new TreeNode(recName + j.ToString()));
                    }
                }
            }
            return ret;
        }


        private void InitdataGridViewParam(int fileNo, int recNo, int dbNo)
        {
/*            int ret = -1;           
            ret = HncApi.HNC_ParamanGetSubClassProp(fileNo, (Byte)ParaSubClassProp.SUBCLASS_ROWNUM, ref rowNum, clientNo);
            if (rowNum < 0)
            {
                return;
            }
          
            ret = HncApi.HNC_ParamanRewriteSubClass(fileNo, recNo, clientNo);
            if (ret < 0)
            {
                return;
            }
 */
            int ret = -1;
            //String[] keys = HNC_MacDataService.MacDataService.GetInstance().GetFolderKeys((int)clientNo, "Parameter");

            long tempRowNum = -1;
            ret = MacDataService.GetInstance().GetHashKeyLength(dbNo, "Parameter:" + ParameterDef.keyType[fileNo], ref tempRowNum);
            if (ret < 0 || rowNum < 0)
            {
                return;
            }
            rowNum = (int)tempRowNum / ParameterDef.subClass[fileNo];

            if (rowNum < dataGridViewParam.RowCount)//删除多余的行
            {
                for (int index = dataGridViewParam.RowCount - 1; index >= rowNum; index--)
                {
                    dataGridViewParam.Rows.RemoveAt(index);
                }
            }
            for (int index = 0; index < dataGridViewParam.RowCount; index++)
            {
                dataGridViewParam.Rows[index].Cells[0].Value = null;
            }
            timerUpdataShow.Enabled = true;
        }

        private int GetParItemText(int fileNo, int recNo, int row, int dbNo, string[] strPar)
        {
/*            Int16 dupNum = 0;
            Int16 dupNo = 0;
            Int32 index = -1;
            Int32 parmID = -1;
            int ret = -1;
            ret = HncApi.HNC_ParamanTransRow2Index(fileNo, recNo, row, ref index, ref dupNum, ref dupNo, clientNo);
            if (index < 0)
            {
                return -1;
            }

           //获取生效方式
            Int32 actType = -1;
            ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_ACT, ref actType, clientNo);
           */
            int ret = -1;
            Int32 parmID = -1;
            String redisKey = "Parameter:" + ParameterDef.keyType[fileNo];
            String fields = "";
            // 获取redis中的fields
            if (ParameterDef.GetParamanId(fileNo, recNo, row, ref fields) != 0)
            {
                return -1;
            }

            String json = "";

            //获取生效方式
            Int32 actType = -1;
            ret = MacDataService.GetInstance().GetHashKeyValueJson(dbNo, redisKey, fields, "EffectWay", ref json);
            
            if (ret != 0)
            {
                return -1;
            }
            actType = int.Parse(json);

            if (actType < 0)
            {
                return -1;
            }
            updateActLanguage(actType, strPar);

            //获取参数号
            //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_ID, ref parmID, dbNo);
            ret = MacDataService.GetInstance().GetHashKeyValueJson(dbNo, redisKey, fields, "Id", ref json);
            if (ret < 0)
            {
                return -1;
            }
            parmID = int.Parse(json);
            strPar[0] = parmID.ToString("D6");

            //获取参数名称
            //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_NAME, ref strPar[1], dbNo);
            ret = MacDataService.GetInstance().GetHashKeyValueJson(dbNo, redisKey, fields, "Name", ref json);
            if (ret < 0)
            {
                return -1;
            }
            strPar[1] = json;

            //获取参数储存类型
            Int32 storeType = -1;
            //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_STORE, ref storeType, dbNo);
            ret = MacDataService.GetInstance().GetHashKeyValueJson(dbNo, redisKey, fields, "StoreType", ref json);
            if (ret != 0)
            {
                return -1;
            }
            storeType = int.Parse(json);
            if (storeType < 0)
            {
                return -1;
            }

            //获取参数值、默认值、最小值和最大值
            String paramJson = "";
            ret = MacDataService.GetInstance().GetHashKeyValueString(dbNo, redisKey, fields, ref paramJson);
            if (ret >= 0)
            {        
                Parameter<String> parameter = Newtonsoft.Json.JsonConvert.DeserializeObject<Parameter<String>>(paramJson);
                strPar[2] = parameter.PropValue.TrimEnd("\0".ToCharArray());//"0x" + iVal.ToString("X2");
                strPar[4] = parameter.DefaultValue.TrimEnd("\0".ToCharArray());//"N/A";
                strPar[5] = parameter.MinValue.TrimEnd("\0".ToCharArray());//"N/A";
                strPar[6] = parameter.MaxValue.TrimEnd("\0".ToCharArray());//"N/A";
            }

            //int iVal = 0;
            //double dVal = 0;
            //const int DFT = 1;
            //const int MIN = 2;
            //const int MAX = 3;
            /*switch (storeType)
            {
                case (sbyte)HNCDATATYPE.DTYPE_BOOL:
                case (sbyte)HNCDATATYPE.DTYPE_UINT:
                case (sbyte)HNCDATATYPE.DTYPE_INT:
                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_VALUE, ref iVal, dbNo);
                    String paramJson = "";
                    ret = MacDataService.GetInstance().GetHashKeyValueString(dbNo, redisKey, fields, ref paramJson);
                    if (ret < 0)
                    {
                        break;
                    }
                    Parameter<int> parameter = Newtonsoft.Json.JsonConvert.DeserializeObject<Parameter<int>>(paramJson);                    
                    //strPar[2] = iVal.ToString();
                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_DFVALUE, ref iVal, dbNo);
                    //if (ret < 0)
                    //{
                    //    break;
                    //}
                    //strPar[4] = iVal.ToString();
                    strPar[2] = parameter.PropValue.ToString();
                    strPar[4] = parameter.DefaultValue.ToString();

                    //
                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_MINVALUE, ref iVal, dbNo);
                    //if (ret < 0)
                    //{
                    //    break;
                    //}
                    //strPar[5] = iVal.ToString();
                    //
                    strPar[5] = parameter.MinValue.ToString();

                    
                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_MAXVALUE, ref iVal, dbNo);
                    //if (ret < 0)
                    //{
                    //    break;
                    //}                    
                    //strPar[6] = iVal.ToString();
                    // 
                    strPar[6] = parameter.MaxValue.ToString();

                    break;
                case (sbyte)HNCDATATYPE.DTYPE_FLOAT: 
                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_VALUE, ref dVal, dbNo);
                    //strPar[2] = dVal.ToString("F6");
                    //                   
                    String paramJsonDouble = "";
                    ret = MacDataService.GetInstance().GetHashKeyValueString(dbNo, redisKey, fields, ref paramJsonDouble);
                    if (ret < 0)
                    {
                        break;
                    }
                    Parameter<double> parameterDouble = Newtonsoft.Json.JsonConvert.DeserializeObject<Parameter<double>>(paramJsonDouble);
                    strPar[2] = parameterDouble.PropValue.ToString("F6");

                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_DFVALUE, ref dVal, dbNo);
                    //if (ret < 0)
                    //{
                    //    break;
                    //
                    //strPar[4] = dVal.ToString("F6");
                    strPar[4] = parameterDouble.DefaultValue.ToString("F6");

                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_MINVALUE, ref dVal, dbNo);
                    //if (ret < 0)
                    //{
                    //    break;
                    //}
                    //strPar[5] = dVal.ToString("F6");
                    strPar[5] = parameterDouble.MinValue.ToString("F6");

                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_MAXVALUE, ref dVal, dbNo);
                    //if (ret < 0)
                    //{
                    //    break;
                    //}
                    //strPar[6] = dVal.ToString("F6");
                    strPar[6] = parameterDouble.MaxValue.ToString("F6");
                    break;
                case (sbyte)HNCDATATYPE.DTYPE_STRING:
                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_VALUE, ref strPar[2], dbNo);
                    //if (ret < 0)
                    //{
                    //    break;
                    //}
                    String paramJsonString = "";
                    ret = MacDataService.GetInstance().GetHashKeyValueString(dbNo, redisKey, fields, ref paramJsonString);
                    if (ret < 0)
                    {
                        break;
                    }
                    Parameter<String> parameterString = Newtonsoft.Json.JsonConvert.DeserializeObject<Parameter<String>>(paramJsonString);
                    strPar[2] = parameterString.PropValue;

                    strPar[4] = "N/A";
                    strPar[5] = "N/A";
                    strPar[6] = "N/A";
                    break;
                case (sbyte)HNCDATATYPE.DTYPE_HEX4:
                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_VALUE, ref iVal, dbNo);
                    String paramJsonHEX4 = "";
                    ret = MacDataService.GetInstance().GetHashKeyValueString(dbNo, redisKey, fields, ref paramJsonHEX4);
                    if (ret < 0)
                    {
                        break;
                    }
                    //paramJsonHEX4 = paramJsonHEX4.Replace("0x", "");
                    Parameter<String> parameterHEX4 = Newtonsoft.Json.JsonConvert.DeserializeObject<Parameter<String>>(paramJsonHEX4);
                    strPar[2] = parameterHEX4.PropValue;//"0x" + iVal.ToString("X2");
                    strPar[4] = parameterHEX4.DefaultValue;//"N/A";
                    strPar[5] = parameterHEX4.MinValue;//"N/A";
                    strPar[6] = parameterHEX4.MaxValue;//"N/A";
                    break;
                case (sbyte)HNCDATATYPE.DTYPE_BYTE:
                    //sbyte[] araayBt = new sbyte[HNCDATATYPE.PARAM_STR_LEN];
                    //ret = HncApi.HNC_ParamanGetParaProp(fileNo, recNo, index, (Byte)ParaPropType.PARA_PROP_VALUE, ref actType, dbNo);
                    String paramJsonSByte = "";
                    ret = MacDataService.GetInstance().GetHashKeyValueString(dbNo, redisKey, fields, ref paramJsonSByte);
                    if (ret < 0)
                    {
                        break;
                    }
                    Parameter<SByte[]> parameterSByte = Newtonsoft.Json.JsonConvert.DeserializeObject<Parameter<SByte[]>>(paramJsonSByte);

                    strPar[2] = GetStringFrmByte(parameterSByte.PropValue);
                    strPar[4] = "N/A";
                    strPar[5] = "N/A";
                    strPar[6] = "N/A";
                    break;
                default:
                    strPar[2] = "0";
                    break;
            }*/
            if (ret < 0)
            {
                return -1;
            }

            return 0;
        }

        private string GetStringFrmByte(sbyte[] array)
        {
            string strByte = "";

            int len = 0;
            for (len = 0; len < array.Length; len++)
            {
                if (array[len] < 0)
                {
                    break;
                }
            }
            if (len == 0)
            {
                return strByte;
            }
            strByte += array[0];
            for (int i = 1; i < len; i++)
            {
                strByte += ",";
                strByte += array[i];
            }

            return strByte;
        }

        private void treeViewParaman_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                return;
            }
            if (e.Node.Parent == null)
            {
                m_fileNo = e.Node.Index;
                m_recNo = 0;
            }
            else
            {
                m_fileNo = e.Node.Parent.Index;
                m_recNo = e.Node.Index;
            }
            rowNum = 0;
            InitdataGridViewParam(m_fileNo, m_recNo, dbNo);
        }

        private void UpdatalistViewParm()
        {
            m_fileNo = 0;
            m_recNo = 0;
            rowNum = 0;
            InitdataGridViewParam(m_fileNo, m_recNo, dbNo);
        }

        private void timerUpdataShow_Tick(object sender, EventArgs e)
        {
            if (!dataGridViewParam.Visible)
            {
                timerUpdataShow.Enabled = false;
                return;
            }

            if (treeViewParaman.Nodes.Count == 0)
            {
                AddParamanItemFrmNet(this.dbNo);
            }
            int index;
            if (dataGridViewParam.Rows.Count < rowNum)
            {
                if (dataGridViewParam.Rows.Count == 0)
                {
                    dataGridViewParam.Rows.Add();
                }
                if (dataGridViewParam.Rows[dataGridViewParam.Rows.Count - 1].Displayed)//动态增加行
                {
                    int rowNum_chang = 15;
                    int rowNum_nowsum = dataGridViewParam.Rows.Count;
                    rowNum_nowsum += rowNum_chang;
                    if (rowNum < rowNum_nowsum)
                    {
                        rowNum_nowsum = rowNum;
                    }
                    for (index = dataGridViewParam.RowCount; index < rowNum_nowsum; index++)
                    {
                        dataGridViewParam.Rows.Add();
                    }
                }
            }
            for (index = 0; index < dataGridViewParam.Rows.Count; index++)
            {
                    if (dataGridViewParam.Rows[index].Displayed && dataGridViewParam.Rows[index].Cells[0].Value == null)
                    {
                        string[] strPar = new string[7];
                        if (GetParItemText(m_fileNo, m_recNo, index, dbNo, strPar) == -1)
                        {
                            break;
                        }
                        for (int jj = 0; jj < 7; jj++)
                        {
                            dataGridViewParam.Rows[index].Cells[jj].Value = strPar[jj];
                        }
                }
            }
        }

        public void ClearAlldata()
        {
            timerUpdataShow.Enabled = false;
            treeViewParaman.Nodes.Clear();
            dataGridViewParam.Rows.Clear();
        }

        private void dataGridViewParam_SizeChanged(object sender, EventArgs e)
        {
            dataGridViewParam.Width = this.Parent.Width - this.treeViewParaman.Width - 10;

            dataGridViewParam.Columns[0].Width = dataGridViewParam.Width / 7 - 60;
            dataGridViewParam.Columns[1].Width = dataGridViewParam.Width / 7 + 120;
            dataGridViewParam.Columns[3].Width = dataGridViewParam.Width / 7 - 40;
            dataGridViewParam.Columns[2].Width = dataGridViewParam.Width / 7;
            dataGridViewParam.Columns[4].Width = dataGridViewParam.Width / 7;
            dataGridViewParam.Columns[5].Width = dataGridViewParam.Width / 7;
        }
    }
}
