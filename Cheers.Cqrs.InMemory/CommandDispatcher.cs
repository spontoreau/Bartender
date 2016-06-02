using System;
using System.Linq;
using System.Threading.Tasks;
using Cheers.Cqrs.Write;
using Cheers.ServiceLocator;

namespace Cheers.Cqrs.InMemory
{
    /// <summary>
    /// Command dispatcher
    /// </summary>
    public class CommandDispatcher : AbstractDispatcher, ICommandDispatcher
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
        public TResult Dispatch<TCommand, TResult>(TCommand command)
            where TCommand : ICommand
            where TResult : IResult
        {
            var handlers = Locator.GetAllServices<ICommandHandler<TCommand, TResult>>().ToArray();
            Validate(handlers.Count(), command);
            return handlers.Single().Handle(command);
        }

        /// <summary>
        /// Dispatch the specified command.dispatchable
        /// </summary>
        /// <param name="command">Command.</param>
        public void Dispatch<TCommand>(TCommand command) 
            where TCommand : ICommand
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
        public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command)
            where TCommand : ICommand
            where TResult : IResult
        {
            var handlers = Locator.GetAllServices<IAsyncCommandHandler<TCommand, TResult>>().ToArray();
            Validate(handlers.Count(), command);
            return await handlers.Single().Handle(command);
        }

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        public Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            /*var handlers = Locator.GetAllServices<IAsyncCommandHandler<TCommand>>().ToArray();
            Validate(handlers.Count(), command);
            await handlers.Single().Handle(command);*/
            throw new NotImplementedException();
        }
    }
}

