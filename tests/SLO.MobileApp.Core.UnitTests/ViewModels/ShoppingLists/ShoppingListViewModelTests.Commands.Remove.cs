using FluentAssertions;
using Force.DeepCloner;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.UnitTests.ViewModels.ShoppingLists
{
    public partial class ShoppingListViewModelTests
    {
        [Fact]
        public async Task ShouldRemoveShoppingListItemAsync()
        {
            // given
            ShoppingItem randomShoppingItem = CreateRandomShoppingItem();
            ShoppingItem existsShoppingListItem = randomShoppingItem;

            ShoppingItem inputShoppingListItem =
                existsShoppingListItem.DeepClone();

            var randomShoppingListItems =
                new ObservableCollection<ShoppingItem>(
                    list: CreateRandomShoppingItems().ToList());

            ObservableCollection<ShoppingItem> currentShoppingListItems =
                randomShoppingListItems.DeepClone();

            ObservableCollection<ShoppingItem> expectedShoppingListItems =
                randomShoppingListItems.DeepClone();

            currentShoppingListItems.Add(existsShoppingListItem);

            // when
            foreach (ShoppingItem shoppingItem in currentShoppingListItems)
            {
                await _shoppingListViewModel.AddShoppingListItemCommand
                    .ExecuteAsync(parameter: shoppingItem);
            }

            await _shoppingListViewModel.RemoveShoppingListItemCommand
                .ExecuteAsync(parameter: inputShoppingListItem);

            // then
            _shoppingListViewModel.ShoppingListItems.Should().BeEquivalentTo(
                expectedShoppingListItems);

            _shoppingListViewModel.ErrorMessage.Should().BeNull();
        }
    }
}
