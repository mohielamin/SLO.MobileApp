using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class NullShoppingListException : Exception
{
    public NullShoppingListException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
