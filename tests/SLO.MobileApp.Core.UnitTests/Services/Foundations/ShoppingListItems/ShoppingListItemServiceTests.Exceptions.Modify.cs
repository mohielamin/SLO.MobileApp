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
    public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
    {
        // given
        ShoppingListItem someShoppingListItem =
            CreateRandomShoppingListItem();

        someShoppingListItem.UpdatedAt =
            someShoppingListItem.UpdatedAt.AddMinutes(1);

        var sqlException = Randomizers.GetSqlException();

        var failedShoppingListItemStoragException =
            new FailedShoppingListItemStorageException(
                exceptionMessage: "Failed shopping list item storage error occurred, " +
                "please contact support.",
                innerException: sqlException);

        var expectedShoppingListItemDependencyException =
            new ShoppingListItemDependencyException(
                exceptionMessage: "Shopping list item dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingListItemStoragException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<ShoppingListItem> modifyShoppingListItemTask =
            _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: someShoppingListItem,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemDependencyException>(
            modifyShoppingListItemTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListItemAsync(
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

    [Fact]
    public async Task ShouldThrowCriticalServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
    {
        // given
        ShoppingListItem someShoppingListItem =
            CreateRandomShoppingListItem();

        someShoppingListItem.UpdatedAt =
            someShoppingListItem.UpdatedAt.AddMinutes(1);

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

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<ShoppingListItem> modifyShoppingListItemTask =
            _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: someShoppingListItem,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemServiceException>(
            modifyShoppingListItemTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
