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
    public async Task ShouldModifyShoppingListAsync()
    {
        // given
        DateTimeOffset currentDateTime =
            Randomizers.GetRandomDateTime();

        ShoppingList randomShoppingList =
            CreateRandomShoppingList(
                dateTimes: currentDateTime);

        ShoppingList storageShoppingList =
            randomShoppingList.DeepClone();

        ShoppingList updatedShoppingList =
            randomShoppingList;

        updatedShoppingList.UpdatedAt =
            updatedShoppingList.UpdatedAt.AddMinutes(1);

        ShoppingList inputShoppingList =
            updatedShoppingList;

        ShoppingList expectedShoppingList =
            updatedShoppingList.DeepClone();

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListByIdAsync(
                inputShoppingList.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingList);

        _storageBrokerMock.Setup(broker =>
            broker.UpdateShoppingListAsync(
                inputShoppingList,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedShoppingList);

        // when
        ShoppingList actualShoppingList =
            await _shoppingListService.ModifyShoppingListAsync(
                shoppingList: inputShoppingList,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        actualShoppingList.Should().BeEquivalentTo(expectedShoppingList);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                inputShoppingList.Id,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListAsync(
                inputShoppingList,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
