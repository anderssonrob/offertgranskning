using Offertgranskning.API.Infrastructure.Configuration;
using Offertgranskning.API.Infrastructure.Configuration.Slices;

var builder = WebApplication.CreateBuilder(args);

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

app.MapSliceEndpoints();

app.Run();