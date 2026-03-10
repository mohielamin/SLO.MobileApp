using Moq;
using SLO.MobileApp.Brokers.Loggings;
using SLO.MobileApp.Brokers.Storages;
using SLO.MobileApp.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Services.Foundations.ShoppingItems;
using Tynamix.ObjectFiller;

namespace SLO.MobileApp.UnitTests.Services.Foundations.ShoppingItems;

public partial class ShoppingItemServiceTests
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IShoppingItemService _shoppingItemService;

    public ShoppingItemServiceTests()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _shoppingItemService =
            new ShoppingItemService(
                storageBroker: _storageBrokerMock.Object,
                loggingBroker: _loggingBrokerMock.Object);
    }

    private static ShoppingItem CreateRandomShoppingItem() =>
        CreateShoppingItemFiller()
        .Create();

    private static Filler<ShoppingItem> CreateShoppingItemFiller()
    {
        var filler = new Filler<ShoppingItem>();

        return filler;
    }

    private void VerifyNoOtherDependencyCalls()
    {
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
}
