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
    [Theory]
    [MemberData(nameof(DependencyExceptions))]
    public async Task ShouldThrowDependencyExceptionOnRetrieveAllByShoppingListIdIfDependencyErrorOccursAndLogItAsync(
        Exception depedencyException)
    {
        // given
        Guid someShoppingListId = Guid.NewGuid();

        var expectedShoppingListItemProcessingDependencyException =
            new ShoppingListItemProcessingDependencyException(
                exceptionMessage: "Shopping list item processing dependency error occurred, " +
                "please contact support.",
                innerException: depedencyException);

        _shoppingListItemServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(depedencyException);

        // when
        ValueTask<IQueryable<ShoppingListItem>> retrieveAllByShoppingListIdAsyncTask =
            _shoppingListItemProcessingService
            .RetrieveAllShoppingListItemsByShoppingListIdAsync(
                shoppingListId: someShoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingDependencyException>(
            retrieveAllByShoppingListIdAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemProcessingDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveAllByShoppingListIdIfServiceErrorOccursAndLogItAsync()
    {
        // given
        Guid someShoppingListId = Guid.NewGuid();
        string exceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(exceptionMessage);

        var failedShoppingListItemProcessingServiceException =
            new FailedShoppingListItemProcessingServiceException(
                exceptionMessage: "Failed shopping list item processing service error occurred, " +
                "please contact support.",
                innerException: someServiceException);

        var expectedShoppingListItemProcessingServiceException =
            new ShoppingListItemProcessingServiceException(
                exceptionMessage: "Shopping list item processing service error occurred, " +
                "please contact support.",
                innerException: failedShoppingListItemProcessingServiceException);

        _shoppingListItemServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<IQueryable<ShoppingListItem>> retrieveAllByShoppingListIdAsyncTask =
            _shoppingListItemProcessingService
            .RetrieveAllShoppingListItemsByShoppingListIdAsync(
                shoppingListId: someShoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingServiceException>(
            retrieveAllByShoppingListIdAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemProcessingServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
