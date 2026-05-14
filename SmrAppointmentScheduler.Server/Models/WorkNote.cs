using System;

namespace SmrAppointmentScheduler.Server.Models;

public class WorkNote
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;

    public string Note { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
