using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests.Context
{
    public class CommandDispatcherTests : DispatcherTests
    {
        [Fact]
        public void ShouldHandleCommandOnce_WhenCallDispatchMethod()
        {
            CommandDispatcher.Dispatch<Command, Result>(Command);
            MockedCommandHandler.Verify(x => x.Handle(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public void ShouldHandleCommandOnce_WhenCallDispatchMethodWithoutResult()
        {
            CommandDispatcher.Dispatch<Command>(Command);
            MockedCommandWithoutResultHandler.Verify(x => x.Handle(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public void ShouldReturnResult_WhenCallDispatchMethod()
        {
            var result = CommandDispatcher.Dispatch<Command, Result>(Command);
            result.ShouldBeSameAs(Result);
        }

        [Fact]
        public void ShouldThrowException_WhenNoCommandHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command, Result>>())
                .Returns(() => new ICommandHandler<Command, Result>[0]);

            Should
                .Throw<DispatcherException>(() => CommandDispatcher.Dispatch<Command, Result>(Command))
                .Message
                .ShouldBe(NoCommandHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenNoCommandWithoutResultHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command>>())
                .Returns(() => new ICommandHandler<Command>[0]);

            Should
                .Throw<DispatcherException>(() => CommandDispatcher.Dispatch<Command>(Command))
                .Message
                .ShouldBe(NoCommandHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenMultipleCommandHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command, Result>>())
                .Returns(() => new [] { MockedCommandHandler.Object, MockedCommandHandler.Object });

            Should
                .Throw<DispatcherException>(() => CommandDispatcher.Dispatch<Command, Result>(Command))
                .Message
                .ShouldBe(MultipleCommandHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenMultipleCommandWithoutResultHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICommandHandler<Command>>())
                .Returns(() => new [] { MockedCommandWithoutResultHandler.Object, MockedCommandWithoutResultHandler.Object });

            Should
                .Throw<DispatcherException>(() => CommandDispatcher.Dispatch<Command>(Command))
                .Message
                .ShouldBe(MultipleCommandHandlerExceptionMessageExpected);
        }
    }
}