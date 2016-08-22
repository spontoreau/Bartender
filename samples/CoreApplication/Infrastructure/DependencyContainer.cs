using System.Collections.Generic;
using Bartender;
using StructureMap;

namespace CoreApplication.Infrastructure
{
    public class DependencyContainer : IDependencyContainer
    {
        public DependencyContainer(IContainer container)
        {
            Container = container;
        }

        private IContainer Container { get; }

        public IEnumerable<T> GetAllInstances<T>()
        {
            return Container.GetAllInstances<T>();
        }
    }
}