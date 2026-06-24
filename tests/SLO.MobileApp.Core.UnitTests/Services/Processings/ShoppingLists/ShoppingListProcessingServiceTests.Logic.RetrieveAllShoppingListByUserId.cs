using FluentAssertions;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingLists;

public partial class ShoppingListProcessingServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllShoppingListsByUserIdAsync()
    {
        // given
        Guid randomId = Guid.NewGuid();
        Guid userId = randomId;

        IQueryable<ShoppingList> randomShoppingLists =
            CreateRandomShoppingLists(createdBy: userId);

        IQueryable<ShoppingList> userShoppingLists =
            randomShoppingLists;

        IQueryable<ShoppingList> retrievedShoppingLists =
            CreateRandomShoppingLists(
                existingShoppingLists: userShoppingLists);

        IReadOnlyList<ShoppingList> expectedShoppingLists =
            userShoppingLists.ToList();

        _shoppingListServiceMock.Setup(service =>
         service.RetrieveAllShoppingListsAsync(
             It.IsAny<CancellationToken>()))
             .ReturnsAsync(retrievedShoppingLists);

        // when
        IReadOnlyList<ShoppingList> actualShoppingLists =
            await _shoppingListProcessingService
            .RetrieveAllShoppingListsByUserIdAsync(
                userId,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingLists.Should().BeEquivalentTo(expectedShoppingLists);

        _shoppingListServiceMock.Verify(service =>
            service.RetrieveAllShoppingListsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
