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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
