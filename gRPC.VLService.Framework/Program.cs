using FrameworkTest.Common.ValuesSolution;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
