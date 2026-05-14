using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmrAppointmentScheduler.Server.Data;
using SmrAppointmentScheduler.Server.DTOs;
using SmrAppointmentScheduler.Server.Models;

namespace SmrAppointmentScheduler.Server.Services;

public class MechanicService : IMechanicService
{
    private readonly AppDbContext _db;

    public MechanicService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<MechanicDto>> GetMechanicsAsync(int? branchId = null)
    {
        var query = _db.Mechanics.AsQueryable();
        if (branchId.HasValue)
        {
            query = query.Where(m => m.BranchId == branchId.Value);
        }

        return await query
            .Select(m => new MechanicDto
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                BranchId = m.BranchId
            })
            .ToListAsync();
    }

    public async Task<List<MechanicAppointmentDto>> GetMechanicAppointmentsAsync(int mechanicId)
    {
        var today = DateTime.Today;
        var end = today.AddDays(2);

        var appointments = await _db.Appointments
            .Include(a => a.AppointmentSlot)
            .ThenInclude(s => s.Mechanic)
            .Include(a => a.AppointmentSlot).ThenInclude(s => s.Branch)
            .Include(a => a.AppointmentSlot).ThenInclude(s => s.ServiceType)
            .Where(a => a.AppointmentSlot.MechanicId == mechanicId && a.AppointmentSlot.Start >= today && a.AppointmentSlot.Start < end)
            .OrderBy(a => a.AppointmentSlot.Start)
            .Select(a => new MechanicAppointmentDto
            {
                AppointmentId = a.Id,
                AppointmentSlotId = a.AppointmentSlotId,
                Start = a.AppointmentSlot.Start,
                End = a.AppointmentSlot.End,
                CustomerName = a.CustomerName,
                Status = a.Status.ToString()
            })
            .ToListAsync();

        return appointments;
    }

    public async Task<AppointmentDetailDto?> GetAppointmentDetailAsync(int appointmentId)
    {
        var appointment = await _db.Appointments
            .Include(a => a.AppointmentSlot).ThenInclude(s => s.Branch)
            .Include(a => a.AppointmentSlot).ThenInclude(s => s.Mechanic)
            .Include(a => a.AppointmentSlot).ThenInclude(s => s.ServiceType)
            .Include(a => a.WorkNotes)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment == null) throw new SmrAppointmentScheduler.Server.Common.Exceptions.NotFoundException("Appointment not found.");

        var detail = new AppointmentDetailDto
        {
            AppointmentId = appointment.Id,
            AppointmentSlotId = appointment.AppointmentSlotId,
            BranchId = appointment.AppointmentSlot.BranchId,
            BranchName = appointment.AppointmentSlot.Branch.Name,
            MechanicId = appointment.AppointmentSlot.MechanicId,
            MechanicName = appointment.AppointmentSlot.Mechanic.FirstName + " " + appointment.AppointmentSlot.Mechanic.LastName,
            ServiceTypeId = appointment.AppointmentSlot.ServiceTypeId,
            ServiceTypeName = appointment.AppointmentSlot.ServiceType.Name,
            Start = appointment.AppointmentSlot.Start,
            End = appointment.AppointmentSlot.End,
            CustomerName = appointment.CustomerName,
            CustomerPhone = appointment.CustomerPhone,
            VehicleRegistration = appointment.VehicleRegistration,
            Status = appointment.Status.ToString(),
            WorkNotes = appointment.WorkNotes.Select(n => new WorkNoteDto { Id = n.Id, AppointmentId = n.AppointmentId, Note = n.Note, CreatedAt = n.CreatedAt }).ToList()
        };

        return detail;
    }

    public async Task AddWorkNoteAsync(int appointmentId, string note)
    {
        var appointment = await _db.Appointments.FindAsync(appointmentId);
        if (appointment == null) throw new SmrAppointmentScheduler.Server.Common.Exceptions.NotFoundException("Appointment not found.");

        if (string.IsNullOrWhiteSpace(note)) throw new SmrAppointmentScheduler.Server.Common.Exceptions.BadRequestException("Note is required.");

        var workNote = new WorkNote
        {
            AppointmentId = appointmentId,
            Note = note,
            CreatedAt = DateTime.UtcNow
        };

        _db.WorkNotes.Add(workNote);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAppointmentStatusAsync(int appointmentId, UpdateAppointmentStatusDto update)
    {
        var appointment = await _db.Appointments.FindAsync(appointmentId);
        if (appointment == null) throw new SmrAppointmentScheduler.Server.Common.Exceptions.NotFoundException("Appointment not found.");

        appointment.Status = update.Status;
        await _db.SaveChangesAsync();
    }
}
