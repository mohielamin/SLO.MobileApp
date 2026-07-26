using SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Coordinations.ShoppingLists;

internal partial class ShoppingListCoordinationService
{
    private delegate ValueTask<IQueryable<ShoppingListItem>> ReturningShoppingListItemsFunction();

    private async ValueTask<IQueryable<ShoppingListItem>> TryCatch(
        CancellationToken cancellationToken,
        ReturningShoppingListItemsFunction returningShoppingListItemsFunction)
    {
        try
        {
            return await returningShoppingListItemsFunction();
        }
        catch (InvalidShoppingListCoordinationException ex)
        {
            throw await CreateValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
    }

    private async ValueTask<ShoppingListCoordinationValidationException> CreateValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListCoordinationValidationException =
            new ShoppingListCoordinationValidationException(
                exceptionMessage: "Shopping list coordination validation error occurred, " +
                "fix the errors and try again please!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListCoordinationValidationException,
            cancellationToken);

        return shoppingListCoordinationValidationException;
    }
}
