using Google.Protobuf;
using Grpc.Core;
using Hades.Transport.Grpc.Abstraction;
using Hades.Transport.Grpc.Abstraction.Serialization;
using System;
using System.Collections.Concurrent;

namespace Hades.Transport.Grpc.Internal
{
    internal class GrpcMethodFactory : IGrpcMethodFactory
    {
        private readonly IGenericProtobufSerializer _serializer;
        private readonly IGenericProtobufDeserializer _deserializer;
        private readonly ConcurrentDictionary<(string, MethodType, Type, Type), IMethod> _methods;

        public GrpcMethodFactory(
            IGenericProtobufSerializer serializer,
            IGenericProtobufDeserializer deserializer)
        {
            _serializer = serializer
                ?? throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer
                ?? throw new ArgumentNullException(nameof(deserializer));

            _methods = new ConcurrentDictionary<(string, MethodType, Type, Type), IMethod>();
        }

        public Method<TRequest, TResponse> Create<TRequest, TResponse>(
            string serviceName,
            string methodName,
            MethodType methodType)
            where TRequest : class, IMessage<TRequest>, new()
            where TResponse : class, IMessage<TResponse>, new()
        {
            var key = ($"{serviceName}/{methodName}", methodType, typeof(TRequest), typeof(TResponse));

            return (Method<TRequest, TResponse>)_methods.GetOrAdd(key, _ =>
            {
                return GrpcUtility.CreateMethod<TRequest, TResponse>(
                    serviceName, 
                    methodName, 
                    methodType, 
                    _serializer, 
                    _deserializer);
            });
        }
    }
}
