using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.UserManagements;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.Users;

internal partial class UserService : IUserService
{
    private readonly IUserManagementBroker _userManagementBroker;
    private readonly ILoggingBroker _loggingBroker;

    public UserService(
        IUserManagementBroker userManagementBroker,
        ILoggingBroker loggingBroker)
    {
        _userManagementBroker = userManagementBroker;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<Guid> RetrieveLoggedInUserAsync(
        CancellationToken cancellationToken) =>
        await TryCatch(
            cancellationToken,
            async () =>
            {
                return await _userManagementBroker.GetLoggedInUserIdAsync(
                    cancellationToken);
            });
}
