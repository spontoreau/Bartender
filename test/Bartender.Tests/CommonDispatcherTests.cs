using Bartender.Tests.Context;
using Shouldly;
using Xunit;

namespace Bartender.Tests
{
    public class CommonDispatcherTests : TestContext
    {
        protected TestDispatcher Dispatcher { get; private set; }

        public CommonDispatcherTests()
        {
            Dispatcher = new TestDispatcher(MockedDependencyContainer.Object);
        }

        [Fact]
        public void ShouldHaveDependencyContainer_WhenInstanceCreated()
        {
            Dispatcher.Container.ShouldNotBeNull();
        }
        [Fact]
        public void ShouldThrowException_WhenNoHandlers()
        {
            ClearMockedDependencies();

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IHandler<Message, Result>>())
                .Message
                .ShouldBe(NoHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IHandler<Message>>())
                .Message
                .ShouldBe(NoHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncHandler<Message, Result>>())
                .Message
                .ShouldBe(NoHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncHandler<Message>>())
                .Message
                .ShouldBe(NoHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncHandler<Message, Result>>())
                .Message
                .ShouldBe(NoHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncHandler<Message>>())
                .Message
                .ShouldBe(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenMultipleHandlers()
        {
            DuplicateMockedDependencies();

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IHandler<Message, Result>>())
                .Message
                .ShouldBe(MultipleHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IHandler<Message>>())
                .Message
                .ShouldBe(MultipleHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncHandler<Message, Result>>())
                .Message
                .ShouldBe(MultipleHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncHandler<Message>>())
                .Message
                .ShouldBe(MultipleHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncHandler<Message, Result>>())
                .Message
                .ShouldBe(MultipleHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncHandler<Message>>())
                .Message
                .ShouldBe(MultipleHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldCallValidate_WhenHaveValidator()
        {
            InitializeValidators();
            Dispatcher.Validate(Message);

            MockedValidator.Verify(x => x.Validate(Message));
        }

        [Fact]
        public async void ShouldApplyValidation_WhenDispatch()
        {
            InitializeValidators();
            MockedValidator.Setup(x => x.Validate(Message)).Throws(new DispatcherException());

            Should
                .Throw<DispatcherException>(() => ((IDispatcher)Dispatcher).Dispatch(Message));

            Should
                .Throw<DispatcherException>(() => ((IDispatcher)Dispatcher).Dispatch<Message, Result>(Message));

            await Should
                    .ThrowAsync<DispatcherException>(async () => await ((IAsyncDispatcher)Dispatcher).DispatchAsync(Message));

            await Should
                    .ThrowAsync<DispatcherException>(async () => await ((IAsyncDispatcher)Dispatcher).DispatchAsync<Message, Result>(Message));

            await Should
                    .ThrowAsync<DispatcherException>(async() => await ((ICancellableAsyncDispatcher)Dispatcher).DispatchAsync(Message, CancellationToken));

            await Should
                    .ThrowAsync<DispatcherException>(async () => await ((ICancellableAsyncDispatcher)Dispatcher).DispatchAsync<Message, Result>(Message, CancellationToken));
        }

        [Fact]
        public async void ShouldApplyValidation_WhenDispatchQuery()
        {
            InitializeValidators();
            MockedValidator.Setup(x => x.Validate(Message)).Throws(new DispatcherException());

            Should
                .Throw<DispatcherException>(() => ((IDispatcher)Dispatcher).Dispatch<Message, Result>(Message));

            await Should
                .ThrowAsync<DispatcherException>(async () => await ((IAsyncDispatcher)Dispatcher).DispatchAsync<Message, Result>(Message));

            await Should
                .ThrowAsync<DispatcherException>(async() => await ((ICancellableAsyncDispatcher)Dispatcher).DispatchAsync<Message, Result>(Message, CancellationToken));
        }

        [Fact]
        public void ShouldReturnTrue_WhenMessageIsPublication()
        {
            var isPublication = Dispatcher.IsPublication(typeof(Publication));

            isPublication.ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotThrowException_WhenMultipleHandlersForPublication()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IHandler<Publication>>())
                .Returns(() => new [] { MockedPublicationHandler.Object, MockedPublicationHandler.Object});
            
            Should
                .NotThrow(() => Dispatcher.GetHandlers<IHandler<Publication>>());
        }
    }
}