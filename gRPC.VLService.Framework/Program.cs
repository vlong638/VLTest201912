using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VLService;
using VLStreamService;

namespace gRPC.VLService.Framework
{
    class VLService01Impl : VLService01.VLService01Base
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            //Console.WriteLine("From Client," + request.ToJson());
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> TransTest10kb(HelloRequest request, ServerCallContext context)
        {
            //Console.WriteLine("From Client," + request.ToJson());
            return Task.FromResult(new HelloReply { Message = @"
			set identity_insert fm_daichan on
	
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10229,5412,'2020-09-22','2020-09-22 08:35:00','','140','中等','20','8','-3',1,'100','2','','','内诊'
        ,'116','72','80','张美贤','','入室','','36.5','99',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10230,5412,'2020-09-22','2020-09-22 08:50:00','','142','中等','20','8','','','','','','','','','','','张美贤','','开始缩宫素8d/min静点','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10231,5412,'2020-09-22','2020-09-22 09:10:00','','144','中等','20','5','','','','','','','','','','','张美贤','','缩宫素调至16d/min，下床活动','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10243,5412,'2020-09-22','2020-09-22 09:40:00','','142','中等','20','3','','','','','','','','104','58','80','张美贤','','上床休息，吸氧','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10244,5412,'2020-09-22','2020-09-22 10:10:00','','142','中等','25','3','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10245,5412,'2020-09-22','2020-09-22 10:40:00','','140','强','30','3','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10251,5412,'2020-09-22','2020-09-22 11:10:00','','144','强','30','3','','','','','','','','115','77','78','张美贤','','自行排尿一次','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10255,5412,'2020-09-22','2020-09-22 11:35:00','','142','强','30','3','-3',1,'100','2','中 中','存','内诊','115','70','78','张美贤','','行分娩镇痛','','','99',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10256,5412,'2020-09-22','2020-09-22 11:50:00','','140','强','30','3','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10257,5412,'2020-09-22','2020-09-22 12:05:00','','136','强','30','3','','','','','','','','120','70','78','张美贤','','胎心监护显示减速，报告范美玲主任，指示停止缩宫素静点，吸氧，左侧卧位','','36.9','99',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10258,5412,'2020-09-22','2020-09-22 12:20:00','','138','一般','15','5','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10259,5412,'2020-09-22','2020-09-22 12:35:00','','134','一般','15','5','','','','','','','','','','','张美贤','','范美玲主任指示缩宫素调至20d/min','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10268,5412,'2020-09-22','2020-09-22 12:50:00','','140','强','30','3','','','','','','','','','','','张美贤','','出现减速，报告范美玲主任，停止缩宫素静点，吸氧，左侧卧位，快速补液','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10269,5412,'2020-09-22','2020-09-22 13:00:00','','146','强','30','3','-3',1,'100','2','','存','内诊','','','','张美贤','','范美玲主任会诊，内诊','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10316,5412,'2020-09-22','2020-09-22 13:30:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10317,5412,'2020-09-22','2020-09-22 11:40:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10318,5412,'2020-09-22','2020-09-22 14:30:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10319,5412,'2020-09-22','2020-09-22 15:00:00','','','','','','','','','','','','','','','','张美贤','','14点40分自然破水','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10322,5412,'2020-09-22','2020-09-22 15:30:00','','132','中等','30','3','-5',1,'100','3','中 中','破','内诊','','','','张美贤','','','','36.4','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10323,5412,'2020-09-22','2020-09-22 15:40:00','','128','中等','30','3','','','','','','','','','','','张美贤','','胎心监护图出现减速，遵医嘱停止缩宫素静点，给予面罩吸调整体位','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10325,5412,'2020-09-22','2020-09-22 15:50:00','','136','中等','30','3','','','','','','','','','','','张美贤','','遵医嘱静点5%糖加维生素c','','','',0,13);
set identity_insert fm_daichan off
" });
        }
    }

    class VLStreamServiceImpl : VLStreamService.VLStreamService.VLStreamServiceBase
    {
        readonly List<Feature> features;
        readonly object myLock = new object();
        readonly Dictionary<Point, List<RouteNote>> routeNotes = new Dictionary<Point, List<RouteNote>>();

        public VLStreamServiceImpl(List<Feature> features)
        {
            this.features = features;
        }

        ///
        public override Task<Feature> GetFeature(Point request, ServerCallContext context)
        {
            return Task.FromResult(CheckFeature(request));
        }
        private Feature CheckFeature(Point location)
        {
            var result = features.FirstOrDefault((feature) => feature.Location.Equals(location));
            if (result == null)
            {
                // No feature was found, return an unnamed feature.
                return new Feature { Name = "", Location = location };
            }
            return result;
        }

        /// <summary>
        /// Gets all features contained within the given bounding rectangle.
        /// </summary>
        public override async Task ListFeatures(Rectangle request, IServerStreamWriter<Feature> responseStream, ServerCallContext context)
        {
            var responses = features.FindAll((feature) => feature.Exists() && request.Contains(feature.Location));
            foreach (var response in responses)
            {
                await responseStream.WriteAsync(response);
            }
        }

        /// <summary>
        /// Gets a stream of points, and responds with statistics about the "trip": number of points,
        /// number of known features visited, total distance traveled, and total time spent.
        /// </summary>
        public override async Task<RouteSummary> RecordRoute(IAsyncStreamReader<Point> requestStream, ServerCallContext context)
        {
            int pointCount = 0;
            int featureCount = 0;
            int distance = 0;
            Point previous = null;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (await requestStream.MoveNext())
            {
                var point = requestStream.Current;
                pointCount++;
                if (CheckFeature(point).Exists())
                {
                    featureCount++;
                }
                if (previous != null)
                {
                    distance += (int)previous.GetDistance(point);
                }
                previous = point;
            }

            stopwatch.Stop();

            return new RouteSummary
            {
                PointCount = pointCount,
                FeatureCount = featureCount,
                Distance = distance,
                ElapsedTime = (int)(stopwatch.ElapsedMilliseconds / 1000)
            };
        }

        /// <summary>
        /// Receives a stream of message/location pairs, and responds with a stream of all previous
        /// messages at each of those locations.
        /// </summary>
        public override async Task RouteChat(IAsyncStreamReader<RouteNote> requestStream, IServerStreamWriter<RouteNote> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var note = requestStream.Current;
                List<RouteNote> prevNotes = AddNoteForLocation(note.Location, note);
                foreach (var prevNote in prevNotes)
                {
                    await responseStream.WriteAsync(prevNote);
                }
            }
        }

        /// <summary>
        /// Adds a note for location and returns a list of pre-existing notes for that location (not containing the newly added note).
        /// </summary>
        private List<RouteNote> AddNoteForLocation(Point location, RouteNote note)
        {
            lock (myLock)
            {
                List<RouteNote> notes;
                if (!routeNotes.TryGetValue(location, out notes))
                {
                    notes = new List<RouteNote>();
                    routeNotes.Add(location, notes);
                }
                var preexistingNotes = new List<RouteNote>(notes);
                notes.Add(note);
                return preexistingNotes;
            }
        }
    }

    class Program
    {
        const string Server = "localhost";
        const int Port = 30051;

        static void Main(string[] args)
        {
            var features = VLStreamServiceEx.LoadFeatures();
            Server server = new Server
            {
                Services = { VLService01.BindService(new VLService01Impl()), VLStreamService.VLStreamService.BindService(new VLStreamServiceImpl(features)) },
                Ports = { new ServerPort(Server, Port, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }

        private static void SimpleRPCTest()
        {
            Server server = new Server
            {
                Services = { VLService01.BindService(new VLService01Impl()) },
                Ports = { new ServerPort(Server, Port, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }
    }
}
