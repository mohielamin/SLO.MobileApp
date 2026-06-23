using Moq;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Services.Foundations.Users;
using SLO.MobileApp.Core.Services.Processings.Users;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.Users;

public partial class UserProcessingServiceTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IUserProcessingService _userProcessingService;

    public UserProcessingServiceTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _userProcessingService =
            new UserProcessingService(
                userService: _userServiceMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _userServiceMock?.VerifyNoOtherCalls();
        _loggingBrokerMock?.VerifyNoOtherCalls();
    }
}
