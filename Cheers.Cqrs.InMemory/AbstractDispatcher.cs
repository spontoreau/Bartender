using Cheers.ServiceLocator;
using System.Collections;
using System.Linq;
using Cheers.Cqrs.InMemory.Exceptions;

namespace Cheers.Cqrs.InMemory
{
    public abstract class AbstractDispatcher
    {
        /// <summary>
        /// Locator
        /// </summary>
        protected ILocator Locator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cheers.Cqrs.InMemory.Dispatcher"/> class.
        /// </summary>
        /// <param name="locator">Locator.</param>
        protected AbstractDispatcher(ILocator locator)
        {
            Locator = locator;
        }

        /// <summary>
        /// Validate
        /// </summary>
        protected void Validate(int nbHandlers, IDispatchable dispatchable)
        {
            if (nbHandlers > 1)
            {
                throw new MultipleHandlerException(dispatchable);
            }
            if (nbHandlers == 0)
            {
                throw new NoHandlerException(dispatchable);
            }
        }
    }
}

