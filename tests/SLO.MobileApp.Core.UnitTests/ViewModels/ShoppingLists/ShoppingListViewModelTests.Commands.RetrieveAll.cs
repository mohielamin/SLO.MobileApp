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
    public async Task ShouldRetrieveAllShoppingListItemsAsync()
    {
        // given
        IQueryable<ShoppingItem> randomShoppingItems =
            CreateRandomShoppingItems();

        IQueryable<ShoppingItem> retrievedShoppingItems =
            randomShoppingItems;

        ObservableCollection<ShoppingItem> expectedShoppingItems =
           new ObservableCollection<ShoppingItem>(
               list: retrievedShoppingItems.ToList());

        _storageBrokerMock.Setup(broker =>
            broker.SelectAllShoppingItemsAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(retrievedShoppingItems);

        // when
        await _shoppingListViewModel
            .RetrieveAllShoppingItemsCommand
            .ExecuteAsync(parameter: null);

        // then
        _shoppingListViewModel.ShoppingItems.Should().BeEquivalentTo(
            expectedShoppingItems);

        _storageBrokerMock.Verify(broker =>
            broker.SelectAllShoppingItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
