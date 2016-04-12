namespace Cheers.Cqrs.Write
{
    /// <summary>
    /// Define a command dispatcher
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatch a command
        /// </summary>
        /// <typeparam name="TCommand">Command type</typeparam>
        /// <typeparam name="TCommand">Command type</typeparam>
        /// <param name="command">Command to dispatch</param>
        /// <returns>Result</returns>
        TResult Dispatch<TCommand, TResult>(TCommand command) 
            where TCommand : ICommand
            where TResult : IResult;
    }
}

