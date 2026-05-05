using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRemoveByIdIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid someShoppingListItemId = Guid.NewGuid();
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
            broker.SelectShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<ShoppingListItem> removeShoppingListItemByIdTask =
            _shoppingListItemService.RemoveShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemDependencyException>(
            removeShoppingListItemByIdTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogCriticalAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
