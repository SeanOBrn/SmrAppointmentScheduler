using System.Collections.Generic;
using System.Threading.Tasks;
using SmrAppointmentScheduler.Server.DTOs;

namespace SmrAppointmentScheduler.Server.Services;

public interface IMechanicService
{
    Task<List<MechanicDto>> GetMechanicsAsync(int? branchId = null);
    Task<List<MechanicAppointmentDto>> GetMechanicAppointmentsAsync(int mechanicId);
    Task<AppointmentDetailDto?> GetAppointmentDetailAsync(int appointmentId);
    Task AddWorkNoteAsync(int appointmentId, string note);
    Task UpdateAppointmentStatusAsync(int appointmentId, UpdateAppointmentStatusDto update);
}
