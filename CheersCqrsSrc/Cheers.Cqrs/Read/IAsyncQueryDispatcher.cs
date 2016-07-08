using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheers.Cqrs.Read
{
    /// <summary>
    /// Define an asynchronous query dispatcher
    /// </summary>
    public interface IAsyncQueryDispatcher
    {
        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        /// <returns>Enumerable of ReadModel</returns>
        Task<IEnumerable<TReadModel>> Dispatch<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery
            where TReadModel : IReadModel;
    }
}

