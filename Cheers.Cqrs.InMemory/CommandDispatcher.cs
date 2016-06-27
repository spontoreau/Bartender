using System.Linq;
using System.Threading.Tasks;
using Cheers.Cqrs.Write;
using Cheers.ServiceLocator;

namespace Cheers.Cqrs.InMemory
{
    /// <summary>
    /// Command dispatcher
    /// </summary>
    public class CommandDispatcher : AbstractDispatcher, ICommandDispatcher, IAsyncCommandDispatcher
    {
        /// <summary>
        /// Create a new instance of CommandDispatcher
        /// </summary>
        /// <param name="locator">Locator</param>
        public CommandDispatcher(ILocator locator)
            : base(locator)
        {
            
        }

        /// <summary>
        /// Dispatch a command
        /// </summary>
        /// <typeparam name="TCommand">Command type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="command">Command to dispatch</param>
        /// <returns>Result</returns>
        TResult ICommandDispatcher.Dispatch<TCommand, TResult>(TCommand command)
        {
            var handlers = Locator.GetAllServices<ICommandHandler<TCommand, TResult>>().ToArray();
            Validate(handlers.Count(), command);
            return handlers.Single().Handle(command);
        }

        /// <summary>
        /// Dispatch the specified command.dispatchable
        /// </summary>
        /// <param name="command">Command.</param>
        void ICommandDispatcher.Dispatch<TCommand>(TCommand command)
        {
            var handlers = Locator.GetAllServices<ICommandHandler<TCommand>>().ToArray();
            Validate(handlers.Count(), command);
            handlers.Single().Handle(command);
        }

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <returns>Result</returns>
        async Task<TResult> IAsyncCommandDispatcher.Dispatch<TCommand, TResult>(TCommand command)
        {
            var handlers = Locator.GetAllServices<IAsyncCommandHandler<TCommand, TResult>>().ToArray();
            Validate(handlers.Count(), command);
            return await handlers.Single().Handle(command);
        }

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        async Task IAsyncCommandDispatcher.Dispatch<TCommand>(TCommand command)
        {
            var handlers = Locator.GetAllServices<IAsyncCommandHandler<TCommand>>().ToArray();
            Validate(handlers.Count(), command);
            await handlers.Single().Handle(command);
        }
    }
}

