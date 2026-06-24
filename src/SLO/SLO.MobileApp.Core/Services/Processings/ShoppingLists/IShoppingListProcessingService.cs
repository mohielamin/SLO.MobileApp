using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingLists;

internal interface IShoppingListProcessingService
{
    ValueTask<IReadOnlyList<ShoppingList>> RetrieveAllShoppingListsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
