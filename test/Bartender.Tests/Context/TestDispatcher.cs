using System.Collections.Generic;

namespace Bartender.Tests.Context
{
    public class TestDispatcher : Dispatcher
    {
        public TestDispatcher(IDependencyContainer dependencyContainer) : base(dependencyContainer) { }
        public new IDependencyContainer Container => base.Container;
        public new IEnumerable<THandler> GetHandlers<THandler>() => base.GetHandlers<THandler>();
    }
}