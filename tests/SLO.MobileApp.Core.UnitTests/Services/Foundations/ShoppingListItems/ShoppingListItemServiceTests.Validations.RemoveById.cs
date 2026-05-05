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
    public async Task ShouldThrowValidationExceptionOnRemoveByIdIfShoppingListItemIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        Guid shoppingListItemId = invalidId;

        var invalidShoppingListItemException =
            new InvalidShoppingListItemException(
                exceptionMessage: "Invalid shopping list item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListItemException.AddData(
            key: nameof(shoppingListItemId),
            values: "Id is required.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListItemException);

        // when
        ValueTask<ShoppingListItem> removeShoppingListItemByIdTask =
            _shoppingListItemService.RemoveShoppingListItemByIdAsync(
                shoppingListItemId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            removeShoppingListItemByIdTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.DeleteShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
