using System;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace FileSystemWatcher
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            fileSystemWatcher1.Path = @"D:\Publish\FileWatcherTest";

            fileSystemWatcher1.EnableRaisingEvents = true;//开始监听
            fileSystemWatcher1.IncludeSubdirectories = true;

            fileSystemWatcher1.Changed += new FileSystemEventHandler(OnFileChanged);
            fileSystemWatcher1.Created += new FileSystemEventHandler(OnFileCreated);
            fileSystemWatcher1.Deleted += new FileSystemEventHandler(OnFileDeleted);
            fileSystemWatcher1.Renamed += new RenamedEventHandler(OnFileRenamed);
        }

        protected override void OnStop()
        {
        }

        bool servicePaused = false;
        FileLogger logger = new FileLogger();
        private void OnFileChanged(Object source, FileSystemEventArgs e)
        {
            if (servicePaused == false)
            {
                logger.Info(e.Name + " 这个文件在：" + DateTime.Now.ToString() + "被改动了！");
            }
        }
        private void OnFileRenamed(Object source, RenamedEventArgs e)
        {
            if (servicePaused == false)
            {
                logger.Info(e.Name + " 这个文件在：" + DateTime.Now.ToString() + "被重命名了！");
            }
        }
        private void OnFileCreated(Object source, FileSystemEventArgs e)
        {
            if (servicePaused == false)
            {
                logger.Info(e.Name + " 这个文件在：" + DateTime.Now.ToString() + "被创建了！");
                File.Copy(e.FullPath, @"D:\Publish\FileWatcherTestLog\" + e.Name);

            }
        }
        private void OnFileDeleted(Object source, FileSystemEventArgs e)
        {
            if (servicePaused == false)
            {
                logger.Info(e.Name + " 这个文件在：" + DateTime.Now.ToString() + "被删除了！");
            }
        }
    }

    public class FileLogger 
    {
        static string _Path;
        static string Path
        {
            get
            {
                if (string.IsNullOrEmpty(_Path))
                {
                    _Path = @"D:\Publish\FileWatcherTestLog\log.txt";
                }
                return _Path;
            }
        }

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
}
