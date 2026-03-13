using Microsoft.EntityFrameworkCore;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
    {
        // given
        Guid shoppingItemIt = Guid.NewGuid();
        string exceptionMessage = Randomizers.GetRandomString();

        var dbUpdateConcurrencyException =
            new DbUpdateConcurrencyException(exceptionMessage);

        var lockedShoppingItemException =
            new LockedShoppingItemException(
                exceptionMessage: "Locked shopping item error occurred, " +
                "try again please!",
                innerException: dbUpdateConcurrencyException);

        var expectedShoppingItemDependencyValidationException =
            new ShoppingItemDependencyValidationException(
                exceptionMessage: "Shopping item dependency validation error occurred, " +
                "try again please!",
                innerException: lockedShoppingItemException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemIt,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dbUpdateConcurrencyException);

        // when
        ValueTask<ShoppingItem> removeShoppingItemByIdTask =
            _shoppingItemService.RemoveShoppingItemByIdAsync(
                shoppingItemIt,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyValidationException>(
            removeShoppingItemByIdTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemIt,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemDependencyValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
