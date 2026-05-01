using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.ViewModels.ShoppingLists;

public partial class ShoppingListViewModel : ObservableObject
{
    public ObservableCollection<ShoppingItem> ShoppingListItems { get; private set; }

    [ObservableProperty]
    private string errorMessage;

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task AddShoppingListItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken)
    {
        if (shoppingItem is null)
        {
            ErrorMessage = "Shopping list item is null.";

            return;
        }

        if (ShoppingListItems is null)
        {
            ShoppingListItems =
                new ObservableCollection<ShoppingItem>();
        }

        ShoppingListItems.Add(item: shoppingItem);
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RetrieveAllShoppingItemsAsync(
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

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

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RemoveShoppingListItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken)
    {
        ShoppingItem matchingShoppingListItem =
            ShoppingListItems.FirstOrDefault(shoppingListItem =>
                shoppingListItem.Id == shoppingItem.Id);

        if (matchingShoppingListItem is null)
        {
            ErrorMessage = $"A shopping list item with Id: {shoppingItem.Id}, " +
                $"could not be found.";

            return;
        }

        ShoppingListItems.Remove(item: matchingShoppingListItem);
    }
}
