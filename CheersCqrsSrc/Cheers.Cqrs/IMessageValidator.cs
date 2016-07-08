namespace Cheers.Cqrs
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

        /// <summary>
        /// True if the message is valid, otherwise false
        /// </summary>
        /// <param name="message">Message.</param> 
        /// <returns>True if the message is valid, otherwise false</returns>
        bool IsValid(TMessage message);
    }
}

