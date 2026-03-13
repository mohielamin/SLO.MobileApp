using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Helpers;

internal static partial class Randomizers
{
    public static string GetRandomString(
           int wordCount = 1,
           int wordMinLength = 2,
           int wordMaxLength = 5) =>
        new MnemonicString(wordCount,
            wordMinLength,
            wordMaxLength)
            .GetValue();
}
