var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Offertgranskning_API>("api");

builder.Build().Run();