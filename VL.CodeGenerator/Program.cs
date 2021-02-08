using Autobots.Infrastracture.Common.DBSolution;
using System;

namespace VL.CodeGenerator
{
    class Program
    {
        static string LocalMSSQL = "Data Source=127.0.0.1,1433;Initial Catalog=VLTest;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sa;Password=123";

        static void Main(string[] args)
        {
            var dbContext = DBHelper.GetDbContext(LocalMSSQL);


            Console.WriteLine("Hello World!");
        }
    }
}
