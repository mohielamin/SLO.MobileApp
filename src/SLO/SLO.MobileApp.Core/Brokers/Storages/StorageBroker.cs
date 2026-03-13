using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

internal sealed partial class StorageBroker : IStorageBroker
{
    private async ValueTask<T> InsertAsync<T>(
        T item, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    private async ValueTask<IQueryable<T>> SelectAllAsync<T>(
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    private async ValueTask<T> SelectByIdAsync<T>(
        CancellationToken cancellationToken,
        params Guid[] ids) =>
        throw new NotImplementedException();

    private async ValueTask<T> UpdateAsync<T>(
        T item, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    private async ValueTask<T> DeleteAsync<T>(
        T item, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
