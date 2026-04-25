using Microsoft.Maui.Controls;

namespace SLO.MobileApp.Features;

public partial class ContentPageBase : ContentPage
{
    internal static BindableProperty CreateProperty<PropertyType, DeclaringType>(
        string propertyName,
        object defaultValue = null,
        BindingMode defaultBindingMode = BindingMode.TwoWay,
        BindableProperty.BindingPropertyChangedDelegate propertyChangedDelegate = null,
        BindableProperty.BindingPropertyChangingDelegate propertyChangingDelegate = null)
    {
        return BindableProperty.Create(
            propertyName,
            returnType: typeof(PropertyType),
            declaringType: typeof(DeclaringType),
            defaultValue,
            defaultBindingMode,
            propertyChanged: propertyChangedDelegate,
            propertyChanging: propertyChangingDelegate);
    }

    internal static BindablePropertyKey CreateReadOnlyProperty<PropertyType, DeclaringType>(
        string propertyName,
        object defaultValue = null)
    {
        return BindableProperty.CreateReadOnly(
            propertyName,
            returnType: typeof(PropertyType),
                declaringType: typeof(DeclaringType),
                defaultValue);
    }
}
