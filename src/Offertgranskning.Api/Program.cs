using Offertgranskning.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices()
    .RegisterDomainServices()
    .RegisterPersistenceServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();