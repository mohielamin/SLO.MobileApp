using System;

namespace SLO.MobileApp.Core.Models.Processings.Users;

public class UserProcessingServiceException : Exception
{
    public UserProcessingServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
