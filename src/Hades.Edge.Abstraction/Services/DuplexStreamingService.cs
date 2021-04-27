﻿using CloudNativeApplicationComponents.Utils;
using System;

namespace Hades.Edge.Abstraction.Services
{
    public class DuplexStreamingService : DynamicServiceBase
    {
        public sealed override DynamicServiceType ServiceType =>  DynamicServiceType.DuplexStreaming;

        public DuplexStreamingService(string catalogName, string serviceName, string dataPlane) 
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
