using System.IO;

namespace Autobots.EMRServices.FileSolution
{
    public class FileHelper
    {
        public static string GetDirectoryToOutput(string suffix)
        {
            var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, suffix);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}
