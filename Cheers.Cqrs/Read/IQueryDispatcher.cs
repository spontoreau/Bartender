using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cheers.Cqrs.Read
{
    /// <summary>
    /// Define a query dispatcher
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Dispatch a query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TReadModel">ReadModel type</typeparam>
        /// <param name="query">Query to dispatch</param>
        /// <returns>Enumerable of ReadModel</returns>
        IEnumerable<TReadModel> Dispatch<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery
            where TReadModel : IReadModel;

        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TReadModel">ReadModel type</typeparam>
        /// <param name="query">Query to dispatch</param>
        /// <returns>Enumerable of ReadModel</returns>
        Task<IEnumerable<TReadModel>> DispatchAsync<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery
            where TReadModel : IReadModel;
    }
}

