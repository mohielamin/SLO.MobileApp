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
    public async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfDependencyValidationErrorOccursAndLogItAsync(
        Exception dependencyValidationException)
    {
        // given
        Guid someShoppingListItemId = Guid.NewGuid();

        var exepctedShoppingListItemProcessingDependencyValidationException =
            new ShoppingListItemProcessingDependencyValidationException(
                exceptionMessage: "Shopping list item processing dependency validation error occurred, " +
                "please try again!",
                innerException: dependencyValidationException.InnerException);

        _shoppingListItemServiceMock.Setup(service =>
            service.RemoveShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyValidationException);

        // when
        ValueTask<ShoppingListItem> removeByIdAsyncTask =
            _shoppingListItemProcessingService.RemoveShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingDependencyValidationException>(
            removeByIdAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RemoveShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    exepctedShoppingListItemProcessingDependencyValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Theory]
    [MemberData(nameof(DependencyExceptions))]
    public async Task ShouldThrowDependencyExceptionOnRemoveByIdIfDependencyErrorOccursAndLogItAsync(
        Exception dependencyException)
    {
        // given
        Guid someShoppingListItemId = Guid.NewGuid();

        var exepctedShoppingListItemProcessingDependencyException =
            new ShoppingListItemProcessingDependencyException(
                exceptionMessage: "Shopping list item processing dependency error occurred, " +
                "please contact support.",
                innerException: dependencyException);

        _shoppingListItemServiceMock.Setup(service =>
            service.RemoveShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyException);

        // when
        ValueTask<ShoppingListItem> removeByIdAsyncTask =
            _shoppingListItemProcessingService.RemoveShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemProcessingDependencyException>(
            removeByIdAsyncTask.AsTask);

        _shoppingListItemServiceMock.Verify(service =>
            service.RemoveShoppingListItemByIdAsync(
                someShoppingListItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    exepctedShoppingListItemProcessingDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
