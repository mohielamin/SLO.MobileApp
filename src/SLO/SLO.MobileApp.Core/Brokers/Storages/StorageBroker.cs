using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SLO.MobileApp.Core.Models.Configurations;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

internal sealed partial class StorageBroker : EFxceptionsContext, IStorageBroker
{
    private readonly IOptions<LocalConfiguration> _localConfiguration;

    public StorageBroker(IOptions<LocalConfiguration> localConfiguration)
    {
        _localConfiguration = localConfiguration;
        EnsureCreated();
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder dbContextOptionsBuilder)
    {
        string connectionString =
            $"Data Source={_localConfiguration.Value.DatabaseFilePath}";

        dbContextOptionsBuilder.UseSqlite(connectionString);
    }

    private void EnsureCreated()
    {
        bool databaseFileExists =
            File.Exists(path: _localConfiguration.Value.DatabaseFilePath);

        if (databaseFileExists)
        {
            return;
        }

        Database.EnsureCreated();
    }

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
