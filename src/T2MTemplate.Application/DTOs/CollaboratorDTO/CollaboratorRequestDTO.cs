using System.ComponentModel.DataAnnotations;

namespace T2MTemplate.Application.DTOs.CollaboratorDTO;

/// <summary>
/// Dados necessários para criar ou atualizar um colaborador.
/// </summary>
public class CollaboratorRequestDTO
{
    /// <summary>Nome completo do colaborador.</summary>
    /// <example>Ada Lovelace</example>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>E-mail corporativo do colaborador.</summary>
    /// <example>ada.lovelace@t2mlab.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Matrícula/identificador funcional do colaborador.</summary>
    /// <example>EMP-001</example>
    [Required]
    [MaxLength(20)]
    public string EmployeeId { get; set; } = string.Empty;
}
