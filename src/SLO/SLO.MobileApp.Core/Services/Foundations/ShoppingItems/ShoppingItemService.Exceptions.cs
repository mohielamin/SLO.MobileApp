using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingItems;

internal sealed partial class ShoppingItemService
{
    private delegate ValueTask<ShoppingItem> ReturningShoppingItemFunctions();
    private delegate ValueTask<IQueryable<ShoppingItem>> ReturningShoppingItemIQueryableFunctions();

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
        catch (InvalidShoppingItemException ex)
        {
            throw await CreateValidationErrorAsync(ex);
        }
        catch (NotFoundShoppingItemException ex)
        {
            throw await CreateValidationErrorAsync(ex);
        }
        catch (DuplicateKeyException ex)
        {
            var alreadyExistsShoppingItemException =
                new AlreadyExistsShoppingItemException(
                    exceptionMessage: "A shopping item with the same Id, " +
                    "already exists.",
                    innerException: ex);

            throw await CreateDependencyValidationErrorAsync(
                alreadyExistsShoppingItemException);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var lockedShoppingItemException =
                new LockedShoppingItemException(
                    exceptionMessage: "Locked shopping item error occurred, " +
                    "try again please!",
                    innerException: ex);

            throw await CreateDependencyValidationErrorAsync(
                lockedShoppingItemException);
        }
        catch (DbUpdateException ex)
        {
            var failedShoppingItemStorageException =
                new FailedShoppingItemStorageException(
                    exceptionMessage: "Failed shopping item storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateDependencyErrorAsync(
                failedShoppingItemStorageException);
        }
        catch (SqlException ex)
        {
            var failedShoppingItemStorageException =
                new FailedShoppingItemStorageException(
                    exceptionMessage: "Failed shopping item storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateCriticalDependencyErrorAsync(
                failedShoppingItemStorageException);
        }
        catch (Exception ex)
        {
            var failedShoppingItemServiceException =
                new FailedShoppingItemServiceException(
                    exceptionMessage: "Failed shopping item service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateServiceErrorAsync(
                failedShoppingItemServiceException);
        }
    }

    private async ValueTask<IQueryable<ShoppingItem>> TryCatch(
        ReturningShoppingItemIQueryableFunctions returningShoppingItemIQueryableFunctions)
    {
        try
        {
            return await returningShoppingItemIQueryableFunctions();
        }
        catch (SqlException ex)
        {
            var failedShoppingItemStorageException =
                new FailedShoppingItemStorageException(
                    exceptionMessage: "Failed shopping item storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateCriticalDependencyErrorAsync(
                failedShoppingItemStorageException);
        }
        catch (Exception ex)
        {
            var failedShoppingItemServiceException =
                new FailedShoppingItemServiceException(
                    exceptionMessage: "Failed shopping item service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateServiceErrorAsync(
                failedShoppingItemServiceException);
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

    private async ValueTask<ShoppingItemDependencyValidationException> CreateDependencyValidationErrorAsync(
        Exception exception)
    {
        var shoppingItemDependencyValidationException =
            new ShoppingItemDependencyValidationException(
                exceptionMessage: "Shopping item dependency validation error occurred, " +
                "try again please!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            shoppingItemDependencyValidationException);

        return shoppingItemDependencyValidationException;
    }

    private async ValueTask<ShoppingItemDependencyException> CreateDependencyErrorAsync(
        Exception exception)
    {
        var shoppingItemDependencyException =
            new ShoppingItemDependencyException(
                exceptionMessage: "Shopping item dependency error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            shoppingItemDependencyException);

        return shoppingItemDependencyException;
    }

    private async ValueTask<ShoppingItemDependencyException> CreateCriticalDependencyErrorAsync(
        Exception exception)
    {
        var shoppingItemDependencyException =
            new ShoppingItemDependencyException(
                exceptionMessage: "Shopping item dependency error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogCriticalAsync(
            shoppingItemDependencyException);

        return shoppingItemDependencyException;
    }

    private async ValueTask<ShoppingItemServiceException> CreateServiceErrorAsync(
        Exception exception)
    {
        var shoppingItemServiceException =
            new ShoppingItemServiceException(
                exceptionMessage: "Shopping item service error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            shoppingItemServiceException);

        return shoppingItemServiceException;
    }
}
