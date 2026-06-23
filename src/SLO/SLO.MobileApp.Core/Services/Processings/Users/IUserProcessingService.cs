using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.Users;

internal interface IUserProcessingService
{
    ValueTask<Guid> RetrieveLoggedInUserAsync(
        CancellationToken cancellationToken);
}
