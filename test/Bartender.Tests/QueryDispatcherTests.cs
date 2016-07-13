using Bartender.Tests.Context;
using Moq;
using Xunit;
using Shouldly;

namespace Bartender.Tests
{
    public class QueryDispatcherTests : TestContext
    {
        [Fact]
        public void ShouldHandleQueryOnce_WhenCallDispatchMethod()
        {
            QueryDispatcher.Dispatch<Query, ReadModel>(Query);
            MockedQueryHandler.Verify(x => x.Handle(Query), Times.Once);
        }

        [Fact]
        public void ShouldReturnReadModel_WhenCallDispatchMethod()
        {
            var readModel = QueryDispatcher.Dispatch<Query, ReadModel>(Query);
            readModel.ShouldBeSameAs(ReadModel);
        }
    }
}
