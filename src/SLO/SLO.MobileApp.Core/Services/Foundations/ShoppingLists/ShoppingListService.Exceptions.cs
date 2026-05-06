using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingLists;

internal partial class ShoppingListService
{
    delegate ValueTask<ShoppingList> ReturningShoppingListFunctions();

    private async ValueTask<ShoppingList> TryCatch(
        CancellationToken cancellationToken,
        ReturningShoppingListFunctions returningShoppingListFunctions)
    {
        try
        {
            return await returningShoppingListFunctions();
        }
        catch (NullShoppingListException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (InvalidShoppingListException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
    }

    private async ValueTask<ShoppingListValidationException> CreateAndLogValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            shoppingListValidationException,
            cancellationToken);

        return shoppingListValidationException;
    }
}
