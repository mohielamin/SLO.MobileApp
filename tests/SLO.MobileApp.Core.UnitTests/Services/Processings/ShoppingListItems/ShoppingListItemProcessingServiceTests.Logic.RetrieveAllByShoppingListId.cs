using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingListItems;

public partial class ShoppingListItemProcessingServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllShoppingListItemsByShoppingListItemAsync()
    {
        // given
        Guid shoppingListId = Guid.NewGuid();

        IQueryable<ShoppingListItem> randomShoppingListItems =
            CreateRandomShoppingListItems(shoppingListId);

        IQueryable<ShoppingListItem> retrievedShoppingListItems =
            randomShoppingListItems;

        IQueryable<ShoppingListItem> matchingShoppingListItem =
            GetMatchingShoppingListItems(
                shoppingListId,
                shoppingListItems: randomShoppingListItems);

        IQueryable<ShoppingListItem> expectedShoppingListItems =
            matchingShoppingListItem.DeepClone();

        _shoppingListItemServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(retrievedShoppingListItems);

        // when
        IQueryable<ShoppingListItem> actualShoppingListItems =
            await _shoppingListItemProcessingService
            .RetrieveAllShoppingListItemsByShoppingListIdAsync(
                shoppingListId,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingListItems.Should().BeEquivalentTo(expectedShoppingListItems);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
