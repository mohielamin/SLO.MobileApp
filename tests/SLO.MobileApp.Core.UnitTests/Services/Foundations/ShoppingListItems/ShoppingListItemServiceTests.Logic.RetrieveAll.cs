using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllShoppingListItemsAsync()
    {
        // given
        IQueryable<ShoppingListItem> randomShoppingListItems =
            CreateRandomShoppingListItems();

        IQueryable<ShoppingListItem> storageShoppingListItems =
            randomShoppingListItems;

        IQueryable<ShoppingListItem> expectedShoppingListItems =
            storageShoppingListItems.DeepClone();

        _storageBrokerMock.Setup(broker =>
            broker.SelectAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingListItems);

        // when
        IQueryable<ShoppingListItem> actualShoppingListItems =
            await _shoppingListItemService.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>());

        // then
        actualShoppingListItems.Should().BeEquivalentTo(
            expectedShoppingListItems);

        _storageBrokerMock.Verify(broker =>
            broker.SelectAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
