using Domain.Entities.UserEntity.IRepository;
using Domain.Entities.UserProfileEntity.IRepository;
using Infrastructure.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Application.Shared;

public abstract class Command : ResponseHelper
{
    protected ApplicationDbContext ApplicationDbContext;
    protected IServiceProvider ServiceProvider;
    protected IConfiguration Configuration;

    public abstract Task<CommandExecutionResult> ExecuteAsync();

    protected IUserRepository userRepository;
    protected IUserProfileRepository userProfileRepository;

    public void Resolve(ApplicationDbContext applicationContext, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        ApplicationDbContext = applicationContext;

        userRepository = serviceProvider.GetService<IUserRepository>();
        userProfileRepository = serviceProvider.GetService<IUserProfileRepository>();
    }
}
