using Moq;
using SLO.MobileApp.Core.Models.Foundations.Users.Exceptions;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveLoggedInUserIfServiceErrorOccursAndLogItAsync()
    {
        // given
        string exceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(exceptionMessage);

        var failedUserServiceException =
            new FailedUserServiceException(
                exceptionMessage: "Failed user service error occurred, " +
                "please contact support.",
                innerException: someServiceException);

        var expectedUserServiceException =
            new UserServiceException(
                exceptionMessage: "User service error occurred, " +
                "please contact support.",
                innerException: failedUserServiceException);

        _userManagementBrokerMock.Setup(broker =>
            broker.GetLoggedInUserIdAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<Guid> retrieveLoggedInUserAsyncTask =
            _userService.RetrieveLoggedInUserAsync(
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<UserServiceException>(
            retrieveLoggedInUserAsyncTask.AsTask);

        _userManagementBrokerMock.Verify(broker =>
            broker.GetLoggedInUserIdAsync(
                It.IsAny<CancellationToken>()));

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedUserServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
