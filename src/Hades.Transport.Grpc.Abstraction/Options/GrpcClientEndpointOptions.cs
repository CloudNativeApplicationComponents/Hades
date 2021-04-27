using Grpc.Core;
using Hades.Transport.Abstraction.Options;
using System;

namespace Hades.Transport.Grpc.Abstraction.Options
{
    public class GrpcClientEndpointOptions : IHadesTransportOptions
    {
        public Uri? BaseUri { get; set; }
        public string? ServiceName { get; set; }
        public string? MethodName { get; set; }
        public ChannelCredentials? Credentials { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(ServiceName)
                && !string.IsNullOrWhiteSpace(MethodName)
                && BaseUri != null
                && Credentials != null;
        }
    }
}
