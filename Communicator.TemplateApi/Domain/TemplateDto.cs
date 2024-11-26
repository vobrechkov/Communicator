using Communicator.TemplateApi.Domain.Entities;

namespace Communicator.TemplateApi.Domain;

public record TemplateDto(NanoId TenantId, NanoId Id, string Name, string Content, DateTime CreatedAt, DateTime? UpdatedAt)
{
    public static TemplateDto From(Template entity)
        => new(entity.TenantId, entity.Id, entity.Name, entity.Content, entity.CreatedAt, entity.UpdatedAt);
}