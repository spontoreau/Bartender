using System;
using System.Threading;
using System.Threading.Tasks;
using Bartender;

namespace XamarinApplication.Domain.Person.Read
{
    public class GetPersonQueryHandler : ICancellableAsyncHandler<GetPersonQuery, GetPersonReadModel>
    {
        public async Task<GetPersonReadModel> HandleAsync(GetPersonQuery message, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(100);

                        if (cancellationToken.IsCancellationRequested)
                            cancellationToken.ThrowIfCancellationRequested();
                    }

                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                //Make things here if you want !
            }

            return null;
        }
    }
}

