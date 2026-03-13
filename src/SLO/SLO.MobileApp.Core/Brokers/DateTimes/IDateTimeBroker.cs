using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.DateTimes;

public interface IDateTimeBroker
{
    ValueTask<DateTimeOffset> GetCurrentDateTimeAsync(
        CancellationToken cancellationToken);
}
