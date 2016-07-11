using Bartender.Tests.Context;
using Moq;
using Shouldly;
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

        [Fact]
        public async void ShouldReturnReadModel_WhenCallDispatchAsyncMethod()
        {
            var readModel = await CancellableAsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query, CancellationToken);
            readModel.ShouldBeSameAs(ReadModel);
        }

        [Fact]
        public void ShouldThrowException_WhenNoAsyncQueryHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new ICancellableAsyncQueryHandler<Query, ReadModel>[0]);

            Should
                .Throw<DispatcherException>(async () => await CancellableAsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query, CancellationToken))
                .Message
                .ShouldBe(NoQueryHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenMultipleAsyncQueryHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new [] { MockedCancellableAsyncQueryHandler.Object, MockedCancellableAsyncQueryHandler.Object });

            Should
                .Throw<DispatcherException>(async () => await CancellableAsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query, CancellationToken))
                .Message
                .ShouldBe(MultipleQueryHandlerExceptionMessageExpected);
        }
    }
}