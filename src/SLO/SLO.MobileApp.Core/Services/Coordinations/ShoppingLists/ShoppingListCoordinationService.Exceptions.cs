using SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;
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
        catch (ShoppingListItemProcessingValidationException ex)
        {
            throw await CreateDependencyValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (ShoppingListItemProcessingDependencyValidationException ex)
        {
            throw await CreateDependencyValidationErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (ShoppingListItemProcessingDependencyException ex)
        {
            throw await CreateDependencyErrorAsync(
                exception: ex,
                cancellationToken);
        }
        catch (ShoppingListItemProcessingServiceException ex)
        {
            throw await CreateDependencyErrorAsync(
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

    private async ValueTask<ShoppingListCoordinationDependencyValidationException> CreateDependencyValidationErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListCoordinationDependencyValidationException =
            new ShoppingListCoordinationDependencyValidationException(
                exceptionMessage: "Shopping list coordination dependency validation error occurred, " +
                "please try again!",
                innerException: exception.InnerException);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListCoordinationDependencyValidationException,
            cancellationToken);

        return shoppingListCoordinationDependencyValidationException;
    }

    private async ValueTask<ShoppingListCoordinationDependencyException> CreateDependencyErrorAsync(
        Exception exception,
        CancellationToken cancellationToken)
    {
        var shoppingListCoordinationDependencyException =
            new ShoppingListCoordinationDependencyException(
                exceptionMessage: "Shopping list coordination dependency error occurred, " +
                "please contact support.",
                innerException: exception);

        await _loggingBroker.LogErrorAsync(
            exception: shoppingListCoordinationDependencyException,
            cancellationToken);

        return shoppingListCoordinationDependencyException;
    }
}
