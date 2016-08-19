using Bartender.Tests.Context;
using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests
{
    public class AsyncDispatcherTests : TestContext
    {
        private IAsyncDispatcher Dispatcher { get; }

        public AsyncDispatcherTests()
        {
            Dispatcher = new TestDispatcher(MockedDependencyContainer.Object);
        }

        [Fact]
        public async void ShouldHandleOnce_WhenCallDispatchAsyncMethod()
        {
            await Dispatcher.DispatchAsync<Message, Result>(Message);
            MockedAsyncHandler.Verify(x => x.HandleAsync(Message), Times.Once);
        }

        [Fact]
        public async void ShouldHandleOnce_WhenCallDispatchAsyncMethodWithoutResult()
        {
            await Dispatcher.DispatchAsync<Message>(Message);
            MockedAsyncFireAndForgetHandler.Verify(x => x.HandleAsync(Message), Times.Once);
        }

        [Fact]
        public async void ShouldReturnResult_WhenCallDispatchAsyncMethod()
        {
            var result = await Dispatcher.DispatchAsync<Message, Result>(Message);
            result.ShouldBeSameAs(Result);
        }

        [Fact]
        public async void ShouldHandleMany_WhenDispatchPublicationWithoutResult()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncHandler<Publication>>())
                .Returns(() => new [] { MockedAsyncPublicationHandler.Object, MockedAsyncPublicationHandler.Object});

            await Dispatcher.DispatchAsync(Publication);

            MockedAsyncPublicationHandler.Verify(x => x.HandleAsync(Publication), Times.Exactly(2));
        }
    }
}