using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class AlreadyExistsShoppingListException : Exception
{
    public AlreadyExistsShoppingListException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
