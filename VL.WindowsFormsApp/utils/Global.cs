using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VL.WindowsFormsApp.utils
{
    public class Global
    {
        public const string queque = "queque.txt";
        public const string lastcheck = "LastCheck";
        //public const string log = "log/";
        public const string logfile = "logfile";
        public const string dataexchangelogfile = "dataexchangelogfile.log";

        public static string backgroundWork
        {
            get
            {
                return SetConfig.GetAppConfig("backgroundWork");
            }
            set
            {
                SetConfig.SetAppConfig("backgroundWork", value);
            }
        }

        public static string syncminute
        {
            get
            {
                return SetConfig.GetAppConfig("syncminute");
            }
            set
            {
                SetConfig.SetAppConfig("syncminute", value);
            }
        }
        public static string log
        {
            get
            {
                return SetConfig.GetAppConfig("log");
            }
            set
            {
                SetConfig.SetAppConfig("log", value);
            }
        }
        public static string files
        {
            get
            {
                return SetConfig.GetAppConfig("files");
            }
            set
            {
                SetConfig.SetAppConfig("files", value);
            }
        }
        public static string taskfile
        {
            get
            {
                return SetConfig.GetAppConfig("taskfile");
            }
            set
            {
                SetConfig.SetAppConfig("taskfile", value);
            }
        }
        public static string queuelog
        {
            get
            {
                return SetConfig.GetAppConfig("queuelog");
            }
            set
            {
                SetConfig.SetAppConfig("queuelog", value);
            }
        }
        public static string dataexchangelog
        {
            get
            {
                return SetConfig.GetAppConfig("dataexchangelog");
            }
            set
            {
                SetConfig.SetAppConfig("dataexchangelog", value);
            }
        }
        public static bool b_dataexchangelog
        {
            get
            {
                return (SetConfig.GetAppConfig("b_dataexchangelog") + "").ToUpper() == Boolean.TrueString.ToUpper();
            }
        }
        public static bool DebugLog
        {
            get
            {
                return (SetConfig.GetAppConfig("DebugLog") + "").ToUpper() == Boolean.TrueString.ToUpper();
            }
        }
        public static string dataexchangeversion
        {
            get
            {
                return SetConfig.GetAppConfig("dataexchangeversion");
            }
        }

        public static string PT_Export_HTML
        {
            get
            {
                return SetConfig.GetAppConfig("PT_Export_HTML");
            }
        }
        public static string PT_Export_FILE
        {
            get
            {
                return SetConfig.GetAppConfig("PT_Export_FILE");
            }
        }

        public static string FyptApiUrl
        {
            get
            {
                return SetConfig.GetAppConfig("fyptapiurl");
            }
            set
            {
                SetConfig.SetAppConfig("fyptapiurl", value);
            }
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="str"></param>
        public static void writeLog(string str)
        {
            if (!Directory.Exists("ErrLog"))
            {
                Directory.CreateDirectory("ErrLog");
            }
            using (StreamWriter sw = new StreamWriter(@"ErrLog\ErrLog.txt", true))
            {
                sw.WriteLine(str);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
        }
    }
    public class SetConfig
    {
        public static void SetAppConfig(string appKey, string appValue)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            var xNode = xDoc.SelectSingleNode("//appSettings");

            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
            if (xElem != null) xElem.SetAttribute("value", appValue);
            else
            {
                var xNewElem = xDoc.CreateElement("add");
                xNewElem.SetAttribute("key", appKey);
                xNewElem.SetAttribute("value", appValue);
                xNode.AppendChild(xNewElem);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }

        public static string GetAppConfig(string appKey)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            var xNode = xDoc.SelectSingleNode("//appSettings");

            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");

            if (xElem != null)
            {
                return xElem.Attributes["value"].Value;
            }
            return string.Empty;
        }
    }
}
