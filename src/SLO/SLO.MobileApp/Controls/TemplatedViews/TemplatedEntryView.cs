using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using SLO.MobileApp.Controls.Bases;
using System.Threading.Tasks;

namespace SLO.MobileApp.Controls.TemplatedViews;

public abstract partial class TemplatedEntryView : TemplatedViewBase
{
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

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

    public bool SelectAllTextOnFocus
    {
        get => (bool)GetValue(SelectAllTextOnFocusProperty);
        set => SetValue(SelectAllTextOnFocusProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty =
        CreateProperty<string, TemplatedEntryView>(
            propertyName: nameof(Placeholder));

    public static readonly BindableProperty TextProperty =
        CreateProperty<string, TemplatedEntryView>(
            propertyName: nameof(Text));

    public static readonly BindableProperty TextColorProperty =
        CreateProperty<Color, TemplatedEntryView>(
            propertyName: nameof(TextColor));

    public static readonly BindableProperty SelectAllTextOnFocusProperty =
        CreateProperty<bool, TemplatedEntryView>(
            propertyName: nameof(SelectAllTextOnFocus),
            defaultValue: false);

    internal void OnEntryFocusedEvent(object sender, FocusEventArgs e)
    {
        if (SelectAllTextOnFocus is false)
        {
            return;
        }

        if (sender is not EntryBase entryBase)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(entryBase.Text))
        {
            return;
        }

        _ = Dispatcher.Dispatch(async () =>
        {
            await Task.Delay(10);
            entryBase.CursorPosition = 0;
            entryBase.SelectionLength = entryBase.Text.Length;
        });
    }
}
