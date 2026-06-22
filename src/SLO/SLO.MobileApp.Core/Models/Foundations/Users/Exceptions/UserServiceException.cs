using System;

namespace SLO.MobileApp.Core.Models.Foundations.Users.Exceptions;

public class UserServiceException : Exception
{
    public UserServiceException(
        string exceptionMessage,
        Exception innerException)
    : base(exceptionMessage, innerException) { }
}
