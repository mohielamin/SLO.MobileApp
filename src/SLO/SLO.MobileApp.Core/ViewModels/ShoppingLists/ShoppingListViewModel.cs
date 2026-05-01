using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingItems;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.ViewModels.ShoppingLists;

internal partial class ShoppingListViewModel : ObservableObject
{
    private readonly IShoppingItemService _shoppingItemService;

    public ShoppingListViewModel(
        IShoppingItemService shoppingItemService) =>
        _shoppingItemService = shoppingItemService;

    public ObservableCollection<ShoppingItem> ShoppingListItems { get; private set; }

    [ObservableProperty]
    private string errorMessage;

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task AddShoppingListItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken) =>
        await TryCatch(async () =>
        {
            ShoppingItem addShoppingItem =
                await _shoppingItemService.AddShoppingItemAsync(
                    shoppingItem, cancellationToken);

            if (ShoppingListItems is null)
            {
                ShoppingListItems =
                    new ObservableCollection<ShoppingItem>();
            }

            ShoppingListItems.Add(item: shoppingItem);
        });

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RetrieveAllShoppingItemsAsync(
        CancellationToken cancellationToken) =>
        await TryCatch(async () =>
        {
            IQueryable<ShoppingItem> retrievedShoppingItems =
                await _shoppingItemService.RetrieveAllShoppingItemsAsync(
                    cancellationToken);

            ShoppingListItems =
                new ObservableCollection<ShoppingItem>(
                    list: retrievedShoppingItems.ToList());
        });

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task ModifyShoppingListItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken)
    {
        ShoppingItem matchingShoppingItem =
            ShoppingListItems.FirstOrDefault(shoppingListItem =>
                shoppingListItem.Id == shoppingItem.Id);

        if (matchingShoppingItem is null)
        {
            ErrorMessage = $"A shopping list item with Id: {shoppingItem.Id}, " +
                "could not be found.";

            return;
        }

        ShoppingListItems.Remove(item: matchingShoppingItem);
        ShoppingListItems.Add(item: shoppingItem);
    }
}
