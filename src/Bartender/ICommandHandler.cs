namespace Bartender
{
    /// <summary>
    /// Define a command handler
    /// </summary>
    public interface ICommandHandler<TCommand> 
        where TCommand : ICommand
    {
        /// <summary>
        /// Handle a command
        /// </summary>
        /// <param name="command">Command to handle</param>
        void Handle(TCommand command);
    }

    /// <summary>
    /// Define a command handler
    /// </summary>
    public interface ICommandHandler<in TCommand, out TResult> 
        where TCommand : ICommand
    {
        /// <summary>
        /// Handle a command
        /// </summary>
        /// <param name="command">Command to handle</param>
        /// <returns>Command result</returns>
        TResult Handle(TCommand command);
    }
}

