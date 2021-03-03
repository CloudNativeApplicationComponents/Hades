using Grpc.Core;
using Hades.Transport.Abstraction.Options;

namespace Hades.Transport.Grpc.Abstraction.Options
{
    public class GrpcServerOptions : IHadesTransportOptions
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public ServerCredentials? Credentials { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(Host)
                && Port != 0
                && Credentials != null;
        }
    }
}
