using System;

namespace SmrAppointmentScheduler.Server.DTOs;

public class WorkNoteDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public string Note { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
