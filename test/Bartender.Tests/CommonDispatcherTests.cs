using Bartender.Tests.Context;
using Shouldly;
using Xunit;

namespace Bartender.Tests
{
    public class CommonDispatcherTests : TestContext
    {
        [Fact]
        public void ShouldHaveDependencyContainer_WhenInstanceCreated()
        {
            Dispatcher.Container.ShouldNotBeNull();
        }

        [Fact]
        public void ShouldThrowExceptiont_WhenNoQueryHandlers()
        {
            ClearMockedQueryDependencies();

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IQueryHandler<Query, ReadModel>>())
                .Message
                .ShouldBe(NoQueryHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncQueryHandler<Query, ReadModel>>())
                .Message
                .ShouldBe(NoQueryHandlerExceptionMessageExpected);


            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Message
                .ShouldBe(NoQueryHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenMultipleQueryHandlers()
        {
            DuplicateMockedQueryDependencies();

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IQueryHandler<Query, ReadModel>>())
                .Message
                .ShouldBe(MultipleQueryHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncQueryHandler<Query, ReadModel>>())
                .Message
                .ShouldBe(MultipleQueryHandlerExceptionMessageExpected);


            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Message
                .ShouldBe(MultipleQueryHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenNoCommandHandlers()
        {
            ClearMockedCommandDependencies();

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICommandHandler<Command, Result>>())
                .Message
                .ShouldBe(NoCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICommandHandler<Command>>())
                .Message
                .ShouldBe(NoCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncCommandHandler<Command, Result>>())
                .Message
                .ShouldBe(NoCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncCommandHandler<Command>>())
                .Message
                .ShouldBe(NoCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncCommandHandler<Command, Result>>())
                .Message
                .ShouldBe(NoCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncCommandHandler<Command>>())
                .Message
                .ShouldBe(NoCommandHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenMultipleCommandHandlers()
        {
            DuplicateMockedCommandDependencies();

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICommandHandler<Command, Result>>())
                .Message
                .ShouldBe(MultipleCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICommandHandler<Command>>())
                .Message
                .ShouldBe(MultipleCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncCommandHandler<Command, Result>>())
                .Message
                .ShouldBe(MultipleCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<IAsyncCommandHandler<Command>>())
                .Message
                .ShouldBe(MultipleCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncCommandHandler<Command, Result>>())
                .Message
                .ShouldBe(MultipleCommandHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(() => Dispatcher.GetHandlers<ICancellableAsyncCommandHandler<Command>>())
                .Message
                .ShouldBe(MultipleCommandHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldCallValidate_WhenCommandHaveValidator()
        {
            InitializeValidators();
            Dispatcher.Validate(Command);

            MockedCommandValidator.Verify(x => x.Validate(Command));
        }

        [Fact]
        public async void ShouldApplyValidation_WhenDispatchCommand()
        {
            InitializeValidators();
            MockedCommandValidator.Setup(x => x.Validate(Command)).Throws(new DispatcherException());

            Should
                .Throw<DispatcherException>(() => CommandDispatcher.Dispatch(Command));

            Should
                .Throw<DispatcherException>(() => CommandDispatcher.Dispatch<Command, Result>(Command));

            await Should
                    .ThrowAsync<DispatcherException>(async () => await AsyncCommandDispatcher.DispatchAsync(Command));

            await Should
                    .ThrowAsync<DispatcherException>(async () => await AsyncCommandDispatcher.DispatchAsync<Command, Result>(Command));

            await Should
                    .ThrowAsync<DispatcherException>(async() => await CancellableAsyncCommandDispatcher.DispatchAsync(Command, CancellationToken));

            await Should
                    .ThrowAsync<DispatcherException>(async () => await CancellableAsyncCommandDispatcher.DispatchAsync<Command, Result>(Command, CancellationToken));
        }

        [Fact]
        public async void ShouldApplyValidation_WhenDispatchQuery()
        {
            InitializeValidators();
            MockedQueryValidator.Setup(x => x.Validate(Query)).Throws(new DispatcherException());

            Should
                .Throw<DispatcherException>(() => QueryDispatcher.Dispatch<Query, ReadModel>(Query));

            await Should
                .ThrowAsync<DispatcherException>(async () => await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query));

            await Should
                .ThrowAsync<DispatcherException>(async() => await CancellableAsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query, CancellationToken));
        }

        [Fact]
        public void ShouldReturnTrue_WhenMessageIsPublication()
        {
            var isPublication = Dispatcher.IsPublication(typeof(Publication));

            isPublication.ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotThrowException_WhenMultipleCommandHandlersForPublication()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Publication>>())
                .Returns(() => new [] { MockedPublicationHandler.Object, MockedPublicationHandler.Object});
            
            Should
                .NotThrow(() => Dispatcher.GetHandlers<ICommandHandler<Publication>>());
        }
    }
}