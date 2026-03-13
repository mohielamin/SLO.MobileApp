using FluentAssertions;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllShoppingItemsAsync()
    {
        // given
        IQueryable<ShoppingItem> randomShoppingItems =
            CreateRandomShoppingItems();

        IQueryable<ShoppingItem> storageShoppingItems =
            randomShoppingItems;

        IQueryable<ShoppingItem> expectedShoppingItems =
            storageShoppingItems;

        _storageBrokerMock.Setup(broker =>
            broker.SelectAllShoppingItemsAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItems);

        // when
        IQueryable<ShoppingItem> actualShoppingItems =
            await _shoppingItemService.RetrieveAllShoppingItemsAsync(
                It.IsAny<CancellationToken>());

        // then
        actualShoppingItems.Should().BeEquivalentTo(expectedShoppingItems);

        _storageBrokerMock.Verify(broker =>
            broker.SelectAllShoppingItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
