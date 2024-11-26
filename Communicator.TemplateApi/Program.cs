using System.Reflection;
using Communicator.Shared.Hosting;
using Communicator.TemplateApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<TemplateDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("TemplateDb"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly(typeof(TemplateDbContext).Assembly);
        
        // Workaround for https://github.com/dotnet/aspire/issues/1023
        sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
    })
    .UseSeeding((context, _) => TemplateDbContextSeeder.SeedAsync(context).GetAwaiter().GetResult())
    .UseAsyncSeeding((context, _, cancellationToken) => TemplateDbContextSeeder.SeedAsync(context, cancellationToken)));

builder.EnrichSqlServerDbContext<TemplateDbContext>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.DisableRetry = true);

//builder.Services.AddMigration<TemplateDbContext, TemplateDbContextSeeder>();

builder.Services.AddHostedService<DbContextInitializer<TemplateDbContext>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}