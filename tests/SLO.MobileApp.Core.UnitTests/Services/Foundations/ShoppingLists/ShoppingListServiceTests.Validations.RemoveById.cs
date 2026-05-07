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
    public async Task ShouldThrowValidationExceptionOnRemoveByIdIfShoppingListIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        Guid shoppingListId = invalidId;

        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListException.AddData(
            key: nameof(shoppingListId),
            values: "Id is required.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListException);

        // when
        ValueTask<ShoppingList> removeShoppingListByIdTask =
            _shoppingListService.RemoveShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            removeShoppingListByIdTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingListAsync(
                It.IsAny<ShoppingList>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveByIdIfShoppingListNotFoundAndLogItAsync()
    {
        // given
        ShoppingList notFoundShoppingList = null;
        ShoppingList storageShoppingList = notFoundShoppingList;
        Guid notFoundShoppingListId = Guid.NewGuid();
        Guid shoppingListId = notFoundShoppingListId;

        var notFoundShoppingListException =
            new NotFoundShoppingListException(
                exceptionMessage: $"A shopping list with Id: {shoppingListId}, " +
                $"could not be found.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: notFoundShoppingListException);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingList);

        // when
        ValueTask<ShoppingList> removeShoppingListByIdTask =
            _shoppingListService.RemoveShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            removeShoppingListByIdTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingListAsync(
                It.IsAny<ShoppingList>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
