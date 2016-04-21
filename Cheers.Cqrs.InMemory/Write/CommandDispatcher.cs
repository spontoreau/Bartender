using System.Linq;
using Cheers.Cqrs.Write;
using Cheers.ServiceLocator;
using Cheers.Cqrs.InMemory.Exceptions;

namespace Cheers.Cqrs.InMemory.Write
{
    /// <summary>
    /// Command dispatcher
    /// </summary>
    public class CommandDispatcher : ICommandDispatcher
    {
        /// <summary>
        /// Locator
        /// </summary>
        private ILocator Locator { get; }

        /// <summary>
        /// Create a new instance of CommandDispatcher
        /// </summary>
        /// <param name="locator">Locator</param>
        public CommandDispatcher(ILocator locator)
        {
            Locator = locator;
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

            if (handlers.Length > 1)
            {
                throw new MultipleCommandHandlerException(typeof(TCommand));
            }
            if (!handlers.Any())
            {
                throw new NoCommandHandlerException(typeof(TCommand));
            }

            return handlers.Single().Handle(command);
        }
    }
}

