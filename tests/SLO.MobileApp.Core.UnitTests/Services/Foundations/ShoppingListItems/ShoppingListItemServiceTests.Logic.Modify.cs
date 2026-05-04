using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    [Fact]
    public async Task ShouldModifyShoppingListItemAsync()
    {
        // given
        DateTimeOffset currentDatetime = Randomizers.GetRandomDateTime();

        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem(
                dateTimes: currentDatetime);

        ShoppingListItem storageShoppingListItem =
            randomShoppingListItem.DeepClone();

        ShoppingListItem inputShoppingListItem =
            randomShoppingListItem;

        inputShoppingListItem.UpdatedAt =
            inputShoppingListItem.UpdatedAt.AddMinutes(1);

        ShoppingListItem updatedShoppingListItem =
            inputShoppingListItem;

        ShoppingListItem expectedShoppingListItem =
            updatedShoppingListItem.DeepClone();

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDatetime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListItemByIdAsync(
                inputShoppingListItem.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingListItem);

        _storageBrokerMock.Setup(broker =>
            broker.UpdateShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedShoppingListItem);

        // when
        ShoppingListItem actualShoppingListItem =
            await _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: inputShoppingListItem,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        actualShoppingListItem.Should().BeEquivalentTo(
            expectedShoppingListItem);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                inputShoppingListItem.Id,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
