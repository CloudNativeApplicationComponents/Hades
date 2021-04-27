using Hades.Transport.Grpc.Internal.Serialization;
using System;

namespace Hades.Transport.Grpc.Internal.Server.EndpointAccessors
{
    internal class GrpcServerEndpointAccessorBase :
        IDisposable
    {
        private bool _disposedValue;

        protected IProtobufEnvelopeSerializer Serializer { get; }
        protected IProtobufEnvelopeDeserializer Deserializer { get; }

        public GrpcServerEndpointAccessorBase(
            IProtobufEnvelopeSerializer serializer,
            IProtobufEnvelopeDeserializer deserializer)
        {
            Serializer = serializer ??
                throw new ArgumentNullException(nameof(serializer));
            Deserializer = deserializer ??
                throw new ArgumentNullException(nameof(deserializer));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
