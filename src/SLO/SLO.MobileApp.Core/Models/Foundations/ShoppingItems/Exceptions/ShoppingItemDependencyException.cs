using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;

internal class ShoppingItemDependencyException : Exception
{
    public ShoppingItemDependencyException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
