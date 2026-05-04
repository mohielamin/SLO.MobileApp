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
    public async Task ShouldThrowValidationExceptionOnModifyIfShoppingListItemIsNullAndLogItAsync()
    {
        // given
        ShoppingListItem nullShoppingListItem = null;

        var nullShoppingListItemException =
            new NullShoppingListItemException(
                exceptionMessage: "Shopping list item is null.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: nullShoppingListItemException);

        // when
        ValueTask<ShoppingListItem> modifyShoppingListItemTask =
            _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: nullShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            modifyShoppingListItemTask.AsTask);

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
                    expectedShoppingListItemValidationException))),
                Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
