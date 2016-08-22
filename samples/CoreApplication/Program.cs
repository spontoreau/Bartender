using System;
using System.Threading.Tasks;
using Bartender;
using ConsoleApplication.Registries;
using CoreApplication.Domain.Personne.Create;
using CoreApplication.Domain.Personne.Read;
using StructureMap;

namespace ConsoleApplication
{
    public class Program
    {
        static IContainer Container { get; set; }

        public static void Main(string[] args)
        {
            var registry = new Registry();
            registry.IncludeRegistry<InfrastructureRegistry>();
            Container = new Container(registry);

            Task t = RunAsync();
            t.Wait();
            Console.ReadKey();
        }

        static async Task RunAsync()
        {
            var dispatcher = Container.GetInstance<IAsyncDispatcher>();
            await dispatcher.DispatchAsync(new CreatePersonCommand());

            var person = await dispatcher.DispatchAsync<GetPersonQuery, GetPersonReadModel>(new GetPersonQuery());

            Console.WriteLine($"Hello {person.Name} !");
        }
    }
}
