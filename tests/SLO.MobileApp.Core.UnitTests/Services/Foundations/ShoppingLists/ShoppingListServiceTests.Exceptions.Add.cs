using EFxceptions.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
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
    public async Task ShouldThrowDependencyValidationExceptionOnAddIfShoppingListAlreadyExistsAndLogItAsync()
    {
        // given
        ShoppingList someShoppingList =
            CreateRandomShoppingList();

        someShoppingList.UpdatedBy =
            someShoppingList.CreatedBy;

        string exceptionMessage = Randomizers.GetRandomString();
        var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

        var alreadyExistsShoppingListException =
            new AlreadyExistsShoppingListException(
                exceptionMessage: $"A shopping list with same Id " +
                $"already exists.",
                innerException: duplicateKeyException);

        var expectedShoppingListDependencyValidationException =
            new ShoppingListDependencyValidationException(
                exceptionMessage: "Shopping list dependency validation error occurred, " +
                "please try again!",
                innerException: alreadyExistsShoppingListException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(duplicateKeyException);

        // when
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                shoppingList: someShoppingList,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListDependencyValidationException>(
            addShoppingListTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
                someShoppingList,
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListDependencyValidationException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
    {
        // given
        ShoppingList someShoppingList =
            CreateRandomShoppingList();

        someShoppingList.UpdatedBy =
            someShoppingList.CreatedBy;

        string exceptionMessage = Randomizers.GetRandomString();
        var dbUpdateException = new DbUpdateException(exceptionMessage);

        var failedShoppingListStorageException =
            new FailedShoppingListStorageException(
                exceptionMessage: "Failed shopping list storage error occurred, " +
                "please contact support.",
                innerException: dbUpdateException);

        var expectedShoppingListDependencyException =
            new ShoppingListDependencyException(
                exceptionMessage: "Shopping list dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingListStorageException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dbUpdateException);

        // when
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                shoppingList: someShoppingList,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListDependencyException>(
            addShoppingListTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
                someShoppingList,
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
    {
        // given
        ShoppingList someShoppingList =
            CreateRandomShoppingList();

        someShoppingList.UpdatedBy =
            someShoppingList.CreatedBy;

        var sqlException = Randomizers.GetSqlException();

        var failedShoppingListStorageException =
            new FailedShoppingListStorageException(
                exceptionMessage: "Failed shopping list storage error occurred, " +
                "please contact support.",
                innerException: sqlException);

        var expectedShoppingListDependencyException =
            new ShoppingListDependencyException(
                exceptionMessage: "Shopping list dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingListStorageException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                shoppingList: someShoppingList,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListDependencyException>(
            addShoppingListTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
                someShoppingList,
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogCriticalAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
    {
        // given
        ShoppingList someShoppingList =
            CreateRandomShoppingList();

        someShoppingList.UpdatedBy =
            someShoppingList.CreatedBy;

        string exceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(exceptionMessage);

        var failedShoppingListServiceException =
            new FailedShoppingListServiceException(
                exceptionMessage: "Failed shopping list service error occurred, " +
                "please contact support.",
                innerException: someServiceException);

        var expectedShoppingListServiceException =
            new ShoppingListServiceException(
                exceptionMessage: "Shopping list service error occurred, " +
                "please contact support.",
                innerException: failedShoppingListServiceException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<ShoppingList> addShoppingListTask =
            _shoppingListService.AddShoppingListAsync(
                shoppingList: someShoppingList,
                cancellationToken: It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<ShoppingListServiceException>(
            addShoppingListTask.AsTask);

        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _storageBrokerMock.Verify(broker =>
            broker.InsertShoppingListAsync(
                someShoppingList,
                It.IsAny<CancellationToken>()),
            Times.Never());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedShoppingListServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
