using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingLists;

public partial class ShoppingListServiceTests
{
    [Fact]
    public async Task ShouldRemoveShoppingListByIdAsync()
    {
        // given
        ShoppingList randomShoppingList =
            CreateRandomShoppingList();

        ShoppingList storageShoppingList =
            randomShoppingList;

        ShoppingList deletedShoppingList =
            storageShoppingList;

        ShoppingList expectedShoppingList =
            deletedShoppingList.DeepClone();

        Guid shoppingListId = storageShoppingList.Id;

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingList);

        _storageBrokerMock.Setup(broker =>
            broker.DeleteShoppingListAsync(
                storageShoppingList,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletedShoppingList);

        // when
        ShoppingList actualShoppingList =
            await _shoppingListService.RemoveShoppingListByIdAsync(
                shoppingListId,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        actualShoppingList.Should().BeEquivalentTo(expectedShoppingList);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingListAsync(
                storageShoppingList,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
