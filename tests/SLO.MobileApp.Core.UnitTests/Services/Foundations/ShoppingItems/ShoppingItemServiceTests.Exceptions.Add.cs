using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
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
    public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
    {
        // given
        ShoppingItem someShoppingItem = CreateRandomShoppingItem();

        someShoppingItem.UpdatedBy =
            someShoppingItem.CreatedBy;

        string exceptionMessage = Randomizers.GetRandomString();

        var duplicateKeyException =
            new DuplicateKeyException(exceptionMessage);

        var alreadyExistsShoppingItemException =
            new AlreadyExistsShoppingItemException(
                exceptionMessage: "A shopping item with the same Id, " +
                "already exists.",
                innerException: duplicateKeyException);

        var expectedShoppingItemDependencyValidationException =
            new ShoppingItemDependencyValidationException(
                exceptionMessage: "Shopping item dependency validation error occurred, " +
                "try again please!",
                innerException: alreadyExistsShoppingItemException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(duplicateKeyException);

        // when
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                someShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyValidationException>(
            addShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
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
    public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
    {
        // given
        ShoppingItem someShoppingItem = CreateRandomShoppingItem();

        someShoppingItem.UpdatedBy =
            someShoppingItem.CreatedBy;

        string exceptionMessage = Randomizers.GetRandomString();

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
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                someShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyException>(
            addShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
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
    public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
    {
        // given
        ShoppingItem someShoppingItem = CreateRandomShoppingItem();

        someShoppingItem.UpdatedBy =
            someShoppingItem.CreatedBy;

        SqlException sqlException = Randomizers.GetSqlException();

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
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                someShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyException>(
            addShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
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
    public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
    {
        // given
        ShoppingItem someShoppingItem = CreateRandomShoppingItem();

        someShoppingItem.UpdatedBy =
            someShoppingItem.CreatedBy;

        string randomExceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(randomExceptionMessage);

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
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                someShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemServiceException>(
            addShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
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
