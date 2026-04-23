using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using SLO.MobileApp.Controls.TemplatedViews;
using System;

namespace SLO.MobileApp.Controls.Buttons;

public partial class SecondaryButton : TemplatedButtonView
{
    public new Color ButtonColor
    {
        get => (Color)GetValue(ButtonColorProperty);
    }

    public SecondaryButton()
    {
        InitializeComponent();
    }

    public override event EventHandler Clicked;

    protected override void OnApplyTemplate()
    {
        SetValue(ButtonColorProperty, Colors.Gray);
    }

    private static new readonly BindableProperty ButtonColorProperty =
        CreateProperty<Color, PrimaryButton>(propertyName: nameof(ButtonColor));

    private void ButtonClickedEvent(object sender, EventArgs e)
    {
        Clicked?.Invoke(this, e);
    }
}