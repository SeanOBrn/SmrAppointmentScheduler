namespace SmrAppointmentScheduler.Server.DTOs;

public class CreateBookingRequestDto
{
    public int AppointmentSlotId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string? CustomerPhone { get; set; }
    public int ServiceTypeId { get; set; }
    public int BranchId { get; set; }
    public string? VehicleRegistration { get; set; }
    public string? Notes { get; set; }
}
