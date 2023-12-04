using Infrastructure.Db;
using Shared;

namespace Application.Shared;

public abstract class Command : ResponseHelper
{
    protected ApplicationDbContext ApplicationDbContext;

    public Command(ApplicationDbContext applicationDbContext)
    {
        ApplicationDbContext = applicationDbContext;
    }

    public abstract Task<CommandExecutionResult> ExecuteAsync();

}
