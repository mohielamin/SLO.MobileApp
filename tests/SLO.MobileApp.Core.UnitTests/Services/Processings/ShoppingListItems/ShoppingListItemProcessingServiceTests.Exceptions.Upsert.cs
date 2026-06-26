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
    [Theory]
    [MemberData(nameof(DependencyValidationExceptions))]
    public async Task ShouldThrowDependencyValidationExceptionOnUpsertIfDependencyValidationErrorOccursAndLogItAsync(
        Exception dependencyValidationException)
    {
        // given
        ShoppingListItem someShoppingListItem = CreateRandomShoppingListItem();

        var expectedShoppingListItemProcessingDependencyValidationException =
            new ShoppingListItemProcessingDependencyValidationException(
                exceptionMessage: "Shopping list item processing dependency validation error occurred, " +
                "please try again!",
                innerException: dependencyValidationException.InnerException);

        _shoppingListItemServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyValidationException);

        // when
        ValueTask<ShoppingListItem> upsertAsyncTask =
            _shoppingListItemProcessingService.UpsertShoppingListItemAsync(
                shoppingListItem: someShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingDependencyValidationException>(
            upsertAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

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
                    expectedShoppingListItemProcessingDependencyValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Theory]
    [MemberData(nameof(DependencyExceptions))]
    public async Task ShouldThrowDependencyExceptionOnUpsertIfDependencyErrorOccursAndLogItAsync(
        Exception dependencyException)
    {
        // given
        ShoppingListItem someShoppingListItem = CreateRandomShoppingListItem();

        var expectedShoppingListItemProcessingDependencyException =
            new ShoppingListItemProcessingDependencyException(
                exceptionMessage: "Shopping list item processing dependency error occurred, " +
                "please contact support.",
                innerException: dependencyException);

        _shoppingListItemServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyException);

        // when
        ValueTask<ShoppingListItem> upsertAsyncTask =
            _shoppingListItemProcessingService.UpsertShoppingListItemAsync(
                shoppingListItem: someShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingDependencyException>(
            upsertAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

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
                    expectedShoppingListItemProcessingDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnUpsertIfServiceErrorOccursAndLogItAsync()
    {
        // given
        ShoppingListItem someShoppingListItem = CreateRandomShoppingListItem();
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
        ValueTask<ShoppingListItem> upsertAsyncTask =
            _shoppingListItemProcessingService.UpsertShoppingListItemAsync(
                shoppingListItem: someShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingServiceException>(
            upsertAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

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
                    expectedShoppingListItemProcessingServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
