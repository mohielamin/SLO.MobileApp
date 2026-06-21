using System;

namespace SLO.MobileApp.Core.Models.Foundations.Users.Exceptions;

public class FailedUserServiceException : Exception
{
    public FailedUserServiceException(
        string exceptionMessage,
        Exception innerException)
       : base(exceptionMessage, innerException) { }
}
