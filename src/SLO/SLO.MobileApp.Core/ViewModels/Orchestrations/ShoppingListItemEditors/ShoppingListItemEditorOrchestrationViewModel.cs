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
        ShoppingListItem addedShoppingListItem =
            await _shoppingListItemService.AddShoppingListItemAsync(
                ShoppingListItem,
                cancellationToken);

        await Callback(addedShoppingListItem);

        await _navigationBroker.PopAsync(cancellationToken);
    }
}
