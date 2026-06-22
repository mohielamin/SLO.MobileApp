using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.Users;

public interface IUserService
{
    ValueTask<Guid> RetrieveLoggedInUserAsync(CancellationToken cancellationToken);
}
