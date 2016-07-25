using System;
using Bartender;

namespace ConsoleApplication
{
    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        public void Handle(CreatePersonCommand command)
        {
            Console.WriteLine("CreatePersonCommand");
        }
    }
}