using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System;
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
            CreateRandomShoppingLists(userId);

        IQueryable<ShoppingList> userShoppingLists =
            randomShoppingLists;

        IQueryable<ShoppingList> retrievedShoppingLists =
            CreateRandomShoppingLists(
                existingShoppingLists: userShoppingLists);

        IQueryable<ShoppingList> expectedShoppingLists =
            retrievedShoppingLists.DeepClone();

        _shoppingListServiceMock.Setup(service =>
         service.RetrieveAllShoppingListsAsync(
             It.IsAny<CancellationToken>()))
             .ReturnsAsync(retrievedShoppingLists);

        // when
        IQueryable<ShoppingList> actualShoppingLists =
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
