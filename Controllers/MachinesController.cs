using Assignement;
using Microsoft.AspNetCore.Mvc;

namespace Assignement.Controllers;

[ApiController]
[Route("api/machines")]
public class MachinesController(IMachineService machineService) : ControllerBase
{
    // Changed the Create endpoint to accept CreateMachineRequest
    [HttpPost]
    public async Task<ActionResult<Machine>> Create([FromBody] CreateMachineRequest request, CancellationToken cancellationToken)
    {
        var created = await machineService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Machine>>> GetAll(CancellationToken cancellationToken)
    {
        var machines = await machineService.GetAllAsync(cancellationToken);
        return Ok(machines);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Machine>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var machine = await machineService.GetByIdAsync(id, cancellationToken);
        return machine is null ? NotFound() : Ok(machine);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await machineService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
