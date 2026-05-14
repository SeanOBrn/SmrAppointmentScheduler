using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmrAppointmentScheduler.Server.Data;
using SmrAppointmentScheduler.Server.DTOs;

namespace SmrAppointmentScheduler.Server.Services;

public class SlotService : ISlotService
{
    private readonly AppDbContext _db;

    public SlotService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AvailableSlotDto>> GetAvailableSlotsAsync(int? branchId = null, int? serviceTypeId = null)
    {
        var start = DateTime.Today;
        var end = start.AddDays(7);

        var query = _db.AppointmentSlots
            .Include(s => s.Branch)
            .Include(s => s.Mechanic)
            .Include(s => s.ServiceType)
            .Include(s => s.Appointment)
            .Where(s => s.Start >= start && s.Start < end && s.Appointment == null);

        if (branchId.HasValue)
        {
            query = query.Where(s => s.BranchId == branchId.Value);
        }

        if (serviceTypeId.HasValue)
        {
            query = query.Where(s => s.ServiceTypeId == serviceTypeId.Value);
        }

        var slots = await query
            .OrderBy(s => s.Start)
            .Select(s => new AvailableSlotDto
            {
                AppointmentSlotId = s.Id,
                BranchId = s.BranchId,
                BranchName = s.Branch.Name,
                MechanicId = s.MechanicId,
                MechanicName = s.Mechanic.FirstName + " " + s.Mechanic.LastName,
                ServiceTypeId = s.ServiceTypeId,
                ServiceTypeName = s.ServiceType.Name,
                Start = s.Start,
                End = s.End,
                IsBooked = s.Appointment != null
            })
            .ToListAsync();

        return slots;
    }
}
