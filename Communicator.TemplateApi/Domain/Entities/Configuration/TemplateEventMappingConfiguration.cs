using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.TemplateApi.Domain.Entities.Configuration;

public class TemplateEventMappingConfiguration : TenantEntityBaseConfiguration<TemplateEventMapping>
{
    protected override void ConfigureEntity(EntityTypeBuilder<TemplateEventMapping> builder)
    {
        builder.HasKey(key => key.Id);

        builder.Property(x => x.TemplateId)
            .HasMaxLength(NanoIdDefaults.Size)
            .HasConversion<NanoIdConverter>();

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(t => t.Template)
            .WithMany()
            .HasForeignKey(t => t.TemplateId);
    }
}