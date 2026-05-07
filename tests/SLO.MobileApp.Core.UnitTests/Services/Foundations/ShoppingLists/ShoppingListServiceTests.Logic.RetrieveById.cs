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
    public async Task ShouldRetrieveShoppingListByIdAsync()
    {
        // given
        ShoppingList randomShoppingList = CreateRandomShoppingList();
        ShoppingList storageShoppingList = randomShoppingList;
        ShoppingList expectedShoppingList = storageShoppingList.DeepClone();
        Guid shoppingListId = storageShoppingList.Id;

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingList);

        // when
        ShoppingList actualShoppingList =
            await _shoppingListService.RetrieveShoppingListByIdAsync(
                shoppingListId,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        actualShoppingList.Should().BeEquivalentTo(expectedShoppingList);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
