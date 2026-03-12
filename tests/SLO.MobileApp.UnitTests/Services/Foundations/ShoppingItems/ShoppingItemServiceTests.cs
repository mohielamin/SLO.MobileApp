using Moq;
using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IShoppingItemService _shoppingItemService;

    public ShoppingItemServiceTests()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _dateTimeBrokerMock = new Mock<IDateTimeBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _shoppingItemService =
            new ShoppingItemService(
                storageBroker: _storageBrokerMock.Object,
                dateTimeBroker: _dateTimeBrokerMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    public static TheoryData<int> InvalidMoreThanOneMinuteCases() =>
        new TheoryData<int>
        {
            Randomizers.GetRandomNumber(2),
            Randomizers.GetRandomNegativeNumber(2),
        };

    private static ShoppingItem CreateRandomShoppingItem(
        DateTimeOffset dateTimes = default) =>
        CreateShoppingItemFiller(dateTimes)
        .Create();

    private static Filler<ShoppingItem> CreateShoppingItemFiller(
        DateTimeOffset dateTimes = default)
    {
        var filler = new Filler<ShoppingItem>();

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
