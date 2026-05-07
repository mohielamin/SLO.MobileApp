using Moq;
using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Services.Foundations.ShoppingLists;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Linq;
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

    public static TheoryData<int> InvalidMinuteCases()
    {
        return new TheoryData<int>
        {
            Randomizers.GetRandomNumber(min: 2),
            Randomizers.GetRandomNumber(min: 2) * -1,
        };
    }

    private static ShoppingList CreateRandomShoppingList(
        DateTimeOffset dateTimes = default) =>
        CreateShoppingListFiller(dateTimes)
        .Create();

    private static IQueryable<ShoppingList> CreateRandomShoppingLists() =>
        CreateShoppingListFiller()
        .Create(count: Randomizers.GetRandomNumber())
        .AsQueryable();

    private static Filler<ShoppingList> CreateShoppingListFiller(
        DateTimeOffset dateTimes = default)
    {
        var filler = new Filler<ShoppingList>();

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
