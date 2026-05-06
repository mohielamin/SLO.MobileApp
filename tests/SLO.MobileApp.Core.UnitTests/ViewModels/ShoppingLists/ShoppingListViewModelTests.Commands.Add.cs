using FluentAssertions;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Fact]
    public async Task ShouldAddShoppingListItemAsync()
    {
        // given
        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem inputShoppingListItem =
            randomShoppingListItem;

        ShoppingListItem addedShoppingListItem =
            inputShoppingListItem;

        var expectedShoppingListItems =
            new ObservableCollection<ShoppingListItem>(
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
