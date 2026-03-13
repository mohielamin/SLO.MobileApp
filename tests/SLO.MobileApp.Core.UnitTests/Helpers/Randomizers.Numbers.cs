using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Helpers;

internal static partial class Randomizers
{
    public static int GetRandomNegativeNumber(
        int min = 1,
        int max = 10) =>
        GetNumber(min, max) * -1;

    public static int GetRandomNumber(
        int min = 1,
        int max = 10) =>
        GetNumber(min, max);

    private static int GetNumber(
        int min,
        int max) =>
        new IntRange(min, max)
        .GetValue();
}
