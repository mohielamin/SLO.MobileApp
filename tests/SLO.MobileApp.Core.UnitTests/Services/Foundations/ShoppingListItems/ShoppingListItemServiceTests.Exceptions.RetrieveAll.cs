using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
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

        var expectedShoppingListItemDepdenencyException =
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
                    expectedShoppingListItemDepdenencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
