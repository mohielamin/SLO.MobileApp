using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldRetrieveShoppingItemByIdAsync()
    {
        // given
        ShoppingItem randomShoppingItem = CreateRandomShoppingItem();
        ShoppingItem storageShoppingItem = randomShoppingItem;
        Guid shoppingItemId = storageShoppingItem.Id;
        ShoppingItem expectedShoppingItem = storageShoppingItem.DeepClone();

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItem);

        // when
        ShoppingItem actualShoppingItem =
            await _shoppingItemService.RetrieveShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingItem.Should().BeEquivalentTo(expectedShoppingItem);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
