using Moq;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Coordinations.ShoppingLists;
using SLO.MobileApp.Core.Services.Processings.ShoppingListItems;
using SLO.MobileApp.Core.Services.Processings.ShoppingLists;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Services.Coordinations.ShoppingLists;

public partial class ShoppingListCoordinationServiceTests
{
    private readonly Mock<IShoppingListProcessingService> _shoppingListProcessingServiceMock;
    private readonly Mock<IShoppingListItemProcessingService> _shoppingListItemProcessingServiceMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IShoppingListCoordinationService _shoppingListCoordinationService;

    public ShoppingListCoordinationServiceTests()
    {
        _shoppingListProcessingServiceMock =
            new Mock<IShoppingListProcessingService>();

        _shoppingListItemProcessingServiceMock =
            new Mock<IShoppingListItemProcessingService>();

        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _shoppingListCoordinationService =
            new ShoppingListCoordinationService(
                shoppingListProcessingService: _shoppingListProcessingServiceMock.Object,
                shoppingListItemProcessingService: _shoppingListItemProcessingServiceMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    private static IQueryable<ShoppingListItem> CreateRandomShoppingListItems(
        Guid shoppingListId) =>
        CreateShoppingListItemFiller(shoppingListId)
        .Create(
            count: Randomizers.GetRandomNumber())
        .AsQueryable();

    private static Filler<ShoppingListItem> CreateShoppingListItemFiller(
        Guid shoppingListId)
    {
        var filler = new Filler<ShoppingListItem>();
        DateTimeOffset dateTimes = Randomizers.GetRandomDateTime();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(
                valueToUse: dateTimes)
            .OnProperty(shoppingListItem =>
                shoppingListItem.Id).Use(shoppingListId);

        return filler;
    }

    private void VerifyNotOtherDependencyCalls()
    {
        _shoppingListProcessingServiceMock.VerifyNoOtherCalls();
        _shoppingListItemProcessingServiceMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
}
