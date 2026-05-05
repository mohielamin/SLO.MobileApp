using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.ViewModels.ShoppingLists;

public partial class ShoppingListViewModel : ObservableObject
{
    public ObservableCollection<ShoppingListItem> ShoppingListItems { get; } =
        new ObservableCollection<ShoppingListItem>();

    [ObservableProperty]
    private string errorMessage;

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task AddShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken)
    {
        if (shoppingListItem is null)
        {
            ErrorMessage = "Shopping list item is null.";

            return;
        }

        ShoppingListItems.Add(item: shoppingListItem);
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RetrieveAllShoppingItemsAsync(
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task ModifyShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken)
    {
        (_, int index) =
            GetMatchingShoppingListItem(shoppingListItem);

        if (index == -1)
        {
            ErrorMessage = $"A shopping list item with Id: {shoppingListItem.Id}, " +
                "could not be found.";

            return;
        }

        ShoppingListItems[index] = shoppingListItem;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RemoveShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken)
    {
        (_, int index) = GetMatchingShoppingListItem(shoppingListItem);

        if (index == -1)
        {
            ErrorMessage = $"A shopping list item with Id: {shoppingListItem.Id}, " +
                $"could not be found.";

            return;
        }

        ShoppingListItems.RemoveAt(index);
    }

    private (ShoppingListItem ShoppingListItem, int Index) GetMatchingShoppingListItem(
        ShoppingListItem shoppingItem)
    {
        ShoppingListItem matchingShoppingListItem =
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
