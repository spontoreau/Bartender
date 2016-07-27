using System;
using Bartender;
using ConsoleApplication.Domain.Personne.Create;
using ConsoleApplication.Registries;
using StructureMap;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var registry = new Registry();
            registry.IncludeRegistry<InfrastructureRegistry>();
            var container = new Container(registry);

            var handler = container.GetInstance<ICommandHandler<CreatePersonCommand>>();

            handler.Handle(new CreatePersonCommand());

            Console.ReadKey();
        }
    }
}
