namespace SmrAppointmentScheduler.Server.DTOs;

public class MechanicDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => FirstName + " " + LastName;
    public int BranchId { get; set; }
}
