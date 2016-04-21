using Cheers.Cqrs.Read;
using System.Collections.Generic;
using System.Linq;
using Cheers.Cqrs.InMemory.Exceptions;
using Cheers.ServiceLocator;

namespace Cheers.Cqrs.InMemory.Read
{
    /// <summary>
    /// Query dispatcher
    /// </summary>
    public class QueryDispatcher : IQueryDispatcher
    {
        /// <summary>
        /// Gets the locator.
        /// </summary>
        /// <value>The locator.</value>
        private ILocator Locator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cheers.Cqrs.InMemory.Read.QueryDispatcher"/> class.
        /// </summary>
        /// <param name="locator">Locator.</param>
        public QueryDispatcher(ILocator locator)
        {
            Locator = locator;
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

            if (handlers.Length > 1)
            {
                throw new MultipleQueryHandlerException(typeof(TQuery));
            }
            if (!handlers.Any())
            {
                throw new NoQueryHandlerException(typeof(TQuery));
            }

            return handlers.Single().Handle(query);
        }
    }
}

