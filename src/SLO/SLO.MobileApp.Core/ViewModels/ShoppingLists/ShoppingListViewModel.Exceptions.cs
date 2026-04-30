using SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.ViewModels.ShoppingLists;

internal partial class ShoppingListViewModel
{
    private delegate Task ReturningShoppingListFunctions();

    private async Task TryCatch(
        ReturningShoppingListFunctions returningShoppingListFunctions)
    {
        try
        {
            await returningShoppingListFunctions();
        }
        catch (ShoppingItemValidationException ex)
        {
            ErrorMessage = ex.InnerException.Message;
        }
        catch (ShoppingItemDependencyValidationException ex)
        {
            ErrorMessage += ex.InnerException.Message;
        }
        catch (ShoppingItemDependencyException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (ShoppingItemServiceException ex)
        {
            ErrorMessage += ex.Message;
        }
    }
}
