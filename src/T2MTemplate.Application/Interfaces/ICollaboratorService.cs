using T2MTemplate.Application.DTOs.CollaboratorDTO;

namespace T2MTemplate.Application.Interfaces;

public interface ICollaboratorService
{
    Task<IEnumerable<CollaboratorResponseDTO>> GetAllAsync();
    Task<CollaboratorResponseDTO> GetByIdAsync(long id);
    Task<CollaboratorResponseDTO> CreateAsync(CollaboratorRequestDTO dto);
    Task<CollaboratorResponseDTO> UpdateAsync(long id, CollaboratorRequestDTO dto);
    Task DeleteAsync(long id);
}
