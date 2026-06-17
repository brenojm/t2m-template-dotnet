using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace T2MTemplate.Api.Extensions;

public static class SGIdAuthenticationExtensions
{
    private const string ConfigSection = "JwtSettings";
    private const string ClaimSGIdSistemas = "Sistemas";

    public static AuthenticationBuilder AddSGIdAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    ValidIssuer = configuration[$"{ConfigSection}:Issuer"],
                    ValidAudience = configuration[$"{ConfigSection}:Audience"],
                    IssuerSigningKey = LoadPublicKey(configuration[$"{ConfigSection}:PublicKey"]!),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("T2MTemplate.Api.Extensions.SGIdAuthentication");
                        logger.LogWarning(ctx.Exception, "Falha na validação do token SGId");
                        return Task.CompletedTask;
                    },

                    OnChallenge = ctx =>
                    {
                        ctx.HandleResponse();
                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        ctx.Response.ContentType = "application/json";
                        var body = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            titulo = "Acesso não autorizado",
                            status = 401,
                            tipo = "Acesso Não Autorizado",
                            detalhes = "Token ausente ou inválido. Faça login no SGId e envie um token válido.",
                            instancia = $"{ctx.Request.Method} {ctx.Request.Path}"
                        });
                        return ctx.Response.WriteAsync(body);
                    },

                    OnForbidden = ctx =>
                    {
                        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                        ctx.Response.ContentType = "application/json";
                        var body = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            titulo = "Acesso proibido",
                            status = 403,
                            tipo = "Acesso Proibido",
                            detalhes = "Você não tem permissão para acessar este recurso.",
                            instancia = $"{ctx.Request.Method} {ctx.Request.Path}"
                        });
                        return ctx.Response.WriteAsync(body);
                    },

                    OnTokenValidated = async ctx =>
                    {
                        var principal = ctx.Principal;
                        if (principal == null) return;

                        var identity = new ClaimsIdentity(principal.Identity);
                        var systemId = configuration[$"{ConfigSection}:SystemId"];
                        var systemsClaims = principal.FindAll(ClaimSGIdSistemas).Select(c => c.Value);

                        foreach (var role in ExtractRoles(systemsClaims, systemId))
                            identity.AddClaim(new Claim(ClaimTypes.Role, role));

                        if (identity.Claims.Any())
                            principal.AddIdentity(identity);

                        await Task.CompletedTask;
                    }
                };
            });
    }

    private static RsaSecurityKey LoadPublicKey(string base64)
    {
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(base64), out _);
        return new RsaSecurityKey(rsa);
    }

    private static IEnumerable<string> ExtractRoles(IEnumerable<string> systemsClaims, string? systemId)
    {
        var roles = new List<string>();
        foreach (var claim in systemsClaims)
        {
            var parts = claim.Split(':', 2);
            if (parts.Length < 2) continue;

            if (!string.Equals(parts[0].Trim(), systemId, StringComparison.OrdinalIgnoreCase))
                continue;

            roles.AddRange(parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }
        return roles;
    }
}
