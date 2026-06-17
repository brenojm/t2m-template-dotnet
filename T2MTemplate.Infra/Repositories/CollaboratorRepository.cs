using Microsoft.EntityFrameworkCore;
using T2MTemplate.Domain.Entities;
using T2MTemplate.Domain.Interfaces;
using T2MTemplate.Infra.Data;

namespace T2MTemplate.Infra.Repositories;

public class CollaboratorRepository : ICollaboratorRepository
{
    private readonly AppDbContext _context;

    public CollaboratorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Collaborator>> GetAllAsync()
        => await _context.Collaborators.ToListAsync();

    public async Task<Collaborator?> GetByIdAsync(long id)
        => await _context.Collaborators.FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Collaborator collaborator)
    {
        await _context.Collaborators.AddAsync(collaborator);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Collaborator collaborator)
    {
        _context.Collaborators.Update(collaborator);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Collaborator collaborator)
    {
        _context.Collaborators.Remove(collaborator);
        await _context.SaveChangesAsync();
    }
}
