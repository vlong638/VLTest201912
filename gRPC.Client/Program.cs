﻿

using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VL.Consolo_Core.Common.ValuesSolution;
using VLService;
using VLStreamService;

namespace gRPC.Client
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