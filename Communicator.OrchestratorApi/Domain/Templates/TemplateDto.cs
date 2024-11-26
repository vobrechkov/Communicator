using Communicator.Shared.Entity;

namespace Communicator.OrchestratorApi.Domain.Templates;

public record TemplateDto(NanoId TenantId, NanoId Id, string Name, string Content, DateTime CreatedAt, DateTime? UpdatedAt);