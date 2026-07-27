using System;

namespace SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;

public class ShoppingListCoordinationDependencyException : Exception
{
    public ShoppingListCoordinationDependencyException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
