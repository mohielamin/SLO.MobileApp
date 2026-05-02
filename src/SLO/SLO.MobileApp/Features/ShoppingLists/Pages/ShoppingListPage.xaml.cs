using Microsoft.Maui.Controls;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.ViewModels.ShoppingLists;
using System;

namespace SLO.MobileApp.Features.ShoppingLists.Pages;

public partial class ShoppingListPage : ContentPage
{
    private readonly ShoppingListViewModel _shoppingListViewModel;

    public ShoppingItem SelectedShoppingListItem { get; set; }

    public ShoppingListPage(
        ShoppingListViewModel shoppingListViewModel)
    {
        InitializeComponent();
        _shoppingListViewModel = shoppingListViewModel;
        this.BindingContext = _shoppingListViewModel;
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

    private async void CaptureAddedShoppingItem(
        ShoppingListItemEditor page)
    {
        if (page.Discarded)
        {
            return;
        }

        var capturedShoppingItem =
                new ShoppingItem
                {
                    Id = Guid.NewGuid(),
                    Name = page.Name,
                    Description = page.Description,
                    Quantity = page.Quantity,
                };

        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(parameter: capturedShoppingItem);

        ItemsCollectionView.ScrollTo(
            item: capturedShoppingItem,
            position: ScrollToPosition.MakeVisible,
            animate: true);
    }

    private async void UpdateModifiedShoppingItem(
        ShoppingListItemEditor page)
    {
        if (SelectedShoppingListItem is null)
        {
            return;
        }

        if (page.Discarded)
        {
            return;
        }

        ShoppingItem modifiedShoppingItem =
            new ShoppingItem
            {
                Id = SelectedShoppingListItem.Id,
                Name = page.Name,
                Description = page.Description,
                Quantity = page.Quantity,
            };

        await _shoppingListViewModel.ModifyShoppingListItemCommand
            .ExecuteAsync(parameter: modifiedShoppingItem);
    }

    private async void RemoveShoppingItemClicked(
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

        await _shoppingListViewModel.RemoveShoppingListItemCommand
            .ExecuteAsync(parameter: shoppingItem);
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

        SelectedShoppingListItem = shoppingItem;

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