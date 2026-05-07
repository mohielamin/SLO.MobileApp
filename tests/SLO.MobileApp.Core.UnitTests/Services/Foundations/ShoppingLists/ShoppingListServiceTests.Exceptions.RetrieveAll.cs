using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingLists;

public partial class ShoppingListServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAsync()
    {
        // given
        var sqlException = Randomizers.GetSqlException();

        var failedShoppingListStorageException =
            new FailedShoppingListStorageException(
                exceptionMessage: "Failed shopping list storage error occurred, " +
                "please contact support.",
                innerException: sqlException);

        var expectedShoppingListDependencyException =
            new ShoppingListDependencyException(
                exceptionMessage: "Shopping list dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingListStorageException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectAllShoppingListsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<IQueryable<ShoppingList>> retrieveAllShoppingListsTask =
            _shoppingListService.RetrieveAllShoppingListsAsync(
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListDependencyException>(
            retrieveAllShoppingListsTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectAllShoppingListsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogCriticalAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
    {
        // given
        var sqlException = Randomizers.GetSqlException();
        string exceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(exceptionMessage);

        var failedShoppingListServiceException =
            new FailedShoppingListServiceException(
                exceptionMessage: "Failed shopping list service error occurred, " +
                "please contact support.",
                innerException: someServiceException);

        var expectedShoppingListServiceException =
            new ShoppingListServiceException(
                exceptionMessage: "Shopping list service error occurred, " +
                "please contact support.",
                innerException: failedShoppingListServiceException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectAllShoppingListsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<IQueryable<ShoppingList>> retrieveAllShoppingListsTask =
            _shoppingListService.RetrieveAllShoppingListsAsync(
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListServiceException>(
            retrieveAllShoppingListsTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectAllShoppingListsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
