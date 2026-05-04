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
    public async Task ShouldThrowValidationExceptionOnModifyIfShoppingListItemIsNullAndLogItAsync()
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
        ValueTask<ShoppingListItem> modifyShoppingListItemTask =
            _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: nullShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            modifyShoppingListItemTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListItemAsync(
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
    public async Task ShouldThrowValidationExceptionOnModifyIfShoppingListItemIsInvalidAndLogItAsync(
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
        ValueTask<ShoppingListItem> modifyShoppingListItemTask =
            _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: invalidShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            modifyShoppingListItemTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListItemAsync(
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
    public async Task ShouldThrowValidationExceptionOnModifyIfUpdateAtSameAsCreateAtAndLogItAsync()
    {
        // given
        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem();

        ShoppingListItem invalidShoppingListItem =
            randomShoppingListItem;

        var invalidShoppingListItemException =
            new InvalidShoppingListItemException(
                exceptionMessage: "Invalid shopping list item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.UpdatedAt),
            values: $"Date is same as {nameof(ShoppingListItem.CreatedAt)}.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListItemException);

        // when
        ValueTask<ShoppingListItem> modifyShoppingListItemTask =
            _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: invalidShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            modifyShoppingListItemTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListItemAsync(
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
    [MemberData(nameof(InvalidMinuteCases))]
    public async Task ShouldThrowValidationExceptionOnModifyIfUpdateAtIsNotRecentAndLogItAsync(
        int invalidMoreThanOneMinuteCase)
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem(
                dateTimes: currentDateTime);

        ShoppingListItem invalidShoppingListItem =
            randomShoppingListItem;

        invalidShoppingListItem.UpdatedAt =
            invalidShoppingListItem.UpdatedAt.AddMinutes(
                minutes: invalidMoreThanOneMinuteCase);

        var invalidShoppingListItemException =
            new InvalidShoppingListItemException(
                exceptionMessage: "Invalid shopping list item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListItemException.AddData(
            key: nameof(ShoppingListItem.UpdatedAt),
            values: "Date is not recent.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListItemException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        // when
        ValueTask<ShoppingListItem> modifyShoppingListItemTask =
            _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: invalidShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            modifyShoppingListItemTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListItemAsync(
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
    public async Task ShouldThrowValidationExceptionOnModifyIfShoppingListItemIsNotFoundAndLogItAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingListItem randomShoppingListItem =
            CreateRandomShoppingListItem(
                dateTimes: currentDateTime);

        ShoppingListItem invalidShoppingListItem =
            randomShoppingListItem;

        invalidShoppingListItem.UpdatedAt =
            invalidShoppingListItem.UpdatedAt.AddMinutes(1);

        Guid shoppingListItemId = invalidShoppingListItem.Id;
        ShoppingListItem nullShoppingListItem = null;
        ShoppingListItem notFoundShoppingListItem = nullShoppingListItem;
        ShoppingListItem storageShoppingListItem = notFoundShoppingListItem;

        var notFoundShoppingListItemException =
            new NotFoundShoppingListItemException(
                exceptionMessage: $"A shopping list item with Id: " +
                $"{shoppingListItemId}, could not be found.");

        var expectedShoppingListItemValidationException =
            new ShoppingListItemValidationException(
                exceptionMessage: "Shopping list item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: notFoundShoppingListItemException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListItemByIdAsync(
                shoppingListItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingListItem);

        // when
        ValueTask<ShoppingListItem> modifyShoppingListItemTask =
            _shoppingListItemService.ModifyShoppingListItemAsync(
                shoppingListItem: invalidShoppingListItem,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListItemValidationException>(
            modifyShoppingListItemTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListItemByIdAsync(
                shoppingListItemId,
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.UpdateShoppingListItemAsync(
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
