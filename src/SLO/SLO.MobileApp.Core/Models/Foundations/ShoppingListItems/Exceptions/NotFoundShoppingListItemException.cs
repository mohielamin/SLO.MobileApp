using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class NotFoundShoppingListItemException : Exception
{
    public NotFoundShoppingListItemException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
