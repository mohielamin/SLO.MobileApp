using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class ShoppingListItemDependencyException : Exception
{
    public ShoppingListItemDependencyException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
