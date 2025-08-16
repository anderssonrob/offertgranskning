var builder = DistributedApplication.CreateBuilder(args);

var sb = builder.AddAzureServiceBus("servicebus")
    .RunAsEmulator(em => em.WithLifetime(ContainerLifetime.Persistent))
    .AddServiceBusQueue("offers-metadata", "offers-metadata");

builder.AddProject<Projects.Offertgranskning_API>("api")
    .WithReference(sb)
    .WaitFor(sb);

builder.Build().Run();