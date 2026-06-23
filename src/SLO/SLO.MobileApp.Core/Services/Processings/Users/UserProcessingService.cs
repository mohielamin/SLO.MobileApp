using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Services.Foundations.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.Users;

internal partial class UserProcessingService : IUserProcessingService
{
    private readonly IUserService _userService;
    private readonly ILoggingBroker _loggingBroker;

    public UserProcessingService(
        IUserService userService,
        ILoggingBroker loggingBroker)
    {
        _userService = userService;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<Guid> RetrieveLoggedInUserAsync(
        CancellationToken cancellationToken) =>
        await TryCatch(
            cancellationToken,
            async () =>
            {
                return await _userService.RetrieveLoggedInUserAsync(
                    cancellationToken);
            });
}
