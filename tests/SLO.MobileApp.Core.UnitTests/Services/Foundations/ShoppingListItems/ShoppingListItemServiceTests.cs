using Moq;
using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingListItems;

public partial class ShoppingListItemServiceTests
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IShoppingListItemService _shoppingListItemService;

    public ShoppingListItemServiceTests()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _dateTimeBrokerMock = new Mock<IDateTimeBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _shoppingListItemService =
            new ShoppingListItemService(
                storageBroker: _storageBrokerMock.Object,
                dateTimeBroker: _dateTimeBrokerMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    public static TheoryData<int> InvalidMinuteCases()
    {
        return new TheoryData<int>
        {
            Randomizers.GetRandomNumber(min: 2),
            Randomizers.GetRandomNegativeNumber(min: 2),
        };
    }

    private static ShoppingListItem CreateRandomShoppingListItem(
        DateTimeOffset dateTimes = default) =>
        CreateShoppingListItemFiller(dateTimes)
        .Create();

    private static IQueryable<ShoppingListItem> CreateRandomShoppingListItems() =>
        CreateShoppingListItemFiller()
        .Create(count: Randomizers.GetRandomNumber())
        .AsQueryable();

    private static Filler<ShoppingListItem> CreateShoppingListItemFiller(
        DateTimeOffset dateTimes = default)
    {
        var filler = new Filler<ShoppingListItem>();

        if (dateTimes == default)
        {
            dateTimes = Randomizers.GetRandomDateTime();
        }

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
