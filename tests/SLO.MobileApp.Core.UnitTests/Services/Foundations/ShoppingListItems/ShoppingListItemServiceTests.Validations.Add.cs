using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfShoppingListItemIsNullAndLogItAsync()
    {
        // given
        ShoppingListItem nullShoppingListItem = null;

        var nullShoppingListItemException =
            new NullShoppingListItemException(
                exceptionMessage: "Shopping list item is null.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: nullShoppingListItemException);

        // when
        ValueTask<ShoppingListItem> addShoppingListItemTask =
            _shoppingListItemService.AddShoppingListItemAsync(
                nullShoppingListItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            addShoppingListItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldThrowValidationExceptionOnAddIfShoppingListItemIsInvalidAndLogItAsync(
        string invalidString)
    {
        // given
        ShoppingListItem invalidShoppingListItem =
            new ShoppingListItem
            {
                Id = default,
                ShoppingListId = default,
                Name = invalidString,
                CreatedBy = default,
                UpdatedBy = default,
                CreatedAt = default,
                UpdatedAt = default,
            };

        var invalidShoppingListItemException =
            new InvalidShoppingListItemException(
                exceptionMessage: "Invalid shopping list item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.Id),
            values: "Id is required.");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.ShoppingListId),
            values: "Id is required.");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.Name),
            values: "Text is required.");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.CreatedBy),
            values: "Id is required.");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.UpdatedBy),
            values: "Id is required.");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.CreatedAt),
            values: "Date is required.");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.UpdatedAt),
            values: "Date is required.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListItemException);

        // when
        ValueTask<ShoppingListItem> addShoppingListItemTask =
            _shoppingListItemService.AddShoppingListItemAsync(
                invalidShoppingListItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            addShoppingListItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfUpdatedByNotSameAsCreatedByAndLogItAsync()
    {
        // given
        Guid notSameId = Guid.NewGuid();

        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem invalidShoppingListItem =
            randomShoppingListItem;

        invalidShoppingListItem.UpdatedBy = notSameId;

        var invalidShoppingListItemException =
            new InvalidShoppingListItemException(
                exceptionMessage: "Invalid shopping list item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.UpdatedBy),
            values: $"Id is not same as {nameof(ShoppingListItem.CreatedBy)}.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListItemException);

        // when
        ValueTask<ShoppingListItem> addShoppingListItemTask =
            _shoppingListItemService.AddShoppingListItemAsync(
                invalidShoppingListItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            addShoppingListItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfUpdatedAtNotSameAsCreatedAtAndLogItAsync()
    {
        // given
        DateTimeOffset notSameDateTime = Randomizers.GetRandomDateTime();

        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem invalidShoppingListItem =
            randomShoppingListItem;

        invalidShoppingListItem.UpdatedBy =
            invalidShoppingListItem.CreatedBy;

        invalidShoppingListItem.UpdatedAt = notSameDateTime;

        var invalidShoppingListItemException =
            new InvalidShoppingListItemException(
                exceptionMessage: "Invalid shopping list item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.UpdatedAt),
            values: $"Date is not same as {nameof(ShoppingListItem.CreatedAt)}.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListItemException);

        // when
        ValueTask<ShoppingListItem> addShoppingListItemTask =
            _shoppingListItemService.AddShoppingListItemAsync(
                invalidShoppingListItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            addShoppingListItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListItemAsync(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListItemValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
