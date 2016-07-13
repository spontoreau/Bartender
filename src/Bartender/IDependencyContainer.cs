using System.Collections.Generic;

namespace Bartender
{
    /// <summary>
    /// Define a dependency container
    /// </summary>
    public interface IDependencyContainer
    {
        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <returns>All instances corresponding to a T type.</returns>
        IEnumerable<T> GetAllInstances<T>();
    }
}