using System;
using System.Reflection;
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
                Validate(query)
                    .GetHandlers<IQueryHandler<TQuery, TReadModel>>()
                    .Single()
                    .Handle(query);
        
        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        /// <returns>ReadModel</returns>
        async Task<TReadModel> IAsyncQueryDispatcher.DispatchAsync<TQuery, TReadModel>(TQuery query)
            => 
                await Validate(query)
                        .GetHandlers<IAsyncQueryHandler<TQuery, TReadModel>>()
                        .Single()
                        .HandleAsync(query);

        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        async Task<TReadModel> ICancellableAsyncQueryDispatcher.DispatchAsync<TQuery, TReadModel>(TQuery query, CancellationToken cancellationToken)
            =>
                await Validate(query)
                        .GetHandlers<ICancellableAsyncQueryHandler<TQuery, TReadModel>>()
                        .Single()
                        .HandleAsync(query, cancellationToken);

        /// <summary>
        /// Dispatch a command
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <returns>Result</returns>
        TResult ICommandDispatcher.Dispatch<TCommand, TResult>(TCommand command)
            =>
                Validate(command)
                    .GetHandlers<ICommandHandler<TCommand, TResult>>()
                    .Single()
                    .Handle(command);

        /// <summary>
        /// Dispatch the specified command.
        /// </summary>
        /// <param name="command">Command.</param>
        void ICommandDispatcher.Dispatch<TCommand>(TCommand command)
            =>
                Validate(command)
                    .GetHandlers<ICommandHandler<TCommand>>()
                    .ToList()
                    .ForEach(h => h.Handle(command));

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <returns>Result</returns>
        async Task<TResult> IAsyncCommandDispatcher.DispatchAsync<TCommand, TResult>(TCommand command)
            => 
                await Validate(command)
                        .GetHandlers<IAsyncCommandHandler<TCommand, TResult>>()
                        .Single()
                        .HandleAsync(command);
            

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        async Task IAsyncCommandDispatcher.DispatchAsync<TCommand>(TCommand command)
        {
	        var handlers = Validate(command).GetHandlers<IAsyncCommandHandler<TCommand>>();

            foreach(var h in handlers) 
                await h.HandleAsync(command);
        }

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result</returns>
        async Task<TResult> ICancellableAsyncCommandDispatcher.DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
            =>
                await Validate(command)
                        .GetHandlers<ICancellableAsyncCommandHandler<TCommand, TResult>>()
                        .Single()
                        .HandleAsync(command, cancellationToken);

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        async Task ICancellableAsyncCommandDispatcher.DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        {
	        var handlers = Validate(command).GetHandlers<ICancellableAsyncCommandHandler<TCommand>>();

            foreach(var h in handlers) 
                await h.HandleAsync(command, cancellationToken);
        }

        /// <summary>
        /// Get handler
        /// </summary>
        /// <returns>Enumerable of handlers</returns>
        protected IEnumerable<THandler> GetHandlers<THandler>()
        {
            var handlers = Container.GetAllInstances<THandler>();

            var messageType = typeof(THandler).GenericTypeArguments.First();

            if(!handlers.Any()) throw new DispatcherException($"No handler for '{messageType.FullName}'.");

            if(!IsPublication(messageType))
                if(handlers.Count() > 1) throw new DispatcherException($"Multiple handler for '{messageType.FullName}'.");

            return handlers;
        }

        /// <summary>
        /// Message validation
        /// </summary>
        protected Dispatcher Validate<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            var validators = Container.GetAllInstances<IMessageValidator<TMessage>>();

            foreach (var validator in validators)
                validator.Validate(message);

            return this;
        }

        /// <summary>
        /// True if a type is a publication, otherwise false
        /// </summary>
        /// <returns>True if a type is a publication, otherwise false</returns>
        protected bool IsPublication(Type type)
        {
            var interfaces = type.GetTypeInfo().GetInterfaces();
            return interfaces.Contains(typeof(IPublication)) && interfaces.Contains(typeof(ICommand));
        }
    }
}
