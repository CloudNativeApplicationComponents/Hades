using Hades.Edge.Abstraction.Services;
using Hades.Edge.Spear.Integration.Internal;
using Hades.Edge.Spear.Integration.Internal.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spear.Client.Builder;
using System;

namespace Hades.Edge.Spear.Integration.Builder
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSpearServiceDicovery(this IServiceCollection services, Action<SpearHttpClientOption> options)
        {
            services.AddSpareHttpClient(options);
            services.AddTransient<IDynamicServiceFactory, DynamicServiceFactory>();
            services.AddTransient<IDynamicServiceDiscovery, SpearDynamicServiceProvider>();

            return services;
        }

        public static IServiceCollection AddSpearServiceDicovery(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSpareHttpClient(configuration);
            services.AddTransient<IDynamicServiceFactory, DynamicServiceFactory>();
            services.AddTransient<IDynamicServiceDiscovery, SpearDynamicServiceProvider>();

            return services;
        }
    }
}