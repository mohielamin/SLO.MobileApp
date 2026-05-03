using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;

internal partial class ShoppingListItemService
{
    private delegate ValueTask<ShoppingListItem> ReturningShoppingListItemFunctions();

    private async ValueTask<ShoppingListItem> TryCatch(
        CancellationToken cancellationToken,
        ReturningShoppingListItemFunctions returningShoppingListItemFunctions)
    {
        try
        {
            return await returningShoppingListItemFunctions();

        }
        catch (NullShoppingListItemException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (InvalidShoppingListItemException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
    }

    private async ValueTask<ShoppingListItemValidationException> CreateAndLogValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListItemValidationException,
            cancellationToken);

        return shoppingListItemValidationException;
    }
}
