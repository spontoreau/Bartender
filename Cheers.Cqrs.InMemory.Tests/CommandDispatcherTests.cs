using System;
using Cheers.Cqrs.Write;
using Moq;
using Xunit;
using Ploeh.AutoFixture;
using FluentAssertions;
using Cheers.ServiceLocator;
using Cheers.Cqrs.InMemory.Exceptions;

namespace Cheers.Cqrs.InMemory.Tests
{
    public class CommandDispatcherTests
    {
        public class Command : ICommand { public string Value { get; set; } }
        public class Result : IResult { public string Value { get; set; } }

        Mock<ICommandHandler<Command, Result>> MockedCommandHandler { get; set; }
        Mock<ICommandHandler<Command>> MockedVoidCommandHandler { get; set; }
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

            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command, Result>>()).Returns(() => new[] { MockedCommandHandler.Object });
            MockedLocator.Setup(method => method.GetService<ICommandHandler<Command, Result>>()).Returns(() => MockedCommandHandler.Object);
            MockedLocator.Setup(method => method.GetAllServices<ICommandHandler<Command>>()).Returns(() => new[] { MockedVoidCommandHandler.Object });
            MockedLocator.Setup(method => method.GetService<ICommandHandler<Command>>()).Returns(() => MockedVoidCommandHandler.Object);
        }

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
    }
}

