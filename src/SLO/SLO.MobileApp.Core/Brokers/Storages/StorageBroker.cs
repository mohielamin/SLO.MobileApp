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
    private readonly LocalConfiguration _localConfiguration;

    public StorageBroker(IOptions<LocalConfiguration> localConfigurationOptions)
    {
        _localConfiguration = localConfigurationOptions.Value;
        EnsureCreated();
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder dbContextOptionsBuilder)
    {
        string connectionString =
            $"Data Source={_localConfiguration.DatabaseFilePath}";

        dbContextOptionsBuilder.UseSqlite(connectionString);
    }

    private async ValueTask<T> InsertAsync<T>(
        T item, CancellationToken cancellationToken)
    {
        Entry(item).State = EntityState.Added;
        await SaveChangesAsync(cancellationToken);

        return item;
    }

    private async ValueTask<IQueryable<T>> SelectAllAsync<T>(
        CancellationToken cancellationToken) where T : class =>
        Set<T>();

    private async ValueTask<T> SelectByIdAsync<T>(
        CancellationToken cancellationToken,
        params Guid[] ids) where T : class =>
        await FindAsync<T>(ids, cancellationToken);

    private async ValueTask<T> UpdateAsync<T>(
        T item, CancellationToken cancellationToken)
    {
        Entry(item).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);

        return item;
    }

    private async ValueTask<T> DeleteAsync<T>(
        T item, CancellationToken cancellationToken)
    {
        Entry(item).State = EntityState.Deleted;
        await SaveChangesAsync(cancellationToken);

        return item;
    }

    private void EnsureCreated()
    {
        bool databaseFileExists =
            File.Exists(path: _localConfiguration.DatabaseFilePath);

        if (databaseFileExists)
        {
            EnsureMigrationApplied();

            return;
        }

        Database.EnsureCreatedAsync();
    }

    private void EnsureMigrationApplied()
    {
        if (Database.HasPendingModelChanges() is false)
        {
            return;
        }

        Database.MigrateAsync();
    }
}