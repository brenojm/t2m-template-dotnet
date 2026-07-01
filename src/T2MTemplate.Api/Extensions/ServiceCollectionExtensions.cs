using T2MTemplate.Application.Interfaces;
using T2MTemplate.Application.Services;
using T2MTemplate.Domain.Interfaces;
using T2MTemplate.Infra.Repositories;

namespace T2MTemplate.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterDependencies(this IServiceCollection services)
    {
        services.AddScoped<ICollaboratorService, CollaboratorService>();
        services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
    }
}
