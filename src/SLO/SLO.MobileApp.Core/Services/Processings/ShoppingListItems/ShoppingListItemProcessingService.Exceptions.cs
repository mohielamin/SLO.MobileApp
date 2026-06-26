using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingListItems;

internal partial class ShoppingListItemProcessingService
{
    private delegate ValueTask<ShoppingListItem> ReturningShoppingListItemFunction();

    private async ValueTask<ShoppingListItem> TryCatch(
        CancellationToken cancellationToken,
        ReturningShoppingListItemFunction returningShoppingListItemFunction)
    {
        try
        {
            return await returningShoppingListItemFunction();
        }
        catch (NullShoppingListItemProcessingException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (InvalidShoppingListItemProcessingException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (ShoppingListItemDependencyValidationException ex)
        {
            throw await CreateAndLogDependencyValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (ShoppingListItemValidationException ex)
        {
            throw await CreateAndLogDependencyValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (ShoppingListItemDependencyException ex)
        {
            throw await CreateAndLogDependencyErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (ShoppingListItemServiceException ex)
        {
            throw await CreateAndLogDependencyErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (Exception ex)
        {
            var failedShoppingListItemProcessingServiceException =
                new FailedShoppingListItemProcessingServiceException(
                    exceptionMessage: "Failed shopping list item processing service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogServiceErrorAsync(
                exception: failedShoppingListItemProcessingServiceException,
                cancellationToken);
        }
    }

    private async ValueTask<ShoppingListItemProcessingValidationException> CreateAndLogValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemProcessingValidationException =
            new ShoppingListItemProcessingValidationException(
                exceptionMessage: "Shopping list item processing validation error occurred, " +
                "fix the errors and try again please!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListItemProcessingValidationException,
            cancellationToken);

        return shoppingListItemProcessingValidationException;
    }

    private async ValueTask<ShoppingListItemProcessingDependencyValidationException> CreateAndLogDependencyValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemProcessingDependencyValidationException =
            new ShoppingListItemProcessingDependencyValidationException(
                exceptionMessage: "Shopping list item processing dependency validation error occurred, " +
                "please try again!",
                innerException: exception.InnerException);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListItemProcessingDependencyValidationException,
            cancellationToken);

        return shoppingListItemProcessingDependencyValidationException;
    }

    private async ValueTask<ShoppingListItemProcessingDependencyException> CreateAndLogDependencyErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemProcessingDependencyError =
            new ShoppingListItemProcessingDependencyException(
                exceptionMessage: "Shopping list item processing dependency error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListItemProcessingDependencyError,
            cancellationToken);


        return shoppingListItemProcessingDependencyError;
    }

    private async ValueTask<ShoppingListItemProcessingServiceException> CreateAndLogServiceErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemProcessingServiceException =
            new ShoppingListItemProcessingServiceException(
                exceptionMessage: "Shopping list item processing service error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListItemProcessingServiceException,
            cancellationToken);

        return shoppingListItemProcessingServiceException;
    }
}
