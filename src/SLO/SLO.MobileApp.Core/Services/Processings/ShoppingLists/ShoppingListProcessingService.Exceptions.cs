using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Processings.ShoppingLists.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingLists;

internal partial class ShoppingListProcessingService
{
    private delegate ValueTask<IReadOnlyList<ShoppingList>> ReturingShoppingListsFunction();

    private async ValueTask<IReadOnlyList<ShoppingList>> TryCatch(
        CancellationToken cancellationToken,
        ReturingShoppingListsFunction returingShoppingListsFunction)
    {
        try
        {
            return await returingShoppingListsFunction();
        }
        catch (InvalidShoppingListProcessingException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (Exception ex)
        {
            var failedShoppingListProcessingServiceException =
                new FailedShoppingListProcessingServiceException(
                    exceptionMessage: "Failed shopping list processing service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogServiceErrorAsync(
                exception: failedShoppingListProcessingServiceException,
                cancellationToken);
        }
    }

    private async ValueTask<ShoppingListProcessingValidationException> CreateAndLogValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListProcessingValidationException =
            new ShoppingListProcessingValidationException(
                exceptionMessage: "Shopping list processing validation error occurred, " +
                "fix the errors and try again please!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListProcessingValidationException,
            cancellationToken);

        return shoppingListProcessingValidationException;
    }

    private async ValueTask<ShoppingListProcessingServiceException> CreateAndLogServiceErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListProcessingServiceException =
            new ShoppingListProcessingServiceException(
                exceptionMessage: "Shopping list processing service error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListProcessingServiceException,
            cancellationToken);

        return shoppingListProcessingServiceException;
    }
}
