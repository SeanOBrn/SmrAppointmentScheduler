using System.Collections.Generic;

namespace SmrAppointmentScheduler.Server.Models;

public class ServiceType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();
}
