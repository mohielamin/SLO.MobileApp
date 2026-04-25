using Microsoft.Maui.Controls;
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

    public static readonly BindableProperty PlaceholderProperty =
        CreateProperty<string, TemplatedEntryView>(
            propertyName: nameof(Placeholder));

    public static readonly BindableProperty TextProperty =
        CreateProperty<string, TemplatedEntryView>(
            propertyName: nameof(Text));

    public static readonly BindableProperty SelectAllTextOnFocus =
        CreateProperty<bool, TemplatedEntryView>(
            propertyName: nameof(IsFullTextSelectionEnabled),
            defaultValue: false,
            propertyChangedDelegate: IsFullTextSelectionEnabledChanged);

    public bool IsFullTextSelectionEnabled
    {
        get => (bool)GetValue(SelectAllTextOnFocus);
        set => SetValue(SelectAllTextOnFocus, value);
    }

    protected override void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        base.OnHandlerChanging(args);

        if (args.NewHandler is not null)
        {
            return;
        }

        this.Focused -= OnEntryFocusedEvent;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (this.Handler is null)
        {
            return;
        }

        if (IsFullTextSelectionEnabled)
        {
            this.Focused += OnEntryFocusedEvent;
        }
    }

    private static void IsFullTextSelectionEnabledChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        if (bindable is not TemplatedEntryView templatedEntryView)
        {
            return;
        }

        if (oldValue.Equals(newValue))
        {
            return;
        }

        if ((bool)newValue is false)
        {
            templatedEntryView.Focused -=
                templatedEntryView.OnEntryFocusedEvent;

            return;
        }

        templatedEntryView.Focused -=
            templatedEntryView.OnEntryFocusedEvent;

        templatedEntryView.Focused +=
            templatedEntryView.OnEntryFocusedEvent;
    }

    private void OnEntryFocusedEvent(object sender, FocusEventArgs e)
    {
        if (IsFullTextSelectionEnabled is false)
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
