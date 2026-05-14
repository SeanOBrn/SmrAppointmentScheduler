using SmrAppointmentScheduler.Server.Models;

namespace SmrAppointmentScheduler.Server.DTOs;

public class UpdateAppointmentStatusDto
{
    public int AppointmentId { get; set; }
    public AppointmentStatus Status { get; set; }
}
