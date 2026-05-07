using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingLists;

public partial class ShoppingListServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid someShoppingListId = Guid.NewGuid();
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
            broker.SelectShoppingListByIdAsync(
                someShoppingListId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<ShoppingList> retrieveShoppingListByIdTask =
            _shoppingListService.RetrieveShoppingListByIdAsync(
                someShoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListDependencyException>(
            retrieveShoppingListByIdTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                someShoppingListId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogCriticalAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
