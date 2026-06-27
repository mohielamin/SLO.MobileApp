using FluentAssertions;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingListItems;

public partial class ShoppingListItemProcessingServiceTests
{
    [Fact]
    public async Task ShouldRemoveShoppingListItemByIdAsync()
    {
        // given
        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem existsShoppingListItem =
            randomShoppingListItem;

        Guid shoppingListItemId = existsShoppingListItem.Id;

        ShoppingListItem removedShoppingListItem =
            existsShoppingListItem;

        ShoppingListItem exepctedShoppingListItem =
            removedShoppingListItem;

        _shoppingListItemServiceMock.Setup(service =>
            service.RemoveShoppingListItemByIdAsync(
                shoppingListItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(removedShoppingListItem);

        // when
        ShoppingListItem actualShoppingListItem =
            await _shoppingListItemProcessingService
            .RemoveShoppingListItemByIdAsync(
                shoppingListItemId,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        actualShoppingListItem.Should().BeEquivalentTo(exepctedShoppingListItem);

        _shoppingListItemServiceMock.Verify(service =>
            service.RemoveShoppingListItemByIdAsync(
                shoppingListItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
