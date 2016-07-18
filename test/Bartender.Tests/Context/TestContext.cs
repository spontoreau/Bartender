using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;

namespace Bartender.Tests.Context
{
    public abstract class TestContext
    {
        protected Query Query { get; } = TestContextFactory.Get<Query>();
        protected ReadModel ReadModel { get; } = TestContextFactory.Get<ReadModel>();
        protected Command Command { get; } = TestContextFactory.Get<Command>();
        protected Result Result { get; } = TestContextFactory.Get<Result>();
        protected CancellationToken CancellationToken { get; } = CancellationToken.None;
        protected Mock<IDependencyContainer> MockedDependencyContainer { get; private set; }
        protected Mock<IQueryHandler<Query, ReadModel>> MockedQueryHandler { get; private set; }
        protected Mock<IAsyncQueryHandler<Query, ReadModel>> MockedAsyncQueryHandler { get; private set; }
        protected Mock<ICancellableAsyncQueryHandler<Query, ReadModel>> MockedCancellableAsyncQueryHandler { get; private set; }
        protected Mock<ICommandHandler<Command, Result>> MockedCommandHandler { get; private set; }
        protected Mock<ICommandHandler<Command>> MockedCommandWithoutResultHandler { get; private set; }
        protected Mock<IAsyncCommandHandler<Command, Result>> MockedAsyncCommandHandler { get; private set; }
        protected Mock<IAsyncCommandHandler<Command>> MockedAsyncCommandWithoutResultHandler { get; private set; }
        protected Mock<ICancellableAsyncCommandHandler<Command, Result>> MockedCancellableAsyncCommandHandler { get; private set; }
        protected Mock<ICancellableAsyncCommandHandler<Command>> MockedCancellableAsyncCommandWithoutResultHandler { get; private set; }
        protected Mock<ICommandHandler<Publication>> MockedPublicationHandler { get; private set; }
        protected Mock<IMessageValidator<Command>> MockedValidator { get; private set; }
        protected TestDispatcher Dispatcher { get; private set; }
        protected IQueryDispatcher QueryDispatcher => (IQueryDispatcher)Dispatcher;
        protected IAsyncQueryDispatcher AsyncQueryDispatcher => (IAsyncQueryDispatcher)Dispatcher;
        protected ICancellableAsyncQueryDispatcher CancellableAsyncQueryDispatcher => (ICancellableAsyncQueryDispatcher)Dispatcher;
        protected ICommandDispatcher CommandDispatcher => (ICommandDispatcher)Dispatcher;
        protected IAsyncCommandDispatcher AsyncCommandDispatcher => (IAsyncCommandDispatcher)Dispatcher;
        protected ICancellableAsyncCommandDispatcher CancellableAsyncCommandDispatcher => (ICancellableAsyncCommandDispatcher)Dispatcher;
        protected readonly string NoQueryHandlerExceptionMessageExpected = $"No handler for '{typeof(Query)}'.";
        protected readonly string MultipleQueryHandlerExceptionMessageExpected = $"Multiple handler for '{typeof(Query)}'.";
        protected readonly string NoCommandHandlerExceptionMessageExpected = $"No handler for '{typeof(Command)}'.";
        protected readonly string MultipleCommandHandlerExceptionMessageExpected = $"Multiple handler for '{typeof(Command)}'.";

        protected TestContext()
        {
            MockedDependencyContainer = new Mock<IDependencyContainer>();

            InitializeQueryDependencies();
            InitializeCommandDependencies();
            InitializeQueryHandlers();
            InitializeCommandHandlers();
            InitializeDispatcher();
        }

