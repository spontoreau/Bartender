using Bartender;
using CoreApplication.Infrastructure;
using StructureMap;

namespace ConsoleApplication.Registries
{
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<IDispatcher>().Use<Dispatcher>();
            For<IAsyncDispatcher>().Use<Dispatcher>();
            For<ICancellableAsyncDispatcher>().Use<Dispatcher>();

            For<IDependencyContainer>().Use<DependencyContainer>();

            Scan(scanner =>
            {
                scanner.AssemblyContainingType<InfrastructureRegistry>();
                scanner.AddAllTypesOf(typeof(IHandler<>));
                scanner.AddAllTypesOf(typeof(IHandler<,>));
                scanner.AddAllTypesOf(typeof(IAsyncHandler<>));
                scanner.AddAllTypesOf(typeof(IAsyncHandler<,>));
                scanner.AddAllTypesOf(typeof(ICancellableAsyncHandler<>));
                scanner.AddAllTypesOf(typeof(ICancellableAsyncHandler<,>));
            });
        }
    }
}