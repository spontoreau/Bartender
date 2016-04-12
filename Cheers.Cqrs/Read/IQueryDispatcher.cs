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
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TReadModel">ReadModel type</typeparam>
        /// <param name="query">Query to dispatch</param>
        /// <returns>ReadModel</returns>
        TReadModel Dispatch<TQuery, TReadModel>(TQuery query)
            where TQuery : IQuery
            where TReadModel : IReadModel;
    }
}

