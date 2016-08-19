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
    public class Dispatcher : IDispatcher, 
                              IAsyncDispatcher, 
                              ICancellableAsyncDispatcher
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
        /// Dispatch a message
        /// </summary>
        /// <param name="message">Message to dispatch</param>
        /// <returns>Result</returns>
        TResult IDispatcher.Dispatch<TMessage, TResult>(TMessage message)
            =>
                Validate(message)
                    .GetHandlers<IHandler<TMessage, TResult>>()
                    .Single()
                    .Handle(message);

        /// <summary>
        /// Dispatch the specified message.
        /// </summary>
        /// <param name="message">Message.</param>
        void IDispatcher.Dispatch<TMessage>(TMessage message)
        {
            var handlers = Validate(message).GetHandlers<IHandler<TMessage>>();
            
            foreach(var h in handlers)
                h.Handle(message);
        }

        /// <summary>
        /// Dispatch a message asynchronously.
        /// </summary>
        /// <param name="message">Message to dispatch</param>
        /// <returns>Result</returns>
        async Task<TResult> IAsyncDispatcher.DispatchAsync<TMessage, TResult>(TMessage message)
            => 
                await Validate(message)
                        .GetHandlers<IAsyncHandler<TMessage, TResult>>()
                        .Single()
                        .HandleAsync(message);

        /// <summary>
        /// Dispatch a message asynchronously.
        /// </summary>
        /// <param name="message">Message to dispatch</param>
        async Task IAsyncDispatcher.DispatchAsync<TMessage>(TMessage message)
        {
	        var handlers = Validate(message).GetHandlers<IAsyncHandler<TMessage>>();

            foreach(var h in handlers) 
                await h.HandleAsync(message);
        }

        /// <summary>
        /// Dispatch a message asynchronously.
        /// </summary>
        /// <param name="message">Message to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result</returns>
        async Task<TResult> ICancellableAsyncDispatcher.DispatchAsync<TMessage, TResult>(TMessage message, CancellationToken cancellationToken)
            =>
                await Validate(message)
                        .GetHandlers<ICancellableAsyncHandler<TMessage, TResult>>()
                        .Single()
                        .HandleAsync(message, cancellationToken);

        /// <summary>
        /// Dispatch a message asynchronously.
        /// </summary>
        /// <param name="message">Message to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        async Task ICancellableAsyncDispatcher.DispatchAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        {
	        var handlers = Validate(message).GetHandlers<ICancellableAsyncHandler<TMessage>>();

            foreach(var h in handlers) 
                await h.HandleAsync(message, cancellationToken);
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
        protected bool IsPublication(Type type) => type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IPublication));
    }
}
