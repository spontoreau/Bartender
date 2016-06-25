using System;
using Cheers.Cqrs.Read;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Cheers.ServiceLocator;
using Cheers.Cqrs.InMemory.Exceptions;
using System.Threading.Tasks;

namespace Cheers.Cqrs.InMemory.Tests
{
    public class QueryDispatcherTests
    {
        public class Query : IQuery { public string Value { get; set; } }
        public class ReadModel : IReadModel { public string Value { get; set; } }

        Mock<IQueryHandler<Query, ReadModel>> MockedQueryHandler { get; set; }
        Mock<IAsyncQueryHandler<Query, ReadModel>> MockedAsyncQueryHandler { get; set; }
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

            MockedAsyncQueryHandler = new Mock<IAsyncQueryHandler<Query, ReadModel>>();

            MockedLocator.Setup(method => method.GetAllServices<IQueryHandler<Query, ReadModel>>()).Returns(() => new[] { MockedQueryHandler.Object });
            MockedLocator.Setup(method => method.GetService<IQueryHandler<Query, ReadModel>>()).Returns(() => MockedQueryHandler.Object);
            MockedLocator.Setup(method => method.GetAllServices<IAsyncQueryHandler<Query, ReadModel>>()).Returns(() => new[] { MockedAsyncQueryHandler.Object });
            MockedLocator.Setup(method => method.GetService<IAsyncQueryHandler<Query, ReadModel>>()).Returns(() => MockedAsyncQueryHandler.Object);
        }

        #region Synchronous dispatch tests
        [Fact]
        public void ShouldCallHandle_WhenDispatchQuery()
        {
            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            dispatcher.Dispatch<Query, ReadModel>(new Query());
            MockedQueryHandler.Verify(method => method.Handle(It.IsAny<Query>()), Times.Once);
        }

        [Fact]
        public void ShouldReturnValue_WhenDispatchQuery()
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
        #endregion

        #region Asynchronous dispatch tests
        [Fact]
        public async void ShouldCallHandle_WhenDispatchQueryAsync()
        {
            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            await dispatcher.DispatchAsync<Query, ReadModel>(new Query());
            MockedAsyncQueryHandler.Verify(method => method.Handle(It.IsAny<Query>()), Times.Once);
        }

        [Fact]
        public async void ShouldReturnValue_WhenDispatchQueryAsync()
        {
            var expected = Fixture.Create<IEnumerable<ReadModel>>();
            var query = Fixture.Create<Query>();
            Query handledQuery = null;

            MockedAsyncQueryHandler.Setup(method => method.Handle(It.IsAny<Query>()))
                .Returns(Task.FromResult(expected))
                .Callback<Query>(q => handledQuery = q);

            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            var actual = await dispatcher.DispatchAsync<Query, ReadModel>(query);

            query.ShouldBeEquivalentTo(handledQuery);
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenNoAsyncQueryHandler()
        {
            MockedLocator.Setup(method => method.GetAllServices<IAsyncQueryHandler<Query, ReadModel>>()).Returns(() => new IAsyncQueryHandler<Query, ReadModel>[] { });

            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            Func<Task> actual = async () => await dispatcher.DispatchAsync<Query, ReadModel>(new Query());
            actual.ShouldThrow<NoHandlerException>().WithMessage(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenMultipleAsyncQueryHandler()
        {
            MockedLocator.Setup(method => method.GetAllServices<IAsyncQueryHandler<Query, ReadModel>>()).Returns(() => new IAsyncQueryHandler<Query, ReadModel>[] { MockedAsyncQueryHandler.Object, MockedAsyncQueryHandler.Object });

            var dispatcher = new QueryDispatcher(MockedLocator.Object);
            Func<Task> actual = async () => await dispatcher.DispatchAsync<Query, ReadModel>(new Query());
            actual.ShouldThrow<MultipleHandlerException>().WithMessage(MultipleHandlerExceptionMessageExpected);

        }
        #endregion
    }
}

