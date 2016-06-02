namespace Cheers.Cqrs.InMemory.Exceptions
{
    public class MultipleHandlerException : HandlerException
	{
        public MultipleHandlerException(IDispatchable dispatchable)
            :base("Multiple {0} handlers for '{1}'.", dispatchable)
        {

        }
	}

}

