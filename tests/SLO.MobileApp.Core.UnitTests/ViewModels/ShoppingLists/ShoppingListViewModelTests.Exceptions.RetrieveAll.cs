using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    [Theory]
    [MemberData(nameof(DependencyExceptions))]
    public async Task ShouldRenderExceptionMessageOnRetrieveAllIfDependencyErrorOccursAsync(
        Exception dependencyException)
    {
        // given
        string expectedExceptionMessage = dependencyException.Message;

        _shoppingItemServiceMock.Setup(service =>
            service.RetrieveAllShoppingItemsAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyException);

        // when
        await _shoppingListViewModel.RetrieveAllShoppingItemsCommand
            .ExecuteAsync(parameter: null);

        // then
        _shoppingListViewModel.ErrorMessage = expectedExceptionMessage;

        _shoppingItemServiceMock.Verify(service =>
            service.RetrieveAllShoppingItemsAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
