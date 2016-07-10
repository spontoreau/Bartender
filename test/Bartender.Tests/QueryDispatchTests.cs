using Bartender.Test.Context;
using Moq;
using Xunit;
using Shouldly;

namespace Bartender.Test
{
    public class QueryDispatchTests
    {
        private Query Query { get; } = Query.New();
        private ReadModel ReadModel { get; } = ReadModel.New();
        private Mock<IDependencyContainer> MockedDependencyContainer { get; }
        private Mock<IQueryHandler<Query, ReadModel>> MockedQueryHandler { get; }
        private IQueryDispatcher QueryDispatcher { get; }
        readonly string NoHandlerExceptionMessageExpected = $"No handler for '{typeof(Query)}'.";

        public QueryDispatchTests()
        {
            MockedDependencyContainer = new Mock<IDependencyContainer>();
            MockedDependencyContainer
                .Setup(method => method.GetAllInstances<IQueryHandler<Query, ReadModel>>())
                .Returns(() => new[] { MockedQueryHandler.Object });
            MockedDependencyContainer
                .Setup(method => method.GetInstance<IQueryHandler<Query, ReadModel>>())
                .Returns(() => MockedQueryHandler.Object);

            MockedQueryHandler = new Mock<IQueryHandler<Query, ReadModel>>();
            MockedQueryHandler
                .Setup(method => method.Handle(Query))
                .Returns(ReadModel);

            QueryDispatcher = (IQueryDispatcher)new Dispatcher(MockedDependencyContainer.Object);
        }

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
    }
}