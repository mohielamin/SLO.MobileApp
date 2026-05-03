using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    [Fact]
    public async Task ShouldAddShoppingItemListAsync()
    {
        // given
        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem inputShoppingListIem =
            randomShoppingListItem;

        ShoppingListItem insertedShoppingListItem =
            inputShoppingListIem;

        ShoppingListItem expectedShoppingListItem =
            insertedShoppingListItem.DeepClone();

        _storageBrokerMock.Setup(broker =>
            broker.InsertShoppingListItemAsync(
                inputShoppingListIem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(insertedShoppingListItem);

        // when
        ShoppingListItem actualShoppingListItem =
            await _shoppingListItemService.AddShoppingListItemAsync(
                inputShoppingListIem,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        actualShoppingListItem.Should().BeEquivalentTo(
            expectedShoppingListItem);

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
                inputShoppingListIem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
