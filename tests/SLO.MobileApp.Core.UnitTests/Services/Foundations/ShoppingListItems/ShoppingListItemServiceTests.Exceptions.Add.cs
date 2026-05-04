using Microsoft.EntityFrameworkCore;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
    {
        // given
        ShoppingListItem someShoppingListItem = CreateRandomShoppingListItem();

        someShoppingListItem.UpdatedBy =
            someShoppingListItem.CreatedBy;

        string exceptionMessage = Randomizers.GetRandomString();
        var dbUpdateException = new DbUpdateException(exceptionMessage);

        var failedShoppingListItemStorageException =
            new FailedShoppingListItemStorageException(
                exceptionMessage: "Failed shopping list item storage error occurred, " +
                "please contact support.",
                innerException: dbUpdateException);

        var expectedShoppingListItemDependencyException =
            new ShoppingListItemDependencyException(
                exceptionMessage: "Shopping list item dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingListItemStorageException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dbUpdateException);

        // when
        ValueTask<ShoppingListItem> addShoppingListItemTask =
            _shoppingListItemService.AddShoppingListItemAsync(
                someShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemDependencyException>(
            addShoppingListItemTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
    {
        // given
        ShoppingListItem someShoppingListItem = CreateRandomShoppingListItem();

        someShoppingListItem.UpdatedBy =
            someShoppingListItem.CreatedBy;

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

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<ShoppingListItem> addShoppingListItemTask =
            _shoppingListItemService.AddShoppingListItemAsync(
                someShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemDependencyException>(
            addShoppingListItemTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
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
