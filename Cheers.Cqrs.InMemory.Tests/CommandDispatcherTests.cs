using System;
using Cheers.Cqrs.Write;
using Moq;
using Xunit;
using Ploeh.AutoFixture;
using FluentAssertions;
using Cheers.ServiceLocator;
using Cheers.Cqrs.InMemory.Exceptions;
using System.Threading.Tasks;

namespace Cheers.Cqrs.InMemory.Tests
{
    public class CommandDispatcherTests
    {
        public class Command : ICommand { public string Value { get; set; } }
        public class Result : IResult { public string Value { get; set; } }

        Mock<ICommandHandler<Command, Result>> MockedCommandHandler { get; set; }
        Mock<ICommandHandler<Command>> MockedVoidCommandHandler { get; set; }
        Mock<IAsyncCommandHandler<Command>> MockedVoidAsyncCommandHandler { get; set; }
        Mock<ILocator> MockedLocator { get; set; }
        Fixture Fixture { get; set; }

        readonly string MultipleHandlerExceptionMessageExpected = $"Multiple command handlers for '{typeof(Command).Name}'.";
        readonly string NoHandlerExceptionMessageExpected = $"No command handler for '{typeof(Command).Name}'.";

        public CommandDispatcherTests()
        {
            Fixture = new Fixture();

            MockedLocator = new Mock<ILocator>();

            MockedCommandHandler = new Mock<ICommandHandler<Command, Result>>();
            MockedCommandHandler.Setup(method => method.Handle(It.IsAny<Command>())).Returns(It.IsAny<Result>);

            MockedVoidCommandHandler = new Mock<ICommandHandler<Command>>();
            MockedVoidAsyncCommandHandler = new Mock<IAsyncCommandHandler<Command>>();

            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command, Result>>()).Returns(() => new[] { MockedCommandHandler.Object });
            MockedLocator.Setup(method => method.GetService<ICommandHandler<Command, Result>>()).Returns(() => MockedCommandHandler.Object);
            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command>>()).Returns(() => new[] { MockedVoidCommandHandler.Object });
            MockedLocator.Setup(method => method.GetService<ICommandHandler<Command>>()).Returns(() => MockedVoidCommandHandler.Object);
            MockedLocator.Setup(method => method.GetAllServices<IAsyncCommandHandler<Command>>()).Returns(() => new[] { MockedVoidAsyncCommandHandler.Object });
            MockedLocator.Setup(method => method.GetService<IAsyncCommandHandler<Command>>()).Returns(() => MockedVoidAsyncCommandHandler.Object);
        }

        #region Synchronous dispatch tests
        [Fact]
        public void ShouldCallHandle_WhenDispatchCommand()
        {
            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            dispatcher.Dispatch<Command, Result>(new Command());
            MockedCommandHandler.Verify(method => method.Handle(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public void ShouldReturnValue_WhenHandleCommand()
        {
            var expected = Fixture.Create<Result>();
            var command = Fixture.Create<Command>();
            Command handledCommand = null;

            MockedCommandHandler.Setup(method => method.Handle(It.IsAny<Command>()))
                .Returns(expected)
                .Callback<Command>(c => handledCommand = c);

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var actual = dispatcher.Dispatch<Command, Result>(command);

            command.ShouldBeEquivalentTo(handledCommand);
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenNoCommandHandler()
        {
            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command, Result>>()).Returns(() => new ICommandHandler<Command, Result>[] { });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(() => dispatcher.Dispatch<Command, Result>(new Command()));
            action.ShouldThrowExactly<NoHandlerException>().WithMessage(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenMultipleCommandHandler()
        {
            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command, Result>>()).Returns(() => new [] { MockedCommandHandler.Object, MockedCommandHandler.Object });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(() => dispatcher.Dispatch<Command, Result>(new Command()));
            action.ShouldThrowExactly<MultipleHandlerException>().WithMessage(MultipleHandlerExceptionMessageExpected);
        }
        #endregion

        #region Synchronous dispatch tests without results
        [Fact]
        public void ShouldCallHandle_WhenDispatchCommandWithoutReturn()
        {
            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            dispatcher.Dispatch<Command>(new Command());
            MockedVoidCommandHandler.Verify(method => method.Handle(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenNoCommandHandlerWithoutReturn()
        {
            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command>>()).Returns(() => new ICommandHandler<Command>[] { });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(() => dispatcher.Dispatch<Command>(new Command()));
            action.ShouldThrowExactly<NoHandlerException>().WithMessage(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenMultipleCommandHandlerWithoutReturn()
        {
            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command>>()).Returns(() => new [] { MockedVoidCommandHandler.Object, MockedVoidCommandHandler.Object });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(() => dispatcher.Dispatch<Command>(new Command()));
            action.ShouldThrowExactly<MultipleHandlerException>().WithMessage(MultipleHandlerExceptionMessageExpected);
        }
        #endregion

        #region Aynchronous dispatch tests

        #endregion

        #region Asynchronous dispatch tests without results
        [Fact]
        public void ShouldCallHandle_WhenDispatchAsyncCommandWithoutReturn()
        {
            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(async () => await dispatcher.DispatchAsync<Command>(new Command()));
            action.Invoke();
            MockedVoidAsyncCommandHandler.Verify(method => method.Handle(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenNoAsyncCommandHandlerWithoutReturn()
        {
            MockedLocator.Setup(method => method.GetAllServices<IAsyncCommandHandler<Command>>()).Returns(() => new IAsyncCommandHandler<Command>[] { });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(async () => await dispatcher.DispatchAsync<Command>(new Command()));
            Exception actual = null;
            try
            {
                action.Invoke();
            }
            catch(Exception ex)
            {
                actual = ex;
            }

            actual
                .Should().NotBeNull()
                .And.Subject
                .Should().BeOfType<NoHandlerException>()
                .Which.Message.ShouldBeEquivalentTo(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenMultipleAsyncCommandHandlerWithoutReturn()
        {
            MockedLocator.Setup(method => method.GetAllServices<IAsyncCommandHandler<Command>>()).Returns(() => new [] { MockedVoidAsyncCommandHandler.Object, MockedVoidAsyncCommandHandler.Object });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(async () => await dispatcher.DispatchAsync<Command>(new Command()));
            Exception actual = null;
            try
            {
                action.Invoke();
            }
            catch(Exception ex)
            {
                actual = ex;
            }

            actual
                .Should().NotBeNull()
                .And.Subject
                .Should().BeOfType<MultipleHandlerException>()
                .Which.Message.ShouldBeEquivalentTo(MultipleHandlerExceptionMessageExpected);
        }
        #endregion
    }
}

