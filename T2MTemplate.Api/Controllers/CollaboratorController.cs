using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T2MTemplate.Application.DTOs.CollaboratorDTO;
using T2MTemplate.Application.Interfaces;

namespace T2MTemplate.Api.Controllers;

[ApiController]
[Route("api/collaborators")]
[Authorize]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorController(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _collaboratorService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _collaboratorService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CollaboratorRequestDTO dto)
    {
        var result = await _collaboratorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] CollaboratorRequestDTO dto)
    {
        var result = await _collaboratorService.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _collaboratorService.DeleteAsync(id);
        return NoContent();
    }
}
