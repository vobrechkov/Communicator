using System.Text.Json;
using Communicator.Domain.IntegrationEvents;
using Communicator.Shared;
using Communicator.Shared.Endpoints;
using Communicator.Shared.Hosting;
using Dapr;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpoints(typeof(Program).Assembly);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblyContaining<Program>();;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCloudEvents();
app.MapSubscribeHandler();
app.MapEndpoints();

/*
app.MapPost($"handle/OrderPlaced", (
    OrderPlaced orderPlaced, 
    ILogger<Program> logger) =>
{
    logger?.LogInformation("Received OrderPlaced event:\n {EventJson}", JsonSerializer.Serialize(orderPlaced));
    return Results.Accepted();
})
.WithOpenApi();
*/

app.Run();
