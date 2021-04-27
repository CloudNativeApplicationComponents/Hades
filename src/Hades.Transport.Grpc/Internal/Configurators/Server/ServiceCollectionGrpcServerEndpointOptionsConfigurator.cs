using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Grpc.Abstraction.Configurators;
using Hades.Transport.Grpc.Abstraction.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Hades.Transport.Grpc.Extensions.Internal.Configurators
{
    internal class ServiceCollectionGrpcServerEndpointOptionsConfigurator :
        ServiceCollectionConfigurator,
        IGrpcServerEndpointOptionsConfigurator
    {
        private readonly OptionsBuilder<GrpcServerEndpointOptions> _optionsBuilder;

        public ServiceCollectionGrpcServerEndpointOptionsConfigurator(
            IServiceCollection collection)
            : base(collection)
        {
            _optionsBuilder = ServiceCollection
                .AddOptions<GrpcServerEndpointOptions>()
                .Validate(op => op.Validate());
        }

        public IGrpcServerEndpointOptionsConfigurator Configure(Action<GrpcServerEndpointOptions> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            _optionsBuilder.Configure(configure);
            return this;
        }

        public IGrpcServerEndpointOptionsConfigurator WithMethodName(string methodName)
        {
            _optionsBuilder.Configure(op => op.MethodName = methodName);
            return this;
        }

        public IGrpcServerEndpointOptionsConfigurator WithServiceName(string serviceName)
        {
            _optionsBuilder.Configure(op => op.ServiceName = serviceName);
            return this;
        }
    }
}
