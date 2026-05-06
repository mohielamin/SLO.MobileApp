using Moq;
using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Services.Foundations.ShoppingLists;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingLists;

public partial class ShoppingListServiceTests
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IShoppingListService _shoppingListService;

    public ShoppingListServiceTests()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _dateTimeBrokerMock = new Mock<IDateTimeBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _shoppingListService =
            new ShoppingListService(
                storageBroker: _storageBrokerMock.Object,
                dateTimeBroker: _dateTimeBrokerMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    private static ShoppingList CreateRandomShoppingList() =>
        CreateShoppingListFiller()
        .Create();

    private static Filler<ShoppingList> CreateShoppingListFiller()
    {
        var filler = new Filler<ShoppingList>();

        DateTimeOffset dateTimes = Randomizers.GetRandomDateTime();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(dateTimes);

        return filler;
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _storageBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
}
