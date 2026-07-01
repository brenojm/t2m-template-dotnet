using AutoMapper;
using Moq;
using T2MTemplate.Application.DTOs.CollaboratorDTO;
using T2MTemplate.Application.Mappings;
using T2MTemplate.Application.Services;
using T2MTemplate.Domain.Entities;
using T2MTemplate.Domain.Exceptions;
using T2MTemplate.Domain.Interfaces;
using Xunit;

namespace T2MTemplate.Tests.Services;

public class CollaboratorServiceTests
{
    private readonly Mock<ICollaboratorRepository> _repositoryMock = new();
    private readonly IMapper _mapper;
    private readonly CollaboratorService _service;

    public CollaboratorServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CollaboratorProfile>());
        _mapper = config.CreateMapper();
        _service = new CollaboratorService(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_MapsDtoAndPersistsCollaborator()
    {
        var dto = new CollaboratorRequestDTO
        {
            Name = "Ada Lovelace",
            Email = "ada@t2m.com",
            EmployeeId = "EMP-001"
        };

        var result = await _service.CreateAsync(dto);

        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(dto.EmployeeId, result.EmployeeId);
        _repositoryMock.Verify(
            r => r.AddAsync(It.Is<Collaborator>(c => c.Email == dto.Email)),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCollaboratorDoesNotExist_ThrowsNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Collaborator?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(42));
    }
}
