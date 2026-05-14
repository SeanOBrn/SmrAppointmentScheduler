using System;

namespace SmrAppointmentScheduler.Server.Common.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string? message = null) : base(message)
    {
    }
}