        private void InitializeQueryDependencies()
        {
            //Query
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IQueryHandler<Query, ReadModel>>())
                .Returns(() => new[] { MockedQueryHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new[] { MockedAsyncQueryHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new[] { MockedCancellableAsyncQueryHandler.Object });
        }

        private void InitializeCommandDependencies()
        {
            //Command
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command, Result>>())
                .Returns(() => new[] { MockedCommandHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command>>())
                .Returns(() => new[] { MockedCommandWithoutResultHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncCommandHandler<Command, Result>>())
                .Returns(() => new[] { MockedAsyncCommandHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncCommandHandler<Command>>())
                .Returns(() => new[] { MockedAsyncCommandWithoutResultHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncCommandHandler<Command, Result>>())
                .Returns(() => new[] { MockedCancellableAsyncCommandHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncCommandHandler<Command>>())
                .Returns(() => new[] { MockedCancellableAsyncCommandWithoutResultHandler.Object });
        }

        private void InitializeQueryHandlers()
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

        private void InitializeCommandHandlers()
        {
            MockedCommandHandler = new Mock<ICommandHandler<Command, Result>>();
            MockedCommandHandler
                .Setup(method => method.Handle(Command))
                .Returns(Result);
            MockedCommandWithoutResultHandler = new Mock<ICommandHandler<Command>>();
            MockedCommandWithoutResultHandler
                .Setup(method => method.Handle(Command));
            MockedAsyncCommandHandler = new Mock<IAsyncCommandHandler<Command, Result>>();
            MockedAsyncCommandHandler
                .Setup(method => method.HandleAsync(Command))
                .Returns(Task.FromResult(Result));
            MockedAsyncCommandWithoutResultHandler = new Mock<IAsyncCommandHandler<Command>>();
            MockedCancellableAsyncCommandHandler = new Mock<ICancellableAsyncCommandHandler<Command, Result>>();
            MockedCancellableAsyncCommandHandler
                .Setup(method => method.HandleAsync(Command, CancellationToken))
                .Returns(Task.FromResult(Result));
            MockedCancellableAsyncCommandWithoutResultHandler = new Mock<ICancellableAsyncCommandHandler<Command>>();

            MockedPublicationHandler = new Mock<ICommandHandler<Publication>>();
        }

        private void InitializeDispatcher()
        {
            Dispatcher = new TestDispatcher(MockedDependencyContainer.Object);
        }

        protected void InitializeValidators()
        {
            MockedValidator = new Mock<IMessageValidator<Command>>();
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IMessageValidator<Command>>())
                .Returns(() => new [] { MockedValidator.Object });
        }

        protected void ClearMockedQueryDependencies()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IQueryHandler<Query, ReadModel>>())
                .Returns(() => new IQueryHandler<Query, ReadModel>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new IAsyncQueryHandler<Query, ReadModel>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new ICancellableAsyncQueryHandler<Query, ReadModel>[0]);
        }

        protected void DuplicateMockedQueryDependencies()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IQueryHandler<Query, ReadModel>>())
                .Returns(() => new [] { MockedQueryHandler.Object, MockedQueryHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new [] { MockedAsyncQueryHandler.Object, MockedAsyncQueryHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new [] { MockedCancellableAsyncQueryHandler.Object, MockedCancellableAsyncQueryHandler.Object });
        }

        protected void ClearMockedCommandDependencies()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command, Result>>())
                .Returns(() => new ICommandHandler<Command, Result>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command>>())
                .Returns(() => new ICommandHandler<Command>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncCommandHandler<Command, Result>>())
                .Returns(() => new IAsyncCommandHandler<Command, Result>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncCommandHandler<Command>>())
                .Returns(() => new IAsyncCommandHandler<Command>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncCommandHandler<Command, Result>>())
                .Returns(() => new ICancellableAsyncCommandHandler<Command, Result>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncCommandHandler<Command>>())
                .Returns(() => new ICancellableAsyncCommandHandler<Command>[0]);
        }

        protected void DuplicateMockedCommandDependencies()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command, Result>>())
                .Returns(() => new [] { MockedCommandHandler.Object, MockedCommandHandler.Object});
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command>>())
                .Returns(() => new [] { MockedCommandWithoutResultHandler.Object, MockedCommandWithoutResultHandler.Object});
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncCommandHandler<Command, Result>>())
                .Returns(() => new [] { MockedAsyncCommandHandler.Object, MockedAsyncCommandHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncCommandHandler<Command>>())
                .Returns(() => new [] { MockedAsyncCommandWithoutResultHandler.Object, MockedAsyncCommandWithoutResultHandler.Object});
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncCommandHandler<Command, Result>>())
                .Returns(() => new [] { MockedCancellableAsyncCommandHandler.Object, MockedCancellableAsyncCommandHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncCommandHandler<Command>>())
                .Returns(() => new [] { MockedCancellableAsyncCommandWithoutResultHandler.Object, MockedCancellableAsyncCommandWithoutResultHandler.Object });
        }
    }
}