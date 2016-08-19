namespace Bartender
{
    /// <summary>
    /// Define a handler
    /// </summary>
    public interface IHandler<in TMessage, out TResult> 
        where TMessage : IMessage
    {
        /// <summary>
        /// Handle a message
        /// </summary>
        /// <param name="message">Message to handle</param>
        /// <returns>Message result</returns>
        TResult Handle(TMessage message);
    }

    /// <summary>
    /// Define a fire and forget handler
    /// </summary>
    public interface IHandler<TMessage> 
        where TMessage : IMessage
    {
        /// <summary>
        /// Handle a message
        /// </summary>
        /// <param name="message">Message to handle</param>
        void Handle(TMessage message);
    }
}

