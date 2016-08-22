using Bartender;

namespace ConsoleApplication.Domain.Personne.Read
{
    public class GetPersonQueryHandler : IHandler<GetPersonQuery, GetPersonReadModel>
    {
        public GetPersonReadModel Handle(GetPersonQuery message)
        {
            return new GetPersonReadModel();
        }
    }
}
