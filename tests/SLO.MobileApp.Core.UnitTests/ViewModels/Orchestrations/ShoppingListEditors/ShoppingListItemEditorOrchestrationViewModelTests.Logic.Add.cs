using FluentAssertions;
using Force.DeepCloner;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Orchestrations.ShoppingListItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.Orchestrations.ShoppingListEditors;

public partial class ShoppingListItemEditorOrchestrationViewModelTests
{
    [Fact]
    public async Task ShouldAddShoppingListItemAsync()
    {
        // given
        ShoppingListItemMode shoppingListItemMode =
            ShoppingListItemMode.New;

        ShoppingListItem emptyShoppingListItem = new();

        ShoppingListItem inputShoppingListItem =
            emptyShoppingListItem;

        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem addedShoppingListItem =
            randomShoppingListItem;

        ShoppingListItem expectedShoppingListItem =
            addedShoppingListItem.DeepClone();

        ShoppingListItem actualShoppingListItem = null;

        _shoppingListItemServiceMock.Setup(service =>
            service.AddShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(addedShoppingListItem);

        Func<ShoppingListItem, ValueTask> callback =
            async (passedInShoppingListItem) =>
            {
                actualShoppingListItem = passedInShoppingListItem;
            };

        // when
        _shoppingListItemEditorOrchestrationViewModel
            .ShoppingListItem = inputShoppingListItem;

        _shoppingListItemEditorOrchestrationViewModel
            .ShoppingListItemMode = shoppingListItemMode;

        _shoppingListItemEditorOrchestrationViewModel
            .Callback = callback;

        await _shoppingListItemEditorOrchestrationViewModel
                .SaveCommand.ExecuteAsync(parameter: null);

        // then
        actualShoppingListItem.Should()
            .BeEquivalentTo(expectedShoppingListItem);

        _shoppingListItemServiceMock.Verify(service =>
            service.AddShoppingListItemAsync(
                inputShoppingListItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _navigationBrokerMock.Verify(broker =>
            broker.PopAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
