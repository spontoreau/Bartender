using Bartender.Tests.Context;
using Moq;
using Xunit;

namespace Bartender.Tests
{
    public class CancellableAsyncQueryDispatcherTests : DispatcherTests
    {
        [Fact]
        public async void ShouldHandleQueryOnce_WhenCallDispatchAsyncMethod()
        {
            await CancellableAsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query, CancellationToken);
            MockedCancellableAsyncQueryHandler.Verify(x => x.HandleAsync(It.IsAny<Query>(), CancellationToken), Times.Once);
        }
    }
}