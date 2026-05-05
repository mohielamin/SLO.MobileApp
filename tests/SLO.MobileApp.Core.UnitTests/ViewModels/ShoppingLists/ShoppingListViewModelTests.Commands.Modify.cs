using FluentAssertions;
using Force.DeepCloner;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Fact]
    public async Task ShouldModifyShoppingListItemAsync()
    {
        // given
        ShoppingListItem randomShoppingListItem = CreateRandomShoppingListItem();
        ShoppingListItem existsShoppingListItem = randomShoppingListItem;
        ShoppingListItem modifiedShoppingListItem = randomShoppingListItem.DeepClone();

        modifiedShoppingListItem.UpdatedAt =
            modifiedShoppingListItem.UpdatedAt = DateTimeOffset.UtcNow;

        ObservableCollection<ShoppingListItem> expectedShoppingListItems =
            [modifiedShoppingListItem];

        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(parameter: existsShoppingListItem);

        // when
        await _shoppingListViewModel.ModifyShoppingListItemCommand
            .ExecuteAsync(parameter: modifiedShoppingListItem);

        // then
        _shoppingListViewModel.ShoppingListItems.Should().BeEqualTo(
            expectedShoppingListItems);
    }
}
