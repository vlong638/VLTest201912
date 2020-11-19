// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Autobots.B2Service.proto
// </auto-generated>
// Original file comments:
// Copyright 2015 gRPC authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Autobots.B2ServiceDefinition {
  /// <summary>
  /// The greeting service definition.
  /// </summary>
  public static partial class B2Service
  {
    static readonly string __ServiceName = "Autobots.B2ServiceDefinition.B2Service";

    static readonly grpc::Marshaller<global::Autobots.B2ServiceDefinition.HelloRequest> __Marshaller_Autobots_B2ServiceDefinition_HelloRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Autobots.B2ServiceDefinition.HelloRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Autobots.B2ServiceDefinition.HelloReply> __Marshaller_Autobots_B2ServiceDefinition_HelloReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Autobots.B2ServiceDefinition.HelloReply.Parser.ParseFrom);

    static readonly grpc::Method<global::Autobots.B2ServiceDefinition.HelloRequest, global::Autobots.B2ServiceDefinition.HelloReply> __Method_SayHello = new grpc::Method<global::Autobots.B2ServiceDefinition.HelloRequest, global::Autobots.B2ServiceDefinition.HelloReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SayHello",
        __Marshaller_Autobots_B2ServiceDefinition_HelloRequest,
        __Marshaller_Autobots_B2ServiceDefinition_HelloReply);

    static readonly grpc::Method<global::Autobots.B2ServiceDefinition.HelloRequest, global::Autobots.B2ServiceDefinition.HelloReply> __Method_TransTest10kb = new grpc::Method<global::Autobots.B2ServiceDefinition.HelloRequest, global::Autobots.B2ServiceDefinition.HelloReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "TransTest10kb",
        __Marshaller_Autobots_B2ServiceDefinition_HelloRequest,
        __Marshaller_Autobots_B2ServiceDefinition_HelloReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Autobots.B2ServiceDefinition.AutobotsB2ServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of B2Service</summary>
    [grpc::BindServiceMethod(typeof(B2Service), "BindService")]
    public abstract partial class B2ServiceBase
    {
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Autobots.B2ServiceDefinition.HelloReply> SayHello(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Autobots.B2ServiceDefinition.HelloReply> TransTest10kb(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for B2Service</summary>
    public partial class B2ServiceClient : grpc::ClientBase<B2ServiceClient>
    {
      /// <summary>Creates a new client for B2Service</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public B2ServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for B2Service that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public B2ServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected B2ServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected B2ServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Autobots.B2ServiceDefinition.HelloReply SayHello(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SayHello(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Autobots.B2ServiceDefinition.HelloReply SayHello(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SayHello, null, options, request);
      }
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Autobots.B2ServiceDefinition.HelloReply> SayHelloAsync(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SayHelloAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Autobots.B2ServiceDefinition.HelloReply> SayHelloAsync(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SayHello, null, options, request);
      }
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Autobots.B2ServiceDefinition.HelloReply TransTest10kb(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return TransTest10kb(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Autobots.B2ServiceDefinition.HelloReply TransTest10kb(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_TransTest10kb, null, options, request);
      }
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Autobots.B2ServiceDefinition.HelloReply> TransTest10kbAsync(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return TransTest10kbAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Autobots.B2ServiceDefinition.HelloReply> TransTest10kbAsync(global::Autobots.B2ServiceDefinition.HelloRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_TransTest10kb, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override B2ServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new B2ServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(B2ServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SayHello, serviceImpl.SayHello)
          .AddMethod(__Method_TransTest10kb, serviceImpl.TransTest10kb).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, B2ServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SayHello, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Autobots.B2ServiceDefinition.HelloRequest, global::Autobots.B2ServiceDefinition.HelloReply>(serviceImpl.SayHello));
      serviceBinder.AddMethod(__Method_TransTest10kb, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Autobots.B2ServiceDefinition.HelloRequest, global::Autobots.B2ServiceDefinition.HelloReply>(serviceImpl.TransTest10kb));
    }

  }
}
#endregion