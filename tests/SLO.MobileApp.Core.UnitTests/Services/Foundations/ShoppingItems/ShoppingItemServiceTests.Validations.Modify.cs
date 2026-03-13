using Force.DeepCloner;
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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedAtIsSameAsCreatedAtAndLogItAsync()
    {
        // given
        ShoppingItem invalidShoppingItem = CreateRandomShoppingItem();

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.UpdatedAt),
            values: $"Date is same as {nameof(ShoppingItem.CreatedAt)}.");

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

    [Theory]
    [MemberData(nameof(InvalidMoreThanOneMinuteCases))]
    public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedAtIsNotRecentAndLogItAsync(
        int invalidMoreThanOneMinuteCase)
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem invalidShoppingItem =
            CreateRandomShoppingItem(currentDateTime);

        invalidShoppingItem.UpdatedAt =
            invalidShoppingItem.UpdatedAt.AddMinutes(
                invalidMoreThanOneMinuteCase);

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.UpdatedAt),
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

        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                invalidShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            modifyShoppingItemTask.AsTask);

        // then

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfStorageShoppingItemIsNotFoundAndLogItAsync()
    {
        // given
        DateTimeOffset currentDatetime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(currentDatetime);
        ShoppingItem inputShoppingItem = randomShoppingItem;

        inputShoppingItem.UpdatedAt =
            inputShoppingItem.UpdatedAt.AddMinutes(1);

        ShoppingItem notFoundShoppingItem = null;
        ShoppingItem storageShoppingItem = notFoundShoppingItem;
        Guid shoppingItemId = inputShoppingItem.Id;

        var notFoundShoppingItemException =
            new NotFoundShoppingItemException(
                exceptionMessage: $"A shopping item with Id: {shoppingItemId}, " +
                "could not be found.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: notFoundShoppingItemException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDatetime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(notFoundShoppingItem);

        // when

        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            modifyShoppingItemTask.AsTask);

        // then

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Once());

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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfCreatedByIsNotSameAsStorageCreatedByAndLogItAsync()
    {
        // given
        DateTimeOffset currentDatetime = Randomizers.GetRandomDateTime();
        Guid noteSameShoppingItemId = Guid.NewGuid();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(currentDatetime);

        ShoppingItem inputShoppingItem = randomShoppingItem;
        ShoppingItem storageShoppingItem = inputShoppingItem.DeepClone();

        inputShoppingItem.UpdatedAt =
            inputShoppingItem.UpdatedAt.AddMinutes(1);

        storageShoppingItem.CreatedBy = noteSameShoppingItemId;
        Guid shoppingItemId = inputShoppingItem.Id;

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.CreatedBy),
            values: $"Id is not same as {nameof(ShoppingItem.CreatedBy)}.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingItemException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDatetime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItem);

        // when

        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            modifyShoppingItemTask.AsTask);

        // then

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Once());

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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfCreatedAtIsNotSameAsStorageCreatedAtAndLogItAsync()
    {
        // given
        DateTimeOffset currentDatetime = Randomizers.GetRandomDateTime();
        DateTimeOffset notSameDateTime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(currentDatetime);

        ShoppingItem inputShoppingItem = randomShoppingItem;
        ShoppingItem storageShoppingItem = inputShoppingItem.DeepClone();

        inputShoppingItem.UpdatedAt =
            inputShoppingItem.UpdatedAt.AddMinutes(1);

        storageShoppingItem.CreatedAt = notSameDateTime;
        Guid shoppingItemId = inputShoppingItem.Id;

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.CreatedAt),
            values: $"Date is not same as {nameof(ShoppingItem.CreatedAt)}.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingItemException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDatetime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItem);

        // when

        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            modifyShoppingItemTask.AsTask);

        // then

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Once());

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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedAtIsSameAsStorageUpdatedAtAndLogItAsync()
    {
        // given
        DateTimeOffset currentDatetime = Randomizers.GetRandomDateTime();

        ShoppingItem randomShoppingItem =
            CreateRandomShoppingItem(currentDatetime);

        ShoppingItem inputShoppingItem = randomShoppingItem;
        ShoppingItem storageShoppingItem = inputShoppingItem;

        inputShoppingItem.UpdatedAt =
            inputShoppingItem.UpdatedAt.AddMinutes(1);

        Guid shoppingItemId = inputShoppingItem.Id;

        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingItemException.AddData(
            key: nameof(ShoppingItem.UpdatedAt),
            values: $"Date is same as {nameof(ShoppingItem.UpdatedAt)}.");

        var expectedShoppingItemValidationException =
            new ShoppingItemValidationException(
                exceptionMessage: "Shopping item validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingItemException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDatetime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingItemByIdAsync(
                shoppingItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingItem);

        // when

        ValueTask<ShoppingItem> modifyShoppingItemTask =
            _shoppingItemService.ModifyShoppingItemAsync(
                inputShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemValidationException>(
            modifyShoppingItemTask.AsTask);

        // then

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingItemByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Once());

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
