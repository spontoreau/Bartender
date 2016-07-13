using System.Threading.Tasks;

namespace Bartender
{
    /// <summary>
    /// Define an asynchronous query dispatcher
    /// </summary>
    public interface IAsyncQueryDispatcher
    {
        /// <summary>
        /// Dispatch a query asynchronously.
        /// </summary>
        /// <param name="query">Query to dispatch</param>
        Task<TReadModel> DispatchAsync<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery;
    }
}

