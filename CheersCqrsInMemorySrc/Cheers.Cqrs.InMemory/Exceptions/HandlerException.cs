using Cheers.Cqrs.Read;
using System;

namespace Cheers.Cqrs.InMemory.Exceptions
{
    public class HandlerException : Exception
    {
        protected HandlerException(string format, IDispatchable dispatchable)
            : base(string.Format(format, dispatchable is IQuery ? "query" : "command", dispatchable.GetType().Name))
        {
        }
    }
}

