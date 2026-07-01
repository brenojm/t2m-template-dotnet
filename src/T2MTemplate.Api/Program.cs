using Microsoft.EntityFrameworkCore;
using T2MTemplate.Api.Extensions;
using T2MTemplate.Api.Middlewares;
using T2MTemplate.Application.Mappings;
using T2MTemplate.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.RegisterDependencies();

builder.Services.AddAutoMapper(typeof(CollaboratorProfile));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSGIdAuthentication(configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "T2MTemplate API v1");
        options.DocumentTitle = "T2MTemplate API";
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
