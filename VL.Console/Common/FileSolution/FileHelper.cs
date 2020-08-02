
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.Consolo_Core.Common.FileSolution
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
