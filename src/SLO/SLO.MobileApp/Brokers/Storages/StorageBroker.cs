using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Brokers.Storages;

internal sealed partial class StorageBroker : IStorageBroker
{
    private async ValueTask<T> InsertAsync<T>(
        T item, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
