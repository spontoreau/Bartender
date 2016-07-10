using Bartender.Test.Context;
using Moq;
using Xunit;
using Shouldly;

namespace Bartender.Test
{
    public class QueryDispatcherTests : DispatcherTests
    {
        [Fact]
        public void ShouldHandleQueryOnce_WhenCallDispatchMethod()
        {
            QueryDispatcher.Dispatch<Query, ReadModel>(Query);
            MockedQueryHandler.Verify(x => x.Handle(It.IsAny<Query>()), Times.Once);
        }

        [Fact]
        public void ShouldReturnReadModel_WhenCallDispatchMethod()
        {
            var readModel = QueryDispatcher.Dispatch<Query, ReadModel>(Query);
            readModel.ShouldBeSameAs(ReadModel);
        }

        [Fact]
        public void ShouldThrowException_WhenNoQueryHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IQueryHandler<Query, ReadModel>>())
                .Returns(() => new IQueryHandler<Query, ReadModel>[0]);

            Should
                .Throw<DispatcherException>(() => QueryDispatcher.Dispatch<Query, ReadModel>(Query))
                .Message
                .ShouldBe(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowException_WhenMultipleQueryHandler()
        {
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IQueryHandler<Query, ReadModel>>())
                .Returns(() => new [] { MockedQueryHandler.Object, MockedQueryHandler.Object });

            Should
                .Throw<DispatcherException>(() => QueryDispatcher.Dispatch<Query, ReadModel>(Query))
                .Message
                .ShouldBe(MultipleHandlerExceptionMessageExpected);
        }
    }
}
