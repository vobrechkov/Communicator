using Communicator.Domain.IntegrationEvents;
using Communicator.Shared;
using Communicator.Shared.Hosting;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost($"produce/{nameof(OrderPlaced)}", async (OrderPlaced orderPlaced, DaprClient daprClient, ILogger<Program> logger) =>
{
    try
    {
        await daprClient.PublishEventAsync(PubSub.Name, PubSub.TopicNames.Orders, orderPlaced);
    }
    catch (Exception e)
    {
        logger?.LogError(e, "Failed to publish event: {Message}.", e.InnerException?.Message ?? e.Message);
        throw;
    }
});
    

app.Run();