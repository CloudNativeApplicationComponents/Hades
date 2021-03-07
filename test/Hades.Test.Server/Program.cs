using Hades.Core.Abstraction.Protocols;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Test.Server
{
    class Program
    {
        const int Port = 30051;

        async static Task Main(string[] args)
        {
            var builder = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) => { })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();
                services.AddSingleton<IHostedService, HadesTestService>();
                //services.AddScoped<IHadesAgent, HadesGrpcServerAgent>();

            })
            .ConfigureLogging((hostingContext, logging) => logging.AddConsole());


            await builder.RunConsoleAsync();
        }


        public class HadesTestService : IHostedService
        {
            private readonly IHadesAgent _hadesAgent;

            public HadesTestService(IHadesAgent hadesAgent)
            {
                _hadesAgent = hadesAgent;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                await _hadesAgent.Start();
            }

            public async Task StopAsync(CancellationToken cancellationToken)
            {
                await _hadesAgent.Stop();
            }
        }
    }
}
