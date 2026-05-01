using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using SLO.MobileApp.Core.ViewModels.ShoppingLists;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    private readonly ShoppingListViewModel _shoppingListViewModel;

    public ShoppingListViewModelTests() =>
        _shoppingListViewModel =
            new ShoppingListViewModel();

    private static ShoppingItem CreateRandomShoppingItem() =>
        CreateShoppingItemFiller()
        .Create();

    private static IQueryable<ShoppingItem> CreateRandomShoppingItems() =>
        CreateShoppingItemFiller()
        .Create(count: Randomizers.GetRandomNumber())
        .AsQueryable();

    private static Filler<ShoppingItem> CreateShoppingItemFiller()
    {
        var filler = new Filler<ShoppingItem>();
        DateTimeOffset randomDateTime = Randomizers.GetRandomDateTime();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(randomDateTime);

        return filler;
    }
}
