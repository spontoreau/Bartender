using Bartender.Tests.Context;
using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests
{
    public class CancellableAsyncCommandDispatcherTests : TestContext
    {
        [Fact]
        public async void ShouldHandleCommandOnce_WhenCallDispatchAsyncMethod()
        {
            await CancellableAsyncCommandDispatcher.DispatchAsync<Command, Result>(Command, CancellationToken);
            MockedCancellableAsyncCommandHandler.Verify(x => x.HandleAsync(Command, CancellationToken), Times.Once);
        }

        [Fact]
        public async void ShouldHandleCommandOnce_WhenCallDispatchAsyncMethodWithoutResult()
        {
            await CancellableAsyncCommandDispatcher.DispatchAsync<Command>(Command, CancellationToken);
            MockedCancellableAsyncCommandWithoutResultHandler.Verify(x => x.HandleAsync(Command, CancellationToken), Times.Once);
        }

        [Fact]
        public async void ShouldReturnResult_WhenCallDispatchMethod()
        {
            var result = await CancellableAsyncCommandDispatcher.DispatchAsync<Command, Result>(Command, CancellationToken);
            result.ShouldBeSameAs(Result);
        }

        [Fact]
        public async void ShouldHandleMany_WhenDispatchPublicationWithoutResult()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncCommandHandler<Publication>>())
                .Returns(() => new [] { MockedCancellableAsyncPublicationHandler.Object, MockedCancellableAsyncPublicationHandler.Object});

            await CancellableAsyncCommandDispatcher.DispatchAsync(Publication, CancellationToken);

            MockedCancellableAsyncPublicationHandler.Verify(x => x.HandleAsync(Publication, CancellationToken), Times.Exactly(2));
        }
    }
}