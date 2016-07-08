namespace Cheers.Cqrs.InMemory.Exceptions
{
    public class NoHandlerException : HandlerException
	{
        public NoHandlerException(IDispatchable dispatchable)
            :base("No {0} handler for '{1}'.", dispatchable)
        {

        }
	}

}

