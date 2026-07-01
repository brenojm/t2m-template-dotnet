using System.Reflection;
using Microsoft.OpenApi.Models;
using T2MTemplate.Application.Mappings;

namespace T2MTemplate.Api.Extensions;

/// <summary>
/// Configuração do Swagger/OpenAPI: metadados da API, autenticação via
/// Bearer JWT (botão "Authorize") e inclusão dos comentários XML.
/// </summary>
public static class SwaggerExtensions
{
    private const string SecuritySchemeId = "Bearer";

    /// <summary>
    /// Registra o gerador de documentação OpenAPI com suporte a token JWT.
    /// </summary>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "T2MTemplate API",
                Version = "v1",
                Description = "API do projeto T2MTemplate. Autentique-se pelo SGId e informe o token JWT no botão \"Authorize\" para acessar os endpoints protegidos.",
                Contact = new OpenApiContact
                {
                    Name = "T2M",
                    Url = new Uri("https://www.t2mlab.com")
                }
            });

            // Esquema de segurança: token JWT no header Authorization (Bearer).
            options.AddSecurityDefinition(SecuritySchemeId, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Cole apenas o token JWT (sem o prefixo \"Bearer \")."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecuritySchemeId
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Comentários XML da própria API e da camada Application (DTOs).
            IncludeXmlComments(options, Assembly.GetExecutingAssembly());
            IncludeXmlComments(options, typeof(CollaboratorProfile).Assembly);
        });

        return services;
    }

    private static void IncludeXmlComments(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options, Assembly assembly)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
}
