using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingLists;

internal partial class ShoppingListService
{
    delegate ValueTask<ShoppingList> ReturningShoppingListFunctions();
    delegate ValueTask<IQueryable<ShoppingList>> ReturningShoppingListIQueryableFunctions();

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
        catch (NotFoundShoppingListException ex)
        {
            throw await CreateAndLogValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (DuplicateKeyException ex)
        {
            var alreadyExistsShoppingListException =
                new AlreadyExistsShoppingListException(
                    exceptionMessage: "A shopping list with same Id " +
                    "already exists.",
                    innerException: ex);

            throw await CreateAndLogDependencyValidationErrorAsync(
                exception: alreadyExistsShoppingListException,
                cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            var failedShoppingListStorageException =
                new FailedShoppingListStorageException(
                    exceptionMessage: "Failed shopping list storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogDependencyErrorAsync(
                exception: failedShoppingListStorageException,
                cancellationToken);
        }
        catch (SqlException ex)
        {
            var failedShoppingListStorageException =
                new FailedShoppingListStorageException(
                    exceptionMessage: "Failed shopping list storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogCriticalDependencyErrorAsync(
                exception: failedShoppingListStorageException,
                cancellationToken);
        }
        catch (Exception ex)
        {
            var failedShoppingListServiceException =
                new FailedShoppingListServiceException(
                    exceptionMessage: "Failed shopping list service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogServiceErrorAsync(
                exception: failedShoppingListServiceException,
                cancellationToken);
        }
    }

    private async ValueTask<IQueryable<ShoppingList>> TryCatch(
        CancellationToken cancellationToken,
        ReturningShoppingListIQueryableFunctions returningShoppingListIQueryableFunctions)
    {
        try
        {
            return await returningShoppingListIQueryableFunctions();
        }
        catch (SqlException ex)
        {
            var failedShoppingListStorageException =
                new FailedShoppingListStorageException(
                    exceptionMessage: "Failed shopping list storage error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogCriticalDependencyErrorAsync(
                exception: failedShoppingListStorageException,
                cancellationToken);
        }
        catch (Exception ex)
        {
            var failedShoppingListServiceException =
                new FailedShoppingListServiceException(
                    exceptionMessage: "Failed shopping list service error occurred, " +
                    "please contact support.",
                    innerException: ex);

            throw await CreateAndLogServiceErrorAsync(
                exception: failedShoppingListServiceException,
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

    private async ValueTask<ShoppingListDependencyValidationException> CreateAndLogDependencyValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListDependencyValidationException =
            new ShoppingListDependencyValidationException(
                exceptionMessage: "Shopping list dependency validation error occurred, " +
                "please try again!",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListDependencyValidationException,
            cancellationToken);

        return shoppingListDependencyValidationException;
    }

    private async ValueTask<ShoppingListDependencyException> CreateAndLogDependencyErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListDependencyException =
            new ShoppingListDependencyException(
                exceptionMessage: "Shopping list dependency error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListDependencyException,
            cancellationToken);

        return shoppingListDependencyException;
    }

    private async ValueTask<ShoppingListDependencyException> CreateAndLogCriticalDependencyErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListDependencyException =
            new ShoppingListDependencyException(
                exceptionMessage: "Shopping list dependency error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogCriticalAsync(
            exception: shoppingListDependencyException,
            cancellationToken);

        return shoppingListDependencyException;
    }

    private async ValueTask<ShoppingListServiceException> CreateAndLogServiceErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListServiceException =
            new ShoppingListServiceException(
                exceptionMessage: "Shopping list service error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListServiceException,
            cancellationToken);

        return shoppingListServiceException;
    }
}
