using Bartender.Tests.Context;
using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests
{
    public class CancellableAsyncQueryDispatcherTests : TestContext
    {
        [Fact]
        public async void ShouldHandleQueryOnce_WhenCallDispatchAsyncMethod()
        {
            await CancellableAsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query, CancellationToken);
            MockedCancellableAsyncQueryHandler.Verify(x => x.HandleAsync(It.IsAny<Query>(), CancellationToken), Times.Once);
        }

        [Fact]
        public async void ShouldReturnReadModel_WhenCallDispatchAsyncMethod()
        {
            var readModel = await CancellableAsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query, CancellationToken);
            readModel.ShouldBeSameAs(ReadModel);
        }
    }
}