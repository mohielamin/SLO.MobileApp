using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingLists;

public partial class ShoppingListServiceTests
{
    [Fact]
    public async Task ShouldCreateShoppingListAsync()
    {
        // given
        ShoppingList randomShoppingList =
            CreateRandomShoppingList();

        ShoppingList inputShoppingList =
            randomShoppingList;

        ShoppingList storageShoppingList =
            inputShoppingList;

        ShoppingList expectedShoppingList =
            storageShoppingList.DeepClone();

        _storageBrokerMock.Setup(broker =>
            broker.InsertShoppingListAsync(
                inputShoppingList,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingList);

        // when
        ShoppingList actualShoppingList =
            await _shoppingListService.AddShoppingListAsync(
                shoppingList: inputShoppingList,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        actualShoppingList.Should().BeEquivalentTo(
            expectedShoppingList);

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
                inputShoppingList,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
