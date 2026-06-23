using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.Users;

public partial class UserProcessingServiceTests
{
    [Fact]
    public async Task ShouldRetrieveLoggedInUserAsync()
    {
        // given
        Guid randomId = Guid.NewGuid();
        Guid retrievedLoggedInUser = randomId;
        Guid expectedLoggedInUser = retrievedLoggedInUser;

        _userServiceMock.Setup(broker =>
            broker.RetrieveLoggedInUserAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(retrievedLoggedInUser);

        // when
        Guid actualLoggedInUser =
            await _userProcessingService.RetrieveLoggedInUserAsync(
                It.IsAny<CancellationToken>());

        // then
        actualLoggedInUser.Should().Be(expectedLoggedInUser);

        _userServiceMock.Verify(broker =>
            broker.RetrieveLoggedInUserAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
