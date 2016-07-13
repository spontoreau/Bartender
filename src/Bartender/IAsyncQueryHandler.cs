using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define an asynchronous query handler
    /// </summary>
    public interface IAsyncQueryHandler<TQuery, TReadModel> 
        where TQuery : IQuery
    {
        /// <summary>
        /// Handle a query asynchronously
        /// </summary>
        /// <param name="query">Query to handle</param>
        /// <returns>ReadModel</returns>
        Task<TReadModel> HandleAsync(TQuery query);
    }
}

