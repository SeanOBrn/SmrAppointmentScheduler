using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmrAppointmentScheduler.Server.Data;
using SmrAppointmentScheduler.Server.DTOs;
using SmrAppointmentScheduler.Server.Models;

namespace SmrAppointmentScheduler.Server.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _db;

    public BookingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BookingConfirmationDto> CreateBookingAsync(CreateBookingRequestDto request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        // Validate slot exists
        var slot = await _db.AppointmentSlots
            .Include(s => s.Appointment)
            .Include(s => s.Branch)
            .Include(s => s.ServiceType)
            .FirstOrDefaultAsync(s => s.Id == request.AppointmentSlotId);

        if (slot == null)
        {
            throw new SmrAppointmentScheduler.Server.Common.Exceptions.NotFoundException("Appointment slot not found.");
        }

        // Prevent double-booking: check if there's already an appointment for this slot
        if (slot.Appointment != null)
        {
            throw new SmrAppointmentScheduler.Server.Common.Exceptions.ConflictException("Appointment slot already booked.");
        }

        // Validate branch and service type
        if (slot.BranchId != request.BranchId)
        {
            throw new SmrAppointmentScheduler.Server.Common.Exceptions.InvalidOperationException("Appointment slot does not belong to the requested branch.");
        }

        if (slot.ServiceTypeId != request.ServiceTypeId)
        {
            throw new SmrAppointmentScheduler.Server.Common.Exceptions.InvalidOperationException("Appointment slot does not match the requested service type.");
        }

        // Create appointment
        var appointment = new Appointment
        {
            AppointmentSlotId = slot.Id,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            VehicleRegistration = request.VehicleRegistration,
            Status = AppointmentStatus.Scheduled,
            BookingReference = GenerateBookingReference()
        };

        _db.Appointments.Add(appointment);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Handle race condition where another booking was created concurrently
            throw new SmrAppointmentScheduler.Server.Common.Exceptions.ConflictException("Failed to create appointment; slot may have been booked.");
        }

        // Add initial note if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            var note = new WorkNote
            {
                Appointment = appointment,
                Note = request.Notes
            };
            _db.WorkNotes.Add(note);
            await _db.SaveChangesAsync();
        }

        return new BookingConfirmationDto
        {
            AppointmentId = appointment.Id,
            AppointmentSlotId = appointment.AppointmentSlotId,
            CustomerName = appointment.CustomerName,
            Status = appointment.Status.ToString(),
            BookingReference = appointment.BookingReference
        };
    }

    private string GenerateBookingReference()
    {
        // Simple unique ref: date + random chars
        return $"BK-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
