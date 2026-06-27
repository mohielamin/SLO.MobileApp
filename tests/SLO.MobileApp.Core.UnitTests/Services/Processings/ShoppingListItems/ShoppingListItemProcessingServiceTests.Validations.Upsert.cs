using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
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

        var nullShoppingListItemProcessingException =
            new NullShoppingListItemProcessingException(
                exceptionMessage: "Shopping list item is null.");

        var expectedShoppingListItemProcessingValidationException =
            new ShoppingListItemProcessingValidationException(
                exceptionMessage: "Shopping list item processing validation error occurred, " +
                "fix the errors and try again please!",
                innerException: nullShoppingListItemProcessingException);

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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnUpsertIfShoppingListItemIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;

        var invalidShoppingListItem =
            new ShoppingListItem
            {
                Id = invalidId,
            };

        var invalidShoppingListItemProcessingException =
            new InvalidShoppingListItemProcessingException(
                exceptionMessage: "Invalid shopping list item processing error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListItemProcessingException.AddData(
            key: nameof(ShoppingListItem.Id),
            values: "Id is required.");

        var expectedShoppingListItemProcessingValidationException =
            new ShoppingListItemProcessingValidationException(
                exceptionMessage: "Shopping list item processing validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListItemProcessingException);

        // when
        ValueTask<ShoppingListItem> upsertAsyncTask =
            _shoppingListItemProcessingService.UpsertShoppingListItemAsync(
                invalidShoppingListItem,
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
