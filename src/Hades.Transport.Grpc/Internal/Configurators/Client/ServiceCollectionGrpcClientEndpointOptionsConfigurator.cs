using CloudNativeApplicationComponents.Utils.Configurators;
using Grpc.Core;
using Hades.Transport.Grpc.Abstraction.Configurators;
using Hades.Transport.Grpc.Abstraction.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Hades.Transport.Grpc.Extensions.Internal.Configurators
{
    internal class ServiceCollectionGrpcClientEndpointOptionsConfigurator :
        ServiceCollectionConfigurator,
        IGrpcClientEndpointOptionsConfigurator
    {
        private readonly OptionsBuilder<GrpcClientEndpointOptions> _optionsBuilder;

        public ServiceCollectionGrpcClientEndpointOptionsConfigurator(
            IServiceCollection collection)
            : base(collection)
        {
            _optionsBuilder = ServiceCollection
                .AddOptions<GrpcClientEndpointOptions>()
                .Validate(op => op.Validate());
        }

        public IGrpcClientEndpointOptionsConfigurator WithBaseUri(Uri baseUri)
        {
            _optionsBuilder.Configure(op => op.BaseUri = baseUri);
            return this;
        }

        public IGrpcClientEndpointOptionsConfigurator Configure(Action<GrpcClientEndpointOptions> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            _optionsBuilder.Configure(configure);
            return this;
        }

        public IGrpcClientEndpointOptionsConfigurator WithMethodName(string methodName)
        {
            _optionsBuilder.Configure(op => op.MethodName = methodName);
            return this;
        }

        public IGrpcClientEndpointOptionsConfigurator WithServiceName(string serviceName)
        {
            _optionsBuilder.Configure(op => op.ServiceName = serviceName);
            return this;
        }

        public IGrpcClientEndpointOptionsConfigurator WithCredentials(ChannelCredentials credentials)
        {
            _optionsBuilder.Configure(op => op.Credentials = credentials);
            return this;
        }
    }
}
