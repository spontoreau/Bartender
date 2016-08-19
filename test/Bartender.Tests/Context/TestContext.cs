using System.Threading;
using System.Threading.Tasks;
using Moq;

namespace Bartender.Tests.Context
{
    public abstract class TestContext
    {
        protected Message Message { get; } = TestContextFactory.Get<Message>();
        protected Result Result { get; } = TestContextFactory.Get<Result>();
        protected Publication Publication { get; } = TestContextFactory.Get<Publication>();
        protected CancellationToken CancellationToken { get; } = CancellationToken.None;

        protected Mock<IDependencyContainer> MockedDependencyContainer { get; private set; }

        protected Mock<IHandler<Message, Result>> MockedHandler { get; private set; }
        protected Mock<IAsyncHandler<Message, Result>> MockedAsyncHandler { get; private set; }
        protected Mock<ICancellableAsyncHandler<Message, Result>> MockedCancellableAsyncHandler { get; private set; }

        protected Mock<IHandler<Message>> MockedFireAndForgetHandler { get; private set; }
        protected Mock<IAsyncHandler<Message>> MockedAsyncFireAndForgetHandler { get; private set; }
        protected Mock<ICancellableAsyncHandler<Message>> MockedCancellableAsyncFireAndForgetHandler { get; private set; }
        
        protected Mock<IHandler<Publication>> MockedPublicationHandler { get; private set; }
        protected Mock<IAsyncHandler<Publication>> MockedAsyncPublicationHandler { get; private set; }
        protected Mock<ICancellableAsyncHandler<Publication>> MockedCancellableAsyncPublicationHandler { get; private set; }

        protected Mock<IMessageValidator<Message>> MockedValidator { get; private set; }

        protected readonly string NoHandlerExceptionMessageExpected = $"No handler for '{typeof(Message)}'.";
        protected readonly string MultipleHandlerExceptionMessageExpected = $"Multiple handler for '{typeof(Message)}'.";

        protected TestContext()
        {
            MockedDependencyContainer = new Mock<IDependencyContainer>();

            InitializeDependencies();
            InitializeHandlers();
        }

        private void InitializeDependencies()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IHandler<Message, Result>>())
                .Returns(() => new[] { MockedHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IHandler<Message>>())
                .Returns(() => new[] { MockedFireAndForgetHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncHandler<Message, Result>>())
                .Returns(() => new[] { MockedAsyncHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncHandler<Message>>())
                .Returns(() => new[] { MockedAsyncFireAndForgetHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncHandler<Message, Result>>())
                .Returns(() => new[] { MockedCancellableAsyncHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncHandler<Message>>())
                .Returns(() => new[] { MockedCancellableAsyncFireAndForgetHandler.Object });
        }

        private void InitializeHandlers()
        {
            MockedHandler = new Mock<IHandler<Message, Result>>();
            MockedHandler
                .Setup(method => method.Handle(Message))
                .Returns(Result);
            MockedFireAndForgetHandler = new Mock<IHandler<Message>>();
            MockedFireAndForgetHandler
                .Setup(method => method.Handle(Message));
            MockedAsyncHandler = new Mock<IAsyncHandler<Message, Result>>();
            MockedAsyncHandler
                .Setup(method => method.HandleAsync(Message))
                .Returns(Task.FromResult(Result));
            MockedAsyncFireAndForgetHandler = new Mock<IAsyncHandler<Message>>();
            MockedCancellableAsyncHandler = new Mock<ICancellableAsyncHandler<Message, Result>>();
            MockedCancellableAsyncHandler
                .Setup(method => method.HandleAsync(Message, CancellationToken))
                .Returns(Task.FromResult(Result));
            MockedCancellableAsyncFireAndForgetHandler = new Mock<ICancellableAsyncHandler<Message>>();

            MockedPublicationHandler = new Mock<IHandler<Publication>>();
            MockedAsyncPublicationHandler = new Mock<IAsyncHandler<Publication>>();
            MockedCancellableAsyncPublicationHandler = new Mock<ICancellableAsyncHandler<Publication>>();
        }

        protected void InitializeValidators()
        {
            MockedValidator = new Mock<IMessageValidator<Message>>();
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IMessageValidator<Message>>())
                .Returns(() => new [] { MockedValidator.Object });
        }

        protected void ClearMockedDependencies()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IHandler<Message, Result>>())
                .Returns(() => new IHandler<Message, Result>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IHandler<Message>>())
                .Returns(() => new IHandler<Message>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncHandler<Message, Result>>())
                .Returns(() => new IAsyncHandler<Message, Result>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncHandler<Message>>())
                .Returns(() => new IAsyncHandler<Message>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncHandler<Message, Result>>())
                .Returns(() => new ICancellableAsyncHandler<Message, Result>[0]);
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncHandler<Message>>())
                .Returns(() => new ICancellableAsyncHandler<Message>[0]);
        }

        protected void DuplicateMockedDependencies()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IHandler<Message, Result>>())
                .Returns(() => new [] { MockedHandler.Object, MockedHandler.Object});
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IHandler<Message>>())
                .Returns(() => new [] { MockedFireAndForgetHandler.Object, MockedFireAndForgetHandler.Object});
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncHandler<Message, Result>>())
                .Returns(() => new [] { MockedAsyncHandler.Object, MockedAsyncHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncHandler<Message>>())
                .Returns(() => new [] { MockedAsyncFireAndForgetHandler.Object, MockedAsyncFireAndForgetHandler.Object});
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncHandler<Message, Result>>())
                .Returns(() => new [] { MockedCancellableAsyncHandler.Object, MockedCancellableAsyncHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncHandler<Message>>())
                .Returns(() => new [] { MockedCancellableAsyncFireAndForgetHandler.Object, MockedCancellableAsyncFireAndForgetHandler.Object });
        }
    }
}