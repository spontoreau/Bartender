using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define an asynchronous handler
    /// </summary>
    public interface IAsyncHandler<TMessage, TResult> 
        where TMessage : IMessage
    {
        /// <summary>
        /// Handle a message
        /// </summary>
        /// <param name="message">Message to handle</param>
        /// <returns>Message result</returns>
        Task<TResult> HandleAsync(TMessage message);
    }

    /// <summary>
    /// Define a fire and forget asynchronous handler
    /// </summary>
    public interface IAsyncHandler<TMessage> 
        where TMessage : IMessage
    {
        /// <summary>
        /// Handle a message
        /// </summary>
        /// <param name="message">Message to handle</param>
        Task HandleAsync(TMessage message);
    }
}
