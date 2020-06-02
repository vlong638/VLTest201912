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
using System.Net;
using System.IO;

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
                var path = @"D:\ListPages.xml";
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
            #region MockLogin
            cmds.Add(new Command("mlogin,0602,模拟用户登录", () =>
            {
                CookieContainer container = new CookieContainer();

                //模拟登陆,成功(注意需要关闭AntiForgeryToken)
                string url = "http://localhost/Research/Account/Login";
                string postData = string.Format("UserName={0}&Password={1}", "vlong638", "701616");
                var result = HttpHelper.Post(url, postData,ref container);
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
            #endregion
            cmds.Start();
        }
    }

    public class HttpHelper
    {
        public static string Post(string url, string postData, ref CookieContainer container, string contentType = "application/x-www-form-urlencoded; charset=UTF-8")
        {
            var result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytepostData = encoding.GetBytes(postData); ;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = contentType;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.CookieContainer = container;//获取验证码时候获取到的cookie会附加在这个容器里面
                request.KeepAlive = true;//建立持久性连接
                if (bytepostData!=null)
                {
                    request.ContentLength = bytepostData.Length;
                    using (Stream requestStm = request.GetRequestStream())
                    {
                        requestStm.Write(bytepostData, 0, bytepostData.Length);
                    }
                }
                //响应
                response = (HttpWebResponse)request.GetResponse();
                container.Add(response.Cookies);
                using (Stream responseStm = response.GetResponseStream())
                {
                    StreamReader redStm = new StreamReader(responseStm, Encoding.UTF8);
                    result = redStm.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static string Get(string url, string postData, ref CookieContainer container)
        {
            var result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytepostData = encoding.GetBytes(postData); ;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:5.0.1) Gecko/20100101 Firefox/5.0.1";
                request.Accept = "image/webp,*/*;q=0.8";

                #region cookie处理
                request.CookieContainer = container;

                //request.CookieContainer = new CookieContainer();//!Very Important.!!!
                //container = request.CookieContainer;
                //var c = request.CookieContainer.GetCookies(request.RequestUri);
                //response = (HttpWebResponse)request.GetResponse();
                //response.Cookies = container.GetCookies(request.RequestUri); 
                #endregion

                response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStm = response.GetResponseStream())
                {
                    StreamReader redStm = new StreamReader(responseStm, Encoding.UTF8);
                    result = redStm.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
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
