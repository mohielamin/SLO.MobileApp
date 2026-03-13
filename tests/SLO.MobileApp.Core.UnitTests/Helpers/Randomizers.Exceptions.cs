using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace SLO.MobileApp.Core.UnitTests.Helpers;

internal static partial class Randomizers
{
    public static SqlException GetSqlException() =>
        (SqlException)RuntimeHelpers
        .GetUninitializedObject(
            typeof(SqlException));
}
