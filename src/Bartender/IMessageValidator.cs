namespace Bartender
{
    /// <summary>
    /// Define a message validator
    /// </summary>
    public interface IMessageValidator<TMessage> where TMessage : IMessage
    {
        /// <summary>
        /// Validate the specified message.
        /// </summary>
        /// <param name="message">Message.</param>
        void Validate(TMessage message);
    }
}

