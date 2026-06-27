using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingListItems;

public partial class ShoppingListItemProcessingServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRetrieveAllByShoppingListIdIfShoppingListIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        Guid shoppingListId = invalidId;

        var invalidShoppingListItemProcessingException =
            new InvalidShoppingListItemProcessingException(
                exceptionMessage: "Invalid shopping list item processing error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListItemProcessingException.AddData(
            key: nameof(shoppingListId),
            values: "Id is required.");

        var expectedShoppingListItemProcessingValidationException =
            new ShoppingListItemProcessingValidationException(
                exceptionMessage: "Shopping list item processing validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListItemProcessingException);

        // when
        ValueTask<IQueryable<ShoppingListItem>> retrieveAllByShoppingListIdAsyncTask =
            _shoppingListItemProcessingService
            .RetrieveAllShoppingListItemsByShoppingListIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingValidationException>(
            retrieveAllByShoppingListIdAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
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
