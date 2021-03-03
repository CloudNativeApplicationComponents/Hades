using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hades.Edge.Abstraction.Features
{
    public interface IExposedServiceFeature : IHadesFeature
    {
        Task SendEvent<T>(T message);
        Task<T> SendUnary<T>(T message);
        Task<T> SendClientStreaming<T>(IAsyncEnumerable<T> message);
        IAsyncEnumerable<T> SendServerStreaming<T>(T message);
        IAsyncEnumerable<T> SendDeuplexStreaming<T>(IAsyncEnumerable<T> message);
    }
}
