using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldRetrieveLoggedInUserAsync()
    {
        // given
        Guid userId = Guid.NewGuid();
        Guid retrievedLoggedInUser = userId;

        Guid expectedLoggedInUser =
            retrievedLoggedInUser.DeepClone();

        _userManagementBrokerMock.Setup(broker =>
            broker.GetLoggedInUserIdAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(retrievedLoggedInUser);

        // when
        Guid actualLoggedInUser =
            await _userService.RetrieveLoggedInUserAsync(
                It.IsAny<CancellationToken>());

        // then
        actualLoggedInUser.Should().Be(expectedLoggedInUser);

        _userManagementBrokerMock.Verify(broker =>
            broker.GetLoggedInUserIdAsync(
                It.IsAny<CancellationToken>()),
            Times.Once());

        VerifyNoOtherDependencyCalls();
    }
}
