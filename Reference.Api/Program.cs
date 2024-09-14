using System;
using System.Reflection;
using Consul;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Reference.Api.Cache;
using Reference.Api.Data;
using Reference.Api.Extensions;
using Reference.Api.MiddleWare;
using Reference.Api.Repositories.Implementations;
using Reference.Api.Repositories.Interfaces;    
using Reference.Api.Security;
using Reference.Api.Services.Implementations;
using Reference.Api.Services.Interfaces;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("MyCustomSource")
            .AddConsoleExporter();
    })
    .WithMetrics(metricsProviderBuilder =>
    {
        metricsProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddPrometheusExporter(); // Prometheus için metrikleri ekle
    });


#region Serilog Configuration
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

builder.Host.UseSerilog((context, loggerConfig) => {
    loggerConfig.ReadFrom.Configuration(context.Configuration);
    loggerConfig.WriteTo.Console(theme: SystemConsoleTheme.Literate);
    loggerConfig.Enrich.WithProperty("Environment", environment);
    var text = builder.Configuration.GetConnectionString("elastic");

    loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration.GetConnectionString("elastic")))
    {   
        AutoRegisterTemplate = true,
        OverwriteTemplate = true,
        DetectElasticsearchVersion = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        NumberOfReplicas = 1,
        NumberOfShards = 2,
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                   EmitEventFailureHandling.WriteToFailureSink |
                                   EmitEventFailureHandling.RaiseCallback,
        FailureSink = new Serilog.Sinks.File.FileSink("./fail.txt", new Serilog.Formatting.Json.JsonFormatter(), null, null)
    });
});

#endregion

#region Use environment variable file by environment

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

#endregion

#region Redis Configuration

builder.Services.AddStackExchangeRedisCache(action =>
{
    action.Configuration = builder.Configuration.GetConnectionString("redis");
});

#endregion

#region Consul Configuration

var address = new Uri(builder.Configuration["Consul:Host"]);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(consul => new ConsulClient(consulConfig =>
{
    consulConfig.Address = address;
}, null, handlerOverride =>
{
    handlerOverride.Proxy = null;
    handlerOverride.UseProxy = false;
}));

#endregion

#region Set DB connection

var connectionString = builder.Configuration.GetConnectionString("postgresql");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString!));

#endregion

#region Register Services

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#endregion

#region Configure the health check

builder.Services.AddHealthChecks();

#endregion

#region Configure the AutoMapper

builder.Services.ConfigureMapping();

#endregion

#region Security

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

#endregion

#region Swagger Configuration

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reference API",
        Version = "v1",
        Description = "Use these credentials to login:<br><b>Email:</b> stefan_turner38@yahoo.com<br><b>Password:</b> QTKL8Opd" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
}).AddAuthentication();

#endregion

#region Fluent Validation

builder.Services.AddFluentValidation(config =>
    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

#endregion

#region AuthorizationPolicies

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("1"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("2"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("3"));
    options.AddPolicy("GuestPolicy", policy => policy.RequireRole("4"));
});

#endregion

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint(context => context.Request.Path == "/metrics");

// Consul Configuration
ConfigureConsulExtension.ServiceRegistration(builder.Configuration, app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

if (app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

//Middleware
app.UseMiddleware<CorrelationIdMiddleware>();

//Use Cors
app.UseCors("AllowOrigin");

app.UseSerilogRequestLogging();

app.UseCustomHealthCheck();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

