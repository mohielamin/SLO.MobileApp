using Moq;
using SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Coordinations.ShoppingLists;

public partial class ShoppingListCoordinationServiceTests
{
    [Theory]
    [MemberData(nameof(DependencyValidationExceptions))]
    public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllShoppingListItemsByIdIfDepenencyValidationErrorOccursAndLogItAsync(
        Exception dependencyValidationException)
    {
        // given
        Guid someShoppingListId = Guid.NewGuid();

        var expectedShoppingListCoordinationDependencyValidationException =
            new ShoppingListCoordinationDependencyValidationException(
                exceptionMessage: "Shopping list coordination dependency validation error occurred, " +
                "please try again!",
                innerException: dependencyValidationException.InnerException);

        _shoppingListItemProcessingServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsByShoppingListIdAsync(
                someShoppingListId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyValidationException);

        // when
        ValueTask<IQueryable<ShoppingListItem>> retrieveAllShoppingListItemsByIdTask =
            _shoppingListCoordinationService.RetrieveAllShoppingListItemsByIdAsync(
                shoppingListId: someShoppingListId,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListCoordinationDependencyValidationException>(
            retrieveAllShoppingListItemsByIdTask.AsTask);

        _shoppingListItemProcessingServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsByShoppingListIdAsync(
                someShoppingListId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListCoordinationDependencyValidationException))),
            Times.Once());

        VerifyNotOtherDependencyCalls();
    }
}
