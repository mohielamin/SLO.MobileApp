using System;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Helpers;

internal static partial class Randomizers
{
    internal static DateTimeOffset GetRandomDateTime() =>
        new DateTimeRange(
            earliestDate: new DateTime())
        .GetValue();
}
