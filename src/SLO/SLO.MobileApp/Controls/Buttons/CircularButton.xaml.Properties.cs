using Microsoft.Maui.Controls;

namespace SLO.MobileApp.Controls.Buttons;

public partial class CircularButton
{
    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public new double HeightRequest
    {
        get => (double)GetValue(HeightRequestProperty);
    }

    public new double WidthRequest
    {
        get => (double)GetValue(WidthRequestProperty);
    }

    public double ButtonDimensions
    {
        get => (double)GetValue(ButtonDimensionsProperty);
    }

    public static readonly BindableProperty SizeProperty =
        CreateProperty<double, CircularButton>(
            propertyName: nameof(Size),
            propertyChangedDelegate: OnButtonSizeChangedEvent);

    private static readonly BindableProperty ButtonDimensionsProperty =
        CreateProperty<double, CircularButton>(
            propertyName: nameof(ButtonDimensions));

    private static new readonly BindableProperty HeightRequestProperty =
        CreateProperty<double, CircularButton>(
            propertyName: nameof(HeightRequest));

    private static new readonly BindableProperty WidthRequestProperty =
        CreateProperty<double, CircularButton>(
            propertyName: nameof(WidthRequest));

    private static void OnButtonSizeChangedEvent(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        if (bindable is not CircularButton circularButton)
        {
            return;
        }

        if (oldValue.Equals(newValue))
        {
            return;
        }

        double widthHeightValue = (double)newValue * 2;
        circularButton.SetValue(ButtonDimensionsProperty, widthHeightValue);
    }
}
