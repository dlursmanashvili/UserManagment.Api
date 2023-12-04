using Application.Shared;
using Shared;

namespace Application.Commands.UserCommands;

public class UserUpdateCommand : Command
{
    public int id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public bool isActive { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await userRepository.UpdateAsyncUser(new Domain.Entities.UserEntity.User() { Id = id, Username = username, Password = password, IsActive = isActive, Email = email });
    }
}
