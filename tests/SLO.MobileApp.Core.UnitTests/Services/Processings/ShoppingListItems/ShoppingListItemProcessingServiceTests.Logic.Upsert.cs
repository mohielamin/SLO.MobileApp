using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingListItems;

public partial class ShoppingListItemProcessingServiceTests
{
    [Fact]
    public async Task ShouldAddShoppingListItemIfShoppingListItemDoesNotExistsAsync()
    {
        // given
        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem inputShoppingListItem = randomShoppingListItem;
        ShoppingListItem addedShoppingListItem = inputShoppingListItem;
        ShoppingListItem expectedShoppingListItem = addedShoppingListItem.DeepClone();

        IQueryable<ShoppingListItem> randomShoppingListItems =
            CreateRandomShoppingListItems();

        IQueryable<ShoppingListItem> retrievedShoppingListItems =
            randomShoppingListItems;

        _shoppingListItemServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(retrievedShoppingListItems);

        _shoppingListItemServiceMock.Setup(broker =>
            broker.AddShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(addedShoppingListItem);

        // when
        ShoppingListItem actualShoppingListItem =
            await _shoppingListItemProcessingService.UpsertShoppingListItemAsync(
                shoppingListItem: inputShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingListItem.Should().BeEquivalentTo(expectedShoppingListItem);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _shoppingListItemServiceMock.Verify(service =>
            service.AddShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _shoppingListItemServiceMock.Verify(service =>
            service.ModifyShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldModifyShoppingListItemIfShoppingListItemExistsAsync()
    {
        // given
        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem inputShoppingListItem = randomShoppingListItem;
        ShoppingListItem existsShoppingListItem = inputShoppingListItem;
        ShoppingListItem modifiedShoppingListItem = existsShoppingListItem;

        ShoppingListItem expectedShoppingListItem =
            modifiedShoppingListItem.DeepClone();

        IQueryable<ShoppingListItem> randomShoppingListItems =
            CreateRandomShoppingListItems(existsShoppingListItem);

        IQueryable<ShoppingListItem> retrievedShoppingListItems =
            randomShoppingListItems;

        _shoppingListItemServiceMock.Setup(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(retrievedShoppingListItems);

        _shoppingListItemServiceMock.Setup(service =>
            service.ModifyShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(modifiedShoppingListItem);

        // when
        ShoppingListItem actualShoppingListItem =
            await _shoppingListItemProcessingService.UpsertShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        actualShoppingListItem.Should().BeEquivalentTo(existsShoppingListItem);

        _shoppingListItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingListItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _shoppingListItemServiceMock.Verify(service =>
            service.ModifyShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _shoppingListItemServiceMock.Verify(service =>
            service.AddShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        VerifyNoOtherDependencyCalls();
    }
}
