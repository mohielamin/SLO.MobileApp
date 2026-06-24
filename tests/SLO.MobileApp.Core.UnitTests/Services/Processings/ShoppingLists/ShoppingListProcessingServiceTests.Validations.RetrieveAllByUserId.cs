using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Processings.ShoppingLists.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingLists;

public partial class ShoppingListProcessingServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRetrieveAllByUserIdIfUserIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        Guid userId = invalidId;

        var invalidShoppingListProcessingException =
            new InvalidShoppingListProcessingException(
                exceptionMessage: "Invalid shopping list processing error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListProcessingException.AddData(
            key: nameof(userId),
            values: "Id is required.");

        var expectedShoppingListProcessingException =
            new ShoppingListProcessingValidationException(
                exceptionMessage: "Shopping list processing validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListProcessingException);

        // when
        ValueTask<IReadOnlyList<ShoppingList>> retrieveAllByUserIdAsyncTask =
            _shoppingListProcessingService.RetrieveAllShoppingListsByUserIdAsync(
                userId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListProcessingValidationException>(
            retrieveAllByUserIdAsyncTask.AsTask);

        _shoppingListServiceMock.Verify(service =>
            service.RetrieveAllShoppingListsAsync(
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListProcessingException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
