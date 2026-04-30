using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.ViewModels.ShoppingLists;

internal partial class ShoppingListViewModel : ObservableObject
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public ShoppingListViewModel(
        IStorageBroker storageBroker,
        IDateTimeBroker dateTimeBroker,
        ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
    }

    public ObservableCollection<ShoppingItem> ShoppingItems { get; private set; }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RetrieveAllShoppingItemsAsync(
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
