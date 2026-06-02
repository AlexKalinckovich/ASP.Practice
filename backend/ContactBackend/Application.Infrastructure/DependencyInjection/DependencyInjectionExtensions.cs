using Data.Core;
using Model.Core.Mappers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Model.Core.DTOs;
using Service.Core;
using Validation.Core;
using Validation.Core.Validators;

namespace ContactBackend.Application.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterDatabase(services, configuration);
        RegisterMappers(services);
        RegisterRepositories(services);
        RegisterValidators(services);
        RegisterDomainServices(services);
        RegisterInfrastructure(services);

        return services;
    }

    private static void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            ConfigureDatabaseOptions(options, configuration));
    }

    private static void ConfigureDatabaseOptions(DbContextOptionsBuilder options, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddScoped<IContactMapper, ContactMapper>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IContactRepository, ContactRepository>();
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        services.AddSingleton<IValidator<ContactCreateDto>, ContactCreateDtoValidator>();
        services.AddSingleton<IValidator<ContactUpdateDto>, ContactUpdateDtoValidator>();
        services.AddSingleton<IValidator<ContactPatchDto>, ContactPatchDtoValidator>();
        services.AddSingleton<IValidator<ContactReadDto>, ContactReadDtoValidator>();
    }

    private static void RegisterDomainServices(IServiceCollection services)
    {
        services.AddScoped<IContactService, ContactService>();
    }

    private static void RegisterInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IValidationFactory, ValidationFactory>();
    }
}