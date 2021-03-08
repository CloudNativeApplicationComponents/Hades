using Google.Protobuf;
using Grpc.Core;
using Hades.Edge.Grpc.Serialization;

namespace Hades.Edge.Grpc.Internal
{
    public static class Utility
    {
        public static Method<TRequest, TResponse> CreateMethod<TRequest, TResponse>(string serviceName, 
            string methodName, 
            MethodType methodType,
            IProtobufMessageSerializer serializer,
            IProtobufMessageDeserializer deserializer)
        where TRequest : class, IMessage<TRequest>, new()
        where TResponse : class, IMessage<TResponse>, new()
        {
            var requestParser = new MessageParser<TRequest>(() => new TRequest()).WithDiscardUnknownFields(false);
            var responseParser = new MessageParser<TResponse>(() => new TResponse()).WithDiscardUnknownFields(false);
            var requestMarshaller = Marshallers.Create(serializer.SerializeMessage, context => deserializer.DeserializeMessage(context, requestParser));
            var responseMarshaller = Marshallers.Create(serializer.SerializeMessage, context => deserializer.DeserializeMessage(context, responseParser));

            return new Method<TRequest, TResponse>(methodType, serviceName, methodName, requestMarshaller, responseMarshaller);
        }
    }
}
