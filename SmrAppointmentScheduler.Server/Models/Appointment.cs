using System.Collections.Generic;

namespace SmrAppointmentScheduler.Server.Models;

public class Appointment
{
    public int Id { get; set; }
    public int AppointmentSlotId { get; set; }
    public AppointmentSlot AppointmentSlot { get; set; } = null!;

    public string CustomerName { get; set; } = null!;
    public string? CustomerPhone { get; set; }
    public string? VehicleRegistration { get; set; }

    // Optional booking reference for external identification
    public string? BookingReference { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    public ICollection<WorkNote> WorkNotes { get; set; } = new List<WorkNote>();
}
