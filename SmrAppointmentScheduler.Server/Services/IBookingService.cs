using System.Threading.Tasks;
using SmrAppointmentScheduler.Server.DTOs;

namespace SmrAppointmentScheduler.Server.Services;

public interface IBookingService
{
    Task<BookingConfirmationDto> CreateBookingAsync(CreateBookingRequestDto request);
}
