﻿using CloudNativeApplicationComponents.Utils;
using System;

namespace Hades.Edge.Abstraction.Services
{
    public class ServerStreamingService : DynamicServiceBase
    {
        public sealed override DynamicServiceType ServiceType => DynamicServiceType.ServerStreaming;

        public ServerStreamingService(string catalogName, string serviceName, string dataPlane) 
            : base(catalogName, serviceName, dataPlane)
        {
        }

        public override void Accept(IDynamicServiceVisitor visitor, AggregationContext context)
        {
            _ = visitor
                ?? throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this, context);
        }
    }
}
