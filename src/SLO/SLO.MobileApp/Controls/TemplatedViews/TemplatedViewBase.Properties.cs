using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace SLO.MobileApp.Controls.TemplatedViews;

public partial class TemplatedViewBase
{
    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
    }

    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
    }

    private static readonly BindablePropertyKey BackgroundColorPropertyKey =
        CreateReadOnlyProperty<Color, TemplatedButtonView>(
            propertyName: nameof(BackgroundColor));

    private static new readonly BindableProperty BackgroundColorProperty =
        BackgroundColorPropertyKey.BindableProperty;

    private static readonly BindablePropertyKey BackgroundPropertyKey =
        CreateReadOnlyProperty<Brush, TemplatedButtonView>(
            nameof(BackgroundColor));

    private static new readonly BindableProperty BackgroundProperty =
        BackgroundPropertyKey.BindableProperty;
}
