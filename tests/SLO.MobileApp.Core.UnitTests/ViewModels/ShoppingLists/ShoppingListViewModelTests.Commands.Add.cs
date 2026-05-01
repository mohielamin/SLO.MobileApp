using FluentAssertions;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Fact]
    public async ValueTask ShouldAddShoppingListItemAsync()
    {
        // given
        ShoppingItem randomShoppingListItem =
            CreateRandomShoppingItem();

        ShoppingItem inputShoppingListItem =
            randomShoppingListItem;

        ShoppingItem addedShoppingListItem =
            inputShoppingListItem;

        var expectedShoppingListItems =
            new ObservableCollection<ShoppingItem>(
                list: [addedShoppingListItem]);

        // when
        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(parameter: inputShoppingListItem);

        // then
        _shoppingListViewModel.ShoppingListItems.Should().BeEqualTo(
            expectedShoppingListItems);

        _shoppingListViewModel.ErrorMessage.Should().BeNull();
    }
}
