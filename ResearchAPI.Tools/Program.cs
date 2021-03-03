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
            CommandCollection cmds = new CommandCollection();
            cmds.Add(new Command("ls", () =>
            {
                foreach (var cmd in cmds)
                {
                    Console.WriteLine(cmd.Name);
                }
            }));
            cmds.Add(new Command("i1 for init database", () =>
            {
                var configs = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Configs", "config.json")).FromJson<DBConfig>();
                var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Docs/SQLs"));
                var dBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c=>c.Key== "ResearchConnectionString").Value);
                foreach (var file in files)
                {
                    var sql = File.ReadAllText(file);
                    Console.WriteLine($"正在执行{file}");
                    dBContext.Execute(sql);
                    Console.WriteLine($"执行成功");
                }
            }));
            cmds.Add(new Command("s1 for synchronization", () =>
            {
                var configs = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Configs", "config.json")).FromJson<DBConfig>();
                var sourceDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c =>c.Key== "SourceData").Value);
                var targetDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c =>c.Key== "TargetData").Value);
                var businessEntities = ConfigHelper.GetBusinessEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/BusinessEntities"), "BusinessEntities_产科.xml");
                foreach (var businessEntity in businessEntities)
                {
                    Console.WriteLine($"正在执行同步{targetDBContext.DbGroup.Connection.Database}.dbo.{businessEntity.TargetName}=>{sourceDBContext.DbGroup.Connection.Database}.dbo.{businessEntity.SourceName}");
                    try
                    {
                        var sql = $"select * into {targetDBContext.DbGroup.Connection.Database}.dbo.{businessEntity.TargetName} from {sourceDBContext.DbGroup.Connection.Database}.dbo.{businessEntity.SourceName}";
                        targetDBContext.Execute(sql);
                        Console.WriteLine($"成功");
                    }
                    catch (Exception ex )
                    {
                        Console.WriteLine($"失败");
                    }
                }
            }));
            cmds.Start();
        }
    }

    public class CommandCollection : List<Command>
    {
        public void Start()
        {
            Console.WriteLine("wait for a command,enter `q` to close");
            string s = "ls";
            do
            {
                var command = this.FirstOrDefault(c => c.Name.StartsWith(s));
                if (command == null)
                {
                    Console.WriteLine("wait for a command,enter `q` to close");
                    continue;
                }
                try
                {
                    command.Execute();
                }
                catch (Exception e
                )
                {
                    var error = e;
                    Console.WriteLine(e.ToString());
                }
                Console.WriteLine("wait for a command,enter `q` to close");
            }
            while ((s = Console.ReadLine().ToLower()) != "q");
        }
    }

    public class Command
    {
        public Command(string name, Action exe)
        {
            Name = name.ToLower();
            Execute = exe;
        }

        public string Name { set; get; }

        public Action Execute { set; get; }
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
