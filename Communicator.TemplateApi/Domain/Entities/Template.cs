namespace Communicator.TemplateApi.Domain.Entities;

public class Template : TenantAuditableEntityBase
{
    public string Name { get; set; }
    public string Content { get; set; }
}