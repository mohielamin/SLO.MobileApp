using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Brokers.Loggings;

public interface ILoggingBroker
{
    ValueTask LogErrorAsync(Exception exception,
        CancellationToken cancellationToken = default);
}
