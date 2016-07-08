using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cheers.Cqrs.Read
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
        /// <returns>Enumerable of ReadModel</returns>
        Task<IEnumerable<TReadModel>> Dispatch<TQuery, TReadModel>(TQuery query, CancellationToken cancellationToken)
            where TQuery : IQuery
            where TReadModel : IReadModel;
    }
}

