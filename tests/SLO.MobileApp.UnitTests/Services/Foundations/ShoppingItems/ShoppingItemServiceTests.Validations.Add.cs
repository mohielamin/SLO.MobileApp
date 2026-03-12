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
    public async Task ShouldThrowValidationExceptionOnAddIfShoppingItemIsNullAndLogItAsync()
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
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                nullShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            addShoppingItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException)),
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldThrowValidationExceptionOnAddIfShoppingItemIsInvalidAndLogItAsync(
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
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                invalidShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            addShoppingItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException)),
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfUpdatedByNotSameAsCreatedByAndLogItAsync()
    {
        // given
        ShoppingItem randomShoppingItem = CreateRandomShoppingItem();
        ShoppingItem invalidShoppingItem = randomShoppingItem;
        Guid notSameId = Guid.NewGuid();
        invalidShoppingItem.UpdatedBy = notSameId;

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.UpdatedBy),
            values: $"Id is not same as {nameof(ShoppingItem.CreatedBy)}.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingItemException);

        // when
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                invalidShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            addShoppingItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException)),
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfUpdatedAtNotSameAsCreatedAtAndLogItAsync()
    {
        // given
        ShoppingItem randomShoppingItem = CreateRandomShoppingItem();
        ShoppingItem invalidShoppingItem = randomShoppingItem;

        invalidShoppingItem.UpdatedBy =
            invalidShoppingItem.CreatedBy;

        DateTimeOffset notSameDateTime = Randomizers.GetRandomDateTime();
        invalidShoppingItem.UpdatedAt = notSameDateTime;

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.UpdatedAt),
            values: $"Date is not same as {nameof(ShoppingItem.CreatedAt)}.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingItemException);

        // when
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                invalidShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            addShoppingItemTask.AsTask);

        // then
        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException)),
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Theory]
    [MemberData(nameof(InvalidMoreThanOneMinuteCases))]
    public async Task ShouldThrowValidationExceptionOnAddIfCreatedAtIsNotRecentAndLogItAsync(
        int invalidMoreThanOneMinuteCase)
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(dateTimes: currentDateTime);

        ShoppingItem invalidShoppingItem = randomShoppingItem;

        invalidShoppingItem.UpdatedBy =
            invalidShoppingItem.CreatedBy;

        invalidShoppingItem.CreatedAt =
            invalidShoppingItem.UpdatedAt =
                invalidShoppingItem.CreatedAt
                .AddMinutes(invalidMoreThanOneMinuteCase);

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.CreatedAt),
            values: "Date is not recent.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingItemException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        // when
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                invalidShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            addShoppingItemTask.AsTask);

        // then
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingItemAsync(
                It.IsAny<ShoppingItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingItemValidationException)),
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
