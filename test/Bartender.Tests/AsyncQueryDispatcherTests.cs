using Bartender.Tests.Context;
using Moq;
using Xunit;
using Shouldly;

namespace Bartender.Tests
{
    public class AsyncQueryDispatcherTest : TestContext
    {
        [Fact]
        public async void ShouldHandleQueryOnce_WhenCallDispatchAsyncMethod()
        {
            await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query);
            MockedAsyncQueryHandler.Verify(x => x.HandleAsync(It.IsAny<Query>()), Times.Once);
        }

        [Fact]
        public async void ShouldReturnReadModel_WhenCallDispatchAsyncMethod()
        {
            var readModel = await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query);
            readModel.ShouldBeSameAs(ReadModel);
        }
    }
}