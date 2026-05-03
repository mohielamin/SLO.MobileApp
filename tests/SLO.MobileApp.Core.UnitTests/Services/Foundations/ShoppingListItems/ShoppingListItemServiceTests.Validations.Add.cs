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
    public async Task ShouldThrowValidationExceptionOnAddIfShoppingListItemIsNullAndLogItAsync()
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
        ValueTask<ShoppingListItem> addShoppingListItemTask =
            _shoppingListItemService.AddShoppingListItemAsync(
                nullShoppingListItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            addShoppingListItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
