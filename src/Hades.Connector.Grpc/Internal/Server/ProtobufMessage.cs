using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;

namespace Hades.Connector.Grpc.Internal.Server
{
    public class ProtobufMessage : IMessage<ProtobufMessage>
    {
        public MessageDescriptor Descriptor => throw new NotImplementedException();

        public int CalculateSize()
        {
            throw new NotImplementedException();
        }

        public ProtobufMessage Clone()
        {
            throw new NotImplementedException();
        }

        public bool Equals(ProtobufMessage other)
        {
            throw new NotImplementedException();
        }

        public void MergeFrom(ProtobufMessage message)
        {
            throw new NotImplementedException();
        }

        public void MergeFrom(CodedInputStream input)
        {
            throw new NotImplementedException();
        }

        public void WriteTo(CodedOutputStream output)
        {
            throw new NotImplementedException();
        }
    }
}
