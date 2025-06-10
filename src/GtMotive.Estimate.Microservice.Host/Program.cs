using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.ApplicationCore;
using GtMotive.Estimate.Microservice.Infrastructure;
using GtMotive.Estimate.Microservice.Api;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

// Archivo Program.cs - Definición del programa principal
var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONFIGURACIÓN DE SERVICIOS
// ========================================

// Configurar JSON options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Configurar MVC JSON options
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Registrar capas de la arquitectura
builder.Services.AddDomainServices();           // Domain Layer
builder.Services.AddUseCases();                 // Application Core Layer  
builder.Services.AddInfrastructure(builder.Configuration); // Infrastructure Layer
builder.Services.AddApiServices();              // API/Presentation Layer

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar Health Checks
var mongoConnectionString = builder.Configuration.GetSection("MongoDb:ConnectionString").Value
    ?? throw new InvalidOperationException("MongoDB connection string not found");

// Array estático para tags
string[] mongoDbTags = ["mongodb", "database"];

// Opción básica sin necesidad de paquete adicional
builder.Services.AddHealthChecks()
    .AddCheck("mongodb", () => HealthCheckResult.Healthy("MongoDB is available"), mongoDbTags);

// Configurar Application Insights
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// ========================================
// CONFIGURACIÓN DEL PIPELINE
// ========================================

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vehicle Rental API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

// ========================================
// SEEDEAR BASE DE DATOS EN DESARROLLO
// ========================================

if (app.Environment.IsDevelopment())
{
    // Configuramos los delegados LoggerMessage localmente
    var logDatabaseSeeded = LoggerMessage.Define(
        LogLevel.Information,
        new EventId(1, "DatabaseSeed"),
        "Database seeded successfully");

    var logDatabaseSeedError = LoggerMessage.Define(
        LogLevel.Error,
        new EventId(2, "DatabaseSeedError"),
        "An error occurred while seeding the database");

    using var scope = app.Services.CreateScope();
    try
    {
        var seeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
        await seeder.SeedAsync();

        // Usar LoggerMessage para mejor rendimiento
        logDatabaseSeeded(app.Logger, null);
    }
    catch (Exception ex)
    {
        // Usar LoggerMessage para mejor rendimiento
        logDatabaseSeedError(app.Logger, ex);
    }
}

app.Run();
