using CloudNativeApplicationComponents.Utils.Configurators;
using Grpc.Core;
using Hades.Transport.Grpc.Abstraction.Configurators;
using Hades.Transport.Grpc.Abstraction.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Hades.Transport.Grpc.Extensions.Internal.Configurators
{
    internal class ServiceCollectionGrpcServerOptionsConfigurator :
        ServiceCollectionConfigurator,
        IGrpcServerOptionsConfigurator
    {
        private readonly OptionsBuilder<GrpcServerOptions> _optionsBuilder;
        public ServiceCollectionGrpcServerOptionsConfigurator(
            IServiceCollection collection)
            : base(collection)
        {
            _optionsBuilder = ServiceCollection
                .AddOptions<GrpcServerOptions>()
                .Validate(op => op.Validate());
        }

        public IGrpcServerOptionsConfigurator Configure(Action<GrpcServerOptions> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            _optionsBuilder.Configure(configure);
            return this;
        }

        public IGrpcServerOptionsConfigurator Credentials(ServerCredentials serverCredentials)
        {
            _ = serverCredentials
                ?? throw new ArgumentNullException(nameof(serverCredentials));

            _optionsBuilder.Configure(o => o.Credentials = serverCredentials);
            return this;
        }

        public IGrpcServerOptionsConfigurator Host(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentNullException(nameof(host));
            _optionsBuilder.Configure(o => o.Host = host);
            return this;
        }

        public IGrpcServerOptionsConfigurator Port(int port)
        {
            _optionsBuilder.Configure(o => o.Port = port);
            return this;
        }
    }
}
