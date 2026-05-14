namespace SmrAppointmentScheduler.Server.DTOs;

public class BookingConfirmationDto
{
    public int AppointmentId { get; set; }
    public int AppointmentSlotId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? BookingReference { get; set; }
}
