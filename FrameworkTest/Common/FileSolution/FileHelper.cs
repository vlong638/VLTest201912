using System.IO;

namespace FrameworkTest.Common.FileSolution
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
    }
}
