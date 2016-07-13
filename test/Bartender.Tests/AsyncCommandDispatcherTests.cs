using Bartender.Tests.Context;
using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests
{
    public class AsyncCommandDispatcherTests : TestContext
    {
        [Fact]
        public async void ShouldHandleCommandOnce_WhenCallDispatchAsyncMethod()
        {
            await AsyncCommandDispatcher.DispatchAsync<Command, Result>(Command);
            MockedAsyncCommandHandler.Verify(x => x.HandleAsync(Command), Times.Once);
        }

        [Fact]
        public async void ShouldHandleCommandOnce_WhenCallDispatchAsyncMethodWithoutResult()
        {
            await AsyncCommandDispatcher.DispatchAsync<Command>(Command);
            MockedAsyncCommandWithoutResultHandler.Verify(x => x.HandleAsync(Command), Times.Once);
        }

        [Fact]
        public async void ShouldReturnResult_WhenCallDispatchMethod()
        {
            var result = await AsyncCommandDispatcher.DispatchAsync<Command, Result>(Command);
            result.ShouldBeSameAs(Result);
        }
    }
}