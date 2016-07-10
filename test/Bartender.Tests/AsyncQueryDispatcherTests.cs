using Bartender.Test.Context;
using Moq;
using Xunit;
using Shouldly;

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

        [Fact]
        public async void ShouldReturnReadModel_WhenCallDispatchAsyncMethod()
        {
            var readModel = await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query);
            readModel.ShouldBeSameAs(ReadModel);
        }

        [Fact]
        public void ShouldThrowException_WhenNoAsyncQueryHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new IAsyncQueryHandler<Query, ReadModel>[0]);

            Should
                .Throw<DispatcherException>(async () => await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query))
                .Message
                .ShouldBe(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenMultipleAsyncQueryHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IAsyncQueryHandler<Query, ReadModel>>())
                .Returns(() => new [] { MockedAsyncQueryHandler.Object, MockedAsyncQueryHandler.Object });

            Should
                .Throw<DispatcherException>(async () => await AsyncQueryDispatcher.DispatchAsync<Query, ReadModel>(Query))
                .Message
                .ShouldBe(MultipleHandlerExceptionMessageExpected);
        }
    }
}