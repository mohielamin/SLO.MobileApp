using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace SLO.MobileApp.Views.Bases;

public partial class EntryBase : Entry
{
    public static readonly BindableProperty IsFullTextSelectionEnabledProperty =
        BindableProperty.Create(
            propertyName: nameof(IsFullTextSelectionEnabled),
            returnType: typeof(bool),
            declaringType: typeof(EntryBase),
            defaultValue: false,
            propertyChanged: IsFullTextSelectionEnabledChanged);

    public bool IsFullTextSelectionEnabled
    {
        get => (bool)GetValue(IsFullTextSelectionEnabledProperty);
        set => SetValue(IsFullTextSelectionEnabledProperty, value);
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
        if (bindable is not EntryBase entryBase)
        {
            return;
        }

        if (oldValue.Equals(newValue))
        {
            return;
        }

        if ((bool)newValue is false)
        {
            entryBase.Focused -= entryBase.OnEntryFocusedEvent;

            return;
        }

        entryBase.Focused -= entryBase.OnEntryFocusedEvent;
        entryBase.Focused += entryBase.OnEntryFocusedEvent;
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
