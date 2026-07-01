using AutoMapper;
using T2MTemplate.Application.DTOs.CollaboratorDTO;
using T2MTemplate.Application.Interfaces;
using T2MTemplate.Domain.Entities;
using T2MTemplate.Domain.Exceptions;
using T2MTemplate.Domain.Interfaces;

namespace T2MTemplate.Application.Services;

public class CollaboratorService : ICollaboratorService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IMapper _mapper;

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, IMapper mapper)
    {
        _collaboratorRepository = collaboratorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CollaboratorResponseDTO>> GetAllAsync()
    {
        var collaborators = await _collaboratorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CollaboratorResponseDTO>>(collaborators);
    }

    public async Task<CollaboratorResponseDTO> GetByIdAsync(long id)
    {
        var collaborator = await _collaboratorRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Collaborator {id} not found.");
        return _mapper.Map<CollaboratorResponseDTO>(collaborator);
    }

    public async Task<CollaboratorResponseDTO> CreateAsync(CollaboratorRequestDTO dto)
    {
        var collaborator = _mapper.Map<Collaborator>(dto);
        await _collaboratorRepository.AddAsync(collaborator);
        return _mapper.Map<CollaboratorResponseDTO>(collaborator);
    }

    public async Task<CollaboratorResponseDTO> UpdateAsync(long id, CollaboratorRequestDTO dto)
    {
        var collaborator = await _collaboratorRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Collaborator {id} not found.");

        _mapper.Map(dto, collaborator);
        await _collaboratorRepository.UpdateAsync(collaborator);
        return _mapper.Map<CollaboratorResponseDTO>(collaborator);
    }

    public async Task DeleteAsync(long id)
    {
        var collaborator = await _collaboratorRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Collaborator {id} not found.");
        await _collaboratorRepository.DeleteAsync(collaborator);
    }
}
