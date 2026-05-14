using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmrAppointmentScheduler.Server.DTOs;
using SmrAppointmentScheduler.Server.Services;

namespace SmrAppointmentScheduler.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MechanicsController : ControllerBase
{
    private readonly IMechanicService _mechanicService;

    public MechanicsController(IMechanicService mechanicService)
    {
        _mechanicService = mechanicService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MechanicDto>>> Get([FromQuery] int? branchId = null)
    {
        var mechanics = await _mechanicService.GetMechanicsAsync(branchId);
        return Ok(mechanics);
    }

    [HttpGet("{id}/appointments")]
    public async Task<ActionResult<List<MechanicAppointmentDto>>> GetAppointments([FromRoute] int id)
    {
        var appointments = await _mechanicService.GetMechanicAppointmentsAsync(id);
        return Ok(appointments);
    }
}
