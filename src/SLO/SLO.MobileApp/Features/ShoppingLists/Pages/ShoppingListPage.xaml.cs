using Microsoft.Maui.Controls;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.ViewModels.ShoppingLists;
using System;

namespace SLO.MobileApp.Features.ShoppingLists.Pages;

public partial class ShoppingListPage : ContentPage
{
    private readonly ShoppingListViewModel _shoppingListViewModel;

    public ShoppingListItem SelectedShoppingListItem { get; set; }

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
                UpdateModifiedShoppingListItem(shoppingListItemEditor);
                break;

            case false:
                CaptureAddedShoppingListItem(shoppingListItemEditor);
                break;
        }
    }

    private async void AddNewItemClicked(
        object sender,
        EventArgs e) =>
        await AppShell.Current.Navigation.PushModalAsync(
            page: new ShoppingListItemEditor(),
            animated: true);

    private async void CaptureAddedShoppingListItem(
        ShoppingListItemEditor page)
    {
        if (page.Discarded)
        {
            return;
        }

        var capturedShoppingListItem =
                new ShoppingListItem
                {
                    Id = Guid.NewGuid(),
                    Name = page.Name,
                    Description = page.Description,
                    Quantity = page.Quantity,
                };

        await _shoppingListViewModel.AddShoppingListItemCommand
            .ExecuteAsync(parameter: capturedShoppingListItem);

        ItemsCollectionView.ScrollTo(
            item: capturedShoppingListItem,
            position: ScrollToPosition.MakeVisible,
            animate: true);
    }

    private async void UpdateModifiedShoppingListItem(
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

        ShoppingListItem modifiedShoppingListItem =
            new ShoppingListItem
            {
                Id = SelectedShoppingListItem.Id,
                Name = page.Name,
                Description = page.Description,
                Quantity = page.Quantity,
            };

        await _shoppingListViewModel.ModifyShoppingListItemCommand
            .ExecuteAsync(parameter: modifiedShoppingListItem);
    }

    private async void RemoveShoppingListItemClicked(
        object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem)
        {
            return;
        }

        if (swipeItem.BindingContext is not
            ShoppingListItem shoppingItem)
        {
            return;
        }

        await _shoppingListViewModel.RemoveShoppingListItemCommand
            .ExecuteAsync(parameter: shoppingItem);
    }

    private async void EditShoppingListItemClicked(
        object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem)
        {
            return;
        }

        if (swipeItem.BindingContext is not
            ShoppingListItem shoppingItem)
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