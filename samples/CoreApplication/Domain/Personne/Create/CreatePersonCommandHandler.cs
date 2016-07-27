using System;
using Bartender;

namespace ConsoleApplication.Domain.Personne.Create
{
    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        public void Handle(CreatePersonCommand command)
        {
            Console.WriteLine("CreatePersonCommand");
        }
    }
}