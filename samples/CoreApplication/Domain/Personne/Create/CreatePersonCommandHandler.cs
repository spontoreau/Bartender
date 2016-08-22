using System;
using System.Threading.Tasks;
using Bartender;

namespace CoreApplication.Domain.Personne.Create
{
    public class CreatePersonCommandHandler : IAsyncHandler<CreatePersonCommand>
    {
        public async Task HandleAsync(CreatePersonCommand command)
        {
            await Task.Run(() => {
                Console.WriteLine("CreatePersonCommand");
            });
        }
    }
}