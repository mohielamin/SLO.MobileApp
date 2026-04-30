using Moq;
using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using SLO.MobileApp.Core.ViewModels.ShoppingLists;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly ShoppingListViewModel _shoppingListViewModel;

    public ShoppingListViewModelTests()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _dateTimeBrokerMock = new Mock<IDateTimeBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _shoppingListViewModel =
            new ShoppingListViewModel(
                storageBroker: _storageBrokerMock.Object,
                dateTimeBroker: _dateTimeBrokerMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _storageBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }

    private IQueryable<ShoppingItem> CreateRandomShoppingItems() =>
        CreateShoppingItemFiller()
        .Create(count: Randomizers.GetRandomNumber())
        .AsQueryable();

    private Filler<ShoppingItem> CreateShoppingItemFiller()
    {
        var filler = new Filler<ShoppingItem>();
        DateTimeOffset randomDateTime = Randomizers.GetRandomDateTime();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(randomDateTime);

        return filler;
    }
}
