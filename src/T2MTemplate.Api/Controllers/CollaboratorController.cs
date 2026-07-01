using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T2MTemplate.Application.DTOs.CollaboratorDTO;
using T2MTemplate.Application.Interfaces;

namespace T2MTemplate.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de colaboradores.
/// </summary>
/// <remarks>Todos os endpoints exigem um token JWT válido (botão "Authorize").</remarks>
[ApiController]
[Route("api/collaborators")]
[Authorize]
[Produces("application/json")]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collaboratorService;

    /// <summary>Cria uma nova instância do controller.</summary>
    public CollaboratorController(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    /// <summary>Lista todos os colaboradores.</summary>
    /// <returns>Coleção de colaboradores cadastrados.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CollaboratorResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _collaboratorService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>Obtém um colaborador pelo identificador.</summary>
    /// <param name="id">Identificador do colaborador.</param>
    /// <returns>O colaborador correspondente.</returns>
    /// <response code="200">Colaborador encontrado.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="404">Colaborador não encontrado.</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CollaboratorResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _collaboratorService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>Cadastra um novo colaborador.</summary>
    /// <param name="dto">Dados do colaborador a ser criado.</param>
    /// <returns>O colaborador recém-criado.</returns>
    /// <response code="201">Colaborador criado com sucesso.</response>
    /// <response code="400">Dados de entrada inválidos.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CollaboratorResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CollaboratorRequestDTO dto)
    {
        var result = await _collaboratorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza um colaborador existente.</summary>
    /// <param name="id">Identificador do colaborador.</param>
    /// <param name="dto">Novos dados do colaborador.</param>
    /// <returns>O colaborador atualizado.</returns>
    /// <response code="200">Colaborador atualizado com sucesso.</response>
    /// <response code="400">Dados de entrada inválidos.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="404">Colaborador não encontrado.</response>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(CollaboratorResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] CollaboratorRequestDTO dto)
    {
        var result = await _collaboratorService.UpdateAsync(id, dto);
        return Ok(result);
    }

    /// <summary>Remove um colaborador.</summary>
    /// <param name="id">Identificador do colaborador.</param>
    /// <response code="204">Colaborador removido com sucesso.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="404">Colaborador não encontrado.</response>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        await _collaboratorService.DeleteAsync(id);
        return NoContent();
    }
}
