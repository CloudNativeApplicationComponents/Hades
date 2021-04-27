using Hades.Edge.Abstraction.Services;
using Spear.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Spear.Client.Services.ServiceCatalogDefinition;

namespace Hades.Edge.Spear.Integration.Internal.Services
{
    internal class SpearDynamicServiceProvider : IDynamicServiceDiscovery
    {
        private readonly ISpearDiscoveryClient _spearDiscoveryClient;
        private readonly IDynamicServiceFactory _dynamicServiceFactory;

        public SpearDynamicServiceProvider(ISpearDiscoveryClient spearDiscoveryClient,
                                            IDynamicServiceFactory dynamicServiceFactory)
        {
            _spearDiscoveryClient =
                spearDiscoveryClient ??
                throw new ArgumentNullException(nameof(spearDiscoveryClient));

            _dynamicServiceFactory =
                dynamicServiceFactory ??
                throw new ArgumentNullException(nameof(dynamicServiceFactory));
        }

        public async Task<IEnumerable<IDynamicService>> GetDynamicServices()
            => await GetDynamicServices(string.Empty);

        public async Task<IEnumerable<IDynamicService>> GetDynamicServices(string dataPlane)
            => CreatDynamicServices(
                await _spearDiscoveryClient.GetServiceCatalogDefinition(
                    new ServiceCatalogDefinitionFilter()
                    {
                        DataPlane = dataPlane
                    }));

        protected virtual IEnumerable<IDynamicService> CreatDynamicServices(
            IEnumerable<ServiceCatalogDefinition> serviceCatalogDefinitions)
        {
            var dynamicServices = new List<IDynamicService>();

            foreach (var serviceCatalogDefinition in serviceCatalogDefinitions ?? Enumerable.Empty<ServiceCatalogDefinition>())
                foreach (var serviceDefinition in serviceCatalogDefinition.Services ?? Enumerable.Empty<ServiceDefinition>())
                {
                    dynamicServices.Add(
                        _dynamicServiceFactory.CreateDynamicService(
                            serviceCatalogDefinition.Name,
                            serviceCatalogDefinition.DataPlane,
                            serviceDefinition.Name,
                            serviceDefinition.MethodType));
                }

            return dynamicServices;
        }
    }
}
