using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingListItems;

public partial class ShoppingListItemProcessingServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnUpsertIfShoppingListItemIsNullAndLogItAsync()
    {
        // given
        ShoppingListItem nullShoppingListItem = null;

        var nullShoppingListItemProcesingException =
            new NullShoppingListItemProcessingException(
                exceptionMessage: "Shopping list item is null.");

        var expectedShoppingListItemProcessingValidationException =
            new ShoppingListItemProcessingValidationException(
                exceptionMessage: "Shopping list item processing validation error occurred, " +
                "fix the errors and try again please!",
                innerException: nullShoppingListItemProcesingException);

        // when
        ValueTask<ShoppingListItem> upsertAsyncTask =
            _shoppingListItemProcessingService.UpsertShoppingListItemAsync(
                nullShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingValidationException>(
            upsertAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Never());

        _shoppingListItemServiceMock.Verify(service =>
            service.ModifyShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _shoppingListItemServiceMock.Verify(service =>
            service.AddShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemProcessingValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
