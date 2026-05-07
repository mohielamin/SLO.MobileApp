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
    public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfShoppingListIdIsInvalidAndLogItAsync()
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
        ValueTask<ShoppingList> retrieveShoppingListByIdTask =
            _shoppingListService.RetrieveShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            retrieveShoppingListByIdTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
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
    public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfShoppingListIsNotFoundAndLogItAsync()
    {
        // given
        Guid randomId = Guid.NewGuid();
        Guid shoppingListId = randomId;
        ShoppingList nullShoppingList = null;
        ShoppingList notFoundShoppingList = nullShoppingList;
        ShoppingList storageShoppingList = notFoundShoppingList;

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
        ValueTask<ShoppingList> retrieveShoppingListByIdTask =
            _shoppingListService.RetrieveShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            retrieveShoppingListByIdTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
