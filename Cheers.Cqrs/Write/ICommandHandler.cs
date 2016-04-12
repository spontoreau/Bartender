namespace Cheers.Cqrs.Write
{
    /// <summary>
    /// Define a command handler
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandler<in TCommand, out TResult> 
        where TCommand : ICommand
        where TResult : IResult
    {
        /// <summary>
        /// Handle a command
        /// </summary>
        /// <param name="command">Command to handle</param>
        /// <returns>Command result</returns>
        TResult Handle(TCommand command);
    }
}

