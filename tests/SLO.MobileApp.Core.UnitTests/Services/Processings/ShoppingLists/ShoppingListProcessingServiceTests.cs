using Moq;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Services.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Services.Processings.ShoppingLists;
using SLO.MobileApp.Core.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingLists;

public partial class ShoppingListProcessingServiceTests
{
    private readonly Mock<IShoppingListService> _shoppingListServiceMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IShoppingListProcessingService _shoppingListProcessingService;

    public ShoppingListProcessingServiceTests()
    {
        _shoppingListServiceMock = new Mock<IShoppingListService>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _shoppingListProcessingService =
            new ShoppingListProcessingService(
                shoppingListService: _shoppingListServiceMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    private static IQueryable<ShoppingList> CreateRandomShoppingLists(
        Guid createdBy = default) =>
        CreateShoppingListFiller(createdBy)
        .Create(count: Randomizers.GetRandomNumber())
        .AsQueryable();


    private static IQueryable<ShoppingList> CreateRandomShoppingLists(
        IQueryable<ShoppingList> existingShoppingLists)
    {
        List<ShoppingList> randomShoppingList =
            CreateShoppingListFiller()
            .Create(count: Randomizers.GetRandomNumber())
            .ToList();

        randomShoppingList.AddRange(existingShoppingLists);

        return randomShoppingList.AsQueryable();
    }

    private static Filler<ShoppingList> CreateShoppingListFiller(
        Guid createdBy = default)
    {
        var filler = new Filler<ShoppingList>();
        DateTimeOffset dateTimes = Randomizers.GetRandomDateTime();

        if (createdBy == Guid.Empty)
        {
            createdBy = Guid.NewGuid();
        }

        filler.Setup()
            .OnType<DateTimeOffset>().Use(dateTimes)
            .OnProperty(shoppinglist =>
                shoppinglist.CreatedBy)
            .Use(createdBy);

        return filler;
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _shoppingListServiceMock?.VerifyNoOtherCalls();
        _loggingBrokerMock?.VerifyNoOtherCalls();
    }
}
