using SLO.MobileApp.Core.Models.Processings.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.Users;

internal partial class UserProcessingService
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
            var failedUserProcessingServiceException =
                new FailedUserProcessingServiceException(
                    exceptionMessage: "Failed user processing service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogServiceErrorAsync(
                exception: failedUserProcessingServiceException,
                cancellationToken);
        }
    }

    private async ValueTask<UserProcessingServiceException> CreateAndLogServiceErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var userProcessingServiceException =
            new UserProcessingServiceException(
                exceptionMessage: "User processing service error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            userProcessingServiceException,
            cancellationToken);

        return userProcessingServiceException;
    }
}
