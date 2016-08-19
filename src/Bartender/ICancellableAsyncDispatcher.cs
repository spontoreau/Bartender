using System.Threading;
using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define a cancellable asynchronous dispatcher.
    /// </summary>
    public interface ICancellableAsyncDispatcher
    {
        /// <summary>
        /// Dispatch a message asynchronously.
        /// </summary>
        /// <param name="message">Message to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result</returns>
        Task<TResult> DispatchAsync<TMessage, TResult>(TMessage message, CancellationToken cancellationToken)
            where TMessage : IMessage;

        /// <summary>
        /// Dispatch a message asynchronously.
        /// </summary>
        /// <param name="message">Message to dispatch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DispatchAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
            where TMessage : IMessage;
    }
}

