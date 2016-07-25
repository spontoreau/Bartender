using Bartender;
using ConsoleApplication.Infrastructure;
using StructureMap;

namespace ConsoleApplication.Registries
{
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<ICommandDispatcher>().Use<Dispatcher>();
            For<IAsyncCommandDispatcher>().Use<Dispatcher>();
            For<ICancellableAsyncCommandDispatcher>().Use<Dispatcher>();
            For<IQueryDispatcher>().Use<Dispatcher>();
            For<IAsyncQueryDispatcher>().Use<Dispatcher>();
            For<ICancellableAsyncCommandDispatcher>().Use<Dispatcher>();

            For<IDependencyContainer>().Use<DependencyContainer>();

            Scan(scanner =>
            {
                scanner.Assembly(this.GetType().Assembly);
                scanner.AddAllTypesOf(typeof(ICommandHandler<,>));
                scanner.AddAllTypesOf(typeof(IAsyncCommandHandler<,>));
                scanner.AddAllTypesOf(typeof(ICancellableAsyncCommandHandler<,>));
                scanner.AddAllTypesOf(typeof(ICommandHandler<>));
                scanner.AddAllTypesOf(typeof(IAsyncCommandHandler<>));
                scanner.AddAllTypesOf(typeof(ICancellableAsyncCommandHandler<>));
                scanner.AddAllTypesOf(typeof(IQueryHandler<,>));
                scanner.AddAllTypesOf(typeof(IAsyncQueryHandler<,>));
                scanner.AddAllTypesOf(typeof(ICancellableAsyncQueryHandler<,>));
            });
        }
    }
}