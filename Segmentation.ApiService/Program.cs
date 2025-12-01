using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Segmentation.ApiService.Handlers;
using Segmentation.ApiService.Middlewares;
using Segmentation.DataAccess;
using Segmentation.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOptions();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomain(builder.Configuration);
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapAdminEndpoints();
app.MapPropertyEndpoints();
app.MapEvaluateEndpoints();

app.MapDefaultEndpoints();

app.Run();

