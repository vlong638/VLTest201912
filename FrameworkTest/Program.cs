﻿using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Business.ExcelGenerator;
using FrameworkTest.Business.GJPredeliveryAsync;
using FrameworkTest.Business.Research;
using FrameworkTest.Business.SDMockCommit;
using FrameworkTest.Business.TaskScheduler;
using FrameworkTest.Common.ConfigSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using FrameworkTest.Common.XMLSolution;
using FrameworkTest.ConfigurableEntity;
using FrameworkTest.Kettle;
using FrameworkTest.Research;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using VL.Consoling.SemiAutoExport;

namespace FrameworkTest
{


    class Program
    {
        static string LocalMSSQL = "Data Source=127.0.0.1,1433;Initial Catalog=VLTest02;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sa;Password=123";
        static string HeleOuterMSSQL = "Data Source=heletech.asuscomm.com,8082;Initial Catalog=HELEESB;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=ESBUSER;Password=ESBPWD";
        static string HeleInnerMSSQL = "Data Source=192.168.50.102,1433;Initial Catalog=VLTest;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=ESBUSER;Password=ESBPWD";

        static void Main(string[] args)
        {
            var obj = new CQJL_WOMAN_FORM_SAVE_Data();
            //var resul22t = TypeDescriptor.GetProperties(typeof(CQJL_WOMAN_FORM_SAVE_Data))[nameof(obj.D1)]
            //    .GetDescription();
            //var resul32t = obj.GetProperty(nameof(obj.D1)).GetDescription();
            //var ttt = SubFunctionCategory.女方信息.GetDescription();
            //var tt2 = obj.D1.GetDescription();


            //if (false)
            //{
            //    var container = new CookieContainer();
            //    var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/CQJL_LIST&sUserID=35021069&sParams=null$45608491-9$2$%E7%8E%8B%E9%A6%99%E7%8E%89$P$P$4406";
            //    var postData = "pageIndex=0&pageSize=1000&sortField=&sortOrder=";
            //    var result = SDHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            //    Console.WriteLine(result);

            //    //var postData = new { pageIndex = 0, pageSize = 20, sortField = "", sortOrder = "" }.ToJson();
            //    //var result = SDHttpHelper.Post(url, postData, ref container, contentType: "application/json; charset=UTF-8");
            //}

            //Console.ReadLine();

            //var cjson = FileHelper.ReadAllText(FileHelper.GetDirectory("Configs"), "config.json");
            //var cxml = FileHelper.ReadAllText(FileHelper.GetDirectory("Configs"), "config.xml");
            //var configDic = ConfigHelper.GetVLConfig(cxml);

            //var jsonConfig = Newtonsoft.Json.JsonConvert.DeserializeObject

            //var tster = @"<If Operator=""NotEmpty"" ComponentName=""登记日期End""> and h.registerdate <= @登记日期End </If>";
            //var xItem = new XDocument(new XElement("root", XElement.Parse(tster)));

            #region tests

            //           var yss = @") 管理率
            //from cc_highrisk h";
            //           foreach (var c in yss)
            //           {
            //               Console.WriteLine(c);
            //           }
            //           var yssen = WebUtility.HtmlDecode(yss);
            //           foreach (var c in yssen)
            //           {
            //               Console.WriteLine(c);
            //           }

            //var ycc = "1/2";
            //var ycc2 = ycc.GetSubStringOrEmpty((ycc?.Length ?? 0) - 1) ?? "";//:"1", 本次胎次


            //var sdate = "2020年07月26日 23时14分";
            //var subb = sdate.GetSubStringOrEmpty(sdate.Length - 1);
            //sdate = null;
            //subb = sdate.GetSubStringOrEmpty(sdate?.Length ?? 0 - 1);
            //            ryrqdata fmrqdate
            //2020年07月26日 21时53分  2020年07月26日 23时14分

            //var sdate = "2020年07月26日 23时14分";
            //var dds = DateTime.Now;
            //DateTime.TryParse(sdate, out dds);

            //var sste = @"1.孕28-30周行胎儿超声心动图；2.一周复查脐血流；3.注意休息，定期四周产检。\n低分子肝素皮下注射。";
            //sste = FSService.RemoveUnacceptableString(sste);

            #region 特殊值异常
            //            var result22 = @"
            //{
            //sj:"".33"",
            //xl:""/"",
            //dsc:""处理成功"",
            //code:""0"",
            //scr:""杭州创业软件"",
            //data1:[
            //{
            //	zd:""孕3产1,孕11+2周,单活胎,疤痕子宫,""

            //}
            //,{
            //	zd:""""

            //}
            //,{
            //	zd:""""

            //}
            //],
            //data:[
            //{
            //D1:""2020-07-15"",
            //D2:"""",
            //D3:"""",
            //D4:"""",
            //D5:"""",
            //D6:"""",
            //D7:"""",
            //D8:"""",
            //D9:"""",
            //D10:""耻骨联合上 横指"",
            //D11:""\"",
            //D12:"""",
            //D13:""168"",
            //D14:"""",
            //D15:"""",
            //D16:""1"",
            //D17:""无"",
            //D18:"""",
            //D19:"""",
            //D20:""1"",
            //D21:""1.6瘢痕子官"",
            //D22:""2"",
            //D23:""2"",
            //D24:"""",
            //D25:"""",
            //D26:""因NT增厚，建议产前诊断"",
            //D27:""个人卫生,心理咨询,营养咨询,避免致畸因素和疾病对胚胎的不良影响"",
            //D28:""1"",
            //D29:"""",
            //D30:""1"",
            //D31:""1"",
            //D32:"""",
            //D33:"""",
            //D34:"""",
            //D35:"""",
            //D36:""张军莲"",
            //D37:""45608805-5"",
            //D38:""2020-07-15 00:00:00.0"",
            //D39:""2020-08-10"",
            //D41:""2"",
            //D42:"""",
            //D43:""无"",
            //D44:"""",
            //D46:"""",
            //D47:"""",
            //D48:"""",
            //D49:"""",
            //D50:"""",
            //D51:"""",
            //D52:"""",
            //D53:"""",
            //D54:""无"",
            //D55:""无"",
            //D56:""1"",
            //D57:""""
            //}
            //]}
            //";
            //            result22 = result22.Replace("\\", "");
            //            var re2 = result22.FromJson<WMH_CQBJ_CQJC_READ>(); 
            #endregion

            //var sss = "117/70";
            //var sl = sss.GetSubStringOrEmpty(sss.IndexOf("/") + 1);
            //sss = "";
            //sl = sss.GetSubStringOrEmpty(sss.IndexOf("/") + 1);

            //VLConstraints.DateTime.GetYearsBy(new DateTime(2019, 7, 13), new DateTime(2020, 7, 14));
            //VLConstraints.DateTime.GetYearsBy(new DateTime(2019, 7, 14), new DateTime(2020, 7, 14));
            //VLConstraints.DateTime.GetYearsBy(new DateTime(2019, 8, 13), new DateTime(2020, 7, 14));
            //VLConstraints.DateTime.GetYearsBy(new DateTime(2019, 5, 13), new DateTime(2020, 7, 14));


            //string ssda = null;
            //bool bb = (ssda ?? "").Contains("1");

            //decimal ddd = 1.1M;
            //int? isd = ddd.ToInt();

            ////[{ "index":"0","pregstatus":"人流","babysex":"0","babyweight":"","pregnantage":"2017年6月"},{ "index":"2","pregstatus":"顺产-足月-健,足月产-亡,巨大胎,顺产-早产-健,早产-亡","babysex":"","babyweight":"","pregnantage":"2020年6月"}]
            //var pregnanthistorystt = @"[{ ""index"":""0"",""pregstatus"":""人流"",""babysex"":""0"",""babyweight"":"""",""pregnantage"":""2017年6月""},{ ""index"":""0"",""pregstatus"":""人流"",""babysex"":""0"",""babyweight"":"""",""pregnantage"":""2018年6月""},{ ""index"":""2"",""pregstatus"":""顺产-足月-健,足月产-亡,巨大胎,顺产-早产-健,早产-亡"",""babysex"":"""",""babyweight"":"""",""pregnantage"":""""}]".FromJson<List<pregnanthistory>>();
            ////var pregnanthistorys = pregnantInfo.pregnanthistory?.FromJson<List<pregnanthistory>>();
            ////孕次排序
            //var pregnanthistorystt2 = pregnanthistorystt.OrderBy(c => c.pregnantage).ToList();

            #region 简单测试

            //VLConstraints.GetGestationalWeeksByPrenatalDate(new DateTime(2020, 10, 8), DateTime.Now);

            ////整理佛山高危`
            //var sourceFile = @"C:\Users\vlong\Desktop\实体.txt";
            //var dicDatas = File.ReadAllText(sourceFile).FromJson<WMH_GWYCF_GW_LIST1>();
            //var targetFile = @"C:\Users\vlong\Desktop\实体2.txt";
            //var sstr = string.Join("\r\n", dicDatas.data.Select(c => c.D9 + "\t" + c.D5 + "\t" + c.D6));
            //File.WriteAllText(targetFile, sstr);

            //try
            //{
            //    var TargetTypes = typeof(TargetType).GetAllEnums();
            //    foreach (TargetType TargetType in TargetTypes)
            //    {
            //        switch (TargetType)
            //        {
            //            case TargetType.PregnantInfo:
            //                break;
            //            case TargetType.HistoryEnquiry:
            //                break;
            //            case TargetType.PhysicalExamination:
            //                break;
            //            case TargetType.ProfessionalExamination:
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //    var nonType = typeof(Int32).GetAllEnums();

            //}
            //catch (Exception ex)
            //{
            //    var eee = ex.ToString();
            //}

            //var sss = @"[{""index"":""0"",""heartrate"":""66"",""position"":""01"",""presentposition"":""1"",""fetalmove"":""1""},{""index"":""2"",""heartrate"":""88"",""position"":""02"",""presentposition"":""2"",""fetalmove"":""2""}]";
            //var fetaldatas = sss.FromJson<List<fetaldata>>();

            //子项解析测试
            //var pregnanthistorysTest = @"[{""index"":""0"",""pregstatus"":""人流"",""babysex"":""0"",""babyweight"":"""",""pregnantage"":""2017年6月""},{""index"":""2"",""pregstatus"":""顺产-足月-健,足月产-亡,巨大胎,顺产-早产-健,早产-亡"",""babysex"":"""",""babyweight"":"""",""pregnantage"":""""}]"
            //    .FromJson<List<pregnanthistory>>();
            //foreach (var pregnanthistory in pregnanthistorysTest)
            //{
            //    var statuss = pregnanthistory.Pregstatuss;
            //}

            //枚举转义测试
            //var r1054 = VLConstraints.GetNationGB_T_2659ByNationGB_T_2659_2000("4");
            //r1054 = VLConstraints.GetNationGB_T_2659ByNationGB_T_2659_2000("5");
            //r1054 = VLConstraints.GetNationGB_T_2659ByNationGB_T_2659_2000("238");

            //var r1534 = VLConstraints.GetDegree_STD_CULTURALDEGByDegree_Hele("11");
            //r1534 = VLConstraints.GetDegree_STD_CULTURALDEGByDegree_Hele("100");
            //r1534 = VLConstraints.GetDegree_STD_CULTURALDEGByDegree_Hele("88");

            //字符串便捷截取方案测试
            //var s20 = "440601002003".GetSubStringOrEmpty(2);
            //var s22 = "440601002003".GetSubStringOrEmpty(2, 2);
            //var s120 = "440601002003".GetSubStringOrEmpty(12);
            //var s130 = "440601002003".GetSubStringOrEmpty(13);
            //var s85 = "440601002003".GetSubStringOrEmpty(8, 5);
            //var s121 = "440601002003".GetSubStringOrEmpty(12, 1);
            //var s111 = "440601002003".GetSubStringOrEmpty(11, 1);

            //var pu = new WMH_CQBJ_JBXX_FORM_SAVEData()
            //{
            //    D1 = "", //@保健号后8位
            //    D1 = "",
            //};
            //var s = pu.ToJson();

            //var ca1 = "4406060121509659";
            //var ca8 = ca1.Substring(8);

            //var r1 = "{ id:\"A83E21BEE34915FDE05355FE80133FE6\"}";
            //var r3 = new { id = "" };
            //var r2 = r1.FromJsonToAnonymousType(r3); 
            #endregion 
            #endregion

            ///命令对象有助于代码的版本控制,集体非方法的形式堆在一起不利于
            CommandCollection cmds = new CommandCollection();
            cmds.Add(new Command("ls", () =>
            {
                foreach (var cmd in cmds)
                {
                    Console.WriteLine(cmd.Name);
                }
            }));
            #region Excel
            cmds.Add(new Command("xls001,xml", () =>
            {

                var path = Path.Combine(AppContext.BaseDirectory, @"Business\ExcelGenerator", "ExportConfig.xml");
                XDocument doc = XDocument.Load(path);
                var tableElements = doc.Descendants(ExportConfig.NodeElementName);
                var configs = tableElements.Select(c => new ExportConfig(c));

                var input = "高危妊娠表";
                var config = configs.FirstOrDefault(c => c.ExportName == input);
                if (config == null)
                {
                    Console.WriteLine("无效的导出配置");
                    return;
                }
                var modelPath = Path.Combine(AppContext.BaseDirectory, @"Business\ExcelGenerator", config.FileName);
                var outputPath = Path.Combine(AppContext.BaseDirectory, @"Business\ExcelGenerator", "TextExport.xlsx");
                if (!File.Exists(modelPath))
                {
                    Console.WriteLine("模板文件不存在");
                    return;
                }
                var search = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("personname", "贾婷婷") };
                using (FileStream s = File.OpenRead(modelPath))
                {
                    var workbook = new XSSFWorkbook(s);
                    foreach (var sheetConfig in config.Sheets)
                    {
                        sheetConfig.UpdateWheres(search);
                        var sheet = workbook.GetSheet(sheetConfig.SheetName);
                        if (sheet != null)
                        {
                            sheetConfig.PrepareSourceData();
                            sheetConfig.Render(sheet);
                        }
                    }
                    using (Stream stream = File.OpenWrite(outputPath))
                    {
                        workbook.Write(stream);
                    }
                }
            }));
            #endregion
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
                var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                var serviceResult = dbContext.DelegateTransaction((group) =>
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
                var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                var serviceResult = dbContext.DelegateTransaction((group) =>
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
                var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                var serviceResult = dbContext.DelegateTransaction((group) =>
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
                var root = new XElement(ViewConfig.RootElementName);
                var tableConfigs = groupedProperties.Select(ps =>
                {
                    var configTable = new ViewConfig()
                    {
                        ViewName = ps.Key,
                        Properties = ps.Select(p => new ViewConfigProperty(p)).ToList(),
                        Wheres = new List<ViewConfigWhere> { new ViewConfigWhere() },
                        OrderBy = new ViewConfigOrderBy(),
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
                var path = @"D:\ListPages.xml";
                XDocument doc = XDocument.Load(path);
                var tableElements = doc.Descendants(ViewConfig.NodeElementName);
                var tableConfig = new ViewConfig(tableElements.First());
                var tableConfigs = tableElements.Select(c => new ViewConfig(c));
            }));
            #endregion
            #region XML
            cmds.Add(new Command("x1,xml", () =>
            {
                XMLEx.TestCreate(@"D:\a.xml");
            }));
            #endregion
            #region 顺德,佛山同步
            cmds.Add(new Command("m1,0602,模拟用户登录-本地实验", () =>
            {
                CookieContainer container = new CookieContainer();

                //模拟登陆,成功(注意需要关闭AntiForgeryToken)
                string url = "http://localhost/Research/Account/Login";
                string postData = string.Format("UserName={0}&Password={1}", "vlong638", "701616");
                var result = FSHttpHelper.Post(url, postData, ref container);
                Console.WriteLine(result);

                //模拟登陆后 请求可匿名的GET接口,成功
                if (false)
                {
                    url = "http://localhost/Research/Home/GetListConfig";
                    postData = "listName=O_PregnantInfo";
                    result = FSHttpHelper.Post(url, postData, ref container);
                }

                //模拟登陆后 请求带权限的GET接口,成功
                if (false)
                {
                    url = "http://localhost/Research/Pregnant/AllStatistics";
                    postData = "";
                    result = FSHttpHelper.Get(url, postData, ref container);
                }

                //模拟登陆后 列表GET及POST接口,成功
                if (true)
                {
                    url = "http://localhost/Research/Pregnant/VisitRecordList?pregnantInfoId=67116";
                    postData = "";
                    result = FSHttpHelper.Get(url, postData, ref container);

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
                    result = FSHttpHelper.Post(url, postData, ref container);
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
                    result = FSHttpHelper.Post(url, postData, ref container);
                }

                Console.WriteLine(result);
            }));
            cmds.Add(new Command("m2,0604,模拟用户登录-申请公钥", () =>
            {
                CookieContainer container = new CookieContainer();
                string url = "http://19.130.211.1:8090/FSFY/logon/publicKey";
                string postData = "";
                var result = FSHttpHelper.Get(url, postData, ref container);
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
                        Exponent = exponent.ToHexBytes(), // new byte[] {01, 00, 01}
                        Modulus = modulus.ToHexBytes(),   // new byte[] {A3, A6, ...}
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
                //var result = SDHttpHelper.Get(url, postData, ref container);
                //var publicKey = result.FromJson<PublicKey>();
                //var partyId = "35";
                //var userId = "021069";
                //var password = 123;
                //var encryptedPassword = "";
                //var keyPair = new RSAKeyPair(publicKey.exponent, publicKey.modulus);
                //encryptedPassword = Encrypt(keyPair, password);
                //postData = $"pwd={encryptedPassword}&uid={partyId + userId}&url={"logon/myRoles"}";
                //result = SDHttpHelper.Post(url, postData, ref container);
                //Console.WriteLine(result);
            }));
            cmds.Add(new Command("m4,0604,模拟用户登录-登录", () =>
            {
                CookieContainer container = new CookieContainer();
                //本地测试 模拟json传参登陆
                if (false)
                {
                    var url = "http://localhost:51228/api/OrientSample/MockLogin";
                    var postData = new { url = "logon/myRoles", uid = 35000528, pwd = "2d36cfe9d49ccdb6cd313c75a7f4308036092f701a068f7fa66ab1835cd03baa3cbc80191e3bf502453d0cacec215a51adcfb883aa24ecc09025b6dc68d9cca20c722dc3e766e92fb15103b434a6c5fc640bbf7937f016c63a11ecad72018a30b0800a67f21d57f6014057f49c29595e7c3f9e5d1874e109a8e9c37be46ce59b" }.ToJson();
                    var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/json;charset=utf-8");
                    Console.WriteLine(result);
                }
                //线上测试 模拟登陆
                if (false)
                {
                    var url = "http://19.130.211.1:8090/FSFY/logon/myRoles";
                    var postData = new { url = "logon/myRoles", uid = "35000528", pwd = "2d36cfe9d49ccdb6cd313c75a7f4308036092f701a068f7fa66ab1835cd03baa3cbc80191e3bf502453d0cacec215a51adcfb883aa24ecc09025b6dc68d9cca20c722dc3e766e92fb15103b434a6c5fc640bbf7937f016c63a11ecad72018a30b0800a67f21d57f6014057f49c29595e7c3f9e5d1874e109a8e9c37be46ce59b" }.ToJson();
                    var result = FSHttpHelper.Post(url, postData, ref container);
                    Console.WriteLine("--------------Mock Login");
                    Console.WriteLine(result);
                }
                //线上测试 模拟提交
                if (true)
                {
                    var url = "http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID=35000528&sParams=9BC060258D073697E050A8C01F0A710D$9BBF6C400D0280F0E050A8C01F0A4CC8$45608491-9$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                    var postData = $@"data=%5B%7B%22D2%22%3A%224406000000000035%22%2C%22D57%22%3A%22%22%2C%22D70%22%3A%22%22%2C%22D71%22%3A%22%22%2C%22D72%22%3A%22%22%2C%22D1%22%3A%2200000035%22%2C%22D3%22%3A%22%E6%B5%8B%E8%AF%95%22%2C%22D4%22%3A%22CN%22%2C%22D5%22%3A%2201%22%2C%22D6%22%3A%2202%22%2C%22D7%22%3A%2212345678998798%22%2C%22D8%22%3A%221990-01-01%22%2C%22curdate1%22%3A%22%22%2C%22D9%22%3A%2232%22%2C%22D10%22%3A%222%22%2C%22D11%22%3A%2213211111111%22%2C%22D12%22%3A%222%22%2C%22D69%22%3A%22%E4%BD%9B%E5%B1%B1%E5%B8%82%E5%A6%87%E5%B9%BC%E4%BF%9D%E5%81%A5%E9%99%A2%22%2C%22D13%22%3A%22%E5%8D%95%E4%BD%8D%22%2C%22D14%22%3A%22%22%2C%22D15%22%3A%2244%22%2C%22D16%22%3A%224419%22%2C%22D17%22%3A%22441901%22%2C%22D18%22%3A%22%22%2C%22D19%22%3A%22%22%2C%22D20%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%B8%9C%E8%8E%9E%E5%B8%82%E5%B8%82%E7%9B%B4%E8%BE%96%E4%B9%A1%E4%B8%9C%E5%B9%B3%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D21%22%3A%2244%22%2C%22D22%22%3A%224406%22%2C%22D23%22%3A%22440604%22%2C%22D24%22%3A%22440604009%22%2C%22D25%22%3A%22440604009025%22%2C%22D26%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%BD%9B%E5%B1%B1%E5%B8%82%E7%A6%85%E5%9F%8E%E5%8C%BA%E7%9F%B3%E6%B9%BE%E9%95%87%E8%A1%97%E9%81%93%E4%B8%9C%E5%B9%B3%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D27%22%3A%2244%22%2C%22D28%22%3A%224401%22%2C%22D29%22%3A%22440114%22%2C%22D30%22%3A%22%22%2C%22D31%22%3A%22%22%2C%22D32%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E5%B9%BF%E5%B7%9E%E5%B8%82%E8%8A%B1%E9%83%BD%E5%8C%BA%22%2C%22D33%22%3A%221%22%2C%22D34%22%3A%222%22%2C%22D35%22%3A%22%22%2C%22D36%22%3A%221%22%2C%22D37%22%3A%22%22%2C%22D38%22%3A%22%22%2C%22D62%22%3A%22%22%2C%22D63%22%3A%22%22%2C%22D64%22%3A%222%22%2C%22D65%22%3A%221%22%2C%22D66%22%3A%221%22%2C%22D67%22%3A%221%22%2C%22D68%22%3A%224%22%2C%22D39%22%3A%22%E8%AF%B7%E9%97%AE%22%2C%22D40%22%3A%22CN%22%2C%22D41%22%3A%2201%22%2C%22D42%22%3A%2204%22%2C%22D43%22%3A%221111111111%22%2C%22D44%22%3A%221990-01-01%22%2C%22D45%22%3A%2230%22%2C%22D46%22%3A%22%22%2C%22D47%22%3A%22%E5%B9%BF%E4%B8%9C%22%2C%22D48%22%3A%221322222222%22%2C%22D49%22%3A%22%22%2C%22D50%22%3A%22%22%2C%22D51%22%3A%2244%22%2C%22D52%22%3A%224406%22%2C%22D53%22%3A%22440605%22%2C%22D54%22%3A%22440605124%22%2C%22D55%22%3A%22%22%2C%22D56%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%BD%9B%E5%B1%B1%E5%B8%82%E5%8D%97%E6%B5%B7%E5%8C%BA%E7%8B%AE%E5%B1%B1%E9%95%87%E6%B2%99%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D58%22%3A%222020-01-10%22%2C%22D59%22%3A%22440023366%22%2C%22D60%22%3A%22%E9%83%AD%E6%99%93%E7%8E%B2%22%2C%22D61%22%3A%22%22%7D%5D";
                    var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
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
            cmds.Add(new Command("fs1,定时任务配置", () =>
            {
                //保存xml
                List<TaskConfig> taskConfigs = new List<TaskConfig>()
                {
                    new TaskConfig(){
                        Id=1,
                        IsActivated=true,
                        Name ="基本信息同步",
                        FrequencyType =FreqencyType.间隔,
                        TaskType =TaskType.定时任务,
                        Interval=  10,
                    }
                };
                var root = new XElement(TaskConfig.RootElementName);
                root.Add(taskConfigs.Select(c => c.ToXElement()));
                root.SaveAs(@"D:\TaskConfigs.xml");


                //读取xml
                var path = @"D:\TaskConfigs.xml";
                XDocument doc = XDocument.Load(path);
                var nodes = doc.Descendants(TaskConfig.NodeElementName);
                var configs = nodes.Select(c => new TaskConfig(c));
                var config = new TaskConfig(nodes.First());
            }));
            cmds.Add(new Command("m11,0616,模拟-查询用户(不存在)", () =>
            {
                CookieContainer container = new CookieContainer();
                //本地测试 查询用户
                if (true)
                {
                    var patientName = "李凤莲";
                    var idcard = $"452427200208013323";
                    var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID=35000528&sParams=P${idcard}$P$P";
                    var postData = "";
                    var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    Console.WriteLine(result);
                }
                //返回值测试
                if (false)
                {
                    var result = @"
{total:""0"",
dsc: ""处理成功"",
code: ""0"",
scr: ""杭州创业软件"",
data:[
],
yjadata:[
]}
                ";
                    var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    bool isExist = re.data.Count != 0;
                }
                if (false)
                {
                    var result = @"
{total:""1"",
dsc: ""处理成功"",
code: ""0"",
scr: ""杭州创业软件"",
data:[
{
                    D1: ""4406060121509762"",
D2: ""A857E6799B91A706E05355FE80137DBD"",
D3: ""???"",
D4: ""44068119911213362X"",
D5: """",
D6: ""佛山市顺德区妇幼保健院"",
D7: ""2020-06-18"",
D8: ""A857E562ABF9A5DFE05355FE801337B1""
}
],
yjadata:[
]}";
                    var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    bool isExist = re.data.Count != 0;
                }

            }));
            cmds.Add(new Command("m12,0616,模拟-获取ID2", () =>
            {
                CookieContainer container = new CookieContainer();
                //本地测试 查询用户
                if (false)
                {
                    var patientName = "姜青青";
                    var idcard = $"522301200408100084";
                    var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID=35000528&sParams=P${idcard}$P$P";
                    var postData = "";
                    var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    Console.WriteLine(result);
                }
                //返回值测试
                if (true)
                {
                    var result = @"
{total:""2"",
dsc: ""处理成功"",
code: ""0"",
scr: ""杭州创业软件"",
data:[
{
                D1: ""4406060121509644"",
D2: ""A82CC45D5B483894E05355FE8013F72B"",
D3: ""姜青青"",
D4: ""522301200408100084"",
D5: ""2004-08-10"",
D6: ""佛山市顺德区妇幼保健院"",
D7: ""2020-06-16"",
D8: ""A82C9478C1BFEB48E05355FE80135A12""
}
,{
                D1: ""4406060121509644"",
D2: ""A82C9478C1CBEB48E05355FE80135A12"",
D3: ""李凤莲"",
D4: ""522301200408100084"",
D5: ""2004-08-10"",
D6: ""佛山市顺德区妇幼保健院"",
D7: ""2020-06-16"",
D8: ""A82C9478C1BFEB48E05355FE80135A12""
}
],
yjadata:[
]}
                ";
                    var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    bool isExist = re.data.Count != 0;
                    var id2 = re.data.First().D2;
                }
            }));
            cmds.Add(new Command("m13,0616,模拟-获取ID1", () =>
            {
                //                //本地测试 查询用户
                //                if (true)
                //                {
                //                    var patientName = "姜青青";
                //                    var idcard = $"522301200408100084";
                //                    var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID=35000528&sParams=P${idcard}$P$P";
                //                    var postData = "";
                //                    var result = SDHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                //                    var re1 = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                //                    bool isExist = re1.data.Count != 0;
                //                    var id2 = re1.data.First().D2;
                //                    url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_READ&sUserID=35000528&sParams={id2}";
                //                    result = SDHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                //                    var re2 = result.FromJson<WMH_CQBJ_JBXX_FORM_READResponse>();
                //                    var id1 = re2.data.First().D75;
                //                    Console.WriteLine(result);
                //                }
                //                //返回值测试
                //                if (true)
                //                {
                ////                    var result = @"
                ////{total:""2"",
                ////dsc: ""处理成功"",
                ////code: ""0"",
                ////scr: ""杭州创业软件"",
                ////data:[
                ////{
                ////                D1: ""4406060121509644"",
                ////D2: ""A82CC45D5B483894E05355FE8013F72B"",
                ////D3: ""姜青青"",
                ////D4: ""522301200408100084"",
                ////D5: ""2004-08-10"",
                ////D6: ""佛山市顺德区妇幼保健院"",
                ////D7: ""2020-06-16"",
                ////D8: ""A82C9478C1BFEB48E05355FE80135A12""
                ////}
                ////,{
                ////                D1: ""4406060121509644"",
                ////D2: ""A82C9478C1CBEB48E05355FE80135A12"",
                ////D3: ""李凤莲"",
                ////D4: ""522301200408100084"",
                ////D5: ""2004-08-10"",
                ////D6: ""佛山市顺德区妇幼保健院"",
                ////D7: ""2020-06-16"",
                ////D8: ""A82C9478C1BFEB48E05355FE80135A12""
                ////}
                ////],
                ////yjadata:[
                ////]}
                ////                ";
                ////                    var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                ////                    bool isExist = re.data.Count != 0;
                ////                    var id2 = re.data.First().D75;
                //                }
            }));
            cmds.Add(new Command("m14,0616,模拟-获取需新建的用户", () =>
            {
                var container = new CookieContainer();
                var pregnantInfos = new List<PregnantInfo>()
                {
                    new PregnantInfo("445222199203080060","张佳宁","13336446460"),
                     new PregnantInfo("430822199405170028","刘文华","13013616407"),
                     new PregnantInfo("440681199412144726","麦淑敏","18219343728"),
                     new PregnantInfo("441625199405215426","黄彩红","14778553939"),
                     new PregnantInfo("441424199709296328","刘苑桥","14715046942"),
                     new PregnantInfo("440681199302084726","陈晓桐","13549964793"),
                     new PregnantInfo("44078119830301232x","黄春凤","13760737592"),
                     new PregnantInfo("44068319881013162X","何映云","13794620167"),
                     new PregnantInfo("441423199302235025","谭杏花","13794004276"),
                     new PregnantInfo("370724198707206205","刘爱伟","13928855187"),
                     new PregnantInfo("440881198205295528","龙珍华","15323333600"),
                     new PregnantInfo("440681199309113665","张丽碧","13690136331"),
                     new PregnantInfo("350822198601301027","赖冬娣","13535838764"),
                     new PregnantInfo("441521199408258228","郑丽雁","13560934796"),
                     new PregnantInfo("430723198507047246","胡荣","15989354586"),
                     new PregnantInfo("440681199011023624","萧静仪","15818078568"),
                     new PregnantInfo("510322199606020707","刘家淑","18381388542"),
                     new PregnantInfo("440681199111182622","梁带旺","13702601225"),
                     new PregnantInfo("452702198708074084","韦立莎","18277222050"),
                     new PregnantInfo("440883199111153526","林春妹","13690199153"),
                     new PregnantInfo("360722199409155120","刘聪华","18664507210"),
                     new PregnantInfo("440223199211114728","邓慧婷","13827703775"),
                     new PregnantInfo("440681199211285426","何家欣","13925941859"),
                     new PregnantInfo("452730199606014769","蓝柳慧","18877813279"),
                     new PregnantInfo("440681199402074760","何燊怡","13622723207"),
                     new PregnantInfo("450422199008231327","万雪梅","15920727397"),
                     new PregnantInfo("510922199411300680","冯菊","18482144854"),
                     new PregnantInfo("441225198901082929","林雪连","13726671355"),
                     new PregnantInfo("441224199111276321","甘玉婷","13679853199"),
                     new PregnantInfo("44142219900314096X","张淑婷","15011886409"),
                     new PregnantInfo("44068119900524202X","梁丽华","13434866175"),
                     new PregnantInfo("450330199901201627","雷兰兰","18145780319"),
                     new PregnantInfo("440923199110212183","陈丽娟","13538984535"),
                     new PregnantInfo("513023198408241224","谢孝燕","18924801376"),
                     new PregnantInfo("452427200208013323","李凤莲","17665663583"),
                     new PregnantInfo("430424199602102340","李晨","13425898687"),
                     new PregnantInfo("440681199002214728","谢小华","15014701364"),
                     new PregnantInfo("441522198803012721","郑惠燕","13480658051"),
                     new PregnantInfo("440681199008065428","何美君","15899812473"),
                     new PregnantInfo("440681199105253623","郭小夏","15363033335"),
                     new PregnantInfo("510322199408255724","郭明英","15281350805"),
                     new PregnantInfo("230203198410121227","田莹","13766551668"),
                     new PregnantInfo("44122319951229142X","王怡君","15815991404"),
                     new PregnantInfo("450881199503155087","卢春菊","19977580481"),
                     new PregnantInfo("440224198710171809","聂兰秀","18676860578"),
                     new PregnantInfo("440681199403153620","梁晓琴","13516633436"),
                     new PregnantInfo("430405199704084041","谭祥鑫","13786452767"),
                     new PregnantInfo("441424199410012546","郑丽丹","15767513313"),
                     new PregnantInfo("430124199110094966","喻娟","18825936610"),
                     new PregnantInfo("44068119790423362X","吕惠蓉","15815612730"),
                     new PregnantInfo("441225199303032563","侯燕梅","15818032116"),
                     new PregnantInfo("45048119920602322X","倪小雲","18777442851"),
                     new PregnantInfo("440229199612103729","黄石尽","13590606910"),
                     new PregnantInfo("412828199311244221","徐艳","17329875217"),
                     new PregnantInfo("440823199812072108","黄彩薇","18824090925"),
                     new PregnantInfo("432930198007253023","李群英","18688259528"),
                     new PregnantInfo("421122198812203567","李美娟","13476677233"),
                     new PregnantInfo("441224199701153743","林家慧","13202882273"),
                     new PregnantInfo("440681199608218029","罗晓琳","13690430749"),
                     new PregnantInfo("511527200211242144","蒋美玲","17883655313"),
                     new PregnantInfo("360730199012251501","官丹丹","18689257591"),
                     new PregnantInfo("421127198709055668","严颖飞","13690825073"),
                     new PregnantInfo("440681197808043623","康趣桥","13695222501"),
                     new PregnantInfo("441823199005247427","陈秀娟","13790017649"),
                     new PregnantInfo("440681199302155424","欧阳凯婷","15217407236"),
                     new PregnantInfo("440222198812101921","曾群花","13727508879"),
                     new PregnantInfo("441802199006173245","陈舒敏","13360362697"),
                     new PregnantInfo("445322198609055528","莫少莲","18666791185"),
                     new PregnantInfo("441224199307040820","李宁","15986137124"),
                     new PregnantInfo("440803198505022426","岑玉斯","13590068912"),
                     new PregnantInfo("440229199209240723","黄玉梅","15875101951"),
                     new PregnantInfo("440223198807250021","陈水莲","15015693310"),
                     new PregnantInfo("440681198310310223","吴艳珍","13715402041"),
                     new PregnantInfo("440681199201233622","翁慧洪","13760943893"),
                     new PregnantInfo("450721199802180563","何雪晴","15812890951"),
                     new PregnantInfo("452523198006136026","王凤琼","15015730927"),
                     new PregnantInfo("513701199204256621","朱丹","15815945058"),
                     new PregnantInfo("440902199202213704","梁燕红","15017763423"),
                     new PregnantInfo("44068119800518542X","何金凤","13924802834"),
                };
                foreach (var pregnantInfo in pregnantInfos)
                {
                    var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID=35000528&sParams=P${pregnantInfo.idcard}$P$P";
                    var postData = "";
                    var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var re1 = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    bool isExist = re1.data.Count != 0;
                    Console.WriteLine($"{isExist},{pregnantInfo.idcard},{pregnantInfo.personname},{pregnantInfo.mobilenumber}");
                }
            }));
            cmds.Add(new Command("m15,0617,模拟-保存基本信息", () =>
            {
                var container = new CookieContainer();
                var userId = "35000528";
                var userName = "廖凤贤";
                var orgId = "45608491-9";
                var orgName = "佛山市妇幼保健院";
                var pregnantInfos = new List<PregnantInfo>()
                {
                     new PregnantInfo("44078119830301232x","黄春凤","13760737592"),
                     new PregnantInfo("44068319881013162X","何映云","13794620167"),
                     new PregnantInfo("441423199302235025","谭杏花","13794004276"),
                     new PregnantInfo("370724198707206205","刘爱伟","13928855187"),
                     new PregnantInfo("440881198205295528","龙珍华","15323333600"),
                     new PregnantInfo("440681199309113665","张丽碧","13690136331"),
                     new PregnantInfo("350822198601301027","赖冬娣","13535838764"),
                     new PregnantInfo("441521199408258228","郑丽雁","13560934796"),
                     new PregnantInfo("430723198507047246","胡荣","15989354586"),
                     new PregnantInfo("440681199011023624","萧静仪","15818078568"),
                     new PregnantInfo("510322199606020707","刘家淑","18381388542"),
                     new PregnantInfo("440681199111182622","梁带旺","13702601225"),
                     new PregnantInfo("452702198708074084","韦立莎","18277222050"),
                     new PregnantInfo("440883199111153526","林春妹","13690199153"),
                     new PregnantInfo("360722199409155120","刘聪华","18664507210"),
                     new PregnantInfo("440223199211114728","邓慧婷","13827703775"),
                     new PregnantInfo("440681199211285426","何家欣","13925941859"),
                     new PregnantInfo("452730199606014769","蓝柳慧","18877813279"),
                     new PregnantInfo("440681199402074760","何燊怡","13622723207"),
                     new PregnantInfo("450422199008231327","万雪梅","15920727397"),
                     new PregnantInfo("510922199411300680","冯菊","18482144854"),
                     new PregnantInfo("441225198901082929","林雪连","13726671355"),
                     new PregnantInfo("441224199111276321","甘玉婷","13679853199"),
                     new PregnantInfo("44142219900314096X","张淑婷","15011886409"),
                     new PregnantInfo("44068119900524202X","梁丽华","13434866175"),
                     new PregnantInfo("450330199901201627","雷兰兰","18145780319"),
                     new PregnantInfo("440923199110212183","陈丽娟","13538984535"),
                     new PregnantInfo("513023198408241224","谢孝燕","18924801376"),
                     new PregnantInfo("452427200208013323","李凤莲","17665663583"),
                     new PregnantInfo("430424199602102340","李晨","13425898687"),
                     new PregnantInfo("440681199002214728","谢小华","15014701364"),
                     new PregnantInfo("441522198803012721","郑惠燕","13480658051"),
                     new PregnantInfo("440681199008065428","何美君","15899812473"),
                     new PregnantInfo("440681199105253623","郭小夏","15363033335"),
                     new PregnantInfo("510322199408255724","郭明英","15281350805"),
                     new PregnantInfo("230203198410121227","田莹","13766551668"),
                     new PregnantInfo("44122319951229142X","王怡君","15815991404"),
                     new PregnantInfo("450881199503155087","卢春菊","19977580481"),
                     new PregnantInfo("440224198710171809","聂兰秀","18676860578"),
                     new PregnantInfo("440681199403153620","梁晓琴","13516633436"),
                     new PregnantInfo("430405199704084041","谭祥鑫","13786452767"),
                     new PregnantInfo("441424199410012546","郑丽丹","15767513313"),
                     new PregnantInfo("430124199110094966","喻娟","18825936610"),
                     new PregnantInfo("44068119790423362X","吕惠蓉","15815612730"),
                     new PregnantInfo("441225199303032563","侯燕梅","15818032116"),
                     new PregnantInfo("45048119920602322X","倪小雲","18777442851"),
                     new PregnantInfo("440229199612103729","黄石尽","13590606910"),
                     new PregnantInfo("412828199311244221","徐艳","17329875217"),
                     new PregnantInfo("440823199812072108","黄彩薇","18824090925"),
                     new PregnantInfo("432930198007253023","李群英","18688259528"),
                     new PregnantInfo("421122198812203567","李美娟","13476677233"),
                     new PregnantInfo("441224199701153743","林家慧","13202882273"),
                     new PregnantInfo("440681199608218029","罗晓琳","13690430749"),
                     new PregnantInfo("511527200211242144","蒋美玲","17883655313"),
                     new PregnantInfo("360730199012251501","官丹丹","18689257591"),
                     new PregnantInfo("421127198709055668","严颖飞","13690825073"),
                     new PregnantInfo("440681197808043623","康趣桥","13695222501"),
                     new PregnantInfo("441823199005247427","陈秀娟","13790017649"),
                     new PregnantInfo("440681199302155424","欧阳凯婷","15217407236"),
                     new PregnantInfo("440222198812101921","曾群花","13727508879"),
                     new PregnantInfo("441802199006173245","陈舒敏","13360362697"),
                     new PregnantInfo("445322198609055528","莫少莲","18666791185"),
                     new PregnantInfo("441224199307040820","李宁","15986137124"),
                     new PregnantInfo("440803198505022426","岑玉斯","13590068912"),
                     new PregnantInfo("440229199209240723","黄玉梅","15875101951"),
                     new PregnantInfo("440223198807250021","陈水莲","15015693310"),
                     new PregnantInfo("440681198310310223","吴艳珍","13715402041"),
                     new PregnantInfo("440681199201233622","翁慧洪","13760943893"),
                     new PregnantInfo("450721199802180563","何雪晴","15812890951"),
                     new PregnantInfo("452523198006136026","王凤琼","15015730927"),
                     new PregnantInfo("513701199204256621","朱丹","15815945058"),
                     new PregnantInfo("440902199202213704","梁燕红","15017763423"),
                     new PregnantInfo("44068119800518542X","何金凤","13924802834"),
                };
                foreach (var pregnantInfo in pregnantInfos)
                {
                    //查询基本信息
                    var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userId}&sParams=P${pregnantInfo.idcard}$P$P";
                    var postData = "";
                    var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var re1 = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    bool isExist = re1.data.Count != 0;
                    Console.WriteLine($"{isExist},{pregnantInfo.idcard},{pregnantInfo.personname},{pregnantInfo.mobilenumber}");
                    if (isExist)
                        continue;
                    //Create 患者主索引
                    url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_QHJC_ID_GET&sUserID={userId}&sParams=1";
                    postData = "";
                    result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var mainId = result.FromJsonToAnonymousType(new { id = "" }).id;
                    Console.WriteLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard}");
                    Console.WriteLine($"mainId:{mainId}");
                    //Create 保健号
                    url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=CDH_GET_ID1&sUserID={userId}&sParams={orgId}";
                    postData = "";
                    result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var careId = result.FromJsonToAnonymousType(new { id = "" }).id;
                    Console.WriteLine($"careId:{careId}");
                    var careIdL8 = careId.Substring(8);
                    //Create 基本信息
                    url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={userId}&sParams=null${mainId}${orgId}$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                    var data = new List<WMH_CQBJ_JBXX_FORM_SAVEData>()
                    {
                        new WMH_CQBJ_JBXX_FORM_SAVEData(){
        D1 = careIdL8, //@保健号后8位
        D2 = careId, //@保健号
        D7 = pregnantInfo.idcard, //身份证
        D58 = DateTime.Now.ToString("yyyy-MM-dd"), //创建时间
        D59 = orgId, //创建机构Id
        D60 = userName, //创建人员
        D61 = "", //病案号
        D69 = orgName, //创建机构名称:佛山市妇幼保健院
        D70 = "", //健康码
        D71 = "", //健康码ID
        D3 = pregnantInfo.personname, //孕妇姓名
        D4 = "", //孕妇国籍 对照表 2) 1)  国籍代码GB/T 2659
        D5 = "", //孕妇民族 1)  民族代码GB/T 3304
        D6 = "", //孕妇证件类型1)   证件类型CV02.01.101
        D8 = "", //生日
        D9 = "", //孕妇年龄
        D10 = "", //孕妇文化程度 1)  文化程度STD_CULTURALDEG
        D11 = pregnantInfo.mobilenumber, //手机号码
        D12 = "", //孕妇职业 1)  职业STD_OCCUPATION
        D13 = "", //孕妇工作单位
        D14 = "", //孕妇籍贯
        D15 = "44", //孕妇户籍地址 [ 对照表] 省2位,市2位,县/区2位,乡镇街道3位,社区/村3位
        D16 = "4451", //孕妇户籍地址 [ 对照表]
        D17 = "445103", //孕妇户籍地址 [ 对照表]
        D18 = "", //孕妇户籍地址 [ 对照表]
        D19 = "", //孕妇户籍地址 [ 对照表]
        D20 = "广东省潮州市潮安区", //户籍详细地址
        D21 = "44", //孕妇现住地址 [ 对照表]
        D22 = "4451", //孕妇现住地址 [ 对照表]
        D23 = "445103", //孕妇现住地址 [ 对照表]
        D24 = "", //孕妇现住地址 [ 对照表]
        D25 = "", //孕妇现住地址 [ 对照表]
        D26 = "广东省潮州市潮安区", //产后休养地址
        D27 = "44", //产后休养地址 [ 对照表]
        D28 = "4451", //产后休养地址 [ 对照表]
        D29 = "445103", //产后休养地址 [ 对照表]
        D30 = "", //产后休养地址 [ 对照表]
        D31 = "", //产后休养地址 [ 对照表]
        D32 = "广东省潮州市潮安区", //产后详细地址
        D33 = "1", //孕妇户籍类型 1)  户籍类型STD_REGISTERT2PE
        D34 = "2", //孕妇户籍分类 非户籍:2 ,户籍:1
        D35 = "", //来本地居住时间 
        D36 = "1", //近亲结婚  [ 对照表]
        D37 = "", //孕妇结婚年龄 
        D38 = "", //丈夫结婚年龄 
        D39 = "", //丈夫姓名
        D40 = "CN", //丈夫国籍  [ 对照表]
        D41 = "01", //丈夫民族  [ 对照表]
        D42 = "01", //丈夫证件类型  [ 对照表]
        D43 = "", //丈夫证件号码
        D44 = "", //丈夫出生日期
        D45 = "", //丈夫登记年龄
        D46 = "", //丈夫职业  [ 对照表]
        D47 = "",  //丈夫工作单位
        D48 = "", //丈夫联系电话
        D49 = "", //丈夫健康状况   [ 对照表]
        D50 = "", //丈夫嗜好   [ 对照表]
        D51 = "44", //丈夫现在地址   [ 对照表]
        D52 = "4406", //丈夫现在地址
        D53 = "", //丈夫现在地址
        D54 = "", //丈夫现在地址
        D55 = "", //丈夫现在地址
        D56 = "", //现住详细地址
        D57 = "",
        D62 = "", //婚姻状况  [ 对照表]
        D63 = "", //医疗费用支付方式  [ 对照表]
        D64 = "", //厨房排风设施 PASS
        D65 = "", //燃料类型 PASS
        D66 = "", //饮水 PASS
        D67 = "", //厕所  PASS
        D68 = "", //禽畜栏 PASS

    }
                    };
                    var json = data.ToString();
                    postData = "data=" + HttpUtility.UrlEncode(data.ToString());
                    result = FSHttpHelper.Post(url, postData, ref container, contentType: "text/text;charset=UTF-8");
                    Console.WriteLine($"result:{result}");
                    break;
                }
            }));

            int currentIndex = -1;
            PregnantInfo currentPregnant = null;
            cmds.Add(new Command("m16,0617,模拟-找到下一个孕妇", () =>
            {
                var container = new CookieContainer();
                var userId = "35000528";
                var userName = "廖凤贤";
                var orgId = "45608491-9";
                var orgName = "佛山市妇幼保健院";
                var pregnantInfos = new List<PregnantInfo>()
                {
new PregnantInfo("441827198704286020","黎颖","13542492882"),
new PregnantInfo("421222199407140021","吴敏","13135961189"),
new PregnantInfo("445381198905176623","陈家惠","13553405227"),
new PregnantInfo("440681198709095449","卢迎秋","13528992555"),
new PregnantInfo("440923199411246328","罗彩连","15915223395"),
new PregnantInfo("450924198707174785","梁艳","13928250717"),
new PregnantInfo("441522199312221742","黄静","13516589339"),
new PregnantInfo("445281199810212504","庄雪梅","13202835037"),
new PregnantInfo("450881199711136024","蔡敏娟","13058311815"),
new PregnantInfo("440681198704295943","钟楚华","13724665131"),
new PregnantInfo("445322199101263128","谢海梅","15217556321"),
new PregnantInfo("44068119911213362X","卢婉霞","13621409466"),
new PregnantInfo("445323199201212420","伍斯敏","13622446006"),
new PregnantInfo("440681199202182062","何妙映","18316554365"),
new PregnantInfo("440823199702204325","钟真真","18819146446"),
new PregnantInfo("360782199008201727","曾彦汐","18870796067"),
new PregnantInfo("445224199602083085","林晓妹","15013664504"),
new PregnantInfo("441224199205026067","梁英菲","13534996267"),
new PregnantInfo("500235199312209488","龙梅","13410046012"),
new PregnantInfo("440232199203084123","王灿兰","13923131893"),
new PregnantInfo("440681199505224768","潘丽诗","13392252231"),
new PregnantInfo("440825200008143428","蔡小娟","18813615617"),
new PregnantInfo("510311198502013322","张群英","13423929543"),
new PregnantInfo("440681198212134721","曾瑞宜","13531361587"),
new PregnantInfo("511224198201054662","陈红","13417842686"),
new PregnantInfo("520321199712140046","梁启美","17873453391"),
new PregnantInfo("530627199909073525","邓琴","18387077296"),
new PregnantInfo("450881198406150685","蔡妹","13690831303"),
new PregnantInfo("513023198902227129","邓德庭","18680445260"),
new PregnantInfo("441202198009082026","程丽珊","13928205721"),
new PregnantInfo("450881199902125643","杨丽珍","13077475155"),
new PregnantInfo("440781198808206226","朱红花","13724674902"),
new PregnantInfo("441881199910105326","张秋怡","13250675012"),
new PregnantInfo("440681199402090621","郭敏仪","13250113937"),
new PregnantInfo("441423198407302023","郑美意","13717198040"),
new PregnantInfo("445322200109123723","蔡汝婷","13417975071"),
new PregnantInfo("440681199106240226","罗倩薇","13929182866"),
new PregnantInfo("440883199508085103","肖水娣","13500091311"),
new PregnantInfo("360727199302033344","叶友琴","19979453510"),
new PregnantInfo("431022199707104568","梁秋丽","13549508402"),
new PregnantInfo("440902198402212445","邹菲菲","13580489757"),
new PregnantInfo("411503198905100883","杨玉玲","18988544740"),
new PregnantInfo("441581199107202526","李惠君","13929126005"),
new PregnantInfo("14021119830223082x","张艳霞","13753243400"),
new PregnantInfo("44068119891007202X","李珏瑜","13534397477"),
new PregnantInfo("460104198909181249","李海滨","13923125441"),
new PregnantInfo("450981198905032144","罗钰瑛","15577511730"),
new PregnantInfo("441424199201206766","温佑英","17820068533"),
new PregnantInfo("440681199109246068","岑甜兴","13516548926"),
new PregnantInfo("44098119950209612X","黎少玲","18664396392"),
new PregnantInfo("511522200106222529","刘昌琴","18783113042"),
new PregnantInfo("44068119921128364X","卢泳仪","13724690637"),
new PregnantInfo("440681199406094726","何银开","13106735738"),
new PregnantInfo("440681198110090246","罗宝贞","18688286468"),
new PregnantInfo("350821198903271525","黄水梅","15960316695"),
new PregnantInfo("441424199203022629","张文娟","13923287507"),
new PregnantInfo("440804199202101160","李晓伦","13923238195"),
new PregnantInfo("440681198811212381","黄燕珊","13434843234"),
new PregnantInfo("440882199109111523","吴细免","15812360988"),
new PregnantInfo("441225199107083267","梁辛梅","13318327886"),
new PregnantInfo("430923198903166360","王柳杨","18925066784"),
new PregnantInfo("510923198908015385","匡任琴","13928278710"),
new PregnantInfo("440681198406302623","杨小韵","13695224663"),
new PregnantInfo("330702199709093522","吴怡","15167959693"),
new PregnantInfo("350182199101113540","陈燕芳","13797850790"),
new PregnantInfo("429006198201161224","吴鹏霞","18923285061"),
new PregnantInfo("522626199907134025","庞慧","18084599278"),
new PregnantInfo("45088119970608774X","刘华弟","13428816342"),
new PregnantInfo("440681197806285469","黄伟娟","13703026602"),
new PregnantInfo("440681198908182107","梁杏仪","13630111162"),
new PregnantInfo("441423199407140444","彭舒淇","18475063810"),
new PregnantInfo("441225199109291326","孔静","13794002925"),
new PregnantInfo("445224198811103068","郑少娜","15521168488"),
new PregnantInfo("440681199012165923","黄淑敏","15899569966"),
new PregnantInfo("440921198612137147","丘宇萍","15089624236"),
new PregnantInfo("511025197810070669","魏常俊","13690685270"),
new PregnantInfo("440181198706040621","郭丽珊","13902403968"),
new PregnantInfo("452122199008050329","蒙秋芬","15017780992"),
new PregnantInfo("44068119830729316X","曾钰芝","13823400658"),
new PregnantInfo("440681199005180447","袁思瑶","13531350961"),
new PregnantInfo("440681198908110226","李思敏","13702826883"),
new PregnantInfo("431124198910153465","何晓燕","13760255197"),
new PregnantInfo("440681199304092025","黎淑桢","13433228140"),
new PregnantInfo("440681199304300226","何嘉欣","13927201812"),
new PregnantInfo("445322199205282243","程银萍","13631465019"),
new PregnantInfo("422202198312181825","郑玲","13889932510"),
new PregnantInfo("441881199012233182","黄玉娣","15919071525"),
new PregnantInfo("450421199510287848","聂灿妮","18613022881"),
new PregnantInfo("440923199201183787","李春花","13726678113"),
new PregnantInfo("440681198409294788","陈丽萍","13794644304"),
new PregnantInfo("440681198410282688","郭焯华","13434816302"),
new PregnantInfo("522301200408100084","姜青青","19130994620"),
new PregnantInfo("440681198807035925","梁倩华","13630166056"),
new PregnantInfo("430524199101125267","周艳","13758761576"),
new PregnantInfo("440881199408227722","黄秋燕","13923298733"),
new PregnantInfo("450881198512091180","张琼","15363408481"),
new PregnantInfo("44178119780210150X","梁小群","13415572990"),
new PregnantInfo("440923198509010802","伍海连","18718533069"),
new PregnantInfo("441284199504033423","邱依晴","18319352069"),
new PregnantInfo("44122619870724234X","梁秋凤","13630118431"),
new PregnantInfo("362204199002050521","刘丽娟","18814110334"),
new PregnantInfo("44068119900715594X","梁小力","13590631996"),
new PregnantInfo("510322199901014107","王梅","15281354191"),
new PregnantInfo("440681199405113622","卢淑娟","18316555761"),
new PregnantInfo("440681199010050022","罗国欣","13450787329"),
new PregnantInfo("51052119950903380X","郭琴","18208336717"),
new PregnantInfo("441624199312083528","梁桂芳","13923279539"),
new PregnantInfo("44068119871204364X","杜结妙","13630028646"),
new PregnantInfo("450422198504163082","谢华琼","17774737251"),
new PregnantInfo("45042119881205552X","余炼","13798655702"),
new PregnantInfo("440681198502220820","朱嘉韵","13790011381"),
new PregnantInfo("441224199504164320","李霞萍","13794059331"),
new PregnantInfo("44528119911106010X","林育璇","13702925017"),
new PregnantInfo("431122198809121722","俞英","13690520536"),
new PregnantInfo("440681199106264746","伍佩君","15976631887"),
new PregnantInfo("440681199012215425","欧阳建欣","13923260717"),
new PregnantInfo("440725198002252126","吕玉玲","13727392183"),
new PregnantInfo("441226199502083722","陈金萍","13527052523"),
new PregnantInfo("440681198604032644","赵碧玉","15920728762"),
new PregnantInfo("445222199203080060","张佳宁","13336446460"),
new PregnantInfo("430822199405170028","刘文华","13013616407"),
new PregnantInfo("440681199412144726","麦淑敏","18219343728"),
new PregnantInfo("441625199405215426","黄彩红","14778553939"),
new PregnantInfo("441424199709296328","刘苑桥","14715046942"),
new PregnantInfo("440681199302084726","陈晓桐","13549964793"),
new PregnantInfo("44078119830301232x","黄春凤","13760737592"),
new PregnantInfo("44068319881013162X","何映云","13794620167"),
new PregnantInfo("441423199302235025","谭杏花","13794004276"),
new PregnantInfo("370724198707206205","刘爱伟","13928855187"),
new PregnantInfo("440881198205295528","龙珍华","15323333600"),
new PregnantInfo("440681199309113665","张丽碧","13690136331"),
new PregnantInfo("350822198601301027","赖冬娣","13535838764"),
new PregnantInfo("441521199408258228","郑丽雁","13560934796"),
new PregnantInfo("430723198507047246","胡荣","15989354586"),
new PregnantInfo("440681199011023624","萧静仪","15818078568"),
new PregnantInfo("510322199606020707","刘家淑","18381388542"),
new PregnantInfo("440681199111182622","梁带旺","13702601225"),
new PregnantInfo("452702198708074084","韦立莎","18277222050"),
new PregnantInfo("440883199111153526","林春妹","13690199153"),
new PregnantInfo("360722199409155120","刘聪华","18664507210"),
new PregnantInfo("440223199211114728","邓慧婷","13827703775"),
new PregnantInfo("440681199211285426","何家欣","13925941859"),
new PregnantInfo("452730199606014769","蓝柳慧","18877813279"),
new PregnantInfo("440681199402074760","何燊怡","13622723207"),
new PregnantInfo("450422199008231327","万雪梅","15920727397"),
new PregnantInfo("510922199411300680","冯菊","18482144854"),
new PregnantInfo("441225198901082929","林雪连","13726671355"),
new PregnantInfo("441224199111276321","甘玉婷","13679853199"),
new PregnantInfo("44142219900314096X","张淑婷","15011886409"),
new PregnantInfo("44068119900524202X","梁丽华","13434866175"),
new PregnantInfo("450330199901201627","雷兰兰","18145780319"),
new PregnantInfo("440923199110212183","陈丽娟","13538984535"),
new PregnantInfo("513023198408241224","谢孝燕","18924801376"),
new PregnantInfo("452427200208013323","李凤莲","17665663583"),
new PregnantInfo("430424199602102340","李晨","13425898687"),
new PregnantInfo("440681199002214728","谢小华","15014701364"),
new PregnantInfo("441522198803012721","郑惠燕","13480658051"),
new PregnantInfo("440681199008065428","何美君","15899812473"),
new PregnantInfo("440681199105253623","郭小夏","15363033335"),
new PregnantInfo("510322199408255724","郭明英","15281350805"),
new PregnantInfo("230203198410121227","田莹","13766551668"),
new PregnantInfo("44122319951229142X","王怡君","15815991404"),
new PregnantInfo("450881199503155087","卢春菊","19977580481"),
new PregnantInfo("440224198710171809","聂兰秀","18676860578"),
new PregnantInfo("440681199403153620","梁晓琴","13516633436"),
new PregnantInfo("430405199704084041","谭祥鑫","13786452767"),
new PregnantInfo("441424199410012546","郑丽丹","15767513313"),
new PregnantInfo("430124199110094966","喻娟","18825936610"),
new PregnantInfo("44068119790423362X","吕惠蓉","15815612730"),
new PregnantInfo("441225199303032563","侯燕梅","15818032116"),
new PregnantInfo("45048119920602322X","倪小雲","18777442851"),
new PregnantInfo("440229199612103729","黄石尽","13590606910"),
new PregnantInfo("412828199311244221","徐艳","17329875217"),
new PregnantInfo("440823199812072108","黄彩薇","18824090925"),
new PregnantInfo("432930198007253023","李群英","18688259528"),
new PregnantInfo("421122198812203567","李美娟","13476677233"),
new PregnantInfo("441224199701153743","林家慧","13202882273"),
new PregnantInfo("440681199608218029","罗晓琳","13690430749"),
new PregnantInfo("511527200211242144","蒋美玲","17883655313"),
new PregnantInfo("360730199012251501","官丹丹","18689257591"),
new PregnantInfo("421127198709055668","严颖飞","13690825073"),
new PregnantInfo("440681197808043623","康趣桥","13695222501"),
new PregnantInfo("441823199005247427","陈秀娟","13790017649"),
new PregnantInfo("440681199302155424","欧阳凯婷","15217407236"),
new PregnantInfo("440222198812101921","曾群花","13727508879"),
new PregnantInfo("441802199006173245","陈舒敏","13360362697"),
new PregnantInfo("445322198609055528","莫少莲","18666791185"),
new PregnantInfo("441224199307040820","李宁","15986137124"),
new PregnantInfo("440803198505022426","岑玉斯","13590068912"),
new PregnantInfo("440229199209240723","黄玉梅","15875101951"),
new PregnantInfo("440223198807250021","陈水莲","15015693310"),
new PregnantInfo("440681198310310223","吴艳珍","13715402041"),
new PregnantInfo("440681199201233622","翁慧洪","13760943893"),
new PregnantInfo("450721199802180563","何雪晴","15812890951"),
new PregnantInfo("452523198006136026","王凤琼","15015730927"),
new PregnantInfo("513701199204256621","朱丹","15815945058"),
new PregnantInfo("440902199202213704","梁燕红","15017763423"),
new PregnantInfo("44068119800518542X","何金凤","13924802834"),
new PregnantInfo("452523197903221429","韦凤坤","13622517225"),
new PregnantInfo("452402199112020622","柳秋再","15077476807"),
new PregnantInfo("452702200110082864","覃宇璐","18520729342"),
new PregnantInfo("440681198909074722","伍嘉琪","13724914628"),
new PregnantInfo("450821200111243861","吴小悦","18823288014"),
new PregnantInfo("440681199403283628","潘绮雯","13827700354"),
new PregnantInfo("440681198510192663","霍均萍","13432676933"),
new PregnantInfo("430522199805140040","刘章萍","17665611659"),
new PregnantInfo("440902199204083261","黄建玲","15627558900"),
new PregnantInfo("440881199606224127","龙彩芹","18719107513"),
new PregnantInfo("522528199505044841","刘天秀","13985719901"),
new PregnantInfo("452730197209304763","袁秀棉","17776417272"),
new PregnantInfo("452502198501103449","覃素萍","18777592522"),
new PregnantInfo("44092119930319832X","陈世梅","15360070536"),
new PregnantInfo("452729199812231021","卢玉","15919344697"),
new PregnantInfo("440623197507040222","李健仪","13316309388"),
new PregnantInfo("441821198603281820","宋春苗","13500262834"),
new PregnantInfo("45072220010409562x","李景雁","18897795871"),
new PregnantInfo("441224198806152945","罗翠华","13660989380"),
new PregnantInfo("445221199705256883","陈银佳","18607695278"),
new PregnantInfo("441702198911183822","吴柳仪","13229205115"),
new PregnantInfo("450881199204256240","杨海月","15015641928"),
new PregnantInfo("450330198612230726","王元月","15011668720"),
new PregnantInfo("511527199404232924","姜平","18826657029"),
new PregnantInfo("441423199212031045","郑嘉蓉","13590121993"),
new PregnantInfo("440603198311103024","蔡创红","13560095183"),
new PregnantInfo("440224199206100483","谢真华","18664864909"),
new PregnantInfo("450924198402264720","龙霞","13416310300"),
new PregnantInfo("360727199102250029","唐琦","13825550675"),
new PregnantInfo("450881199102194122","施永珍","13702906791"),
new PregnantInfo("430426198903168288","匡云娇","13427836952"),
new PregnantInfo("440681199504274229","梁笑芬","13726364267"),
new PregnantInfo("441223198507183526","陈梅容","13724927016"),
new PregnantInfo("430821198101064823","杨海英","13727417616"),
new PregnantInfo("441224199408044847","林雪珊","13129754587"),
new PregnantInfo("44088119940317318X","谭妙丽","13030194150"),
new PregnantInfo("44162519930603446X","李海溶","13760904343"),
new PregnantInfo("440606199512290026","凌倩彤","13690020061"),
new PregnantInfo("45213019790902302X","赵冰梅","18144951448"),
new PregnantInfo("440921199401184263","孔繁玲","13424623719"),
new PregnantInfo("440681198811168026","何丽娟","13435495162"),
new PregnantInfo("440681199011285421","欧阳丽珊","13794086873"),
new PregnantInfo("430722198607105902","吴丹","13825582201"),
new PregnantInfo("440681197906193676","刘庆佳","15118729776"),
new PregnantInfo("612323198209260827","罗红英","13600358610"),
new PregnantInfo("440681199709244760","黄嘉颖","18529230924"),
new PregnantInfo("440681199102272044","周翠环","13630128670"),
new PregnantInfo("511524199203130949","李娇","13928560227"),
new PregnantInfo("441224199003192323","钱雁芳","13679765030"),
new PregnantInfo("450924199104137162","梁丹","15919944873"),
new PregnantInfo("440883199311275085","肖亮飞","15818818434"),
new PregnantInfo("450803199609177541","苏允雁","13531319624"),
new PregnantInfo("532130199610300941","杨成燕","19984078313"),
new PregnantInfo("452424199305071109","吴红梅","13415729632"),
new PregnantInfo("440681199407082348","黄婉荧","15015720473"),
new PregnantInfo("445102199201250344","苏裕烘","15015774275"),
new PregnantInfo("450881198610155045","卢晓霞","13192458668"),
new PregnantInfo("44068119960417062X","龙梓晴","13690719234"),
new PregnantInfo("430421199303160381","刘琴","13710876269"),
new PregnantInfo("360731199211028720","吴石花","15217914059"),
new PregnantInfo("431322199307300021","何晶","18152784792"),
new PregnantInfo("440107198310020049","伍建敏","15398988323"),
new PregnantInfo("440681199003132089","梁倩徽","13686504244"),
new PregnantInfo("445122198902256627","刘丽玲","13719160264"),
new PregnantInfo("612322199510290222","熊梦琪","13629161029"),
new PregnantInfo("440681198303153629","伍润芬","13690770654"),
new PregnantInfo("440681199107192326","陈婉淳","13144124008"),
new PregnantInfo("440681199204234225","何婉怡","13724669182"),
new PregnantInfo("452226199005185448","黄新莲","15914843897"),
new PregnantInfo("440183199602164424","刘嘉敏","13425675244"),
new PregnantInfo("440681199012274767","梁健娴","13690830596"),
new PregnantInfo("360721199010011620","陈玉梅","13923150272"),
new PregnantInfo("620422198912123540","权金涛","18823136642"),
new PregnantInfo("440582199002030049","纪晓苹","13719430049"),
new PregnantInfo("43050319820416254X","胡丽英","13674075376"),
new PregnantInfo("429006199409012820","刘文硕","15875591393"),
new PregnantInfo("441224198504105780","植玉莹","13724682230"),
new PregnantInfo("431125200004053127","杨少玉","18974634808"),
new PregnantInfo("440681198602103680","梁艳意","15916192393"),
new PregnantInfo("441882198906171525","祝丽娟","13670629620"),
new PregnantInfo("420922198707104922","周红铃","18665470907"),
new PregnantInfo("440681199008215967","岑佩仪","13790063039"),
new PregnantInfo("440681199008205443","胡佩碧","13925420486"),
new PregnantInfo("420921198912102647","汤姣","14750301748"),
new PregnantInfo("450481199107130660","欧业丽","13662395402"),
new PregnantInfo("441523199502257582","范小颜","13923274417"),
new PregnantInfo("440681198706033621","唐雪莲","13823492973"),
new PregnantInfo("440681198812112649","苏婉玲","13590644339"),
new PregnantInfo("440881199002143123","陈海桃","15914583136"),
new PregnantInfo("452427198705182128","虞晓莹","13424640932"),
new PregnantInfo("440221198509211929","龚燕珍","13825505688"),
new PregnantInfo("360735199509091648","黄蓉","13387078871"),
new PregnantInfo("440921198912117164","梁翠丽","13751516917"),
new PregnantInfo("440881199408285949","邱舒婷","18578394707"),
new PregnantInfo("445322199701283122","覃雪梅","15818044341"),
new PregnantInfo("450802198502142022","覃少花","13528053407"),
new PregnantInfo("44068119941227474X","吴欣桐","13078432011"),
new PregnantInfo("452122199302112746","陆秀琴","13226969480"),
new PregnantInfo("510322199708255726","杨才琴","15328383963"),
new PregnantInfo("420982199602147222","程瑶","15571220637"),
new PregnantInfo("441225199505012587","莫锦兰","18823496684"),
new PregnantInfo("52212419990921722X","吴捡梅","17688286772"),
new PregnantInfo("500226199808231527","黄阳依","13411805305"),
new PregnantInfo("440681199904082621","冯杏桃","13118835584"),
new PregnantInfo("440681199512190420","苏海茵","13798604348"),
new PregnantInfo("450802198405073205","黄兰芳","13113786912"),
new PregnantInfo("44011119851022098X","孔淑苗","13570347643"),
new PregnantInfo("440681198910058025","梁翠玲","13431661874"),
new PregnantInfo("452128198512290528","梁永利","13129192256"),
new PregnantInfo("450521199112256128","周琼","15914555489"),
new PregnantInfo("532801199906051143","玉书","18022734147"),
new PregnantInfo("411403200201118721","郭梦晴","17797784395"),
new PregnantInfo("440683198810112322","卢仲茹","13413286162"),
new PregnantInfo("460103199209041526","黎秋蕾","18666545697"),
new PregnantInfo("412821198107151045","申菊","13798593326"),
new PregnantInfo("430522198809024982","邓梅花","18988927323"),
new PregnantInfo("45088119860610064X","黄玉娇","13927271145"),
new PregnantInfo("440681199209158022","梁秋仪","18934318293"),
new PregnantInfo("430624198911087363","王田","18682410120"),
new PregnantInfo("431022199009154920","尹圳兰","13684945072"),
new PregnantInfo("440681198211025523","欧阳敏谊","13590634211"),
new PregnantInfo("450881198703145321","李嘉","13763385293"),
new PregnantInfo("412727199005104165","朱雪茹","18578380545"),
new PregnantInfo("511323199210082124","余贞仪","18898698263"),
new PregnantInfo("440681198912112064","李锦君","15989951886"),
new PregnantInfo("43058119850622176X","唐露","18988613788"),
new PregnantInfo("450923199508076005","王冬琴","13650343894"),
new PregnantInfo("440883198707202646","凌康艳","13827726784"),
new PregnantInfo("15272219900922242X","温凤","18122733521"),
new PregnantInfo("410104199009080042","姚媛","15625155309"),
new PregnantInfo("44170219950918432X","黄小燕","13690396348"),
new PregnantInfo("445221199701301923","卢荣璇","15718370289"),
new PregnantInfo("440681199004065949","梁景霞","13727375206"),
new PregnantInfo("440981198703082221","张宁","13798655977"),
new PregnantInfo("440682199610233240","罗晓雯","13249238645"),
new PregnantInfo("421023198811116120","刘婷","18823236247"),
new PregnantInfo("450802198312272344","杨云","15816100040"),
new PregnantInfo("440681199508094727","伍敏妍","15976632339"),
new PregnantInfo("430681198808214624","伏炼","13825551071"),
new PregnantInfo("450881198607107480","黄美凤","13713151038"),
new PregnantInfo("440681199111208028","梁嘉恩","13129056990"),
new PregnantInfo("440681199304034722","黄璇欣","13751523124"),
new PregnantInfo("450802199611101523","覃丽香","18576087503"),
new PregnantInfo("440681199212204720","胡怡慧","13630139687"),
new PregnantInfo("440681199102105481","李佩雯","13794064355"),
new PregnantInfo("440681199802195449","刘泳金","13670656885"),
new PregnantInfo("450924198409104164","周健英","13250108880"),
new PregnantInfo("440681199307110241","黄柏怡","18566381448"),
new PregnantInfo("440624197708093888","谢雪锋","13535825118"),
new PregnantInfo("440681198002265969","沓丽环","18924874930"),
new PregnantInfo("440802199306141228","钟洁","13827780614"),
new PregnantInfo("440681199211232623","区子茵","18924859872"),
new PregnantInfo("430224198609121821","刘勤","18923268018"),
new PregnantInfo("440681198807044848","苏淑微","15913565524"),
new PregnantInfo("440681198309101248","冯彩文","13630061701"),
new PregnantInfo("440781197401067565","谭苏美","13924865592"),
new PregnantInfo("440811199101270348","徐静","13060754468"),
new PregnantInfo("411421198602210062","张婷","15016100521"),
new PregnantInfo("440681199512134824","司徒宝怡","13249225075"),
new PregnantInfo("430702199006211549","雷梦妮","18125795868"),
new PregnantInfo("440681198910084741","苏艳君","13450568608"),
new PregnantInfo("440681199108252642","熊馨","15627215102"),
new PregnantInfo("440181198901262721","梁淑仪","13527644132"),
new PregnantInfo("450881198407107443","李艳红","13542565733"),
new PregnantInfo("440681198805215965","佘翠欣","13823428330"),
new PregnantInfo("441823199109253725","练雪玲","13929113497"),
new PregnantInfo("440681198710204788","谭惠捷","13798653056"),
new PregnantInfo("45242820040829084X","杨小莲","18378410085"),
new PregnantInfo("441823199409198326","林妙霞","13415278025"),
new PregnantInfo("440804198509040863","陈思宇","13674053322"),
new PregnantInfo("452124198811293328","罗忠慧","13471110114"),
new PregnantInfo("44512219920715122X","张瑞燕","13510065208"),
new PregnantInfo("410426199012292526","郭亚锦","18237449232"),
new PregnantInfo("445122199606025220","陈玉铃","15768618397"),
new PregnantInfo("440681198511250220","廖淑茵","13392296880"),
new PregnantInfo("441823199405068348","陈娟萍","13534331335"),
new PregnantInfo("430581200208094264","汤晶","18166189854"),
new PregnantInfo("450421199403102529","郭妹妹","18775426692"),
new PregnantInfo("440681199504133629","黎翠萍","18316551399"),
new PregnantInfo("441223198812295022","祝慧冰","15915928160"),
new PregnantInfo("441882199610086027","成淑芬","18319898302"),
new PregnantInfo("441827199305314324","苏玉婷","13903066061"),
new PregnantInfo("430523199109213548","唐婷","13509221065"),
new PregnantInfo("440783199608043925","吴日妹","13760468289"),
new PregnantInfo("450422198902071386","何钰萍","13450827419"),
new PregnantInfo("440882198706144740","陈小妹","15918805344"),
new PregnantInfo("44532319881010122X","陈小燕","13232148492"),
new PregnantInfo("360430199507070327","严洁","13380070016"),
new PregnantInfo("445221198508111947","黄媚","158202991321"),
new PregnantInfo("522728198110082428","潘付萍","13336438071"),
new PregnantInfo("450881199307021188","陈燕华","18177577274"),
new PregnantInfo("452427199108021961","钟丽珍","13288209421"),
new PregnantInfo("440681198906063622","何彩燕","13927298663"),
new PregnantInfo("452626198103143409","凌秀青","13059368808"),
new PregnantInfo("45080219941008152X","胡杰美","18927753713"),
new PregnantInfo("45252319810715416X","梁海梅","13415556202"),
new PregnantInfo("412826198103151429","王伟娜","13420650662"),
new PregnantInfo("440681197808253620","张凤莲","18924808836"),
new PregnantInfo("440681198701054765","苏丽敏","13727371007"),
new PregnantInfo("441283198411120041","陈金玲","13432665524"),
new PregnantInfo("440783199007237222","陈仕媛","18306613347"),
new PregnantInfo("450881199608082689","莫彩凤","13421460680"),
new PregnantInfo("441424198704200345","曾坤燕","18520975847"),
new PregnantInfo("44158119930129396X","陈建如","15899519912"),
new PregnantInfo("440903199412031825","黄嘉欣","15919056232"),
new PregnantInfo("440681199707196021","陈晓玲","15015563082"),
new PregnantInfo("450881198511085328","韦清新","13380279530"),
new PregnantInfo("452133199008160922","张海明","13612574176"),
new PregnantInfo("450821198812121584","蒙晓英","13129396735"),
new PregnantInfo("440681199209282621","黄淑华","15976665463"),
new PregnantInfo("44068119950807542X","何佩仪","13590651081"),
new PregnantInfo("440681198409230223","吴淑冰","15917017532"),
new PregnantInfo("421081198910105607","杨芳","18923265594"),
new PregnantInfo("445322199612191023","潘嘉欣","15011912445"),
new PregnantInfo("431129198904142227","罗凤芹","15774126575"),
new PregnantInfo("45088119970509602X","覃婷婷","18378510204"),
new PregnantInfo("440221198409202726","钟秀霞","15812964691"),
new PregnantInfo("440681198605130625","邓颖安","15818092202"),
new PregnantInfo("430225198706036529","罗芳","13611437047"),
new PregnantInfo("450923199705285404","冯琳","18902564025"),
new PregnantInfo("441424199404120540","李淑花","15999656856"),
new PregnantInfo("440681199807072325","周燕铃","13106669762"),
new PregnantInfo("420881199308020042","董艳华","18672613319"),
new PregnantInfo("440681199204065425","何雁君","15916120745"),
new PregnantInfo("430624199103118525","傅配","17373042320"),
new PregnantInfo("430482198906185084","王红艳","18890381151"),
new PregnantInfo("441225198902263529","张秀云","13425725860"),
new PregnantInfo("412821198806164929","马玉玲","13450887835"),
new PregnantInfo("441881199308237422","姚爱娣","15113373304"),
new PregnantInfo("440681198410154221","曾秀映","13425690700"),
new PregnantInfo("440902198107113647","郑桂林","13715499388"),
new PregnantInfo("440982199106053183","林春静","13668900905"),
new PregnantInfo("445323198808012121","罗伙珍","18218615431"),
new PregnantInfo("440882199405181163","陈玉兰","18169840853"),
new PregnantInfo("44068119930705362X","潘佩璇","13144733341"),
new PregnantInfo("440681199107304244","黄玉鸣","13621410244"),
new PregnantInfo("510923199309187120","童艳","15817835049"),
new PregnantInfo("522121198710105227","帅远南","18998349633"),
new PregnantInfo("51102819761115632X","李恭芬","13612614749"),
new PregnantInfo("440681199307292620","梁楚炫","13078443826"),
new PregnantInfo("45252319971018232X","朱梅英","13113733043"),
new PregnantInfo("440681198111140620","黎家丽","15017642850"),
new PregnantInfo("432522199201160966","邓媛","13928553877"),
new PregnantInfo("44068119930901478X","黎慧瑶","13425953128"),
new PregnantInfo("450422199907011328","万金婷","13531337897"),
new PregnantInfo("430623198511031225","贺义","18316493959"),
new PregnantInfo("440681199606244725","邓淑欣","17324111890"),
new PregnantInfo("452501197910013427","杨庆红","13702620731"),
new PregnantInfo("440681199311264227","廖嘉仪","13549953973"),
new PregnantInfo("440681199207312065","赵彩敏","13590528861"),
new PregnantInfo("430511199011102529","周瑾","13702436618"),
new PregnantInfo("430482198703070025","夏敏","13923175928"),
new PregnantInfo("440681198002292027","何巧堂","13392758529"),
new PregnantInfo("452502198101055548","黄映凤","13425753698"),
new PregnantInfo("440681199008100422","赵涤雯","15099881091"),
new PregnantInfo("52222519970101322X","李远琴","15329768653"),
new PregnantInfo("430626199603126225","邱新华","18873097790"),
new PregnantInfo("441423199008214725","彭茶花","13727467286"),
new PregnantInfo("440681199212015428","欧阳艳娟","13433119597"),
new PregnantInfo("440232198105144124","张美秀","13380268852"),
new PregnantInfo("440681198705284744","吴彩简","13790092350"),
new PregnantInfo("460003199308166644","黄万成","18217990729"),
new PregnantInfo("412724198912017504","马灵灵","15916566618"),
new PregnantInfo("441522199109137983","郑玉情","15800242796"),
new PregnantInfo("440982199108156725","黄思奕","15815989499"),
new PregnantInfo("445222199802252223","李怡雯","13434952542"),
new PregnantInfo("440881199909234146","宣石链","13242049423"),
new PregnantInfo("440681198612135942","李秀艺","13790031657"),
new PregnantInfo("440681199106144744","麦赛珊","15015794545"),
new PregnantInfo("360729199003293229","陈罗田","18721111299"),
new PregnantInfo("440882198610011580","黄月球","13922781046"),
new PregnantInfo("441722197905234129","刘光丽","13420868327"),
new PregnantInfo("440229199212031025","徐燕娴","15118608202"),
new PregnantInfo("430525198104196126","谢玉平","13417414762"),
new PregnantInfo("441223199401193827","谢丽宜","15119858740"),
new PregnantInfo("440681198508141728","何玉佩","13690726382"),
new PregnantInfo("441522199110223360","郑坚女","13229237050"),
new PregnantInfo("440605199507150022","何丽婷","13924576763"),
new PregnantInfo("440681198608114866","苏琼琚","13726385035"),
new PregnantInfo("44068119931215234X","谭惠婷","17688218768"),
new PregnantInfo("440681198706135927","何智欣","13824517199"),
new PregnantInfo("45088119870718626X","李凤容","13620324112"),
new PregnantInfo("420923198011151807","陆秀丹","13827760760"),
new PregnantInfo("440681199210135928","邓惠文","13535723812"),
new PregnantInfo("440681199511214785","陈凯欣","13336492015"),
new PregnantInfo("441423200012030465","冯润萍","15627763702"),
new PregnantInfo("440681199512052642","梁嘉怡","13620136741"),
new PregnantInfo("511602198710058104","张清","13326782654"),
new PregnantInfo("440681198705260021","董嘉露","13790068807"),
new PregnantInfo("440681198610252686","梁柳琴","15915203020"),
new PregnantInfo("440882199412081240","林柳荣","13679862282"),
new PregnantInfo("450421199003203048","刘海霞","18038741598"),
new PregnantInfo("450303198909191028","曹果露","18675188054"),
new PregnantInfo("440623197910224785","黄骚妹","13715516379"),
new PregnantInfo("460028198809063240","李芳云","18789945869"),
new PregnantInfo("440606199901250028","邝晓珑","13927243773"),
new PregnantInfo("43102619890708032X","何芳华","15697354505"),
new PregnantInfo("440182199409271221","徐文好","15920423539"),
new PregnantInfo("431121199206276926","唐晨","18797749936"),
new PregnantInfo("450924198804264723","徐春梅","13690315948"),
new PregnantInfo("421126198702056987","何水银","13530740010"),
new PregnantInfo("441224199505203723","黎群青","18316810136"),
new PregnantInfo("445122199106171221","张素凌","15899951082"),
new PregnantInfo("440181199010151248","何欣微","13538880346"),
new PregnantInfo("420625197907234420","王文勤","13715517527"),
new PregnantInfo("440681199103062401","黄蕴斯","13630046891"),
new PregnantInfo("441523198210247580","彭新丽","18988673195"),
new PregnantInfo("440681198912095444","欧阳锦意","15112995780"),
new PregnantInfo("452624199303082622","韦凤","19994503677"),
new PregnantInfo("440233198810146041","温彩凤","15919674053"),
new PregnantInfo("440681199403166827","曾婉莹","18825973982"),
new PregnantInfo("362202198812301524","肖宁博","18928907294"),
new PregnantInfo("441502199101132349","周映萍","13610075332"),
new PregnantInfo("441224199206164023","莫玉燕","15918185455"),
new PregnantInfo("440681198211284728","吴碧甜","13715481128"),
new PregnantInfo("440681198706240225","梁细欢","15989996338"),
new PregnantInfo("440681198808084243","左淑仪","13630181901"),
new PregnantInfo("440681199012155960","杨泳琴","13413259945"),
new PregnantInfo("440804199603011705","庞珂","18973057039"),
new PregnantInfo("511325198905064123","李丹凤","13288391084"),
new PregnantInfo("431124199210114742","蒋金花","17841047665"),
new PregnantInfo("430124199002262547","闵永芳","13690730141"),
new PregnantInfo("440681198807195427","梁斯敏","13928220977"),
new PregnantInfo("445222199301193843","吴萍珊","13715285884"),
new PregnantInfo("441421199807232441","黄湘梅","15622350681"),
new PregnantInfo("440681198705122702","吴杏琼","13715493006"),
new PregnantInfo("44068119920910362X","吴雪焕","13425902471"),
new PregnantInfo("432522198705040728","朱婷","13760884587"),
new PregnantInfo("445381199407227525","吴冰冰","13538661029"),
new PregnantInfo("440681199004212646","郭清转","13727477964"),
new PregnantInfo("445381200010166344","杜思敏","13630041486"),
new PregnantInfo("445381199710251421","李清清","15918707276"),
new PregnantInfo("440681199211122643","叶子彤","15015663370"),
new PregnantInfo("440923198404194844","王琴","13425935227"),
new PregnantInfo("43038119950905004X","杨丹","18820895031"),
new PregnantInfo("440681198105032666","杨似诗","13690524160"),
new PregnantInfo("441302200102148021","林景红","13250152223"),
new PregnantInfo("440681198910276823","李惠冰","18025877578"),
new PregnantInfo("522729199705062427","何彩虹","18932040446"),
new PregnantInfo("532523199703231429","熊保芬","18214326805"),
new PregnantInfo("440921198701174521","黄瑞梅","13630063929"),
new PregnantInfo("45098119960610320X","周小萍","18942473307"),
new PregnantInfo("440681198812251024","游凯婷","18566006607"),
new PregnantInfo("44522219900120204X","丘翠云","18200961033"),
new PregnantInfo("431128199302050825","陈建英","18620887282"),
new PregnantInfo("441502198001062625","何玉妙","13729582618"),
new PregnantInfo("450821198710053242","冯靖婷","13923227078"),
new PregnantInfo("441223199311232626","张洁玲","15015775883"),
new PregnantInfo("440681199006032040","梁楚君","13823413231"),
new PregnantInfo("452130199505043621","陈小静","15296509495"),
new PregnantInfo("360725198510031426","肖燕","13535638392"),
new PregnantInfo("440681199602185465","欧阳雪怡","13630058399"),
new PregnantInfo("441882198902091528","成海丽","13530553596"),
new PregnantInfo("440681199003314747","李锦欣","13425973313"),
new PregnantInfo("440681199805043643","伍泳欣","17324552406"),
new PregnantInfo("450881198806096227","杨超兰","13632128869"),
new PregnantInfo("450422199109220563","李茵倩","17328010004"),
new PregnantInfo("450323198506272726","雷素军","13528915089"),
new PregnantInfo("440221199208083526","龚玲娣","17817809998"),
new PregnantInfo("450821199502052901","谢春连","13112701741"),
new PregnantInfo("450421199204127029","容洁怡","18319053327"),
new PregnantInfo("441402199407011026","赖敏君","15017071304"),
new PregnantInfo("452226197704251548","覃燕梅","13481256716"),
new PregnantInfo("41152119870517002X","王芳菲","13928822572"),
new PregnantInfo("432502198503180022","禹芃","13923275593"),
new PregnantInfo("441224199306216089","徐凤娌","15820242900"),
new PregnantInfo("440681198009043664","陈淑珍","13715500071"),
new PregnantInfo("440783199302233320","周柳意","13690572247"),
new PregnantInfo("440681199310202622","黄炜莹","13728505162"),
new PregnantInfo("450422198506223325","黄丽萍","13425605080"),
new PregnantInfo("362204199209105323","谢甜兰","18267009013"),
new PregnantInfo("44060619900517002X","麦惠珍","13516524114"),
new PregnantInfo("440823198710101321","杨华珍","15818019042"),
new PregnantInfo("450881198609105729","甘丽勇","13928265811"),
new PregnantInfo("45088119920110564X","蓝绮梦","15915200626"),
new PregnantInfo("513723199305264805","郑晶","13288031880"),
new PregnantInfo("450802199702153628","张小柳","15918188602"),
new PregnantInfo("44068119920913598X","黄晓彤","13129152163"),
new PregnantInfo("440681198507078069","李明欣","15899805833"),
new PregnantInfo("440681199509203681","周影彤","13923270700"),
new PregnantInfo("510322199208022766","张倩","13889948006"),
new PregnantInfo("44088319900309504X","林伟玲","15920729109"),
new PregnantInfo("440681198212082028","黄艳兰","13679739118"),
new PregnantInfo("445222199001080089","贝佳慧","18924321830"),
new PregnantInfo("440681198807272349","卢敏锋","13553346384"),
new PregnantInfo("360302197811203542","贺静","13360344003"),
new PregnantInfo("440681199010134840","何健儿之女","13825505250"),
new PregnantInfo("440923198910105744","王伟平","13380243086"),
new PregnantInfo("510902199509097021","蔡佳","18211490291"),
new PregnantInfo("441284198905210027","杨洁玲","13794640521"),
new PregnantInfo("452223198906015047","潘小丽","18825772127"),
new PregnantInfo("510923199110057521","杨勤","13702488104"),
new PregnantInfo("420106197802154049","吴慧敏","13902828277"),
new PregnantInfo("440681199405300022","何碧然","18029357936"),
new PregnantInfo("450881199107240861","李丽敏","15917010678"),
new PregnantInfo("44022419880325286X","谷珍霞","18688284190"),
new PregnantInfo("440823199011095000","梁青梅","15011857853"),
new PregnantInfo("440681199210150423","罗欣潼","13923123932"),
new PregnantInfo("440681199101252623","梁海欣","18138490215"),
new PregnantInfo("440681198901273620","梁秋欣","13450853161"),
new PregnantInfo("440223199105122222","郭秀云","13534184721"),
new PregnantInfo("510521200008152929","康中萍","18384321292"),
new PregnantInfo("430822199205177129","张晖","13413209285"),
new PregnantInfo("440823199107141147","吴鹏林","18988681137"),
new PregnantInfo("440681199407070427","欧阳湉","18923201333"),
new PregnantInfo("450881199805125043","卢东霞","18078045325"),
new PregnantInfo("1000278256","何文欣","13622723207"),
new PregnantInfo("513701199111084621","柏秀容","13703021499"),
new PregnantInfo("440681198801262043","冯丽芬","18923103395"),
new PregnantInfo("440681199406085440","欧阳敏欣","15820698072"),
new PregnantInfo("440681198709055922","郭淑贞","13549973929"),
new PregnantInfo("612323198401102925","杨宝琴","18088832281"),
new PregnantInfo("445222198311132740","陈燕妮","13435256725"),
new PregnantInfo("430703198711068321","王文芳","15876101686"),
new PregnantInfo("441224199507091729","张小荣","13450172734"),
new PregnantInfo("440681199003244240","康婉琪","13824533311"),
new PregnantInfo("440681198502230228","郭焕贞","15989967371"),
new PregnantInfo("370685199210011324","马蜀玉","18823108589"),
new PregnantInfo("452423198006091723","周敏玲","13531398353"),
new PregnantInfo("441223199110122041","黄宝怡","13570470911"),
new PregnantInfo("44068119950227476X","何慧雯","13790038037"),
new PregnantInfo("432503198608174047","邱志华","13929124608"),
new PregnantInfo("440681199112135940","陈晓莹","15899967465"),
new PregnantInfo("440681198710194743","胡丽碧","13823425746"),
new PregnantInfo("440681199302123625","吴秀琴","13129065960"),
new PregnantInfo("440681198905315963","邓亿雯","13450852797"),
new PregnantInfo("441223198306145320","龙燕萍","13695220946"),
new PregnantInfo("450330198506190767","谭翠兰","15889574926"),
new PregnantInfo("450881199308035709","陆丽华","18125220909"),
new PregnantInfo("44068119851024374X","胡瑞珊","15918155406"),
new PregnantInfo("440681198708202644","梁秋宜","13790076075"),
new PregnantInfo("142325198506130527","韩芳芳","15934260963"),
new PregnantInfo("441323198503062028","陈佛英","15099903971"),
new PregnantInfo("445381198411280420","李超梅","13450583102"),
new PregnantInfo("440681199703114746","谭淑莹","13531316518"),
new PregnantInfo("450422198501203325","张美霞","18520999962"),
new PregnantInfo("511622198906297743","李小庆","13425054079"),
new PregnantInfo("441229198007225521","高金娣","13549985962"),
new PregnantInfo("410521199010048048","高歌","13018555219"),
new PregnantInfo("440981199502148823","宁晓连","15119675483"),
new PregnantInfo("450106199211081524","覃婷婷","13878807748"),
new PregnantInfo("452131199705021220","黄丽婷","19994672410"),
new PregnantInfo("41282819931106276X","凡曼曼","15893946542"),
new PregnantInfo("441228197902061323","叶秀欢","13824515820"),
new PregnantInfo("411324198302154584","杨培","15916589480"),
new PregnantInfo("43040519890215304X","刘彦廷","13702617521"),
new PregnantInfo("440681198407304866","何凤彩","13715406361"),
new PregnantInfo("440681199308274264","陈小霞","15015793024"),
new PregnantInfo("44068119980704472x","黄韵婷","15918140161"),
new PregnantInfo("511128197810010628","张秀花","15821983331"),
new PregnantInfo("412822198911070840","田鑫","18102752025"),
new PregnantInfo("450421199207297824","徐芬","18038812606"),
new PregnantInfo("431022198712107021","陈先宇","13316300350"),
new PregnantInfo("420624198302042221","唐品荣","15971019265"),
new PregnantInfo("441225199407292925","梁秋坚","13679837321"),
new PregnantInfo("430481199707220025","谢芝云","18473431657"),
new PregnantInfo("45042219881020230X","李水娇","18218113600"),
new PregnantInfo("360424198709105506","温成秀","18316976260"),
new PregnantInfo("440783198708251823","梁明芬","15813799818"),
new PregnantInfo("441825199712250222","黄木桃","15816205700"),
new PregnantInfo("441324198606240620","廖珠玲","13536655086"),
new PregnantInfo("421181199512201943","范春丽","13581230793"),
new PregnantInfo("441481198203181980","余宴英","13690834152"),
new PregnantInfo("362322199107265420","徐翠娟","18696759949"),
new PregnantInfo("362424198411275926","刘芳芳","13510878129"),
new PregnantInfo("452524197812070422","梁凤平","15118664556"),
new PregnantInfo("440982199512131861","黄伟婵","15986624763"),
new PregnantInfo("45042319851218084X","覃张献","13825569219"),
new PregnantInfo("441226198609164026","李结玲","13531399149"),
new PregnantInfo("510923199702087927","王艳萍","13458163850"),
new PregnantInfo("421127198510123221","郑笑","13713221657"),
new PregnantInfo("452723198906012484","黄永红","18718234015"),
new PregnantInfo("441821198905180929","李艳玉","13420636383"),
new PregnantInfo("420322198803234222","陈支姣","13380288560"),
new PregnantInfo("422802198706023961","周艳","13711017912"),
new PregnantInfo("440681198612302640","梁带梅","NULL"),
new PregnantInfo("341282198303151422","路莉莉","13929913675"),
new PregnantInfo("342901199212226623","章俊慧","15256675536"),
new PregnantInfo("450922199006130984","梁凤清","13129079656"),
new PregnantInfo("45088119951029232X","梁怡","18566357865"),
new PregnantInfo("431125198705263143","李阿梅","15386319898"),
new PregnantInfo("431126198605070041","乐珍秀","17620012295"),
new PregnantInfo("441781199101070025","李冰心","13211177067"),
new PregnantInfo("430626199111288025","周享林","13790010791"),
new PregnantInfo("440823198712102125","庞秀娇","13420751555"),
new PregnantInfo("440681198603204723","伍趣甜","18038789944"),
new PregnantInfo("450881198503303268","杨馥源","13702569890"),
new PregnantInfo("440681198707034749","陈羡玲","13450859993"),
new PregnantInfo("440184199606203028","黄金劲","15015858574"),
new PregnantInfo("440681199011092662","关丽诗","15989987965"),
new PregnantInfo("440681198806114760","程敏萍","13435417826"),
new PregnantInfo("441822197907043460","刘秀美","13726647838"),
new PregnantInfo("511302199010085329","吕秋蓉","17790182785"),
new PregnantInfo("220104199007087325","夏青","18825429867"),
new PregnantInfo("440681198908263660","梁燕君","15017426282"),
new PregnantInfo("450821198707300820","彭士玲","13726310403"),
new PregnantInfo("440681198406146026","苏淑景","13702620370"),
new PregnantInfo("440681199210294806","梁佩岚","13790098896"),
new PregnantInfo("441225199107195824","李伟裕","15217540854"),
new PregnantInfo("440681198812025924","杜丽君","13516615761"),
new PregnantInfo("450721199703023941","劳春玉","13714441016"),
new PregnantInfo("441522199511253106","林慧萍","13268149241"),
new PregnantInfo("44182319970307552X","陈桂珍","15119925811"),
new PregnantInfo("440681198811268027","黎秀清","13534560038"),
new PregnantInfo("440232199102056422","杨文珍","13928533691"),
new PregnantInfo("440681199408148046","钟键华","17674169886"),
new PregnantInfo("440681199104014241","梅云燕","13425792302"),
new PregnantInfo("445322199510083726","李燕红","18023766109"),
new PregnantInfo("450422199304152121","姚海梅","13516605037"),
new PregnantInfo("441523199306067562","范丹凤","13026781963"),
new PregnantInfo("370832198902203347","薛梅","13929117154"),
new PregnantInfo("450422199105251127","廖玉燕","15390910228"),
new PregnantInfo("441324199011243624","巫远芳","13680751361"),
new PregnantInfo("440681198509074723","梁艳贞","13727480148"),
new PregnantInfo("510781199403074125","张艺莲","18320909270"),
new PregnantInfo("440281198910040026","黎慧星","15814860759"),
new PregnantInfo("440681198507040861","林泳球","13929187866"),
new PregnantInfo("441823198911146420","唐韦委","13724685202"),
new PregnantInfo("452727198703021829","余彩祝","18176174792"),
new PregnantInfo("452402199312091249","李土英","13686557859"),
new PregnantInfo("142701198712186928","马艮红","15015829461"),
new PregnantInfo("421125199101103328","王泉","15002270969"),
new PregnantInfo("440921199706296020","张晓仪","15918177303"),
new PregnantInfo("440881199612304625","吴清清","13434718231"),
new PregnantInfo("513029199701012407","施虹竹","15916166283"),
new PregnantInfo("510503199001163483","王修红","18924833593"),
new PregnantInfo("440681199108123648","陈焕湘","13450863511"),
new PregnantInfo("362427200006212525","王雨","15159835811"),
new PregnantInfo("440681198904302669","周丽芬","13424617549"),
new PregnantInfo("440981199106193227","邓梅清","18824824961"),
new PregnantInfo("441623199307142726","张晶晶","13670643583"),
new PregnantInfo("450221198805111940","覃承丽","18823485004"),
new PregnantInfo("450881199102221725","王晓君","18302078929"),
new PregnantInfo("440681199306026822","李丽君","15015651924"),
new PregnantInfo("440681199410040229","郭柳宜","15916562128"),
new PregnantInfo("450330198510101405","覃桂兰","18316544258"),
new PregnantInfo("412702198503040043","黄永华","15820613712"),
new PregnantInfo("450921199307270423","黄馨霆","18776195332"),
new PregnantInfo("440681199706246824","李嘉慧","13078440442"),
new PregnantInfo("440681199204080625","辛海怡","13539344200"),
new PregnantInfo("450121198802152442","杨琼","18520970881"),
new PregnantInfo("440681199912175465","李淑婷","13827700152"),
new PregnantInfo("440881199005262240","黄恩凤","13641416963"),
new PregnantInfo("411422199103023925","张海梅","18613135640"),
new PregnantInfo("441224198002185725","梁洁梅","13420621338"),
new PregnantInfo("450802198210152069","韦素芳","13326784777"),
new PregnantInfo("441522199311183027","林晓玲","13450883344"),
new PregnantInfo("440681198702255446","冯秀群","13690335665"),
new PregnantInfo("450422199605151528","罗丽萍","13113749403"),
new PregnantInfo("421122198911085420","邓小良","15813602161"),
new PregnantInfo("450803198706217520","梁叶兰","18707852160"),
new PregnantInfo("230204198607041243","徐威","18923255585"),
new PregnantInfo("45088120010813234X","陈凤娟","15914584865"),
new PregnantInfo("441424199211252600","钟优丽","15907536651"),
new PregnantInfo("440681198304241049","黄结明","15099876717"),
new PregnantInfo("450421199202197023","陀燕花","13630012558"),
new PregnantInfo("36073119921219822X","钟玲玲","18707971401"),
new PregnantInfo("511525199408197369","张琼","13702650260"),
new PregnantInfo("520121198708270029","陈荣","15815621706"),
new PregnantInfo("41132519931011134X","孔祥业","17633661668"),
new PregnantInfo("445281199609134129","罗丹妮","13652966984"),
new PregnantInfo("440681198909036822","李思艺","13794099695"),
new PregnantInfo("432503198811225022","龚彩云","18823474232"),
new PregnantInfo("350600199004014543","郑雅华","18138351772"),


                     new PregnantInfo("421122198812203567","李美娟","13476677233"),
                };
                for (int i = currentIndex + 1; i < pregnantInfos.Count(); i++)
                {
                    var pregnantInfo = pregnantInfos[i];
                    //查询基本信息
                    var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userId}&sParams=P${pregnantInfo.idcard}$P$P";
                    var postData = "";
                    var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var re1 = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    bool isExist = re1.data.Count != 0;
                    Console.WriteLine($"{isExist},{pregnantInfo.idcard},{pregnantInfo.personname},{pregnantInfo.mobilenumber}");
                    if (isExist)
                        continue;
                    currentPregnant = pregnantInfo;
                    currentIndex = i;
                    break;
                }
            }));
            cmds.Add(new Command("m171,0617,模拟-保存待提交的数据Raw=>中文会乱码", () =>
            {
                var container = new CookieContainer();
                var userId = "35000528";
                var userName = "廖凤贤";
                var orgId = "45608491-9";
                var orgName = "佛山市妇幼保健院";
                var pregnantInfo = currentPregnant;
                if (pregnantInfo == null)
                    return;
                StringBuilder sb = new StringBuilder();
                //Create 患者主索引
                var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_QHJC_ID_GET&sUserID={userId}&sParams=1";
                var postData = "";
                var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var mainId = result.FromJsonToAnonymousType(new { id = "" }).id;
                Console.WriteLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard}");
                Console.WriteLine($"mainId:{mainId}");
                sb.AppendLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard},Phone:{pregnantInfo.mobilenumber}");
                sb.AppendLine("Create 患者主索引");
                sb.AppendLine(url);
                sb.AppendLine($"mainId:{mainId}");
                //Create 保健号
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=CDH_GET_ID1&sUserID={userId}&sParams={orgId}";
                postData = "";
                result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var careId = result.FromJsonToAnonymousType(new { id = "" }).id;
                Console.WriteLine($"careId:{careId}");
                sb.AppendLine("Create 保健号");
                sb.AppendLine(url);
                sb.AppendLine($"careId:{careId}");
                var careIdL8 = careId.Substring(8);
                //--------查重
                url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_CC&sUserID={userId}&sParams={mainId}$P${pregnantInfo.idcard}&pageSize=10000&pageIndex=0";
                postData = "";
                result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                sb.AppendLine("--------查重");
                sb.AppendLine(result);
                //Create 基本信息
                url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={userId}&sParams=null${mainId}${orgId}$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                //http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID=35000528&sParams=null${mainId}$45608491-9$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80
                var data = new List<WMH_CQBJ_JBXX_FORM_SAVEData>()
                    {
                        new WMH_CQBJ_JBXX_FORM_SAVEData(){
        D1 = careIdL8, //@保健号后8位
        D2= careId, //@保健号
        D7 = pregnantInfo.idcard,   //身份证              
        D58 = DateTime.Now.ToString("yyyy-MM-dd"),//创建时间
        curdate1 =DateTime.Now.ToString("yyyy-MM-dd"),
        D59 = orgId,//创建机构Id
        D60 = userName, //创建人员
        D61 = null,//病案号
        D69 = orgName, //创建机构名称:佛山市妇幼保健院
        D70="",//健康码
        D71="",// 之前模拟的时候填了别人用过的 //健康码ID
        D3 = pregnantInfo.personname,//孕妇姓名
        D4 = "", //孕妇国籍 对照表 2) 1)  国籍代码GB/T 2659
        D5 = "", //孕妇民族 1)  民族代码GB/T 3304
        D6 = "", //孕妇证件类型1)   证件类型CV02.01.101
        D8="",//生日
        D9 = "", //孕妇年龄
        D10 = "", //孕妇文化程度 1)  文化程度STD_CULTURALDEG
        D11= pregnantInfo.mobilenumber, //手机号码
        D12 = "",//孕妇职业 1)  职业STD_OCCUPATION
        D13 = "", //孕妇工作单位
        D14 = "", //孕妇籍贯
        D15 ="44", //孕妇户籍地址 [ 对照表] 省2位,市2位,县/区2位,乡镇街道3位,社区/村3位
        D16 ="4419", //孕妇户籍地址 [ 对照表]
        D17 ="441901",//孕妇户籍地址 [ 对照表]
        D18 ="441901103", //孕妇户籍地址 [ 对照表]
        D19 = "", //孕妇户籍地址 [ 对照表]
        D20 = "", //户籍详细地址
        D21 = "", //孕妇现住地址 [ 对照表]
        D22 = "", //孕妇现住地址 [ 对照表]
        D23 = "", //孕妇现住地址 [ 对照表]
        D24 = "", //孕妇现住地址 [ 对照表]
        D25 = "", //孕妇现住地址 [ 对照表]
        D26 = "", //产后休养地址
        D27 = "", //产后休养地址 [ 对照表]
        D28 = "", //产后休养地址 [ 对照表]
        D29 = "", //产后休养地址 [ 对照表]
        D30 = "", //产后休养地址 [ 对照表]
        D31 = "", //产后休养地址 [ 对照表]
        D32 = "", //产后详细地址
        D33 = "", //孕妇户籍类型 1)  户籍类型STD_REGISTERT2PE
        D34 = "", //孕妇户籍分类 非户籍:2 ,户籍:1
        D35 = "", //来本地居住时间 
        D36 = "", //近亲结婚  [ 对照表]
        D37 = "", //孕妇结婚年龄 
        D38 = "", //丈夫结婚年龄 
        D39 = "", //丈夫姓名
        D40 = "", //丈夫国籍  [ 对照表]
        D41 = "", //丈夫民族  [ 对照表]
        D42 = "", //丈夫证件类型  [ 对照表]
        D43 = "", //丈夫证件号码
        D44 = "", //丈夫出生日期
        D45 = "", //丈夫登记年龄
        D46 = "", //丈夫职业  [ 对照表]
        D47 = "",  //丈夫工作单位
        D48 = "", //丈夫联系电话
        D49 = "", //丈夫健康状况   [ 对照表]
        D50 = "", //丈夫嗜好   [ 对照表]
        D51 = "", //丈夫现在地址   [ 对照表]
        D52 = "", //丈夫现在地址
        D53 = "", //丈夫现在地址
        D54 = "", //丈夫现在地址
        D55 = "", //丈夫现在地址
        D56 = "", //现住详细地址
        D57 = "",
        D62 = "", //婚姻状况  [ 对照表]
        D63 = "", //医疗费用支付方式  [ 对照表]
        D64 = "", //厨房排风设施 PASS
        D65 = "", //燃料类型 PASS
        D66 = "", //饮水 PASS
        D67 = "", //厕所  PASS
        D68 = "", //禽畜栏 PASS
    }
                    };
                var json = data.ToJson();
                postData = "data=" + json;
                result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var file = Path.Combine(Directory.GetCurrentDirectory(), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                sb.AppendLine("Create 基本信息");
                sb.AppendLine(url);
                sb.AppendLine(json);
                sb.AppendLine(result);
                File.WriteAllText(file, sb.ToString());

                Console.WriteLine($"result:{file}");
            }));
            cmds.Add(new Command("m172,0617,模拟-保存待提交的数据UrlEncode(json)=>KO", () =>
            {
                var container = new CookieContainer();
                var userId = "35000528";
                var userName = "廖凤贤";
                var orgId = "45608491-9";
                var orgName = "佛山市妇幼保健院";
                var pregnantInfo = currentPregnant;
                if (pregnantInfo == null)
                    return;
                StringBuilder sb = new StringBuilder();
                //Create 患者主索引
                var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_QHJC_ID_GET&sUserID={userId}&sParams=1";
                var postData = "";
                var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var mainId = result.FromJsonToAnonymousType(new { id = "" }).id;
                Console.WriteLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard}");
                Console.WriteLine($"mainId:{mainId}");
                sb.AppendLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard},Phone:{pregnantInfo.mobilenumber}");
                sb.AppendLine("Create 患者主索引");
                sb.AppendLine(url);
                sb.AppendLine($"mainId:{mainId}");
                //Create 保健号
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=CDH_GET_ID1&sUserID={userId}&sParams={orgId}";
                postData = "";
                result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var careId = result.FromJsonToAnonymousType(new { id = "" }).id;
                Console.WriteLine($"careId:{careId}");
                sb.AppendLine("Create 保健号");
                sb.AppendLine(url);
                sb.AppendLine($"careId:{careId}");
                var careIdL8 = careId.Substring(8);
                //--------查重
                url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_CC&sUserID={userId}&sParams={mainId}$P${pregnantInfo.idcard}&pageSize=10000&pageIndex=0";
                postData = "";
                result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                sb.AppendLine("--------查重");
                sb.AppendLine(result);
                //Create 基本信息
                url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={userId}&sParams=null${mainId}${orgId}$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                //http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID=35000528&sParams=null${mainId}$45608491-9$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80
                var data = new List<WMH_CQBJ_JBXX_FORM_SAVEData>()
                    {
                        new WMH_CQBJ_JBXX_FORM_SAVEData(){
        D1 = careIdL8, //@保健号后8位
        D2= careId, //@保健号
        D7 = pregnantInfo.idcard,   //身份证              
        D58 = DateTime.Now.ToString("yyyy-MM-dd"),//创建时间
        curdate1 =DateTime.Now.ToString("yyyy-MM-dd"),
        D59 = orgId,//创建机构Id
        D60 = userName, //创建人员
        D61 = null,//病案号
        D69 = orgName, //创建机构名称:佛山市妇幼保健院
        D70="",//健康码
        D71="",// 之前模拟的时候填了别人用过的 //健康码ID
        D3 = pregnantInfo.personname,//孕妇姓名
        D4 = "", //孕妇国籍 对照表 2) 1)  国籍代码GB/T 2659
        D5 = "", //孕妇民族 1)  民族代码GB/T 3304
        D6 = "", //孕妇证件类型1)   证件类型CV02.01.101
        D8="",//生日
        D9 = "", //孕妇年龄
        D10 = "", //孕妇文化程度 1)  文化程度STD_CULTURALDEG
        D11= pregnantInfo.mobilenumber, //手机号码
        D12 = "",//孕妇职业 1)  职业STD_OCCUPATION
        D13 = "", //孕妇工作单位
        D14 = "", //孕妇籍贯
        D15 ="44", //孕妇户籍地址 [ 对照表] 省2位,市2位,县/区2位,乡镇街道3位,社区/村3位
        D16 ="4419", //孕妇户籍地址 [ 对照表]
        D17 ="441901",//孕妇户籍地址 [ 对照表]
        D18 ="441901103", //孕妇户籍地址 [ 对照表]
        D19 = "", //孕妇户籍地址 [ 对照表]
        D20 = "", //户籍详细地址
        D21 = "", //孕妇现住地址 [ 对照表]
        D22 = "", //孕妇现住地址 [ 对照表]
        D23 = "", //孕妇现住地址 [ 对照表]
        D24 = "", //孕妇现住地址 [ 对照表]
        D25 = "", //孕妇现住地址 [ 对照表]
        D26 = "", //产后休养地址
        D27 = "", //产后休养地址 [ 对照表]
        D28 = "", //产后休养地址 [ 对照表]
        D29 = "", //产后休养地址 [ 对照表]
        D30 = "", //产后休养地址 [ 对照表]
        D31 = "", //产后休养地址 [ 对照表]
        D32 = "", //产后详细地址
        D33 = "", //孕妇户籍类型 1)  户籍类型STD_REGISTERT2PE
        D34 = "", //孕妇户籍分类 非户籍:2 ,户籍:1
        D35 = "", //来本地居住时间 
        D36 = "", //近亲结婚  [ 对照表]
        D37 = "", //孕妇结婚年龄 
        D38 = "", //丈夫结婚年龄 
        D39 = "", //丈夫姓名
        D40 = "", //丈夫国籍  [ 对照表]
        D41 = "", //丈夫民族  [ 对照表]
        D42 = "", //丈夫证件类型  [ 对照表]
        D43 = "", //丈夫证件号码
        D44 = "", //丈夫出生日期
        D45 = "", //丈夫登记年龄
        D46 = "", //丈夫职业  [ 对照表]
        D47 = "",  //丈夫工作单位
        D48 = "", //丈夫联系电话
        D49 = "", //丈夫健康状况   [ 对照表]
        D50 = "", //丈夫嗜好   [ 对照表]
        D51 = "", //丈夫现在地址   [ 对照表]
        D52 = "", //丈夫现在地址
        D53 = "", //丈夫现在地址
        D54 = "", //丈夫现在地址
        D55 = "", //丈夫现在地址
        D56 = "", //现住详细地址
        D57 = "",
        D62 = "", //婚姻状况  [ 对照表]
        D63 = "", //医疗费用支付方式  [ 对照表]
        D64 = "", //厨房排风设施 PASS
        D65 = "", //燃料类型 PASS
        D66 = "", //饮水 PASS
        D67 = "", //厕所  PASS
        D68 = "", //禽畜栏 PASS
    }
                    };
                var json = data.ToJson();
                postData = "data=" + HttpUtility.UrlEncode(json);
                result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var file = Path.Combine(Directory.GetCurrentDirectory(), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                sb.AppendLine("Create 基本信息");
                sb.AppendLine(url);
                sb.AppendLine(json);
                sb.AppendLine(result);
                File.WriteAllText(file, sb.ToString());

                Console.WriteLine($"result:{file}");
            }));
            cmds.Add(new Command("m18,0618,模拟-不保存待提交的数据", () =>
            {
                var container = new CookieContainer();
                var userId = "35000528";
                var userName = "廖凤贤";
                var orgId = "45608491-9";
                var orgName = "佛山市妇幼保健院";
                var pregnantInfo = currentPregnant;
                if (pregnantInfo == null)
                    return;
                StringBuilder sb = new StringBuilder();
                //Create 患者主索引
                var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_QHJC_ID_GET&sUserID={userId}&sParams=1";
                var postData = "";
                var result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var mainId = result.FromJsonToAnonymousType(new { id = "" }).id;
                Console.WriteLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard}");
                Console.WriteLine($"mainId:{mainId}");
                sb.AppendLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard},Phone:{pregnantInfo.mobilenumber}");
                sb.AppendLine("Create 患者主索引");
                sb.AppendLine(url);
                sb.AppendLine($"mainId:{mainId}");
                //Create 保健号
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=CDH_GET_ID1&sUserID={userId}&sParams={orgId}";
                postData = "";
                result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var careId = result.FromJsonToAnonymousType(new { id = "" }).id;
                Console.WriteLine($"careId:{careId}");
                sb.AppendLine("Create 保健号");
                sb.AppendLine(url);
                sb.AppendLine($"careId:{careId}");
                var careIdL8 = careId.Substring(8);
                //--------查重
                url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_CC&sUserID={userId}&sParams={mainId}$P${pregnantInfo.idcard}&pageSize=10000&pageIndex=0";
                postData = "";
                result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                sb.AppendLine("--------查重");
                sb.AppendLine(result);
                //Create 基本信息
                url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={userId}&sParams=null${mainId}${orgId}$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                //http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID=35000528&sParams=null${mainId}$45608491-9$%E5%BB%96%E5%87%A4%E8%B4%A4$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80
                var data = new List<WMH_CQBJ_JBXX_FORM_SAVEData>()
                    {
                        new WMH_CQBJ_JBXX_FORM_SAVEData(){
        D1 = careIdL8, //@保健号后8位
        D2= careId, //@保健号
        D7 = pregnantInfo.idcard,   //身份证              
        D58 = DateTime.Now.ToString("yyyy-MM-dd"),//创建时间
        curdate1 =DateTime.Now.ToString("yyyy-MM-dd"),
        D59 = orgId,//创建机构Id
        D60 = userName, //创建人员
        D61 = null,//病案号
        D69 = orgName, //创建机构名称:佛山市妇幼保健院
        D70="",//健康码
        D71="",// 之前模拟的时候填了别人用过的 //健康码ID
        D3 = pregnantInfo.personname,//孕妇姓名
        D4 = "", //孕妇国籍 对照表 2) 1)  国籍代码GB/T 2659
        D5 = "", //孕妇民族 1)  民族代码GB/T 3304
        D6 = "", //孕妇证件类型1)   证件类型CV02.01.101
        D8="",//生日
        D9 = "", //孕妇年龄
        D10 = "", //孕妇文化程度 1)  文化程度STD_CULTURALDEG
        D11= pregnantInfo.mobilenumber, //手机号码
        D12 = "",//孕妇职业 1)  职业STD_OCCUPATION
        D13 = "", //孕妇工作单位
        D14 = "", //孕妇籍贯
        D15 ="44", //孕妇户籍地址 [ 对照表] 省2位,市2位,县/区2位,乡镇街道3位,社区/村3位
        D16 ="4419", //孕妇户籍地址 [ 对照表]
        D17 ="441901",//孕妇户籍地址 [ 对照表]
        D18 ="441901103", //孕妇户籍地址 [ 对照表]
        D19 = "", //孕妇户籍地址 [ 对照表]
        D20 = "", //户籍详细地址
        D21 = "", //孕妇现住地址 [ 对照表]
        D22 = "", //孕妇现住地址 [ 对照表]
        D23 = "", //孕妇现住地址 [ 对照表]
        D24 = "", //孕妇现住地址 [ 对照表]
        D25 = "", //孕妇现住地址 [ 对照表]
        D26 = "", //产后休养地址
        D27 = "", //产后休养地址 [ 对照表]
        D28 = "", //产后休养地址 [ 对照表]
        D29 = "", //产后休养地址 [ 对照表]
        D30 = "", //产后休养地址 [ 对照表]
        D31 = "", //产后休养地址 [ 对照表]
        D32 = "", //产后详细地址
        D33 = "", //孕妇户籍类型 1)  户籍类型STD_REGISTERT2PE
        D34 = "", //孕妇户籍分类 非户籍:2 ,户籍:1
        D35 = "", //来本地居住时间 
        D36 = "", //近亲结婚  [ 对照表]
        D37 = "", //孕妇结婚年龄 
        D38 = "", //丈夫结婚年龄 
        D39 = "", //丈夫姓名
        D40 = "", //丈夫国籍  [ 对照表]
        D41 = "", //丈夫民族  [ 对照表]
        D42 = "", //丈夫证件类型  [ 对照表]
        D43 = "", //丈夫证件号码
        D44 = "", //丈夫出生日期
        D45 = "", //丈夫登记年龄
        D46 = "", //丈夫职业  [ 对照表]
        D47 = "",  //丈夫工作单位
        D48 = "", //丈夫联系电话
        D49 = "", //丈夫健康状况   [ 对照表]
        D50 = "", //丈夫嗜好   [ 对照表]
        D51 = "", //丈夫现在地址   [ 对照表]
        D52 = "", //丈夫现在地址
        D53 = "", //丈夫现在地址
        D54 = "", //丈夫现在地址
        D55 = "", //丈夫现在地址
        D56 = "", //现住详细地址
        D57 = "",
        D62 = "", //婚姻状况  [ 对照表]
        D63 = "", //医疗费用支付方式  [ 对照表]
        D64 = "", //厨房排风设施 PASS
        D65 = "", //燃料类型 PASS
        D66 = "", //饮水 PASS
        D67 = "", //厕所  PASS
        D68 = "", //禽畜栏 PASS
    }
                    };
                var json = data.ToJson();
                postData = "data:" + HttpUtility.UrlEncode(json);
                //result = SDHttpHelper.Post(url, postData, ref container, contentType: "text/text;charset=UTF-8");
                var file = Path.Combine(Directory.GetCurrentDirectory(), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                sb.AppendLine("Create 基本信息");
                sb.AppendLine(url);
                sb.AppendLine(json);
                //sb.AppendLine(result);
                File.WriteAllText(file, sb.ToString());

                Console.WriteLine($"result:{file}");
            }));
            cmds.Add(new Command("m21,0618,模拟-获取待新增的孕妇", () =>
            {
                SDService.TempPregnantInfos = SDService.GetPregnantInfoForCreate();
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(pregnantInfo.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\ToCreate_" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m22,0618,模拟-模拟新增", () =>
            {
                SDService.MockCommitCreatePregnantInfo(SDService.TempPregnantInfos);
            }));
            cmds.Add(new Command("m23,0618,模拟-获取待更新的孕妇", () =>
            {
                SDService.TempPregnantInfos = SDService.GetPregnantInfosToUpdate();
            }));
            cmds.Add(new Command("m24,0618,模拟-模拟更新(仅记录数据)", () =>
            {
                var testOne = false;
                var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
                var dbContext = DBHelper.GetSqlDbContext(conntectingStringSD);
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    if (testOne)
                        break;

                    StringBuilder sb = new StringBuilder();
                    #region mock commit
                    var container = new CookieContainer();
                    var userId = "35000528";
                    var userName = "廖凤贤";
                    var orgId = "45608491-9";
                    var orgName = "佛山市妇幼保健院";
                    var url = "";
                    var postData = "";
                    var result = "";
                    if (pregnantInfo == null)
                        return;
                    //查询孕妇 概要数据(各类Id)
                    url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userId}&sParams=P${pregnantInfo.idcard}$P$P";
                    result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var resultBase = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    sb.AppendLine(result);
                    if (resultBase.data.Count == 0)
                    {
                        var message = $"待更新的孕妇{pregnantInfo.personname}缺少概要数据";
                        Console.WriteLine(message);
                        var syncForFSTemp = new SyncOrder()
                        {
                            TargetType = TargetType.PregnantInfo,
                            SourceId = pregnantInfo.Id.ToString(),
                            SyncTime = DateTime.Now,
                            SyncStatus = SyncStatus.Error,
                            ErrorMessage = message,
                        };
                        var serviceResultTemp = dbContext.DelegateTransaction((group) =>
                        {
                            return group.Connection.Insert(syncForFSTemp, transaction: group.Transaction);
                        });
                        testOne = true;
                        continue;
                    }
                    //查询孕妇 基本数据
                    var baseMain = resultBase.data.First();
                    url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_READ&sUserID={userId}&sParams={baseMain.MainId}";
                    result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var resultBaseInfo = result.FromJson<WMH_CQBJ_JBXX_FORM_READResponse>();
                    if (resultBaseInfo.data.Count == 0)
                    {
                        var message = $"待更新的孕妇{pregnantInfo.personname}缺少基本数据";
                        Console.WriteLine(message);
                        var syncForFSTemp = new SyncOrder()
                        {
                            TargetType = TargetType.PregnantInfo,
                            SourceId = pregnantInfo.Id.ToString(),
                            SyncTime = DateTime.Now,
                            SyncStatus = SyncStatus.Error,
                            ErrorMessage = message,
                        };
                        var serviceResultTemp = dbContext.DelegateTransaction((group) =>
                        {
                            return group.Connection.Insert(syncForFSTemp, transaction: group.Transaction);
                        });
                        testOne = true;
                        continue;
                    }
                    #region 更新用户数据
                    //>>>>>>>>>>>>>> baseInfo
                    //{ "D1":"21509773","D2":"4406060121509773","D3":"吴素敏","D4":"","D5":"","D6":"","D7":"44200019930330260x","D8":"",
                    //"D9":"","D10":"","D11":"16603076028","D12":"","D13":"","D14":"","D15":"44","D16":"4419","D17":"441901",
                    //"D18":"441901103","D19":"","D20":"","D21":"","D22":"","D23":"","D24":"","D25":"","D26":"","D27":"","D28":"",
                    //"D29":"","D30":"","D31":"","D32":"","D33":"","D34":"","D35":"","D36":"","D37":"","D38":"","D39":"","D40":"",
                    //"D41":"","D42":"","D43":"","D44":"","D45":"","D46":"","D47":"","D48":"","D49":"","D50":"","D51":"","D52":"",
                    //"D53":"","D54":"","D55":"","D56":"","D57":"","D58":"2020-06-18","D59":"45608491-9","D60":"廖凤贤","D61":"",
                    //"D62":"","D63":"","D64":"","D65":"","D66":"","D67":"","D68":"","D69":"佛山市顺德区妇幼保健院","D70":"",
                    //"D71":"","D72":"","D73":"","D74":"","D75":"A859402EC9E10237E05355FE8013C745","D76":"","D77":""}
                    //>>>>>>>>>>>>>> pregnantInfo
                    //{ "Id":64181,"pregnantbookid":null,"personname":"吴素敏","birthday":"1993-03-30T00:00:00","idtype":"1","idcard":"44200019930330260x",
                    //"nationalitycode":"156","nationcode":"01","registrationtype":null,"workname":"自由职业","workcode":null,
                    //    "educationcode":"70","workplace":null,"homeaddress":"440600000000","liveplace":"440600000000","zipcode":null,
                    //    "mobilenumber":"16603076028","husbandname":"梁洪标","husbandnationalitycode":null,"husbandnationcode":null,
                    //    "husbandidtype":"1","husbandidcard":"440681198907022638","husbandbirthday":"0001-01-01T00:00:00",
                    //    "husbandeducationcode":null,"husbandmobile":"13450765716","husbandliveaddresscode":null}

                    var jbxx = resultBaseInfo.data.First();
                    var data = new WMH_CQBJ_JBXX_FORM_SAVEData(jbxx);
                    //public string D1 { set; get; } //登录用户Id
                    //public string D2 { set; get; } //@保健号
                    //public string D3 { set; get; } //孕妇姓名
                    data.D3 = pregnantInfo.personname;
                    //public string D4 { set; get; } //孕妇国籍 对照表 2) 1)  国籍代码GB/T 2659
                    data.D4 = VLConstraints.GetCountry_GB_T_2659ByCountry_GB_T_2659_2000(pregnantInfo.nationalitycode);
                    //public string D5 { set; get; } //孕妇民族 1)  民族代码GB/T 3304
                    data.D5 = pregnantInfo.nationcode;
                    //public string D6 { set; get; } //孕妇证件类型1)   证件类型CV02.01.101
                    data.D6 = VLConstraints.GetCardType_CV02_01_101ByCardType_Hele(pregnantInfo.idtype);
                    //public string D7 { set; get; } //身份证
                    data.D7 = pregnantInfo.idcard;
                    //public string D8 { set; get; } //生日
                    data.D8 = pregnantInfo.birthday?.ToString("yyyy-MM-dd");
                    //public string D9 { set; get; } //孕妇年龄
                    data.D9 = pregnantInfo.createage;
                    //public string D10 { set; get; } //孕妇文化程度 1)  文化程度STD_CULTURALDEG
                    data.D10 = VLConstraints.GetDegree_STD_CULTURALDEGByDegree_Hele(pregnantInfo.educationcode);
                    //public string D11 { set; get; } //手机号码
                    data.D11 = pregnantInfo.mobilenumber;
                    //public string D12 { set; get; } //孕妇职业 1)  职业STD_OCCUPATION PASS(未有在用)
                    //data.D12 = VLConstraints.GetOccupation_STD_OCCUPATIONByOccupation_Hele(pregnantInfo.workcode);
                    //public string D13 { set; get; } //孕妇工作单位
                    data.D13 = pregnantInfo.workplace;
                    //public string D14 { set; get; } //孕妇籍贯 PASS
                    //public string D15 { set; get; } //孕妇户籍地址 省2位,市2位,县/区2位,乡镇街道3位,社区/村3位
                    data.D15 = pregnantInfo.homeaddress.GetSubStringOrEmpty(0, 2);
                    //public string D16 { set; get; } //孕妇户籍地址
                    data.D16 = pregnantInfo.homeaddress.GetSubStringOrEmpty(0, 4);
                    //public string D17 { set; get; } //孕妇户籍地址
                    data.D17 = pregnantInfo.homeaddress.GetSubStringOrEmpty(0, 6);
                    //public string D18 { set; get; } //孕妇户籍地址
                    data.D18 = pregnantInfo.homeaddress.GetSubStringOrEmpty(0, 9);
                    //public string D19 { set; get; } //孕妇户籍地址
                    data.D19 = pregnantInfo.homeaddress;
                    //public string D20 { set; get; } //户籍详细地址
                    data.D20 = pregnantInfo.homeaddress_text;
                    //public string D21 { set; get; } //孕妇现住地址
                    data.D21 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 2);
                    //public string D22 { set; get; } //孕妇现住地址
                    data.D22 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 4);
                    //public string D23 { set; get; } //孕妇现住地址
                    data.D23 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 6);
                    //public string D24 { set; get; } //孕妇现住地址
                    data.D24 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 9);
                    //public string D25 { set; get; } //孕妇现住地址
                    data.D25 = pregnantInfo.liveplace;
                    //public string D26 { set; get; } //孕妇现住地址-详细
                    data.D26 = pregnantInfo.liveplace_text;
                    //public string D27 { set; get; } //产后休养地址
                    data.D27 = pregnantInfo.restregioncode.GetSubStringOrEmpty(0, 2);
                    //public string D28 { set; get; } //产后休养地址
                    data.D28 = pregnantInfo.restregioncode.GetSubStringOrEmpty(0, 4);
                    //public string D29 { set; get; } //产后休养地址
                    data.D29 = pregnantInfo.restregioncode.GetSubStringOrEmpty(0, 6);
                    //public string D30 { set; get; } //产后休养地址
                    data.D30 = pregnantInfo.restregioncode.GetSubStringOrEmpty(0, 9);
                    //public string D31 { set; get; } //产后休养地址
                    data.D31 = pregnantInfo.restregioncode;
                    //public string D32 { set; get; } //产后详细地址
                    data.D32 = pregnantInfo.restregiontext;

                    //public string D33 { set; get; } //孕妇户籍类型 1)  户籍类型STD_REGISTERT2PE
                    //data.D33 = VLConstraints.GetRegisterType_STD_REGISTERT2PE_By_RegisterType_HELE(pregnantInfo.isagrregister);
                    //public string D34 { set; get; } //孕妇户籍分类 非户籍:2 ,户籍:1   PASS
                    //public string D35 { set; get; } //来本地居住时间  PASS
                    //public string D36 { set; get; } //近亲结婚        PASS
                    //public string D37 { set; get; } //孕妇结婚年龄    PASS
                    //public string D38 { set; get; } //丈夫结婚年龄    PASS


                    //public string D39 { set; get; } //丈夫姓名
                    data.D39 = pregnantInfo.husbandname;
                    //public string D40 { set; get; } //丈夫国籍 PASS(无数据)
                    //data.D40 = VLConstraints.GetCountry_GB_T_2659ByCountry_GB_T_2659_2000(pregnantInfo.husbandnationalitycode);
                    //public string D41 { set; get; } //丈夫民族 PASS(无数据)
                    //data.D41 = pregnantInfo.husbandnationcode;
                    //public string D42 { set; get; } //丈夫证件类型
                    data.D42 = VLConstraints.GetCardType_CV02_01_101ByCardType_Hele(pregnantInfo.husbandidtype);
                    //public string D43 { set; get; } //丈夫证件号码
                    data.D43 = pregnantInfo.husbandidcard;
                    //public string D44 { set; get; } //丈夫出生日期
                    data.D44 = pregnantInfo.husbandbirthday?.ToString("yyyy-MM-dd");
                    //public string D45 { set; get; } //丈夫登记年龄
                    data.D45 = pregnantInfo.husbandage;
                    //public string D46 { set; get; } //丈夫职业  [ 对照表] PASS(未有在用)
                    //data.D12 = VLConstraints.GetOccupation_STD_OCCUPATIONByOccupation_Hele(pregnantInfo.husbandworkcode);
                    //public string D47 { set; get; }  //丈夫工作单位 PASS(未有在用)
                    //public string D48 { set; get; } //丈夫联系电话
                    data.D48 = pregnantInfo.husbandmobile;
                    //public string D49 { set; get; } //丈夫健康状况   PASS
                    //public string D50 { set; get; } //丈夫嗜好       PASS
                    //public string D51 { set; get; } //丈夫现在地址 由于我方系统录入的是丈夫的户籍地址,经确认采用孕妇的现住地址
                    data.D51 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 2);
                    //public string D52 { set; get; } //丈夫现在地址
                    data.D52 = pregnantInfo.liveplace.GetSubStringOrEmpty(2, 2);
                    //public string D53 { set; get; } //丈夫现在地址
                    data.D53 = pregnantInfo.liveplace.GetSubStringOrEmpty(4, 2);
                    //public string D54 { set; get; } //丈夫现在地址
                    data.D54 = pregnantInfo.liveplace.GetSubStringOrEmpty(6, 3);
                    //public string D55 { set; get; } //丈夫现在地址
                    data.D55 = pregnantInfo.liveplace.GetSubStringOrEmpty(9, 3);
                    //public string D56 { set; get; } //现住详细地址
                    data.D56 = pregnantInfo.liveplace_text;
                    //public string D57 { set; get; }
                    //public string D58 { set; get; } //创建时间
                    //public string D59 { set; get; } //创建机构
                    //public string D60 { set; get; } //创建人员
                    //public string D61 { set; get; } //病案号
                    //public string D62 { set; get; } //婚姻状况
                    data.D62 = VLConstraints.GetMaritalStatus_STD_MARRIAGEByMaritalStatus_HELE(pregnantInfo.maritalstatuscode);
                    //public string D63 { set; get; } //医疗费用支付方式 PASS
                    //public string D64 { set; get; } //厨房排风设施 PASS
                    //public string D65 { set; get; } //燃料类型 PASS
                    //public string D66 { set; get; } //饮水 PASS
                    //public string D67 { set; get; } //厕所  PASS
                    //public string D68 { set; get; } //禽畜栏 PASS
                    //public string D69 { set; get; } //创建机构名称:佛山市妇幼保健院
                    //public string D70 { set; get; } //健康码
                    //public string D71 { set; get; } //健康码ID 
                    #endregion


                    testOne = true;
                    #endregion
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update_" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m25,0618,模拟-模拟更新", () =>
            {
                var isTestOne = true;
                SDService.MockCommitUpdatePregnantInfo(isTestOne);
            }));
            cmds.Add(new Command("m26,0618,模拟-筛选待新增的孕妇", () =>
            {
                SDService.TempPregnantInfos = SDService.GetPregnantInfoForCreateBefore0630();
                var dbContext = DBHelper.GetSqlDbContext(SDService.ConntectingStringSD);
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    Console.WriteLine($"当前孕妇{pregnantInfo.personname}");
                    var userInfo = SDService.UserInfo;
                    var container = new CookieContainer();
                    var userId = userInfo.UserId;
                    var userName = userInfo.UserName;
                    var encodeUserName = userInfo.EncodeUserName;
                    var orgId = userInfo.OrgId;
                    var orgName = userInfo.OrgName;
                    var url = "";
                    var postData = "";
                    var result = "";
                    if (pregnantInfo == null)
                        return;
                    //孕妇是否已存在
                    url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userId}&sParams=P${pregnantInfo.idcard}$P$P";
                    result = FSHttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    if (re.data.Count != 0)
                    {
                        Console.WriteLine($"孕妇{pregnantInfo.personname}已存在");
                        continue;
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(pregnantInfo.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\ToCreate_" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    }
                }
            }));
            cmds.Add(new Command("m301,0623,模拟-获取待新增的`问询病史`", () =>
            {
                var userInfo = SDService.UserInfo;
                SDService.TempPregnantInfos = SDService.GetPregnantInfosToCreateEnquiries();
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(pregnantInfo.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m302,0623,模拟-获取待更新的`问询病史`", () =>
            {
                var userInfo = SDService.UserInfo;
                SDService.TempPregnantInfos = SDService.GetPregnantInfosToUpdateEnquiries();
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(pregnantInfo.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m303,0623,模拟-获取李丽", () =>
            {
                var userInfo = SDService.UserInfo;
                SDService.TempPregnantInfos = SDService.GetPregnantInfosForTest();
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(pregnantInfo.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Test-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m31,0623,模拟-新增`问询病史`", () =>
            {
                //
                var pregnanthistorys2 = @"[{ ""index"":""0"",""pregstatus"":""人流"",""babysex"":""0"",""babyweight"":"""",""pregnantage"":""2017年6月""},{ ""index"":""2"",""pregstatus"":""顺产-足月-健,足月产-亡,巨大胎,顺产-早产-健,早产-亡"",""babysex"":"""",""babyweight"":"""",""pregnantage"":""""}]".FromJson<List<pregnanthistory>>();


                var userInfo = SDService.UserInfo;
                var dbContext = DBHelper.GetSqlDbContext(SDService.ConntectingStringSD);
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    var serviceResult = dbContext.DelegateTransaction((Func<DbGroup, bool>)((group) =>
                    {
                        var syncForFS = new SyncOrder()
                        {
                            TargetType = TargetType.HistoryEnquiry,
                            SourceId = pregnantInfo.Id.ToString(),
                            SyncStatus = SyncStatus.Success
                        };
                        try
                        {
                            syncForFS.SyncTime = DateTime.Now;
                            var base8 = SDService.GetBase8(userInfo, pregnantInfo.idcard, ref sb);
                            if (!base8.IsAvailable)
                            {
                                syncForFS.SyncStatus = SyncStatus.Error;
                                syncForFS.ErrorMessage = "No Base8 Data";
                                SDService.SaveSyncOrder(dbContext.DbGroup, syncForFS);
                                return (bool)true;
                            }
                            var enquiryResponse = SDService.GetEnquiry(userInfo, base8, ref sb);
                            var enquiryData = enquiryResponse.data.FirstOrDefault();
                            var result = SDService.PostUpdateEnquiryToFS(pregnantInfo, enquiryData, userInfo, base8, ref sb);
                            if (!result.Contains((string)"处理成功"))
                            {
                                throw new NotImplementedException(result);
                            }
                            //[{ "index":"0","pregstatus":"人流","babysex":"0","babyweight":"","pregnantage":"2017年6月"},{ "index":"2","pregstatus":"顺产-足月-健,足月产-亡,巨大胎,顺产-早产-健,早产-亡","babysex":"","babyweight":"","pregnantage":""}]
                            var pregnanthistorys = pregnantInfo.pregnanthistory?.FromJson<List<pregnanthistory>>();
                            //孕次排序
                            pregnanthistorys = pregnanthistorys.OrderBy(c => c.pregnantage).ToList();
                            foreach (var pregnanthistory in pregnanthistorys)
                            {
                                pregnanthistory.PregnantageIndex = pregnanthistorys.IndexOf(pregnanthistory) + 1;
                            }
                            var enquiryPregnanthResponse = SDService.GetEnquiryPregnanths(userInfo, base8, ref sb);
                            //新增
                            var toAddHistories = pregnanthistorys.Where(c => enquiryPregnanthResponse.data.FirstOrDefault(d => d.IssueDate == c.pregnantage) == null).OrderBy(c => c.pregnantage);
                            foreach (var toAddHistory in toAddHistories)
                            {
                                var toAdd = new WMH_CQBJ_CQJC_PRE_SAVE();
                                toAdd.UpdateEnquiry(pregnantInfo, toAddHistory);
                                toAdd._state = "added";
                                if (toAdd.Validate(ref sb))
                                {
                                    result = SDService.PostAddEnquiryPregnanth(toAdd, userInfo, base8, ref sb);
                                }
                                if (!result.Contains((string)"处理成功"))
                                {
                                    throw new NotImplementedException(result);
                                }
                            }
                            foreach (var enquiryPregnanth in enquiryPregnanthResponse.data)
                            {
                                var toChange = new WMH_CQBJ_CQJC_PRE_SAVE(enquiryPregnanth);
                                var pregnanthistory = pregnanthistorys.FirstOrDefault(c => c.pregnantage == enquiryPregnanth.IssueDate);
                                if (pregnanthistory == null)
                                {
                                    //删除
                                    result = SDService.DeleteEnquiryPregnanth(toChange, userInfo, base8, ref sb);
                                    if (!result.Contains((string)"处理成功"))
                                    {
                                        throw new NotImplementedException(result);
                                    }
                                    continue;
                                }
                                //更改
                                toChange.UpdateEnquiry(pregnantInfo, pregnanthistory);
                                toChange._state = "modified";
                                if (toChange.Validate(ref sb))
                                {
                                    result = SDService.UpdateEnquiryPregnanth(toChange, userInfo, base8, ref sb);
                                }
                                if (!result.Contains((string)"处理成功"))
                                {
                                    throw new NotImplementedException(result);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            syncForFS.SyncStatus = SyncStatus.Error;
                            syncForFS.ErrorMessage = ex.ToString();
                        }
                        syncForFS.Id = SDService.SaveSyncOrder(dbContext.DbGroup, syncForFS);
                        sb.AppendLine((string)syncForFS.ToJson());
                        return (bool)(syncForFS.SyncStatus != SyncStatus.Error);
                    }));
                    if (!serviceResult.IsSuccess)
                    {
                        sb.Append(serviceResult.Messages);
                    }
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m32,0623,模拟-更新`问询病史`", () =>
            {
                var userInfo = SDService.UserInfo;
                var dbContext = DBHelper.GetSqlDbContext(SDService.ConntectingStringSD);
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    var serviceResult = dbContext.DelegateTransaction((Func<DbGroup, bool>)((group) =>
                    {
                        var syncForFS = SDService.GetSyncOrderBySource(group, TargetType.HistoryEnquiry, pregnantInfo.Id.ToString()).First();
                        try
                        {
                            syncForFS.SyncTime = DateTime.Now;
                            var base8 = SDService.GetBase8(userInfo, pregnantInfo.idcard, ref sb);
                            if (!base8.IsAvailable)
                            {
                                syncForFS.SyncStatus = SyncStatus.Error;
                                syncForFS.ErrorMessage = "No Base8 Data";
                                SDService.SaveSyncOrder(dbContext.DbGroup, syncForFS);
                                return (bool)true;
                            }
                            var enquiryResponse = SDService.GetEnquiry(userInfo, base8, ref sb);
                            var enquiryData = enquiryResponse.data.FirstOrDefault();
                            var result = SDService.PostUpdateEnquiryToFS(pregnantInfo, enquiryData, userInfo, base8, ref sb);
                            if (!result.Contains((string)"处理成功"))
                            {
                                throw new NotImplementedException(result);
                            }
                            //[{ "index":"0","pregstatus":"人流","babysex":"0","babyweight":"","pregnantage":"2017年6月"},{ "index":"2","pregstatus":"顺产-足月-健,足月产-亡,巨大胎,顺产-早产-健,早产-亡","babysex":"","babyweight":"","pregnantage":""}]
                            var pregnanthistorys = pregnantInfo.pregnanthistory?.FromJson<List<pregnanthistory>>();
                            var enquiryPregnanthResponse = SDService.GetEnquiryPregnanths(userInfo, base8, ref sb);
                            //新增
                            var toAddHistories = pregnanthistorys.Where(c => enquiryPregnanthResponse.data.FirstOrDefault(d => d.IssueDate == c.pregnantage) == null);
                            foreach (var toAddHistory in toAddHistories)
                            {
                                var toAdd = new WMH_CQBJ_CQJC_PRE_SAVE();
                                toAdd.UpdateEnquiry(pregnantInfo, toAddHistory);
                                toAdd._state = "added";
                                if (toAdd.Validate(ref sb))
                                {
                                    result = SDService.PostAddEnquiryPregnanth(toAdd, userInfo, base8, ref sb);
                                }
                                if (!result.Contains((string)"处理成功"))
                                {
                                    throw new NotImplementedException(result);
                                }
                            }
                            foreach (var enquiryPregnanth in enquiryPregnanthResponse.data)
                            {
                                var toChange = new WMH_CQBJ_CQJC_PRE_SAVE(enquiryPregnanth);
                                var pregnanthistory = pregnanthistorys.FirstOrDefault(c => c.pregnantage == enquiryPregnanth.IssueDate);
                                if (pregnanthistory == null)
                                {
                                    //删除
                                    result = SDService.DeleteEnquiryPregnanth(toChange, userInfo, base8, ref sb);
                                    if (!result.Contains((string)"处理成功"))
                                    {
                                        throw new NotImplementedException(result);
                                    }
                                    continue;
                                }
                                //更改
                                toChange.UpdateEnquiry(pregnantInfo, pregnanthistory);
                                toChange._state = "modified";
                                if (toChange.Validate(ref sb))
                                {
                                    result = SDService.UpdateEnquiryPregnanth(toChange, userInfo, base8, ref sb);
                                }
                                if (!result.Contains((string)"处理成功"))
                                {
                                    throw new NotImplementedException(result);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            syncForFS.SyncStatus = SyncStatus.Error;
                            syncForFS.ErrorMessage = ex.ToString();
                        }
                        SDService.SaveSyncOrder(dbContext.DbGroup, syncForFS);
                        sb.AppendLine((string)syncForFS.ToJson());
                        return (bool)(syncForFS.SyncStatus != SyncStatus.Error);
                    }));
                    if (!serviceResult.IsSuccess)
                    {
                        sb.Append(serviceResult.Messages);
                    }
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            List<PhysicalExaminationModel> examinations = new List<PhysicalExaminationModel>();
            cmds.Add(new Command("m401,0628,模拟-获取待新增的`体格检查`", () =>
            {
                var userInfo = SDService.UserInfo;
                examinations = SDService.GetPhysicalExaminationDatasForCreatePhysicalExaminations();
                foreach (var examination in examinations)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(examination.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), examination.pi_personname + "_" + examination.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m402,0628,模拟-获取待更新的`体格检查`", () =>
            {
                var userInfo = SDService.UserInfo;
                examinations = SDService.GetPhysicalExaminationDatasForUpdatePhysicalExaminations();
                foreach (var examination in examinations)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(examination.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), examination.pi_personname + "_" + examination.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m403,0628,模拟-获取李丽", () =>
            {
                var userInfo = SDService.UserInfo;
                SDService.TempPregnantInfos = SDService.GetPregnantInfosForTest();
                foreach (var pregnantInfo in SDService.TempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(pregnantInfo.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Test-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            #region 体格检查
            //cmds.Add(new Command("m41,0628,模拟-新增`体格检查`", () =>
            //{
            //    var userInfo = SDBLL.UserInfo;
            //    var dbContext = DBHelper.GetDbContext(SDBLL.ConntectingStringSD);
            //    foreach (var examination in examinations)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        var serviceResult = context.DelegateTransaction((Func<DbGroup, bool>)((group) =>
            //        {
            //            var syncForFS = new SyncOrder()
            //            {
            //                TargetType = TargetType.PhysicalExamination,
            //                SourceId = examination.Id.ToString(),
            //                SyncStatus = SyncStatus.Success
            //            };
            //            try
            //            {
            //                syncForFS.SyncTime = DateTime.Now;
            //                //获取Base8信息
            //                var base8 = SDBLL.GetBase8(userInfo, examination.idcard, ref sb);
            //                if (!base8.IsAvailable)
            //                {
            //                    syncForFS.SyncStatus = SyncStatus.Error;
            //                    syncForFS.ErrorMessage = "No Base8 Data";
            //                    SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //                    return (bool)true;
            //                }
            //                //获取体格检查
            //                var physicalExaminationId = SDBLL.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref sb);
            //                if (!string.IsNullOrEmpty(physicalExaminationId))
            //                {
            //                    syncForFS.SyncStatus = SyncStatus.Existed;
            //                    SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //                    return (bool)true;
            //                }
            //                //新建体格检查
            //                var datas = new List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data>();
            //                var data = new WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data();
            //                UpdateExamination(examination, data);
            //                datas.Add(data);
            //                var result = SDBLL.CreatePhysicalExamination(datas, userInfo, base8, ref sb);
            //                if (!result.Contains("处理成功"))
            //                {
            //                    syncForFS.SyncStatus = SyncStatus.Error;
            //                    SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //                    return (bool)true;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                syncForFS.SyncStatus = SyncStatus.Error;
            //                syncForFS.ErrorMessage = ex.ToString();
            //                sb.Append(ex.ToString());
            //            }
            //            syncForFS.Id = SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //            sb.AppendLine((string)syncForFS.ToJson());
            //            return (bool)(syncForFS.SyncStatus != SyncStatus.Error);
            //        }));
            //        if (!serviceResult.IsSuccess)
            //        {
            //            sb.Append(serviceResult.Messages);
            //        }
            //        var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), examination.pi_personname + "_" + examination.idcard + ".txt");
            //        File.WriteAllText(file, sb.ToString());
            //        Console.WriteLine($"result:{file}");
            //    }
            //}));
            //cmds.Add(new Command("m42,0628,模拟-更新`体格检查`", () =>
            //{
            //    var userInfo = SDBLL.UserInfo;
            //    var dbContext = DBHelper.GetDbContext(SDBLL.ConntectingStringSD);
            //    foreach (var examination in examinations)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        var serviceResult = context.DelegateTransaction((Func<DbGroup, bool>)((group) =>
            //        {
            //            var syncForFS = SDBLL.GetSyncOrder((DbGroup)context.DbGroup, (TargetType)TargetType.PhysicalExamination, examination.Id.ToString());
            //            try
            //            {
            //                syncForFS.SyncTime = DateTime.Now;
            //                //获取Base8信息
            //                var base8 = SDBLL.GetBase8(userInfo, examination.idcard, ref sb);
            //                if (!base8.IsAvailable)
            //                {
            //                    syncForFS.SyncStatus = SyncStatus.Error;
            //                    syncForFS.ErrorMessage = "No Base8 Data";
            //                    SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //                    return (bool)true;
            //                }
            //                //获取体格检查
            //                var physicalExaminationId = SDBLL.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref sb);
            //                if (string.IsNullOrEmpty(physicalExaminationId))
            //                {
            //                    syncForFS.SyncStatus = SyncStatus.NotExisted;
            //                    SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //                    return (bool)true;
            //                }
            //                var physicalExamination = SDBLL.GetPhysicalExamination(physicalExaminationId,userInfo, base8, DateTime.Now, ref sb);
            //                if (physicalExamination == null)
            //                {
            //                    syncForFS.SyncStatus = SyncStatus.NotExisted;
            //                    SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //                    return (bool)true;
            //                }
            //                //更新体格检查
            //                var datas = new List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data>();
            //                var data = new WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data(physicalExamination);
            //                UpdateExamination(examination, data);
            //                datas.Add(data);
            //                var result = SDBLL.UpdatePhysicalExamination(physicalExaminationId,datas, userInfo, base8, ref sb);
            //                if (!result.Contains("处理成功"))
            //                {
            //                    syncForFS.SyncStatus = SyncStatus.Error;
            //                    SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //                    return (bool)true;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                syncForFS.SyncStatus = SyncStatus.Error;
            //                syncForFS.ErrorMessage = ex.ToString();
            //                sb.Append(ex.ToString());
            //            }
            //            syncForFS.Id = SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
            //            sb.AppendLine((string)syncForFS.ToJson());
            //            return (bool)(syncForFS.SyncStatus != SyncStatus.Error);
            //        }));
            //        if (!serviceResult.IsSuccess)
            //        {
            //            sb.Append(serviceResult.Messages);
            //        }
            //        var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), examination.pi_personname + "_" + examination.idcard + ".txt");
            //        File.WriteAllText(file, sb.ToString());
            //        Console.WriteLine($"result:{file}");
            //    }
            //})); 
            #endregion
            var context = new ServiceContext();
            var syncTask_Create_PhysicalExaminationModel = new ProfessionalExaminationModel_SyncTask_Create(context);
            var syncTask_Update_PhysicalExaminationModel = new ProfessionalExaminationModel_SyncTask_Update(context);
            var sourceDatas = new List<ProfessionalExaminationModel_SourceData>();
            cmds.Add(new Command("m501,0701,模拟-获取待新增的`专科检查`", () =>
            {
                var userInfo = SDService.UserInfo;
                sourceDatas = syncTask_Create_PhysicalExaminationModel.GetSourceDatas(userInfo);
                foreach (var sourceData in sourceDatas)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-专科检查" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m502,0703,模拟-获取待更新的`专科检查`", () =>
            {
                var userInfo = SDService.UserInfo;
                sourceDatas = syncTask_Update_PhysicalExaminationModel.GetSourceDatas(userInfo);
                foreach (var sourceData in sourceDatas)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-专科检查" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
            }));
            cmds.Add(new Command("m51,0701,模拟-新增`专科检查`", () =>
            {
                //syncTask_Create_PhysicalExaminationModel.DoLogOnGetSource = (sourceData) =>
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendLine(sourceData.ToJson());
                //    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Create-专科检查" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                //    File.WriteAllText(file, sb.ToString());
                //    Console.WriteLine($"result:{file}");
                //};
                //syncTask_Create_PhysicalExaminationModel.DoLogOnWork = (sourceData, sb) =>
                //{
                //    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                //    File.WriteAllText(file, sb.ToString());
                //    Console.WriteLine($"result:{file}");
                //};
                ////执行
                //var userInfo = SDBLL.UserInfo;
                //foreach (var sourceData in sourceDatas)
                //{
                //    syncTask_Create_PhysicalExaminationModel.DoLogOnGetSource?.Invoke(sourceData);
                //    syncTask_Create_PhysicalExaminationModel.DoWork(serviceContext, userInfo, sourceData);
                //}
            }));
            cmds.Add(new Command("m52,0703,模拟-更新`专科检查`", () =>
            {
                //syncTask_Update_PhysicalExaminationModel.DoLogOnGetSource = (sourceData) =>
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendLine(sourceData.ToJson());
                //    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Update-专科检查" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                //    File.WriteAllText(file, sb.ToString());
                //    Console.WriteLine($"result:{file}");
                //};
                //syncTask_Update_PhysicalExaminationModel.DoLogOnWork = (sourceData, sb) =>
                //{
                //    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                //    File.WriteAllText(file, sb.ToString());
                //    Console.WriteLine($"result:{file}");
                //};
                ////执行
                //var userInfo = SDBLL.UserInfo;
                //foreach (var sourceData in sourceDatas)
                //{
                //    syncTask_Update_PhysicalExaminationModel.DoLogOnGetSource?.Invoke(sourceData);
                //    syncTask_Update_PhysicalExaminationModel.DoWork(serviceContext, userInfo, sourceData);
                //}
            }));
            cmds.Add(new Command("m91,0622,自动同步-新增`孕妇档案`", () =>
            {
                var dbContext = new ServiceContext();
                var syncTask = new PregnantInfo_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m92,0623,自动同步-更新`孕妇档案`", () =>
            {
                var dbContext = new ServiceContext();
                var syncTask = new PregnantInfo_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m93,0628,自动同步-新增`问询病史`", () =>
            {
                var dbContext = new ServiceContext();
                var syncTask = new Enquiry_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m94,0629,自动同步-更新`问询病史`", () =>
            {
                var dbContext = new ServiceContext();
                var syncTask = new Enquiry_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m95,0630,自动同步-新增`体格检查`", () =>
            {
                var dbContext = new ServiceContext();
                var syncTask = new PhysicalExaminationModel_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m96,0630,自动同步-更新`体格检查`", () =>
            {
                var dbContext = new ServiceContext();
                var syncTask = new PhysicalExaminationModel_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m97,0630,自动同步-新增`专科检查`", () =>
            {
                var dbContext = new ServiceContext();
                var syncTask = new ProfessionalExaminationModel_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m98,0703,自动同步-更新`专科检查`", () =>
            {
                var dbContext = new ServiceContext();
                var syncTask = new ProfessionalExaminationModel_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m99,0703,自动同步-扩展范围`孕妇档案`", () =>
            {
                var syncTask = new PregnantInfo_SyncTask_CreateOrUpdate(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-CreateOrUpdate-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\CreateOrUpdate-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                //自动执行
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m101,0702,行政区", () =>
            {
                List<string> errorData = new List<string>();
                List<cnarea> datas = new List<cnarea>();
                List<cnarea> errorDatas = new List<cnarea>();
                try
                {
                    using (var fileStream = File.OpenRead(@"C:\Users\vlong\Desktop\cnarea20191031.sql"))
                    {
                        using (StreamReader sr = new StreamReader(fileStream))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line.StartsWith("("))
                                {
                                    //(1,0,0,110000000000,000000,'','北京市','北京','北京','BeiJing',116.407526,39.904030),
                                    //var values = line.Trim().TrimStart("(").TrimEnd("),").Split(',');
                                    //if (values.Length == 12)

                                    var text = line.Trim().TrimStart("(").TrimEnd("),");
                                    var intP = @"(\w+)";
                                    var varcharP = @"'([\w\,\、\（\）\〇\(\)\?\·氹\s\\\'\.\’]*)'";
                                    //var varcharP = @"'.*'";
                                    var decimalP = @"([\w\.]+)";
                                    var pattern = $@"{intP},{intP},{intP},{intP},{intP},{varcharP},{varcharP},{varcharP},{varcharP},{varcharP},{decimalP},{decimalP}";
                                    var regex = new Regex(pattern);
                                    var match = regex.Match(text);
                                    if (match.Groups.Count == 13)
                                    {
                                        datas.Add(new cnarea()
                                        {
                                            id = match.Groups[1].ToString(),
                                            level = match.Groups[2].ToString(),
                                            parent_code = match.Groups[3].ToString(),
                                            area_code = match.Groups[4].ToString(),
                                            zip_code = match.Groups[5].ToString(),
                                            city_code = match.Groups[6].ToString(),
                                            name = match.Groups[7].ToString(),
                                            short_name = match.Groups[8].ToString(),
                                            merger_name = match.Groups[9].ToString(),
                                            pinyin = match.Groups[10].ToString(),
                                            lng = match.Groups[11].ToString(),
                                            lat = match.Groups[12].ToString(),
                                        });
                                        //break;
                                    }
                                    else
                                    {
                                        errorData.Add(line);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("数据库连接失败," + ex.ToString());
                }

                var connectingString = HeleInnerMSSQL;
                using (var connection = new SqlConnection(connectingString))
                {
                    connection.Open();
                    foreach (var data in datas)
                    {
                        try
                        {
                            var id = connection.Execute("insert  into cnarea(id,level,parent_code,area_code,zip_code,city_code,name,short_name,merger_name,pinyin,lng,lat) values(@id,@level,@parent_code,@area_code,@zip_code,@city_code,@name,@short_name,@merger_name,@pinyin,@lng,@lat)", data);
                        }
                        catch (Exception ex)
                        {
                            data.message = ex.ToString();
                            errorDatas.Add(data);
                        }
                    }
                    connection.Close();
                }

                var errors = string.Join("\r\n", errorDatas.Select(c => c.ToJson()));
                File.WriteAllText(@"D:\errorData.txt", errors);
            }));
            cmds.Add(new Command("m102,0703,DES", () =>
            {
                //public key
                //"exponent":"010001"
                //"modulus":"00af8dfa5a14e97e58cac7238a5d4ca89478cedcfd196ea643735d64c74df659cd259c8bd60ec046c4d3f6dec3965dc0351f117f8a0ae62ad61c3a41d38c6a93215025c658587f4aa7ceaa9ed08c2ced8873254c417a77403aff9a0abb3bc1d2ff42f856e1a4d447ed0a1626e1099f304b6602e69cdca1a376ae6bf0dad13844cf"
                //密码
                //123
                //加密结果
                //2d36cfe9d49ccdb6cd313c75a7f4308036092f701a068f7fa66ab1835cd03baa3cbc80191e3bf502453d0cacec215a51adcfb883aa24ecc09025b6dc68d9cca20c722dc3e766e92fb15103b434a6c5fc640bbf7937f016c63a11ecad72018a30b0800a67f21d57f6014057f49c29595e7c3f9e5d1874e109a8e9c37be46ce59b

                var str = File.ReadAllText(@"C:\Users\vlong\Documents\WeChat Files\xialiu638\FileStorage\File\2020-07\加密前.txt");
                var sKey = "ABC123AB";

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
                des.Mode = CipherMode.ECB;
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);// 密匙
                //des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);// 初始化向量
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                var retA = ms.ToArray();
                var retB = Convert.ToBase64String(retA);
            }));
            cmds.Add(new Command("m110,0722,自动同步-所有任务", () =>
            {
                Console.WriteLine($"任务启动=>孕妇档案-新建");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new PregnantInfo_SyncTask_Create(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>孕妇档案-更新");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new PregnantInfo_SyncTask_Update(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>问询病史-新建");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new Enquiry_SyncTask_Create(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>问询病史-更新");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new Enquiry_SyncTask_Update(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>体格检查-新建");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new PhysicalExaminationModel_SyncTask_Create(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>体格检查-更新");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new PhysicalExaminationModel_SyncTask_Update(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>专科检查-新建");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new ProfessionalExaminationModel_SyncTask_Create(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>专科检查-更新");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new ProfessionalExaminationModel_SyncTask_Update(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>孕妇出院-新增");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new PregnantDischarge_SyncTask_Create(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>孕妇出院-更新");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new PregnantDischarge_SyncTask_Update(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.inp_no + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.inp_no + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>婴儿出院-新增");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new ChildDischarge_SyncTask_Create(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

                Console.WriteLine($"任务启动=>婴儿出院-更新");
                Task.Factory.StartNew(() =>
                {
                    var syncTask = new ChildDischarge_SyncTask_Update(context);
                    syncTask.DoLogOnGetSource = (sourceData) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(sourceData.ToJson());
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.DoLogOnWork = (sourceData, sb) =>
                    {
                        var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                        File.WriteAllText(file, sb.ToString());
                        Console.WriteLine($"result:{file}");
                    };
                    syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
                });

            }));
            cmds.Add(new Command("m111,0721,自动同步-新增`孕妇出院`", () =>
            {
                var syncTask = new PregnantDischarge_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m112,0721,自动同步-更新`孕妇出院`", () =>
            {
                var syncTask = new PregnantDischarge_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m113,0722,自动同步-新增`婴儿出院`", () =>
            {
                var syncTask = new ChildDischarge_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m114,0722,自动同步-更新`婴儿出院`", () =>
            {
                var syncTask = new ChildDischarge_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            cmds.Add(new Command("m115,0723,自动同步-新增`孕妇档案`强制重试", () =>
            {
                var syncTask = new PregnantInfo_SyncTask_Create(new ServiceContext());
                syncTask.RetryAmount = 100;
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDService.UserInfo);
            }));
            #endregion
            #region 国健,产前记录解析
            cmds.Add(new Command("gj1,产前记录解析", () =>
            {
                var str = File.ReadAllText(@"C:\Users\vlong\Desktop\新建文本文档.html", Encoding.GetEncoding("GBK"));
                StringBuilder sb = new StringBuilder();
                PreDeliveryParser.GetPreDeliveries(str, ref sb);
            }));
            #endregion
            #region 科研,数据初始化
            cmds.Add(new Command("pre01,prepare01,产时信息", () =>
            {
                var textPattern = @"""?([\w+\.<&;>\s-：:，,=%/]*)""?";
                var ss = "\t\t\t";
                var mm = new Regex($"{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}").Match(ss);
                var mmCount = mm.Groups.Count;
                var path = @"D:\WorkSpace\Docs\05.科研管理\赵毅超数据\产时信息.txt";
                var lines = File.ReadAllLines(path);
                Regex regex = new Regex($@"{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}");
                for (int i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        //母亲身份证	出生体重	xingbiedm	taifangwdm	fenmianfsdm	apgar5	apgar10	分娩日期	chuxuel	huiyinqkdm
                        //"110102198302070064"    "3620"  "1" "01"    "1" "9" "0"         "4/4/2019 00:00:00" "0" "1"
                        var line = lines[i];
                        var match = regex.Match(line);
                        if (match.Groups.Count == 13)
                        {
                            var delivery = new Delivery();
                            var colIndex = 1;
                            delivery.Idcard = match.Groups[colIndex].ToString(); colIndex++;
                            delivery.WeightOnBirth = match.Groups[colIndex].ToString(); colIndex++;
                            delivery.xingbiedm = match.Groups[colIndex].ToString(); colIndex++;
                            delivery.taifangwdm = match.Groups[colIndex].ToString(); colIndex++;
                            delivery.fenmianfsdm = match.Groups[colIndex].ToString(); colIndex++;
                            delivery.apgar5 = match.Groups[colIndex].ToString(); colIndex++;
                            delivery.apgar10 = match.Groups[colIndex].ToString(); colIndex++;
                            colIndex++;
                            colIndex++;
                            delivery.DeliveryDate = match.Groups[colIndex].ToDateTime(); colIndex++;
                            delivery.chuxuel = match.Groups[colIndex].ToString(); colIndex++;
                            delivery.huiyinqkdm = match.Groups[colIndex].ToString(); colIndex++;
                            if (delivery.DeliveryDate == null)
                                continue;
                            var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                            var serviceResult = dbContext.DelegateTransaction((group) =>
                            {
                                DeliveryRepository repository = new DeliveryRepository(dbContext);
                                delivery.Id = repository.Insert(delivery);
                                return delivery.Id;
                            });
                        }
                        else
                        {
                            var isUnmatched = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = ex.ToString();
                    }
                }
            }));
            cmds.Add(new Command("pre02,prepare02,B超数据", () =>
            {
                var textPattern = @"""?([\w+\.<&;>\s-：:，,=%/]*)""?";
                var ss = "\t\t\t";
                var mm = new Regex($"{textPattern}\t{textPattern}\t{textPattern}\t{textPattern}").Match(ss);
                var mmCount = mm.Groups.Count;
                //建表
                if (false)
                {
                    var fields = "idcard,chushengyz,bcheckweek,jcdate,age,height,preweight,parity,gravidity,bloodtype,rhbloodtype,education,dateofprenatal,sbp,dbp,preterm,mociyuejing,pluse,tailin,yunnanga,yunnangb,yunnangc,taiwei,taixin,gr,yangshui,jidongm,nt,dingtunc,shuangdingj,touwei,guguc,taierfw,fmfs,fmfsdm,fmdate,sex,tz";
                    var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                    var serviceResult = dbContext.DelegateTransaction((group) =>
                    {
                        group.Connection.Execute(DBHelper.GetCreateTableSQL("BCheck", fields, ','), transaction: group.Transaction);
                        return true;
                    });
                }
                //导数据
                var path = @"D:\WorkSpace\Docs\05.科研管理\赵毅超数据\B超数据.txt";
                var lines = File.ReadAllLines(path);
                //var patten = $"\"?{textPattern}\"?";
                //for (int i = 0; i < 38; i++)
                //    patten += $@"\t""?{ textPattern}""?";
                //Regex regex = new Regex(patten);
                for (int i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        //idcard chushengyz  bcheckweek jcdate  age height  preweight parity  gravidity bloodtype   rhbloodtype education   dateofprenatal sbp dbp preterm mociyuejing pluse   tailin yunnanga    yunnangb yunnangc    taiwei taixin  gr yangshui    jidongm nt  dingtunc shuangdingj touwei guguc   taierfw fmfs    fmfsdm fmdate  sex tz
                        //"\"110101198207073041\"\t\"40\"\t\"25\"\t\"21/11/2017\"\t\"36\"\t\"160\"\t\"60\"\t\"0\"\t\"2\"\t\"0\"\t\"2\"\t\"20\"\t\"8/3/2018\"\t\"113\"\t\"73\"\t\"0\"\t\"1/6/2017\"\t\"84\"\t\"280\"\t\t\t\t\"LSA\"\t\"143\"\t\"0级 \"\t\t\"2.8\"\t\t\t\"5.9\"\t\"22.4\"\t\"4.2\"\t\"20.2\"\t\"阴道自然分娩\"\t\"1\"\t\"11/3/2018\"\t\"1\"\t\"3490\""
                        var line = lines[i];
                        var match = line.Split('\t').Select(c => c.Trim("\"")).ToList();
                        if (match.Count == 38)
                        {
                            var entity = new BCheck();
                            var colIndex = 0;
                            entity.idcard = match[colIndex].ToString(); colIndex++;
                            entity.chushengyz = match[colIndex].ToString(); colIndex++;
                            entity.bcheckweek = match[colIndex].ToString(); colIndex++;
                            entity.jcdate = match[colIndex].ToString(); colIndex++;
                            entity.age = match[colIndex].ToString(); colIndex++;
                            entity.height = match[colIndex].ToString(); colIndex++;
                            entity.preweight = match[colIndex].ToString(); colIndex++;
                            entity.parity = match[colIndex].ToString(); colIndex++;
                            entity.gravidity = match[colIndex].ToString(); colIndex++;
                            entity.bloodtype = match[colIndex].ToString(); colIndex++;
                            entity.rhbloodtype = match[colIndex].ToString(); colIndex++;
                            entity.education = match[colIndex].ToString(); colIndex++;
                            entity.dateofprenatal = match[colIndex].ToString().ToDateTime(); colIndex++;
                            entity.sbp = match[colIndex].ToString(); colIndex++;
                            entity.dbp = match[colIndex].ToString(); colIndex++;
                            entity.preterm = match[colIndex].ToString(); colIndex++;
                            entity.mociyuejing = match[colIndex].ToString(); colIndex++;
                            entity.pluse = match[colIndex].ToString(); colIndex++;
                            entity.tailin = match[colIndex].ToString(); colIndex++;
                            entity.yunnanga = match[colIndex].ToString(); colIndex++;
                            entity.yunnangb = match[colIndex].ToString(); colIndex++;
                            entity.yunnangc = match[colIndex].ToString(); colIndex++;
                            entity.taiwei = match[colIndex].ToString(); colIndex++;
                            entity.taixin = match[colIndex].ToString(); colIndex++;
                            entity.gr = match[colIndex].ToString(); colIndex++;
                            entity.yangshui = match[colIndex].ToString(); colIndex++;
                            entity.jidongm = match[colIndex].ToString(); colIndex++;
                            entity.nt = match[colIndex].ToString(); colIndex++;
                            entity.dingtunc = match[colIndex].ToString(); colIndex++;
                            entity.shuangdingj = match[colIndex].ToString(); colIndex++;
                            entity.touwei = match[colIndex].ToString(); colIndex++;
                            entity.guguc = match[colIndex].ToString(); colIndex++;
                            entity.taierfw = match[colIndex].ToString(); colIndex++;
                            entity.fmfs = match[colIndex].ToString(); colIndex++;
                            entity.fmfsdm = match[colIndex].ToString(); colIndex++;
                            entity.fmdate = match[colIndex].ToString().ToDateTime(); colIndex++;
                            entity.sex = match[colIndex].ToString(); colIndex++;
                            entity.tz = match[colIndex].ToString(); colIndex++;

                            if (entity.idcard == null)
                                continue;
                            var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                            var serviceResult = dbContext.DelegateTransaction((group) =>
                            {
                                BCheckRepository repository = new BCheckRepository(dbContext);
                                entity.Id = repository.Insert(entity);
                                return entity.Id;
                            });
                        }
                        else
                        {
                            var isUnmatched = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = ex.ToString();
                    }
                }
            }));
            cmds.Add(new Command("pre03,fixData,R_Check", () =>
            {
                TransformInfo trans = new TransformInfo() {
                    TableName = "R_Check"
                };
                trans.KeyFields = new List<string>() { 
                    "PatientId",
                    "CheckOrderId",
                    "InspectionId",
                };
                trans.Transforms = new List<TransfromBase >() { 
                    new SimpleTransfrom (){ 
                        From= "IssueTime",
                        To= "IssueTime2",
                        Type= SimpleTransformType.DateTime,
                    }
                };
                var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                var repository = new CommonRepository(dbContext);
                var data = dbContext.DelegateTransaction((group) =>
                {
                    return repository.GetDataTable(group, trans.GetSourceSQL());
                }).Data;
                //Multiple Update
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var row = data.Rows[i];
                    Console.WriteLine(i);
                    if (!trans.IsParamsValid(row))
                        continue;
                    var sql = trans.GetSQLToUpdate(row);
                    sb.AppendLine(sql);
                    if (i % 100 == 0 || i == data.Rows.Count - 1)
                    {
                        var serviceResult = dbContext.DelegateTransaction((group) =>
                        {
                            group.Connection.Execute(sb.ToString(), transaction: group.Transaction);
                            return true;
                        });
                        sb = new StringBuilder();
                    }
                }
                //Single Update
                //foreach (DataRow row in data.Rows)
                //{
                //    var pars = trans.GetParams(row);
                //    Console.WriteLine(pars.ToJson());
                //    if (!trans.IsParamsValid(row))
                //        continue;
                //    var sql = trans.GetSQLToUpdate(row);
                //    group.Connection.Execute(sql, pars, group.Transaction);
                //}
            }));
            cmds.Add(new Command("pre04,fixData,R_Patient", () =>
            {
                TransformInfo trans = new TransformInfo()
                {
                    TableName = "R_Patient"
                };
                trans.KeyFields = new List<string>() {
                    "idcard",
                    "lastmenstrualperiod",
                };
                trans.Transforms = new List<TransfromBase >() {
                    new SimpleTransfrom (){
                        From= "lastmenstrualperiod",
                        To= "lastmenstrualperiod2",
                        Type= SimpleTransformType.Date,
                    }
                };
                var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                var repository = new CommonRepository(dbContext);
                var data = dbContext.DelegateTransaction((group) =>
                {
                    return repository.GetDataTable(group, trans.GetSourceSQL());
                }).Data;
                //Multiple Update
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var row = data.Rows[i];
                    Console.WriteLine(i);
                    if (!trans.IsParamsValid(row))
                        continue;
                    var sql = trans.GetSQLToUpdate(row);
                    sb.AppendLine(sql);
                    if (i % 100 == 0 || i == data.Rows.Count - 1)
                    {
                        var serviceResult = dbContext.DelegateTransaction((group) =>
                        {
                            repository.Execute(group, sb.ToString());
                            return true;
                        });
                        sb = new StringBuilder();
                    }
                }
            }));
            cmds.Add(new Command("pre05,fixData,R_CheckOrder", () =>
            {
                TransformInfo trans = new TransformInfo()
                {
                    TableName = "R_CheckOrder"
                };
                trans.KeyFields = new List<string>() {
                    "patientId",
                    "checkorderid",
                };
                trans.Transforms = new List<TransfromBase>() {
                    new PregnantTransfrom (){
                        LastMenstrualPeriod= "lastmenstrualperiod",
                        DateToCheck = "IssueTime",
                        GestationalWeeks= "GestationalWeeks",
                        GestationalDays= "GestationalDays",
                        Type= PregnantTransformType.GestationalWeeksAndDay,
                    }
                };
                trans.SourceSQL = @"
select p.idcard,p.lastmenstrualperiod
,co.checkorderid,co.patientId,co.IssueTime 
from R_Patient p
left join R_PatientIds_Dis ids on ids.idcard = p.idcard
left join R_CheckOrder co on co.patientId = ids.patientId and p.lastmenstrualperiod < co.issuetime and dateadd(year,1,p.lastmenstrualperiod) > co.issuetime
where co.checkorderid is not null
";
                var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                var repository = new CommonRepository(dbContext);
                var data = dbContext.DelegateTransaction((group) =>
                {
                    return repository.GetDataTable(group, trans.GetSourceSQL());
                }).Data;
                //Multiple Update
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var row = data.Rows[i];
                    Console.WriteLine(i);
                    if (!trans.IsParamsValid(row))
                        continue;
                    var sql = trans.GetSQLToUpdate(row);
                    sb.AppendLine(sql);
                    if (i % 100 == 0 || i == data.Rows.Count - 1)
                    {
                        var serviceResult = dbContext.DelegateTransaction((group) =>
                        {
                            repository.Execute(group, sb.ToString());
                            return true;
                        });
                        sb = new StringBuilder();
                    }
                }
            }));
            cmds.Add(new Command("pre06,Combine", () =>
            {
                MergeInfo merge = new MergeInfo();
                merge.TableName = "R_CheckOrder_Value_Combined";
                merge.Fields = new List<string>()
                {
                    "T0246",
                    "T0150",
                    "T0157",
                    "T0303",
                    "T0251",
                    "T0307",
                    "T0160",
                    "T0308",
                    "T0167",
                    "T0170",
                    "T0184",
                    "T0153",
                    "T0183",
                    "T0266",
                    "T0243",
                    "T0249",
                    "T0250",
                    "T0195",
                    "T0155",
                    "T0148",
                    "T0166",
                    "T0149",
                    "T0258",
                    "T0247",
                    "T0175",
                    "T0272",
                    "T0165",
                    "T0154",
                    "T0298",
                    "T0186",
                    "T0159",
                    "T0248",
                    "T0200",
                    "T0296",
                    "T0168",
                    "T0193",
                    "T0245",
                    "T0174",
                    "T0178",
                    "T0169",
                };

                var dbContext = DBHelper.GetSqlDbContext(LocalMSSQL);
                var repository = new CommonRepository(dbContext);
                var data = dbContext.DelegateTransaction((group) =>
                {
                    return repository.GetDataTable(group, @"select distinct(idcard) from R_Patient");
                }).Data;


                //Multiple Update
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var idcardRow = data.Rows[i];
                    Console.WriteLine(i);
                    DataRow currentOrder = null;
                    var serviceResult = dbContext.DelegateTransaction((group) =>
                    {
                        var idcard = idcardRow.GetRowValue("idcard");
                        var orders = repository.GetDataTable(group, $"select * from R_CheckOrder_Value where idcard = {idcard.ToMSSQLValue()} order by issueTime");
                        var combinedOrders = orders.Clone();
                        var rootOrder = combinedOrders.NewRow();
                        if (orders.Rows.Count == 0)
                            return true;
                        rootOrder.ItemArray = orders.Rows[0].ItemArray;
                        for (int j = 1; j < orders.Rows.Count; j++)
                        {
                            var rootDays = rootOrder.GetRowValue("GestationalTotalDays").ToInt().Value;
                            currentOrder = orders.Rows[j];
                            var currentDays = currentOrder.GetRowValue("GestationalTotalDays").ToInt().Value;
                            if (Math.Abs(currentDays - rootDays) < 3)
                            {
                                //合并
                                bool isMerged = false;
                                foreach (var field in merge.Fields)
                                {
                                    if (rootOrder.GetRowValue(field).IsNullOrEmpty()
                                    && !currentOrder.GetRowValue(field).IsNullOrEmpty())
                                    {
                                        rootOrder[field] = currentOrder[field];
                                        isMerged = true;
                                    }
                                }
                                if (isMerged)
                                {
                                    var checkOrderIds = rootOrder.GetRowValue("checkOrderId") + "," + currentOrder.GetRowValue("checkOrderId");
                                    rootOrder["checkOrderId"] = checkOrderIds;
                                }
                            }
                            else
                            {
                                //更新总计
                                int sum = 0;
                                foreach (var field in merge.Fields)
                                {
                                    if (!rootOrder.GetRowValue(field).IsNullOrEmpty())
                                        sum++;
                                }
                                rootOrder["TSum"] = sum;
                                //保存
                                combinedOrders.Rows.Add(rootOrder);
                                //重置
                                if (j < orders.Rows.Count)
                                {
                                    rootOrder = combinedOrders.NewRow();
                                    if (j + 1 < orders.Rows.Count)
                                    {
                                        rootOrder.ItemArray = orders.Rows[j + 1].ItemArray;
                                    }
                                }
                            }
                        }
                        //repository.InsertDataTable(group, combinedOrders, merge.TableName);
                        //取集中24-27=169-189 //30218286
                        var rangeMin = 169;
                        var rangeMax = 189;
                        currentOrder = GetByConcentration(group, repository, combinedOrders, rangeMin, rangeMax);
                        repository.InsertDataRow(group, currentOrder, "R_CheckOrder_Value_Combined_24_27");
                        //取集中28_31=190-217 //10676071
                        rangeMin = 190;
                        rangeMax = 217;
                        currentOrder = GetByConcentration(group, repository, combinedOrders, rangeMin, rangeMax);
                        repository.InsertDataRow(group, currentOrder, "R_CheckOrder_Value_Combined_28_31");
                        //取最晚32+ //30218286
                        currentOrder = GetLast(group, repository, combinedOrders);
                        repository.InsertDataRow(group, currentOrder, "R_CheckOrder_Value_Combined_32");

                        return true;
                    });
                }
            }));
            #endregion
            #region 湖州,Profile格式化
            cmds.Add(new Command("pro01,Profile", () =>
            {
                var input = @"D:\WorkSpace\Repository\VLTest201912\FrameworkTest\Business\Profile\Input.txt";
                var output = @"D:\WorkSpace\Repository\VLTest201912\FrameworkTest\Business\Profile\" + DateTime.Now.ToString("HHmmss")+ ".txt";
                StringBuilder sb = new StringBuilder();
                var texts = File.ReadAllLines(input);
                for (int i = 0; i < texts.Count(); i++)
                {
                    var item = texts[i];
                    var sTemp = GetValue(item);
                    if (sTemp.StartsWith("select")
                    || sTemp.StartsWith("from")
                    || sTemp.StartsWith("where")
                    || sTemp.StartsWith("group by")
                    || sTemp.StartsWith("order by")
                    )
                    {
                        sb.AppendLine(sTemp);
                    }
                    else
                    {
                        if (sTemp.StartsWith(","))
                        {
                            sTemp += GetValue(texts[i + 1]);
                            i++;
                        }
                        if (sTemp.IsNullOrEmpty())
                        {
                            continue;
                        }
                        sb.AppendLine("\t" + sTemp);
                    }
                }
                File.WriteAllText(output, sb.ToString());
            }));
            #endregion
            #region 湖州,报表

            cmds.Add(new Command("hz01,HuZhou01,数据预处理Analysis_SpecialCase", () =>
            {
                var dbContext = DBHelper.GetSqlDbContext("");
                var serviceResult = dbContext.DelegateTransaction((group) =>
                {
                    //生成完整列表
                    var result = group.Connection.Execute(@"
insert into Analysis_SpecialCase ([ChildId], [GeneratedAtMonth], [IsValidCase])
select a.* from
(
	select g.ChildId,null as GeneratedAtMonth,0 as IsValidCase from cc_generalinfo g
	left join Analysis_SpecialCase a on g.childid = a.childid
	where a.childid is null and g.childId is not null
) as a
", transaction: group.Transaction);
                    //更新需要更新的数据


                    return result;
                });
            }));

            #endregion
            #region WebSercice
            cmds.Add(new Command("ws01,WebSercice01,多服务测试", () =>
            {
                while (true)
                {
                    try
                    {
                        var service01 = new Sample01WebService.Sample01WebServiceSoapClient();
                        var result = service01.HelloCommon();
                        Console.WriteLine($"service01:{result}");

                        var service02 = new Sample02WebService.Sample02WebServiceSoapClient();
                        result = service02.HelloCommon();
                        Console.WriteLine($"service02:{result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    System.Threading.Thread.Sleep(3000);
                }
            }));
            #endregion


            cmds.Start();
        }

        private static string GetValue(string item)
        {
            return item.Trim().Replace("''", "'");
        }

        private static DataRow GetByConcentration(DbGroup group, CommonRepository repository, DataTable combinedOrders, int rangeMin, int rangeMax)
        {
            DataRow currentOrder = null;
            var currentSum = 0;
            for (int j = 0; j < combinedOrders.Rows.Count; j++)
            {
                var currentRow = combinedOrders.Rows[j];
                var rangeValue = currentRow.GetRowValue("GestationalTotalDays").ToInt();
                if (rangeValue < rangeMin || rangeValue > rangeMax)
                    continue;
                var sum = currentRow.GetRowValue("TSum").ToInt();
                if (sum.HasValue && sum <= currentSum)
                    continue;
                currentSum = sum.Value;
                currentOrder = currentRow;
            }
            return currentOrder;
        }
        private static DataRow GetLast(DbGroup group, CommonRepository repository, DataTable combinedOrders)
        {
            DataRow currentOrder = null;
            var currentRange = 0;
            for (int j = 0; j < combinedOrders.Rows.Count; j++)
            {
                var currentRow = combinedOrders.Rows[j];
                var rangeValue = currentRow.GetRowValue("GestationalTotalDays").ToInt();
                if (rangeValue <= 224 | rangeValue < currentRange)
                    continue;
                currentRange = rangeValue.Value;
                currentOrder = currentRow;
            }
            return currentOrder;
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



