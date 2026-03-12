using Microsoft.Data.SqlClient;
using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
    {
        // given
        ShoppingItem someShoppingItem = CreateRandomShoppingItem();

        someShoppingItem.UpdatedBy =
            someShoppingItem.CreatedBy;

        SqlException sqlException = Randomizers.GetSqlException();

        var failedShoppingItemStorageException =
            new FailedShoppingItemStorageException(
                exceptionMessage: "Failed shopping item storage error occurred, " +
                "please contact support.",
                innerException: sqlException);

        var expectedShoppingItemDependencyException =
            new ShoppingItemDependencyException(
                exceptionMessage: "Shopping item dependency error occurred, " +
                "please contact support.",
                innerException: failedShoppingItemStorageException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<ShoppingItem> addShoppingItemTask =
            _shoppingItemService.AddShoppingItemAsync(
                someShoppingItem,
                It.IsAny<CancellationToken>());

        await Assert.ThrowsAsync<ShoppingItemDependencyException>(
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
                    expectedShoppingItemDependencyException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
