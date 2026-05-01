using FluentAssertions;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Fact]
    public async Task ShouldRenderNotFoundErrorMessageOnModifyIfShoppingListItemIsNotFoundAsync()
    {
        // given
        IQueryable<ShoppingItem> randomShoppingListItems =
            CreateRandomShoppingItems();

        var currentShoppingListItems =
            new ObservableCollection<ShoppingItem>(
                list: randomShoppingListItems.ToList());

        ShoppingItem randomShoppingItem = CreateRandomShoppingItem();
        ShoppingItem notFoundShoppingItem = randomShoppingItem;
        ShoppingItem inputShoppingListItem = notFoundShoppingItem;

        string expectedErrorMessage =
            $"A shopping list item with Id: {notFoundShoppingItem.Id}, " +
            $"could not be found.";


        // when
        foreach (ShoppingItem shoppingItem in currentShoppingListItems)
        {
            await _shoppingListViewModel.AddShoppingListItemCommand
                .ExecuteAsync(parameter: shoppingItem);
        }

        await _shoppingListViewModel.ModifyShoppingListItemCommand
            .ExecuteAsync(parameter: inputShoppingListItem);

        // then
        _shoppingListViewModel.ShoppingListItems.Should().BeEquivalentTo(
            currentShoppingListItems);

        _shoppingListViewModel.ErrorMessage.Should().BeEquivalentTo(
            expectedErrorMessage);
    }
}
