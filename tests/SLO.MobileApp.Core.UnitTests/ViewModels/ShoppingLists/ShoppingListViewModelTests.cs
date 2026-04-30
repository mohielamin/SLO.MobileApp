using Moq;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using SLO.MobileApp.Core.ViewModels.ShoppingLists;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists;

public partial class ShoppingListViewModelTests
{
    private readonly Mock<IShoppingItemService> _shoppingItemServiceMock;
    private readonly ShoppingListViewModel _shoppingListViewModel;

    public ShoppingListViewModelTests()
    {
        _shoppingItemServiceMock = new Mock<IShoppingItemService>();

        _shoppingListViewModel =
            new ShoppingListViewModel(
                shoppingItemService: _shoppingItemServiceMock.Object);
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _shoppingItemServiceMock.VerifyNoOtherCalls();
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
