namespace Bartender
{
    /// <summary>
    /// Dispatcher.
    /// </summary>
    public class Dispatcher
    {
        /// <summary>
        /// Dependency container.
        /// </summary>
        private IDependencyContainer Container { get; }

        /// <summary>
        /// Initializes a new instance of the Dispatcher class.
        /// </summary>
        /// <param name="container">Dependency container.</param>
        public Dispatcher(IDependencyContainer container)
        {
            Container = container;
        }
    }
}