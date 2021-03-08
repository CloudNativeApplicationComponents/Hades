using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Hades.Edge.Grpc.Internal.Messages
{
    internal sealed class ProtobufMessage : IMessage<ProtobufMessage>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , IBufferMessage
#endif
    {
        private static readonly MessageParser<ProtobufMessage> _parser = new MessageParser<ProtobufMessage>(() => new ProtobufMessage());
        private UnknownFieldSet _unknownFields;

        public static MessageParser<ProtobufMessage> Parser 
            => _parser;

        public static MessageDescriptor Descriptor 
            => MessageReflection.Descriptor.MessageTypes[0]; 

        MessageDescriptor IMessage.Descriptor
            => Descriptor;

        public ProtobufMessage()
        { }

        public ProtobufMessage(ProtobufMessage other) : this()
        {
            _unknownFields = UnknownFieldSet.Clone(other._unknownFields);
        }

        public ProtobufMessage Clone()
        {
            return new ProtobufMessage(this);
        }

        public override bool Equals(object other)
        {
            return Equals(other as ProtobufMessage);
        }

        public bool Equals(ProtobufMessage other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            return Equals(_unknownFields, other._unknownFields);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            if (_unknownFields != null)
            {
                hash ^= _unknownFields.GetHashCode();
            }
            return hash;
        }

        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        public void WriteTo(CodedOutputStream output)
        {
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
            output.WriteRawMessage(this);
#else
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
#endif
        }

#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
        void IBufferMessage.InternalWriteTo(ref WriteContext output)
        {
            if (_unknownFields != null)
            {
                _unknownFields.WriteTo(ref output);
            }
        }
#endif

        public int CalculateSize()
        {
            int size = 0;
            if (_unknownFields != null)
            {
                size += _unknownFields.CalculateSize();
            }
            return size;
        }

        public void MergeFrom(ProtobufMessage other)
        {
            if (other == null)
            {
                return;
            }
            _unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        public void MergeFrom(CodedInputStream input)
        {
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
            input.ReadRawMessage(this);
#else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
        }
      }
#endif
        }

#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE

        void IBufferMessage.InternalMergeFrom(ref ParseContext input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                _unknownFields = tag switch
                {
                    _ => UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input),
                };
            }
        }
#endif
    }
}
