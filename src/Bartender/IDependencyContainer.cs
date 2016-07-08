using System.Collections.Generic;

namespace Bartender
{
    /// <summary>
    /// Define a dependency container
    /// </summary>
    public interface ILocator
    {
        /// <summary>
        /// Gets a service.
        /// </summary>
        /// <returns>The service.</returns>
        /// <typeparam name="TService">The 1st type parameter.</typeparam>
        TService GetService<TService>() where TService : class;

        /// <summary>
        /// Gets all services.
        /// </summary>
        /// <returns>The all services.</returns>
        /// <typeparam name="TService">The 1st type parameter.</typeparam>
        IEnumerable<TService> GetAllServices<TService>() where TService : class;
    }
}