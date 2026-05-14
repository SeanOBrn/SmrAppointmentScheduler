using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmrAppointmentScheduler.Server.DTOs;
using SmrAppointmentScheduler.Server.Services;

namespace SmrAppointmentScheduler.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlotsController : ControllerBase
{
    private readonly ISlotService _slotService;

    public SlotsController(ISlotService slotService)
    {
        _slotService = slotService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AvailableSlotDto>>> Get([FromQuery] int? branchId = null, [FromQuery] int? serviceTypeId = null)
    {
        var slots = await _slotService.GetAvailableSlotsAsync(branchId, serviceTypeId);
        return Ok(slots);
    }
}
