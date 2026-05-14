using System;

namespace SmrAppointmentScheduler.Server.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string? message = null) : base(message)
    {
    }
}
