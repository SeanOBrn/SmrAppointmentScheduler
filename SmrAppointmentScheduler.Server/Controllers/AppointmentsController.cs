using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmrAppointmentScheduler.Server.DTOs;
using SmrAppointmentScheduler.Server.Services;

namespace SmrAppointmentScheduler.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMechanicService _mechanicService;

    public AppointmentsController(IMechanicService mechanicService)
    {
        _mechanicService = mechanicService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var detail = await _mechanicService.GetAppointmentDetailAsync(id);
        if (detail == null) return NotFound();
        return Ok(detail);
    }

    [HttpPost("{id}/worknotes")]
    public async Task<IActionResult> AddWorkNote([FromRoute] int id, [FromBody] WorkNoteDto noteDto)
    {
        if (noteDto == null || string.IsNullOrWhiteSpace(noteDto.Note)) return BadRequest("Note is required.");

        try
        {
            await _mechanicService.AddWorkNoteAsync(id, noteDto.Note);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateAppointmentStatusDto update)
    {
        if (update == null) return BadRequest();

        try
        {
            await _mechanicService.UpdateAppointmentStatusAsync(id, update);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
