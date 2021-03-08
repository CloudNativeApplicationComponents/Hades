using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hades.Edge.Abstraction.Connectors
{
    public interface IHadesConnector
    {
        Task BeginSaga();
        Task CommitSaga();
        Task RollbackSaga();

        Task SendEvent<T>(T message);
        Task<T> SendUnary<T>(T message);
        Task<T> SendClientStreaming<T>(IAsyncEnumerable<T> message);
        IAsyncEnumerable<T> SendServerStreaming<T>(T message);
        IAsyncEnumerable<T> SendDeuplexStreaming<T>(IAsyncEnumerable<T> message);
    }
}
