using Bartender.Tests.Context;
using Xunit;
using Shouldly;
using Moq;

namespace Bartender.Tests
{
    public class AsyncQueryDispatcherTest : TestContext
    {
        [Fact]
        public async void ShouldHandleQueryOnce_WhenCallDispatchAsyncMethod()
        {
            await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query);
            MockedAsyncQueryHandler.Verify(x => x.HandleAsync(Query), Times.Once);
        }

        [Fact]
        public async void ShouldReturnReadModel_WhenCallDispatchAsyncMethod()
        {
            var readModel = await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query);
            readModel.ShouldBeSameAs(ReadModel);
        }
    }
}