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
    public async Task ShouldThrowServiceExceptionOnRetrieveAllByUserIdIfServiceErrorOccursAndLogItAsync()
    {
        // given
        Guid someUserId = Guid.NewGuid();
        string exceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(exceptionMessage);

        var failedShoppingListProcessingServiceException =
            new FailedShoppingListProcessingServiceException(
                exceptionMessage: "Failed shopping list processing service error occurred, " +
                "please contact support.",
                innerException: someServiceException);

        var expectedShoppingListProcessingServiceException =
            new ShoppingListProcessingServiceException(
                exceptionMessage: "Shopping list processing service error occurred, " +
                "please contact support.",
                innerException: failedShoppingListProcessingServiceException);

        _shoppingListServiceMock.Setup(broker =>
            broker.RetrieveAllShoppingListsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<IReadOnlyList<ShoppingList>> retrieveAllByUserIdAsyncTask =
            _shoppingListProcessingService.RetrieveAllShoppingListsByUserIdAsync(
                userId: someUserId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListProcessingServiceException>(
            retrieveAllByUserIdAsyncTask.AsTask);

        _shoppingListServiceMock.Verify(broker =>
            broker.RetrieveAllShoppingListsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListProcessingServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
