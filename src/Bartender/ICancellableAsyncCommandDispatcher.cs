using System.Threading;
using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define a cancellable asynchronous command dispatcher.
    /// </summary>
    public interface ICancellableAsyncCommandDispatcher
    {
        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result</returns>
        Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand;

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand;
    }
}

