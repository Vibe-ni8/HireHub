using HireHub.Core.Data.Interface;
using HireHub.Core.Utils.Common;
using HireHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HireHub.Infrastructure.Utils.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services,
        IConfigurationManager configuration)
    {
        //var connectionString = configuration.GetConnectionString(AppSettingKey.DefaultConnection) 
        //    ?? throw new InvalidOperationException(ExceptionMessage.ConnectionStringNotConfigured);
        //services.AddDbContext<AuthDbContext>(options => options
        //.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        var connectionString = configuration.GetConnectionString(AppSettingKey.DefaultConnection)
           ?? throw new InvalidOperationException(ExceptionMessage.ConnectionStringNotConfigured);
        services.AddDbContext<HireHubDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISlotRepository, SlotRepository>();
        services.AddScoped<IUserSlotRepository, UserSlotRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<ICandidateMapRepository, CandidateMapRepository>();

        return services;
    }
}
