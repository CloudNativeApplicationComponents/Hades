using Hades.Transport.Abstraction.Server;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hades.Transport.Internal.Server
{
    internal class HadesServerAgentManager : BackgroundService, IHadesServerAgentManager
    {
        private readonly List<IHadesServerAgent> _agents;
        public HadesServerAgentManager()
        {
            _agents = new List<IHadesServerAgent>();
        }
        public void AddAgent(IHadesServerAgent agent)
        {
            _ = agent
                ?? throw new ArgumentNullException(nameof(agent));
            _agents.Add(agent);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var agent in _agents)
            {
                await agent.StartAsync(stoppingToken);
                stoppingToken.Register(async () => { await agent.StopAsync(); });
            }
        }
    }
}
