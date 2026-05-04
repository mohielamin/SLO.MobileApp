using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        catch (DbUpdateException ex)
        {
            var failedShoppingListItemStorageException =
                new FailedShoppingListItemStorageException(
                    exceptionMessage: "Failed shopping list item storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogDependencyErrorAsync(
                failedShoppingListItemStorageException,
                cancellationToken);
        }
        catch (SqlException ex)
        {
            var failedShoppingListItemStorageException =
                new FailedShoppingListItemStorageException(
                    exceptionMessage: "Failed shopping list item storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogCriticalDependencyErrorAsync(
                failedShoppingListItemStorageException,
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

    private async ValueTask<ShoppingListItemDependencyException> CreateAndLogCriticalDependencyErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemDependencyException =
            new ShoppingListItemDependencyException(
                exceptionMessage: "Shopping list item dependency error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogCriticalAsync(
            exception: shoppingListItemDependencyException);

        return shoppingListItemDependencyException;
    }

    private async ValueTask<ShoppingListItemDependencyException> CreateAndLogDependencyErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemDependencyException =
            new ShoppingListItemDependencyException(
                exceptionMessage: "Shopping list item dependency error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            shoppingListItemDependencyException);

        return shoppingListItemDependencyException;
    }
}
