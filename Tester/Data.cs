using FrameworkTest.Common.ValuesSolution;
using System.Collections.Generic;
using System.IO;

namespace Tester
{
    public class Data
    {
        public static void AddData(List<Grouper> grouper)
        {
            var data = File.ReadAllLines(@"C:\Users\Administrator\Desktop\杭妇院\1207.csv");
            foreach (string line in data)
            {
                var values = line.Split(',');
                grouper.Add(new Grouper(values[0], values[3].ToInt() ?? 0, values[4] == "NULL" ? "" : values[4], values[5]));


            }

        }
    }
}
