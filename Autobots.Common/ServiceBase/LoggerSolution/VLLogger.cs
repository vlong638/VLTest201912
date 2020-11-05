using System.IO;

namespace FrameworkTest.Common.LoggerSolution
{
    public class VLLogger
    {
        public VLLogger(string directory)
        {
            Directory = directory;
        }

        public string Directory { set; get; }

        public void Log(string text, string file = "log.txt")
        {
            var path = Path.Combine(Directory, file);
            File.AppendAllText(path, text);
        }
    }
}
