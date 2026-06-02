using ContactBackend.Application.API.Endpoints;
using ContactBackend.Application.API.Middleware;
using ContactBackend.Application.API.Serialization;
using ContactBackend.Application.Infrastructure.DependencyInjection;
using Data.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigureCorsSettings(builder);
ConfigureJsonSerialization(builder);
RegisterApplicationServices(builder);

WebApplication app = builder.Build();

await ApplyDatabaseMigrationsAsync(app);
ConfigureRequestPipeline(app);

app.Run();

static void ConfigureCorsSettings(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
}

static void ConfigureJsonSerialization(WebApplicationBuilder builder)
{
    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
    });
}

static void RegisterApplicationServices(WebApplicationBuilder builder)
{
    builder.Services.AddOpenApi();
    builder.Services.AddApplicationServices(builder.Configuration);
}

static async Task ApplyDatabaseMigrationsAsync(WebApplication app)
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await dbContext.Database.MigrateAsync();
    }
}

static void ConfigureRequestPipeline(WebApplication app)
{
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseRouting();
    app.UseCors("AllowFrontend");

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.MapContactEndpoints();
}