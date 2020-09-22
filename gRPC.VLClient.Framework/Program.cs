using FrameworkTest.Common.ValuesSolution;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLService;
using VLStreamService;

namespace gRPC.VLClient.Framework
{
    class Program
    {
        const string Server = "localhost";
        const int Port = 30051;

        static void Main(string[] args)
        {
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);

            //SimpleRPCTest(channel);
            SteamRPCTest(channel);
        }

        private static void SimpleRPCTest(Channel channel)
        {
            var client = new VLService.VLService01.VLService01Client(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("FromServer, " + reply.ToJson());

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void SteamRPCTest(Channel channel)
        {
            var client = new LocalClient(new VLStreamService.VLStreamService.VLStreamServiceClient(channel));

            // Looking for a valid feature
            client.GetFeature(409146138, -746188906);

            // Feature missing.
            client.GetFeature(0, 0);

            // Looking for features between 40, -75 and 42, -73.
            client.ListFeatures(400000000, -750000000, 420000000, -730000000).Wait();

            // Record a few randomly selected points from the features file.
            client.RecordRoute(VLStreamServiceEx.LoadFeatures(), 10).Wait();

            // Send and receive some notes.
            client.RouteChat().Wait();

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }


    /// <summary>
    /// Sample client code that makes gRPC calls to the server.
    /// </summary>
    public class LocalClient
    {
        readonly VLStreamService.VLStreamService.VLStreamServiceClient client;

        public LocalClient(VLStreamService.VLStreamService.VLStreamServiceClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Blocking unary call example.  Calls GetFeature and prints the response.
        /// </summary>
        public void GetFeature(int lat, int lon)
        {
            try
            {
                Log("*** GetFeature: lat={0} lon={1}", lat, lon);

                Point request = new Point { Latitude = lat, Longitude = lon };

                Feature feature = client.GetFeature(request);
                if (feature.Exists())
                {
                    Log("Found feature called \"{0}\" at {1}, {2}",
                        feature.Name, feature.Location.GetLatitude(), feature.Location.GetLongitude());
                }
                else
                {
                    Log("Found no feature at {0}, {1}",
                        feature.Location.GetLatitude(), feature.Location.GetLongitude());
                }
            }
            catch (RpcException e)
            {
                Log("RPC failed " + e);
                throw;
            }
        }

        /// <summary>
        /// Server-streaming example. Calls listFeatures with a rectangle of interest. Prints each response feature as it arrives.
        /// </summary>
        public async Task ListFeatures(int lowLat, int lowLon, int hiLat, int hiLon)
        {
            try
            {
                Log("*** ListFeatures: lowLat={0} lowLon={1} hiLat={2} hiLon={3}", lowLat, lowLon, hiLat,
                    hiLon);

                Rectangle request = new Rectangle
                {
                    Lo = new Point { Latitude = lowLat, Longitude = lowLon },
                    Hi = new Point { Latitude = hiLat, Longitude = hiLon }
                };

                using (var call = client.ListFeatures(request))
                {
                    var responseStream = call.ResponseStream;
                    StringBuilder responseLog = new StringBuilder("Result: ");

                    while (await responseStream.MoveNext())
                    {
                        Feature feature = responseStream.Current;
                        responseLog.Append(feature.ToString());
                    }
                    Log(responseLog.ToString());
                }
            }
            catch (RpcException e)
            {
                Log("RPC failed " + e);
                throw;
            }
        }

        /// <summary>
        /// Client-streaming example. Sends numPoints randomly chosen points from features 
        /// with a variable delay in between. Prints the statistics when they are sent from the server.
        /// </summary>
        public async Task RecordRoute(List<Feature> features, int numPoints)
        {
            try
            {
                Log("*** RecordRoute");
                using (var call = client.RecordRoute())
                {
                    // Send numPoints points randomly selected from the features list.
                    StringBuilder numMsg = new StringBuilder();
                    Random rand = new Random();
                    for (int i = 0; i < numPoints; ++i)
                    {
                        int index = rand.Next(features.Count);
                        Point point = features[index].Location;
                        Log("Visiting point {0}, {1}", point.GetLatitude(), point.GetLongitude());

                        await call.RequestStream.WriteAsync(point);

                        // A bit of delay before sending the next one.
                        await Task.Delay(rand.Next(1000) + 500);
                    }
                    await call.RequestStream.CompleteAsync();

                    RouteSummary summary = await call.ResponseAsync;
                    Log("Finished trip with {0} points. Passed {1} features. "
                        + "Travelled {2} meters. It took {3} seconds.", summary.PointCount,
                        summary.FeatureCount, summary.Distance, summary.ElapsedTime);

                    Log("Finished RecordRoute");
                }
            }
            catch (RpcException e)
            {
                Log("RPC failed", e);
                throw;
            }
        }

        /// <summary>
        /// Bi-directional streaming example. Send some chat messages, and print any
        /// chat messages that are sent from the server.
        /// </summary>
        public async Task RouteChat()
        {
            try
            {
                Log("*** RouteChat");
                var requests = new List<RouteNote>
                    {
                        NewNote("First message", 0, 0),
                        NewNote("Second message", 0, 1),
                        NewNote("Third message", 1, 0),
                        NewNote("Fourth message", 0, 0)
                    };

                using (var call = client.RouteChat())
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                            Log("Got message \"{0}\" at {1}, {2}", note.Message,
                                note.Location.Latitude, note.Location.Longitude);
                        }
                    });

                    foreach (RouteNote request in requests)
                    {
                        Log("Sending message \"{0}\" at {1}, {2}", request.Message,
                            request.Location.Latitude, request.Location.Longitude);

                        await call.RequestStream.WriteAsync(request);
                    }
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;

                    Log("Finished RouteChat");
                }
            }
            catch (RpcException e)
            {
                Log("RPC failed", e);
                throw;
            }
        }

