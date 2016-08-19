using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests.Context
{
    public class DispatcherTests : TestContext
    {
        private IDispatcher Dispatcher { get; }

        public DispatcherTests()
        {
            Dispatcher = new TestDispatcher(MockedDependencyContainer.Object);
        }
        
        [Fact]
        public void ShouldHandleOnce_WhenCallDispatchMethod()
        {
            Dispatcher.Dispatch<Message, Result>(Message);
            MockedHandler.Verify(x => x.Handle(Message), Times.Once);
        }

        [Fact]
        public void ShouldHandleOnce_WhenCallFireAndForgetDispatchMethod()
        {
            Dispatcher.Dispatch<Message>(Message);
            MockedFireAndForgetHandler.Verify(x => x.Handle(Message), Times.Once);
        }

        [Fact]
        public void ShouldReturnResult_WhenCallDispatchMethod()
        {
            var result = Dispatcher.Dispatch<Message, Result>(Message);
            result.ShouldBeSameAs(Result);
        }

        [Fact]
        public void ShouldHandleMany_WhenDispatchPublication()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IHandler<Publication>>())
                .Returns(() => new [] { MockedPublicationHandler.Object, MockedPublicationHandler.Object});

            Dispatcher.Dispatch(Publication);

            MockedPublicationHandler.Verify(x => x.Handle(Publication), Times.Exactly(2));
        }
    }
}