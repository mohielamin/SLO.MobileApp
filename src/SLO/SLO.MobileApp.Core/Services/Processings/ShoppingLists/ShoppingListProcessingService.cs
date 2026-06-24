using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Services.Foundations.ShoppingLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingLists;

internal partial class ShoppingListProcessingService : IShoppingListProcessingService
{
    private readonly IShoppingListService _shoppingListService;
    private readonly ILoggingBroker _loggingBroker;

    public ShoppingListProcessingService(
        IShoppingListService shoppingListService,
        ILoggingBroker loggingBroker)
    {
        _shoppingListService = shoppingListService;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<IReadOnlyList<ShoppingList>> RetrieveAllShoppingListsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken) =>
        await TryCatch(
            cancellationToken,
            async () =>
            {
                ValidateShoppingListOnRetrieveAllByUserId(userId);

                IQueryable<ShoppingList> retrievedShoppingLists =
                    await _shoppingListService.RetrieveAllShoppingListsAsync(
                        cancellationToken);

                return MatchingUserShoppingLists(
                    shoppingLists: retrievedShoppingLists,
                    userId);
            });

    private IReadOnlyList<ShoppingList> MatchingUserShoppingLists(
        IQueryable<ShoppingList> shoppingLists,
        Guid userId)
    {
        return shoppingLists.Where(shoppingList =>
            shoppingList.CreatedBy == userId)
            .ToList();
    }
}
