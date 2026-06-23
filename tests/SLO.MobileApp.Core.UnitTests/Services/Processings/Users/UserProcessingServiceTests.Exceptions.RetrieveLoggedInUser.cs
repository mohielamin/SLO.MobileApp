using Moq;
using SLO.MobileApp.Core.Models.Processings.Users;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.Users;

public partial class UserProcessingServiceTests
{
    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveLoggedInUserIfServiceErrorOccursAndLogItAsync()
    {
        // given
        string exceptionMessage = Randomizers.GetRandomString();
        var someServiceException = new Exception(exceptionMessage);

        var failedUserProcessingServiceException =
            new FailedUserProcessingServiceException(
                exceptionMessage: "Failed user processing service error occurred, " +
                "please contact support.",
                innerException: someServiceException);

        var expectedUserProcessingServiceException =
            new UserProcessingServiceException(
                exceptionMessage: "User processing service error occurred, " +
                "please contact support.",
                innerException: failedUserProcessingServiceException);

        _userServiceMock.Setup(broker =>
            broker.RetrieveLoggedInUserAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(someServiceException);

        // when
        ValueTask<Guid> retrieveLoggedInUserAsynTask =
            _userProcessingService.RetrieveLoggedInUserAsync(
                It.IsAny<CancellationToken>());

        // then
        await Assert.ThrowsAsync<UserProcessingServiceException>(
            retrieveLoggedInUserAsynTask.AsTask);

        _userServiceMock.Verify(broker =>
            broker.RetrieveLoggedInUserAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        _loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(
                It.Is(Randomizers.SameExceptionAs(
                    expectedUserProcessingServiceException))),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
