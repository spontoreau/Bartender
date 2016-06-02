using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cheers.Cqrs.Read;
using Cheers.ServiceLocator;
using Cheers.Cqrs.InMemory.Exceptions;

namespace Cheers.Cqrs.InMemory
{
    /// <summary>
    /// Query dispatcher
    /// </summary>
    public class QueryDispatcher : AbstractDispatcher, IQueryDispatcher
    {
        /// <summary>
        /// Create a new instance of QueryDispatcher
        /// </summary>
        /// <param name="locator">Locator</param>
        public QueryDispatcher(ILocator locator)
            : base(locator)
        {
            
        }

        /// <summary>
        /// Dispatch the specified query.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TReadModel">ReadModel type</typeparam>
        public IEnumerable<TReadModel> Dispatch<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery
            where TReadModel : IReadModel
        {
            var handlers = Locator.GetAllServices<IQueryHandler<TQuery, TReadModel>>().ToArray();
            Validate(handlers.Count(), query);
            return handlers.Single().Handle(query);
        }

        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TReadModel">ReadModel type</typeparam>
        /// <param name="query">Query to dispatch</param>
        /// <returns>Enumerable of ReadModel</returns>
        public async Task<IEnumerable<TReadModel>> DispatchAsync<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery
            where TReadModel : IReadModel
        {
            var handlers = Locator.GetAllServices<IAsyncQueryHandler<TQuery, TReadModel>>().ToArray();
            Validate(handlers.Count(), query);
            return await handlers.Single().Handle(query);
        }
    }
}