        private void Log(string s, params object[] args)
        {
            Console.WriteLine(string.Format(s, args));
        }

        private void Log(string s)
        {
            Console.WriteLine(s);
        }

        private RouteNote NewNote(string message, int lat, int lon)
        {
            return new RouteNote
            {
                Message = message,
                Location = new Point { Latitude = lat, Longitude = lon }
            };
        }
    }
}


//{"id":10229,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"08:35","taiFangwei":"","taixin":"140","gongsuoQd":"中等","chixu":"20","jiange":"8","xianlu":"-3","FetalPresentation":"头先露","Capacity":"100","gongkou":"2","gongjing":"","taimo":"","jianchaType":"内诊","xueya1":"116","xueya2":"72","maibo":"80","jianchaRen":"张美贤","yaowu":"","qita":"入室","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"36.5","xueyang":"99","status":0,"OperatorId":13}
//{"id":10230,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"08:50","taiFangwei":"","taixin":"142","gongsuoQd":"中等","chixu":"20","jiange":"8","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"开始缩宫素8d/min静点","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10231,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"09:10","taiFangwei":"","taixin":"144","gongsuoQd":"中等","chixu":"20","jiange":"5","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"缩宫素调至16d/min，下床活动","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10243,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"09:40","taiFangwei":"","taixin":"142","gongsuoQd":"中等","chixu":"20","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"104","xueya2":"58","maibo":"80","jianchaRen":"张美贤","yaowu":"","qita":"上床休息，吸氧","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10244,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"10:10","taiFangwei":"","taixin":"142","gongsuoQd":"中等","chixu":"25","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10245,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"10:40","taiFangwei":"","taixin":"140","gongsuoQd":"强","chixu":"30","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10251,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"11:10","taiFangwei":"","taixin":"144","gongsuoQd":"强","chixu":"30","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"115","xueya2":"77","maibo":"78","jianchaRen":"张美贤","yaowu":"","qita":"自行排尿一次","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10255,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"11:35","taiFangwei":"","taixin":"142","gongsuoQd":"强","chixu":"30","jiange":"3","xianlu":"-3","FetalPresentation":"头先露","Capacity":"100","gongkou":"2","gongjing":"中 中","taimo":"存","jianchaType":"内诊","xueya1":"115","xueya2":"70","maibo":"78","jianchaRen":"张美贤","yaowu":"","qita":"行分娩镇痛","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"99","status":0,"OperatorId":13}
//{"id":10256,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"11:50","taiFangwei":"","taixin":"140","gongsuoQd":"强","chixu":"30","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10257,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"12:05","taiFangwei":"","taixin":"136","gongsuoQd":"强","chixu":"30","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"120","xueya2":"70","maibo":"78","jianchaRen":"张美贤","yaowu":"","qita":"胎心监护显示减速，报告范美玲主任，指示停止缩宫素静点，吸氧，左侧卧位","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"36.9","xueyang":"99","status":0,"OperatorId":13}
//{"id":10258,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"12:20","taiFangwei":"","taixin":"138","gongsuoQd":"一般","chixu":"15","jiange":"5","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10259,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"12:35","taiFangwei":"","taixin":"134","gongsuoQd":"一般","chixu":"15","jiange":"5","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"范美玲主任指示缩宫素调至20d/min","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10268,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"12:50","taiFangwei":"","taixin":"140","gongsuoQd":"强","chixu":"30","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"出现减速，报告范美玲主任，停止缩宫素静点，吸氧，左侧卧位，快速补液","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10269,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"13:00","taiFangwei":"","taixin":"146","gongsuoQd":"强","chixu":"30","jiange":"3","xianlu":"-3","FetalPresentation":"头先露","Capacity":"100","gongkou":"2","gongjing":"","taimo":"存","jianchaType":"内诊","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"范美玲主任会诊，内诊","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10316,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"13:30","taiFangwei":"","taixin":"","gongsuoQd":"","chixu":"","jiange":"","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10317,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"11:40","taiFangwei":"","taixin":"","gongsuoQd":"","chixu":"","jiange":"","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10318,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"14:30","taiFangwei":"","taixin":"","gongsuoQd":"","chixu":"","jiange":"","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10319,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"15:00","taiFangwei":"","taixin":"","gongsuoQd":"","chixu":"","jiange":"","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"14点40分自然破水","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10322,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"15:30","taiFangwei":"","taixin":"132","gongsuoQd":"中等","chixu":"30","jiange":"3","xianlu":"-5","FetalPresentation":"头先露","Capacity":"100","gongkou":"3","gongjing":"中 中","taimo":"破","jianchaType":"内诊","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"36.4","xueyang":"","status":0,"OperatorId":13}
//{"id":10323,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"15:40","taiFangwei":"","taixin":"128","gongsuoQd":"中等","chixu":"30","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"胎心监护图出现减速，遵医嘱停止缩宫素静点，给予面罩吸调整体位","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
//{"id":10325,"chanfuId":5412,"daichanDate":"2020-09-22","daichanTime":"15:50","taiFangwei":"","taixin":"136","gongsuoQd":"中等","chixu":"30","jiange":"3","xianlu":"","FetalPresentation":"","Capacity":"","gongkou":"","gongjing":"","taimo":"","jianchaType":"","xueya1":"","xueya2":"","maibo":"","jianchaRen":"张美贤","yaowu":"","qita":"遵医嘱静点5%糖加维生素c","createTime":null,"updateTime":null,"yangshuixz":"","tiwen":"","xueyang":"","status":0,"OperatorId":13}
////{"id":10326,"chanfu_Id":5412,"OperatorId":13,"daichan_date":{"DateTime":"2020-09-22 16:10:00"},"daichan_time":{"DateTime":"2020-09-22 16:10:00"},"tai_fangwei":null,"tai_xin":"132","gongsuo_qd":"中等","chixu":"30","jiange":"3","xianlu":null,"gongkou":null,"gongjing":null,"tai_mo":null,"jiancha_Type":null,"xueya1":null,"xueya2":null,"maibo":null,"jiancha_ren":"张美贤","yaowu":null,"qita":"自觉头晕恶心遵医嘱胃复安入壶","yangshuixz":null,"tiwen":null,"xueyang":null,"status":1,"create_time":{"DateTime":"2020-09-22 16:25:12"},"update_time":{"DateTime":"2020-09-22 16:25:12"},"FetalPresentation":0,"Capacity":null}

