using Tynamix.ObjectFiller;

namespace SLO.MobileApp.UnitTests.Helpers;

internal static partial class Randomizers
{
    public static int GetRandomNumber(
        int min = 1,
        int max = 10) =>
        new IntRange(min, max)
        .GetValue();
}
