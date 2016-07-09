using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define a cancellable asynchronous query handler.
    /// </summary>
    public interface ICancellableAsyncQueryHandler<TQuery, TReadModel>
        where TQuery : IQuery
    {
        /// <summary>
        /// Handle a query asynchronously
        /// </summary>
        /// <param name="query">Query to handle</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ReadModel</returns>
        Task<TReadModel> HandleAsync(TQuery query, CancellationToken cancellationToken);
    }
}

