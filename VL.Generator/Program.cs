using System;
using System.IO;

namespace VL.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var sql = @"D:\Project\VLTest2015\VL.Generator\SQLs\PREGNANTINFO.sql";
            var lines = File.ReadAllLines(sql);
            foreach (var line in lines)
            {

            }




            Console.WriteLine("Hello World!");
        }
    }
}
