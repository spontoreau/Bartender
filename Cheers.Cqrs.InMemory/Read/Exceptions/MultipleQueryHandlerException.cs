using System;

namespace Cheers.Cqrs.InMemory.Exceptions
{
    /// <summary>
    /// Multiple query handler exception
    /// </summary>
    public class MultipleQueryHandlerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Cheers.Cqrs.InMemory.Exceptions.MultipleQueryHandlerException"/> class.
        /// </summary>
        /// <param name="queryType">Query type.</param>
        public MultipleQueryHandlerException(Type queryType)
            : base($"Multiple implementations of handlers for command '{queryType.Name}'.")
        {

        }
    }
}

