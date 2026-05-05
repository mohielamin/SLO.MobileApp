using FluentAssertions;
using Force.DeepCloner;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
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
            ShoppingListItem randomShoppingListItem = CreateRandomShoppingListItem();
            ShoppingListItem existsShoppingListItem = randomShoppingListItem;

            ShoppingListItem inputShoppingListItem =
                existsShoppingListItem.DeepClone();

            var randomShoppingListItems =
                new ObservableCollection<ShoppingListItem>(
                    list: CreateRandomShoppingListItems().ToList());

            ObservableCollection<ShoppingListItem> currentShoppingListItems =
                randomShoppingListItems.DeepClone();

            ObservableCollection<ShoppingListItem> expectedShoppingListItems =
                randomShoppingListItems.DeepClone();

            currentShoppingListItems.Add(existsShoppingListItem);

            // when
            foreach (ShoppingListItem shoppingItem in currentShoppingListItems)
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
