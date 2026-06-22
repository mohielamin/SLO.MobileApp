using SLO.MobileApp.Core.Models.Foundations.Users.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.Users;

internal partial class UserService
{
    private delegate ValueTask<Guid> ReturningUserFunction();

    private async ValueTask<Guid> TryCatch(
        CancellationToken cancellationToken,
        ReturningUserFunction returningUserFunction)
    {
        try
        {
            return await returningUserFunction();
        }
        catch (Exception ex)
        {
            var failedUserServiceException =
                new FailedUserServiceException(
                    exceptionMessage: "Failed user service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogServiceErrorAsync(
                exception: failedUserServiceException,
                cancellationToken);
        }
    }

    private async ValueTask<UserServiceException> CreateAndLogServiceErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var userServiceException =
            new UserServiceException(
                exceptionMessage: "User service error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: userServiceException,
            cancellationToken);

        return userServiceException;
    }
}
