using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;

public class ShoppingListItemProcessingDependencyException : Exception
{
    public ShoppingListItemProcessingDependencyException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
