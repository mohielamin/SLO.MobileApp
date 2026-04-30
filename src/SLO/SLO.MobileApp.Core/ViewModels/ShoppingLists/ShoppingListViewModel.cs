using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;
using SLO.MobileApp.Core.Services.Foundations.ShoppingItems;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.ViewModels.ShoppingLists;

internal partial class ShoppingListViewModel : ObservableObject
{
    private readonly IShoppingItemService _shoppingItemService;

    public ShoppingListViewModel(
        IShoppingItemService shoppingItemService) =>
        _shoppingItemService = shoppingItemService;

    public ObservableCollection<ShoppingItem> ShoppingItems { get; private set; }

    [ObservableProperty]
    private string errorMessage;

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RetrieveAllShoppingItemsAsync(
        CancellationToken cancellationToken)
    {
        try
        {

            IQueryable<ShoppingItem> retrievedShoppingItems =
                await _shoppingItemService.RetrieveAllShoppingItemsAsync(
                    cancellationToken);

            ShoppingItems =
                new ObservableCollection<ShoppingItem>(
                    list: retrievedShoppingItems.ToList());
        }
        catch (ShoppingItemDependencyException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (ShoppingItemServiceException ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
