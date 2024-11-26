namespace Communicator.TemplateApi.Domain.Entities;

public class TemplateEventMapping : TenantEntityBase
{
    public string EventType { get; set; }
    public NanoId TemplateId { get; set; }

    public Template Template { get; set; }
}