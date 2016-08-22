using System;
using Bartender;
using ConsoleApplication.Domain.Personne.Create;
using ConsoleApplication.Domain.Personne.Read;
using ConsoleApplication.Registries;
using StructureMap;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var registry = new Registry();
            registry.IncludeRegistry<InfrastructureRegistry>();
            var container = new Container(registry);

            var createPersonCommandHandler = container.GetInstance<IHandler<CreatePersonCommand>>();
            createPersonCommandHandler.Handle(new CreatePersonCommand());

            var getPersonQueryHandler = container.GetInstance<IHandler<GetPersonQuery, GetPersonReadModel>>();
            var person = getPersonQueryHandler.Handle(new GetPersonQuery());

            Console.WriteLine($"Hello {person.Name} !");

            Console.ReadKey();
        }
    }
}


