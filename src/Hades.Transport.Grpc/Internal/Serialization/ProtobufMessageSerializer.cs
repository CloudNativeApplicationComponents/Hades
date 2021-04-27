using CloudNativeApplicationComponents.Utils;
using Google.Protobuf;
using Grpc.Core;
using Hades.Transport.Abstraction.Messaging;
using Hades.Transport.Grpc.Internal.Messages;
using System.Linq;
using System.Net.Mime;

namespace Hades.Transport.Grpc.Internal.Serialization
{
    internal class ProtobufMessageSerializer :
        IProtobufEnvelopeSerializer,
        IProtobufEnvelopeDeserializer,
        IRpcExceptionEnvelopeSerializer,
        IRpcExceptionEnvelopeDeserializer
    {
        //TODO create repository for content type
        protected const string _contentType = "application/vnd.v1.protobufmessage+grpc";
        public IRpcExceptionEnvelopeSerializer ErrorSerializer => this;
        public IRpcExceptionEnvelopeDeserializer ErrorDeserializer => this;

        public Envelope Deserialize(ProtobufMessage protobufMessage, Metadata? metadata = null)
        {
            var envelope = new Envelope(contentType: new ContentType(_contentType))
            {
                Body = protobufMessage.ToByteArray(),
                CorrelationId = metadata?.GetValue(nameof(Envelope.CorrelationId)),
                EnvelopeId = metadata?.GetValue(nameof(Envelope.EnvelopeId)) ?? string.Empty
            };

            metadata?.ForEach(t =>
            {
                //Envelope should not contain its fields as header
                if (t.Key != nameof(Envelope.CorrelationId) || t.Key != nameof(Envelope.EnvelopeId))
                    envelope.Headers.Add(t.Key, t.Value);
            });

            return envelope;
        }

        public Envelope DeserializeException(RpcException rpcException, Metadata? metadata = null)
        {
            var envelope = new Envelope(new ContentType(_contentType))
            {
                CorrelationId = metadata?.GetValue(nameof(Envelope.CorrelationId)),
                EnvelopeId = metadata?.GetValue(nameof(Envelope.EnvelopeId)) ?? string.Empty
            };

            envelope.AddException(rpcException);
            metadata?.ForEach(t => envelope.Headers.Add(t.Key, t.Value));

            return envelope;
        }

        public (ProtobufMessage, Metadata) Serialize(Envelope envelope)
        {
            var protoMessage = envelope.Body is not null ?
                ProtobufMessage.Parser.ParseFrom((byte[])envelope.Body) :
                new ProtobufMessage();

            var metadata = new Metadata();

            foreach (var item in
                envelope.Headers.AllKeys
                .ToDictionary(x => x, x => envelope.Headers[x]))
                metadata.Add(item.Key, item.Value);

            metadata.Add(nameof(Envelope.CorrelationId), envelope.CorrelationId);
            metadata.Add(nameof(Envelope.EnvelopeId), envelope.EnvelopeId);
            
            return (protoMessage, metadata);
        }

        public (RpcException, Metadata) SerializeException(Envelope envelope)
        {
            var metadata = new Metadata();

            foreach (var item in
                envelope.Headers.AllKeys
                .ToDictionary(x => x, x => envelope.Headers[x]))
                metadata.Add(item.Key, item.Value);

            //TODO We have some prombelm here
            var rpcException = new RpcException(Status.DefaultCancelled);

            return (rpcException, metadata);
        }
    }
}
