using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingLists;

public partial class ShoppingListServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllShoppingListsAsync()
    {
        // given
        IQueryable<ShoppingList> randomShoppingLists =
            CreateRandomShoppingLists();

        IQueryable<ShoppingList> storageShoppingLists =
            randomShoppingLists;

        IQueryable<ShoppingList> expectedShoppingLists =
            storageShoppingLists.DeepClone();

        _storageBrokerMock.Setup(broker =>
            broker.SelectAllShoppingListsAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingLists);

        // when
        IQueryable<ShoppingList> actualShoppingList =
            await _shoppingListService.RetrieveAllShoppingListsAsync(
                It.IsAny<CancellationToken>());

        // then
        actualShoppingList.Should().BeEquivalentTo(
            expectedShoppingLists);

        _storageBrokerMock.Verify(broker =>
            broker.SelectAllShoppingListsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
