using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define an asynchronous command dispatcher
    /// </summary>
    public interface IAsyncCommandDispatcher
    {
        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <returns>Result</returns>
        Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}

