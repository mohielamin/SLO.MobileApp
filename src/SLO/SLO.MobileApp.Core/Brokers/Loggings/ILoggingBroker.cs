using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Loggings;

public interface ILoggingBroker
{
    ValueTask LogErrorAsync(Exception exception,
        CancellationToken cancellationToken = default);

    ValueTask LogCriticalAsync(Exception exception,
        CancellationToken cancellationToken = default);
}
