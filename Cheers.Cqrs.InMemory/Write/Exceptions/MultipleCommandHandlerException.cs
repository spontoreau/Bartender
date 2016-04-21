using System;

namespace Cheers.Cqrs.InMemory.Exceptions
{
    /// <summary>
    /// Multiple command handler exception
    /// </summary>
    public class MultipleCommandHandlerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Cheers.Cqrs.InMemory.Exceptions.MultipleCommandHandlerException"/> class.
        /// </summary>
        /// <param name="commandType">Command type.</param>
        public MultipleCommandHandlerException(Type commandType)
            : base($"Multiple implementations of handlers for command '{commandType.Name}'.")
        {

        }
    }
}

