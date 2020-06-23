
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.FileSolution
{
    class FileHelper
    {

        public static string GetDirectoryToOutput(string suffix)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), suffix);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}
