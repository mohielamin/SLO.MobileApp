using SLO.MobileApp.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Models.Foundations.ShoppingItems.Exceptions;
using System;
using System.Threading.Tasks;

namespace SLO.MobileApp.Services.Foundations.ShoppingItems;

internal sealed partial class ShoppingItemService
{
    private delegate ValueTask<ShoppingItem> ReturningShoppingItemFunctions();

    private async ValueTask<ShoppingItem> TryCatch(
        ReturningShoppingItemFunctions returningShoppingItemFunctions)
    {
        try
        {
            return await returningShoppingItemFunctions();
        }
        catch (NullShoppingItemException ex)
        {
            throw await CreateValidationErrorAsync(ex);
        }
    }

    private async ValueTask<ShoppingItemValidationException> CreateValidationErrorAsync(
        Exception exception)
    {
        var shoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            shoppingItemValidationException);

        return shoppingItemValidationException;
    }
}
