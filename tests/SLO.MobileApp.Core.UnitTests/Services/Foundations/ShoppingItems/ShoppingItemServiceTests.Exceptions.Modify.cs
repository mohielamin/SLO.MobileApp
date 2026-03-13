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
    public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(currentDateTime);

        randomShoppingItem.UpdatedAt =
            randomShoppingItem.UpdatedAt.AddMinutes(1);

        var exceptionMessage = Randomizers.GetRandomString();

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

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dbUpdateConcurrencyException);

        // when
        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                randomShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyValidationException>(
            modifyShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingItemAsync(
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
    public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(currentDateTime);

        randomShoppingItem.UpdatedAt =
            randomShoppingItem.UpdatedAt.AddMinutes(1);

        var exceptionMessage = Randomizers.GetRandomString();

        var dbUpdateException =
            new DbUpdateException(exceptionMessage);

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

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dbUpdateException);

        // when
        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                randomShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyException>(
            modifyShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingItemAsync(
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
    public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(currentDateTime);

        randomShoppingItem.UpdatedAt =
            randomShoppingItem.UpdatedAt.AddMinutes(1);

        var sqlException = Randomizers.GetSqlException();

        var failedShoppingItemStorageException =
            new FailedShoppingItemStorageException(
                exceptionMessage: "Failed shopping item storage error occurred, " +
                "please contact support.",
                innerException: sqlException);

        var expectedShoppingItemDependencyException =
            new ShoppingItemDependencyException(
                exceptionMessage: "Shopping item dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingItemStorageException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                randomShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyException>(
            modifyShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogCriticalAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(currentDateTime);

        randomShoppingItem.UpdatedAt =
            randomShoppingItem.UpdatedAt.AddMinutes(1);

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

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                randomShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemServiceException>(
            modifyShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingItemAsync(
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
