using Moq;
using SLO.MobileApp.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Models.Foundations.ShoppingItems.Exceptions;
using SLO.MobileApp.UnitTests.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfShoppingItemIsNullAndLogItAsync()
    {
        // given
        ShoppingItem nullShoppingItem = null;

        var nullShoppingItemException =
            new NullShoppingItemException(
                exceptionMessage: "Shopping item is null.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the error and try again please!",
                innerException: nullShoppingItemException);

        // when
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                nullShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            addShoppingItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException)),
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
