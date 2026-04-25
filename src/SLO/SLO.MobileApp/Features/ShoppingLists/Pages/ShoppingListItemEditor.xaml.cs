using Microsoft.Maui.Controls;
using SLO.MobileApp.Controls.Buttons;

namespace SLO.MobileApp.Features.ShoppingLists.Pages;

public partial class ShoppingListItemEditor : ContentPageBase
{
    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public decimal Quantity
    {
        get => (decimal)GetValue(QuantityProperty);
        set => SetValue(QuantityProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public bool EditMode { get; init; }

    public bool Discarded { get; private set; }

    public ShoppingListItemEditor(bool editMode = false)
    {
        InitializeComponent();
        EditMode = editMode;
        this.BindingContext = this;
    }

    public static readonly BindableProperty NameProperty =
        CreateProperty<string, ShoppingListItemEditor>(
            propertyName: nameof(Name));

    public static readonly BindableProperty QuantityProperty =
        CreateProperty<decimal, ShoppingListItemEditor>(
            propertyName: nameof(Quantity));

    public static readonly BindableProperty DescriptionProperty =
        CreateProperty<string, ShoppingListItemEditor>(
            propertyName: nameof(Description));

    private async void ClosePageClicked(object sender, System.EventArgs e)
    {
        if (sender is SecondaryButton)
        {
            Discarded = true;
        }

        await AppShell.Current.Navigation.PopModalAsync();
    }
}