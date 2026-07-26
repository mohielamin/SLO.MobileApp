using System;

namespace SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;

public class ShoppingListCoordinationDependencyValidationException : Exception
{
    public ShoppingListCoordinationDependencyValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
