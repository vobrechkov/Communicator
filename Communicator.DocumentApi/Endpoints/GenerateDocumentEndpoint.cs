
using Communicator.DocumentApi.Domain;

namespace Communicator.DocumentApi.Endpoints;

public class GenerateDocumentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("generate/{tenantId}/{eventType}", async (string tenantId, string eventType, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new GenerateDocumentCommand(tenantId, eventType), ct);
            return Results.Accepted();
        });
    }
}

public record GenerateDocumentCommand(NanoId TenantId, string EventType) : IRequest<Unit>;

public class GenerateDocumentCommandHandler(
    ILogger<GenerateDocumentCommandHandler> logger,
    DaprClient daprClient)
    : IRequestHandler<GenerateDocumentCommand, Unit>
{
    public async Task<Unit> Handle(GenerateDocumentCommand request, CancellationToken cancellationToken)
    {
        var template = await daprClient.InvokeMethodAsync<TemplateDto>(
            "comm-template-api",
            $"templates/{request.EventType}",
            cancellationToken);

        logger?.LogInformation("Generated document for {TenantId} with template {TemplateId}\n{TemplateContent}",
            request.TenantId, template.Id, template.Content);

        return Unit.Value;
    }
}