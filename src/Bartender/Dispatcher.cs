using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Dispatcher.
    /// </summary>
    public class Dispatcher : IQueryDispatcher, 
                              IAsyncQueryDispatcher, 
                              ICancellableAsyncQueryDispatcher,
                              ICommandDispatcher,
                              IAsyncCommandDispatcher,
                              ICancellableAsyncCommandDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the Dispatcher class.
        /// </summary>
        /// <param name="container">Dependency container.</param>
        public Dispatcher(IDependencyContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Dependency container.
        /// </summary>
        protected IDependencyContainer Container { get; }

        /// <summary>
        /// Dispatch the specified query.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>ReadModel</returns>
        TReadModel IQueryDispatcher.Dispatch<TQuery, TReadModel>(TQuery query)
            => 
                GetHandlers<IQueryHandler<TQuery, TReadModel>>()
                    .Single()
                    .Handle(query);
        
        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        /// <returns>ReadModel</returns>
        async Task<TReadModel> IAsyncQueryDispatcher.DispatchAsync<TQuery, TReadModel>(TQuery query)
            => 
                await GetHandlers<IAsyncQueryHandler<TQuery, TReadModel>>()
                        .Single()
                        .HandleAsync(query);

        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        async Task<TReadModel> ICancellableAsyncQueryDispatcher.DispatchAsync<TQuery, TReadModel>(TQuery query, CancellationToken cancellationToken)
            =>
                await GetHandlers<ICancellableAsyncQueryHandler<TQuery, TReadModel>>()
                        .Single()
                        .HandleAsync(query, cancellationToken);

        /// <summary>
        /// Dispatch a command
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <returns>Result</returns>
        TResult ICommandDispatcher.Dispatch<TCommand, TResult>(TCommand command)
            =>
                GetHandlers<ICommandHandler<TCommand, TResult>>()
                    .Single()
                    .Handle(command);

        /// <summary>
        /// Dispatch the specified command.
        /// </summary>
        /// <param name="command">Command.</param>
        void ICommandDispatcher.Dispatch<TCommand>(TCommand command)
            =>
                GetHandlers<ICommandHandler<TCommand>>()
                    .Single()
                    .Handle(command);

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <returns>Result</returns>
        Task<TResult> IAsyncCommandDispatcher.DispatchAsync<TCommand, TResult>(TCommand command)
            => 
                GetHandlers<IAsyncCommandHandler<TCommand, TResult>>()
                    .Single()
                    .HandleAsync(command);
            

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        Task IAsyncCommandDispatcher.DispatchAsync<TCommand>(TCommand command)
            =>
                GetHandlers<IAsyncCommandHandler<TCommand>>()
                    .Single()
                    .HandleAsync(command);

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result</returns>
        Task<TResult> ICancellableAsyncCommandDispatcher.DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
            =>
                GetHandlers<ICancellableAsyncCommandHandler<TCommand, TResult>>()
                    .Single()
                    .HandleAsync(command, cancellationToken);

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task ICancellableAsyncCommandDispatcher.DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
            => 
                GetHandlers<ICancellableAsyncCommandHandler<TCommand>>()
                    .Single()
                    .HandleAsync(command, cancellationToken);

        /// <summary>
        /// Get handler
        /// </summary>
        /// <returns>Enumerable of handlers</returns>
        protected IEnumerable<THandler> GetHandlers<THandler>()
        {
            var handlers = Container.GetAllInstances<THandler>();

            var messageType = typeof(THandler).GenericTypeArguments.First().FullName;

            if(!handlers.Any()) throw new DispatcherException($"No handler for '{messageType}'.");
            if(handlers.Count() > 1) throw new DispatcherException($"Multiple handler for '{messageType}'.");

            return handlers;
        }
    }
}
