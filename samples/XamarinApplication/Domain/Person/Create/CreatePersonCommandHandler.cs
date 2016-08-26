using System;
using System.Threading;
using System.Threading.Tasks;
using Bartender;

namespace XamarinApplication.Domain.Person.Create
{
    public class CreatePersonCommandHandler : ICancellableAsyncHandler<CreatePersonCommand>
    {
        public async Task HandleAsync(CreatePersonCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Run(() =>
                {
                    while(true)
                    {
                        Thread.Sleep(100);

                        if (cancellationToken.IsCancellationRequested)
                            cancellationToken.ThrowIfCancellationRequested();
                    }

                }, cancellationToken);
            }
            catch(OperationCanceledException)
            {
                //Make things here if you want !
            }
        }
    }
}

