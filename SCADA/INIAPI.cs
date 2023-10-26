﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SCADA
{
    public class INIAPI
    {
        public string strFilePath = Application.StartupPath + "\\hisoutput.ini";
        public string strSec = "hisoutput.ini";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        public void savedata(string data)
        {
            WritePrivateProfileString(strSec, "hisdata", data, strFilePath);
        }

        public string loaddata()
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(strSec, "hisdata", "", temp, 1024, strFilePath);
            if (temp.ToString() == "" || temp == null)
            {
                return "0";
            }
            else
            {
                return temp.ToString();
            }
        }
    }
}
