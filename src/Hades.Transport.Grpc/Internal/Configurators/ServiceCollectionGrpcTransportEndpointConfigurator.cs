using Hades.Transport.Grpc.Abstraction.Configurators;
using Hades.Transport.Grpc.Abstraction;
using Hades.Transport.Grpc.Abstraction.Serialization;
using Hades.Transport.Grpc.Internal.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Hades.Transport.Grpc.Extensions.Internal.Configurators;
using CloudNativeApplicationComponents.Utils.Configurators;

namespace Hades.Transport.Grpc.Internal.Configurators
{
    internal class ServiceCollectionGrpcTransportEndpointConfigurator :
        ServiceCollectionConfigurator,
        IGrpcTransportEndpointConfigurator
    {
        public ServiceCollectionGrpcTransportEndpointConfigurator(
            IServiceCollection serviceCollection)
            : base(serviceCollection)
        {
            ServiceCollection
                .TryAddSingleton<IGrpcTransportFactory, GrpcTransportFactory>();
            ServiceCollection
                .TryAddSingleton<IGrpcMethodFactory, GrpcMethodFactory>();

            ServiceCollection
                .TryAddSingleton<GenericProtobufSerializer>();
            ServiceCollection
                .TryAddSingleton<IGenericProtobufSerializer>(sp => sp.GetRequiredService<GenericProtobufSerializer>());
            ServiceCollection
                .TryAddSingleton<IGenericProtobufDeserializer>(sp => sp.GetRequiredService<GenericProtobufSerializer>());


            ServiceCollection
                .TryAddSingleton<ProtobufMessageSerializer>();
            ServiceCollection
                .TryAddSingleton<IProtobufEnvelopeDeserializer>(sp => sp.GetRequiredService<ProtobufMessageSerializer>());
            ServiceCollection
                .TryAddSingleton<IProtobufEnvelopeSerializer>(sp => sp.GetRequiredService<ProtobufMessageSerializer>());
            ServiceCollection
                .TryAddSingleton<IRpcExceptionEnvelopeDeserializer>(sp => sp.GetRequiredService<ProtobufMessageSerializer>());
            ServiceCollection
                .TryAddSingleton<IRpcExceptionEnvelopeSerializer>(sp => sp.GetRequiredService<ProtobufMessageSerializer>());
        }

        public IGrpcTransportEndpointConfigurator UseClient(Action<IGrpcTransportClientConfigurator> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var configurator = new ServiceCollectionGrpcClientTransportConfigurator(ServiceCollection);
            configure?.Invoke(configurator);
            return this;
        }

        public IGrpcTransportEndpointConfigurator UseServer(Action<IGrpcTransportServerConfigurator> configure)
        {
            _ = configure
                ?? throw new ArgumentNullException(nameof(configure));

            var configurator = new ServiceCollectionGrpcServerTransportConfigurator(ServiceCollection);
            configure?.Invoke(configurator);
            return this;
        }
    }
}
