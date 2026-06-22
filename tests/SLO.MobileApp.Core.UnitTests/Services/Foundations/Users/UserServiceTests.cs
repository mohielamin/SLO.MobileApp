using Moq;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.UserManagements;
using SLO.MobileApp.Core.Services.Foundations.Users;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.Users;

public partial class UserServiceTests
{
    private readonly Mock<IUserManagementBroker> _userManagementBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userManagementBrokerMock = new Mock<IUserManagementBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _userService =
            new UserService(
                userManagementBroker: _userManagementBrokerMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _userManagementBrokerMock?.VerifyNoOtherCalls();
        _loggingBrokerMock?.VerifyNoOtherCalls();

    }
}
