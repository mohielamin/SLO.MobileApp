using Moq;
using SLO.MobileApp.Core.Brokers.Navigations;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Orchestrations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.UnitTests.Helpers;
using SLO.MobileApp.Core.ViewModels.Orchestrations.ShoppingListItemEditors;
using System;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.Orchestrations.ShoppingListEditors;

public partial class ShoppingListItemEditorOrchestrationViewModelTests
{
    private readonly Mock<IShoppingListItemService> _shoppingListItemServiceMock;
    private readonly Mock<INavigationBroker> _navigationBrokerMock;
    private readonly ShoppingListItemEditorOrchestrationViewModel _shoppingListItemEditorOrchestrationViewModel;

    public ShoppingListItemEditorOrchestrationViewModelTests()
    {
        _shoppingListItemServiceMock =
            new Mock<IShoppingListItemService>();

        _navigationBrokerMock = new Mock<INavigationBroker>();

        _shoppingListItemEditorOrchestrationViewModel =
            new ShoppingListItemEditorOrchestrationViewModel(
                shoppingListItemService: _shoppingListItemServiceMock.Object,
                navigationBroker: _navigationBrokerMock.Object);
    }

    public static TheoryData<ShoppingListItemMode> ShoppingListItemModes()
    {
        return new TheoryData<ShoppingListItemMode>
        {
            ShoppingListItemMode.New,
            ShoppingListItemMode.Edit,
        };
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _shoppingListItemServiceMock.VerifyNoOtherCalls();
        _navigationBrokerMock.VerifyNoOtherCalls();
    }

    private static ShoppingListItem CreateRandomShoppingListItem() =>
        CreateShoppingListItemFiller()
        .Create();

    private static Filler<ShoppingListItem> CreateShoppingListItemFiller()
    {
        var filler = new Filler<ShoppingListItem>();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(
                Randomizers.GetRandomDateTime);

        return filler;
    }
}
