using Offertgranskning.API.Infrastructure.Configuration;
using Offertgranskning.API.Infrastructure.Configuration.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.RegisterApplicationServices()
    .RegisterDomainServices()
    .RegisterPersistenceServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseStatusCodePages();

app.MapDefaultEndpoints();
app.MapEndpoints();

app.Run();