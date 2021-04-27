using Hades.Transport.Abstraction.Options;

namespace Hades.Transport.Grpc.Abstraction.Options
{
    public class GrpcServerEndpointOptions : IHadesTransportOptions
    {
        public string? ServiceName { get; set; }
        public string? MethodName { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(ServiceName)
                && !string.IsNullOrWhiteSpace(MethodName);
        }
    }
}
