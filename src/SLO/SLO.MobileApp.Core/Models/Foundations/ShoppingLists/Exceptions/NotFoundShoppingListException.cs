using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class NotFoundShoppingListException : Exception
{
    public NotFoundShoppingListException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
