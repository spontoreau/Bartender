using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cheers.Cqrs.Write
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
        Task<TResult> Dispatch<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand
            where TResult : IResult;

        /// <summary>
        /// Dispatch a command asynchronously.
        /// </summary>
        /// <param name="command">Command to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand;
    }
}

