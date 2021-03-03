using Hades.Core.Abstraction.Protocols;
using Spear.Abstraction;
using System;
using System.Threading.Tasks;

namespace Hades.Connector.Grpc.Internal.Server
{
    internal class HadesGrpcServerAgent : IHadesAgent
    {
        private readonly ISpearClient _spearClient;

        public HadesGrpcServerAgent(ISpearClient spearClient)
        {
            _spearClient = spearClient
                ?? throw new ArgumentNullException(nameof(spearClient));
        }

        public Task Start()
        {
            //using (var discovery = _spearClient.Discovery())
            //{
            //    discovery
            //        .DiscoverAllServices()
            //        .Select(service =>
            //        {
            //            var methods = service.Methods.Select(t =>
            //            {
            //                if (!t.HasClientStreaming && !t.HasServerStreaming)
            //                {
            //                    var method = new UnaryGrpcMethod<HelloRequest, HelloReply>(t.Name, request =>
            //                    {
            //                        //push to queue
            //                        return Task.FromResult(new HelloReply());
            //                    });
            //                    return method;
            //                }
            //                //else if (t.HasClientStreaming && !t.HasServerStreaming)
            //                //{
            //                //    var method = new ClientStreamingGrpcMethod<HelloRequest, HelloReply>(t.Name, request =>
            //                //    {
            //                //        return Task.FromResult(new HelloReply());
            //                //    });
            //                //    return method;
            //                //}
            //                throw new NotImplementedException();
            //            });
            //            var grpcService = new GrpcService(service.Name, methods);
            //        });
            //}
            throw new NotImplementedException();
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
