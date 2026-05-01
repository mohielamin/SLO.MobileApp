using FluentAssertions;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Theory]
    [MemberData(nameof(DependencyValidationExceptions))]
    public async Task ShouldRenderInnerExceptionMessageOnAddIfDependencyValidationErrorOccursAsync(
        Exception dependencyValidationException)
    {
        // given
        ShoppingItem someShoppingItem =
            CreateRandomShoppingItem();

        string expectedErrorMessage =
            dependencyValidationException.InnerException.Message;

        _shoppingItemServiceMock.Setup(service =>
            service.AddShoppingItemAsync(
               someShoppingItem,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyValidationException);

        // when
        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(someShoppingItem);

        // then
        _shoppingListViewModel.ErrorMessage.Should().BeEquivalentTo(
            expectedErrorMessage);

        _shoppingItemServiceMock.Verify(service =>
            service.AddShoppingItemAsync(
                someShoppingItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Theory]
    [MemberData(nameof(DependencyExceptions))]
    public async Task ShouldRenderExceptionMessageOnAddIfDependencyErrorOccursAsync(
        Exception dependencyException)
    {
        // given
        ShoppingItem someShoppingItem =
            CreateRandomShoppingItem();

        string expectedErrorMessage =
            dependencyException.Message;

        _shoppingItemServiceMock.Setup(service =>
            service.AddShoppingItemAsync(
               someShoppingItem,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyException);

        // when
        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(someShoppingItem);

        // then
        _shoppingListViewModel.ErrorMessage.Should().BeEquivalentTo(
            expectedErrorMessage);

        _shoppingItemServiceMock.Verify(service =>
            service.AddShoppingItemAsync(
                someShoppingItem,
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
