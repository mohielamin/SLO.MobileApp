using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;

public class NullShoppingListItemProcessingException : Exception
{
    public NullShoppingListItemProcessingException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
