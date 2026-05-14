using System;

namespace SmrAppointmentScheduler.Server.DTOs;

public class MechanicAppointmentDto
{
    public int AppointmentId { get; set; }
    public int AppointmentSlotId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string CustomerName { get; set; } = null!;
    public string Status { get; set; } = null!;
}
