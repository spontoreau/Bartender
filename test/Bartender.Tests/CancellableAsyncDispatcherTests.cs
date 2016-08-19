using Bartender.Tests.Context;
using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests
{
    public class CancellableAsyncDispatcherTests : TestContext
    {
        private ICancellableAsyncDispatcher Dispatcher { get; }

        public CancellableAsyncDispatcherTests()
        {
            Dispatcher = new TestDispatcher(MockedDependencyContainer.Object);
        }

        [Fact]
        public async void ShouldHandleOnce_WhenCallDispatchAsyncMethod()
        {
            await Dispatcher.DispatchAsync<Message, Result>(Message, CancellationToken);
            MockedCancellableAsyncHandler.Verify(x => x.HandleAsync(Message, CancellationToken), Times.Once);
        }

        [Fact]
        public async void ShouldHandleOnce_WhenCallDispatchAsyncMethodWithoutResult()
        {
            await Dispatcher.DispatchAsync<Message>(Message, CancellationToken);
            MockedCancellableAsyncFireAndForgetHandler.Verify(x => x.HandleAsync(Message, CancellationToken), Times.Once);
        }

        [Fact]
        public async void ShouldReturnResult_WhenCallDispatchMethod()
        {
            var result = await Dispatcher.DispatchAsync<Message, Result>(Message, CancellationToken);
            result.ShouldBeSameAs(Result);
        }

        [Fact]
        public async void ShouldHandleMany_WhenDispatchPublicationWithoutResult()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<ICancellableAsyncHandler<Publication>>())
                .Returns(() => new [] { MockedCancellableAsyncPublicationHandler.Object, MockedCancellableAsyncPublicationHandler.Object});

            await Dispatcher.DispatchAsync(Publication, CancellationToken);

            MockedCancellableAsyncPublicationHandler.Verify(x => x.HandleAsync(Publication, CancellationToken), Times.Exactly(2));
        }
    }
}