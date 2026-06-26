using Moq;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Processings.ShoppingListItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingListItems;

public partial class ShoppingListItemProcessingServiceTests
{
    private readonly Mock<IShoppingListItemService> _shoppingListItemServiceMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IShoppingListItemProcessingService _shoppingListItemProcessingService;

    public ShoppingListItemProcessingServiceTests()
    {
        _shoppingListItemServiceMock = new Mock<IShoppingListItemService>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _shoppingListItemProcessingService =
            new ShoppingListItemProcessingService(
                shoppingListItemService: _shoppingListItemServiceMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _shoppingListItemServiceMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }

    private static IQueryable<ShoppingListItem> CreateRandomShoppingListItems() =>
        CreateShoppingListItemFiller()
        .Create(count: Randomizers.GetRandomNumber())
        .AsQueryable();

    private static ShoppingListItem CreateRandomShoppingListItem() =>
        CreateShoppingListItemFiller()
        .Create();

    private static Filler<ShoppingListItem> CreateShoppingListItemFiller()
    {
        var filler = new Filler<ShoppingListItem>();

        DateTimeOffset dateTimes = Randomizers.GetRandomDateTime();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(dateTimes);

        return filler;
    }
}
