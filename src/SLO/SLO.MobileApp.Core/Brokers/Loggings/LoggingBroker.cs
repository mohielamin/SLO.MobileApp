using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Loggings;

internal sealed class LoggingBroker : ILoggingBroker
{
    public async ValueTask LogErrorAsync(Exception exception,
        CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public async ValueTask LogCriticalAsync(Exception exception,
        CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();
}
