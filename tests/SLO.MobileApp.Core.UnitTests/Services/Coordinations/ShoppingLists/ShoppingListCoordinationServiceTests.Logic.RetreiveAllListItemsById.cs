using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Coordinations.ShoppingLists;

public partial class ShoppingListCoordinationServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllShoppingListItemsByIdAsync()
    {
        // given
        Guid someShoppingListId = Guid.NewGuid();
        IQueryable<ShoppingListItem> randomShoppingListItems =
            CreateRandomShoppingListItems(shoppingListId: someShoppingListId);

        IQueryable<ShoppingListItem> retrieveShoppingListItems =
            randomShoppingListItems;

        IQueryable<ShoppingListItem> expectedShoppingListItems =
            retrieveShoppingListItems.DeepClone();

        _shoppingListItemProcessingServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsByShoppingListIdAsync(
                someShoppingListId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(retrieveShoppingListItems);

        // when
        IQueryable<ShoppingListItem> actualShoppingListItems =
            await _shoppingListCoordinationService.RetrieveAllShoppingListItemsByIdAsync(
                shoppingListId: someShoppingListId,
                cancellationToken: It.IsAny<CancellationToken>());
        // then
        actualShoppingListItems.Should().BeEquivalentTo(
            expectedShoppingListItems);

        _shoppingListItemProcessingServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsByShoppingListIdAsync(
                someShoppingListId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNotOtherDependencyCalls();
    }
}
