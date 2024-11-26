using Communicator.Domain.IntegrationEvents;
using Communicator.OrchestratorApi.Domain.Templates;
using Communicator.Shared;
using Communicator.Shared.Endpoints;
using Dapr.Client;

namespace Communicator.OrchestratorApi.Endpoints;

public class OrderCreatedEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"/subscription/{nameof(OrderPlaced)}", (
                    OrderPlaced notification,
                    IPublisher sender,
                    CancellationToken ct) => sender.Publish(notification, ct))
            .WithTopic(PubSub.Name, PubSub.TopicNames.Orders);
    }
}

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger, DaprClient daprClient) : INotificationHandler<OrderPlaced>
{
    public async Task Handle(OrderPlaced order, CancellationToken cancellationToken)
    {
        await daprClient.InvokeMethodAsync(
            "comm-document-api", 
            $"generate/{order.TenantId}/{nameof(OrderPlaced)}",
            cancellationToken);
        
        logger.LogInformation("Order {OrderId} was placed on {Date}", order.OrderId, order.TimeStamp); 
        await Task.CompletedTask;
    }
}