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
        static void Main()
        {
            var registry = new Registry();
            registry.IncludeRegistry<InfrastructureRegistry>();
            var container = new Container(registry);

            var dispatcher = container.GetInstance<IDispatcher>();
            dispatcher.Dispatch(new CreatePersonCommand());

            var person = dispatcher.Dispatch<GetPersonQuery, GetPersonReadModel>(new GetPersonQuery());

            Console.WriteLine($"Hello {person.Name} !");
            Console.ReadKey();
        }
    }
}


