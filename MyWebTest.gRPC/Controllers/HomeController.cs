using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.TimeSpanSolution;
using Grpc.Core;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel.Description;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using VLService;

namespace MyWebTest.gRPC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        const string Server = "localhost";
        const int Port = 30051;
        public ActionResult Compare01()
        {
            //local
            var localService = new SampleService();
            var tLocal = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                   var result = localService.SayHello(new HelloRequest() { Name = i.ToString() });
                }
            });
            //WebService
            ServiceReference2.SampleWebServiceSoapClient webService = new ServiceReference2.SampleWebServiceSoapClient();
            var tWebService = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    try
                    {
                        var result = webService.HelloWorld(); // new ServiceReference2.HelloRequest() { Name = i.ToString() });
                    }
                    catch (Exception ex)
                    {
                        var error = ex.ToString();
                    }
                }
            });
            //WebAPI
            var container = new System.Net.CookieContainer();
            var tWebAPI = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = HttpHelper.Get("http://localhost:8017/api/values/" + i, "", ref container);
                }
            });
            //WebAPI
            var tWebAPIList = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = HttpHelper.Get("http://localhost:8017/api/values" , "", ref container);
                }
            });
            //gRPC
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            var client = new VLService01.VLService01Client(channel);
            var tRPC = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = client.SayHello(new HelloRequest { Name = i.ToString() });
                }
            });

            var summary = $@"local:{tLocal.TotalMilliseconds}
WebService:{tWebService.TotalMilliseconds}
WebAPI:{tWebAPI.TotalMilliseconds}
WebAPIList:{tWebAPIList.TotalMilliseconds}
tRPC:{tRPC.TotalMilliseconds}
";
        //千次 毫秒数 数据量极小值
        //local: 7.9977
        //WebService: 948.5253
        //WebAPI: 26639.0061
        //tRPC: 767.9921

        //千次 毫秒数 数据量极小值
        //local: 0.9968
        //WebService: 873
        //WebAPI: 1461.112
        //WebAPIList: 633.9954
        //tRPC: 1004.9978


            return View();
        }
        public ActionResult Compare10kb()
        {
            //local
            var localService = new SampleService();
            var tLocal = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = localService.TransTest10kb(new HelloRequest() { Name = i.ToString() });
                }
            });
            //WebService
            ServiceReference2.SampleWebServiceSoapClient webService = new ServiceReference2.SampleWebServiceSoapClient();
            var tWebService = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = webService.TransTest10kb(new ServiceReference2.HelloRequest() { Name = i.ToString() });
                }
            });
            //WebAPI
            var container = new System.Net.CookieContainer();
            var tWebAPI = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = HttpHelper.Get("http://localhost:8017/api/hello/" + i, "", ref container);
                }
            });
            //gRPC
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            var client = new VLService01.VLService01Client(channel);
            var tRPC = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = client.TransTest10kb(new HelloRequest { Name = i.ToString() });
                }
            });

            var summary = $@"local:{tLocal.TotalMilliseconds}
WebService:{tWebService.TotalMilliseconds}
WebAPI:{tWebAPI.TotalMilliseconds}
tRPC: { tRPC.TotalMilliseconds}
";
        //千次 毫秒数 数据量10kb
        //local: 2.0007
        //WebService: 1218.0075
        //WebAPI: 1605.9833
        //tRPC: 1168.0022



            return View();
        }
        public ActionResult DynamicWebService()
        {
            if (true)
            {
                //服务地址
                var url = "https://localhost:44334/SampleWebService.asmx";
                string ns = string.Format("ProxyServiceReference");
                //获取WSDL
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                System.Web.Services.Description.ServiceDescription sd = System.Web.Services.Description.ServiceDescription.Read(stream);//服务的描述信息都可以通过ServiceDescription获取
                string classname = sd.Services[0].Name;
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(ns);
                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();
                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");
                //编译代理类
                CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);
                if (cr.Errors.HasErrors == true)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }
                //生成代理实例，并调用方法
                Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(ns + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                //调用HelloWorld方法
                MethodInfo method = t.GetMethod("HelloWorldStr");
                //获取Add方法的参数
                ParameterInfo[] paraInfos = method.GetParameters();
                Console.WriteLine("HelloWorld方法有{0}个参数：", paraInfos.Length);
                foreach (ParameterInfo paramInfo in paraInfos)
                {
                    Console.WriteLine("参数名：{0}，参数类型：{1}", paramInfo.Name, paramInfo.ParameterType.Name);
                }
                var name = "vl";
                var paras = new object[] { name };
                try
                {
                    object result = method.Invoke(obj, paras);
                    Console.WriteLine("调用HelloWorld方法，返回{0}", result.ToString());
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return View();
        }

    }
}