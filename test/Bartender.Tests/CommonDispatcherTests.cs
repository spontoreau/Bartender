using Moq;
using Shouldly;
using Xunit;

namespace Bartender.Tests
{
    public class CommonDispatcherTests
    {
        [Fact]
        public void ShouldHaveDependencyContainer_WhenInstanceCreated()
        {
            var mockedDependencyContainer = new Mock<IDependencyContainer>();
            var dispatcher = new Dispatcher(mockedDependencyContainer.Object);
            dispatcher.Container.ShouldNotBeNull();
        }
    }
}