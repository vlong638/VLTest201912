using Dapper;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using FrameworkTest.Common.XMLSolution;
using FrameworkTest.ConfigurableEntity;
using FrameworkTest.Kettle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using VL.Consoling.SemiAutoExport;

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
                                bulkCopy.ColumnMappings.Add("ID", "ID");
                                bulkCopy.ColumnMappings.Add("patientid", "patientid");
                                bulkCopy.ColumnMappings.Add("idcard", "idcard");
                                bulkCopy.ColumnMappings.Add("name", "name");
                                bulkCopy.ColumnMappings.Add("orderid", "orderid");
                                bulkCopy.ColumnMappings.Add("setid", "setid");
                                bulkCopy.ColumnMappings.Add("deliverydate", "deliverydate");
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
            cmds.Add(new Command("c3_0526,List应用配置,保存XML", () =>
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
                var groupedProperties = serviceResult.Data.GroupBy(c => c.TableName).ToList();
                var root = new XElement(EntityAppConfig.RootElementName);
                var tableConfigs = groupedProperties.Select(ps => {
                    var configTable = new EntityAppConfig()
                    {
                        ViewName = ps.Key,
                        Properties = ps.Select(p => new EntityAppConfigProperty(p)).ToList()
                    };
                    return configTable;
                });
                root.Add(tableConfigs.Select(c => c.ToXElement()));
                var path = @"D:\ListPages.xml";
                root.SaveAs(path);
            }));
            cmds.Add(new Command("c4_0527,应用配置,读取XML", () =>
            {
                //读取xml
                var path = @"D:\tables.xml";
                XDocument doc = XDocument.Load(path);
                var tableElements = doc.Descendants(EntityAppConfig.NodeElementName);
                var tableConfig = new EntityAppConfig(tableElements.First());
                var tableConfigs = tableElements.Select(c => new EntityAppConfig(c));
            }));
            #endregion
            #region XML
            cmds.Add(new Command("x1,xml", () =>
            {
                XMLHelper.TestCreate(@"D:\a.xml");
            }));
            #endregion
            cmds.Add(new Command("---------------------MockLogin-------------------", () => { }));
            #region MockLogin,顺德
            cmds.Add(new Command("m1,0602,模拟用户登录-本地实验", () =>
            {
                CookieContainer container = new CookieContainer();

                //模拟登陆,成功(注意需要关闭AntiForgeryToken)
                string url = "http://localhost/Research/Account/Login";
                string postData = string.Format("UserName={0}&Password={1}", "vlong638", "701616");
                var result = HttpHelper.Post(url, postData, ref container);
                Console.WriteLine(result);

                //模拟登陆后 请求可匿名的GET接口,成功
                if (false)
                {
                    url = "http://localhost/Research/Home/GetListConfig";
                    postData = "listName=O_PregnantInfo";
                    result = HttpHelper.Post(url, postData, ref container);
                }

                //模拟登陆后 请求带权限的GET接口,成功
                if (false)
                {
                    url = "http://localhost/Research/Pregnant/AllStatistics";
                    postData = "";
                    result = HttpHelper.Get(url, postData, ref container);
                }

                //模拟登陆后 列表GET及POST接口,成功
                if (true)
                {
                    url = "http://localhost/Research/Pregnant/VisitRecordList?pregnantInfoId=67116";
                    postData = "";
                    result = HttpHelper.Get(url, postData, ref container);


                    //Request URL: http://localhost/Research/Pregnant/GetPagedListOfVisitRecord
                    //Request Method: POST
                    //Status Code: 200 OK
                    //Remote Address: [::1]:80
                    //Referrer Policy: no - referrer - when - downgrade

                    //Accept: application / json, text / javascript, */*; q=0.01
                    //Accept-Encoding: gzip, deflate, br
                    //Accept-Language: zh-CN,zh;q=0.9
                    //Connection: keep-alive
                    //Content-Length: 35
                    //Content-Type: application/x-www-form-urlencoded; charset=UTF-8
                    //Cookie: __RequestVerificationToken_L1Jlc2VhcmNo0=kJfR9jnJV5Q5rnvYYtM0KWVJA4EaYioZ2HUSxpmLNmEVkIS8-KEqum7SduHjinr3hS5CUjINli5bR6EffSLoFbwnd9ISMRzzYx1WMPs_Uiw1; .ASPXAUTH=F8B0F3F0678201D6AA0C80061769C204AAFA650929054D5EFC113CCEC9A5E7DFD198C21BD699ED22A0812DB2DACD875F16D0B7EA129FC292B839C6F1C208417633963DBB122D8AE3916D0E1D6680E97B3595EF446D13E6518DB3A8D3293804EEA56DB8A1218EB8665EC684D8335489806559B5B83F7258D0B54BFDE3ED80E700C7A85D76EF675814D95A231D58FC6F53D2445759C0B84242DC6668F79632F42B9AC2A24E3212040BD0CDE24BCF9F891A10BF9F1C05DB674468852860B88AA0D674142252BDE9703AD8DFE6C0B7393FB664D62C2D3829793F7E00B9C53CB16147EF5456ED04442772101F72DFA88E8ACABF0336EE88DE0BDC429EA4DF264B7C60B7E058210AF909430827DE020514A5B90717D68B19F32368D3E97AF7FB1C52581C06690EC3DADA821706CE74B1A6FB4B102BC23DDA5055B6E7FFADD81CFA6219
                    //Host: localhost
                    //Origin: http://localhost
                    //Referer: http://localhost/Research/Pregnant/VisitRecordList?pregnantInfoId=64116
                    //Sec-Fetch-Dest: empty
                    //Sec-Fetch-Mode: cors
                    //Sec-Fetch-Site: same-origin
                    //User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36
                    //X-Requested-With: XMLHttpRequest
                    //page: 1
                    //rows: 20
                    //pregnantInfoId: 64116

                    url = "http://localhost/Research/Pregnant/GetPagedListOfVisitRecord";
                    postData = "page=1&rows=20&pregnantInfoId=64116";
                    result = HttpHelper.Post(url, postData, ref container);
                }

                //模拟登陆后 请求带权限的POST接口,404(注意 此处有奇怪的问题,发布站点页面无法访问,调试可以访问,暂时无法解决)
                if (false)
                {
                    //Accept: application / json, text / javascript, */*; q=0.01
                    //Accept-Encoding: gzip, deflate, br
                    //Accept-Language: zh-CN,zh;q=0.9
                    //Connection: keep-alive
                    //Content-Length: 14
                    //Content-Type: application/x-www-form-urlencoded; charset=UTF-8
                    //Cookie: __RequestVerificationToken_L1Jlc2VhcmNo0=kJfR9jnJV5Q5rnvYYtM0KWVJA4EaYioZ2HUSxpmLNmEVkIS8-KEqum7SduHjinr3hS5CUjINli5bR6EffSLoFbwnd9ISMRzzYx1WMPs_Uiw1; .ASPXAUTH=6AA32F3BDD9F97CFB1894C9E6C5FC68B743BD199C685C2C5F2796822A62D0700E84BE84CBB682E29C6CA4EA7CC97B3E402947859B5AA5F4A58ADAD09D09013282C7E38D6E735C2F7AD0C1EC46922BB02A482227672545C1BBC25388AB688CD08C8A2D21D13BFDA642909A1130DFCD185FBB57DBE7C858558CFA6424C1EFEA05EAB35BBD459A162D9CA8B44FC8CE2A8A358B2579D3859FB7F853E58EB7EDFC8B8CD5A12F6F82860C8C1B685FC7218E20E70F7E642F230FE374530890A6F05C86C5E470742E221C96F0E4178015A6437BDDA821E3E6D0C8C981158E189B78A2AD88465DBC3EE11CFE935097559082DD4C5E87957FDA46206BA993743EE8EA070B2476C00BD5BFE9649C80A507EEC0EB4049E22974F25D6A987CAB2254D2762582E6700878FEDC36305CBB4AA2F114536017E85724EF84D0690F1D6DD483A87D30C
                    //Host: localhost
                    //Origin: http://localhost
                    //Referer: http://localhost/Research/Pregnant/PregnantInfoList
                    //Sec-Fetch-Dest: empty
                    //Sec-Fetch-Mode: cors
                    //Sec-Fetch-Site: same-origin
                    //User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36
                    //X-Requested-With: XMLHttpRequest

                    url = "http://localhost/Pregnant/GetPagedListOfPregnantInfo";
                    postData = string.Format("page={0}&rows={1}", "1", "20");
                    result = HttpHelper.Post(url, postData, ref container);
                }

                Console.WriteLine(result);
            }));
            cmds.Add(new Command("m2,0604,模拟用户登录-申请公钥", () =>
            {
                CookieContainer container = new CookieContainer();
                string url = "http://19.130.211.1:8090/FSFY/logon/publicKey";
                string postData = "";
                var result = HttpHelper.Get(url, postData, ref container);
                Console.WriteLine(result);
                //http://localhost/VL.API/api/OrientSample/GetOne
            }));
            cmds.Add(new Command("m3,0604,模拟用户登录-公钥及加密本地测试", () =>
            {
                //public key
                //"exponent":"010001"
                //"modulus":"00af8dfa5a14e97e58cac7238a5d4ca89478cedcfd196ea643735d64c74df659cd259c8bd60ec046c4d3f6dec3965dc0351f117f8a0ae62ad61c3a41d38c6a93215025c658587f4aa7ceaa9ed08c2ced8873254c417a77403aff9a0abb3bc1d2ff42f856e1a4d447ed0a1626e1099f304b6602e69cdca1a376ae6bf0dad13844cf"
                //密码
                //123
                //加密结果
                //2d36cfe9d49ccdb6cd313c75a7f4308036092f701a068f7fa66ab1835cd03baa3cbc80191e3bf502453d0cacec215a51adcfb883aa24ecc09025b6dc68d9cca20c722dc3e766e92fb15103b434a6c5fc640bbf7937f016c63a11ecad72018a30b0800a67f21d57f6014057f49c29595e7c3f9e5d1874e109a8e9c37be46ce59b

                var exponent = "010001";
                var modulus = "00af8dfa5a14e97e58cac7238a5d4ca89478cedcfd196ea643735d64c74df659cd259c8bd60ec046c4d3f6dec3965dc0351f117f8a0ae62ad61c3a41d38c6a93215025c658587f4aa7ceaa9ed08c2ced8873254c417a77403aff9a0abb3bc1d2ff42f856e1a4d447ed0a1626e1099f304b6602e69cdca1a376ae6bf0dad13844cf";

                //使用
                if (true)
                {
                    RSAParameters rsaParameters = new RSAParameters()
                    {
                        Exponent = FromHex(exponent), // new byte[] {01, 00, 01}
                        Modulus = FromHex(modulus),   // new byte[] {A3, A6, ...}
                    };
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.ImportParameters(rsaParameters);
                    var password = "123";
                    var passwordBytes = Encoding.UTF8.GetBytes(password);
                    var encryptedBytes = rsa.Encrypt(passwordBytes, false);


                    //验证结果 base64格式 与原结果不一致
                    var encryptedPasswordBase64 = Convert.ToBase64String(encryptedBytes);
                    //AEAmntPiYgcluUUFdr5sfaNnTX1ebIxfBSB8TgGM5hmToJPneaLDBv4OTWsv3JOA51A028KIkArFzEeDWJkGgf/sYzH407hwsu4pMAWWyDbvX7sqwVlLQ+Q0/zuqqCXEQYmzvFJUW+Oy3oOQpYJDZdTzKOxqY4CUQ9WyAhmlo3Hx 

                    var encryptedPasswordHex = encryptedBytes.ToHex();
                    //00385429C2844A501574CAAC9F40791A7BC967D8638FE5FBA0B6462048E2F9C11BE1030D2E0ACFFFBF871CA4971927BF14BBE1E9CBB5002B7DD8E127C530C659258EA78B67ADBBD428FB538895078FD2FDAF336426C92586A780E6F60498944D6C9AF4CCC4250402C539F4CD9441062819A6389F904A5C4BFFED295C8DC3AEF08B




                    //原始加密结果
                    //2d36cfe9d49ccdb6cd313c75a7f4308036092f701a068f7fa66ab1835cd03baa3cbc80191e3bf502453d0cacec215a51adcfb883aa24ecc09025b6dc68d9cca20c722dc3e766e92fb15103b434a6c5fc640bbf7937f016c63a11ecad72018a30b0800a67f21d57f6014057f49c29595e7c3f9e5d1874e109a8e9c37be46ce59b
                }


                //using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
                //{
                //    rSACryptoServiceProvider.ImportParameters(new RSAParameters());
                //    encryptedData = rSACryptoServiceProvider.Encrypt(data, fOAEP);
                //}


                //CookieContainer container = new CookieContainer();
                //string url = "http://localhost/VL.API/api/OrientSample/GetPublicKey";
                //string postData = "";
                //var result = HttpHelper.Get(url, postData, ref container);
                //var publicKey = result.FromJson<PublicKey>();
                //var partyId = "35";
                //var userId = "021069";
                //var password = 123;
                //var encryptedPassword = "";
                //var keyPair = new RSAKeyPair(publicKey.exponent, publicKey.modulus);
                //encryptedPassword = Encrypt(keyPair, password);
                //postData = $"pwd={encryptedPassword}&uid={partyId + userId}&url={"logon/myRoles"}";
                //result = HttpHelper.Post(url, postData, ref container);
                //Console.WriteLine(result);
            }));
            cmds.Add(new Command("m4,0604,模拟用户登录-线上模拟测试", () =>
            {
                CookieContainer container = new CookieContainer();
                //本地测试 模拟json传参登陆
                if (false)
                {
                    var url = "http://localhost:51228/api/OrientSample/MockLogin";
                    var postData = new { url = "logon/myRoles", uid = 35000528, pwd = "2d36cfe9d49ccdb6cd313c75a7f4308036092f701a068f7fa66ab1835cd03baa3cbc80191e3bf502453d0cacec215a51adcfb883aa24ecc09025b6dc68d9cca20c722dc3e766e92fb15103b434a6c5fc640bbf7937f016c63a11ecad72018a30b0800a67f21d57f6014057f49c29595e7c3f9e5d1874e109a8e9c37be46ce59b" }.ToJson();
                    var result = HttpHelper.Post(url, postData, ref container, contentType: "application/json;charset=utf-8");
                    Console.WriteLine(result);
                }
                //线上测试 模拟登陆
                if (false)
                {
                    var url = "http://19.130.211.1:8090/FSFY/logon/myRoles";
                    var postData = new { url = "logon/myRoles", uid = "35000528", pwd = "2d36cfe9d49ccdb6cd313c75a7f4308036092f701a068f7fa66ab1835cd03baa3cbc80191e3bf502453d0cacec215a51adcfb883aa24ecc09025b6dc68d9cca20c722dc3e766e92fb15103b434a6c5fc640bbf7937f016c63a11ecad72018a30b0800a67f21d57f6014057f49c29595e7c3f9e5d1874e109a8e9c37be46ce59b" }.ToJson();
                    var result = HttpHelper.Post(url, postData, ref container);
                    Console.WriteLine("--------------Mock Login");
                    Console.WriteLine(result);
                }
                //线上测试 模拟提交
                if (true)
                {
                    var url = "http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID=35000528&sParams=9BC060258D073697E050A8C01F0A710D$9BBF6C400D0280F0E050A8C01F0A4CC8$45608491-9$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                    var postData = $@"data=%5B%7B%22D2%22%3A%224406000000000035%22%2C%22D57%22%3A%22%22%2C%22D70%22%3A%22%22%2C%22D71%22%3A%22%22%2C%22D72%22%3A%22%22%2C%22D1%22%3A%2200000035%22%2C%22D3%22%3A%22%E6%B5%8B%E8%AF%95%22%2C%22D4%22%3A%22CN%22%2C%22D5%22%3A%2201%22%2C%22D6%22%3A%2202%22%2C%22D7%22%3A%2212345678998798%22%2C%22D8%22%3A%221990-01-01%22%2C%22curdate1%22%3A%22%22%2C%22D9%22%3A%2232%22%2C%22D10%22%3A%222%22%2C%22D11%22%3A%2213211111111%22%2C%22D12%22%3A%222%22%2C%22D69%22%3A%22%E4%BD%9B%E5%B1%B1%E5%B8%82%E5%A6%87%E5%B9%BC%E4%BF%9D%E5%81%A5%E9%99%A2%22%2C%22D13%22%3A%22%E5%8D%95%E4%BD%8D%22%2C%22D14%22%3A%22%22%2C%22D15%22%3A%2244%22%2C%22D16%22%3A%224419%22%2C%22D17%22%3A%22441901%22%2C%22D18%22%3A%22%22%2C%22D19%22%3A%22%22%2C%22D20%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%B8%9C%E8%8E%9E%E5%B8%82%E5%B8%82%E7%9B%B4%E8%BE%96%E4%B9%A1%E4%B8%9C%E5%B9%B3%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D21%22%3A%2244%22%2C%22D22%22%3A%224406%22%2C%22D23%22%3A%22440604%22%2C%22D24%22%3A%22440604009%22%2C%22D25%22%3A%22440604009025%22%2C%22D26%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%BD%9B%E5%B1%B1%E5%B8%82%E7%A6%85%E5%9F%8E%E5%8C%BA%E7%9F%B3%E6%B9%BE%E9%95%87%E8%A1%97%E9%81%93%E4%B8%9C%E5%B9%B3%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D27%22%3A%2244%22%2C%22D28%22%3A%224401%22%2C%22D29%22%3A%22440114%22%2C%22D30%22%3A%22%22%2C%22D31%22%3A%22%22%2C%22D32%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E5%B9%BF%E5%B7%9E%E5%B8%82%E8%8A%B1%E9%83%BD%E5%8C%BA%22%2C%22D33%22%3A%221%22%2C%22D34%22%3A%222%22%2C%22D35%22%3A%22%22%2C%22D36%22%3A%221%22%2C%22D37%22%3A%22%22%2C%22D38%22%3A%22%22%2C%22D62%22%3A%22%22%2C%22D63%22%3A%22%22%2C%22D64%22%3A%222%22%2C%22D65%22%3A%221%22%2C%22D66%22%3A%221%22%2C%22D67%22%3A%221%22%2C%22D68%22%3A%224%22%2C%22D39%22%3A%22%E8%AF%B7%E9%97%AE%22%2C%22D40%22%3A%22CN%22%2C%22D41%22%3A%2201%22%2C%22D42%22%3A%2204%22%2C%22D43%22%3A%221111111111%22%2C%22D44%22%3A%221990-01-01%22%2C%22D45%22%3A%2230%22%2C%22D46%22%3A%22%22%2C%22D47%22%3A%22%E5%B9%BF%E4%B8%9C%22%2C%22D48%22%3A%221322222222%22%2C%22D49%22%3A%22%22%2C%22D50%22%3A%22%22%2C%22D51%22%3A%2244%22%2C%22D52%22%3A%224406%22%2C%22D53%22%3A%22440605%22%2C%22D54%22%3A%22440605124%22%2C%22D55%22%3A%22%22%2C%22D56%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%BD%9B%E5%B1%B1%E5%B8%82%E5%8D%97%E6%B5%B7%E5%8C%BA%E7%8B%AE%E5%B1%B1%E9%95%87%E6%B2%99%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D58%22%3A%222020-01-10%22%2C%22D59%22%3A%22440023366%22%2C%22D60%22%3A%22%E9%83%AD%E6%99%93%E7%8E%B2%22%2C%22D61%22%3A%22%22%7D%5D";
                    var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    Console.WriteLine("--------------Mock Commit");
                    Console.WriteLine(result);
                }
            }));
            cmds.Add(new Command("m5,0605,模拟用户登录-WebKit", () =>
            {
                


                //Type obj = Type.GetTypeFromProgID("ScriptControl");
                //if (obj == null) return;`
                //object ScriptControl = Activator.CreateInstance(obj);
                //obj.InvokeMember("Language", BindingFlags.SetProperty, null, ScriptControl, new object[] { "JavaScript" });
                //string js = "function time(a, b, msg){ var sum = a + b; return new Date().getTime() + ': ' + msg + ' = ' + sum }";
                //obj.InvokeMember("AddCode", BindingFlags.InvokeMethod, null, ScriptControl, new object[] { js });
                //var result = obj.InvokeMember("Eval", BindingFlags.InvokeMethod, null, ScriptControl, new object[] { "time(a, b, 'a + b')" }).ToString();

            }));
            #endregion

            cmds.Start();
        }

        static byte[] FromHex(string hex)
        {
            if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0) throw new ArgumentException("not a hexidecimal string");
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes.Add(Convert.ToByte(hex.Substring(i, 2), 16));
            }
            return bytes.ToArray();
        }
    }

    public class LoginModel
    {
        public string url { set; get; }
        public int uid { set; get; }
        public string pwd { set; get; }
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
