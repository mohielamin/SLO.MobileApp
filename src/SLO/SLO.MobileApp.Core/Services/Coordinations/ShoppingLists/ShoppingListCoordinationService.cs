using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Processings.ShoppingListItems;
using SLO.MobileApp.Core.Services.Processings.ShoppingLists;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Coordinations.ShoppingLists;

internal class ShoppingListCoordinationService : IShoppingListCoordinationService
{
    private readonly IShoppingListProcessingService _shoppingListProcessingService;
    private readonly IShoppingListItemProcessingService _shoppingListItemProcessingService;
    private readonly ILoggingBroker _loggingBroker;

    public ShoppingListCoordinationService(
        IShoppingListProcessingService shoppingListProcessingService,
        IShoppingListItemProcessingService shoppingListItemProcessingService,
        ILoggingBroker loggingBroker)
    {
        _shoppingListProcessingService = shoppingListProcessingService;
        _shoppingListItemProcessingService = shoppingListItemProcessingService;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<IQueryable<ShoppingListItem>> RetrieveAllShoppingListItemsByIdAsync(
        Guid shoppingListId,
        CancellationToken cancellationToken)
    {
        return await _shoppingListItemProcessingService
            .RetrieveAllShoppingListItemsByShoppingListIdAsync(
            shoppingListId,
            cancellationToken);
    }
}
