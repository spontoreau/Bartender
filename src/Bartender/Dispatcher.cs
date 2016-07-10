using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Dispatcher.
    /// </summary>
    public class Dispatcher : IQueryDispatcher, IAsyncQueryDispatcher
    {
        /// <summary>
        /// Dependency container.
        /// </summary>
        public IDependencyContainer Container { get; }

        /// <summary>
        /// Initializes a new instance of the Dispatcher class.
        /// </summary>
        /// <param name="container">Dependency container.</param>
        public Dispatcher(IDependencyContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Dispatch the specified query.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>ReadModel</returns>
        TReadModel IQueryDispatcher.Dispatch<TQuery, TReadModel>(TQuery query)
        {
            var handlers = Container.GetAllInstances<IQueryHandler<TQuery, TReadModel>>();

            if(!handlers.Any()) throw new DispatcherException($"No handler for '{typeof(TQuery)}'.");
            if(handlers.Count() > 1) throw new DispatcherException($"Multiple handler for '{typeof(TQuery)}'.");

            return handlers.Single().Handle(query);
        }

        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        async Task<TReadModel> IAsyncQueryDispatcher.DispatchAsync<TQuery, TReadModel>(TQuery query)
        {
            var handlers = Container.GetAllInstances<IAsyncQueryHandler<TQuery, TReadModel>>();
            return await handlers.Single().HandleAsync(query);
        }
    }
}
