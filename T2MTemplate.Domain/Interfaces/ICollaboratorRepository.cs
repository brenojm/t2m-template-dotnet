using T2MTemplate.Domain.Entities;

namespace T2MTemplate.Domain.Interfaces;

public interface ICollaboratorRepository
{
    Task<IEnumerable<Collaborator>> GetAllAsync();
    Task<Collaborator?> GetByIdAsync(long id);
    Task AddAsync(Collaborator collaborator);
    Task UpdateAsync(Collaborator collaborator);
    Task DeleteAsync(Collaborator collaborator);
}
