using System.Collections.Generic;
using System.Threading.Tasks;
using SmrAppointmentScheduler.Server.DTOs;

namespace SmrAppointmentScheduler.Server.Services;

public interface ISlotService
{
    Task<List<AvailableSlotDto>> GetAvailableSlotsAsync(int? branchId = null, int? serviceTypeId = null);
}
