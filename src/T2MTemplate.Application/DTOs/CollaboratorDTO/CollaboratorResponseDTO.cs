namespace T2MTemplate.Application.DTOs.CollaboratorDTO;

/// <summary>
/// Representação de um colaborador retornada pela API.
/// </summary>
public class CollaboratorResponseDTO
{
    /// <summary>Identificador único do colaborador.</summary>
    /// <example>1</example>
    public long Id { get; set; }

    /// <summary>Nome completo do colaborador.</summary>
    /// <example>Ada Lovelace</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>E-mail corporativo do colaborador.</summary>
    /// <example>ada.lovelace@t2mlab.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>Matrícula/identificador funcional do colaborador.</summary>
    /// <example>EMP-001</example>
    public string EmployeeId { get; set; } = string.Empty;
}
