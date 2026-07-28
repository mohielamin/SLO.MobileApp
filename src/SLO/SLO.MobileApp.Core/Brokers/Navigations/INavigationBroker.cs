using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Navigations;

public interface INavigationBroker
{
    ValueTask PopAsync(CancellationToken cancellationToken);
}
