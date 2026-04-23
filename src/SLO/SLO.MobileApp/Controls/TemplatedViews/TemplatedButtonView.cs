using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace SLO.MobileApp.Controls.TemplatedViews;

public abstract partial class TemplatedButtonView : TemplatedViewBase
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public Color ButtonColor
    {
        get => (Color)GetValue(ButtonColorProperty);
        set => SetValue(ButtonColorProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
    }

    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
    }

    public abstract event EventHandler Clicked;

    public static readonly BindableProperty TextProperty =
        CreateProperty<string, TemplatedButtonView>(
            propertyName: nameof(Text),
            defaultValue: string.Empty);

    public static readonly BindableProperty FontSizeProperty =
        CreateProperty<double, TemplatedButtonView>(
            propertyName: nameof(FontSize));

    public static readonly BindableProperty FontAttributesProperty =
        CreateProperty<FontAttributes, TemplatedButtonView>(
            propertyName: nameof(FontAttributes));

    public static readonly BindableProperty ButtonColorProperty =
        CreateProperty<Color, TemplatedButtonView>(
            propertyName: nameof(ButtonColor));

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
