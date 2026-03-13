using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.DateTimes;

internal class DateTimeBroker : IDateTimeBroker
{
    public async ValueTask<DateTimeOffset> GetCurrentDateTimeAsync(
        CancellationToken cancellationToken) =>
        DateTimeOffset.UtcNow;
}
