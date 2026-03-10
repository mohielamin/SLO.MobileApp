using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Models.Foundations.ShoppingItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldAddShoppingItemAsync()
    {
        // given
        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem();

        ShoppingItem inputShoppingItem =
            randomShoppingItem;

        inputShoppingItem.UpdatedBy =
            inputShoppingItem.CreatedBy;

        ShoppingItem storageShoppingItem =
            inputShoppingItem.DeepClone();

        ShoppingItem expectedShoppingItem =
            storageShoppingItem;

        _storageBrokerMock.Setup(broker =>
            broker.InsertShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItem);

        // when
        ShoppingItem actualShoppingItem =
            await _shoppingItemService.AddShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingItem.Should().BeEquivalentTo(expectedShoppingItem);

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
