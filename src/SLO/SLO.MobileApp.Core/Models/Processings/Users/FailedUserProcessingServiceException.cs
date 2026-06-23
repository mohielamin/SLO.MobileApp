using System;

namespace SLO.MobileApp.Core.Models.Processings.Users;

public class FailedUserProcessingServiceException : Exception
{
    public FailedUserProcessingServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
