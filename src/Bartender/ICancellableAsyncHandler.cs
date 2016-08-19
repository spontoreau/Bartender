using System.Threading;
using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define a cancellable asynchronous handler
    /// </summary>
    public interface ICancellableAsyncHandler<TMessage, TResult>
        where TMessage : IMessage
    {
        /// <summary>
        /// Handle a message
        /// </summary>
        /// <param name="message">Message to handle</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Message result</returns>
        Task<TResult> HandleAsync(TMessage message, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Define a cancellable asynchronous fire and forget handler
    /// </summary>
    public interface ICancellableAsyncHandler<TMessage>
        where TMessage : IMessage
    {
        /// <summary>
        /// Handle a message
        /// </summary>
        /// <param name="message">Message to handle</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
}

