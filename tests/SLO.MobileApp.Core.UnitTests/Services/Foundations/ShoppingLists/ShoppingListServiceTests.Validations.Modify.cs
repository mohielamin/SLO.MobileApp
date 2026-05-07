using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingLists;

public partial class ShoppingListServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfShoppingListIsNullAndLogItAsync()
    {
        // given
        ShoppingList nullShoppingList = null;

        var nullShoppingListException =
            new NullShoppingListException(
                exceptionMessage: "Shopping list is null.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: nullShoppingListException);

        // when
        ValueTask<ShoppingList> modifyShoppingListTask =
            _shoppingListService.ModifyShoppingListAsync(
                nullShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            modifyShoppingListTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListAsync(
                It.IsAny<ShoppingList>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldThrowValidationExceptionOnModifyIfShoppingListIsInvalidAndLogItAsync(
        string invalidString)
    {
        // given
        ShoppingList invalidShoppingList =
            new ShoppingList
            {
                Id = default,
                Name = invalidString,
                CreatedBy = default,
                UpdatedBy = default,
                CreatedAt = default,
                UpdatedAt = default,
            };

        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.Id),
            values: "Id is required.");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.Name),
            values: "Text is required.");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.CreatedBy),
            values: "Id is required.");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.UpdatedBy),
            values: "Id is required.");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.CreatedAt),
            values: "Date is required.");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.UpdatedAt),
            values: "Date is required.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListException);

        // when
        ValueTask<ShoppingList> modifyShoppingListTask =
            _shoppingListService.ModifyShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            modifyShoppingListTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListAsync(
                It.IsAny<ShoppingList>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedAtIsSameAsCreatedAtAndLogItAsync()
    {
        // given
        ShoppingList randomShoppingList = CreateRandomShoppingList();
        ShoppingList invalidShoppingList = randomShoppingList;

        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.UpdatedAt),
            values: $"Date is same as {nameof(ShoppingList.CreatedAt)}.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListException);

        // when
        ValueTask<ShoppingList> modifyShoppingListTask =
            _shoppingListService.ModifyShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            modifyShoppingListTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListAsync(
                It.IsAny<ShoppingList>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
