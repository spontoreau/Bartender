using System.Threading.Tasks;

namespace Cheers.Cqrs.Write
{
    /// <summary>
    /// Define an asynchronous command handler
    /// </summary>
    public interface IAsyncCommandHandler<TCommand> 
        where TCommand : ICommand
    {
        /// <summary>
        /// Handle a command
        /// </summary>
        /// <param name="command">Command to handle</param>
        Task Handle(TCommand command);
    }

    /// <summary>
    /// Define an asynchronous command handler
    /// </summary>
    public interface IAsyncCommandHandler<TCommand, TResult> 
        where TCommand : ICommand
        where TResult : IResult
    {
        /// <summary>
        /// Handle a command
        /// </summary>
        /// <param name="command">Command to handle</param>
        /// <returns>Command result</returns>
        Task<TResult> Handle(TCommand command);
    }
}

