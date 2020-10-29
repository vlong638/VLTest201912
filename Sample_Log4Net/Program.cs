using System;

namespace Sample_Log4Net
{
    class Program
    {
        static void Main(string[] args)
        {
            Log4NetLogger.Error("1");

            Console.WriteLine("Hello World!");
        }
    }
}
