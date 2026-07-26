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
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRetrieveAllShoppingListItemsByIdIfShoppingListItemIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        Guid shoppingListId = invalidId;

        var invalidShoppingListCoordinationException =
            new InvalidShoppingListCoordinationException(
                exceptionMessage: "Invalid shopping list coordination error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListCoordinationException.AddData(
            key: nameof(shoppingListId),
            values: "Id is required.");

        var expectedShoppingListCoordinationValidationException =
            new ShoppingListCoordinationValidationException(
                exceptionMessage: "Shopping list coordination validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListCoordinationException);

        // when
        ValueTask<IQueryable<ShoppingListItem>> retrieveAllShoppingListItemsByIdTask =
            _shoppingListCoordinationService.RetrieveAllShoppingListItemsByIdAsync(
                shoppingListId: invalidId,
                It.IsAny<CancellationToken>());

        // then
        _shoppingListItemProcessingServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsByShoppingListIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListCoordinationValidationException))),
            Times.Once());

        VerifyNotOtherDependencyCalls();
    }
}
