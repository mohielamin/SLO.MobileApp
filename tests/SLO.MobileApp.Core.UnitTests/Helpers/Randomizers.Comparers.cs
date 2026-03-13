using FluentAssertions;
using System;
using System.Linq.Expressions;
using Xeptions;

namespace SLO.MobileApp.Core.UnitTests.Helpers;

internal static partial class Randomizers
{
    public static void AreEquivalent(object actual, object expected) =>
        CompareObjects(actual, expected);

    internal static Expression<Func<Exception, bool>> SameExceptionAs(
        Exception expectedException) =>
        actualException =>
        AreEqual(actualException, expectedException);

    private static bool AreEqual(Exception actualException,
        Exception expectedException)
    {
        actualException.Message.Should().BeEquivalentTo(expectedException.Message);

        actualException.InnerException.Message.Should().BeEquivalentTo(
            expectedException.InnerException.Message);

        if (actualException.InnerException is Xeption)
        {
            actualException.InnerException.Data.Should().BeEquivalentTo(
                expectedException.InnerException.Data);
        }

        return true;
    }

    private static bool CompareObjects(object actual, object expected)
    {
        actual.Should().BeEquivalentTo(expected);

        return true;
    }
}
