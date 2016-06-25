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
        Mock<IAsyncCommandHandler<Command, Result>> MockedAsyncCommandHandler { get; set; }
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
            MockedAsyncCommandHandler = new Mock<IAsyncCommandHandler<Command, Result>>();
            MockedAsyncCommandHandler.Setup(method => method.Handle(It.IsAny<Command>())).Returns(Task.FromResult(It.IsAny<Result>()));

            MockedVoidCommandHandler = new Mock<ICommandHandler<Command>>();
            MockedVoidAsyncCommandHandler = new Mock<IAsyncCommandHandler<Command>>();

            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command, Result>>()).Returns(() => new[] { MockedCommandHandler.Object });
            MockedLocator.Setup(method => method.GetService<ICommandHandler<Command, Result>>()).Returns(() => MockedCommandHandler.Object);

            MockedLocator.Setup(method => method.GetAllServices<IAsyncCommandHandler<Command, Result>>()).Returns(() => new[] { MockedAsyncCommandHandler.Object });
            MockedLocator.Setup(method => method.GetService<IAsyncCommandHandler<Command, Result>>()).Returns(() => MockedAsyncCommandHandler.Object);

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
            dispatcher.Dispatch(new Command());
            MockedVoidCommandHandler.Verify(method => method.Handle(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenNoCommandHandlerWithoutReturn()
        {
            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command>>()).Returns(() => new ICommandHandler<Command>[] { });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(() => dispatcher.Dispatch(new Command()));
            action.ShouldThrowExactly<NoHandlerException>().WithMessage(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenMultipleCommandHandlerWithoutReturn()
        {
            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command>>()).Returns(() => new [] { MockedVoidCommandHandler.Object, MockedVoidCommandHandler.Object });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var action = new Action(() => dispatcher.Dispatch(new Command()));
            action.ShouldThrowExactly<MultipleHandlerException>().WithMessage(MultipleHandlerExceptionMessageExpected);
        }
        #endregion

        #region Aynchronous dispatch tests
        [Fact]
        public async void ShouldCallHandle_WhenDispatchCommandAsync()
        {
            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            await dispatcher.DispatchAsync<Command, Result>(new Command());
            MockedAsyncCommandHandler.Verify(method => method.Handle(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public async void ShouldReturnValue_WhenHandleCommandAsync()
        {
            var expected = Fixture.Create<Result>();
            var command = Fixture.Create<Command>();
            Command handledCommand = null;

            MockedAsyncCommandHandler.Setup(method => method.Handle(It.IsAny<Command>()))
                .Returns(Task.FromResult(expected))
                .Callback<Command>(c => handledCommand = c);

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            var actual = await dispatcher.DispatchAsync<Command, Result>(command);

            command.ShouldBeEquivalentTo(handledCommand);
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenNoAsyncCommandHandler()
        {
            MockedLocator.Setup(method => method.GetAllServices<IAsyncCommandHandler<Command, Result>>()).Returns(() => new IAsyncCommandHandler<Command, Result>[] { });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            Func<Task> actual = async () => { await dispatcher.DispatchAsync<Command, Result>(new Command()); };
            actual.ShouldThrowExactly<NoHandlerException>().WithMessage(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenMultipleAsyncCommandHandler()
        {
            MockedLocator.Setup(method => method.GetAllServices<IAsyncCommandHandler<Command, Result>>()).Returns(() => new [] { MockedAsyncCommandHandler.Object, MockedAsyncCommandHandler.Object });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            Func<Task> actual = async () => await dispatcher.DispatchAsync<Command, Result>(new Command());
            actual.ShouldThrowExactly<MultipleHandlerException>().WithMessage(MultipleHandlerExceptionMessageExpected);
        }
        #endregion

        #region Asynchronous dispatch tests without results
        [Fact]
        public async void ShouldCallHandle_WhenDispatchAsyncCommandWithoutReturn()
        {
            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            await dispatcher.DispatchAsync(new Command());
            MockedVoidAsyncCommandHandler.Verify(method => method.Handle(It.IsAny<Command>()), Times.Once);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenNoAsyncCommandHandlerWithoutReturn()
        {
            MockedLocator.Setup(method => method.GetAllServices<IAsyncCommandHandler<Command>>()).Returns(() => new IAsyncCommandHandler<Command>[] { });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            Func<Task> actual = async () => await dispatcher.DispatchAsync(new Command());
            actual.ShouldThrowExactly<NoHandlerException>().WithMessage(NoHandlerExceptionMessageExpected);
        }

        [Fact]
        public void ShouldThrowExcpetion_WhenMultipleAsyncCommandHandlerWithoutReturn()
        {
            MockedLocator.Setup(method => method.GetAllServices<IAsyncCommandHandler<Command>>()).Returns(() => new [] { MockedVoidAsyncCommandHandler.Object, MockedVoidAsyncCommandHandler.Object });

            var dispatcher = new CommandDispatcher(MockedLocator.Object);
            Func<Task> actual = async () => await dispatcher.DispatchAsync(new Command());
            actual.ShouldThrowExactly<MultipleHandlerException>().WithMessage(MultipleHandlerExceptionMessageExpected);
        }
        #endregion
    }
}

