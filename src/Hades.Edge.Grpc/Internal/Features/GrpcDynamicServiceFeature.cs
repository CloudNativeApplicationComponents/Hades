using Hades.Edge.Abstraction.Features;
using Hades.Edge.Abstraction.Services;
using Hades.Transport.Grpc.Abstraction;
using System;
using System.Collections.Generic;

namespace Hades.Edge.Grpc.Internal.Features
{
    internal class GrpcDynamicServiceFeature : IDynamicServiceFeature
    {
        private readonly IGrpcTransportFactory _transportFactory;
        public GrpcDynamicServiceFeature(IGrpcTransportFactory transportFactory)
        {
            _transportFactory = transportFactory
                ?? throw new ArgumentNullException(nameof(transportFactory));
        }

        public IEnumerable<IDynamicService> GetDynamicServices()
        {
            //return _serviceDiscovery.GetDynamicServices(DataPlane.Grpc.ToString());
            throw new NotImplementedException();
        }
    }
}
