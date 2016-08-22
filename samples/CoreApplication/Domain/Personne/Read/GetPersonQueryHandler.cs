using System.Threading.Tasks;
using Bartender;

namespace CoreApplication.Domain.Personne.Read
{
    public class GetPersonQueryHandler : IAsyncHandler<GetPersonQuery, GetPersonReadModel>
    {
        public async Task<GetPersonReadModel> HandleAsync(GetPersonQuery message)
            =>
                await Task.Run<GetPersonReadModel>(() => new GetPersonReadModel());
    }
}