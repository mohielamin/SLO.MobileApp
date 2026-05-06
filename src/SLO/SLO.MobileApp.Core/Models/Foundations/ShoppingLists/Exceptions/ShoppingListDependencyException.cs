using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class ShoppingListDependencyException : Exception
{
    public ShoppingListDependencyException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
