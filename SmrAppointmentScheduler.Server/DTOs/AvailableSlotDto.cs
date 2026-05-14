using System;

namespace SmrAppointmentScheduler.Server.DTOs;

public class AvailableSlotDto
{
    public int AppointmentSlotId { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public int MechanicId { get; set; }
    public string MechanicName { get; set; } = null!;
    public int ServiceTypeId { get; set; }
    public string ServiceTypeName { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsBooked { get; set; }
}
