using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingLists;

public partial class ShoppingListServiceTests
{
    [Fact]
    public async Task ShouldCreateShoppingListAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingList randomShoppingList =
            CreateRandomShoppingList(
                dateTimes: currentDateTime);

        ShoppingList inputShoppingList =
            randomShoppingList;

        inputShoppingList.UpdatedBy =
            inputShoppingList.CreatedBy;

        ShoppingList storageShoppingList =
            inputShoppingList;

        ShoppingList expectedShoppingList =
            storageShoppingList.DeepClone();

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

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

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
                inputShoppingList,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
