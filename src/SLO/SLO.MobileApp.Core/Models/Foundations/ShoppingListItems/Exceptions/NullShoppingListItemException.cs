using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class NullShoppingListItemException : Exception
{
    public NullShoppingListItemException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
