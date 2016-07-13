using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests.Context
{
    public class CommandDispatcherTests : TestContext
    {
        [Fact]
        public void ShouldHandleCommandOnce_WhenCallDispatchMethod()
        {
            CommandDispatcher.Dispatch<Command, Result>(Command);
            MockedCommandHandler.Verify(x => x.Handle(Command), Times.Once);
        }

        [Fact]
        public void ShouldHandleCommandOnce_WhenCallDispatchMethodWithoutResult()
        {
            CommandDispatcher.Dispatch<Command>(Command);
            MockedCommandWithoutResultHandler.Verify(x => x.Handle(Command), Times.Once);
        }

        [Fact]
        public void ShouldReturnResult_WhenCallDispatchMethod()
        {
            var result = CommandDispatcher.Dispatch<Command, Result>(Command);
            result.ShouldBeSameAs(Result);
        }
    }
}