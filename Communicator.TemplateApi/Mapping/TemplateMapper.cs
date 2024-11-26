using Communicator.TemplateApi.Domain;
using Communicator.TemplateApi.Domain.Entities;

namespace Communicator.TemplateApi.Mapping;

public static class TemplateMapper
{
    public static TemplateDto ToDto(this Template entity)
        => new(entity.TenantId, entity.Id, entity.Name, entity.Content, entity.CreatedAt, entity.UpdatedAt);
    
    public static Template ToDomain(this TemplateDto dto)
        => new()
        {
            TenantId = dto.TenantId,
            Id = dto.Id,
            Name = dto.Name,
            Content = dto.Content,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
}