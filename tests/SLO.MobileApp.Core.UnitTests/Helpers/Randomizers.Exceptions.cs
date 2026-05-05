using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace SLO.MobileApp.Core.UnitTests.Helpers;

internal static partial class Randomizers
{
    private static SqlException _sqlException;

    public static SqlException GetSqlException()
    {
        if (_sqlException is null)
        {
            _sqlException =
                (SqlException)RuntimeHelpers
                .GetUninitializedObject(typeof(SqlException));
        }

        return _sqlException;
    }
}
