using Aspire.Hosting.Dapr;
using Microsoft.Extensions.Configuration.Json;

var builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.Sources.Add(new JsonConfigurationSource { Path = "appsettings.Local.json", Optional = true });

var pubSub = builder.AddDaprPubSub("pubsub");

var sqlServer = builder.AddSqlServer("sqlserver", port: 11433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var templateDb = sqlServer.AddDatabase("TemplateDb");

builder.AddProject<Projects.Communicator_ProducerApi>("producer-api")
    .WithReference(pubSub)
    .WithDaprSidecar(new DaprSidecarOptions { AppPort = 5000 });

var templateApi = builder.AddProject<Projects.Communicator_TemplateApi>("template-api")
    .WithReference(pubSub)
    .WithDaprSidecar(new DaprSidecarOptions { AppPort = 5002 })
    .WithReference(templateDb)
    .WaitFor(templateDb);

var documentApi = builder.AddProject<Projects.Communicator_DocumentApi>("document-api")
    .WithReference(pubSub)
    .WithDaprSidecar(new DaprSidecarOptions { AppPort = 5003 });

builder.AddProject<Projects.Communicator_OrchestratorApi>("orchestrator-api")
    .WithReference(pubSub)
    .WithDaprSidecar(new DaprSidecarOptions { AppPort = 5001 })
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WaitFor(templateApi);

builder.Build().Run();