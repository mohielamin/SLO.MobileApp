using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace SLO.MobileApp.Controls.TemplatedViews;

public abstract partial class TemplatedLabelView : TemplatedViewBase
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty TextProperty =
        CreateProperty<string, TemplatedLabelView>(
            propertyName: nameof(Text));

    public static readonly BindableProperty TextColorProperty =
        CreateProperty<Color, TemplatedLabelView>(
            propertyName: nameof(TextColor));

    public static readonly BindableProperty FontAttributesProperty =
        CreateProperty<FontAttributes, TemplatedLabelView>(
            propertyName: nameof(FontAttributes));

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
        CreateProperty<TextAlignment, TemplatedLabelView>(
            propertyName: nameof(HorizontalTextAlignment));

    public static readonly BindableProperty VerticalTextAlignmentProperty =
        CreateProperty<TextAlignment, TemplatedLabelView>(
            propertyName: nameof(VerticalTextAlignment));
}
