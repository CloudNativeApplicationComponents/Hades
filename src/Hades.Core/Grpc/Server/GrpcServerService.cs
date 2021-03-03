﻿using Google.Protobuf;
using Grpc.Core;
using Hades.Core.Abbstraction.Grpc.Server;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hades.Core.Grpc
{
    public class GrpcServerService : IGrpcServerService
    {
        public string Name { get; }

        public IEnumerable<IGrpcServerMethod> Methods { get; }

        public GrpcServerService(string name, IEnumerable<IGrpcServerMethod> methods)
        {
            if (string.IsNullOrWhiteSpace(nameof(name)))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Methods = methods
                ?? throw new ArgumentNullException(nameof(methods));
        }

        public ServerServiceDefinition Build()
        {
            var builder = ServerServiceDefinition.CreateBuilder();

            Methods.Foreach(t => t.Bind(this, builder));
            return builder.Build();
        }

        protected virtual Method<TRequest, TResponse> CreateMethod<TRequest, TResponse>(string serviceName, string methodName, MethodType methodType)
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new()
        {
            var requestParser = new MessageParser<TRequest>(() => new TRequest());
            var responseParser = new MessageParser<TResponse>(() => new TResponse());
            var requestMarshaller = Marshallers.Create(SerializeMessage, context => DeserializeMessage(context, requestParser));
            var responseMarshaller = Marshallers.Create(SerializeMessage, context => DeserializeMessage(context, responseParser));

            return new Method<TRequest, TResponse>(methodType, serviceName, methodName, requestMarshaller, responseMarshaller);
        }

        protected virtual void SerializeMessage<T>(T message, SerializationContext context)
        where T : IMessage<T>
        {
#if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
            if (message is IBufferMessage)
            {
                context.SetPayloadLength(message.CalculateSize());
                MessageExtensions.WriteTo(message, context.GetBufferWriter());
                context.Complete();
                return;
            }
#endif
            context.Complete(MessageExtensions.ToByteArray(message));
        }

        protected virtual T DeserializeMessage<T>(DeserializationContext context, MessageParser<T> parser)
            where T : IMessage<T>
        {
#if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
            if (MessageCache<T>.IsBufferMessage)
            {
                return parser.ParseFrom(context.PayloadAsReadOnlySequence());
            }
#endif
            return parser.ParseFrom(context.PayloadAsNewBuffer());
        }

        void IGrpcServerService.Visit<TRequest, TResponse>(IUnaryGrpcServerMethod<TRequest, TResponse> method, ServerServiceDefinition.Builder builder)
        {
            var grpcMethod = CreateMethod<TRequest, TResponse>(this.Name, method.Name, method.MethodType);
            builder.AddMethod(grpcMethod, method.Execute);
        }

        void IGrpcServerService.Visit<TRequest, TResponse>(IClientStreamingGrpcServerMethod<TRequest, TResponse> method, ServerServiceDefinition.Builder builder)
        {
            var grpcMethod = CreateMethod<TRequest, TResponse>(Name, method.Name, method.MethodType);
            builder.AddMethod(grpcMethod, (requestStream, context) =>
            {
                return method.Execute(requestStream.ReadAllAsync(context.CancellationToken), context);
            });
        }

        void IGrpcServerService.Visit<TRequest, TResponse>(IServerStreamingGrpcServerMethod<TRequest, TResponse> method, ServerServiceDefinition.Builder builder)
        {
            var grpcMethod = CreateMethod<TRequest, TResponse>(Name, method.Name, method.MethodType);
            builder.AddMethod(grpcMethod, async (request, responseStream, context) =>
            {
                var result = method.Execute(request, context);
                await foreach (var item in result)
                {
                    await responseStream.WriteAsync(item);
                }
            });
        }

        void IGrpcServerService.Visit<TRequest, TResponse>(IDuplexStreamingGrpcServerMethod<TRequest, TResponse> method, ServerServiceDefinition.Builder builder)
        {
            var grpcMethod = CreateMethod<TRequest, TResponse>(Name, method.Name, method.MethodType);
            builder.AddMethod(grpcMethod, async (requestStream, responseStream, context) =>
            {
                var result = method.Execute(requestStream.ReadAllAsync(), context);
                await foreach (var item in result)
                {
                    await responseStream.WriteAsync(item);
                }
            });
        }

        static class MessageCache<T>
        {
            public static readonly bool IsBufferMessage = IntrospectionExtensions.GetTypeInfo(typeof(IBufferMessage)).IsAssignableFrom(typeof(T));
        }

    }
}
