using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Reference.Api.Data;
using Reference.Api.Extensions;
using Reference.Api.Repositories.Implementations;
using Reference.Api.Repositories.Interfaces;
using Reference.Api.Services.Implementations;
using Reference.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure the logging mechanism
builder.Services.AddLogging();

// Use environment variable files by environment
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Set Db connection string in appsetting file
var connectionString = builder.Configuration.GetConnectionString("postgresql");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString!));

builder.Services.AddControllers();

// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure the health check
builder.Services.AddHealthChecks();

// Configure the AutoMapper
builder.Services.ConfigureMapping();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the FluentValidation
builder.Services.AddFluentValidation(config =>
    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomHealthCheck();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

