using Moq;
using Xunit;

namespace Bartender.Test
{
    public class QueryDispatchTests
    {
        public class Query : IQuery { public string Value { get; set; } }
        public class ReadModel { public string Value { get; set; } }

        private Mock<IDependencyContainer> MockedDependencyContainer { get; }
        private Mock<IQueryHandler<Query, ReadModel>> MockedQueryHandler { get; }
        private IQueryDispatcher QueryDispatcher { get; }

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
                .Setup(method => method.Handle(It.IsAny<Query>()))
                .Returns(It.IsAny<ReadModel>);

            QueryDispatcher = (IQueryDispatcher)new Dispatcher(MockedDependencyContainer.Object);
        }

        [Fact]
        public void ShouldHandleQueryOnce_WhenCallDispatchMethod()
        {
            QueryDispatcher.Dispatch<Query, ReadModel>(new Query());
            MockedQueryHandler.Verify(x => x.Handle(It.IsAny<Query>()), Times.Once);
        }
    }
}