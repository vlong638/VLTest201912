using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ResearchAPI.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            var businessEntities = ConfigHelper.GetBusinessEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/BusinessEntities"), "BusinessEntities_产科.xml");
            var configs = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Configs", "config.json")).FromJson<DBConfig>();
            Console.WriteLine("1 for enum update");
            Console.WriteLine("2 for init database");
            var item = Console.ReadLine();
            switch (item)
            {
                case "1":
                    break;
                case "2":
                    var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Docs/SQLs"));
                    var dBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First().Value);
                    foreach (var file in files)
                    {
                        var sql = File.ReadAllText(file);
                        Console.WriteLine($"正在执行{file}");
                        try
                        {
                            dBContext.Execute(sql);
                            Console.WriteLine($"执行成功");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            break;
                        }
                    }
                    break;
                case "3":
                    break;
                default:
                    return;
            }

            Console.ReadLine();
        }
    }
    /// <summary>
    /// 配置样例
    /// </summary>
    public class DBConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public List<VLKeyValue> ConnectionStrings { set; get; }
    }
}
