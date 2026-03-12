using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldModifyShoppingItemAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(dateTimes: currentDateTime);

        ShoppingItem storageShoppingItem = randomShoppingItem;

        ShoppingItem modifiedShoppingItem =
            storageShoppingItem.DeepClone();

        modifiedShoppingItem.CreatedAt =
            modifiedShoppingItem.CreatedAt.AddMinutes(1);

        ShoppingItem updatedShoppingItem = modifiedShoppingItem;
        ShoppingItem expectedShoppingItem = updatedShoppingItem;

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                modifiedShoppingItem.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItem);

        _storageBrokerMock.Setup(broker =>
            broker.UpdateShoppingItemAsync(
                modifiedShoppingItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedShoppingItem);

        // when
        ShoppingItem actualShoppingItem =
            await _shoppingItemService.ModifyShoppingItemAsync(
                modifiedShoppingItem,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingItem.Should().BeEquivalentTo(expectedShoppingItem);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                modifiedShoppingItem.Id,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingItemAsync(
                modifiedShoppingItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
