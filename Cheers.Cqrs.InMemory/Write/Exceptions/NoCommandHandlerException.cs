using System;

namespace Cheers.Cqrs.InMemory.Exceptions
{
    /// <summary>
    /// No command handler exception
    /// </summary>
    public class NoCommandHandlerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cheers.Cqrs.InMemory.Exceptions.NoCommandHandlerException"/> class.
        /// </summary>
        /// <param name="commandType">Command type.</param>
        public NoCommandHandlerException(Type commandType)
            : base($"No implementation of handlers for command '{commandType.Name}'.")
        {

        }
    }
}

