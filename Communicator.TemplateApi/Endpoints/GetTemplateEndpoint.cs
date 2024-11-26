using Communicator.TemplateApi.Data;
using Communicator.TemplateApi.Domain;
using Communicator.TemplateApi.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Communicator.TemplateApi.Endpoints;

public class GetTemplateEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("templates/{id}", async (NanoId id, ISender sender, CancellationToken ct) =>
        {
            var template = await sender.Send(new GetTemplateQuery(id), ct);
            
            return template is not null
                ? Results.Ok(template)
                : Results.NotFound();
        });
    }
}

public record GetTemplateQuery(NanoId Id) : IRequest<TemplateDto?>;

public class GetTemplateQueryHandler(TemplateDbContext dbContext)
    : IRequestHandler<GetTemplateQuery, TemplateDto?>
{
    public async Task<TemplateDto?> Handle(GetTemplateQuery request, CancellationToken ct)
    {
        return await dbContext.Templates
            .Select(s => s.ToDto())
            .SingleOrDefaultAsync(x => x.Id == request.Id, ct);
    }
}