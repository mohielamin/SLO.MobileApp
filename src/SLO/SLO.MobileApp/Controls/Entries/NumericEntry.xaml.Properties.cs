using Microsoft.Maui.Controls;

namespace SLO.MobileApp.Controls.Entries;

public partial class NumericEntry
{
    public static readonly BindableProperty ValueProperty =
        CreateProperty<int, NumericEntry>(
            propertyName: nameof(Value),
            propertyChangedDelegate: ValueChangedEvent);

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty MinValueProperty =
        CreateProperty<int, NumericEntry>(
            propertyName: nameof(MinValue),
            propertyChangedDelegate: MinValueChangedEvent);

    public int MinValue
    {
        get => (int)GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    private static void ValueChangedEvent(BindableObject bindable,
        object oldValue, object newValue)
    {
        var numericEntry = bindable as NumericEntry;

        if (numericEntry is null)
        {
            return;
        }

        if (oldValue.Equals(newValue))
        {
            return;
        }

        if (NotNumeric(value: newValue))
        {
            numericEntry.Value = (int)oldValue;
        }

        if (numericEntry.MinValue <= (int)newValue)
        {
            return;
        }

        numericEntry.Value = numericEntry.MinValue;
    }

    private static void MinValueChangedEvent(BindableObject bindable,
        object oldValue, object newValue)
    {
        var numericEntry = bindable as NumericEntry;

        if (numericEntry is null)
        {
            return;
        }

        if (oldValue.Equals(newValue))
        {
            return;
        }

        if (numericEntry.Value > (int)newValue)
        {
            return;
        }

        numericEntry.Value = (int)newValue;
    }

    private static bool NotNumeric(object value)
    {
        int? intValue = value as int?;

        if (intValue is null)
        {
            return true;
        }

        return false;
    }
}
