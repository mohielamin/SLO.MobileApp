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
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid shoppingItemId = Guid.NewGuid();
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

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<ShoppingItem> retrieveShoppingItemByIdTask =
            _shoppingItemService.RetrieveShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyException>(
            retrieveShoppingItemByIdTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogCriticalAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
    {
        // given
        Guid shoppingItemId = Guid.NewGuid();
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
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<ShoppingItem> retrieveShoppingItemByIdTask =
            _shoppingItemService.RetrieveShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemServiceException>(
            retrieveShoppingItemByIdTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
