using Moq;
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
    }
}