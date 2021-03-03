using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hades.Core.Abstraction.Connector.Server
{
    public interface IHadesClientApi
    {
        Task BeginSaga();
        Task CommitSaga();
        Task RollbackSaga();

        Task RaiseEvent<T>(T message);
        Task<T> Unary<T>(T message);
        Task<T> ClientStreaming<T>(IAsyncEnumerable<T> message);
        IAsyncEnumerable<T> ServerStreaming<T>(T message);
        IAsyncEnumerable<T> DeuplexStreaming<T>(IAsyncEnumerable<T> message);
    }
}
