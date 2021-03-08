﻿using Google.Protobuf;
using Grpc.Core;

namespace Hades.Edge.Grpc.Serialization
{
    public interface IProtobufMessageSerializer
    {
        void SerializeMessage<T>(T message, SerializationContext context)
        where T : IMessage<T>;
    }
}