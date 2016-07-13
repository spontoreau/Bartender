using Bartender.Tests.Context;
using Moq;
using Xunit;

namespace Bartender.Tests
{
    public class AsyncCommandDispatcherTests : DispatcherTests
    {
        [Fact]
        public async void ShouldHandleCommandOnce_WhenCallDispatchAsyncMethod()
        {
            await AsyncCommandDispatcher.DispatchAsync<Command, Result>(Command);
            MockedAsyncCommandHandler.Verify(x => x.HandleAsync(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public async void ShouldHandleCommandOnce_WhenCallDispatchAsyncMethodWithoutResult()
        {
            await AsyncCommandDispatcher.DispatchAsync<Command>(Command);
            MockedAsyncCommandWithoutResultHandler.Verify(x => x.HandleAsync(It.IsAny<Command>()), Times.Once);
        }
    }
}