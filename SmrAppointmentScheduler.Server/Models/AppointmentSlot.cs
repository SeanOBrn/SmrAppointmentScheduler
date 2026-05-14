using System;
using System.Collections.Generic;

namespace SmrAppointmentScheduler.Server.Models;

public class AppointmentSlot
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public int MechanicId { get; set; }
    public Mechanic Mechanic { get; set; } = null!;

    public int ServiceTypeId { get; set; }
    public ServiceType ServiceType { get; set; } = null!;

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public Appointment? Appointment { get; set; }
}
