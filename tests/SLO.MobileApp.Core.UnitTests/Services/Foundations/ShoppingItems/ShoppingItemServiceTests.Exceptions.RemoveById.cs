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

    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRemoveByIdIfDbUpdateErrorOccursAndLogItAsync()
    {
        // given
        Guid shoppingItemIt = Guid.NewGuid();
        string exceptionMessage = Randomizers.GetRandomString();
        var dbUpdateException = new DbUpdateException(exceptionMessage);

        var failedShoppingItemStorageException =
            new FailedShoppingItemStorageException(
                exceptionMessage: "Failed shopping item storage error occurred, " +
                "please contact support.",
                innerException: dbUpdateException);

        var expectedShoppingItemDependencyException =
            new ShoppingItemDependencyException(
                exceptionMessage: "Shopping item dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingItemStorageException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemIt,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dbUpdateException);

        // when
        ValueTask<ShoppingItem> removeShoppingItemByIdTask =
            _shoppingItemService.RemoveShoppingItemByIdAsync(
                shoppingItemIt,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyException>(
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
                    expectedShoppingItemDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRemoveByIdIfServiceErrorOccursAndLogItAsync()
    {
        // given
        Guid shoppingItemIt = Guid.NewGuid();
        string exceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(exceptionMessage);

        var failedShoppingItemServiceException =
            new FailedShoppingItemServiceException(
                exceptionMessage: "Failed shopping item service error occurred, " +
                "please contact support.",
                innerException: someServiceException);

        var expectedShoppingItemServiceException =
            new ShoppingItemServiceException(
                exceptionMessage: "Shopping item service error occurred, " +
                "please contact support.",
                innerException: failedShoppingItemServiceException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemIt,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<ShoppingItem> removeShoppingItemByIdTask =
            _shoppingItemService.RemoveShoppingItemByIdAsync(
                shoppingItemIt,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemServiceException>(
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
                    expectedShoppingItemServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
