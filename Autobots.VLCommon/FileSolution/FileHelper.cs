using System.IO;

namespace VLAutobots.Infrastracture.Common.FileSolution
{
    public class FileHelper
    {
        public static string GetDirectory(string suffix)
        {
            var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, suffix);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public static string ReadAllText(string directory, string file)
        {
            var fullpath = Path.Combine(directory, file);
            if (!File.Exists(fullpath))
                return null;
            return File.ReadAllText(fullpath);
        }
    }
}
