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
    public async Task ShouldThrowValidationExceptionOnAddIfShoppingListIsNullAndLogItAsync()
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
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                nullShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            addShoppingListTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
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
    public async Task ShouldThrowValidationExceptionOnAddIfShoppingListIsInvalidAndLogItAsync(
        string invalidString)
    {
        // given
        ShoppingList invalidShoppingList =
            new ShoppingList
            {
                Id = default,
                Name = invalidString
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
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            addShoppingListTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
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
    public async Task ShouldThrowValidationExceptionOnAddIfUpdateByNotSameAsCreatedByAndLogItAsync()
    {
        // given
        ShoppingList randomShoppingList = CreateRandomShoppingList();
        Guid notSameId = Guid.NewGuid();
        ShoppingList invalidShoppingList = randomShoppingList;
        invalidShoppingList.UpdatedBy = notSameId;

        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.UpdatedBy),
            values: $"Id is not same as {nameof(ShoppingList.CreatedBy)}.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListException);

        // when
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            addShoppingListTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
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
    public async Task ShouldThrowValidationExceptionOnAddIfUpdateAtNotSameAsCreatedAtAndLogItAsync()
    {
        // given
        ShoppingList randomShoppingList = CreateRandomShoppingList();

        randomShoppingList.UpdatedBy =
            randomShoppingList.CreatedBy;

        DateTimeOffset notSameDateTime = Randomizers.GetRandomDateTime();
        ShoppingList invalidShoppingList = randomShoppingList;
        invalidShoppingList.UpdatedAt = notSameDateTime;

        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.UpdatedAt),
            values: $"Date is not same as {nameof(ShoppingList.CreatedAt)}.");

        var expectedShoppingListValidationException =
            new ShoppingListValidationException(
                exceptionMessage: "Shopping list validation error occurred, " +
                "fix the errors and try again please!",
                innerException: invalidShoppingListException);

        // when
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            addShoppingListTask.AsTask);

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
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
    public async Task ShouldThrowValidationExceptionOnAddIfCreatedAtIsNotRecentAndLogItAsync(
        int invalidMoreThenOneMinuteCase)
    {
        // given
        DateTimeOffset currentDateTime = Randomizers.GetRandomDateTime();

        ShoppingList randomShoppingList =
            CreateRandomShoppingList(
                dateTimes: currentDateTime);

        randomShoppingList.UpdatedBy =
            randomShoppingList.CreatedBy;

        ShoppingList invalidShoppingList = randomShoppingList;

        invalidShoppingList.CreatedAt =
            invalidShoppingList.CreatedAt.AddMinutes(
                minutes: invalidMoreThenOneMinuteCase);

        invalidShoppingList.UpdatedAt =
            invalidShoppingList.CreatedAt;

        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        invalidShoppingListException.AddData(
            key: nameof(ShoppingList.CreatedAt),
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
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                invalidShoppingList,
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListValidationException>(
            addShoppingListTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
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
