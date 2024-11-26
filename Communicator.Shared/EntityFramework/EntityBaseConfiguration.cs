using Communicator.Shared.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Shared.EntityFramework;

public class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(key => key.Id);
        
        builder.Property(e => e.Id)
            .HasConversion(new NanoIdConverter()) 
            .HasMaxLength(NanoIdDefaults.Size) 
            .IsRequired();
        
        ConfigureEntity(builder);
    }

    protected virtual void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
    {
    }
}

public class TenantEntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : TenantEntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(key => key.Id);
        
        builder.Property(e => e.Id)
            .HasConversion(new NanoIdConverter())
            .HasMaxLength(NanoIdDefaults.Size)
            .IsRequired();
        
        builder.Property(e => e.TenantId)
            .HasConversion(new NanoIdConverter())
            .HasMaxLength(NanoIdDefaults.Size)
            .IsRequired();
        
        ConfigureEntity(builder);
    }
    
    protected virtual void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
    {
    }
}