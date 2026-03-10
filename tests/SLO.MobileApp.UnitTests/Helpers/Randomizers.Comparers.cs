using FluentAssertions;
using System;
using System.Linq.Expressions;

namespace SLO.MobileApp.UnitTests.Helpers;

internal static partial class Randomizers
{
    public static void AreEquivalent(object actual, object expected) =>
        CompareObjects(actual, expected);

    public static Expression<Func<Exception, bool>> SameExceptionAs(
        Exception expectedException) =>
        actualException =>
        CompareObjects(actualException, expectedException);

    private static bool CompareObjects(object actual, object expected)
    {
        actual.Should().BeEquivalentTo(expected);

        return true;
    }
}
