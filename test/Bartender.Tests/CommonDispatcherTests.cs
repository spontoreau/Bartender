using Bartender.Tests.Context;
using Moq;
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
                .Throw<DispatcherException>(() => QueryDispatcher.Dispatch<Query, ReadModel>(Query))
                .Message
                .ShouldBe(NoQueryHandlerExceptionMessageExpected);

            Should
                .Throw<DispatcherException>(async () => await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query))
                .Message
                .ShouldBe(NoQueryHandlerExceptionMessageExpected);


            Should
                .Throw<DispatcherException>(async () => await CancellableAsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query, CancellationToken))
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
    }
}