using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogItAsync()
    {
        // given
        var sqlException = Randomizers.GetSqlException();

        var failedShoppingListItemStorageException =
            new FailedShoppingListItemStorageException(
                exceptionMessage: "Failed shopping list item storage error occurred, " +
                "please contact support.",
                innerException: sqlException);

        var expectedShoppingListItemDependencyException =
            new ShoppingListItemDependencyException(
                exceptionMessage: "Shopping list item dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingListItemStorageException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<IQueryable<ShoppingListItem>> retrieveAllShoppingListItemsTask =
            _shoppingListItemService.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemDependencyException>(
            retrieveAllShoppingListItemsTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogCriticalAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
    {
        // given
        string exceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(exceptionMessage);

        var failedShoppingListItemServiceException =
            new FailedShoppingListItemServiceException(
                exceptionMessage: "Failed shopping list item service error occurred, " +
                "please contact support.",
                innerException: someServiceException);

        var expectedShoppingListItemServiceException =
            new ShoppingListItemServiceException(
                exceptionMessage: "Shopping list item service error occurred, " +
                "please contact support.",
                innerException: failedShoppingListItemServiceException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<IQueryable<ShoppingListItem>> retrieveAllShoppingListItemsTask =
            _shoppingListItemService.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemServiceException>(
            retrieveAllShoppingListItemsTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
