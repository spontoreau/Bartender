using System.Threading.Tasks;
using Moq;

namespace Bartender.Test.Context
{
    public abstract class DispatcherTests
    {
        protected Query Query { get; } = Query.New();
        protected ReadModel ReadModel { get; } = ReadModel.New();
        protected Mock<IDependencyContainer> MockedDependencyContainer { get; }
        protected Mock<IQueryHandler<Query, ReadModel>> MockedQueryHandler { get; }
        protected Mock<IAsyncQueryHandler<Query, ReadModel>> MockedAsyncQueryHandler { get; }
        protected IQueryDispatcher QueryDispatcher { get; }
        protected IAsyncQueryDispatcher AsyncQueryDispatcher { get; }
        protected readonly string NoHandlerExceptionMessageExpected = $"No handler for '{typeof(Query)}'.";
        protected readonly string MultipleHandlerExceptionMessageExpected = $"Multiple handler for '{typeof(Query)}'.";
        
        protected DispatcherTests()
        {
            MockedDependencyContainer = new Mock<IDependencyContainer>();
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IQueryHandler<Query, ReadModel>>())
                .Returns(() => new[] { MockedQueryHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetInstance<IQueryHandler<Query, ReadModel>>())
                .Returns(() => MockedQueryHandler.Object);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new[] { MockedAsyncQueryHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetInstance<IAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => MockedAsyncQueryHandler.Object);

            MockedQueryHandler = new Mock<IQueryHandler<Query, ReadModel>>();
            MockedQueryHandler
                .Setup(method => method.Handle(Query))
                .Returns(ReadModel);
            MockedAsyncQueryHandler = new Mock<IAsyncQueryHandler<Query, ReadModel>>();
            MockedAsyncQueryHandler
                .Setup(method => method.HandleAsync(Query))
                .Returns(Task.FromResult(ReadModel));

            var dispatcher = new Dispatcher(MockedDependencyContainer.Object);
            QueryDispatcher = (IQueryDispatcher)dispatcher;
            AsyncQueryDispatcher = (IAsyncQueryDispatcher)dispatcher;

        }
    }
}