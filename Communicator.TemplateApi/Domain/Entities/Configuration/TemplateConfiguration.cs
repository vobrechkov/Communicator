
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.TemplateApi.Domain.Entities.Configuration;

public class TemplateConfiguration : TenantEntityBaseConfiguration<Template>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Template> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("getutcdate()")
            .IsRequired();
    }
}