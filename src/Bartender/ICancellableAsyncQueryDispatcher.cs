using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define a cancellable asynchronous query dispatcher.
    /// </summary>
    public interface ICancellableAsyncQueryDispatcher
    {
        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<TReadModel> DispatchAsync<TQuery, TReadModel>(TQuery query, CancellationToken cancellationToken)
            where TQuery : IQuery;
    }
}

