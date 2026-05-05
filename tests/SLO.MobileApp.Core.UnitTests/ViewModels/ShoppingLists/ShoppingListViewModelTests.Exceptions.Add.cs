using FluentAssertions;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Fact]
    public async Task ShouldRenderNullShoppingListItemErrorMessageOnAddIfIfShoppingListItemIsNullAsync()
    {
        // given
        ShoppingListItem nullShoppingListItem = null;

        string expectedErrorMessage =
            "Shopping list item is null.";

        // when
        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(nullShoppingListItem);

        // then
        _shoppingListViewModel.ErrorMessage.Should().BeEquivalentTo(
            expectedErrorMessage);
    }
}
