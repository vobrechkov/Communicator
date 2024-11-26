using Communicator.TemplateApi.Domain.Entities;
using Communicator.TemplateApi.Domain.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Communicator.TemplateApi.Data;

public class TemplateDbContext(DbContextOptions<TemplateDbContext> options) : DbContext(options)
{
    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateEventMapping> TemplateEventMappings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplateConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}