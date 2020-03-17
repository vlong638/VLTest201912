using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileSystemWatcher
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }


        public static string Directory = @"D:\txjh";
        protected override void OnStart(string[] args)
        {
            fileSystemWatcher1.Path = Directory;
            fileSystemWatcher1.EnableRaisingEvents = true;//开始监听
            fileSystemWatcher1.IncludeSubdirectories = true;

            //fileSystemWatcher1.Changed += new FileSystemEventHandler(OnFileChanged);
            fileSystemWatcher1.Created += new FileSystemEventHandler(OnFileCreated);
            //fileSystemWatcher1.Deleted += new FileSystemEventHandler(OnFileDeleted);
            //fileSystemWatcher1.Renamed += new RenamedEventHandler(OnFileRenamed);

            ////首次同步全部
            //var files = Directory.GetFiles(directory);
            //logger.Info("已有文件:" + string.Join(Environment.NewLine, files));
            //foreach (var file in files)
            //{
            //    ParseTXJH(file);
            //}
        }
        public bool IsFileInUse(string fileName)
        {
            bool inUse = true;

            FileStream fs = null;
            try
            {

                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,

                FileShare.None);

                inUse = false;
            }
            catch
            {

            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用
        }

        private void ParseTXJH(string dataFile,string xmlFile)
        {
            try
            {
                if (!File.Exists(dataFile) || !File.Exists(xmlFile))
                {
                    logger.Info($"内容尚未齐全,dataFile:{dataFile},xmlFile:{xmlFile}");
                    return;
                }
                logger.Info($"开始解析,dataFile:{dataFile},xmlFile:{xmlFile}");

                #region 数据解析
                //基础数据解析
                long binglih = 0;
                DateTime startTime = DateTime.MinValue;
                int tryCount = 1;
                while (IsFileInUse(xmlFile)&& tryCount<3)
                {
                    System.Threading.Thread.Sleep(1000);
                    logger.Info($"文件正在占用中,休眠1s,xmlFile:{xmlFile}");
                    tryCount++;
                }
                var xml = System.IO.File.ReadAllText(xmlFile);
                Regex regexMRID = new Regex(@"\<MRID\>(\d+)\</MRID\>");
                var matchMRID = regexMRID.Match(xml);
                if (matchMRID.Groups.Count != 2)
                {
                    logger.Error("必要数据项缺失,MRID");
                    return;
                }
                else
                {
                    if (!long.TryParse(matchMRID.Groups[1].Value, out binglih))
                    {
                        logger.Error("无效的数据,MRID:" + matchMRID.Groups[1].Value);
                        return;
                    }
                }
                Regex regexStartTime = new Regex(@"\<StartTime\>([\d\s]+)\</StartTime\>");
                var matchStartTime = regexStartTime.Match(xml);
                if (matchStartTime.Groups.Count != 2)
                {
                    logger.Error("必要数据项缺失,StartTime");
                    return;
                }
                else
                {
                    var timeStr = matchStartTime.Groups[1].Value;
                    var indexer = 0;
                    if (timeStr.Length != 14)
                    {
                        logger.Error($"无效的数据,Length:{timeStr.Length},StartTime:{timeStr}");
                        return;
                    }
                    if (!int.TryParse(timeStr.Substring(indexer, 4), out int year))
                    {
                        logger.Error("无效的数据,StartTime:" + timeStr);
                        return;
                    }
                    indexer += 4;
                    if (!int.TryParse(timeStr.Substring(indexer, 2), out int month))
                    {
                        logger.Error("无效的数据,StartTime:" + timeStr);
                        return;
                    }
                    indexer += 2;
                    if (!int.TryParse(timeStr.Substring(indexer, 2), out int day))
                    {
                        logger.Error("无效的数据,StartTime:" + timeStr);
                        return;
                    }
                    indexer += 2;
                    if (!int.TryParse(timeStr.Substring(indexer, 2), out int hour))
                    {
                        logger.Error("无效的数据,StartTime:" + timeStr);
                        return;
                    }
                    indexer += 2;
                    if (!int.TryParse(timeStr.Substring(indexer, 2), out int minite))
                    {
                        logger.Error("无效的数据,StartTime:" + timeStr);
                        return;
                    }
                    indexer += 2;
                    if (!int.TryParse(timeStr.Substring(indexer, 2), out int second))
                    {
                        logger.Error("无效的数据,StartTime:" + timeStr);
                        return;
                    }
                    startTime = new DateTime(year, month, day, hour, minite, second);
                }
                //仪器数据解析
                tryCount = 1;
                while (IsFileInUse(dataFile) && tryCount < 3)
                {
                    System.Threading.Thread.Sleep(1000);
                    logger.Info($"文件正在占用中,休眠1s,dataFile:{dataFile}");
                    tryCount++;
                }
                var data = System.IO.File.ReadAllBytes(dataFile);
                var model = new DrawTXJHModel();
                var dataCount = data.Length / 17;
                model.data1 = new int[dataCount];
                model.data2 = new int[dataCount];
                model.data3 = new int[dataCount];
                model.data4 = new int[dataCount];
                model.data5 = new int[dataCount];
                for (var i = 0; i < data.Length; ++i)
                {
                    var dataIndex = i / 17;
                    switch (i % 17)
                    {
                        case 3: model.data1[dataIndex] = data[i]; break;
                        case 7: model.data2[dataIndex] = data[i]; break;
                        case 11: model.data3[dataIndex] = data[i]; break;
                        case 15: model.data4[dataIndex] = data[i]; break;
                        case 16: model.data5[dataIndex] = data[i]; break;
                    }
                }
                GetDataForTXJHModel entity = new GetDataForTXJHModel();
                entity.binglih = binglih;
                entity.StartTime = startTime;
                entity.FetalHeartData = string.Join(",", model.data1);
                entity.UCData = string.Join(",", model.data3);
                #endregion

                //using (var connection = FileSystemWatcher.DBHelper.GetSQLServerDbConnection(@"Data Source=192.168.50.102;Initial Catalog=fmpt;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=huzfypt;Password=huz3305@2018."))
                using (var connection = DBHelper.GetSQLServerDbConnection(@"Data Source=10.31.102.24,1434;Initial Catalog=fmpt;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=HELETECHUSER;Password=HELEtech123"))
                {
                    var command = connection.CreateCommand();
                    try
                    {
                        connection.Open();
                        command.CommandText = "select count(*) from FM_TXJH where binglih = @binglih and CONVERT(varchar(100), FM_TXJH.StartTime,20)=@FormatStartTime;";
                        command.Parameters.Add(new SqlParameter("@binglih", entity.binglih));
                        command.Parameters.Add(new SqlParameter("@FormatStartTime", entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss")));//2020-03-16 15:20:57
                        var result = (int)command.ExecuteScalar();
                        if (result > 0)
                        {
                            logger.Info($"已有该数据,binglih:{binglih},StartTime:{entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                            return;
                        }

                        command.Parameters.Clear();
                        command.CommandText = "insert into fm_TXJH(binglih,StartTime,FetalHeartData,UCData)values(@binglih,@StartTime,@FetalHeartData,@UCData)";
                        command.Parameters.Add(new SqlParameter("@binglih", entity.binglih));
                        command.Parameters.Add(new SqlParameter("@StartTime", entity.StartTime));
                        command.Parameters.Add(new SqlParameter("@FetalHeartData", entity.FetalHeartData));
                        command.Parameters.Add(new SqlParameter("@UCData", entity.UCData));
                        command.ExecuteNonQuery();
                        command.Dispose();
                        connection.Close();
                        logger.Info($"数据同步成功,binglih:{binglih}");
                    }
                    catch (Exception ex)
                    {
                        logger.Error("插入数据库时报错,", ex);
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("数据库无法连接," + ex.ToString());
            }
        }

        protected override void OnStop()
        {
        }

        bool servicePaused = false;
        FileLogger logger = new FileLogger();
        //private void OnFileChanged(Object source, FileSystemEventArgs e)
        //{
        //    if (servicePaused == false)
        //    {
        //        logger.Info(e.Name + " 这个文件在：" + DateTime.Now.ToString() + "被改动了！");
        //    }
        //}
        //private void OnFileRenamed(Object source, RenamedEventArgs e)
        //{
        //    if (servicePaused == false)
        //    {
        //        logger.Info(e.Name + " 这个文件在：" + DateTime.Now.ToString() + "被重命名了！");
        //    }
        //}
        private void OnFileCreated(Object source, FileSystemEventArgs e)
        {
            if (servicePaused == false)
            {
                logger.Info(e.Name + " 这个文件在：" + DateTime.Now.ToString() + "被创建了！");
                string dataPath;
                string xmlPath;
                string rawName;
                if (e.Name.EndsWith(".xml")||e.Name.EndsWith(".bin"))
                {
                    rawName = e.Name.Replace(".xml", "").Replace(".bin", "");
                    var dataPathTemp = e.FullPath.Substring(0, e.FullPath.LastIndexOf("\\") + 1) + rawName + ".bin";
                    var xmlPathTemp = e.FullPath.Substring(0, e.FullPath.LastIndexOf("\\") + 1) + rawName + ".xml";
                    ParseTXJH(dataPathTemp, xmlPathTemp);
                }
            }
        }
        //private void OnFileDeleted(Object source, FileSystemEventArgs e)
        //{
        //    if (servicePaused == false)
        //    {
        //        logger.Info(e.Name + " 这个文件在：" + DateTime.Now.ToString() + "被删除了！");
        //    }
        //}
    }

    public class GetDataForTXJHModel
    {
        /// <summary>
        /// 病历号
        /// </summary>
        public long binglih { set; get; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 胎心数据
        /// </summary>
        public string FetalHeartData { set; get; }
        /// <summary>
        /// 宫缩数据
        /// </summary>
        public string UCData { set; get; }
    }
    
    public class FileLogger
    {
        static string _Path = Service1.Directory + "\\SyncLog.txt";
        static string Path { get { return _Path; } }

        public void Info(LogData locator)
        {
            File.AppendAllText(Path, locator.ToString());
        }

        public void Info(string message)
        {
            Info(new LogData(message));
        }

        public void Info(string message, Exception ex)
        {
            Info(new LogData(message + ex.ToString()));
        }

        public void Error(LogData locator)
        {
            File.AppendAllText(Path, locator.ToString());
        }

        public void Error(string message)
        {
            Error(new LogData(message));
        }

        public void Error(string message, Exception ex)
        {
            Error(new LogData(message + ex.ToString()));
        }
    }

    /// <summary>
    /// 胎心监护数据
    /// </summary>
    public class DrawTXJHModel
    {
        public int[] data1 { set; get; }
        public int[] data2 { set; get; }
        public int[] data3 { set; get; }
        public int[] data4 { set; get; }
        public int[] data5 { set; get; }
    }

    public class LogData
    {
        public LogData(string message)
        {
            Message = message;
        }
        public LogData(string className, string fuctionName, string message)
        {
            ClassName = className;
            FuctionName = fuctionName;
            Message = message;
        }

        public LogData(string className, string fuctionName, string sesction, string message)
        {
            ClassName = className;
            FuctionName = fuctionName;
            Section = sesction;
            Message = message;
        }

        public string ClassName { set; get; }
        public string FuctionName { set; get; }
        public string Section { set; get; }
        public string Message { set; get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------------------------");
            sb.AppendLine(string.Format("-------ClassName  :{0}", ClassName));
            sb.AppendLine(string.Format("-------FuctionName:{0}", FuctionName));
            sb.AppendLine(string.Format("-------Section    :{0}", Section));
            sb.AppendLine(string.Format("-------LogTime    :{0}", DateTime.Now));
            sb.AppendLine(string.Format("-------Message    :{0}", Message));
            return sb.ToString();
        }
    }

    public static class DBHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// VLTODO 可以优化为连接池
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetSQLServerDbConnection(string connectingString)
        {
            return new SqlConnection(connectingString);
        }
    }
}
