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
    public async Task ShouldAddShoppingItemAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(dateTimes: currentDateTime);

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

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        // when
        ShoppingItem actualShoppingItem =
            await _shoppingItemService.AddShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingItem.Should().BeEquivalentTo(expectedShoppingItem);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
