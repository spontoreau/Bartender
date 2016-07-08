using System.Collections.Generic;

namespace Cheers.Cqrs.Read
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
        /// <returns>Enumerable of ReadModel</returns>
        IEnumerable<TReadModel> Dispatch<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery
            where TReadModel : IReadModel;
    }
}

