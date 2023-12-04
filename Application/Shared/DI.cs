using Domain.Entities.UserEntity.IRepository;
using Domain.Entities.UserProfileEntity.IRepository;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Shared;

public static class DI
{
    public static void DependecyResolver(IServiceCollection services)
    {
        services.AddScoped<IQueryExecutor, QueryExecutor>();

        services.AddScoped<ICommandExecutor, CommandExecutor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
    }
}
