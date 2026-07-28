using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLO.MobileApp.Core.Brokers.Navigations;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Orchestrations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.ViewModels.Orchestrations.ShoppingListItemEditors;

public partial class ShoppingListItemEditorOrchestrationViewModel : ObservableObject
{
    private readonly IShoppingListItemService _shoppingListItemService;
    private readonly INavigationBroker _navigationBroker;

    [ObservableProperty]
    private Func<ShoppingListItem, ValueTask> callback;

    [ObservableProperty]
    private ShoppingListItem shoppingListItem;

    [ObservableProperty]
    private ShoppingListItemMode shoppingListItemMode;


    public ShoppingListItemEditorOrchestrationViewModel(
        IShoppingListItemService shoppingListItemService,
        INavigationBroker navigationBroker)
    {
        _shoppingListItemService = shoppingListItemService;
        _navigationBroker = navigationBroker;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task SaveAsync(
        CancellationToken cancellationToken)
    {
        ShoppingListItem savedShoppingListItem =
            ShoppingListItemMode switch
            {
                ShoppingListItemMode.Edit =>
                await EditShoppingListItemAsync(cancellationToken),

                _ => await AddShoppingListItemAsync(cancellationToken),
            };

        await Callback(savedShoppingListItem);

        await _navigationBroker.PopAsync(cancellationToken);
    }

    private async ValueTask<ShoppingListItem> AddShoppingListItemAsync(
        CancellationToken cancellationToken)
    {
        return await _shoppingListItemService.AddShoppingListItemAsync(
                ShoppingListItem,
                cancellationToken);
    }

    private async ValueTask<ShoppingListItem> EditShoppingListItemAsync(
        CancellationToken cancellationToken)
    {
        return await _shoppingListItemService.ModifyShoppingListItemAsync(
            ShoppingListItem,
            cancellationToken);
    }
}
