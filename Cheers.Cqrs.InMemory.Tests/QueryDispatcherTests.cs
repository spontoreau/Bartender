using System;
using Cheers.Cqrs.Read;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Cheers.ServiceLocator;
using Cheers.Cqrs.InMemory.Exceptions;

namespace Cheers.Cqrs.InMemory.Tests
{
    public class QueryDispatcherTests
    {
        public class Query : IQuery { public string Value { get; set; } }
        public class ReadModel : IReadModel { public string Value { get; set; } }

        Mock<IQueryHandler<Query, ReadModel>> MockedQueryHandler { get; set; }
        Mock<ILocator> MockedLocator { get; set; }
        Fixture Fixture { get; set; }

        readonly string MultipleHandlerExceptionMessageExpected = $"Multiple query handlers for '{typeof(Query).Name}'.";
        readonly string NoHandlerExceptionMessageExpected = $"No query handler for '{typeof(Query).Name}'.";

        public QueryDispatcherTests()
        {
            Fixture = new Fixture();

            MockedLocator = new Mock<ILocator>();

            MockedQueryHandler = new Mock<IQueryHandler<Query, ReadModel>>();
            MockedQueryHandler.Setup(method => method.Handle(It.IsAny<Query>())).Returns(It.IsAny<IEnumerable<ReadModel>>);

            MockedLocator.Setup(method => method.GetAllServices<IQueryHandler<Query, ReadModel>>()).Returns(() => new[] { MockedQueryHandler.Object });
            MockedLocator.Setup(method => method.GetService<IQueryHandler<Query, ReadModel>>()).Returns(() => MockedQueryHandler.Object);
        }

        [Fact]
        public void ShouldCallHandle_WhenDispatchQuery()
        {
            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            dispatcher.Dispatch<Query, ReadModel>(new Query());
            MockedQueryHandler.Verify(method => method.Handle(It.IsAny<Query>()), Times.Once);
        }

        [Fact]
        public void ShouldReturnValue_WhenHandleQuery()
        {
            var expected = Fixture.Create<IEnumerable<ReadModel>>();
            var query = Fixture.Create<Query>();
            Query handledQuery = null;

            MockedQueryHandler.Setup(method => method.Handle(It.IsAny<Query>()))
                .Returns(expected)
                .Callback<Query>(q => handledQuery = q);

            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            var actual = dispatcher.Dispatch<Query, ReadModel>(query);

            query.ShouldBeEquivalentTo(handledQuery);
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenNoQueryHandler()
        {
            MockedLocator.Setup(method => method.GetAllServices<IQueryHandler<Query, ReadModel>>()).Returns(() => new IQueryHandler<Query, ReadModel>[] { });

            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            var action = new Action(() => dispatcher.Dispatch<Query, ReadModel>(new Query()));
            action.ShouldThrowExactly<NoHandlerException>().WithMessage(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenMultipleQueryHandler()
        {
            MockedLocator.Setup(method => method.GetAllServices<IQueryHandler<Query, ReadModel>>()).Returns(() => new IQueryHandler<Query, ReadModel>[] { MockedQueryHandler.Object, MockedQueryHandler.Object });

            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            var action = new Action(() => dispatcher.Dispatch<Query, ReadModel>(new Query()));
            action.ShouldThrowExactly<MultipleHandlerException>().WithMessage(MultipleHandlerExceptionMessageExpected);
        }
    }
}

