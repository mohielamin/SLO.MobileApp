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
    public ObservableCollection<ShoppingItem> ShoppingListItems { get; } =
        new ObservableCollection<ShoppingItem>();

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
        (_, int index) =
            GetMatchingShoppingListItem(shoppingItem);

        if (index == -1)
        {
            ErrorMessage = $"A shopping list item with Id: {shoppingItem.Id}, " +
                "could not be found.";

            return;
        }

        ShoppingListItems[index] = shoppingItem;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RemoveShoppingListItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken)
    {
        (_, int index) = GetMatchingShoppingListItem(shoppingItem);

        if (index == -1)
        {
            ErrorMessage = $"A shopping list item with Id: {shoppingItem.Id}, " +
                $"could not be found.";

            return;
        }

        ShoppingListItems.RemoveAt(index);
    }

    private (ShoppingItem ShoppingListItem, int Index) GetMatchingShoppingListItem(
        ShoppingItem shoppingItem)
    {
        ShoppingItem matchingShoppingListItem =
            ShoppingListItems.FirstOrDefault(listItem =>
                listItem.Id == shoppingItem.Id);

        if (matchingShoppingListItem is null)
        {
            return (ShoppingListItem: null, Index: -1);

        }

        int index = ShoppingListItems.IndexOf(
            item: matchingShoppingListItem);

        return (ShoppingListItem: matchingShoppingListItem, index);
    }
}
