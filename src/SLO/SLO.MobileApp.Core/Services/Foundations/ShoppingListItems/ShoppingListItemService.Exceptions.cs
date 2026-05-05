using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;

internal partial class ShoppingListItemService
{
    private delegate ValueTask<ShoppingListItem> ReturningShoppingListItemFunctions();
    private delegate ValueTask<IQueryable<ShoppingListItem>> ReturningShoppingListItemIQueryableFunctions();

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
        catch (NotFoundShoppingListItemException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (DuplicateKeyException ex)
        {
            var alreadyExistsShoppingListItemException =
                new AlreadyExistsShoppingListItemException(
                    exceptionMessage: "A shopping list item with same Id " +
                    "already exists.",
                    innerException: ex);

            throw await CreateAndLogDependencyValidationErrorAsync(
                exception: alreadyExistsShoppingListItemException,
                cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var lockedShoppingListItemException =
                new LockedShoppingListItemException(
                    exceptionMessage: "Locked shopping list item error occurred.",
                    innerException: ex);

            throw await CreateAndLogDependencyValidationErrorAsync(
                exception: lockedShoppingListItemException,
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
                exception: failedShoppingListItemStorageException,
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
                exception: failedShoppingListItemStorageException,
                cancellationToken);
        }
        catch (Exception ex)
        {
            var failedShoppingListItemServiceException =
                new FailedShoppingListItemServiceException(
                    exceptionMessage: "Failed shopping list item service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogServiceErrorAsync(
                exception: failedShoppingListItemServiceException,
                cancellationToken);
        }
    }

    private async ValueTask<IQueryable<ShoppingListItem>> TryCatch(
        CancellationToken cancellationToken,
        ReturningShoppingListItemIQueryableFunctions returningShoppingListItemIQueryableFunctions)
    {
        try
        {
            return await returningShoppingListItemIQueryableFunctions();
        }
        catch (SqlException ex)
        {
            var failedShoppingListItemStorageException =
                new FailedShoppingListItemStorageException(
                    exceptionMessage: "Failed shopping list item storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogCriticalDependencyErrorAsync(
                exception: failedShoppingListItemStorageException,
                cancellationToken);
        }
        catch (Exception ex)
        {
            var failedShoppingListItemServiceException =
                new FailedShoppingListItemServiceException(
                    exceptionMessage: "Failed shopping list item service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogServiceErrorAsync(
                exception: failedShoppingListItemServiceException,
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

    private async ValueTask<ShoppingListItemDependencyValidationException> CreateAndLogDependencyValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemDependencyValidationException =
            new ShoppingListItemDependencyValidationException(
                exceptionMessage: "Shopping list item dependency validation error occurred, " +
                "try again please!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListItemDependencyValidationException);

        return shoppingListItemDependencyValidationException;
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
            exception: shoppingListItemDependencyException);

        return shoppingListItemDependencyException;
    }

    private async ValueTask<ShoppingListItemServiceException> CreateAndLogServiceErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListItemServiceException =
            new ShoppingListItemServiceException(
                exceptionMessage: "Shopping list item service error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListItemServiceException,
            cancellationToken);

        return shoppingListItemServiceException;
    }
}
