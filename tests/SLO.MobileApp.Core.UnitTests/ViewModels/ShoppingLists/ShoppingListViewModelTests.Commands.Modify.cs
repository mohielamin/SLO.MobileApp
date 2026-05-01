using FluentAssertions;
using Force.DeepCloner;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
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
        ShoppingItem randomShoppingItem = CreateRandomShoppingItem();
        ShoppingItem existsShoppingItem = randomShoppingItem;
        ShoppingItem modifiedShoppingItem = randomShoppingItem.DeepClone();

        modifiedShoppingItem.UpdatedAt =
            modifiedShoppingItem.UpdatedAt = DateTimeOffset.UtcNow;

        ObservableCollection<ShoppingItem> expectedShoppingItems =
            [modifiedShoppingItem];

        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(parameter: existsShoppingItem);

        // when
        await _shoppingListViewModel.ModifyShoppingListItemCommand
            .ExecuteAsync(parameter: modifiedShoppingItem);

        // then
        _shoppingListViewModel.ShoppingListItems.Should().BeEqualTo(
            expectedShoppingItems);
    }
}
