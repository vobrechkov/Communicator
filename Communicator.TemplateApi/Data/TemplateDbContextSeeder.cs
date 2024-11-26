using Communicator.TemplateApi.Domain.Entities;
using Google.Api;
using Microsoft.EntityFrameworkCore;

namespace Communicator.TemplateApi.Data;

public static class TemplateDbContextSeeder
{
    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        if (context.Set<Template>().Any())
        {
            return;
        }

        var templateEventMappings = new List<TemplateEventMapping>
        {
            new()
            {
                EventType = "OrderCreated",
                Template = new()
                {
                    Name = "Order Created",
                    Content = "<p>Order with {{orderId}} has been created.</p>"
                }
            }
        };

        await context.Set<TemplateEventMapping>()
                     .AddRangeAsync(templateEventMappings, cancellationToken)
                     .ConfigureAwait(false);
        
        await context.SaveChangesAsync(cancellationToken)
                     .ConfigureAwait(false);
    }
}