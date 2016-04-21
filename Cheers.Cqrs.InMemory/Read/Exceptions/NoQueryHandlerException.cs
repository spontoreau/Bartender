using System;

namespace Cheers.Cqrs.InMemory.Exceptions
{
    /// <summary>
    /// No query handler exception
    /// </summary>
    public class NoQueryHandlerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cheers.Cqrs.InMemory.Exceptions.NoQueryHandlerException"/> class.
        /// </summary>
        /// <param name="queryType">Query type.</param>
        public NoQueryHandlerException(Type queryType)
            : base($"No implementation of handlers for command '{queryType.Name}'.")
        {

        }
    }
}

