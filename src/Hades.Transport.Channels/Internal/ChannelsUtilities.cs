using CloudNativeApplicationComponents.Utils.Configurators;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hades.Transport.Channels.Internal
{
    internal static class ChannelsUtilities
    {
        public static IServiceCollection GetServiceCollection(
            object value,
            string argName)
        {
            //Todo : refactor to remove this method and fetch servicecollection in other way

            if (value is ServiceCollectionConfigurator serviceCollectionConfigurator)
            {
                return serviceCollectionConfigurator.ServiceCollection;
            }
            else
            {
                throw new ArgumentException($"The {argName} is not an instance of ServiceCollectionConfigurator.", argName);
            }
        }

    }
}
