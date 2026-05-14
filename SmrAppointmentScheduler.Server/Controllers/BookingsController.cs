using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmrAppointmentScheduler.Server.DTOs;
using SmrAppointmentScheduler.Server.Services;

namespace SmrAppointmentScheduler.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequestDto request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required.");
        }

        try
        {
            var confirmation = await _bookingService.CreateBookingAsync(request);
            return CreatedAtAction(null, confirmation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "An error occurred while creating the booking." });
        }
    }
}
