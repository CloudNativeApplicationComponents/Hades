using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Hades.Broker;
using Hades.Transport.Channels;
using Hades.Broker.Transport.Integration;
using Hades.Transport.Grpc;
using Grpc.Core;

namespace Hades.Transport.Sample
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
             .ConfigureServices((hostContext, services) =>
             {

             }).UseHadesBroker(c =>
             {
                 c.UseTransport(epcg =>
                 {
                     epcg.UseGrpc(gcg =>
                     {
                         gcg.UseServer(sgcg =>
                         {
                             sgcg.WithOptions(op =>
                             {
                                 op.Host("127.0.01")
                                 .Port(8090)
                                 .Credentials(ServerCredentials.Insecure);
                             })
                             .AddEndpoint(cg =>
                             {
                                 cg.WithOptions(op =>
                                 {
                                     op.WithServiceName("ServiceName1")
                                     .WithMethodName("MethodName1");
                                 }).AddUnaryChannel(cg =>
                                 {
                                     cg.AddConsumer(c.Builder, ccg =>
                                     {
                                         ccg.WithTopic("Topic1")
                                         .WithGroup("Group1");
                                     }).AddProducer(c.Builder, ccg =>
                                     {
                                         ccg.WithTopic("Topic1");
                                     });
                                 });
                             });
                         })
                         .UseClient(cgcg =>
                         {
                             cgcg.AddEndpoint(cg =>
                             {
                                 cg.WithOptions(op =>
                                 {
                                     op.WithBaseUri(new Uri("http://127.0.01:8091"))
                                     .WithServiceName("ServiceName1")
                                     .WithMethodName("MethodName1")
                                     .WithCredentials(ChannelCredentials.Insecure);
                                 }).AddUnaryChannel(cg =>
                                 {
                                     cg.AddConsumer(c.Builder, ccg =>
                                     {
                                         ccg.WithTopic("Topic1")
                                         .WithGroup("Group1");
                                     }).AddProducer(c.Builder, ccg =>
                                     {
                                         ccg.WithTopic("Topic1");
                                     });
                                 });
                             });
                         });
                     });
                 });
             }).ConfigureLogging(b =>
             {
                 b.AddConsole();
             }).Build();

            await host.RunAsync();
        }
    }
}
