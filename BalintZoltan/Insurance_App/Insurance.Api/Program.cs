using Application.Abstractions;
using Application.Services;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IGeographyService, GeographyService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<InsuranceDbContext>();

    await dbContext.Database.MigrateAsync();
    await GeographySeeder.SeedAsync(
        dbContext,
        app.Environment.ContentRootPath);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
