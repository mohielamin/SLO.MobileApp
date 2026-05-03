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
    public async Task ShouldAddShoppingItemListAsync()
    {
        // given
        DateTimeOffset currentDateTime =
            Randomizers.GetRandomDateTime();

        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem(
                dateTimes: currentDateTime);

        ShoppingListItem inputShoppingListIem =
            randomShoppingListItem;

        inputShoppingListIem.UpdatedBy =
            inputShoppingListIem.CreatedBy;

        ShoppingListItem insertedShoppingListItem =
            inputShoppingListIem;

        ShoppingListItem expectedShoppingListItem =
            insertedShoppingListItem.DeepClone();

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

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

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
                inputShoppingListIem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
