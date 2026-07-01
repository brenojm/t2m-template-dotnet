using System.ComponentModel.DataAnnotations;

namespace T2MTemplate.Application.DTOs.CollaboratorDTO;

public class CollaboratorRequestDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string EmployeeId { get; set; } = string.Empty;
}
