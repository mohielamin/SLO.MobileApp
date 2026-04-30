using FluentAssertions;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Fact]
    public async ValueTask ShouldAddShoppingListItemAsync()
    {
        // given
        IQueryable<ShoppingItem> randomShoppingListItems =
            CreateRandomShoppingItems();

        IQueryable<ShoppingItem> currentShoppingListItems =
            randomShoppingListItems;

        ShoppingItem randomShoppingListItem =
            CreateRandomShoppingItem();

        ShoppingItem inputShoppingListItem =
            randomShoppingListItem;

        ShoppingItem addedShoppingListItem =
            inputShoppingListItem;

        var expectedShoppingListItems =
            new ObservableCollection<ShoppingItem>(
                list: [addedShoppingListItem]);

        _shoppingItemServiceMock.Setup(service =>
            service.AddShoppingItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(addedShoppingListItem);

        // when
        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(parameter: inputShoppingListItem);

        // then
        _shoppingListViewModel.ShoppingListItems.Should().BeEqualTo(
            expectedShoppingListItems);

        _shoppingItemServiceMock.Verify(service =>
            service.AddShoppingItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
