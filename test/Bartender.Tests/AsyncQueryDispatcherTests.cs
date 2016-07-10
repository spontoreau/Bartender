using Bartender.Test.Context;
using Moq;
using Xunit;

namespace Bartender.Test
{
    public class AsyncQueryDispatcherTest : DispatcherTests
    {
        [Fact]
        public async void ShouldHandleQueryOnce_WhenCallDispatchAsyncMethod()
        {
            await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query);
            MockedAsyncQueryHandler.Verify(x => x.HandleAsync(It.IsAny<Query>()), Times.Once);
        }
    }
}