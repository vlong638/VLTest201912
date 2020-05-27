using VL.Consoling.SemiAutoExport;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FrameworkTest.Kettle;
using FrameworkTest.ConfigurableEntity;
using FrameworkTest.Common.DBSolution;
using System.Runtime.CompilerServices;
using FrameworkTest.Common.XMLSolution;
using System.Xml.Linq;

namespace FrameworkTest
{
    class Program
    {
        static string LocalMSSQL = "Data Source=127.0.0.1,1433;Initial Catalog=VLTest;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sa;Password=123";
        static string HeleOuterMSSQL = "Data Source=heletech.asuscomm.com,8082;Initial Catalog=HELEESB;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=ESBUSER;Password=ESBPWD";
        static string HeleInnerMSSQL = "Data Source=192.168.50.102,1433;Initial Catalog=VLTest;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=ESBUSER;Password=ESBPWD";

        static void Main(string[] args)
        {
            ///命令对象有助于代码的版本控制,集体非方法的形式堆在一起不利于
            CommandCollection cmds = new CommandCollection();
            cmds.Add(new Command("ls", () =>
            {
                foreach (var cmd in cmds)
                {
                    Console.WriteLine(cmd.Name);
                }
            }));
            #region SQL
            cmds.Add(new Command("---------------------SQL-------------------", () => { }));
            cmds.Add(new Command("s1_0000,数据库连接测试", () =>
            {
                var connectingString = LocalMSSQL;
                try
                {
                    using (var connection = new SqlConnection(connectingString))
                    {
                        connection.Open();
                        var id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                        connection.Close();
                    }
                    Console.WriteLine("数据库连接成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("数据库连接失败," + ex.ToString());
                }
            }));
            cmds.Add(new Command("s12_0000,数据库连接测试", () =>
            {
                var context = DBHelper.GetDbContext(LocalMSSQL);
                var serviceResult = context.DelegateTransaction((group) =>
                {
                    var id = group.Connection.ExecuteScalar<long>(@"select max(id) from O_LabResult", transaction: group.Transaction);
                    return id;
                });

                //if (!serviceResult.IsSuccess)
                //    return Error(serviceResult.Data, serviceResult.Messages);
                //return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List });
            }));
            cmds.Add(new Command("s2_0525,数据库插入性能测试", () =>
            {
                var amount = 10000;
                Console.WriteLine($"基于连接:{nameof(LocalMSSQL)}测试");
                //无特殊处理,10000条,耗时2.5秒
                if (false)
                {
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        var id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                        id++;
                        var t1 = DateTime.Now;
                        for (long i = id; i < id + amount; i++)
                        {
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = 0,
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            var sql = $@"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                            var result = connection.Execute(sql, entity);
                        }
                        var t2 = DateTime.Now;
                        var ts = t2 - t1;
                        Console.WriteLine($"一次连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}");
                    }
                }
                //Values拼接单次200条,10000条,耗时15.58秒
                if (false)
                {
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        var bach = 200;
                        var id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                        id++;
                        var t1 = DateTime.Now;
                        var currentBach = 1;
                        for (long i = id; i < id + amount; i++)
                        {
                            var sql = $@"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values";
                            var valueSQLs = new List<string>();
                            Dictionary<string, object> values = new Dictionary<string, object>();
                            for (int j = currentBach; j <= bach; j++)
                            {
                                O_LabResult entity = new O_LabResult()
                                {
                                    ID = i,
                                    patientid = 0,
                                    idcard = i.ToString(),
                                    name = i.ToString(),
                                    orderid = i.ToString(),
                                    setid = 0,
                                    deliverydate = DateTime.Now,
                                };
                                valueSQLs.Add($@"(@ID{i},@patientid{i},@idcard{i},@name{i},@orderid{i},@setid{i},@deliverydate{i})");
                                values.Add("@ID" + i, entity.ID);
                                values.Add("@patientid" + i, entity.patientid);
                                values.Add("@idcard" + i, entity.idcard);
                                values.Add("@name" + i, entity.name);
                                values.Add("@orderid" + i, entity.orderid);
                                values.Add("@setid" + i, entity.setid);
                                values.Add("@deliverydate" + i, entity.deliverydate);
                                i++;
                            }
                            sql += string.Join(",", valueSQLs);
                            var result = connection.Execute(sql, values);
                        }
                        var t2 = DateTime.Now;
                        var ts = t2 - t1;
                        Console.WriteLine($"一次连接{amount}次一批次{bach}条插入(无事务),耗时:{ts.TotalSeconds}");
                    }
                }
                //独立连接,增加些许消耗
                if (false)
                {
                    var id = 0L;
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                    }
                    id++;
                    var t1 = DateTime.Now;
                    for (long i = id; i <= id + amount; i++)
                    {
                        using (var connection = new SqlConnection(LocalMSSQL))
                        {
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = 0,
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            var sql = $@"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                            var result = connection.Execute(sql, entity);
                        }
                    }
                    var t2 = DateTime.Now;
                    var ts = t2 - t1;
                    Console.WriteLine($"独立连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}");
                }
                //开启事务,增加些许消耗
                if (false)
                {
                    var id = 0L;
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                    }
                    id++;
                    var t1 = DateTime.Now;
                    for (long i = id; i <= id + amount; i++)
                    {
                        using (var connection = new SqlConnection(LocalMSSQL))
                        {
                            connection.Open();
                            var transaction = connection.BeginTransaction();
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = 0,
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            var sql = $@"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                            var result = connection.Execute(sql, entity, transaction);
                            transaction.Commit();
                            connection.Close();
                        }
                    }
                    var t2 = DateTime.Now;
                    var ts = t2 - t1;
                    Console.WriteLine($"独立连接{amount}次插入(有事务),耗时:{ts.TotalSeconds}");
                }
                //SqlBulkCopy批量,10000条,耗时0.2秒
                if (false)
                {
                    var t1 = DateTime.Now;
                    var id = 0L;
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                    }
                    id++;
                    var dt = new DataTable();
                    dt.Columns.Add("ID", typeof(long));
                    dt.Columns.Add("patientid", typeof(long));
                    dt.Columns.Add("idcard", typeof(string));
                    dt.Columns.Add("name", typeof(string));
                    dt.Columns.Add("orderid", typeof(string));
                    dt.Columns.Add("setid", typeof(int));
                    dt.Columns.Add("deliverydate", typeof(DateTime));
                    for (long i = id; i < id + amount; i++)
                    {
                        O_LabResult entity = new O_LabResult()
                        {
                            ID = i,
                            patientid = 0,
                            idcard = i.ToString(),
                            name = i.ToString(),
                            orderid = i.ToString(),
                            setid = 0,
                            deliverydate = DateTime.Now,
                        };
                        dt.Rows.Add(entity.ID, entity.patientid, entity.idcard, entity.name, entity.orderid, entity.setid, entity.deliverydate);
                    }
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        connection.Open();
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            try
                            {
                                //插入到数据库的目标表 TbA：表名
                                bulkCopy.DestinationTableName = "[dbo].[O_LabResult]";
                                //内存表的字段 对应数据库表的字段 
                                bulkCopy.ColumnMappings.Add("ID","ID");
                                bulkCopy.ColumnMappings.Add("patientid","patientid");
                                bulkCopy.ColumnMappings.Add("idcard","idcard");
                                bulkCopy.ColumnMappings.Add("name","name");
                                bulkCopy.ColumnMappings.Add("orderid","orderid");
                                bulkCopy.ColumnMappings.Add("setid","setid");
                                bulkCopy.ColumnMappings.Add("deliverydate","deliverydate");
                                bulkCopy.WriteToServer(dt);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        connection.Close();
                    }
                    var t2 = DateTime.Now;
                    var ts = t2 - t1;
                    Console.WriteLine($"SqlBulkCopy,{amount}次插入(无事务),耗时:{ts.TotalSeconds}");
                }
                Console.WriteLine($"基于连接:{nameof(HeleInnerMSSQL)}测试");
                //万条30秒,公司数据库性能瓶颈300条/s
                if (false)
                {
                    using (var connection = new SqlConnection(HeleInnerMSSQL))
                    {
                        var id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                        id++;
                        var t1 = DateTime.Now;
                        for (long i = id; i < id + amount; i++)
                        {
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = 0,
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            var sql = $@"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                            var result = connection.Execute(sql, entity);
                        }
                        var t2 = DateTime.Now;
                        var ts = t2 - t1;
                        Console.WriteLine($"一次连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}");
                    }
                }
            }));
            #endregion
            #region SemiAutoExport,半自动导出
            cmds.Add(new Command("c1_0525,半自动导出", () =>
            {
                var data = new AllService().GetData(1);
            }));
            #endregion
            #region ConfigurableEntity,配置化对象
            cmds.Add(new Command("c2_0526,数据库表结构配置", () =>
            {
                var context = DBHelper.GetDbContext(LocalMSSQL);
                var serviceResult = context.DelegateTransaction((group) =>
                {
                    var sql = $@"
select 
    def.*
    from
    (
	    SELECT
			    tb.name as TableName,
			    col.column_id Id,
			    col.name AS ColumnName,
			    typ.name as DataType,
			    col.max_length AS MaxLength,
			    col.[precision] AS Precision,
			    col.scale AS Scale,
			    col.is_nullable AS IsNullable,
			    col.is_identity AS IsIdentity,
		        case when exists
				    (SELECT 1
					    FROM
						    sys.indexes idx
							    join sys.index_columns idxCol
							    on(idx.object_id = idxCol.object_id)
					     WHERE
							    idx.object_id = col.object_id
							     AND idxCol.index_column_id = col.column_id
							     AND idx.is_primary_key = 1
				     ) THEN 1 ELSE 0 END AS IsPrimaryKey,
			    cast(isnull(prop.[value], '-') as varchar(200)) AS Description
	    FROM sys.columns col
	    left join sys.types typ on(col.system_type_id = typ.system_type_id)
	    left join sys.extended_properties prop on(col.object_id = prop.major_id AND prop.minor_id = col.column_id)
	    left join sys.tables tb on col.object_id = tb.object_id
	    where tb.name is not null
    ) as def
order by def.[TableName],def.Id
                        ";
                    var entityDBConfig = group.Connection.Query<EntityDBConfig>(sql, transaction: group.Transaction);
                    return entityDBConfig;
                });
            }));
            cmds.Add(new Command("c3_0526,应用配置,保存XML", () =>
            {
                //读取数据
                var context = DBHelper.GetDbContext(LocalMSSQL);
                var serviceResult = context.DelegateTransaction((group) =>
                {
                    var sql = $@"
select 
    def.*
    from
    (
	    SELECT
			    tb.name as TableName,
			    col.column_id Id,
			    col.name AS ColumnName,
			    typ.name as DataType,
			    col.max_length AS MaxLength,
			    col.[precision] AS Precision,
			    col.scale AS Scale,
			    col.is_nullable AS IsNullable,
			    col.is_identity AS IsIdentity,
		        case when exists
				    (SELECT 1
					    FROM
						    sys.indexes idx
							    join sys.index_columns idxCol
							    on(idx.object_id = idxCol.object_id)
					     WHERE
							    idx.object_id = col.object_id
							     AND idxCol.index_column_id = col.column_id
							     AND idx.is_primary_key = 1
				     ) THEN 1 ELSE 0 END AS IsPrimaryKey,
			    cast(isnull(prop.[value], '-') as varchar(200)) AS Description
	    FROM sys.columns col
	    left join sys.types typ on(col.system_type_id = typ.system_type_id)
	    left join sys.extended_properties prop on(col.object_id = prop.major_id AND prop.minor_id = col.column_id)
	    left join sys.tables tb on col.object_id = tb.object_id
	    where tb.name is not null
    ) as def
order by def.[TableName],def.Id
                        ";
                    var entityDBConfig = group.Connection.Query<EntityDBConfig>(sql, transaction: group.Transaction);
                    return entityDBConfig;
                });

                //写成xml
                var path = @"D:\tables.xml";
                var groupedProperties = serviceResult.Data.GroupBy(c => c.TableName).ToList();
                var root = new XElement("Tables");
                var tableConfigs = groupedProperties.Select(ps => {
                    var configTable = new EntityAppConfigTable()
                    {
                        TableName = ps.Key,
                        Properties = ps.Select(p => new EntityAppConfigProperty(p)).ToList()
                    };
                    return configTable;
                });
                root.Add(tableConfigs.Select(c => c.ToXElement()));
                root.SaveAs(path);
            }));
            cmds.Add(new Command("c4_0527,应用配置,读取XML", () =>
            {
                //读取xml
                var path = @"D:\tables.xml";
                XDocument doc = XDocument.Load(path);
                var tableElements = doc.Descendants("Table");
                var tableConfig = new EntityAppConfigTable(tableElements.First());
                var tableConfigs = tableElements.Select(c => new EntityAppConfigTable(c));
            }));
            #endregion
            #region XML
            cmds.Add(new Command("x1,xml", () =>
            {
                XMLHelper.TestCreate(@"D:\a.xml");
            }));
            #endregion
            cmds.Start();
        }
    }

    public class Constraints
    {
    }

    #region CommandMode

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

    #endregion
}
