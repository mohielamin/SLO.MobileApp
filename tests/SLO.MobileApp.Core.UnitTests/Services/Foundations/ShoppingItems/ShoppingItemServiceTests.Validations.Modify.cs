using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfShoppingItemIsNullAndLogItAsync()
    {
        // given
        ShoppingItem nullShoppingItem = null;

        var nullShoppingItemException =
            new NullShoppingItemException(
                exceptionMessage: "Shopping item is null.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: nullShoppingItemException);

        // when
        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                nullShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            modifyShoppingItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldThrowValidationExceptionOnModifyIfShoppingItemIsInvalidAndLogItAsync(
        string invalidString)
    {
        // given
        ShoppingItem invalidShoppingItem =
            new ShoppingItem
            {
                Name = invalidString,
            };

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.Id),
            values: "Id is required.");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.CreatedBy),
            values: "Id is required.");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.UpdatedBy),
            values: "Id is required.");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.Name),
            values: "Text is required.");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.CreatedAt),
            values: "Date is required.");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.UpdatedAt),
            values: "Date is required.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingItemException);

        // when
        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                invalidShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            modifyShoppingItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