//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10229,5412,'2020-09-22','2020-09-22 08:35:00','','140','中等','20','8','-3',1,'100','2','','','内诊'
//        ,'116','72','80','张美贤','','入室','','36.5','99',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10230,5412,'2020-09-22','2020-09-22 08:50:00','','142','中等','20','8','','','','','','','','','','','张美贤','','开始缩宫素8d/min静点','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10231,5412,'2020-09-22','2020-09-22 09:10:00','','144','中等','20','5','','','','','','','','','','','张美贤','','缩宫素调至16d/min，下床活动','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10243,5412,'2020-09-22','2020-09-22 09:40:00','','142','中等','20','3','','','','','','','','104','58','80','张美贤','','上床休息，吸氧','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10244,5412,'2020-09-22','2020-09-22 10:10:00','','142','中等','25','3','','','','','','','','','','','张美贤','','','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10245,5412,'2020-09-22','2020-09-22 10:40:00','','140','强','30','3','','','','','','','','','','','张美贤','','','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10251,5412,'2020-09-22','2020-09-22 11:10:00','','144','强','30','3','','','','','','','','115','77','78','张美贤','','自行排尿一次','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10255,5412,'2020-09-22','2020-09-22 11:35:00','','142','强','30','3','-3',1,'100','2','中 中','存','内诊','115','70','78','张美贤','','行分娩镇痛','','','99',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10256,5412,'2020-09-22','2020-09-22 11:50:00','','140','强','30','3','','','','','','','','','','','张美贤','','','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10257,5412,'2020-09-22','2020-09-22 12:05:00','','136','强','30','3','','','','','','','','120','70','78','张美贤','','胎心监护显示减速，报告范美玲主任，指示停止缩宫素静点，吸氧，左侧卧位','','36.9','99',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10258,5412,'2020-09-22','2020-09-22 12:20:00','','138','一般','15','5','','','','','','','','','','','张美贤','','','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10259,5412,'2020-09-22','2020-09-22 12:35:00','','134','一般','15','5','','','','','','','','','','','张美贤','','范美玲主任指示缩宫素调至20d/min','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10268,5412,'2020-09-22','2020-09-22 12:50:00','','140','强','30','3','','','','','','','','','','','张美贤','','出现减速，报告范美玲主任，停止缩宫素静点，吸氧，左侧卧位，快速补液','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10269,5412,'2020-09-22','2020-09-22 13:00:00','','146','强','30','3','-3',1,'100','2','','存','内诊','','','','张美贤','','范美玲主任会诊，内诊','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10316,5412,'2020-09-22','2020-09-22 13:30:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10317,5412,'2020-09-22','2020-09-22 11:40:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10318,5412,'2020-09-22','2020-09-22 14:30:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10319,5412,'2020-09-22','2020-09-22 15:00:00','','','','','','','','','','','','','','','','张美贤','','14点40分自然破水','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10322,5412,'2020-09-22','2020-09-22 15:30:00','','132','中等','30','3','-5',1,'100','3','中 中','破','内诊','','','','张美贤','','','','36.4','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10323,5412,'2020-09-22','2020-09-22 15:40:00','','128','中等','30','3','','','','','','','','','','','张美贤','','胎心监护图出现减速，遵医嘱停止缩宫素静点，给予面罩吸调整体位','','','',0,13);
//INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10325,5412,'2020-09-22','2020-09-22 15:50:00','','136','中等','30','3','','','','','','','','','','','张美贤','','遵医嘱静点5%糖加维生素c','','','',0,13);
			

//INSERT INTO fm_daichan([id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [create_time], [update_time], [status], [yangshuixz], [tiwen], [xueyang], [FetalPresentation], [Capacity], [OperatorId]) VALUES(5111, 3078, '2020-05-21', '2020-05-21 14:30:00.000', '右枕前', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', N'系统', N' ', N' ', '2020-05-21 14:30:32.000', '2020-05-21 14:38:43.877', 0, ' ', ' ', ' ', 0, '', NULL);

