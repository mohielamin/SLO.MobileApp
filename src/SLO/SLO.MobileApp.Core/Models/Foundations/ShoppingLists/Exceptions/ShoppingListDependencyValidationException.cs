using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class ShoppingListDependencyValidationException : Exception
{
    public ShoppingListDependencyValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
