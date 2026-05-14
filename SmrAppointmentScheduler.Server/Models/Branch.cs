using System.Collections.Generic;

namespace SmrAppointmentScheduler.Server.Models;

public class Branch
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }

    public ICollection<Mechanic> Mechanics { get; set; } = new List<Mechanic>();
    public ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();
}
