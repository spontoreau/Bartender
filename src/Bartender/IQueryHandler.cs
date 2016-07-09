using System.Collections.Generic;

namespace Bartender
{
    /// <summary>
    /// Define a query handler
    /// </summary>
    public interface IQueryHandler<in TQuery, out TReadModel> 
        where TQuery : IQuery
    {
        /// <summary>
        /// Handle a query
        /// </summary>
        /// <param name="query">Query to handle</param>
        /// <returns>ReadModel</returns>
        TReadModel Handle(TQuery query);
    }
}

