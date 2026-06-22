using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.UserManagements;

public interface IUserManagementBroker
{
    ValueTask<Guid> GetLoggedInUserIdAsync(CancellationToken cancellationToken);
}
