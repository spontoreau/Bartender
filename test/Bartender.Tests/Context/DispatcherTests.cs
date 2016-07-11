using System.Threading;
using System.Threading.Tasks;
using Moq;

namespace Bartender.Tests.Context
{
    public abstract class DispatcherTests
    {
        protected Query Query { get; } = ContextFactory.Get<Query>();
        protected ReadModel ReadModel { get; } = ContextFactory.Get<ReadModel>();
        protected Command Command { get; } = ContextFactory.Get<Command>();
        protected Result Result { get; } = ContextFactory.Get<Result>();
        protected CancellationToken CancellationToken { get; } = CancellationToken.None;
        protected Mock<IDependencyContainer> MockedDependencyContainer { get; private set; }
        protected Mock<IQueryHandler<Query, ReadModel>> MockedQueryHandler { get; private set; }
        protected Mock<IAsyncQueryHandler<Query, ReadModel>> MockedAsyncQueryHandler { get; private set; }
        protected Mock<ICancellableAsyncQueryHandler<Query, ReadModel>> MockedCancellableAsyncQueryHandler { get; private set; }
        protected IQueryDispatcher QueryDispatcher { get; private set; }
        protected IAsyncQueryDispatcher AsyncQueryDispatcher { get; private set; }
        protected ICancellableAsyncQueryDispatcher CancellableAsyncQueryDispatcher { get; private set; }
        protected readonly string NoHandlerExceptionMessageExpected = $"No handler for '{typeof(Query)}'.";
        protected readonly string MultipleHandlerExceptionMessageExpected = $"Multiple handler for '{typeof(Query)}'.";
        
        protected DispatcherTests()
        {
            InitializeDependencyContainer();
            InitializeHandlers();
            InitializeDispatchers();
        }

        private void InitializeDependencyContainer()
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
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new[] { MockedCancellableAsyncQueryHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetInstance<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => MockedCancellableAsyncQueryHandler.Object);

        }

        private void InitializeHandlers()
        {
            MockedQueryHandler = new Mock<IQueryHandler<Query, ReadModel>>();
            MockedQueryHandler
                .Setup(method => method.Handle(Query))
                .Returns(ReadModel);

            MockedAsyncQueryHandler = new Mock<IAsyncQueryHandler<Query, ReadModel>>();
            MockedAsyncQueryHandler
                .Setup(method => method.HandleAsync(Query))
                .Returns(Task.FromResult(ReadModel));

            MockedCancellableAsyncQueryHandler = new Mock<ICancellableAsyncQueryHandler<Query, ReadModel>>();
            MockedCancellableAsyncQueryHandler
                .Setup(method => method.HandleAsync(Query, CancellationToken))
                .Returns(Task.FromResult(ReadModel));
        }

        private void InitializeDispatchers()
        {
            var dispatcher = new Dispatcher(MockedDependencyContainer.Object);
            QueryDispatcher = (IQueryDispatcher)dispatcher;
            AsyncQueryDispatcher = (IAsyncQueryDispatcher)dispatcher;
            CancellableAsyncQueryDispatcher = (ICancellableAsyncQueryDispatcher)dispatcher;
        }
    }
}