// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: protos/Autobots.Infrastracture.DataLogCenter.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Autobots.Infrastracture.DataLogCenter {
  public static partial class DataLogServiceDefinition
  {
    static readonly string __ServiceName = "Autobots.Infrastracture.DataLogCenter.DataLogServiceDefinition";

    static readonly grpc::Marshaller<global::Autobots.Infrastracture.DataLogCenter.DataLogRequest> __Marshaller_Autobots_Infrastracture_DataLogCenter_DataLogRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Autobots.Infrastracture.DataLogCenter.DataLogRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Autobots.Infrastracture.DataLogCenter.DataLogResponse> __Marshaller_Autobots_Infrastracture_DataLogCenter_DataLogResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::Autobots.Infrastracture.DataLogCenter.DataLogRequest, global::Autobots.Infrastracture.DataLogCenter.DataLogResponse> __Method_CommitDataLog = new grpc::Method<global::Autobots.Infrastracture.DataLogCenter.DataLogRequest, global::Autobots.Infrastracture.DataLogCenter.DataLogResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CommitDataLog",
        __Marshaller_Autobots_Infrastracture_DataLogCenter_DataLogRequest,
        __Marshaller_Autobots_Infrastracture_DataLogCenter_DataLogResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Autobots.Infrastracture.DataLogCenter.AutobotsInfrastractureDataLogCenterReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of DataLogServiceDefinition</summary>
    [grpc::BindServiceMethod(typeof(DataLogServiceDefinition), "BindService")]
    public abstract partial class DataLogServiceDefinitionBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Autobots.Infrastracture.DataLogCenter.DataLogResponse> CommitDataLog(global::Autobots.Infrastracture.DataLogCenter.DataLogRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for DataLogServiceDefinition</summary>
    public partial class DataLogServiceDefinitionClient : grpc::ClientBase<DataLogServiceDefinitionClient>
    {
      /// <summary>Creates a new client for DataLogServiceDefinition</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public DataLogServiceDefinitionClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for DataLogServiceDefinition that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public DataLogServiceDefinitionClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected DataLogServiceDefinitionClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected DataLogServiceDefinitionClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Autobots.Infrastracture.DataLogCenter.DataLogResponse CommitDataLog(global::Autobots.Infrastracture.DataLogCenter.DataLogRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CommitDataLog(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Autobots.Infrastracture.DataLogCenter.DataLogResponse CommitDataLog(global::Autobots.Infrastracture.DataLogCenter.DataLogRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CommitDataLog, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Autobots.Infrastracture.DataLogCenter.DataLogResponse> CommitDataLogAsync(global::Autobots.Infrastracture.DataLogCenter.DataLogRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CommitDataLogAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Autobots.Infrastracture.DataLogCenter.DataLogResponse> CommitDataLogAsync(global::Autobots.Infrastracture.DataLogCenter.DataLogRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CommitDataLog, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override DataLogServiceDefinitionClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new DataLogServiceDefinitionClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(DataLogServiceDefinitionBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_CommitDataLog, serviceImpl.CommitDataLog).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, DataLogServiceDefinitionBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_CommitDataLog, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Autobots.Infrastracture.DataLogCenter.DataLogRequest, global::Autobots.Infrastracture.DataLogCenter.DataLogResponse>(serviceImpl.CommitDataLog));
    }

  }
}
#endregion
