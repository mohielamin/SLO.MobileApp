using Force.DeepCloner;
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

    [Theory]
    [MemberData(nameof(InvalidMinuteCases))]
    public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedAtIsNotRecentAndLogItAsync(
        int invalidMoreThanOneMinuteCase)
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingList randomShoppingList =
            CreateRandomShoppingList(
                dateTimes: currentDateTime);

        ShoppingList invalidShoppingList = randomShoppingList;

        invalidShoppingList.UpdatedAt =
            invalidShoppingList.UpdatedAt.AddMinutes(
                minutes: invalidMoreThanOneMinuteCase);

        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.UpdatedAt),
            values: "Date is not recent.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        // when
        ValueTask<ShoppingList> modifyShoppingListTask =
            _shoppingListService.ModifyShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            modifyShoppingListTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

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
    public async Task ShouldThrowValidationExceptionOnModifyIfStorageShoppingListIsNotFoundAndLogItAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();
        ShoppingList notFoundShoppingList = null;
        ShoppingList storageShoppingList = notFoundShoppingList;

        ShoppingList randomShoppingList =
            CreateRandomShoppingList(
                dateTimes: currentDateTime);

        ShoppingList invalidShoppingList = randomShoppingList;

        invalidShoppingList.UpdatedAt =
            invalidShoppingList.UpdatedAt.AddMinutes(1);

        var notFoundShoppingListException =
            new NotFoundShoppingListException(
                exceptionMessage: $"A shopping list with Id: {invalidShoppingList.Id}, " +
                $"could not be found.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: notFoundShoppingListException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListByIdAsync(
                invalidShoppingList.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingList);

        // when
        ValueTask<ShoppingList> modifyShoppingListTask =
            _shoppingListService.ModifyShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            modifyShoppingListTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                invalidShoppingList.Id,
                It.IsAny<CancellationToken>()),
            Times.Once());

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
    public async Task ShouldThrowValidationExceptionOnModifyIfInputCreatedByNotSameAsStorageCreatedByAndLogItAsync()
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingList randomShoppingList =
            CreateRandomShoppingList(
                dateTimes: currentDateTime);

        ShoppingList storageShoppingList = randomShoppingList;

        ShoppingList invalidShoppingList =
            randomShoppingList.DeepClone();

        invalidShoppingList.UpdatedAt =
            invalidShoppingList.UpdatedAt.AddMinutes(1);

        Guid randomId = Guid.NewGuid();
        Guid notSameShoppingListId = randomId;
        invalidShoppingList.CreatedBy = notSameShoppingListId;

        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.CreatedBy),
            values: $"Id is not same as {nameof(ShoppingList.CreatedBy)}.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentDateTime);

        _storageBrokerMock.Setup(broker =>
            broker.SelectShoppingListByIdAsync(
                invalidShoppingList.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(storageShoppingList);

        // when
        ValueTask<ShoppingList> modifyShoppingListTask =
            _shoppingListService.ModifyShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            modifyShoppingListTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.SelectShoppingListByIdAsync(
                invalidShoppingList.Id,
                It.IsAny<CancellationToken>()),
            Times.Once());

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
