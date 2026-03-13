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
    public async Task ShouldThrowValidationExceptionOnRemoveByIdIfShoppingItemIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        Guid shoppingItemId = invalidId;

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(shoppingItemId),
            values: "Id is required.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingItemException);

        // when
        ValueTask<ShoppingItem> removeShoppingItemByIdTask =
            _shoppingItemService.RemoveShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            removeShoppingItemByIdTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveByIdIfShoppingItemIdIsNotFoundAndLogItAsync()
    {
        // given
        Guid notFoundShoppingItemId = Guid.NewGuid();
        Guid shoppingItemId = notFoundShoppingItemId;
        ShoppingItem nullShoppingItem = null;
        ShoppingItem storageShoppingItem = nullShoppingItem;

        var notFoundShoppingItemException =
            new NotFoundShoppingItemException(
                exceptionMessage: $"A shopping item with Id: {shoppingItemId}, " +
                $"could not be found.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: notFoundShoppingItemException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItem);

        // when
        ValueTask<ShoppingItem> removeShoppingItemByIdTask =
            _shoppingItemService.RemoveShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            removeShoppingItemByIdTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
