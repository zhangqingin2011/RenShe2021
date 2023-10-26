﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SCADA
{
    public class XmlWR
    {
        public Dictionary<string, string> configDictionary = new Dictionary<string, string>();

        public enum KEYS
        {
            服务器IP,
            下载路径,
            上传路径,
            本地接收路径
        }

        public string Path = @"..\data\set\TaskManage.xml";

        public void ReadXml(string path)
        {
            Path = path;
            if (!File.Exists(path))
            {
                return;
            }
            FileStream stream = File.OpenRead(path);
            var doc = new XmlDocument();
            doc.Load(stream);
            XmlNode node = doc.SelectSingleNode("/Root/appsetting");
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Attributes != null)
                {
                    if (!configDictionary.ContainsKey(item.Attributes[0].InnerText))
                    {
                        configDictionary.Add(item.Attributes[0].InnerText, item.Attributes[1].InnerText);
                        //Console.WriteLine(item.Attributes[0].InnerText + ":" + item.Attributes[1].InnerText);
                    }
                }
            }
            stream.Close();
        }

        public string ReadNodeValue(int keyindex)
        {
            FileStream stream = File.OpenRead(Path);
            var doc = new XmlDocument();
            doc.Load(stream);
            XmlNode node = doc.SelectSingleNode("/Root/appsetting");
            XmlNode item = node.ChildNodes[keyindex];
            string value = item.Attributes[1].InnerText;
            configDictionary[((KEYS)keyindex).ToString()] = value;
            stream.Close();
            return value;
        }

        public void WriteNodeValue(int keyindex, string value)
        {
            FileStream stream = File.OpenRead(Path);
            var doc = new XmlDocument();
            doc.Load(stream);
            XmlNode node = doc.SelectSingleNode("/Root/appsetting");
            XmlNode item = node.ChildNodes[keyindex];
            XmlElement m_element = (XmlElement)item;
            m_element.SetAttribute("value", value);
            configDictionary[((KEYS)keyindex).ToString()] = value;
            stream.Close();
            doc.Save(Path);
        }
    }
}
