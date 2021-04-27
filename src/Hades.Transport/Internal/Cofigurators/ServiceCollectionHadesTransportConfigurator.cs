using CloudNativeApplicationComponents.Utils.Configurators;
using Hades.Transport.Abstraction.Configurators;
using Hades.Transport.Abstraction.Server;
using Hades.Transport.Internal.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Hades.Transport.Internal.Configurators
{
    internal class ServiceCollectionHadesTransportConfigurator : 
        ServiceCollectionConfigurator,
        IHadesTransportConfigurator
    {
        public ServiceCollectionHadesTransportConfigurator(
            IServiceCollection collection)
            : base(collection)
        {
            ServiceCollection
                .TryAddSingleton<IHadesServerAgentManager, HadesServerAgentManager>();
            ServiceCollection
                .AddHostedService(sp => sp.GetRequiredService<IHadesServerAgentManager>());
        }
    }
}
