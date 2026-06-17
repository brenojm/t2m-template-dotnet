using AutoMapper;
using T2MTemplate.Application.DTOs.CollaboratorDTO;
using T2MTemplate.Domain.Entities;

namespace T2MTemplate.Application.Mappings;

public class CollaboratorProfile : Profile
{
    public CollaboratorProfile()
    {
        CreateMap<Collaborator, CollaboratorResponseDTO>();
        CreateMap<CollaboratorRequestDTO, Collaborator>();
    }
}
