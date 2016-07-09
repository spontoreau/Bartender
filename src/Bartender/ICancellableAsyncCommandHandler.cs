using System.Threading;
using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define a cancellable asynchronous command handler
    /// </summary>
    public interface ICancellableAsyncCommandHandler<TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// Handle a command
        /// </summary>
        /// <param name="command">Command to handle</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task HandleAsync(TCommand command, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Define a cancellable asynchronous command handler
    /// </summary>
    public interface ICancellableAsyncCommandHandler<TCommand, TResult>
        where TCommand : ICommand
    {
        /// <summary>
        /// Handle a command
        /// </summary>
        /// <param name="command">Command to handle</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Command result</returns>
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}

