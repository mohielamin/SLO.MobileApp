using FluentAssertions;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Fact]
    public async Task ShouldRenderNotFoundErrorMessageOnRemoveIfShoppingListItemIsNotFoundAsync()
    {
        // given
        var currentShoppingListItems =
            new ObservableCollection<ShoppingListItem>(
                list: CreateRandomShoppingListItems().ToList());

        ObservableCollection<ShoppingListItem> expectedShoppingListItems =
            currentShoppingListItems;

        ShoppingListItem randomShoppingListItem = CreateRandomShoppingListItem();
        ShoppingListItem notFoundShoppingListItem = randomShoppingListItem;
        ShoppingListItem inputShoppingListItem = notFoundShoppingListItem;

        string expectedErrorMessage =
            $"A shopping list item with Id: {notFoundShoppingListItem.Id}, " +
            $"could not be found.";


        // when
        foreach (ShoppingListItem shoppingItem in currentShoppingListItems)
        {
            await _shoppingListViewModel.AddShoppingListItemCommand
                .ExecuteAsync(parameter: shoppingItem);
        }

        await _shoppingListViewModel.RemoveShoppingListItemCommand
            .ExecuteAsync(parameter: inputShoppingListItem);

        // then
        _shoppingListViewModel.ShoppingListItems.Should().BeEquivalentTo(
            expectedShoppingListItems);

        _shoppingListViewModel.ErrorMessage.Should().BeEquivalentTo(
            expectedErrorMessage);
    }
}
