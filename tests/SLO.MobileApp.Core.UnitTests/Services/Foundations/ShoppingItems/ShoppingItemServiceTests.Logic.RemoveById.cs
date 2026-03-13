using FluentAssertions;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldRemoveShoppingItemByIdAsync()
    {
        // given
        ShoppingItem randomShoppingItem = CreateRandomShoppingItem();
        ShoppingItem storageShoppingItem = randomShoppingItem;
        ShoppingItem deletedShoppingItem = storageShoppingItem;
        Guid shoppingItemId = storageShoppingItem.Id;

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItem);

        _storageBrokerMock.Setup(broker =>
            broker.DeleteShoppingItemAsync(
                storageShoppingItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletedShoppingItem);

        // when
        ShoppingItem actualShoppingItem =
            await _shoppingItemService.RemoveShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingItem.Should().BeEquivalentTo(storageShoppingItem);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingItemAsync(
                storageShoppingItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
