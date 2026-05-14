using System;
using System.Collections.Generic;

namespace SmrAppointmentScheduler.Server.DTOs;

public class AppointmentDetailDto
{
    public int AppointmentId { get; set; }
    public int AppointmentSlotId { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public int MechanicId { get; set; }
    public string MechanicName { get; set; } = null!;
    public int ServiceTypeId { get; set; }
    public string ServiceTypeName { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string CustomerName { get; set; } = null!;
    public string? CustomerPhone { get; set; }
    public string? VehicleRegistration { get; set; }
    public string Status { get; set; } = null!;
    public List<WorkNoteDto> WorkNotes { get; set; } = new List<WorkNoteDto>();
}
