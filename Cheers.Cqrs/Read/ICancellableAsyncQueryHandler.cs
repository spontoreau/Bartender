using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cheers.Cqrs.Read
{
    /// <summary>
    /// Define a cancellable asynchronous query handler.
    /// </summary>
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TReadModel">ReadModel type</typeparam>
    public interface ICancellableAsyncQueryHandler<TQuery, TReadModel>
        where TQuery : IQuery
        where TReadModel : IReadModel
    {
        /// <summary>
        /// Handle a query asynchronously
        /// </summary>
        /// <param name="query">Query to handle</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Enumerable of ReadModel</returns>
        Task<IEnumerable<TReadModel>> Handle(TQuery query, CancellationToken cancellationToken);
    }
}

