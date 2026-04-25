using Microsoft.Maui.Controls;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SLO.MobileApp.Features.ShoppingLists.Pages;

public partial class ShoppingListPage : ContentPage
{
    public ObservableCollection<ShoppingItem> ShoppingItems { get; } =
        new ObservableCollection<ShoppingItem>();

    public ShoppingItem SelectedShoppingItem { get; set; }

    public ShoppingListPage()
    {
        InitializeComponent();
        this.BindingContext = this;
    }

    protected override void OnNavigatedTo(
        NavigatedToEventArgs args)
    {
        var shoppingListItemEditor =
            args?.PreviousPage as ShoppingListItemEditor;

        switch (shoppingListItemEditor?.EditMode)
        {
            case true:
                UpdateModifiedShoppingItem(shoppingListItemEditor);
                break;

            case false:
                CaptureAddedShoppingItem(shoppingListItemEditor);
                break;
        }
    }

    private async void AddNewItemClicked(
        object sender,
        EventArgs e) =>
        await AppShell.Current.Navigation.PushModalAsync(
            page: new ShoppingListItemEditor(),
            animated: true);

    private void CaptureAddedShoppingItem(
        ShoppingListItemEditor page)
    {
        if (page.Discarded)
        {
            return;
        }

        var capturedShoppingItem =
                new ShoppingItem
                {
                    Name = page.Name,
                    Description = page.Description,
                    Quantity = page.Quantity,
                };

        ShoppingItems.Add(capturedShoppingItem);

        ItemsCollectionView.ScrollTo(
            item: capturedShoppingItem,
            position: ScrollToPosition.MakeVisible,
            animate: true);
    }

    private void UpdateModifiedShoppingItem(
        ShoppingListItemEditor page)
    {
        if (SelectedShoppingItem is null)
        {
            return;
        }

        if (page.Discarded)
        {
            return;
        }

        ShoppingItem foundShoppingItem =
            ShoppingItems.FirstOrDefault(shoppingItem =>
            shoppingItem.Equals(SelectedShoppingItem));

        if (foundShoppingItem is null)
        {
            return;
        }

        ShoppingItem updatedShoppingItem =
            new ShoppingItem
            {
                Name = page.Name,
                Description = page.Description,
                Quantity = page.Quantity,
            };

        int currentItemIndex = ShoppingItems.IndexOf(foundShoppingItem);
        ShoppingItems[currentItemIndex] = updatedShoppingItem;
    }

    private void RemoveShoppingItemClicked(
        object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem)
        {
            return;
        }

        if (swipeItem.BindingContext is not
            ShoppingItem shoppingItem)
        {
            return;
        }

        ShoppingItems.Remove(item: shoppingItem);
    }

    private async void EditShoppingItemClicked(
        object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem)
        {
            return;
        }

        if (swipeItem.BindingContext is not
            ShoppingItem shoppingItem)
        {
            return;
        }

        SelectedShoppingItem = shoppingItem;

        var shoppingListItemEditor =
            new ShoppingListItemEditor(
                editMode: true)
            {
                Name = shoppingItem.Name,
                Quantity = shoppingItem.Quantity,
                Description = shoppingItem.Description
            };

        await AppShell.Current.Navigation.PushModalAsync(
            page: shoppingListItemEditor,
            animated: true);
    }
}