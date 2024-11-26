namespace Communicator.DocumentApi.Domain;

public record TemplateDto(NanoId TenantId, NanoId Id, string Name, string Content, DateTime CreatedAt, DateTime? UpdatedAt);