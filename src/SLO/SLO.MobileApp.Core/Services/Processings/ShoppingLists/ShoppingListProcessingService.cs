using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Services.Foundations.ShoppingLists;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingLists;

internal class ShoppingListProcessingService : IShoppingListProcessingService
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

    public async ValueTask<IQueryable<ShoppingList>> RetrieveAllShoppingListsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
