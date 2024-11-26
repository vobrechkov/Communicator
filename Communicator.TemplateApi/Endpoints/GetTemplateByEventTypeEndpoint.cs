using Communicator.TemplateApi.Data;
using Communicator.TemplateApi.Domain;
using Communicator.TemplateApi.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Communicator.TemplateApi.Endpoints;

public class GetTemplateByEventTypeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("templates/{eventType}", async (string eventType, ISender sender, CancellationToken ct) =>
        {
            var template = await sender.Send(new GetTemplateByEventTypeQuery(eventType), ct);
            
            return template is not null
                ? Results.Ok(template)
                : Results.NotFound();
        });
    }
}

public record GetTemplateByEventTypeQuery(string EventType) : IRequest<TemplateDto?>;

public class GetTemplateByEventTypeHandler(TemplateDbContext dbContext)
    : IRequestHandler<GetTemplateByEventTypeQuery, TemplateDto?>
{
    public async Task<TemplateDto?> Handle(GetTemplateByEventTypeQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.TemplateEventMappings
            .Include(i => i.Template)
            .Where(x => x.EventType == request.EventType)
            .Select(s => s.Template.ToDto())
            .SingleOrDefaultAsync(cancellationToken);
    }
}