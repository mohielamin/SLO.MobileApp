using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    [Fact]
    public async Task ShouldRemoveShoppingListItemByIdAsync()
    {
        // given
        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem storageShoppingListItem =
            randomShoppingListItem;

        ShoppingListItem deletedShoppingListItem =
            storageShoppingListItem;

        ShoppingListItem expectedShoppingListItem =
            deletedShoppingListItem.DeepClone();

        Guid shoppingListItemId = storageShoppingListItem.Id;

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListItemByIdAsync(
                shoppingListItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingListItem);

        _storageBrokerMock.Setup(broker =>
            broker.DeleteShoppingListItemAsync(
                storageShoppingListItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletedShoppingListItem);

        // when
        ShoppingListItem actualShoppingListItem =
            await _shoppingListItemService.RemoveShoppingListItemByIdAsync(
                shoppingListItemId,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        actualShoppingListItem.Should().BeEquivalentTo(
            expectedShoppingListItem);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                shoppingListItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingListItemAsync(
                storageShoppingListItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
