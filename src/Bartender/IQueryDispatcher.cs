using System.Collections.Generic;

namespace Bartender
{
    /// <summary>
    /// Define a query dispatcher
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Dispatch a query
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        /// <returns>ReadModel</returns>
        TReadModel Dispatch<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery;
    }
}

