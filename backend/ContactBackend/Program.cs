using ContactBackend.Application.API.Endpoints;
using ContactBackend.Application.API.Middleware;
using ContactBackend.Application.API.Serialization;
using ContactBackend.Application.Infrastructure.DependencyInjection;
using Data.Core;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddOpenApi();
builder.Services.AddApplicationServices(builder.Configuration);

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapContactEndpoints();
app.Run();