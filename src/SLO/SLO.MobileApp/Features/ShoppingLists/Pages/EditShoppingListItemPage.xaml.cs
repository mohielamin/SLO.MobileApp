using Microsoft.Maui.Controls;
using SLO.MobileApp.Controls.Buttons;
using System;
using System.Runtime.CompilerServices;

namespace SLO.MobileApp.Features.ShoppingLists.Pages;

public partial class EditShoppingListItemPage : ContentPage
{
    public string Name
    {
        get => GetValue();
        set => SetValue(value);
    }

    public string Description
    {
        get => GetValue();
        set => SetValue(value);
    }

    public decimal Quantity
    {
        get => GetValue();
        set => SetValue(value);
    }

    public bool Discarded { get; private set; }

    public EditShoppingListItemPage(
        string name,
        string description,
        decimal quantity)
    {
        InitializeComponent();
        Name = name;
        Description = description;
        Quantity = quantity;
        this.BindingContext = this;
    }

    public static readonly BindableProperty NameProperty =
        CreateProperty<string>(propertyName: nameof(Name));

    public static readonly BindableProperty DescriptionProperty =
        CreateProperty<string>(propertyName: nameof(Description));

    public static readonly BindableProperty QuantityProperty =
        CreateProperty<decimal>(propertyName: nameof(Quantity));

    protected override void OnParentChanging(ParentChangingEventArgs args)
    {
        base.OnParentChanging(args);
    }

    private dynamic GetValue(
        [CallerMemberName] string propertyName = null) =>
        propertyName switch
        {
            nameof(Name) => GetValue(NameProperty),
            nameof(Description) => GetValue(DescriptionProperty),
            nameof(Quantity) => GetValue(QuantityProperty),

            _ => throw new ArgumentNullException(nameof(propertyName))
        };

    private void SetValue(
        object value,
        [CallerMemberName] string propertyName = null)
    {
        switch (propertyName)
        {
            case nameof(Name):
                SetValue(NameProperty, value);
                return;

            case nameof(Description):
                SetValue(DescriptionProperty, value);
                return;

            case nameof(Quantity):
                SetValue(QuantityProperty, value);
                return;

            default:
                throw new ArgumentNullException(nameof(propertyName));
        }
    }
    private static BindableProperty CreateProperty<T>(
        string propertyName,
        string defaultValue = null) =>
        BindableProperty.Create(
            propertyName,
            returnType: typeof(T),
            declaringType: typeof(AddShoppingListItemPage),
            defaultValue,
            defaultBindingMode: BindingMode.TwoWay);

    private async void ClosePageClicked(object sender, EventArgs e)
    {
        if (sender is SecondaryButton)
        {
            Discarded = true;
        }

        await AppShell.Current.Navigation.PopModalAsync();
    }
}